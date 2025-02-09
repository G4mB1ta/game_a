using ECS.Aspects.Units;
using ECS.Components.Combats;
using ECS.Components.Units;
using ECS.Enums;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems.Combats {
    public partial class AttackSystem : SystemBase {

        /// <summary>
        ///     Returns true if the unit is able to attack.
        ///     The unit must have a target and the target must have a position.
        ///     Also, the target must be within the attack range.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>True if unit is can to attack.</returns>
        private bool IsAbleToAttack(UnitAspect unit) {
            if (unit.TargetEntity == Entity.Null) return false;

            if (unit.TargetPosition.Equals(float3.zero)) return false;

            if (!unit.IsTargetInRange()) return false;

            return true;
        }
        
        protected override void OnCreate() {
            RequireForUpdate<AttackStats>();
        }

        protected override void OnUpdate() {
            AttackJob();
        }

        private void AttackJob() {
            foreach (var (unit, attackTimer) in SystemAPI.Query<UnitAspect, RefRW<AttackTimer>>()) {
                if (attackTimer.ValueRW.CanAttack()) {
                    if (IsAbleToAttack(unit)) {
                        switch (unit.AttackStats.type) {
                            case AttackType.Melee:
                                break;
                            case AttackType.Ranged:
                                PerformRangedAttack(unit);
                                break;
                        }
                        attackTimer.ValueRW.Reset();
                    }
                }
                else {
                    attackTimer.ValueRW.Update(SystemAPI.Time.DeltaTime);
                }            
            }
        }

        /// <summary>
        ///     Perform a ranged attack. The unit will instantiate a projectile and shoot it towards the target.
        ///     Set the projectile's damage, speed, direction, owner, and target.
        /// </summary>
        /// <param name="unit">The unit performs the ranged attack.</param>
        private void PerformRangedAttack(UnitAspect unit) {
            
            if (unit.AttackStats.projectilePrefab == Entity.Null) return;
            
            float3 direction = math.normalize(unit.TargetPosition - unit.Transform.Position);
                                
            Entity bullet = EntityManager.Instantiate(unit.AttackStats.projectilePrefab);
            EntityManager.SetComponentData(bullet, new LocalTransform {
                Position = unit.Transform.Position,
                Rotation = quaternion.LookRotationSafe(direction, math.up()),
                Scale = 1f
            });

            if (EntityManager.HasComponent<Projectile>(bullet)) {
                Debug.Log("Projectile component exists");
                return;
            }
            

            EntityManager.SetComponentData(bullet, new Projectile {
                damage = unit.AttackStats.damage,
                speed = 25f,
                direction = math.normalize(unit.TargetPosition - unit.Transform.Position),
                lifeTime = 100f,
                timer = 0f,
                owner = unit.Entity,
                target = unit.TargetEntity
            });
        }
    }
}
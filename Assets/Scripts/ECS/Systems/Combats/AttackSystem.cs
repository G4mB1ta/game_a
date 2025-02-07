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
        
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<OffensiveStats>();
        }

        public void OnUpdate(ref SystemState state) {

            foreach (var (unit, attackTimer) in SystemAPI.Query<UnitAspect, RefRW<AttackTimer>>())
                if (attackTimer.ValueRW.CanAttack()) {
                    if (IsAbleToAttack(unit)) {
                        switch (unit.OffensiveStats.type) {
                            case AttackType.Melee:
                                PerformMeleeAttack();
                                break;
                            case AttackType.Ranged:
                                PerformRangedAttack();
                                break;
                        }
                        attackTimer.ValueRW.Reset();
                    }
                }
                else {
                    attackTimer.ValueRW.Update(SystemAPI.Time.DeltaTime);
                }
        }

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

        private void PerformMeleeAttack() {
            Debug.Log("<color=green>Performing melee attack</color>");
        }

        private void PerformRangedAttack() {
            Debug.Log("<color=yellow>Performing ranged attack</color>");
            
        }

        protected override void OnCreate() {
            RequireForUpdate<OffensiveStats>();
        }

        protected override void OnUpdate() {
            foreach (var (unit, attackTimer) in SystemAPI.Query<UnitAspect, RefRW<AttackTimer>>()) {
                if (attackTimer.ValueRW.CanAttack()) {
                    if (IsAbleToAttack(unit)) {
                        switch (unit.OffensiveStats.type) {
                            case AttackType.Melee:
                                PerformMeleeAttack();
                                break;
                            case AttackType.Ranged:
                                PerformRangedAttack();
                                
                                Entity bullet = EntityManager.Instantiate(unit.OffensiveStats.projectilePrefab);
                                EntityManager.SetComponentData(bullet, new LocalTransform {
                                    Position = unit.Transform.Position,
                                    Rotation = unit.Transform.Rotation,
                                    Scale = 1f
                                });
                                
                                if (!EntityManager.HasComponent<Projectile>(bullet)) return;
                                
                                EntityManager.SetComponentData(bullet, new Projectile {
                                    damage = unit.OffensiveStats.damage,
                                    speed = 10f,
                                    direction = math.normalize(unit.TargetPosition - unit.Transform.Position),
                                    owner = unit.Entity,
                                });
                                
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
    }
}
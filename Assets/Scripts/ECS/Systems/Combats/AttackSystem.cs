using ECS.Aspects.Units;
using ECS.Components.Units;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Systems.Combats {
    public partial struct AttackSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<AttackTimer>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var (unit, attackTimer) in SystemAPI.Query<UnitAspect, RefRW<AttackTimer>>())
                if (attackTimer.ValueRW.CanAttack()) {
                    if (IsAbleToAttack(unit)) {
                        // Todo: Implement attack logic here
                        
                        
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
    }
}
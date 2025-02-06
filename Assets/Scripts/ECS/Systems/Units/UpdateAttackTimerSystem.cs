using ECS.Aspects.Units;
using ECS.Components.Units;
using Unity.Burst;
using Unity.Entities;

namespace ECS.Systems.Units {
    public partial struct UpdateAttackTimerSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<AttackTimer>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var unitAspect in SystemAPI.Query<UnitAspect>()) {
                unitAspect.UpdateAttackTimer();
            }
        }
    }
}
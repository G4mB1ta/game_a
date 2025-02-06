using ECS.Components.Units;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Systems.Units {
    [UpdateBefore(typeof(FindTargetSystem))]
    public partial struct UpdateTargetPosition : ISystem {
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var targetData in SystemAPI.Query<RefRW<TargetData>>()) {
                var target = targetData.ValueRO.target;
                if (!state.EntityManager.Exists(target) || target == Entity.Null) {
                    targetData.ValueRW.position = float3.zero;
                }
                else {
                    targetData.ValueRW.position = SystemAPI.GetComponent<LocalTransform>(target).Position;
                }
            }
        }
    }
}
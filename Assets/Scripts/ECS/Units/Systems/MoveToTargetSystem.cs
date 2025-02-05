using ECS.Units.Aspects;
using ECS.Units.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Units.Systems {
    [BurstCompile]
    public partial struct MoveToTargetSystem : ISystem {

        public void OnUpdate(ref SystemState state) {
            foreach (var unitAspect in SystemAPI.Query<UnitAspect>()) {
                if (state.EntityManager.Exists(unitAspect.Target)) {
                    var targetTransform =
                        state.EntityManager.GetComponentData<LocalTransform>(unitAspect.Target);
                    var direction = math.normalize(targetTransform.Position - unitAspect.Transform.Position);
                    unitAspect.Move(direction, SystemAPI.Time.DeltaTime);
                }
            }
        }
    }
}
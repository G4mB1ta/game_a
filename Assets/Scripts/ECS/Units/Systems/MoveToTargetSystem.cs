using ECS.Units.Aspects;
using ECS.Units.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Units.Systems {
    /// <inheritdoc cref="Unity.Entities.ISystem" />
    /// <summary>
    /// System that moves units towards their target if the target exists or is out of attack range.
    /// </summary>
    [BurstCompile]
    public partial struct MoveToTargetSystem : ISystem {

        public void OnUpdate(ref SystemState state) {
            foreach (var unitAspect in SystemAPI.Query<UnitAspect>()) {
                var transform = unitAspect.Transform;
                
                // If the target exists, move towards it
                if (state.EntityManager.Exists(unitAspect.Target)) {
                    var targetTransform =
                        state.EntityManager.GetComponentData<LocalTransform>(unitAspect.Target);
                    var direction = math.normalize(targetTransform.Position - transform.Position);

                    // If the target is out of attack range, move towards it
                    if (math.distance(targetTransform.Position, transform.Position)
                        > unitAspect.OffensiveStats.attackRange) {
                        unitAspect.Move(direction, SystemAPI.Time.DeltaTime);
                    }
                }
            }
        }
    }
}
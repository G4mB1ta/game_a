using ECS.Systems.Units;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnitAspect = ECS.Aspects.Units.UnitAspect;

namespace ECS.Systems.Units {
    
    /// <inheritdoc cref="Unity.Entities.ISystem" />
    /// <summary>
    ///     System that moves units towards their target if the target exists or is out of attack range.
    /// </summary>
    [UpdateAfter(typeof(FindTargetSystem))]
    public partial struct MoveToTargetSystem : ISystem {

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var unitAspect in SystemAPI.Query<UnitAspect>()) {
                var transform = unitAspect.Transform;

                // If the target exists, move towards it
                if (state.EntityManager.Exists(unitAspect.TargetEntity)) {
                    var targetTransform =
                        state.EntityManager.GetComponentData<LocalTransform>(unitAspect.TargetEntity);
                    var direction = math.normalize(targetTransform.Position - transform.Position);

                    // If the target is out of attack range, move towards it
                    if (math.distance(targetTransform.Position, transform.Position)
                        > unitAspect.OffensiveStats.attackRange)
                        unitAspect.Move(direction, SystemAPI.Time.DeltaTime);
                }
            }
        }
    }
}
using ECS.Units.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Units.Systems.Temp {
    public partial class FindTargetSystem : SystemBase {
        

        protected override void OnUpdate() {
            // Find all entities with the TargetComponent
            foreach (var (targetComponent, entity)
                     in SystemAPI.Query<RefRW<TargetComponent>>()
                         .WithEntityAccess())

                // If entity is not targeting, find a target
                if (targetComponent.ValueRO.target == Entity.Null) {
                    targetComponent.ValueRW.target = SearchForNewTarget(entity);
                }

                // If entity is targeting, check if target is still alive
                else {
                    // If target is dead, set target to null
                    if (!EntityManager.Exists(targetComponent.ValueRO.target)) {
                        targetComponent.ValueRW.target = SearchForNewTarget(entity);
                    }
                    // If target is alive
                    else {
                        var entityPosition = EntityManager.GetComponentData<LocalTransform>(entity).Position;
                        var entityPosition2D = new float2(entityPosition.x, entityPosition.z);

                        var targetPosition = EntityManager
                            .GetComponentData<LocalTransform>(targetComponent.ValueRO.target).Position;
                        var targetPosition2D = new float2(targetPosition.x, targetPosition.z);

                        // If target is out of range, search for a new target 
                        if (math.distance(targetPosition2D, entityPosition2D) >
                            EntityManager.GetComponentData<AttackComponent>(entity).attackRange ||
                            !IsValidTarget(entity, targetComponent.ValueRO.target))
                            targetComponent.ValueRW.target = SearchForNewTarget(entity);
                    }
                }
            
        }

        private bool IsValidTarget(Entity entity, Entity target) {
            if (EntityManager.HasComponent<TeamComponent>(target) == false) return false;
            
            return EntityManager.HasComponent<HealthComponent>(entity) &&
                   EntityManager.GetComponentData<TeamComponent>(entity).team !=
                   EntityManager.GetComponentData<TeamComponent>(target).team;
        }

        private Entity SearchForNewTarget(Entity entity) {
            Entity target = Entity.Null;
            var entityPosition = EntityManager.GetComponentData<LocalTransform>(entity).Position;
            var entityPosition2D = new float2(entityPosition.x, entityPosition.z);
            var closestDistance = float.MaxValue;
            
            // Find all entities with the HealthComponent and set target to the nearest entity
            foreach (var (targetTransform, targetEntity)
                     in SystemAPI.Query<RefRO<LocalTransform>>().WithEntityAccess()) {
                
                
                var targetPosition = targetTransform.ValueRO.Position;
                var targetPosition2D = new float2(targetPosition.x, targetPosition.z);
                var distance = math.distance(targetPosition2D, entityPosition2D);

                if (distance < closestDistance && IsValidTarget(entity, targetEntity)) {
                    closestDistance = distance;
                    target = targetEntity;
                }
            }

            return target;
        }
    }
}
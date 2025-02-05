using ECS.Units.Aspects;
using ECS.Units.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Units.Systems.Temp {
    [BurstCompile]
    public partial struct FindTargetSystem : ISystem {
        
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<TargetComponent>();
        }

        public void OnUpdate(ref SystemState state) {
            foreach (var unitAspect in SystemAPI.Query<UnitAspect>()) {
                
                if (unitAspect.Target == Entity.Null) {
                    unitAspect.Target = SearchForNewTarget(ref state, unitAspect.Entity);
                }
                else {
                    if (!state.EntityManager.Exists(unitAspect.Target)) {
                        unitAspect.Target = SearchForNewTarget(ref state, unitAspect.Entity);
                    }
                    else {
                        var entityPosition = 
                            state.EntityManager.GetComponentData<LocalTransform>(unitAspect.Entity).Position;
                        var entityPosition2D = new float2(entityPosition.x, entityPosition.z);

                        var targetPosition = 
                            state.EntityManager.GetComponentData<LocalTransform>(unitAspect.Target).Position;
                        var targetPosition2D = new float2(targetPosition.x, targetPosition.z);

                        if (math.distance(targetPosition2D, entityPosition2D) >
                            state.EntityManager.GetComponentData<OffensiveStats>(unitAspect.Entity).attackRange ||
                            !IsValidTarget(ref state, unitAspect.Entity, unitAspect.Target))
                            unitAspect.Target = SearchForNewTarget(ref state, unitAspect.Entity);
                    }
                }
            }
        }
        

        private bool IsValidTarget(ref SystemState state, Entity entity, Entity target) {
            if (state.EntityManager.HasComponent<SideTag>(target) == false) return false;
            
            return state.EntityManager.HasComponent<Health>(entity) &&
                   state.EntityManager.GetComponentData<SideTag>(entity).Side !=
                   state.EntityManager.GetComponentData<SideTag>(target).Side;
        }

        private Entity SearchForNewTarget(ref SystemState state, Entity entity) {
            Entity target = Entity.Null;
            var entityPosition = state.EntityManager.GetComponentData<LocalTransform>(entity).Position;
            var entityPosition2D = new float2(entityPosition.x, entityPosition.z);
            var closestDistance = float.MaxValue;
            
            // Find all entities with the HealthComponent and set target to the nearest entity
            foreach (var (targetTransform, targetEntity)
                     in SystemAPI.Query<RefRO<LocalTransform>>().WithEntityAccess()) {
                
                var targetPosition = targetTransform.ValueRO.Position;
                var targetPosition2D = new float2(targetPosition.x, targetPosition.z);
                var distance = math.distance(targetPosition2D, entityPosition2D);

                if (distance < closestDistance && IsValidTarget(ref state, entity, targetEntity)) {
                    closestDistance = distance;
                    target = targetEntity;
                }
            }
            return target;
        }
    }
}
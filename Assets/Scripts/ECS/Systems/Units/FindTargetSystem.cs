using ECS.Components.Units;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnitAspect = ECS.Aspects.Units.UnitAspect;

namespace ECS.Systems.Units {
    public partial struct FindTargetSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<TargetData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var unit in SystemAPI.Query<UnitAspect>())
                if (unit.TargetEntity == Entity.Null) {
                    unit.TargetEntity = SearchForNewTarget(ref state, unit);
                }
                else {
                    if (!state.EntityManager.Exists(unit.TargetEntity)) {
                        unit.TargetEntity = SearchForNewTarget(ref state, unit);
                    }
                    else {
                        var entityPosition = unit.Transform.Position;
                        var entityPosition2D = new float2(entityPosition.x, entityPosition.z);

                        var targetPosition = unit.TargetPosition;
                        var targetPosition2D = new float2(targetPosition.x, targetPosition.z);

                        if (math.distance(targetPosition2D, entityPosition2D) > unit.OffensiveStats.attackRange ||
                            !IsValidTarget(ref state, unit.Entity, unit.TargetEntity))
                            unit.TargetEntity = SearchForNewTarget(ref state, unit);
                    }
                }
        }

        
        /// <summary>
        ///     Check if the target is a valid target.
        /// </summary>
        /// <param name="state">state of current frame</param>
        /// <param name="unit"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool IsValidTarget(UnitAspect unit, UnitAspect target) {

            bool isSameSide = unit.SideTag.side == target.SideTag.side;

            return !isSameSide;
        }
        
        /// <summary>
        ///     Check if the target is a valid target.
        /// </summary>
        /// <param name="state">state of current frame</param>
        /// <param name="unit"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private bool IsValidTarget(ref SystemState state, Entity unit, Entity target) {
            bool isSameSide = state.EntityManager.GetComponentData<SideTag>(unit).side ==
                              state.EntityManager.GetComponentData<SideTag>(target).side;
            return !isSameSide;
        }

        private Entity SearchForNewTarget(ref SystemState state, UnitAspect unit) {
            var target = Entity.Null;
            var entityPosition = unit.Transform.Position;
            var entityPosition2D = new float2(entityPosition.x, entityPosition.z);
            var closestDistance = float.MaxValue;

            foreach (var targetAspect in SystemAPI.Query<UnitAspect>()) {
                var targetPostion = targetAspect.Transform.Position;
                var targetPosition2D = new float2(targetPostion.x, targetPostion.z);
                var distance = math.distance(targetPosition2D, entityPosition2D);
                
                if (distance < closestDistance && IsValidTarget(unit, targetAspect)) {
                    closestDistance = distance;
                    target = targetAspect.Entity;
                }
            }
            return target;
        }
    }
}
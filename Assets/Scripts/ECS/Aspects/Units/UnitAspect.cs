using ECS.Components.Units;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Aspects.Units 
{
    public readonly partial struct UnitAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> transform;
        private readonly RefRW<SideTag> sideTag;
        private readonly RefRW<Movement> movement;
        private readonly RefRW<TargetData> target;
        private readonly RefRW<Health> health;
        private readonly RefRW<AttackStats> offensiveStats;
        private readonly RefRW<AttackTimer> attackTimer;

        public LocalTransform Transform
        {
            get => transform.ValueRO;
            set => transform.ValueRW = value;
        }

        public SideTag SideTag
        {
            get => sideTag.ValueRO;
            set => sideTag.ValueRW = value;
        }

        public AttackStats AttackStats
        {
            get => offensiveStats.ValueRO;
            set => offensiveStats.ValueRW = value;
        }

        public AttackTimer AttackTimer
        {
            get => attackTimer.ValueRO;
            set => attackTimer.ValueRW = value;
        }

        public TargetData TargetData
        {
            get => target.ValueRO;
            set => target.ValueRW = value;
        }

        public Entity TargetEntity
        {
            get => target.ValueRO.target;
            set => target.ValueRW.target = value;
        }

        public float3 TargetPosition
        {
            get => target.ValueRO.position;
            set => target.ValueRW.position = value;
        }

        /// <summary>
        ///     Move the unit in the specified direction with deltaTime.
        /// </summary>
        /// <param name="direction">The direction of movement.</param>
        /// <param name="deltaTime">The deltaTime of current frame.</param>
        public void Move(float3 direction, float deltaTime)
        {
            transform.ValueRW.Position += direction * movement.ValueRO.speed * deltaTime;
        }

        
        /// <summary>
        ///     Update attacker timer based on the attack speed.
        /// </summary>
        public void UpdateAttackTimer()
        {
            if (offensiveStats.ValueRO.attackSpeed > 0)
            {
                attackTimer.ValueRW.attackInterval = 1f / offensiveStats.ValueRO.attackSpeed;
            }
            else
            {
                attackTimer.ValueRW.attackInterval = float.MaxValue;
            }
        }

        /// <summary>
        ///     Check if the target is within the attack range.
        /// </summary>
        /// <returns>true if target is in the attack range</returns>
        public bool IsTargetInRange() {
            var unitPosition = transform.ValueRO.Position;
            var targetPosition = target.ValueRO.position;
            
            var unitPosition2D = new float2(unitPosition.x, unitPosition.z);
            var targetPosition2D = new float2(targetPosition.x, targetPosition.z);
            
            return math.distance(unitPosition2D, targetPosition2D) <= offensiveStats.ValueRO.range;
        }
    }
}
using ECS.Components.Units;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Aspects.Units 
{
    // Add the partial keyword and implement IAspect
    public readonly partial struct UnitAspect : IAspect
    {
        public readonly Entity Entity;

        // Components should be marked as readonly
        private readonly RefRW<LocalTransform> transform;
        private readonly RefRW<SideTag> sideTag;
        private readonly RefRW<Movement> movement;
        private readonly RefRW<TargetData> target;
        private readonly RefRW<Health> health;
        private readonly RefRW<OffensiveStats> offensiveStats;
        private readonly RefRW<AttackTimer> attackTimer;

        // Properties
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

        public OffensiveStats OffensiveStats
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

        // Methods
        public void Move(float3 direction, float deltaTime)
        {
            transform.ValueRW.Position += direction * movement.ValueRO.speed * deltaTime;
        }

        public void UpdateAttackTimer()
        {
            if (offensiveStats.ValueRO.attackSpeed > 0)
            {
                attackTimer.ValueRW.attackInterval = 1f / offensiveStats.ValueRO.attackSpeed;
            }
            else
            {
                // Consider using a different logging approach that's compatible with Burst
                // Debug.LogWarning isn't Burst compatible
                // You might want to handle this case differently
                attackTimer.ValueRW.attackInterval = float.MaxValue;
            }
        }

        public void Attack(EntityCommandBuffer.ParallelWriter commandBuffer, int sortKey)
        {
            // Implement attack logic using command buffer
            // This is just an example implementation
            if (target.ValueRO.target != Entity.Null)
            {
                // Add attack logic here using command buffer
                // Example: Create damage event, etc.
            }
        }

        public bool IsTargetInRange() {
            var unitPosition = transform.ValueRO.Position;
            var targetPosition = target.ValueRO.position;
            
            var unitPosition2D = new float2(unitPosition.x, unitPosition.z);
            var targetPosition2D = new float2(targetPosition.x, targetPosition.z);
            
            return math.distance(unitPosition2D, targetPosition2D) <= offensiveStats.ValueRO.range;
        }
    }
}
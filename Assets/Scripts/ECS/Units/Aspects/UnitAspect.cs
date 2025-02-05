using ECS.Units.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Units.Aspects {
    public readonly partial struct UnitAspect : IAspect {
        private readonly Entity _entity;

        private readonly RefRW<Unit> _unit;
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<Movement> _movement;

        private readonly RefRW<TargetComponent> _target;
        private readonly RefRW<Health> _health;
        private readonly RefRW<OffensiveStats> _offensiveStats;
        
        public Entity Entity => _entity;

        public LocalTransform Transform {
            get => _transform.ValueRO;
            set => _transform.ValueRW = value;
        }
        
        public OffensiveStats OffensiveStats {
            get => _offensiveStats.ValueRO;
            set => _offensiveStats.ValueRW = value;
        }

        public Entity Target {
            get => _target.ValueRO.target;
            set => _target.ValueRW.target = value;
        }

        public void SetTarget(Entity target) {
            _target.ValueRW.target = target;
        }
        
        public Entity GetTarget() {
            return _target.ValueRO.target;
        }

        public void Move(float3 direction, float deltaTime) {
            _transform.ValueRW.Position += direction * _movement.ValueRO.speed * deltaTime;
        }

        public Entity GetEntity() {
            return _entity;
        }
    }
}
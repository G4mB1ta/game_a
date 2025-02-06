using ECS.Components.Units;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Aspects.Units {
    public readonly partial struct UnitAspect : IAspect {
        private readonly Entity _entity;

        private readonly RefRW<LocalTransform> _transform;

        
        private readonly RefRW<Unit> _unit;
        private readonly RefRW<SideTag> _sideTag;
        private readonly RefRW<Movement> _movement;
        private readonly RefRW<TargetData> _target;
        private readonly RefRW<Health> _health;
        private readonly RefRW<OffensiveStats> _offensiveStats;
        
        private readonly RefRW<AttackTimer> _attackTimer;
        
        public Entity Entity => _entity;

        public LocalTransform Transform {
            get => _transform.ValueRO;
            set => _transform.ValueRW = value;
        }
        
        public SideTag SideTag {
            get => _sideTag.ValueRO;
            set => _sideTag.ValueRW = value;
        }
        
        public OffensiveStats OffensiveStats {
            get => _offensiveStats.ValueRO;
            set => _offensiveStats.ValueRW = value;
        }
        
        public Entity TargetEntity {
            get => _target.ValueRO.target;
            set => _target.ValueRW.target = value;
        }
        
        public float3 TargetPosition {
            get => _target.ValueRO.position;
            set => _target.ValueRW.position = value;
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

        public void UpdateAttackTimer() {
            if (_offensiveStats.ValueRO.attackSpeed != 0) {
                _attackTimer.ValueRW.interval = 60 / _offensiveStats.ValueRO.attackSpeed;
            } else {
                Debug.LogWarning("Attack speed is zero, cannot update attack timer.");
            }
        }
    }
}
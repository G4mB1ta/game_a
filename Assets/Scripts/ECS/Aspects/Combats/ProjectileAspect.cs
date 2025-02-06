using ECS.Components.Combats;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Aspects.Combats {
    public readonly partial struct ProjectileAspect : IAspect {
        private readonly Entity _entity;
        private readonly RefRW<Projectile> _projectile;
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<DamageValue> _damageValue;
        
        public LocalTransform Transform {
            get => _transform.ValueRO;
            set => _transform.ValueRW = value;
        }
        
        public void SetDirection(float3 direction) {
            _projectile.ValueRW.direction = direction;
        }
        
        public void Move(float deltaTime) {
            _transform.ValueRW.Position += _projectile.ValueRO.direction * _projectile.ValueRO.speed * deltaTime;
        }
    }
}
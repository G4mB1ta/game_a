using ECS.Components.Combats;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.Systems.Combats {
    public partial struct ProjectileSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Projectile>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var (localTransform, projectile) 
                     in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Projectile>>())
                localTransform.ValueRW.Position +=
                    projectile.ValueRO.direction * projectile.ValueRO.speed * SystemAPI.Time.DeltaTime;
        }
    }
}
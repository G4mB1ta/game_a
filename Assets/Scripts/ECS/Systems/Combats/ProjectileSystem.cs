using ECS.Components.Combats;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Systems.Combats {
    public partial struct ProjectileSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Projectile>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            UpdateProjectile(ref state, ecb);
        }

        /// <summary>
        ///     Update the projectile's position and rotation. The projectile will move towards the target.
        /// </summary>
        private void UpdateProjectile(ref SystemState state, EntityCommandBuffer ecb) {
            foreach (var (localTransform, projectile, entity)
                     in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Projectile>>().WithEntityAccess()) {
                
                // Update the timer of the projectile
                projectile.ValueRW.timer += SystemAPI.Time.fixedDeltaTime;
                if (projectile.ValueRO.timer > projectile.ValueRO.lifeTime) {
                    // Schedule entity destruction instead of immediate destruction
                    ecb.DestroyEntity(entity);
                    continue;
                }

                var position = localTransform.ValueRO.Position;

                if (projectile.ValueRO.target != Entity.Null) {
                    var targetPosition =
                        state.EntityManager.GetComponentData<LocalTransform>(projectile.ValueRO.target).Position;

                    // Update direction of the projectile
                    projectile.ValueRW.direction = math.normalize(targetPosition - position);

                    // Update the rotation of the projectile
                    localTransform.ValueRW.Rotation =
                        quaternion.LookRotationSafe(targetPosition - localTransform.ValueRO.Position, math.up());
                }

                // Update the position of the projectile
                localTransform.ValueRW.Position +=
                    projectile.ValueRO.direction * projectile.ValueRO.speed * SystemAPI.Time.DeltaTime;
            }
        }
    }
}
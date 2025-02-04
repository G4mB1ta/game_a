using ECS.Units.Components;
using Unity.Entities;

namespace ECS.Units.Systems {
    public partial struct HealthSystem : ISystem {
        public void OnUpdate(ref SystemState state) {
            foreach (var (health, entity) 
                     in SystemAPI.Query<RefRO<HealthComponent>>().WithEntityAccess()) {
                
                // If the health is less than or equal to 0, Entity manager will kill the entity
                if (health.ValueRO.currenthealth <= 0) {
                    state.EntityManager.DestroyEntity(entity);
                }
                
                //
            }
        }
    }
}
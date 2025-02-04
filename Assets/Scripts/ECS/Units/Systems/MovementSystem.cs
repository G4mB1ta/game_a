using ECS.Units.Components;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.Units.Systems {
    public partial class MovementSystem : SystemBase {
        protected override void OnUpdate() {
            foreach (var (transform, move)
                     in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveComponent>> ()) {
                
                
            }
        }
    }
}
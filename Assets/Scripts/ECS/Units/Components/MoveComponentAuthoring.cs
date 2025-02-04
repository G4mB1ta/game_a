using Unity.Entities;
using UnityEngine;

namespace ECS.Units.Components {
    public class MoveComponentAuthoring : MonoBehaviour {
        public float speed;
    }
    
    public struct MoveComponent : IComponentData {
        public float speed;
    }
    
    public class MoveComponentBaker : Baker<MoveComponentAuthoring> {
        public override void Bake(MoveComponentAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MoveComponent {
                speed = authoring.speed
            });
        }
    }
}
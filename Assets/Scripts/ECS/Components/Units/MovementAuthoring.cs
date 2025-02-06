using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Units {
    public class MovementAuthoring : MonoBehaviour {
        public float speed;
        
        public class MovementBaker : Baker<MovementAuthoring> {
            public override void Bake(MovementAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Movement {
                    speed = authoring.speed
                });
            }
        }
    }
    
    public struct Movement : IComponentData {
        public float speed;
    }
}
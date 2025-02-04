using Unity.Entities;
using UnityEngine;

namespace ECS.Units.Components {
    public class BuffComponentAuthoring : MonoBehaviour {
        
    }
    
    public struct BuffComponent : IComponentData {
        
    }
    
    public class BuffComponentBaker : Baker<BuffComponentAuthoring> {
        public override void Bake(BuffComponentAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BuffComponent {
                
            });
        }
    }
}
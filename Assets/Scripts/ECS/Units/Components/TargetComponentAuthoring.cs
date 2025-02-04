using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Units.Components {
    public class TargetComponentAuthoring : MonoBehaviour {
    }
    
    public struct TargetComponent : IComponentData {
        public Entity target;
    }
    
    public class TargetComponentBaker : Baker<TargetComponentAuthoring> {
        public override void Bake(TargetComponentAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // Set the target entity as null            
            AddComponent(entity, new TargetComponent {
                target = Entity.Null,
            });
        }
    }
}
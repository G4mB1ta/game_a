using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Components.Units {
    public class TargetDataAuthoring : MonoBehaviour {
        public class TargetDataBaker : Baker<TargetDataAuthoring> {
            public override void Bake(TargetDataAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
            
                // Set the target entity as null            
                AddComponent(entity, new TargetData {
                    target = Entity.Null,
                });
            }
        }
    }
    
    public struct TargetData : IComponentData {
        public Entity target;
        public float3 position;
    }

}
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Units.Components {
    public class SideTagAuthoring : MonoBehaviour {
        public Side side;
        public class SideTagAuthoringBaker : Baker<SideTagAuthoring> {
            public override void Bake(SideTagAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SideTag {
                    Side = authoring.side
                });
            }
        }
    }

    public struct SideTag : IComponentData {
        public Side Side;
    }
}
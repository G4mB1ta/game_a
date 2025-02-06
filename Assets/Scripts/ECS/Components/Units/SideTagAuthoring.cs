using ECS.Enums;
using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Units {
    public class SideTagAuthoring : MonoBehaviour {
        public Side side;
        public class SideTagAuthoringBaker : Baker<SideTagAuthoring> {
            public override void Bake(SideTagAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SideTag {
                    side = authoring.side
                });
            }
        }
    }

    public struct SideTag : IComponentData {
        public Side side;
    }
}
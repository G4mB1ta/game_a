using ECS.Enums;
using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Combats {
    public class OwnerInfoAuthoring : MonoBehaviour {
        private class OwnerInfoAuthoringBaker : Baker<OwnerInfoAuthoring> {
            public override void Bake(OwnerInfoAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new OwnerInfo {
                });
            }
        }
    }
    
    public struct OwnerInfo : IComponentData {
        public Entity owner;
    }
}
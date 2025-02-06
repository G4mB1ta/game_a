using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Combats {
    public class DamageValueAuthoring : MonoBehaviour {
        public float value;

        private class DamageValueAuthoringBaker : Baker<DamageValueAuthoring> {
            public override void Bake(DamageValueAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DamageValue {
                    value = authoring.value
                });
            }
        }
    }

    public struct DamageValue : IComponentData {
        public float value;
    }
}
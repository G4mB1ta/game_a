using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Units {
    public class HealthAuthoring : MonoBehaviour {
        public float maxHealth;
        private class HealthBaker : Baker<HealthAuthoring> {
            public override void Bake(HealthAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Health {
                    maxHealth = authoring.maxHealth,
                    currenthealth = authoring.maxHealth
                });
            }
        }
    }

    public struct Health : IComponentData {
        public float maxHealth;
        public float currenthealth;
    }
}
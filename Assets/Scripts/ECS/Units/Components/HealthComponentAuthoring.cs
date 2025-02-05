using Unity.Entities;
using UnityEngine;

namespace ECS.Units.Components {
    public class HealthComponentAuthoring : MonoBehaviour {
        public float maxHealth;
        private class HealthBaker : Baker<HealthComponentAuthoring> {
            public override void Bake(HealthComponentAuthoring authoring) {
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
using Unity.Entities;
using UnityEngine;

namespace ECS.Units.Components {
    public class HealthComponentAuthoring : MonoBehaviour {
        public float maxHealth;
    }

    public struct HealthComponent : IComponentData {
        public float maxHealth;
        public float currenthealth;
    }

    public class HealthComponentBaker : Baker<HealthComponentAuthoring> {
        public override void Bake(HealthComponentAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new HealthComponent {
                maxHealth = authoring.maxHealth,
                currenthealth = authoring.maxHealth
            });
        }
    }
}
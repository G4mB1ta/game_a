using Unity.Entities;
using UnityEngine;

namespace ECS.Units.Components {
    public class OffensiveStatsAuthoring : MonoBehaviour {
        public float attackDamage;
        public float attackSpeed;
        public float attackRange;
        public class OffensiveBaker : Baker<OffensiveStatsAuthoring> {
            public override void Bake(OffensiveStatsAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new OffensiveStats {
                    attackDamage = authoring.attackDamage,
                    attackSpeed = authoring.attackSpeed,
                    attackRange = authoring.attackRange
                });
            }
        }
    }

    public struct OffensiveStats : IComponentData {
        public float attackDamage;
        public float attackSpeed;
        public float attackRange;
    }
}
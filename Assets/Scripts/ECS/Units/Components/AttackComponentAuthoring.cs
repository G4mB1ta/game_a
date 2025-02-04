using Unity.Entities;
using UnityEngine;

namespace ECS.Units.Components {
    public class AttackComponentAuthoring : MonoBehaviour {
        public float attackDamage;
        public float attackSpeed;
        public float attackRange;
    }

    public struct AttackComponent : IComponentData {
        public float attackDamage;
        public float attackSpeed;
        public float attackRange;
    }

    public class AttackComponentBaker : Baker<AttackComponentAuthoring> {
        public override void Bake(AttackComponentAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new AttackComponent {
                attackDamage = authoring.attackDamage,
                attackSpeed = authoring.attackSpeed,
                attackRange = authoring.attackRange
            });
        }
    }
}
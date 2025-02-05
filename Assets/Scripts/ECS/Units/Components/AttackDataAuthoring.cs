using Unity.Entities;
using UnityEngine;

namespace ECS.Units.Components {
    public class AttackDataAuthoring : MonoBehaviour {
        public float attackDamage;
        public float attackSpeed;
        public float attackRange;
        public class AttackDataBaker : Baker<AttackDataAuthoring> {
            public override void Bake(AttackDataAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new AttackData {
                    attackDamage = authoring.attackDamage,
                    attackSpeed = authoring.attackSpeed,
                    attackRange = authoring.attackRange
                });
            }
        }
    }

    public struct AttackData : IComponentData {
        public float attackDamage;
        public float attackSpeed;
        public float attackRange;
    }
}
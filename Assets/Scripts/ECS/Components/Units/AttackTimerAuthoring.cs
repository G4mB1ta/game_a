using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Units {
    public class AttackTimerAuthoring : MonoBehaviour {
        private class AttackTimerAuthoringBaker : Baker<AttackTimerAuthoring> {
            public override void Bake(AttackTimerAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new AttackTimer {
                    
                });
            }
        }
    }
    
    public struct AttackTimer : IComponentData {
        public float timeElapsed;
        public float interval;
    }
}
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Components.Combats {
    public class ProjectileAuthoring : MonoBehaviour {
        public float speed;
        private class ProjectileAuthoringBaker : Baker<ProjectileAuthoring> {
            public override void Bake(ProjectileAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Projectile {
                    speed = authoring.speed,
                });
            }
        }
    }
    
    public struct Projectile : IComponentData {
        public float speed;
        public float3 direction;
    }
}
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Components.Combats {
    public class ProjectileAuthoring : MonoBehaviour {
        public float damage;
        public float speed;
        public float3 direction;
        private class ProjectileAuthoringBaker : Baker<ProjectileAuthoring> {
            public override void Bake(ProjectileAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Projectile {
                    damage = authoring.damage,
                    speed = authoring.speed,
                    direction = authoring.direction
                });
            }
        }
    }
    
    public struct Projectile : IComponentData {
        public float damage;
        public float speed;
        public float3 direction;
        public Entity owner;
    }
}
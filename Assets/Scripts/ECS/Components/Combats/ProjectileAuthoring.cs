using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Components.Combats {
    public class ProjectileAuthoring : MonoBehaviour {
        public float damage;
        public float speed;
        public float3 direction;
        public float lifeTime;
        public float timer = 0;
        
        private class ProjectileAuthoringBaker : Baker<ProjectileAuthoring> {
            public override void Bake(ProjectileAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Projectile {
                    damage = authoring.damage,
                    speed = authoring.speed,
                    direction = authoring.direction,
                    lifeTime = authoring.lifeTime,
                    timer = authoring.timer,
                    target = Entity.Null,
                    owner = Entity.Null
                });
            }
        }
    }
    
    public struct Projectile : IComponentData {
        public float damage;
        public float speed;
        public float3 direction;
        public Entity owner;
        public float timer;
        public float lifeTime;
        public Entity target;
    }
}
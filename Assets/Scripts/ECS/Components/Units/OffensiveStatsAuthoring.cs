using System;
using ECS.Enums;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Components.Units {
    public class OffensiveStatsAuthoring : MonoBehaviour {
        [Header("Component Data")]
        public AttackType type;
        public float damage;
        public float attackSpeed;
        public float range;
        public GameObject projectilePrefab;

        [Header("Debug")] 
        [SerializeField] private Color color = Color.green;
        [SerializeField] private int segments = 32;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            DrawGizmoCircle(transform.position, range, segments);
        }
        
        /// <summary>
        ///     Draw a circle in the scene view using gizmos.
        /// </summary>
        /// <param name="center">Vector3 as center of circle.</param>
        /// <param name="radius">Distance of center to lines.</param>
        /// <param name="segments">Numbers of lines in circle.</param>
        void DrawGizmoCircle(Vector3 center, float radius, int segments)
        {
            float angleStep = 360f / segments;
            Vector3 prevPoint = center + new Vector3(radius, 0, 0);
        
            for (int i = 1; i <= segments; i++)
            {
                float angle = Mathf.Deg2Rad * (i * angleStep);
                Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(prevPoint, newPoint);
                prevPoint = newPoint;
            }
        }

        public class OffensiveBaker : Baker<OffensiveStatsAuthoring> {
            public override void Bake(OffensiveStatsAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new OffensiveStats {
                    type = authoring.type,
                    damage = authoring.damage,
                    attackSpeed = authoring.attackSpeed,
                    range = authoring.range,
                    projectilePrefab = GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct OffensiveStats : IComponentData {
        public AttackType type;
        public float damage;
        public float attackSpeed;
        public float range;
        public Entity projectilePrefab;
    }
}
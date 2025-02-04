using Unity.Entities;
using UnityEngine;

namespace ECS.Units.Components {
    public class TeamComponentAuthoring : MonoBehaviour {
        public Team team;
    }

    public struct TeamComponent : IComponentData {
        public Team team;
    }

    public class TeamComponentAuthoringBaker : Baker<TeamComponentAuthoring> {
        public override void Bake(TeamComponentAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TeamComponent {
                team = authoring.team
            });
        }
    }
}
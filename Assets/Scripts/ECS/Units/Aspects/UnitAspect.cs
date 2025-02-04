using ECS.Units.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Units.Aspects {
    public readonly partial struct UnitAspect : IAspect {
        public readonly Entity Entity;

        // UnityAssetReference
        private readonly RefRO<LocalTransform> localTransform;
        
        // TeamComponent
        private readonly RefRO<TeamComponent> teamComponent;
        
        private readonly RefRO<HealthComponent> healthComponent;
        
        private readonly RefRW<TargetComponent> targetComponent;
        private readonly RefRO<AttackComponent> attackComponent;
        
        public float3 Position => localTransform.ValueRO.Position;
        public float AttackRange => attackComponent.ValueRO.attackRange;
        public Entity Target { get => targetComponent.ValueRO.target; set => targetComponent.ValueRW.target = value; }
        public Team Team => teamComponent.ValueRO.team;
        
    }
}
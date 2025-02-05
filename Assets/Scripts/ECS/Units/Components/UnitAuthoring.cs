﻿using Unity.Entities;
using UnityEngine;

namespace ECS.Units.Components {
    public class UnitAuthoring : MonoBehaviour {
        private class UnitAuthoringBaker : Baker<UnitAuthoring> {
            public override void Bake(UnitAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Unit());
            }
        }
    }
    
    public struct Unit : IComponentData {
    }
}
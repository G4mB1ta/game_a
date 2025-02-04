using Unity.Entities;
using UnityEngine;

public class RotateSpeedAuthoring : MonoBehaviour {
    public float rotateSpeed;
}

public struct RotateSpeed : IComponentData {
    public float rotateSpeed;
}

public class RotateSpeedBaker : Baker<RotateSpeedAuthoring> {
    public override void Bake(RotateSpeedAuthoring authoring) {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new RotateSpeed {
            rotateSpeed = authoring.rotateSpeed
        });
    }
}
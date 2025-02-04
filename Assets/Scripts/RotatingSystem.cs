using Unity.Entities;
using Unity.Transforms;

public partial struct RotatingSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        foreach (var (transform, rotateSpeed)
                 in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>()) {
            transform.ValueRW = transform.ValueRW.RotateY(rotateSpeed.ValueRO.rotateSpeed * SystemAPI.Time.DeltaTime);
        }
    }
}
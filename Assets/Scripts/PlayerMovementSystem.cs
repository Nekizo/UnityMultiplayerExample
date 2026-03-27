using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerInput>();
    }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((
            RefRO<PlayerInput> input,
            RefRW<LocalTransform> transform
        )in SystemAPI.Query<RefRO<PlayerInput>, RefRW<LocalTransform>>()
            .WithAll<Simulate>()
        ){
            float speed =10;
            transform.ValueRW.Position +=new float3( input.ValueRO.move,0)*speed*SystemAPI.Time.DeltaTime;
            // Debug.Log(new float3( input.ValueRO.move,0));
            
            
        }
        
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

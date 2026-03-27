using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using Unity.NetCode;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct TestNetcodeEntitiesClientSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // state.RequireForUpdate<PlayerInput>();
    }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // PlayerInput playerInput =SystemAPI.GetSingleton<PlayerInput>();
        // if(Input.GetKeyDown(KeyCode.Tab))
        // {
        //     Entity rpcEntity = state.EntityManager.CreateEntity();
        //     // state.EntityManager.AddComponentData(rpcEntity, new SimpleRpc
        //     // {
        //     //     value=34,
        //     // } );
        //     // state.EntityManager.AddComponentData(rpcEntity,new SendRpcCommandRequest());
        // }

        
       
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

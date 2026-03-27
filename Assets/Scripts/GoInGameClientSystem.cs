// #define DEBUG_LOG
#nullable enable
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

#if DEBUG_LOG
    using UnityEngine;
#endif

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct GoInGameClientSystem : ISystem
{
    #if !DEBUG_LOG
        [BurstCompile]
    #endif
    public void OnCreate(ref SystemState state)
    {
       state.RequireForUpdate<EntitesReferences>(); 
       state.RequireForUpdate<NetworkId>(); 
    //    state.RequireForUpdate<PlayerInput>();
    }

    #if !DEBUG_LOG
        [BurstCompile]
    #endif
    public void OnUpdate(ref SystemState state)
    {
        EntitesReferences entitesReferences = SystemAPI.GetSingleton<EntitesReferences>();
        // PlayerInput playerInput =SystemAPI.GetSingleton<PlayerInput>();
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach((
                RefRO<NetworkId> networkId,
                Entity entity
            ) in SystemAPI.Query<RefRO<NetworkId>>().
                WithNone<NetworkStreamInGame>().
                WithEntityAccess()
            ){
            entityCommandBuffer.AddComponent<NetworkStreamInGame>(entity);
            // entityCommandBuffer.Playback(state.EntityManager);
            #if DEBUG_LOG
                Debug.Log("Setting Client as in Game");
            #endif

            Entity rpcEntity = entityCommandBuffer.CreateEntity();
            entityCommandBuffer.AddComponent(rpcEntity,new GoInGameRequestRpc());
            entityCommandBuffer.AddComponent(rpcEntity,new SendRpcCommandRequest());
        }
        entityCommandBuffer.Playback(state.EntityManager);
        
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

public struct GoInGameRequestRpc : IRpcCommand
{
    
}
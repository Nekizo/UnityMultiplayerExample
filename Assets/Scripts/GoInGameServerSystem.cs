// #define DEBUG_LOG
#nullable enable
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct GoInGameServerSystem : ISystem
{
    #if !DEBUG_LOG
        [BurstCompile]
    #endif
    public void OnCreate(ref SystemState state)
    {
       state.RequireForUpdate<EntitesReferences>(); 
       state.RequireForUpdate<NetworkId>(); 
    }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // SystemAPI.Query<RefRO<NetworkId>>()
        int playerCount = state.GetEntityQuery(typeof(NetworkId)).CalculateEntityCount();

        // foreach((
        //         RefRO<NetworkId> networkId,
        //         Entity entity
        //     ) in SystemAPI.Query<RefRO<NetworkId>>()
        //         .WithEntityAccess()
        //     )
        // {
        //     playerCount = 1;
        // }
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        EntitesReferences entitesReferences = SystemAPI.GetSingleton<EntitesReferences>();
        foreach((
                RefRO<ReceiveRpcCommandRequest> receiveRpcCommandRequest,
                Entity entity
            ) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>()
                .WithAll<GoInGameRequestRpc>()
                .WithEntityAccess()
            )
        {
            if (playerCount < 4)
            {

                entityCommandBuffer.AddComponent<NetworkStreamInGame>(receiveRpcCommandRequest.ValueRO.SourceConnection);
                Entity playerEntity = entityCommandBuffer.Instantiate(entitesReferences.playerPrefabGameObject);
                
                entityCommandBuffer.SetComponent(playerEntity, LocalTransform.FromPosition(new float3(
                    UnityEngine.Random.Range(-1,1),
                    UnityEngine.Random.Range(-1,1),
                    0
                    )));
                NetworkId networkId = SystemAPI.GetComponent<NetworkId>(receiveRpcCommandRequest.ValueRO.SourceConnection);
                entityCommandBuffer.AddComponent(playerEntity,new GhostOwner{
                    NetworkId = networkId.Value
                });

                entityCommandBuffer.AddComponent(receiveRpcCommandRequest.ValueRO.SourceConnection,new PlayerData
                {
                    name ="User " + playerCount
                });
                
                // Debug.Log("bla :: "+receiveRpcCommandRequest.ValueRO.SourceConnection);
                entityCommandBuffer.AppendToBuffer(receiveRpcCommandRequest.ValueRO.SourceConnection,new LinkedEntityGroup
                {
                    Value=playerEntity, 

                });
                
            } 
            entityCommandBuffer.DestroyEntity(entity);
        }
        entityCommandBuffer.Playback(state.EntityManager);
            
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
public struct PlayerData: IComponentData{
    public FixedString128Bytes name;
}

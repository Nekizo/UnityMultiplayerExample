using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using Unity.Collections;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct ChatServerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state){
        // state.RequireForUpdate<ChatData>();
        Entity entity = state.EntityManager.CreateEntity();
        // state.EntityManager.AddComponent(entity,typeof(ChatData));
        // state.RequireForUpdate<SendMessageDate>();
        // state.RequireForUpdate<ReceiveRpcCommandRequest>();
    }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // ChatData chatData = SystemAPI.GetSingleton<ChatData>();
        EntityCommandBuffer entityCommandBuffer= new EntityCommandBuffer(Allocator.Temp);
        foreach((
            RefRO<SendMessageRpc> messageData,
            RefRO<ReceiveRpcCommandRequest> rececivrRpc,
            Entity entity
        )in SystemAPI.Query<
            RefRO<SendMessageRpc>, 
            RefRO<ReceiveRpcCommandRequest>
        >()
            .WithEntityAccess()
        ){
            // Debug.Log("Recive Rpc: "+messageData.ValueRO.message +"::"+rececivrRpc.ValueRO.SourceConnection);
            Entity rpcEntity = entityCommandBuffer.CreateEntity();
            // chatData.massege
            RefRO<PlayerData> playerData =SystemAPI.GetComponentRO<PlayerData>(rececivrRpc.ValueRO.SourceConnection);
            string massege="\n"+
                playerData.ValueRO.name+
                "::"+
                messageData.ValueRO.message ;
            entityCommandBuffer.AddComponent(rpcEntity, new ChatStatusRpc {
                // message = chatData.massege
                message = massege
            } );
            entityCommandBuffer.AddComponent(rpcEntity,new SendRpcCommandRequest());
            // state.EntityManager.AddComponentData(rpcEntity,new SendRpcCommandRequest());
            // Debug.Log(chatData.massege);


            entityCommandBuffer.DestroyEntity(entity);

        }
        // SystemAPI.SetSingleton(chatData);
        entityCommandBuffer.Playback(state.EntityManager);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

public struct ChatStatusRpc : IRpcCommand
{
    public FixedString512Bytes message;
}
// public struct ChatData : IComponentData
// {
//     public FixedString64Bytes massege;
// }
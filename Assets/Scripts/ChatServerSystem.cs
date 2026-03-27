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
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
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
            Entity rpcEntity = entityCommandBuffer.CreateEntity();
            RefRO<PlayerData> playerData =SystemAPI.GetComponentRO<PlayerData>(rececivrRpc.ValueRO.SourceConnection);
            string massege="\n"+
                playerData.ValueRO.name+
                "::"+
                messageData.ValueRO.message ;
            entityCommandBuffer.AddComponent(rpcEntity, new ChatStatusRpc {
                message = massege
            } );
            entityCommandBuffer.AddComponent(rpcEntity,new SendRpcCommandRequest());


            entityCommandBuffer.DestroyEntity(entity);

        }
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

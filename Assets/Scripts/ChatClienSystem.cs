using UnityEngine;
using Unity.Entities;
using Unity.NetCode;
using Unity.Collections;
using Unity.VisualScripting;
using System.Threading.Tasks;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial class ChatClienSystem : SystemBase 
{
    protected override void OnCreate()
    {
        RequireForUpdate<ChatStatusRpc>();
        RequireForUpdate<ChatManager>();
        RequireForUpdate<ChatManagerTag>();
        RequireForUpdate<ReceiveRpcCommandRequest>();
    }

    protected override void OnUpdate()
    {
        ChatManager chatManager = EntityManager.GetComponentObject<ChatManager>(SystemAPI.GetSingletonEntity<ChatManagerTag>());
        EntityCommandBuffer entityCommandBuffer= new EntityCommandBuffer(Allocator.Temp);
        foreach((
            RefRO<ChatStatusRpc> chatStatus,
            RefRO<ReceiveRpcCommandRequest> rececivrRpc,
            Entity entity
        )in SystemAPI.Query<
            RefRO<ChatStatusRpc>, 
            RefRO<ReceiveRpcCommandRequest>
        >()
            .WithEntityAccess()
        ){
            chatManager.AddMessage(chatStatus.ValueRO.message);


            entityCommandBuffer.DestroyEntity(entity);

        }
        entityCommandBuffer.Playback(EntityManager);
        

    }

    protected override void OnDestroy()
    {
        
    } 
}

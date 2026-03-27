using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Entities;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.NetCode;
public class ChatManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text textField;
    [SerializeField] Button button;
    void Awake(){
        var entityManager = ClientServerBootstrap.ClientWorld.EntityManager;
        Entity entity = entityManager.CreateEntity();
        entityManager.AddComponentObject(entity,this);
        entityManager.AddComponent(entity,typeof(ChatManagerTag));
        button.onClick.AddListener(SendMessage);
        InputSingleton.CreateIfNull();
        inputField.onSubmit.AddListener(SendMessage);
        inputField.onSelect.AddListener((a)=>{InputSingleton.inputActions.Player.Disable();});
        inputField.onEndEdit.AddListener((a)=>{InputSingleton.inputActions.Player.Enable();});

    }
    
    public void AddMessage(FixedString512Bytes message)
    {
        textField.text += message.ToString();
    }

    void SendMessage(){
        // World.DefaultGameObjectInjectionWorld.EntityManager;
        var entityManager = ClientServerBootstrap.ClientWorld.EntityManager;
        Entity rpcEntity = entityManager.CreateEntity();
        entityManager.AddComponentData(rpcEntity, new SendMessageRpc{
            message = inputField.text
        } );
        entityManager.AddComponentData(rpcEntity,new SendRpcCommandRequest());
        Debug.Log(inputField.text);

    }
    
}
// public class Baker : Baker<ChatManager>
// {
//     public override void Bake(ChatManager authoring)
//     {
//         Entity entity = GetEntity(TransformUsageFlags.Dynamic);
//         AddComponentObject(entity,authoring);
//         AddComponent(entity,new ChatManagerTag());
//     }
// }
public struct ChatManagerTag : IComponentData{}
public struct SendMessageRpc : IRpcCommand{
    public FixedString512Bytes  message;
}
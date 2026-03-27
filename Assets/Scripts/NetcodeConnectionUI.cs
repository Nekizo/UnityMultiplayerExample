using UnityEngine;
using UnityEngine.UI;
using Unity.NetCode;
using Unity.Entities;
using UnityEngine.SceneManagement;
using Unity.Networking.Transport;

public class NetcodeConnectionUI : MonoBehaviour
{
    [SerializeField]private Button startServerButton;
    [SerializeField]private Button joinGameButton;
    void Awake()
    {
        startServerButton.onClick.AddListener(StartServer);
        joinGameButton.onClick.AddListener(GameJoin);
    }
    private void StartServer(){
        World serverWorld = ClientServerBootstrap.CreateServerWorld("ServerWorld");
        World clientWorld = ClientServerBootstrap.CreateClientWorld("ClientWorld");
        foreach(World world in World.All)
        {
            if (world.Flags == WorldFlags.Game)
            {
                world.Dispose();
                break;
            }
        } 
        if (World.DefaultGameObjectInjectionWorld == null)
        {
            World.DefaultGameObjectInjectionWorld = serverWorld;
        }
        // SceneManager.LoadSceneAsync("EntitySubscene",LoadSceneMode.Additive);
        SceneManagerSingleton.instance.SwitchScene(GameScene.game);
        ushort port=7979;
        RefRW<NetworkStreamDriver> netwoworkStreamDriver = 
            serverWorld.EntityManager.CreateEntityQuery(typeof(NetworkStreamDriver)).GetSingletonRW<NetworkStreamDriver>();
        netwoworkStreamDriver.ValueRW.Listen(NetworkEndpoint.AnyIpv4.WithPort(port));
        
        NetworkEndpoint connectNetworkEndpoint = NetworkEndpoint.LoopbackIpv4.WithPort(port);
        netwoworkStreamDriver = 
            clientWorld.EntityManager.CreateEntityQuery(typeof(NetworkStreamDriver)).GetSingletonRW<NetworkStreamDriver>();
        netwoworkStreamDriver.ValueRW.Connect(clientWorld.EntityManager,connectNetworkEndpoint);




    }
    private void GameJoin()
    {
        World clientWorld = ClientServerBootstrap.CreateClientWorld("ClientWorld");
        foreach(World world in World.All)
        {
            if (world.Flags == WorldFlags.Game)
            {
                world.Dispose();
                break;
            }
        } 
        if (World.DefaultGameObjectInjectionWorld == null)
        {
            World.DefaultGameObjectInjectionWorld = clientWorld;
        }
        // SceneManager.LoadSceneAsync("EntitySubscene",LoadSceneMode.Additive);

        SceneManagerSingleton.instance.SwitchScene(GameScene.game);
        ushort port=7979;
        string ip = "127.0.0.1";
        
        NetworkEndpoint connectNetworkEndpoint = NetworkEndpoint.Parse(ip,port) ;
        RefRW<NetworkStreamDriver> netwoworkStreamDriver = 
            clientWorld.EntityManager.CreateEntityQuery(typeof(NetworkStreamDriver)).GetSingletonRW<NetworkStreamDriver>();
        netwoworkStreamDriver.ValueRW.Connect(clientWorld.EntityManager,connectNetworkEndpoint);
        



    }

}

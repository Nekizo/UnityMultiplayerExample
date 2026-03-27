using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;

[UpdateInGroup(typeof(GhostInputSystemGroup))]
// [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial class InputClientSystem : SystemBase
{
    // InputSystem_Actions inputActions;
    protected override void OnCreate()
    {
        RequireForUpdate<PlayerInput>();
        RequireForUpdate<NetworkStreamInGame>();
        // inputActions = new InputSystem_Actions();
        InputSingleton.CreateIfNull();
    }

    protected override void OnStartRunning()
    {
        InputSingleton.inputActions.Player.Enable();

    }
    protected override void OnStopRunning()
    {

        InputSingleton.inputActions.Player.Disable();
    }

    protected override void OnUpdate()
    {
        Vector2 currentMove = InputSingleton.inputActions.Player.Move.ReadValue<Vector2>(); 
        // SystemAPI.SetSingleton(new PlayerInput{
        //    move = currentMove
        // });
        // Debug.Log(currentMove);
        foreach (
            RefRW<PlayerInput> playerInput
                in SystemAPI.Query<RefRW<PlayerInput>>()
            .WithAll<GhostOwnerIsLocal>())
        {
            playerInput.ValueRW.move = currentMove;
        }

    }

    protected override void OnDestroy()
    {
        
    }
}
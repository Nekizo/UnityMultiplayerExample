using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.NetCode;

class PlayerInputBake : MonoBehaviour
{
    class PlayerInputBakeBaker : Baker<PlayerInputBake>
    {
        public override void Bake(PlayerInputBake authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerInput());
        }
    }   
}


public struct PlayerInput : IInputComponentData{
    public float2 move;
}

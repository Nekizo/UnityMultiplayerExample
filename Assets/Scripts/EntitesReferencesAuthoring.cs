using Unity.Entities;
using UnityEngine;

public class EntitesReferencesAuthoring : MonoBehaviour{
    public GameObject playerPrefabGameObject;
   public class Baker : Baker<EntitesReferencesAuthoring>{
        public override void Bake(EntitesReferencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitesReferences{
                playerPrefabGameObject = GetEntity(
                    authoring.playerPrefabGameObject,
                    TransformUsageFlags.Dynamic)
            });
        }
    } 
}

public struct EntitesReferences : IComponentData{
    public Entity playerPrefabGameObject;
}
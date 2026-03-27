using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerSingleton : MonoBehaviour
{
    public static SceneManagerSingleton instance;
    
    void Awake()
    {
        instance = this;
        SwitchScene(GameScene.hub);
    }
    
    public Scene[] GetOpenScenes()
    {
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }
        return loadedScenes;
    }
    
    private void UnLoadScene()
    {
        Scene[] scenes =GetOpenScenes();
        for (int i = 0; i < scenes.Length; i++)
        {
            Debug.Log(scenes[i].name);
            if (scenes[i].name != gameObject.scene.name)
            {
                SceneManager.UnloadSceneAsync(scenes[i]);
                Debug.Log("yes");
            }
            
        }
        
    }
    public void SwitchScene(GameScene scene)
    {
        UnLoadScene();
        switch (scene)
        {
            case GameScene.hub:
                SceneManager.LoadSceneAsync("Hab",LoadSceneMode.Additive);
                break;
            
            case GameScene.game:
                SceneManager.LoadSceneAsync("SampleScene",LoadSceneMode.Additive);
                break;
        }
    }
}
public enum GameScene
{
    hub=0,
    game=1
}

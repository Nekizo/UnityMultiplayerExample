using UnityEngine;

public class InputSingleton //: MonoBehaviour
{

    public static InputSystem_Actions inputActions;
    public static void CreateIfNull()
    {
        if (inputActions == null)
        {
            inputActions=new InputSystem_Actions();

        }
    }
    // void Awake()
    // {
    //     CreateIfNull();
    // }

}

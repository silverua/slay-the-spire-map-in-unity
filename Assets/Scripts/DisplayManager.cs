using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    void Start()
    {
        //Activate all available displays
            for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();

        }

        Camera secondaryCamera = Instantiate(Camera.main);
        secondaryCamera.targetDisplay = 1; // Second display
        secondaryCamera.transform.position = Camera.main.transform.position;
        secondaryCamera.transform.rotation = Camera.main.transform.rotation;
        secondaryCamera.fieldOfView = Camera.main.fieldOfView;
    }

}
using UnityEngine;

public class ScrollNonUI : MonoBehaviour
{
    public bool freezeX;
    public bool freezeY;
    private Vector2 offset;
    // distance from the center of this Game Object to the point where we clicked to start dragging 
    private Vector3 pointerDisplacement;
    private float zDisplacement;
    private bool dragging;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        zDisplacement = -mainCamera.transform.position.z + transform.position.z;
    }

    public void OnMouseDown()
    {
        pointerDisplacement = -transform.position + MouseInWorldCoords();
        dragging = true;
    }

    public void OnMouseUp()
    {
        dragging = false;
    }
    
    private void Update ()
    {
        if (!dragging) return;
        
        var mousePos = MouseInWorldCoords();
        //Debug.Log(mousePos);
        transform.position = new Vector3(
            freezeX ? transform.position.x : mousePos.x - pointerDisplacement.x,
            freezeY ? transform.position.y : mousePos.y - pointerDisplacement.y,
            transform.position.z);
    }
    
    // returns mouse position in World coordinates for our GameObject to follow. 
    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        //Debug.Log(screenMousePos);
        screenMousePos.z = zDisplacement;
        return mainCamera.ScreenToWorldPoint(screenMousePos);
    }
}

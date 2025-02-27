using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public float speed;
    public BoundingBox bBoxL, bBoxR;
    public GameObject mario;

    private float movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos = new Vector3(pos.x + Time.deltaTime * speed * movement, pos.y, pos.z);
        transform.position = pos;
        if (bBoxL.active)
        {
            movement = -1;
        }
        else if (bBoxR.active)
        {
            movement = 1;
        }
        else
        {
            movement = 0;
        }
    }
    
    //Camera follow box
    //once Mario reaches the sides of a defined box, start to move the camera in that direction
    //have two boxes, if mario enters either of them, move the camera in that direction
    public void OnMoveCamera(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<float>();
    }
}

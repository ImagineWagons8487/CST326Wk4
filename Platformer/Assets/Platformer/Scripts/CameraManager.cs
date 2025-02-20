using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public float speed;

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
    }

    public void OnMoveCamera(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<float>();
    }
}

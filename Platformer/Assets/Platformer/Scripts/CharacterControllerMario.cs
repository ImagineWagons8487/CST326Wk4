using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector4 = System.Numerics.Vector4;

public class CharacterController : MonoBehaviour
{
    public float acceleration = 3f;
    public float maxSpeed = 10f;
    public float jumpImpulse = 8f;
    public float jumpBoostForce = 8f;

    [Header("Debug Stuff")] public bool isGrounded;
    
    private Rigidbody rb;
    private float horizontalAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // rb.linearVelocity += Vector3.right * horizontalAmount; //adds {-1, 1} to Vector3.right and adds it to rb.linearVelocity
        //not time-based, adds that value every frame, causes problems between differing frame rates, instead use Time.deltaTime
        rb.linearVelocity += Vector3.right * (horizontalAmount * Time.deltaTime * acceleration);
        // float currentSpeed = rb.linearVelocity.magnitude;
        // currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        // rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;

        float horizontalSpeed = rb.linearVelocity.x;
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);

        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.x = horizontalSpeed;
        rb.linearVelocity = newVelocity;
        //clamp vertical velocity as well

        //Test if character on ground surface
        Collider c = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        float castDistance = c.bounds.extents.y + .01f; //the half height of the capsule

        Color color = (isGrounded) ? Color.green : Color.red;
        Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, color, 0f, false);
        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        }
        else if (Input.GetKey(KeyCode.Space) && !isGrounded)
        {
            if(rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.up * jumpBoostForce, ForceMode.Acceleration);
        }

        if (horizontalAmount == 0f)
        {
            Vector3 decayedVelocity = rb.linearVelocity;
            decayedVelocity.x *= 1f - Time.deltaTime * 4f;
            rb.linearVelocity = decayedVelocity;
        }
        else
        {
            float yawRotation = (horizontalAmount > 0f) ? 90f : -90f;
            Quaternion rotation = Quaternion.Euler(0f, yawRotation, 0f);
            transform.rotation = rotation;
        }
    }

    public void OnMovePlayer(InputAction.CallbackContext context)
    {
        horizontalAmount = context.ReadValue<float>();
    }
}

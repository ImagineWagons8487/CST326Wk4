using System;
using System.Numerics;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = System.Numerics.Vector4;

public class CharacterController : MonoBehaviour
{
    public float acceleration = 3f;
    public float maxSpeed = 10f;
    public float jumpImpulse = 8f;
    public float jumpBoostForce = 8f;
    public float apexBoostForce = 10f;
    public Animator animator;
    [Header("Debug Stuff")] public bool isGrounded;
    
    private Rigidbody rb;
    private float horizontalAmount;
    
    public float coyoteTimer;
    private RigidbodyConstraints originalConstraints;

    private bool apexLegends;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        coyoteTimer = 0;
        originalConstraints = rb.constraints;
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
        // rb.linearVelocity += Vector3.right * horizontalAmount; //adds {-1, 1} to Vector3.right and adds it to rb.linearVelocity
        //not time-based, adds that value every frame, causes problems between differing frame rates, instead use Time.deltaTime
        rb.linearVelocity += Vector3.right * (horizontalAmount * Time.deltaTime * acceleration);
        // float currentSpeed = rb.linearVelocity.magnitude;
        // currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        // rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;

        float horizontalSpeed = rb.linearVelocity.x;
        if (apexLegends)
        {
            horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed * 2, maxSpeed * 2);
        }
        else
        {
            horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);   
        }

        float verticalSpeed = rb.linearVelocity.y;
        verticalSpeed = Mathf.Clamp(verticalSpeed, -maxSpeed * 2, maxSpeed);
        
        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.x = horizontalSpeed;
        newVelocity.y = verticalSpeed;
        rb.linearVelocity = newVelocity;
        //clamp vertical velocity as well

        //Test if character on ground surface
        Collider c = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        // float castDistance = c.bounds.extents.y + .01f; //the half height of the capsule
        
        //since origin is at feet of mario, castDistance should be really small
        float castDistance = .1f;

        Color color = (isGrounded) ? Color.green : Color.red;
        Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, color, 0f, false);
        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);
        
        //Apex modifier: At apex of jump, speed up a little bit
        //if not grounded, holding jump, y velocity reaches 0 +- 1, AND absolute val of x velocity is greater than .5f is  speed up a little bit
        //move left and right dependent on rotation, left is 90 degrees, right is -90 degrees
        if (!isGrounded && Input.GetKey(KeyCode.Space) && Math.Abs(rb.linearVelocity.y)<1f && Math.Abs(rb.linearVelocity.x) > .5f)
        {
            rb.AddForce(transform.forward * (apexBoostForce * Time.deltaTime), ForceMode.VelocityChange);
            apexLegends = true;
        }
        else
        {
            apexLegends = false;
        }
        
        //Coyote Time:
        //If walking off surface, have a tiny tiny bit of buffer time/frames to still allow the player to jump
        if (!isGrounded)
            coyoteTimer += Time.deltaTime; //if not grounded, coyoteTime starts
        else
            coyoteTimer = 0; //if grounded, reset coyoteTime
        //if not grounded, in coyote time, AND not pressing space, have a little bit of frozen y pos
        if (!isGrounded && coyoteTimer < .1f && !Input.GetKey(KeyCode.Space))
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        else //otherwise, return to normal constraints
            rb.constraints = originalConstraints;
        //if space down AND (grounded OR (not grounded AND coyoteTimer not past coyote limit))
            //when pressing the space down
            //if you're on the ground, you'll jump. OR
            //you're not on the ground and coyote time is still active, then you'll jump
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || (!isGrounded && coyoteTimer < .1f)))
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        //if holding space while not grounded, get jump boost force over time
        else if (Input.GetKey(KeyCode.Space) && !isGrounded)
        {
            if(rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.up * (jumpBoostForce * Time.deltaTime), ForceMode.VelocityChange);
        }
        //if horizontal movement is 0, slow down with a specified value over time
        if (horizontalAmount == 0f)
        {
            Vector3 decayedVelocity = rb.linearVelocity;
            decayedVelocity.x *= 1f - Time.deltaTime * 4f;
            rb.linearVelocity = decayedVelocity;
        }
        //if horizontal movement is nonzero, turn left if negative horizontal amount, turn right if positive
        else
        {
            float yawRotation = (horizontalAmount > 0f) ? 90f : -90f;
            Quaternion rotation = Quaternion.Euler(0f, yawRotation, 0f);
            transform.rotation = rotation;
        }
    }

    //new input system things
    public void OnMovePlayer(InputAction.CallbackContext context)
    {
        horizontalAmount = context.ReadValue<float>();
    }
    //give parameters made in the animator a value from the script
    void UpdateAnimation()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("InAir", !isGrounded);
    }
}

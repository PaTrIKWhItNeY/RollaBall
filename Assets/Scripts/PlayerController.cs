/************************************************************
- COMPONENT OF: Player
- REQUIRED DEPENDENCIES: Rigidbody Component
- DESCRIPTION: It listens for WASD and Arrow key presses to set 
-              vertical and horizontal directions. It uses those
-              to push the player's Rigidbody in that direction
-              at a preset force. Manages a double jump system by
-              tracking grounded status and available jumps.
- AUTHOR: PWhitney
- VERSION 1.1: Replaced infinite jump mechanism with double jump behavior
*************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Gravity fields
[SerializeField] private float gravityMultiplier = 1.5f; // Custom multiplier to adjust the strength of downward gravity force
    // Movement fields
    private float horizontalMovement;
    private float verticalMovement;
    private float force;
    private float jumpForce;

    // Double Jump fields
    private int maxJumps = 2;          // Maximum allowed jumps before landing
    private int jumpsRemaining;        // Tracks current available jumps
    private bool isGrounded;           // Tracks if the player is touching the floor

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       force = 30.25f;
       jumpForce = 55.8f;

       // Initialize available jumps to max jumps
       jumpsRemaining = maxJumps;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       MovePlayer();
    }

    // Moves the player
    private void MovePlayer()
    {
        Vector3 direction = new Vector3(horizontalMovement, 0, verticalMovement);
        GetComponent<Rigidbody>().AddForce(direction * force); 
    }

    // Gets the user key input and uses it to assign movement directions
    private void SetMoveDirection(Vector2 input)
    {
        horizontalMovement = input.x;
        verticalMovement = input.y;
    }

    // Listens for WASD and arrow key input then calls SetMoveDirection
    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        SetMoveDirection(ctx.ReadValue<Vector2>());  
    }

    // Listens for Spacebar input then calls Jump
    public void OnJumpInput(InputAction.CallbackContext ctx)
    {
        // Only attempt to jump when the input action is performed (button pressed)
        if (ctx.performed)
        {
            JumpPlayer();  
        }
    }

    // Jumps the player if jumps remain
    private void JumpPlayer()
    {
        // Check if player has jump charges left
        if (jumpsRemaining > 0)
        {
            // Reset vertical velocity prior to second jump for consistent jump height
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // Apply upward jump force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Deduct one available jump charge
            jumpsRemaining--;
        }
    }

    // Resets jump count when colliding with ground surfaces
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Ground" tag
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpsRemaining = maxJumps; // Reset jump counter back to 2
        }
    }

    // Detects when the player leaves the ground surface
    private void OnCollisionExit(Collision collision)
    {
        // Check if leaving the "Ground" tagged object
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
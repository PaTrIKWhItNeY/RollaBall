
/*
GameObject: Main Camera
Dependencies: UnityEngine, UnityEngine.InputSystem

Description:
Handles third-person camera rotation around a target transform via keyboard input 
(Q and E keys) and smoothly follows the target's position and orientation.

VERSION 1.1:
- Refactored camera logic into dedicated, modular methods.
- Added a dedicated FollowPlayer method to calculate position offsets and apply smooth movement.
- Kept the Update/LateUpdate lifecycle clean by delegating tasks.
*/

using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    // The target Transform (typically the Player) that the camera follows.
    [SerializeField] private Transform target;

    [Header("Camera Positioning")]
    // Distance behind the target.
    [SerializeField] private float distance = 6f;
    // Height above the target base position.
    [SerializeField] private float height = 3f;
    // Speed factor or dampening time offset for following movement.
    [SerializeField] private float followSpeed = 10f;

    [Header("Keyboard Camera Rotation")]
    // Degrees per second the camera rotates when pressing Q/E.
    [SerializeField] private float rotateSpeed = 100f;

    // Horizontal rotation angle around the target in degrees.
    private float yaw;
    // Vertical angle in degrees looking down at the target.
    private float pitch = 20f;
    // Vector velocity cache reference required by Vector3.SmoothDamp.
    private Vector3 cameraVelocity;

    private void LateUpdate()
    {
        // Early exit if no target is assigned to avoid NullReferenceExceptions.
        if (target == null)
            return;

        // Process horizontal camera rotation input from keyboard.
        HandleRotationInput();

        // Smoothly position and orient the camera relative to the target.
        FollowPlayer();
    }

    /*
     Handles horizontal rotation input (Q and E keys) and updates the camera's yaw angle.
    */
    private void HandleRotationInput()
    {
        float rotateInput = 0f;

        // Rotate left when Q key is pressed.
        if (Keyboard.current.qKey.isPressed)
        {
            rotateInput = -1f;
        }

        // Rotate right when E key is pressed.
        if (Keyboard.current.eKey.isPressed)
        {
            rotateInput = 1f;
        }

        // Update horizontal rotation angle based on input speed and elapsed frame time.
        yaw += rotateInput * rotateSpeed * Time.deltaTime;

        // Clamp vertical pitch within a safe range to avoid flipping.
        pitch = Mathf.Clamp(pitch, -10f, 70f);
    }

    /*
     Calculates target camera position and rotation, then smoothly interpolates position and updates camera focus.
    */
    private void FollowPlayer()
    {
        // Convert Euler pitch and yaw angles into a Quaternion rotation matrix.
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Calculate offset vector positioned behind the pivot point based on camera distance.
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);

        // Determine final position based on target position, height offset, and rotation offset.
        Vector3 targetPosition = target.position + (Vector3.up * height) + offset;

        // Smoothly transition camera position to target position using SmoothDamp to avoid jarring movement.
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref cameraVelocity,
            0.05f
        );

        // Make the camera look directly at the target elevated slightly off the target's base pivot.
        transform.LookAt(target.position + (Vector3.up * 0.5f));
    }
}
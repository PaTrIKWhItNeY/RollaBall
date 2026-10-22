using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float mouseSensitivity = 0.1f;

    private float xRotation = 0f;

    public void Look(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();

        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        // Rotate player left/right
        playerTransform.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 20f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

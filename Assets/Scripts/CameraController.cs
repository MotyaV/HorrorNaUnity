using UnityEngine;
using UnityEngine.SceneManagement; // Add this line to access SceneManager

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Check if the current scene is the game over scene
        if (SceneManager.GetActiveScene().name == "GameOver") 
        {
            // Unlock the cursor
            Cursor.lockState = CursorLockMode.None;
            return; // Exit the Update method, no need for camera control
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
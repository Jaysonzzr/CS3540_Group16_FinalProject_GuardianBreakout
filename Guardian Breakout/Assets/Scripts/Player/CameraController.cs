using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;

    public GamePref gamePref;

    float sensitivity;
    public float smoothing = 2.0f;

    private Vector2 mouseLook;
    private Vector2 smoothV;

    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuManager.isGamePaused)
        {
            sensitivity = gamePref.sensitivity;

            // Get the mouse input and calculate the camera movement
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);
            mouseLook += smoothV;

            // Clamp the vertical camera movement to prevent flipping
            mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

            // Apply the camera rotation
            transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            player.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);

            // Move the camera to follow the player
            transform.position = player.transform.position + offset;
        }
    }
}

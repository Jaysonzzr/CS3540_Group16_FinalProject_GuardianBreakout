using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform playerBody;
    public float mouseSensitivity = 200;
    float pitch = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = transform.parent.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // yaw
        playerBody.Rotate(Vector3.up * moveX);

        pitch -= moveY;

        transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
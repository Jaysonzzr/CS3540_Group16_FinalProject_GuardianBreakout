using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoDisplay : MonoBehaviour
{
    public Canvas myCanvas;
    public Text infoText;
    public GameObject inventory;
    public PlayerController playerController;
    public CameraController cameraController;
    public float maxDistance = 5.0f; // Maximum distance at which object info will be displayed

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.CompareTag("Friendly") || hitObject.CompareTag("Unfriendly"))
            {
                float distance = Vector3.Distance(transform.position, hitObject.transform.position);
                if (distance <= maxDistance)
                {
                    // Display information about the inspected object
                    infoText.text = hitObject.name + " (E)";

                    if (hitObject.CompareTag("Friendly"))
                    {
                        infoText.color = new Color(122/255f, 221/255f, 122/255f);
                    }
                    else
                    {
                        infoText.color = new Color(222/255f, 77/255f, 77/255f);
                    }

                    GameObject obj = myCanvas.transform.Find("InventoryGroup").gameObject;
                    if (obj.activeSelf)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            inventory.SetActive(false);
                            infoText.enabled = true;
                            playerController.enabled = true;
                            cameraController.enabled = true;
                            Cursor.lockState = CursorLockMode.Locked;
                            Cursor.visible = false;
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            inventory.SetActive(true);
                            infoText.enabled = false;
                            playerController.enabled = false;
                            cameraController.enabled = false;
                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                        }
                    }
                }
                else
                {
                    // Clear the information text if the player is too far away from the inspectable object
                    infoText.text = "";
                }
            }
            else
            {
                // Clear the information text if the player isn't looking at an inspectable object
                infoText.text = "";
            }
        }
        else
        {
            // Clear the information text if the player isn't looking at anything
            infoText.text = "";
        }
    }
}


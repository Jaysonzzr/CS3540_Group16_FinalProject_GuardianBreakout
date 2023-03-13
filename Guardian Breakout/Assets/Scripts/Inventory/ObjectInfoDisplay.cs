using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoDisplay : MonoBehaviour
{
    public Canvas myCanvas;
    public Text infoText;
    public Slider processBar;
    public AudioClip openInventorySFX;
    public AudioClip closeInventorySFX;
    public PlayerController playerController;
    public CameraController cameraController;
    public float maxDistance = 5.0f; // Maximum distance at which object info will be displayed

    float currentProcess;
    bool opening = false;
    float coolDown = 0.1f;

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
                        GameObject obj = myCanvas.transform.Find(hitObject.name.ToString() + "Inventory").gameObject;
                        if (obj.activeSelf)
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                obj.SetActive(false);
                                infoText.enabled = true;
                                AudioSource.PlayClipAtPoint(closeInventorySFX, Camera.main.transform.position);
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
                                obj.SetActive(true);
                                infoText.enabled = false;
                                AudioSource.PlayClipAtPoint(openInventorySFX, Camera.main.transform.position);
                                playerController.enabled = false;
                                cameraController.enabled = false;
                                Cursor.lockState = CursorLockMode.None;
                                Cursor.visible = true;
                            }
                        }
                    }
                    else if (hitObject.CompareTag("Unfriendly"))
                    {
                        infoText.color = new Color(222/255f, 77/255f, 77/255f);
                        GameObject obj = myCanvas.transform.Find(hitObject.name.ToString() + "Inventory").gameObject;
                        if (obj.activeSelf)
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                obj.SetActive(false);
                                infoText.enabled = true;
                                AudioSource.PlayClipAtPoint(closeInventorySFX, Camera.main.transform.position);
                                playerController.enabled = true;
                                cameraController.enabled = true;
                                Cursor.lockState = CursorLockMode.Locked;
                                Cursor.visible = false;

                                currentProcess = 0.0f;
                                opening = false;
                            }
                        }
                        else
                        {
                            if (!opening && Input.GetKeyDown(KeyCode.E))
                            {
                                infoText.enabled = false;
                                playerController.enabled = false;
                                cameraController.enabled = false;

                                opening = true;
                                processBar.gameObject.SetActive(true);
                            }

                            if (coolDown <= 0)
                            {
                                if (Input.GetKeyDown(KeyCode.E))
                                {
                                    infoText.enabled = true;
                                    playerController.enabled = true;
                                    cameraController.enabled = true;

                                    opening = false;
                                    processBar.gameObject.SetActive(false);
                                    currentProcess = 0.0f;
                                    coolDown = 0.1f;
                                }
                            }
                            else if (opening && coolDown > 0)
                            {
                                coolDown -= Time.deltaTime;
                            }

                            if (opening && currentProcess < 100)
                            {
                                currentProcess += Time.deltaTime * 50;
                                processBar.value = currentProcess;
                            }
                            else if (currentProcess >= 100)
                            {
                                processBar.gameObject.SetActive(false);
                                obj.SetActive(true);
                                AudioSource.PlayClipAtPoint(openInventorySFX, Camera.main.transform.position);
                                Cursor.lockState = CursorLockMode.None;
                                Cursor.visible = true;
                            }
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

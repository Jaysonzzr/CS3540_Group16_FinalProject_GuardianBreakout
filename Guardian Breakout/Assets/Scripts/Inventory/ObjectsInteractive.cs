using UnityEngine;
using UnityEngine.UI;
using System;

public class ObjectsInteractive : MonoBehaviour
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
    bool exercising = false;
    public Text exerciseText;

    bool reading = false;
    public Text readText;
    public Text readKeyText;
    int randomIndex;

    NPCBehavior.NPCStates originState;

    bool locked = false;
    float coolDown = 0.1f;

    GameObject player;

    private void Start() 
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.CompareTag("Friendly") || hitObject.CompareTag("Unfriendly") || hitObject.CompareTag("Bed") ||
                hitObject.CompareTag("Shower") || hitObject.CompareTag("Exercise") || hitObject.CompareTag("Read") || hitObject.CompareTag("NPC"))
            {
                float distance = Vector3.Distance(transform.position, hitObject.transform.position);
                if (distance <= maxDistance)
                {
                    // Display information about the inspected object
                    infoText.text = hitObject.name + " (E)";

                    if (hitObject.CompareTag("Friendly"))
                    {
                        infoText.color = new Color(122/255f, 221/255f, 122/255f);
                        GameObject obj = myCanvas.transform.Find("Inventories/" + hitObject.name.ToString() + "Inventory").gameObject;
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

                                GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
                            }
                        }
                        else
                        {
                            if (Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
                            {
                                obj.SetActive(true);
                                infoText.enabled = false;
                                AudioSource.PlayClipAtPoint(openInventorySFX, Camera.main.transform.position);
                                playerController.enabled = false;
                                cameraController.enabled = false;
                                Cursor.lockState = CursorLockMode.None;
                                Cursor.visible = true;

                                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                            }
                        }
                    }
                    
                    if (hitObject.CompareTag("Unfriendly"))
                    {
                        infoText.color = new Color(222/255f, 77/255f, 77/255f);
                        GameObject obj = myCanvas.transform.Find("Inventories/" + hitObject.name.ToString() + "Inventory").gameObject;
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

                                GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
                            }
                        }
                        else
                        {
                            if (!opening && Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
                            {
                                infoText.enabled = false;
                                playerController.enabled = false;
                                cameraController.enabled = false;

                                opening = true;
                                processBar.gameObject.SetActive(true);

                                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
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

                    if (hitObject.CompareTag("Bed"))
                    {
                        infoText.color = new Color(255/255f, 255/255f, 255/255f);
                        infoText.enabled = true;

                        if (Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
                        {
                            player.GetComponent<CharacterController>().enabled = false;
                            playerController.enabled = false;
                            cameraController.enabled = false;
                            
                            Camera.main.transform.SetParent(hitObject.transform);
                            Camera.main.transform.position = new Vector3(
                                hitObject.transform.position.x, hitObject.transform.position.y + 1, hitObject.transform.position.z - 1.4f
                            );
                            Camera.main.transform.rotation = Quaternion.Euler(
                                new Vector3(hitObject.transform.rotation.eulerAngles.x - 20,
                                 hitObject.transform.rotation.eulerAngles.y, 
                                 hitObject.transform.root.eulerAngles.z
                                )
                            );
                            locked = true;
                            coolDown = 0.1f;

                            GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                        }
                    }

                    if (hitObject.CompareTag("Shower"))
                    {
                        infoText.color = new Color(255/255f, 255/255f, 255/255f);
                        infoText.enabled = true;
                    }

                    if (hitObject.CompareTag("Exercise"))
                    {
                        infoText.color = new Color(255/255f, 255/255f, 255/255f);
                        infoText.enabled = true;

                        if (Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
                        {
                            player.GetComponent<CharacterController>().enabled = false;
                            playerController.enabled = false;
                            cameraController.enabled = false;
                            
                            Camera.main.transform.SetParent(hitObject.transform);
                            Camera.main.transform.position = new Vector3(
                                hitObject.transform.position.x - 2.5f, hitObject.transform.position.y + 2, hitObject.transform.position.z - 0.16f
                            );
                            Camera.main.transform.rotation = Quaternion.Euler(
                                new Vector3(hitObject.transform.rotation.eulerAngles.x + 15,
                                 hitObject.transform.rotation.eulerAngles.y - 90, 
                                 hitObject.transform.root.eulerAngles.z
                                )
                            );
                            locked = true;
                            exercising = true;
                            coolDown = 0.1f;

                            GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                        }
                    }

                    if (hitObject.CompareTag("Read"))
                    {
                        infoText.color = new Color(255/255f, 255/255f, 255/255f);
                        infoText.enabled = true;

                        if (Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
                        {
                            player.GetComponent<CharacterController>().enabled = false;
                            playerController.enabled = false;
                            cameraController.enabled = false;
                            
                            Camera.main.transform.SetParent(hitObject.transform);
                            Camera.main.transform.position = new Vector3(
                                hitObject.transform.position.x - 2.5f, hitObject.transform.position.y + 2, hitObject.transform.position.z - 0.16f
                            );
                            Camera.main.transform.rotation = Quaternion.Euler(
                                new Vector3(hitObject.transform.rotation.eulerAngles.x + 15,
                                 hitObject.transform.rotation.eulerAngles.y - 90, 
                                 hitObject.transform.root.eulerAngles.z
                                )
                            );
                            locked = true;
                            reading = true;
                            coolDown = 0.1f;

                            randomIndex = UnityEngine.Random.Range(0, 4);

                            GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                        }
                    }

                    if (hitObject.CompareTag("NPC"))
                    {
                        infoText.color = new Color(255/255f, 255/255f, 255/255f);
                        GameObject obj = myCanvas.transform.Find("NPCs/" + hitObject.name.ToString()).gameObject;
                        if (obj.activeSelf)
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                obj.SetActive(false);
                                infoText.enabled = true;
                                playerController.enabled = true;
                                cameraController.enabled = true;
                                Cursor.lockState = CursorLockMode.Locked;
                                Cursor.visible = false;

                                GameObject.FindObjectOfType<PlayerStats>().lockUI = false;

                                hitObject.GetComponent<NPCBehavior>().currentState = originState;
                            }
                        }
                        else
                        {
                            if (Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
                            {
                                obj.SetActive(true);
                                infoText.enabled = false;
                                playerController.enabled = false;
                                cameraController.enabled = false;
                                Cursor.lockState = CursorLockMode.None;
                                Cursor.visible = true;

                                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;

                                originState = hitObject.GetComponent<NPCBehavior>().currentState;
                                hitObject.GetComponent<NPCBehavior>().currentState = NPCBehavior.NPCStates.Trade;
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

        if (locked)
        {
            if (Input.GetKeyDown(KeyCode.E) && coolDown <= 0)
            {
                player.GetComponent<CharacterController>().enabled = true;
                playerController.enabled = true;
                cameraController.enabled = true;

                Camera.main.transform.SetParent(player.transform);
                locked = false;

                GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
                currentProcess = 0.0f;

                if (exercising)
                {
                    exercising = false;
                    myCanvas.transform.Find("PlayerStats").gameObject.SetActive(false);
                    exerciseText.gameObject.SetActive(false);
                    processBar.gameObject.SetActive(false);
                }

                if (reading)
                {
                    reading = false;
                    myCanvas.transform.Find("PlayerStats").gameObject.SetActive(false);
                    readText.gameObject.SetActive(false);
                    readKeyText.gameObject.SetActive(false);
                    processBar.gameObject.SetActive(false);
                }
            }
            else if (coolDown > 0)
            {
                coolDown -= Time.deltaTime;
            }

            if (exercising)
            {
                infoText.enabled = false;
                exerciseText.gameObject.SetActive(true);
                processBar.gameObject.SetActive(true);
                processBar.value = currentProcess;

                GameObject.FindObjectOfType<PlayerStats>().lockStats = true;
                myCanvas.transform.Find("PlayerStats").gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    currentProcess += 300 * Time.deltaTime;
                }
                
                if (currentProcess > 0)
                {
                    currentProcess -= 5 * Time.deltaTime;
                }

                if (currentProcess >= 100)
                {
                    currentProcess = 0.0f;
                    GameObject.FindObjectOfType<PlayerStats>().currentStrength += 2;
                }
            }

            if (reading)
            {
                infoText.enabled = false;
                readText.gameObject.SetActive(true);
                readKeyText.gameObject.SetActive(true);
                processBar.gameObject.SetActive(true);
                processBar.value = currentProcess;

                GameObject.FindObjectOfType<PlayerStats>().lockStats = true;
                myCanvas.transform.Find("PlayerStats").gameObject.SetActive(true);

                string[] keys = {"Q", "B", "Y", "P"};
                
                string randomKey = keys[randomIndex];

                readKeyText.text = randomKey;

                if (Input.anyKeyDown)
                {
                    foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(keyCode))
                        {
                            if (keyCode.ToString() == randomKey)
                            {
                                currentProcess += 500 * Time.deltaTime;
                                randomIndex = UnityEngine.Random.Range(0, 4);
                            }
                        }
                    }
                }
                
                if (currentProcess > 0)
                {
                    currentProcess -= 5 * Time.deltaTime;
                }

                if (currentProcess >= 100)
                {
                    currentProcess = 0.0f;
                    GameObject.FindObjectOfType<PlayerStats>().currentIntellect += 2;
                }
            }
        }
    }
}

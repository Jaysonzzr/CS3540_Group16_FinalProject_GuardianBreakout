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
    private PlayerController playerController;
    private CameraController cameraController;
    private InventoryManager inventoryManager;
    private CraftManager craftManager;
    public float maxDistance = 5.0f; // Maximum distance at which object info will be displayed

    public Text itemInfo;

    float currentProcess;
    public bool opening = false;
    bool exercising = false;
    public Text exerciseText;

    bool reading = false;
    public Text readText;
    public Text readKeyText;
    int randomIndex;

    bool showering = false;

    bool eating = false;
    float eatIncreaseTime = 2;
    float currentEatingTime = 0;

    NPCBehavior.NPCStates originState;

    public bool locked = false;
    public float coolDown = 0.1f;

    GameObject player;

    public GameObject water;
    GameObject shower;

    GameObject tradingNpc;
    public GameObject[] inventories;

    public AudioClip tradeSFX1;
    public AudioClip tradeSFX2;

    public AudioClip bookSFX1;
    public AudioClip bookSFX2;

    public AudioClip workoutSFX1;
    public AudioClip workoutSFX2;

    public AudioSource washSFX;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        cameraController = Camera.main.GetComponent<CameraController>();
        inventoryManager = GameObject.Find("LevelManager").GetComponent<InventoryManager>();
        craftManager = GameObject.Find("LevelManager").GetComponent<CraftManager>();
        itemInfo = myCanvas.transform.Find("ItemInfo").GetComponent<Text>();
    }

    void Update()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if ((hitObject.CompareTag("Friendly") || hitObject.CompareTag("Unfriendly") || hitObject.CompareTag("BedF") ||
                hitObject.CompareTag("BedB") || hitObject.CompareTag("Exercise") || hitObject.CompareTag("Read") ||
                hitObject.CompareTag("NPC") || hitObject.CompareTag("Eating") || hitObject.CompareTag("ShowerF") ||
                hitObject.CompareTag("ShowerB")) && !craftManager.crafting)
            {
                float distance = Vector3.Distance(transform.position, hitObject.transform.position);
                if (distance <= maxDistance)
                {
                    if (hitObject.CompareTag("Friendly"))
                    {
                        infoText.text = hitObject.name + " (E)";
                        infoText.color = new Color(122 / 255f, 221 / 255f, 122 / 255f);
                        GameObject obj = myCanvas.transform.Find("Inventories/" + hitObject.name.ToString() + "Inventory").gameObject;
                        if (obj.activeSelf)
                        {
                            if (Input.GetKeyDown(KeyCode.E) || player.GetComponent<PlayerBehavior>().getHurt)
                            {
                                obj.SetActive(false);
                                infoText.enabled = true;
                                AudioSource.PlayClipAtPoint(closeInventorySFX, Camera.main.transform.position);
                                itemInfo.text = "";
                                playerController.enabled = true;
                                cameraController.enabled = true;
                                Cursor.lockState = CursorLockMode.Locked;
                                Cursor.visible = false;

                                GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
                                player.GetComponent<PlayerBehavior>().getHurt = false;
                            }
                        }
                        else
                        {
                            if ((Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI))
                            {
                                obj.SetActive(true);
                                infoText.enabled = false;
                                AudioSource.PlayClipAtPoint(openInventorySFX, Camera.main.transform.position);
                                playerController.enabled = false;
                                cameraController.enabled = false;
                                Cursor.lockState = CursorLockMode.None;
                                Cursor.visible = true;

                                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                                player.GetComponent<PlayerBehavior>().getHurt = false;
                            }
                        }
                    }

                    if (hitObject.CompareTag("Unfriendly"))
                    {
                        infoText.text = hitObject.name + " (E)";
                        infoText.color = new Color(222 / 255f, 77 / 255f, 77 / 255f);
                        GameObject obj = myCanvas.transform.Find("Inventories/" + hitObject.name.ToString() + "Inventory").gameObject;
                        if (obj.activeSelf)
                        {
                            if (Input.GetKeyDown(KeyCode.E) || player.GetComponent<PlayerBehavior>().getHurt)
                            {
                                obj.SetActive(false);
                                infoText.enabled = true;
                                AudioSource.PlayClipAtPoint(closeInventorySFX, Camera.main.transform.position);
                                itemInfo.text = "";
                                playerController.enabled = true;
                                cameraController.enabled = true;
                                Cursor.lockState = CursorLockMode.Locked;
                                Cursor.visible = false;

                                currentProcess = 0.0f;
                                opening = false;

                                GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
                                player.GetComponent<PlayerBehavior>().getHurt = false;
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
                                player.GetComponent<PlayerBehavior>().getHurt = false;
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

                                    GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
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

                    if (hitObject.CompareTag("BedF") || hitObject.CompareTag("BedB"))
                    {
                        infoText.text = hitObject.name + " (E)";
                        infoText.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                        infoText.enabled = true;

                        GameObject cell = hitObject.transform.parent.gameObject;

                        if (Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
                        {
                            // player.GetComponent<CharacterController>().enabled = false;
                            player.GetComponent<PlayerBehavior>().getHurt = false;
                            playerController.enabled = false;
                            cameraController.enabled = false;

                            if (hitObject.CompareTag("BedB"))
                            {
                                Camera.main.transform.position = new Vector3(
                                    cell.transform.position.x + 28.7f, cell.transform.position.y + 1, cell.transform.position.z - 23
                                );
                            }
                            else
                            {
                                Camera.main.transform.position = new Vector3(
                                    cell.transform.position.x - 28.7f, cell.transform.position.y + 1, cell.transform.position.z + 23
                                );
                            }

                            Camera.main.transform.rotation = Quaternion.Euler(
                                new Vector3(hitObject.transform.rotation.eulerAngles.x - 20,
                                 hitObject.transform.rotation.eulerAngles.y,
                                 hitObject.transform.rotation.eulerAngles.z
                                )
                            );

                            locked = true;
                            coolDown = 0.1f;

                            GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                        }
                    }

                    if (hitObject.CompareTag("ShowerF") || hitObject.CompareTag("ShowerB"))
                    {
                        infoText.text = hitObject.name + " (E)";
                        infoText.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                        infoText.enabled = true;

                        if (Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
                        {
                            // player.GetComponent<CharacterController>().enabled = false;
                            player.GetComponent<PlayerBehavior>().getHurt = false;
                            playerController.enabled = false;
                            cameraController.enabled = false;

                            washSFX.Play();

                            if (hitObject.CompareTag("ShowerF"))
                            {
                                Camera.main.transform.position = new Vector3(
                                    hitObject.transform.position.x, hitObject.transform.position.y + 0.6f, hitObject.transform.position.z + 1
                                );
                                shower = Instantiate(water, new Vector3(
                                    hitObject.transform.position.x, hitObject.transform.position.y + 2.2f, hitObject.transform.position.z + 0.4f
                                ), Quaternion.Euler(new Vector3(50, 0, -90)));
                            }
                            else
                            {
                                Camera.main.transform.position = new Vector3(
                                    hitObject.transform.position.x, hitObject.transform.position.y + 0.6f, hitObject.transform.position.z - 1
                                );
                                shower = Instantiate(water, new Vector3(
                                    hitObject.transform.position.x, hitObject.transform.position.y + 2.2f, hitObject.transform.position.z - 0.4f
                                ), Quaternion.Euler(new Vector3(50, 180, -90)));
                            }
                            Camera.main.transform.rotation = Quaternion.Euler(
                                new Vector3(hitObject.transform.rotation.eulerAngles.x,
                                 hitObject.transform.rotation.eulerAngles.y + 90,
                                 hitObject.transform.rotation.eulerAngles.z
                                )
                            );

                            locked = true;
                            showering = true;
                            coolDown = 0.1f;

                            GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                        }
                    }

                    if (hitObject.CompareTag("Exercise"))
                    {
                        infoText.text = hitObject.name + " (E)";
                        infoText.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                        infoText.enabled = true;

                        if (Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
                        {
                            // player.GetComponent<CharacterController>().enabled = false;
                            player.GetComponent<PlayerBehavior>().getHurt = false;
                            playerController.enabled = false;
                            cameraController.enabled = false;

                            AudioClip workoutSFX = UnityEngine.Random.Range(0, 2) == 0 ? workoutSFX1 : workoutSFX2;
                            AudioSource.PlayClipAtPoint(workoutSFX, hitObject.transform.position);

                            Camera.main.transform.position = new Vector3(
                                hitObject.transform.position.x - 0.75f, hitObject.transform.position.y + 0.6f, hitObject.transform.position.z - 0.048f
                            );
                            Camera.main.transform.rotation = Quaternion.Euler(
                                new Vector3(hitObject.transform.rotation.eulerAngles.x + 15,
                                 hitObject.transform.rotation.eulerAngles.y - 90,
                                 hitObject.transform.rotation.eulerAngles.z
                                )
                            );

                            locked = true;
                            exercising = true;
                            coolDown = 0.1f;

                            GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                            Camera.main.transform.Find("Barbell").gameObject.SetActive(true);
                        }
                    }

                    if (hitObject.CompareTag("Read"))
                    {
                        infoText.text = hitObject.name + " (E)";
                        infoText.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                        infoText.enabled = true;

                        if (Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
                        {
                            // player.GetComponent<CharacterController>().enabled = false;
                            player.GetComponent<PlayerBehavior>().getHurt = false;
                            playerController.enabled = false;
                            cameraController.enabled = false;

                            AudioClip bookSFX = UnityEngine.Random.Range(0, 2) == 0 ? bookSFX1 : bookSFX2;
                            AudioSource.PlayClipAtPoint(bookSFX, hitObject.transform.position);

                            Camera.main.transform.position = new Vector3(
                                hitObject.transform.position.x - 1.08f, hitObject.transform.position.y + 1.98f, hitObject.transform.position.z - 0.648f
                            );
                            Camera.main.transform.rotation = Quaternion.Euler(
                                new Vector3(hitObject.transform.rotation.eulerAngles.x + 35,
                                 hitObject.transform.rotation.eulerAngles.y + 90,
                                 hitObject.transform.rotation.eulerAngles.z
                                )
                            );

                            locked = true;
                            reading = true;
                            coolDown = 0.1f;

                            randomIndex = UnityEngine.Random.Range(0, 4);

                            GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                            Camera.main.transform.Find("Book").gameObject.SetActive(true);
                        }
                    }

                    if (hitObject.CompareTag("NPC"))
                    {
                        if (hitObject.GetComponent<NPCBehavior>().currentState != NPCBehavior.NPCStates.Hurt &&
                            hitObject.GetComponent<NPCBehavior>().currentState != NPCBehavior.NPCStates.Chase &&
                            hitObject.GetComponent<NPCBehavior>().currentState != NPCBehavior.NPCStates.Attack &&
                            hitObject.GetComponent<NPCBehavior>().currentState != NPCBehavior.NPCStates.Sit)
                        {
                            infoText.text = hitObject.name + " (E)";
                            infoText.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                            GameObject obj = myCanvas.transform.Find("NPCsInventories/" + hitObject.name.ToString()).gameObject;
                            if (obj.activeSelf)
                            {
                                if (Input.GetKeyDown(KeyCode.E) || player.GetComponent<PlayerBehavior>().getHurt)
                                {
                                    obj.SetActive(false);
                                    infoText.enabled = true;
                                    playerController.enabled = true;
                                    cameraController.enabled = true;
                                    Cursor.lockState = CursorLockMode.Locked;
                                    Cursor.visible = false;

                                    itemInfo.text = "";

                                    GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
                                    player.GetComponent<PlayerBehavior>().getHurt = false;

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

                                    tradingNpc = hitObject;

                                    if (hitObject.GetComponent<NPCBehavior>().currentState != NPCBehavior.NPCStates.Dead)
                                    {
                                        AudioClip tradeSFX = UnityEngine.Random.Range(0, 2) == 0 ? tradeSFX1 : tradeSFX2;
                                        AudioSource.PlayClipAtPoint(tradeSFX, hitObject.transform.position);
                                    }

                                    GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                                    player.GetComponent<PlayerBehavior>().getHurt = false;

                                    originState = hitObject.GetComponent<NPCBehavior>().currentState;

                                    if (hitObject.GetComponent<NPCBehavior>().currentState != NPCBehavior.NPCStates.Dead)
                                    {
                                        hitObject.GetComponent<NPCBehavior>().currentState = NPCBehavior.NPCStates.Trade;
                                        GameObject.Find(
                                            "NPCsInventories/" + hitObject.name + "/MyInventory/MainInventoryName"
                                        ).gameObject.GetComponent<Text>().text = "Trading Contents";

                                        GameObject.Find(
                                            "NPCsInventories/" + hitObject.name + "/MyInventory/MainMission"
                                        ).gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        GameObject.Find(
                                            "NPCsInventories/" + hitObject.name + "/MyInventory/MainInventoryName"
                                        ).gameObject.GetComponent<Text>().text = "Contents";

                                        GameObject.Find(
                                            "NPCsInventories/" + hitObject.name + "/MyInventory/MainMission"
                                        ).gameObject.SetActive(false);
                                    }
                                }
                            }
                        }
                        else
                        {
                            infoText.text = hitObject.name;
                            infoText.color = new Color(222 / 255f, 77 / 255f, 77 / 255f);
                        }
                    }

                    if (hitObject.CompareTag("Eating"))
                    {
                        infoText.enabled = true;

                        if (inventoryManager.holdStuff && inventoryManager.holding.name == "Meal(Clone)")
                        {
                            infoText.text = hitObject.name + " (E)";
                            infoText.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);

                            if (Input.GetKeyDown(KeyCode.E) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
                            {
                                // player.GetComponent<CharacterController>().enabled = false;
                                player.GetComponent<PlayerBehavior>().getHurt = false;
                                playerController.enabled = false;
                                cameraController.enabled = false;

                                Camera.main.transform.position = new Vector3(
                                    hitObject.transform.position.x - 1.08f, hitObject.transform.position.y + 1.98f, hitObject.transform.position.z - 0.648f
                                );
                                Camera.main.transform.rotation = Quaternion.Euler(
                                    new Vector3(hitObject.transform.rotation.eulerAngles.x + 35,
                                    hitObject.transform.rotation.eulerAngles.y + 90,
                                    hitObject.transform.rotation.eulerAngles.z
                                    )
                                );

                                locked = true;
                                eating = true;
                                coolDown = 0.1f;

                                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                            }
                        }
                        else
                        {
                            infoText.text = "Go get a plate at the bar first";
                            infoText.color = new Color(222 / 255f, 77 / 255f, 77 / 255f);
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

                foreach (GameObject inventory in inventories)
                {
                    if (inventory.activeSelf)
                    {
                        inventory.SetActive(false);
                        infoText.enabled = true;
                        playerController.enabled = true;
                        cameraController.enabled = true;
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                        currentProcess = 0.0f;
                        opening = false;

                        GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
                        player.GetComponent<PlayerBehavior>().getHurt = false;

                        tradingNpc.GetComponent<NPCBehavior>().currentState = originState;
                    }
                }
            }
        }
        else
        {
            // Clear the information text if the player isn't looking at anything
            infoText.text = "";
        }

        if (locked)
        {
            GameObject.Find("LevelManager").GetComponent<InventoryManager>().lockLootBar = true;

            if ((Input.GetKeyDown(KeyCode.E) && coolDown <= 0) || player.GetComponent<PlayerBehavior>().getHurt)
            {
                GameObject.Find("LevelManager").GetComponent<InventoryManager>().lockLootBar = false;
                GameObject.FindObjectOfType<PlayerStats>().lockStats = false;

                player.GetComponent<CharacterController>().enabled = true;
                playerController.enabled = true;
                cameraController.enabled = true;

                locked = false;

                player.GetComponent<PlayerBehavior>().getHurt = false;
                GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
                currentProcess = 0.0f;

                if (exercising)
                {
                    exercising = false;
                    myCanvas.transform.Find("PlayerStats").gameObject.SetActive(false);
                    Camera.main.transform.Find("Barbell").gameObject.SetActive(false);
                    exerciseText.gameObject.SetActive(false);
                    processBar.gameObject.SetActive(false);
                }

                if (reading)
                {
                    reading = false;
                    myCanvas.transform.Find("PlayerStats").gameObject.SetActive(false);
                    Camera.main.transform.Find("Book").gameObject.SetActive(false);
                    readText.gameObject.SetActive(false);
                    readKeyText.gameObject.SetActive(false);
                    processBar.gameObject.SetActive(false);
                }

                if (eating)
                {
                    eating = false;
                    myCanvas.transform.Find("PlayerStats").gameObject.SetActive(false);
                    Camera.main.transform.Find("Plate").gameObject.SetActive(true);
                    processBar.gameObject.SetActive(false);

                    if (inventoryManager.holdStuff)
                    {
                        Destroy(inventoryManager.holding);
                        inventoryManager.holdStuff = false;
                    }
                }

                if (showering)
                {
                    showering = false;
                    washSFX.Stop();
                    Destroy(shower, 5 * Time.deltaTime);
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

                Transform barbell = Camera.main.transform.Find("Barbell").transform;
                float barbellPos = -0.15f + (0.3f * (currentProcess / 100)) + Camera.main.transform.position.y;

                barbell.position = new Vector3(barbell.position.x, barbellPos, barbell.position.z);

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    currentProcess += 800 * Time.deltaTime;
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

                string[] keys = { "Q", "B", "Y", "P" };

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
                                currentProcess += 1000 * Time.deltaTime;
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

            if (eating)
            {
                infoText.enabled = false;
                processBar.gameObject.SetActive(true);
                processBar.value = currentProcess;

                GameObject.FindObjectOfType<PlayerStats>().lockStats = true;
                myCanvas.transform.Find("PlayerStats").gameObject.SetActive(true);

                Transform plate = Camera.main.transform.Find("Plate").transform;
                plate.position = new Vector3(plate.position.x, Camera.main.transform.position.y - 0.4f, plate.position.z);

                if (currentProcess >= 100)
                {
                    Destroy(inventoryManager.holding);
                    currentProcess = -1;
                }
                else if (currentProcess >= 0 && currentProcess < 100)
                {
                    currentProcess += 5 * Time.deltaTime;

                    currentEatingTime += Time.deltaTime;
                    if (currentEatingTime > eatIncreaseTime && GameObject.FindObjectOfType<PlayerStats>().currentHealth <= 96)
                    {
                        currentEatingTime = 0.0f;
                        GameObject.FindObjectOfType<PlayerStats>().currentHealth += 4;
                    }
                }
            }

            if (showering)
            {
                infoText.enabled = false;
            }
        }
    }
}
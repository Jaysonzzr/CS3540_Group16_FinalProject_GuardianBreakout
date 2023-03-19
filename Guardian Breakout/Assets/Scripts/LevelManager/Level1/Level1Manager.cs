using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1Manager : MonoBehaviour
{
    private GameObject player;
    private CharacterController characterController;
    private PlayerController playerController;
    private CameraController cameraController;

    private Text getUpText;
    private GameObject tutorialHints;
    private Image getUpCover;

    public InventorySlot[] toolBar;
    public GameObject[] doors;
    public AcceptMissions missions;
    public GameObject jess;
    public GameObject trade;
    public GameObject craftTable;

    bool gameStart = false;
    bool accept = false;
    bool combatStart = false;
    bool hasDead = false;
    bool finish = false;
    bool tradeing = false;
    bool couldCraft = false;
    bool crafting = false;
    bool ableCraft = false;
    bool looting = false;
    bool wallBreaking = false;

    int keyCount = 1;
    int missionCount = 1;
    int combatCount = 1;
    int deadCount = 1;
    int finishCount = 1;
    int craftCount = 1;
    int craftTableCount = 1;
    int ableCraftCount = 1;
    int lootCount = 1;
    int axeCount = 1;

    Color startColor;
    float startWaitingTime = 1.0f;
    float colorDuration = 3.0f;
    float currentColorDuration;

    public string welcomeText;
    public string missionText;
    public string combatText;
    public string deadText;
    public string finishText;
    public string craftText;
    public string craftTableText;
    public string ableCraftText;
    public string lootText;
    public string wallText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        characterController = player.GetComponent<CharacterController>();
        playerController = player.GetComponent<PlayerController>();
        cameraController = Camera.main.GetComponent<CameraController>();

        tutorialHints = transform.Find("TutorialHints").gameObject;
        getUpText = transform.Find("GetUpText").GetComponent<Text>();
        getUpCover = transform.Find("GetUpCover").GetComponent<Image>();
        GameInit();
        
        startColor = getUpCover.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (keyCount == 1)
        {
            WakeUp();
        }
        else if (!gameStart)
        {
            getUpCover.enabled = false;
            GetUp();
        }

        Missions();
        Combat();
        Respawn();
        Finish();
        Craft();
        CraftTable();
        AbleCraft();
        Loot();
        WallBreak();
    }

    void GameInit()
    {
        getUpCover.enabled = true;
        getUpText.enabled = true;
        characterController.enabled = false;
        playerController.enabled = false;
        cameraController.enabled = false;
        Camera.main.GetComponent<ObjectsInteractive>().locked = true;
        Camera.main.GetComponent<ObjectsInteractive>().coolDown = 0.1f;
        GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
        GameObject.Find("LevelManager").GetComponent<CraftManager>().enabled = false;

        Camera.main.transform.localPosition = new Vector3(2.111f, 0.397f, -2.288f);
        Camera.main.transform.rotation = Quaternion.Euler(-20, -90f, 0);

        foreach (GameObject door in doors)
        {
            door.tag = "Untagged";
        }
    }

    void WakeUp()
    {
        if (startWaitingTime <= 0)
        {
            currentColorDuration += Time.deltaTime;
            float t = currentColorDuration / colorDuration;
            t = Mathf.Clamp01(t);

            float alpha = Mathf.Lerp(startColor.a, 0.0f, t);

            Color c = getUpCover.color;
            c.a = alpha;
            getUpCover.color = c;

            if (Input.GetKeyDown(KeyCode.E))
            {
                keyCount--;
            }
        }
        else
        {
            startWaitingTime -= Time.deltaTime;
        }
    }

    void GetUp()
    {
        Time.timeScale = 0;
        getUpText.enabled = false;
        tutorialHints.SetActive(true);

        tutorialHints.transform.Find("Hints/TitleText").GetComponent<Text>().text = "Guardian Breakout";
        tutorialHints.transform.Find("Hints/MainText").GetComponent<Text>().text = welcomeText;

        cameraController.enabled = false;
        playerController.enabled = false;
        GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
        Camera.main.GetComponent<ObjectsInteractive>().enabled = false;
        Camera.main.transform.localPosition = new Vector3(0, 1.6f, 0);

        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 1;
            GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
            Camera.main.GetComponent<ObjectsInteractive>().enabled = true;
            cameraController.enabled = true;
            playerController.enabled = true;
            tutorialHints.SetActive(false);

            gameStart = true;
        }
    }

    void Missions()
    {
        if (missionCount == 1 && missions.missionAccetped)
        {
            accept = true;
            missionCount--;
        }

        if (accept)
        {
            tutorialHints.transform.Find("Hints/TitleText").GetComponent<Text>().text = "Quest List";
            tutorialHints.transform.Find("Hints/MainText").GetComponent<Text>().text = missionText;
            PauseGame(tutorialHints);

            if (Input.GetKeyDown(KeyCode.F))
            {
                cameraController.enabled = false;
                playerController.enabled = false;
                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;

                foreach (GameObject door in doors)
                {
                    door.tag = "CellDoorB";
                }

                accept = false;
            }
        }
    }

    void Combat()
    {
        int selectedSlotIdx = GameObject.FindObjectOfType<InventoryManager>().selectedSlot;
        foreach (InventorySlot slot in toolBar) 
        {
            if (slot.transform.childCount > 0 && slot.transform.GetChild(0).name == "Sock" && combatCount == 1)
            {
                combatStart = true;
                combatCount--;
            }
        }

        if (combatStart)
        {
            jess.GetComponent<CharacterController>().enabled = false;
            jess.transform.position = new Vector3(-12f, -0.05000004f, 29f);
            Debug.Log(jess.transform.position.x);
            jess.GetComponent<NPCBehavior>().currentState = NPCBehavior.NPCStates.Chase;

            tutorialHints.transform.Find("Hints/TitleText").GetComponent<Text>().text = "Combat";
            tutorialHints.transform.Find("Hints/MainText").GetComponent<Text>().text = combatText;
            PauseGame(tutorialHints);

            if (Input.GetKeyDown(KeyCode.F))
            {
                cameraController.enabled = false;
                playerController.enabled = false;
                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                jess.GetComponent<CharacterController>().enabled = true;
                combatStart = false;
            }
        }
    }

    void Respawn()
    {
        if (GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentHealth <= 0 && deadCount == 1)
        {
            hasDead = true;
            deadCount--;
        }

        if (hasDead)
        {
            tutorialHints.transform.Find("Hints/TitleText").GetComponent<Text>().text = "Passed Out";
            tutorialHints.transform.Find("Hints/MainText").GetComponent<Text>().text = deadText;
            PauseGame(tutorialHints);

            if (Input.GetKeyDown(KeyCode.F))
            {
                jess.GetComponent<CharacterController>().enabled = false;
                jess.transform.position = new Vector3(-8.92f, -0.05000004f, 27f);
                jess.GetComponent<NPCBehavior>().currentState = NPCBehavior.NPCStates.Idle;
                cameraController.enabled = false;
                playerController.enabled = false;
                hasDead = false;
                jess.GetComponent<CharacterController>().enabled = false;
            }
        }
    }

    void Finish()
    {
        foreach (InventorySlot slot in toolBar)
        {
            if (slot.transform.childCount > 0 && slot.transform.GetChild(0).name == "Sock")
            {
                finish = true;
            }
        }

        if (finish && trade.activeSelf && finishCount == 1)
        {
            tradeing = true;
            finishCount--;
        }

        if (tradeing)
        {
            tutorialHints.transform.Find("Hints/TitleText").GetComponent<Text>().text = "Trading and Submitting request";
            tutorialHints.transform.Find("Hints/MainText").GetComponent<Text>().text = finishText;
            PauseGame(tutorialHints);

            if (Input.GetKeyDown(KeyCode.F))
            {
                cameraController.enabled = false;
                playerController.enabled = false;
                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                tradeing = false;
            }
        }
    }

    void Craft()
    {
        foreach (InventorySlot slot in toolBar)
        {
            if (slot.transform.childCount > 0 && slot.transform.GetChild(0).name == "Tape" && craftCount == 1)
            {
                couldCraft = true;
                craftCount--;
            }
        }

        if (couldCraft)
        {
            tutorialHints.transform.Find("Hints/TitleText").GetComponent<Text>().text = "Craft system";
            tutorialHints.transform.Find("Hints/MainText").GetComponent<Text>().text = craftText;
            PauseGame(tutorialHints);

            if (Input.GetKeyDown(KeyCode.F))
            {
                cameraController.enabled = false;
                playerController.enabled = false;
                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                GameObject.Find("LevelManager").GetComponent<CraftManager>().enabled = true;
                couldCraft = false;
            }
        }
    }

    void CraftTable()
    {
        if (craftTable.activeSelf && craftTableCount == 1)
        {
            crafting = true;
            craftTableCount--;
        }

        if (crafting)
        {
            tutorialHints.transform.Find("Hints/TitleText").GetComponent<Text>().text = "Craft system";
            tutorialHints.transform.Find("Hints/MainText").GetComponent<Text>().text = craftTableText;
            GameObject.Find("LevelManager").GetComponent<CraftManager>().enabled = false;
            PauseGame(tutorialHints);

            if (Input.GetKeyDown(KeyCode.F))
            {
                cameraController.enabled = false;
                playerController.enabled = false;
                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                GameObject.Find("LevelManager").GetComponent<CraftManager>().enabled = true;
                crafting = false;
            }
        }
    }

    void AbleCraft()
    {
        if (GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentIntellect == 40 && ableCraftCount == 1)
        {
            ableCraft = true;
            ableCraftCount--;
        }

        if (ableCraft)
        {
            tutorialHints.transform.Find("Hints/TitleText").GetComponent<Text>().text = "Craft system";
            tutorialHints.transform.Find("Hints/MainText").GetComponent<Text>().text = ableCraftText;
            PauseGame(tutorialHints);

            if (Input.GetKeyDown(KeyCode.F))
            {
                cameraController.enabled = false;
                playerController.enabled = false;
                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                ableCraft = false;
            }
        }
    }

    void Loot()
    {
        if (jess.GetComponent<NPCBehavior>().dying && lootCount == 1)
        {
            looting = true;
            lootCount--;
        }

        if (looting)
        {
            tutorialHints.transform.Find("Hints/TitleText").GetComponent<Text>().text = "Looting";
            tutorialHints.transform.Find("Hints/MainText").GetComponent<Text>().text = lootText;
            PauseGame(tutorialHints);

            if (Input.GetKeyDown(KeyCode.F))
            {
                looting = false;
            }
        }
    }

    void WallBreak()
    {
        foreach (InventorySlot slot in toolBar)
        {
            if (slot.transform.childCount > 0 && slot.transform.GetChild(0).name == "Pickaxe" && axeCount == 1)
            {
                wallBreaking = true;
                axeCount--;
            }
        }

        if (wallBreaking)
        {
            tutorialHints.transform.Find("Hints/TitleText").GetComponent<Text>().text = "Wall Breaking";
            tutorialHints.transform.Find("Hints/MainText").GetComponent<Text>().text = wallText;
            PauseGame(tutorialHints);

            if (Input.GetKeyDown(KeyCode.F))
            {
                cameraController.enabled = false;
                playerController.enabled = false;
                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
                wallBreaking = false;
            }
        }
    }

    public void PauseGame(GameObject pauseObject)
    {
        Time.timeScale = 0;
        pauseObject.SetActive(true);

        cameraController.enabled = false;
        playerController.enabled = false;
        GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
        Camera.main.GetComponent<ObjectsInteractive>().enabled = false;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 1;
            GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
            Camera.main.GetComponent<ObjectsInteractive>().enabled = true;
            cameraController.enabled = true;
            playerController.enabled = true;
            pauseObject.SetActive(false);
        }
    }
}

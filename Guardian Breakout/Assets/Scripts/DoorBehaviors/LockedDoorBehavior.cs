using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorBehavior : MonoBehaviour
{
    Transform player;
    GameObject[] guards;

    InventoryManager inventoryManager;

    Transform leftDoor;
    Transform rightDoor;

    Vector3 leftDoorStartPos;
    Vector3 rightDoorStartPos;

    public string keyName;

    public bool allowNPC = false;
    public GameObject[] allowedNPC;

    int nearbyCharacters;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        guards = GameObject.FindGameObjectsWithTag("Guard");

        inventoryManager = GameObject.Find("LevelManager").GetComponent<InventoryManager>();

        leftDoor = transform.Find("Left_Door_Final").transform;
        rightDoor = transform.Find("Right_Door_Final").transform;

        leftDoorStartPos = leftDoor.localPosition;
        rightDoorStartPos = rightDoor.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        nearbyCharacters = 0;

        if (inventoryManager.Has(keyName) || inventoryManager.Has(keyName + "(Clone)"))
        {
            CheckProximity(player);
        }

        foreach (GameObject guard in guards)
        {
            CheckProximity(guard.transform);
        }

        if (allowNPC)
        {
            foreach (GameObject npc in allowedNPC)
            {
                CheckProximity(npc.transform);
            }
        }
   
        OpenOrCloseDoor();
    }

    void CheckProximity(Transform character)
    {
        float distance = Vector3.Distance(character.position, transform.position);
        if (distance < 2)
        {
            nearbyCharacters++;
        }
    }

    void OpenOrCloseDoor()
    {
        if (nearbyCharacters > 0)
        {
            Vector3 leftDoorTargetPos = new Vector3(leftDoorStartPos.x + 1.3f, leftDoorStartPos.y, leftDoorStartPos.z);
            Vector3 rightDoorTargetPos = new Vector3(rightDoorStartPos.x - 1.3f, rightDoorStartPos.y, rightDoorStartPos.z);

            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftDoorTargetPos, Time.deltaTime * 5);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightDoorTargetPos, Time.deltaTime * 5);
        }
        else
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftDoorStartPos, Time.deltaTime * 5);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightDoorStartPos, Time.deltaTime * 5);
        }
    }
}

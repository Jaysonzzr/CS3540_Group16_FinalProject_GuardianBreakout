using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public Canvas myCanvas;
    public InventorySlot[] toolBar;

    public AudioClip openUISFX;
    public AudioClip closeUISFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject obj = myCanvas.transform.Find("Missions").gameObject;
        if (obj.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                obj.SetActive(false);
                AudioSource.PlayClipAtPoint(closeUISFX, Camera.main.transform.position);

                GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.L) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
            {
                obj.SetActive(true);
                AudioSource.PlayClipAtPoint(openUISFX, Camera.main.transform.position);

                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
            }
        }

        Transform missions = myCanvas.transform.Find("Missions/Missions").gameObject.transform;
        for (int i = 0; i < missions.childCount; i++)
        {
            if (missions.GetChild(i).gameObject.activeSelf)
            {
                GameObject npc = myCanvas.transform.Find("NPCsInventories/" + missions.GetChild(i).name).gameObject;

                foreach (InventorySlot slot in toolBar)
                {
                    if (slot.transform.childCount > 0 &&
                        slot.transform.GetChild(0).name == npc.transform.Find("MyInventory/MainMission/Requires").GetComponent<MissionRequires>().require)
                    {
                        missions.GetChild(i).transform.Find("Finish").gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}

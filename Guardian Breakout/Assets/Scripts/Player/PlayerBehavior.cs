using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public InventorySlot[] toolBar;

    float increaseTime = 1;
    float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int selectedSlotIdx = GameObject.FindObjectOfType<InventoryManager>().selectedSlot;
        if (toolBar[selectedSlotIdx].transform.childCount > 0 && 
            toolBar[selectedSlotIdx].transform.GetChild(0).name == "Meal(Clone)")
        {
            Camera.main.transform.Find("Plate").gameObject.SetActive(true);
        }
        else
        {
            Camera.main.transform.Find("Plate").gameObject.SetActive(false);
        }

        if (toolBar[selectedSlotIdx].transform.childCount > 0 &&
            toolBar[selectedSlotIdx].transform.GetChild(0).name == "Pickaxe")
        {
            Camera.main.transform.Find("Pickaxe").gameObject.SetActive(true);
        }
        else
        {
            Camera.main.transform.Find("Pickaxe").gameObject.SetActive(false);
        }

        if (toolBar[selectedSlotIdx].transform.childCount > 0 &&
            toolBar[selectedSlotIdx].transform.GetChild(0).name == "Crowbar")
        {
            Camera.main.transform.Find("Crowbar").gameObject.SetActive(true);
        }
        else
        {
            Camera.main.transform.Find("Crowbar").gameObject.SetActive(false);
        }

        if (GameObject.Find("LevelManager").GetComponent<PlayerStats>().isDead)
        {
            transform.position = new Vector3(5.4873f, 0.59f, -26.9027f);
            transform.rotation = Quaternion.Euler(0, 0, 0); 
            transform.GetComponent<PlayerController>().enabled = false;
            
            Camera.main.GetComponent<CameraController>().enabled = false;
            Camera.main.transform.position = new Vector3(4.453f + transform.position.x, 0.775f + transform.position.y, 0.165f + transform.position.z);
            Camera.main.transform.rotation = Quaternion.Euler(-34, -90, 0);
            
            if (GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentHealth <= 95)
            {
                if (currentTime >= increaseTime)
                {
                    GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentHealth += 5;
                    currentTime = 0.0f;
                }
                else
                {
                    currentTime += Time.deltaTime;
                }
            }
            else
            {
                GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentHealth = 100;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject.Find("LevelManager").GetComponent<PlayerStats>().isDead = false;
                transform.GetComponent<PlayerController>().enabled = true;
                Camera.main.GetComponent<CameraController>().enabled = true;
            }
        }
    }
}

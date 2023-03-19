using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallBreaker : MonoBehaviour
{
    public float waitTime = 5;
    private float originalWaitTime;
    public float maxDistance = 5;

    public InventorySlot[] toolBar;
    public Slider processBar;

    float currentProcess;

    void Start()
    {
        originalWaitTime = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        processBar.value = currentProcess;
        int selectedSlotIdx = GameObject.FindObjectOfType<InventoryManager>().selectedSlot;
        if(Physics.Raycast(transform.position,transform.forward, out RaycastHit hit, maxDistance))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.CompareTag("Breakable"))
            {
                if (toolBar[selectedSlotIdx].transform.childCount > 0 && 
                    toolBar[selectedSlotIdx].transform.GetChild(0).name == "Pickaxe")
                {
                    if(Input.GetMouseButton(0))
                    {
                        waitTime -= Time.deltaTime;
                        hitObject.GetComponent<Outline>().enabled = true;
                        currentProcess = (1 - waitTime / originalWaitTime) * 100;
                        processBar.gameObject.SetActive(true);
                    }
                    
                    if(Input.GetMouseButtonUp(0))
                    {
                        waitTime = originalWaitTime;
                        currentProcess = 0.0f;
                        hitObject.GetComponent<Outline>().enabled = false;
                        processBar.gameObject.SetActive(false);
                    }
                    
                    if(waitTime <= 0)
                    {
                        Destroy(hit.transform.gameObject);
                        currentProcess = 0.0f;
                        processBar.gameObject.SetActive(false);
                        waitTime = originalWaitTime;
                    }
                }
            }
        }
        else
        {
            processBar.gameObject.SetActive(false);
        }
    }
}

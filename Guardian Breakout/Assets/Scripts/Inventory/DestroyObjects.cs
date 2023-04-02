using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    InventoryManager inventoryManager;

    public InventorySlot[] toolBar;
    public string objectName;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("LevelManager").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other) 
    {
        for (int i = 0; i < toolBar.Length; i++)
        {
            if (toolBar[i].transform.childCount > 0)
            {
                if (toolBar[i].transform.GetChild(0).name == objectName)
                {
                    inventoryManager.holdStuff = false;
                    Destroy(toolBar[i].transform.GetChild(0).gameObject);
                }
            }
        }
    }
}

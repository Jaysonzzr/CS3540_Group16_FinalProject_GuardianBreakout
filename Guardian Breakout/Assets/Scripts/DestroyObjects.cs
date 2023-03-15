using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    public InventorySlot[] toolBar;
    public string objectName;

    // Start is called before the first frame update
    void Start()
    {
        
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
                    Destroy(toolBar[i].transform.GetChild(0).gameObject);
                }
            }
        }
    }
}

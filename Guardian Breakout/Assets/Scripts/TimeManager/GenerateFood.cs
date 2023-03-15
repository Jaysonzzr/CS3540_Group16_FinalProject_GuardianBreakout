using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFood : MonoBehaviour
{
    public InventorySlot[] foodSlots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<TimeManager>().currentState == TimeManager.State.Breakfast ||
            FindObjectOfType<TimeManager>().currentState == TimeManager.State.Lunch ||
            FindObjectOfType<TimeManager>().currentState == TimeManager.State.Dinner)
        {
            foreach (InventorySlot slot in foodSlots)
            {
                if (slot.transform.childCount == 0)
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Items/Meal");

                    GameObject newPrefabInstance = Instantiate(prefab, slot.transform.position, slot.transform.rotation);
                    newPrefabInstance.transform.SetParent(slot.transform);
                }
                else if (slot.transform.childCount > 1)
                {
                    for (int i = 0; i < slot.transform.childCount; i++)
                    {
                        Destroy(slot.transform.GetChild(1).gameObject);
                    }
                }
            }
        }
        else
        {
            foreach (InventorySlot slot in foodSlots)
            {
                if (slot.transform.childCount > 0)
                {
                    Destroy(slot.transform.GetChild(0).gameObject);
                }
            }
        }
    }
}

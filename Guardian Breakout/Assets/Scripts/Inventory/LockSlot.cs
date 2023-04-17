using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSlot : MonoBehaviour
{
    public InventorySlot[] tradeSlots;

    // Start is called before the first frame update
    void Start()
    {
        foreach (InventorySlot slot in tradeSlots)
        {
            if (slot.transform.childCount > 0)
            {
                slot.transform.GetChild(0).GetComponent<DraggableItem>().enabled = false;
            }
            else
            {
                slot.GetComponent<InventorySlot>().enabled = false;
            }
        }
    }
}

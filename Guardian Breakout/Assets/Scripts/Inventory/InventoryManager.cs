using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public int selectedSlot = -1;

    public bool lockLootBar = false;
    public bool holdStuff = false;
    public GameObject holding;

    private void Start() 
    {
        ChangeSelectedSlot(0);
    }

    private void Update() 
    {
        if (Input.inputString != null && !lockLootBar)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 7)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        if (inventorySlots[selectedSlot].transform.childCount > 0)
        {
            holdStuff = true;
            holding = inventorySlots[selectedSlot].transform.GetChild(0).gameObject;
        }
        else
        {
            holdStuff = false;
            holding = null;
        }
    }

    public bool Has(string item)
    {
        bool result = false;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.transform.childCount > 0 && slot.transform.GetChild(0).name == item)
            {
                result = true;
            }
        }
        return result;
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }
}

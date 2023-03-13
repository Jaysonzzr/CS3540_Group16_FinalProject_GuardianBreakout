using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Crafting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject craftTable;
    public AudioClip popUpSFX;
    public Color overColor;
    public Color originColor;
    public Color unableCraftColor;
    public Image image;
    public Slider craftProcess;

    bool couldCraft;
    float currentProcess;

    public InventorySlot[] inventorySlots;
    public InventorySlot[] targetSlots;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        craftTable.SetActive(true);
        AudioSource.PlayClipAtPoint(popUpSFX, Camera.main.transform.position);
        image.color = overColor;
        couldCraft = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        craftTable.SetActive(false);
        image.color = originColor;
        currentProcess = 0.0f;
        couldCraft = false;
    }

    private void Update() 
    {
        craftProcess.value = currentProcess;
        if (couldCraft && CouldCraft())
        {
            if (Input.GetMouseButton(0))
            {
                currentProcess += Time.deltaTime * 50;
            }

            if (Input.GetMouseButtonUp(0))
            {
                currentProcess = 0.0f;
            }
        }
        
        if (!CouldCraft())
        {
            transform.GetChild(0).transform.GetComponent<Image>().color = unableCraftColor;
        }
        else
        {
            transform.GetChild(0).transform.GetComponent<Image>().color = originColor;
        }

        if (currentProcess >= 100)
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                DeleteItems();
                if (slot.transform.childCount == 0)
                {
                    string prefabName = craftTable.name;
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Items/" + prefabName);

                    GameObject newPrefabInstance = Instantiate(prefab, slot.transform.position, slot.transform.rotation);
                    newPrefabInstance.transform.SetParent(slot.transform);
                    currentProcess = 0.0f;
                    break;
                }
            }
        }
    }

    bool CouldCraft()
    {
        int intellectRequired;
        
        if (transform.parent.name == "AdvancedItems")
        {
            intellectRequired = 50;
        }
        else if (transform.parent.name == "HardItems")
        {
            intellectRequired = 40;
        }
        else
        {
            intellectRequired = 30;
        }

        if (FindObjectOfType<PlayerStats>().currentIntellect < intellectRequired)
        {
            return false;
        }

        List<string> inventory = new List<string>();
        List<string> target = new List<string>();

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.transform.childCount > 0)
            {
                inventory.Add(slot.transform.GetChild(0).name);
            }
        }

        foreach (InventorySlot slot in targetSlots)
        {
            target.Add(slot.transform.GetChild(0).name);
        }

        var set1 = new HashSet<string>(target);
        var set2 = new HashSet<string>(inventory);

        // Check if all elements in set1 appear in set2
        if (set1.IsSubsetOf(set2)) {
            // Check if set1 contains no duplicates
            if (set1.Count == target.Count) {
                return true;
            }
        }
        return false;
    }

    void DeleteItems()
    {
        for (int i = 0; i < targetSlots.Length; i++)
        {
            for (int n = 0; n < inventorySlots.Length; n++)
            {
                if (inventorySlots[n].transform.childCount > 0 && 
                    inventorySlots[n].transform.GetChild(0).name == targetSlots[i].transform.GetChild(0).name)
                {   
                    Destroy(inventorySlots[n].transform.GetChild(0).gameObject);
                    break;
                }
            }
        }
    }
}

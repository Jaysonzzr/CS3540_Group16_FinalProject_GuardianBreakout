using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text itemInfo;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        itemInfo = GameObject.Find("Canvas/ItemInfo").GetComponent<Text>();

        offset = new Vector3(0, 30, 0);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {        
        itemInfo.transform.position = 
            new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z + offset.z);
        
        // Remove "(Clone)" from the object name
        string objectName = transform.name.Replace("(Clone)", "").Trim();
        itemInfo.text = objectName;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        itemInfo.text = "";
    }
}

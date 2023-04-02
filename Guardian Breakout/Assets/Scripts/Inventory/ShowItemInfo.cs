using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text itemInfo;

    // Start is called before the first frame update
    void Start()
    {
        itemInfo = GameObject.Find("Canvas/ItemInfo").GetComponent<Text>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {        
        // Remove "(Clone)" from the object name
        string objectName = transform.name.Replace("(Clone)", "").Trim();
        itemInfo.text = objectName;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        itemInfo.text = "";
    }
}

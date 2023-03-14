using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnMouseOverTradeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color affordableColor;
    public Color unaffordableColor;
    public Color originColor;

    Image image;
    GameObject infoImage;

    int cost;

    bool entering = false;
    bool affordable = false;
    bool unlock = false;
    int costTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        infoImage = transform.parent.parent.transform.Find("Costs/" + transform.name).gameObject;
        cost = int.Parse(infoImage.transform.GetChild(1).name);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!unlock)
        {
            if (GameObject.FindObjectOfType<PlayerStats>().currentMoney >= cost)
            {
                image.color = affordableColor;
                affordable = true;
            }
            else
            {
                image.color = unaffordableColor;
            }
            infoImage.SetActive(true);
            
            entering = true;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!unlock)
        {
            image.color = originColor;
            infoImage.SetActive(false);

            entering = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (entering)
        {
            if (Input.GetMouseButtonDown(0) && affordable)
            {
                unlock = true;
            }
        }

        if (unlock)
        {
            infoImage.SetActive(false);
            image.color = originColor;

            if (transform.childCount > 0)
            {
                transform.GetChild(0).GetComponent<DraggableItem>().enabled = true;
            }
            else
            {
                GetComponent<InventorySlot>().enabled = false;
            }

            if (costTime == 1)
            {
                GameObject.FindObjectOfType<PlayerStats>().currentMoney -= cost;
                costTime--;
            }
        }
    }
}

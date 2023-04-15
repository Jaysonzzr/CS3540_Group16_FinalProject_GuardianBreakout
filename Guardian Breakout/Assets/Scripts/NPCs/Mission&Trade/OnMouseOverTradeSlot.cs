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
    bool dead = false;
    int costTime = 1;

    public AudioClip sellSFX;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        infoImage = transform.parent.parent.transform.Find("MainMission/Costs/" + transform.name).gameObject;
        cost = int.Parse(infoImage.transform.GetChild(0).name);
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
        if (entering && !unlock)
        {
            string npcName = transform.parent.parent.parent.name;
            if (GameObject.Find("NPCs/" + npcName).gameObject.GetComponent<NPCBehavior>().currentState == NPCBehavior.NPCStates.Dead)
            {
                dead = true;
            }
            else if (Input.GetMouseButtonDown(0) && affordable)
            {
                AudioSource.PlayClipAtPoint(sellSFX, Camera.main.transform.position);
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

        if (dead)
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
        }
    }
}

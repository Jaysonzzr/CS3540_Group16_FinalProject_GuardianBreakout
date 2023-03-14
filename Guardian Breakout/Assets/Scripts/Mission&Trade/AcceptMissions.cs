using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AcceptMissions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Canvas myCanvas;

    public Color originColor;
    public Color overColor;
    public Color acceptedColor;

    Image acceptImage;
    Text acceptText;
    
    bool entering;
    public bool missionAccetped = false;

    void Start() 
    {
        acceptImage = GetComponent<Image>();
        acceptText = transform.GetChild(0).GetComponent<Text>();

        acceptImage.color = originColor;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!missionAccetped)
        {
            acceptImage.color = overColor;
            entering = true;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!missionAccetped)
        {
            acceptImage.color = originColor;
            entering = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (entering)
        {
            if (Input.GetMouseButtonDown(0))
            {
                acceptImage.color = acceptedColor;
                acceptText.text = "Acceptted";
                missionAccetped = true;

                string name = transform.parent.parent.name;
                myCanvas.transform.Find("Missions/Missions/" + name).gameObject.SetActive(true);
            }
        }
    }
}

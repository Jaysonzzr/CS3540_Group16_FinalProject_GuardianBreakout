using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FinishMission : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Canvas myCanvas;

    public Color originColor;
    public Color overColor;
    public Color finishedColor;

    Image finishImage;
    Text finishText;

    bool entering;
    public bool couldFinish = false;
    public bool missionFinished = false;
    public bool getReward = false;
    int getTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        finishImage = GetComponent<Image>();
        finishText = transform.GetChild(0).GetComponent<Text>();

        finishImage.color = finishedColor;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (couldFinish && !missionFinished)
        {
            finishImage.color = overColor;
            entering = true;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (couldFinish && !missionFinished)
        {
            finishImage.color = originColor;
            entering = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        string name = transform.parent.parent.name;
        
        if (entering)
        {
            if (Input.GetMouseButtonDown(0))
            {
                finishImage.color = finishedColor;
                finishText.text = "Finished";
                couldFinish = false;
                missionFinished = true;
                getReward = true;

                myCanvas.transform.Find("Missions/Missions/" + name).gameObject.SetActive(false);
            }
        }

        if (!couldFinish)
        {
            finishImage.color = finishedColor;
        }

        if (couldFinish && !entering)
        {
            finishImage.color = originColor;
        }

        if (missionFinished)
        {
            transform.parent.Find("Requires").GetChild(0).GetChild(0).GetComponent<DraggableItem>().enabled = false;
        }

        if (getReward && getTime == 1)
        {
            GameObject rewardMoney = myCanvas.transform.Find("NPCs/" + name + "/MyInventory/Rewards").gameObject.transform.GetChild(0).gameObject;
            GameObject.FindObjectOfType<PlayerStats>().currentMoney += int.Parse(rewardMoney.name);
            getTime--;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionRequires : MonoBehaviour
{
    public string require;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.Find("Accept").GetComponent<AcceptMissions>().missionAccetped == true)
        {
            transform.GetChild(0).GetComponent<InventorySlot>().enabled = true;

            if (transform.GetChild(0).transform.childCount > 0)
            {
                if (transform.GetChild(0).transform.GetChild(0).name == require || transform.GetChild(0).transform.GetChild(0).name == require + "(Clone)")
                {
                    transform.parent.Find("Finish").GetComponent<FinishMission>().couldFinish = true;
                }
            }
            else
            {
                transform.parent.Find("Finish").GetComponent<FinishMission>().couldFinish = false;
            }
        }
    }
}

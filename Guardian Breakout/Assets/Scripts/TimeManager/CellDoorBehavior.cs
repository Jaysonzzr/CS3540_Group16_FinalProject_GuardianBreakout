using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDoorBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] cellDoorsF = GameObject.FindGameObjectsWithTag("CellDoorF");
        foreach (GameObject obj in cellDoorsF) 
        {
            if (FindObjectOfType<TimeManager>().currentState == TimeManager.State.LightsOut)
            {
                Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetRotation, Time.deltaTime);
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(0, 180, 0);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetRotation, Time.deltaTime);
            }
        }

        GameObject[] cellDoorsB = GameObject.FindGameObjectsWithTag("CellDoorB");
        foreach (GameObject obj in cellDoorsB) 
        {
            if (FindObjectOfType<TimeManager>().currentState == TimeManager.State.LightsOut)
            {
                Quaternion targetRotation = Quaternion.Euler(0, -90, 0);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetRotation, Time.deltaTime);
            }
            else
            {
                Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
                obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetRotation, Time.deltaTime);
            }
        }
    }
}

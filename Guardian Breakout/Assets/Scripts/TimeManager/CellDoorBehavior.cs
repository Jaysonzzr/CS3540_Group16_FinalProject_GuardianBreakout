using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDoorBehavior : MonoBehaviour
{
    private GameObject[] cellDoors;

    // Start is called before the first frame update
    void Start()
    {
        cellDoors = GameObject.FindGameObjectsWithTag("CellDoor");
    }

    // Update is called once per frame
    void Update()
    {
        cellDoors = GameObject.FindGameObjectsWithTag("CellDoor");

        if (!GameObject.Find("LevelManager").GetComponent<TimeManager>().cellDoorOpen)
        {
            foreach (GameObject obj in cellDoors)
            {
                if (obj.transform.localRotation != Quaternion.Euler(0, 90, 0))
                {
                    obj.transform.localRotation = Quaternion.Lerp(obj.transform.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime);
                }
            }
        }
        else
        {
            foreach (GameObject obj in cellDoors)
            {
                if (obj.transform.localRotation != Quaternion.Euler(0, 180, 0))
                {
                    obj.transform.localRotation = Quaternion.Lerp(obj.transform.localRotation, Quaternion.Euler(0, 180, 0), Time.deltaTime);
                }
            }
        }
    }
}

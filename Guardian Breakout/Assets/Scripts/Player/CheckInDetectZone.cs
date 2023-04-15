using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInDetectZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehavior>().isInDetectZone = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehavior>().isInDetectZone = false;
        }      
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardOfficeCollider : MonoBehaviour
{
    private void OnTriggerStay(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehavior>().isInGuardOffice = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehavior>().isInGuardOffice = false;
        }      
    }
}

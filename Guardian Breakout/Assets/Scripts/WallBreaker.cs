using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreaker : MonoBehaviour
{
    public float waitTime = 5;
    private float originalWaitTime;
    public float maxDistance = 5;
    void Start()
    {
        originalWaitTime = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            waitTime -= Time.deltaTime;
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            waitTime = originalWaitTime;
        }
        
        if(waitTime <= 0)
        {
            checkForWallBreak();
            waitTime = originalWaitTime;
        }
    }

    void checkForWallBreak()
    {
        if(Physics.Raycast(transform.position,transform.forward, out RaycastHit hit, maxDistance))
        {
            if(hit.collider.CompareTag("Breakable"))
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }
}

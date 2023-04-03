using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallBreaker : MonoBehaviour
{
    public float waitTime = 5;
    private float originalWaitTime;
    public float maxDistance = 5;
    public Slider processBar;

    float currentProcess;

    public InventoryManager inventoryManager;

    Transform pickaxe;
    Animator pickaxeAnim;
    Quaternion pickaxeRot;

    bool breaking = false;

    // Add a variable to store the last breakable object
    private GameObject lastBreakableObject;

    void Start()
    {
        originalWaitTime = waitTime;
        inventoryManager = GameObject.Find("LevelManager").GetComponent<InventoryManager>();

        pickaxe = Camera.main.transform.Find("Pickaxe").transform;
        pickaxeAnim = pickaxe.GetComponent<Animator>();
        pickaxeRot = pickaxe.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        processBar.value = currentProcess;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.CompareTag("Breakable") && hit.distance <= maxDistance)
            {
                if (inventoryManager.holdStuff && (inventoryManager.holding.name == "Pickaxe" || inventoryManager.holding.name == "Pickaxe(Clone)"))
                {
                    // If the breakable object changed, reset the process
                    if (lastBreakableObject != null && lastBreakableObject != hitObject)
                    {
                        waitTime = originalWaitTime;
                        currentProcess = 0.0f;
                        lastBreakableObject.transform.GetComponent<Outline>().enabled = false;
                    }

                    if (Input.GetMouseButton(0))
                    {
                        waitTime -= Time.deltaTime;
                        hitObject.GetComponent<Outline>().enabled = true;
                        currentProcess = (1 - waitTime / originalWaitTime) * 100;
                        processBar.gameObject.SetActive(true);

                        breaking = true;

                        pickaxeAnim.SetInteger("is_attacking", 1);
                    }
                    else
                    {
                        waitTime = originalWaitTime;
                        currentProcess = 0.0f;
                        hitObject.GetComponent<Outline>().enabled = false;
                        processBar.gameObject.SetActive(false);

                        pickaxe.localRotation = Quaternion.Lerp(pickaxe.localRotation, pickaxeRot, Time.deltaTime * 5);
                        pickaxeAnim.SetInteger("is_attacking", 0);
                    }

                    if (waitTime <= 0)
                    {
                        //Destroy(hit.transform.gameObject);
                        hit.transform.GetComponent<MeshRenderer>().enabled = false;
                        hit.transform.GetComponent<BoxCollider>().enabled = false;
                        currentProcess = 0.0f;
                        processBar.gameObject.SetActive(false);
                        waitTime = originalWaitTime;
                    }

                    // Update the last breakable object
                    lastBreakableObject = hitObject;
                }
            }
            else
            {
                if (lastBreakableObject != null)
                {
                    lastBreakableObject.transform.GetComponent<Outline>().enabled = false;
                }

                waitTime = originalWaitTime;
                currentProcess = 0.0f;

                if (breaking)
                {
                    processBar.gameObject.SetActive(false);
                    breaking = false;
                }

                pickaxe.localRotation = Quaternion.Lerp(pickaxe.localRotation, pickaxeRot, Time.deltaTime * 5);
                pickaxeAnim.SetInteger("is_attacking", 0);
            }
        }
    }
}
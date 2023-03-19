using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollCallCollision : MonoBehaviour
{
    private GameObject tutorialHints;
    private Level1Manager level1Manager;
    
    private bool entering = false;
    private int count = 1;

    public string rollCallText;
    
    // Start is called before the first frame update
    void Start()
    {
        tutorialHints = transform.parent.Find("TutorialHints").gameObject;
        level1Manager = transform.parent.GetComponent<Level1Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (entering)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                entering = false;
            }

            tutorialHints.transform.Find("Hints/TitleText").GetComponent<Text>().text = "Roll Call";
            tutorialHints.transform.Find("Hints/MainText").GetComponent<Text>().text = rollCallText;

            level1Manager.PauseGame(tutorialHints);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && count == 1)
        {
            entering = true;
            count--;
        }
    }
}

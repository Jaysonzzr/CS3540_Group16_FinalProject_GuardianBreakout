using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int startingHealth = 100;
    public int startingStrength = 20;
    public int startingIntellect = 20;
    
    public int timeServed;
    public int money;

    public int currentHealth;
    public int currentStrength;
    public int currentIntellect;

    public Canvas myCanvas;
    public Text playerHealth;
    public Text playerStrngth;
    public Text playerIntellect;

    public AudioClip openUISFX;
    public AudioClip closeUISFX;

    // Start is called before the first frame update
    void Start()
    {
        timeServed = 0;
        money = 0;

        currentHealth = startingHealth;
        currentStrength = startingStrength;
        currentIntellect = startingIntellect;

        playerHealth.text = currentHealth.ToString();
        playerStrngth.text = currentStrength.ToString();
        playerIntellect.text = currentIntellect.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject obj = myCanvas.transform.Find("PlayerStats").gameObject;
        if (obj.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                obj.SetActive(false);
                AudioSource.PlayClipAtPoint(closeUISFX, Camera.main.transform.position);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                obj.SetActive(true);
                AudioSource.PlayClipAtPoint(openUISFX, Camera.main.transform.position);
            }
        }

        playerHealth.text = currentHealth.ToString();
        playerStrngth.text = currentStrength.ToString();
        playerIntellect.text = currentIntellect.ToString();
    }
}

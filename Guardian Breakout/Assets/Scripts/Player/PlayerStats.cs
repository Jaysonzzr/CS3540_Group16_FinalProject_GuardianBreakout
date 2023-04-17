using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int startingHealth = 100;
    public int startingStrength = 20;
    public int startingIntellect = 20;
    public int startingMoney = 0;
    
    public int currentHealth;
    public int currentStrength;
    public int currentIntellect;
    public int currentMoney;

    public Canvas myCanvas;
    public Text playerHealth;
    public Text playerStrngth;
    public Text playerIntellect;
    public Text playerMoney;

    public AudioClip openUISFX;
    public AudioClip closeUISFX;

    public bool lockUI = false;
    public bool lockStats = false;

    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        currentStrength = startingStrength;
        currentIntellect = startingIntellect;
        currentMoney = startingMoney;

        playerHealth.text = currentHealth.ToString();
        playerStrngth.text = currentStrength.ToString();
        playerIntellect.text = currentIntellect.ToString();
        playerMoney.text = currentMoney.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuManager.isGamePaused)
        {
            GameObject obj = myCanvas.transform.Find("PlayerStats").gameObject;
            if (obj.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.V) && !lockStats)
                {
                    obj.SetActive(false);
                    AudioSource.PlayClipAtPoint(closeUISFX, Camera.main.transform.position);

                    lockUI = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.V) && !lockUI)
                {
                    obj.SetActive(true);
                    AudioSource.PlayClipAtPoint(openUISFX, Camera.main.transform.position);

                    lockUI = true;
                }
            }

            if (currentHealth <= 0)
            {
                isDead = true;
            }

            playerHealth.text = currentHealth.ToString();
            playerStrngth.text = currentStrength.ToString();
            playerIntellect.text = currentIntellect.ToString();
            playerMoney.text = currentMoney.ToString();
        }
    }
}

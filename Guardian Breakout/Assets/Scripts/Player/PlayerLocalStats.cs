using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLocalStats : MonoBehaviour
{
    private PlayerStats playerStats;

    private Text health;
    private Text money;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.Find("LevelManager").GetComponent<PlayerStats>();

        health = GameObject.Find("Canvas/Health").GetComponent<Text>();
        money = GameObject.Find("Canvas/Money").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        health.text = "Health: " + playerStats.currentHealth.ToString();
        money.text = "Money: " + playerStats.currentMoney.ToString();
    }
}

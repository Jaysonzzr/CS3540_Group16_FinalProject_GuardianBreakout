using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text timerText;
    private float startTime;

    public static Timer instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        startTime = Time.unscaledTime;
    }

    void Update()
    {
        timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();

        float elapsedTime = Time.unscaledTime - startTime;
        int minutes = (int)elapsedTime / 60;
        int seconds = (int)elapsedTime % 60;
        int milliseconds = (int)(elapsedTime * 1000) % 1000;

        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }
    }
}

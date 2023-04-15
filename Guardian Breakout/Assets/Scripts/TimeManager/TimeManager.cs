using UnityEngine;
using UnityEngine.UI;
using System;

// Use these lines to check the State
/*
    if (FindObjectOfType<TimeManager>().currentState == TimeManager.State.WakeRoll)
    {
        transform.position = new Vector3(0, 10, 0);
    }
*/

public class TimeManager : MonoBehaviour
{
    public Text clockText;
    public Text scheduleText;
    public AudioClip bellSFX;
    public DateTime currentTime;

    public bool generateFood = false;
    public bool cellDoorOpen = false;

    public enum State
    {
        WakeRoll, Breakfast, MorningFree, Lunch, Job,
        Exercise, Shower, Dinner, NightFree, NightRoll, LockCell, LightsOut
    };
    public State currentState;

    public int trappedDay = 0;

    void Start()
    {
        // Initialize the clock time
        currentTime = new DateTime(2023, 3, 11, 6, 30, 0);

        currentState = State.LightsOut;
    }

    void Update()
    {
        // Increment the current time by 2 minutes for every second that passes in real time
        currentTime = currentTime.AddMinutes(1.2f * Time.deltaTime);

        // Update the clock text
        clockText.text = currentTime.ToString("HH:mm");

        Schedule();

        if (currentState == State.Breakfast || currentState == State.Lunch || currentState == State.Dinner)
        {
            generateFood = true;
        }
        else
        {
            generateFood = false;
        }

        if (currentState == State.LightsOut)
        {
            cellDoorOpen = false;
        }
        else
        {
            cellDoorOpen = true;
        }
    }

    void Schedule()
    {
        switch(currentState)
        {
            case State.LightsOut:
                scheduleText.text = "Lights Out";
                if (currentTime.Hour == 7)
                {
                    trappedDay++;
                    currentState = State.WakeRoll;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
            case State.WakeRoll:
                scheduleText.text = "Roll Call";
                if (currentTime.Hour == 8)
                {
                    currentState = State.Breakfast;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
            case State.Breakfast:
                scheduleText.text = "Breakfast Time";
                if (currentTime.Hour == 9)
                {
                    currentState = State.MorningFree;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
            case State.MorningFree:
                scheduleText.text = "Free Time";
                if (currentTime.Hour == 12)
                {
                    currentState = State.Lunch;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
            case State.Lunch:
                scheduleText.text = "Lunch Time";
                if (currentTime.Hour == 13)
                {
                    currentState = State.Job;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
            case State.Job:
                scheduleText.text = "Free Time";
                if (currentTime.Hour == 15)
                {
                    currentState = State.Exercise;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
            case State.Exercise:
                scheduleText.text = "Exercise Time";
                if (currentTime.Hour == 16)
                {
                    currentState = State.Shower;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
            case State.Shower:
                scheduleText.text = "Shower Time";
                if (currentTime.Hour == 17)
                {
                    currentState = State.Dinner;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
            case State.Dinner:
                scheduleText.text = "Dinner Time";
                if (currentTime.Hour == 18)
                {
                    currentState = State.NightFree;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
            case State.NightFree:
                scheduleText.text = "Free Time";
                if (currentTime.Hour == 21)
                {
                    currentState = State.NightRoll;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
            case State.NightRoll:
                scheduleText.text = "Roll Call";
                if (currentTime.Hour == 22)
                {
                    currentState = State.LockCell;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
            case State.LockCell:
                scheduleText.text = "Get Back To Your Cell!";
                if (currentTime.Hour == 23)
                {
                    currentState = State.LightsOut;
                    AudioSource.PlayClipAtPoint(bellSFX, Camera.main.transform.position);
                }
                break;
        }
    }
}

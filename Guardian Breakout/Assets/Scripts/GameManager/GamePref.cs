using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GamePref : MonoBehaviour
{
    public AudioMixer audioMixer;

    public float sensitivity;

    public Slider sensitivitySlider;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        sensitivity = sensitivitySlider.value;
        audioMixer.SetFloat("BGM", volumeSlider.value);
    }
}

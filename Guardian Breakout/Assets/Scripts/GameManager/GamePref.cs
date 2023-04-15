using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GamePref : MonoBehaviour
{
    public AudioMixer audioMixer;
    public float sensitivity;
    public float volume;

    private Slider sensitivitySlider;
    private Slider volumeSlider;

    public static GamePref instance;

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sensitivitySlider = Utility.FindObjectByTagIncludingInactive("Sensitivity").GetComponent<Slider>();
        volumeSlider = Utility.FindObjectByTagIncludingInactive("BGMVolume").GetComponent<Slider>();

        LoadSliderValues();

        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.AddListener(delegate { SaveSensitivity(); });
        }
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(delegate { SaveVolume(); });
        }
    }

    void Update()
    {
        if (sensitivitySlider != null)
        {
            sensitivity = sensitivitySlider.value;
            audioMixer.SetFloat("MouseSensitivity", sensitivity);
        }
        if (volumeSlider != null)
        {
            volume = volumeSlider.value;
            audioMixer.SetFloat("BGM", volume);
        }
    }

    private void LoadSliderValues()
    {
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 0.5f);
            sensitivity = sensitivitySlider.value;
        }
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
            volume = volumeSlider.value;
        }
    }

    private void SaveSensitivity()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("Volume", volume);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

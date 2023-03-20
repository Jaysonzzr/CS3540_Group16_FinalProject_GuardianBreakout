using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Text gameText;

    public AudioClip winSFX;

    public static bool isGameOver = false;

    public string nextLevel;

    private PlayerController playerController;
    private CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelBeat()
    {
        isGameOver = true;
        gameText.text = "Level Complete";
        gameText.gameObject.SetActive(true);

        playerController.enabled = false;
        cameraController.enabled = false;

        Camera.main.GetComponent<AudioSource>().pitch = 2;
        //AudioSource.PlayClipAtPoint(winSFX, Camera.main.transform.position);

        if (!string.IsNullOrEmpty(nextLevel))
        {
            Invoke("LoadNextLevel", 2);
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}

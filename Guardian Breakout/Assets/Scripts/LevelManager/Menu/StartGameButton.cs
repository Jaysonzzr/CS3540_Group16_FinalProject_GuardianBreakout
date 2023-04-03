using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public int gameStartScene;
    public GameObject levelChoose;

    public void GameStart()
    {
        levelChoose.SetActive(true);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(gameStartScene);
    }
}

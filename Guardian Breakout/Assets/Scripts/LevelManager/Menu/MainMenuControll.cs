using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControll : MonoBehaviour
{
    public GameObject levelChoose;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void Update() 
    {
        if (levelChoose.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                levelChoose.SetActive(false);
            }
        }
    }
}

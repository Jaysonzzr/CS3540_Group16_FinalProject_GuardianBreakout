using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControll : MonoBehaviour
{
    public GameObject[] canvas;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void Update() 
    {
        foreach (GameObject canvas in canvas)
        {
            if (canvas.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    canvas.SetActive(false);
                }
            }
        }
    }
}

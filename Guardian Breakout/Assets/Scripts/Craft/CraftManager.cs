using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour
{
    public PlayerController playerController;
    public CameraController cameraController;


    public Canvas myCanvas;
    public InventorySlot[] inventorySlots;

    public AudioClip openUISFX;
    public AudioClip closeUISFX;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject obj = myCanvas.transform.Find("CraftTable").gameObject;
        if (obj.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                obj.SetActive(false);
                AudioSource.PlayClipAtPoint(closeUISFX, Camera.main.transform.position);
                playerController.enabled = true;
                cameraController.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                GameObject.FindObjectOfType<PlayerStats>().lockUI = false;
            }

            foreach (InventorySlot slot in inventorySlots)
            {
                DraggableItem item = slot.GetComponentInChildren<DraggableItem>();

                if (item != null)
                {
                    
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C) && !GameObject.FindObjectOfType<PlayerStats>().lockUI)
            {
                obj.SetActive(true);
                AudioSource.PlayClipAtPoint(openUISFX, Camera.main.transform.position);
                playerController.enabled = false;
                cameraController.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                GameObject.FindObjectOfType<PlayerStats>().lockUI = true;
            }
        }
    }
}

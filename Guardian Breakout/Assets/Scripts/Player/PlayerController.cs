using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventorySlot[] toolBar;

    public float moveSpeed = 10;
    public float gravity = 9.81f;
    public float airControl = 10;

    public Animator playerController;

    CharacterController controller;
    Vector3 input, moveDirection;

    public Collider damageBox;
    bool attacking;
    float hurtTime = 0.5f;
    float currentHurtTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerAttack();
    }

    void PlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        input *= moveSpeed;

        if(controller.isGrounded)
        {
            moveDirection = input;
        }
        else
        {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }
        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(input * Time.deltaTime);
    }

    void PlayerAttack()
    {
        int selectedSlotIdx = GameObject.FindObjectOfType<InventoryManager>().selectedSlot;
        if (toolBar[selectedSlotIdx].transform.childCount == 0 && Input.GetMouseButtonDown(0))
        {
            damageBox.enabled = true;
        }
        else
        {
            damageBox.enabled = false;
        }
    }
}

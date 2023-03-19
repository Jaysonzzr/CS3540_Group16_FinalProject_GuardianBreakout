using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventorySlot[] toolBar;

    public float moveSpeed = 10;
    public float gravity = 9.81f;
    public float airControl = 10;

    public Animator playerAnim;

    CharacterController controller;
    Vector3 input, moveDirection;

    public BoxCollider damageBox;
    bool attacking;
    public bool crwobarAttack;
    float attackCoolDown = 2f;
    float currentAttackTime = 0;
    float currentDuration = 0;

    Vector3 crowbarPos;
    Quaternion crowbarRot;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnim = Camera.main.transform.Find("PlayerBody").GetComponent<Animator>();
        damageBox = transform.Find("DamageBox").GetComponent<BoxCollider>();

        crowbarPos = Camera.main.transform.Find("Crowbar").transform.localPosition;
        crowbarRot = Camera.main.transform.Find("Crowbar").transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerAttack();

        currentAttackTime += Time.deltaTime;
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
        if (currentAttackTime >= attackCoolDown)
        {
            int selectedSlotIdx = GameObject.FindObjectOfType<InventoryManager>().selectedSlot;
            if (toolBar[selectedSlotIdx].transform.childCount == 0 && Input.GetMouseButtonDown(0))
            {
                transform.GetComponent<PlayerBehavior>().damage = Mathf.RoundToInt(10 * transform.GetComponent<PlayerBehavior>().damageBonus);
                attacking = true;
                currentAttackTime = 0.0f;
                currentDuration = 0.0f;
                damageBox.enabled = true;
            }
        }

        if (attacking)
        {
            playerAnim.SetInteger("is_punching", 1);
            var animDuration = playerAnim.GetCurrentAnimatorStateInfo(0).length;
            
            if (currentDuration > animDuration)
            {
                currentDuration = 0.0f;
                damageBox.enabled = false;
                playerAnim.SetInteger("is_punching", 0);
                attacking = false;
            }
            else
            {
                currentDuration += Time.deltaTime;
            } 
        }
    }

    public void PlayerCrowbarAttack()
    {
        Animator anim = Camera.main.transform.Find("Crowbar").GetComponent<Animator>();

        if (currentAttackTime >= attackCoolDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject crowbar = Camera.main.transform.Find("Crowbar").gameObject;
                
                crowbar.transform.localPosition = crowbarPos;
                crowbar.transform.localRotation = crowbarRot;

                crwobarAttack = true;
                currentAttackTime = 0.0f;
                currentDuration = 0.0f;
                damageBox.enabled = true;
            }
        }

        if (crwobarAttack)
        {
            anim.SetInteger("is_attacking", 1);
            var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
            
            if (currentDuration > animDuration)
            {
                currentDuration = 0.0f;
                damageBox.enabled = false;
                anim.SetInteger("is_attacking", 0);
                crwobarAttack = false;
            }
            else
            {
                currentDuration += Time.deltaTime;
            } 
        }
        else
        {
            damageBox.enabled = false;
            anim.SetInteger("is_attacking", 0);
        }
    }
}

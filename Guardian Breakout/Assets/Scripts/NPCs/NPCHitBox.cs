using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHitBox : MonoBehaviour
{
    private CharacterController controller;
    GameObject player;

    // Settings for bounce-back effect
    public float bounceBackSpeed = 15f;
    public float bounceBackDuration = 0.5f;

    private float bounceBackEndTime = 0f;

    public bool getHurt;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (Time.time < bounceBackEndTime)
        {
            // Calculate move direction (backward)
            Vector3 moveDirection = player.transform.forward;

            // Apply move direction with a certain speed
            controller.SimpleMove(moveDirection.normalized * bounceBackSpeed);
        }

        if (getHurt)
        {
            TakeDamage();
            getHurt = false;
        }
    }

    // Call this function when the player takes damage
    public void TakeDamage()
    {
        // Set the end time of the bounce-back effect
        bounceBackEndTime = Time.time + bounceBackDuration;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("PlayerDamageBox"))
        {
            getHurt = true;
        }
    }
}


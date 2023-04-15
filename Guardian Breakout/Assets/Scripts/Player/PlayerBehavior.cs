using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public InventorySlot[] toolBar;

    float increaseTime = 1;
    float currentTime;

    bool hurted = false;
    public bool getHurt = false;
    Vector3 hurtDirection;
    public float bounceBackSpeed = 15f;
    public float bounceBackDuration = 0.5f;

    public int damage;
    public float damageBonus;

    public bool isInGuardOffice;

    private float bounceBackEndTime = 0f;
    CharacterController controller;
    InventoryManager inventoryManager;

    Vector3 crowbarPos;
    Quaternion crowbarRot;

    public AudioClip hurtSFX1;
    public AudioClip hurtSFX2;
    public AudioClip deadSFX1;
    public AudioClip deadSFX2;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        crowbarPos = Camera.main.transform.Find("Crowbar").transform.localPosition;
        crowbarRot = Camera.main.transform.Find("Crowbar").transform.localRotation;

        inventoryManager = GameObject.Find("LevelManager").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInGuardOffice)
        {
            print("IN");
        }
        else
        {
            print("Out");
        }

        damageBonus = GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentStrength / 30;

        if (inventoryManager.holding != null)
        {
            if (inventoryManager.holding.name == "Meal(Clone)")
            {
                Camera.main.transform.Find("Plate").gameObject.SetActive(true);
            }
        }
        else
        {
            Camera.main.transform.Find("Plate").gameObject.SetActive(false);
        }

        if (inventoryManager.holding != null)
        {
            if (inventoryManager.holding.name == "Pickaxe" ||
                inventoryManager.holding.name == "Pickaxe(Clone)")
            {
                Camera.main.transform.Find("Pickaxe").gameObject.SetActive(true);
            }
        }
        else
        {
            Camera.main.transform.Find("Pickaxe").gameObject.SetActive(false);
        }

        if (inventoryManager.holding != null)
        {
            if (inventoryManager.holding.name == "Crowbar" ||
                inventoryManager.holding.name == "Crowbar(Clone)")
            {
                Camera.main.transform.Find("Crowbar").gameObject.SetActive(true);

                damage = Mathf.RoundToInt(40 * damageBonus);
                if (GetComponent<PlayerController>().enabled)
                {
                    GetComponent<PlayerController>().PlayerCrowbarAttack();
                }
            }
        }
        else
        {
            GameObject crowbar = Camera.main.transform.Find("Crowbar").gameObject;
                
            crowbar.transform.localPosition = crowbarPos;
            crowbar.transform.localRotation = crowbarRot;
            transform.GetComponent<PlayerController>().crwobarAttack = false;
            crowbar.SetActive(false);
        }

        if (GameObject.Find("LevelManager").GetComponent<PlayerStats>().isDead)
        {
            transform.position = new Vector3(5.4873f, 0.59f, -26.9027f);
            transform.rotation = Quaternion.Euler(0, 0, 0); 
            transform.GetComponent<PlayerController>().enabled = false;
            
            Camera.main.GetComponent<CameraController>().enabled = false;
            Camera.main.transform.position = new Vector3(4.453f + transform.position.x, 0.775f + transform.position.y, 0.165f + transform.position.z);
            Camera.main.transform.rotation = Quaternion.Euler(-34, -90, 0);
            
            if (GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentHealth <= 95)
            {
                if (currentTime >= increaseTime)
                {
                    GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentHealth += 5;
                    currentTime = 0.0f;
                }
                else
                {
                    currentTime += Time.deltaTime;
                }
            }
            else
            {
                GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentHealth = 100;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject.Find("LevelManager").GetComponent<PlayerStats>().isDead = false;
                transform.GetComponent<PlayerController>().enabled = true;
                Camera.main.GetComponent<CameraController>().enabled = true;
            }
        }

        if (hurted)
        {
            if (GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentHealth > 0)
            {
                AudioClip hurtSFX = Random.Range(0, 2) == 0 ? hurtSFX1 : hurtSFX2;
                AudioSource.PlayClipAtPoint(hurtSFX, Camera.main.transform.position);
            }
            else
            {
                AudioClip deadSFX = Random.Range(0, 2) == 0 ? deadSFX1 : deadSFX2;
                AudioSource.PlayClipAtPoint(deadSFX, Camera.main.transform.position);
            }

            TakeDamage();
            getHurt = true;
            hurted = false;
        }

        if (Time.time < bounceBackEndTime && GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentHealth >= 20)
        {
            // Apply move direction with a certain speed
            controller.SimpleMove(hurtDirection.normalized * bounceBackSpeed);
        }
    }

    public void TakeDamage()
    {
        // Set the end time of the bounce-back effect
        bounceBackEndTime = Time.time + bounceBackDuration;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("NPCDamageBox"))
        {
            hurted = true;
            hurtDirection = other.gameObject.transform.forward;
            if (other.transform.parent.GetComponent<NPCBehavior>())
            {
                GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentHealth -= other.transform.parent.GetComponent<NPCBehavior>().npcDamage;
            }
            else
            {
                GameObject.Find("LevelManager").GetComponent<PlayerStats>().currentHealth -= other.transform.parent.GetComponent<GuardBehavior>().npcDamage;
            }
            other.enabled = false;
        }

        else if (other.CompareTag("Win"))
        {
            FindObjectOfType<LevelManager>().LevelBeat();
        }
    }
}

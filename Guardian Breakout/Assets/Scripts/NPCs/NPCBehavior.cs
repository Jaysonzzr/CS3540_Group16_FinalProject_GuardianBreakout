using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NPCBehavior : MonoBehaviour
{
    public enum NPCStates
    {
        Idle,
        Patrol,
        Hurt,
        Sit,
        Chase,
        Attack,
        Dead,
        Trade
    }

    public NPCStates currentState;

    public float startingHealth;
    public float currentHealth;

    public GameObject player;
    private CharacterController controller;
    Animator anim;

    public GameObject[] wanderPoints;
    Vector3 nextDestination;

    float distanceToPlayer;
    int currentDestinationIdx = 0;

    public bool dying = false;
    int deadTime;

    public int npcDamage;

    public float attackDistance = 3;
    public float chaseDistance = 10;
    BoxCollider damageBox;
    public float attackRate = 2;
    float attackTime = 0;
    float currentDuration = 0;

    public float bounceBackSpeed = 15f;
    public float bounceBackDuration = 0.5f;

    private float bounceBackEndTime = 0f;

    public bool getHurt = false;
    bool hasDead;
    float hurtTime = 0;

    public AudioClip hurtSFX1;
    public AudioClip hurtSFX2;

    public AudioClip deadSFX1;
    public AudioClip deadSFX2;

    public GameObject levelManager;
    int currentHour;
    NavMeshAgent agent;
    bool workOrNot = false;
    public GameObject sitPoint;
    Vector3 oriPoint;
    public GameObject meal;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        damageBox = transform.Find("DamageBox").GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        currentState = NPCStates.Idle;
        // FindNextPoint();

        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance
            (transform.position, player.transform.position);

        currentHour = levelManager.GetComponent<TimeManager>().currentTime.Hour;

        switch(currentState)
        {
            case NPCStates.Idle:
                UpdateIdleState();
                break;
            case NPCStates.Patrol:
                UpdatePatrolState();
                break;
            case NPCStates.Trade:
                UpdateTradeState();
                break;
            case NPCStates.Sit:
                UpdateSitState();
                break;
            case NPCStates.Hurt:
                UpdateHurtState();
                break;
            case NPCStates.Chase:
                UpdateChaseState();
                break;
            case NPCStates.Attack:
                UpdateAttackState();
                break;
            case NPCStates.Dead:
                UpdateDeadState();
                break;
        }

        if (getHurt)
        {
            AudioClip hurtSFX = UnityEngine.Random.Range(0, 2) == 0 ? hurtSFX1 : hurtSFX2;
            AudioSource.PlayClipAtPoint(hurtSFX, transform.position);

            TakeDamage();
            currentState = NPCStates.Hurt;
            hurtTime = 0.0f;
            getHurt = false;
        }
        if (currentState != NPCStates.Attack)
        {
            damageBox.enabled = false;
        }

        if (currentState != NPCStates.Sit)
        {
            controller.enabled = true;
        }
        else
        {
            controller.enabled = false;
        }

        if (currentHealth <= 0 && !hasDead)
        {
            dying = true;
            hasDead = true;
            currentState = NPCStates.Dead;
        }
    }

    void UpdateIdleState()
    {
        anim.SetInteger("animState", 0);
        hasDead = false;
        currentHealth = startingHealth;
        if (currentHour >= 7 && currentHour < 23 && SceneManager.GetActiveScene().name != "Level1")
        {
            FindNextPoint();
            if (Vector3.Distance(transform.position, nextDestination) > 1.5f)
            {
                currentState = NPCStates.Patrol;
            }
        }
    }

    void UpdatePatrolState()
    {
        FindNextPoint();
        anim.SetInteger("animState", 1);

        agent.stoppingDistance = 0;

        agent.speed = 3f;

        
        if (Vector3.Distance(transform.position, nextDestination) < 1.5f)
        {
            if(!workOrNot)
            {
                if (currentHour == 7 || currentHour == 15 || currentHour == 16 || currentHour == 21 || currentHour == 22)
                {
                    currentState = NPCStates.Idle;
                }
                else
                {
                    oriPoint = transform.position;
                    currentState = NPCStates.Sit;
                }
            }
        }
        
        //FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);
    }

    void UpdateTradeState()
    {
        anim.SetInteger("animState", 3);
        FaceTarget(player.transform.position);
    }

    void UpdateHurtState()
    {
        anim.SetInteger("animState", 7);
        var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;

        if (Time.time < bounceBackEndTime)
        {
            // Calculate move direction (backward)
            Vector3 moveDirection = player.transform.forward;

            // Apply move direction with a certain speed
            controller.SimpleMove(moveDirection.normalized * bounceBackSpeed);
        }
        
        hurtTime += Time.deltaTime;
        if (hurtTime > animDuration)
        {
            currentState = NPCStates.Attack;
        }

        FaceTarget(player.transform.position);
    }

    void UpdateSitState()
    {
        anim.SetInteger("animState", 6);
        GetComponent<CharacterController>().enabled = false;
        transform.position = sitPoint.transform.position;
        transform.rotation = sitPoint.transform.rotation;
        meal.SetActive(true);
        hasDead = false;
        currentHealth = startingHealth;
        if (currentHour >= 7 && currentHour < 23 && SceneManager.GetActiveScene().name != "Level1")
        {
            FindNextPoint();
            if (Vector3.Distance(oriPoint, nextDestination) > 1.5f)
            {
                transform.position = oriPoint;
                GetComponent<CharacterController>().enabled = true;
                meal.SetActive(false);
                currentState = NPCStates.Patrol;
            }
        }
    }

    void UpdateChaseState()
    {
        anim.SetInteger("animState", 2);

        if (distanceToPlayer <= attackDistance)
        {
            currentState = NPCStates.Attack;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = NPCStates.Idle;
        }

        FaceTarget(player.transform.position);
    }

    void UpdateAttackState()
    {
        if (distanceToPlayer <= attackDistance)
        {
            currentState = NPCStates.Attack;
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = NPCStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = NPCStates.Idle;
        }

        FaceTarget(player.transform.position);
        anim.SetInteger("animState", 4);
        var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        if (attackTime >= 0.7f * animDuration)
        {
            damageBox.enabled = true;
            //Debug.Log("true");
            currentDuration = 0.0f;
            if (currentDuration > animDuration)
            {
                currentDuration = 0.0f;
                damageBox.enabled = false;
            }
            else
            {
                currentDuration += Time.deltaTime;
            }
            attackTime = 0.0f;
        }

        else
        {
            attackTime += Time.deltaTime;
        }
    }

    void UpdateDeadState()
    {
        anim.SetInteger("animState", 5);

        if (GameObject.Find("LevelManager").GetComponent<TimeManager>().currentTime.Hour == deadTime + 2)
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);

            // Check if the game object is outside the camera view.
            if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1) 
            {
                // transform.position = new Vector3(0, 0, 0);
                currentState = NPCStates.Idle;
            }
        }

        if (dying)
        {
            AudioClip deadSFX = UnityEngine.Random.Range(0, 2) == 0 ? deadSFX1 : deadSFX2;
            AudioSource.PlayClipAtPoint(deadSFX, transform.position);

            deadTime = GameObject.Find("LevelManager").GetComponent<TimeManager>().currentTime.Hour;
            dying = false;
        }
    }
    
    void FindNextPoint()
    {
        if (currentHour == 7 || currentHour == 21)
        {
            workOrNot = false;
            //Roll call
            nextDestination = wanderPoints[0].transform.position;
        }
        else if (currentHour == 8 || currentHour == 12 || currentHour == 17)
        {
            workOrNot = false;
            //dinning hall
            nextDestination = wanderPoints[1].transform.position;
        }
        else if ((currentHour >= 9 && currentHour < 12) || (currentHour >= 13 && currentHour < 15) || (currentHour >= 18 && currentHour < 21))
        {
            workOrNot = true;
            //Free time
            if (nextDestination == wanderPoints[2].transform.position && Vector3.Distance(transform.position, nextDestination) < 1.5f)
            {
                nextDestination = wanderPoints[3].transform.position;
            }
            else if (nextDestination == wanderPoints[3].transform.position && Vector3.Distance(transform.position, nextDestination) < 1.5f)
            {
                nextDestination = wanderPoints[2].transform.position;
            }
            else if (nextDestination != wanderPoints[3].transform.position && nextDestination != wanderPoints[2].transform.position)
            {
                nextDestination = wanderPoints[2].transform.position;
            }
        }
        else if (currentHour == 15)
        {
            workOrNot = false;
            //Exercise
            nextDestination = wanderPoints[4].transform.position;
        }
        else if (currentHour == 16)
        {
            workOrNot = false;
            //Bath
            nextDestination = wanderPoints[5].transform.position;
        }
        else if (currentHour == 22)
        {
            workOrNot = false;
            nextDestination = wanderPoints[6].transform.position;
        }
    }
    
    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp
            (transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    public void TakeDamage()
    {
        // Set the end time of the bounce-back effect
        bounceBackEndTime = Time.time + bounceBackDuration;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("PlayerDamageBox") && currentState != NPCStates.Dead)
        {
            getHurt = true;
            currentHealth -= other.transform.parent.GetComponent<PlayerBehavior>().damage;
            other.enabled = false;
        }
    }
}

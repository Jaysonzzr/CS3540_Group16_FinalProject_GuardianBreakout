using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBehavior : MonoBehaviour
{
    InventoryManager inventoryManager;

    NavMeshAgent agent;

    public enum NPCStates
    {
        Idle,
        Patrol,
        Hurt,
        Sit,
        Chase,
        Attack,
        Dead,
        Alert,
    }

    public NPCStates currentState;

    public float startingHealth;
    public float currentHealth;

    public GameObject player;
    private CharacterController controller;
    Animator anim;

    public GameObject[] wanderPoints;
    int currentDestinationIndex = 0;
    Vector3 nextDestination;

    float distanceToPlayer;

    public bool dying = false;
    int deadTime;

    public int npcDamage;

    public float attackDistance = 3;
    public float chaseDistance = 10;
    BoxCollider damageBox;
    public float attackRate = 2;
    float attackTime = 0;
    float currentDuration = 0;

    public float bounceBackSpeed = 0.1f;
    public float bounceBackDuration = 0.5f;

    private float bounceBackEndTime = 0f;

    public bool getHurt = false;
    bool hasDead;
    float hurtTime = 0;

    public AudioClip hurtSFX1;
    public AudioClip hurtSFX2;

    public AudioClip deadSFX1;
    public AudioClip deadSFX2;

    public Transform enemyEyes;
    public float fieldOfView = 90;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("LevelManager").GetComponent<InventoryManager>();
        player = GameObject.FindWithTag("Player");
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        damageBox = transform.Find("DamageBox").GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        currentState = NPCStates.Patrol;
        FindNextPoint();

        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance
            (transform.position, player.transform.position);

        switch (currentState)
        {
            case NPCStates.Idle:
                UpdateIdleState();
                break;
            case NPCStates.Patrol:
                UpdatePatrolState();
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
            case NPCStates.Alert:
                UpdateAlertState();
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

        if (IsPlayerInClearFOV() && player.GetComponent<PlayerBehavior>().isInDetectZone)
        {
            currentState = NPCStates.Chase;
        }
    }

    void UpdateIdleState()
    {
        anim.SetInteger("animState", 0);
        hasDead = false;
        currentHealth = startingHealth;
    }

    void UpdatePatrolState()
    {
        anim.SetInteger("animState", 1);

        agent.stoppingDistance = 0;

        agent.speed = 3f;
        
        if (Vector3.Distance(transform.position, nextDestination) < 3)
        {
            FindNextPoint();
        }
        
        if (distanceToPlayer < chaseDistance && IsPlayerInClearFOV())
        {
            currentState = NPCStates.Alert;
            FaceTarget(player.transform.position);
        }

        // FaceTarget(nextDestination);
        agent.SetDestination(nextDestination);
    }

    void UpdateSitState()
    {
        anim.SetInteger("animState", 6);
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
            currentState = NPCStates.Chase;
        }

        FaceTarget(player.transform.position);
    }

    void UpdateChaseState()
    {
        anim.SetInteger("animState", 2);

        if (distanceToPlayer <= attackDistance)
        {
            currentState = NPCStates.Attack;
        }
        else if (distanceToPlayer > (chaseDistance * 3))
        {
            currentState = NPCStates.Patrol;
        }

        // FaceTarget(player.transform.position);
        agent.SetDestination(player.transform.position);
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
            currentState = NPCStates.Patrol;
        }

        FaceTarget(player.transform.position);
        anim.SetInteger("animState", 4);
        var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        if (attackTime >= 0.7f * animDuration)
        {
            damageBox.enabled = true;

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
                currentState = NPCStates.Patrol;
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

    void UpdateAlertState()
    {
        anim.SetInteger("animState", 6);

        var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        ObjectsInteractive objectsInteractive = Camera.main.GetComponent<ObjectsInteractive>();

        // Will be modified from item name to tag
        if (inventoryManager.holdStuff && (inventoryManager.holding != null && 
            (inventoryManager.holding.name == "Pickaxe" || inventoryManager.holding.name == "Pickaxe(Clone)" ||
            inventoryManager.holding.name == "Crowbar" || inventoryManager.holding.name == "Crowbar(Clone)")) &&
            IsPlayerInClearFOV())
        {
            currentState = NPCStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = NPCStates.Patrol;
        }
        else if (objectsInteractive.opening && IsPlayerInClearFOV())
        {
            currentState = NPCStates.Chase;
        }

        FaceTarget(player.transform.position);
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

    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }

            return false;
        }

        return false;
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, wanderPoints.Length);
        } while (newIndex == currentDestinationIndex);

        currentDestinationIndex = newIndex;
    }
}

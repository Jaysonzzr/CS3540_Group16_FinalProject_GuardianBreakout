using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NPCBehavior : MonoBehaviour
{
    public enum NPCStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead,
        Trade
    }

    public NPCStates currentState;

    public float enemySpeed = 5;
    public GameObject player;

    public GameObject[] wanderPoints;
    Vector3 nextDestination;

    float distanceToPlayer;
    int currentDestinationIdx = 0;

    public bool dying = false;
    int deadTime;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        currentState = NPCStates.Patrol;
        FindNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance
            (transform.position, player.transform.position);

        switch(currentState)
        {
            case NPCStates.Patrol:
                UpdatePatrolState();
                break;
            case NPCStates.Trade:
                UpdateTradeState();
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
    }

    void UpdatePatrolState()
    {
        anim.SetInteger("animState", 1);

        if (Vector3.Distance(transform.position, nextDestination) < 1)
        {
            FindNextPoint();
        }

        FaceTarget(nextDestination);

        transform.position = Vector3.MoveTowards
            (transform.position, nextDestination, Time.deltaTime * enemySpeed);
    }

    void UpdateTradeState()
    {
        anim.SetInteger("animState", 3);
    }

    void UpdateChaseState()
    {
        anim.SetInteger("animState", 2);
    }

    void UpdateAttackState()
    {
        anim.SetInteger("animState", 4);
    }

    void UpdateDeadState()
    {
        anim.SetInteger("animState", 5);

        if (GameObject.Find("LevelManager").GetComponent<TimeManager>().currentTime.Hour == deadTime + 1)
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);

            // Check if the game object is outside the camera view.
            if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1) 
            {
                transform.position = new Vector3(0, 0, 0);
                currentState = NPCStates.Patrol;
            }
        }

        if (dying)
        {
            deadTime = GameObject.Find("LevelManager").GetComponent<TimeManager>().currentTime.Hour;
            dying = false;
        }
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIdx].transform.position;

        currentDestinationIdx = (currentDestinationIdx + 1) 
            % wanderPoints.Length;
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp
            (transform.rotation, lookRotation, 10 * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

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

    }

    void UpdateChaseState()
    {

    }

    void UpdateAttackState()
    {

    }

    void UpdateDeadState()
    {
        
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

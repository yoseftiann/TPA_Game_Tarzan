using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatorAI : MonoBehaviour
{
    public enum States
    {
        Wild,
        Pet
    }
    public States currentState;
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    private Transform zone;
    public LayerMask whatIsGround, whatIsPlayer;
    public LayerMask whatisEnemy;
    public int patrolSpeed, chaseSpeed, followSpeed;

    [Header("Patroling")]
    public Vector3 walkPoint;
    public bool walkPointset;
    public float padding;
    private Transform nearestEnemy;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    public float damage;
    bool alreadyAttacked;

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Animator anim;
    
    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Start() {
        zone = agent.transform.parent;
        currentState = States.Wild;
    }

    private void FindNearestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, sightRange, whatisEnemy);
        float shortestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            NavigatorAI navAI = enemy.GetComponent<NavigatorAI>();
            if(enemy.transform != this.transform && navAI && navAI.currentState == NavigatorAI.States.Wild)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy.transform;
                }
            }
        }
    }

    private bool isInsideZone(BoxCollider collider, Vector3 potentialWalkPoint)
    {
        if((potentialWalkPoint.x >= collider.bounds.min.x && potentialWalkPoint.x <= collider.bounds.max.x) && (potentialWalkPoint.z >= collider.bounds.min.z && potentialWalkPoint.z <= collider.bounds.max.z))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        switch(currentState)
        {
            case States.Wild:
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

                if(!playerInSightRange && !playerInAttackRange) Patroling();
                if(playerInSightRange && !playerInAttackRange) ChasePlayer();
                if(playerInAttackRange && playerInSightRange) AttackPlayer();
                break;

            case States.Pet:
                nearestEnemy = null;
                FindNearestEnemy();
                if(!nearestEnemy)
                {
                    FollowPlayer();
                }
                else if(EnemyInAttackRange())
                {
                    AttackEnemy();
                }
                else
                {
                    // Debug.Log("ChaseEnemy");
                    ChaseEnemy();
                }
                break;
        }
        AlignToSlope();
    }

    private bool EnemyInAttackRange()
    {
        if (nearestEnemy)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, nearestEnemy.position);
            return distanceToEnemy <= attackRange;
        }
        return false;
    }

    private void AttackEnemy()
    {
        anim.SetBool("isAttacking", true);
        agent.SetDestination(transform.position);

        transform.LookAt(nearestEnemy);

        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ChaseEnemy()
    {
        agent.speed = chaseSpeed;
        anim.SetBool("isAttacking",false);
        agent.SetDestination(nearestEnemy.position);
        Debug.Log(nearestEnemy.name);
    }

    private void Patroling()
    {
        agent.speed = patrolSpeed;
        anim.SetBool("isChase", false);
        if(!walkPointset) SearchWalkPoint();

        if(walkPointset) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 3f)
        {
            walkPointset = false;
        }
    }

    private void SearchWalkPoint()
    {
        BoxCollider zoneCollider = zone.GetComponent<BoxCollider>();

        float randomX = Random.Range(zoneCollider.bounds.min.x + padding, zoneCollider.bounds.max.x - padding);
        float randomZ = Random.Range(zoneCollider.bounds.min.z + padding, zoneCollider.bounds.max.z - padding);

        Vector3 potentialWalkPoint = new Vector3(randomX, transform.position.y, randomZ);

        //isInside
        if (isInsideZone(zoneCollider, potentialWalkPoint))
        {
            walkPoint = potentialWalkPoint;
            walkPointset = true;
        }
        else
        {
            Debug.Log("Failed to find a valid");
        }
    }

    private void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        anim.SetBool("isIdle",false);
        anim.SetBool("isChase", true);
        anim.SetBool("isAttacking", false);
        // Debug.Log("Chase PLayer");
        agent.SetDestination(player.position);
    }

    private void AlignToSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f, whatIsGround))
        {
            Quaternion toRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 5f);
        }
    }


    private void AttackPlayer()
    {
        anim.SetBool("isAttacking", true);
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            //attack here 
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

private void FollowPlayer()
{
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    if (distanceToPlayer > 8f)
    {
        agent.isStopped = false;
        agent.speed = followSpeed;
        anim.SetBool("isIdle", false);
        anim.SetBool("isChase", false);
        anim.SetBool("isAttacking", false);
        agent.SetDestination(player.position);
    }
    else
    {
        anim.SetBool("isIdle", true);
        agent.isStopped = true;
        Vector3 lookDirection = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookDirection);
    }
}


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}

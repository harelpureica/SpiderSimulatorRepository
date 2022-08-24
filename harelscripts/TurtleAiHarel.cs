using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleAiHarel : MonoBehaviour
{ 
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;
    public Animator turtleAnimtor;
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        turtleAnimtor=GetComponent<Animator>();
        player = FindObjectOfType<RobotOneMovement>().transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        turtleAnimtor.SetBool("Walk",true);
        turtleAnimtor.SetBool("Retreat",false);

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 2f)
            walkPointSet = false;
           // StartCoroutine("waitBetweenPatrolAnimation");
    }
    /*IEnumerator  waitBetweenPatrolAnimation()
    {
        
        yield return new WaitForSeconds(2f);
    }*/
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y+1, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up,out RaycastHit hitinfo, 3f, whatIsGround))
        {
            walkPoint=hitinfo.point;
                 walkPointSet = true;
           
        }
            
    }

    private void ChasePlayer()
    {
        
Patroling();
    }

    private void AttackPlayer()
    {
        turtleAnimtor.SetBool("Retreat",true);
         turtleAnimtor.SetBool("Walk",false);

        
        
        
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

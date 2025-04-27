using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform firePoint; // FirePoint for projectile spawning
    public GameObject projectile; // Enemy's projectile prefab
    public LayerMask whatIsGround, whatIsPlayer;

    public float health = 100;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private Animator animator; // Reference to the Animator

    public float rotationSpeed = 5f; // Speed of rotation toward the player

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component

        // Set instant movement (no acceleration)
        agent.acceleration = 1000f; // Very high acceleration for instant speed
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }

        // Play running animation if moving
        UpdateAnimation();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

        // Instantly rotate toward the player
        RotateTowardsPlayer();
    }

    private void AttackPlayer()
    {
        // Stop moving and rotate toward the player
        agent.SetDestination(transform.position);
        RotateTowardsPlayer();

        // Check if the enemy is stationary
        if (IsStationary())
        {
            if (!alreadyAttacked)
            {
                // Trigger the attack animation
                animator.SetTrigger("Shoot");

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
        else
        {
            // Stop attacking if the enemy starts moving
            animator.ResetTrigger("Shoot");
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation; // Instantly set the rotation to face the player
    }

    private bool IsStationary()
    {
        // Check if the NavMeshAgent's velocity is close to zero
        return agent.velocity.magnitude <= 0.1f;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void UpdateAnimation()
    {
        // Check if the enemy is moving
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isRunning", true); // Play running animation
        }
        else
        {
            animator.SetBool("isRunning", false); // Stop running animation
        }
    }

    // This method will be called by the animation event
    public void OnShootAnimationEvent()
    {
        // Instantiate the projectile at the FirePoint
        GameObject bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);

        // Set the direction of the projectile to the enemy's forward direction
        if (bullet.TryGetComponent<Bullet>(out var bulletScript))
        {
            bulletScript.SetDirection(transform.forward); // Use the enemy's forward direction
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}


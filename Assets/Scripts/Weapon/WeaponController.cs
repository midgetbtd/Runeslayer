using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint; // The point from which bullets are fired
    public GameObject bulletPrefab; // The bullet prefab
    public float bulletForce = 20f; // Speed of the bullet
    public float attackCooldown = 1f; // Time between attacks

    private float lastAttackTime = 0f; // Tracks the last time the player attacked
    private Transform target; // The current enemy target
    private PlayerMovement playerMovement; // Reference to the player's movement script
    private bool isAttacking = false; // Tracks if the character is currently attacking

    void Start()
    {
        // Find the PlayerMovement script on the player
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Check if the player is stationary
        if (playerMovement != null && playerMovement.IsStationary())
        {
            // Find the closest enemy to target
            FindClosestEnemy();

            // If there's a target, make the player face it
            if (target != null)
            {
                FaceTarget();
            }

            // Start the attack sequence if not already attacking
            if (!isAttacking)
            {
                StartCoroutine(AttackSequence());
            }
        }
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true;

        // Wait for the attack cooldown duration before starting attacks
        float waitTime = Mathf.Max(0, (lastAttackTime + attackCooldown) - Time.time);
        yield return new WaitForSeconds(waitTime);

        while (playerMovement.IsStationary())
        {
            // Find the closest enemy to target
            FindClosestEnemy();

            // If there's a target, attack
            if (target != null)
            {
                TriggerShootingAnimation();
                lastAttackTime = Time.time;
            }

            // Wait for the attack cooldown duration
            yield return new WaitForSeconds(attackCooldown);
        }

        isAttacking = false;
    }

    void FindClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        // Find all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        target = closestEnemy;
    }

    void FaceTarget()
    {
        // Rotate the player to face the target
        Vector3 directionToEnemy = target.position - transform.position;
        directionToEnemy.y = 0; // Ignore vertical differences
        if (directionToEnemy != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void TriggerShootingAnimation()
    {
        // Trigger the shooting animation only if stationary
        if (playerMovement != null && playerMovement.animator != null && playerMovement.IsStationary())
        {
            playerMovement.animator.SetTrigger("Shoot");
        }
    }

    // This method will be called by the animation event
    public void OnShootAnimationEvent()
    {
        if (playerMovement.IsStationary())
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            if (bullet.TryGetComponent<Bullet>(out var bulletScript))
            {
                bulletScript.SetDirection(firePoint.forward); // Use firePoint's forward direction
            }
        }
    }
}


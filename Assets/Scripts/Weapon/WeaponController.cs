using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint; // The point from which bullets are fired
    public GameObject bulletPrefab; // The bullet prefab
    public float bulletForce = 20f; // Speed of the bullet
    public float attackCooldown = 1f; // Time between attacks

    private float lastAttackTime = 0f; // Tracks the last time an attack was made
    private Transform target; // The current target
    private Animator animator; // Reference to the Animator
    private bool isAttacking = false; // Tracks if the entity is currently attacking

    void Start()
    {
        // Try to get an Animator component (optional)
        animator = GetComponent<Animator>();
    }

    public void Attack(Transform target = null)
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return; // Enforce cooldown

        lastAttackTime = Time.time;

        // Trigger attack animation if available
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Fire projectile if it's a ranged attack
        if (bulletPrefab != null && firePoint != null)
        {
            FireProjectile(target);
        }
    }

    private void FireProjectile(Transform target)
    {
        // Instantiate the projectile at the fire point
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Set the direction of the projectile
        if (bullet.TryGetComponent<Bullet>(out var bulletScript))
        {
            Vector3 direction = target != null
                ? (target.position - firePoint.position).normalized
                : firePoint.forward;
            bulletScript.SetDirection(direction);
        }
    }
}


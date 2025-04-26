using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Travel speed of the bullet
    public float lifetime = 5f; // Maximum lifetime of the bullet
    private Rigidbody rb; // Reference to the Rigidbody

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody already added to the prefab

        if (rb == null)
        {
            return;
        }
    }

    void Start()
    {
        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector3 forwardDirection)
    {
        // Apply initial velocity to the Rigidbody
        rb.linearVelocity = forwardDirection.normalized * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has an EnemyAI component
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            // Apply damage to the enemy
            enemy.TakeDamage(25f); // Example damage value
        }

        // Destroy the bullet on collision
        Destroy(gameObject);
    }
}

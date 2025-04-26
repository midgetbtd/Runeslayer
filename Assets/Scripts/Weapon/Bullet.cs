using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Travel speed of the bullet
    public float lifetime = 5f; // Maximum lifetime of the bullet
    private Rigidbody rb; // Reference to the Rigidbody
    private float damage = 25f; // Default damage

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the bullet after its lifetime
    }

    public void SetDirection(Vector3 forwardDirection)
    {
        rb.linearVelocity = forwardDirection.normalized * speed;
    }

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Apply damage to the target
        if (collision.gameObject.TryGetComponent<EnemyAI>(out var enemy))
        {
            enemy.TakeDamage(damage);
        }
        else if (collision.gameObject.TryGetComponent<PlayerMovement>(out var player))
        {
            // Apply damage to the player (if needed)
            Debug.Log($"Player hit! Damage: {damage}");
        }

        Destroy(gameObject); // Destroy the bullet on collision
    }
}

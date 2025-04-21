using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Travel speed of the bullet
    public float lifetime = 5f; // Maximum lifetime of the bullet
    private Vector3 moveDirection; // Direction the bullet will travel

    void Start()
    {
        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector3 forwardDirection)
    {
        // Set the direction the bullet will travel
        moveDirection = forwardDirection.normalized;
    }

    void Update()
    {
        // Move the bullet in the set direction
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy the bullet on collision
        Destroy(gameObject);
    }
}

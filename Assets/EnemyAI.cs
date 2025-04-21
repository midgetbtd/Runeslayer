using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float speed = 5f; // Speed at which the enemy moves

    void Update()
    {
        if (player != null)
        {
            // Move the enemy towards the player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}

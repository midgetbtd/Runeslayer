using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public Transform currentTarget; // The currently targeted enemy
    public float targetingRange = 10f; // Range within which enemies can be targeted
    public LayerMask targetLayer; // Layer mask to identify targets (e.g., "Player" or "Enemy")

    void Update()
    {
        FindNearestTarget();
    }

    private void FindNearestTarget()
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, targetingRange, targetLayer);

        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider target in targetsInRange)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target.transform;
            }
        }

        currentTarget = closestTarget; // Update the current target
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the targeting range in the Scene view
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
}

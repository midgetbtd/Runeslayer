using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform target; // The target to follow
    private float posY = 3f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, -3.0f, 3.0f), // Clamp the x position between -2 and 2
            posY, // Keep the y position unchanged 
            Mathf.Clamp(target.position.z, -2.5f, 1.0f));// Clamp the y position between 0 and 5
    }
}
// The camera will follow the target's x and z position, but keep the y position fixed at 3
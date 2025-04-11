using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 4, 0);
    public Vector3 boxSize = new Vector3(2.0f, 5.0f, 5.0f); // Dimensions of the box (width, height, depth)

    private Vector3 initialPlayerPosition; // To store the initial position of the player
    private bool shouldFollow = false; // Flag to determine if the camera should follow
    private Vector3 dynamicOffset; // Dynamic offset to maintain initial camera position

    void Start()
    {
        if (target != null)
        {
            initialPlayerPosition = target.position; // Store the initial position of the player
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Check if the player is outside the box
        if (!shouldFollow && !IsPlayerWithinBox())
        {
            shouldFollow = true;
            dynamicOffset = transform.position - target.position; // Calculate dynamic offset
        }
        // Check if the player is back within the box
        else if (shouldFollow && IsPlayerWithinBox())
        {
            shouldFollow = false;
        }

        Vector3 cameraPos = transform.position;

        // Update the position only if the player is outside the box
        if (shouldFollow)
        {
            cameraPos = target.position + dynamicOffset;
        }

        transform.position = cameraPos;
    }

    // Helper method to check if the player is within the box
    private bool IsPlayerWithinBox()
    {
        Vector3 playerOffset = target.position - initialPlayerPosition;

        return Mathf.Abs(playerOffset.x) <= boxSize.x / 2 &&
               Mathf.Abs(playerOffset.y) <= boxSize.y / 2 &&
               Mathf.Abs(playerOffset.z - 1.5f) <= boxSize.z / 2;
    }
}

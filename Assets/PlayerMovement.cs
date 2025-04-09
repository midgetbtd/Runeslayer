using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Joystick movementJoystick; // Joystick for movement

    [SerializeField]
    Animator animator;

    [SerializeField]
    float moveSpeed = 5f;

    void Update()
    {
        // Movement input
        float horizontalMove = movementJoystick.Horizontal;
        float verticalMove = movementJoystick.Vertical;

        Vector3 movement = new Vector3(horizontalMove, 0, verticalMove).normalized;

        // Move the player
        if (movement.magnitude > 0.1f)
        {
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

            // Rotate the player to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500 * Time.deltaTime);

            animator.SetFloat("Speed", movement.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0f);   animator.SetFloat("Speed", 0f);
        }
    }
}

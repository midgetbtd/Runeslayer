using UnityEngine;
using System.Collections; // Add this to use IEnumerator
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Joystick joystick; // Joystick for movement
    [SerializeField] private Transform PlayerSprite;
    [SerializeField] public Animator animator; // Animator for animations
    [SerializeField] private float speed = 5f; // Speed of the player
    [SerializeField] private float dodgeDistance = 5f; // Distance covered during dodge
    [SerializeField] private float dodgeDuration = 0.2f; // Duration of the dodge
    [SerializeField] private float dodgeCooldown = 1f; // Cooldown between dodges
    [SerializeField] private Transform enemyTarget; // Reference to the enemy's Transform

    private bool isDodging = false; // Tracks if the player is currently dodging
    private bool canDodge = true; // Tracks if the player can dodge
    private Vector3 dodgeDirection; // Direction of the dodge

    private PlayerInputActions inputActions; // Reference to the Input Actions

    void Awake()
    {
        // Initialize the Input Actions
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        // Enable the Dodge action
        inputActions.Player.Dodge.performed += OnDodgePerformed;
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        // Disable the Dodge action
        inputActions.Player.Dodge.performed -= OnDodgePerformed;
        inputActions.Player.Disable();
    }

    void FixedUpdate()
    {
        if (isDodging) return; // Disable normal movement during dodge

        // Update PlayerSprite position based on joystick input
        PlayerSprite.position = new Vector3(
            joystick.Horizontal + transform.position.x,
            0.1f, // Keep the PlayerSprite slightly above the ground
            joystick.Vertical + transform.position.z
        );

        // Rotate the player to face the PlayerSprite
        transform.LookAt(new Vector3(PlayerSprite.position.x, transform.position.y, PlayerSprite.position.z));
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        // Handle movement
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            // Move the player forward
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void OnDodgePerformed(InputAction.CallbackContext context)
    {
        if (canDodge)
        {
            Debug.Log("Dodge action triggered by button!"); // Debug to confirm input is working
            StartCoroutine(Dodge());
        }
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        canDodge = false;

        // Play dodge animation
        animator.SetTrigger("Dodge");

        // Calculate dodge direction (current movement direction or forward if stationary)
        dodgeDirection = (joystick.Horizontal != 0 || joystick.Vertical != 0)
            ? new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized
            : transform.forward;

        // Apply dodge movement
        float elapsedTime = 0f;
        while (elapsedTime < dodgeDuration)
        {
            Vector3 dodgeMovement = dodgeDirection * (dodgeDistance / dodgeDuration) * Time.deltaTime;
            transform.Translate(dodgeMovement, Space.World);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDodging = false;

        // Wait for cooldown
        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }

    public bool IsStationary()
    {
        // Example logic: Check if the player is not moving
        return joystick.Horizontal == 0 && joystick.Vertical == 0;
    }

    public void TriggerDodge()
    {
        if (canDodge)
        {
            StartCoroutine(Dodge());
        }
    }
}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Joystick joystick; // Joystick for movement

    [SerializeField]
    Transform PlayerSprite;

    [SerializeField]
    Animator animator;

    [SerializeField]
    float speed = 5f; // Speed of the player


    void Update()
    {
        PlayerSprite.position = new Vector3(joystick.Horizontal + transform.position.x, 0.1f, joystick.Vertical + transform.position.z);
        
        transform.LookAt(new Vector3 (PlayerSprite.position.x, 0, PlayerSprite.position.z));

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
}

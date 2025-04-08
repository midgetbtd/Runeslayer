using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Joystick joystick;

    [SerializeField]
    Transform PlayerSprite;

    [SerializeField]
    Animator animator;

    bool Movement;

    void Start()
    {
        
    }
    void Update()
    {
        PlayerSprite.position = new Vector3(joystick.Horizontal + transform.position.x, 0.1f,  PlayerSprite.position.y + joystick.Vertical + transform.position.z);
        
        transform.LookAt(new Vector3 (PlayerSprite.position.x, 0, PlayerSprite.position.z));

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        float speed = new Vector2(joystick.Horizontal, joystick.Vertical).magnitude;

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
            animator.SetFloat("Speed", 0.67f);

            Movement = true;
        }
        else
        {
            if (Movement == true)
            {
                animator.SetFloat("Speed", 0);
                Movement = false;
            }
        }   
    }

}

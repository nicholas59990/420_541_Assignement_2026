using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator; // plays the animations
    private CharacterMovement movement; // our movement script
    private Rigidbody rb; // To read the player's speed

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Send the current speed to the blend tree
        Vector3 characterSpeed = rb.linearVelocity;
        characterSpeed.y = 0;
        animator.SetFloat("CharacterSpeed", characterSpeed.magnitude);

        // On the Ground? (controls Falling and ends the flip)
        bool grounded = movement.IsGrounded;
        animator.SetBool("IsGrounded", grounded);

        // When Fire1 (left mouse button / left Ctrl) is released, roll
        if (Input.GetButtonUp("Fire1"))
        {
            animator.SetTrigger("doRoll");
        }

        if (grounded)
        {
            // Thorw away a double jump request that was never used
            animator.ResetTrigger("doDoubleJump");
        }
        else
        {
            // Double Jump. Space again in mid-air
            // The flip only plays if CharacterMovement really jumped
            if (Input.GetButtonDown("Jump") && movement.TryDoubleJump())
            {
                animator.SetTrigger("doDoubleJump");
            }
        }
    }
}

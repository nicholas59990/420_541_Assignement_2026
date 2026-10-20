using UnityEngine;
// Moves the player with a Rigidbody: 
// A/D or Left/Right arrows to ROTATE, W/S or Up/Down to move Forward/Backward, Space to jump.
[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;      // units per second
    [SerializeField] private float sprintSpeed = 14f;   // units per second
    [SerializeField] private float turnSpeed = 150f;    // degrees per second
    [SerializeField] private float jumpForce = 5f;      // strength of the jump (Min Jump Force)
    [SerializeField] private float maxJumpForce = 12f;  // Strongest jump force (after 3 seconds)
    [SerializeField] private float maxJumpChargeTime = 3f; // Max time required to charge the jump
   [SerializeField] private float doubleJumpForce = 4f;
    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f; // ray length
    [SerializeField] private LayerMask groundMask;        // ground layers

    private Rigidbody rb;               // the physics body we move
    private Vector3 moveDirection;      // worked out from input in Update()
    private float jumpHoldTime = 0f;    // Track how long Space has been held
    private bool isGrounded;            // true when standing on the ground
    private bool jumpRequested = false; // set in Update(), used later
    private bool doubleJumpRequested = false; // set in TryDoubleJump()
    private bool hasDoubleJumped = false;     // one per jump
    private bool isChargingJump = false; // Tracks whether player is currently charging a jump
    private PlayerPowerUps powerUps; // Used to check if the double jump is unlocked

    public bool IsGrounded => isGrounded;

    // Multiplies the normal speed. 1 = normal, 1.5 = 50% faster
    // Other scripts (like PlayerPowerUps) are allowed to change it
    public float SpeedMultiplier {get; set; } = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        powerUps = GetComponent<PlayerPowerUps>();
    }

    private void Update()
    {
        // 1. Ground Check
        isGrounded = Physics.Raycast(  transform.position + transform.up*groundDistance/2, -transform.up, groundDistance, groundMask);

        // Back on the ground: the double jump is available again
        if (isGrounded)
        {
            hasDoubleJumped = false;
        }


        // 2. Read Inputs
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right turn input
        float moveZ = Input.GetAxisRaw("Vertical");  // W/S or Up/Down forward/backward


        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        // Rotate the player transform in Update for responsive turning visuals
        // Calculate forward movement relative to current facing direction
        moveDirection = cameraForward * moveZ + cameraRight * moveX;

        if (moveDirection.magnitude > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                targetRotation, 
                turnSpeed * Time.deltaTime);
        }

        // 3. Handle Jump Input
        // Start changing when Space is pressed while grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isChargingJump)
        {
            isChargingJump = true;
            jumpHoldTime = 0f; // start the charge timer at zero
        }

        // Increase charge time while Space is held
        if (isChargingJump && Input.GetKey(KeyCode.Space))
        {
            jumpHoldTime += Time.deltaTime;

            // If the max charge time is reached jump automatically
            if (jumpHoldTime >= maxJumpChargeTime)
            {
                jumpHoldTime = maxJumpChargeTime; // do not change beyond the max
                jumpRequested = true; // request a jump in FixedUpdate
                isChargingJump = false; // Stop charging
            }
        }

        // Jump immediately if Space is released early
        if (isChargingJump && Input.GetKeyUp(KeyCode.Space))
        {
            jumpRequested = true;
            isChargingJump = false;
        }

        // Sprint while Shift is held and the players is grounded
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isGrounded)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = 7f;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
 
        // Calculate jump strength from the time Space was held
        if (jumpRequested)
        {
            // Convert charge time to a value between 0 - 1
            // 0 = No Charge & 1 = Max Charge
            float charge = Mathf.Clamp01(jumpHoldTime / maxJumpChargeTime);

            // Interpolate between the minimum and maximum jump forces.
            // Short hold = closer to jumpForce; long hold = closer to maxJumpForce.
            // Mathf.Lerp is Unity's function for calculating this value.
            float currentJumpForce = Mathf.Lerp(jumpForce, maxJumpForce, charge);

            // Perform the jump using the calculated force
            Jump(currentJumpForce);

            // Reset the request so it is not processed repeatedly
            jumpRequested = false;

            // Reset the charge timer for the next jump
            jumpHoldTime = 0f;
        }
 
        // The second jump, in mid-air
        if (doubleJumpRequested)
        {
            Jump(doubleJumpForce);
            doubleJumpRequested = false;
        }
    }
 
    private void Jump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f,
                                        rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.VelocityChange);
    }


    private void MovePlayer()
    {
        // Calculate velocity based on current forward vector
        Vector3 targetVelocity = moveDirection * moveSpeed * SpeedMultiplier;

        // Apply movement while preserving vertical velocity (gravity/jumping)
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

     // Tries to jump a second time in mid-air.
    // Returns true if the double jump really happened.
    public bool TryDoubleJump()
    {

        // Locked unless the blue pickup is active
        if (powerUps == null || !powerUps.DoubleJumpActive) return false;

        // Not allowed on the ground, or if it was already used
        if (isGrounded || hasDoubleJumped) return false;
 
        doubleJumpRequested = true;
        hasDoubleJumped = true;

        // Play the double jump sound
        powerUps.PlayDoubleJumpSound();
        return true;
    }
}

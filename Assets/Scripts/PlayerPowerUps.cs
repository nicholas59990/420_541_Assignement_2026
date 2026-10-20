using UnityEngine;

// Gives the player a speed boost for a limited time
public class PlayerPowerUps : MonoBehaviour
{

    [SerializeField] private float boostMultiplier = 1.5f; // 50% faster
    [SerializeField] private AudioClip doubleJumpSound;

    // Seconds of boost left. The HUD reads it; only this script changes it
    public float SpeedTimeLeft {get; private set;}
    public float ScoreTimeLeft {get; private set;}
    public float ScoreMultiplier {get; private set;} = 1;

    private CharacterMovement movement;
    private float doubleJumpEndTime;

    public bool DoubleJumpActive => Time.time < doubleJumpEndTime;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
    }

    // Activates boost
    public void ActivateSpeedBoost(float duration) 
    {
        SpeedTimeLeft = duration;
        movement.SpeedMultiplier = boostMultiplier;
    }

    // Activates double points
    public void ActivateScoreBoost(float duration)
    {
        ScoreTimeLeft = duration;
        ScoreMultiplier = 2;
    }

    // Activates 30 Sec Jump
    public void ActivateDoubleJump(float duration)
    {
        doubleJumpEndTime = Time.time + duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpeedTimeLeft <= 0f) return; // No boost running

        SpeedTimeLeft -= Time.deltaTime;

        if (SpeedTimeLeft <= 0f) 
        {
            movement.SpeedMultiplier = 1f; // Back to normal speed
        }

    if (DoubleJumpActive && Input.GetKeyDown(KeyCode.Space))
    {
        if (movement.TryDoubleJump() && doubleJumpSound != null)
        {
            AudioSource.PlayClipAtPoint(doubleJumpSound, transform.position);
        }
    }

        // Count down the score boost
        if (ScoreTimeLeft > 0f)
        {
            ScoreTimeLeft -= Time.deltaTime;

            if (ScoreTimeLeft <= 0f)
            {
                ScoreTimeLeft = 0f;
                ScoreMultiplier = 1;
            }
        }
    }
}

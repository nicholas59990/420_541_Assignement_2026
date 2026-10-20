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
    private float doubleJumpEndTime; // game time when the double jump runs out

    // Is the double jump unlocked right now? (true = yes, false = no)
    // It checks the clock every time another script asks.
    // Time.time = seconds since the game started
    // doubleJumpEndTime = the moment the power up runs out
    // Still before that moment = true. After it = false.
    public bool DoubleJumpActive => Time.time < doubleJumpEndTime;

    private void Awake()
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

    // Called by CharacterMovement when a double jump really happens
    public void PlayDoubleJumpSound()
    {
    if (doubleJumpSound != null)
        AudioSource.PlayClipAtPoint(doubleJumpSound, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (SpeedTimeLeft > 0f) // No boost running
        {
            SpeedTimeLeft -= Time.deltaTime;

            if (SpeedTimeLeft <= 0f) 
            {
                SpeedTimeLeft = 0f;
                movement.SpeedMultiplier = 1f; // Back to normal speed
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

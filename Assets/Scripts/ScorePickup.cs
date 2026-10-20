using UnityEngine;

// A coin that adds points to the score when the player touches it.
// Needs a collider with Is Trigger ticked on the same GameObject.
public class ScorePickup : MonoBehaviour
{
    // How many points this coin is worth (change it in the Inspector)
    [SerializeField] private int points = 10;

    // Sound played when the coin is collected
    [SerializeField] private AudioClip pickupSound;

    // Called automatically when something enters this trigger
    private void OnTriggerEnter(Collider other)
    {
        // Only react to the player, ignore everything else
        if (other.CompareTag("Player"))
        {
            // Get the player's power ups
            PlayerPowerUps power = other.GetComponent<PlayerPowerUps>();

            // Use double points if boost active, otherwise normal points
            int mult = 1;

            if (power != null)
            {
                mult = Mathf.RoundToInt(power.ScoreMultiplier);
            }

            // Add the points to the score
            ScoreManager.Instance.AddScore(points * mult);

            // Play the pickup sound where the coin was
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Remove the coin from the scene: it's collected once and gone
            Destroy(gameObject);
        }
    }
}
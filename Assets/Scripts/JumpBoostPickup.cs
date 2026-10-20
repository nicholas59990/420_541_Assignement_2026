using UnityEngine;

// A blue pickup. When the player touches it, they get the double jump
// for a few seconds, then the pickup hides and comes back later
public class JumpBoostPickup : MonoBehaviour
{
    [SerializeField] private float duration = 30f;     // how long the double jump lasts
    [SerializeField] private float respawnDelay = 5f;  // time hidden before it comes back
    [SerializeField] private GameObject visuals;       // child object to hide
    [SerializeField] private AudioClip pickupSound;    // sound when collected
    [SerializeField] private GameObject pickupEffect;  // particle prefab

    // Called automatically when something enters this trigger
    private void OnTriggerEnter(Collider other)
    {
        // Only the player has a PlayerPowerUps component, so ignore everything else
        PlayerPowerUps powerUps = other.GetComponent<PlayerPowerUps>();
        if (powerUps == null) return;

        // Give the player the double jump
        powerUps.ActivateDoubleJump(duration);

        // Play the sound (if there is an Audio Source and a clip)
        if (TryGetComponent(out AudioSource audio) && pickupSound != null)
            audio.PlayOneShot(pickupSound);

        // Spawn the particles (if a prefab is assigned)
        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        // Hide the pickup and turn off the collider so it can't be collected again
        if (visuals != null) visuals.SetActive(false);
        GetComponent<Collider>().enabled = false;

        // Call Reappear() after respawnDelay seconds
        Invoke(nameof(Reappear), respawnDelay);
    }

    // Brings the pickup back after the delay
    private void Reappear()
    {
        if (visuals != null) visuals.SetActive(true);
        GetComponent<Collider>().enabled = true;
    }
}

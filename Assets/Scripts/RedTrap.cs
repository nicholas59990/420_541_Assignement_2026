using UnityEngine;

// Damages the player when they touch the red trap
public class RedTrap : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterHealth characterHealth = collision.gameObject.GetComponent<CharacterHealth>();

            if (characterHealth != null)
            {
                characterHealth.TakeDamage();
            }
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the player's health
public class CharacterHealth : MonoBehaviour
{

    public int health = 3; 

    // Called when character touches trap
    public void TakeDamage()
    {
        health--;

        Debug.Log("Health: " + health);
        
        // Restart level if health reaches zero since that means you died
        if (health <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

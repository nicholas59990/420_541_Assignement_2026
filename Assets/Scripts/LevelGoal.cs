using UnityEngine;
using UnityEngine.SceneManagement;

// Loads the next level when the player reaches the goal
public class levelGoal : MonoBehaviour
{

    [SerializeField] private string nextSceneName;
    [SerializeField] private bool isFinalGoal;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Save the score before opening the End Screen
        if (isFinalGoal)
        {
            ScoreManager.Instance.SaveFinalScore();
        }
        
        // Load the next level or the End Screen
        SceneManager.LoadScene(nextSceneName);
    }
}

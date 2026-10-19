using UnityEngine;
using UnityEngine.SceneManagement; // Needed for SceneManager

// Loads scenes by name. Buttons call LoadScene() and QuitGame()
// from their On Click list.
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        // Tells Editor game is not playing so stop 
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

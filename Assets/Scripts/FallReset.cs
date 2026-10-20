using UnityEngine;
using UnityEngine.SceneManagement;

// Restarts the level if the player falls off the map
public class FallReset : MonoBehaviour
{
   [SerializeField] private float fallHeight = -10f;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < fallHeight)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

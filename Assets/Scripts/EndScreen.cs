using UnityEngine;
using TMPro;

// Shows the score the player finished the level with
public class EndScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finalScoreText.text = "Final Score: " + ScoreManager.FinalScore;
    }
}

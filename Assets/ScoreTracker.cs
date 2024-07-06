using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreTracker : MonoBehaviour
{
    private float _score;
    private float _levelHighScore;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelHighScoreText;

    private void Awake()
    {
        _levelHighScore = PlayerPrefs.GetFloat($"{SceneManager.GetActiveScene().name} high score", 0);
        levelHighScoreText.text = "LEVEL HIGH SCORE: " + _levelHighScore.ToString("0.00");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Collectable")) return;
        
        _score += other.GetComponent<CollectableComponent>().scoreAmount;
        scoreText.text = "SCORE: " + _score.ToString("0.00");

        if (_score > _levelHighScore)
        {
            _levelHighScore = _score;
            levelHighScoreText.text = "LEVEL HIGH SCORE: " + _levelHighScore.ToString("0.00");

            PlayerPrefs.SetFloat($"{SceneManager.GetActiveScene().name} high score", _score);
            PlayerPrefs.Save();
        }
            
        Destroy(other.gameObject);
    }
}

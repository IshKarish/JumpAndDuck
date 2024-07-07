using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    private float _score;
    private float _levelHighScore;
    
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Collectable")) return;
        
        _score += other.GetComponent<CollectableComponent>().scoreAmount;
        scoreText.text = "SCORE: " + _score.ToString("0.00");

        if (_score > _levelHighScore)
        {
            _levelHighScore = _score;
        }
            
        Destroy(other.gameObject);
    }
}

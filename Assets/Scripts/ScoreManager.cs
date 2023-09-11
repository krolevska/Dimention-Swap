using UnityEngine;
using TMPro; // Include TextMeshPro namespace

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI text;
    public int necessaryScore = 50;

    public int health;
    public TextMeshProUGUI healthText;

    private void UpdateHealthText()
    {
        healthText.text = "Health: " + health;
    }

    public void DecreaseHealth(float damage)
    {
        health -= (int)damage;
        UpdateHealthText();

        if (health <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    // Static singleton instance
    public static ScoreManager instance;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Reset the score.
        score = 0;
        health = 20;
        UpdateHealthText();
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        text.text = "Score: " + score;
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScoreText(); // Update the score text when the score changes

        if (score == necessaryScore)
        {
            GameManager.instance.EndLevel();
        }
    }
}

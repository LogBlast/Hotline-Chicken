using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text playerHealthText;
    public Text scoreText;
    public Text timeText;

    private float playerHealth = 100;
    private int score = 0;
    private float timeElapsed = 0f;
    private string resultTime;

    void Start()
    {
        UpdatePlayerHealthUI();
        UpdateScoreUI();
    }

    void Update()
    {
        UpdateTimeUI();
    }

    public void SetPlayerHealth(float health)
    {
        playerHealth = health;
        UpdatePlayerHealthUI();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdatePlayerHealthUI()
    {
        playerHealthText.text = "Health: " + playerHealth.ToString();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void UpdateTimeUI()
    {
        timeElapsed += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeElapsed / 60F);
        int seconds = Mathf.FloorToInt(timeElapsed % 60F);
        resultTime = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        timeText.text = resultTime;
    }

    public int GetScore()
    {
        return this.score;
    }

    public string GetTime()
    {
        return this.resultTime;
    }

}

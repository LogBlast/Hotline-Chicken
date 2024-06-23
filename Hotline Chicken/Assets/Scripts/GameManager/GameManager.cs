using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;

    private int enemiesAlive;

    void Start()
    {
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void DecreaseEnemyCount()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        // Transmettre les données au menu de fin via une classe statique
        EndGameData.Score = uiManager.GetScore();
        EndGameData.TimeElapsed = uiManager.GetTime();

        // Charger la scène du menu de fin
        SceneManager.LoadScene("EndMenu");
    }
}

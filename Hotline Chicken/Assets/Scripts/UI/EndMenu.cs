using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndMenu : MonoBehaviour
{
    public TMP_Text endScoreText;
    public TMP_Text endTimeText;

    void Start()
    {
        // Afficher les données du jeu terminé
        endScoreText.text = "Score : " + EndGameData.Score;
        endTimeText.text = "Time : " + EndGameData.TimeElapsed;
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Retour au menu principal");
    }
}

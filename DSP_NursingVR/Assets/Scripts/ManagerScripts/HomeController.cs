using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the loading of the high scores to be displayed
/// </summary>
public class HomeController : MonoBehaviour
{
    private const string LEVEL_01 = "Level_01_Ward";

    /// <summary>
    /// COnstructor
    /// </summary>
    void Start () {
		DisplayRecentScore();
        DisplayHighScores();
	}

    /// <summary>
    /// Display the most recent score, retrieved from the player prefs controller
    /// </summary>
    private void DisplayRecentScore() {
        GameObject text = GameObject.Find("RecentScoresText");
        text.GetComponent<TextMesh>().text += "\n" + FindObjectOfType<PlayerPrefsController>().GetPlayerScore();
    }

    /// <summary>
    /// Gets a list of the high scores from the player prefs controller and iterates through diaplying the leaderboard
    /// </summary>
    private void DisplayHighScores() {
        GameObject text = GameObject.Find("HighScoresText");
        List<float> scores = FindObjectOfType<PlayerPrefsController>().GetHighScores();
        for (int i = 0; i < scores.Count; i++) {
            text.GetComponent<TextMesh>().text += "\n" + (i + 1) + ". " + scores[i];
        }
    }

    /// <summary>
    /// Loads the first scene
    /// </summary>
    public void LoadGame() {
        SceneManager.LoadScene(LEVEL_01);
    }
}
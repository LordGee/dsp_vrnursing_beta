using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{

    private const string LEVEL_01 = "Level_01_Ward";


    void Start () {
		DisplayRecentScore();
        DisplayHighScores();
	}

    private void DisplayRecentScore()
    {
        GameObject text = GameObject.Find("RecentScoresText");
        text.GetComponent<TextMesh>().text += "\n" + FindObjectOfType<PlayerPrefsController>().GetPlayerScore();
    }

    private void DisplayHighScores()
    {
        GameObject text = GameObject.Find("HighScoresText");
        List<float> scores = FindObjectOfType<PlayerPrefsController>().GetHighScores();
        for (int i = 0; i < scores.Count; i++)
        {
            if (scores[i] != null)
            {
                text.GetComponent<TextMesh>().text += "\n" + (i + 1) + ". " + scores[i];
            }
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(LEVEL_01);
    }
	


}

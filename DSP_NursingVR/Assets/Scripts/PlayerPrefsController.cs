using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{

    /* Current Player */
    private const string CURRENT_SCORE = "current_score";
    private const string CURRENT_PLAYER = "current_player";

    /* High Score Board */
    private const string HIGH_SCORE_ARRAY = "high_score_";
    private const int NO_OF_SCORES = 5;
   
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey(CURRENT_SCORE))
        {
            PlayerPrefs.SetFloat(CURRENT_SCORE, 0);
        }
    }

    /* Set values */
    public void SetPlayerScore(float _value)
    {
        PlayerPrefs.SetFloat(CURRENT_SCORE, _value);
        AddNewScoreToHighScore(_value);
    }

    public void SetPlayerName(string _value)
    {
        PlayerPrefs.SetString(CURRENT_PLAYER, _value);
    }
    

    /* Get values */
    public float GetPlayerScore() { return PlayerPrefs.GetFloat(CURRENT_SCORE); }
    public string GetPlayerName() { return PlayerPrefs.GetString(CURRENT_PLAYER); }

    public List<float> GetHighScores()
    {
        List<float> scores = new List<float>();
        for (int i = 0; i < NO_OF_SCORES; i++)
        {
            if (PlayerPrefs.HasKey(HIGH_SCORE_ARRAY + (i + 1)))
            {
                scores.Add(PlayerPrefs.GetFloat(HIGH_SCORE_ARRAY + (i + 1)));
            }
        }
        scores.Sort();
        scores.Reverse();
        return scores;
    }

    private void AddNewScoreToHighScore(float _testScore)
    {
        bool isAvailableSpace = false;      
        for ( int i = 0; i < NO_OF_SCORES; i++ ) {
            if ( !PlayerPrefs.HasKey(HIGH_SCORE_ARRAY + (i + 1)) ) {
                isAvailableSpace = true;
                PlayerPrefs.SetFloat(HIGH_SCORE_ARRAY + (i + 1), _testScore);
                break;
            }
        }
        if (!isAvailableSpace) {
            int lowestIndex = -1;
            float lowestScore = 0;
            for ( int i = 0; i < NO_OF_SCORES; i++ ) {
                float thisTest = PlayerPrefs.GetFloat(HIGH_SCORE_ARRAY + (i + 1));
                if ( thisTest < _testScore) {
                    if (thisTest < lowestScore || lowestScore == 0f) {
                        lowestScore = thisTest;
                        lowestIndex = (i + 1);
                    }
                }
            }
            if (lowestIndex != -1) {
                PlayerPrefs.SetFloat(HIGH_SCORE_ARRAY + lowestIndex, _testScore);
            }
        }
    }

}

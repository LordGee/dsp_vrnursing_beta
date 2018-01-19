using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the saving and loading of variable values that are to be stored on the users local machine
/// </summary>
public class PlayerPrefsController : MonoBehaviour
{
    private const string CURRENT_SCORE = "current_score";
    private const string HIGH_SCORE_ARRAY = "high_score_";
    private const int NO_OF_SCORES = 5;
   
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Constructor - If first time played on local machine sets the initial current score to zero to avoid any errors later
    /// </summary>
    void Start() {
        if (!PlayerPrefs.HasKey(CURRENT_SCORE)) {
            PlayerPrefs.SetFloat(CURRENT_SCORE, 0);
        }
    }

    /// <summary>
    /// Sets the Players previous score
    /// Also calls function to potentially add that score to the high score leaderboard
    /// </summary>
    /// <param name="_value">Score submitted</param>
    public void SetPlayerScore(float _value) {
        PlayerPrefs.SetFloat(CURRENT_SCORE, _value);
        AddNewScoreToHighScore(_value);
    }   

    /// <summary>
    /// Returns the users most recent / previous score
    /// </summary>
    /// <returns>Last Score</returns>
    public float GetPlayerScore() { return PlayerPrefs.GetFloat(CURRENT_SCORE); }

    /// <summary>
    /// Get all high scores from various player prefs
    /// Iterates through the high score naming convention to populate a list of the top 5 high scores.
    /// The list is then sorted and reversed before being passed back to the calling function.
    /// </summary>
    /// <returns>List of high scores</returns>
    public List<float> GetHighScores() {
        List<float> scores = new List<float>();
        for (int i = 0; i < NO_OF_SCORES; i++) {
            if (PlayerPrefs.HasKey(HIGH_SCORE_ARRAY + (i + 1))) {
                scores.Add(PlayerPrefs.GetFloat(HIGH_SCORE_ARRAY + (i + 1)));
            }
        }
        scores.Sort();
        scores.Reverse();
        return scores;
    }
    /// <summary>
    /// Adds new scores to the high score player prefs if applicable
    /// First iterates through to check if the leader board is already full
    /// If not the score can take available slot.
    /// Else the score then iteracts through again testing if it is higher then any of the previously stored values
    /// he lowest score index is recorded and if the player is greater that index will be overwritten with the new score.
    /// </summary>
    /// <param name="_testScore">Score to test</param>
    private void AddNewScoreToHighScore(float _testScore) {
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

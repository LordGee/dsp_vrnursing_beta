using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

/// <summary>
/// Core of the game, the game controller handles all the games states, player variables, timers.
/// </summary>
public class GameController : MonoBehaviour
{
    public AudioClip[] endGameStates;
    private ConstantController.GAME_STATE currentGameState;
    private float hydrationLevel, hydrationTimer;
    private float hungerLevel, hungerTimer;
    private float gameTimer, timerInterval, gameScore, finishTimer;
    private bool waterSpawned, foodSpawned, winCondition, once = false;
    private float alpha, maxAlpha = 0.9f;
    private const string LEVEL_00 = "Level_00_Home";
    
    /// <summary>
    /// Constructor - Sets the default values to start the game
    /// </summary>
    private void Start() {
        currentGameState = ConstantController.GAME_STATE.Brief;
        hydrationTimer = hungerTimer = 0f;
        CanvasController.gameHydration = hydrationLevel = ConstantController.HYDRATION_MAX;
        CanvasController.gameEnergy = hungerLevel = ConstantController.HUNGER_MAX;
        EventController.TriggerEvent(ConstantController.EV_SPAWN_FOOD);
        EventController.TriggerEvent(ConstantController.EV_SPAWN_WATER);
        EventController.TriggerEvent(ConstantController.EV_SPAWN_HAZARD);
        gameScore = 0f;
        winCondition = true;
        gameTimer = timerInterval = ConstantController.GAME_TIME;
        UpdateGameScore(gameScore);
        UpdateGameTimer();
        VRTK_BasicTeleport.BlinkColourColor = Color.clear;
    }

    /// <summary>
    /// Called every frame
    /// </summary>
    private void Update() {
        gameTimer -= Time.deltaTime;
        if ( Mathf.Floor(timerInterval) - Mathf.Floor(gameTimer) >= 1f ) {
            timerInterval = gameTimer;
            UpdateGameTimer();
        }
        switch ( currentGameState ) {
            case ConstantController.GAME_STATE.Brief:
                UpdateBrief();
                break;
            case ConstantController.GAME_STATE.Playing:
                UpdatePlaying();
                break;
            case ConstantController.GAME_STATE.GameOver:
                UpdateGameOver();
                break;
            default:
                Debug.LogError("No active game state!");
                break;
        }
    }

    /// <summary>
    /// After eating an apple you hunger levels are replenished a new object is spawned after a period of time
    /// </summary>
    public void ReplenishHungerLevel() {
        CanvasController.gameEnergy = hungerLevel = ConstantController.HUNGER_MAX;
        hungerTimer = 0f;
        AdjustFadeColor();
        StartCoroutine(DelayNewSpawn(ConstantController.EV_SPAWN_FOOD, 30f));
    }

    /// <summary>
    /// After drinking your hydration levels are replenished a new object is spawned after a period of time
    /// </summary>
    public void ReplenishHydrationLevel() {
        CanvasController.gameHydration = hydrationLevel = ConstantController.HYDRATION_MAX;
        hydrationTimer = 0f;
        AdjustFadeColor();
        StartCoroutine(DelayNewSpawn(ConstantController.EV_SPAWN_WATER, 20f));
    }

    /// <summary>
    /// Spawn a new hazard after a period of time
    /// </summary>
    public void SpawnHazard() {
        StartCoroutine(DelayNewSpawn(ConstantController.EV_SPAWN_HAZARD, 45f));
    }

    /// <summary>
    /// A coroutine that spawns a new object afte a set amount of time
    /// </summary>
    /// <param name="_spawn">Object to spawn</param>
    /// <param name="_delay">Amount of time to delay</param>
    /// <returns></returns>
    private IEnumerator DelayNewSpawn(string _spawn, float _delay) {
        yield return new WaitForSeconds(_delay);
        EventController.TriggerEvent(_spawn);
    }

    /// <summary>
    /// As hydration and hunger deminish the alpha value of a fade colour increase to deminish the players vision
    /// </summary>
    private void AdjustFadeColor() {
        float increment = maxAlpha / (ConstantController.HUNGER_MAX + ConstantController.HYDRATION_MAX);
        alpha = 0f;
        alpha += (ConstantController.HUNGER_MAX - hungerLevel) * increment;
        alpha += (ConstantController.HYDRATION_MAX - hydrationLevel) * increment;
        if ( alpha > maxAlpha ) {
            alpha = maxAlpha;
        }
        VRTK_BasicTeleport.BlinkColourColor = new Color(0.2f, 0f, 0F, alpha);
        VRTK_SDK_Bridge.HeadsetFade(VRTK_BasicTeleport.BlinkColourColor, 0.1f);
    }

    /// <summary>
    /// Obsolete function - moved to task1.cs
    /// </summary>
    private void UpdateBrief() {
        currentGameState = ConstantController.GAME_STATE.Playing;
    }

    /// <summary>
    /// While the game state is set playing, this is the function that gets executed every frame
    /// It checks the condition of the player and deminishes the hydration and hunger levels when required
    /// </summary>
    private void UpdatePlaying() {
        if ( Time.timeSinceLevelLoad < ConstantController.GAME_TIME ) {
            if ( hydrationLevel > 0 ) {
                hydrationTimer += Time.deltaTime;
                if ( hydrationTimer > ConstantController.HYDRATION_DECREASE_TIME ) {
                    hydrationLevel -= 1f;
                    AdjustFadeColor();
                    CanvasController.gameHydration = hydrationLevel;
                    hydrationTimer = 0f;
                }
            } else {
                winCondition = false;
                GetComponent<AudioSource>().clip = endGameStates[0];
                GetComponent<AudioSource>().Play();
                currentGameState = ConstantController.GAME_STATE.GameOver;
            }
            if ( hungerLevel > 0 ) {
                hungerTimer += Time.deltaTime;
                if ( hungerTimer > ConstantController.HUNGER_DECREASE_TIME ) {
                    hungerLevel -= 1f;
                    AdjustFadeColor();
                    CanvasController.gameEnergy = hungerLevel;
                    hungerTimer = 0f;
                }
            } else {
                winCondition = false;
                GetComponent<AudioSource>().clip = endGameStates[1];
                GetComponent<AudioSource>().Play();
                currentGameState = ConstantController.GAME_STATE.GameOver;
            }
        } else {
            winCondition = false;
            currentGameState = ConstantController.GAME_STATE.GameOver;
        }
    }

    /// <summary>
    /// When the main task has been completed this function is called to set the game state and the win condition
    /// </summary>
    public void FinalTaskComplete() {
        winCondition = true;
        currentGameState = ConstantController.GAME_STATE.GameOver;
    }

    /// <summary>
    /// Final function to end the game, checking if the win condition is true adds additional bonus points
    /// Then starts a coroutine to load the home scene once any voice audio has finished
    /// </summary>
    private void UpdateGameOver() {
        if (!once) {
            if ( winCondition ) {
                GetComponent<AudioSource>().clip = endGameStates[2];
                GetComponent<AudioSource>().Play();
                gameScore += Mathf.Ceil(gameTimer);
            }
            once = !once;
            if (GetComponent<AudioSource>().isPlaying)
                finishTimer = GetComponent<AudioSource>().clip.length;
            else
                finishTimer = 1f;
            StartCoroutine(LoadMainMenu(finishTimer));
        }
    }

    /// <summary>
    /// Returns to the home scene after saving the players score via the player prefs controller
    /// </summary>
    /// <param name="_length">Time before loading the home scene</param>
    /// <returns></returns>
    private IEnumerator LoadMainMenu(float _length) {
        yield return new WaitForSeconds(_length);
        FindObjectOfType<PlayerPrefsController>().SetPlayerScore(gameScore);
        SceneManager.LoadScene(LEVEL_00);
    }

    /// <summary>
    /// Updates the players score, and updates the status canvas
    /// </summary>
    /// <param name="_score">Points to be added to the players score</param>
    public void UpdateGameScore(float _score) {
        gameScore += Mathf.Ceil(_score);
        CanvasController.gameScore = gameScore;
    }

    /// <summary>
    /// Updates the main game timer. 
    /// </summary>
    public void UpdateGameTimer() {
        CanvasController.gameTimer = gameTimer;
        EventController.TriggerEvent(ConstantController.EV_UPDATE_STATUS_CANVAS);
    }

    /// <summary>
    /// GETTERS
    /// </summary>
    
    public float GetGameTimer() { return gameTimer; }
    public float GetHydrationLevel() { return hydrationLevel; }
    public float GetHungerLevel() { return hungerLevel; }

    /// <summary>
    /// EVENTS
    /// </summary>

    void OnEnable() {
        EventController.StartListening(ConstantController.EV_UPDATE_SCORE, UpdateGameScore);
        EventController.StartListening(ConstantController.EV_DRINK, ReplenishHydrationLevel);
        EventController.StartListening(ConstantController.EV_EAT, ReplenishHungerLevel);
        EventController.StartListening(ConstantController.EV_HAZARD_REMOVED, SpawnHazard);
    }

    void OnDisable() {
        EventController.StopListening(ConstantController.EV_UPDATE_SCORE, UpdateGameScore);
        EventController.StopListening(ConstantController.EV_DRINK, ReplenishHydrationLevel);
        EventController.StopListening(ConstantController.EV_EAT, ReplenishHungerLevel);
        EventController.StopListening(ConstantController.EV_HAZARD_REMOVED, SpawnHazard);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRTK;

public class GameController : MonoBehaviour
{
    public AudioClip[] endGameStates;

    private ConstantController.GAME_STATE currentGameState;
    private float hydrationLevel, hydrationTimer;
    private float hungerLevel, hungerTimer;
    private float gameTimer, timerInterval, gameScore;
    private bool waterSpawned, foodSpawned, winCondition;
    private float alpha;
    private float maxAlpha = 0.9f;
    private const string LEVEL_00 = "Level_00_Home";

    public float GetHydrationLevel() { return hydrationLevel; }
    public float GetHungerLevel() { return hungerLevel; }


    private void Start()
    {
        currentGameState = ConstantController.GAME_STATE.Brief;
        hydrationTimer = hungerTimer = 0f;
        CanvasController.gameHydration = hydrationLevel = ConstantController.HYDRATION_MAX;
        CanvasController.gameEnergy = hungerLevel = ConstantController.HUNGER_MAX;
        EventController.TriggerEvent(ConstantController.EV_SPAWN_FOOD);
        EventController.TriggerEvent(ConstantController.EV_SPAWN_WATER);
        EventController.TriggerEvent(ConstantController.EV_SPAWN_HAZARD);
        gameScore = 0f;
        winCondition = true;
        gameTimer = timerInterval = 300f;
        UpdateGameScore(gameScore);
        UpdateGameTimer();
        VRTK_BasicTeleport.BlinkColourColor = Color.clear;
    }

    private void Update()
    {
        gameTimer -= Time.deltaTime;
        if ( Mathf.Floor(timerInterval) - Mathf.Floor(gameTimer) >= 1f )
        {
            timerInterval = gameTimer;
            UpdateGameTimer();
        }
        switch ( currentGameState )
        {
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

    public void ReplenishHungerLevel()
    {
        CanvasController.gameEnergy = hungerLevel = ConstantController.HUNGER_MAX;
        hungerTimer = 0f;
        AdjustFadeColor();
        StartCoroutine(DelayNewSpawn(ConstantController.EV_SPAWN_FOOD, 30f));
    }

    public void ReplenishHydrationLevel()
    {
        CanvasController.gameHydration = hydrationLevel = ConstantController.HYDRATION_MAX;
        hydrationTimer = 0f;
        AdjustFadeColor();
        StartCoroutine(DelayNewSpawn(ConstantController.EV_SPAWN_WATER, 20f));
    }

    public void SpawnHazard()
    {
        StartCoroutine(DelayNewSpawn(ConstantController.EV_SPAWN_HAZARD, 45f));
    }

    private IEnumerator DelayNewSpawn(string _spawn, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        EventController.TriggerEvent(_spawn);
    }


    private void AdjustFadeColor()
    {
        float increment = maxAlpha / (ConstantController.HUNGER_MAX + ConstantController.HYDRATION_MAX);
        alpha = 0f;
        alpha += (ConstantController.HUNGER_MAX - hungerLevel) * increment;
        alpha += (ConstantController.HYDRATION_MAX - hydrationLevel) * increment;
        if ( alpha > maxAlpha )
        {
            alpha = maxAlpha;
        }
        VRTK_BasicTeleport.BlinkColourColor = new Color(0.2f, 0f, 0F, alpha);
        VRTK_SDK_Bridge.HeadsetFade(VRTK_BasicTeleport.BlinkColourColor, 0.1f);
    }


    private void UpdateBrief()
    {
        currentGameState = ConstantController.GAME_STATE.Playing;
    }

    private void UpdatePlaying()
    {
        if ( Time.timeSinceLevelLoad < ConstantController.GAME_TIME )
        {
            if ( hydrationLevel > 0 )
            {
                hydrationTimer += Time.deltaTime;
                if ( hydrationTimer > ConstantController.HYDRATION_DECREASE_TIME )
                {
                    hydrationLevel -= 1f;
                    AdjustFadeColor();
                    CanvasController.gameHydration = hydrationLevel;
                    hydrationTimer = 0f;
                }
            }
            else
            {
                winCondition = false;
                GetComponent<AudioSource>().clip = endGameStates[0];
                GetComponent<AudioSource>().Play();
                currentGameState = ConstantController.GAME_STATE.GameOver;
            }
            if ( hungerLevel > 0 )
            {
                hungerTimer += Time.deltaTime;
                if ( hungerTimer > ConstantController.HUNGER_DECREASE_TIME )
                {
                    hungerLevel -= 1f;
                    AdjustFadeColor();
                    CanvasController.gameEnergy = hungerLevel;
                    hungerTimer = 0f;
                }
            }
            else
            {
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

    public void FinalTaskComplete()
    {
        winCondition = true;
        currentGameState = ConstantController.GAME_STATE.GameOver;
    }

    private bool once = false;
    private void UpdateGameOver()
    {
        if (!once) {
            if ( winCondition ) {
                GetComponent<AudioSource>().clip = endGameStates[2];
                GetComponent<AudioSource>().Play();
                gameScore += Mathf.Ceil(gameTimer);
            }
            once = !once;
            StartCoroutine(LoadMainMenu(GetComponent<AudioSource>().clip.length));
        }
    }

    private IEnumerator LoadMainMenu(float _length)
    {
        yield return new WaitForSeconds(_length);
        FindObjectOfType<PlayerPrefsController>().SetPlayerScore(gameScore);
        SceneManager.LoadScene(LEVEL_00);
    }

    public void UpdateGameScore(float _score)
    {
        gameScore += Mathf.Ceil(_score);
        CanvasController.gameScore = gameScore;
    }

    public void UpdateGameTimer()
    {
        CanvasController.gameTimer = gameTimer;
        EventController.TriggerEvent(ConstantController.EV_UPDATE_STATUS_CANVAS);
    }

    public float GetGameTimer()
    {
        return gameTimer;
    }

    void OnEnable()
    {
        EventController.StartListening(ConstantController.EV_UPDATE_SCORE, UpdateGameScore);
        EventController.StartListening(ConstantController.EV_DRINK, ReplenishHydrationLevel);
        EventController.StartListening(ConstantController.EV_EAT, ReplenishHungerLevel);
        EventController.StartListening(ConstantController.EV_HAZARD_REMOVED, SpawnHazard);
    }

    void OnDisable()
    {
        EventController.StopListening(ConstantController.EV_UPDATE_SCORE, UpdateGameScore);
        EventController.StopListening(ConstantController.EV_DRINK, ReplenishHydrationLevel);
        EventController.StopListening(ConstantController.EV_EAT, ReplenishHungerLevel);
        EventController.StopListening(ConstantController.EV_HAZARD_REMOVED, SpawnHazard);
    }
}



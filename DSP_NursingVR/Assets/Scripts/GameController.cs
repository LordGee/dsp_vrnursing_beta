using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private ConstantController.GAME_STATE currentGameState;
    private ConstantController.PLAYER_STATE currentPlayerState;
    private float hydrationLevel, hydrationTimer;
    private float hungerLevel, hungerTimer;
    private float gameTimer, timerInterval, gameScore;
    private bool waterSpawned = false, foodSpawned = false;

    public float GetHydrationLevel() { return hydrationLevel; }
    public float GetHungerLevel() { return hungerLevel; }
    

    private void Start() {
        currentGameState = ConstantController.GAME_STATE.Brief;
        currentPlayerState = ConstantController.PLAYER_STATE.OK;
        hydrationTimer = 0f;
        hungerTimer = 0f;
        hydrationLevel = ConstantController.HYDRATION_MAX;
        hungerLevel = ConstantController.HUNGER_MAX;
        waterSpawned = foodSpawned = false;
        gameScore = 0f;
        gameTimer = timerInterval = 300f;
        UpdateGameScore(gameScore);
        UpdateGameTimer();
    }

    private void Update()
    {
        gameTimer -= Time.deltaTime;
        if (Mathf.Floor(timerInterval) - Mathf.Floor(gameTimer) >= 1f)
        {
            timerInterval = gameTimer;
            UpdateGameTimer();
        }
        switch (currentGameState) {
            case ConstantController.GAME_STATE.Brief:
                UpdateBrief();
                break;
            case ConstantController.GAME_STATE.Playing:
                UpdatePlaying();
                break;
            case ConstantController.GAME_STATE.Needs:
                switch (currentPlayerState) {
                    case ConstantController.PLAYER_STATE.Water:
                        // NeedWater();
                        break;
                    case ConstantController.PLAYER_STATE.Food:
                        NeedFood();
                        break;
                    case ConstantController.PLAYER_STATE.OK:
                        currentGameState = ConstantController.GAME_STATE.Playing;
                        break;
                    default:
                        Debug.LogError("No active player state!");
                        break;
                }
                break;
            case ConstantController.GAME_STATE.GameOver:
                UpdateGameOver();
                break;
            default:
                Debug.LogError("No active game state!");
                break;
        }
    }

    public void ReplenishHydrationLevel() {
        hydrationLevel = ConstantController.HYDRATION_MAX;
        hydrationTimer = 0f;
        currentPlayerState = ConstantController.PLAYER_STATE.OK;
    }

    public void ReplenishHungerLevel() {
        hungerLevel = ConstantController.HUNGER_MAX;
        hungerTimer = 0f;
        currentPlayerState = ConstantController.PLAYER_STATE.OK;
    }
    private void UpdateBrief() {
        // Display Brief
        //Debug.Log("Displaying Brief");
        currentGameState = ConstantController.GAME_STATE.Playing;
        //Debug.Log("Now Playing");
    }

    private void UpdatePlaying() {
        if (Time.timeSinceLevelLoad < ConstantController.GAME_TIME) {
            if (hydrationLevel > 0) {
                hydrationTimer += Time.deltaTime;
                if (hydrationTimer > ConstantController.HYDRATION_DECREASE_TIME) {
                    hydrationLevel -= 1f;
                    hydrationTimer = 0f;
                    if (hydrationLevel <= 2 && !waterSpawned) {
                        EventController.TriggerEvent(ConstantController.EV_SPAWN_WATER);
                        waterSpawned = true;
                    }
                    //Debug.Log("Reduced hydration level, new level = " + hydrationLevel);
                }
            } else {
                currentPlayerState = ConstantController.PLAYER_STATE.Water;
                currentGameState = ConstantController.GAME_STATE.Needs;
                Debug.Log("Player NEEDS Water Badley, level = " + hydrationLevel);
            }
            if (hungerLevel > 0) {
                hungerTimer += Time.deltaTime;
                if (hungerTimer > ConstantController.HUNGER_DECREASE_TIME) {
                    hungerLevel -= 1f;
                    hungerTimer = 0f;
                    //Debug.Log("Reduced hunger level, new level = " + hungerLevel);
                }
            } else {
                currentPlayerState = ConstantController.PLAYER_STATE.Food;
                currentGameState = ConstantController.GAME_STATE.Needs;
                Debug.Log("Player NEEDS Food Badley, level = " + hungerLevel);
            }
        } else {
            currentGameState = ConstantController.GAME_STATE.GameOver;
            Debug.Log("Game Over" + System.DateTime.Now);
        }
    }

    private void DrinkWater() {
        ReplenishHydrationLevel();
        Debug.Log("Thirst has been quenched");
    }

    private void NeedFood() {
        ReplenishHungerLevel();
        Debug.Log("That burger was tasty");
    }

    private void UpdateGameOver() {
        Debug.Log("Thats ALL Folks!");
    }

    public void UpdateGameScore(float _score)
    {
        gameScore += _score;
        GameObject.Find("Txt_Score").GetComponent<Text>().text = gameScore.ToString("F");
    }

    public void UpdateGameTimer() {
        GameObject.Find("Txt_Timer").GetComponent<Text>().text = Mathf.Floor(gameTimer).ToString();
    }

    void OnEnable()
    {
        EventController.StartListening(ConstantController.EV_UPDATE_SCORE, UpdateGameScore);
    }

    void OnDisable()
    {
        EventController.StopListening(ConstantController.EV_UPDATE_SCORE, UpdateGameScore);
    }
}



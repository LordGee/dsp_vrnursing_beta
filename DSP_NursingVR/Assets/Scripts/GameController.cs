﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class GameController : MonoBehaviour {

    private ConstantController.GAME_STATE currentGameState;
    private float hydrationLevel, hydrationTimer;
    private float hungerLevel, hungerTimer;
    private float gameTimer, timerInterval, gameScore;
    private bool waterSpawned, foodSpawned;
    private float alpha;
    private float maxAlpha = 0.8f;

    public float GetHydrationLevel() { return hydrationLevel; }
    public float GetHungerLevel() { return hungerLevel; }
    

    private void Start() {
        currentGameState = ConstantController.GAME_STATE.Brief;
        hydrationTimer = 0f;
        hungerTimer = 0f;
        CanvasController.gameHydration = hydrationLevel = ConstantController.HYDRATION_MAX;
        CanvasController.gameEnergy = hungerLevel = ConstantController.HUNGER_MAX;
        EventController.TriggerEvent(ConstantController.EV_SPAWN_FOOD);
        EventController.TriggerEvent(ConstantController.EV_SPAWN_WATER);
        gameScore = 0f;
        gameTimer = timerInterval = 300f;
        UpdateGameScore(gameScore);
        UpdateGameTimer();
        VRTK_BasicTeleport.BlinkColourColor = Color.clear;
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
        EventController.TriggerEvent(ConstantController.EV_SPAWN_FOOD);
    }

    public void ReplenishHydrationLevel() {
        CanvasController.gameHydration = hydrationLevel = ConstantController.HYDRATION_MAX;
        hydrationTimer = 0f;
        AdjustFadeColor();
        EventController.TriggerEvent(ConstantController.EV_SPAWN_WATER);
    }


    private void AdjustFadeColor()
    {
        float increment = maxAlpha / (ConstantController.HUNGER_MAX + ConstantController.HYDRATION_MAX);
        alpha = 0f;
        alpha += (ConstantController.HUNGER_MAX - hungerLevel) * increment;
        alpha += (ConstantController.HYDRATION_MAX - hydrationLevel) * increment;
        Debug.Log(alpha);
        if (alpha  > maxAlpha)
        {
            alpha = maxAlpha;
        }
        VRTK_BasicTeleport.BlinkColourColor = new Color(0.2f, 0f, 0F, alpha);
        VRTK_SDK_Bridge.HeadsetFade(VRTK_BasicTeleport.BlinkColourColor, 0.1f);
    }


    private void UpdateBrief() {
        currentGameState = ConstantController.GAME_STATE.Playing;
    }

    private void UpdatePlaying() {
        if (Time.timeSinceLevelLoad < ConstantController.GAME_TIME) {
            if (hydrationLevel > 0) {
                hydrationTimer += Time.deltaTime;
                if (hydrationTimer > ConstantController.HYDRATION_DECREASE_TIME) {
                    hydrationLevel -= 1f;
                    AdjustFadeColor();
                    CanvasController.gameHydration = hydrationLevel;
                    hydrationTimer = 0f;
                }
            } else {
                Debug.Log("Player NEEDS Water Badley, level = " + hydrationLevel);
            }
            if (hungerLevel > 0) {
                hungerTimer += Time.deltaTime;
                if (hungerTimer > ConstantController.HUNGER_DECREASE_TIME) {
                    hungerLevel -= 1f;
                    AdjustFadeColor();
                    CanvasController.gameEnergy = hungerLevel;
                    hungerTimer = 0f;
                }
            } else {
                Debug.Log("Player NEEDS Food Badley, level = " + hungerLevel);
            }
        } else {
            currentGameState = ConstantController.GAME_STATE.GameOver;
            Debug.Log("Game Over" + System.DateTime.Now);
        }
    }

    private void UpdateGameOver() {
        Debug.Log("Thats ALL Folks!");
    }

    public void UpdateGameScore(float _score)
    {
        gameScore += _score;
        CanvasController.gameScore = gameScore;
    }

    public void UpdateGameTimer() {
        CanvasController.gameTimer = gameTimer;
        EventController.TriggerEvent(ConstantController.EV_UPDATE_STATUS_CANVAS);
    }

    void OnEnable()
    {
        EventController.StartListening(ConstantController.EV_UPDATE_SCORE, UpdateGameScore);
        EventController.StartListening(ConstantController.EV_DRINK, ReplenishHydrationLevel);
        EventController.StartListening(ConstantController.EV_EAT, ReplenishHungerLevel);
    }

    void OnDisable()
    {
        EventController.StopListening(ConstantController.EV_UPDATE_SCORE, UpdateGameScore);
        EventController.StopListening(ConstantController.EV_DRINK, ReplenishHydrationLevel);
        EventController.StopListening(ConstantController.EV_EAT, ReplenishHungerLevel);
    }
}



using System;
using System.Collections.Generic;
using UnityEngine;

public class ConstantController {

    // Event Names
    public const string EV_SPAWN_WATER = "SpawnWaterEvent";
    public const string EV_SPAWN_FOOD = "SpawnFoodEvent";
    public const string EV_UPDATE_SCORE = "UpdateScoreEvent";
    public const string EV_UPDATE_STATUS_CANVAS = "UpdateStatusCanvas";
    public const string EV_OPEN_STATUS_CANVAS = "OpenStatusCanvas";
    public const string EV_CLOSE_STATUS_CANVAS = "CloseStatusCanvas";


    // Game Controller
    public enum GAME_STATE { Brief, Playing, Needs, GameOver };
    public enum TASK_STATE { Task3 };
    public enum PLAYER_STATE { Water, Food, OK };

    public const float HYDRATION_MAX = 8f;
    public const float HYDRATION_DECREASE_TIME = 20f;
    public const float HUNGER_MAX = 8f;
    public const float HUNGER_DECREASE_TIME = 36f;
    public const float GAME_TIME = 300f;

    // Interactable Objects
    public const string TAG_PICKUP = "Pickup";
    public const string TAG_WATER = "Water";
    public const string TAG_FOOD = "Food";

    public static readonly string[] pickupObjects =
    {
        TAG_PICKUP,
        TAG_WATER,
        TAG_FOOD
    };

    // Task Events
    public const string TASK_SELECTION_OPTION = "SelectedOption";
    public const string TASK_ACCEPT_OPTION = "AcceptOption";
    public const string TASK_ACCEPT = "Accept";
    public const string TASK_IGNORE = "Ignore";
    public const string TASK_DELEGATE = "Delegate";
    public const string TASK_WIN = "WinTask";
    public const string TASK_COMPLETE = "TaskComplete";

    // GameObjects
    public const string GO_STATUS_CANVAS = "StatusCanvas";

    // Pipe Game
    public enum PIPE_PIECES
    {
        Start,
        Game,
        Finish
    };

    public const string TAG_PIPES = "Pipe";
    public const string TAG_PIPE_CONNECTOR = "PipeConnector";
    public const string TAG_PIPE_PART = "PipePart";
}


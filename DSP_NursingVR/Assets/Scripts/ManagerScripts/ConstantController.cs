public class ConstantController {

    // Event Names
    public const string EV_SPAWN_WATER = "SpawnWaterEvent";
    public const string EV_SPAWN_FOOD = "SpawnFoodEvent";
    public const string EV_SPAWN_COLLECTABLE = "SpawnCollectableEvent";
    public const string EV_SPAWN_HAZARD = "SpawnHazard";
    public const string EV_UPDATE_SCORE = "UpdateScoreEvent";
    public const string EV_UPDATE_STATUS_CANVAS = "UpdateStatusCanvas";
    public const string EV_OPEN_STATUS_CANVAS = "OpenStatusCanvas";
    public const string EV_CLOSE_STATUS_CANVAS = "CloseStatusCanvas";
    public const string EV_EAT = "EatFood";
    public const string EV_DRINK = "DrinkWater";
    public const string EV_COLLECTED = "CollectObject";
    public const string EV_HAZARD_REMOVED = "HazardCleard";
    
    // Game Controller
    public enum GAME_STATE { Brief, Playing, Needs, GameOver };
    public enum TASK_STATE { Task3 };
    
    public const float HYDRATION_MAX = 8f;
    public const float HYDRATION_DECREASE_TIME = 20f;
    public const float HUNGER_MAX = 8f;
    public const float HUNGER_DECREASE_TIME = 36f;
    public const float GAME_TIME = 300f;
    public const float TASK_TIME = 120f;

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
    public const string TASK_START_SIGNAL = "Step1";
    public const string TASK_END_SIGNAL = "Step2";

    // GameObjects
    public const string GO_STATUS_CANVAS = "StatusCanvas";
    public const string GO_FOOT_COLLIDER = "[VRTK][AUTOGEN][FootColliderContainer]";

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


using UnityEngine;

/// <summary>
/// Handles the unique process of task two
/// </summary>
public class Task2 : MonoBehaviour
{
    public GameObject bed7;
    private GameObject telephone, bed7Object;
    private bool[] objectSuccess;
    private bool winStatus, once, phoneAnswered, taskStarted;
    private float taskTimer, timerUpdate;
    private const int TASK_INDEX = 1;

    /// <summary>
    /// Constructor
    /// </summary>
    void Start() {
        telephone = GameObject.Find("Telephone");
        MakeTheCall();
    }

    /// <summary>
    /// Update method handles the task time and updaes the display value as well
    /// as the colour of the text depending on how long is left on the clock.
    /// </summary>
    void Update() {
        if (taskStarted) {
            taskTimer -= Time.deltaTime;
            if ( taskTimer < 0 ) {
                taskTimer = 0;
                taskStarted = false;
                FindObjectOfType<Telephone>().PrepareFailedCall();
            } else if ( taskTimer < 60f && taskTimer > 30f ) {
                GameObject.Find("Timer").GetComponent<TextMesh>().color = new Color(1f, 0.8f, 0.4f, 1f);
            } else if ( taskTimer < 30f ) {
                GameObject.Find("Timer").GetComponent<TextMesh>().color = new Color(1f, 0f, 0f, 1f);
            }
            if ( Mathf.Floor(taskTimer) != Mathf.Floor(timerUpdate) ) {
                GameObject.Find("Timer").GetComponent<TextMesh>().text = taskTimer.ToString("F0");
                timerUpdate = Mathf.Floor(taskTimer);
            }
        }
    }

    /// <summary>
    /// If this class is destroyed the game object in scene is also destroyed
    /// </summary>
    private void OnDestroy() {
        Destroy(bed7Object);
    }

    /// <summary>
    /// Sets the telephone to start ringing
    /// </summary>
    private void MakeTheCall() {
        telephone.GetComponent<Telephone>().StartRinging();
    }

    /// <summary>
    /// Instantiates the main main task objects into the scene and sets a 
    /// counter of bools to test which objects have been successfully positioned
    /// </summary>
    public void StartTask()
    {
        bed7Object = Instantiate(bed7);
        taskTimer = ConstantController.TASK_TIME;
        objectSuccess = new bool[3];
        for ( int i = 0; i < objectSuccess.Length; i++ ) {
            objectSuccess[i] = false;
        }
        taskStarted = true;
        GameObject.Find("Counter").GetComponent<TextMesh>().text = "0 / 3";
    }

    /// <summary>
    /// Sets the bool index to true for a successfully placed game object
    /// </summary>
    /// <param name="_index">Index of object</param>
    public void UpdateObjectResult(int _index) {
        objectSuccess[_index] = true;
        TestForWinCondition();
    }

    /// <summary>
    /// Checks if the task has been completed or not. If true then additonal time is added 
    /// to the players clock to allow for the player to get to the telephone at the other end of the room
    /// </summary>
    private void TestForWinCondition() {
        winStatus = true;
        int counter = 0;
        for ( int i = 0; i < objectSuccess.Length; i++ ) {
            if (!objectSuccess[i]) {
                winStatus = false;
            } else {
                counter++;
            }
        }
        GameObject.Find("Counter").GetComponent<TextMesh>().text = counter + " / 3";
        if (winStatus) {
            FindObjectOfType<Telephone>().PrepareReturnCall();
            taskTimer += 30f;
        } 
    }

    /// <summary>
    /// Plays the given audio clip
    /// </summary>
    /// <param name="_clip">Desired audio clip</param>
    public void PlaySFX(AudioClip _clip) {
        GetComponent<AudioSource>().clip = _clip;
        GetComponent<AudioSource>().Play();
    }

    public bool GetWinStatus() { return winStatus; }

    /// <summary>
    /// Once the task has been completed will trigger events to end the task
    /// </summary>
    /// <param name="_bonus">Bonus points to be added</param>
    public void WinTask(float _bonus) {
        if (!once) {
            EventController.StopListening(ConstantController.TASK_WIN, WinTask);
            EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, _bonus + Mathf.Floor(taskTimer));
            EventController.TriggerEvent(ConstantController.TASK_COMPLETE, TASK_INDEX);
            once = true;
        }
    }

    private void OnEnable() {
        EventController.StartListening(ConstantController.TASK_WIN, WinTask);
    }
}
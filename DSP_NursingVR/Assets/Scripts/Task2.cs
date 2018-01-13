using UnityEngine;

public class Task2 : MonoBehaviour
{
    public GameObject bed7;

    private GameObject telephone, bed7Object;
    private bool[] objectSuccess;
    private bool winStatus, once, phoneAnswered, taskStarted;
    private float taskTimer, timerUpdate;
    private const int TASK_INDEX = 1;


    void Start()
    {
        telephone = GameObject.Find("Telephone");
        MakeTheCall();
    }

    void Update()
    {
        if (taskStarted)
        {
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
            if ( Mathf.Floor(taskTimer) != Mathf.Floor(timerUpdate) )
            {
                GameObject.Find("Timer").GetComponent<TextMesh>().text = taskTimer.ToString("F0");
                timerUpdate = Mathf.Floor(taskTimer);
            }
        }
    }

    private void OnDestroy()
    {
        Destroy(bed7Object);
    }

    private void MakeTheCall()
    {
        telephone.GetComponent<Telephone>().StartRinging();
    }

    public void StartTask()
    {
        bed7Object = Instantiate(bed7);
        taskTimer = ConstantController.TASK_TIME;
        objectSuccess = new bool[3];
        for ( int i = 0; i < objectSuccess.Length; i++ ) {
            objectSuccess[i] = false;
        }
        taskStarted = true;
    }

    public void UpdateObjectResult(int _index)
    {
        objectSuccess[_index] = true;
        TestForWinCondition();
    }

    private void TestForWinCondition()
    {
        winStatus = true;
        for ( int i = 0; i < objectSuccess.Length; i++ ) {
            if ( !objectSuccess[i] ) { winStatus = false; }
        }
        if (winStatus) {
            FindObjectOfType<Telephone>().PrepareReturnCall();
            taskTimer += 30f;
        } 
    }

    public bool GetWinStatus() { return winStatus; }

    public void WinTask(float _bonus)
    {
        if (!once) {
            EventController.StopListening(ConstantController.TASK_WIN, WinTask);
            EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, _bonus + Mathf.Floor(taskTimer));
            EventController.TriggerEvent(ConstantController.TASK_COMPLETE, TASK_INDEX);
            once = true;
        }
    }

    private void OnEnable()
    {
        EventController.StartListening(ConstantController.TASK_WIN, WinTask);
    }
}
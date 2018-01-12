using UnityEngine;

public class Task2 : MonoBehaviour
{
    public GameObject bed7;

    private bool[] objectSuccess;
    private bool winStatus, once;
    private float taskTimer;
    private const int TASK_INDEX = 1;


    void Start()
    {
        bed7 = Instantiate(bed7);
        taskTimer = ConstantController.TASK_TIME;
        objectSuccess = new bool[3];
        for (int i = 0; i < objectSuccess.Length; i++) {
            objectSuccess[i] = false;
        }
    }

    void Update()
    {
        if (!winStatus) {
            taskTimer -= Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        Destroy(bed7);
    }

    public void UpdateObjectResult(int _index)
    {
        objectSuccess[_index] = true;
        TestForWinCondition();
    }

    private void TestForWinCondition()
    {
        winStatus = true;
        for ( int i = 0; i < objectSuccess.Length; i++ )
        {
            if ( !objectSuccess[i] ) { winStatus = false; }
        }

        if ( winStatus ) { WinTask(); } 
    }

    private void WinTask()
    {
        if (!once)
        {
            EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, 80f + Mathf.Floor(taskTimer));
            EventController.TriggerEvent(ConstantController.TASK_COMPLETE, TASK_INDEX);
            once = true;
        }
    }
}
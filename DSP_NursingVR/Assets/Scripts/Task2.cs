using UnityEngine;

public class Task2 : MonoBehaviour
{
    public GameObject bed7;

    private bool[] objectSuccess;
    private bool winStatus, once;
    private float taskTimer;


    void Start()
    {
        Instantiate(bed7);
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

    public void UpdateObjectResult(int _index)
    {
        objectSuccess[_index] = true;
        Debug.Log("Bool is now " + objectSuccess[_index]);
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
            Debug.Log("You Win!");
            FindObjectOfType<GameController>().UpdateGameScore(80f + taskTimer);
            EventController.TriggerEvent(ConstantController.TASK_COMPLETE, 1f);
            once = true;
        }
    }
}
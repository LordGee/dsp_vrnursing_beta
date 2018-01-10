using UnityEngine;

public class Task2 : MonoBehaviour
{
    private bool[] objectSuccess;
    private bool winStatus;

    void Start()
    {
        objectSuccess = new bool[2];
        for (int i = 0; i < objectSuccess.Length; i++) {
            objectSuccess[i] = false;
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
        for ( int i = 0; i < objectSuccess.Length; i++ )
        {
            if ( !objectSuccess[i] ) { winStatus = false; }
        }

        if ( winStatus ) { WinTask(); } 
    }

    private void WinTask()
    {
        Debug.Log("You Win!");
    }
}
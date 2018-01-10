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

    void Update()
    {
        for (int i = 0; i < objectSuccess.Length; i++) {
            if (!objectSuccess[i]) { winStatus = false; }
        }

        if (winStatus) { WinTask(); }
        else
        {
            Debug.Log("Not won yet");
        }
    }

    public void UpdateObjectResult(int _index)
    {
        objectSuccess[_index] = true;
        Debug.Log("Bool is now " + objectSuccess[_index]);
    }

    private void WinTask()
    {
        Debug.Log("You Win!");
    }
}
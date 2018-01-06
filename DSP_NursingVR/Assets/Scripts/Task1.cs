using System.Collections;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class Task1 : MonoBehaviour
{
    public GameObject taskCanvas, spawnCanvas;
    public string taskInstructions;

    private GameObject canvas;
    private bool isActive;
    private int itemCount;

    void Start()
    {
        EventController.TriggerEvent(ConstantController.EV_SPAWN_COLLECTABLE);
        spawnCanvas = GameObject.Find("StartInstructions");

        taskCanvas.transform.Find("Instructions").GetComponent<Text>().text = taskInstructions;
        canvas = Instantiate(taskCanvas, spawnCanvas.transform);
        StartCoroutine(DestroyCanvas());

        isActive = true;
        itemCount = 0;
    }

    private IEnumerator DestroyCanvas()
    {
        yield return new WaitForSeconds(30f);
        Destroy(spawnCanvas);
    }

    public bool GetIsActive() { return isActive; }
    
    public void ItemHasBeenCollected()
    {
        isActive = false;
        itemCount++;
        if (itemCount >= 4) {
            WinTask();
        }
    }

    private void WinTask()
    {
        Debug.Log("Task 1 Completed");
        EventController.TriggerEvent(ConstantController.TASK_COMPLETE, 0);
        // destroy this
    }
}
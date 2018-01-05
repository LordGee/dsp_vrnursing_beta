using UnityEngine;

public class Task1 : MonoBehaviour
{

    public GameObject taskCanvas;
    private bool isActive;
    private int itemCount = 0;

    void Start()
    {
        EventController.TriggerEvent(ConstantController.EV_SPAWN_COLLECTABLE);
        isActive = true;
        itemCount++;
    }

    public bool GetIsActive() { return isActive; }
    
    public void ItemHasBeenCollected()
    {
        isActive = false;
        itemCount++;
        if (itemCount >= 4)
        {
            WinTask();
        }
    }

    private void WinTask()
    {
        // destroy this
        Debug.Log("Task 1 Completed");
    }
}
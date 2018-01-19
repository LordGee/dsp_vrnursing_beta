using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the starting and ending of tasks
/// </summary>
public class TaskController : MonoBehaviour
{
    public GameObject[] tasks;
    private List<GameObject> currentTask; 
    private int currentIndex;
    private int selectedOption;

    /// <summary>
    /// Constructor - Initials the first task
    /// </summary>
    void Start() {
        selectedOption = 0;
        currentTask = new List<GameObject>();
        InitiateTask(0);
    }

    /// <summary>
    /// Inititates a given task
    /// </summary>
    /// <param name="_index">Task index to be initiated</param>
    public void InitiateTask(int _index) {
        currentTask.Add(Instantiate(tasks[_index]));
    }

    /// <summary>
    /// Called via an event when the task is to be terminated
    /// </summary>
    /// <param name="_index">Task index to end</param>
    private void TaskCompleted(float _index) {
        if ( currentTask[(int)_index] != null ) {
            StartCoroutine(EndTask((int)_index));
        } else {
            Debug.LogWarning("Unable to destroy task " + _index + " because it does not exist");
        }
    }

    /// <summary>
    /// Coroutine to destroy a given task after 3 seconds
    /// </summary>
    /// <param name="_index">Task index to be destroyed</param>
    private IEnumerator EndTask(int _index) {
        yield return new WaitForSeconds(3f);
        Destroy(currentTask[_index]);
        if (_index == 0) {
            FindObjectOfType<GameController>().FinalTaskComplete();
        }
    }

    /// <summary>
    /// Events to start
    /// </summary>
    void OnEnable() {
        EventController.StartListening(ConstantController.TASK_COMPLETE, TaskCompleted);
        EventController.StartListening(ConstantController.TASK_SELECTION_OPTION, SelectedOption);
        EventController.StartListening(ConstantController.TASK_ACCEPT_OPTION, AcceptOption);
    }

    /// <summary>
    /// Events to end
    /// </summary>
    void OnDisable() {
        EventController.StopListening(ConstantController.TASK_COMPLETE, TaskCompleted);
        EventController.StopListening(ConstantController.TASK_SELECTION_OPTION, SelectedOption);
        EventController.StopListening(ConstantController.TASK_ACCEPT_OPTION, AcceptOption);
    }

    /// <summary>
    /// This function should be moved to the Task3 script.
    /// TODO: If time allows reallocated this function to the task 3 script
    /// </summary>
    public void AcceptButton() {
        EventController.TriggerEvent(ConstantController.TASK_ACCEPT);
    }

    /// <summary>
    /// This function should be moved to the Task3 script.
    /// TODO: If time allows reallocated this function to the task 3 script
    /// </summary>
    public void DelegateButton() {
        EventController.TriggerEvent(ConstantController.TASK_DELEGATE);
    }

    /// <summary>
    /// This function should be moved to the Task3 script.
    /// TODO: If time allows reallocated this function to the task 3 script
    /// </summary>
    public void SelectedOption(float _value) {
        selectedOption = (int)_value;
    }

    /// <summary>
    /// This function should be moved to the Task3 script.
    /// TODO: If time allows reallocated this function to the task 3 script
    /// </summary>
    public void AcceptOption() {
        if ( selectedOption == 0 ){
            // do nothing for now
        } else if ( selectedOption == 1 || selectedOption == 2 ) {
            AcceptButton();
        } else if ( selectedOption == 3 ) {
            DelegateButton();
        }
    }
}
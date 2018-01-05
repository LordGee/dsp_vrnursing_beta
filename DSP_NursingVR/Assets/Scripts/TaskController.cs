using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskController : MonoBehaviour
{

    public GameObject[] tasks;

    private List<GameObject> currentTask;
    private int currentIndex;
    private int selectedOption;
    

    void Start()
    {
        selectedOption = 0;
        InitiateTask(0);
    }

    private void InitiateTask(int _index)
    {
        currentTask.Add(Instantiate(tasks[_index]));
    }

    private void TaskCompleted()
    {
        StartCoroutine(EndTask());
    }

    private IEnumerator EndTask() {
        yield return new WaitForSeconds(5f);
        // Destroy(currentTask);
    }

    public void AcceptButton() {
        EventController.TriggerEvent(ConstantController.TASK_ACCEPT);
    }

    public void DelegateButton() {
        EventController.TriggerEvent(ConstantController.TASK_DELEGATE);
    }

    public void SelectedOption(float _value)
    {
        selectedOption = (int)_value;
    }

    public void AcceptOption()
    {
        if (selectedOption == 0 || selectedOption == 2) {
            // do nothing for now
        } else if (selectedOption == 1) {
            AcceptButton();
        } else if (selectedOption == 3) {
            DelegateButton();
        }
    }

    void OnEnable()
    {
        EventController.StartListening(ConstantController.TASK_COMPLETE, TaskCompleted);
        EventController.StartListening(ConstantController.TASK_SELECTION_OPTION, SelectedOption);
        EventController.StartListening(ConstantController.TASK_ACCEPT_OPTION, AcceptOption);
    }

    void OnDisable() {
        EventController.StopListening(ConstantController.TASK_COMPLETE, TaskCompleted);
        EventController.StopListening(ConstantController.TASK_SELECTION_OPTION, SelectedOption);
        EventController.StopListening(ConstantController.TASK_ACCEPT_OPTION, AcceptOption);
    }

}


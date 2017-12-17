using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Task3 : MonoBehaviour {

    public GameObject triggerEventPrefab, taskCanvas, taskChallenge, gamePosition;
    public string taskInstructions;

    private GameObject[] beds;
    private GameObject bed, trigger, canvas, task;
    private float lightTimer, lightStrobeSpeed = 0.5f, lightOnValue = 1.5f, taskTimer, gameLength = 120f;
    private bool lightStatus;
    private int step;
    private PipeManager pm;

    void Start()
    {
        beds = GameObject.FindGameObjectsWithTag("Bed");
        bed = beds[Random.Range(0, beds.Length)];
        lightTimer = Time.timeSinceLevelLoad;
        trigger = Instantiate(triggerEventPrefab, bed.transform);
        step = 1;
        taskTimer = 120f;
    }

    void Update()
    {
        if (step == 1)
        {
            if (trigger != null && !trigger.GetComponent<TriggerEventStart>().HasTriggerActivated()) {
                SignalNewTask();
            } else {
                EndSignal();
            }
        } else if (step == 2) {
            LaunchInstructions();
        } else if (step == 3) {
            RotateInstructions();
        } else if (step == 4) {
            LaunchGame();
        } else if (step == 5) {
            taskTimer -= Time.deltaTime;
        }
    }

    private void OnDestroy() {
        Destroy(task);
    }

    private void SignalNewTask()
    {
        if (Time.timeSinceLevelLoad - lightTimer > lightStrobeSpeed) {
            if (!lightStatus) {
                bed.GetComponentInChildren<Light>().range = lightOnValue;
                lightStatus = !lightStatus;
            } else {
                bed.GetComponentInChildren<Light>().range = 0f;
                lightStatus = !lightStatus;
            }
            lightTimer = Time.timeSinceLevelLoad;
        }   
    }

    private void EndSignal()
    {
        bed.GetComponentInChildren<Light>().range = 0f;
        lightStatus = !lightStatus;
        step++;
        Destroy(trigger.gameObject);
    }

    private void LaunchInstructions()
    {
        float newX = (bed.transform.position.x > 0) ? 2.5f : -2.5f;
        canvas = Instantiate(
            taskCanvas,
            new Vector3(bed.transform.position.x + newX, bed.transform.position.y + 2.5f, bed.transform.position.z),
            Quaternion.identity
        );
        step++;
    }

    private void RotateInstructions()
    {
        GameObject player = GameObject.FindGameObjectWithTag("MainCamera");
        Vector3 rotateTo = player.transform.forward * 100;
        Quaternion rotate = Quaternion.LookRotation(rotateTo - canvas.transform.localPosition);
        canvas.transform.localRotation = Quaternion.RotateTowards(canvas.transform.localRotation, rotate, 0.5f);
        canvas.GetComponentInChildren<Text>().text = taskInstructions;
    }

    private void LaunchGame()
    {
        Debug.LogWarning("LaunchGame");
        Destroy(canvas);
        task = Instantiate(taskChallenge);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = gamePosition.transform.position;
        player.transform.rotation = gamePosition.transform.rotation;
        step++;
    }

    private bool onlyOnce = false;
    public void AcceptButton()
    {
        if (!onlyOnce)
        {
            step++;
            onlyOnce = true;
        }
    }

    public void DelegateButton() {
        Debug.Log("DELEGATE");
    }

    private bool once;
    public void WinTask()
    {
        if (!once)
        {
            once = true;
            step++;
            Debug.Log("WINNER!!! - " + taskTimer.ToString("F"));
            EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, taskTimer);
            PipeManager.DropUnusedPipes();
            EventController.TriggerEvent(ConstantController.TASK_COMPLETE);
        }
    }

    

    private void OnEnable() {
        EventController.StartListening(ConstantController.TASK_ACCEPT, AcceptButton);
        EventController.StartListening(ConstantController.TASK_DELEGATE, DelegateButton);
        EventController.StartListening(ConstantController.TASK_WIN, WinTask);
    }

    private void OnDisable() {
        EventController.StopListening(ConstantController.TASK_ACCEPT, AcceptButton);
        EventController.StopListening(ConstantController.TASK_DELEGATE, DelegateButton);
        EventController.StopListening(ConstantController.TASK_WIN, WinTask);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Task3 : MonoBehaviour {

    public GameObject triggerEventPrefab, taskCanvas, taskAcceptor, taskChallenge, gamePosition;
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
        RotateTrigger();
        step = 1;
        taskTimer = 120f;
        EventController.StartListening(ConstantController.TASK_END_SIGNAL, EndSignal);
    }

    void Update()
    {
        if (step == 1) {
            SignalNewTask();
        }  else if (step == 3) {
            // RotateInstructions();
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
                bed.transform.parent.GetComponentInChildren<Light>().range = lightOnValue;
                lightStatus = !lightStatus;
            } else {
                bed.transform.parent.GetComponentInChildren<Light>().range = 0f;
                lightStatus = !lightStatus;
            }
            lightTimer = Time.timeSinceLevelLoad;
        }   
    }

    private void EndSignal()
    {
        bed.transform.parent.GetComponentInChildren<Light>().range = 0f;
        lightStatus = !lightStatus;
        step++;
        Destroy(trigger.gameObject);
        EventController.StopListening(ConstantController.TASK_END_SIGNAL, EndSignal);
        LaunchInstructions();
    }

    private void LaunchInstructions()
    {
        EventController.StartListening(ConstantController.TASK_ACCEPT, AcceptButton);
        // float newX = (bed.transform.parent.position.x > 0) ? 2.5f : -2.5f;
        taskCanvas.transform.Find("Instructions").GetComponent<Text>().text = taskInstructions;
        canvas = Instantiate( taskCanvas, bed.transform );
        Instantiate(taskAcceptor, bed.transform);
        step++;
    }

    private void RotateTrigger()
    {
        if (bed.transform.position.z < 0) {
            GameObject teleportPoint = trigger.transform.Find("TeleportPoint").gameObject;
            teleportPoint.transform.Rotate(Vector3.up, 180f);
        }
        /*
        // GameObject player = GameObject.FindGameObjectWithTag("MainCamera");
        Vector3 rotateTo = bed.transform.forward * 1f;
        GameObject teleportPoint = trigger.transform.Find("TeleportPoint").gameObject;
        Quaternion rotate = Quaternion.LookRotation(rotateTo - teleportPoint.transform.localPosition);
        teleportPoint.transform.localRotation = Quaternion.RotateTowards(teleportPoint.transform.localRotation, rotate, 360f);
        */
    }

    private void LaunchGame()
    {
        EventController.StopListening(ConstantController.TASK_ACCEPT, AcceptButton);
        EventController.StartListening(ConstantController.TASK_WIN, WinTask);
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
            LaunchGame();
        }
    }

    public void DelegateButton() {
        EventController.StopListening(ConstantController.TASK_DELEGATE, DelegateButton);
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
            EventController.StopListening(ConstantController.TASK_WIN, WinTask);
            EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, taskTimer);
            PipeManager.DropUnusedPipes();
            EventController.TriggerEvent(ConstantController.TASK_COMPLETE);
        }
    }

    

    private void OnEnable() {
        EventController.StartListening(ConstantController.TASK_DELEGATE, DelegateButton);
    }

    private void OnDisable() {
        

    }
}


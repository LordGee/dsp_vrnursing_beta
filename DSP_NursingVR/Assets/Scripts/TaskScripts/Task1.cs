﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Task1 : MonoBehaviour
{
    public GameObject taskCanvas, spawnCanvas, triggerEventPrefab;
    public AudioClip[] voiceOverAudioClips;
    public string[] taskInstructions;

    private bool isActive;
    private int itemCount, voiceIndex;
    private float spawnTimer = 0;
    private const int AMOUNT_TO_WIN = 3;
    private const int TASK_INDEX = 0;

    void Start()
    {
        spawnCanvas = GameObject.Find("StartInstructions");
        Instantiate(taskCanvas, spawnCanvas.transform);

        voiceIndex = 0;
        PlayAudioClips();

        isActive = true;
        itemCount = 0;
    }

    void Update()
    {
        if (!isActive)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > 60 || FindObjectOfType<GameController>().GetGameTimer() < 60)
            {
                SpawnNewCollectable();
            }
        }
    }

    private void PlayAudioClips()
    {
        if (voiceIndex != voiceOverAudioClips.Length)
        {
            GetComponent<AudioSource>().clip = voiceOverAudioClips[voiceIndex];
            GetComponent<AudioSource>().Play();
            if (voiceIndex < taskInstructions.Length)
            {
                GameObject.Find("Instructions").GetComponent<Text>().text = taskInstructions[voiceIndex];
            }
            StartCoroutine(PlayNextClip(GetComponent<AudioSource>().clip.length));
            voiceIndex++;
        }
        else
        {
            SpawnNewCollectable();
            SetTeleportPoint();
            StartCoroutine(DestroyCanvas());
        }
    }

    private IEnumerator PlayNextClip(float _waitFor)
    {
        yield return  new WaitForSeconds(_waitFor);
        if ( GetComponent<AudioSource>().isPlaying ) {
            StartCoroutine(PlayNextClip(1f));
        } else {
            PlayAudioClips();
        }
    }

    private IEnumerator DestroyCanvas()
    {
        yield return new WaitForSeconds(2f);
        Destroy(spawnCanvas);
    }

    private void SetTeleportPoint()
    {
        Transform collect = FindObjectOfType<SpawnController>().GetCurrentCollectableLocation();
        triggerEventPrefab = Instantiate(triggerEventPrefab, collect.transform);
        triggerEventPrefab.transform.localScale = new Vector3(30f, 30f, 30f);
        triggerEventPrefab.transform.position = new Vector3(triggerEventPrefab.transform.position.x, triggerEventPrefab.transform.position.y - 0.7f, triggerEventPrefab.transform.position.z - 2.5f);
        EventController.StartListening(ConstantController.TASK_END_SIGNAL, TeleportPointFound);
    }

    private void TeleportPointFound()
    {
        Destroy(triggerEventPrefab);
        EventController.StopListening(ConstantController.TASK_END_SIGNAL, TeleportPointFound);
    }

    private void SpawnNewCollectable()
    {
        EventController.TriggerEvent(ConstantController.EV_SPAWN_COLLECTABLE);
        isActive = true;
        spawnTimer = 0f;
    }

    public bool GetIsActive() { return isActive; }
    
    public void ItemHasBeenCollected()
    {
        spawnTimer = 0f;
        isActive = false;
        itemCount++;
        if (itemCount >= AMOUNT_TO_WIN) {
            WinTask();
        } else if (itemCount == 1) {
            FindObjectOfType<TaskController>().InitiateTask(TASK_INDEX + 1);
        } else if ( itemCount == 2 ) {
            FindObjectOfType<TaskController>().InitiateTask(TASK_INDEX + 2);
        }
    }

    private void WinTask()
    {
        EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, Mathf.Floor(FindObjectOfType<GameController>().GetGameTimer()));
        EventController.TriggerEvent(ConstantController.TASK_COMPLETE, TASK_INDEX);
    }
}
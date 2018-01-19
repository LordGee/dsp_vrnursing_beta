using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the unique process of task one
/// </summary>
public class Task1 : MonoBehaviour
{
    public GameObject taskCanvas, spawnCanvas, triggerEventPrefab;
    public AudioClip[] voiceOverAudioClips;
    public string[] taskInstructions;
    private bool isActive;
    private int itemCount, voiceIndex;
    private float spawnTimer = 0;
    private const int AMOUNT_TO_WIN = 3, TASK_INDEX = 0;

    /// <summary>
    /// Constructor - sets the initial implmentation of the first task
    /// </summary>
    private void Start() {
        spawnCanvas = GameObject.Find("StartInstructions");
        Instantiate(taskCanvas, spawnCanvas.transform);
        voiceIndex = 0;
        PlayAudioClips();
        isActive = true;
        itemCount = 0;
    }

    /// <summary>
    /// The update function keeps track of the main task timer and determines when a main task item should be spawned
    /// </summary>
    void Update() {
        if (!isActive) {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > 60 || FindObjectOfType<GameController>().GetGameTimer() < 60) {
                SpawnNewCollectable();
            }
        }
    }

    /// <summary>
    /// Plays audio clips from an array in sequential order
    /// </summary>
    private void PlayAudioClips() {
        if (voiceIndex != voiceOverAudioClips.Length) {
            GetComponent<AudioSource>().clip = voiceOverAudioClips[voiceIndex];
            GetComponent<AudioSource>().Play();
            if (voiceIndex < taskInstructions.Length) {
                GameObject.Find("Instructions").GetComponent<Text>().text = taskInstructions[voiceIndex];
            }
            StartCoroutine(PlayNextClip(GetComponent<AudioSource>().clip.length));
            voiceIndex++;
        } else {
            SpawnNewCollectable();
            SetTeleportPoint();
            StartCoroutine(DestroyCanvas());
        }
    }

    /// <summary>
    /// Plays the next clip if audio source is not already playing
    /// </summary>
    /// <param name="_waitFor">Length of current clip</param>
    private IEnumerator PlayNextClip(float _waitFor) {
        yield return  new WaitForSeconds(_waitFor);
        if ( GetComponent<AudioSource>().isPlaying ) {
            StartCoroutine(PlayNextClip(1f));
        } else {
            PlayAudioClips();
        }
    }

    /// <summary>
    /// Destroys the brief canvas after a short period of time
    /// </summary>
    private IEnumerator DestroyCanvas() {
        yield return new WaitForSeconds(2f);
        Destroy(spawnCanvas);
    }

    /// <summary>
    /// Sets a teleport point for the first collectable object (part of tutorial)
    /// </summary>
    private void SetTeleportPoint() {
        Transform collect = FindObjectOfType<SpawnController>().GetCurrentCollectableLocation();
        triggerEventPrefab = Instantiate(triggerEventPrefab, collect.transform);
        triggerEventPrefab.transform.localScale = new Vector3(30f, 30f, 30f);
        triggerEventPrefab.transform.position = new Vector3(triggerEventPrefab.transform.position.x, triggerEventPrefab.transform.position.y - 0.7f, triggerEventPrefab.transform.position.z - 2.5f);
        EventController.StartListening(ConstantController.TASK_END_SIGNAL, TeleportPointFound);
    }

    /// <summary>
    /// Once teleport point has been triggerd by the player it is then destroyed
    /// </summary>
    private void TeleportPointFound() {
        Destroy(triggerEventPrefab);
        EventController.StopListening(ConstantController.TASK_END_SIGNAL, TeleportPointFound);
    }

    /// <summary>
    /// When required will spawn a new main task object to be collected
    /// </summary>
    private void SpawnNewCollectable() {
        EventController.TriggerEvent(ConstantController.EV_SPAWN_COLLECTABLE);
        isActive = true;
        spawnTimer = 0f;
    }

    public bool GetIsActive() { return isActive; }
    
    /// <summary>
    /// Determines what happens once a main task object has been collected
    /// Can initiate new task or side missions
    /// </summary>
    public void ItemHasBeenCollected() {
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

    /// <summary>
    /// Once all main task objects have been collected will trigger end game events
    /// </summary>
    private void WinTask() {
        EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, Mathf.Floor(FindObjectOfType<GameController>().GetGameTimer()));
        EventController.TriggerEvent(ConstantController.TASK_COMPLETE, TASK_INDEX);
    }
}
using System.Collections;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class Task1 : MonoBehaviour
{
    public GameObject taskCanvas, spawnCanvas;
    public AudioClip[] voiceOverAudioClips;
    public string[] taskInstructions;

    private bool isActive;
    private int itemCount, voiceIndex;

    void Start()
    {
        EventController.TriggerEvent(ConstantController.EV_SPAWN_COLLECTABLE);

        spawnCanvas = GameObject.Find("StartInstructions");
        Instantiate(taskCanvas, spawnCanvas.transform);

        voiceIndex = 0;
        PlayAudioClips();

        isActive = true;
        itemCount = 0;
    }

    private void PlayAudioClips()
    {
        if (voiceIndex != voiceOverAudioClips.Length)
        {
            GetComponent<AudioSource>().clip = voiceOverAudioClips[voiceIndex];
            GetComponent<AudioSource>().Play();
            GameObject.Find("Instructions").GetComponent<Text>().text = taskInstructions[voiceIndex];
            StartCoroutine(PlayNextClip(GetComponent<AudioSource>().clip.length));
            voiceIndex++;
        }
        else
        {
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
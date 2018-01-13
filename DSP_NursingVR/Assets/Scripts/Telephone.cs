using System.Collections;
using UnityEngine;

public class Telephone : MonoBehaviour
{
    public AudioClip[] phoneInstructions;

    private AudioSource audio;
    private bool ring;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void StartRinging()
    {
        audio.Play();
        ring = true;
        RepeatRing(audio.clip.length);
    }

    public void StopRinging()
    {
        audio.Stop();
        ring = false;
    }

    private IEnumerator RepeatRing(float _length)
    {
        yield return new WaitForSeconds(_length);
        if (ring) {
            StartRinging();
        }
    }

    public void AnswerThePhone()
    {
        StopRinging();
        if (!FindObjectOfType<Task2>().GetWinStatus())
        {
            audio.clip = phoneInstructions[0];
            audio.Play();
        }
    }
}

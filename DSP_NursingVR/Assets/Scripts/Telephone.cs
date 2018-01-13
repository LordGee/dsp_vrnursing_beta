using System.Collections;
using UnityEngine;

public class Telephone : MonoBehaviour
{
    public AudioClip[] phoneInstructions;
    public AudioClip ringer;

    private AudioSource audio;
    private bool ring, answered;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void StartRinging()
    {
        audio.clip = ringer;
        audio.Play();
        ring = true;
        answered = false;
        RepeatRing(audio.clip.length);
    }

    public void StopRinging()
    {
        audio.Stop();
        ring = false;
        answered = true;
    }

    private IEnumerator RepeatRing(float _length)
    {
        yield return new WaitForSeconds(_length);
        if ( audio.isPlaying )
        {
            RepeatRing(1f);
        }
        else if (ring) {
            StartRinging();
        }
    }

    public void AnswerThePhone()
    {
        if (!answered)
        {
            StopRinging();
            if ( !FindObjectOfType<Task2>().GetWinStatus() )
            {
                audio.clip = phoneInstructions[0];
                audio.Play();
                StartCoroutine(NextClip(1, audio.clip.length));
            }
        }
    }

    private IEnumerator NextClip(int _index, float _length)
    {
        yield return new WaitForSeconds(_length);
        if (audio.isPlaying) {
            NextClip(_index, 1f);
        } else {
            audio.clip = phoneInstructions[_index];
            audio.Play();
        }
    }
}

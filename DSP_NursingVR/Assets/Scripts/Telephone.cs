using System.Collections;
using UnityEngine;
using VRTK;

public class Telephone : MonoBehaviour
{
    public AudioClip[] phoneInstructions;
    public AudioClip ringer;

    private new AudioSource audio;
    private bool ring, answered;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        StartCheckingCollisions(false);
    }

    public void StartCheckingCollisions(bool _bool)
    {
        GetComponent<Rigidbody>().detectCollisions = _bool;
    }

    public void StartRinging()
    {
        StartCheckingCollisions(true);
        audio.clip = ringer;
        audio.Play();
        ring = true;
        answered = false;
        StartCoroutine(RepeatRing(audio.clip.length));
    }

    public void StopRinging()
    {
        StartCheckingCollisions(false);
        audio.Stop();
        ring = false;
        answered = true;
    }

    private IEnumerator RepeatRing(float _length)
    {
        yield return new WaitForSeconds(_length);
        if ( audio.isPlaying ) {
            StartCoroutine(RepeatRing(1f));
        } else if (ring) {
            StartRinging();
        }
    }

    void OnTriggerStay(Collider _col)
    {
        if ( _col.transform.parent.transform.parent.tag == "Player" && ring && FindObjectOfType<PlayerControllers>().CheckGripPressed())
        {
            AnswerThePhone();
        }
    }

    public void AnswerThePhone()
    {
        if ( !answered )
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
            StartCoroutine(NextClip(_index, 1f));
        } else {
            audio.clip = phoneInstructions[_index];
            audio.Play();
        }
    }
}

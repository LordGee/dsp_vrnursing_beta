using System.Collections;
using UnityEngine;
using VRTK;

public class Telephone : MonoBehaviour
{
    public AudioClip[] phoneInstructions;
    public AudioClip ringer;

    private new AudioSource audio;
    private bool ring, answered, call;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        CheckingCollisions(false);
        call = false;
    }

    public void CheckingCollisions(bool _bool)
    {
        GetComponent<Rigidbody>().detectCollisions = _bool;
    }

    public void StartRinging()
    {
        CheckingCollisions(true);
        audio.clip = ringer;
        audio.Play();
        audio.loop = true;
        ring = true;
        answered = false;
    }

    public void StopRinging()
    {
        CheckingCollisions(false);
        audio.Stop();
        audio.loop = false;
        ring = false;
        answered = true;
    }

    /* private IEnumerator RepeatRing(float _length)
    {
        yield return new WaitForSeconds(_length);
        if ( audio.isPlaying ) {
            StartCoroutine(RepeatRing(1f));
        } else if (ring) {
            StartRinging();
        }
    }*/

    void OnTriggerStay(Collider _col)
    {
        if ( _col.transform.parent.transform.parent.tag == "Player" && FindObjectOfType<PlayerControllers>().CheckGripPressed())
        {
            if (ring) {
                if (!call) {
                    AnswerThePhone();
                } else {
                    AnswerThePhone();
                }
            } else if (call) {
                MakeTheCall();
            }
        }
    }

    public void AnswerThePhone()
    {
        if ( !answered ) {
            StopRinging();
            if ( !call ) {
                audio.clip = phoneInstructions[0];
                audio.Play();
                StartCoroutine(NextClip(1, audio.clip.length));
                FindObjectOfType<Task2>().StartTask();
            } else {
                audio.clip = phoneInstructions[5];
                audio.Play();
                WinTask2(0f);
            }
        }
    }

    private void MakeTheCall()
    {
        ring = answered = call = false;
        CheckingCollisions(false);
        audio.clip = phoneInstructions[2];
        audio.Play();
        StartCoroutine(NextClip(3, audio.clip.length + 1f));
        WinTask2(50f);
    }

    public void PrepareReturnCall()
    {
        CheckingCollisions(true);
        call = true;
    }

    public void PrepareFailedCall()
    {
        StartRinging();
        call = true;
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

    private void WinTask2(float _bonus)
    {
        EventController.TriggerEvent(ConstantController.TASK_WIN, _bonus);
    }
}

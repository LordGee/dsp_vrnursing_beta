using System.Collections;
using UnityEngine;

/// <summary>
/// Handles all the telephone interaction and collisions
/// </summary>
public class Telephone : MonoBehaviour
{
    public AudioClip[] phoneInstructions;
    public AudioClip ringer;
    private new AudioSource audio;
    private bool ring, answered, call;

    void Start() {
        audio = GetComponent<AudioSource>();
        CheckingCollisions(false);
        call = false;
    }

    /// <summary>
    /// Allows to switch on / off collision detection, so its only active when required
    /// </summary>
    /// <param name="_bool">Check collision?</param>
    public void CheckingCollisions(bool _bool) {
        GetComponent<Rigidbody>().detectCollisions = _bool;
    }

    /// <summary>
    /// Starts the phone ringing, and prepares the phone to be answered.
    /// </summary>
    public void StartRinging() {
        CheckingCollisions(true);
        audio.clip = ringer;
        audio.Play();
        audio.loop = true;
        ring = true;
        answered = false;
    }

    /// <summary>
    /// Stops the phone ringing and prevents further collisions.
    /// </summary>
    public void StopRinging() {
        CheckingCollisions(false);
        audio.Stop();
        audio.loop = false;
        ring = false;
        answered = true;
    }

    /// <summary>
    /// Collision detection with player t answer or make a call, depending on the task state.
    /// </summary>
    void OnTriggerStay(Collider _col) {
        if ( _col.transform.parent.transform.parent.tag == "Player" && FindObjectOfType<PlayerControllers>().CheckGripPressed()) {
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

    /// <summary>
    /// After answering the phone this function ensures the correct audio is played
    /// </summary>
    public void AnswerThePhone() {
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

    /// <summary>
    /// After answering the phone this function ensures the correct audio is played
    /// </summary>
    private void MakeTheCall() {
        ring = answered = call = false;
        CheckingCollisions(false);
        audio.clip = phoneInstructions[2];
        audio.Play();
        StartCoroutine(NextClip(3, audio.clip.length + 1f));
        WinTask2(50f);
    }

    /// <summary>
    /// prepares the phone ready to make a call.
    /// </summary>
    public void PrepareReturnCall() {
        CheckingCollisions(true);
        call = true;
    }

    /// <summary>
    /// If player fails to complete task this function prepares a call response.
    /// </summary>
    public void PrepareFailedCall() {
        StartRinging();
        call = true;
    }

    /// <summary>
    /// Plays next clip in sequence, determined by index value and length of previous audio.
    /// </summary>
    /// <param name="_index">Next index value</param>
    /// <param name="_length">Time of current clip playing</param>
    private IEnumerator NextClip(int _index, float _length) {
        yield return new WaitForSeconds(_length);
        if (audio.isPlaying) {
            StartCoroutine(NextClip(_index, 1f));
        } else {
            audio.clip = phoneInstructions[_index];
            audio.Play();
        }
    }

    /// <summary>
    /// Once task is one informs the task controller that the task is complete.
    /// </summary>
    /// <param name="_bonus"></param>
    private void WinTask2(float _bonus) {
        EventController.TriggerEvent(ConstantController.TASK_WIN, _bonus);
    }
}
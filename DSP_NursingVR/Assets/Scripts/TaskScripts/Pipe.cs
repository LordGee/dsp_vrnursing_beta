using System.Collections;
using UnityEngine;

/// <summary>
/// Handles each individual pipe piece, colour, rotation and position.
/// </summary>
public class Pipe : MonoBehaviour {

    public bool connectedSupply;
    public AudioClip turn, connect;
    public int rowIndex, colIndex;
    public ConstantController.PIPE_PIECES pipeType;
    private Color currentColour, startColour;
    private bool triggerOnce = false, wait = false;

    void Awake() {
        startColour = gameObject.GetComponentInChildren<MeshRenderer>().material.color;
    }

    /// <summary>
    /// Sets the colour of the Pipe Piece depending on state
    /// </summary>
    /// <param name="_colour">Desired colour</param>
    public void SetColour(Color _colour) {
        currentColour = _colour;
        MeshRenderer[] mr = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in mr) {
            if (m.tag == ConstantController.TAG_PIPE_PART) {
                m.material.color = currentColour;
            }
        }
    }

    public bool CheckConnectedSupply() { return connectedSupply; }
    
    /// <summary>
    /// sets the colour of the pipe piece if it is connected to another positive pipe piece
    /// </summary>
    public void Connected() {
        if (pipeType == ConstantController.PIPE_PIECES.Game) {
            SetColour(Color.green);
            if (!connectedSupply) {
                connectedSupply = true;
                PlaySFX(connect);
            }
        } else if (pipeType == ConstantController.PIPE_PIECES.Finish) {
            if (!triggerOnce) {
                EventController.TriggerEvent(ConstantController.TASK_WIN);
                triggerOnce = true;
                PlaySFX(connect);
            }
        }
    }

    /// <summary>
    /// Sets the colour back to default if it is disconnected for a positive source.
    /// </summary>
    public void Disconnected()     {
        if (pipeType == ConstantController.PIPE_PIECES.Game) {
            SetColour(startColour);
            connectedSupply = false;
        }
    }

    public void PipeSelected() { RotateAntiClockwise(); }

    /// <summary>
    /// Rotate the pipe piece in a clockwise rotation
    /// </summary>
    public void RotateClockwise()     {
        if (!wait) {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90f);
            PlaySFX(turn);
            wait = true;
            StartCoroutine(Wait());
        }
    }

    /// <summary>
    /// Rotate the pipe piece in a anti-clockwise rotation
    /// </summary>
    public void RotateAntiClockwise() {
        if (!wait) {
            PlaySFX(turn);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 90f);
            wait = true;
            StartCoroutine(Wait());
        }
    }

    /// <summary>
    /// Short delay to prevent too many rotations
    /// </summary>
    private IEnumerator Wait() {
        yield return new WaitForSeconds(0.1f);
        wait = false;
    }

    /// <summary>
    /// Sets n initial rotation for a random amount of times when spawning
    /// </summary>
    public void InitialRotation() {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90f);
    }

    public ConstantController.PIPE_PIECES GetPipeType() { return pipeType; }

    /// <summary>
    /// Adds gravity and removes constraints so the objects can fall after the game is completed
    /// </summary>
    public void AddGravity() {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    /// <summary>
    /// Plays audio clip
    /// </summary>
    /// <param name="_clip">Desired audio clip</param>
    private void PlaySFX(AudioClip _clip) {
        GetComponent<AudioSource>().clip = _clip;
        GetComponent<AudioSource>().Play();
    }
}
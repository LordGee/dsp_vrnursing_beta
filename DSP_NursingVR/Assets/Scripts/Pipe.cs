﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {

    public bool connectedSupply;
    private Color currentColour, startColour;
    public int rowIndex, colIndex;
    public ConstantController.PIPE_PIECES pipeType;

    void Awake()
    {
        // startColour = new Color(255f, 204f, 112f);
        startColour = gameObject.GetComponentInChildren<MeshRenderer>().material.color;
    }

    public void SetColour(Color _colour)
    {
        currentColour = _colour;
        MeshRenderer[] mr = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in mr) {
            if (m.tag == ConstantController.TAG_PIPE_PART) {
                m.material.color = currentColour;
            }
        }
    }

    public bool CheckConnectedSupply()
    {
        return connectedSupply;
    }

    public bool triggerOnce = false;
    public void Connected()
    {
        if (pipeType == ConstantController.PIPE_PIECES.Game)
        {
            SetColour(Color.green);
            connectedSupply = true;
        } else if (pipeType == ConstantController.PIPE_PIECES.Finish) {
            if (!triggerOnce)
            {
                EventController.TriggerEvent(ConstantController.TASK_WIN);
                triggerOnce = true;
            }
        }
    }

    public void Disconnected()
    {
        if (pipeType == ConstantController.PIPE_PIECES.Game) {
            SetColour(startColour);
            connectedSupply = false;
        }
    }

    private bool wait = false;
    public void RotateClockwise()
    {
        if (!wait)
        {
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 90f);
            wait = true;
            StartCoroutine(Wait());
        }
        
    }

    public void RotateAntiClockwise() {
        if (!wait) {
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z - 90f);
            wait = true;
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(1f);
        wait = false;
    }

    public void InitialRotation()
    {
        transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 90f);
    }

    public ConstantController.PIPE_PIECES GetPipeType()
    {
        return pipeType;
    }

    public void AddGravity()
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        //GetComponent<Rigidbody>().freezeRotation = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
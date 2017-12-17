using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TriggerEventStart : MonoBehaviour
{

    private bool triggerActivated;
	
	void Start ()
	{
	    triggerActivated = false;
	}

    void OnTriggerStay(Collider _col)
    {
        if (_col.transform.tag == "Player")
        {
            triggerActivated = true;
        }
    }

    public bool HasTriggerActivated()
    {
        return triggerActivated;
    }
}

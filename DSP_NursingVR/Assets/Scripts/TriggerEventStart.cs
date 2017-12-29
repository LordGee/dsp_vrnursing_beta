using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;
using Valve.VR.InteractionSystem;
using VRTK;

public class TriggerEventStart : MonoBehaviour
{

    private bool triggerActivated;
	
	void Start ()
	{
	    triggerActivated = false;
	}

    void OnTriggerEnter(Collider _col)
    {
        if (_col.name == "[VRTK][AUTOGEN][FootColliderContainer]" )
        {
            triggerActivated = true;
            EventController.TriggerEvent(ConstantController.TASK_END_SIGNAL);
        }
    }

    void OnCollisionEnter(Collision _col)
    {
        if ( _col.transform.name == "[VRTK][AUTOGEN][FootColliderContainer]" )
        {
            triggerActivated = true;
            EventController.TriggerEvent(ConstantController.TASK_END_SIGNAL);
        }
    }

    public bool HasTriggerActivated()
    {
        return triggerActivated;
    }
}

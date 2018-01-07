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
        if (_col.name == ConstantController.GO_FOOT_COLLIDER )
        {
            triggerActivated = true;
            EventController.TriggerEvent(ConstantController.TASK_END_SIGNAL);
        }
    }

    void OnCollisionEnter(Collision _col)
    {
        if ( _col.transform.name == ConstantController.GO_FOOT_COLLIDER )
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

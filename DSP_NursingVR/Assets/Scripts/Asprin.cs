using System;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Asprin : MonoBehaviour
{
    public void CollectedAsprin()
    {
        FindObjectOfType<SpawnController>().CollectObject();
    }


    /*
    private bool test = false;

    void OnTriggerEnter(Collider _col)
    {
        test = !test;
    }

    void OnTriggerExit(Collider _col)
    {
        test = !test;
    }

    void OnTriggerStay(Collider _col)
    {
        if (test && _col.transform.parent.transform.parent.tag == "Player" )
        {
            if ( _col.transform.parent.transform.parent.GetComponent<PlayerControllers>().CheckGripPressed() )
            {
                FindObjectOfType<SpawnController>().CollectObject();
            }
        }
    }
    */
}
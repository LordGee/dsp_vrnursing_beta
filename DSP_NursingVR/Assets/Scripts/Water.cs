﻿using UnityEngine;
using Valve.VR.InteractionSystem;

public class Water : MonoBehaviour
{
    void OnTriggerStay(Collider _col)
    {
        if ( _col.transform.parent.transform.parent.tag == "Player" )
        {
            if ( _col.transform.parent.transform.parent.GetComponent<PlayerControllers>().CheckGripPressed() )
            {
                FindObjectOfType<FoodDrinkController>().ConsumeWater();
            }
        }
    }
}
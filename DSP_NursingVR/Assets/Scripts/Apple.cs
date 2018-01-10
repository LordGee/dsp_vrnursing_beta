using UnityEngine;
using Valve.VR.InteractionSystem;

public class Apple : MonoBehaviour
{
    public void AppleEaten()
    {
        FindObjectOfType<SpawnController>().ConsumeFood();
    }

    /*
    void OnTriggerStay(Collider _col)
    {
        if ( _col.transform.parent.transform.parent.tag == "Player" )
        {
            if ( _col.transform.parent.transform.parent.GetComponent<PlayerControllers>().CheckGripPressed() )
            {
                FindObjectOfType<SpawnController>().ConsumeFood();
            }
        }
    }
    */
}
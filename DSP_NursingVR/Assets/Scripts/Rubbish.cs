using UnityEngine;
using VRTK;

public class Rubbish : MonoBehaviour
{
    void OnCollisionEnter(Collision _col)
    {
        if ( _col != null && _col.transform.name == "pasted__gaizi" )
        {
            if ( !GetComponent<VRTK_InteractableObject>().IsGrabbed() )
            {
                FindObjectOfType<Task2>().UpdateObjectResult(1);
                EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, 40f);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }
}
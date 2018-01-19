using UnityEngine;
using VRTK;

/// <summary>
/// Tests if the bottle has been positioned in the correct place
/// </summary>
public class Rubbish : MonoBehaviour
{
    public AudioClip clip;

    void OnCollisionEnter(Collision _col) {
        if ( _col != null && _col.transform.name == "pasted__gaizi" ) {
            if ( !GetComponent<VRTK_InteractableObject>().IsGrabbed() ) {
                FindObjectOfType<Task2>().UpdateObjectResult(1);
                FindObjectOfType<Task2>().PlaySFX(clip);
                EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, 40f);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }
}
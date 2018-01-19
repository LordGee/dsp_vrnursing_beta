using UnityEngine;
using VRTK;

/// <summary>
/// Tests if the bottle has been positioned in the correct place
/// </summary>
public class Monitor : MonoBehaviour
{
    public AudioClip clip;

    /// <summary>
    /// Tests for collision and resets the objects position if collision is detected in the correct place
    /// </summary>
    /// <param name="_col"></param>
    void OnCollisionEnter(Collision _col) {
        if (_col != null && _col.transform.name == "SideUnit") {
            if ( !GetComponent<VRTK_InteractableObject>().IsGrabbed() ) {
                transform.localPosition = new Vector3(0f, 0.015f, 0f);
                transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                GetComponent<MeshCollider>().enabled = false;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().isKinematic = true;
                FindObjectOfType<Task2>().UpdateObjectResult(0);
                FindObjectOfType<Task2>().PlaySFX(clip);
                EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, 40f);
            }
        }
    }
}
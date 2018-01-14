using UnityEngine;
using VRTK;

public class Monitor : MonoBehaviour
{
    public AudioClip clip;

    void OnCollisionEnter(Collision _col)
    {
        if (_col != null && _col.transform.name == "SideUnit")
        {
            if ( !GetComponent<VRTK_InteractableObject>().IsGrabbed() )
            {
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
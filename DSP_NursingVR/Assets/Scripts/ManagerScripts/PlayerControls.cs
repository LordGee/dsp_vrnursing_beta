using UnityEngine;
using Valve.VR;

/// <summary>
/// Obsolete - most functions have been moved to the 
/// PlayerControllers.cs script
/// </summary>
public class PlayerControls : MonoBehaviour
{
    private EVRButtonId triggerButton = EVRButtonId.k_EButton_SteamVR_Trigger;

    private EVRButtonId gripButton = EVRButtonId.k_EButton_Grip;

    private EVRButtonId menuButton = EVRButtonId.k_EButton_ApplicationMenu;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device controller
        { get { return SteamVR_Controller.Input((int) trackedObj.index); } }

    private GameObject pickup;

    private RaycastHit pipeHit;
    private bool pipeSelected;

    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        if (controller == null)
        {
            Debug.LogWarning("Controller not initilsed!");
            return;
        }

        if (controller.GetPressDown(gripButton) && pickup != null)
        {
            pickup.transform.parent = transform;
            pickup.GetComponent<Rigidbody>().isKinematic = true;

            if (pickup.transform.parent.tag == "Consume") {
                // access gaze, hold for 2 seconds... then destroy obj, replen, etc...
            }
            
            Debug.Log("Picked Up " + pickup.name);
        }
        if (controller.GetPressUp(gripButton) && pickup != null)
        {
            pickup.transform.parent = null;
            pickup.GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("Put Down " + pickup.name);
            pickup = null;
        }

        if (controller.GetPressDown(menuButton))
        {
            if (CanvasController.canvasStatus)
            {
                EventController.TriggerEvent(ConstantController.EV_CLOSE_STATUS_CANVAS);
            }
            else
            {
                EventController.TriggerEvent(ConstantController.EV_OPEN_STATUS_CANVAS);
            }
        }
        

        if (FindObjectOfType<PipeManager>() != null)
        {
            if (controller.GetPressDown(triggerButton))
            {
                RaycastHit hitInfo = new RaycastHit();
                Ray ray = new Ray(this.transform.position, transform.forward);
                Physics.Raycast(ray, out hitInfo, 100f);
                if (hitInfo.transform.tag != null && hitInfo.transform.tag == ConstantController.TAG_PIPES)
                {
                    pipeSelected = true;
                    pipeHit = hitInfo;
                }
            }

            if (controller.GetPressUp(triggerButton))
            {
                pipeSelected = false;
            }

            if (transform.rotation.z > 0.2f && pipeSelected)
            {
                pipeHit.transform.GetComponent<Pipe>().RotateClockwise();
            }
            else if (transform.rotation.z < -0.2f && pipeSelected)
            {
                pipeHit.transform.GetComponent<Pipe>().RotateAntiClockwise();
            }
        }
    }

    void OnTriggerEnter(Collider _col)
    {
        Debug.Log(_col.gameObject.tag);
        if (PickupObjectsFind(_col.gameObject.tag))
        {
            pickup = _col.gameObject;
        }
    }

    void OnTriggerExit(Collider _col)
    {
        // pickup = null;
    }

    private bool PickupObjectsFind(string _tag)
    {
        for (int i = 0; i < ConstantController.pickupObjects.Length; i++)
        {
            if (ConstantController.pickupObjects[i] == _tag)
            {
                return true;
            }
        }
        return false;
    }

    private void SelectedPipesRotation(RaycastHit _hit)
    {
        Debug.Log(_hit.transform.name);
    }

    public bool CheckGripPressed()
    {
        if (controller.GetPressDown(gripButton))
        {
            return true;
        }
        return false;
    }
}
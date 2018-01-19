using UnityEngine;
using VRTK;

/// <summary>
/// Handles the controller interactions, some of these actions have been moved to others areas.
/// </summary>
public class PlayerControllers : MonoBehaviour
{
    private void Start() {
        if ( GetComponent<VRTK_ControllerEvents>() == null ) {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "PlayerControllers", "VRTK_ControllerEvents", "the same"));
            return;
        }
        //Setup controller event listeners
        GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);
        GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerInteractionEventHandler(DoTriggerUnclicked);
        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);
        GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += new ControllerInteractionEventHandler(DoButtonTwoPressed);
    }

    /* Trigger Button */
    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e) {
        /* if ( FindObjectOfType<PipeManager>() != null ) {
            PipeTriggerClicked();
        } */
    }

    private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e) {
        /* if (FindObjectOfType<PipeManager>() != null) {
            PipeTriggerUnclicked();
        } */
    }

    /* Grip Button */
    private void DoGripPressed(object sender, ControllerInteractionEventArgs e) {
        gripPressed = true;
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e) {
        gripPressed = false;
    }

    /* Button Two */
    private void DoButtonTwoPressed(object sender, ControllerInteractionEventArgs e) {
        CanvasStatusAction();
    }

    /************************************/
    /* Functions for controller actions */
    /************************************/

    private bool pipeSelected, gripPressed;
    private RaycastHit pipeHit;

    private void CanvasStatusAction() {
        if ( CanvasController.canvasStatus ) {
            EventController.TriggerEvent(ConstantController.EV_CLOSE_STATUS_CANVAS);
        } else {
            EventController.TriggerEvent(ConstantController.EV_OPEN_STATUS_CANVAS);
        }
    }

    private void PipeTriggerClicked() {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = new Ray(this.transform.position, transform.forward);
        Physics.Raycast(ray, out hitInfo, 100f);
        if ( hitInfo.transform.tag != null && hitInfo.transform.tag == ConstantController.TAG_PIPES ) {
            pipeSelected = true;
            pipeHit = hitInfo;
        }
        if ( transform.rotation.z > 0f && pipeSelected ) {
            pipeHit.transform.GetComponent<Pipe>().RotateClockwise();
        } else if ( transform.rotation.z < -0f && pipeSelected ) {
            pipeHit.transform.GetComponent<Pipe>().RotateAntiClockwise();
        }
    }

    private void PipeTriggerUnclicked() { pipeSelected = false; }

    public bool CheckGripPressed() { return gripPressed; }
}

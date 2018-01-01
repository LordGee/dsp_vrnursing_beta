using UnityEngine;
using VRTK;

public class PlayerControllers : MonoBehaviour
{
    private void Start()
    {
        if ( GetComponent<VRTK_ControllerEvents>() == null )
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "PlayerControllers", "VRTK_ControllerEvents", "the same"));
            return;
        }

        //Setup controller event listeners
        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

        GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);
        GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerInteractionEventHandler(DoTriggerUnclicked);

        GetComponent<VRTK_ControllerEvents>().TriggerAxisChanged += new ControllerInteractionEventHandler(DoTriggerAxisChanged);

        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);

        GetComponent<VRTK_ControllerEvents>().GripClicked += new ControllerInteractionEventHandler(DoGripClicked);
        GetComponent<VRTK_ControllerEvents>().GripUnclicked += new ControllerInteractionEventHandler(DoGripUnclicked);

        GetComponent<VRTK_ControllerEvents>().GripAxisChanged += new ControllerInteractionEventHandler(DoGripAxisChanged);

        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
        GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);

        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

        GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);

        GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(DoButtonOnePressed);
        GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DoButtonOneReleased);

        GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += new ControllerInteractionEventHandler(DoButtonTwoPressed);
        GetComponent<VRTK_ControllerEvents>().ButtonTwoReleased += new ControllerInteractionEventHandler(DoButtonTwoReleased);

        GetComponent<VRTK_ControllerEvents>().StartMenuPressed += new ControllerInteractionEventHandler(DoStartMenuPressed);
        GetComponent<VRTK_ControllerEvents>().StartMenuReleased += new ControllerInteractionEventHandler(DoStartMenuReleased);

    }

    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {
        VRTK_Logger.Info("Controller on index '" + index + "' " + button + " has been " + action
                + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    /* Trigger Button */
    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "pressed", e);
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "released", e);
    }

    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        if ( FindObjectOfType<PipeManager>() != null )
        {
            PipeTriggerClicked();
        }
    }

    private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        if (FindObjectOfType<PipeManager>() != null)
        {
            PipeTriggerUnclicked();
        }
    }

    private void DoTriggerAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "axis changed", e);
    }

    /* Grip Button */
    private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "pressed", e);
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "released", e);
    }

    private void DoGripClicked(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "clicked", e);
    }

    private void DoGripUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "unclicked", e);
    }

    private void DoGripAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "axis changed", e);
    }

    /* Touchpad Button */
    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "pressed down", e);
    }

    private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "released", e);
    }

    private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "touched", e);
    }

    private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "untouched", e);
    }

    private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "axis changed", e);
    }

    /* Button One */
    private void DoButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON ONE", "pressed down", e);
    }

    private void DoButtonOneReleased(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON ONE", "released", e);
    }

    /* Button Two */
    private void DoButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        CanvasStatusAction();
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON TWO", "pressed down", e);
    }

    private void DoButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON TWO", "released", e);
    }

    /* Menu Button */
    private void DoStartMenuPressed(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "START MENU", "pressed down", e);
    }

    private void DoStartMenuReleased(object sender, ControllerInteractionEventArgs e)
    {
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "START MENU", "released", e);
    }


    /************************************/
    /* Functions for controller actions */
    /************************************/

    private bool pipeSelected;
    private RaycastHit pipeHit;

    private void CanvasStatusAction()
    {
        if ( CanvasController.canvasStatus )
        {
            Debug.Log("De-activate menu");
            EventController.TriggerEvent(ConstantController.EV_CLOSE_STATUS_CANVAS);
        } else
        {
            Debug.Log("Activate menu");
            EventController.TriggerEvent(ConstantController.EV_OPEN_STATUS_CANVAS);
        }
    }

    private void PipeTriggerClicked()
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = new Ray(this.transform.position, transform.forward);
        Physics.Raycast(ray, out hitInfo, 100f);
        if ( hitInfo.transform.tag != null && hitInfo.transform.tag == ConstantController.TAG_PIPES )
        {
            pipeSelected = true;
            pipeHit = hitInfo;
        }
        if ( transform.rotation.z > 0.2f && pipeSelected )
        {
            pipeHit.transform.GetComponent<Pipe>().RotateClockwise();
        } else if ( transform.rotation.z < -0.2f && pipeSelected )
        {
            pipeHit.transform.GetComponent<Pipe>().RotateAntiClockwise();
        }
    }

    private void PipeTriggerUnclicked()
    {
        pipeSelected = false;
    }


}

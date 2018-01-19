using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles diffeerent button presses and display of the task acceptor machine
/// </summary>
public class ButtonPress : MonoBehaviour
{
    public int buttonNumber;
    private Animator anim;
    private Text display;
    private string[] textOptions = {"Want?", "Take", "Take", "Ignore" };

    void Start() {
        anim = GetComponentInParent<Animator>();
        display = GameObject.Find("ChoiceText").GetComponent<Text>();
        display.text = textOptions[0];
    }

    /// <summary>
    /// When button is pressed display is updated and button option is stored and executes the correct animation
    /// </summary>
    public void PressButton() {
        anim.SetTrigger("PushButton" + buttonNumber + "Trigger");
        display.text = textOptions[buttonNumber];
        EventController.TriggerEvent(ConstantController.TASK_SELECTION_OPTION, buttonNumber);
    }
}



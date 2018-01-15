using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour {

    public int buttonNumber;

    private Animator anim;
    private Text display;
    private string[] textOptions = {"Want?", "Take", "Take", "Ignore" };

    void Start() {
        anim = GetComponentInParent<Animator>();
        display = GameObject.Find("ChoiceText").GetComponent<Text>();
        display.text = textOptions[0];
    }

    public void PressButton()
    {
        anim.SetTrigger("PushButton" + buttonNumber + "Trigger");
        display.text = textOptions[buttonNumber];
        EventController.TriggerEvent(ConstantController.TASK_SELECTION_OPTION, buttonNumber);
    }
}



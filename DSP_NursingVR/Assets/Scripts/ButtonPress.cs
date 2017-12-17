using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour {

    public int buttonNumber;

    private Animator anim;
    private Text display;
    private string[] textOptions = {"Want?", "Take", "Ignore", "Delegate" };

    void Start() {
        anim = GetComponentInParent<Animator>();
        display = GameObject.Find("ChoiceText").GetComponent<Text>();
        display.text = textOptions[0];
    }

    void OnTriggerStay(Collider _col)
    {
        if (_col.gameObject.tag == "Player") {
            if (_col.gameObject.GetComponent<PlayerControls>().CheckGripPressed()) {
                anim.SetTrigger("PushButton" + buttonNumber + "Trigger");
                display.text = textOptions[buttonNumber];
                EventController.TriggerEvent(ConstantController.TASK_SELECTION_OPTION, buttonNumber);
            }
        }
    }
}



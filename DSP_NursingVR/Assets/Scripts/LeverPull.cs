using System.Collections;
using UnityEngine;

public class LeverPull : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    void OnTriggerStay(Collider _col)
    {
        if (_col.gameObject.tag == "Player")
        {
            if (_col.gameObject.GetComponent<PlayerControls>().CheckGripPressed())
            {
                anim.SetTrigger("LeverPullTrigger");
                StartCoroutine(WaitForAnimation());
            }
        }
    }

    public IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(3f);
        EventController.TriggerEvent(ConstantController.TASK_ACCEPT_OPTION);
    }

}


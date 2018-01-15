using System.Collections;
using UnityEngine;

public class LeverPull : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    public void PullTheLever()
    {
        anim.SetTrigger("LeverPullTrigger");
        StartCoroutine(WaitForAnimation());
    }

    public IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(3f);
        EventController.TriggerEvent(ConstantController.TASK_ACCEPT_OPTION);
    }

}


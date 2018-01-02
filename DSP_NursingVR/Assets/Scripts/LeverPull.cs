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
        if ( _col.transform.parent.transform.parent.tag == "Player" )
        {
            if ( _col.transform.parent.transform.parent.GetComponent<PlayerControllers>().CheckGripPressed() )
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


using System.Collections;
using UnityEngine;

public class Telephone : MonoBehaviour
{
    public void StartRinging()
    {
        GetComponent<AudioSource>().Play();
    }

    public void StopRinging()
    {
        GetComponent<AudioSource>().Stop();
    }

    private IEnumerator RepeatRing(float _length)
    {
        yield return new WaitForSeconds(_length);
        StartRinging();
    }
}

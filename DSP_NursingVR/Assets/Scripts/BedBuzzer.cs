using System;
using System.Collections.Generic;
using UnityEngine;

public class BedBuzzer : MonoBehaviour
{
    void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying && GetComponent<Light>().range > 0.1f)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
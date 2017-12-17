using System;
using System.Collections.Generic;
using UnityEngine;

public class BedBuzzer : MonoBehaviour
{
    private AudioSource audio;
    private float light;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audio.isPlaying && GetComponent<Light>().range > 0.1f)
        {
            audio.Play();
        }
    }
}
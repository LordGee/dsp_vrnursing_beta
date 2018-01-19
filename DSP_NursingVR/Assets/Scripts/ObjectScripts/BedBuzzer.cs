using UnityEngine;

/// <summary>
/// Play audio buzzer in time with the flashing light
/// </summary>
public class BedBuzzer : MonoBehaviour
{
    void Update() {
        if (!GetComponent<AudioSource>().isPlaying && GetComponent<Light>().range > 0.1f) {
            GetComponent<AudioSource>().Play();
        }
    }
}
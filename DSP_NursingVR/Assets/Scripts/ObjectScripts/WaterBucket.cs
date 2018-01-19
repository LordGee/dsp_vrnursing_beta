using UnityEngine;

public class WaterBucket : MonoBehaviour
{
    public void HazardRemoved()     {
        FindObjectOfType<SpawnController>().RemoveHazard();
    }
}
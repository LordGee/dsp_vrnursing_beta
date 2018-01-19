using UnityEngine;

public class Scissors : MonoBehaviour
{
    public void HazardRemoved()     {
        FindObjectOfType<SpawnController>().RemoveHazard();
    }
}
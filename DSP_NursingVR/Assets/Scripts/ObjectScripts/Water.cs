using UnityEngine;

public class Water : MonoBehaviour
{
    public void WaterDrank()     {
        FindObjectOfType<SpawnController>().ConsumeWater();
    }
}
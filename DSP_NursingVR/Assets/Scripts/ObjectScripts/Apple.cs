using UnityEngine;

public class Apple : MonoBehaviour
{
    public void AppleEaten()     {
        FindObjectOfType<SpawnController>().ConsumeFood();
    }
}
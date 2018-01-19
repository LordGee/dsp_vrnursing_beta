using UnityEngine;

public class Asprin : MonoBehaviour
{
    public void CollectedAsprin()     {
        FindObjectOfType<SpawnController>().CollectObject();
    }
}
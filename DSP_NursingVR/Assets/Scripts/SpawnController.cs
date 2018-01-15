using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

    [SerializeField] public GameObject waterObject, foodObject, collectableObject;
    public GameObject[] hazards;
    public AudioClip drinking, eating, collecting, thud;
    public Transform collectableSpawnPoint;

    private GameObject currentWater, currentFood, currentCollectable, currentHazard;
    private Transform[] spawnPoints;
    private bool[] spawnActive;
    private int currentIndex = -1, currentWaterIndex = -1, currentFoodIndex = -1, currentCollectableIndex = -1, currentHazardIndex = -1;

    void Start()
    {
        GameObject spawn = GameObject.Find("SpawnPoints");
        spawnPoints = spawn.GetComponentsInChildren<Transform>();
        spawnActive = new bool[spawnPoints.Length];
        for ( int i = 0; i < spawnActive.Length; i++ )
        {
            spawnActive[i] = false;
        }
    }

    private void OnEnable()
    {
        EventController.StartListening(ConstantController.EV_SPAWN_WATER, SpawnWater);
        EventController.StartListening(ConstantController.EV_SPAWN_FOOD, SpawnFood);
        EventController.StartListening(ConstantController.EV_SPAWN_COLLECTABLE, SpawnCollectable);
        EventController.StartListening(ConstantController.EV_SPAWN_HAZARD, SpawnNewHazard);
    }

    private void OnDisable()
    {
        EventController.StopListening(ConstantController.EV_SPAWN_WATER, SpawnWater);
        EventController.StopListening(ConstantController.EV_SPAWN_FOOD, SpawnFood);
        EventController.StopListening(ConstantController.EV_SPAWN_COLLECTABLE, SpawnCollectable);
        EventController.StopListening(ConstantController.EV_SPAWN_HAZARD, SpawnNewHazard);
    }

    private void SpawnWater()
    {
        SpawnObject(waterObject, ref currentWater);
        currentWaterIndex = currentIndex;
    }

    private void SpawnFood()
    {
        SpawnObject(foodObject, ref currentFood);
        currentFoodIndex = currentIndex;
    }

    private void SpawnCollectable()
    {
        currentCollectable = Instantiate(collectableObject, collectableSpawnPoint.position, Quaternion.identity);
    }

    private void SpawnNewHazard()
    {
        SpawnObject(hazards[Random.Range(0, hazards.Length)], ref currentHazard);
        currentHazardIndex = currentIndex;
    }

    public void ConsumeWater()
    {
        EndObject(ref currentWater, ref currentWaterIndex, ConstantController.EV_DRINK, 12f);
        GetComponent<AudioSource>().clip = drinking;
        GetComponent<AudioSource>().Play();
    }

    public void ConsumeFood()
    {
        EndObject(ref currentFood, ref currentFoodIndex, ConstantController.EV_EAT, 25f);
        GetComponent<AudioSource>().clip = eating;
        GetComponent<AudioSource>().Play();
    }

    public void CollectObject()
    {
        Destroy(currentCollectable);
        EventController.TriggerEvent(ConstantController.EV_COLLECTED);
        EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, 100f);
        GetComponent<AudioSource>().clip = collecting;
        GetComponent<AudioSource>().Play();
        FindObjectOfType<Task1>().ItemHasBeenCollected();
    }

    public void RemoveHazard()
    {
        EndObject(ref currentHazard, ref currentHazardIndex, ConstantController.EV_HAZARD_REMOVED, 250f);
        GetComponent<AudioSource>().clip = thud;
        GetComponent<AudioSource>().Play();
    }

    private void EndObject(ref GameObject _item, ref int _index, string _event, float _score)
    {
        Destroy(_item);
        if (_index != -1)
        {
            spawnActive[_index] = false;
            _index = -1;
            EventController.TriggerEvent(_event);
            EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, _score);
        }
    }

    private void SpawnObject(GameObject _obj, ref GameObject _current)
    {
        int randomSpawnLocation = Random.Range(2, spawnPoints.Length);
        while ( spawnActive[randomSpawnLocation] )
        {
            randomSpawnLocation = Random.Range(2, spawnPoints.Length);
        }
        spawnActive[randomSpawnLocation] = true;
        currentIndex = randomSpawnLocation;
        _current = Instantiate(_obj, spawnPoints[randomSpawnLocation - 1].position, Quaternion.identity);
    }

    public Transform GetCurrentCollectableLocation() { return currentCollectable.transform; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    [SerializeField]public GameObject waterObject, foodObject, collectableObject;
    private GameObject currentWater, currentFood, currentCollectable;
    private Transform[] spawnPoints;
    private bool[] spawnActive;
    private int currentIndex = -1, currentWaterIndex = -1, currentFoodIndex = -1, currentCollectableIndex = -1;

    void Start () {
        GameObject spawn = GameObject.Find("SpawnPoints");
        spawnPoints = spawn.GetComponentsInChildren<Transform>();
        spawnActive = new bool[spawnPoints.Length];
        for (int i = 0; i < spawnActive.Length; i++) {
            spawnActive[i] = false;
        }
    }
	
    private void OnEnable() {
        EventController.StartListening(ConstantController.EV_SPAWN_WATER, SpawnWater);
        EventController.StartListening(ConstantController.EV_SPAWN_FOOD, SpawnFood);
        EventController.StartListening(ConstantController.EV_SPAWN_COLLECTABLE, SpawnCollectable);
    }

    private void OnDisable() {
        EventController.StopListening(ConstantController.EV_SPAWN_WATER, SpawnWater);
        EventController.StopListening(ConstantController.EV_SPAWN_FOOD, SpawnFood);
        EventController.StopListening(ConstantController.EV_SPAWN_COLLECTABLE, SpawnCollectable);
    }

    private void SpawnWater() {
        SpawnObject(waterObject, ref currentWater);
        currentWaterIndex = currentIndex;
    }

    private void SpawnFood() {
        SpawnObject(foodObject, ref currentFood);
        currentFoodIndex = currentIndex;
    }

    private void SpawnCollectable()
    {
        SpawnObject(collectableObject, ref currentCollectable);
        currentCollectableIndex = currentIndex;
    }

    public void ConsumeWater()
    {
        EndObject(ref currentWater, ref currentWaterIndex, ConstantController.EV_DRINK, 12f);
    }

    public void ConsumeFood()
    {
        EndObject(ref currentFood, ref currentFoodIndex, ConstantController.EV_EAT, 25f);
    }
    
    public void CollectObject()
    {
        EndObject(ref currentCollectable, ref currentCollectableIndex, ConstantController.EV_COLLECTED, 100f);
        FindObjectOfType<Task1>().ItemHasBeenCollected();
    }

    private void EndObject(ref GameObject _item, ref int _index, string _event, float _score)
    {
        Destroy(_item);
        spawnActive[_index] = false;
        _index = -1;
        EventController.TriggerEvent(_event);
        EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, _score);
    }

    private void SpawnObject(GameObject _obj, ref GameObject _current) {
        int randomSpawnLocation = Random.Range(0, spawnPoints.Length);
        while (spawnActive[randomSpawnLocation]) {
            randomSpawnLocation = Random.Range(0, spawnPoints.Length);
        }
        spawnActive[randomSpawnLocation] = true;
        currentIndex = randomSpawnLocation; 
        Debug.Log(spawnPoints[randomSpawnLocation].name);
        _current = Instantiate(_obj, spawnPoints[randomSpawnLocation - 1].position, Quaternion.identity);
    }

    public Transform GetCurrentCollectableLocation() { return currentCollectable.transform; }
}

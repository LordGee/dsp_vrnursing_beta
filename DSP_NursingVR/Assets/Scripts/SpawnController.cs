using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    [SerializeField]public GameObject waterObject, foodObject;
    private GameObject currentWater, currentFood;
    private Transform[] spawnPoints;
    private bool[] spawnActive;
    private int currentIndex = -1, currentWaterIndex = -1, currentFoodIndex = -1;

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
    }

    private void OnDisable() {
        EventController.StopListening(ConstantController.EV_SPAWN_WATER, SpawnWater);
        EventController.StopListening(ConstantController.EV_SPAWN_FOOD, SpawnFood);
    }

    private void SpawnWater() {
        SpawnObject(waterObject, ref currentWater);
        currentWaterIndex = currentIndex;
    }

    private void SpawnFood() {
        SpawnObject(foodObject, ref currentFood);
        currentFoodIndex = currentIndex;
    }

    public void ConsumeWater()
    {
        Destroy(currentWater);
        spawnActive[currentWaterIndex] = false;
        currentWaterIndex = -1;
        EventController.TriggerEvent(ConstantController.EV_DRINK);
        EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, 10f);
    }

    public void ConsumeFood()
    {
        Destroy(currentFood);
        spawnActive[currentFoodIndex] = false;
        currentFoodIndex = -1;
        EventController.TriggerEvent(ConstantController.EV_EAT);
        EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, 20f);
    }

    private void SpawnObject(GameObject _obj, ref GameObject _current) {
        int randomSpawnLocation = Random.Range(1, spawnPoints.Length);
        while (spawnActive[randomSpawnLocation]) {
            randomSpawnLocation = Random.Range(1, spawnPoints.Length);
        }
        spawnActive[randomSpawnLocation] = true;
        currentIndex = randomSpawnLocation; 
        _current = Instantiate(_obj, spawnPoints[randomSpawnLocation - 1].position, Quaternion.identity);
    }
}

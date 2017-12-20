using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDrinkController : MonoBehaviour {

    [SerializeField]public GameObject waterObject, foodObject;
    private GameObject currentWater, currentFood;
    private Transform[] spawnPoints;
    private bool activeObject;

    void Start () {
        GameObject spawn = GameObject.Find("SpawnPoints");
        spawnPoints = spawn.GetComponentsInChildren<Transform>();
        activeObject = false;
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
    }

    private void SpawnFood() {
        SpawnObject(foodObject, ref currentFood);
    }

    private void SpawnObject(GameObject _obj, ref GameObject _current) {
        int randomSpawnLocation = Random.Range(1, spawnPoints.Length);
        // Debug.Log("Random Number = " + randomSpawnLocation);
        _current = Instantiate(_obj, spawnPoints[randomSpawnLocation].position, Quaternion.identity);
        activeObject = true;
    }
}

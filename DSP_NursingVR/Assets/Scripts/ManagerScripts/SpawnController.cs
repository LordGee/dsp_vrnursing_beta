using UnityEngine;

/// <summary>
/// Handles the spawning of new collectable type objects.
/// </summary>
public class SpawnController : MonoBehaviour
{
    [SerializeField] public GameObject waterObject, foodObject, collectableObject;
    public GameObject[] hazards;
    public AudioClip drinking, eating, collecting, thud;
    public Transform collectableSpawnPoint;

    private GameObject currentWater, currentFood, currentCollectable, currentHazard;
    private Transform[] spawnPoints;
    private bool[] spawnActive;
    private int currentIndex = -1, currentWaterIndex = -1, currentFoodIndex = -1, currentHazardIndex = -1;

    /// <summary>
    /// Constructor - Finds all the spawn locations and sets the active value to false to the points can be usable.
    /// </summary>
    void Awake() {
        GameObject spawn = GameObject.Find("SpawnPoints");
        spawnPoints = spawn.GetComponentsInChildren<Transform>();
        spawnActive = new bool[spawnPoints.Length];
        for ( int i = 0; i < spawnActive.Length; i++ ) {
            spawnActive[i] = false;
        }
    }

    /// <summary>
    /// Events to start
    /// </summary>
    private void OnEnable() {
        EventController.StartListening(ConstantController.EV_SPAWN_WATER, SpawnWater);
        EventController.StartListening(ConstantController.EV_SPAWN_FOOD, SpawnFood);
        EventController.StartListening(ConstantController.EV_SPAWN_COLLECTABLE, SpawnCollectable);
        EventController.StartListening(ConstantController.EV_SPAWN_HAZARD, SpawnNewHazard);
    }

    /// <summary>
    /// Events to end
    /// </summary>
    private void OnDisable() {
        EventController.StopListening(ConstantController.EV_SPAWN_WATER, SpawnWater);
        EventController.StopListening(ConstantController.EV_SPAWN_FOOD, SpawnFood);
        EventController.StopListening(ConstantController.EV_SPAWN_COLLECTABLE, SpawnCollectable);
        EventController.StopListening(ConstantController.EV_SPAWN_HAZARD, SpawnNewHazard);
    }

    /// <summary>
    /// Starts the process of spawning a water bottle object
    /// </summary>
    private void SpawnWater() {
        SpawnObject(waterObject, ref currentWater);
        currentWaterIndex = currentIndex;
    }

    /// <summary>
    /// Starts the process of spawning a apple object
    /// </summary>
    private void SpawnFood() {
        SpawnObject(foodObject, ref currentFood);
        currentFoodIndex = currentIndex;
    }

    /// <summary>
    /// Starts the process of spawning a asprin bottle object
    /// </summary>
    private void SpawnCollectable() {
        currentCollectable = Instantiate(collectableObject, collectableSpawnPoint.position, Quaternion.identity);
    }

    /// <summary>
    /// Starts the process of spawning a pair of scissors or a water bucket object
    /// </summary>
    private void SpawnNewHazard() {
        SpawnObject(hazards[Random.Range(0, hazards.Length)], ref currentHazard);
        currentHazardIndex = currentIndex;
    }

    /// <summary>
    /// Starts the process of removing a water bottle object after it has been collected
    /// </summary>
    public void ConsumeWater() {
        EndObject(ref currentWater, ref currentWaterIndex, ConstantController.EV_DRINK, 12f);
        GetComponent<AudioSource>().clip = drinking;
        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// Starts the process of removing a apple object after it has been collected
    /// </summary>
    public void ConsumeFood() {
        EndObject(ref currentFood, ref currentFoodIndex, ConstantController.EV_EAT, 25f);
        GetComponent<AudioSource>().clip = eating;
        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// Starts the process of removing a asprin bottle object after it has been collected
    /// </summary>
    public void CollectObject() {
        Destroy(currentCollectable);
        EventController.TriggerEvent(ConstantController.EV_COLLECTED);
        EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, 100f);
        GetComponent<AudioSource>().clip = collecting;
        GetComponent<AudioSource>().Play();
        FindObjectOfType<Task1>().ItemHasBeenCollected();
    }

    /// <summary>
    /// Starts the process of removing a hazard object after it has been dealt with
    /// </summary>
    public void RemoveHazard() {
        EndObject(ref currentHazard, ref currentHazardIndex, ConstantController.EV_HAZARD_REMOVED, 250f);
        GetComponent<AudioSource>().clip = thud;
        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// Destroys the object, frees up the sspawn location to be used again, executes the correct event and sets an appropriate score.
    /// </summary>
    /// <param name="_item">Reference of the game object to be destroyed</param>
    /// <param name="_index">The index of the spawn position the object lives</param>
    /// <param name="_event">The event name to trigger to aknowledge specific functions</param>
    /// <param name="_score">Score to be added for this action</param>
    private void EndObject(ref GameObject _item, ref int _index, string _event, float _score) {
        Destroy(_item);
        if (_index != -1) {
            spawnActive[_index] = false;
            _index = -1;
            EventController.TriggerEvent(_event);
            EventController.TriggerEvent(ConstantController.EV_UPDATE_SCORE, _score);
        }
    }

    /// <summary>
    /// Spawns object in an available random location
    /// </summary>
    /// <param name="_obj">Object to spawn</param>
    /// <param name="_current">Copy of object spawned</param>
    private void SpawnObject(GameObject _obj, ref GameObject _current) {
        int randomSpawnLocation = Random.Range(2, spawnPoints.Length);
        while ( spawnActive[randomSpawnLocation] ) {
            randomSpawnLocation = Random.Range(2, spawnPoints.Length);
        }
        spawnActive[randomSpawnLocation] = true;
        currentIndex = randomSpawnLocation;
        _current = Instantiate(_obj, spawnPoints[randomSpawnLocation - 1].position, Quaternion.identity);
    }

    /// <summary>
    /// Returns current location of object
    /// </summary>
    /// <returns>Tranform values of collectable object</returns>
    public Transform GetCurrentCollectableLocation() { return currentCollectable.transform; }
}

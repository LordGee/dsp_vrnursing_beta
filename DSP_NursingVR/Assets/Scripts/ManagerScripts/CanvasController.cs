using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages all UI Canvas within the game
/// </summary>
public class CanvasController : MonoBehaviour
{
    public static float gameTimer, gameScore, gameHydration, gameEnergy;
    public static bool canvasStatus;
    [SerializeField] public Sprite[] imageHydration, imageEnergy;
    public GameObject statusCanvas;

    /// <summary>
    /// Constructor - Calls the function to deactivate the status canvas on awake
    /// </summary>
    private void Awake() {
        DeactivateCanvas(statusCanvas);
    }

    /// <summary>
    /// Deactivates the given canvas game object
    /// </summary>
    /// <param name="_canvas">Canvas to be deeactivated</param>
    public void DeactivateCanvas(GameObject _canvas) {
        _canvas.SetActive(false);
        canvasStatus = false;
    }

    /// <summary>
    /// Deactivates just the status canvas
    /// </summary>
    public void DeactivateCanvas() {
        statusCanvas.SetActive(false);
        canvasStatus = false;
    }

    /// <summary>
    /// Activate the given canvas game object
    /// </summary>
    /// <param name="_canvas">Canvas to be activated</param>
    public void ActivateCanvas(GameObject _canvas) {
        _canvas.SetActive(true);
        canvasStatus = true;
    }

    /// <summary>
    /// Activates just the status canvas
    /// </summary>
    public void OpenStatusCanvas() {
        ActivateCanvas(statusCanvas);
        UpdateStatusCanvas();
    }

    /// <summary>
    /// Updates the status canvas when required
    /// </summary>
    private void UpdateStatusCanvas() {
        statusCanvas.transform.Find("Score").GetComponent<Text>().text = Mathf.Floor(gameScore).ToString();
        statusCanvas.transform.Find("Timer").GetComponent<Text>().text = Mathf.Floor(gameTimer).ToString();
        statusCanvas.transform.Find("Hydration").GetComponent<Image>().sprite = imageHydration[(int) gameHydration];
        statusCanvas.transform.Find("Energy").GetComponent<Image>().sprite = imageEnergy[(int) gameEnergy];
    }

    /// <summary>
    /// List of events to be listened for
    /// </summary>
    private void OnEnable() {
        EventController.StartListening(ConstantController.EV_OPEN_STATUS_CANVAS, OpenStatusCanvas);
        EventController.StartListening(ConstantController.EV_CLOSE_STATUS_CANVAS, DeactivateCanvas);
        EventController.StartListening(ConstantController.EV_UPDATE_STATUS_CANVAS, UpdateStatusCanvas);
    }

    /// <summary>
    /// List of events to stop listening when this closes
    /// </summary>
    private void OnDisable() {
        EventController.StopListening(ConstantController.EV_OPEN_STATUS_CANVAS, OpenStatusCanvas);
        EventController.StopListening(ConstantController.EV_CLOSE_STATUS_CANVAS, DeactivateCanvas);
        EventController.StopListening(ConstantController.EV_UPDATE_STATUS_CANVAS, UpdateStatusCanvas);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static float gameTimer, gameScore, gameHydration, gameEnergy;
    public static bool canvasStatus;
    [SerializeField] public Sprite[] imageHydration, imageEnergy;
    public GameObject statusCanvas;

    private void Awake()
    {
        // statusCanvas = GameObject.Find(ConstantController.GO_STATUS_CANVAS);
        DeactivateCanvas(statusCanvas);
    }

    public void DeactivateCanvas(GameObject _canvas)
    {
        _canvas.SetActive(false);
        canvasStatus = false;
    }

    public void DeactivateCanvas()
    {
        statusCanvas.SetActive(false);
        canvasStatus = false;
    }

    public void ActivateCanvas(GameObject _canvas)
    {
        _canvas.SetActive(true);
        canvasStatus = true;
    }

    public void OpenStatusCanvas()
    {
        ActivateCanvas(statusCanvas);
        UpdateStatusCanvas();
    }

    private void UpdateStatusCanvas()
    {
        statusCanvas.transform.Find("Score").GetComponent<Text>().text = Mathf.Floor(gameScore).ToString();
        statusCanvas.transform.Find("Timer").GetComponent<Text>().text = Mathf.Floor(gameTimer).ToString();
        statusCanvas.transform.Find("Hydration").GetComponent<Image>().sprite = imageHydration[(int) gameHydration];
        statusCanvas.transform.Find("Energy").GetComponent<Image>().sprite = imageEnergy[(int) gameEnergy];
    }

    private void OnEnable()
    {
        EventController.StartListening(ConstantController.EV_OPEN_STATUS_CANVAS, OpenStatusCanvas);
        EventController.StartListening(ConstantController.EV_CLOSE_STATUS_CANVAS, DeactivateCanvas);
        EventController.StartListening(ConstantController.EV_UPDATE_STATUS_CANVAS, UpdateStatusCanvas);
    }

    private void OnDisable()
    {
        EventController.StopListening(ConstantController.EV_OPEN_STATUS_CANVAS, OpenStatusCanvas);
        EventController.StopListening(ConstantController.EV_CLOSE_STATUS_CANVAS, DeactivateCanvas);
        EventController.StopListening(ConstantController.EV_UPDATE_STATUS_CANVAS, UpdateStatusCanvas);
    }
}
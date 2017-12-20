using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    private GameObject statusCanvas;
    public static float gameTimer, gameScore, gameHydration, gameEnergy;
    public static bool canvasStatus;

    void Start() {
        statusCanvas = GameObject.Find(ConstantController.GO_STATUS_CANVAS);
        DeactivateCanvas(statusCanvas);
    }

    public void DeactivateCanvas(GameObject _canvas) {
        _canvas.SetActive(false);
        canvasStatus = false;
    }

    public void ActivateCanvas(GameObject _canvas) {
        _canvas.SetActive(true);
        canvasStatus = true;
    }

    public void OpenStatusCanvas() {
        ActivateCanvas(statusCanvas);
        UpdateStatusCanvas();
    }

    private void UpdateStatusCanvas()
    {
        statusCanvas.transform.Find("Score").GetComponent<Text>().text = Mathf.Floor(gameScore).ToString();
    }

    void OnEnable()
    {
        EventController.StartListening(ConstantController.EV_UPDATE_STATUS_CANVAS, UpdateStatusCanvas);
    }

    void OnDisable()
    {
        EventController.StopListening(ConstantController.EV_UPDATE_STATUS_CANVAS, UpdateStatusCanvas);
    }
}

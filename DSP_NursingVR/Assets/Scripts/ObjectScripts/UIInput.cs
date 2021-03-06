﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Obsolete - used for the original interaction of canvas buttons.
/// Kept for future reference
/// </summary>
[RequireComponent(typeof(SteamVR_LaserPointer))]
public class UIInput : MonoBehaviour {

    private SteamVR_LaserPointer laserPointer;
    private SteamVR_TrackedController trackedController;

    private void OnEnable() {
        laserPointer = GetComponent<SteamVR_LaserPointer>();

        laserPointer.PointerIn += HandlePointerIn;

        laserPointer.PointerOut += HandlePointerOut; 

        trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null) {
            trackedController = GetComponentInParent<SteamVR_TrackedController>();
        }

        trackedController.TriggerClicked += HandleTriggerClicked;
    }

    private void OnDisable()
    {
        laserPointer.PointerIn -= HandlePointerIn;
        laserPointer.PointerOut -= HandlePointerOut;
        trackedController.TriggerClicked -= HandleTriggerClicked;
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e) {
        try
        {
            if ( EventSystem.current.currentSelectedGameObject != null )
            {
                ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }
        catch (Exception exception)
        {
            Debug.Log("Need to fix this, but not now" + exception);
            throw;
        }

    }

    private void HandlePointerIn(object sender, PointerEventArgs e) {
        Button button = e.target.GetComponent<Button>();
        if (button != null) {
            button.Select();
        }
    }

    private void HandlePointerOut(object sender, PointerEventArgs e) {

        Button button = e.target.GetComponent<Button>();
        if (button != null) {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
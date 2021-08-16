using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public characterController player;
    public CameraController screenShotcamera;
    public Canvas canvas;
    public bool playerControls;
    public bool playerCamera;

    private void Start()
    {
        player.enabled = true;
        screenShotcamera.enabled = false;
        player.GetComponentInChildren<Camera>().enabled = true;
        screenShotcamera.GetComponentInChildren<Camera>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (playerControls == true)
            {
                player.enabled = false;
                screenShotcamera.enabled = true;
                playerControls = false;
            }
            else
            {
                player.enabled = true;
                screenShotcamera.enabled = false;
                playerControls = true;
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (playerCamera == true)
            {
                player.GetComponentInChildren<Camera>().enabled = false;
                screenShotcamera.GetComponentInChildren<Camera>().enabled = true;
                playerCamera = false;
                canvas.enabled = false;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                player.GetComponentInChildren<Camera>().enabled = true;
                screenShotcamera.GetComponentInChildren<Camera>().enabled = false;
                playerCamera = true;
                canvas.enabled = true;
            }
        }
    }
}

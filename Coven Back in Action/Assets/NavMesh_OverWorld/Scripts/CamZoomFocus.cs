using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TbsFramework.Gui;

public class CamZoomFocus : MonoBehaviour
{

    public PlayerController player;
    public CameraController camController;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isInCombat)
        {
            cam.fieldOfView = 30;
            camController.enabled = false;
            gameObject.transform.SetParent(player.gameObject.transform);
        }
        else
        {
            cam.fieldOfView = 60;
            camController.enabled = true;
            gameObject.transform.parent = null;
            gameObject.transform.position = camController.startingPos;
        }
    }
}

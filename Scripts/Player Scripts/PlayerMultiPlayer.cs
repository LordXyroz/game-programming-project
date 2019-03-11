using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMultiPlayer : NetworkBehaviour {

    [Header("Managers")]
    [SerializeField]
    private GameObject managers;
    [SerializeField]
    private HideTiles tileScript;
    [SerializeField]
    private DayNightCycle dayNightCycle;

    [Header("UI")]
    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private GameObject arrow;

    [Header("Inventory")]
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private GameObject handContainer;
    [SerializeField]
    private GameObject inventoryContainer;


    [Header("Cameras")]
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private Camera uiCam;
    [SerializeField]
    private Camera minimapCam;
    [SerializeField]
    private Camera minimapIconCam;
    [SerializeField]
    private Camera minimapTextCam;

    [Header("Audio")]
    [SerializeField]
    private AudioListener listener;

    // Use this for initialization
    void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        
        // Enables or disables scripts of the player object.
        // Ensures we don't have duplicate cameras rendering,
        // only your own minimap icon displays, etc
        if (isLocalPlayer)
        {
            managers.SetActive(true);
            tileScript.enabled = true;

            ui.SetActive(true);
            arrow.SetActive(true);

            inventory.enabled = true;
            //handContainer.SetActive(true);
            inventoryContainer.SetActive(true);
            
            mainCam.gameObject.SetActive(true);
            uiCam.gameObject.SetActive(true);
            minimapCam.gameObject.SetActive(true);
            minimapIconCam.gameObject.SetActive(true);
            minimapTextCam.gameObject.SetActive(true);

            listener.enabled = true;
        }
        else
        {
            managers.SetActive(false);
            tileScript.enabled = false;

            ui.SetActive(false);
            arrow.SetActive(false);

            inventory.enabled = false;
            //handContainer.SetActive(false);
            inventoryContainer.SetActive(false);

            mainCam.gameObject.SetActive(false);
            uiCam.gameObject.SetActive(false);
            minimapCam.gameObject.SetActive(false);
            minimapIconCam.gameObject.SetActive(false);
            minimapTextCam.gameObject.SetActive(false);

            listener.enabled = false;
        }
    }

    private void Update()
    {

    }
}

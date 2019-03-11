using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class OptionsMenu : MonoBehaviour {

    [Header("Managers")]
    public InputManager inputManager;
    public NetworkManager networkManager;
    public DialogManager manager;

    [Header("Objects")]
    public PlayerMovement player;

    public GameObject dialogBox;
    public GameObject optionsMenu;

    public GameObject keybindingMenu;
    public GameObject healthBar;
    public GameObject manaBar;

    public GameObject hotbar;
    public GameObject inventory;

    [Header("Variables")]
    private int callbackID;
    public bool options = false;

    [Header("Buttons")]
    public Button keybindings;
    public Button quit;

	// Use this for initialization
	void Start()
    {
        //Sets up defaults for the menu
        // Adds ToggleOptions function to escape key dictionary
        inputManager.RegisterAction(InputManager.Keys.escape, ToggleOptions, GetInstanceID());

        keybindings.onClick.AddListener(KeybindingMenu);
        quit.onClick.AddListener(QuitGame);

        networkManager = FindObjectOfType<NetworkManager>();
    }
    
    public void KeybindingMenu()
    {
        // Removes ToggleOptions function from escape key dictionary
        inputManager.RemoveAction(InputManager.Keys.escape, GetInstanceID());

        // Disables options menu
        options = false;
        optionsMenu.SetActive(false);

        // Enables keybinding menu
        keybindingMenu.SetActive(true);
    }

    public void ReturnToMenu()
    {
        // Enables options menu
        options = true;
        optionsMenu.SetActive(true);

        // Adds ToggleOptions function to escape key dictionary
        inputManager.RegisterAction(InputManager.Keys.escape, ToggleOptions, GetInstanceID());
    }

    public void ToggleOptions()
    {
        // Changes behaviour based what item is active

        // Closes dialog box on escape
        if (dialogBox.activeSelf)
            manager.CloseBox();
        // Closes inventory on escape
        else if (inventory.activeSelf)
            inventory.SetActive(false);
        // Toggles options menu on escape
        else
        {
            options = !options;
            player.rotate = !options;
            optionsMenu.SetActive(options);
            healthBar.SetActive(!options);
            manaBar.SetActive(!options);
            hotbar.SetActive(!options);
            Cursor.visible = options;
            Cursor.lockState = (options) ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void QuitGame()
    {
        // Stops host and client when exiting scene
        networkManager.StopHost();
        SceneManager.LoadScene("Main Menu");
    }
    
}

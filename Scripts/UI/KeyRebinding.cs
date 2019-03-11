using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

public class KeyRebinding : MonoBehaviour {

    [Header("Manager")]
    public InputManager inputManager;

    [Header("Objects")]
    public OptionsMenu optionsMenu;

    public GameObject window;
    public Button abort;

    [Header("Text fields")]
    public Text escapeText;
    public Text forwardText;
    public Text backwardText;
    public Text leftText;
    public Text rightText;
    public Text jumpText;
    public Text primaryText;
    public Text secondaryText;
    public Text toggleText;
    public Text interractText;
    public Text inventoryText;

    public Text actionbar1Text;
    public Text actionbar2Text;
    public Text actionbar3Text;
    public Text actionbar4Text;
    public Text actionbar5Text;
    public Text actionbar6Text;
    public Text actionbar7Text;
    public Text actionbar8Text;
    public Text actionbar9Text;

    [Header("Keybinding buttons")]
    public Button escape;
    public Button forward;
    public Button backward;
    public Button left;
    public Button right;
    public Button jump;
    public Button primaryFire;
    public Button secondaryFire;
    public Button toggleView;
    public Button interract;
    public Button inventory;

    public Button actionbar1;
    public Button actionbar2;
    public Button actionbar3;
    public Button actionbar4;
    public Button actionbar5;
    public Button actionbar6;
    public Button actionbar7;
    public Button actionbar8;
    public Button actionbar9;
    
    [Header("Variables")]
    public KeyCode code;
    public bool done = true;
    public bool stop = false;

    private void OnEnable()
    {
        // Runs following code when player enters the key rebinding menu
        
        // Makes you return to options menu instead of the game when pressing escape
        inputManager.RegisterAction(InputManager.Keys.escape, ReturnToOptions, GetInstanceID());

        // Sets default values
        stop = false;

        // Removes any function calls from abort button, and adds an abort function
        abort.onClick.RemoveAllListeners();
        abort.onClick.AddListener(Abort);

        // Removes any function calls from all the buttons for keybidings
        escape.onClick.RemoveAllListeners();
        forward.onClick.RemoveAllListeners();
        backward.onClick.RemoveAllListeners();
        left.onClick.RemoveAllListeners();
        right.onClick.RemoveAllListeners();
        jump.onClick.RemoveAllListeners();
        primaryFire.onClick.RemoveAllListeners();
        secondaryFire.onClick.RemoveAllListeners();
        toggleView.onClick.RemoveAllListeners();
        interract.onClick.RemoveAllListeners();
        inventory.onClick.RemoveAllListeners();

        actionbar1.onClick.RemoveAllListeners();
        actionbar2.onClick.RemoveAllListeners();
        actionbar3.onClick.RemoveAllListeners();
        actionbar4.onClick.RemoveAllListeners();
        actionbar5.onClick.RemoveAllListeners();
        actionbar6.onClick.RemoveAllListeners();
        actionbar7.onClick.RemoveAllListeners();
        actionbar8.onClick.RemoveAllListeners();
        actionbar9.onClick.RemoveAllListeners();

        // Adds functions to each key rebind button
        escape.onClick.AddListener(() => Rebind(InputManager.Keys.escape));
        forward.onClick.AddListener(() => Rebind(InputManager.Keys.forward));
        backward.onClick.AddListener(() => Rebind(InputManager.Keys.backward));
        left.onClick.AddListener(() => Rebind(InputManager.Keys.left));
        right.onClick.AddListener(() => Rebind(InputManager.Keys.right));
        jump.onClick.AddListener(() => Rebind(InputManager.Keys.jump));
        primaryFire.onClick.AddListener(() => Rebind(InputManager.Keys.mouse0));
        secondaryFire.onClick.AddListener(() => Rebind(InputManager.Keys.mouse1));
        toggleView.onClick.AddListener(() => Rebind(InputManager.Keys.toggleView));
        interract.onClick.AddListener(() => Rebind(InputManager.Keys.interract));
        inventory.onClick.AddListener(() => Rebind(InputManager.Keys.inventory));

        actionbar1.onClick.AddListener(() => Rebind(InputManager.Keys.actionbar1));
        actionbar2.onClick.AddListener(() => Rebind(InputManager.Keys.actionbar2));
        actionbar3.onClick.AddListener(() => Rebind(InputManager.Keys.actionbar3));
        actionbar4.onClick.AddListener(() => Rebind(InputManager.Keys.actionbar4));
        actionbar5.onClick.AddListener(() => Rebind(InputManager.Keys.actionbar5));
        actionbar6.onClick.AddListener(() => Rebind(InputManager.Keys.actionbar6));
        actionbar7.onClick.AddListener(() => Rebind(InputManager.Keys.actionbar7));
        actionbar8.onClick.AddListener(() => Rebind(InputManager.Keys.actionbar8));
        actionbar9.onClick.AddListener(() => Rebind(InputManager.Keys.actionbar9));
    }

    // Rebind function that takes a key as paramter instead of having a new function for every single button
    public void Rebind(InputManager.Keys key)
    {
        // Stops the player from moving
        inputManager.transform.parent.GetComponent<PlayerMovement>().move = false;
        // Starts a coroutine
        StartCoroutine(Test(key));
    }

    private void Abort()
    {
        // Incase player doesn't want to rebind a key
        stop = true;
    }

    // TODO: change name of function to better represent what it does
    public IEnumerator Test(InputManager.Keys key)
    {
        // Enables a window telling player what to do
        window.SetActive(true);

        // done is used to check when if a player has pressed a new key, default it to false
        done = false;

        // Saves the old key incase player wants to abort keybinding
        KeyCode old = inputManager.RemoveKey(key);
        while (true)
        {
            // If the player has pressed a key
            if (done)
            {
                // Checks if new keybinding has been added successfully
                if (inputManager.AddKey(key, code))
                    break;  // Exits the while loop

                // If not added successfully, we try again for a new key
                done = false;
                code = KeyCode.None;
            }
            // If player aborts keybinding
            else if (stop)
            {
                // Re adds the old key
                inputManager.AddKey(key, old);
                stop = false;
                break;  // Exits the while loop
            }
            // Yield so other code/threads can run
            yield return null;
        }
        
        // Enables player to move again and disables info window
        inputManager.transform.parent.GetComponent<PlayerMovement>().move = true;
        window.SetActive(false);
    }

    private void OnGUI()
    {
        // Uses OnGUI() function to check if a key has been pressed,
        // and what key was pressed. Instead of using many, many if statements
        if (!done)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                code = e.keyCode;
                done = true;
            }
        }
    }

    // Function to return to previous options screen
    public void ReturnToOptions()
    {
        inputManager.RemoveAction(InputManager.Keys.escape, GetInstanceID());
        optionsMenu.ReturnToMenu();
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Try/catches to update what key currently is bound, and display the key as text on screen
        try
        {
            escapeText.text = inputManager.keybindings[InputManager.Keys.escape].ToString();
            forwardText.text = inputManager.keybindings[InputManager.Keys.forward].ToString();
            backwardText.text = inputManager.keybindings[InputManager.Keys.backward].ToString();
            leftText.text = inputManager.keybindings[InputManager.Keys.left].ToString();
            rightText.text = inputManager.keybindings[InputManager.Keys.right].ToString();
            jumpText.text = inputManager.keybindings[InputManager.Keys.jump].ToString();
            primaryText.text = inputManager.keybindings[InputManager.Keys.mouse0].ToString();
            secondaryText.text = inputManager.keybindings[InputManager.Keys.mouse1].ToString();
            toggleText.text = inputManager.keybindings[InputManager.Keys.toggleView].ToString();
            interractText.text = inputManager.keybindings[InputManager.Keys.interract].ToString();
            inventoryText.text = inputManager.keybindings[InputManager.Keys.inventory].ToString();

            actionbar1Text.text = inputManager.keybindings[InputManager.Keys.actionbar1].ToString();
            actionbar2Text.text = inputManager.keybindings[InputManager.Keys.actionbar2].ToString();
            actionbar3Text.text = inputManager.keybindings[InputManager.Keys.actionbar3].ToString();
            actionbar4Text.text = inputManager.keybindings[InputManager.Keys.actionbar4].ToString();
            actionbar5Text.text = inputManager.keybindings[InputManager.Keys.actionbar5].ToString();
            actionbar6Text.text = inputManager.keybindings[InputManager.Keys.actionbar6].ToString();
            actionbar7Text.text = inputManager.keybindings[InputManager.Keys.actionbar7].ToString();
            actionbar8Text.text = inputManager.keybindings[InputManager.Keys.actionbar8].ToString();
            actionbar9Text.text = inputManager.keybindings[InputManager.Keys.actionbar9].ToString();
        }
        catch (KeyNotFoundException)
        {
            // We know that an exception will be thrown during keybinding because the key/value pair will not be found
            // as it's temporarily removed from the dictionary
        }
    }
}


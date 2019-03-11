using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogManager : MonoBehaviour {
    [Header("Important")]
    public InputManager inputManager;

    [Header("Gameobjects")]
    public GameObject dialogBox;
    public Text dialogText;

    [Header("Buttons")]
    public Button next;
    public Button close;
    public Button giveQuest;
    public Button accept;
    public Button decline;

    [Header("Text variables")]
    public string[] dialogs;
    public int currentLine;

    [Header("Player")]
    [SerializeField]
    private PlayerMovement player;

    // Use this for initialization
    void Start () {
        dialogText.text = dialogs[0];

        close.onClick.AddListener(CloseBox);
        next.onClick.AddListener(NextLine);
	}

	
	void Update () {
        // Checks if quest buttons are active or not
        if (!(accept.gameObject.activeSelf || decline.gameObject.activeSelf))
        {
            // If there is more dialog, we want to be able to click next and not close the dialog box
            if (currentLine < dialogs.Length - 1)
            {
                next.gameObject.SetActive(true);
                close.gameObject.SetActive(false);
            }
            else
            {
                next.gameObject.SetActive(false);
                close.gameObject.SetActive(true);
            }
        }
        else
        {
            next.gameObject.SetActive(false);
            close.gameObject.SetActive(false);
        }
    }

    public void CloseBox()
    {
        // Resets buttons and values when closing dialog box
        currentLine = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        accept.gameObject.SetActive(false);
        decline.gameObject.SetActive(false);

        // Enable player to move and rotate again
        player.move = true;
        player.rotate = true;

        dialogBox.SetActive(false);
    }

    void NextLine()
    {
        dialogText.text = dialogs[++currentLine];
    }

    // Depricated function to show text, only shows 1 line
    public void ShowBox(string text)
    {
        dialogText.text = text;
        dialogBox.SetActive(true);
    }

    public void ShowDialog(string[] text)
    {
        // Stops player from moving and rotating while in a dialog box
        player.rotate = false;
        player.move = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Starts displaying text
        dialogs = text;
        dialogText.text = dialogs[0];
        dialogBox.SetActive(true);
    }
}

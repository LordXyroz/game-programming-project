using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainmenuScript : MonoBehaviour {
    public Button start;
    public Button options;
    public Button quit;

    private void Start()
    {
        start.onClick.AddListener(StartGame);
        options.onClick.AddListener(OptionsMenu);
        quit.onClick.AddListener(QuitGame);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Marco");
    }

    public void OptionsMenu()
    {

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

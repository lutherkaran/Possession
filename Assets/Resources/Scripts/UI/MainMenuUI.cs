using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.BEGINNING); });

        settingsButton.onClick.AddListener(() => { });

        quitButton.onClick.AddListener(() =>
        {
            Quit();
        });

        Time.timeScale = 1.0f;
    }

    private void Quit()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}

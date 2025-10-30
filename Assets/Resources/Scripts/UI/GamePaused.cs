using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GamePaused : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() => GameManager.instance.TogglePause());

        MainMenuButton.onClick.AddListener(() => Loader.Load(Loader.Scene.MENU));

        settingsButton.onClick.AddListener(() =>
        {
            Hide();
        });

        quitButton.onClick.AddListener(() => Application.Quit());
    }

    private void Start()
    {
        GameManager.instance.OnGamePaused += GamePaused_OnGamePaused;
        GameManager.instance.OnGameUnpaused += GamePaused_OnGameUnpaused;

        Hide();
    }

    private void GamePaused_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GamePaused_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

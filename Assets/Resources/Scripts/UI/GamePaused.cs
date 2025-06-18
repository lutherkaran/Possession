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
        resumeButton.onClick.AddListener(() => GameManager.Instance.TogglePause());
        
        MainMenuButton.onClick.AddListener(() => Loader.Load(Loader.Scene.MainMenuScene));

        settingsButton.onClick.AddListener(() =>
        {
            Hide();
            //OptionsUI.Instance.Show(Show);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GamePaused_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GamePaused_OnGameUnpaused;

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

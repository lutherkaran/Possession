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
        //MainMenuButton.onClick.AddListener(() =);
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
        CameraManager.instance.GetMouseAim().HideMouse();
    }

    private void GamePaused_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
        CameraManager.instance.GetMouseAim().ShowMouse();
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

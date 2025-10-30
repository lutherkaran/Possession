using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private GameState state;

    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimerMax = 5f;
    private float gamePlayingTimer;

    private bool isGamePaused = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            instance = null;
        }

        instance = this;
        state = GameState.WaitingToStart;
    }

    private void Start()
    {
        InputManager.instance.OnGamePaused += GameManager_OnGamePaused;
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case GameState.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime; break;
            case GameState.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0)
                {
                    state = GameState.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                }
                break;
            case GameState.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = GameState.GameOver;
                }
                break;
            case GameState.GameOver:
                break;
        }
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        TogglePause();
    }

    public bool isGamePlaying()
    {
        return state == GameState.GamePlaying;
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            CameraManager.instance.GetMouseAim().ToggleMouseInteraction();
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            CameraManager.instance.GetMouseAim().ToggleMouseInteraction();
        }
    }
}
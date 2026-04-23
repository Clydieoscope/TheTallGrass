using UnityEngine;
using System;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    Settings,
    Won,
    Lost
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState CurrentState { get; private set; }
    public GameState PrevState {get; private set; }

    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetState(GameState.Paused);
        Debug.Log(PrevState.ToString() + " " + CurrentState.ToString());
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        PrevState = CurrentState;
        CurrentState = newState;
        switch (CurrentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 0f;
                AudioListener.pause = true;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                AudioListener.pause = false;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                AudioListener.pause = true;
                break;
            case GameState.Settings:
                Time.timeScale = 0f;
                AudioListener.pause = true;
                break;
            case GameState.Won:
            case GameState.Lost:
                Time.timeScale = 0.5f;
                AudioListener.pause = true;
                break;
        }
        Debug.Log("Game State Changed to: " + newState);

        OnGameStateChanged?.Invoke(newState);
    }

    public void Win()
    {
        SetState(GameState.Won);
    }

    public void Lose()
    {
        SetState(GameState.Lost);
    }
}
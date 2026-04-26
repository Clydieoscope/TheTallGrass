using UnityEngine;
using UnityEngine.Events;
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
    public GameState PrevState { get; private set; }

    public event Action<GameState> OnGameStateChanged;

    [Header("Game End Events")]
    public UnityEvent OnWin;
    public UnityEvent OnLose;
    public UnityEvent OnEnd;

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
        Debug.Log(PrevState.ToString() + " " + CurrentState.ToString());
        SetState(GameState.MainMenu);
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        PrevState = CurrentState;
        CurrentState = newState;

        switch (CurrentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 0.5f;
                break;
            case GameState.Paused:
            case GameState.Settings:
                Time.timeScale = 0f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Won:
            case GameState.Lost:
                Time.timeScale = 0f;
                break;
        }

        Debug.Log("Game State Changed to: " + newState);
        OnGameStateChanged?.Invoke(newState);
    }

    public void Win()
    {
        SetState(GameState.Won);
        OnWin?.Invoke();
        OnEnd?.Invoke();
    }

    public void Lose()
    {
        SetState(GameState.Lost);
        OnLose?.Invoke();
        OnEnd?.Invoke();
    }
}
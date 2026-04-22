using UnityEngine;
using System;

public enum GameState
{
    Playing,
    Won,
    Lost
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState CurrentState { get; private set; }

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
        SetState(GameState.Playing);
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
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
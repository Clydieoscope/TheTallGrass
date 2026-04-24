using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField] private AudioMixer audioMixer;
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
                Time.timeScale = 0.5f;
                audioMixer.SetFloat("Game", -80f);
                audioMixer.SetFloat("Music", 0f);
                audioMixer.SetFloat("UI", 0f);
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                audioMixer.SetFloat("Game", 0f);
                audioMixer.SetFloat("Music", 0f);
                audioMixer.SetFloat("UI", 0f);
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                audioMixer.SetFloat("Game", -80f);
                audioMixer.SetFloat("Music", 0f);
                audioMixer.SetFloat("UI", 0f);
                break;
            case GameState.Settings:
                Time.timeScale = 0f;
                audioMixer.SetFloat("Game", -80f);
                audioMixer.SetFloat("Music", 0f);
                audioMixer.SetFloat("UI", 0f);
                break;
            case GameState.Won:
            case GameState.Lost:
                Time.timeScale = 0f;
                audioMixer.SetFloat("Game", -80f);
                audioMixer.SetFloat("Music", 0f);
                audioMixer.SetFloat("UI", -80f);
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
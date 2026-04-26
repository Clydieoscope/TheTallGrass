using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    [SerializeField] private AudioMixer masterMixer;

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
        HandleGameStateChanged(GameStateManager.Instance.CurrentState);
    }

    private void OnEnable()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        HandleGameStateChanged(GameStateManager.Instance.CurrentState);
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                SetGroupVolume("Game",  -80f);
                SetGroupVolume("Music",   0f);
                SetGroupVolume("UI",      0f);
                Debug.Log("Volume settings of " + GameState.MainMenu);
                break;
            case GameState.Playing:
                SetGroupVolume("Game",    0f);
                SetGroupVolume("Music",   0f);
                SetGroupVolume("UI",      0f);
                break;
            case GameState.Paused:
            case GameState.Settings:
                SetGroupVolume("Game",  -80f);
                SetGroupVolume("Music",   0f);
                SetGroupVolume("UI",      0f);
                break;
            case GameState.Won:
            case GameState.Lost:
                SetGroupVolume("Game",  -80f);
                SetGroupVolume("Music",   0f);
                SetGroupVolume("UI",      0f);
                break;
        }
    }

    // Called externally when a smooth dB transition is needed (e.g. end sequence)
    public void SetGameVolumeDecibels(float decibels) =>
        SetGroupVolume("Game", decibels);

    // --- Game group (slider-driven) ---

    public void SetVolume(float sliderValue)
    {
        float db = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        SetGroupVolume("Master", db);
        Debug.Log("Set Master volume to " + db);
    }

    public float GetVolume()
    {
        masterMixer.GetFloat("Game", out float volume);
        return volume;
    }

    // --- Master group ---

    public float GetMasterVolume()
    {
        masterMixer.GetFloat("Master", out float volume);
        return volume;
    }

    public void SetMasterVolumeDecibels(float decibels) =>
        SetGroupVolume("Master", decibels);

    // --- Internal helper ---

    private void SetGroupVolume(string group, float decibels) =>
        masterMixer.SetFloat(group, decibels);
}
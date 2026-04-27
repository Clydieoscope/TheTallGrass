using UnityEngine;
using StarterAssets;

public enum StealthUIState
{
    Visible,
    Hidden,
    Detected
}

public class StealthUIManager : MonoBehaviour
{
    public static StealthUIManager Instance;

    public event System.Action<StealthUIState> OnStateChanged;

    private StealthUIState _currentState;
    public StealthUIState CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState == value) return;
            _currentState = value;
            OnStateChanged?.Invoke(_currentState);
            Debug.Log("StealthUIManager state changed to " + _currentState);
        }
    }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GhoulChaseTracker.Instance.OnChaseCountChanged += HandleChaseCountChanged;
    }

    private void OnDisable()
    {
        if (GhoulChaseTracker.Instance != null)
            GhoulChaseTracker.Instance.OnChaseCountChanged -= HandleChaseCountChanged;
    }

    private void Update()
    {
        // Detected always takes priority
        if (GhoulChaseTracker.Instance.ChasingCount > 0) return;

        bool isCrouched = ThirdPersonController.Instance.IsCrouched();
        bool hasStealh = StealthSystem.Instance.IsStealthed();

        if (isCrouched && hasStealh)
            CurrentState = StealthUIState.Hidden;
        else
            CurrentState = StealthUIState.Visible;
    }

    private void HandleChaseCountChanged(int count)
    {
        if (count > 0)
            CurrentState = StealthUIState.Detected;
        else
        {
            // Re-evaluate immediately when detection drops
            bool isCrouched = ThirdPersonController.Instance.IsCrouched();
            bool hasStealth = StealthSystem.Instance.IsStealthed();

            CurrentState = (isCrouched && hasStealth)
                ? StealthUIState.Hidden
                : StealthUIState.Visible;
        }
    }
}
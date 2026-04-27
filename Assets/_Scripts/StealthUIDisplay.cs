using UnityEngine;
using UnityEngine.UI;

public class StealthUIDisplay : MonoBehaviour
{
    [SerializeField] private GameObject visibleIndicator;
    [SerializeField] private GameObject hiddenIndicator;
    [SerializeField] private GameObject detectedIndicator;
    [SerializeField] private GameObject inGrassIndicator;


    private void Start()
    {
        StealthUIManager.Instance.OnStateChanged += UpdateUI;
        StealthSystem.Instance.OnStealthChanged.AddListener(UpdateGrassIndicator);

        // Sync UI to current state immediately
        UpdateUI(StealthUIManager.Instance.CurrentState);
        UpdateGrassIndicator();
    }

    private void OnDisable()
    {
        if (StealthUIManager.Instance != null)
            StealthUIManager.Instance.OnStateChanged -= UpdateUI;

        if (StealthSystem.Instance != null)
            StealthSystem.Instance.OnStealthChanged.RemoveListener(UpdateGrassIndicator);
    }

    // private void OnEnable()
    // {
    //     StealthUIManager.Instance.OnStateChanged += UpdateUI;
    //     StealthSystem.Instance.OnStealthChanged.AddListener(UpdateGrassIndicator);
    // }

    // private void OnDisable()
    // {
    //     if (StealthUIManager.Instance != null)
    //         StealthUIManager.Instance.OnStateChanged -= UpdateUI;

    //     StealthSystem.Instance.OnStealthChanged.RemoveListener(UpdateGrassIndicator);
    // }

    private void UpdateGrassIndicator()
    {
        inGrassIndicator.SetActive(StealthSystem.Instance.IsStealthed());
    }

    private void UpdateUI(StealthUIState state)
    {
        visibleIndicator.SetActive(state == StealthUIState.Visible);
        hiddenIndicator.SetActive(state == StealthUIState.Hidden);
        detectedIndicator.SetActive(state == StealthUIState.Detected);
    }
}
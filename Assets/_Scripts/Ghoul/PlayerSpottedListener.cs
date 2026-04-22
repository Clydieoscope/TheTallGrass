using UnityEngine;
using UnityEngine.Events;

public class PlayerSpottedListener : MonoBehaviour
{
    [SerializeField] private PlayerSpotted playerSpottedChannel;
    // [SerializeField] private UnityEvent onPlayerSpotted;

    private void OnEnable() => playerSpottedChannel.Event += OnPlayerSpotted;
    private void OnDisable() => playerSpottedChannel.Event -= OnPlayerSpotted;

    private void OnPlayerSpotted(GameObject agent, GameObject target)
    {
        // onPlayerSpotted?.Invoke();
        AmbientSFXHandler.Instance.PlaySpotted();
    }
}
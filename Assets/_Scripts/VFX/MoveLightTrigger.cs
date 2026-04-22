using UnityEngine;
using UnityEngine.Events;

public class MoveLightTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onPlayerEntered;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;

        hasTriggered = true;
        onPlayerEntered?.Invoke();
    }
}
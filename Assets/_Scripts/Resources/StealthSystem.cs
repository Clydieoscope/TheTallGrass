using UnityEngine;
using UnityEngine.Events;

public class StealthSystem : MonoBehaviour
{
    [SerializeField] private float maxStealth = 100f;
    [SerializeField] private float currentStealth = 0f;

    public UnityEvent OnStealthChanged;
    public UnityEvent OnHidden;

    public void AddStealth(float amount)
    {
        currentStealth += amount;
        currentStealth = Mathf.Clamp(currentStealth, 0f, maxStealth);
        OnStealthChanged?.Invoke();
    }

    public void RemoveStealth(float amount)
    {
        currentStealth -= amount;
        currentStealth = Mathf.Clamp(currentStealth, 0f, maxStealth);
        OnStealthChanged?.Invoke();
        
    }

    public float GetStealth()
    {
        return currentStealth;
    }

    public bool IsStealthed()
    {
        return currentStealth > 0f;
    }
}
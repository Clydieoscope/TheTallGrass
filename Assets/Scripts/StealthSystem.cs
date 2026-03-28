using UnityEngine;

public class StealthSystem : MonoBehaviour
{
    [SerializeField] private float maxStealth = 100f;
    private float currentStealth = 0f;

    public void AddStealth(float amount)
    {
        currentStealth += amount;
        currentStealth = Mathf.Clamp(currentStealth, 0f, maxStealth);
        Debug.Log("Stealth: " + currentStealth);
    }

    public void RemoveStealth(float amount)
    {
        currentStealth -= amount;
        currentStealth = Mathf.Clamp(currentStealth, 0f, maxStealth);
        Debug.Log("Stealth: " + currentStealth);
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
using UnityEngine;

public class GrassStealthZone : MonoBehaviour
{
    [SerializeField] private float stealthAmount = 1f;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            StealthSystem stealth = other.GetComponent<StealthSystem>();

            if (stealth != null)
            {
                stealth.AddStealth(stealthAmount);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            StealthSystem stealth = other.GetComponent<StealthSystem>();

            if (stealth != null)
            {
                stealth.RemoveStealth(stealthAmount);
            }
        }
    }
}
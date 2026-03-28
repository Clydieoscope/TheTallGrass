using UnityEngine;

public class GrassStealthZone : MonoBehaviour
{
    [SerializeField] private float stealthAmount = 1f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetTrigger("IsTouched");

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
        animator.SetTrigger("IsTouched");


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
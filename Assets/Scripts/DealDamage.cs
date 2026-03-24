using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem player = other.GetComponent<HealthSystem>();
            player.TakeDamage(damage);
        }
    }

}

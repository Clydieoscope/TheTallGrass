using UnityEngine;
 
public class HealingZone : MonoBehaviour
{
    [Header("Healing Settings")]
    [SerializeField] private float healPerSecond = 10f;
 
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        HealthSystem health = other.GetComponent<HealthSystem>();


        if (health != null && !health.IsDead())
        {
            health.Heal(healPerSecond * Time.deltaTime);
        }
    }
 
    // Visual debug in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0.4f, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        Gizmos.color = new Color(0f, 1f, 0.4f, 0.8f);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
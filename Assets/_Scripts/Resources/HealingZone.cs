using UnityEngine;
 
public class HealingZone : MonoBehaviour
{
    [Header("Healing Settings")]
    [SerializeField] private float healPerSecond = 10f;

    [Header("Audio")]
    [SerializeField] private AudioSource healingAudioSource;
    [SerializeField] private float soundInterval = 2f;

    private float _soundTimer = 0f;
 
    private void Start()
    {
        if (healingAudioSource == null)
            healingAudioSource = GetComponent<AudioSource>();

        if (healingAudioSource == null)
            Debug.LogWarning($"HealingZone: No AudioSource found on {gameObject.name}.");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        HealthSystem health = other.GetComponent<HealthSystem>();

        if (health != null && !health.IsDead())
        {
            health.Heal(healPerSecond * Time.deltaTime);

            _soundTimer -= Time.deltaTime;
            if (_soundTimer <= 0f)
            {
                healingAudioSource.Play();
                _soundTimer = soundInterval;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _soundTimer = 0f;
    }
 
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0.4f, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        Gizmos.color = new Color(0f, 1f, 0.4f, 0.8f);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
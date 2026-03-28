using UnityEngine;
using StarterAssets;

public class LineOfSightDetector : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_playerLayerMask;
    [SerializeField]
    private float m_detectionRange = 10.0f;
    [SerializeField]
    private float m_detectionHeight = 2f;

    [SerializeField] private bool showDebugVisuals = true;

    private GameObject m_lastSeenTarget;
    private Vector3 m_lastSeenPosition;
    private float m_lastSeenTime;
    [SerializeField]
    private float m_memoryDuration = 7f; // seconds

    [SerializeField] private float m_stealthDetectionRange = 5f;
    

    public GameObject PerformDetection(GameObject potentialTarget)
    {
        RaycastHit hit;
        Vector3 direction = potentialTarget.transform.position - transform.position;

        float distance = direction.magnitude;

        StealthSystem stealth = potentialTarget.GetComponent<StealthSystem>();
        ThirdPersonController controller = potentialTarget.GetComponent<ThirdPersonController>();

        float currentDetectionRange = m_detectionRange;

        if (stealth != null && stealth.IsStealthed() && controller != null && controller.IsCrouched())
        {
            currentDetectionRange = m_stealthDetectionRange;
        }

        // Exit early if no one is detected within range
        if (distance > currentDetectionRange)
        {
            return HandleMemory();
        }

        bool didHit = Physics.Raycast(
            transform.position + Vector3.up * m_detectionHeight,
            direction.normalized,
            out hit,
            currentDetectionRange,
            m_playerLayerMask
        );

        if (showDebugVisuals && this.enabled)
        {
            Debug.DrawLine(
                transform.position + Vector3.up * m_detectionHeight,
                potentialTarget.transform.position,
                Color.magenta
            );
        }

        if (didHit && hit.collider.gameObject == potentialTarget)
        {
            // Player is visible
            m_lastSeenTarget = potentialTarget;
            m_lastSeenTime = Time.time;
            m_lastSeenPosition = potentialTarget.transform.position;

            return potentialTarget;
        }

        return HandleMemory();
    }

    private GameObject HandleMemory()
    {
        // Player NOT visible, check memory
        if (m_lastSeenTarget != null)
        {
            float timeSinceLastSeen = Time.time - m_lastSeenTime;

            if (timeSinceLastSeen <= m_memoryDuration)
            {
                // Still remember player, keep chasing
                return m_lastSeenTarget;
            }
        }

        // Memory expired
        m_lastSeenTarget = null;
        return null;
    }

    private void OnDrawGizmos()
    {
        if (!showDebugVisuals|| this.enabled)
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + Vector3.up * m_detectionHeight, 0.3f);
        
    }
}

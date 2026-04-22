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
    [SerializeField] private float m_detectionAngle = 360f;
    [SerializeField] private float m_behindDetectionRange = 2f; // total FOV in degrees
    

    public GameObject PerformDetection(GameObject potentialTarget)
    {
        RaycastHit hit;
        Vector3 direction = potentialTarget.transform.position - transform.position;
        float distance = direction.magnitude;

        StealthSystem stealth = potentialTarget.GetComponent<StealthSystem>();
        ThirdPersonController controller = potentialTarget.GetComponent<ThirdPersonController>();

        float currentDetectionRange = m_detectionRange;

        if (stealth != null && stealth.IsStealthed() && controller != null && controller.IsCrouched())
            currentDetectionRange = m_stealthDetectionRange;

        if (distance > currentDetectionRange)
            return HandleMemory();

        // Exit early if outside field of view and far from behind
        float angle = Vector3.Angle(transform.forward, direction.normalized);
        if (angle > m_detectionAngle / 2f && distance > m_behindDetectionRange)
            return HandleMemory();

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
        if (!showDebugVisuals || !this.enabled)
            return;

        Vector3 origin = transform.position + Vector3.up * m_detectionHeight;

        // Behind detection range circle
        Gizmos.color = new Color(1f, 0f, 1f, 0.2f);
        int circleSegments = 32;
        for (int i = 0; i < circleSegments; i++)
        {
            float angleA = (360f / circleSegments) * i;
            float angleB = (360f / circleSegments) * (i + 1);

            Vector3 pointA = origin + Quaternion.Euler(0, angleA, 0) * Vector3.forward * m_behindDetectionRange;
            Vector3 pointB = origin + Quaternion.Euler(0, angleB, 0) * Vector3.forward * m_behindDetectionRange;

            Gizmos.DrawLine(pointA, pointB);
        }

        // Detection sphere origin
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(origin, 0.3f);

        // FOV cone
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.2f);
        float halfAngle = m_detectionAngle / 2f;
        Vector3 leftBoundary = Quaternion.Euler(0, -halfAngle, 0) * transform.forward * m_detectionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, halfAngle, 0) * transform.forward * m_detectionRange;

        Gizmos.DrawLine(origin, origin + leftBoundary);
        Gizmos.DrawLine(origin, origin + rightBoundary);

        // Arc between boundaries
        int segments = 20;
        for (int i = 0; i < segments; i++)
        {
            float angleA = -halfAngle + (m_detectionAngle / segments) * i;
            float angleB = -halfAngle + (m_detectionAngle / segments) * (i + 1);

            Vector3 pointA = origin + Quaternion.Euler(0, angleA, 0) * transform.forward * m_detectionRange;
            Vector3 pointB = origin + Quaternion.Euler(0, angleB, 0) * transform.forward * m_detectionRange;

            Gizmos.DrawLine(pointA, pointB);
        }

        // Stealth range arc
        Gizmos.color = new Color(0f, 1f, 1f, 0.2f);
        for (int i = 0; i < segments; i++)
        {
            float angleA = -halfAngle + (m_detectionAngle / segments) * i;
            float angleB = -halfAngle + (m_detectionAngle / segments) * (i + 1);

            Vector3 pointA = origin + Quaternion.Euler(0, angleA, 0) * transform.forward * m_stealthDetectionRange;
            Vector3 pointB = origin + Quaternion.Euler(0, angleB, 0) * transform.forward * m_stealthDetectionRange;

            Gizmos.DrawLine(pointA, pointB);
        }
    }
}

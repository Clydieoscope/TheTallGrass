using UnityEngine;

public class TrackPosition : MonoBehaviour
{
    [SerializeField] private LayerMask trackedLayers;
    [SerializeField] private float trackingHeightOffset = 1f;

    private Material grassMat;
    private static readonly int TrackerPositionID = Shader.PropertyToID("_TrackerPosition");

    private void Start()
    {
        grassMat = GetComponent<Renderer>().material;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((trackedLayers.value & (1 << other.gameObject.layer)) == 0) return;

        grassMat.SetVector(TrackerPositionID, other.transform.position + Vector3.up * trackingHeightOffset);
    }
}
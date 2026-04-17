using UnityEngine;

public class TrackPosition : MonoBehaviour
{
    private GameObject player;
    private Material grassMat;
    private Transform playerTransform;
    [SerializeField] private float trackingHeightOffset = 1f;

    void Start()
    {
        grassMat = GetComponent<Renderer>().material;
        player = GameObject.Find("Player");
        
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        grassMat.SetVector("_TrackerPosition", playerTransform.position + Vector3.up * trackingHeightOffset);
    }
}
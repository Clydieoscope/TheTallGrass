using UnityEngine;

public class MoveLightTrigger : MonoBehaviour
{
    [SerializeField] private MoveLight moveLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && moveLight != null)
        {
            moveLight.Activate();
        }
    }
}
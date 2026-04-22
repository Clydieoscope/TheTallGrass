using UnityEngine;

public class GoalActivator : MonoBehaviour
{
    [SerializeField] private Collider GoalCollider;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GoalCollider.enabled = true;
            GetComponent<Collider>().enabled = false;
            Debug.Log("Goal has been activated.");
        }
    }
}

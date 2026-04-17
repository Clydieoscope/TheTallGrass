using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private HealthSystem healthSystem;

    private Team ownerTeam;

    void Start()
    {
        SetOwnerTeam(healthSystem.team);
    }

    public void SetOwnerTeam(Team team)
    {
        ownerTeam = team;
    }

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem target = other.GetComponent<HealthSystem>();

        if (target == null)
        {
            Debug.Log("No target found");
            return;
        }

        // Prevent friendly fire
        if (target.team == ownerTeam)
            return;

        Debug.Log("Dealing damage to " + target.team);
        target.TakeDamage(damage);
    }
}
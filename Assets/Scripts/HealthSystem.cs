using UnityEngine;
// using Math;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private MonoBehaviour playerController;
    [SerializeField] private AudioSource mouthSource;

    private Animator anim;
    private bool dead;

    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    public void TakeDamage(float damage)
    {
        anim.SetTrigger("Hit");
        health = Mathf.Clamp(health - damage, 0f, maxHealth);
        Debug.Log(health);

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    public void Die()
    {
        dead = true;

        // disable player controller
        if (playerController != null)
            playerController.enabled = false;

        mouthSource.Stop();
        anim.SetTrigger("Dead");
        anim.SetLayerWeight(anim.GetLayerIndex("Combat"), 0f);

        GameStateManager.Instance.Lose();
        // call lose UI here.
    }

    public bool isDead()
    {
        return dead;
    }
}

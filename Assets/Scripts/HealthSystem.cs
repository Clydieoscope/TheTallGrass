using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health;

    [Header("References")]
    [SerializeField] private MonoBehaviour playerController;
    public CameraVFXHandler vfxCam;

    [Header("Events")]
    public UnityEvent<float> OnHealthChanged;   // normalized 0-1, drives HUD bar
    public UnityEvent OnHit;                    // wire to sfx.PlayHitSound, anim Hit trigger, etc.
    public UnityEvent OnDeath;                  // wire to sfx.PlayDeath, GameStateManager.Lose, etc.

    private Animator _anim;
    private PlayerSFXHandler _sfx;
    private bool _dead;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _sfx = GetComponent<PlayerSFXHandler>();

        health = maxHealth;
        OnHealthChanged?.Invoke(GetHealthNormalized());
    }

    public void TakeDamage(float damage)
    {
        if (_dead) return;

        health = Mathf.Clamp(health - damage, 0f, maxHealth);
        Debug.Log(health);

        OnHit?.Invoke();
        vfxCam?.TakeDamageEffect();

        if (_anim != null)
            _anim.SetTrigger("Hit");

        OnHealthChanged?.Invoke(GetHealthNormalized());

        if (health <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (_dead) return;

        health = Mathf.Min(maxHealth, health + amount);
        OnHealthChanged?.Invoke(GetHealthNormalized());
    }

    public void Die()
    {
        if (_dead) return;
        _dead = true;

        if (playerController != null)
            playerController.enabled = false;

        if (_anim != null)
        {
            _anim.SetTrigger("Dead");
            _anim.SetLayerWeight(_anim.GetLayerIndex("Combat"), 0f);
        }

        // OnDeath replaces direct GameStateManager and sfx calls.
        // Wire in Inspector: sfx.PlayDeath, GameStateManager.Instance.Lose, lose UI, etc.
        OnDeath?.Invoke();
    }

    public float GetHealthNormalized() => health / maxHealth;
    public float GetCurrentHealth() => health;
    public float GetMaxHealth() => maxHealth;
    public bool IsDead() => _dead;
}

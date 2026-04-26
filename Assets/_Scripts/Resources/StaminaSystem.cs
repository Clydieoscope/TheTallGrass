using UnityEngine;
using UnityEngine.Events;

public class StaminaSystem : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float drainRate = 20f;             // per second while sprinting
    [SerializeField] private float regenRate = 10f;             // per second while idle/walking
    [SerializeField] private float regenDelay = 1.5f;           // seconds before regen begins after sprinting

    [Header("Exhaustion Penalty")]
    [Tooltip("Speed multiplier applied when stamina hits zero. 0.5 = half normal walk speed.")]
    [SerializeField] [Range(0.1f, 0.9f)] private float exhaustionSpeedMultiplier = 0.5f;
    [Tooltip("Stamina percentage that must be reached before the penalty is lifted.")]
    [SerializeField] [Range(5f, 50f)] private float recoveryThreshold = 25f;

    [Header("Attack Regen")]
    [Tooltip("Seconds after the last attack before stamina starts regenerating.")]
    [SerializeField] private float attackRegenDelayDuration = 1.5f;

    [Header("Events")]
    public UnityEvent<float> OnStaminaChanged;  // normalized 0-1
    public UnityEvent OnExhausted;
    public UnityEvent OnRecovered;

    private float currentStamina;
    private float regenDelayTimer = 0f;
    private bool isExhausted = false;

    private bool isAttacking = false;
    private float attackRegenDelay = 1.5f;

    private StarterAssets.StarterAssetsInputs _input;
    private StarterAssets.ThirdPersonController _controller;

    private void Start()
    {
        currentStamina = maxStamina;
        _input = GetComponent<StarterAssets.StarterAssetsInputs>();
        _controller = GetComponent<StarterAssets.ThirdPersonController>();

        OnStaminaChanged?.Invoke(GetStaminaNormalized());
    }

    private void Update()
    {
        if (_input == null) return;

        // Tick down attack regen delay
        if (attackRegenDelay > 0f)
        {
            attackRegenDelay -= Time.deltaTime;
            if (attackRegenDelay <= 0f)
                isAttacking = false;
        }

        bool wantsToSprint = _input.sprint;
        bool canSprint = wantsToSprint && !isExhausted && currentStamina > 0f;

        if (canSprint)
        {
            // Drain stamina
            regenDelayTimer = regenDelay;
            currentStamina = Mathf.Max(0f, currentStamina - drainRate * Time.deltaTime);
            OnStaminaChanged?.Invoke(GetStaminaNormalized());

            if (currentStamina <= 0f)
                TriggerExhaustion();

        }
        else
        {
            // Count down regen delay
            if (regenDelayTimer > 0f)
            {
                regenDelayTimer -= Time.deltaTime;
            }
            else if (currentStamina < maxStamina && !isAttacking)
            {
                currentStamina = Mathf.Min(maxStamina, currentStamina + regenRate * Time.deltaTime);
                OnStaminaChanged?.Invoke(GetStaminaNormalized());

                if (isExhausted && currentStamina >= recoveryThreshold)
                    LiftExhaustion();
            }
        }

        // Keep blocking sprint while exhausted
        if (isExhausted && _input.sprint)
            _input.sprint = false;
    }

    private void TriggerExhaustion()
    {
        isExhausted = true;
        _input.sprint = false;

        if (_controller != null)
            _controller.SetExhaustionMultiplier(exhaustionSpeedMultiplier);

        OnExhausted?.Invoke();
    }

    private void LiftExhaustion()
    {
        isExhausted = false;

        if (_controller != null)
            _controller.SetExhaustionMultiplier(1f);

        OnRecovered?.Invoke();
    }

    public float GetStaminaNormalized() => currentStamina / maxStamina;
    public float GetCurrentStamina() => currentStamina;
    public bool IsExhausted() => isExhausted;

    public bool HasStamina(float amount) => currentStamina >= amount;

    public void UseStamina(float amount)
    {
        currentStamina = Mathf.Max(0f, currentStamina - amount);
        OnStaminaChanged?.Invoke(GetStaminaNormalized());

        isAttacking = true;
        attackRegenDelay = attackRegenDelayDuration;

        if (currentStamina <= 0f)
            TriggerExhaustion();
    }
}
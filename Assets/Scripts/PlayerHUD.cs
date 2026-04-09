using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private CanvasGroup healthCanvasGroup;

    [Header("Stamina Bar")]
    [SerializeField] private Image staminaBarFill;
    [SerializeField] private CanvasGroup staminaCanvasGroup;

    // Health color thresholds — Green → Yellow → Red
    private static readonly Color HealthHigh = new Color(0.18f, 0.85f, 0.35f);
    private static readonly Color HealthMid  = new Color(0.95f, 0.76f, 0.05f);
    private static readonly Color HealthLow  = new Color(0.90f, 0.18f, 0.18f);

    private float staminaFadeTimer = 0f;
    private const float staminaFadeDelay = 2f;
    private bool staminaWasFull = false;

    private void Start()
    {
        HealthSystem health = FindObjectOfType<HealthSystem>();
        StaminaSystem stamina = FindObjectOfType<StaminaSystem>();

        if (health != null)
            health.OnHealthChanged.AddListener(UpdateHealthBar);

        if (stamina != null)
            stamina.OnStaminaChanged.AddListener(UpdateStaminaBar);
    }

    public void UpdateHealthBar(float normalized)
    {
        if (healthBarFill == null) return;

        healthBarFill.fillAmount = normalized;

        if (normalized > 0.5f)
            healthBarFill.color = Color.Lerp(HealthMid, HealthHigh, (normalized - 0.5f) * 2f);
        else
            healthBarFill.color = Color.Lerp(HealthLow, HealthMid, normalized * 2f);
    }

    public void UpdateStaminaBar(float normalized)
    {
        if (staminaBarFill == null) return;

        staminaBarFill.fillAmount = normalized;

        staminaFadeTimer = staminaFadeDelay;
        staminaWasFull = false;

        if (staminaCanvasGroup != null)
            staminaCanvasGroup.alpha = 1f;
    }

    private void Update()
    {
        if (staminaCanvasGroup == null) return;

        if (!staminaWasFull && staminaBarFill != null && staminaBarFill.fillAmount >= 1f)
        {
            staminaFadeTimer -= Time.deltaTime;
            if (staminaFadeTimer <= 0f)
                staminaWasFull = true;
        }

        if (staminaWasFull)
            staminaCanvasGroup.alpha = Mathf.MoveTowards(staminaCanvasGroup.alpha, 0f, Time.deltaTime * 2f);
    }
}

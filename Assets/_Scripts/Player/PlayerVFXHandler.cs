using UnityEngine;

public class PlayerVFXHandler : MonoBehaviour
{
    [SerializeField] private Animator lighterAnimator;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private string lighterTriggerName = "Click";

    public void PlayLighterAnimation()
    {
        if (lighterAnimator != null)
            lighterAnimator.SetTrigger(lighterTriggerName);
    }

    public void PlaySmokeParticles()
    {
        if (smokeParticles != null)
            smokeParticles.Play();
    }
}
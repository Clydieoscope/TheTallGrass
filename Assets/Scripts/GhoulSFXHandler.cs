using UnityEngine;

public class GhoulSFXHandler : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource attackSource;
    [SerializeField] private AudioSource mouthSource;

    // [Header("Clips")]
    
    public void PlayAttackSound()
    {
        attackSource.Play();
    }

    public void PlayFootstep()
    {
        footstepSource.Play();
    }
}

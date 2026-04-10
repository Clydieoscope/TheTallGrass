using UnityEngine;

public class PlayerSFXHandler : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource foleySource;
    [SerializeField] private AudioSource mouthSource;
    
    [Range(0f, 1f)] public float volume = 1f;

    public AudioClip heartbeatClip;
    public AudioClip breathingClip;

    [Header("Footsteps")]
    public AudioClip[] footstepClips;

    [Header("Hit")]
    public AudioClip[] hitClips;

    [Header("Pain")]
    public AudioClip[] painClips;

    [Header("Death")]
    public AudioClip[] deathClips;


    private void PlayRandom(AudioSource source, AudioClip[] clips)
    {
        if (clips.Length == 0) return;

        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];

        source.PlayOneShot(clip, volume);
    }

    public void PlayHitSound()
    {
        PlayRandom(foleySource, hitClips);
        PlayRandom(mouthSource, painClips);
    }

    public void PlayFootstep()
    {
        PlayRandom(footstepSource, footstepClips);
    }

    public void PlayDeath()
    {
        PlayRandom(mouthSource, deathClips);
    }

    public void PlayHeartbeat()
    {
        foleySource.PlayOneShot(heartbeatClip, volume);
    }

    public void PlayBreathing()
    {
        mouthSource.PlayOneShot(breathingClip, volume);
    }

    public void disableMouthSource()
    {
        mouthSource.Stop();
    }
}

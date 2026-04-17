using UnityEngine;

public class GhoulSFXHandler : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource attackSource;
    [SerializeField] private AudioSource mouthSource;

    public AudioClip howlClip;

    [Header("Chirps")]
    public AudioClip[] chirpClips;

    [Header("Screams")]
    public AudioClip[] screamClips;

    [Header("Grunts")]
    public AudioClip[] gruntClips;

    [Header("Deaths")]
    public AudioClip[] deathClips;

    [Header("Hits")]
    public AudioClip[] hitClips;

    [Range(0f, 1f)] public float volume = 1f;
    private void PlayRandom(AudioSource source, AudioClip[] clips)
    {
        if (clips.Length == 0) return;

        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];

        source.PlayOneShot(clip, volume);
    }

    public void PlayAttackSound()
    {
        attackSource.Play();
    }

    public void PlayGrunt()
    {
        PlayRandom(mouthSource, gruntClips);
    }

    public void PlayDeath()
    {
        // mouthSource.Stop();
        PlayRandom(mouthSource, deathClips);
    }

    public void PlayHit()
    {
        PlayRandom(mouthSource, hitClips);
    }

    public void PlayFootstep()
    {
        footstepSource.Play();
    }

    public void PlayScream()
    {
        PlayRandom(mouthSource, screamClips);
    }

    public void PlayChirp()
    {
        PlayRandom(mouthSource, chirpClips);
    }

    public void PlayHowl()
    {
        mouthSource.PlayOneShot(howlClip, volume);
    }

    
}

using UnityEngine;

public class GhoulSFXHandler : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource attackSource;
    [SerializeField] private AudioSource mouthSource;

    [Header("Chirps")]
    public AudioClip[] chirpClips;


    [Header("Screams")]
    public AudioClip[] screamClips;
    [Range(0f, 1f)] public float volume = 1f;

    public void PlayAttackSound()
    {
        attackSource.Play();
    }

    public void PlayFootstep()
    {
        footstepSource.Play();
    }

    public void PlayScream()
    {
        if (screamClips.Length == 0) return;

        int index = Random.Range(0, screamClips.Length);
        AudioClip clip = screamClips[index];

        mouthSource.PlayOneShot(clip, volume);

    }

    public void PlayChirp()
    {
        if (chirpClips.Length == 0) return;

        int index = Random.Range(0, chirpClips.Length);
        AudioClip clip = chirpClips[index];

        mouthSource.PlayOneShot(clip, volume);
    }
}

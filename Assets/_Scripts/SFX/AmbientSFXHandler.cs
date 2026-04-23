using UnityEngine;

public class AmbientSFXHandler : MonoBehaviour
{
    public static AmbientSFXHandler Instance { get; private set; }

    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private AudioSource lowHealthAudioSource;
    [SerializeField] private AudioSource dangerAudioSource;

    public AudioClip creepySiren;
    public AudioClip pickup;
    public AudioClip hit;
    public AudioClip spotted;
    public AudioClip warning_1;
    public AudioClip warning_2;
    public AudioClip warning_3;
    public AudioClip chase;

    private float _lastSpottedTime = -30f;

    [Range(0f, 1f)] public float volume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlaySpotted()
    {
        if (Time.time - _lastSpottedTime < 30f) return;

        mainAudioSource.PlayOneShot(spotted, volume);
        _lastSpottedTime = Time.time;
    }
    public void PlaySiren() => mainAudioSource.PlayOneShot(creepySiren, volume);
    public void PlayPickup() => mainAudioSource.PlayOneShot(pickup, volume);
    public void PlayHit() => mainAudioSource.PlayOneShot(hit, volume);
    public void PlayWarningOne() => mainAudioSource.PlayOneShot(warning_1, volume);
    public void PlayWarningTwo() => mainAudioSource.PlayOneShot(warning_2, volume);
    public void PlayWarningThree() => mainAudioSource.PlayOneShot(warning_3, volume);
    public void PlayDanger() => dangerAudioSource.Play();
    public void StopDanger() => dangerAudioSource.Stop();
    public void PlayLowHealth() => lowHealthAudioSource.Play();
    public void StopLowHealth() => lowHealthAudioSource.Stop();
}
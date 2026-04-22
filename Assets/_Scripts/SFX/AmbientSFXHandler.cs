using UnityEngine;

public class AmbientSFXHandler : MonoBehaviour
{
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private AudioSource lowHealthAudioSource;
    [SerializeField] private AudioSource dangerAudioSource;

    public AudioClip creepySiren;
    public AudioClip bone;

    [Range(0f, 1f)] public float volume = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySiren()
    {
        mainAudioSource.PlayOneShot(creepySiren, volume);
        
    }

    public void PlayBone()
    {
        mainAudioSource.PlayOneShot(bone, volume);
        
    }

    public void PlayDanger()
    {
        dangerAudioSource.Play();
    }

    public void StopDanger()
    {
        dangerAudioSource.Stop();
    }

    
}

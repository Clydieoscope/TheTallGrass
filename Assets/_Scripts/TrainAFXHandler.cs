using UnityEngine;

public class TrainAFXHandler : MonoBehaviour
{
    private ParticleSystem[] _particleSystems;
    private AudioSource[] _audioSources;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        _audioSources = GetComponentsInChildren<AudioSource>();
    }

    public void EnableEffects()
    {
        foreach (var ps in _particleSystems)
            ps.Play();

        foreach (var audio in _audioSources)
            audio.Play();
    }

    public void DisableEffects()
    {
        foreach (var ps in _particleSystems)
            ps.Stop();

        foreach (var audio in _audioSources)
            audio.Stop();
    }
}
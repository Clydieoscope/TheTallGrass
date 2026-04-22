using UnityEngine;

public class ParticleParentController : MonoBehaviour
{
    private ParticleSystem[] particles;

    void Awake()
    {
        // Get all ParticleSystems in children (including inactive if needed)
        particles = GetComponentsInChildren<ParticleSystem>(true);
    }

    public void PlayAll()
    {
        foreach (ParticleSystem ps in particles)
        {
            ps.Play();
        }
    }

    public void StopAll()
    {
        foreach (ParticleSystem ps in particles)
        {
            ps.Stop();
        }
    }
}
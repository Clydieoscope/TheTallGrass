using UnityEngine;

public class SpawnListener : MonoBehaviour
{
    [SerializeField] private ParticleParentController spawnParticleController;
    [SerializeField] private AudioSource spawnAudioSource;
    private void OnEnable()
    {
        GameEvents.OnAgentSpawned += HandleAgentSpawned;
    }

    private void OnDisable()
    {
        GameEvents.OnAgentSpawned -= HandleAgentSpawned;
    }

    private void HandleAgentSpawned(GameObject agent)
    {
        spawnParticleController.PlayAll();
        spawnAudioSource.Play();
    }
}
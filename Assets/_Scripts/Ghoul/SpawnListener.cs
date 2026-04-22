using UnityEngine;

public class SpawnListener : MonoBehaviour
{
    [SerializeField] private GameObject mySpawner;
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

    private void HandleAgentSpawned(GameObject agent, GameObject spawner)
    {
        if (spawner != mySpawner)
            return;

        spawnParticleController.PlayAll();
        spawnAudioSource.Play();
    }
}
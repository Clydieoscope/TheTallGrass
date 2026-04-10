using UnityEngine;

public class GameVFXHandler : MonoBehaviour
{
    [SerializeField] private AudioSource nonDiegeticSource;

    [Range(0f, 1f)] public float volume = 1f;
    public AudioClip deathClip;


    public void PlayDeath()
    {
        nonDiegeticSource.PlayOneShot(deathClip, volume);
    }

}

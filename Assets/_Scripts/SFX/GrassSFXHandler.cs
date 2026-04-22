using UnityEngine;

public class GrassSFXHandler : MonoBehaviour
{

    private AudioSource audioSource;

    [Header("Rustle")]
    public AudioClip[] rustleClips;
    [Range(0f, 1f)] public float volume = 1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("Grass AudioSource component not found! Please attach this component");
        }
    }

    public void PlayRustle()
    {
        if (rustleClips.Length == 0) return;

        int index = Random.Range(0, rustleClips.Length);
        AudioClip clip = rustleClips[index];

        audioSource.PlayOneShot(clip, volume);
    }
}

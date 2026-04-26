using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class GameEndHandler : MonoBehaviour
{
    public static GameEndHandler Instance;

    [Header("End Sequence")]
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private AudioSource winMusicSource;
    [SerializeField] private AudioSource loseMusicSource;
    [SerializeField] private float sequenceDuration = 3f;

    private Vignette _vignette;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {

        if (postProcessVolume == null)
        {
            Debug.LogError("PostProcessVolume is not assigned on GoalTrigger!");
            return;
        }
        
        if (postProcessVolume.profile.TryGetSettings(out _vignette))
        {
            _vignette.intensity.value = 0.156f;
            Debug.Log("Vignette found successfully.");
        }
        else
        {
            Debug.LogError("Vignette effect not found in PostProcessVolume profile!");
        }

        if (winMusicSource != null)
        {
            winMusicSource.volume = 0f;
            winMusicSource.Stop();
        }

        if (loseMusicSource != null)
        {
            loseMusicSource.volume = 0f;
            loseMusicSource.Stop();
        }
    }

    public void TriggerLose()
    {
        StartCoroutine(EndSequence(won: false));
    }

    public void TriggerWin()
    {
        StartCoroutine(EndSequence(won: true));
    }

    private IEnumerator EndSequence(bool won)
    {
        float elapsed = 0f;
        float startTimeScale = Time.timeScale;
        float startVignette = _vignette.intensity.value;

        float startMixerVolume = VolumeManager.Instance.GetVolume();

        AudioSource musicToPlay = won ? winMusicSource : loseMusicSource;
        musicToPlay.volume = 0f;
        musicToPlay.Play();

        while (elapsed < sequenceDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / sequenceDuration;

            Time.timeScale = Mathf.Lerp(startTimeScale, 0f, t);
            _vignette.intensity.value = Mathf.Lerp(startVignette, 1f, t);

            float mixerVolume = Mathf.Lerp(startMixerVolume, -80f, t);
            VolumeManager.Instance.SetGameVolumeDecibels(mixerVolume);
            musicToPlay.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        Time.timeScale = 0f;
        _vignette.intensity.value = 1f;
        musicToPlay.volume = 1f;

        if (won)
            GameStateManager.Instance.Win();
        else
            GameStateManager.Instance.Lose();
    }
}
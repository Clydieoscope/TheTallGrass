using System.Collections;
using UnityEngine;

public class ChaseMusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource chaseMusicSource;
    [SerializeField] private float fadeInDuration  = 1f;
    [SerializeField] private float fadeOutDuration = 2f;

    private float _originalVolume;
    private Coroutine _fadeCoroutine;

    private void Start()
    {
        _originalVolume = chaseMusicSource.volume;
    }

    private void OnEnable()
    {
        GhoulChaseTracker.Instance.OnChaseCountChanged += HandleChaseCountChanged;
    }

    private void OnDisable()
    {
        if (GhoulChaseTracker.Instance != null)
            GhoulChaseTracker.Instance.OnChaseCountChanged -= HandleChaseCountChanged;
    }

    private void HandleChaseCountChanged(int count)
    {
        if (count > 0)
            StartFade(_originalVolume, fadeInDuration);
        else
            StartFade(0f, fadeOutDuration);
    }

    private void StartFade(float targetVolume, float duration)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(Fade(targetVolume, duration));
    }

    private IEnumerator Fade(float targetVolume, float duration)
    {
        float startVolume = chaseMusicSource.volume;
        float elapsed = 0f;

        if (targetVolume > 0f && !chaseMusicSource.isPlaying)
            chaseMusicSource.Play();

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            chaseMusicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }

        chaseMusicSource.volume = targetVolume;

        if (targetVolume == 0f)
            chaseMusicSource.Stop();
    }
}
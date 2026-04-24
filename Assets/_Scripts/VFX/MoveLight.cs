using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class MoveLight : MonoBehaviour
{
    private SplineAnimate splineAnimate;
    private Light _light;
    private AudioSource[] _audioSources;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1.5f;

    private void Start()
    {
        _light = GetComponent<Light>();
        _audioSources = GetComponents<AudioSource>();
        splineAnimate = GetComponent<SplineAnimate>();

        if (splineAnimate != null)
        {
            splineAnimate.enabled = false;
            splineAnimate.Completed += OnSplineCompleted;

            transform.position = splineAnimate.Container.EvaluatePosition(0f);
        }
    }

    private void OnDestroy()
    {
        if (splineAnimate != null)
            splineAnimate.Completed -= OnSplineCompleted;
    }

    public void Activate()
    {
        if (splineAnimate != null)
        {
            splineAnimate.enabled = true;
            splineAnimate.Play();
        }
    }

    private void OnSplineCompleted()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        if (_light == null) yield break;

        float startIntensity = _light.intensity;
        float[] startVolumes = new float[_audioSources.Length];

        for (int i = 0; i < _audioSources.Length; i++)
            startVolumes[i] = _audioSources[i].volume;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            _light.intensity = Mathf.Lerp(startIntensity, 0f, t);

            for (int i = 0; i < _audioSources.Length; i++)
                _audioSources[i].volume = Mathf.Lerp(startVolumes[i], 0f, t);

            yield return null;
        }

        _light.intensity = 0f;
        _light.enabled = false;

        foreach (var source in _audioSources)
        {
            source.volume = 0f;
            source.Stop();
        }
    }
}
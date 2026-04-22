using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class MoveLight : MonoBehaviour
{
    private SplineAnimate splineAnimate;
    private Light _light;
    
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1.5f;

    private void Start()
    {
        _light = GetComponent<Light>();
        splineAnimate = GetComponent<SplineAnimate>();

        if (splineAnimate != null)
        {
            splineAnimate.enabled = false;
            splineAnimate.Completed += OnSplineCompleted;

            // Position at the start of the spline on scene load
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
        StartCoroutine(FadeOutLight());
    }
 
    private IEnumerator FadeOutLight()
    {
        if (_light == null) yield break;
 
        float startIntensity = _light.intensity;
        float t = 0f;
 
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            _light.intensity = Mathf.Lerp(startIntensity, 0f, t);
            yield return null;
        }
 
        _light.intensity = 0f;
        _light.enabled = false;
    }
}
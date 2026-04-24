using System.Collections;
using System;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        GetComponent<Canvas>().sortingOrder = 999;
        canvasGroup.blocksRaycasts = false;
    }

    public void FadeToBlackAndBack(Action onBlack = null)
    {
        StartCoroutine(FadeSequence(onBlack));
    }

    private IEnumerator FadeSequence(Action onBlack)
    {
        yield return StartCoroutine(Fade(0f, 1f, -80f));

        onBlack?.Invoke();

        yield return StartCoroutine(Fade(1f, 0f, 0f));
    }

    private IEnumerator Fade(float fromAlpha, float toAlpha, float toVolume)
    {
        float elapsed = 0f;
        canvasGroup.alpha = fromAlpha;

        // VolumeManager.Instance.GetMasterVolume(out float startVolume);
        float startVolume = VolumeManager.Instance.GetMasterVolume();

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, t);
            VolumeManager.Instance.SetMasterVolumeDecibels(Mathf.Lerp(startVolume, toVolume, t));

            yield return null;
        }

        canvasGroup.alpha = toAlpha;
        canvasGroup.blocksRaycasts = toAlpha > 0f;
        VolumeManager.Instance.SetMasterVolumeDecibels(toVolume);
    }
}
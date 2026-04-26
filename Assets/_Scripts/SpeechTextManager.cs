using System.Collections;
using TMPro;
using UnityEngine;

public class SpeechTextManager : MonoBehaviour
{
    public static SpeechTextManager Instance;

    [SerializeField] private TextMeshProUGUI speechText;
    [SerializeField] private float defaultDisplayDuration = 3f;

    private Coroutine _clearCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowSpeech(string text, float duration = -1f)
    {
        speechText.text = text;

        if (_clearCoroutine != null)
            StopCoroutine(_clearCoroutine);

        float displayDuration = duration > 0f ? duration : defaultDisplayDuration;
        _clearCoroutine = StartCoroutine(ClearAfterDelay(displayDuration));
    }

    public void ShowSpeechDefault(string text)
    {
        ShowSpeech(text);
    }

    public void ClearSpeech()
    {
        if (_clearCoroutine != null)
            StopCoroutine(_clearCoroutine);

        speechText.text = string.Empty;
    }

    private IEnumerator ClearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        speechText.text = string.Empty;
    }
}
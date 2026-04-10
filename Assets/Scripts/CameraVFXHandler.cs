using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraVFXHandler : MonoBehaviour
{
    public float intensity = 0;

    [Header("Death Blur Settings")]
    [SerializeField] private float deathBlurFocusDistance = 0.1f;   // how close focus collapses to
    [SerializeField] private float deathBlurAperture = 32f;         // high aperture = strong blur
    [SerializeField] private float deathBlurFocalLength = 300f;     // high focal length = more compression/blur
    [SerializeField] private float deathBlurFadeSpeed = 1.5f;       // how fast blur fades in

    private PostProcessVolume _volume;
    private Vignette _vignette;
    private DepthOfField _dof;

    void Start()
    {
        _volume = GetComponent<PostProcessVolume>();

        _volume.profile.TryGetSettings<Vignette>(out _vignette);
        _volume.profile.TryGetSettings<DepthOfField>(out _dof);

        if (!_vignette)
            Debug.Log("ERROR, vignette empty");
        else
            _vignette.enabled.Override(false);

        if (!_dof)
            Debug.Log("ERROR, depth of field empty — add a Depth of Field layer to the Post Process profile");
        else
            _dof.enabled.Override(false);
    }

    public void TakeDamageEffect()
    {
        StartCoroutine(StartTakeDamageEffect());
    }

    public void DeathEffect()
    {
        StartCoroutine(StartDeathEffect());
    }

    private IEnumerator StartTakeDamageEffect()
    {
        intensity = 0.4f;

        _vignette.enabled.Override(true);
        _vignette.intensity.Override(0.4f);

        yield return new WaitForSeconds(0.4f);

        while (intensity > 0)
        {
            intensity -= 0.01f;
            if (intensity < 0) intensity = 0;
            _vignette.intensity.Override(intensity);
            yield return new WaitForSeconds(0.1f);
        }

        _vignette.enabled.Override(false);
    }

    private IEnumerator StartDeathEffect()
    {
        if (_dof == null) yield break;

        // Start from current focus distance and aperture if already active,
        // otherwise initialize from neutral values
        float currentFocus = _dof.enabled ? _dof.focusDistance.value : 10f;
        float currentAperture = _dof.enabled ? _dof.aperture.value : 5.6f;
        float currentFocalLength = _dof.enabled ? _dof.focalLength.value : 50f;

        _dof.enabled.Override(true);
        _dof.focusDistance.Override(currentFocus);
        _dof.aperture.Override(currentAperture);
        _dof.focalLength.Override(currentFocalLength);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * deathBlurFadeSpeed;

            _dof.focusDistance.Override(Mathf.Lerp(currentFocus, deathBlurFocusDistance, t));
            _dof.aperture.Override(Mathf.Lerp(currentAperture, deathBlurAperture, t));
            _dof.focalLength.Override(Mathf.Lerp(currentFocalLength, deathBlurFocalLength, t));

            yield return null;
        }

        // Ensure we land exactly on target values
        _dof.focusDistance.Override(deathBlurFocusDistance);
        _dof.aperture.Override(deathBlurAperture);
        _dof.focalLength.Override(deathBlurFocalLength);
    }
}

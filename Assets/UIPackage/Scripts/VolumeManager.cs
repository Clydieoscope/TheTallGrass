using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    public AudioMixer masterMixer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Game group
    public void SetVolume(float sliderValue)
    {
        float volumeInDecibels = Mathf.Log10(sliderValue) * 20;
        masterMixer.SetFloat("Game", volumeInDecibels);
        Debug.Log("Set Volume to " + volumeInDecibels);
    }

    public float GetVolume()
    {
        masterMixer.GetFloat("Game", out float volume);
        return volume;
    }

    public void SetVolumeDecibels(float decibels)
    {
        masterMixer.SetFloat("Game", decibels);
    }

    // Master group
    public float GetMasterVolume()
    {
        masterMixer.GetFloat("Master", out float volume);
        return volume;
    }

    public void SetMasterVolumeDecibels(float decibels)
    {
        masterMixer.SetFloat("Master", decibels);
    }
}
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer masterMixer;

    public void SetVolume(float sliderValue)
    {
        // Convert linear 0.0001-1.0 value to -80dB to 0dB
        // thanks AI
        float volumeInDecibels = Mathf.Log10(sliderValue) * 20;
        masterMixer.SetFloat("masterVol", volumeInDecibels);
        Debug.Log("Set Volume to " + volumeInDecibels);
    }
}

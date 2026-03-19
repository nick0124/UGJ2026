using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    public Slider volumeSlider;
    private float masterVolume = 1f;

    void Start()
    {
        LoadVolume();
    }

    public void SetMasterVolume()
    {
        masterVolume = Mathf.Clamp01(volumeSlider.value);
        AudioListener.volume = masterVolume;
        
        // Сохраняем настройку
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
    }
    
    public float GetMasterVolume()
    {
        return masterVolume;
    }
    
    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            volumeSlider.value = masterVolume;
            AudioListener.volume = masterVolume;
        }
    }
}

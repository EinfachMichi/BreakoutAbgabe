using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;

    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();

        musicSlider.value = SaveSystem.instance.activeSave.musicVolume;
        effectSlider.value = SaveSystem.instance.activeSave.effectVolume;
        SetMusicVolume();
        SetEffectVolume();
    }

    public void SetMusicVolume()
    {
        foreach (Sound s in _audioManager.music)
        {
            s.source.volume = musicSlider.value;
        }
        SaveSystem.instance.activeSave.musicVolume = musicSlider.value;
        SaveSystem.instance.Save();
    }

    public void SetEffectVolume()
    {
        foreach (Sound s in _audioManager.effectSounds)
        {
            s.source.volume = effectSlider.value;
        }
        SaveSystem.instance.activeSave.effectVolume = effectSlider.value;
        SaveSystem.instance.Save();
    }
}

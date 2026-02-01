using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider efectSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSlider.onValueChanged.AddListener(SetVolume);
        efectSlider.onValueChanged.AddListener(SetEfectVolume);

        efectSlider.value = PlayerPrefs.GetFloat("efxVol", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("SoundsVol", 0.75f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetVolume(float volume)
    {
        Mixer.SetFloat("SoundsVol", Mathf.Log10(musicSlider.value +0.001f) * 20);
    }

    public void SetEfectVolume(float volume)
    {
        Mixer.SetFloat("efxVol", Mathf.Log10(efectSlider.value + 0.001f) * 20);
    }
}

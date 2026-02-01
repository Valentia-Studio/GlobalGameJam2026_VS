using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider efectSlider;
    void Start()
    {
        // Asegurar rango 0..1 en caso de que en el inspector estén distintos
        musicSlider.minValue = 0f;
        musicSlider.maxValue = 1f;
        efectSlider.minValue = 0f;
        efectSlider.maxValue = 1f;

        // Prioridad: PlayerPrefs -> AudioMixer -> default(1)
        float savedMusic;
        float savedEfx;

        if (PlayerPrefs.HasKey("SoundsVol"))
        {
            savedMusic = PlayerPrefs.GetFloat("SoundsVol", 1f);
            Debug.Log($"AudioManager: Usando PlayerPrefs SoundsVol = {savedMusic}");
        }
        else if (Mixer.GetFloat("SoundsVol", out float dbMusic))
        {
            savedMusic = DbToLinear(dbMusic);
            Debug.Log($"AudioManager: Leyendo AudioMixer SoundsVol {dbMusic} dB -> {savedMusic} linear");
        }
        else
        {
            savedMusic = 1f;
            Debug.Log("AudioManager: No hay PlayerPrefs ni AudioMixer value para SoundsVol. Usando 1.0");
        }

        if (PlayerPrefs.HasKey("efxVol"))
        {
            savedEfx = PlayerPrefs.GetFloat("efxVol", 1f);
            Debug.Log($"AudioManager: Usando PlayerPrefs efxVol = {savedEfx}");
        }
        else if (Mixer.GetFloat("efxVol", out float dbEfx))
        {
            savedEfx = DbToLinear(dbEfx);
            Debug.Log($"AudioManager: Leyendo AudioMixer efxVol {dbEfx} dB -> {savedEfx} linear");
        }
        else
        {
            savedEfx = 1f;
            Debug.Log("AudioManager: No hay PlayerPrefs ni AudioMixer value para efxVol. Usando 1.0");
        }

        // Inicializar sliders antes de añadir listeners
        musicSlider.value = Mathf.Clamp01(savedMusic);
        efectSlider.value = Mathf.Clamp01(savedEfx);

        // Aplicar al mixer
        ApplyMusicVolume(musicSlider.value);
        ApplyEfectVolume(efectSlider.value);

        // Añadir listeners después de la inicialización
        musicSlider.onValueChanged.AddListener(SetVolume);
        efectSlider.onValueChanged.AddListener(SetEfectVolume);
    }

    public void SetVolume(float volume)
    {
        ApplyMusicVolume(volume);
        PlayerPrefs.SetFloat("SoundsVol", volume);
        PlayerPrefs.Save();
    }

    public void SetEfectVolume(float volume)
    {
        ApplyEfectVolume(volume);
        PlayerPrefs.SetFloat("efxVol", volume);
        PlayerPrefs.Save();
    }

    private void ApplyMusicVolume(float linear)
    {
        float dB = linear <= 0f ? -80f : Mathf.Log10(Mathf.Clamp(linear, 0.0001f, 1f)) * 20f;
        Mixer.SetFloat("SoundsVol", dB);
    }

    private void ApplyEfectVolume(float linear)
    {
        float dB = linear <= 0f ? -80f : Mathf.Log10(Mathf.Clamp(linear, 0.0001f, 1f)) * 20f;
        Mixer.SetFloat("efxVol", dB);
    }

    // Convierte dB -> lineal (0..1)
    private float DbToLinear(float dB)
    {
        if (dB <= -80f) return 0f;
        return Mathf.Clamp01(Mathf.Pow(10f, dB / 20f));
    }
}

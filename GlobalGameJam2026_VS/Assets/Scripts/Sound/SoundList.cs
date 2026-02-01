using UnityEngine;

public class SoundList : MonoBehaviour
{
   [Header("Reference to this script")]
    public static SoundList instance;

    [Header("Sounds")]
    public AudioClip test;
    public AudioClip train_Start;
    public AudioClip train_PassingThroughTunnel;
    public AudioClip train_Stop;
    public AudioClip train_ChooChooSound;
    public AudioClip animal_Scared;
    public AudioClip elephant;
    public AudioClip owl;
    public AudioClip mouse;
    public AudioClip zebra;
    public AudioClip tiger;
    public AudioClip chameleon_Transform;
    public AudioClip grabPeople;
    public AudioClip peopleTalking;

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip inGameMusic;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

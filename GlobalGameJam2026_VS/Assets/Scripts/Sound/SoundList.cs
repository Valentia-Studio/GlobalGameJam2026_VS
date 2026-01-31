using UnityEngine;

public class SoundList : MonoBehaviour
{
   [Header("Reference to this script")]
    public static SoundList instance;
    
    [Header("Sounds")]
    public AudioClip test;

    [Header("Music")]
    public AudioClip bgMusic;

    private void Awake()
    {
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

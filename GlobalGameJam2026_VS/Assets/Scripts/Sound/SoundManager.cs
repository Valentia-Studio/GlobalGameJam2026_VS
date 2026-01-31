using UnityEngine;
using System.Collections.Generic;
public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    AudioSource soundSource, musicSource;


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

    private void Start()
    {
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
    }

    /// <summary>
    /// Call this function in case you want to make a sound (check sounds in "SoundList" to play)
    /// </summary>
    /// <param name="sound"></param>
    public void PlaySound(AudioClip sound)
    {
        soundSource.pitch = Random.Range(.95f, 1.05f);

        soundSource.PlayOneShot(sound, .5f);
    }

    /// <summary>
    /// Call this function in case you want to change music (check music in "SoundList" to play)
    /// </summary>
    /// <param name="song"></param>
    public void PlayMusic(AudioClip song)
    {
        musicSource.clip = song;
        musicSource.volume = .5f;
        musicSource.Play();
    }
    public void PlayMusic()
    {
        musicSource.volume = .5f;
        musicSource.Play();
    }

}

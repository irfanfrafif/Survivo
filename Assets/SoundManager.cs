using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource musicSource, effectSource;

    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioClip[] bgms;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void PlaySound(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    void PlayBGM(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void ChangeEffectVolume(float value)
    {
        effectSource.volume = value;
    }

    public void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void PlayClip(int i)
    {
        PlaySound(clips[i]);
    }

    public void PlayBGM(int i)
    {
        PlayBGM(bgms[i]);
    }

    public float GetMasterVolume()
    {
        return AudioListener.volume;
    }

    public float GetEffectVolume()
    {
        return effectSource.volume;
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }
}

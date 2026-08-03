using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    MUSICAMENU,
    PASSOS,
    BATEGOBLIN,
    ATIRASLIME,
    PESCARIA,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;

    [Header("Canais de Áudio")]
    [SerializeField] private AudioSource sfxSource;   // Canal de Efeitos
    [SerializeField] private AudioSource musicSource; // Canal de Música

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.sfxSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }

    public static void PlayMusic(SoundType sound, float volume = 1)
    {
        instance.musicSource.clip = instance.soundList[(int)sound];
        instance.musicSource.volume = volume;
        instance.musicSource.loop = true; 
        instance.musicSource.Play();
    }
}

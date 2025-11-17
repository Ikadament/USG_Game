using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SoundClass[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (SoundClass sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.spatialBlend = sound.spatialBlend;
            sound.source.loop = sound.loop;
        }
    }

    private void Start()
    {
        Play("Bg");
        
    }

    public void Play(string name)
    {
        SoundClass s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
        Debug.Log("Play");
    }
}

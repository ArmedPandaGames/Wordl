using UnityEngine;
using System;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance;

    public Sound[] sounds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string id)
    {
        Sound s = Array.Find(sounds, sound => sound.id == id);
        if (s == null) return;

        s.source.Play();
    }

    public void Stop(string id)
    {
        Sound s = Array.Find(sounds, sound => sound.id == id);
        if (s == null) return;

        s.source.Stop();
    }

    public void PlayOneShot(string id)
    {
        Sound s = Array.Find(sounds, sound => sound.id == id);
        if (s == null) return;

        s.source.PlayOneShot(s.clip);
    }
}
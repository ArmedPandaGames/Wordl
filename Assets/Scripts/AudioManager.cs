using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SoundPlayer.Instance.Play("music");
    }

    public void MuteAudio()
    {
        SoundPlayer.Instance.PlayOneShot("toggle");
        if (audioSource != null)
        {
            foreach (Sound s in SoundPlayer.Instance.sounds)
            {
                if (s.id != "music") // Don't mute music when muting audio
                    s.source.mute = !s.source.mute;
            }
        }
    }

    public void MuteMusic()
    {
        SoundPlayer.Instance.PlayOneShot("toggle");
        if (audioSource != null)
        {
            foreach (Sound s in SoundPlayer.Instance.sounds)
            {
                if (s.id == "music")
                {
                    s.source.mute = !s.source.mute;
                }
            }
        }
    }
}

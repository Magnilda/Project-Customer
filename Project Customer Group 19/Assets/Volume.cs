using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    private float volume;
    [SerializeField] private AudioSource SoundTrack;
    [SerializeField] private AudioSource SoundEffect;
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] Slider test;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartSoundTrack("menu_music");
        ChangeVolume(test);
    }

    void Update()
    {
        if(test !=null)
        {
            ChangeVolume(test);
        }
    }


    public void ChangeVolume(Slider sl)
    {
        volume = sl.value;
        SoundTrack.volume = volume;
        SoundEffect.volume = volume;
    }

    public void StopSoundEffect()
    {
        SoundEffect.Stop();
    }

    public void StopSoundTrack()
    {
        SoundTrack.Stop();
    }

    public void StartSoundTrack(string name)
    {
        foreach(AudioClip ac in audioClips)
        {
            if(ac.name == name)
            {
                SoundTrack.clip = ac;
                SoundTrack.Play();
            }
        }
    }

    public void StartSoundEffect(string name)
    {
        foreach (AudioClip ac in audioClips)
        {
            if (ac.name == name)
            {
                SoundEffect.clip = ac;
                SoundEffect.Play();
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;
    public AudioMixerGroup audioMixer;

    private void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
            //return;
        }*/
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = audioMixer;
        }
    }

    public void Play(string name, bool randomPitch)
    {
        //Find a file with a certain name in the sounds array
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //If var randomPitch is true, then randomize a pitch 
        if (randomPitch == true)
        {
            RandomPitch(s);
        }       
        
        //Play sound file
        s.source.Play(); 
    }

    void RandomPitch(Sound s)
    {
        s.source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
    }

}

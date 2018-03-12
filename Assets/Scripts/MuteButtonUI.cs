using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MuteButtonUI : MonoBehaviour {

    public AudioMixer audioMixer;
    private bool isPressed = true;

    public Sprite mute;
    public Sprite unmute;

    public void MuteGame()
    {
        if (isPressed)
        {
            audioMixer.SetFloat("volume", -80f);
            GetComponent<Image>().sprite = mute;
        }
        else
        {
            audioMixer.SetFloat("volume", 0f);
            GetComponent<Image>().sprite = unmute;
        }
        isPressed = !isPressed;
    }
	
}

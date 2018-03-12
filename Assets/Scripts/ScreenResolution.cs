using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ScreenResolution : MonoBehaviour {

    
    public AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager object found!");
        }
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            audioManager.Play("BGMusic0", false);
        }
        else if (SceneManager.GetActiveScene().name == "EndScreen")
        {
            audioManager.Play("EndMusic", false);
        }
    }


    // Update is called once per frame
    void FixedUpdate () {
        //Screen.SetResolution(460,690, false);		
	}
    
}

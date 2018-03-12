using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class StartMenu : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject settingsMenu;
    public AudioMixer audioMixer;


	public void StartGame()
    {
        SceneManager.LoadScene("Level01");
    }

    public void SettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);                
    }

    public void ReturnToMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void FacebookLink()
    {
        Application.OpenURL("https://www.facebook.com/sohrau/");
    }

    public void SohrauLink()
    {
        Application.OpenURL("http://sohrau.pl/");
    }

}

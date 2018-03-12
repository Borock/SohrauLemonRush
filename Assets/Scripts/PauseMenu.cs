using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour {

    public GameMaster gm;

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
        SceneManager.LoadScene("MainMenu");
    }
	
    public void ResumeGame()
    {
        Debug.Log("Resuming");
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TryAgain()
    {
        gm.Retry();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);        
    }    
}

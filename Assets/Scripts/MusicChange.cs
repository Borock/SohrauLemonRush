using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicChange : MonoBehaviour {


	
	void Start () {
        if(SceneManager.GetActiveScene().name == "Level01")
        {
            
            FindObjectOfType<AudioManager>().Play("BGMusic", false);
        }
        else if ((SceneManager.GetActiveScene().name == "Level02"))
        {
            FindObjectOfType<AudioManager>().Play("BGMusic2", false);
        }
        else if ((SceneManager.GetActiveScene().name == "Level03"))
        {
            FindObjectOfType<AudioManager>().Play("BGMusic3", false);
        }		
	}
	
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountDownUI : MonoBehaviour {

    public TextMeshProUGUI countText;
    private int countdown = 3;

	// Use this for initialization
	void Start () {
        StartCoroutine(Countdown());	
	}

    IEnumerator Countdown()
    {
        for(int i = countdown; i>0 ; i--)
        {
            countText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        gameObject.SetActive(false);
    }	
}

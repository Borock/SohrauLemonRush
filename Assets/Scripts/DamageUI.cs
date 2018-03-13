using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour {

    [SerializeField] private Sprite[] hpIndicators;
    private Image lives;

    private void Start()
    {
        lives = GetComponent<Image>();
    }

    public void HPIndicator(int hp)
    {
        if (hpIndicators.Length == 0)
        {
            Debug.LogError("HP Sprite array is empty!");
        }

        if(hp >= 5)
        {
            //lives.sprite = hpIndicators[5];
        }
        else if (hp == 4)
        {
            lives.sprite = hpIndicators[4];
        }
        else if (hp == 3)
        {
            lives.sprite = hpIndicators[3];
        }
        else if (hp == 2)
        {
            lives.sprite = hpIndicators[2];
        }
        else if (hp == 1)
        {
            lives.sprite = hpIndicators[1];
        }
        else if (hp <= 0)
        {
            lives.sprite = hpIndicators[0];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpriteGenerator : MonoBehaviour {
        
    [SerializeField] private Sprite[] sprite;
    private int spriteNumer;

	// Use this for initialization
	void Start () {
        //When object is spawning, select a random number from the sprite array and set the randomized sprite as active
        spriteNumer = Random.Range(0, sprite.Length);        
        GetComponent<SpriteRenderer>().sprite = sprite[spriteNumer];		
	}
	
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour {

    float spin;

    private void Awake()
    {
        //At spawn, randomize the rotation rate
        spin = Random.Range(-10, 10);
    }

    // Update is called once per frame
    void Update () {

        //Apply rotation every frame
        transform.Rotate(new Vector3(0f, 0f, spin));		
	}
}

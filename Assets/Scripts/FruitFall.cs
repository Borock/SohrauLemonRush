using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitFall : MonoBehaviour {   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Lemon")
        {
            //Decrease HP by some %
            //Debug.Log("Lemon fell!");
            GameMaster.gm.SendMessage("DamagePlayer");                       
            Destroy(collision.gameObject);
        }

        if(collision.tag == "Obstacle" || collision.tag == "LifeUp")
        {
            Destroy(collision.gameObject);
        }
    }
}

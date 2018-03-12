using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCatch : MonoBehaviour {

     private void OnTriggerEnter2D(Collider2D collision)
     {
        if (collision.gameObject.tag == "Lemon")
        {
            GameMaster.gm.SendMessage("LemonCatch");
            Destroy(collision.gameObject);
        }
        
        if (collision.gameObject.tag == "Obstacle")
        {
            GameMaster.gm.SendMessage("DamagePlayer");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "LifeUp")
        {
            GameMaster.gm.SendMessage("LifeUp");      //Add function for restoring 1HP
            Destroy(collision.gameObject);
        }
     }
}

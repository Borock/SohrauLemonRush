using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Vector2 mousePosition;
    private float mousePosition2;   //Test for axis' control
    private float mousePositionX;
    [SerializeField]
    private float offsetX;
    private float offsetY;

    private void Start()
    {
        //movementSpeed = (movementSpeed * Camera.main.scaledPixelWidth) / 450;
        //Get mouse position at the start 
        mousePosition = Input.mousePosition;        

        //Get offset x, that is the space in which the player can move
        offsetX = Screen.width - gameObject.GetComponent<Collider2D>().bounds.extents.x; //This value needs fine tuning 
        offsetX = Camera.main.ScreenToWorldPoint(new Vector3(offsetX,0,0)).x;               //This is used to convert the offset to screen position

        //Offset in Y axix - the Y position of the player
        offsetY = -4.2f;      //Change this to be based on screen size or some shit
      
    }

    private void FixedUpdate()
    {
        //Calculate mouse X position, convert it to world space and clamp it in the camera view, substracted by offsets specified above
        mousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        mousePosition = new Vector2(Mathf.Clamp(mousePositionX, -offsetX, offsetX), offsetY);
        
        //Move game object to the location of the mouse (Y axis stays the same)
        gameObject.transform.position = mousePosition;

        //This works, after setting it up in unity editor BUT it only reacts to the movement of mouse, not to its position.
        /*
        mousePosition2 = Input.GetAxis("Horizontal");          //Horizontal axis is not corresponding to mouse in any way!! 
        Debug.Log(mousePosition2);                             
        */
    }
}

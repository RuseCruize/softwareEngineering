using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/*
    AI program
*/

public class AI_Enemy : MonoBehaviour
{
    // Moving
    public Transform groundDetection;
    public Transform nextPlatorm;
    public float speed;
    public float distance;

    public Rigidbody2D rb;          //Holds the character in Unity

    // Jumping
    public float jumpForce;
    // Find near enemy

    // Find near weapon


    public bool movingRight = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        RaycastHit2D platInfo = Physics2D.Raycast(nextPlatorm.position, Vector2.up, jumpForce);
       
        if (groundInfo.collider == false)
        {
            if(movingRight == true)
            {                
                //rb.velocity = Vector2.up * 1f;
                rb.AddForce (Vector2.up * 2f);
                //transform.eulerAngles = new Vector3(0, -180, 0);
                //movingRight = false;                
            }
            else if(movingRight == false)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }
}

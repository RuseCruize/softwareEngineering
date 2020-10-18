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

    // Jumping

    // Find near enemy

    //Find near weapon

    //private bool jump = false;
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

        if (groundInfo.collider == false)
        {
            if(movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                
                movingRight = false;
                
            }else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }

        }

    }
}

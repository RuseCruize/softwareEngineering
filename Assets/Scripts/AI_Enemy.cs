using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/*
    AI program
    
    move n position or stop at position
        shoot or skip turn
    
    if near player
        agro or shoot
    else
        patrol 


*/

public class AI_Enemy : MonoBehaviour
{
    // Moving
    public float dirX;

    public float speed = 2f;
    public float distLimit = 10f;
    public float moveCounter;    

    public Rigidbody2D rb;          //Holds the character in Unity

    public Transform Player;

    // Find near enemy
    public float agroRange;     // Tuning it, but In Progress

    // Find near weapon
    // In Progress
    
    // Shoot
    //  In Progress

    public bool movingRight = false;

    Vector3 localScale; //origin location
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
		dirX = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Range of Guy
        float distToPlayer = Vector2.Distance(transform.position, Player.position);
        print("distToPlayer:" + distToPlayer);

        /* if(distToPlayer < agroRange)
        {
            //  chase player
            ChasePlayer();
        }
        else    //  stop chase
        {
            stopChasePlayer();
        } */

        if (transform.position.x < -1f)     // Fix space 
		{
            dirX = 1f;
            moveCounter = transform.position.x;
        }
		else if (transform.position.x > 15f)
			dirX = -1f;
    }

    void FixedUpdate()
	{
		rb.velocity = new Vector2 (dirX * speed, rb.velocity.y);
	}

    void LateUpdate()
	{
		CheckWhereToFace();
	}

    void CheckWhereToFace()
	{
		if (dirX > 0)
			movingRight = true;
		else if (dirX < 0)
			movingRight = false;

		if (((movingRight) && (localScale.x < 0)) || ((!movingRight) && (localScale.x > 0)))
			localScale.x *= -1;

		transform.localScale = localScale;
	}

    //  Jumpping process in switches
    void OnTriggerEnter2D (Collider2D col)
    {
        switch (col.tag) 
        {
        case "Wall":
            rb.AddForce (Vector2.up * 400f);
            break;                          
        }
    } 


    /* void ChasePlayer()
    {
        if( transform.position.x < Player.position.x)
        {
            //  move right if player on left side
            rb.velocity = new Vector2(speed, 0);
        }
        else if( transform.position.x > Player.position.x)
        {
            // move left if player on right side
            rb.velocity = new Vector2(-speed,0);
        }
    }
    void stopChasePlayer()
    {
        rb.velocity = new Vector2(0,0);
    } */

    // Testing 
    /* void OnTriggerEnter2D()
    {
        Debug.Log("Trigger Touched");
    }
    void OnTriggerStay2D()
    {
        Debug.Log("Still Touching");
    }
    void OnTriggerExit2D()
    {
        Debug.Log("Exit Trigger");
    } */

}

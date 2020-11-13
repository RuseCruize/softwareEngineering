using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : MonoBehaviour
{
    public float speed;
    public float jumpVelocity;
    public Rigidbody2D body;
    public LayerMask groundLayer;
    public int health;
    public int distanceMoved;
    public static int totalDistance = 1000;
    public string owner;

    public Guy(Vector3 position, string owner)
    {
        this.owner = owner;
        this.health = 100;
    }

    bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, 1, groundLayer);
        return raycastHit.collider != null;
    }

    public void Move()
    {
        bool isUp = Input.GetKey(KeyCode.UpArrow);
        float leftSpeed = Input.GetKey(KeyCode.LeftArrow) ? -speed : 0;
        float rightSpeed = Input.GetKey(KeyCode.RightArrow) ? speed : 0;

        if (this.isGrounded() && isUp)
        {
            // Jump logic, check to see if touching ground
            body.velocity = Vector2.up * jumpVelocity;
        }

        transform.position += transform.right * leftSpeed * Time.deltaTime;
        transform.position += transform.right * rightSpeed * Time.deltaTime;

        if (isUp || (leftSpeed < 0 || rightSpeed > 0) && (leftSpeed + rightSpeed != 0))
            distanceMoved++;
    }

    public bool FullMovement()
    {
        if (distanceMoved >= totalDistance)
        {
            distanceMoved = 0;
            return true;
        }
        else
            return false;
    }
}

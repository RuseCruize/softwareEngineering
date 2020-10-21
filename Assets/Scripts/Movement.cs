using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float jumpVelocity;
    public Rigidbody2D body;
    public LayerMask groundLayer;
    public bool IsActive = false;

    // Update is called once per frame
    void Update()
    {
        if(IsActive)
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
        }
    }

    bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, 1, groundLayer);
        return raycastHit.collider != null;
    }

    public void SetActive()
    {
        IsActive = true;
    }

    public void SetInactive()
    {
        IsActive = false;
    }
}

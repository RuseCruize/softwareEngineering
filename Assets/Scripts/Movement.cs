using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D body;
    private BoxCollider2D boxCollider;
    public LayerMask groundLayer;

    private void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D> ();
    }

    // Update is called once per frame
    void Update()
    {
        bool isUp = Input.GetKey(KeyCode.UpArrow);
        float leftSpeed = Input.GetKey(KeyCode.LeftArrow) ? -speed : 0;
        float rightSpeed = Input.GetKey(KeyCode.RightArrow) ? speed : 0;

        if (this.isGrounded() && isUp)
        {
            // Jump logic, check to see if touching ground
            float jumpVelocity = 10;
            body.velocity = Vector2.up * jumpVelocity;
        }

        transform.position += transform.right * leftSpeed * Time.deltaTime;
        transform.position += transform.right * rightSpeed * Time.deltaTime;
    }

    bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, 1, groundLayer);
        return raycastHit.collider != null;
    }
}

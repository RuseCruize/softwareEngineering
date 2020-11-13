using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : MonoBehaviour
{
    public static int maxDistance = 5;

    public float startPosition;

    public enum State
    {
        Moving,
        Acting,
        Waiting,
        Dead
    }

    public State currentState;
    public GameManager.Weapon currentWeapon;

    public float speed;
    public float jumpVelocity;
    public float distanceMoved;

    public Rigidbody2D body;
    public LayerMask groundLayer;
    public SpriteRenderer spriteRenderer;

    public int health;
    public string owner;

    public Guy(Vector3 position, string owner)
    {
        this.owner = owner;
        health = 100;
        currentState = State.Waiting;
    }

    bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, 1, groundLayer);
        return raycastHit.collider != null;
    }

    public void Activate()
    {
        startPosition = transform.position.x;
        currentState = State.Moving;
        currentWeapon = GameManager.Weapon.Unarmed;
    }

    public void Move()
    {
        bool isUp = Input.GetKey(KeyCode.UpArrow);
        bool endTurn = Input.GetKey(KeyCode.Space);
        float leftSpeed = Input.GetKey(KeyCode.LeftArrow) ? -speed : 0;
        float rightSpeed = Input.GetKey(KeyCode.RightArrow) ? speed : 0;

        if (rightSpeed != 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }


        if (this.isGrounded() && isUp)
        {
            // Jump logic, check to see if touching ground
            body.velocity = Vector2.up * jumpVelocity;
        }

        transform.position += transform.right * leftSpeed * Time.deltaTime;
        transform.position += transform.right * rightSpeed * Time.deltaTime;

        float endPosition = transform.position.x;
        distanceMoved = Mathf.Abs(startPosition - endPosition);

        if (distanceMoved >= maxDistance || endTurn)
        {
            currentState = State.Acting;
        }
    }

    void SelectWeapon(GameManager.Weapon weapon)
    {
        currentWeapon = weapon;

        switch (weapon)
        {
            case GameManager.Weapon.Machete:
                break;

            case GameManager.Weapon.Pistol:
                break;
        }
    }

    public void Act()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon(GameManager.Weapon.Machete);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectWeapon(GameManager.Weapon.Pistol);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            currentState = State.Waiting;
        }
    }
}

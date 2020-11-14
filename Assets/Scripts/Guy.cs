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
    public GameManager.WeaponType currentWeaponType;
    public GameObject currentWeapon;

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
        currentWeaponType = GameManager.WeaponType.Unarmed;
        currentWeapon = null;
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
        else if (leftSpeed != 0)
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

    public void MoveAI()
    {

    }

    void SelectWeapon(GameManager.WeaponType weaponType)
    {
        if (currentWeapon != null)
        {
            GameObject.Destroy(currentWeapon);
        }

        currentWeaponType = weaponType;

        switch (weaponType)
        {
            case GameManager.WeaponType.Machete:
                currentWeapon = GameObject.Instantiate(GameManager.STATE.Machete);
                currentWeapon.transform.parent = transform;
                if (spriteRenderer.flipX)
                {
                    currentWeapon.transform.position = transform.position + new Vector3(-0.2f, -0.4f, 0);
                    currentWeapon.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    currentWeapon.transform.position = transform.position + new Vector3(0.2f, -0.4f, 0);
                }
                break;

            case GameManager.WeaponType.Pistol:
                currentWeapon = GameObject.Instantiate(GameManager.STATE.Pistol);
                currentWeapon.transform.parent = transform;
                if (spriteRenderer.flipX)
                {
                    currentWeapon.transform.position = transform.position + new Vector3(-0.2f, -0.45f, 0);
                    currentWeapon.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    currentWeapon.transform.position = transform.position + new Vector3(0.2f, -0.45f, 0);
                }
                break;
        }

        
    }

    public float Aim(Vector3 target)
    {
        float lookAngle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;

        if (currentWeapon.GetComponent<SpriteRenderer>().flipX)
        {
            if (lookAngle > 90)
            {
                lookAngle = Mathf.Clamp(lookAngle, 130, 180);
            }
            else
            {
                lookAngle = Mathf.Clamp(lookAngle, -180, -130);
            }

            lookAngle = lookAngle + 180;
        }
        else
        {
            lookAngle = Mathf.Clamp(lookAngle, -50f, 50f);
        }

        currentWeapon.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);

        return lookAngle;
    }

    public void Act()
    {
        // Select Weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon(GameManager.WeaponType.Machete);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectWeapon(GameManager.WeaponType.Pistol);
        }

        // Aim Weapon
        if (currentWeapon != null)
        {
            Vector3 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - currentWeapon.transform.position;
            Aim(lookDirection);
        }

        // Use Weapon
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
            SelectWeapon(GameManager.WeaponType.Unarmed);
            currentState = State.Waiting;
        }
    }

    public void ActAI()
    {

    }

    void Attack()
    {
        switch (currentWeaponType)
        {
            case GameManager.WeaponType.Machete:
                RaycastHit2D[] targets = Physics2D.LinecastAll(currentWeapon.transform.position, currentWeapon.transform.rotation * Vector3.forward, (1 << 9));
                for (int i = 0; i < targets.Length; i++)
                {
                    Guy targetGuy = targets[i].collider.GetComponent<Guy>();
                    if (targetGuy.owner != this.owner)
                    {
                        Debug.Log("HIT");
                    }
                }
                break;
            case GameManager.WeaponType.Pistol:
                
                break;
        }
    }

    void OnDrawGizmos()
    {
        if (currentWeaponType == GameManager.WeaponType.Machete)
        {
            Gizmos.DrawLine(currentWeapon.transform.position, currentWeapon.transform.position + new Vector3(10, 10, 10) + currentWeapon.transform.forward);
            
        }
    }
}

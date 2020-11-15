﻿using System.Collections;
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
    float lookAngle;
    Vector3 clickPoint;

    public float speed;
    public float jumpVelocity;
    public float distanceMoved;

    public Rigidbody2D body;
    public LayerMask groundLayer;
    public SpriteRenderer spriteRenderer;

    public float maxHealth;
    public float health;
    public string owner;

    //[SerializeField] HealthBar healthBar;


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
        if (currentState != State.Dead)
        {
            startPosition = transform.position.x;
            currentState = State.Moving;
            currentWeaponType = GameManager.WeaponType.Unarmed;
            currentWeapon = null;
        }
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
                break;

            case GameManager.WeaponType.Pistol:
                currentWeapon = GameObject.Instantiate(GameManager.STATE.Pistol);
                break;
        }

        currentWeapon.transform.parent = transform;
    }

    public void Aim(Vector3 target)
    {
        Vector3 guyCenter = target + currentWeapon.transform.position - transform.position;
        if (guyCenter.x >= 0)
        {
            currentWeapon.GetComponent<SpriteRenderer>().flipX = false;
            spriteRenderer.flipX = false;

            if (currentWeaponType == GameManager.WeaponType.Machete)
            {
                currentWeapon.transform.position = transform.position + new Vector3(0.2f, -0.4f, 0);
            }
            else if (currentWeaponType == GameManager.WeaponType.Pistol)
            {
                currentWeapon.transform.position = transform.position + new Vector3(0.2f, -0.4f, 0);
            }
        }
        else
        {
            currentWeapon.GetComponent<SpriteRenderer>().flipX = true;
            spriteRenderer.flipX = true;

            if (currentWeaponType == GameManager.WeaponType.Machete)
            {
                currentWeapon.transform.position = transform.position + new Vector3(-0.2f, -0.45f, 0);
            }
            else if (currentWeaponType == GameManager.WeaponType.Pistol)
            {
                currentWeapon.transform.position = transform.position + new Vector3(-0.2f, -0.45f, 0);
            }
        }

        lookAngle = Mathf.Atan2(target.y, guyCenter.x) * Mathf.Rad2Deg;

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
            clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPoint.z = 0;
            Vector3 lookDirection = clickPoint - currentWeapon.transform.position;
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
                GameObject damageFlash = GameObject.Instantiate(GameManager.STATE.MacheteFlash);
                damageFlash.transform.position = currentWeapon.transform.position;
                damageFlash.transform.rotation = currentWeapon.transform.rotation;
                if (spriteRenderer.flipX)
                {
                    damageFlash.transform.localScale = new Vector3(-10, 5, 5);
                }
                
                break;
            case GameManager.WeaponType.Pistol:
                GameObject bullet = GameObject.Instantiate(GameManager.STATE.Bullet);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bullet.transform.position = currentWeapon.transform.position;
                bullet.transform.rotation = currentWeapon.transform.rotation;
                if (spriteRenderer.flipX)
                {
                    bulletScript.flipped = true;
                    bullet.transform.localScale = new Vector3(-1.5f, 1, 1);
                }
                break;
        }
    }

    void OnDrawGizmos()
    {
        if (currentWeaponType != GameManager.WeaponType.Unarmed)
        {
            Gizmos.DrawLine(currentWeapon.transform.position, clickPoint);
        }
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(Hurt());
        
        health -= damage;
        if (health <= 0)
        {
            currentState = State.Dead;
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Hurt()
    {
        Color originalColor = spriteRenderer.color;
        Color flashColor = new Color(255, 0, 0);
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Disappear()
    {
        float alpha = spriteRenderer.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1.0f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0, t));
            spriteRenderer.material.color = newColor;
            yield return null;
        }
    }
}

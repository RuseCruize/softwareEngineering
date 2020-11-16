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

    // Only used by AI
    public NavNode currentNode;
    public int nextNodeIndex;

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
            SelectWeapon(GameManager.WeaponType.Unarmed);
            nextNodeIndex = -100;
            distanceMoved = 0;
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


        if (isGrounded() && isUp)
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

    List<Guy> GetVisibleEnemies()
    {
        List<Guy> visibleGuys = new List<Guy>();
        MatchManager matchManager = FindObjectOfType<MatchManager>();
        Guy guy;
        RaycastHit2D[] hits;

        for (int i = 0; i < matchManager.players.Length; i++)
        {
            if (matchManager.players[matchManager.currentPlayer] != matchManager.players[i])
            {
                for (int j = 0; j < matchManager.players[i].guys.Count; j++)
                {
                    guy = matchManager.players[i].guys[j].GetComponent<Guy>();
                    hits = Physics2D.LinecastAll(transform.position, guy.transform.position);
                    // Debug.DrawLine(transform.position, guy.transform.position, Color.red, 0.5f);
                    bool hitWall = false;
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider.gameObject.tag != "Guy")
                        {
                            hitWall = true;
                            break;
                        }
                    }

                    if (!hitWall)
                    {
                        visibleGuys.Add(guy);
                    }
                }
            }
        }

        return visibleGuys;
    }

    List<Guy> GetEnemies()
    {
        List<Guy> enemies = new List<Guy>();
        MatchManager matchManager = FindObjectOfType<MatchManager>();

        for (int i = 0; i < matchManager.players.Length; i++)
        {
            if (matchManager.players[matchManager.currentPlayer] != matchManager.players[i])
            {
                foreach (GameObject guy in matchManager.players[matchManager.currentPlayer].guys)
                {
                    enemies.Add(guy.GetComponent<Guy>());
                }
            }
        }

        return enemies;
    }

    void FindNextNearestNode(Vector3 target)
    {
        for (int i = 0; i < currentNode.adjacentNodes.Count; i++)
        {
            NavNode node = currentNode.adjacentNodes[i];
            if (Vector3.Distance(node.transform.position, target) < Vector3.Distance(currentNode.adjacentNodes[nextNodeIndex].transform.position, target))
            {
                nextNodeIndex = i;
            }
        }
    }

    void ChooseNextNode()
    {
        List<Guy> visibleGuys;

        switch (GameManager.STATE.computerLevel)
        {
            case 1:
                visibleGuys = GetVisibleEnemies();
                if (visibleGuys.Count > 0)
                {
                    Debug.Log("EZ: See an enemy, staying put. They are at " + visibleGuys[0].gameObject.transform.position + ". See num: " + visibleGuys.Count);
                    nextNodeIndex = -1;
                }
                else
                {
                    Debug.Log("EZ: See no enemies, moving to first adjacent point: " + currentNode.adjacentNodes[0]);
                    nextNodeIndex = 0;
                }
                break;

            case 2:
                visibleGuys = GetVisibleEnemies();
                if (visibleGuys.Count > 0)
                {
                    Guy target = visibleGuys[0];
                    foreach (Guy guy in visibleGuys)
                    {   
                        if (guy.health < target.health)
                        {
                            target = guy;
                        }
                    }

                    if (target.health <= GameManager.STATE.BulletDamage)
                    {
                        nextNodeIndex = -1;
                    }
                    else if (target.health <= GameManager.STATE.MacheteDamage)
                    {
                        FindNextNearestNode(target.transform.position);
                    }
                }
                else
                {
                    Debug.Log("HARD: See no enemies, moving towards lowest health enemy.");
                    Guy target = null;
                    foreach (Guy enemy in GetEnemies()) {
                        if (target == null)
                        {
                            target = enemy;
                        }

                        if (enemy.health < target.health)
                        {
                            target = enemy;
                        }
                    }
                    FindNextNearestNode(target.transform.position);
                    Debug.Log("Moving to " + currentNode.adjacentNodes[nextNodeIndex].gameObject.name);
                }
                break;
        }
    }

    public void MoveAI()
    {
        if (nextNodeIndex == -100)
        {
            Debug.Log("Choosing next position...");
            ChooseNextNode();
        }

        if (nextNodeIndex == -1 || distanceMoved >= maxDistance || Mathf.Abs(transform.position.x - currentNode.adjacentNodes[nextNodeIndex].transform.position.x) < 0.01f)
        {
            
            if (nextNodeIndex >= 0)
            {
                // Debug.Log(transform.position.x - currentNode.adjacentNodes[nextNodeIndex].transform.position.x);
                Debug.Log("Reached destination " + currentNode.adjacentNodes[nextNodeIndex].gameObject.name);
                currentNode = currentNode.adjacentNodes[nextNodeIndex];
            }

            if (isGrounded())
            {
                nextNodeIndex = -100;
                currentState = State.Acting;
            }
        }
        else
        {
            NavNode targetNode = currentNode.adjacentNodes[nextNodeIndex];
            if (isGrounded() && currentNode.shouldJump[nextNodeIndex])
            {
                // Jump logic, check to see if touching ground
                body.velocity = Vector2.up * jumpVelocity;
            }
            if (targetNode.transform.position.x > transform.position.x)
            {
                transform.position += transform.right * speed * Time.deltaTime;
            }
            else
            {
                transform.position -= transform.right * speed * Time.deltaTime;
            }

            float endPosition = transform.position.x;
            distanceMoved = Mathf.Abs(startPosition - endPosition);
        }
        
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

        if (currentWeapon != null)
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

            lookAngle += 180;
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
            currentState = State.Waiting;
        }
    }

    public void ActAI()
    {
        List<Guy> visibleGuys = GetVisibleEnemies();
        Guy target = null;

        switch (GameManager.STATE.computerLevel)
        {
            case 1:
                if (visibleGuys.Count > 0)
                {
                    target = visibleGuys[0];
                }
                break;

            case 2:
                if (visibleGuys.Count > 0)
                {
                    target = visibleGuys[0];
                }
                break;
        }

        if (target != null)
        {
            SelectWeapon(GameManager.WeaponType.Pistol);
            Debug.Log("Aiming at " + transform.InverseTransformPoint(target.transform.position));
            Aim(transform.InverseTransformPoint(target.transform.position));
            Aim(transform.InverseTransformPoint(target.transform.position));
            Debug.DrawLine(currentWeapon.transform.position, target.transform.position, Color.red, 4f);
            Debug.DrawLine(currentWeapon.transform.position, target.transform.position - currentWeapon.transform.position, Color.red, 4f);
            Attack();
        }
        else
        {
            SelectWeapon(GameManager.WeaponType.Machete);
            Debug.Log("No one to attack, so aiming at self at " + transform.position);
            Aim(transform.position);
            Attack();
        }

        currentState = State.Waiting;
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
            health = 0;
            currentState = State.Dead;
            SelectWeapon(GameManager.WeaponType.Unarmed);
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

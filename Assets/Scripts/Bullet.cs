using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public int speedMultiplier;
    public bool flipped;
    public bool stopped;
    float flyTime;
    public float maxFlyTime;

    void Start()
    {
        flyTime = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        if (stopped)
        {
            return;
        }

        if (flipped)
        {
            transform.position += -transform.right * Time.deltaTime * speedMultiplier;
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * speedMultiplier;
        }
        
        if (Time.timeSinceLevelLoad - flyTime > maxFlyTime)
        {
            stopped = true;
            GameObject.FindObjectOfType<MatchManager>().advanceTurn = true;
        }
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.tag == GameManager.STATE.guyPrefab.tag)
        {
            Guy guy = hit.gameObject.GetComponent<Guy>();
            MatchManager matchManager = FindObjectOfType<MatchManager>();
            Player currentPlayer = matchManager.players[matchManager.currentPlayer];

            if (hit.gameObject == currentPlayer.guys[currentPlayer.currentGuy])
            {
                Debug.Log("Ignore self");
            }
            else
            {
                Debug.Log(matchManager.players[matchManager.currentPlayer].playerName);
                Debug.Log(guy.owner);
                stopped = true;
                guy.TakeDamage(GameManager.STATE.BulletDamage);
                StartCoroutine(BulletHit(true));
            }
        }
        else if (hit.GetType() == typeof(TilemapCollider2D))
        {
            // Hit something else
            stopped = true;
            // Debug.Log(hit.gameObject.name);
            StartCoroutine(BulletHit(false));
        }
    }

    IEnumerator BulletHit(bool hitGuy)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float alpha = spriteRenderer.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.1f)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0, t));
            spriteRenderer.material.color = newColor;
            yield return null;
        }

        Destroy(gameObject);
        GameObject.FindObjectOfType<MatchManager>().advanceTurn = true;
    }
}

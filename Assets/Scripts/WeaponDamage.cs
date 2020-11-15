using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public float delay;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        float alpha = spriteRenderer.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / delay)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0, t));
            spriteRenderer.material.color = newColor;
            yield return null;
        }

        Destroy(gameObject);
        GameObject.FindObjectOfType<MatchManager>().advanceTurn = true;
    }

    void OnTriggerEnter2D(Collider2D hit)
    {
        MatchManager matchManager = GameObject.FindObjectOfType<MatchManager>();
        Guy hitGuy = hit.gameObject.GetComponent<Guy>();
        string hitTag = hit.gameObject.tag;

        if (hitGuy != null && matchManager.players[matchManager.currentPlayer].playerName != hitGuy.owner)
        {
            if (hitTag == "Guy")
            {
                if (hitGuy.health > 0)
                {
                    hitGuy.TakeDamage(GameManager.STATE.MacheteDamage);
                    Debug.Log("Damaged Guy.");
                    Debug.Log(matchManager.players[matchManager.currentPlayer].playerName);
                    Debug.Log(hitGuy.owner);
                }
            }
            else
            {
                Debug.Log(hitTag);
            }
        }
        else
        {
            if (hitGuy == null)
            {
                Debug.Log("Failed to hit non-guy.");
            }
            else
            {
                Debug.Log("Failed to hit own team (self or other).");
            }
        }
    }
}

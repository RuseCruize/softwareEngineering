using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string playerName;
    public bool isComputer;
    public int computerLevel;
    public int numGuys;

    public List<GameObject> guys;

    public int currentGuy;

    public Player(string playerName, int numGuys, int computerLevel)
    {
        this.playerName = playerName;
        this.numGuys = numGuys;
        this.computerLevel = computerLevel;

        if (computerLevel > 0)
        {
            isComputer = true;
        }
        else
        {
            isComputer = false;
        }

        guys = new List<GameObject>();
        currentGuy = 0;
    }

    public void SpawnGuy(GameObject spawnPoint)
    {
        Vector3 position = spawnPoint.transform.position;
        NavNode node = spawnPoint.GetComponent<NavNode>();

        GameObject guy = GameObject.Instantiate(GameManager.STATE.guyPrefab, position, Quaternion.identity);
        Guy guyScript = guy.GetComponent<Guy>();
        guyScript.owner = playerName;
        guyScript.currentNode = node;

        if (guy.transform.position.x > 0)
        {
            guyScript.spriteRenderer.flipX = true;
        }
        guys.Add(guy);
    }

    public GameObject GetGuy()
    {
        return guys[currentGuy];
    }

    public void NextGuy()
    {
        currentGuy = (currentGuy + 1) % numGuys;
    }
}

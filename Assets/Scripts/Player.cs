﻿using System.Collections;
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
            this.isComputer = true;
        }
        else
        {
            this.isComputer = false;
        }

        guys = new List<GameObject>();
        currentGuy = 0;
    }

    public void SpawnGuy(Vector3 position)
    {
        Debug.Log(playerName + ": Creating Guy at " + position.ToString());
        GameObject guy = GameObject.Instantiate(GameManager.STATE.guyPrefab, position, Quaternion.identity);
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
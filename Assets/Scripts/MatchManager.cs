﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public int numGuys;
    private int numPlayers;
    public List<GameObject> spawnPoints;
    public Guy currentGuy;

    public int turn;
    public Player[] players;
    public int currentPlayer;
    bool started;

    void Start()
    {
        started = false;
        numPlayers = GameManager.STATE.numPlayers;
        int neededSpawns = numGuys * numPlayers;
        
        if (neededSpawns > spawnPoints.Count)
        {
            numGuys = 1; // reduce number of guys if we can't support it on this map
        }

        CreatePlayers();
        SpawnGuys();
        StartGame();
        started = true;
    }

    public void CreatePlayers()
    {
        players = new Player[numPlayers];
        players[0] = new Player("Player 1", numGuys, 0); // human

        for (int i = 0; i < numPlayers; i++)
        {
            players[i] = new Player("Player " + i, numGuys, GameManager.STATE.computerLevel); // potentially AI
        }
    }

    public void SpawnGuys()
    {
        int neededSpawns = numGuys * numPlayers;

        if (numPlayers == 2)
        {
            if (numGuys == 1)
            {
                players[0].SpawnGuy(spawnPoints[0].transform.position);
                players[1].SpawnGuy(spawnPoints[3].transform.position);
            }
            else
            {
                players[0].SpawnGuy(spawnPoints[0].transform.position);
                players[0].SpawnGuy(spawnPoints[1].transform.position);
                players[1].SpawnGuy(spawnPoints[2].transform.position);
                players[1].SpawnGuy(spawnPoints[3].transform.position);
            }
        }
        else
        {
            players[0].SpawnGuy(spawnPoints[0].transform.position);
            players[1].SpawnGuy(spawnPoints[1].transform.position);
            players[2].SpawnGuy(spawnPoints[2].transform.position);
            players[3].SpawnGuy(spawnPoints[3].transform.position);
        }
    }

    public bool GameContinues()
    {
        if (!started) return false;

        // Checks if two players have guys left
        bool hasGuy = false;
        for (int i = 0; i < numPlayers; i++)
        {
            if (players[i].guys.Count > 0)
            {
                if (hasGuy)
                {
                    return true;
                }
                else
                {
                    hasGuy = true;
                }
            }
        }

        return false;
    }

    void StartGame()
    {
        currentPlayer = 0;
        currentGuy = players[currentPlayer].GetGuy().GetComponent<Guy>();
        currentGuy.Activate();
    }

    void NextTurn()
    {
        currentPlayer = (currentPlayer + 1) % players.Length;
        players[currentPlayer].NextGuy();
        currentGuy = players[currentPlayer].GetGuy().GetComponent<Guy>();
        currentGuy.Activate();
    }

    void Update()
    {
        if (GameContinues())
        {
            switch(currentGuy.currentState)
            {
                case Guy.State.Moving:
                    currentGuy.Move();
                    break;

                case Guy.State.Acting:
                    currentGuy.Act();
                    break;

                case Guy.State.Waiting:
                    this.NextTurn();
                    break;

                case Guy.State.Dead:
                    Debug.Log("DEAD");
                    break;
            }
        }
    }
}

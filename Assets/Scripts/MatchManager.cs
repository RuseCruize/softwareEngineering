using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    public int numGuys;
    private int numPlayers;
    public List<GameObject> spawnPoints;
    public NavNode[] navNodes;
    public Guy currentGuy;

    public int turn;
    public Player[] players;
    public int currentPlayer;
    bool started;

    public bool advanceTurn;

    void Start()
    {
        advanceTurn = false;
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
        navNodes = FindObjectsOfType<NavNode>();
        started = true;
    }

    public void CreatePlayers()
    {
        players = new Player[numPlayers];

        for (int i = 0; i < numPlayers; i++)
        {
            if (i >= numPlayers / 2)
            {
                players[i] = new Player("Player " + (i+1), numGuys, GameManager.STATE.rightPlayerLevels);
            }
            else
            {
                players[i] = new Player("Player " + (i+1), numGuys, GameManager.STATE.leftPlayerLevels);
            }
        }
    }

    public void SpawnGuys()
    {
        if (numPlayers == 2)
        {
            if (numGuys == 1)
            {
                players[0].SpawnGuy(spawnPoints[0]);
                players[1].SpawnGuy(spawnPoints[3]);
            }
            else
            {
                players[0].SpawnGuy(spawnPoints[0]);
                players[0].SpawnGuy(spawnPoints[1]);
                players[1].SpawnGuy(spawnPoints[2]);
                players[1].SpawnGuy(spawnPoints[3]);
            }
        }
        else
        {
            players[0].SpawnGuy(spawnPoints[0]);
            players[1].SpawnGuy(spawnPoints[1]);
            players[2].SpawnGuy(spawnPoints[2]);
            players[3].SpawnGuy(spawnPoints[3]);
        }
    }

    public bool GameContinues()
    {
        if (!started) return false;

        // Checks if two players have guys left
        bool hasGuy = false;
        for (int i = 0; i < numPlayers; i++)
        {
            for (int j = 0; j < players[i].guys.Count; j++)
            {
                if (players[i].guys[j].GetComponent<Guy>().currentState != Guy.State.Dead)
                {
                    if (hasGuy)
                    {
                        return true;
                    }
                    else
                    {
                        hasGuy = true;
                        break;
                    }
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
        turn++;
    }

    void Update()
    {
        if (GameContinues())
        {
            switch(currentGuy.currentState)
            {
                case Guy.State.Moving:
                    if (players[currentPlayer].isComputer)
                    {
                        currentGuy.MoveAI(players[currentPlayer].computerLevel);
                    }
                    else
                    {
                        currentGuy.Move();
                    }
                    break;

                case Guy.State.Acting:
                    if (players[currentPlayer].isComputer)
                    {
                        currentGuy.ActAI(players[currentPlayer].computerLevel);
                    }
                    else
                    {
                        currentGuy.Act();
                    }
                        
                    break;

                case Guy.State.Waiting:
                    if (advanceTurn)
                    {
                        advanceTurn = false;
                        NextTurn();
                    }
                    break;

                case Guy.State.Dead:
                    // Debug.Log("DEAD");
                    NextTurn();
                    break;
            }
        }
        else {
            SceneManager.LoadScene(0);
        }
    }
}

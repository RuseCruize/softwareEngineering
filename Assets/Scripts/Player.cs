using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public bool isComputer;
    public int numGuys;

    public Player(string playerName, bool isComputer, int numGuys)
    {
        this.playerName = playerName;
        this.isComputer = isComputer;
        this.numGuys = numGuys;
    }
}

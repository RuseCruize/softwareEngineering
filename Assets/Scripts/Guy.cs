using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : MonoBehaviour
{
    GameObject guyObject;
    public int health;
    public string owner;

    public Guy(Vector3 position, string owner)
    {
        this.owner = owner;
        this.health = 100;
    }
}

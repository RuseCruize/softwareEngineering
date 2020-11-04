using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float jumpVelocity;
    public Rigidbody2D body;
    public LayerMask groundLayer;
    public bool IsActive = false;

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            
        }
    }

    

    public void SetActive()
    {
        IsActive = true;
    }

    public void SetInactive()
    {
        IsActive = false;
    }
}

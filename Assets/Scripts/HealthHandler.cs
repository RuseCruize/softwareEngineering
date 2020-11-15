using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    //[SerializeField] private HealthBar healthBar;
    //[SerializeField] public MatchManager mm;
    public HealthBar healthBar;
    public Guy guy;

    // Start is called before the first frame update
    void Start()
    {
        //healthBar = (HealthBar)Instantiate(HealthBar);
        healthBar.SetSize(1f);

    }

    // Update is called once per frame
    void Update()
    {
       healthBar.transform.position = guy.transform.position + new Vector3(0.0f, 1.2f, 0.0f);
       float currentHealth = (float)(guy.health / guy.maxHealth);
       healthBar.SetSize(currentHealth);
    }
}

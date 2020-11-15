using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaHandler : MonoBehaviour
{
    [SerializeField] StaminaBar staminaBar;
    [SerializeField] MatchManager mm;

    // Start is called before the first frame update
    void Start()
    {
        staminaBar.SetSize(mm.currentGuy.health / mm.currentGuy.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.SetSize((5-mm.currentGuy.distanceMoved) / 5);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy_WeaponManager : MonoBehaviour
{
    public Guy_Weapon primaryWeapon;
    public Guy_Weapon secondaryWeapon;
    
    public bool Aiming = false;
    public bool canFire = false;

    [HideInInspector]
    public Guy_Weapon selectedWeapon;
    // Start is called before the first frame update
    void Start()
    {
        primaryWeapon.manager = this;
        secondaryWeapon.manager = this;
        primaryWeapon.ActivateWeapon(true);
        secondaryWeapon.ActivateWeapon(false);
        selectedWeapon = primaryWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            if (selectedWeapon != primaryWeapon) {
                primaryWeapon.ActivateWeapon(true);
                secondaryWeapon.ActivateWeapon(false);
                selectedWeapon = primaryWeapon;
            }
        }
        if (Input.GetKeyDown("2")) {
            if (selectedWeapon != secondaryWeapon) {
                primaryWeapon.ActivateWeapon(false);
                secondaryWeapon.ActivateWeapon(true);
                selectedWeapon = secondaryWeapon;                
            }
        }
        
    }
}

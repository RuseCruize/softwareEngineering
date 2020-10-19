using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Guy_Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform weaponTip;
    public bool Aiming = false;
    public bool canFire = false;
    public bool singleFire = true;
    public float fireRate = 1.0f;
    public GameObject Guy_Bullet;
    public Vector2 lookDirection;
    public float lookAngle;
    public float weaponVelocity = 10f;
    public int bulletsPerMagazine = 1;
    public float weaponDamage = 30;
    public AudioClip fireAudio;

    [HideInInspector]
    public Guy_WeaponManager manager;

    float nextFireTime = 0;
    int bulletsPerMagazineDefault = 1;
    AudioSource audioSource;
    SpriteRenderer sprite;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        manager.canFire = true;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.Aiming) { 
            lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f);
            if (Input.GetMouseButtonDown(0)) {
                Fire();
                manager.Aiming = false;
                return;
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            manager.Aiming = true;
        }
        
    }

    void Fire() {
        GameObject guyBullet = Instantiate(Guy_Bullet, weaponTip.position, weaponTip.rotation);
        guyBullet.GetComponent<Rigidbody2D>().velocity = weaponTip.up * weaponVelocity;
    }

    public void ActivateWeapon(bool activate) {
        if (sprite == null) {
            sprite = GetComponent<SpriteRenderer>();
        }
        if (activate == true) {
            sprite.color = new Color(1,1,1,1);
        } else {
            sprite.color = new Color(1,1,1,0);
        }
        gameObject.SetActive(activate);
    }
}

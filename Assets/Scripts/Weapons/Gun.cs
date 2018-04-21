using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && weaponCooldown.canUse)
        {
            Shoot();
        }
    }

    private void Start()
    {
        weaponCooldown.InitCooldown();
    }

    public override void Shoot()
    {
        Vector3 startDirection = transform.Find("BarrelEnd").position - transform.Find("BarrelStart").position;
        GameObject bullet = projectileObjectPool.PoolNext(transform.Find("BarrelEnd").position);

        bullet.GetComponent<GunBullet>().startProjectileMovement(startDirection);
        weaponCooldown.startTimer();
    }
    
}

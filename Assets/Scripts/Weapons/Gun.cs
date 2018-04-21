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
        GameObject bullet = projectileObjectPool.PoolNext(transform.Find("BarrelEnd").position + new Vector3(0, 0, 0.1f));
        bullet.GetComponent<GunBullet>().startProjectileMovement(transform.Find("BarrelEnd").position - transform.Find("BarrelStart").position);
        weaponCooldown.startTimer();
    }
    
}

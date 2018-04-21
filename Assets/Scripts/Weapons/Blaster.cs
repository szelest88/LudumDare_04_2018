using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : Gun
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && weaponCooldown.canUse)
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
        Vector3 startDirection = transform.Find("BarrelEnd").localPosition - transform.Find("BarrelStart").localPosition;
        GameObject bullet = projectileObjectPool.PoolNext(transform.Find("BarrelEnd").position);

        bullet.GetComponent<BlasterProjectile>().startProjectileMovement(startDirection);
        weaponCooldown.startTimer();
    }
}

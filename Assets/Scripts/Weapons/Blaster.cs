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
        Vector3 startDirection = transform.Find("BarrelEnd").position - transform.Find("BarrelStart").position;
        GameObject bullet = projectileObjectPool.PoolNext(transform.Find("BarrelEnd").position - startDirection.normalized);

        bullet.GetComponent<BlasterProjectile>().startProjectileMovement(transform.Find("BarrelEnd").position - transform.Find("BarrelStart").position);
        weaponCooldown.startTimer();
    }
}

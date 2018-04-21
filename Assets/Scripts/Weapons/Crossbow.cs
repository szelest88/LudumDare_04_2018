using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : Weapon
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && weaponCooldown.canUse)
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
        
        bullet.transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z);

        bullet.GetComponent<CrossbowBolt>().startProjectileMovement(startDirection);
        weaponCooldown.startTimer();
    }
}

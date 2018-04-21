using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : Projectile
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            dealDamage(projectileDamage, collision.gameObject);
            gameObject.SetActive(false);
        }
    }
}

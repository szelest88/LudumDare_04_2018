using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Projectile
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PlayerBody"))
        {
            dealDamage(projectileDamage, collision.gameObject);
            gameObject.SetActive(false);
        }
    }
}

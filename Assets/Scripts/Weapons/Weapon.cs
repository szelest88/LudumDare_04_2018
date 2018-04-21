using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public ObjectPool projectileObjectPool;
    public Cooldown weaponCooldown;
    
    public abstract void Shoot();

    
    

    //public void ShootZigZag()
    //{
    //    GameObject bullet = zigZagProjectilePool.PoolNext(transform.Find("BarrelEnd").position + new Vector3(0, 0, 0.2f));
    //    bullet.GetComponent<ZigZagProjectileController>().startProjectileMovement(transform.Find("BarrelEnd").position - transform.Find("BarrelStart").position);
    //}
}

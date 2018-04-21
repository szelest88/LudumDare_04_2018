using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public ObjectPool basicProjectilesPool;
    public ObjectPool zigZagProjectilePool;

	void Start ()
    {
		
	}
	
	
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.A))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ShootZigZag();
        }
    }

    public void Shoot()
    {
        GameObject bullet = basicProjectilesPool.PoolNext(transform.Find("BarrelEnd").position + new Vector3(0,0,0.2f));
        bullet.GetComponent<BasicProjectileController>().startProjectileMovement(transform.Find("BarrelEnd").position - transform.Find("BarrelStart").position);
    }

    public void ShootZigZag()
    {
        GameObject bullet = zigZagProjectilePool.PoolNext(transform.Find("BarrelEnd").position + new Vector3(0, 0, 0.2f));
        bullet.GetComponent<ZigZagProjectileController>().startProjectileMovement(transform.Find("BarrelEnd").position - transform.Find("BarrelStart").position);
    }
}

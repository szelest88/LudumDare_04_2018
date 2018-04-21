using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public ObjectPool basicProjectilesPool;

	void Start ()
    {
		
	}
	
	
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.A))
        {
            Shoot();
        }
	}

    void Shoot()
    {
        GameObject bullet = basicProjectilesPool.PoolNext(transform.Find("BarrelEnd").position + new Vector3(0,0,0.2f));
        bullet.GetComponent<BasicProjectileController>().startProjectileMovement(transform.Find("BarrelEnd").position - transform.Find("Barrel").position);
    }
}

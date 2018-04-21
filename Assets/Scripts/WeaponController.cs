using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
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
        GameObject bullet = Instantiate(bulletPrefab, transform.Find("BarrelEnd").position + new Vector3(0,0,0.2f), Quaternion.identity);
        bullet.GetComponent<BasicProjectileController>().startProjectileMovement(transform.Find("BarrelEnd").position - transform.Find("Barrel").position);
    }
}

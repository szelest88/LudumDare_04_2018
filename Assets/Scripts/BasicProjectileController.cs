using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectileController : Projectile
{

    // Use this for initialization
    void Start ()
    {
		
	}

    private void Awake()
    {
        
    }

    public override void startProjectileMovement(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction * projectileSpeed);
    }

    public override void dealDamage(float damage, GameObject affectedObject)
    {
        affectedObject.GetComponentInParent<Unit>().Health -= projectileDamage;
    }


    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            dealDamage(projectileDamage, collision.gameObject);
            gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}

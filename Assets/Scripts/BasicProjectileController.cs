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
        affectedObject.GetComponent<Unit>().hp -= projectileDamage;
    }


    private void OnCollisionEnter(Collision collision)
    {
        // if collision.go to jest przeciwnik
        dealDamage(projectileDamage, collision.gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}

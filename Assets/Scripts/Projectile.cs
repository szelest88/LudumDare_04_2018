using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent script for all projectiles.
/// It contains common methods, that every projectile should have.
/// </summary>
public abstract class Projectile : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
    public float projectileSpeed;
    public float projectileDamage;

    public abstract void startProjectileMovement(Vector3 direction);
    public abstract void dealDamage(float damage, GameObject affectedObject);



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagProjectileController : Projectile
{
    Vector3 startingDirection;
    bool swap = false;
    float timer;
    float timerStartValue = 0.3f;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if(timer < 0)
        {
            timer = timerStartValue;
            Vector3 direction = Quaternion.AngleAxis(90, Vector3.up) * startingDirection;

            
            if (swap) GetComponent<Rigidbody>().AddForce(direction * 200);
            else GetComponent<Rigidbody>().AddForce(direction * -200);

            swap = !swap;
        }
        timer -= Time.deltaTime;



    }
    public override void startProjectileMovement(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction * projectileSpeed);
        startingDirection = direction;

        timer = timerStartValue;

        Vector3 dir = Quaternion.AngleAxis(90, Vector3.up) * startingDirection;

        GetComponent<Rigidbody>().AddForce(dir * 100);
    }

    public override void dealDamage(float damage, GameObject affectedObject)
    {
        affectedObject.GetComponent<Unit>().Health -= projectileDamage;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            dealDamage(projectileDamage, collision.gameObject);
        }

    }
}

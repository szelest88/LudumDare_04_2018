using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float bulletSpeed;

    // Use this for initialization
    void Start ()
    {
		
	}

    private void Awake()
    {
        
    }

    public void startMovement(Vector3 direction)
    {
        GetComponent<Rigidbody>().AddForce(direction * bulletSpeed);
    }
    // Update is called once per frame
    void Update ()
    {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover     : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	void moveBy(Vector3 translation)
    {
        transform.position = transform.position + translation;
    }
    void moveTo(Vector3 targetPos)
    {
        transform.position = targetPos;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x + 0.01f, transform.position.y, transform.position.z);
    }
}

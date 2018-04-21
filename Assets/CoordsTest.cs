using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordsTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
        transform.position = new Vector3(transform.position.x + 0.001f, transform.position.y, transform.position.z);
	}
}

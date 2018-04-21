using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGenerator : MonoBehaviour {

    GameObject baseTile;

    void GenerateTileField()
    {
        GameObject go = transform.GetChild(0).gameObject;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject anotherOne = Instantiate(go);
                anotherOne.transform.parent = this.transform;
                anotherOne.transform.Translate(i*2, transform.position.y, j*2);
            }
        }
        //transform.GetChild(0);
    }
	// Use this for initialization
	void Start () {
        GenerateTileField();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

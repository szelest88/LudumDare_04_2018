using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarManager : MonoBehaviour {

    public Unit unit;

    public List<GameObject> elements;

	// Use this for initialization
	void Start () {
		 
	}
	
	// Update is called once per frame
	void Update () {

        for(int i =0; i < elements.Count; i++)
        {
            if (i < unit.Health)
                elements[i].SetActive(true);
            else
                elements[i].SetActive(false);
        }
	} 
}

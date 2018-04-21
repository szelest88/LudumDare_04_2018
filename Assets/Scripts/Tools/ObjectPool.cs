using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject pooledObject;
    public int numberOfObjects;
    public List<GameObject> pooledObjects;
    private int counter = 0;
	// Use this for initialization
	void Start ()
    {
        pooledObjects = new List<GameObject>();

        for (int i=0; i<numberOfObjects; i++)
        {
            GameObject go = Instantiate(pooledObject);
            pooledObjects.Add(go);
            go.SetActive(false);
        }
	}

    public GameObject PoolNext(Vector3 position)
    {
        int activeCounter = 0;
        GameObject go = new GameObject();
        do
        {
            go = pooledObjects[counter];
            counter++;
            if (counter == numberOfObjects) counter = 0;

            activeCounter++;
            if (activeCounter == numberOfObjects) throw new UnityException("No remaining pooled objects");
        }
        while (go.activeInHierarchy);
        
        go.transform.position = position;
        go.SetActive(true);

        return go;
    }
	
    
	// Update is called once per frame
	void Update ()
    {
		
	}
}

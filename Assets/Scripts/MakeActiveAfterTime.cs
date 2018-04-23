using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeActiveAfterTime : MonoBehaviour {

	public List<GameObject> objectsToModify;

	public float timeToModify = 1;

	public bool valueToApply = true;

	public bool isActive = true;

	void Start()
	{
		if (objectsToModify.Count <= 0 || timeToModify <= 0)
			isActive = false;
	}

	// Update is called once per frame
	void Update () {

		if (isActive)
		{
			timeToModify -= Time.deltaTime;

			if (timeToModify <= 0)
			{
				isActive = false;

				for (int i = 0; i < objectsToModify.Count; i++)
					if (objectsToModify [i] != null)
						objectsToModify [i].SetActive (valueToApply);
			}
		}
	}
}

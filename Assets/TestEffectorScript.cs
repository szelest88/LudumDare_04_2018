using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffectorScript : MonoBehaviour {
    public ControllerWrapper left, right;
    // Use this for initialization
    void Start() {
        time = 0;
        doLeft = true;
        if (left == null || right == null)
        {
            Debug.LogError("There is a testeffector script with no controllerwrapper object assigned!");
        }
	}
    float time;
    bool doLeft;
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time > 2)
        {
            doLeft = !doLeft;
            if (doLeft)
                left.VrapperTriggerHaptics(0.5f, 1);
            else
                right.VrapperTriggerHaptics(0.5f, 1);
            time = 0;
                
        }
	}
}

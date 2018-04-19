using UnityEngine;
using System.Collections;

public class ControllerWrapper : SteamVR_TrackedController {

    public bool is_left;
    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)controllerIndex); } }
    public Vector3 velocity { get { return controller.velocity; } }
    public Vector3 angularVelocity { get { return controller.angularVelocity; } }

    IEnumerator LongVibration(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            controller.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
        yield return null;
    }
}
    /// <summary>
    /// Triggers the haptic feedback for the selected controller
    /// </summary>
    /// <param name="seconds">duration in seconds</param>
    /// <param name="force">force from 0 to 1</param>
    /// 
    public void VrapperTriggerHaptics(float seconds, float force)
    {
        if(gameObject.activeSelf) // marudził bez tego
            StartCoroutine(LongVibration(seconds, force));
    }
    // Use this for initialization
    protected override void Start () {
        base.Start();
        // TODO: implement
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        
        //Debug.Log("trigger state: "+controller.GetState().rAxis1.x); // gets the trigger state (analog, 0-1)
        //if (is_left)
        //{
        //    Debug.Log("position:" + transform.position.ToString("F4")); // if we want controller position (in global (?) coordinates)
        //}
        //if (padTouched) // while touched
        //{
                // by default, vector is printed with 1 floating point decimal, which is terribly incaurate. That's why "F2" is given in parameter.
                //Debug.Log("pad touched:" + controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).ToString("F2"));
        //}
	}

    public override void OnTriggerClicked(ClickedEventArgs e)
    {
        base.OnTriggerClicked(e);
        Debug.Log("trigger clicked!"); // to be specific, when trigger in very high position - not the "click" itself
        VrapperTriggerHaptics(0.1f,0.5f);
    }

    public override void OnTriggerUnclicked(ClickedEventArgs e)
    {
        base.OnTriggerUnclicked(e);
    }

    public override void OnMenuClicked(ClickedEventArgs e)
    {
        base.OnMenuClicked(e);
    }

    public override void OnMenuUnclicked(ClickedEventArgs e)
    {
        base.OnMenuUnclicked(e);
    }

    public override void OnSteamClicked(ClickedEventArgs e)
    {
        base.OnSteamClicked(e);
    }

    public override void OnPadClicked(ClickedEventArgs e)
    {
        base.OnPadClicked(e);
    }

    public override void OnPadUnclicked(ClickedEventArgs e)
    {
        base.OnPadUnclicked(e);
    }

    public override void OnPadTouched(ClickedEventArgs e) // when did touched (i.e. wasn't touched before and now is, it's not continuous)
    {
        base.OnPadTouched(e);
    }

    public override void OnPadUntouched(ClickedEventArgs e)
    {
        base.OnPadUntouched(e);
    }

    public override void OnGripped(ClickedEventArgs e)
    {
        base.OnGripped(e);
    }

    public override void OnUngripped(ClickedEventArgs e)
    {
        base.OnUngripped(e);
    }
}

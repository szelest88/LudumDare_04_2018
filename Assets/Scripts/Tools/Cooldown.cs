using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour
{
    private float timerValue;

    private bool isTimerRunning;
    public bool canUse

    {
        get;
        private set;
    }

    public float cooldownTime;

    public event EventHandler cdElapsed;

    public void InitCooldown()
    {
        this.cooldownTime = cooldownTime;
        isTimerRunning = false;
        timerValue = cooldownTime;
        canUse = true;

        cdElapsed += Cooldown_cdElapsed;
    }

    private void Cooldown_cdElapsed(object sender, EventArgs e)
    {
        canUse = true;
    }
    public void SetTime(float time)
    {
        timerValue = time;
    }
    public float GetTime()
    {
        return timerValue;
    }

    public void startTimer()
    {
        isTimerRunning = true;
        canUse = false;

    }
    public void stopTimer()
    {
        isTimerRunning = false;
    }

    public void resetTimer()
    {
        timerValue = cooldownTime;

    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timerValue -= Time.deltaTime;
            if (timerValue < 0)
            {
                if (cdElapsed != null) cdElapsed.Invoke(this, EventArgs.Empty);
                stopTimer();
                resetTimer();
            }
        }
    }
}

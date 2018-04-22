using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public enum PlayerState
    {
        IDLE,
        TELEPORTING,
        SHOOTING,
    };

    public enum PlayerEvent
    {
        ENTER,
        EXIT,
        MOVE_START,
        TRIGGER_START,
        TRIGGER_END,
        TIMEOUT,
    }


    public PlayerState playerState;


    public ParabolicPointer parabolicPointer;
    public GameObject gun;
    public Coroutine timeoutCoroutine;

    // Use this for initialization
    void Start()
    {
        init(PlayerState.IDLE);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public override void MoveStart()
    {
        process(PlayerEvent.MOVE_START);
    }

    public void OnTriggerStart()
    {
        process(PlayerEvent.TRIGGER_START);
    }
    public void OnTriggerEnd()
    {
        process(PlayerEvent.TRIGGER_END);
    }

    void init(PlayerState state)
    {
        playerState = state;
        processEvent(state, PlayerEvent.ENTER);
    }


    void process(PlayerEvent playerEvent)
    {
        var newState = processEvent(playerState, playerEvent);

        while (newState != null)
        {
            processEvent(playerState, PlayerEvent.EXIT);

            playerState = newState.Value;

            newState = processEvent(playerState, PlayerEvent.ENTER);
        }
    }

    PlayerState? processEvent(PlayerState playerState, PlayerEvent playerEvent)
    {
        switch (playerState)
        {
            case PlayerState.IDLE:
                switch (playerEvent)
                {
                    case PlayerEvent.MOVE_START:
                        return PlayerState.TELEPORTING;
                }
                break;

            case PlayerState.TELEPORTING:
                switch (playerEvent)
                {
                    case PlayerEvent.ENTER:
                        parabolicPointer.enabled = true;
                        break;

                    case PlayerEvent.EXIT:
                        parabolicPointer.enabled = false;
                        break;

                    case PlayerEvent.TRIGGER_END:
                        var pointerPos = parabolicPointer.GetPosition();
                        if (pointerPos != null)
                        {
                            var response = GameControllerScript.Instance.IWannaMoveWorldPos(pointerPos.Value, this);
                            if (response.canMove)
                            {
                                transform.position = response.targetWorldPos;
                                GameControllerScript.Instance.IJustMovedGridPos(transform.position, this);
                                return PlayerState.SHOOTING;
                            }
                        }
                        break;
                }
                break;

            case PlayerState.SHOOTING:
                switch (playerEvent)
                {
                    case PlayerEvent.ENTER:
                        gun.SetActive(true);
                        break;

                    case PlayerEvent.EXIT:
                        gun.SetActive(false);
                        break;

                    case PlayerEvent.TRIGGER_START:
                        if (timeoutCoroutine == null && transform.GetComponentInChildren<Blaster>().weaponCooldown.canUse)
                        {
                            transform.GetComponentInChildren<Blaster>().Shoot();
                            timeoutCoroutine = StartCoroutine(WaitForTurnEnd());
                        }
                        break;

                    case PlayerEvent.TIMEOUT:
                        GameControllerScript.Instance.MoveEnd(this);
                        return PlayerState.IDLE;
                }
                break;

        }
        return null;
    }

    public IEnumerator WaitForTurnEnd()
    {
        yield return new WaitForSeconds(1f);
        timeoutCoroutine = null;
        process(PlayerEvent.TIMEOUT);
    }
}

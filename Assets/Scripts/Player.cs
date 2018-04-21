using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    enum PlayerState
    {
        TELEPORTING,
        SHOOTING,
        IDLE
    };

    enum PlayerEvent
    {
        ENTER,
        EXIT,
        MOVE_START,
        TELEPORT
    }
    PlayerState playerState;


    public ParabolicPointer parabolicPointer;
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

        parabolicPointer.enabled = true;
    }

    public void OnTrigger()
    {
        process(PlayerEvent.TELEPORT);
    }

    void init(PlayerState state)
    {
        playerState = state;
        processEvent(state, PlayerEvent.ENTER);
    }


    void process(PlayerEvent playerEvent)
    {
        var newState = processEvent(playerState, playerEvent);

        if (newState != null)
        {
            processEvent(playerState, PlayerEvent.EXIT);

            playerState = newState.Value;

            processEvent(playerState, PlayerEvent.ENTER);
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

                    case PlayerEvent.TELEPORT:
                        var pointerPos = parabolicPointer.GetPosition();
                        if(pointerPos!=null)
                        {

                        }
                        break;
                }
                break;

            case PlayerState.SHOOTING:
                //TODO: implement
                break;

        }
        return null;
    }
}

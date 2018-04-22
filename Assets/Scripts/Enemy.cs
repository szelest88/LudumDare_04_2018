using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

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
        TIMEOUT,
        MOVE_ANIM_FINISHED
    }

    public ObjectPool projectileObjectPool;

    public PlayerState playerState;
    


    // Use this for initialization
    void Start()
    {
        init(PlayerState.IDLE);
    }

    public override void MoveStart()
    {
        if (Health > 0)
        {
            process(PlayerEvent.MOVE_START);
        }
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
                        var possibleMoves = GameControllerScript.Instance.WhereCanIMoveWorldPos(this);
                        var idx = UnityEngine.Random.Range(0, possibleMoves.Count);
                        var newPosition = possibleMoves[idx];

                        var response = GameControllerScript.Instance.IWannaMoveWorldPos(newPosition, this);
                        StartCoroutine(Move(newPosition, response.movePathWorldPos));
                        break;

                    case PlayerEvent.EXIT:
                        break;

                    case PlayerEvent.MOVE_ANIM_FINISHED:
                        return PlayerState.SHOOTING;
                }
                break;

            case PlayerState.SHOOTING:
                switch (playerEvent)
                {
                    case PlayerEvent.ENTER:
                        Shoot();
                        break;

                    case PlayerEvent.EXIT:
                        break;

                    case PlayerEvent.TIMEOUT:
                        GameControllerScript.Instance.MoveEnd(this);
                        return PlayerState.IDLE;
                }
                break;

        }
        return null;
    }

    private IEnumerator Move(Vector3 newPosition, List<Vector3> positions)
    {
        var currentPos = transform.position;

        foreach (var pos in positions)
        {
            yield return MoveFromTo(transform, currentPos, pos, moveSpeed);
            currentPos = pos;
        }

        GameControllerScript.Instance.IJustMovedWorldPos(newPosition, this);
        process(PlayerEvent.MOVE_ANIM_FINISHED);
    }

    IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed)
    {
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        objectToMove.position = b;
    }

    private void Shoot()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        GameObject projectileObject = projectileObjectPool.PoolNext(transform.position);
        projectileObject.GetComponent<EnemyBullet>().startProjectileMovement(direction);

        foreach (var playerCollider in GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(playerCollider, projectileObject.GetComponent<Collider>());
        }
        Debug.Log("chuj");

        StartCoroutine(WaitForTurnEnd());
    }

    public IEnumerator WaitForTurnEnd()
    {
        yield return new WaitForSeconds(1f);
        process(PlayerEvent.TIMEOUT);
    }
}

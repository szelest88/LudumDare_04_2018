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
        process(PlayerEvent.MOVE_START);
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
                        transform.position = possibleMoves[idx];
                        var response = GameControllerScript.Instance.IWannaMoveWorldPos(transform.position, this);
                        //response.movePathWorldPos
                        GameControllerScript.Instance.IJustMovedWorldPos(transform.position, this);
                        return PlayerState.SHOOTING;

                    case PlayerEvent.EXIT:
                        break;
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

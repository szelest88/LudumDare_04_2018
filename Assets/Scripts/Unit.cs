using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent script for all units in game.
/// This includes all opponents as well as the player.
/// </summary>
public abstract class Unit : MonoBehaviour
{
    [SerializeField]
    private float health;
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health <= 0) gameObject.SetActive(false);
        }
    }

	/// <summary>
	/// How many spaces can this unit move each turn.
	/// </summary>
	public int moveSpeed = 3;

	/// <summary>
	/// How many spaces can this unit move this turn.
	/// </summary>
	public int movesRemaining;


	public Vector3 gridPos;


    // Use this for initialization
    void Start ()
    {
		
	}


	/// <summary>
	/// Called by game manager, when it's time for this unit to move.
	/// </summary>
	public virtual void MoveStart()
	{
	}

	/// <summary>
	/// General method to inflict damage to this unit.
	/// </summary>
	public virtual void GetHit(int damage)
	{
		
	}

}

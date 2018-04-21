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

    // Use this for initialization
    void Start ()
    {
		
	}

}

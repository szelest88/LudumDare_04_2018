using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameControllerScript : MonoBehaviour {

	public enum TileType
	{
		EMPTY,
		UNIT,
		WALL,
		OBSTACLE
	}

	[Serializable]
	public struct TileData
	{
		public TileType type;
		public GameObject objRef;
		public UnitScript unitRef;
	}

	public TileData[,] tilesMap;

	/// <summary>
	/// Size of every tile in the scene.
	/// </summary>
	public Vector3 tileSize = Vector3.one;

	/// <summary>
	/// Offset of first Tile with coords 0.0, relative to this object.
	/// </summary>
	public Vector3 firstTileOffset = Vector3.zero;

	public static GameControllerScript Instance;

	void Awake()
	{
		// This is a specific singleton, that lives only as long as the scene does:
		GameControllerScript.Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}

}

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
		public Unit unitRef;
	}

	/// <summary>
	/// Defines, how far the manager will look for when initing the game field array.
	/// The manager looks only forward in each axis.
	/// </summary>
	public Vector3 mapSize = new Vector3(10, 10, 3);

	public TileData[,,] tilesMap;

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

		InitTilesArray ();
	}


	public Vector3 MultVec3(Vector3 vec1, Vector3 vec2)
	{
		return new Vector3 (vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z);
	}

	public Vector3 MultVec3(Vector3 source, float multX, float multY, float multZ)
	{
		return new Vector3 (source.x * multX, source.y * multY, source.z * multZ);
	}

	public Vector3 MultVec3(Vector3 source, float multValue)
	{
		return MultVec3 (source, multValue, multValue, multValue);
	}

	public void OnTeleSelectionStart()
	{
		
	}

	public void OnTeleSelectionUpdate(Vector3 worldPos)
	{
		
	}

	public void OnTeleSelectionStop()
	{
		
	}

	public void OnTeleportApply()
	{
		
	}

	public TileData RayCastToTileData(RaycastHit hit)
	{
		TileData returnVal = new TileData ();

		if (hit.collider == null)
		{
			returnVal.type = TileType.EMPTY;
			return returnVal;
		}

		returnVal.objRef = hit.collider.gameObject;
		returnVal.type = TileType.WALL;

		Unit unitScript = returnVal.objRef.GetComponent<Unit>();

		if (unitScript != null)
		{
			returnVal.unitRef = unitScript;
			returnVal.type = TileType.UNIT;
		}

		return returnVal;
	}

	public void InitTilesArray()
	{
		tilesMap = new TileData[(int)mapSize.x, (int)mapSize.y, (int)mapSize.z];

		RaycastHit hit;

		for (int iterX = 0; iterX < mapSize.x; iterX++)
		{
			for (int iterY = 0; iterY < mapSize.y; iterY++)
			{
				for (int iterZ = 0; iterZ < mapSize.z; iterZ++)
				{
					tilesMap [iterX, iterY, iterZ] = new TileData ();
						
					Vector3 checkPos = MultVec3 (tileSize, iterX, iterY, iterZ) + transform.position;

					Vector3 colliderSize = MultVec3 (tileSize, 0.4f);

					if (Physics.BoxCast (checkPos + Vector3.up, colliderSize, Vector3.down, out hit, transform.rotation, 1))
					{
						tilesMap [iterX, iterY, iterZ] = RayCastToTileData (hit);
					}
					else
					{
						tilesMap [iterX, iterY, iterZ].type = TileType.EMPTY;
					}
				}
			}
		}

		//Physics.BoxCast (transform.position, tileSize * 0.8f, Vector3.one, out hit, Quaternion.identity, 0);
	}

}

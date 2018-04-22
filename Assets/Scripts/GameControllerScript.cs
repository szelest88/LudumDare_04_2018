using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameControllerScript : MonoBehaviour {

	[Serializable]
	public enum GameFlowState
	{
		DELAY_FOR_NEXT_MOVE,
		WAITING_FOR_UNIT
	}

	[Serializable]
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

	public struct CanMoveResponse
	{
		public bool canMove;

		public Vector3 targetWorldPos;
		public Vector3 targetLocalPos;

		public Vector3 targetGridPos;

		/// <summary>
		/// How many grid spaces the unit will move.
		/// </summary>
		public int spacesMoved;

		/// <summary>
		///  List of points in world orient, that the object should follow to get to the end pos.
		/// </summary>
		public List<Vector3> movePathWorldPos;

		public List<Vector3> movePathGridPos;
	}

	public GameObject lookTileSelectionObj;
	public GameObject playerTileWallObj;
	public GameObject currPlayerHighlightObj;
	public GameObject playerWallObj;

	public GameObject possibleMovePrefab;

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

	public float delayBetweenMoves = 1;

	public float moveDelayTimer = 1;


	public int currentUnitIndex = 0;
	public List<Unit> unitsList;

	public List<GameObject> possibleMovesObjsList;

	public GameFlowState currentGameState = GameFlowState.DELAY_FOR_NEXT_MOVE;

	public static GameControllerScript Instance;

	void Awake()
	{
		// This is a specific singleton, that lives only as long as the scene does:
		GameControllerScript.Instance = this;
	}

	// Use this for initialization
	void Start () {

		possibleMovesObjsList = new List<GameObject> ();

		InitTilesArray ();

		//OnTeleSelectionStart ();
		//OnTeleSelectionUpdate(transform.position + new Vector3(1.56f, 0, 1.80f));
	}



	void Update()
	{
		if (currentGameState == GameFlowState.DELAY_FOR_NEXT_MOVE)
		{
			moveDelayTimer -= Time.deltaTime;

			if (moveDelayTimer <= 0)
			{
				currentGameState = GameFlowState.WAITING_FOR_UNIT;
				NextMove ();
			}
		}

	}

	public void NextMove()
	{
		if (currentUnitIndex >= unitsList.Count)
			currentUnitIndex = 0;

		while (unitsList [currentUnitIndex] == null)
		{
			unitsList.RemoveAt (currentUnitIndex);
			if (currentUnitIndex >= unitsList.Count)
				currentUnitIndex = 0;
		}

		unitsList [currentUnitIndex].movesRemaining = unitsList [currentUnitIndex].moveSpeed;
		unitsList [currentUnitIndex].MoveStart ();

		//DisplayPossibleMoves (unitsList [currentUnitIndex]);

		if (currPlayerHighlightObj != null)
		{
			currPlayerHighlightObj.SetActive (true);
			currPlayerHighlightObj.transform.position = unitsList [currentUnitIndex].transform.position;
		}
	}

	public Vector3 GetUnitPos(Unit who)
	{
		for (int iterX = 0; iterX < mapSize.x; iterX++)
		{
			for (int iterY = 0; iterY < mapSize.y; iterY++)
			{
				for (int iterZ = 0; iterZ < mapSize.z; iterZ++)
				{
					if (tilesMap [iterX, iterY, iterZ].type == TileType.UNIT && tilesMap [iterX, iterY, iterZ].unitRef == who)
						return new Vector3 (iterX, iterY, iterZ);
				}
			}
		}

		return new Vector3 (-1, -1, -1);
	}

	/// <summary>
	/// Should be called only when the unit controlled by player gets moved.
	/// </summary>
	public void PlayerUnitMoved(Unit who)
	{
		if (playerWallObj != null)
		{
			playerWallObj.SetActive (true);
			playerWallObj.transform.position = who.transform.position;
		}
	}

	public void DisplayPossibleMoves(Unit who)
	{
		StopDisplayingMoves ();

		if (possibleMovePrefab == null)
			return;

//		int iterY = 0;
//
//		for (int iterX = 0; iterX < mapSize.x; iterX++)
//		{
//			for (int iterZ = 0; iterZ < mapSize.z; iterZ++)
//			{
//				if (tilesMap [iterX, iterY, iterZ].type == TileType.EMPTY && GetMoveDistToTargetTile (who.gridPos, new Vector3 (iterX, iterY, iterZ)) <= who.movesRemaining)
//				{
//					GameObject newObj = Instantiate (possibleMovePrefab, GridToWorldPos(new Vector3(iterX, iterY, iterZ)), transform.rotation, transform);
//					possibleMovesObjsList.Add (newObj);
//				}
//			}
//		}

		List<Vector3> WorldPosList = WhereCanIMoveWorldPos (who);

		for (int i = 0; i < WorldPosList.Count; i++)
		{
			GameObject newObj = Instantiate (possibleMovePrefab, WorldPosList[i], transform.rotation, transform);
			possibleMovesObjsList.Add (newObj);
		}
	}

	public void StopDisplayingMoves()
	{
		for (int i = 0; i < possibleMovesObjsList.Count; i++)
			Destroy (possibleMovesObjsList [i]);

		possibleMovesObjsList.Clear ();
	}

	public void MoveEnd(Unit who)
	{
		if (unitsList.Contains (who) == false)
			return;

		if (unitsList [currentUnitIndex] == who)
		{
			StopDisplayingMoves ();

			currentUnitIndex += 1;
			if (currentUnitIndex >= unitsList.Count)
				currentUnitIndex = 0;

			moveDelayTimer = delayBetweenMoves;

			currentGameState = GameFlowState.DELAY_FOR_NEXT_MOVE;

			if (currPlayerHighlightObj != null)
			{
				currPlayerHighlightObj.SetActive (false);
			}
		}
	}


	public Vector3 WorldPosToGridPos(Vector3 worldPos)
	{
		return LocalPosToGridPos (transform.InverseTransformPoint(worldPos));
	}

	public Vector3 LocalPosToGridPos(Vector3 localPos)
	{
		Vector3 halfTileSize = MultVec3 (tileSize, 0.5f);

		localPos += halfTileSize;

		Vector3 returnVal;

		returnVal.x = localPos.x / tileSize.x;
		//returnVal.y = localPos.y / tileSize.y;
		returnVal.y = 0;
		returnVal.z = localPos.z / tileSize.z;

		returnVal.x = Mathf.Floor (returnVal.x);
		returnVal.y = Mathf.Floor (returnVal.y);
		returnVal.z = Mathf.Floor (returnVal.z);

		return returnVal;
	}

	public Vector3 GridToWorldPos(Vector3 gridPos)
	{
		Vector3 returnVal;

		returnVal = MultVec3 (gridPos, tileSize);

		return returnVal;
	}

	public List<Vector3> WhereCanIMoveWorldPos(Unit who)
	{
		List<Vector3> returnList = WhereCanIMoveGridPos(who);

		for (int i = 0; i < returnList.Count; i++)
			returnList [i] = GridToWorldPos (returnList [i]);

		return returnList;
	}

	public List<Vector3> WhereCanIMoveGridPos(Unit who)
	{
		List<Vector3> returnList = new List<Vector3> ();

		int iterY = 0;

		for (int iterX = 0; iterX < mapSize.x; iterX++)
		{
			for (int iterZ = 0; iterZ < mapSize.z; iterZ++)
			{
				if (tilesMap [iterX, iterY, iterZ].type == TileType.EMPTY && GetMoveDistToTargetTile (who.gridPos, new Vector3 (iterX, iterY, iterZ)) <= who.movesRemaining)
				{
					returnList.Add (new Vector3(iterX, iterY, iterZ));
				}
			}
		}

		return returnList;
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

	public void OnTeleSelectionStart(Unit who = null)
	{
		if (lookTileSelectionObj != null)
			lookTileSelectionObj.SetActive (true);

		if (who != null)
			DisplayPossibleMoves (who);
	}

	public void OnTeleSelectionUpdate(Vector3 worldPos, Unit who = null)
	{
		if (lookTileSelectionObj == null)
			return;

		if (who == null)
		{
			lookTileSelectionObj.transform.position = GridToWorldPos(WorldPosToGridPos(worldPos));
		}
		else
		{
			CanMoveResponse response = IWannaMoveWorldPos (worldPos, who);

			if (response.canMove == false)
				lookTileSelectionObj.SetActive (false);
			else
			{
				lookTileSelectionObj.SetActive (true);
				lookTileSelectionObj.transform.position = response.targetWorldPos;
			}
		}

	}

	public void OnTeleSelectionStop()
	{
		if (lookTileSelectionObj != null)
			lookTileSelectionObj.SetActive (false);

		StopDisplayingMoves ();
	}

	public void PlayerChangedWorldPos(Vector3 worldPos)
	{
		if (playerTileWallObj != null)
		{
			playerTileWallObj.SetActive (true);
			playerTileWallObj.transform.position = worldPos;
		}
	}

	public void PlayerChangedLocalPos(Vector3 localPos)
	{
		PlayerChangedWorldPos (transform.TransformPoint (localPos));
	}


	public CanMoveResponse IWannaMoveWorldPos(Vector3 worldPos, Unit movingUnit)
	{
		return IWannaMoveLocalPos (transform.InverseTransformPoint(worldPos), movingUnit);
	}

	public CanMoveResponse IWannaMoveLocalPos(Vector3 localPos, Unit movingUnit)
	{
		return IWannaMoveGridPos(LocalPosToGridPos(localPos), movingUnit);
	}

	public CanMoveResponse IWannaMoveGridPos(Vector3 gridPos, Unit movingUnit)
	{
		CanMoveResponse responseObj = new CanMoveResponse ();

		if (unitsList.Contains (movingUnit) == false || unitsList[currentUnitIndex] != movingUnit || movingUnit.movesRemaining <= 0)
		{
			responseObj.canMove = false;
			return responseObj;
		}

		if (gridPos.x < 0 || gridPos.y < 0 || gridPos.z < 0 ||
			gridPos.x >= mapSize.x || gridPos.y >= mapSize.y || gridPos.z >= mapSize.z ||
			tilesMap[(int)gridPos.x, (int)gridPos.y, (int)gridPos.z].type != TileType.EMPTY)
		{
			responseObj.canMove = false;
			return responseObj;
		}

		List<Vector3> posesList = GetMovePosesToTargetTile (movingUnit.gridPos, gridPos);

		while (posesList.Count > movingUnit.movesRemaining)
			posesList.RemoveAt (posesList.Count - 1);

		gridPos = posesList [posesList.Count - 1];

		responseObj.spacesMoved = 1;
		responseObj.canMove = true;

		responseObj.targetGridPos = gridPos;
		responseObj.targetLocalPos = GridToWorldPos (responseObj.targetGridPos);
		responseObj.targetWorldPos = transform.InverseTransformPoint (responseObj.targetLocalPos);

		responseObj.movePathGridPos = posesList;

		responseObj.movePathWorldPos = new List<Vector3> ();

		for (int i = 0; i < responseObj.movePathGridPos.Count; i++)
			responseObj.movePathWorldPos.Add (GridToWorldPos(responseObj.movePathGridPos[i]));

		return responseObj;

	}

	public void IJustMovedWorldPos(Vector3 worldPos, Unit movingUnit)
	{
		IJustMovedLocalPos (transform.InverseTransformPoint(worldPos), movingUnit);
	}

	public void IJustMovedLocalPos(Vector3 localPos, Unit movingUnit)
	{
		IJustMovedGridPos(LocalPosToGridPos(localPos), movingUnit);
	}

	public void IJustMovedGridPos(Vector3 gridPos, Unit movingUnit)
	{
		movingUnit.gridPos = gridPos;

		Vector3 fromGridPos = GetUnitPos (movingUnit);

		if (fromGridPos.x > -1)
		{
			tilesMap [(int)gridPos.x, (int)gridPos.y, (int)gridPos.z].type = TileType.UNIT;
			tilesMap [(int)gridPos.x, (int)gridPos.y, (int)gridPos.z].objRef = movingUnit.gameObject;
			tilesMap [(int)gridPos.x, (int)gridPos.y, (int)gridPos.z].unitRef = movingUnit;

			tilesMap [(int)fromGridPos.x, (int)fromGridPos.y, (int)fromGridPos.z].type = TileType.EMPTY;
			tilesMap [(int)fromGridPos.x, (int)fromGridPos.y, (int)fromGridPos.z].objRef = null;
			tilesMap [(int)fromGridPos.x, (int)fromGridPos.y, (int)fromGridPos.z].unitRef = null;
		}

		if (unitsList.Contains (movingUnit) == false || unitsList [currentUnitIndex] != movingUnit)
			return;

		if (currPlayerHighlightObj != null)
		{
			currPlayerHighlightObj.SetActive (true);
			currPlayerHighlightObj.transform.position = movingUnit.transform.position;
		}


	}

	public int GetMoveDistToTargetTile(Vector3 fromGridPos, Vector3 toGridPos)
	{
		List<Vector3> posesList = GetMovePosesToTargetTile (fromGridPos, toGridPos);

		if (posesList == null)
			return 40000;
		else 
			return posesList.Count;
	}

	public List<Vector3> GetMovePosesToTargetTile(Vector3 fromGridPos, Vector3 toGridPos)
	{
		List<Vector3> returnList = new List<Vector3> ();

		int gridHeight = (int)fromGridPos.y;

		int[,,] valueGrid = new int[(int)mapSize.x, (int)mapSize.y, (int)mapSize.z];

		List<Vector3> currSearchTiles = new List<Vector3> ();
		List<Vector3> newSearchTiles = new List<Vector3> ();

		Vector3 checkVec1 = new Vector3 (1, 0, 0);
		Vector3 checkVec2 = new Vector3 (-1, 0, 0);
		Vector3 checkVec3 = new Vector3 (0, 0, 1);
		Vector3 checkVec4 = new Vector3 (0, 0, -1);

		currSearchTiles.Add (toGridPos);

		int currDist = 2;
		valueGrid [(int)toGridPos.x, (int)toGridPos.y, (int)toGridPos.z] = 1;

		bool foundStart = false;

		while (foundStart == false)
		{
			if (currSearchTiles.Count <= 0)
				foundStart = true;

			for (int i = 0; i < currSearchTiles.Count; i++)
			{
				if (currSearchTiles [i] == fromGridPos)
				{
					i = currSearchTiles.Count;
					foundStart = true;

					break;
				}

				if (MarkGridValue (valueGrid, currSearchTiles [i] + checkVec1, currDist, mapSize))
					newSearchTiles.Add (currSearchTiles [i] + checkVec1);
				if (MarkGridValue (valueGrid, currSearchTiles [i] + checkVec2, currDist, mapSize))
					newSearchTiles.Add (currSearchTiles [i] + checkVec2);
				if (MarkGridValue (valueGrid, currSearchTiles [i] + checkVec3, currDist, mapSize))
					newSearchTiles.Add (currSearchTiles [i] + checkVec3);
				if (MarkGridValue (valueGrid, currSearchTiles [i] + checkVec4, currDist, mapSize))
					newSearchTiles.Add (currSearchTiles [i] + checkVec4);
			}

			currSearchTiles.Clear ();
			currSearchTiles = new List<Vector3> (newSearchTiles);

			newSearchTiles.Clear ();

			currDist += 1;
		}

		bool foundEnd = false;

		Vector3 checkedPos = fromGridPos;

		int myDist = 40000;

		while (foundEnd == false)
		{
			if (checkedPos == toGridPos)
			{
				//returnList.Add (toGridPos);

				foundEnd = true;
			}
			else
			{
				bool nextTileFound = false;
				int bestDist = myDist;
				Vector3 bestPos = Vector3.zero;

				int checkedVal = -1;

				checkedVal = GetGridValue (valueGrid, checkedPos + checkVec1, mapSize);
				if (checkedVal > -1 && checkedVal < bestDist)
				{
					bestDist = checkedVal;
					bestPos = checkedPos + checkVec1;
					nextTileFound = true;
				}

				checkedVal = GetGridValue (valueGrid, checkedPos + checkVec2, mapSize);
				if (checkedVal > -1 && checkedVal < bestDist)
				{
					bestDist = checkedVal;
					bestPos = checkedPos + checkVec2;
					nextTileFound = true;
				}

				checkedVal = GetGridValue (valueGrid, checkedPos + checkVec3, mapSize);
				if (checkedVal > -1 && checkedVal < bestDist)
				{
					bestDist = checkedVal;
					bestPos = checkedPos + checkVec3;
					nextTileFound = true;
				}

				checkedVal = GetGridValue (valueGrid, checkedPos + checkVec4, mapSize);
				if (checkedVal > -1 && checkedVal < bestDist)
				{
					bestDist = checkedVal;
					bestPos = checkedPos + checkVec4;
					nextTileFound = true;
				}

				if (nextTileFound)
				{
					returnList.Add (bestPos);
					checkedPos = bestPos;
					myDist = bestDist;
				}
				else
				{
					// If there is no target pos, then return null, as there is no correct path:
					return null;
				}
			}
		}

		return returnList;
	}

	public bool MarkGridValue(int[,,] valueGrid, Vector3 markPos, int distVal, Vector3 valGridSize)
	{
		// If target tile outta bounds, return:
		if (markPos.x < 0 || markPos.y < 0 || markPos.z < 0 ||
		    markPos.x >= valGridSize.x || markPos.y >= valGridSize.y || markPos.z >= valGridSize.z)
			return false;

		if (tilesMap [(int)markPos.x, (int)markPos.y, (int)markPos.z].type != TileType.EMPTY)
			return false;

		// If this tile's dist is smaller, than proposed, then do nothing:
		if (valueGrid [(int)markPos.x, (int)markPos.y, (int)markPos.z] > 0 && valueGrid [(int)markPos.x, (int)markPos.y, (int)markPos.z] <= distVal)
			return false;

		valueGrid [(int)markPos.x, (int)markPos.y, (int)markPos.z] = distVal;

		return true;
	}

	public int GetGridValue(int[,,] valueGrid, Vector3 markPos, Vector3 valGridSize)
	{
		// If target tile outta bounds, return:
		if (markPos.x < 0 || markPos.y < 0 || markPos.z < 0 ||
			markPos.x >= valGridSize.x || markPos.y >= valGridSize.y || markPos.z >= valGridSize.z)
			return -1;

		if (valueGrid [(int)markPos.x, (int)markPos.y, (int)markPos.z] <= 0)
			return -1;

		return valueGrid [(int)markPos.x, (int)markPos.y, (int)markPos.z];
	}

	public TileData RayCastToTileData(RaycastHit hit, Vector3 gridPos)
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

		if (unitScript == null)
			unitScript = returnVal.objRef.GetComponentInParent<Unit>();

		if (unitScript != null)
		{
			returnVal.unitRef = unitScript;

			returnVal.unitRef.gridPos = gridPos;
			returnVal.unitRef.movesRemaining = 0;
			returnVal.type = TileType.UNIT;
		}

		return returnVal;
	}

	public void InitTilesArray()
	{
		unitsList = new List<Unit> ();
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

					if (Physics.BoxCast (checkPos + Vector3.up * 1.5f, colliderSize, Vector3.down, out hit, transform.rotation, 1))
					{
						tilesMap [iterX, iterY, iterZ] = RayCastToTileData (hit, new Vector3(iterX, iterY, iterZ));

						if (tilesMap [iterX, iterY, iterZ].unitRef != null)
							unitsList.Add (tilesMap [iterX, iterY, iterZ].unitRef);
					}
					else
					{
						if (Physics.BoxCast (checkPos + Vector3.up, colliderSize, Vector3.down, out hit, transform.rotation, 2))
							tilesMap [iterX, iterY, iterZ].type = TileType.EMPTY;
						else
							tilesMap [iterX, iterY, iterZ].type = TileType.WALL;
					}
				}
			}
		}

		//Physics.BoxCast (transform.position, tileSize * 0.8f, Vector3.one, out hit, Quaternion.identity, 0);
	}

}

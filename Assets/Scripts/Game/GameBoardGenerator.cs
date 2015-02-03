using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameBoardGenerator : MonoBehaviour 
{
	public GameObject Hex;
	public int gridWidthInHexes;
	public int gridHeightInHexes;
	public static GameBoardGenerator instance = null;
	public int GridLayerMask = 1 << 8;

	private float hexWidth;
	private float hexHeight;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		setSizes();
		createGrid();
	}

	void setSizes()
	{
		hexWidth = Hex.renderer.bounds.size.x;
		hexHeight = Hex.renderer.bounds.size.z;
	}

	Vector3 calcInitPos()
	{
		Vector3 initPos;
		initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 0,
		                      gridHeightInHexes / 2f * hexHeight - hexHeight / 2);
		
		return initPos;
	}

	public Vector3 calcWorldCoord(Vector2 gridPos)
	{
		Vector3 initPos = calcInitPos();
		float offset = 0;
		if (gridPos.y % 2 != 0)
			offset = hexWidth / 2;
		
		float x =  initPos.x + offset - gridPos.x * hexWidth;
		float z = initPos.z - gridPos.y * hexHeight * 0.75f;
		return new Vector3(x, 0, z);
	}

	void createGrid()
	{
		GameObject hexGridGO = GameObject.Find("Game Board");
		
		for (float y = 0; y < gridHeightInHexes; y++)
		{
			for (float x = 0; x < gridWidthInHexes; x++)
			{
				GameObject hex = (GameObject)Instantiate(Hex);
				if (y == 0 || y == 1 || y == 6 || y == 7)
				{
					hex.transform.tag = "Column" + (y + 1);
				}
				hex.name = "hex " + (y) + "-" + (x) ;
				hex.layer = GridLayerMask;
				Vector2 gridPos = new Vector2(x, y);
				hex.transform.position = calcWorldCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;
				var tb = (GameTile)hex.GetComponent("GameTile");
				tb.tile = new Tile((int)y, (int)x);
				GameBoard.instance.board.Add(tb.tile.Location, tb.tile);
			}
		}
		foreach(Tile tile in GameBoard.instance.board.Values)
			tile.FindNeighbours(GameBoard.instance.board, new Vector2(gridHeightInHexes, gridWidthInHexes));
		hexGridGO.transform.Rotate(new Vector3(0, -90, 90));
		hexGridGO.transform.localScale = new Vector3(1, 40, 40);
		hexGridGO.transform.CenterOnChildred();
		hexGridGO.transform.localPosition = new Vector3(0, 0, 0);
	}  
}

public static class GO_Extensions
{
	public static void CenterOnChildred(this Transform aParent)
	{
		var childs = aParent.Cast<Transform>().ToList();
		var pos = Vector3.zero;
		foreach(var C in childs)
		{
			pos += C.position;
			C.parent = null;
		}
		pos /= childs.Count;
		aParent.position = pos;
		foreach(var C in childs)
			C.parent = aParent;
	}    
}

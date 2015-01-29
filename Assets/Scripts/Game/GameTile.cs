using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameTile : MonoBehaviour {

	public Tile tile;
	public Texture2D cursorDragging;
	public Texture2D cursorExchange;
	public bool TimeOfPositionning = true;
	public static GameTile instance = null;
	public static List<string> AvailableStartingColumns = new List<string>();

	private Vector2 cursorHotspot = Vector2.zero;


	void Awake()
	{
		instance = this;

	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseEnter()
	{
		if (GameBoard.instance.isDragging && AvailableStartingColumns.Any(e => e.Contains(this.tag)))
		{
			SetCursorToDrag();
		}
		else
		{
			SetCursorToDefault();
		}
	}

	void OnMouseOver()
	{
		if (GameBoard.instance.droppedCard && AvailableStartingColumns.Any(e => e.Contains(this.tag)))
		{
			GameBoard.instance.CardSelected.transform.position = this.transform.position + new Vector3(0, 0, -1);
			GameBoard.instance.droppedCard = false;
			GameBoard.instance.isDragging = false;
		}
	}


	public void SetCursorToDrag()
	{
		Cursor.SetCursor(cursorDragging, cursorHotspot, CursorMode.Auto);
	}
	public void SetCursorToExchange()
	{
		Cursor.SetCursor(cursorExchange, cursorHotspot, CursorMode.Auto);
	}
	public void SetCursorToDefault()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
}

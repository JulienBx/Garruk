﻿ using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameTile : MonoBehaviour {

	public Tile tile;
	public Texture2D cursorDragging;
	public Texture2D cursorExchange;
	public static GameTile instance = null;
	public static List<string> AvailableStartingColumns = new List<string>();
	public Material DefaultMaterial;
	public Material OpaqueMaterial;

	private Vector2 cursorHotspot = Vector2.zero;
	public float hexWidth;
	public float hexHeight;
	public int gridWidthInHexes = 5;
	public int gridHeightInHexes = 8;

	void Awake()
	{
		instance = this;

	}
	// Use this for initialization
	void Start () {
		setSizes();
	}

	void setSizes()
	{
		hexWidth = renderer.bounds.size.x;
		hexHeight = renderer.bounds.size.y;
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

	public void changeColor(Color color)
	{
		if (color.a == 1)
			color.a = 130f / 255f;
		renderer.material = OpaqueMaterial;
		renderer.material.color = color;
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

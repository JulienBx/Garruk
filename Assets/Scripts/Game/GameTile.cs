﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameTile : MonoBehaviour {
	
	public Tile tile;
	public Texture2D cursorDragging;
	public Texture2D cursorExchange;
	public Texture2D cursorAttack;
	public Texture2D cursorTarget;
	public static GameTile instance = null;
	public static List<string> AvailableStartingColumns = new List<string>();
	public Material DefaultMaterial;
	public Material OpaqueMaterial;

	public Texture[] backTile ;
	
	private Vector2 cursorHotspot = Vector2.zero;
	public float hexWidth;
	public float hexHeight;
	public int gridWidthInHexes = 5;
	public int gridHeightInHexes = 8;
	public int pathIndex;
	
	public bool Passable = false;
	
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
		if (GameBoard.instance.TimeOfPositionning)
		{
			if (GameBoard.instance.isDragging && AvailableStartingColumns.Any(e => e.Contains(this.tag)))
			{
				SetCursorToDrag();
			} else
			{
				SetCursorToDefault();
			}
		} else
		{
			if (GameBoard.instance.isMoving && this.Passable)
			{
				Vector3 newPosition = this.transform.position;
				newPosition.z = -1;
				GameBoard.instance.CardSelected.transform.position = newPosition;
				if (GameBoard.instance.CardSelected.currentTile.Equals(this))
				{
					GamePlayingCard.instance.attemptToMoveTo = null;
				}
				else
				{
					GamePlayingCard.instance.attemptToMoveTo = this;
				}
			}
			else
			{
				GamePlayingCard.instance.attemptToMoveTo = null;
			}
		}
	}
	
	void OnMouseOver()
	{
		if (GameBoard.instance.droppedCard && AvailableStartingColumns.Any(e => e.Contains(this.tag)))
		{
			Vector3 pos = transform.TransformPoint(Vector3.zero) + new Vector3(0, 0, -2);
			RaycastHit hit;
			
			if (Physics.Raycast(pos, Vector3.forward, out hit))
			{
				if (hit.transform.gameObject.tag != "PlayableCard")
				{
					GameBoard.instance.CardSelected.transform.position = this.transform.position + new Vector3(0, 0, -1);
					GameBoard.instance.droppedCard = false;
					GameBoard.instance.isDragging = false;
				}
			}
			
		}
	}

	public void ShowFace() 
	{
		renderer.materials[1].mainTexture = backTile [this.tile.type];
	}
	
	public void changeColor(Color color)
	{
		if (color.a == 1)
			color.a = 130f / 255f;
		renderer.material = OpaqueMaterial;
		renderer.material.color = color;
	}
	
	public static void RemovePassableTile()
	{
		foreach(Transform go in GameBoard.instance.gameObject.transform)
		{
			if (!go.gameObject.name.Equals("Game Board"))
			{
				go.renderer.material = GameTile.instance.DefaultMaterial;
				go.GetComponent<GameTile>().Passable = false;
			}
		}
	}
	
	public static void InitIndexPathTile()
	{
		foreach(Transform go in GameBoard.instance.gameObject.transform)
		{
			if (!go.gameObject.name.Equals("Game Board"))
			{
				go.GetComponent<GameTile>().pathIndex = -1;
			}
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
	public void SetCursorToAttack()
	{
		Cursor.SetCursor(cursorAttack, Vector2.zero, CursorMode.Auto);
	}
	public void SetCursorToTarget()
	{
		Cursor.SetCursor(cursorTarget, Vector2.zero, CursorMode.Auto);
	}
}

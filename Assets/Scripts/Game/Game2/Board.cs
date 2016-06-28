using System;
using UnityEngine;

public class Board
{
	int boardWidth = 6 ; 
	int boardHeight = 8;
	GameObject[,] tiles ;
	GameObject[] verticalBorders ;
	GameObject[] horizontalBorders ;

	public Board ()
	{
		this.tiles = new GameObject[this.boardWidth,this.boardHeight];
		this.verticalBorders = new GameObject[this.boardWidth+1];
		this.horizontalBorders = new GameObject[this.boardHeight+1];
	}

	public int getBoardWidth(){
		return this.boardWidth;
	}

	public int getBoardHeight(){
		return this.boardHeight;
	}

	public TileC getTileC(int x, int y){
		return this.tiles[x,y].GetComponent<TileC>();
	}
}


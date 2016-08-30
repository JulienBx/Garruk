using System;
using UnityEngine;
using System.Collections.Generic;

public class Board
{
	int boardWidth = 6 ; 
	int boardHeight = 8;
	int nbTiles = 0 ;
	GameObject[,] tiles ;
	GameObject[] verticalBorders ;
	GameObject[] horizontalBorders ;

	public Board ()
	{
		this.tiles = new GameObject[this.boardWidth,this.boardHeight];
		this.verticalBorders = new GameObject[this.boardWidth+1];
		this.horizontalBorders = new GameObject[this.boardHeight+1];
	}

	public void addHorizontalBorder(int i, GameObject horizontalBorder){
		this.horizontalBorders [i] = horizontalBorder;
	}

	public void sizeHorizontalBorder(int i, Vector3 position, Vector3 scale){
		this.horizontalBorders [i].transform.localPosition = position;
		this.horizontalBorders [i].transform.localScale = scale;
	}

	public void sizeVerticalBorder(int i, Vector3 position, Vector3 scale){
		this.verticalBorders [i].transform.localPosition = position;
		this.verticalBorders [i].transform.localScale = scale;
	}

	public void addVerticalBorder(int i, GameObject verticalBorder){
		this.verticalBorders [i] = verticalBorder;
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

	public TileC getTileC(TileM t){
		return this.tiles[t.x,t.y].GetComponent<TileC>();
	}

	public void addTile(int x, int y, bool isRock, GameObject g){
		this.nbTiles++;
		this.tiles[x,y] = g ;
		this.getTileC(x,y).setTile(x,y);
		this.getTileC(x,y).setRock(isRock);
	}

	public bool isLoaded(){
		return (this.nbTiles==48);
	}

	public int[,] getOpponentStartingTilesOccupancy(){
		int[, ]tilesOccupancy = new int[6,2];
		for(int i = 0 ; i < this.boardWidth ; i++){
			for(int j = 6 ; j < this.boardHeight ; j++){
				if(this.getTileC(i,j).isRock()){
					tilesOccupancy[i,j-6]=2;
				}
				else if(this.getTileC(i,j).hasCharacter()){
					tilesOccupancy[i,j-6]=1;
				}
				else{
					tilesOccupancy[i,j-6]=0;
				}
			}
		}
		return tilesOccupancy;
	}

	public List<TileM> getTileNeighbours(TileM t){
		List<TileM> tiles = new List<TileM>();
		if(t.x>0){
			tiles.Add(new TileM(t.x-1,t.y));
		}
		if(t.y>0){
			tiles.Add(new TileM(t.x,t.y-1));
		}
		if(t.x<this.getBoardWidth()-1){
			tiles.Add(new TileM(t.x+1,t.y));
		}
		if(t.y<this.getBoardHeight()-1){
			tiles.Add(new TileM(t.x,t.y+1));
		}
		return tiles;
	} 

	public TileM getMouseTile(){
		Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		TileM tile = new TileM();

		if(Game.instance.isFirstPlayer()){
			tile.x = Mathf.FloorToInt(vec.x/Game.instance.getTileScale())+3;
			tile.y = Mathf.FloorToInt(vec.y/Game.instance.getTileScale())+4;
		}
		else{
			tile.x = (GameView.instance.boardWidth-1)-Mathf.FloorToInt(vec.x/Game.instance.getTileScale())+3;
			tile.y = (GameView.instance.boardHeight-1)-Mathf.FloorToInt(vec.y/Game.instance.getTileScale())+4;
		}

		return tile;
	}
}


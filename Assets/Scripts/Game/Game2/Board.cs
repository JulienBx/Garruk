﻿using System;
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

	public List<TileM> getAdjacentOpponentsTargets(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();
		int playerID;

		foreach (TileM t in neighbourTiles)
		{
			playerID = board[t.x,t.y];
			if (playerID >= 0)
			{
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						cibles.Add(t);
					}
				}
			}
		}
		return cibles;
	}

	public List<TileM> getAdjacentTargets(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();
		int playerID;

		foreach (TileM t in neighbourTiles)
		{
			playerID = board[t.x,t.y];
			if (playerID >= 0)
			{
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					cibles.Add(t);
				}
			}
		}
		return cibles;
	}

	public List<TileM> getOpponentsTargets(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();

		for(int i = 0 ; i < Game.instance.getCards().getNumberOfCards(); i++)
		{
			if(Game.instance.getCards().getCardC(i).canBeTargeted()){
				if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(i).getCardM().isMine()){
					cibles.Add(Game.instance.getCards().getCardC(i).getTileM());
				}
			}
		}
		return cibles;
	}

	public List<TileM> getAllysTargets(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();

		for(int i = 0 ; i < Game.instance.getCards().getNumberOfCards(); i++)
		{
			if(Game.instance.getCards().getCardC(i).canBeTargeted()){
				if(i!=Game.instance.getCurrentCardID()){
					if(card.getCardM().isMine()==Game.instance.getCards().getCardC(i).getCardM().isMine()){
						cibles.Add(Game.instance.getCards().getCardC(i).getTileM());
					}
				}
			}
		}
		return cibles;
	}

	public List<TileM> getAdjacentCristals(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();

		foreach (TileM t in neighbourTiles)
		{
			if (board[t.x,t.y] == -2){
				cibles.Add(t);
			}
		}
		return cibles;
	}

	public List<TileM> get1TileAwayOpponents(int[,] board, CardC card, TileM tile){
		List<TileM> cibles = new List<TileM>();
		int playerID;

		if(tile.x>1){
			playerID = board[tile.x-2,tile.y];
			if (playerID >= 0){
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						cibles.Add(new TileM(tile.x-2, tile.y));
					}
				}
			}
		}
		if(tile.y>1){
			playerID = board[tile.x,tile.y-2];
			if (playerID >= 0){
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						cibles.Add(new TileM(tile.x, tile.y-2));
					}
				}
			}
		}
		if(tile.x<this.getBoardWidth()-2){
			playerID = board[tile.x+2,tile.y];
			if (playerID >= 0){
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						cibles.Add(new TileM(tile.x+2, tile.y));
					}
				}
			}
		}
		if(tile.y<this.getBoardHeight()-2){
			playerID = board[tile.x,tile.y+2];
			if (playerID >= 0){
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						cibles.Add(new TileM(tile.x, tile.y+2));
					}
				}
			}
		}
			
		return cibles;
	}

	public List<TileM> getAdjacentAllysTargets(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();
		int playerID;

		foreach (TileM t in neighbourTiles)
		{
			playerID = board[t.x,t.y];
			if (playerID >= 0)
			{
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()==Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						if(card.getCardM().getCharacterType()!=Game.instance.getCards().getCardC(playerID).getCardM().getCharacterType()){
							cibles.Add(t);
						}
					}
				}
			}
		}
		return cibles;
	}

	public List<TileM> getMySelfWithNeighbours(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();
		bool hasSomeone = false;
		int playerID;

		foreach (TileM t in neighbourTiles)
		{
			playerID = board[t.x,t.y];
			if (playerID >= 0)
			{
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					hasSomeone = true;
				}
			}
		}

		if(hasSomeone){
			cibles.Add(tile);
		}
		return cibles;
	}

	public int[,] getCurrentBoard(){
		int[,] board = new int[this.boardWidth,this.boardHeight];
		for(int i = 0 ; i < this.boardWidth ; i++){
			for(int j = 0 ; j < this.boardHeight ; j++){
				board[i,j] = this.getTileC(i,j).getBoardValue();
			}
		}
		return board;
	}

	public void startTargets(List<TileM> targets){
		for (int i = 0 ; i < targets.Count ; i++){
			this.getTileC(targets[i]).setTarget(true);
		}
	}

	public void stopTargets(List<TileM> targets){
		for (int i = 0 ; i < targets.Count ; i++){
			this.getTileC(targets[i]).setTarget(false);
		}
	}
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NeighbourTiles {
	
	public List<Tile> tiles;
	public List<int> UID;

	public NeighbourTiles(int x, int y, int[,] characterTiles, int distance){

		bool areNewTiles = true ;
		int compteurDistance = 1 ;
		this.tiles = new List<Tile>();
		this.UID = new List<int>();
		List<Tile> newTiles = new List<Tile>();
		tiles.Add(new Tile(x, y, 0));
		newTiles.Add(new Tile(x, y, 0));
		List<Tile> tempTiles = new List<Tile>();
		List<Tile> tempNewTiles = new List<Tile>();
		int width = GameController.instance.boardWidth;

		UID.Add(y*width+x);

		while (areNewTiles == true && compteurDistance<=distance){
			areNewTiles = false ;
			tempNewTiles = new List<Tile>();
			for (int i = 0 ; i < newTiles.Count ; i++){
				tempTiles = this.getImmediateNeighbours(newTiles[i].x, newTiles[i].y);
				for (int j = 0 ; j < tempTiles.Count ; j++){
					if (characterTiles[tempTiles[j].x,tempTiles[j].y]<5){
						if (!UID.Contains((tempTiles[j].y*width+tempTiles[j].x))){
							tempNewTiles.Add(tempTiles[j]);
							UID.Add(tempTiles[j].y*width+tempTiles[j].x);
							areNewTiles = true ;
						}
					}
				}
			}
			newTiles = new List<Tile>();
			for (int i = 0 ; i < tempNewTiles.Count ; i++){
				newTiles.Add(tempNewTiles[i]);
				tiles.Add(new Tile(tempNewTiles[i].x, tempNewTiles[i].y, compteurDistance));
				//Debug.Log("J'ajoute le neighbour "+tempNewTiles[i].x+","+tempNewTiles[i].y+","+compteurDistance);
			}
			compteurDistance ++ ;
		}
	}

	public List<Tile> getImmediateNeighbours(int x, int y){
		List<Tile> tempTiles = new List<Tile>();
		int width = GameController.instance.boardWidth;
		int height = GameController.instance.boardHeight;
		int decalage ;
		if ((width - x) % 2 == 0)
		{
			decalage = 1;
		} else
		{
			decalage = 0;
		}

		if (x > 0){
			if (decalage==0 && y > 0){
				tempTiles.Add(new Tile(x-1, y-1));
			}
			if (decalage==1){
				tempTiles.Add(new Tile(x-1, y));
			}
			
			if (decalage==0 && y < height-2){
				tempTiles.Add(new Tile(x-1, y));
			}
			else if(decalage==1 && y < height-1){
				tempTiles.Add(new Tile(x-1, y+1));
			}
		}
		if (y < height-1){
			tempTiles.Add(new Tile(x, y+1));
		}
		if (y > 0){
			tempTiles.Add(new Tile(x, y-1));
		}
		if (x < width-1){
			if (decalage==0 && y >0){
				tempTiles.Add(new Tile(x+1, y-1));
			}
			if (decalage==1){
				tempTiles.Add(new Tile(x+1, y));
			}

			if (decalage==0 && y < height-2){
				tempTiles.Add(new Tile(x+1, y));
			}
			else if(decalage==1 && y < height-1){
				tempTiles.Add(new Tile(x+1, y+1));
			}
		}
		
		return tempTiles;
	}

	public List<Tile> getNeighboursWithin(int distance){
		List<Tile> tempTiles = new List<Tile>();
		for (int i = 0 ; i < this.tiles.Count ; i++){
			if (this.tiles[i].distance <= distance){
				tempTiles.Add(this.tiles[i]);
			}
		}
		return tempTiles ;
	}
}

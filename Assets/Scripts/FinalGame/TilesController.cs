using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilesController
{
	GameObject [,] tiles ;

	public TilesController (int a, int b)
	{
		this.tiles = new GameObject[a,b];
	}

	public void addTile(int a, int b, GameObject tile, bool isRock){
		this.tiles[a,b] = tile;
	}

	public void createTiles(int width, int height){
		
		List<TileModel> rocks = new List<TileModel>();
		int nbRocksToAdd = UnityEngine.Random.Range(3, 6);
		int compteurRocks = 0;
		bool isOk = true;
		TileModel t = new TileModel();
		while (compteurRocks<nbRocksToAdd){
			isOk = false;
			while (!isOk)
			{
				t = new TileModel();
				t.randomize(width, height);
				isOk = true;
				for (int a = 0; a < rocks.Count && isOk; a++){
					if (rocks[a].getX() == t.getX() && rocks [a].getY() == t.getY())
					{
						isOk = false;
					}
				}
			}
			rocks.Add(t);
			compteurRocks++;
		}

		bool isRock = false;
		for (int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				isRock = false;
				for (int z = 0; z < rocks.Count && !isRock; z++){
					if (rocks [z].getX() == x && rocks [z].getY() == y){
						isRock = true;
					}
				}
				if(NewGameController.instance.isUsingRPC()){
					// A remplir
				}
				else{
					NewGameController.instance.createTile(x,y,isRock);
				}
			}
		}
	}

	public void createTile(int x, int y, bool isRock, GameObject g)
	{
		this.tiles[x,y] = g;
		this.tiles[x,y].name = "Tile "+x+","+y;
		this.getTileController(x,y).setTile(new TileModel(x,y));
		if(isRock){
			this.getTileController(x,y).setRock();
		}
	}

	public TileController getTileController(int x, int y){
		return this.tiles[x,y].GetComponent<TileController>();
	}

	public void resize(float realWidth, int width, int height){
		float scale = Mathf.Min(1f,realWidth/6.05f);
		for (int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				this.getTileController(x,y).setPosition(new Vector3((-width/2f+0.5f)*scale+scale*x,(-height/2f+0.5f)*scale+scale*y,0));
				this.getTileController(x,y).setScale(new Vector3(scale,scale,scale));
				this.getTileController(x,y).show(true);
			}
		}
	}
}
﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tile {
	
	public int x;
	public int y;
	public int distance;
	public NeighbourTiles neighbours ;

	public Tile(int x, int y, int distance){
		this.x = x ;
		this.y = y ;
		this.distance = distance ;
	}

	public Tile(int x, int y){
		this.x = x ;
		this.y = y ;
	}

	public void setNeighbours(int[,] characterTiles, int distance){
		this.neighbours = new NeighbourTiles(this.x, this.y, characterTiles, distance);
	}
}
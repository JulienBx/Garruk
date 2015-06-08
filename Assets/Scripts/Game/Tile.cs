using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tile
{
	
	public int x;
	public int y;
	public int distance;
	public StatModifier StatModifier;
	public NeighbourTiles neighbours;

	public Tile(int x, int y, int distance)
	{
		this.x = x;
		this.y = y;
		this.distance = distance;
	}

	public Tile(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public void setNeighbours(int[,] characterTiles, int distance)
	{
		this.neighbours = new NeighbourTiles(this.x, this.y, characterTiles, distance);
	}

	public List<Tile> getImmediateNeighbouringTiles()
	{
		if (neighbours == null)
		{
			this.neighbours = new NeighbourTiles(this.x, this.y, GameController.instance.getCharacterTilesArray(), distance);
		}
		return neighbours.getImmediateNeighbours(this.x, this.y);
	}
}

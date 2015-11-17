using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tile
{
	
	public int x;
	public int y;
	public int distance;
	
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
	
	public List<Tile> getImmediateNeighbourTiles()
	{
		List<Tile> tempTiles = new List<Tile>();
		int width = GameView.instance.getBoardWidth();
		int height = GameView.instance.getBoardHeight();
		
		if (x > 0)
		{
			tempTiles.Add(new Tile(x - 1, y));
		}
		if (x < width - 1)
		{
			tempTiles.Add(new Tile(x + 1, y));
		}
		if (y > 0)
		{
			tempTiles.Add(new Tile(x, y - 1));
		}
		if (y < height - 1)
		{
			tempTiles.Add(new Tile(x, y + 1));
		}
		
		return tempTiles;
	}
}

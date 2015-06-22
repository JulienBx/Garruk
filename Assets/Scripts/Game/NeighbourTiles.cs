using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NeighbourTiles
{
	public List<Tile> tiles;
	public Dictionary<int, float> UID;
	public int[,] characterTiles;

	public NeighbourTiles(int x, int y, int[,] characterTiles, float moveRemaining)
	{
		int distance = 0;
		tiles = new List<Tile>();
		UID = new Dictionary<int, float>();
		this.characterTiles = characterTiles;
		findWay(new Tile(x, y), moveRemaining, distance);
	}

	void findWay(Tile t, float moveRemaining, int distance)
	{
		if (moveRemaining < 0)
		{
			return;
		}
		int uidtemp = t.y * GameController.instance.boardWidth + t.x;
		if (UID.ContainsKey(uidtemp))
		{
			if (UID [uidtemp] > moveRemaining)
			{
				return;
			} else
			{
				UID [uidtemp] = moveRemaining;
				tiles.Find(e => e.x == t.x && e.y == t.y).distance = distance;
			}
		} else
		{
			UID.Add(uidtemp, moveRemaining);
			tiles.Add(new Tile(t.x, t.y, distance));
		}


		foreach (Tile temp in getImmediateNeighbours(t.x, t.y))
		{
			float newRemaining = moveRemaining;
			if (characterTiles [temp.x, temp.y] < 5)
			{
//				foreach (StatModifier stm in GameController.instance.getTile(temp.x, temp.y).tile.StatModifier)
//				{
//					if (stm.Stat == ModifierStat.Stat_Move && stm.Type == ModifierType.Type_Multiplier)
//					{
//						newRemaining = newRemaining + (stm.Amount) / 100f;
//					}
//				}
//				findWay(temp, (newRemaining - 1), (distance + 1));
			}
		}
	}


	public List<Tile> getImmediateNeighbours(int x, int y)
	{
		List<Tile> tempTiles = new List<Tile>();
		int width = GameController.instance.boardWidth;
		int height = GameController.instance.boardHeight;

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

	public List<Tile> getNeighboursWithin(int distance)
	{
		List<Tile> tempTiles = new List<Tile>();
		for (int i = 0; i < this.tiles.Count; i++)
		{
			if (this.tiles [i].distance <= distance)
			{
				tempTiles.Add(this.tiles [i]);
			}
		}
		return tempTiles;
	}
}

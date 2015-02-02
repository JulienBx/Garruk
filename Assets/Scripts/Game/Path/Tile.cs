using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tile : GridObject 
{
	public Tile(int x, int y) : base (x, y)
	{
	}
	public IEnumerable AllNeighbours { get; set; }
	public static Point[][] NeighbourShift
	{
		get
		{
			return new Point[][]
			{
				new Point[] {
					new Point(1, 1),
					new Point(1, 0),
					new Point(0, -1),
					new Point(-1, 0),
					new Point(-1, 1),
					new Point(0, 1)
				},
				new Point[] {
					new Point(1, 0),
					new Point(1, -1),
					new Point(0, -1),
					new Point(-1, -1),
					new Point(-1, 0),
					new Point(0, 1)
				}
			};
		}
	}
	
	public void FindNeighbours(Dictionary<Point, Tile> Board,
	                           Vector2 BoardSize)
	{   
		List<Tile> neighbours = new List<Tile>();

		Point[] neighboursShift;
		if (X % 2 == 0)
		{
			neighboursShift = NeighbourShift[0];
		} else
		{
			neighboursShift = NeighbourShift[1];
		}
		foreach (Point point in neighboursShift)
		{
			int neighbourX = X + point.X;
			int neighbourY = Y + point.Y;
			//x coordinate offset specific to straight axis coordinates

			if (neighbourX >= 0 &&
			    neighbourX < (int)BoardSize.x &&
			    neighbourY >= 0 && neighbourY < (int)BoardSize.y)
			{
				neighbours.Add(Board[new Point(neighbourX, neighbourY)]);
			}
		}
		
		AllNeighbours = neighbours;
	}
}

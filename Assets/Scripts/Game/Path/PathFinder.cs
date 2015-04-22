using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class PathFinder
{
//	static public Pa//th<Tile> FindPath(
//		Tile start, 
//		Tile destination)
//	{
//		var closed = new HashSet<Tile>();
//		var queue = new PriorityQueue<double, Path<Tile>>();
//		queue.Enqueue(0, new Path<Tile>(start));
//		while (!queue.IsEmpty)
//		{
//			var path = queue.Dequeue();
//			if (closed.Contains(path.LastStep))
//				continue;
//			if (path.LastStep.Equals(destination))
//				return path;
//			closed.Add(path.LastStep);
//			foreach(Tile n in path.LastStep.Neighbours)
//			{
//				double d = distance(path.LastStep, n);
//				var newPath = path.AddStep(n, d);
//				queue.Enqueue(newPath.TotalCost + estimate(n, destination), newPath);
//			}
//		}
//		return null;
//	}

//	static double distance(Tile tile1, Tile tile2)
//	{
//		return 1;
//	}
//
//	static double estimate(Tile a, Tile b)
//	{
		//voir hex_to_cube en dessous
//		int x1 = a.X;
//		int x2 = b.X;
//		int z1 = a.Y - (a.X + (a.X & 1)) / 2;
//		int z2 = b.Y - (b.X + (b.X & 1)) / 2;
//		int	y1 = -x1 - z1;
//		int	y2 = -x2 - z2;
//
//		return Mathf.Max(Mathf.Abs(x1 - x2), Mathf.Abs(y1 - y2), Mathf.Abs(z1 - z2));
//		return 0;
//	}

	/*
	hex_to_cube(q (colonne), r (ligne))
	{
		x = q
		z = r - (q + (q&1)) / 2
		y = -x-z
	}*/


}
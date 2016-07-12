using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileM
{
	public int x;
	public int y;

	public TileM(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public TileM()
	{
		this.x = -1;
		this.y = -1;
	}

	public void randomize(int xmax, int ymax){
		this.x = Random.Range(0, xmax);
		this.y = Random.Range(1, ymax-1);
	}
}

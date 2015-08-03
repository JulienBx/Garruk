using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cup : Competition 
{
	public int NbRounds;
	public int CupPrize;

	public Cup()
	{
		this.texture = Sprite.Create (new Texture2D (1, 1, TextureFormat.ARGB32, false), new Rect (0, 0, 1, 1), new Vector2 (0.5f, 0.5f));
	}
	public Cup(int id, int nbrounds, int cupprize, string name)
	{
		this.Id = id;
		this.NbRounds = nbrounds;
		this.CupPrize = cupprize;
		this.Name = name;
	}
}




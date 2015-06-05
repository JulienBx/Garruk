using UnityEngine;
using System.Collections.Generic;

public class CardViewModel
{
	public Texture[] cardFaces;
	public Texture attackArea;
	public Texture speedArea;
	public Texture moveArea;
	public Texture soldCardTexture;
	public Texture[] lifeLevel;
	public Texture[] attackLevel;
	public Texture[] speedLevel;
	public Texture[] moveLevel;
	public string title;
	public int attack;
	public int life;
	public int move;
	public int speed;
	public string titleClass;
	public Rect centralWindowsRect;
	public Rect collectionPointsWindowsRect;

	public CardViewModel ()
	{
		this.attackArea = new Texture ();
		this.speedArea = new Texture ();
		this.moveArea = new Texture ();
		this.cardFaces = new Texture[6];
		this.lifeLevel = new Texture[6];
		this.attackLevel = new Texture[6];
		this.speedLevel = new Texture[6];
		this.moveLevel = new Texture[6];
		this.centralWindowsRect = new Rect ();
		this.collectionPointsWindowsRect = new Rect ();

	}
}


using UnityEngine;
using System.Collections.Generic;

public class CardGameViewModel
{
	public GUIStyle[] styles;
	public string nextLevel;
	public GUIStyle nextLevelStyle;
	public Rect nextLevelRect;
	
	public CardGameViewModel ()
	{
		this.nextLevelRect=new Rect();
		this.nextLevelStyle = new GUIStyle ();
		this.nextLevel = "+1 Niveau !";
	}
	public void initStyles()
	{
		this.nextLevelStyle = this.styles [0];
	}
	public void resize(float cardHeight)
	{
		this.nextLevelStyle.fontSize = (int)cardHeight * 2 / 100;
	}
}


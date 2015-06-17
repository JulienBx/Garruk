using System;
using UnityEngine;

public class DeckOrderFeaturesViewModel
{

	public GUIStyle[] styles;
	public GUIStyle titleStyle;
	public GUIStyle leftArrowStyle;
	public GUIStyle rightArrowStyle;
	public string deckOrderName;
	public bool buttonsEnabled;
	public bool displayRightArrow;
	public bool displayLeftArrow;
	public bool guiEnabled;
	public Rect leftArrowRect;
	public Rect rightArrowRect;
	public Rect deckOrderNameRect;

	public DeckOrderFeaturesViewModel ()
	{
		this.deckOrderName = "";
		this.titleStyle = new GUIStyle ();
		this.leftArrowStyle = new GUIStyle ();
		this.rightArrowStyle = new GUIStyle ();
		this.displayLeftArrow = false;
		this.displayRightArrow = false;
		this.guiEnabled = true;
		this.buttonsEnabled = true;
	}
	public void initStyles()
	{
		this.leftArrowStyle = this.styles [0];
		this.rightArrowStyle = this.styles [1];
		this.titleStyle = this.styles [2];
	}
	public void resize(float cardHeight)
	{
		this.titleStyle.fontSize = (int)cardHeight * 8 / 100;
	}
}


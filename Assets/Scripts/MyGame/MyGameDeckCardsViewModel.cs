using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MyGameDeckCardsViewModel
{

	public List<int> deckCardsToBeDisplayed;
	public int nbCardsToDisplay;
	public string labelNoDecks;
	public GUIStyle[] styles;
	public GUIStyle labelNoStyle;

	public MyGameDeckCardsViewModel ()
	{
		this.deckCardsToBeDisplayed = new List<int> ();
		this.styles=new GUIStyle[0];
		this.labelNoStyle = new GUIStyle ();
		this.labelNoDecks = "";
	}
	public void initStyles()
	{
		this.labelNoStyle = this.styles [0];
	}
	public void resize(int heightScreen)
	{
		this.labelNoStyle.fontSize = heightScreen * 2 / 100;
	}
}


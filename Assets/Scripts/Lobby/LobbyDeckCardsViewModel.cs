using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class LobbyDeckCardsViewModel
{
	public string noDeckLabel;
	public GUIStyle[] styles;
	public GUIStyle noDeckLabelStyle;

	public LobbyDeckCardsViewModel ()
	{
		this.noDeckLabel = "";
		this.noDeckLabelStyle=new GUIStyle();
	}
	public void initStyles()
	{
		this.noDeckLabelStyle= this.styles [0];
	}
	public void resize(int heightScreen)
	{
		this.noDeckLabelStyle.fontSize = heightScreen * 2 / 100;
	}
}


using System;
using UnityEngine;
public class LobbyPlayersViewModel
{
	public GUIStyle[] styles;

	public GUIStyle labelStyle;
	public string label;
	
	public LobbyPlayersViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.labelStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.labelStyle = this.styles [0];
	}
	public void resize(int heightScreen)
	{
		this.labelStyle.fontSize= heightScreen * 2 / 100;
	}
}


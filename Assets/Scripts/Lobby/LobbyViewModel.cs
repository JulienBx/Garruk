using System;
using UnityEngine;
public class LobbyViewModel
{
	public GUIStyle[] styles;
	public bool displayView;
	public bool guiEnabled;
	public bool isPopUpDisplayed;
	public bool gameButtonsEnabled;
	
	public LobbyViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.displayView = true;
		this.guiEnabled = true;
		this.gameButtonsEnabled = true;
		this.isPopUpDisplayed = false;
	}
	public void initStyles()
	{
	}
	public void resize(int heightScreen)
	{
	}
}

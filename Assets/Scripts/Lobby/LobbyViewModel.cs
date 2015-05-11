using System;
using UnityEngine;
public class LobbyViewModel
{
	public GUIStyle[] styles;
	public bool displayView;
	public bool guiEnabled;
	public bool isPopUpDisplayed;
	
	public LobbyViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.displayView = true;
		this.guiEnabled = true;
		this.isPopUpDisplayed = false;
	}
	public void initStyles()
	{
	}
	public void resize(int heightScreen)
	{
	}
}

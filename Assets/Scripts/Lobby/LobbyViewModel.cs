using System;
using UnityEngine;
public class LobbyViewModel
{
	public GUIStyle[] styles;
	public bool displayView;
	public bool guiEnabled;
	public bool isPopUpDisplayed;
	public bool[] buttonsEnabled;
	
	public LobbyViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.displayView = true;
		this.guiEnabled = true;
		this.isPopUpDisplayed = false;
		this.buttonsEnabled = new bool[4];
		for(int i=0;i<this.buttonsEnabled.Length;i++)
		{
			this.buttonsEnabled[i]=true;
		}
	}
	public void initStyles()
	{
	}
	public void resize(int heightScreen)
	{
	}
}

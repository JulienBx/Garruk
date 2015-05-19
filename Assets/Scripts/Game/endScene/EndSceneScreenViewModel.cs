using System;
using UnityEngine;

public class EndSceneScreenViewModel
{
	public int heightScreen;
	public int widthScreen;
	public Rect centralWindow;
	public Rect mainBlock;
	
	public EndSceneScreenViewModel ()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;	
	}
	public void initStyles()
	{
	}
	public void resize()
	{
		this.heightScreen=Screen.height;
		this.widthScreen=Screen.width;
		this.mainBlock = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.88f * this.heightScreen);
	}
}


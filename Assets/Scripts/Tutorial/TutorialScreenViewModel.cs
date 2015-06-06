using System;
using UnityEngine;

public class TutorialScreenViewModel
{
	public int heightScreen;
	public int widthScreen;
	public Rect centralWindow;
	public Rect mainBlock;
	
	public TutorialScreenViewModel ()
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
		this.mainBlock = new Rect (this.widthScreen * 0.1f, 0.05f * this.heightScreen, this.widthScreen * 0.8f, 0.9f * this.heightScreen);
	}
}


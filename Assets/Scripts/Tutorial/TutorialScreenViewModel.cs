using System;
using UnityEngine;

public class TutorialScreenViewModel
{
	public int heightScreen;
	public int widthScreen;
	public Rect centralWindow;
	public Rect mainBlock;
	public float pictureWidth;
	public float pictureHeight;
	
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
		if(this.mainBlock.height*0.6f*1.77f<this.mainBlock.width)
		{
			this.pictureHeight = this.mainBlock.height * 0.6f;
		}
		else
		{
			this.pictureHeight = this.mainBlock.height * 0.5f;
		}
		this.pictureWidth = this.pictureHeight * 1.77f;
	}
}


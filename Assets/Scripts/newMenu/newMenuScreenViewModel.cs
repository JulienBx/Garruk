using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class newMenuScreenViewModel 
{
	public int heightScreen;
	public int widthScreen; 
	public Rect mainBlock;
	public Rect[] buttonsArea;
	public float buttonHeight;
	public float buttonWidth;
	public float buttonsAreaHeight;
	
	public newMenuScreenViewModel ()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		this.mainBlock = new Rect ();
		this.buttonsArea=new Rect[5];
	}
	public void resize()
	{	
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		this.mainBlock = new Rect (0, 0, Screen.width * 0.2f, Screen.height);
	}
	
}


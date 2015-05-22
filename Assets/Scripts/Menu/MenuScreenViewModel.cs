using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MenuScreenViewModel {

	public int heightScreen=Screen.height;
	public int widthScreen=Screen.width; 

	public MenuScreenViewModel ()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
	}
	public void resize()
	{	
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
	}
	
}


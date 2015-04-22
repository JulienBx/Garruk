using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class UserDataViewModel {

	public GUIStyle[] styles;
	public GUIStyle welcomeStyle;
	public int credits;
	public string username;
	
	public UserDataViewModel ()
	{
		this.styles = new GUIStyle[0];
		this.welcomeStyle = new GUIStyle ();
	}
	public void initStyles()
	{	
		this.welcomeStyle = this.styles[0];
	}
	public void resize(int heightScreen, int widthScreen)
	{	
		this.welcomeStyle.fontSize = heightScreen*20/1000;
		this.welcomeStyle.fixedHeight = (int)heightScreen*3/100;
		this.welcomeStyle.fixedWidth = (int)widthScreen*15/100;
		
	}
	
}


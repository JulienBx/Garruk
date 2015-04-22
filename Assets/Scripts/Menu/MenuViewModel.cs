using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MenuViewModel {

	public GUIStyle[] styles;
	public GUIStyle menuBackgroundStyle;
	public GUIStyle titleStyle;
	public GUIStyle buttonStyle;
	public GUIStyle logOutButtonStyle;
	
	public MenuViewModel ()
	{
		this.styles = new GUIStyle[0];
		this.menuBackgroundStyle = new GUIStyle ();
		this.titleStyle = new GUIStyle ();
		this.buttonStyle = new GUIStyle ();
		this.logOutButtonStyle = new GUIStyle ();
	}
	public void initStyles()
	{	
		this.titleStyle = this.styles[0];
		this.buttonStyle= this.styles[1];
		this.logOutButtonStyle= this.styles[2];
		this.menuBackgroundStyle= this.styles[3];
	}
	public void resize(int heightScreen, int widthScreen)
	{	
		this.titleStyle.fontSize = heightScreen*25/1000;
		this.titleStyle.fixedHeight = (int)heightScreen*6/100;
		this.titleStyle.fixedWidth = (int)widthScreen*15/100;

		this.buttonStyle.fontSize = heightScreen*20/1000;
		this.buttonStyle.fixedHeight = (int)heightScreen*6/100;
		this.buttonStyle.fixedWidth = (int)widthScreen*11/100;

		this.logOutButtonStyle.fontSize = heightScreen*20/1000;
		this.logOutButtonStyle.fixedHeight = (int)heightScreen*3/100;
		this.logOutButtonStyle.fixedWidth = (int)widthScreen*15/100;

		
	}
	
}


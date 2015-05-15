using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class EndGameFriendlyBoardViewModel {

	public GUIStyle[] styles;
	public GUIStyle mainLabelStyle;
	public GUIStyle subMainLabelStyle;
	public string mainLabelText;
	public string subMainLabelText;
	
	public EndGameFriendlyBoardViewModel ()
	{
		this.styles = new GUIStyle[0];
		this.mainLabelStyle = new GUIStyle ();
		this.subMainLabelStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.mainLabelStyle = this.styles [0];
		this.subMainLabelStyle = this.styles [1];
	}
	public void resize(int heightScreen)
	{
		this.mainLabelStyle.fontSize = heightScreen*4 / 100;
		this.subMainLabelStyle.fontSize = heightScreen*3 / 100;
	}
}

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class CupLobbyViewModel
{
	
	public GUIStyle[] styles;
	public GUIStyle paginationStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle buttonStyle;
	
	public CupLobbyViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.paginationStyle = new GUIStyle ();
		this.buttonStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.paginationStyle = this.styles [0];
		this.paginationActivatedStyle = this.styles [1];
		this.buttonStyle = this.styles [2];
	}
	public void resize(int heightScreen)
	{
		this.paginationStyle.fontSize=heightScreen * 2 / 100;
		this.paginationActivatedStyle.fontSize=heightScreen * 2 / 100;
		this.buttonStyle.fontSize = heightScreen * 3 / 100;
	}
}
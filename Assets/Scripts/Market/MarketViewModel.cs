using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MarketViewModel {
	
	public GUIStyle[] styles;
	public GUIStyle paginationStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle sortDefaultButtonStyle;
	public GUIStyle sortActivatedButtonStyle;
	public bool displayView;
	public bool guiEnabled;
	public bool isPopUpDisplayed;
	public bool isBeingDragged;
	
	public MarketViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.displayView = true;
		this.guiEnabled = true;
		this.isPopUpDisplayed = false;
		this.isBeingDragged = false;
	}
	public void initStyles()
	{
		this.paginationStyle = this.styles [0];
		this.paginationActivatedStyle = this.styles [1];
		this.sortDefaultButtonStyle = this.styles[2];
		this.sortActivatedButtonStyle = this.styles[3];
	}
	public void resize(int heightScreen)
	{
		this.paginationStyle.fontSize =heightScreen * 2 / 100;
		this.paginationActivatedStyle.fontSize = heightScreen * 2 / 100;
		this.sortDefaultButtonStyle.fontSize = heightScreen * 2 / 100;
		this.sortActivatedButtonStyle.fontSize = heightScreen * 2 / 100;
	}
}
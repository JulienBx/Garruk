using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyGameViewModel
{
	public GUIStyle paginationStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle sortDefaultButtonStyle;
	public GUIStyle sortActivatedButtonStyle;
	public bool isBeingDragged;
	public bool displayView;
	public bool guiEnabled;
	public bool isPopUpDisplayed;

	public GUIStyle[] styles;

	public MyGameViewModel()
	{
		this.styles=new GUIStyle[0];
		this.displayView = true;
		this.guiEnabled = true;
		this.isPopUpDisplayed = false;
		this.isBeingDragged = false;
	}
	public void initStyles()
	{
		this.paginationStyle = styles [0];
		this.paginationActivatedStyle = styles [1];
		this.sortDefaultButtonStyle = styles [2];
		this.sortActivatedButtonStyle = styles [3];
	}
	public void resize(int heightScreen)
	{
		this.paginationStyle.fontSize =heightScreen * 2 / 100;
		this.paginationActivatedStyle.fontSize = heightScreen * 2 / 100;
		this.sortDefaultButtonStyle.fontSize = heightScreen * 2 / 100;
		this.sortActivatedButtonStyle.fontSize = heightScreen * 2 / 100;
	}
}

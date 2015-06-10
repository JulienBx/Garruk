using System;
using UnityEngine;

public class StoreViewModel
{

	public GUIStyle[] styles;
	public GUIStyle paginationStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle buttonAddCreditsStyle;
	public GUIStyle titleStyle;
	public GUIStyle buttonStyle;
	public bool guiEnabled;
	public bool[] buttonsEnabled;
	public bool isPopUpDisplayed;
	public bool hideGUI;
	public bool areMoreThan1CardDisplayed;
	public bool canAddCredits;
	public string title;

	public StoreViewModel ()
	{
		this.guiEnabled = true;
		this.isPopUpDisplayed = false;
		this.hideGUI = false;
		this.canAddCredits = false;
		this.paginationStyle = new GUIStyle ();
		this.paginationActivatedStyle = new GUIStyle ();
		this.buttonAddCreditsStyle = new GUIStyle ();
		this.buttonStyle = new GUIStyle ();
		this.titleStyle = new GUIStyle ();
		this.buttonsEnabled = new bool[2];
		for(int i=0;i<this.buttonsEnabled.Length;i++)
		{
			this.buttonsEnabled[i]=true;
		}
	}
	public void initStyles()
	{
		this.paginationStyle = this.styles [0];
		this.paginationActivatedStyle = this.styles [1];
		this.buttonAddCreditsStyle = this.styles [2];
		this.titleStyle = this.styles [3];
		this.buttonStyle = this.styles [4];
	}
	public void resize(int heightScreen)
	{
		this.paginationStyle.fontSize = heightScreen * 2 / 100;
		this.paginationActivatedStyle.fontSize = heightScreen * 2 / 100;
		this.buttonAddCreditsStyle.fontSize = heightScreen * 3 / 100;
		this.titleStyle.fontSize = heightScreen * 3 / 100;
	}
	
}

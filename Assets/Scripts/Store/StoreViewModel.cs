using System;
using UnityEngine;

public class StoreViewModel
{

	public GUIStyle[] styles;
	public GUIStyle buttonStyle;
	public bool guiEnabled;
	public bool isPopUpDisplayed;
	public bool isCardZoomed;
	public int creationCost;

	public StoreViewModel ()
	{
		guiEnabled = true;
		isPopUpDisplayed = false;
		isCardZoomed = false;
	}
	public void initStyles()
	{
		this.buttonStyle = this.styles [0];
	}
	public void resize(int heightSreen)
	{
		this.buttonStyle.fontSize = heightSreen * 2 / 100;
	}
	
}

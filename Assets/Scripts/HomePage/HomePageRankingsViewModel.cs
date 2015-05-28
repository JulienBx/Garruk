using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageRankingsViewModel 
{
	
	public int totalNbWins;
	public int totalNbLooses;
	public int division;
	public int ranking;
	public int rankingPoints;
	public GUIStyle[] styles;
	public GUIStyle informationsStyle;
	public GUIStyle buttonStyle;
	
	//public GUIStyle[] styles;
	
	public HomePageRankingsViewModel ()
	{
		this.informationsStyle = new GUIStyle ();
		this.buttonStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.informationsStyle = this.styles [0];
		this.buttonStyle = this.styles [1];
	}
	public void resize(int heightScreen)
	{
		this.informationsStyle.fontSize = heightScreen*2/100;
		this.buttonStyle.fontSize = heightScreen * 2 / 100;
	}
}
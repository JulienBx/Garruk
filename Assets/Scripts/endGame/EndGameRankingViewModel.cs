using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class EndGameRankingViewModel 
{
	public string title;
	public int rankingPoints;
	public int ranking;

	public GUIStyle[] styles;
	public GUIStyle titleStyle;
	public GUIStyle rankingStyle;
	public GUIStyle rankingPointsStyle;
	
	public EndGameRankingViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.titleStyle=new GUIStyle();
		this.rankingStyle = new GUIStyle ();
		this.rankingPointsStyle = new GUIStyle ();
		this.title = "Votre classement";
	}
	public void initStyles()
	{
		this.titleStyle = this.styles [0];
		this.rankingStyle = this.styles [1];
		this.rankingPointsStyle = this.styles [2];
	}
	public void resize(int heightScreen)
	{
		this.titleStyle.fontSize = heightScreen * 2 / 100;
		this.rankingStyle.fontSize = heightScreen * 25 / 1000;
		this.rankingPointsStyle.fontSize = heightScreen * 2 / 100;
	}

}

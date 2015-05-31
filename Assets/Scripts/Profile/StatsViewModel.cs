using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class StatsViewModel {

	public int totalNbWins;
	public int totalNbLooses;
	public int division;
	public int ranking;
	public int rankingPoints;
	public GUIStyle[] styles;
	public GUIStyle informationsStyle;
	public GUIStyle buttonStyle;

	//public GUIStyle[] styles;
	
	public StatsViewModel (){
	}
	public StatsViewModel (int totalnbwins, int totalnblooses, int division, int ranking, int rankingpoints){
		this.totalNbWins = totalnbwins;
		this.totalNbLooses = totalnblooses;
		this.division = division;
		this.ranking = ranking;
		this.rankingPoints = rankingpoints;
	}
	public void initStyles()
	{
		this.informationsStyle = this.styles [0];
		this.buttonStyle = this.styles [1];
	}
	public void resize(int heightScreen)
	{
		this.informationsStyle.fontSize = heightScreen*25/1000;
	}
	
}

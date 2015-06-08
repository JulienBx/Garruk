using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class StatsViewModel {

	public int totalNbWins;
	public int totalNbLooses;
	public int division;
	public string ranking;
	public string collectionRanking;
	public string rankingPoints;
	public string collectionPoints;
	public GUIStyle[] styles;
	public GUIStyle informationsStyle;
	public GUIStyle buttonStyle;
	public GUIStyle subInformationsStyle;

	//public GUIStyle[] styles;
	
	public StatsViewModel ()
	{
	}
	public StatsViewModel (int totalnbwins, int totalnblooses, int division, int ranking, int rankingPoints, int collectionPoints, int collectionRanking)
	{
		this.totalNbWins = totalnbwins;
		this.totalNbLooses = totalnblooses;
		this.division = division;
		if(ranking!=0)
		{
			this.ranking="Classement : " + ranking.ToString();
			this.rankingPoints="("+ rankingPoints.ToString() + " pts)";
		}
		else
		{
			this.ranking="";
			this.rankingPoints="";
		}
		if(collectionRanking!=0)
		{
			this.collectionRanking="Class collection : " + collectionRanking.ToString();
			this.collectionPoints="("+ collectionRanking.ToString() + " pts)";
		}
		else
		{
			this.collectionRanking="";
			this.collectionPoints="";
		}
	}
	public void initStyles()
	{
		this.informationsStyle = this.styles [0];
		this.buttonStyle = this.styles [1];
		this.subInformationsStyle = this.styles [2];
	}
	public void resize(int heightScreen)
	{
		this.informationsStyle.fontSize = heightScreen*25/1000;
		this.subInformationsStyle.fontSize = heightScreen * 2 / 100;
	}
	
}

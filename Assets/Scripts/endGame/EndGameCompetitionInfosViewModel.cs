using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class EndGameCompetitionInfosViewModel
{
	
	public GUIStyle[] styles;
	public GUIStyle competitionPictureStyle;
	public GUIStyle titleStyle;
	public GUIStyle informationsStyle;
	public string title;
	public int nbGames;
	public int titlePrize;
	public int promotionPrize;
	public int nbRounds;
	public int cupPrize;
	
	public EndGameCompetitionInfosViewModel ()
	{
		this.competitionPictureStyle = new GUIStyle ();
		this.titleStyle = new GUIStyle ();
		this.informationsStyle = new GUIStyle ();
		this.title = "Informations sur la compétition";
	}
	public void initStyles()
	{
		this.competitionPictureStyle = this.styles [0];
		this.titleStyle = this.styles [1];
		this.informationsStyle = this.styles[2];
	}
	public void resize(int heightScreen)
	{
		this.titleStyle.fontSize = heightScreen * 2 / 100;
		this.informationsStyle.fontSize = heightScreen * 2 / 100;
	}
}
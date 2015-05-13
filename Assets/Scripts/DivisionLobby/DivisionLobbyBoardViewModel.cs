using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class DivisionLobbyBoardViewModel
{
	public int nbWinsDivision;
	public int nbLoosesDivision;
	public int remainingGames;

	public int nbWinsForPromotion;
	public int nbWinsForTitle;
	public int nbWinsForRelegation;
	public int titlePrize;
	public int promotionPrize;
	public string divisionName;

	public GUIStyle[] styles;
	public GUIStyle activeGaugeBackgroundStyle;
	public GUIStyle gaugeBackgroundStyle;
	public GUIStyle startActiveGaugeBackgroundStyle;
	public GUIStyle relegationBarStyle;
	public GUIStyle promotionBarStyle;
	public GUIStyle titleBarStyle;
	public GUIStyle divisionLabelStyle;
	public GUIStyle divisionStrikeLabelStyle;
	public GUIStyle remainingGamesStyle;
	public GUIStyle promotionPrizeLabelStyle;
	public GUIStyle titlePrizeLabelStyle;
	public GUIStyle relegationLabelStyle;
	public GUIStyle promotionLabelStyle;
	public GUIStyle titleLabelStyle;
	public GUIStyle relegationValueLabelStyle;
	public GUIStyle promotionValueLabelStyle;
	public GUIStyle titleValueLabelStyle;

	public Texture2D[] gaugeBackgrounds;

	public float gaugeSpace1;
	public float gaugeSpace2;
	public float gaugeSpace3;
	public float gaugeSpace4;
	public float gaugeSpace1Width;
	public float gaugeSpace2Width;
	public float gaugeSpace3Width;
	public float gaugeSpace4Width;
	public float gaugeWidth;
	public float gaugeHeight;
	public float startActiveGaugeWidth;
	public float activeGaugeWidth;
	public float relegationBarWidth;
	public float promotionBarWidth;
	public float titleBarWidth;
	
	public DivisionLobbyBoardViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.activeGaugeBackgroundStyle=new GUIStyle();
		this.gaugeBackgroundStyle=new GUIStyle();
		this.startActiveGaugeBackgroundStyle=new GUIStyle();
		this.relegationBarStyle=new GUIStyle();
		this.promotionBarStyle=new GUIStyle();
		this.titleBarStyle=new GUIStyle();
		this.divisionLabelStyle=new GUIStyle();
		this.divisionStrikeLabelStyle=new GUIStyle();
		this.remainingGamesStyle=new GUIStyle();
		this.promotionPrizeLabelStyle=new GUIStyle();
		this.titlePrizeLabelStyle=new GUIStyle();
		this.relegationLabelStyle=new GUIStyle();
		this.promotionLabelStyle=new GUIStyle();
		this.titleLabelStyle=new GUIStyle();
		this.relegationValueLabelStyle=new GUIStyle();
		this.promotionValueLabelStyle=new GUIStyle();
		this.titleValueLabelStyle=new GUIStyle();
		this.gaugeBackgrounds=new Texture2D[0];
		this.gaugeSpace1=0f;
		this.gaugeSpace2=0f;
		this.gaugeSpace3=0f;
		this.gaugeSpace4=0.1f;
		this.gaugeSpace1Width=0;
		this.gaugeSpace2Width=0;
		this.gaugeSpace3Width=0;
		this.gaugeSpace4Width=0;
		this.gaugeWidth=0;
		this.gaugeHeight=0;
		this.startActiveGaugeWidth=0.2f;
		this.activeGaugeWidth=0f;
		this.relegationBarWidth=0f;
		this.promotionBarWidth=0f;
		this.titleBarWidth=0.005f;
	}
	public void initStyles()
	{
		this.activeGaugeBackgroundStyle=this.styles[0];
		this.gaugeBackgroundStyle=this.styles[1];
		this.startActiveGaugeBackgroundStyle=this.styles[2];
		this.relegationBarStyle=this.styles[3];
		this.promotionBarStyle = this.styles [4];
		this.titleBarStyle=this.styles[5];
		this.divisionLabelStyle=this.styles[6];
		this.divisionStrikeLabelStyle=this.styles[7];
		this.remainingGamesStyle=this.styles[8];
		this.promotionPrizeLabelStyle=this.styles[9];
		this.titlePrizeLabelStyle=this.styles[10];
		this.relegationLabelStyle=this.styles[11];
		this.promotionLabelStyle=this.styles[12];
		this.titleLabelStyle=this.styles[13];
		this.relegationValueLabelStyle=this.styles[14];
		this.promotionValueLabelStyle=this.styles[15];
		this.titleValueLabelStyle=this.styles[16];
	}
	public void resize(int heightScreen)
	{
		this.divisionLabelStyle.fontSize = heightScreen * 3 / 100;
		this.divisionStrikeLabelStyle.fontSize= heightScreen * 2 / 100;
		this.remainingGamesStyle.fontSize= heightScreen * 2 / 100;
		this.promotionPrizeLabelStyle.fontSize= heightScreen * 2 / 100;
		this.titlePrizeLabelStyle.fontSize= heightScreen * 2 / 100;
		this.relegationLabelStyle.fontSize = heightScreen * 2 / 100;
		this.promotionLabelStyle.fontSize = heightScreen * 2 / 100;
		this.titleLabelStyle.fontSize =heightScreen * 2 / 100;
		this.relegationValueLabelStyle.fontSize =heightScreen * 2 / 100;
		this.promotionValueLabelStyle.fontSize = heightScreen * 2 / 100;
		this.titleValueLabelStyle.fontSize = heightScreen * 2 / 100;
		this.startActiveGaugeBackgroundStyle.fontSize = heightScreen * 10 / 100;
	}
}
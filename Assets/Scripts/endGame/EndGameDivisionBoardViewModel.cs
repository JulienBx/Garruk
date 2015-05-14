using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameDivisionBoardViewModel 
{

	public int nbWinsForPromotion;
	public int nbWinsForRelegation;
	public int nbWinsForTitle;
	public int titlePrize;
	public int promotionPrize;
	public int id;
	public string divisionName;

	public int hasWon;
	public int nbWinsDivision;
	public int nbLoosesDivision;
	public int remainingGames;
	public bool title;
	public bool promotion;
	public bool endSeason;
	public bool relegation;

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

	public float gaugeSpace1=0f;
	public float gaugeSpace1Start=0f;
	public float gaugeSpace1Finish=0f;
	public float gaugeSpace2=0f;
	public float gaugeSpace2Start=0f;
	public float gaugeSpace2Finish=0f;
	public float gaugeSpace3=0f;
	public float gaugeSpace3Start=0f;
	public float gaugeSpace3Finish=0f;
	public float gaugeSpace4=0.1f;
	public float gaugeSpace1Width;
	public float gaugeSpace2Width;
	public float gaugeSpace3Width;
	public float gaugeSpace4Width;
	public float gaugeWidth;
	public float gaugeHeight;
	public float startActiveGaugeWidth=0.2f;
	public float activeGaugeWidth=0f;
	public float activeGaugeWidthStart=0f;
	public float activeGaugeWidthFinish=0f;
	public float relegationBarWidth=0f;
	public float relegationBarFinish=0f;
	public float promotionBarWidth=0f;
	public float promotionBarFinish=0f;
	public float titleBarWidth=0.005f; 
	public float titleBarFinish=0.005f;
	public float transformRatio=0f;
	public float transformSpeed=0.5f;
	
	public EndGameDivisionBoardViewModel ()
	{
		this.gaugeSpace1=0f;
		this.gaugeSpace1Start=0f;
		this.gaugeSpace1Finish=0f;
		this.gaugeSpace2=0f;
		this.gaugeSpace2Start=0f;
		this.gaugeSpace2Finish=0f;
		this.gaugeSpace3=0f;
		this.gaugeSpace3Start=0f;
		this.gaugeSpace3Finish=0f;
		this.gaugeSpace4=0.1f;
		this.startActiveGaugeWidth=0.2f;
		this.activeGaugeWidth=0f;
		this.activeGaugeWidthStart=0f;
		this.activeGaugeWidthFinish=0f;
		this.relegationBarWidth=0f;
		this.relegationBarFinish=0f;
		this.promotionBarWidth=0f;
		this.promotionBarFinish=0f;
		this.titleBarWidth=0.005f; 
		this.titleBarFinish=0.005f;
		this.transformRatio=0f;
		this.transformSpeed=0.5f;
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

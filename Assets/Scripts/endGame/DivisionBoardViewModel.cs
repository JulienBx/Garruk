using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DivisionBoardViewModel {

	public DivisionSkill division;
	public int hasWon;
	public int nbWinsDivision;
	public int nbLoosesDivision;
	public int remainingGames;
	public bool title;
	public bool promotion;
	public bool endSeason;
	public bool relegation;

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


	
	public DivisionBoardViewModel (DivisionSkill division,
	                               Texture2D[] gaugebackground){

		this.division = division;
		this.gaugeBackgrounds = gaugebackground;
	}

	public DivisionBoardViewModel (){

	}

	public void initializeGauge(){
		
		if(this.nbWinsDivision-this.hasWon>=this.division.NbWinsForPromotion || 
		   (this.division.NbWinsForPromotion==-1 && this.nbWinsDivision-this.hasWon>=this.division.NbWinsForRelegation))
		{
			this.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[2];
			if(this.nbWinsDivision==this.division.NbWinsForTitle)
			{
				this.titleBarFinish=0f;
			}
			float tempFloat = 1f-(this.startActiveGaugeWidth+this.gaugeSpace4);
			this.activeGaugeWidthStart=tempFloat*(((float)this.nbWinsDivision-(float)this.hasWon)/(float)this.division.NbWinsForTitle);
			this.activeGaugeWidth=this.activeGaugeWidthStart;
			this.activeGaugeWidthFinish=tempFloat*(((float)this.nbWinsDivision)/(float)this.division.NbWinsForTitle);
			
			this.gaugeSpace3Start=(1f-((float)this.nbWinsDivision-(float)this.hasWon)/(float)this.division.NbWinsForTitle)*tempFloat;
			this.gaugeSpace3=this.gaugeSpace3Start;
			this.gaugeSpace3Finish=(1f-((float)this.nbWinsDivision)/(float)this.division.NbWinsForTitle)*tempFloat;
		}
		else if(this.nbWinsDivision-this.hasWon>=this.division.NbWinsForRelegation && this.division.NbWinsForPromotion!=-1)
		{
			this.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[1];
			this.promotionBarWidth=0.005f;
			this.gaugeSpace3=(1f-(this.gaugeSpace4+this.startActiveGaugeWidth+this.promotionBarWidth+this.titleBarWidth))*((float)this.division.NbWinsForTitle-(float)this.division.NbWinsForPromotion)/(float)this.division.NbWinsForTitle;
			if(this.nbWinsDivision==this.division.NbWinsForPromotion)
			{
				this.promotionBarFinish=0f;
			}
			else
			{	
				this.promotionBarFinish=this.promotionBarWidth;
			}
			float tempFloat = 1f-(this.startActiveGaugeWidth+this.gaugeSpace4+this.gaugeSpace3+this.titleBarWidth);
			this.activeGaugeWidthStart=tempFloat*(((float)this.nbWinsDivision-(float)this.hasWon)/(float)this.division.NbWinsForPromotion);
			this.activeGaugeWidth=this.activeGaugeWidthStart;
			this.activeGaugeWidthFinish=tempFloat*(((float)this.nbWinsDivision)/(float)this.division.NbWinsForPromotion);
			
			this.gaugeSpace2Start=(1f-((float)this.nbWinsDivision-(float)this.hasWon)/(float)this.division.NbWinsForPromotion)*tempFloat;
			this.gaugeSpace2=this.gaugeSpace2Start;
			this.gaugeSpace2Finish=(1f-((float)this.nbWinsDivision)/(float)this.division.NbWinsForPromotion)*tempFloat;
		}
		else
		{
			this.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[0];
			if(this.division.NbWinsForPromotion!=-1){
				this.promotionBarWidth=0.005f;
				this.promotionBarFinish=this.promotionBarWidth;
			}
			this.relegationBarWidth=0.005f;
			this.gaugeSpace3=(1f-(this.gaugeSpace4+this.startActiveGaugeWidth+this.promotionBarWidth+this.titleBarWidth))*((float)this.division.NbWinsForTitle-(float)this.division.NbWinsForPromotion)/(float)this.division.NbWinsForTitle;
			if(this.division.NbWinsForPromotion!=-1){
				this.gaugeSpace2=(1f-(this.gaugeSpace4+this.gaugeSpace3+this.startActiveGaugeWidth+this.promotionBarWidth+this.titleBarWidth))*((float)this.division.NbWinsForPromotion-(float)this.division.NbWinsForRelegation)/(float)this.division.NbWinsForPromotion;
			}
			if(this.nbWinsDivision==this.division.NbWinsForRelegation)
			{
				this.relegationBarFinish=0f;
			}
			else
			{
				this.relegationBarFinish=this.relegationBarWidth;
			}
			float tempFloat = 1f-(this.startActiveGaugeWidth+this.gaugeSpace4+this.gaugeSpace3+this.gaugeSpace2+this.titleBarWidth+this.promotionBarWidth);
			this.activeGaugeWidthStart=tempFloat*(((float)this.nbWinsDivision-(float)this.hasWon)/(float)this.division.NbWinsForRelegation);
			this.activeGaugeWidth=this.activeGaugeWidthStart;
			this.activeGaugeWidthFinish=tempFloat*(((float)this.nbWinsDivision)/(float)this.division.NbWinsForRelegation);
			
			this.gaugeSpace1Start=(1f-((float)this.nbWinsDivision-(float)this.hasWon)/(float)this.division.NbWinsForRelegation)*tempFloat;
			this.gaugeSpace1=this.gaugeSpace1Start;
			this.gaugeSpace1Finish=(1f-((float)this.nbWinsDivision)/(float)this.division.NbWinsForRelegation)*tempFloat;
		}
	}
	public void drawGauge(){

		this.startActiveGaugeBackgroundStyle.fixedWidth = this.startActiveGaugeWidth*this.gaugeWidth;
		this.activeGaugeBackgroundStyle.fixedWidth = this.activeGaugeWidth*this.gaugeWidth;
		this.relegationBarStyle.fixedWidth = this.relegationBarWidth*this.gaugeWidth;
		this.promotionBarStyle.fixedWidth = this.promotionBarWidth*this.gaugeWidth;
		this.titleBarStyle.fixedWidth = this.titleBarWidth*this.gaugeWidth;
		this.gaugeSpace1Width=this.gaugeSpace1*this.gaugeWidth;
		this.gaugeSpace2Width=this.gaugeSpace2*this.gaugeWidth;
		this.gaugeSpace3Width=this.gaugeSpace3*this.gaugeWidth;
		this.gaugeSpace4Width=this.gaugeSpace4*this.gaugeWidth;

	}
}

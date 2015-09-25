//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//
//public class EndGameController : MonoBehaviour 
//{
//
//	public GameObject MenuObject;
//	public GameObject TutorialObject;
//	private EndGameView view;
//	public static EndGameController instance;
//
//	public int displayPopUpDelay;
//	public Texture2D[] gaugeBackgrounds;
//	public string[] roundsName;
//
//	public GUIStyle[] competInfosVMStyle;
//	public GUIStyle[] cupBoardVMStyle;
//	public GUIStyle[] divisionBoardVMStyle;
//	public GUIStyle[] friendlyBoardVMStyle;
//	public GUIStyle[] opponentVMStyle;
//	public GUIStyle[] rankingVMStyle;
//	public GUIStyle[] resultsVMStyle;
//	public GUIStyle[] screenVMStyle;
//	public GUIStyle[] endGameVMStyle;
//
//	private EndGameModel model;
//
//	private bool toUpdateGauge=false;
//	private bool toStartTimer = false;
//	private bool toUpdateUserResults = false;
//
//	private float transformRatio=0f;
//	private float transformSpeed=0.5f;
//
//	private float timer;
//	private bool isTutorialLaunched;
//	private GameObject tutorial;
//
//	// Use this for initialization
//	void Start () 
//	{
//		instance = this;
//		this.model = new EndGameModel ();
//		this.view = Camera.main.gameObject.AddComponent <EndGameView>();
//		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
//		StartCoroutine (this.initialization ());
//	}
//	// Update is called once per frame
//	void Update () 
//	{
//		if (this.toStartTimer)
//		{
//			this.timer += Time.deltaTime;
//			if (this.timer > this.displayPopUpDelay) {
//				this.toStartTimer=false;
//				if(view.cupBoardVM.winCup)
//				{
//					view.setWinCupPopUp(true);
//
//				}
//				else if(view.cupBoardVM.endCup)
//				{
//					view.setEndCupPopUp(true);
//				}
//			}
//		}
//		if (this.toUpdateGauge)
//		{
//			this.transformRatio = this.transformRatio + this.transformSpeed * Time.deltaTime;
//			this.computeGauge();
//			this.drawGauge ();
//		}
//	}
//	private IEnumerator initialization()
//	{
//		yield return StartCoroutine (model.getEndGameData ());
//		this.initStyles ();
//		this.initVMs ();
//		switch(model.lastResults[0].GameType)
//		{
//		case 0:
//			this.resize();
//			if(model.currentUser.TutorialStep==5)
//			{
//				this.tutorial = Instantiate(this.TutorialObject) as GameObject;
//				MenuObject.GetComponent<MenuController>().setTutorialLaunched(true);
//				this.tutorial.AddComponent<EndGameTutorialController>();
//				this.tutorial.GetComponent<EndGameTutorialController>().launchSequence(0);
//				this.isTutorialLaunched=true;
//			}
//			break;
//		case 1:
//			this.initializeGauge();
//			this.resize();
//			this.toUpdateGauge = true;
//			break;
//		case 2:
//			this.initializeRounds();
//			this.resize();
//			if(view.cupBoardVM.winCup || view.cupBoardVM.endCup)
//			{
//				this.toStartTimer = true;
//			}
//			break;
//		}
//	}
//	private void initVMs()
//	{
//		view.endGameRankingVM.rankingPoints = model.currentUser.RankingPoints;
//		view.endGameRankingVM.ranking = model.currentUser.Ranking;
//		view.endGameVM.gameType= model.lastResults [0].GameType;
//
//		switch(view.endGameVM.gameType)
//		{
//		case 0:
//			view.endGameResultsVM.resultsTitle="Vos derniers résultats en match amical";
//			if(model.lastResults[0].HasWon)
//			{
//				view.friendlyBoardVM.mainLabelText="BRAVO !";
//				view.friendlyBoardVM.subMainLabelText="Venez en match officiel vous mesurer aux meilleurs joueurs !";
//			}
//			else
//			{
//				view.friendlyBoardVM.mainLabelText="DOMMAGE !";
//				view.friendlyBoardVM.subMainLabelText="C'est en s'entrainant qu'on progresse ! Courage !";
//			}
//			break;
//		case 1:
//			view.endGameResultsVM.resultsTitle="Vos résultats de division";
//			view.divisionBoardVM.nbWinsForTitle=model.currentDivision.NbWinsForTitle;
//			view.divisionBoardVM.nbWinsForPromotion=model.currentDivision.NbWinsForPromotion;
//			view.divisionBoardVM.nbWinsForRelegation=model.currentDivision.NbWinsForRelegation;
//			view.divisionBoardVM.divisionName=model.currentDivision.Name;
//			view.divisionBoardVM.id=model.currentDivision.Id;
//			view.divisionBoardVM.titlePrize=model.currentDivision.TitlePrize;
//			view.divisionBoardVM.promotionPrize=model.currentDivision.PromotionPrize;
//			view.divisionBoardVM.nbWinsDivision=model.nbWinsDivision;
//			view.divisionBoardVM.nbLoosesDivision=model.nbLoosesDivision;
//			view.divisionBoardVM.remainingGames=model.currentDivision.NbGames-model.lastResults.Count;
//			view.divisionBoardVM.hasWon=System.Convert.ToInt32(model.lastResults[0].HasWon);
//			switch(model.competitionState)
//			{
//			case 3:
//				view.divisionBoardVM.title=true;
//				break;
//			case 2:
//				view.divisionBoardVM.promotion=true;
//				break;
//			case 1:
//				view.divisionBoardVM.endSeason=true;
//				break;
//			case 0:
//				view.divisionBoardVM.relegation=true;
//				break;
//			}
//			view.endGameCompetInfosVM.nbGames = model.currentDivision.NbGames;
//			view.endGameCompetInfosVM.titlePrize = model.currentDivision.TitlePrize;
//			view.endGameCompetInfosVM.promotionPrize = model.currentDivision.PromotionPrize;
//			//view.endGameCompetInfosVM.competitionPictureStyle.normal.background = model.currentDivision.texture;
//			StartCoroutine (model.currentDivision.setPicture ());
//			break;
//		case 2:
//			view.endGameResultsVM.resultsTitle="Vos résultats de coupe";
//			view.cupBoardVM.name = model.currentCup.Name;
//			view.cupBoardVM.nbRounds = model.currentCup.NbRounds;
//			view.cupBoardVM.cupPrize=model.currentCup.CupPrize;
//			switch(model.competitionState)
//			{
//			case 1:
//				view.cupBoardVM.winCup=true;
//				break;
//			case 0:
//				view.cupBoardVM.endCup=true;
//				break;
//			}
//			view.endGameCompetInfosVM.nbRounds = model.currentCup.NbRounds;
//			view.endGameCompetInfosVM.cupPrize = model.currentCup.CupPrize;
//			//view.endGameCompetInfosVM.competitionPictureStyle.normal.background = model.currentCup.texture;
//			StartCoroutine (model.currentCup.setPicture ());
//			break;
//		}
//		for(int i=0;i<model.lastResults.Count;i++)
//		{
//			view.endGameResultsVM.resultsStyles.Add (new GUIStyle());
//			view.endGameResultsVM.focusButtonStyles.Add(new GUIStyle());
//			view.endGameResultsVM.resultsLabel.Add (model.lastResults[i].Date.ToString("dd/MM/yyyy"));
//			if(i==0)
//			{
//				view.endGameResultsVM.focusButtonStyles[i]=view.endGameResultsVM.selectedFocusButtonStyle;
//			}
//			else
//			{
//				view.endGameResultsVM.focusButtonStyles[i]=view.endGameResultsVM.focusButtonStyle;
//			}
//			if(model.lastResults[i].HasWon)
//			{
//				view.endGameResultsVM.resultsStyles[i]=view.endGameResultsVM.wonLabelStyle;
//				view.endGameResultsVM.resultsLabel[i]=view.endGameResultsVM.resultsLabel[i]+" Victoire";
//			}
//			else
//			{
//				view.endGameResultsVM.resultsStyles[i]=view.endGameResultsVM.defeatLabelStyle;
//				view.endGameResultsVM.resultsLabel[i]=view.endGameResultsVM.resultsLabel[i]+" Défaite";
//			}
//		}
//		if(model.lastResults.Count>0)
//		{
//			view.endGameOpponentVM.username = model.lastResults [0].Opponent.Username;
//			view.endGameOpponentVM.totalNbWins = model.lastResults [0].Opponent.TotalNbWins;
//			view.endGameOpponentVM.totalNbLooses = model.lastResults [0].Opponent.TotalNbLooses;
//			view.endGameOpponentVM.ranking = model.lastResults [0].Opponent.Ranking;
//			view.endGameOpponentVM.rankingPoints = model.lastResults [0].Opponent.RankingPoints;
//			view.endGameOpponentVM.division = model.lastResults [0].Opponent.Division;
//			//view.endGameOpponentVM.profilePictureStyle.normal.background = model.lastResults [0].Opponent.texture;
//			StartCoroutine (model.lastResults [0].Opponent.setProfilePicture ());
//		}
//	}
//	public void displayOpponent(int chosenOpponent)
//	{
//		view.endGameOpponentVM.username = model.lastResults [chosenOpponent].Opponent.Username;
//		view.endGameOpponentVM.totalNbWins = model.lastResults [chosenOpponent].Opponent.TotalNbWins;
//		view.endGameOpponentVM.totalNbLooses = model.lastResults [chosenOpponent].Opponent.TotalNbLooses;
//		view.endGameOpponentVM.ranking = model.lastResults [chosenOpponent].Opponent.Ranking;
//		view.endGameOpponentVM.rankingPoints = model.lastResults [chosenOpponent].Opponent.RankingPoints;
//		view.endGameOpponentVM.division = model.lastResults [chosenOpponent].Opponent.Division;
//		//view.endGameOpponentVM.profilePictureStyle.normal.background = model.lastResults [chosenOpponent].Opponent.texture;
//		StartCoroutine (model.lastResults [chosenOpponent].Opponent.setProfilePicture ());
//		view.endGameResultsVM.focusButtonStyles[chosenOpponent]=view.endGameResultsVM.selectedFocusButtonStyle;
//		view.endGameResultsVM.focusButtonStyles[view.endGameResultsVM.chosenResult]=view.endGameResultsVM.focusButtonStyle;
//		view.endGameResultsVM.chosenResult = chosenOpponent;
//	}
//	public void resize()
//	{
//		view.endGameScreenVM.resize ();
//		view.endGameOpponentVM.resize (view.endGameScreenVM.heightScreen);
//		view.endGameRankingVM.resize (view.endGameScreenVM.heightScreen);
//		view.endGameResultsVM.resize (view.endGameScreenVM.heightScreen);
//		view.endGameVM.resize (view.endGameScreenVM.heightScreen);
//		switch(model.lastResults[0].GameType)
//		{
//		case 0:
//			view.friendlyBoardVM.resize (view.endGameScreenVM.heightScreen);
//			if(isTutorialLaunched)
//			{
//				this.tutorial.GetComponent<EndGameTutorialController>().resize();
//			}
//			break;
//		case 1:
//			view.divisionBoardVM.resize (view.endGameScreenVM.heightScreen);
//			view.endGameCompetInfosVM.resize (view.endGameScreenVM.heightScreen);
//			this.resizeGauge();
//			this.drawGauge();
//			break;
//		case 2:
//			view.cupBoardVM.resize (view.endGameScreenVM.heightScreen);
//			view.endGameCompetInfosVM.resize (view.endGameScreenVM.heightScreen);
//			this.resizeRounds();
//			break;
//		}
//	}
//	public void initializeGauge(){
//		
//		if(view.divisionBoardVM.nbWinsDivision-view.divisionBoardVM.hasWon>=view.divisionBoardVM.nbWinsForPromotion || 
//		   (view.divisionBoardVM.nbWinsForPromotion==-1 && view.divisionBoardVM.nbWinsDivision-view.divisionBoardVM.hasWon>=view.divisionBoardVM.nbWinsForRelegation))
//		{
//			view.divisionBoardVM.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[2];
//			if(view.divisionBoardVM.nbWinsDivision==view.divisionBoardVM.nbWinsForTitle)
//			{
//				view.divisionBoardVM.titleBarFinish=0f;
//			}
//			float tempFloat = 1f-(view.divisionBoardVM.startActiveGaugeWidth+view.divisionBoardVM.gaugeSpace4);
//			view.divisionBoardVM.activeGaugeWidthStart=tempFloat*(((float)view.divisionBoardVM.nbWinsDivision-(float)view.divisionBoardVM.hasWon)/(float)view.divisionBoardVM.nbWinsForTitle);
//			view.divisionBoardVM.activeGaugeWidth=view.divisionBoardVM.activeGaugeWidthStart;
//			view.divisionBoardVM.activeGaugeWidthFinish=tempFloat*(((float)view.divisionBoardVM.nbWinsDivision)/(float)view.divisionBoardVM.nbWinsForTitle);
//			
//			view.divisionBoardVM.gaugeSpace3Start=(1f-((float)view.divisionBoardVM.nbWinsDivision-(float)view.divisionBoardVM.hasWon)/(float)view.divisionBoardVM.nbWinsForTitle)*tempFloat;
//			view.divisionBoardVM.gaugeSpace3=view.divisionBoardVM.gaugeSpace3Start;
//			view.divisionBoardVM.gaugeSpace3Finish=(1f-((float)view.divisionBoardVM.nbWinsDivision)/(float)view.divisionBoardVM.nbWinsForTitle)*tempFloat;
//		}
//		else if(view.divisionBoardVM.nbWinsDivision-view.divisionBoardVM.hasWon>=view.divisionBoardVM.nbWinsForRelegation && view.divisionBoardVM.nbWinsForPromotion!=-1)
//		{
//			view.divisionBoardVM.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[1];
//			view.divisionBoardVM.promotionBarWidth=0.005f;
//			view.divisionBoardVM.gaugeSpace3=(1f-(view.divisionBoardVM.gaugeSpace4+view.divisionBoardVM.startActiveGaugeWidth+view.divisionBoardVM.promotionBarWidth+view.divisionBoardVM.titleBarWidth))*((float)view.divisionBoardVM.nbWinsForTitle-(float)view.divisionBoardVM.nbWinsForPromotion)/(float)view.divisionBoardVM.nbWinsForTitle;
//			if(view.divisionBoardVM.nbWinsDivision==view.divisionBoardVM.nbWinsForPromotion)
//			{
//				view.divisionBoardVM.promotionBarFinish=0f;
//			}
//			else
//			{	
//				view.divisionBoardVM.promotionBarFinish=view.divisionBoardVM.promotionBarWidth;
//			}
//			float tempFloat = 1f-(view.divisionBoardVM.startActiveGaugeWidth+view.divisionBoardVM.gaugeSpace4+view.divisionBoardVM.gaugeSpace3+view.divisionBoardVM.titleBarWidth);
//			view.divisionBoardVM.activeGaugeWidthStart=tempFloat*(((float)view.divisionBoardVM.nbWinsDivision-(float)view.divisionBoardVM.hasWon)/(float)view.divisionBoardVM.nbWinsForPromotion);
//			view.divisionBoardVM.activeGaugeWidth=view.divisionBoardVM.activeGaugeWidthStart;
//			view.divisionBoardVM.activeGaugeWidthFinish=tempFloat*(((float)view.divisionBoardVM.nbWinsDivision)/(float)view.divisionBoardVM.nbWinsForPromotion);
//			
//			view.divisionBoardVM.gaugeSpace2Start=(1f-((float)view.divisionBoardVM.nbWinsDivision-(float)view.divisionBoardVM.hasWon)/(float)view.divisionBoardVM.nbWinsForPromotion)*tempFloat;
//			view.divisionBoardVM.gaugeSpace2=view.divisionBoardVM.gaugeSpace2Start;
//			view.divisionBoardVM.gaugeSpace2Finish=(1f-((float)view.divisionBoardVM.nbWinsDivision)/(float)view.divisionBoardVM.nbWinsForPromotion)*tempFloat;
//		}
//		else
//		{
//			view.divisionBoardVM.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[0];
//			if(view.divisionBoardVM.nbWinsForPromotion!=-1){
//				view.divisionBoardVM.promotionBarWidth=0.005f;
//				view.divisionBoardVM.promotionBarFinish=view.divisionBoardVM.promotionBarWidth;
//			}
//			view.divisionBoardVM.relegationBarWidth=0.005f;
//			view.divisionBoardVM.gaugeSpace3=(1f-(view.divisionBoardVM.gaugeSpace4+view.divisionBoardVM.startActiveGaugeWidth+view.divisionBoardVM.promotionBarWidth+view.divisionBoardVM.titleBarWidth))*((float)view.divisionBoardVM.nbWinsForTitle-(float)view.divisionBoardVM.nbWinsForPromotion)/(float)view.divisionBoardVM.nbWinsForTitle;
//			if(view.divisionBoardVM.nbWinsForPromotion!=-1){
//				view.divisionBoardVM.gaugeSpace2=(1f-(view.divisionBoardVM.gaugeSpace4+view.divisionBoardVM.gaugeSpace3+view.divisionBoardVM.startActiveGaugeWidth+view.divisionBoardVM.promotionBarWidth+view.divisionBoardVM.titleBarWidth))*((float)view.divisionBoardVM.nbWinsForPromotion-(float)view.divisionBoardVM.nbWinsForRelegation)/(float)view.divisionBoardVM.nbWinsForPromotion;
//			}
//			if(view.divisionBoardVM.nbWinsDivision==view.divisionBoardVM.nbWinsForRelegation)
//			{
//				view.divisionBoardVM.relegationBarFinish=0f;
//			}
//			else
//			{
//				view.divisionBoardVM.relegationBarFinish=view.divisionBoardVM.relegationBarWidth;
//			}
//			float tempFloat = 1f-(view.divisionBoardVM.startActiveGaugeWidth+view.divisionBoardVM.gaugeSpace4+view.divisionBoardVM.gaugeSpace3+view.divisionBoardVM.gaugeSpace2+view.divisionBoardVM.titleBarWidth+view.divisionBoardVM.promotionBarWidth);
//			view.divisionBoardVM.activeGaugeWidthStart=tempFloat*(((float)view.divisionBoardVM.nbWinsDivision-(float)view.divisionBoardVM.hasWon)/(float)view.divisionBoardVM.nbWinsForRelegation);
//			view.divisionBoardVM.activeGaugeWidth=view.divisionBoardVM.activeGaugeWidthStart;
//			view.divisionBoardVM.activeGaugeWidthFinish=tempFloat*(((float)view.divisionBoardVM.nbWinsDivision)/(float)view.divisionBoardVM.nbWinsForRelegation);
//			
//			view.divisionBoardVM.gaugeSpace1Start=(1f-((float)view.divisionBoardVM.nbWinsDivision-(float)view.divisionBoardVM.hasWon)/(float)view.divisionBoardVM.nbWinsForRelegation)*tempFloat;
//			view.divisionBoardVM.gaugeSpace1=view.divisionBoardVM.gaugeSpace1Start;
//			view.divisionBoardVM.gaugeSpace1Finish=(1f-((float)view.divisionBoardVM.nbWinsDivision)/(float)view.divisionBoardVM.nbWinsForRelegation)*tempFloat;
//		}
//	}
//	private void computeGauge()
//	{	
//		if(this.transformRatio>=1f)
//		{
//			this.transformRatio=1f;
//			this.toUpdateGauge=false;
//			if(view.divisionBoardVM.title)
//			{
//				view.setTitlePopUp(true);
//			}
//			else if(view.divisionBoardVM.promotion)
//			{
//				view.setPromotionPopUp(true);
//			}
//			else if(view.divisionBoardVM.endSeason)
//			{
//				view.setEndSeasonPopUp(true);
//			}
//			else if(view.divisionBoardVM.relegation)
//			{
//				view.setRelegationPopUp(true);
//			}
//		}
//		if(view.divisionBoardVM.activeGaugeWidthStart!=view.divisionBoardVM.activeGaugeWidthFinish)
//		{
//			view.divisionBoardVM.activeGaugeWidth=
//				view.divisionBoardVM.activeGaugeWidthStart
//					+this.transformRatio*(view.divisionBoardVM.activeGaugeWidthFinish-view.divisionBoardVM.activeGaugeWidthStart);
//		}
//		if(view.divisionBoardVM.gaugeSpace1Start!=view.divisionBoardVM.gaugeSpace1Finish)
//		{
//			view.divisionBoardVM.gaugeSpace1=
//				view.divisionBoardVM.gaugeSpace1Start+
//					this.transformRatio*(view.divisionBoardVM.gaugeSpace1Finish-view.divisionBoardVM.gaugeSpace1Start);
//		}
//		if(view.divisionBoardVM.gaugeSpace2Start!=view.divisionBoardVM.gaugeSpace2Finish)
//		{
//			view.divisionBoardVM.gaugeSpace2=
//				view.divisionBoardVM.gaugeSpace2Start+
//					this.transformRatio*(view.divisionBoardVM.gaugeSpace2Finish-view.divisionBoardVM.gaugeSpace2Start);
//		}
//		if(view.divisionBoardVM.gaugeSpace3Start!=view.divisionBoardVM.gaugeSpace3Finish)
//		{
//			view.divisionBoardVM.gaugeSpace3=
//				view.divisionBoardVM.gaugeSpace3Start+
//					transformRatio*(view.divisionBoardVM.gaugeSpace3Finish-view.divisionBoardVM.gaugeSpace3Start);
//		}
//		if(view.divisionBoardVM.relegationBarWidth!=view.divisionBoardVM.relegationBarFinish && this.transformRatio==1f)
//		{
//			view.divisionBoardVM.activeGaugeWidth=view.divisionBoardVM.activeGaugeWidth+view.divisionBoardVM.relegationBarWidth;
//			view.divisionBoardVM.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[1];
//			view.divisionBoardVM.relegationBarWidth=view.divisionBoardVM.relegationBarFinish;
//		}
//		if(view.divisionBoardVM.promotionBarWidth!=view.divisionBoardVM.promotionBarFinish && this.transformRatio==1f)
//		{
//			view.divisionBoardVM.activeGaugeWidth=view.divisionBoardVM.activeGaugeWidth+view.divisionBoardVM.promotionBarWidth;
//			view.divisionBoardVM.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[2];
//			view.divisionBoardVM.promotionBarWidth=view.divisionBoardVM.promotionBarFinish;
//		}
//		if(view.divisionBoardVM.titleBarWidth!=view.divisionBoardVM.titleBarFinish && this.transformRatio==1f)
//		{
//			view.divisionBoardVM.activeGaugeWidth=view.divisionBoardVM.activeGaugeWidth+view.divisionBoardVM.titleBarWidth;
//			view.divisionBoardVM.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[3];
//			view.divisionBoardVM.titleBarWidth=view.divisionBoardVM.titleBarFinish;
//		}
//	}
//	public void drawGauge()
//	{
//		view.divisionBoardVM.startActiveGaugeBackgroundStyle.fixedWidth = view.divisionBoardVM.startActiveGaugeWidth*view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.activeGaugeBackgroundStyle.fixedWidth = view.divisionBoardVM.activeGaugeWidth*view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.relegationBarStyle.fixedWidth = view.divisionBoardVM.relegationBarWidth*view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.promotionBarStyle.fixedWidth = view.divisionBoardVM.promotionBarWidth*view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.titleBarStyle.fixedWidth = view.divisionBoardVM.titleBarWidth*view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.gaugeSpace1Width=view.divisionBoardVM.gaugeSpace1*view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.gaugeSpace2Width=view.divisionBoardVM.gaugeSpace2*view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.gaugeSpace3Width=view.divisionBoardVM.gaugeSpace3*view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.gaugeSpace4Width=view.divisionBoardVM.gaugeSpace4*view.divisionBoardVM.gaugeWidth;
//		
//	}
//	public void resizeGauge()
//	{
//		view.divisionBoardVM.divisionLabelStyle.fixedHeight = (int)view.endGameScreenVM.heightScreen * 35 / 1000;
//		view.divisionBoardVM.divisionStrikeLabelStyle.fixedHeight = (int)view.endGameScreenVM.blockTopLeftHeight * 5 / 100;
//		view.divisionBoardVM.remainingGamesStyle.fixedHeight = (int)view.endGameScreenVM.blockTopLeftHeight * 5 / 100;
//		view.divisionBoardVM.promotionPrizeLabelStyle.fixedHeight = (int)view.endGameScreenVM.blockTopLeftHeight * 5 / 100;
//		view.divisionBoardVM.titlePrizeLabelStyle.fixedHeight = (int)view.endGameScreenVM.blockTopLeftHeight * 5 / 100;
//		view.divisionBoardVM.gaugeWidth = view.endGameScreenVM.blockTopLeftWidth*0.9f;
//		view.divisionBoardVM.gaugeHeight = view.endGameScreenVM.blockTopLeftHeight * 0.3f;
//		view.divisionBoardVM.gaugeBackgroundStyle.fixedWidth = view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.gaugeBackgroundStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;
//		view.divisionBoardVM.relegationLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.relegationLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;
//		view.divisionBoardVM.promotionLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.promotionLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;
//		view.divisionBoardVM.titleLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.titleLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;
//		view.divisionBoardVM.relegationValueLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.relegationValueLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;
//		view.divisionBoardVM.promotionValueLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.promotionValueLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;
//		view.divisionBoardVM.titleValueLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
//		view.divisionBoardVM.titleValueLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;
//		view.divisionBoardVM.startActiveGaugeBackgroundStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;
//		view.divisionBoardVM.activeGaugeBackgroundStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;
//		view.divisionBoardVM.relegationBarStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;
//		view.divisionBoardVM.promotionBarStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;
//		view.divisionBoardVM.titleBarStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;
//	}
//	private void initializeRounds()
//	{
//		for (int i=0; i<model.currentCup.NbRounds;i++)
//		{
//			view.cupBoardVM.roundsStyle.Add(new GUIStyle());
//			view.cupBoardVM.roundsName.Add (this.roundsName[i]);
//			if(i>model.currentCup.NbRounds-model.lastResults.Count)
//			{
//				view.cupBoardVM.roundsStyle[i]=view.cupBoardVM.winRoundStyle;
//			}
//			else if(i==model.currentCup.NbRounds-model.lastResults.Count)
//			{
//				if(model.lastResults[0].HasWon)
//				{
//					view.cupBoardVM.roundsStyle[i]=view.cupBoardVM.winRoundStyle;
//				}
//				else
//				{
//					view.cupBoardVM.roundsStyle[i]=view.cupBoardVM.looseRoundStyle;
//				}
//			}
//			else
//			{
//				view.cupBoardVM.roundsStyle[i]=view.cupBoardVM.notPlayedRoundStyle;
//			}
//		}
//	}
//	private void resizeRounds()
//	{
//		for(int i=0;i<model.currentCup.NbRounds;i++)
//		{
//			view.cupBoardVM.roundsStyle[i].fixedHeight=(int)view.endGameScreenVM.blockTopLeftHeight*50/(100*model.currentCup.NbRounds);
//			view.cupBoardVM.roundsStyle[i].fontSize=(int)view.cupBoardVM.roundsStyle[i].fixedHeight*60/100;
//		}
//	}
//	private void initStyles()
//	{
//		view.endGameOpponentVM.styles=new GUIStyle[this.opponentVMStyle.Length];
//		for(int i=0;i<this.opponentVMStyle.Length;i++)
//		{
//			view.endGameOpponentVM.styles[i]=this.opponentVMStyle[i];
//		}
//		view.endGameOpponentVM.initStyles();
//		view.endGameRankingVM.styles=new GUIStyle[this.rankingVMStyle.Length];
//		for(int i=0;i<this.rankingVMStyle.Length;i++)
//		{
//			view.endGameRankingVM.styles[i]=this.rankingVMStyle[i];
//		}
//		view.endGameRankingVM.initStyles();
//		view.endGameResultsVM.styles=new GUIStyle[this.resultsVMStyle.Length];
//		for(int i=0;i<this.resultsVMStyle.Length;i++)
//		{
//			view.endGameResultsVM.styles[i]=this.resultsVMStyle[i];
//		}
//		view.endGameResultsVM.initStyles();
//		view.endGameScreenVM.styles=new GUIStyle[this.screenVMStyle.Length];
//		for(int i=0;i<this.screenVMStyle.Length;i++)
//		{
//			view.endGameScreenVM.styles[i]=this.screenVMStyle[i];
//		}
//		view.endGameScreenVM.initStyles();
//		view.endGameVM.styles=new GUIStyle[this.endGameVMStyle.Length];
//		for(int i=0;i<this.endGameVMStyle.Length;i++)
//		{
//			view.endGameVM.styles[i]=this.endGameVMStyle[i];
//		}
//		view.endGameVM.initStyles();
//		switch(model.lastResults[0].GameType)
//		{
//		case 0:
//			view.friendlyBoardVM.styles=new GUIStyle[this.friendlyBoardVMStyle.Length];
//			for(int i=0;i<this.friendlyBoardVMStyle.Length;i++)
//			{
//				view.friendlyBoardVM.styles[i]=this.friendlyBoardVMStyle[i];
//			}
//			view.friendlyBoardVM.initStyles();
//			break;
//		case 1:
//			view.divisionBoardVM.styles=new GUIStyle[this.divisionBoardVMStyle.Length];
//			for(int i=0;i<this.divisionBoardVMStyle.Length;i++)
//			{
//				view.divisionBoardVM.styles[i]=this.divisionBoardVMStyle[i];
//			}
//			view.divisionBoardVM.initStyles();
//			view.endGameCompetInfosVM.styles=new GUIStyle[this.competInfosVMStyle.Length];
//			for(int i=0;i<this.competInfosVMStyle.Length;i++)
//			{
//				view.endGameCompetInfosVM.styles[i]=this.competInfosVMStyle[i];
//			}
//			view.endGameCompetInfosVM.initStyles();
//			break;
//		case 2:
//			view.cupBoardVM.styles=new GUIStyle[this.cupBoardVMStyle.Length];
//			for(int i=0;i<this.cupBoardVMStyle.Length;i++)
//			{
//				view.cupBoardVM.styles[i]=this.cupBoardVMStyle[i];
//			}
//			view.cupBoardVM.initStyles();
//			view.endGameCompetInfosVM.styles=new GUIStyle[this.competInfosVMStyle.Length];
//			for(int i=0;i<this.competInfosVMStyle.Length;i++)
//			{
//				view.endGameCompetInfosVM.styles[i]=this.competInfosVMStyle[i];
//			}
//			view.endGameCompetInfosVM.initStyles();
//			break;
//		}
//	}
//	public void quitEndGame()
//	{
//		Application.LoadLevel("HomePage");
//	}
//	public void setButtonsGui(bool value)
//	{
//		view.endGameVM.exitButtonEnabled = value;
//	}
//	public IEnumerator endTutorial()
//	{
//		MenuController.instance.setButtonsGui (false);
//		yield return StartCoroutine (model.currentUser.setTutorialStep (6));
//		Application.LoadLevel ("Store");
//	}
//}

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


public class DivisionLobbyController : MonoBehaviour
{
	public static DivisionLobbyController instance;
	private DivisionLobbyModel model;
	private DivisionLobbyView view;

	public Texture2D[] gaugeBackgrounds;
	public GUIStyle[] screenVMStyle;
	public GUIStyle[] boardVMStyle;
	public GUIStyle[] resultsVMStyle;
	public GUIStyle[] divisionLobbyVMStyle;
	public GUIStyle[] opponnentVMStyle;
	public GUIStyle[] competInfosVMStyle;
	
	public GameObject MenuObject;
	
	void Start()
	{
		instance = this;
		this.model = new DivisionLobbyModel ();
		this.view = Camera.main.gameObject.AddComponent <DivisionLobbyView>();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization());
	}
	private IEnumerator initialization()
	{
		yield return StartCoroutine(model.getDivisionLobbyData());
		this.initStyles ();
		this.initVM ();
		this.initializeGauge ();
		this.resize ();
	}
	private void initVM()
	{
		for(int i =0;i<model.results.Count;i++)
		{
			if(model.results[i].HasWon)
			{
				view.boardVM.nbWinsDivision++;
			}
			else
			{
				view.boardVM.nbLoosesDivision++;
			}
		}
		view.boardVM.remainingGames=model.currentDivision.NbGames-view.boardVM.nbWinsDivision-view.boardVM.nbLoosesDivision;
		view.boardVM.nbWinsForPromotion = model.currentDivision.NbWinsForPromotion;
		view.boardVM.nbWinsForTitle = model.currentDivision.NbWinsForTitle;
		view.boardVM.nbWinsForRelegation = model.currentDivision.NbWinsForRelegation;
		view.boardVM.divisionName = model.currentDivision.Name;
		for(int i=0;i<model.results.Count;i++)
		{
			view.resultsVM.resultsStyles.Add (new GUIStyle());
			view.resultsVM.focusButtonStyles.Add(new GUIStyle());
			view.resultsVM.resultsLabel.Add (model.results[i].Date.ToString("dd/MM/yyyy"));
			if(i==0)
			{
				view.resultsVM.focusButtonStyles[i]=view.resultsVM.selectedFocusButtonStyle;
			}
			else
			{
				view.resultsVM.focusButtonStyles[i]=view.resultsVM.focusButtonStyle;
			}
			if(model.results[i].HasWon)
			{
				view.resultsVM.resultsStyles[i]=view.resultsVM.wonLabelStyle;
				view.resultsVM.resultsLabel[i]=view.resultsVM.resultsLabel[i]+" Victoire";
			}
			else
			{
				view.resultsVM.resultsStyles[i]=view.resultsVM.defeatLabelStyle;
				view.resultsVM.resultsLabel[i]=view.resultsVM.resultsLabel[i]+" Défaite";
			}
		}
		if(model.results.Count>0)
		{
			view.opponentVM.username = model.results [0].Opponent.Username;
			view.opponentVM.totalNbWins = model.results [0].Opponent.TotalNbWins;
			view.opponentVM.totalNbLooses = model.results [0].Opponent.TotalNbLooses;
			view.opponentVM.ranking = model.results [0].Opponent.Ranking;
			view.opponentVM.rankingPoints = model.results [0].Opponent.RankingPoints;
			view.opponentVM.division = model.results [0].Opponent.Division;
			view.opponentVM.profilePictureStyle.normal.background = model.results [0].Opponent.texture;
			StartCoroutine (model.results [0].Opponent.setProfilePicture ());
		}
		view.competInfosVM.nbGames = model.currentDivision.NbGames;
		view.competInfosVM.titlePrize = model.currentDivision.TitlePrize;
		view.competInfosVM.promotionPrize = model.currentDivision.PromotionPrize;
		view.competInfosVM.competitionPictureStyle.normal.background = model.currentDivision.texture;
		StartCoroutine (model.currentDivision.setPicture ());
	}
	public void displayOpponent(int chosenOpponent)
	{
		view.opponentVM.username = model.results [chosenOpponent].Opponent.Username;
		view.opponentVM.totalNbWins = model.results [chosenOpponent].Opponent.TotalNbWins;
		view.opponentVM.totalNbLooses = model.results [chosenOpponent].Opponent.TotalNbLooses;
		view.opponentVM.ranking = model.results [chosenOpponent].Opponent.Ranking;
		view.opponentVM.rankingPoints = model.results [chosenOpponent].Opponent.RankingPoints;
		view.opponentVM.division = model.results [chosenOpponent].Opponent.Division;
		view.opponentVM.profilePictureStyle.normal.background = model.results [chosenOpponent].Opponent.texture;
		StartCoroutine (model.results [chosenOpponent].Opponent.setProfilePicture ());
		view.resultsVM.focusButtonStyles[chosenOpponent]=view.resultsVM.selectedFocusButtonStyle;
		view.resultsVM.focusButtonStyles[view.resultsVM.chosenResult]=view.resultsVM.focusButtonStyle;
		view.resultsVM.chosenResult = chosenOpponent;
	}
	private void initializeGauge()
	{	
		if(view.boardVM.nbWinsDivision>=view.boardVM.nbWinsForPromotion || 
		   (view.boardVM.nbWinsForPromotion==-1 && view.boardVM.nbWinsDivision>=view.boardVM.nbWinsForRelegation))
		{
			view.boardVM.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[2];
			float tempFloat = 1f-(view.boardVM.startActiveGaugeWidth+view.boardVM.gaugeSpace4);
			view.boardVM.activeGaugeWidth=tempFloat*((float)view.boardVM.nbWinsDivision/(float)view.boardVM.nbWinsForTitle);
			view.boardVM.gaugeSpace3=(1f-(float)view.boardVM.nbWinsDivision/(float)view.boardVM.nbWinsForTitle)*tempFloat;
		}
		else if(view.boardVM.nbWinsDivision>=view.boardVM.nbWinsForRelegation && view.boardVM.nbWinsForPromotion!=-1)
		{
			view.boardVM.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[1];
			view.boardVM.promotionBarWidth=0.005f;
			view.boardVM.gaugeSpace3=(1f-(view.boardVM.gaugeSpace4+view.boardVM.startActiveGaugeWidth+view.boardVM.promotionBarWidth+view.boardVM.titleBarWidth))*((float)view.boardVM.nbWinsForTitle-(float)view.boardVM.nbWinsForPromotion)/(float)view.boardVM.nbWinsForTitle;
			float tempFloat = 1f-(view.boardVM.startActiveGaugeWidth+view.boardVM.gaugeSpace4+view.boardVM.gaugeSpace3+view.boardVM.titleBarWidth);
			view.boardVM.activeGaugeWidth=tempFloat*((float)view.boardVM.nbWinsDivision/(float)view.boardVM.nbWinsForPromotion);
			view.boardVM.gaugeSpace2=(1f-(float)view.boardVM.nbWinsDivision/(float)view.boardVM.nbWinsForPromotion)*tempFloat;
		}
		else
		{
			view.boardVM.activeGaugeBackgroundStyle.normal.background=this.gaugeBackgrounds[0];
			if(view.boardVM.nbWinsForPromotion!=-1)
			{
				view.boardVM.promotionBarWidth=0.005f;
			}
			view.boardVM.relegationBarWidth=0.005f;
			view.boardVM.gaugeSpace3=(1f-(view.boardVM.gaugeSpace4+view.boardVM.startActiveGaugeWidth+view.boardVM.promotionBarWidth+view.boardVM.titleBarWidth))*((float)view.boardVM.nbWinsForTitle-(float)view.boardVM.nbWinsForPromotion)/(float)view.boardVM.nbWinsForTitle;
			if(view.boardVM.nbWinsForPromotion!=-1)
			{
				view.boardVM.gaugeSpace2=(1f-(view.boardVM.gaugeSpace4+view.boardVM.gaugeSpace3+view.boardVM.startActiveGaugeWidth+view.boardVM.promotionBarWidth+view.boardVM.titleBarWidth))*((float)view.boardVM.nbWinsForPromotion-(float)view.boardVM.nbWinsForRelegation)/(float)view.boardVM.nbWinsForPromotion;
			}
			float tempFloat = 1f-(view.boardVM.startActiveGaugeWidth+view.boardVM.gaugeSpace4+view.boardVM.gaugeSpace3+view.boardVM.gaugeSpace2+view.boardVM.titleBarWidth+view.boardVM.promotionBarWidth);
			view.boardVM.activeGaugeWidth=tempFloat*((float)view.boardVM.nbWinsDivision/(float)view.boardVM.nbWinsForRelegation);
			view.boardVM.gaugeSpace1=(1f-((float)view.boardVM.nbWinsDivision)/(float)view.boardVM.nbWinsForRelegation)*tempFloat;
		}
	}
	private void initStyles()
	{
		view.screenVM.styles=new GUIStyle[this.screenVMStyle.Length];
		for(int i=0;i<this.screenVMStyle.Length;i++)
		{
			view.screenVM.styles[i]=this.screenVMStyle[i];
		}
		view.screenVM.initStyles();
		view.divisionLobbyVM.styles=new GUIStyle[this.divisionLobbyVMStyle.Length];
		for(int i=0;i<this.divisionLobbyVMStyle.Length;i++)
		{
			view.divisionLobbyVM.styles[i]=this.divisionLobbyVMStyle[i];
		}
		view.divisionLobbyVM.initStyles();
		view.resultsVM.styles=new GUIStyle[this.resultsVMStyle.Length];
		for(int i=0;i<this.resultsVMStyle.Length;i++)
		{
			view.resultsVM.styles[i]=this.resultsVMStyle[i];
		}
		view.resultsVM.initStyles();
		view.opponentVM.styles=new GUIStyle[this.opponnentVMStyle.Length];
		for(int i=0;i<this.opponnentVMStyle.Length;i++)
		{
			view.opponentVM.styles[i]=this.opponnentVMStyle[i];
		}
		view.opponentVM.initStyles();
		view.boardVM.styles=new GUIStyle[this.boardVMStyle.Length];
		for(int i=0;i<this.boardVMStyle.Length;i++)
		{
			view.boardVM.styles[i]=this.boardVMStyle[i];
		}
		view.boardVM.initStyles();
		view.competInfosVM.styles=new GUIStyle[this.competInfosVMStyle.Length];
		for(int i=0;i<this.competInfosVMStyle.Length;i++)
		{
			view.competInfosVM.styles[i]=this.competInfosVMStyle[i];
		}
		view.competInfosVM.initStyles();
	}
	public void resize()
	{
		view.screenVM.resize ();
		view.divisionLobbyVM.resize (view.screenVM.heightScreen);
		view.resultsVM.resize (view.screenVM.heightScreen);
		view.opponentVM.resize (view.screenVM.heightScreen);
		view.resultsVM.resize (view.screenVM.heightScreen);
		view.boardVM.resize (view.screenVM.heightScreen);
		view.competInfosVM.resize (view.screenVM.heightScreen);
		this.resizeGauge ();
	}
	private void resizeGauge()
	{
		view.boardVM.gaugeWidth = view.screenVM.blockTopLeftWidth*0.9f;
		view.boardVM.gaugeHeight = view.screenVM.blockTopLeftHeight * 0.3f;
		view.boardVM.startActiveGaugeBackgroundStyle.fixedWidth = view.boardVM.startActiveGaugeWidth*view.boardVM.gaugeWidth;
		view.boardVM.activeGaugeBackgroundStyle.fixedWidth = view.boardVM.activeGaugeWidth*view.boardVM.gaugeWidth;
		view.boardVM.relegationBarStyle.fixedWidth = view.boardVM.relegationBarWidth*view.boardVM.gaugeWidth;
		view.boardVM.promotionBarStyle.fixedWidth = view.boardVM.promotionBarWidth*view.boardVM.gaugeWidth;
		view.boardVM.titleBarStyle.fixedWidth = view.boardVM.titleBarWidth*view.boardVM.gaugeWidth;
		view.boardVM.gaugeSpace1Width=view.boardVM.gaugeSpace1*view.boardVM.gaugeWidth;
		view.boardVM.gaugeSpace2Width=view.boardVM.gaugeSpace2*view.boardVM.gaugeWidth;
		view.boardVM.gaugeSpace3Width=view.boardVM.gaugeSpace3*view.boardVM.gaugeWidth;
		view.boardVM.gaugeSpace4Width=view.boardVM.gaugeSpace4*view.boardVM.gaugeWidth;
		view.boardVM.divisionLabelStyle.fixedHeight = (int)view.screenVM.heightScreen * 35 / 1000;
		view.boardVM.divisionStrikeLabelStyle.fixedHeight = (int)view.screenVM.blockTopLeftHeight * 5 / 100;
		view.boardVM.remainingGamesStyle.fixedHeight = (int)view.screenVM.blockTopLeftHeight * 5 / 100;
		view.boardVM.promotionPrizeLabelStyle.fixedHeight = (int)view.screenVM.blockTopLeftHeight * 5 / 100;		
		view.boardVM.titlePrizeLabelStyle.fixedHeight = (int)view.screenVM.blockTopLeftHeight * 5 / 100;
		view.boardVM.gaugeBackgroundStyle.fixedWidth = view.boardVM.gaugeWidth;
		view.boardVM.gaugeBackgroundStyle.fixedHeight = view.boardVM.gaugeHeight;
		view.boardVM.relegationLabelStyle.fixedWidth = 5f / 100f * view.boardVM.gaugeWidth;
		view.boardVM.relegationLabelStyle.fixedHeight = 5f / 100f * view.screenVM.blockTopLeftHeight;
		view.boardVM.promotionLabelStyle.fixedWidth = 5f / 100f * view.boardVM.gaugeWidth;
		view.boardVM.promotionLabelStyle.fixedHeight = 5f / 100f * view.screenVM.blockTopLeftHeight;
		view.boardVM.titleLabelStyle.fixedWidth = 5f / 100f * view.boardVM.gaugeWidth;
		view.boardVM.titleLabelStyle.fixedHeight = 5f / 100f * view.screenVM.blockTopLeftHeight;
		view.boardVM.relegationValueLabelStyle.fixedWidth = 5f / 100f * view.boardVM.gaugeWidth;
		view.boardVM.relegationValueLabelStyle.fixedHeight = 5f / 100f * view.screenVM.blockTopLeftHeight;
		view.boardVM.promotionValueLabelStyle.fixedWidth = 5f / 100f * view.boardVM.gaugeWidth;
		view.boardVM.promotionValueLabelStyle.fixedHeight = 5f / 100f * view.screenVM.blockTopLeftHeight;
		view.boardVM.titleValueLabelStyle.fixedWidth = 5f / 100f * view.boardVM.gaugeWidth;
		view.boardVM.titleValueLabelStyle.fixedHeight = 5f / 100f * view.screenVM.blockTopLeftHeight;
		view.boardVM.startActiveGaugeBackgroundStyle.fixedHeight = view.boardVM.gaugeHeight;
		view.boardVM.activeGaugeBackgroundStyle.fixedHeight = view.boardVM.gaugeHeight;
		view.boardVM.relegationBarStyle.fixedHeight = view.boardVM.gaugeHeight;
		view.boardVM.promotionBarStyle.fixedHeight = view.boardVM.gaugeHeight;
		view.boardVM.titleBarStyle.fixedHeight = view.boardVM.gaugeHeight;
	}
	public void joinDivisionGame()
	{
		ApplicationModel.gameType = 1; // 1 pour Official
		Application.LoadLevel("Game");
	}
	public void quitDivisionLobby()
	{
		Application.LoadLevel("Lobby");
	}
}
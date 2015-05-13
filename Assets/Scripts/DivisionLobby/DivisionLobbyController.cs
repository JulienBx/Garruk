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
	public GUIStyle[] lastOpponnentVMStyle;
	
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
		this.setPagination ();
		this.displayResults ();
		this.initializeGauge ();
		this.resize ();
	}
	public void loadData()
	{
		this.setPagination ();
		this.displayResults ();
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
		view.boardVM.titlePrize = model.currentDivision.TitlePrize;
		view.boardVM.promotionPrize = model.currentDivision.PromotionPrize;
		view.boardVM.divisionName = model.currentDivision.Name;
		if(model.results.Count>0)
		{
			view.lastOpponentVM.username=model.results[0].Opponent.Username;
			view.lastOpponentVM.totalNbWins=model.results[0].Opponent.TotalNbWins;
			view.lastOpponentVM.totalNbLooses=model.results[0].Opponent.TotalNbLooses;
			view.lastOpponentVM.ranking=model.results[0].Opponent.Ranking;
			view.lastOpponentVM.rankingPoints=model.results[0].Opponent.RankingPoints;
			view.lastOpponentVM.division=model.results[0].Opponent.Division;
			view.lastOpponentVM.profilePictureStyle.normal.background=model.results[0].Opponent.texture;
		}
	}
	public void displayResults()
	{
		view.resultsVM.start = view.resultsVM.chosenPage * 5;
		if((view.resultsVM.chosenPage+1)*5>model.results.Count)
		{
			view.resultsVM.finish=model.results.Count;
		}
		else
		{
			view.resultsVM.finish=(view.resultsVM.chosenPage+1) * 5;
		}
		view.resultsVM.profilePictureButtonStyles = new List<GUIStyle> ();
		view.resultsVM.labelStyles = new List<GUIStyle> ();
		view.resultsVM.backgroundStyles = new List<GUIStyle> ();
		view.resultsVM.label = new List<string> ();
		view.resultsVM.totalNbWins = new List<int> ();
		view.resultsVM.totalNbLooses = new List<int> ();
		view.resultsVM.ranking = new List<int> ();
		view.resultsVM.division = new List<int> ();
		view.resultsVM.username = new List<string> ();
		for(int i =view.resultsVM.start;i<view.resultsVM.finish;i++)
		{
			view.resultsVM.username.Add(model.results[i].Opponent.Username);
			view.resultsVM.totalNbWins.Add(model.results[i].Opponent.TotalNbWins);
			view.resultsVM.totalNbLooses.Add(model.results[i].Opponent.TotalNbLooses);
			view.resultsVM.ranking.Add(model.results[i].Opponent.Ranking);
			view.resultsVM.division.Add (model.results[i].Opponent.Division);
			view.resultsVM.profilePictureButtonStyles.Add (new GUIStyle());
			view.resultsVM.profilePictureButtonStyles[view.resultsVM.profilePictureButtonStyles.Count-1].normal.background=model.results[i].Opponent.texture;
			view.resultsVM.profilePictureButtonStyles[view.resultsVM.profilePictureButtonStyles.Count-1].stretchHeight=true;
			view.resultsVM.profilePictureButtonStyles[view.resultsVM.profilePictureButtonStyles.Count-1].stretchWidth=true;
			StartCoroutine(model.results[i].Opponent.setProfilePicture());
			if(model.results[i].HasWon)
			{
				view.resultsVM.backgroundStyles.Add (view.resultsVM.wonBackgroundStyle);
				view.resultsVM.labelStyles.Add (view.resultsVM.wonLabelStyle);
				view.resultsVM.label.Add ("Victoire");
			}
			else
			{
				view.resultsVM.backgroundStyles.Add (view.resultsVM.defeatBackgroundStyle);
				view.resultsVM.labelStyles.Add (view.resultsVM.defeatLabelStyle);
				view.resultsVM.label.Add ("Défaite");
			}
		}
	}
	private void setPagination()
	{
		view.resultsVM.chosenPage = 0;
		view.resultsVM.nbPages=Mathf.CeilToInt(model.results.Count/5f);
		view.resultsVM.pageDebut=0;
		
		if (view.resultsVM.nbPages>5)
		{
			view.resultsVM.pageFin = 4 ;
		}
		else
		{
			view.resultsVM.pageFin = view.resultsVM.nbPages ;
		}
		
		view.resultsVM.paginatorGuiStyle = new GUIStyle[view.resultsVM.nbPages];
		for (int i = 0; i < view.resultsVM.nbPages; i++) 
		{ 
			if (i==0)
			{
				view.resultsVM.paginatorGuiStyle[0]=view.divisionLobbyVM.paginationActivatedStyle;
			}
			else{
				view.resultsVM.paginatorGuiStyle[i]=view.divisionLobbyVM.paginationStyle;
			}
		}
	}
	public void paginationBack()
	{
		view.resultsVM.pageDebut = view.resultsVM.pageDebut-15;
		view.resultsVM.pageFin = view.resultsVM.pageDebut+15;
	}
	public void paginationSelect(int chosenPage)
	{
		view.resultsVM.paginatorGuiStyle[view.resultsVM.chosenPage]=view.divisionLobbyVM.paginationStyle;
		view.resultsVM.chosenPage=chosenPage;
		view.resultsVM.paginatorGuiStyle[chosenPage]=this.view.divisionLobbyVM.paginationActivatedStyle;
		this.displayResults();
	}
	public void paginationNext()
	{
		view.resultsVM.pageDebut = view.resultsVM.pageDebut+15;
		view.resultsVM.pageFin = Mathf.Min(view.resultsVM.pageFin+15, view.resultsVM.nbPages);
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
			view.boardVM.activeGaugeBackgroundStyle.normal.background=view.boardVM.gaugeBackgrounds[0];
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
		view.lastOpponentVM.styles=new GUIStyle[this.lastOpponnentVMStyle.Length];
		for(int i=0;i<this.lastOpponnentVMStyle.Length;i++)
		{
			view.lastOpponentVM.styles[i]=this.lastOpponnentVMStyle[i];
		}
		view.lastOpponentVM.initStyles();
		view.boardVM.styles=new GUIStyle[this.boardVMStyle.Length];
		for(int i=0;i<this.boardVMStyle.Length;i++)
		{
			view.boardVM.styles[i]=this.boardVMStyle[i];
		}
		view.boardVM.initStyles();
	}
	public void resize()
	{
		view.screenVM.resize ();
		view.divisionLobbyVM.resize (view.screenVM.heightScreen);
		view.resultsVM.resize (view.screenVM.heightScreen);
		view.lastOpponentVM.resize (view.screenVM.heightScreen);
		view.resultsVM.resize (view.screenVM.heightScreen);
		view.boardVM.resize (view.screenVM.heightScreen);
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
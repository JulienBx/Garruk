using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class EndGameView : MonoBehaviour
{
	public EndGameResultsViewModel endGameResultsVM;
	public EndGameOpponentViewModel endGameOpponentVM;
	public EndGameDivisionBoardViewModel divisionBoardVM;
	public EndGameCupBoardViewModel cupBoardVM;
	public EndGameScreenViewModel endGameScreenVM;
	public EndGameRankingViewModel endGameRankingVM;
	public EndGameFriendlyBoardViewModel friendlyBoardVM;
	public EndGameViewModel endGameVM;
	public EndGameCompetitionInfosViewModel endGameCompetInfosVM;

	private bool canDisplay=false;
	private bool titlePopUp=false;
	private bool promotionPopUp=false;
	private bool relegationPopUp=false;
	private bool endSeasonPopUp=false;
	private bool winCupPopUp=false;
	private bool endCupPopUp=false;

	
	void Start ()
	{
		this.endGameResultsVM=new EndGameResultsViewModel();
		this.endGameOpponentVM=new EndGameOpponentViewModel();
		this.cupBoardVM=new EndGameCupBoardViewModel();
		this.endGameRankingVM = new EndGameRankingViewModel ();
		this.divisionBoardVM=new EndGameDivisionBoardViewModel();
		this.endGameScreenVM=new EndGameScreenViewModel();
		this.friendlyBoardVM=new EndGameFriendlyBoardViewModel();
		this.endGameVM=new EndGameViewModel();
		this.endGameCompetInfosVM = new EndGameCompetitionInfosViewModel ();
	}

	void Update()
	{
		if (Screen.width != endGameScreenVM.widthScreen || Screen.height != endGameScreenVM.heightScreen) 
		{
			EndGameController.instance.resize();
		}
		if(this.titlePopUp){
			if(Input.GetKeyDown(KeyCode.Escape)) {
				this.titlePopUp=false;
			}
			else if(Input.GetKeyDown(KeyCode.Return)) {
				this.titlePopUp = false;
			}
		}
		if(this.promotionPopUp){
			if(Input.GetKeyDown(KeyCode.Escape)) {
				this.promotionPopUp=false;
			}
			else if(Input.GetKeyDown(KeyCode.Return)) {
				this.promotionPopUp = false;
			}
		}
		if(this.relegationPopUp){
			if(Input.GetKeyDown(KeyCode.Escape)) {
				this.relegationPopUp=false;
			}
			else if(Input.GetKeyDown(KeyCode.Return)) {
				this.relegationPopUp = false;
			}
		}
		if(this.endSeasonPopUp){
			if(Input.GetKeyDown(KeyCode.Escape)) {
				this.endSeasonPopUp=false;
			}
			else if(Input.GetKeyDown(KeyCode.Return)) {
				this.endSeasonPopUp = false;
			}
		}
		if(this.endCupPopUp){
			if(Input.GetKeyDown(KeyCode.Escape)) {
				this.endCupPopUp=false;
			}
			else if(Input.GetKeyDown(KeyCode.Return)) {
				this.endCupPopUp = false;
			}
		}
		if(this.winCupPopUp){
			if(Input.GetKeyDown(KeyCode.Escape)) {
				this.winCupPopUp=false;
			}
			else if(Input.GetKeyDown(KeyCode.Return)) {
				this.winCupPopUp = false;
			}
		}
	}

	public void OnGUI() 
	{
		// endGameScreenVM.block SUP GAUCHE
		GUILayout.BeginArea(endGameScreenVM.blockTopLeft,endGameScreenVM.blockBorderStyle);
		{
			switch(endGameVM.gameType)
			{
			case 0:
				GUILayout.FlexibleSpace();
				GUILayout.Label (friendlyBoardVM.mainLabelText,friendlyBoardVM.mainLabelStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label (friendlyBoardVM.subMainLabelText,friendlyBoardVM.subMainLabelStyle);
				GUILayout.FlexibleSpace();
				break;
			case 1:
				GUILayout.Label(divisionBoardVM.divisionName,divisionBoardVM.divisionLabelStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space (endGameScreenVM.blockTopLeftWidth*5/100);
					GUILayout.Label("Série : "+divisionBoardVM.nbWinsDivision+" V, "+divisionBoardVM.nbLoosesDivision+" D",divisionBoardVM.divisionStrikeLabelStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label("Matchs restants : "+divisionBoardVM.remainingGames.ToString(),divisionBoardVM.remainingGamesStyle);
					GUILayout.Space (endGameScreenVM.blockTopLeftWidth*5/100);
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.BeginVertical();
					{
						GUILayout.BeginHorizontal(GUILayout.Height(5f / 100f * endGameScreenVM.blockTopLeftHeight));
						{
							GUILayout.Space (divisionBoardVM.startActiveGaugeBackgroundStyle.fixedWidth);
							GUILayout.Space (divisionBoardVM.activeGaugeBackgroundStyle.fixedWidth);
							GUILayout.Space (divisionBoardVM.gaugeSpace1Width);
							if(divisionBoardVM.relegationBarWidth!=0){
								GUILayout.Space (divisionBoardVM.relegationBarStyle.fixedWidth/2f);
								GUILayout.Space (-divisionBoardVM.relegationLabelStyle.fixedWidth/2f);
								GUILayout.Label ("Relégation",divisionBoardVM.relegationLabelStyle);
								GUILayout.Space (-divisionBoardVM.relegationLabelStyle.fixedWidth/2f);
								GUILayout.Space (divisionBoardVM.relegationBarStyle.fixedWidth/2f);
							}
							GUILayout.Space (divisionBoardVM.gaugeSpace2Width);
							if(divisionBoardVM.promotionBarWidth!=0){
								GUILayout.Space (divisionBoardVM.promotionBarStyle.fixedWidth/2f);
								GUILayout.Space (-divisionBoardVM.promotionLabelStyle.fixedWidth/2f);
								GUILayout.Label ("Promotion",divisionBoardVM.promotionLabelStyle);
								GUILayout.Space (-divisionBoardVM.promotionLabelStyle.fixedWidth/2f);
								GUILayout.Space (divisionBoardVM.promotionBarStyle.fixedWidth/2f);
							}
							GUILayout.Space (divisionBoardVM.gaugeSpace3Width);
							if(divisionBoardVM.titleBarWidth!=0){
								GUILayout.Space (divisionBoardVM.titleBarStyle.fixedWidth/2f);
								GUILayout.Space (-divisionBoardVM.titleLabelStyle.fixedWidth/2f);
								GUILayout.Label ("Titre",divisionBoardVM.titleLabelStyle);
								GUILayout.Space (-divisionBoardVM.titleLabelStyle.fixedWidth/2f);
								GUILayout.Space (divisionBoardVM.titleBarStyle.fixedWidth/2f);
							}
						}
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal(divisionBoardVM.gaugeBackgroundStyle);
						{
							GUILayout.Label (divisionBoardVM.nbWinsDivision+"V",divisionBoardVM.startActiveGaugeBackgroundStyle);
							GUILayout.Label ("",divisionBoardVM.activeGaugeBackgroundStyle);
							GUILayout.Space (divisionBoardVM.gaugeSpace1Width);
							GUILayout.Label ("",divisionBoardVM.relegationBarStyle);
							GUILayout.Space (divisionBoardVM.gaugeSpace2Width);
							GUILayout.Label ("",divisionBoardVM.promotionBarStyle);
							GUILayout.Space (divisionBoardVM.gaugeSpace3Width);
							GUILayout.Label ("",divisionBoardVM.titleBarStyle);
							GUILayout.Space (divisionBoardVM.gaugeSpace4Width);
						}
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal(GUILayout.Height(5f / 100f * endGameScreenVM.blockTopLeftHeight));
						{
							GUILayout.Space (divisionBoardVM.startActiveGaugeBackgroundStyle.fixedWidth);
							GUILayout.Space (divisionBoardVM.activeGaugeBackgroundStyle.fixedWidth);
							GUILayout.Space (divisionBoardVM.gaugeSpace1Width);
							if(divisionBoardVM.relegationBarWidth!=0){
								GUILayout.Space (divisionBoardVM.relegationBarStyle.fixedWidth/2f);
								GUILayout.Space (-divisionBoardVM.relegationValueLabelStyle.fixedWidth/2f);
								GUILayout.Label (divisionBoardVM.nbWinsForRelegation.ToString()+" V",divisionBoardVM.relegationValueLabelStyle);
								GUILayout.Space (-divisionBoardVM.relegationValueLabelStyle.fixedWidth/2f);
								GUILayout.Space (divisionBoardVM.relegationBarStyle.fixedWidth/2f);
							}
							GUILayout.Space (divisionBoardVM.gaugeSpace2Width);
							if(divisionBoardVM.promotionBarWidth!=0){
								GUILayout.Space (divisionBoardVM.promotionBarStyle.fixedWidth/2f);
								GUILayout.Space (-divisionBoardVM.promotionValueLabelStyle.fixedWidth/2f);
								GUILayout.Label (divisionBoardVM.nbWinsForPromotion.ToString()+" V",divisionBoardVM.promotionValueLabelStyle);
								GUILayout.Space (-divisionBoardVM.promotionValueLabelStyle.fixedWidth/2f);
								GUILayout.Space (divisionBoardVM.promotionBarStyle.fixedWidth/2f);
							}
							GUILayout.Space (divisionBoardVM.gaugeSpace3Width);
							if(divisionBoardVM.titleBarWidth!=0){
								GUILayout.Space (divisionBoardVM.titleBarStyle.fixedWidth/2f);
								GUILayout.Space (-divisionBoardVM.titleValueLabelStyle.fixedWidth/2f);
								GUILayout.Label (divisionBoardVM.nbWinsForTitle.ToString()+" V",divisionBoardVM.titleValueLabelStyle);
								GUILayout.Space (-divisionBoardVM.titleValueLabelStyle.fixedWidth/2f);
								GUILayout.Space (divisionBoardVM.titleBarStyle.fixedWidth/2f);
							}
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndVertical();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.FlexibleSpace();
				break;
			case 2:
				GUILayout.Label(cupBoardVM.name,cupBoardVM.cupLabelStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space (0.2f*endGameScreenVM.blockTopLeftWidth);
					GUILayout.BeginVertical();
					{
						for (int i=0;i<cupBoardVM.nbRounds;i++)
						{
							GUILayout.Label(cupBoardVM.roundsName[i],cupBoardVM.roundsStyle[i]);
							GUILayout.Space ((endGameScreenVM.blockTopLeftHeight*0.5f*0.2f)/cupBoardVM.nbRounds);
						}
					}
					GUILayout.EndVertical();
					GUILayout.Space (0.2f*endGameScreenVM.blockTopLeftWidth);
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.FlexibleSpace();
				break;
			}
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(endGameScreenVM.blockBottom,endGameScreenVM.blockBorderStyle);
		{
			GUILayout.Label(endGameResultsVM.resultsTitle, endGameResultsVM.titleStyle,GUILayout.Height(0.15f * endGameScreenVM.blockBottomHeight));
			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginVertical(GUILayout.Width(endGameScreenVM.blockBottomWidth*0.30f));
				{
					endGameResultsVM.scrollPosition = GUILayout.BeginScrollView(endGameResultsVM.scrollPosition,GUILayout.Height(4*0.2f * endGameScreenVM.blockBottomHeight));
					
					for (int i = 0; i < endGameResultsVM.resultsLabel.Count; i++)
					{	
						GUILayout.BeginHorizontal();
						{
							if (GUILayout.Button(endGameResultsVM.resultsLabel[i], endGameResultsVM.resultsStyles[i],GUILayout.Height(0.2f * endGameScreenVM.blockBottomHeight)))
							{
							}
							if (GUILayout.Button(">", endGameResultsVM.focusButtonStyles[i],GUILayout.Height(0.2f * endGameScreenVM.blockBottomHeight),GUILayout.Width(0.2f * endGameScreenVM.blockBottomHeight)))
							{
								if (endGameResultsVM.chosenResult != i)
								{
									EndGameController.instance.displayOpponent(i);
								}
							}
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndScrollView();
				}
				GUILayout.EndVertical();
				GUILayout.BeginVertical();
				{
					GUILayout.BeginHorizontal(endGameOpponentVM.backgroundStyle,GUILayout.Height(4*0.2f * endGameScreenVM.blockBottomHeight));
					{
						GUILayout.Space(endGameScreenVM.blockBottomWidth*5/100);
						if(GUILayout.Button("",endGameOpponentVM.profilePictureStyle,GUILayout.Height(4*0.2f * endGameScreenVM.blockBottomHeight),GUILayout.Width(4*0.2f * endGameScreenVM.blockBottomHeight)))
						{
							ApplicationModel.profileChosen=endGameOpponentVM.username;
							Application.LoadLevel("profile");
						}
						GUILayout.Space(endGameScreenVM.blockBottomWidth*5/100);
						GUILayout.BeginVertical();
						{
							GUILayout.FlexibleSpace();
							GUILayout.Label (endGameOpponentVM.username,endGameOpponentVM.usernameStyle);
							GUILayout.FlexibleSpace();
							GUILayout.FlexibleSpace();
							GUILayout.Label ("Victoires : "+endGameOpponentVM.totalNbWins,endGameOpponentVM.informationsStyle);
							GUILayout.FlexibleSpace();
							GUILayout.Label ("Défaites : "+endGameOpponentVM.totalNbLooses,endGameOpponentVM.informationsStyle);
							GUILayout.FlexibleSpace();
							GUILayout.Label ("Ranking : "+endGameOpponentVM.ranking,endGameOpponentVM.informationsStyle);
							GUILayout.FlexibleSpace();
							GUILayout.Label ("Ranking Points : "+endGameOpponentVM.rankingPoints,endGameOpponentVM.informationsStyle);
							GUILayout.FlexibleSpace();
							GUILayout.Label ("Division : "+endGameOpponentVM.division,endGameOpponentVM.informationsStyle);
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(endGameScreenVM.blockTopRight,endGameScreenVM.blockBorderStyle);
		{
			if(GUILayout.Button("Quitter",endGameVM.buttonStyle))
			{
				EndGameController.instance.quitEndGame();
			}
		}
		GUILayout.EndArea ();
		if(endGameVM.gameType!=0)
		{
			GUILayout.BeginArea(endGameScreenVM.blockMiddleRight,endGameScreenVM.blockBorderStyle);
			{
				GUILayout.Label (endGameRankingVM.title,endGameRankingVM.titleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Ranking : "+endGameRankingVM.ranking,endGameRankingVM.rankingStyle);
				GUILayout.Label ("("+endGameRankingVM.rankingPoints+" pts )",endGameRankingVM.rankingPointsStyle);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			GUILayout.BeginArea (endGameScreenVM.blockBottomRight,endGameScreenVM.blockBorderStyle);
			{
				GUILayout.Label (endGameCompetInfosVM.title,endGameCompetInfosVM.titleStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button("",endGameCompetInfosVM.competitionPictureStyle,GUILayout.Width(endGameScreenVM.blockBottomRightHeight*1f/2f),GUILayout.Height(endGameScreenVM.blockBottomRightHeight*1f/2f)))
					{
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				if(endGameVM.gameType==1)
				{
					GUILayout.Label ("Nombre de matchs : "+endGameCompetInfosVM.nbGames,endGameCompetInfosVM.informationsStyle);
					GUILayout.Label ("Prime de titre : "+endGameCompetInfosVM.titlePrize+" crédits",endGameCompetInfosVM.informationsStyle);
					if(endGameCompetInfosVM.promotionPrize>0)
					{
						GUILayout.Label ("Prime de montée : "+endGameCompetInfosVM.promotionPrize+" crédits",endGameCompetInfosVM.informationsStyle);
					}
				}
				else if(endGameVM.gameType==2)
				{
					GUILayout.Label ("Nombre de tours : "+endGameCompetInfosVM.nbRounds,endGameCompetInfosVM.informationsStyle);
					GUILayout.Label ("Prime de victoire : "+endGameCompetInfosVM.cupPrize+" crédits",endGameCompetInfosVM.informationsStyle);
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea ();
		}
		
		// endGameScreenVM.block INF DROIT

		if (titlePopUp) {
			GUILayout.BeginArea(endGameScreenVM.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenVM.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("BRAVO !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous avez remporté le titre de la division "+ divisionBoardVM.id+" !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label (divisionBoardVM.titlePrize.ToString()+ " crédits sont ajoutés à votre portefeuille",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					if(divisionBoardVM.nbWinsForPromotion!=-1)
					{
						GUILayout.Label ("Vous accédez à la division "+ (divisionBoardVM.id-1).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
						GUILayout.FlexibleSpace();
					}
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenVM.centralWindowButtonStyle)){
							this.titlePopUp=false;	
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
		if (promotionPopUp) 
		{
			GUILayout.BeginArea(endGameScreenVM.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenVM.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("BRAVO !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous avez été promu en division "+ (divisionBoardVM.id-1).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label (divisionBoardVM.promotionPrize.ToString()+ " crédits sont ajoutés à votre portefeuille",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenVM.centralWindowButtonStyle)){
							this.promotionPopUp=false;	
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
		if (relegationPopUp) {
			GUILayout.BeginArea(endGameScreenVM.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenVM.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("OUPS !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous descendez en division "+ (divisionBoardVM.id+1).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenVM.centralWindowButtonStyle)){
							this.relegationPopUp=false;	
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
		if (endSeasonPopUp) {
			GUILayout.BeginArea(endGameScreenVM.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenVM.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("BIEN JOUE !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous conservez votre place en division "+ (divisionBoardVM.id).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenVM.centralWindowButtonStyle))
						{
							this.endSeasonPopUp=false;	
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
		if (winCupPopUp) {
			GUILayout.BeginArea(endGameScreenVM.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenVM.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("BRAVO !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous avez remporté la "+ (cupBoardVM.name).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label (cupBoardVM.cupPrize.ToString()+ " crédits sont ajoutés à votre portefeuille",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenVM.centralWindowButtonStyle)){
							winCupPopUp=false;	
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
		if (endCupPopUp) {
			GUILayout.BeginArea(endGameScreenVM.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenVM.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("OUPS !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous êtes malheureusement éliminé de la "+ (cupBoardVM.name).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenVM.centralWindowButtonStyle)){
							endCupPopUp=false;	
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
	}
	public void setTitlePopUp(bool value)
	{
		this.titlePopUp = value;
	}
	public void setPromotionPopUp(bool value)
	{
		this.promotionPopUp = value;
	}
	public void setRelegationPopUp(bool value)
	{
		this.relegationPopUp = value;
	}
	public void setEndSeasonPopUp(bool value)
	{
		this.endSeasonPopUp = value;
	}
	public void setWinCupPopUp(bool value)
	{
		this.winCupPopUp = value;
	}
	public void setEndCupPopUp(bool value)
	{
		this.endCupPopUp = value;
	}

}



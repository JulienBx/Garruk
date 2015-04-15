using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class EndGameView : MonoBehaviour
{
	public LastResultsViewModel lastResultsVM;
	public LastOpponentViewModel lastOpponentVM;
	public DivisionBoardViewModel divisionBoardVM;
	public CupBoardViewModel cupBoardVM;
	public EndGameScreenViewModel endGameScreenVM;
	public CurrentUserViewModel currentUserVM;
	public FriendlyBoardViewModel friendlyBoardVM;
	public EndGameViewModel endGameVM;

	private bool canDisplay=false;
	private bool titlePopUp=false;
	private bool promotionPopUp=false;
	private bool relegationPopUp=false;
	private bool endSeasonPopUp=false;
	private bool winCupPopUp=false;
	private bool endCupPopUp=false;

	
	void Start ()
	{
		this.lastResultsVM=new LastResultsViewModel();
		this.lastOpponentVM=new LastOpponentViewModel();
		this.cupBoardVM=new CupBoardViewModel();
		this.divisionBoardVM=new DivisionBoardViewModel();
		this.endGameScreenVM=new EndGameScreenViewModel();
		this.friendlyBoardVM=new FriendlyBoardViewModel();
		this.endGameVM=new EndGameViewModel();
	}

	void Update()
	{
		if (this.canDisplay)
		{
			if (Screen.width != endGameScreenVM.widthScreen || Screen.height != endGameScreenVM.heightScreen) {
				EndGameController.instance.resizeScreen();
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
	}

	public void OnGUI() 
	{
		if(this.canDisplay){
			// endGameScreenVM.block SUP GAUCHE
			GUILayout.BeginArea(endGameScreenVM.blockTopLeft,endGameScreenVM.blockBorderStyle);
			{
				switch(endGameVM.gameType)
				{
				case 0:
					GUILayout.FlexibleSpace();
					GUILayout.Label (friendlyBoardVM.mainLabelText,friendlyBoardVM.mainLabelStyle);
					GUILayout.Label (friendlyBoardVM.subMainLabelText,friendlyBoardVM.subMainLabelStyle);
					GUILayout.FlexibleSpace();
					break;
				case 1:
					GUILayout.Label("Division "+divisionBoardVM.division.Id.ToString(),divisionBoardVM.divisionLabelStyle);
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
									GUILayout.Label (divisionBoardVM.division.NbWinsForRelegation.ToString()+" V",divisionBoardVM.relegationValueLabelStyle);
									GUILayout.Space (-divisionBoardVM.relegationValueLabelStyle.fixedWidth/2f);
									GUILayout.Space (divisionBoardVM.relegationBarStyle.fixedWidth/2f);
								}
								GUILayout.Space (divisionBoardVM.gaugeSpace2Width);
								if(divisionBoardVM.promotionBarWidth!=0){
									GUILayout.Space (divisionBoardVM.promotionBarStyle.fixedWidth/2f);
									GUILayout.Space (-divisionBoardVM.promotionValueLabelStyle.fixedWidth/2f);
									GUILayout.Label (divisionBoardVM.division.NbWinsForPromotion.ToString()+" V",divisionBoardVM.promotionValueLabelStyle);
									GUILayout.Space (-divisionBoardVM.promotionValueLabelStyle.fixedWidth/2f);
									GUILayout.Space (divisionBoardVM.promotionBarStyle.fixedWidth/2f);
								}
								GUILayout.Space (divisionBoardVM.gaugeSpace3Width);
								if(divisionBoardVM.titleBarWidth!=0){
									GUILayout.Space (divisionBoardVM.titleBarStyle.fixedWidth/2f);
									GUILayout.Space (-divisionBoardVM.titleValueLabelStyle.fixedWidth/2f);
									GUILayout.Label (divisionBoardVM.division.NbWinsForTitle.ToString()+" V",divisionBoardVM.titleValueLabelStyle);
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
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space (endGameScreenVM.blockTopLeftWidth*5/100);
						GUILayout.Label("Prime de promotion : "+divisionBoardVM.division.PromotionPrize.ToString()+" crédits",divisionBoardVM.promotionPrizeLabelStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label("Prime de titre : "+divisionBoardVM.division.TitlePrize.ToString()+" crédits",divisionBoardVM.titlePrizeLabelStyle);
						GUILayout.Space (endGameScreenVM.blockTopLeftWidth*5/100);
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					break;
				case 2:
					GUILayout.Label(cupBoardVM.cup.Name,cupBoardVM.cupLabelStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space (0.2f*endGameScreenVM.blockTopLeftWidth);
						GUILayout.BeginVertical();
						{
							for (int i=0;i<cupBoardVM.cup.NbRounds;i++)
							{
								GUILayout.Label(cupBoardVM.roundsName[i],cupBoardVM.roundsStyle[cupBoardVM.cup.NbRounds-1-i]);
								GUILayout.Space ((endGameScreenVM.blockTopLeftHeight*0.5f*0.2f)/cupBoardVM.cup.NbRounds);
							}
						}
						GUILayout.EndVertical();
						GUILayout.Space (0.2f*endGameScreenVM.blockTopLeftWidth);
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label("Récompense : "+cupBoardVM.cup.CupPrize.ToString()+" crédits",cupBoardVM.cupPrizeLabelStyle);
					GUILayout.FlexibleSpace();
					break;
				}
			}
			GUILayout.EndArea();
			
			// endGameScreenVM.block INF GAUCHE
			GUILayout.BeginArea(endGameScreenVM.blockBottomLeft,endGameScreenVM.blockBorderStyle);
			{
				GUILayout.Label ("Votre dernier adversaire",lastOpponentVM.lastOpponentLabelStyle);
				GUILayout.BeginHorizontal(lastOpponentVM.lastOpponentBackgroundStyle);
				{
					GUILayout.Space(endGameScreenVM.blockBottomLeftWidth*5/100);
					if(GUILayout.Button("",lastOpponentVM.lastOpponentProfilePictureButtonStyle))
					{
						ApplicationModel.profileChosen=lastOpponentVM.Username;
						Application.LoadLevel("profile");
					}
					GUILayout.Space(endGameScreenVM.blockBottomLeftWidth*5/100);
					GUILayout.BeginVertical();
					{
						GUILayout.Label (lastOpponentVM.Username
						                 ,lastOpponentVM.lastOponnentUsernameLabelStyle);
						GUILayout.Space(lastOpponentVM.profilePictureSize*10/100);
						GUILayout.Label ("Victoires : "+lastOpponentVM.TotalNbWins
						                 ,lastOpponentVM.lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Défaites : "+lastOpponentVM.TotalNbLooses
						                 ,lastOpponentVM.lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Ranking : "+lastOpponentVM.Ranking
						                 ,lastOpponentVM.lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Ranking Points : "+lastOpponentVM.RankingPoints
						                 ,lastOpponentVM.lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Division : "+lastOpponentVM.Division
						                 ,lastOpponentVM.lastOponnentInformationsLabelStyle);
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			
			// endGameScreenVM.block SUP DROIT
			GUILayout.BeginArea(endGameScreenVM.blockTopRight,endGameScreenVM.blockBorderStyle);
			{
				GUILayout.Label ("Vos statistiques",currentUserVM.rankingLabelStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Victoires : "+currentUserVM.TotalNbWins,currentUserVM.yourRankingStyle);
				GUILayout.Label ("Défaites : "+currentUserVM.TotalNbLooses,currentUserVM.yourRankingStyle);
				GUILayout.Label ("Ranking : "+currentUserVM.Ranking,currentUserVM.yourRankingStyle);
				GUILayout.Label ("("+currentUserVM.Ranking+" pts)",currentUserVM.yourRankingPointsStyle);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			
			// endGameScreenVM.block INF DROIT
			GUILayout.BeginArea(endGameScreenVM.blockBottomRight,endGameScreenVM.blockBorderStyle);
			{
				GUILayout.Label (lastResultsVM.lastResultsLabel,lastResultsVM.lastResultsLabelStyle);
				for (int i=lastResultsVM.start;i<lastResultsVM.finish;i++){
					if(lastResultsVM.lastResults[i].HasWon)
					{
						GUILayout.BeginHorizontal(lastResultsVM.winBackgroundResultsListStyle);
					}
					else
					{
						GUILayout.BeginHorizontal(lastResultsVM.defeatBackgroundResultsListStyle);
					}
					{
						
						GUILayout.Space(endGameScreenVM.blockBottomRightWidth*5/100);
						if(GUILayout.Button("",lastResultsVM.profilePictureButtonStyle[i]))
						{
							ApplicationModel.profileChosen=lastResultsVM.lastResults[i].Opponent.Username;
							Application.LoadLevel("profile");
						}
						GUILayout.Space(endGameScreenVM.blockBottomRightWidth*5/100);
						GUILayout.BeginVertical();
						{
							GUILayout.Space(lastResultsVM.profilePicturesSize*5/100);
							if(lastResultsVM.lastResults[i].HasWon)
							{
								GUILayout.Label ("Victoire",lastResultsVM.winLabelResultsListStyle);
							}
							else
							{
								GUILayout.Label ("Défaite",lastResultsVM.defeatLabelResultsListStyle);
							}
							GUILayout.Space(lastResultsVM.profilePicturesSize*5/100);
							GUILayout.Label (lastResultsVM.lastResults[i].Opponent.Username
							                 ,lastResultsVM.opponentsInformationsStyle);
							GUILayout.Label ("V : "+lastResultsVM.lastResults[i].Opponent.TotalNbWins
							                 +" D : " +lastResultsVM.lastResults[i].Opponent.TotalNbLooses
							                 ,lastResultsVM.opponentsInformationsStyle);
							GUILayout.Label ("Ranking : "+lastResultsVM.lastResults[i].Opponent.Ranking
							                 ,lastResultsVM.opponentsInformationsStyle);
							GUILayout.Label ("Division : "+lastResultsVM.lastResults[i].Opponent.Division
							                 ,lastResultsVM.opponentsInformationsStyle);
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(endGameScreenVM.blockBottomRightHeight*10/1000);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (lastResultsVM.pageDebut>0){
						if (GUILayout.Button("...",lastResultsVM.paginationStyle)){
							EndGameController.instance.paginationBehaviour(0);
						}
					}
					GUILayout.Space(endGameScreenVM.widthScreen*0.01f);
					for (int i = lastResultsVM.pageDebut ; i < lastResultsVM.pageFin ; i++){
						if (GUILayout.Button(""+(i+1),lastResultsVM.paginatorGuiStyle[i])){
							EndGameController.instance.paginationBehaviour(1,i);
						}
						GUILayout.Space(endGameScreenVM.widthScreen*0.01f);
					}
					if (lastResultsVM.nbPages>lastResultsVM.pageFin){
						if (GUILayout.Button("...",lastResultsVM.paginationStyle)){
							EndGameController.instance.paginationBehaviour(2);
						}
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(endGameScreenVM.blockBottomRightHeight*10/1000);
			}
			GUILayout.EndArea();
		}
		if (titlePopUp) {
			GUILayout.BeginArea(endGameScreenVM.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenVM.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("BRAVO !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous avez remporté le titre de la division "+ divisionBoardVM.division.Id.ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label (divisionBoardVM.division.TitlePrize.ToString()+ " crédits sont ajoutés à votre portefeuille",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					if(divisionBoardVM.division.NbWinsForPromotion!=-1)
					{
						GUILayout.Label ("Vous accédez à la division "+ (divisionBoardVM.division.Id-1).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
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
		if (promotionPopUp) {
			GUILayout.BeginArea(endGameScreenVM.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenVM.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("BRAVO !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous avez été promu en division "+ (divisionBoardVM.division.Id-1).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label (divisionBoardVM.division.PromotionPrize.ToString()+ " crédits sont ajoutés à votre portefeuille",endGameScreenVM.centralWindowTitleStyle);
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
					GUILayout.Label ("Vous descendez en division "+ (divisionBoardVM.division.Id+1).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
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
					GUILayout.Label ("Vous conservez votre place en division "+ (divisionBoardVM.division.Id).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenVM.centralWindowButtonStyle)){
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
					GUILayout.Label ("Vous avez remporté la "+ (cupBoardVM.cup.Name).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label (cupBoardVM.cup.CupPrize.ToString()+ " crédits sont ajoutés à votre portefeuille",endGameScreenVM.centralWindowTitleStyle);
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
					GUILayout.Label ("Vous êtes malheureusement éliminé de la "+ (cupBoardVM.cup.Name).ToString()+" !",endGameScreenVM.centralWindowTitleStyle);
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

	public void setCanDisplay(bool value)
	{
		this.canDisplay = value;
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



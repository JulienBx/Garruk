using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class EndGameView : MonoBehaviour
{
	public LastResultsViewModel lastResultsViewModel;
	public LastOpponentViewModel lastOpponentViewModel;
	public DivisionBoardViewModel divisionBoardViewModel;
	public CupBoardViewModel cupBoardViewModel;
	public EndGameScreenViewModel endGameScreenViewModel;
	public CurrentUserViewModel currentUserViewModel;
	public FriendlyBoardViewModel friendlyBoardViewModel;
	public EndGameViewModel endGameViewModel;

	private bool canDisplay=false;
	private bool titlePopUp=false;
	private bool promotionPopUp=false;
	private bool relegationPopUp=false;
	private bool endSeasonPopUp=false;
	private bool winCupPopUp=false;
	private bool endCupPopUp=false;

	
	void Start ()
	{
		this.lastResultsViewModel=new LastResultsViewModel();
		this.lastOpponentViewModel=new LastOpponentViewModel();
		this.cupBoardViewModel=new CupBoardViewModel();
		this.divisionBoardViewModel=new DivisionBoardViewModel();
		this.endGameScreenViewModel=new EndGameScreenViewModel();
		this.friendlyBoardViewModel=new FriendlyBoardViewModel();
		this.endGameViewModel=new EndGameViewModel();
	}

	void Update()
	{
		if (this.canDisplay)
		{
			if (Screen.width != endGameScreenViewModel.widthScreen || Screen.height != endGameScreenViewModel.heightScreen) {
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
			// endGameScreenViewModel.block SUP GAUCHE
			GUILayout.BeginArea(endGameScreenViewModel.blockTopLeft,endGameScreenViewModel.blockBorderStyle);
			{
				switch(endGameViewModel.gameType)
				{
				case 0:
					GUILayout.FlexibleSpace();
					GUILayout.Label (friendlyBoardViewModel.mainLabelText,friendlyBoardViewModel.mainLabelStyle);
					GUILayout.Label (friendlyBoardViewModel.subMainLabelText,friendlyBoardViewModel.subMainLabelStyle);
					GUILayout.FlexibleSpace();
					break;
				case 1:
					GUILayout.Label("Division "+divisionBoardViewModel.division.Id.ToString(),divisionBoardViewModel.divisionLabelStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space (endGameScreenViewModel.blockTopLeftWidth*5/100);
						GUILayout.Label("Série : "+divisionBoardViewModel.nbWinsDivision+" V, "+divisionBoardViewModel.nbLoosesDivision+" D",divisionBoardViewModel.divisionStrikeLabelStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label("Matchs restants : "+divisionBoardViewModel.remainingGames.ToString(),divisionBoardViewModel.remainingGamesStyle);
						GUILayout.Space (endGameScreenViewModel.blockTopLeftWidth*5/100);
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						GUILayout.BeginVertical();
						{
							GUILayout.BeginHorizontal(GUILayout.Height(5f / 100f * endGameScreenViewModel.blockTopLeftHeight));
							{
								GUILayout.Space (divisionBoardViewModel.startActiveGaugeBackgroundStyle.fixedWidth);
								GUILayout.Space (divisionBoardViewModel.activeGaugeBackgroundStyle.fixedWidth);
								GUILayout.Space (divisionBoardViewModel.gaugeSpace1Width);
								if(divisionBoardViewModel.relegationBarWidth!=0){
									GUILayout.Space (divisionBoardViewModel.relegationBarStyle.fixedWidth/2f);
									GUILayout.Space (-divisionBoardViewModel.relegationLabelStyle.fixedWidth/2f);
									GUILayout.Label ("Relégation",divisionBoardViewModel.relegationLabelStyle);
									GUILayout.Space (-divisionBoardViewModel.relegationLabelStyle.fixedWidth/2f);
									GUILayout.Space (divisionBoardViewModel.relegationBarStyle.fixedWidth/2f);
								}
								GUILayout.Space (divisionBoardViewModel.gaugeSpace2Width);
								if(divisionBoardViewModel.promotionBarWidth!=0){
									GUILayout.Space (divisionBoardViewModel.promotionBarStyle.fixedWidth/2f);
									GUILayout.Space (-divisionBoardViewModel.promotionLabelStyle.fixedWidth/2f);
									GUILayout.Label ("Promotion",divisionBoardViewModel.promotionLabelStyle);
									GUILayout.Space (-divisionBoardViewModel.promotionLabelStyle.fixedWidth/2f);
									GUILayout.Space (divisionBoardViewModel.promotionBarStyle.fixedWidth/2f);
								}
								GUILayout.Space (divisionBoardViewModel.gaugeSpace3Width);
								if(divisionBoardViewModel.titleBarWidth!=0){
									GUILayout.Space (divisionBoardViewModel.titleBarStyle.fixedWidth/2f);
									GUILayout.Space (-divisionBoardViewModel.titleLabelStyle.fixedWidth/2f);
									GUILayout.Label ("Titre",divisionBoardViewModel.titleLabelStyle);
									GUILayout.Space (-divisionBoardViewModel.titleLabelStyle.fixedWidth/2f);
									GUILayout.Space (divisionBoardViewModel.titleBarStyle.fixedWidth/2f);
								}
							}
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal(divisionBoardViewModel.gaugeBackgroundStyle);
							{
								GUILayout.Label (divisionBoardViewModel.nbWinsDivision+"V",divisionBoardViewModel.startActiveGaugeBackgroundStyle);
								GUILayout.Label ("",divisionBoardViewModel.activeGaugeBackgroundStyle);
								GUILayout.Space (divisionBoardViewModel.gaugeSpace1Width);
								GUILayout.Label ("",divisionBoardViewModel.relegationBarStyle);
								GUILayout.Space (divisionBoardViewModel.gaugeSpace2Width);
								GUILayout.Label ("",divisionBoardViewModel.promotionBarStyle);
								GUILayout.Space (divisionBoardViewModel.gaugeSpace3Width);
								GUILayout.Label ("",divisionBoardViewModel.titleBarStyle);
								GUILayout.Space (divisionBoardViewModel.gaugeSpace4Width);
							}
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal(GUILayout.Height(5f / 100f * endGameScreenViewModel.blockTopLeftHeight));
							{
								GUILayout.Space (divisionBoardViewModel.startActiveGaugeBackgroundStyle.fixedWidth);
								GUILayout.Space (divisionBoardViewModel.activeGaugeBackgroundStyle.fixedWidth);
								GUILayout.Space (divisionBoardViewModel.gaugeSpace1Width);
								if(divisionBoardViewModel.relegationBarWidth!=0){
									GUILayout.Space (divisionBoardViewModel.relegationBarStyle.fixedWidth/2f);
									GUILayout.Space (-divisionBoardViewModel.relegationValueLabelStyle.fixedWidth/2f);
									GUILayout.Label (divisionBoardViewModel.division.NbWinsForRelegation.ToString()+" V",divisionBoardViewModel.relegationValueLabelStyle);
									GUILayout.Space (-divisionBoardViewModel.relegationValueLabelStyle.fixedWidth/2f);
									GUILayout.Space (divisionBoardViewModel.relegationBarStyle.fixedWidth/2f);
								}
								GUILayout.Space (divisionBoardViewModel.gaugeSpace2Width);
								if(divisionBoardViewModel.promotionBarWidth!=0){
									GUILayout.Space (divisionBoardViewModel.promotionBarStyle.fixedWidth/2f);
									GUILayout.Space (-divisionBoardViewModel.promotionValueLabelStyle.fixedWidth/2f);
									GUILayout.Label (divisionBoardViewModel.division.NbWinsForPromotion.ToString()+" V",divisionBoardViewModel.promotionValueLabelStyle);
									GUILayout.Space (-divisionBoardViewModel.promotionValueLabelStyle.fixedWidth/2f);
									GUILayout.Space (divisionBoardViewModel.promotionBarStyle.fixedWidth/2f);
								}
								GUILayout.Space (divisionBoardViewModel.gaugeSpace3Width);
								if(divisionBoardViewModel.titleBarWidth!=0){
									GUILayout.Space (divisionBoardViewModel.titleBarStyle.fixedWidth/2f);
									GUILayout.Space (-divisionBoardViewModel.titleValueLabelStyle.fixedWidth/2f);
									GUILayout.Label (divisionBoardViewModel.division.NbWinsForTitle.ToString()+" V",divisionBoardViewModel.titleValueLabelStyle);
									GUILayout.Space (-divisionBoardViewModel.titleValueLabelStyle.fixedWidth/2f);
									GUILayout.Space (divisionBoardViewModel.titleBarStyle.fixedWidth/2f);
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
						GUILayout.Space (endGameScreenViewModel.blockTopLeftWidth*5/100);
						GUILayout.Label("Prime de promotion : "+divisionBoardViewModel.division.PromotionPrize.ToString()+" crédits",divisionBoardViewModel.promotionPrizeLabelStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label("Prime de titre : "+divisionBoardViewModel.division.TitlePrize.ToString()+" crédits",divisionBoardViewModel.titlePrizeLabelStyle);
						GUILayout.Space (endGameScreenViewModel.blockTopLeftWidth*5/100);
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					break;
				case 2:
					GUILayout.Label(cupBoardViewModel.cup.Name,cupBoardViewModel.cupLabelStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space (0.2f*endGameScreenViewModel.blockTopLeftWidth);
						GUILayout.BeginVertical();
						{
							for (int i=0;i<cupBoardViewModel.cup.NbRounds;i++)
							{
								GUILayout.Label(cupBoardViewModel.roundsName[i],cupBoardViewModel.roundsStyle[cupBoardViewModel.cup.NbRounds-1-i]);
								GUILayout.Space ((endGameScreenViewModel.blockTopLeftHeight*0.5f*0.2f)/cupBoardViewModel.cup.NbRounds);
							}
						}
						GUILayout.EndVertical();
						GUILayout.Space (0.2f*endGameScreenViewModel.blockTopLeftWidth);
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label("Récompense : "+cupBoardViewModel.cup.CupPrize.ToString()+" crédits",cupBoardViewModel.cupPrizeLabelStyle);
					GUILayout.FlexibleSpace();
					break;
				}
			}
			GUILayout.EndArea();
			
			// endGameScreenViewModel.block INF GAUCHE
			GUILayout.BeginArea(endGameScreenViewModel.blockBottomLeft,endGameScreenViewModel.blockBorderStyle);
			{
				GUILayout.Label ("Votre dernier adversaire",lastOpponentViewModel.lastOpponentLabelStyle);
				GUILayout.BeginHorizontal(lastOpponentViewModel.lastOpponentBackgroundStyle);
				{
					GUILayout.Space(endGameScreenViewModel.blockBottomLeftWidth*5/100);
					if(GUILayout.Button("",lastOpponentViewModel.lastOpponentProfilePictureButtonStyle))
					{
						ApplicationModel.profileChosen=lastOpponentViewModel.Username;
						Application.LoadLevel("profile");
					}
					GUILayout.Space(endGameScreenViewModel.blockBottomLeftWidth*5/100);
					GUILayout.BeginVertical();
					{
						GUILayout.Label (lastOpponentViewModel.Username
						                 ,lastOpponentViewModel.lastOponnentUsernameLabelStyle);
						GUILayout.Space(lastOpponentViewModel.profilePictureSize*10/100);
						GUILayout.Label ("Victoires : "+lastOpponentViewModel.TotalNbWins
						                 ,lastOpponentViewModel.lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Défaites : "+lastOpponentViewModel.TotalNbLooses
						                 ,lastOpponentViewModel.lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Ranking : "+lastOpponentViewModel.Ranking
						                 ,lastOpponentViewModel.lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Ranking Points : "+lastOpponentViewModel.RankingPoints
						                 ,lastOpponentViewModel.lastOponnentInformationsLabelStyle);
						GUILayout.Label ("Division : "+lastOpponentViewModel.Division
						                 ,lastOpponentViewModel.lastOponnentInformationsLabelStyle);
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			
			// endGameScreenViewModel.block SUP DROIT
			GUILayout.BeginArea(endGameScreenViewModel.blockTopRight,endGameScreenViewModel.blockBorderStyle);
			{
				GUILayout.Label ("Vos statistiques",currentUserViewModel.rankingLabelStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Victoires : "+currentUserViewModel.TotalNbWins,currentUserViewModel.yourRankingStyle);
				GUILayout.Label ("Défaites : "+currentUserViewModel.TotalNbLooses,currentUserViewModel.yourRankingStyle);
				GUILayout.Label ("Ranking : "+currentUserViewModel.Ranking,currentUserViewModel.yourRankingStyle);
				GUILayout.Label ("("+currentUserViewModel.Ranking+" pts)",currentUserViewModel.yourRankingPointsStyle);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			
			// endGameScreenViewModel.block INF DROIT
			GUILayout.BeginArea(endGameScreenViewModel.blockBottomRight,endGameScreenViewModel.blockBorderStyle);
			{
				GUILayout.Label (lastResultsViewModel.lastResultsLabel,lastResultsViewModel.lastResultsLabelStyle);
				for (int i=lastResultsViewModel.start;i<lastResultsViewModel.finish;i++){
					if(lastResultsViewModel.lastResults[i].HasWon)
					{
						GUILayout.BeginHorizontal(lastResultsViewModel.winBackgroundResultsListStyle);
					}
					else
					{
						GUILayout.BeginHorizontal(lastResultsViewModel.defeatBackgroundResultsListStyle);
					}
					{
						
						GUILayout.Space(endGameScreenViewModel.blockBottomRightWidth*5/100);
						if(GUILayout.Button("",lastResultsViewModel.profilePictureButtonStyle[i]))
						{
							ApplicationModel.profileChosen=lastResultsViewModel.lastResults[i].Opponent.Username;
							Application.LoadLevel("profile");
						}
						GUILayout.Space(endGameScreenViewModel.blockBottomRightWidth*5/100);
						GUILayout.BeginVertical();
						{
							GUILayout.Space(lastResultsViewModel.profilePicturesSize*5/100);
							if(lastResultsViewModel.lastResults[i].HasWon)
							{
								GUILayout.Label ("Victoire",lastResultsViewModel.winLabelResultsListStyle);
							}
							else
							{
								GUILayout.Label ("Défaite",lastResultsViewModel.defeatLabelResultsListStyle);
							}
							GUILayout.Space(lastResultsViewModel.profilePicturesSize*5/100);
							GUILayout.Label (lastResultsViewModel.lastResults[i].Opponent.Username
							                 ,lastResultsViewModel.opponentsInformationsStyle);
							GUILayout.Label ("V : "+lastResultsViewModel.lastResults[i].Opponent.TotalNbWins
							                 +" D : " +lastResultsViewModel.lastResults[i].Opponent.TotalNbLooses
							                 ,lastResultsViewModel.opponentsInformationsStyle);
							GUILayout.Label ("Ranking : "+lastResultsViewModel.lastResults[i].Opponent.Ranking
							                 ,lastResultsViewModel.opponentsInformationsStyle);
							GUILayout.Label ("Division : "+lastResultsViewModel.lastResults[i].Opponent.Division
							                 ,lastResultsViewModel.opponentsInformationsStyle);
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(endGameScreenViewModel.blockBottomRightHeight*10/1000);
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (lastResultsViewModel.pageDebut>0){
						if (GUILayout.Button("...",lastResultsViewModel.paginationStyle)){
							EndGameController.instance.paginationBehaviour(0);
						}
					}
					GUILayout.Space(endGameScreenViewModel.widthScreen*0.01f);
					for (int i = lastResultsViewModel.pageDebut ; i < lastResultsViewModel.pageFin ; i++){
						if (GUILayout.Button(""+(i+1),lastResultsViewModel.paginatorGuiStyle[i])){
							EndGameController.instance.paginationBehaviour(1,i);
						}
						GUILayout.Space(endGameScreenViewModel.widthScreen*0.01f);
					}
					if (lastResultsViewModel.nbPages>lastResultsViewModel.pageFin){
						if (GUILayout.Button("...",lastResultsViewModel.paginationStyle)){
							EndGameController.instance.paginationBehaviour(2);
						}
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(endGameScreenViewModel.blockBottomRightHeight*10/1000);
			}
			GUILayout.EndArea();
		}
		if (titlePopUp) {
			GUILayout.BeginArea(endGameScreenViewModel.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenViewModel.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("BRAVO !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous avez remporté le titre de la division "+ divisionBoardViewModel.division.Id.ToString()+" !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label (divisionBoardViewModel.division.TitlePrize.ToString()+ " crédits sont ajoutés à votre portefeuille",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					if(divisionBoardViewModel.division.NbWinsForPromotion!=-1)
					{
						GUILayout.Label ("Vous accédez à la division "+ (divisionBoardViewModel.division.Id-1).ToString()+" !",endGameScreenViewModel.centralWindowTitleStyle);
						GUILayout.FlexibleSpace();
					}
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenViewModel.centralWindowButtonStyle)){
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
			GUILayout.BeginArea(endGameScreenViewModel.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenViewModel.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("BRAVO !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous avez été promu en division "+ (divisionBoardViewModel.division.Id-1).ToString()+" !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label (divisionBoardViewModel.division.PromotionPrize.ToString()+ " crédits sont ajoutés à votre portefeuille",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenViewModel.centralWindowButtonStyle)){
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
			GUILayout.BeginArea(endGameScreenViewModel.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenViewModel.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("OUPS !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous descendez en division "+ (divisionBoardViewModel.division.Id+1).ToString()+" !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenViewModel.centralWindowButtonStyle)){
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
			GUILayout.BeginArea(endGameScreenViewModel.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenViewModel.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("BIEN JOUE !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous conservez votre place en division "+ (divisionBoardViewModel.division.Id).ToString()+" !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenViewModel.centralWindowButtonStyle)){
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
			GUILayout.BeginArea(endGameScreenViewModel.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenViewModel.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("BRAVO !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous avez remporté la "+ (cupBoardViewModel.cup.Name).ToString()+" !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label (cupBoardViewModel.cup.CupPrize.ToString()+ " crédits sont ajoutés à votre portefeuille",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenViewModel.centralWindowButtonStyle)){
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
			GUILayout.BeginArea(endGameScreenViewModel.centralWindow);
			{
				GUILayout.BeginVertical(endGameScreenViewModel.centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("OUPS !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vous êtes malheureusement éliminé de la "+ (cupBoardViewModel.cup.Name).ToString()+" !",endGameScreenViewModel.centralWindowTitleStyle);
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",endGameScreenViewModel.centralWindowButtonStyle)){
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



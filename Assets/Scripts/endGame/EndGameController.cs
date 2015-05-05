using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class EndGameController : MonoBehaviour {

	public GameObject MenuObject;

	private string ServerDirectory = "img/profile/";
	private string URLDefaultProfilePicture = "http://54.77.118.214/GarrukServer/img/profile/defautprofilepicture.png";

	private EndGameView view;
	public static EndGameController instance;

	public int displayPopUpDelay;
	public Texture2D[] gaugeBackgrounds;
	public string[] roundsName;
	
	public GUIStyle blockBorderStyle;
	public GUIStyle rankingLabelStyle;
	public GUIStyle yourRankingStyle;
	public GUIStyle yourRankingPointsStyle;
	public GUIStyle lastResultsLabelStyle;
	public GUIStyle lastOpponentLabelStyle;
	public GUIStyle lastOponnentInformationsLabelStyle;
	public GUIStyle lastOponnentUsernameLabelStyle;
	public GUIStyle lastOpponentProfilePictureButtonStyle;
	public GUIStyle opponentsInformationsStyle;
	public GUIStyle winLabelResultsListStyle;
	public GUIStyle defeatLabelResultsListStyle;
	public GUIStyle winBackgroundResultsListStyle;
	public GUIStyle defeatBackgroundResultsListStyle;
	public GUIStyle lastOpponentBackgroundStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle paginationStyle;
	public GUIStyle mainLabelStyle;
	public GUIStyle subMainLabelStyle;
	public GUIStyle divisionLabelStyle;
	public GUIStyle divisionStrikeLabelStyle;
	public GUIStyle remainingGamesStyle;
	public GUIStyle gaugeBackgroundStyle;
	public GUIStyle startActiveGaugeBackgroundStyle;
	public GUIStyle activeGaugeBackgroundStyle;
	public GUIStyle relegationBarStyle;
	public GUIStyle promotionBarStyle;
	public GUIStyle titleBarStyle;
	public GUIStyle relegationLabelStyle;
	public GUIStyle promotionLabelStyle;
	public GUIStyle titleLabelStyle;
	public GUIStyle relegationValueLabelStyle;
	public GUIStyle promotionValueLabelStyle;
	public GUIStyle titleValueLabelStyle;
	public GUIStyle titlePrizeLabelStyle;
	public GUIStyle promotionPrizeLabelStyle;
	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	public GUIStyle centralWindowButtonStyle;
	public GUIStyle winRoundStyle;
	public GUIStyle looseRoundStyle;
	public GUIStyle notPlayedRoundStyle;
	public GUIStyle cupLabelStyle;
	public GUIStyle cupPrizeLabelStyle;

	private EndGameModel model;

	private bool toUpdateGauge=false;
	private bool toStartTimer = false;
	private bool toUpdateUserResults = false;

	private float transformRatio=0f;
	private float transformSpeed=0.5f;

	private float timer;

	// Use this for initialization
	void Start () {

		instance = this;
		this.model = new EndGameModel ();
		this.view = Camera.main.gameObject.AddComponent <EndGameView>();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization ());
	}
	// Update is called once per frame
	void Update () {
		if (this.toStartTimer)
		{
			this.timer += Time.deltaTime;
			if (this.timer > this.displayPopUpDelay) {
				this.toStartTimer=false;
				if(view.cupBoardVM.winCup)
				{
					view.setWinCupPopUp(true);

				}
				else if(view.cupBoardVM.endCup)
				{
					view.setEndCupPopUp(true);
				}
			}
		}
		if (this.toUpdateGauge){
			this.transformRatio = this.transformRatio + this.transformSpeed * Time.deltaTime;
			this.computeGauge();
			view.divisionBoardVM.drawGauge ();
		}
	}
	private IEnumerator initialization(){
		yield return StartCoroutine (model.getEndGameData ());
		this.initVMs ();
		if(toUpdateUserResults)
		{
			StartCoroutine(model.updateUserResults());
		}
		this.initStyles ();
		this.setStyles ();
		this.picturesInitialization ();
		this.setProfilePictures ();
		view.lastResultsVM.displayPage ();
		switch(model.lastResults[0].GameType)
		{
		case 0:
			break;
		case 1:
			view.divisionBoardVM.initializeGauge();
			view.divisionBoardVM.drawGauge();
			this.toUpdateGauge = true;
			break;
		case 2:
			this.initializeRounds();
			if(view.cupBoardVM.winCup || view.cupBoardVM.endCup)
			{
				this.toStartTimer = true;
			}
			break;
		}
		view.setCanDisplay (true);
		}
	private void initVMs()
	{

		view.lastResultsVM = new LastResultsViewModel(model.lastResults);
		view.currentUserVM = new CurrentUserViewModel (model.currentUser);
		view.lastOpponentVM = new LastOpponentViewModel(model.lastResults[0].Opponent);
		view.endGameVM = new EndGameViewModel (model.lastResults [0].GameType);
		view.endGameScreenVM = new EndGameScreenViewModel();
		switch(view.endGameVM.gameType)
		{
		case 0:
			view.lastResultsVM.lastResultsLabel="Vos derniers résultats";
			if(model.lastResults[0].HasWon)
			{
				view.friendlyBoardVM=new FriendlyBoardViewModel("BRAVO !","Venez en match officiel vous mesurer aux meilleurs joueurs !");
			}
			else
			{
				view.friendlyBoardVM=new FriendlyBoardViewModel("DOMMAGE !","C'est en s'entrainant qu'on progresse ! Courage !");
			}
			break;
		case 1:
			view.lastResultsVM.lastResultsLabel="Vos résultats de division";
			view.divisionBoardVM = new DivisionBoardViewModel(model.currentDivision,this.gaugeBackgrounds);

			for(int i=0;i<model.lastResults.Count;i++)
			{
				if(model.lastResults[i].HasWon)
				{
					view.divisionBoardVM.nbWinsDivision++;
				}
				else
				{
					view.divisionBoardVM.nbLoosesDivision++;
				}
			}
			view.divisionBoardVM.remainingGames=model.currentDivision.NbGames-model.lastResults.Count;
			view.divisionBoardVM.hasWon=System.Convert.ToInt32(model.lastResults[0].HasWon);
			if(view.divisionBoardVM.nbWinsDivision>=model.currentDivision.NbWinsForTitle)
			{
				view.divisionBoardVM.title=true;
				if(view.divisionBoardVM.nbWinsDivision!=-1)
				{
					model.currentUser.Division--;
				}
				model.currentUser.Money=model.currentUser.Money+model.currentDivision.TitlePrize;
				model.currentUser.NbGamesDivision=0;
				this.toUpdateUserResults=true;
				model.trophyWon=new Trophy(model.currentUser.Id, model.lastResults[0].GameType,model.currentDivision.Id);
			}
			else if(model.lastResults.Count>=view.divisionBoardVM.division.NbGames)
			{
				if(view.divisionBoardVM.nbWinsDivision>=model.currentDivision.NbWinsForPromotion)
				{
					view.divisionBoardVM.promotion=true;
					model.currentUser.Division--;
					model.currentUser.Money=model.currentUser.Money+model.currentDivision.PromotionPrize;
					model.currentUser.NbGamesDivision=0;
					this.toUpdateUserResults=true;
				}
				else if(view.divisionBoardVM.nbWinsDivision>=model.currentDivision.NbWinsForRelegation)
				{
					view.divisionBoardVM.endSeason=true;
					model.currentUser.NbGamesDivision=0;
					this.toUpdateUserResults=true;
				}
				else if(view.divisionBoardVM.nbWinsDivision<model.currentDivision.NbWinsForRelegation)
				{
					view.divisionBoardVM.relegation=true;
					model.currentUser.Division++;
					model.currentUser.NbGamesDivision=0;
					this.toUpdateUserResults=true;
				}
			}
			else if((model.currentDivision.NbGames-model.lastResults.Count)<
			        (model.currentDivision.NbWinsForRelegation-view.divisionBoardVM.nbWinsDivision))
			{
				view.divisionBoardVM.relegation=true;
				model.currentUser.Division++;
				model.currentUser.NbGamesDivision=0;
				this.toUpdateUserResults=true;
			}

			break;
		case 2:
			view.lastResultsVM.lastResultsLabel="Vos résultats de coupe";
			view.cupBoardVM = new CupBoardViewModel(model.currentCup,this.roundsName);
			if(view.lastResultsVM.lastResults.Count>=model.currentCup.NbRounds && model.lastResults[0].HasWon)
			{
				view.cupBoardVM.winCup=true;
				model.currentUser.Money=model.currentUser.Money+model.currentCup.CupPrize;
				model.currentUser.NbGamesCup=0;
				this.toUpdateUserResults=true;
				model.trophyWon=new Trophy(model.currentUser.Id, model.lastResults[0].GameType,model.currentCup.Id);
			}
			else if(!model.lastResults[0].HasWon)
			{
				view.cupBoardVM.endCup=true;
				model.currentUser.NbGamesCup=0;
				this.toUpdateUserResults=true;
			}
			break;
		}
	}
	public void resizeScreen(){
		this.setStyles();
			if(model.lastResults[0].GameType==1)
			{
				view.divisionBoardVM.drawGauge();
			}
		}
	private void setStyles() {
		
		view.endGameScreenVM.heightScreen = Screen.height;
		view.endGameScreenVM.widthScreen = Screen.width;

		view.endGameScreenVM.computeScreenDisplay ();

		view.lastResultsVM.profilePicturesSize=(int)view.endGameScreenVM.blockBottomRightHeight* 17 / 100;
		view.lastOpponentVM.profilePictureSize = (int)view.endGameScreenVM.blockBottomLeftHeight * 85 / 100;
		
		view.lastResultsVM.lastResultsLabelStyle.fontSize = view.endGameScreenVM.heightScreen * 2 / 100;
		view.lastResultsVM.lastResultsLabelStyle.fixedHeight = (int)view.endGameScreenVM.heightScreen * 35 / 1000;
		
		view.lastOpponentVM.lastOpponentLabelStyle.fontSize = view.endGameScreenVM.heightScreen * 2 / 100;
		view.lastOpponentVM.lastOpponentLabelStyle.fixedHeight = (int)view.endGameScreenVM.heightScreen * 35 / 1000;

		view.currentUserVM.rankingLabelStyle.fontSize = view.endGameScreenVM.heightScreen * 2 / 100;
		view.currentUserVM.rankingLabelStyle.fixedHeight = (int)view.endGameScreenVM.heightScreen * 35 / 1000;
		
		view.currentUserVM.yourRankingStyle.fontSize = view.endGameScreenVM.heightScreen * 3 / 100;
		view.currentUserVM.yourRankingStyle.fixedHeight = (int)view.endGameScreenVM.heightScreen * 4 / 100;
		
		view.currentUserVM.yourRankingPointsStyle.fontSize = view.endGameScreenVM.heightScreen * 25 / 1000;
		view.currentUserVM.yourRankingPointsStyle.fixedHeight = (int)view.endGameScreenVM.heightScreen * 35 / 1000;
		
		view.lastResultsVM.winLabelResultsListStyle.fontSize = (int)view.lastResultsVM.profilePicturesSize * 18 / 100;
		view.lastResultsVM.winLabelResultsListStyle.fixedHeight = (int)view.lastResultsVM.profilePicturesSize*20/100;
		view.lastResultsVM.winLabelResultsListStyle.fixedWidth = 2*(int)view.lastResultsVM.profilePicturesSize;
		
		view.lastResultsVM.defeatLabelResultsListStyle.fontSize = (int)view.lastResultsVM.profilePicturesSize * 18 / 100;
		view.lastResultsVM.defeatLabelResultsListStyle.fixedHeight = (int)view.lastResultsVM.profilePicturesSize*20/100;
		view.lastResultsVM.defeatLabelResultsListStyle.fixedWidth = 2*(int)view.lastResultsVM.profilePicturesSize;
		
		view.lastResultsVM.opponentsInformationsStyle.fontSize = (int)view.lastResultsVM.profilePicturesSize * 15 / 100;
		view.lastResultsVM.opponentsInformationsStyle.fixedHeight = (int)view.lastResultsVM.profilePicturesSize * 17 / 100;
		view.lastResultsVM.opponentsInformationsStyle.fixedWidth = 2*(int)view.lastResultsVM.profilePicturesSize;
		
		for(int i=0;i<view.lastResultsVM.profilePictureButtonStyle.Count;i++){
			view.lastResultsVM.profilePictureButtonStyle[i].fixedWidth = view.lastResultsVM.profilePicturesSize;
			view.lastResultsVM.profilePictureButtonStyle[i].fixedHeight = view.lastResultsVM.profilePicturesSize;
		}

		view.lastOpponentVM.lastOponnentInformationsLabelStyle.fontSize = (int)view.lastOpponentVM.profilePictureSize * 8 / 100;
		view.lastOpponentVM.lastOponnentInformationsLabelStyle.fixedHeight = (int)view.lastOpponentVM.profilePictureSize*15/100;
		view.lastOpponentVM.lastOponnentInformationsLabelStyle.fixedWidth = (int)view.lastOpponentVM.profilePictureSize*1.5f;

		view.lastOpponentVM.lastOponnentUsernameLabelStyle.fontSize = (int)view.lastOpponentVM.profilePictureSize * 10 / 100;
		view.lastOpponentVM.lastOponnentUsernameLabelStyle.fixedHeight = (int)view.lastOpponentVM.profilePictureSize*15/100;
		view.lastOpponentVM.lastOponnentUsernameLabelStyle.fixedWidth = (int)view.lastOpponentVM.profilePictureSize*1.5f;

		view.lastOpponentVM.lastOpponentProfilePictureButtonStyle.fixedWidth = (int)view.lastOpponentVM.profilePictureSize;
		view.lastOpponentVM.lastOpponentProfilePictureButtonStyle.fixedHeight = (int)view.lastOpponentVM.profilePictureSize;
		
		view.lastResultsVM.paginationStyle.fontSize = (int)view.endGameScreenVM.blockBottomRightHeight*3/100;
		view.lastResultsVM.paginationStyle.fixedWidth = (int)view.endGameScreenVM.blockBottomRightWidth*10/100;
		view.lastResultsVM.paginationStyle.fixedHeight = (int)view.endGameScreenVM.blockBottomRightHeight*4/100;
		view.lastResultsVM.paginationActivatedStyle.fontSize = (int)view.endGameScreenVM.blockBottomRightHeight*3/100;
		view.lastResultsVM.paginationActivatedStyle.fixedWidth = (int)view.endGameScreenVM.blockBottomRightWidth*10/100;
		view.lastResultsVM.paginationActivatedStyle.fixedHeight = (int)view.endGameScreenVM.blockBottomRightHeight*4/100;

		switch(model.lastResults[0].GameType)
		{
		case 0:
			view.friendlyBoardVM.mainLabelStyle.fontSize = (int)view.endGameScreenVM.blockTopLeftHeight * 10 / 100;
			view.friendlyBoardVM.mainLabelStyle.fixedHeight = (int)view.endGameScreenVM.blockTopLeftHeight * 15 / 100;
			
			view.friendlyBoardVM.subMainLabelStyle.fontSize = (int)view.endGameScreenVM.blockTopLeftHeight * 7 / 100;
			view.friendlyBoardVM.subMainLabelStyle.fixedHeight = (int)view.endGameScreenVM.blockTopLeftHeight * 15 / 100;
			break;
		case 1:
			view.divisionBoardVM.divisionLabelStyle.fontSize = view.endGameScreenVM.heightScreen * 2 / 100;
			view.divisionBoardVM.divisionLabelStyle.fixedHeight = (int)view.endGameScreenVM.heightScreen * 35 / 1000;
			
			view.divisionBoardVM.divisionStrikeLabelStyle.fontSize= (int)view.endGameScreenVM.blockTopLeftHeight * 4 / 100;
			view.divisionBoardVM.divisionStrikeLabelStyle.fixedHeight = (int)view.endGameScreenVM.blockTopLeftHeight * 5 / 100;
			
			view.divisionBoardVM.remainingGamesStyle.fontSize= (int)view.endGameScreenVM.blockTopLeftHeight * 4 / 100;
			view.divisionBoardVM.remainingGamesStyle.fixedHeight = (int)view.endGameScreenVM.blockTopLeftHeight * 5 / 100;
			
			view.divisionBoardVM.promotionPrizeLabelStyle.fontSize= (int)view.endGameScreenVM.blockTopLeftHeight * 4 / 100;
			view.divisionBoardVM.promotionPrizeLabelStyle.fixedHeight = (int)view.endGameScreenVM.blockTopLeftHeight * 5 / 100;
			
			view.divisionBoardVM.titlePrizeLabelStyle.fontSize= (int)view.endGameScreenVM.blockTopLeftHeight * 4 / 100;
			view.divisionBoardVM.titlePrizeLabelStyle.fixedHeight = (int)view.endGameScreenVM.blockTopLeftHeight * 5 / 100;

			view.divisionBoardVM.gaugeWidth = view.endGameScreenVM.blockTopLeftWidth*0.9f;
			view.divisionBoardVM.gaugeHeight = view.endGameScreenVM.blockTopLeftHeight * 0.3f;

			view.divisionBoardVM.gaugeBackgroundStyle.fixedWidth = view.divisionBoardVM.gaugeWidth;
			view.divisionBoardVM.gaugeBackgroundStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;

			view.divisionBoardVM.relegationLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
			view.divisionBoardVM.relegationLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenVM.blockTopLeftHeight;
			view.divisionBoardVM.relegationLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;
			
			view.divisionBoardVM.promotionLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
			view.divisionBoardVM.promotionLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenVM.blockTopLeftHeight;
			view.divisionBoardVM.promotionLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;
			
			view.divisionBoardVM.titleLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
			view.divisionBoardVM.titleLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenVM.blockTopLeftHeight;
			view.divisionBoardVM.titleLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;
			
			view.divisionBoardVM.relegationValueLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
			view.divisionBoardVM.relegationValueLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenVM.blockTopLeftHeight;
			view.divisionBoardVM.relegationValueLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;
			
			view.divisionBoardVM.promotionValueLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
			view.divisionBoardVM.promotionValueLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenVM.blockTopLeftHeight;
			view.divisionBoardVM.promotionValueLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;
			
			view.divisionBoardVM.titleValueLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardVM.gaugeWidth;
			view.divisionBoardVM.titleValueLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenVM.blockTopLeftHeight;
			view.divisionBoardVM.titleValueLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenVM.blockTopLeftHeight;

			view.divisionBoardVM.startActiveGaugeBackgroundStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;
			view.divisionBoardVM.startActiveGaugeBackgroundStyle.fontSize = (int)view.divisionBoardVM.gaugeHeight * 50 / 100;

			view.divisionBoardVM.activeGaugeBackgroundStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;

			view.divisionBoardVM.relegationBarStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;

			view.divisionBoardVM.promotionBarStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;

			view.divisionBoardVM.titleBarStyle.fixedHeight = view.divisionBoardVM.gaugeHeight;

			break;
		case 2:
			for(int i=0;i<view.cupBoardVM.roundsStyle.Count;i++)
			{
				view.cupBoardVM.roundsStyle[i].fixedHeight=(int)view.endGameScreenVM.blockTopLeftHeight*50/(100*view.cupBoardVM.roundsStyle.Count);
				view.cupBoardVM.roundsStyle[i].fontSize=(int)view.cupBoardVM.roundsStyle[i].fixedHeight*60/100;
			}
			break;
		}
	}
	private void computeGauge(){
		
		if(this.transformRatio>=1f)
		{
			this.transformRatio=1f;
			this.toUpdateGauge=false;
			if(view.divisionBoardVM.title)
			{
				view.setTitlePopUp(true);
			}
			else if(view.divisionBoardVM.promotion)
			{
				view.setPromotionPopUp(true);
			}
			else if(view.divisionBoardVM.endSeason)
			{
				view.setEndSeasonPopUp(true);
			}
			else if(view.divisionBoardVM.relegation)
			{
				view.setRelegationPopUp(true);
			}
		}
		if(view.divisionBoardVM.activeGaugeWidthStart!=view.divisionBoardVM.activeGaugeWidthFinish)
		{
			view.divisionBoardVM.activeGaugeWidth=
				view.divisionBoardVM.activeGaugeWidthStart
					+this.transformRatio*(view.divisionBoardVM.activeGaugeWidthFinish-view.divisionBoardVM.activeGaugeWidthStart);
		}
		if(view.divisionBoardVM.gaugeSpace1Start!=view.divisionBoardVM.gaugeSpace1Finish)
		{
			view.divisionBoardVM.gaugeSpace1=
				view.divisionBoardVM.gaugeSpace1Start+
					this.transformRatio*(view.divisionBoardVM.gaugeSpace1Finish-view.divisionBoardVM.gaugeSpace1Start);
		}
		if(view.divisionBoardVM.gaugeSpace2Start!=view.divisionBoardVM.gaugeSpace2Finish)
		{
			view.divisionBoardVM.gaugeSpace2=
				view.divisionBoardVM.gaugeSpace2Start+
					this.transformRatio*(view.divisionBoardVM.gaugeSpace2Finish-view.divisionBoardVM.gaugeSpace2Start);
		}
		if(view.divisionBoardVM.gaugeSpace3Start!=view.divisionBoardVM.gaugeSpace3Finish)
		{
			view.divisionBoardVM.gaugeSpace3=
				view.divisionBoardVM.gaugeSpace3Start+
					transformRatio*(view.divisionBoardVM.gaugeSpace3Finish-view.divisionBoardVM.gaugeSpace3Start);
		}
		if(view.divisionBoardVM.relegationBarWidth!=view.divisionBoardVM.relegationBarFinish && this.transformRatio==1f)
		{
			view.divisionBoardVM.activeGaugeWidth=view.divisionBoardVM.activeGaugeWidth+view.divisionBoardVM.relegationBarWidth;
			view.divisionBoardVM.activeGaugeBackgroundStyle.normal.background=view.divisionBoardVM.gaugeBackgrounds[1];
			view.divisionBoardVM.relegationBarWidth=view.divisionBoardVM.relegationBarFinish;
		}
		if(view.divisionBoardVM.promotionBarWidth!=view.divisionBoardVM.promotionBarFinish && this.transformRatio==1f)
		{
			view.divisionBoardVM.activeGaugeWidth=view.divisionBoardVM.activeGaugeWidth+view.divisionBoardVM.promotionBarWidth;
			view.divisionBoardVM.activeGaugeBackgroundStyle.normal.background=view.divisionBoardVM.gaugeBackgrounds[2];
			view.divisionBoardVM.promotionBarWidth=view.divisionBoardVM.promotionBarFinish;
		}
		if(view.divisionBoardVM.titleBarWidth!=view.divisionBoardVM.titleBarFinish && this.transformRatio==1f)
		{
			view.divisionBoardVM.activeGaugeWidth=view.divisionBoardVM.activeGaugeWidth+view.divisionBoardVM.titleBarWidth;
			view.divisionBoardVM.activeGaugeBackgroundStyle.normal.background=view.divisionBoardVM.gaugeBackgrounds[3];
			view.divisionBoardVM.titleBarWidth=view.divisionBoardVM.titleBarFinish;
		}
	}
	private void picturesInitialization(){

		view.lastResultsVM.profilePictures =new Texture2D[view.lastResultsVM.lastResults.Count];

		for(int i =0;i<view.lastResultsVM.lastResults.Count;i++)
		{
			view.lastResultsVM.profilePictures[i]=new Texture2D(view.lastResultsVM.profilePicturesSize,
			                                                           view.lastResultsVM.profilePicturesSize, 
			                                                      TextureFormat.ARGB32, 
			                                                      false);
			view.lastResultsVM.profilePictureButtonStyle.Add(new GUIStyle());
			view.lastResultsVM.profilePictureButtonStyle[i].normal.background=view.lastResultsVM.profilePictures[i];
			view.lastResultsVM.profilePictureButtonStyle[i].fixedWidth = view.lastResultsVM.profilePicturesSize;
			view.lastResultsVM.profilePictureButtonStyle[i].fixedHeight = view.lastResultsVM.profilePicturesSize;
		}

		view.lastOpponentVM.lastOpponentProfilePictureButtonStyle.normal.background = view.lastResultsVM.profilePictures[0];
		view.lastResultsVM.nbPages=Mathf.CeilToInt(view.lastResultsVM.lastResults.Count/5f);
		view.lastResultsVM.pageDebut=0;

		if (view.lastResultsVM.nbPages>5){
			view.lastResultsVM.pageFin = 4 ;
		}
		else{
			view.lastResultsVM.pageFin = view.lastResultsVM.nbPages ;
		}

		view.lastResultsVM.paginatorGuiStyle = new GUIStyle[view.lastResultsVM.nbPages];
		for (int i = 0; i < view.lastResultsVM.nbPages; i++) 
		{ 
			if (i==0){
				view.lastResultsVM.paginatorGuiStyle[i]=this.paginationActivatedStyle;
			}
			else{
				view.lastResultsVM.paginatorGuiStyle[i]=this.paginationStyle;
			}
		}
	}
	public void paginationBehaviour(int value, int page=0){
		view.lastResultsVM.paginationBehaviour (value,page);
		}
	private void initializeRounds(){
		for (int i=0; i<model.currentCup.NbRounds;i++)
		{
			view.cupBoardVM.roundsStyle.Add(new GUIStyle());
			if(i<view.lastResultsVM.lastResults.Count-1)
			{
				view.cupBoardVM.roundsStyle[i]=winRoundStyle;
			}
			else if(i==view.lastResultsVM.lastResults.Count-1)
			{
				if(model.lastResults[0].HasWon)
				{
					view.cupBoardVM.roundsStyle[i]=winRoundStyle;
				}
				else
				{
					view.cupBoardVM.roundsStyle[i]=looseRoundStyle;
				}
			}
			else
			{
				view.cupBoardVM.roundsStyle[i]=notPlayedRoundStyle;
			}
			view.cupBoardVM.roundsStyle[i].fixedHeight=(int)view.endGameScreenVM.blockTopLeftHeight*50/(100*model.currentCup.NbRounds);
			view.cupBoardVM.roundsStyle[i].fontSize=(int)view.cupBoardVM.roundsStyle[i].fixedHeight*60/100;
		}
	}
	private void setProfilePictures(){

		for(int i =0;i<view.lastResultsVM.lastResults.Count;i++)
		{
			if (view.lastResultsVM.lastResults[i].Opponent.Picture.StartsWith(ServerDirectory))
			{
				StartCoroutine(loadProfilePicture(i));
			}
			else
			{
				StartCoroutine(loadDefaultProfilePicture(i));
			}
		}
	}
	private IEnumerator loadProfilePicture(int i){

		var www = new WWW(ApplicationModel.host + view.lastResultsVM.lastResults[i].Opponent.Picture);
		yield return www;
		www.LoadImageIntoTexture(view.lastResultsVM.profilePictures[i]);
	}
	private IEnumerator loadDefaultProfilePicture(int i){

		var www = new WWW(URLDefaultProfilePicture);
		yield return www;
		www.LoadImageIntoTexture(view.lastResultsVM.profilePictures[i]);
	}
	private void initStyles(){

		view.endGameScreenVM.centralWindowStyle=this.centralWindowStyle;
		view.endGameScreenVM.centralWindowTitleStyle=this.centralWindowTitleStyle;
		view.endGameScreenVM.centralWindowButtonStyle=this.centralWindowButtonStyle;
		view.endGameScreenVM.blockBorderStyle=this.blockBorderStyle;

		view.lastResultsVM.lastResultsLabelStyle=this.lastResultsLabelStyle;
		view.lastResultsVM.winLabelResultsListStyle=this.winLabelResultsListStyle;
		view.lastResultsVM.defeatLabelResultsListStyle=this.defeatLabelResultsListStyle;
		view.lastResultsVM.opponentsInformationsStyle=this.opponentsInformationsStyle;
		view.lastResultsVM.paginationStyle=this.paginationStyle;
		view.lastResultsVM.paginationActivatedStyle=this.paginationActivatedStyle;
		view.lastResultsVM.winBackgroundResultsListStyle=this.winBackgroundResultsListStyle;
		view.lastResultsVM.defeatBackgroundResultsListStyle=this.defeatBackgroundResultsListStyle;

		view.lastOpponentVM.lastOpponentProfilePictureButtonStyle=this.lastOpponentProfilePictureButtonStyle;
		view.lastOpponentVM.lastOpponentLabelStyle=this.lastOpponentLabelStyle;
		view.lastOpponentVM.lastOponnentInformationsLabelStyle=this.lastOponnentInformationsLabelStyle;
		view.lastOpponentVM.lastOponnentUsernameLabelStyle=this.lastOponnentUsernameLabelStyle;
		view.lastOpponentVM.lastOpponentBackgroundStyle=this.lastOpponentBackgroundStyle;

		view.currentUserVM.rankingLabelStyle=this.rankingLabelStyle;
		view.currentUserVM.yourRankingStyle=this.yourRankingStyle;
		view.currentUserVM.yourRankingPointsStyle=this.yourRankingPointsStyle;

		switch(model.lastResults[0].GameType)
		{
		case 0:
			view.friendlyBoardVM.mainLabelStyle=this.mainLabelStyle;
			view.friendlyBoardVM.subMainLabelStyle=this.subMainLabelStyle;
			break;
		case 1:
			view.divisionBoardVM.activeGaugeBackgroundStyle=this.activeGaugeBackgroundStyle;
			view.divisionBoardVM.gaugeBackgroundStyle=this.gaugeBackgroundStyle;
			view.divisionBoardVM.startActiveGaugeBackgroundStyle=this.startActiveGaugeBackgroundStyle;
			view.divisionBoardVM.relegationBarStyle=this.relegationBarStyle;
			view.divisionBoardVM.promotionBarStyle=this.promotionBarStyle;
			view.divisionBoardVM.titleBarStyle=this.titleBarStyle;
			view.divisionBoardVM.divisionLabelStyle=this.divisionLabelStyle;
			view.divisionBoardVM.divisionStrikeLabelStyle=this.divisionStrikeLabelStyle;
			view.divisionBoardVM.remainingGamesStyle=this.remainingGamesStyle;
			view.divisionBoardVM.promotionPrizeLabelStyle=this.promotionPrizeLabelStyle;
			view.divisionBoardVM.titlePrizeLabelStyle=this.titlePrizeLabelStyle;
			view.divisionBoardVM.relegationLabelStyle=this.relegationLabelStyle;
			view.divisionBoardVM.promotionLabelStyle=this.promotionLabelStyle;
			view.divisionBoardVM.titleLabelStyle=this.titleLabelStyle;
			view.divisionBoardVM.relegationValueLabelStyle=this.relegationValueLabelStyle;
			view.divisionBoardVM.promotionValueLabelStyle=this.promotionValueLabelStyle;
			view.divisionBoardVM.titleValueLabelStyle=this.titleValueLabelStyle;
			break;
		case 2:
			view.cupBoardVM.cupLabelStyle=this.cupLabelStyle;
			view.cupBoardVM.cupPrizeLabelStyle=this.cupPrizeLabelStyle;
			break;
		}
	}
}

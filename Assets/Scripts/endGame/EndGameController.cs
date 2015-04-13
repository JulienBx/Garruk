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

	private EndGameModel model=new EndGameModel();

	private bool toUpdateGauge=false;
	private bool toStartTimer = false;
	private bool toUpdateUserResults = false;

	private float transformRatio=0f;
	private float transformSpeed=0.5f;

	private float timer;

	// Use this for initialization
	void Start () {

		instance = this;
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
				if(view.cupBoardViewModel.winCup)
				{
					view.setWinCupPopUp(true);

				}
				else if(view.cupBoardViewModel.endCup)
				{
					view.setEndCupPopUp(true);
				}
			}
		}
		if (this.toUpdateGauge){
			this.transformRatio = this.transformRatio + this.transformSpeed * Time.deltaTime;
			this.computeGauge();
			view.divisionBoardViewModel.drawGauge ();
		}
	}
	private IEnumerator initialization(){
		yield return StartCoroutine (model.getEndGameData ());
		this.initViewModels ();
		if(toUpdateUserResults)
		{
			StartCoroutine(model.updateUserResults());
		}
		this.initStyles ();
		this.setStyles ();
		this.picturesInitialization ();
		this.setProfilePictures ();
		view.lastResultsViewModel.displayPage ();
		switch(model.lastResults[0].GameType)
		{
		case 0:
			break;
		case 1:
			view.divisionBoardViewModel.initializeGauge();
			view.divisionBoardViewModel.drawGauge();
			this.toUpdateGauge = true;
			break;
		case 2:
			this.initializeRounds();
			if(view.cupBoardViewModel.winCup || view.cupBoardViewModel.endCup)
			{
				this.toStartTimer = true;
			}
			break;
		}
		view.setCanDisplay (true);
		}
	private void initViewModels()
	{

		view.lastResultsViewModel = new LastResultsViewModel(model.lastResults);
		view.currentUserViewModel = new CurrentUserViewModel (model.currentUser);
		view.lastOpponentViewModel = new LastOpponentViewModel(model.lastResults[0].Opponent);
		view.endGameViewModel = new EndGameViewModel (model.lastResults [0].GameType);
		view.endGameScreenViewModel = new EndGameScreenViewModel();
		switch(view.endGameViewModel.gameType)
		{
		case 0:
			view.lastResultsViewModel.lastResultsLabel="Vos derniers résultats";
			if(model.lastResults[0].HasWon)
			{
				view.friendlyBoardViewModel=new FriendlyBoardViewModel("BRAVO !","Venez en match officiel vous mesurer aux meilleurs joueurs !");
			}
			else
			{
				view.friendlyBoardViewModel=new FriendlyBoardViewModel("DOMMAGE !","C'est en s'entrainant qu'on progresse ! Courage !");
			}
			break;
		case 1:
			view.lastResultsViewModel.lastResultsLabel="Vos résultats de division";
			view.divisionBoardViewModel = new DivisionBoardViewModel(model.currentDivision,this.gaugeBackgrounds);

			for(int i=0;i<model.lastResults.Count;i++)
			{
				if(model.lastResults[i].HasWon)
				{
					view.divisionBoardViewModel.nbWinsDivision++;
				}
				else
				{
					view.divisionBoardViewModel.nbLoosesDivision++;
				}
			}
			view.divisionBoardViewModel.remainingGames=model.currentDivision.NbGames-model.lastResults.Count;
			view.divisionBoardViewModel.hasWon=System.Convert.ToInt32(model.lastResults[0].HasWon);
			if(view.divisionBoardViewModel.nbWinsDivision>=model.currentDivision.NbWinsForTitle)
			{
				view.divisionBoardViewModel.title=true;
				if(view.divisionBoardViewModel.nbWinsDivision!=-1)
				{
					model.currentUser.Division--;
				}
				model.currentUser.Money=model.currentUser.Money+model.currentDivision.TitlePrize;
				model.currentUser.NbGamesDivision=0;
				this.toUpdateUserResults=true;
				model.trophyWon=new Trophy(model.currentUser.Id, model.lastResults[0].GameType,model.currentDivision.Id);
			}
			else if(model.lastResults.Count>=view.divisionBoardViewModel.division.NbGames)
			{
				if(view.divisionBoardViewModel.nbWinsDivision>=model.currentDivision.NbWinsForPromotion)
				{
					view.divisionBoardViewModel.promotion=true;
					model.currentUser.Division--;
					model.currentUser.Money=model.currentUser.Money+model.currentDivision.PromotionPrize;
					model.currentUser.NbGamesDivision=0;
					this.toUpdateUserResults=true;
				}
				else if(view.divisionBoardViewModel.nbWinsDivision>=model.currentDivision.NbWinsForRelegation)
				{
					view.divisionBoardViewModel.endSeason=true;
					model.currentUser.NbGamesDivision=0;
					this.toUpdateUserResults=true;
				}
				else if(view.divisionBoardViewModel.nbWinsDivision<model.currentDivision.NbWinsForRelegation)
				{
					view.divisionBoardViewModel.relegation=true;
					model.currentUser.Division++;
					model.currentUser.NbGamesDivision=0;
					this.toUpdateUserResults=true;
				}
			}
			else if((model.currentDivision.NbGames-model.lastResults.Count)<
			        (model.currentDivision.NbWinsForRelegation-view.divisionBoardViewModel.nbWinsDivision))
			{
				view.divisionBoardViewModel.relegation=true;
				model.currentUser.Division++;
				model.currentUser.NbGamesDivision=0;
				this.toUpdateUserResults=true;
			}

			break;
		case 2:
			view.lastResultsViewModel.lastResultsLabel="Vos résultats de coupe";
			view.cupBoardViewModel = new CupBoardViewModel(model.currentCup,this.roundsName);
			if(view.lastResultsViewModel.lastResults.Count>=model.currentCup.NbRounds && model.lastResults[0].HasWon)
			{
				view.cupBoardViewModel.winCup=true;
				model.currentUser.Money=model.currentUser.Money+model.currentCup.CupPrize;
				model.currentUser.NbGamesCup=0;
				this.toUpdateUserResults=true;
				model.trophyWon=new Trophy(model.currentUser.Id, model.lastResults[0].GameType,model.currentCup.Id);
			}
			else if(!model.lastResults[0].HasWon)
			{
				view.cupBoardViewModel.endCup=true;
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
				view.divisionBoardViewModel.drawGauge();
			}
		}
	private void setStyles() {
		
		view.endGameScreenViewModel.heightScreen = Screen.height;
		view.endGameScreenViewModel.widthScreen = Screen.width;

		view.endGameScreenViewModel.computeScreenDisplay ();

		view.lastResultsViewModel.profilePicturesSize=(int)view.endGameScreenViewModel.blockBottomRightHeight* 17 / 100;
		view.lastOpponentViewModel.profilePictureSize = (int)view.endGameScreenViewModel.blockBottomLeftHeight * 85 / 100;
		
		view.lastResultsViewModel.lastResultsLabelStyle.fontSize = view.endGameScreenViewModel.heightScreen * 2 / 100;
		view.lastResultsViewModel.lastResultsLabelStyle.fixedHeight = (int)view.endGameScreenViewModel.heightScreen * 35 / 1000;
		
		view.lastOpponentViewModel.lastOpponentLabelStyle.fontSize = view.endGameScreenViewModel.heightScreen * 2 / 100;
		view.lastOpponentViewModel.lastOpponentLabelStyle.fixedHeight = (int)view.endGameScreenViewModel.heightScreen * 35 / 1000;

		view.currentUserViewModel.rankingLabelStyle.fontSize = view.endGameScreenViewModel.heightScreen * 2 / 100;
		view.currentUserViewModel.rankingLabelStyle.fixedHeight = (int)view.endGameScreenViewModel.heightScreen * 35 / 1000;
		
		view.currentUserViewModel.yourRankingStyle.fontSize = view.endGameScreenViewModel.heightScreen * 3 / 100;
		view.currentUserViewModel.yourRankingStyle.fixedHeight = (int)view.endGameScreenViewModel.heightScreen * 4 / 100;
		
		view.currentUserViewModel.yourRankingPointsStyle.fontSize = view.endGameScreenViewModel.heightScreen * 25 / 1000;
		view.currentUserViewModel.yourRankingPointsStyle.fixedHeight = (int)view.endGameScreenViewModel.heightScreen * 35 / 1000;
		
		view.lastResultsViewModel.winLabelResultsListStyle.fontSize = (int)view.lastResultsViewModel.profilePicturesSize * 18 / 100;
		view.lastResultsViewModel.winLabelResultsListStyle.fixedHeight = (int)view.lastResultsViewModel.profilePicturesSize*20/100;
		view.lastResultsViewModel.winLabelResultsListStyle.fixedWidth = 2*(int)view.lastResultsViewModel.profilePicturesSize;
		
		view.lastResultsViewModel.defeatLabelResultsListStyle.fontSize = (int)view.lastResultsViewModel.profilePicturesSize * 18 / 100;
		view.lastResultsViewModel.defeatLabelResultsListStyle.fixedHeight = (int)view.lastResultsViewModel.profilePicturesSize*20/100;
		view.lastResultsViewModel.defeatLabelResultsListStyle.fixedWidth = 2*(int)view.lastResultsViewModel.profilePicturesSize;
		
		view.lastResultsViewModel.opponentsInformationsStyle.fontSize = (int)view.lastResultsViewModel.profilePicturesSize * 15 / 100;
		view.lastResultsViewModel.opponentsInformationsStyle.fixedHeight = (int)view.lastResultsViewModel.profilePicturesSize * 17 / 100;
		view.lastResultsViewModel.opponentsInformationsStyle.fixedWidth = 2*(int)view.lastResultsViewModel.profilePicturesSize;
		
		for(int i=0;i<view.lastResultsViewModel.profilePictureButtonStyle.Count;i++){
			view.lastResultsViewModel.profilePictureButtonStyle[i].fixedWidth = view.lastResultsViewModel.profilePicturesSize;
			view.lastResultsViewModel.profilePictureButtonStyle[i].fixedHeight = view.lastResultsViewModel.profilePicturesSize;
		}

		view.lastOpponentViewModel.lastOponnentInformationsLabelStyle.fontSize = (int)view.lastOpponentViewModel.profilePictureSize * 8 / 100;
		view.lastOpponentViewModel.lastOponnentInformationsLabelStyle.fixedHeight = (int)view.lastOpponentViewModel.profilePictureSize*15/100;
		view.lastOpponentViewModel.lastOponnentInformationsLabelStyle.fixedWidth = (int)view.lastOpponentViewModel.profilePictureSize*1.5f;

		view.lastOpponentViewModel.lastOponnentUsernameLabelStyle.fontSize = (int)view.lastOpponentViewModel.profilePictureSize * 10 / 100;
		view.lastOpponentViewModel.lastOponnentUsernameLabelStyle.fixedHeight = (int)view.lastOpponentViewModel.profilePictureSize*15/100;
		view.lastOpponentViewModel.lastOponnentUsernameLabelStyle.fixedWidth = (int)view.lastOpponentViewModel.profilePictureSize*1.5f;

		view.lastOpponentViewModel.lastOpponentProfilePictureButtonStyle.fixedWidth = (int)view.lastOpponentViewModel.profilePictureSize;
		view.lastOpponentViewModel.lastOpponentProfilePictureButtonStyle.fixedHeight = (int)view.lastOpponentViewModel.profilePictureSize;
		
		view.lastResultsViewModel.paginationStyle.fontSize = (int)view.endGameScreenViewModel.blockBottomRightHeight*3/100;
		view.lastResultsViewModel.paginationStyle.fixedWidth = (int)view.endGameScreenViewModel.blockBottomRightWidth*10/100;
		view.lastResultsViewModel.paginationStyle.fixedHeight = (int)view.endGameScreenViewModel.blockBottomRightHeight*4/100;
		view.lastResultsViewModel.paginationActivatedStyle.fontSize = (int)view.endGameScreenViewModel.blockBottomRightHeight*3/100;
		view.lastResultsViewModel.paginationActivatedStyle.fixedWidth = (int)view.endGameScreenViewModel.blockBottomRightWidth*10/100;
		view.lastResultsViewModel.paginationActivatedStyle.fixedHeight = (int)view.endGameScreenViewModel.blockBottomRightHeight*4/100;

		switch(model.lastResults[0].GameType)
		{
		case 0:
			view.friendlyBoardViewModel.mainLabelStyle.fontSize = (int)view.endGameScreenViewModel.blockTopLeftHeight * 10 / 100;
			view.friendlyBoardViewModel.mainLabelStyle.fixedHeight = (int)view.endGameScreenViewModel.blockTopLeftHeight * 15 / 100;
			
			view.friendlyBoardViewModel.subMainLabelStyle.fontSize = (int)view.endGameScreenViewModel.blockTopLeftHeight * 7 / 100;
			view.friendlyBoardViewModel.subMainLabelStyle.fixedHeight = (int)view.endGameScreenViewModel.blockTopLeftHeight * 15 / 100;
			break;
		case 1:
			view.divisionBoardViewModel.divisionLabelStyle.fontSize = view.endGameScreenViewModel.heightScreen * 2 / 100;
			view.divisionBoardViewModel.divisionLabelStyle.fixedHeight = (int)view.endGameScreenViewModel.heightScreen * 35 / 1000;
			
			view.divisionBoardViewModel.divisionStrikeLabelStyle.fontSize= (int)view.endGameScreenViewModel.blockTopLeftHeight * 4 / 100;
			view.divisionBoardViewModel.divisionStrikeLabelStyle.fixedHeight = (int)view.endGameScreenViewModel.blockTopLeftHeight * 5 / 100;
			
			view.divisionBoardViewModel.remainingGamesStyle.fontSize= (int)view.endGameScreenViewModel.blockTopLeftHeight * 4 / 100;
			view.divisionBoardViewModel.remainingGamesStyle.fixedHeight = (int)view.endGameScreenViewModel.blockTopLeftHeight * 5 / 100;
			
			view.divisionBoardViewModel.promotionPrizeLabelStyle.fontSize= (int)view.endGameScreenViewModel.blockTopLeftHeight * 4 / 100;
			view.divisionBoardViewModel.promotionPrizeLabelStyle.fixedHeight = (int)view.endGameScreenViewModel.blockTopLeftHeight * 5 / 100;
			
			view.divisionBoardViewModel.titlePrizeLabelStyle.fontSize= (int)view.endGameScreenViewModel.blockTopLeftHeight * 4 / 100;
			view.divisionBoardViewModel.titlePrizeLabelStyle.fixedHeight = (int)view.endGameScreenViewModel.blockTopLeftHeight * 5 / 100;

			view.divisionBoardViewModel.gaugeWidth = view.endGameScreenViewModel.blockTopLeftWidth*0.9f;
			view.divisionBoardViewModel.gaugeHeight = view.endGameScreenViewModel.blockTopLeftHeight * 0.3f;

			view.divisionBoardViewModel.gaugeBackgroundStyle.fixedWidth = view.divisionBoardViewModel.gaugeWidth;
			view.divisionBoardViewModel.gaugeBackgroundStyle.fixedHeight = view.divisionBoardViewModel.gaugeHeight;

			view.divisionBoardViewModel.relegationLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardViewModel.gaugeWidth;
			view.divisionBoardViewModel.relegationLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenViewModel.blockTopLeftHeight;
			view.divisionBoardViewModel.relegationLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenViewModel.blockTopLeftHeight;
			
			view.divisionBoardViewModel.promotionLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardViewModel.gaugeWidth;
			view.divisionBoardViewModel.promotionLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenViewModel.blockTopLeftHeight;
			view.divisionBoardViewModel.promotionLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenViewModel.blockTopLeftHeight;
			
			view.divisionBoardViewModel.titleLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardViewModel.gaugeWidth;
			view.divisionBoardViewModel.titleLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenViewModel.blockTopLeftHeight;
			view.divisionBoardViewModel.titleLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenViewModel.blockTopLeftHeight;
			
			view.divisionBoardViewModel.relegationValueLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardViewModel.gaugeWidth;
			view.divisionBoardViewModel.relegationValueLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenViewModel.blockTopLeftHeight;
			view.divisionBoardViewModel.relegationValueLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenViewModel.blockTopLeftHeight;
			
			view.divisionBoardViewModel.promotionValueLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardViewModel.gaugeWidth;
			view.divisionBoardViewModel.promotionValueLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenViewModel.blockTopLeftHeight;
			view.divisionBoardViewModel.promotionValueLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenViewModel.blockTopLeftHeight;
			
			view.divisionBoardViewModel.titleValueLabelStyle.fixedWidth = 5f / 100f * view.divisionBoardViewModel.gaugeWidth;
			view.divisionBoardViewModel.titleValueLabelStyle.fontSize = 4 / 100 * (int)view.endGameScreenViewModel.blockTopLeftHeight;
			view.divisionBoardViewModel.titleValueLabelStyle.fixedHeight = 5f / 100f * view.endGameScreenViewModel.blockTopLeftHeight;

			view.divisionBoardViewModel.startActiveGaugeBackgroundStyle.fixedHeight = view.divisionBoardViewModel.gaugeHeight;
			view.divisionBoardViewModel.startActiveGaugeBackgroundStyle.fontSize = (int)view.divisionBoardViewModel.gaugeHeight * 50 / 100;

			view.divisionBoardViewModel.activeGaugeBackgroundStyle.fixedHeight = view.divisionBoardViewModel.gaugeHeight;

			view.divisionBoardViewModel.relegationBarStyle.fixedHeight = view.divisionBoardViewModel.gaugeHeight;

			view.divisionBoardViewModel.promotionBarStyle.fixedHeight = view.divisionBoardViewModel.gaugeHeight;

			view.divisionBoardViewModel.titleBarStyle.fixedHeight = view.divisionBoardViewModel.gaugeHeight;

			break;
		case 2:
			for(int i=0;i<view.cupBoardViewModel.roundsStyle.Count;i++)
			{
				view.cupBoardViewModel.roundsStyle[i].fixedHeight=(int)view.endGameScreenViewModel.blockTopLeftHeight*50/(100*view.cupBoardViewModel.roundsStyle.Count);
				view.cupBoardViewModel.roundsStyle[i].fontSize=(int)view.cupBoardViewModel.roundsStyle[i].fixedHeight*60/100;
			}
			break;
		}
	}
	private void computeGauge(){
		
		if(this.transformRatio>=1f)
		{
			this.transformRatio=1f;
			this.toUpdateGauge=false;
			if(view.divisionBoardViewModel.title)
			{
				view.setTitlePopUp(true);
			}
			else if(view.divisionBoardViewModel.promotion)
			{
				view.setPromotionPopUp(true);
			}
			else if(view.divisionBoardViewModel.endSeason)
			{
				view.setEndSeasonPopUp(true);
			}
			else if(view.divisionBoardViewModel.relegation)
			{
				view.setRelegationPopUp(true);
			}
		}
		if(view.divisionBoardViewModel.activeGaugeWidthStart!=view.divisionBoardViewModel.activeGaugeWidthFinish)
		{
			view.divisionBoardViewModel.activeGaugeWidth=
				view.divisionBoardViewModel.activeGaugeWidthStart
					+this.transformRatio*(view.divisionBoardViewModel.activeGaugeWidthFinish-view.divisionBoardViewModel.activeGaugeWidthStart);
		}
		if(view.divisionBoardViewModel.gaugeSpace1Start!=view.divisionBoardViewModel.gaugeSpace1Finish)
		{
			view.divisionBoardViewModel.gaugeSpace1=
				view.divisionBoardViewModel.gaugeSpace1Start+
					this.transformRatio*(view.divisionBoardViewModel.gaugeSpace1Finish-view.divisionBoardViewModel.gaugeSpace1Start);
		}
		if(view.divisionBoardViewModel.gaugeSpace2Start!=view.divisionBoardViewModel.gaugeSpace2Finish)
		{
			view.divisionBoardViewModel.gaugeSpace2=
				view.divisionBoardViewModel.gaugeSpace2Start+
					this.transformRatio*(view.divisionBoardViewModel.gaugeSpace2Finish-view.divisionBoardViewModel.gaugeSpace2Start);
		}
		if(view.divisionBoardViewModel.gaugeSpace3Start!=view.divisionBoardViewModel.gaugeSpace3Finish)
		{
			view.divisionBoardViewModel.gaugeSpace3=
				view.divisionBoardViewModel.gaugeSpace3Start+
					transformRatio*(view.divisionBoardViewModel.gaugeSpace3Finish-view.divisionBoardViewModel.gaugeSpace3Start);
		}
		if(view.divisionBoardViewModel.relegationBarWidth!=view.divisionBoardViewModel.relegationBarFinish && this.transformRatio==1f)
		{
			view.divisionBoardViewModel.activeGaugeWidth=view.divisionBoardViewModel.activeGaugeWidth+view.divisionBoardViewModel.relegationBarWidth;
			view.divisionBoardViewModel.activeGaugeBackgroundStyle.normal.background=view.divisionBoardViewModel.gaugeBackgrounds[1];
			view.divisionBoardViewModel.relegationBarWidth=view.divisionBoardViewModel.relegationBarFinish;
		}
		if(view.divisionBoardViewModel.promotionBarWidth!=view.divisionBoardViewModel.promotionBarFinish && this.transformRatio==1f)
		{
			view.divisionBoardViewModel.activeGaugeWidth=view.divisionBoardViewModel.activeGaugeWidth+view.divisionBoardViewModel.promotionBarWidth;
			view.divisionBoardViewModel.activeGaugeBackgroundStyle.normal.background=view.divisionBoardViewModel.gaugeBackgrounds[2];
			view.divisionBoardViewModel.promotionBarWidth=view.divisionBoardViewModel.promotionBarFinish;
		}
		if(view.divisionBoardViewModel.titleBarWidth!=view.divisionBoardViewModel.titleBarFinish && this.transformRatio==1f)
		{
			view.divisionBoardViewModel.activeGaugeWidth=view.divisionBoardViewModel.activeGaugeWidth+view.divisionBoardViewModel.titleBarWidth;
			view.divisionBoardViewModel.activeGaugeBackgroundStyle.normal.background=view.divisionBoardViewModel.gaugeBackgrounds[3];
			view.divisionBoardViewModel.titleBarWidth=view.divisionBoardViewModel.titleBarFinish;
		}
	}
	private void picturesInitialization(){

		view.lastResultsViewModel.profilePictures =new Texture2D[view.lastResultsViewModel.lastResults.Count];

		for(int i =0;i<view.lastResultsViewModel.lastResults.Count;i++)
		{
			view.lastResultsViewModel.profilePictures[i]=new Texture2D(view.lastResultsViewModel.profilePicturesSize,
			                                                           view.lastResultsViewModel.profilePicturesSize, 
			                                                      TextureFormat.ARGB32, 
			                                                      false);
			view.lastResultsViewModel.profilePictureButtonStyle.Add(new GUIStyle());
			view.lastResultsViewModel.profilePictureButtonStyle[i].normal.background=view.lastResultsViewModel.profilePictures[i];
			view.lastResultsViewModel.profilePictureButtonStyle[i].fixedWidth = view.lastResultsViewModel.profilePicturesSize;
			view.lastResultsViewModel.profilePictureButtonStyle[i].fixedHeight = view.lastResultsViewModel.profilePicturesSize;
		}

		view.lastOpponentViewModel.lastOpponentProfilePictureButtonStyle.normal.background = view.lastResultsViewModel.profilePictures[0];
		view.lastResultsViewModel.nbPages=Mathf.CeilToInt(view.lastResultsViewModel.lastResults.Count/5f);
		view.lastResultsViewModel.pageDebut=0;

		if (view.lastResultsViewModel.nbPages>5){
			view.lastResultsViewModel.pageFin = 4 ;
		}
		else{
			view.lastResultsViewModel.pageFin = view.lastResultsViewModel.nbPages ;
		}

		view.lastResultsViewModel.paginatorGuiStyle = new GUIStyle[view.lastResultsViewModel.nbPages];
		for (int i = 0; i < view.lastResultsViewModel.nbPages; i++) 
		{ 
			if (i==0){
				view.lastResultsViewModel.paginatorGuiStyle[i]=this.paginationActivatedStyle;
			}
			else{
				view.lastResultsViewModel.paginatorGuiStyle[i]=this.paginationStyle;
			}
		}
	}
	public void paginationBehaviour(int value, int page=0){
		view.lastResultsViewModel.paginationBehaviour (value,page);
		}
	private void initializeRounds(){
		for (int i=0; i<model.currentCup.NbRounds;i++)
		{
			view.cupBoardViewModel.roundsStyle.Add(new GUIStyle());
			if(i<view.lastResultsViewModel.lastResults.Count-1)
			{
				view.cupBoardViewModel.roundsStyle[i]=winRoundStyle;
			}
			else if(i==view.lastResultsViewModel.lastResults.Count-1)
			{
				if(model.lastResults[0].HasWon)
				{
					view.cupBoardViewModel.roundsStyle[i]=winRoundStyle;
				}
				else
				{
					view.cupBoardViewModel.roundsStyle[i]=looseRoundStyle;
				}
			}
			else
			{
				view.cupBoardViewModel.roundsStyle[i]=notPlayedRoundStyle;
			}
			view.cupBoardViewModel.roundsStyle[i].fixedHeight=(int)view.endGameScreenViewModel.blockTopLeftHeight*50/(100*model.currentCup.NbRounds);
			view.cupBoardViewModel.roundsStyle[i].fontSize=(int)view.cupBoardViewModel.roundsStyle[i].fixedHeight*60/100;
		}
	}
	private void setProfilePictures(){

		for(int i =0;i<view.lastResultsViewModel.lastResults.Count;i++)
		{
			if (view.lastResultsViewModel.lastResults[i].Opponent.Picture.StartsWith(ServerDirectory))
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

		var www = new WWW(ApplicationModel.host + view.lastResultsViewModel.lastResults[i].Opponent.Picture);
		yield return www;
		www.LoadImageIntoTexture(view.lastResultsViewModel.profilePictures[i]);
	}
	private IEnumerator loadDefaultProfilePicture(int i){

		var www = new WWW(URLDefaultProfilePicture);
		yield return www;
		www.LoadImageIntoTexture(view.lastResultsViewModel.profilePictures[i]);
	}
	private void initStyles(){

		view.endGameScreenViewModel.centralWindowStyle=this.centralWindowStyle;
		view.endGameScreenViewModel.centralWindowTitleStyle=this.centralWindowTitleStyle;
		view.endGameScreenViewModel.centralWindowButtonStyle=this.centralWindowButtonStyle;
		view.endGameScreenViewModel.blockBorderStyle=this.blockBorderStyle;

		view.lastResultsViewModel.lastResultsLabelStyle=this.lastResultsLabelStyle;
		view.lastResultsViewModel.winLabelResultsListStyle=this.winLabelResultsListStyle;
		view.lastResultsViewModel.defeatLabelResultsListStyle=this.defeatLabelResultsListStyle;
		view.lastResultsViewModel.opponentsInformationsStyle=this.opponentsInformationsStyle;
		view.lastResultsViewModel.paginationStyle=this.paginationStyle;
		view.lastResultsViewModel.paginationActivatedStyle=this.paginationActivatedStyle;
		view.lastResultsViewModel.winBackgroundResultsListStyle=this.winBackgroundResultsListStyle;
		view.lastResultsViewModel.defeatBackgroundResultsListStyle=this.defeatBackgroundResultsListStyle;

		view.lastOpponentViewModel.lastOpponentProfilePictureButtonStyle=this.lastOpponentProfilePictureButtonStyle;
		view.lastOpponentViewModel.lastOpponentLabelStyle=this.lastOpponentLabelStyle;
		view.lastOpponentViewModel.lastOponnentInformationsLabelStyle=this.lastOponnentInformationsLabelStyle;
		view.lastOpponentViewModel.lastOponnentUsernameLabelStyle=this.lastOponnentUsernameLabelStyle;
		view.lastOpponentViewModel.lastOpponentBackgroundStyle=this.lastOpponentBackgroundStyle;

		view.currentUserViewModel.rankingLabelStyle=this.rankingLabelStyle;
		view.currentUserViewModel.yourRankingStyle=this.yourRankingStyle;
		view.currentUserViewModel.yourRankingPointsStyle=this.yourRankingPointsStyle;

		switch(model.lastResults[0].GameType)
		{
		case 0:
			view.friendlyBoardViewModel.mainLabelStyle=this.mainLabelStyle;
			view.friendlyBoardViewModel.subMainLabelStyle=this.subMainLabelStyle;
			break;
		case 1:
			view.divisionBoardViewModel.activeGaugeBackgroundStyle=this.activeGaugeBackgroundStyle;
			view.divisionBoardViewModel.gaugeBackgroundStyle=this.gaugeBackgroundStyle;
			view.divisionBoardViewModel.startActiveGaugeBackgroundStyle=this.startActiveGaugeBackgroundStyle;
			view.divisionBoardViewModel.relegationBarStyle=this.relegationBarStyle;
			view.divisionBoardViewModel.promotionBarStyle=this.promotionBarStyle;
			view.divisionBoardViewModel.titleBarStyle=this.titleBarStyle;
			view.divisionBoardViewModel.divisionLabelStyle=this.divisionLabelStyle;
			view.divisionBoardViewModel.divisionStrikeLabelStyle=this.divisionStrikeLabelStyle;
			view.divisionBoardViewModel.remainingGamesStyle=this.remainingGamesStyle;
			view.divisionBoardViewModel.promotionPrizeLabelStyle=this.promotionPrizeLabelStyle;
			view.divisionBoardViewModel.titlePrizeLabelStyle=this.titlePrizeLabelStyle;
			view.divisionBoardViewModel.relegationLabelStyle=this.relegationLabelStyle;
			view.divisionBoardViewModel.promotionLabelStyle=this.promotionLabelStyle;
			view.divisionBoardViewModel.titleLabelStyle=this.titleLabelStyle;
			view.divisionBoardViewModel.relegationValueLabelStyle=this.relegationValueLabelStyle;
			view.divisionBoardViewModel.promotionValueLabelStyle=this.promotionValueLabelStyle;
			view.divisionBoardViewModel.titleValueLabelStyle=this.titleValueLabelStyle;
			break;
		case 2:
			view.cupBoardViewModel.cupLabelStyle=this.cupLabelStyle;
			view.cupBoardViewModel.cupPrizeLabelStyle=this.cupPrizeLabelStyle;
			break;
		}
	}
}

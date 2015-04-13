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

	private EndGameView endGameView;
	public static EndGameController instance;
	public LastResultsViewModel lastResultsViewModel;
	public LastOpponentViewModel lastOpponentViewModel;
	public DivisionBoardViewModel divisionBoardViewModel;
	public CupBoardViewModel cupBoardViewModel;
	public ScreenConfigurationViewModel screenConfigurationViewModel;
	public CurrentUserViewModel currentUserViewModel;
	public FriendlyBoardViewModel friendlyBoardViewModel;
	public EndGameViewModel endGameViewModel;

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
		this.endGameView = Camera.main.gameObject.AddComponent <EndGameView>();
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
				if(cupBoardViewModel.winCup)
				{
					endGameView.setWinCupPopUp(true);

				}
				else if(cupBoardViewModel.endCup)
				{
					endGameView.setEndCupPopUp(true);
				}
			}
		}
		if (this.toUpdateGauge){
			this.transformRatio = this.transformRatio + this.transformSpeed * Time.deltaTime;
			this.computeGauge();
			divisionBoardViewModel.drawGauge ();
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
		lastResultsViewModel.displayPage ();
		switch(model.lastResults[0].GameType)
		{
		case 0:
			break;
		case 1:
			divisionBoardViewModel.initializeGauge();
			divisionBoardViewModel.drawGauge();
			this.toUpdateGauge = true;
			break;
		case 2:
			this.initializeRounds();
			if(cupBoardViewModel.winCup || cupBoardViewModel.endCup)
			{
				this.toStartTimer = true;
			}
			break;
		}
		endGameView.setCanDisplay (true);
		}
	private void initViewModels()
	{

		this.lastResultsViewModel = new LastResultsViewModel(model.lastResults);
		this.currentUserViewModel = new CurrentUserViewModel (model.currentUser);
		this.lastOpponentViewModel = new LastOpponentViewModel(model.lastResults[0].Opponent);
		this.endGameViewModel = new EndGameViewModel (model.lastResults [0].GameType);
		this.screenConfigurationViewModel = new ScreenConfigurationViewModel();
		switch(this.endGameViewModel.gameType)
		{
		case 0:
			lastResultsViewModel.lastResultsLabel="Vos derniers résultats";
			if(model.lastResults[0].HasWon)
			{
				this.friendlyBoardViewModel=new FriendlyBoardViewModel("BRAVO !","Venez en match officiel vous mesurer aux meilleurs joueurs !");
			}
			else
			{
				this.friendlyBoardViewModel=new FriendlyBoardViewModel("DOMMAGE !","C'est en s'entrainant qu'on progresse ! Courage !");
			}
			break;
		case 1:
			lastResultsViewModel.lastResultsLabel="Vos résultats de division";
			this.divisionBoardViewModel = new DivisionBoardViewModel(model.currentDivision,this.gaugeBackgrounds);

			for(int i=0;i<model.lastResults.Count;i++)
			{
				if(model.lastResults[i].HasWon)
				{
					divisionBoardViewModel.nbWinsDivision++;
				}
				else
				{
					divisionBoardViewModel.nbLoosesDivision++;
				}
			}
			divisionBoardViewModel.remainingGames=model.currentDivision.NbGames-model.lastResults.Count;
			divisionBoardViewModel.hasWon=System.Convert.ToInt32(model.lastResults[0].HasWon);
			if(divisionBoardViewModel.nbWinsDivision>=model.currentDivision.NbWinsForTitle)
			{
				divisionBoardViewModel.title=true;
				if(divisionBoardViewModel.nbWinsDivision!=-1)
				{
					model.currentUser.Division--;
				}
				model.currentUser.Money=model.currentUser.Money+model.currentDivision.TitlePrize;
				model.currentUser.NbGamesDivision=0;
				this.toUpdateUserResults=true;
				model.trophyWon=new Trophy(model.currentUser.Id, model.lastResults[0].GameType,model.currentDivision.Id);
			}
			else if(model.lastResults.Count>=divisionBoardViewModel.division.NbGames)
			{
				if(divisionBoardViewModel.nbWinsDivision>=model.currentDivision.NbWinsForPromotion)
				{
					divisionBoardViewModel.promotion=true;
					model.currentUser.Division--;
					model.currentUser.Money=model.currentUser.Money+model.currentDivision.PromotionPrize;
					model.currentUser.NbGamesDivision=0;
					this.toUpdateUserResults=true;
				}
				else if(divisionBoardViewModel.nbWinsDivision>=model.currentDivision.NbWinsForRelegation)
				{
					divisionBoardViewModel.endSeason=true;
					model.currentUser.NbGamesDivision=0;
					this.toUpdateUserResults=true;
				}
				else if(divisionBoardViewModel.nbWinsDivision<model.currentDivision.NbWinsForRelegation)
				{
					divisionBoardViewModel.relegation=true;
					model.currentUser.Division++;
					model.currentUser.NbGamesDivision=0;
					this.toUpdateUserResults=true;
				}
			}
			else if((model.currentDivision.NbGames-model.lastResults.Count)<
				        (model.currentDivision.NbWinsForRelegation-divisionBoardViewModel.nbWinsDivision))
			{
				divisionBoardViewModel.relegation=true;
				model.currentUser.Division++;
				model.currentUser.NbGamesDivision=0;
				this.toUpdateUserResults=true;
			}

			break;
		case 2:
			lastResultsViewModel.lastResultsLabel="Vos résultats de coupe";
			this.cupBoardViewModel = new CupBoardViewModel(model.currentCup,this.roundsName);
			if(lastResultsViewModel.lastResults.Count>=model.currentCup.NbRounds && model.lastResults[0].HasWon)
			{
				cupBoardViewModel.winCup=true;
				model.currentUser.Money=model.currentUser.Money+model.currentCup.CupPrize;
				model.currentUser.NbGamesCup=0;
				this.toUpdateUserResults=true;
				model.trophyWon=new Trophy(model.currentUser.Id, model.lastResults[0].GameType,model.currentCup.Id);
			}
			else if(!model.lastResults[0].HasWon)
			{
				cupBoardViewModel.endCup=true;
				model.currentUser.NbGamesCup=0;
				this.toUpdateUserResults=true;
			}
			break;
		}
		endGameView.lastResultsViewModel = this.lastResultsViewModel;
		endGameView.lastOpponentViewModel = this.lastOpponentViewModel;
		endGameView.divisionBoardViewModel = this.divisionBoardViewModel;
		endGameView.cupBoardViewModel = this.cupBoardViewModel;
		endGameView.screenConfigurationViewModel = this.screenConfigurationViewModel;
		endGameView.friendlyBoardViewModel = this.friendlyBoardViewModel;
		endGameView.currentUserViewModel = this.currentUserViewModel;
		endGameView.endGameViewModel=this.endGameViewModel;

	}
	public void resizeScreen(){
		this.setStyles();
			if(model.lastResults[0].GameType==1)
			{
				divisionBoardViewModel.drawGauge();
			}
		}
	private void setStyles() {
		
		screenConfigurationViewModel.heightScreen = Screen.height;
		screenConfigurationViewModel.widthScreen = Screen.width;

		screenConfigurationViewModel.computeScreenDisplay ();

		lastResultsViewModel.profilePicturesSize=(int)screenConfigurationViewModel.blockBottomRightHeight* 17 / 100;
		lastOpponentViewModel.profilePictureSize = (int)screenConfigurationViewModel.blockBottomLeftHeight * 85 / 100;
		
		lastResultsViewModel.lastResultsLabelStyle.fontSize = screenConfigurationViewModel.heightScreen * 2 / 100;
		lastResultsViewModel.lastResultsLabelStyle.fixedHeight = (int)screenConfigurationViewModel.heightScreen * 35 / 1000;
		
		lastOpponentViewModel.lastOpponentLabelStyle.fontSize = screenConfigurationViewModel.heightScreen * 2 / 100;
		lastOpponentViewModel.lastOpponentLabelStyle.fixedHeight = (int)screenConfigurationViewModel.heightScreen * 35 / 1000;

		currentUserViewModel.rankingLabelStyle.fontSize = screenConfigurationViewModel.heightScreen * 2 / 100;
		currentUserViewModel.rankingLabelStyle.fixedHeight = (int)screenConfigurationViewModel.heightScreen * 35 / 1000;
		
		currentUserViewModel.yourRankingStyle.fontSize = screenConfigurationViewModel.heightScreen * 3 / 100;
		currentUserViewModel.yourRankingStyle.fixedHeight = (int)screenConfigurationViewModel.heightScreen * 4 / 100;
		
		currentUserViewModel.yourRankingPointsStyle.fontSize = screenConfigurationViewModel.heightScreen * 25 / 1000;
		currentUserViewModel.yourRankingPointsStyle.fixedHeight = (int)screenConfigurationViewModel.heightScreen * 35 / 1000;
		
		lastResultsViewModel.winLabelResultsListStyle.fontSize = (int)lastResultsViewModel.profilePicturesSize * 18 / 100;
		lastResultsViewModel.winLabelResultsListStyle.fixedHeight = (int)lastResultsViewModel.profilePicturesSize*20/100;
		lastResultsViewModel.winLabelResultsListStyle.fixedWidth = 2*(int)lastResultsViewModel.profilePicturesSize;
		
		lastResultsViewModel.defeatLabelResultsListStyle.fontSize = (int)lastResultsViewModel.profilePicturesSize * 18 / 100;
		lastResultsViewModel.defeatLabelResultsListStyle.fixedHeight = (int)lastResultsViewModel.profilePicturesSize*20/100;
		lastResultsViewModel.defeatLabelResultsListStyle.fixedWidth = 2*(int)lastResultsViewModel.profilePicturesSize;
		
		lastResultsViewModel.opponentsInformationsStyle.fontSize = (int)lastResultsViewModel.profilePicturesSize * 15 / 100;
		lastResultsViewModel.opponentsInformationsStyle.fixedHeight = (int)lastResultsViewModel.profilePicturesSize * 17 / 100;
		lastResultsViewModel.opponentsInformationsStyle.fixedWidth = 2*(int)lastResultsViewModel.profilePicturesSize;
		
		for(int i=0;i<lastResultsViewModel.profilePictureButtonStyle.Count;i++){
			lastResultsViewModel.profilePictureButtonStyle[i].fixedWidth = lastResultsViewModel.profilePicturesSize;
			lastResultsViewModel.profilePictureButtonStyle[i].fixedHeight = lastResultsViewModel.profilePicturesSize;
		}

		lastOpponentViewModel.lastOponnentInformationsLabelStyle.fontSize = (int)lastOpponentViewModel.profilePictureSize * 8 / 100;
		lastOpponentViewModel.lastOponnentInformationsLabelStyle.fixedHeight = (int)lastOpponentViewModel.profilePictureSize*15/100;
		lastOpponentViewModel.lastOponnentInformationsLabelStyle.fixedWidth = (int)lastOpponentViewModel.profilePictureSize*1.5f;

		lastOpponentViewModel.lastOponnentUsernameLabelStyle.fontSize = (int)lastOpponentViewModel.profilePictureSize * 10 / 100;
		lastOpponentViewModel.lastOponnentUsernameLabelStyle.fixedHeight = (int)lastOpponentViewModel.profilePictureSize*15/100;
		lastOpponentViewModel.lastOponnentUsernameLabelStyle.fixedWidth = (int)lastOpponentViewModel.profilePictureSize*1.5f;

		lastOpponentViewModel.lastOpponentProfilePictureButtonStyle.fixedWidth = (int)lastOpponentViewModel.profilePictureSize;
		lastOpponentViewModel.lastOpponentProfilePictureButtonStyle.fixedHeight = (int)lastOpponentViewModel.profilePictureSize;
		
		lastResultsViewModel.paginationStyle.fontSize = (int)screenConfigurationViewModel.blockBottomRightHeight*3/100;
		lastResultsViewModel.paginationStyle.fixedWidth = (int)screenConfigurationViewModel.blockBottomRightWidth*10/100;
		lastResultsViewModel.paginationStyle.fixedHeight = (int)screenConfigurationViewModel.blockBottomRightHeight*4/100;
		lastResultsViewModel.paginationActivatedStyle.fontSize = (int)screenConfigurationViewModel.blockBottomRightHeight*3/100;
		lastResultsViewModel.paginationActivatedStyle.fixedWidth = (int)screenConfigurationViewModel.blockBottomRightWidth*10/100;
		lastResultsViewModel.paginationActivatedStyle.fixedHeight = (int)screenConfigurationViewModel.blockBottomRightHeight*4/100;

		switch(model.lastResults[0].GameType)
		{
		case 0:
			friendlyBoardViewModel.mainLabelStyle.fontSize = (int)screenConfigurationViewModel.blockTopLeftHeight * 10 / 100;
			friendlyBoardViewModel.mainLabelStyle.fixedHeight = (int)screenConfigurationViewModel.blockTopLeftHeight * 15 / 100;
			
			friendlyBoardViewModel.subMainLabelStyle.fontSize = (int)screenConfigurationViewModel.blockTopLeftHeight * 7 / 100;
			friendlyBoardViewModel.subMainLabelStyle.fixedHeight = (int)screenConfigurationViewModel.blockTopLeftHeight * 15 / 100;
			break;
		case 1:
			divisionBoardViewModel.divisionLabelStyle.fontSize = screenConfigurationViewModel.heightScreen * 2 / 100;
			divisionBoardViewModel.divisionLabelStyle.fixedHeight = (int)screenConfigurationViewModel.heightScreen * 35 / 1000;
			
			divisionBoardViewModel.divisionStrikeLabelStyle.fontSize= (int)screenConfigurationViewModel.blockTopLeftHeight * 4 / 100;
			divisionBoardViewModel.divisionStrikeLabelStyle.fixedHeight = (int)screenConfigurationViewModel.blockTopLeftHeight * 5 / 100;
			
			divisionBoardViewModel.remainingGamesStyle.fontSize= (int)screenConfigurationViewModel.blockTopLeftHeight * 4 / 100;
			divisionBoardViewModel.remainingGamesStyle.fixedHeight = (int)screenConfigurationViewModel.blockTopLeftHeight * 5 / 100;
			
			divisionBoardViewModel.promotionPrizeLabelStyle.fontSize= (int)screenConfigurationViewModel.blockTopLeftHeight * 4 / 100;
			divisionBoardViewModel.promotionPrizeLabelStyle.fixedHeight = (int)screenConfigurationViewModel.blockTopLeftHeight * 5 / 100;
			
			divisionBoardViewModel.titlePrizeLabelStyle.fontSize= (int)screenConfigurationViewModel.blockTopLeftHeight * 4 / 100;
			divisionBoardViewModel.titlePrizeLabelStyle.fixedHeight = (int)screenConfigurationViewModel.blockTopLeftHeight * 5 / 100;

			divisionBoardViewModel.gaugeWidth = screenConfigurationViewModel.blockTopLeftWidth*0.9f;
			divisionBoardViewModel.gaugeHeight = screenConfigurationViewModel.blockTopLeftHeight * 0.3f;

			divisionBoardViewModel.gaugeBackgroundStyle.fixedWidth = divisionBoardViewModel.gaugeWidth;
			divisionBoardViewModel.gaugeBackgroundStyle.fixedHeight = divisionBoardViewModel.gaugeHeight;

			divisionBoardViewModel.relegationLabelStyle.fixedWidth = 5f / 100f * divisionBoardViewModel.gaugeWidth;
			divisionBoardViewModel.relegationLabelStyle.fontSize = 4 / 100 * (int)screenConfigurationViewModel.blockTopLeftHeight;
			divisionBoardViewModel.relegationLabelStyle.fixedHeight = 5f / 100f * screenConfigurationViewModel.blockTopLeftHeight;
			
			divisionBoardViewModel.promotionLabelStyle.fixedWidth = 5f / 100f * divisionBoardViewModel.gaugeWidth;
			divisionBoardViewModel.promotionLabelStyle.fontSize = 4 / 100 * (int)screenConfigurationViewModel.blockTopLeftHeight;
			divisionBoardViewModel.promotionLabelStyle.fixedHeight = 5f / 100f * screenConfigurationViewModel.blockTopLeftHeight;
			
			divisionBoardViewModel.titleLabelStyle.fixedWidth = 5f / 100f * divisionBoardViewModel.gaugeWidth;
			divisionBoardViewModel.titleLabelStyle.fontSize = 4 / 100 * (int)screenConfigurationViewModel.blockTopLeftHeight;
			divisionBoardViewModel.titleLabelStyle.fixedHeight = 5f / 100f * screenConfigurationViewModel.blockTopLeftHeight;
			
			divisionBoardViewModel.relegationValueLabelStyle.fixedWidth = 5f / 100f * divisionBoardViewModel.gaugeWidth;
			divisionBoardViewModel.relegationValueLabelStyle.fontSize = 4 / 100 * (int)screenConfigurationViewModel.blockTopLeftHeight;
			divisionBoardViewModel.relegationValueLabelStyle.fixedHeight = 5f / 100f * screenConfigurationViewModel.blockTopLeftHeight;
			
			divisionBoardViewModel.promotionValueLabelStyle.fixedWidth = 5f / 100f * divisionBoardViewModel.gaugeWidth;
			divisionBoardViewModel.promotionValueLabelStyle.fontSize = 4 / 100 * (int)screenConfigurationViewModel.blockTopLeftHeight;
			divisionBoardViewModel.promotionValueLabelStyle.fixedHeight = 5f / 100f * screenConfigurationViewModel.blockTopLeftHeight;
			
			divisionBoardViewModel.titleValueLabelStyle.fixedWidth = 5f / 100f * divisionBoardViewModel.gaugeWidth;
			divisionBoardViewModel.titleValueLabelStyle.fontSize = 4 / 100 * (int)screenConfigurationViewModel.blockTopLeftHeight;
			divisionBoardViewModel.titleValueLabelStyle.fixedHeight = 5f / 100f * screenConfigurationViewModel.blockTopLeftHeight;

			divisionBoardViewModel.startActiveGaugeBackgroundStyle.fixedHeight = divisionBoardViewModel.gaugeHeight;
			divisionBoardViewModel.startActiveGaugeBackgroundStyle.fontSize = (int)divisionBoardViewModel.gaugeHeight * 50 / 100;

			divisionBoardViewModel.activeGaugeBackgroundStyle.fixedHeight = divisionBoardViewModel.gaugeHeight;

			divisionBoardViewModel.relegationBarStyle.fixedHeight = divisionBoardViewModel.gaugeHeight;

			divisionBoardViewModel.promotionBarStyle.fixedHeight = divisionBoardViewModel.gaugeHeight;

			divisionBoardViewModel.titleBarStyle.fixedHeight = divisionBoardViewModel.gaugeHeight;

			break;
		case 2:
			for(int i=0;i<cupBoardViewModel.roundsStyle.Count;i++)
			{
				cupBoardViewModel.roundsStyle[i].fixedHeight=(int)screenConfigurationViewModel.blockTopLeftHeight*50/(100*cupBoardViewModel.roundsStyle.Count);
				cupBoardViewModel.roundsStyle[i].fontSize=(int)cupBoardViewModel.roundsStyle[i].fixedHeight*60/100;
			}
			break;
		}
	}
	private void computeGauge(){
		
		if(this.transformRatio>=1f)
		{
			this.transformRatio=1f;
			this.toUpdateGauge=false;
			if(divisionBoardViewModel.title)
			{
				endGameView.setTitlePopUp(true);
			}
			else if(divisionBoardViewModel.promotion)
			{
				endGameView.setPromotionPopUp(true);
			}
			else if(divisionBoardViewModel.endSeason)
			{
				endGameView.setEndSeasonPopUp(true);
			}
			else if(divisionBoardViewModel.relegation)
			{
				endGameView.setRelegationPopUp(true);
			}
		}
		if(divisionBoardViewModel.activeGaugeWidthStart!=divisionBoardViewModel.activeGaugeWidthFinish)
		{
			divisionBoardViewModel.activeGaugeWidth=
				divisionBoardViewModel.activeGaugeWidthStart
					+this.transformRatio*(divisionBoardViewModel.activeGaugeWidthFinish-divisionBoardViewModel.activeGaugeWidthStart);
		}
		if(divisionBoardViewModel.gaugeSpace1Start!=divisionBoardViewModel.gaugeSpace1Finish)
		{
			divisionBoardViewModel.gaugeSpace1=
				divisionBoardViewModel.gaugeSpace1Start+
					this.transformRatio*(divisionBoardViewModel.gaugeSpace1Finish-divisionBoardViewModel.gaugeSpace1Start);
		}
		if(divisionBoardViewModel.gaugeSpace2Start!=divisionBoardViewModel.gaugeSpace2Finish)
		{
			divisionBoardViewModel.gaugeSpace2=
				divisionBoardViewModel.gaugeSpace2Start+
					this.transformRatio*(divisionBoardViewModel.gaugeSpace2Finish-divisionBoardViewModel.gaugeSpace2Start);
		}
		if(divisionBoardViewModel.gaugeSpace3Start!=divisionBoardViewModel.gaugeSpace3Finish)
		{
			divisionBoardViewModel.gaugeSpace3=
				divisionBoardViewModel.gaugeSpace3Start+
					transformRatio*(divisionBoardViewModel.gaugeSpace3Finish-divisionBoardViewModel.gaugeSpace3Start);
		}
		if(divisionBoardViewModel.relegationBarWidth!=divisionBoardViewModel.relegationBarFinish && this.transformRatio==1f)
		{
			divisionBoardViewModel.activeGaugeWidth=divisionBoardViewModel.activeGaugeWidth+divisionBoardViewModel.relegationBarWidth;
			divisionBoardViewModel.activeGaugeBackgroundStyle.normal.background=divisionBoardViewModel.gaugeBackgrounds[1];
			divisionBoardViewModel.relegationBarWidth=divisionBoardViewModel.relegationBarFinish;
		}
		if(divisionBoardViewModel.promotionBarWidth!=divisionBoardViewModel.promotionBarFinish && this.transformRatio==1f)
		{
			divisionBoardViewModel.activeGaugeWidth=divisionBoardViewModel.activeGaugeWidth+divisionBoardViewModel.promotionBarWidth;
			divisionBoardViewModel.activeGaugeBackgroundStyle.normal.background=divisionBoardViewModel.gaugeBackgrounds[2];
			divisionBoardViewModel.promotionBarWidth=divisionBoardViewModel.promotionBarFinish;
		}
		if(divisionBoardViewModel.titleBarWidth!=divisionBoardViewModel.titleBarFinish && this.transformRatio==1f)
		{
			divisionBoardViewModel.activeGaugeWidth=divisionBoardViewModel.activeGaugeWidth+divisionBoardViewModel.titleBarWidth;
			divisionBoardViewModel.activeGaugeBackgroundStyle.normal.background=divisionBoardViewModel.gaugeBackgrounds[3];
			divisionBoardViewModel.titleBarWidth=divisionBoardViewModel.titleBarFinish;
		}
	}
	private void picturesInitialization(){

		lastResultsViewModel.profilePictures =new Texture2D[lastResultsViewModel.lastResults.Count];

		for(int i =0;i<lastResultsViewModel.lastResults.Count;i++)
		{
			lastResultsViewModel.profilePictures[i]=new Texture2D(lastResultsViewModel.profilePicturesSize,
			                                                      lastResultsViewModel.profilePicturesSize, 
			                                                      TextureFormat.ARGB32, 
			                                                      false);
			lastResultsViewModel.profilePictureButtonStyle.Add(new GUIStyle());
			lastResultsViewModel.profilePictureButtonStyle[i].normal.background=lastResultsViewModel.profilePictures[i];
			lastResultsViewModel.profilePictureButtonStyle[i].fixedWidth = lastResultsViewModel.profilePicturesSize;
			lastResultsViewModel.profilePictureButtonStyle[i].fixedHeight = lastResultsViewModel.profilePicturesSize;
		}

		lastOpponentViewModel.lastOpponentProfilePictureButtonStyle.normal.background = lastResultsViewModel.profilePictures[0];
		lastResultsViewModel.nbPages=Mathf.CeilToInt(lastResultsViewModel.lastResults.Count/5f);
		lastResultsViewModel.pageDebut=0;

		if (lastResultsViewModel.nbPages>5){
			lastResultsViewModel.pageFin = 4 ;
		}
		else{
			lastResultsViewModel.pageFin = lastResultsViewModel.nbPages ;
		}

		lastResultsViewModel.paginatorGuiStyle = new GUIStyle[lastResultsViewModel.nbPages];
		for (int i = 0; i < lastResultsViewModel.nbPages; i++) 
		{ 
			if (i==0){
				lastResultsViewModel.paginatorGuiStyle[i]=this.paginationActivatedStyle;
			}
			else{
				lastResultsViewModel.paginatorGuiStyle[i]=this.paginationStyle;
			}
		}
	}
	public void paginationBehaviour(int value, int page=0){
		lastResultsViewModel.paginationBehaviour (value,page);
		}
	private void initializeRounds(){
		for (int i=0; i<model.currentCup.NbRounds;i++)
		{
			cupBoardViewModel.roundsStyle.Add(new GUIStyle());
			if(i<lastResultsViewModel.lastResults.Count-1)
			{
				cupBoardViewModel.roundsStyle[i]=winRoundStyle;
			}
			else if(i==lastResultsViewModel.lastResults.Count-1)
			{
				if(model.lastResults[0].HasWon)
				{
					cupBoardViewModel.roundsStyle[i]=winRoundStyle;
				}
				else
				{
					cupBoardViewModel.roundsStyle[i]=looseRoundStyle;
				}
			}
			else
			{
				cupBoardViewModel.roundsStyle[i]=notPlayedRoundStyle;
			}
			cupBoardViewModel.roundsStyle[i].fixedHeight=(int)screenConfigurationViewModel.blockTopLeftHeight*50/(100*model.currentCup.NbRounds);
			cupBoardViewModel.roundsStyle[i].fontSize=(int)cupBoardViewModel.roundsStyle[i].fixedHeight*60/100;
		}
	}
	private void setProfilePictures(){

		for(int i =0;i<lastResultsViewModel.lastResults.Count;i++)
		{
			if (lastResultsViewModel.lastResults[i].Opponent.Picture.StartsWith(ServerDirectory))
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

		var www = new WWW(ApplicationModel.host + lastResultsViewModel.lastResults[i].Opponent.Picture);
		yield return www;
		www.LoadImageIntoTexture(lastResultsViewModel.profilePictures[i]);
	}
	private IEnumerator loadDefaultProfilePicture(int i){

		var www = new WWW(URLDefaultProfilePicture);
		yield return www;
		www.LoadImageIntoTexture(lastResultsViewModel.profilePictures[i]);
	}
	private void initStyles(){

		screenConfigurationViewModel.centralWindowStyle=this.centralWindowStyle;
		screenConfigurationViewModel.centralWindowTitleStyle=this.centralWindowTitleStyle;
		screenConfigurationViewModel.centralWindowButtonStyle=this.centralWindowButtonStyle;
		screenConfigurationViewModel.blockBorderStyle=this.blockBorderStyle;

		lastResultsViewModel.lastResultsLabelStyle=this.lastResultsLabelStyle;
		lastResultsViewModel.winLabelResultsListStyle=this.winLabelResultsListStyle;
		lastResultsViewModel.defeatLabelResultsListStyle=this.defeatLabelResultsListStyle;
		lastResultsViewModel.opponentsInformationsStyle=this.opponentsInformationsStyle;
		lastResultsViewModel.paginationStyle=this.paginationStyle;
		lastResultsViewModel.paginationActivatedStyle=this.paginationActivatedStyle;
		lastResultsViewModel.winBackgroundResultsListStyle=this.winBackgroundResultsListStyle;
		lastResultsViewModel.defeatBackgroundResultsListStyle=this.defeatBackgroundResultsListStyle;

		lastOpponentViewModel.lastOpponentProfilePictureButtonStyle=this.lastOpponentProfilePictureButtonStyle;
		lastOpponentViewModel.lastOpponentLabelStyle=this.lastOpponentLabelStyle;
		lastOpponentViewModel.lastOponnentInformationsLabelStyle=this.lastOponnentInformationsLabelStyle;
		lastOpponentViewModel.lastOponnentUsernameLabelStyle=this.lastOponnentUsernameLabelStyle;
		lastOpponentViewModel.lastOpponentBackgroundStyle=this.lastOpponentBackgroundStyle;

		currentUserViewModel.rankingLabelStyle=this.rankingLabelStyle;
		currentUserViewModel.yourRankingStyle=this.yourRankingStyle;
		currentUserViewModel.yourRankingPointsStyle=this.yourRankingPointsStyle;

		switch(model.lastResults[0].GameType)
		{
		case 0:
			friendlyBoardViewModel.mainLabelStyle=this.mainLabelStyle;
			friendlyBoardViewModel.subMainLabelStyle=this.subMainLabelStyle;
			break;
		case 1:
			divisionBoardViewModel.activeGaugeBackgroundStyle=this.activeGaugeBackgroundStyle;
			divisionBoardViewModel.gaugeBackgroundStyle=this.gaugeBackgroundStyle;
			divisionBoardViewModel.startActiveGaugeBackgroundStyle=this.startActiveGaugeBackgroundStyle;
			divisionBoardViewModel.relegationBarStyle=this.relegationBarStyle;
			divisionBoardViewModel.promotionBarStyle=this.promotionBarStyle;
			divisionBoardViewModel.titleBarStyle=this.titleBarStyle;
			divisionBoardViewModel.divisionLabelStyle=this.divisionLabelStyle;
			divisionBoardViewModel.divisionStrikeLabelStyle=this.divisionStrikeLabelStyle;
			divisionBoardViewModel.remainingGamesStyle=this.remainingGamesStyle;
			divisionBoardViewModel.promotionPrizeLabelStyle=this.promotionPrizeLabelStyle;
			divisionBoardViewModel.titlePrizeLabelStyle=this.titlePrizeLabelStyle;
			divisionBoardViewModel.relegationLabelStyle=this.relegationLabelStyle;
			divisionBoardViewModel.promotionLabelStyle=this.promotionLabelStyle;
			divisionBoardViewModel.titleLabelStyle=this.titleLabelStyle;
			divisionBoardViewModel.relegationValueLabelStyle=this.relegationValueLabelStyle;
			divisionBoardViewModel.promotionValueLabelStyle=this.promotionValueLabelStyle;
			divisionBoardViewModel.titleValueLabelStyle=this.titleValueLabelStyle;
			break;
		case 2:
			cupBoardViewModel.cupLabelStyle=this.cupLabelStyle;
			cupBoardViewModel.cupPrizeLabelStyle=this.cupPrizeLabelStyle;
			break;
		}
	}
}

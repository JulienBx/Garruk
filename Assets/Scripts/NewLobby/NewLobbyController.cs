using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class NewLobbyController : MonoBehaviour
{
	public static NewLobbyController instance;
	private NewLobbyModel model;
	
	public GameObject blockObject;
	
	private GameObject mainBlock;
	private GameObject mainBlockTitle;
	private GameObject mainBlockSubTitle;
	private GameObject competitionBlock;
	private GameObject competitionBlockTitle;
	private GameObject lastResultsBlock;
	private GameObject lastResultsBlockTitle;
	private GameObject statsBlock;
	private GameObject statsBlockTitle;
	private GameObject popUp;
	private GameObject profilePopUp;
	private GameObject menu;
	private GameObject tutorial;
	private GameObject[] results;
	private GameObject[] stats;
	private GameObject competitionPicture;
	private GameObject competitionDescription;
	private GameObject playButton;
	private GameObject divisionProgression;
	private GameObject cupProgression;
	private GameObject paginationButtons;
	private GameObject mainCamera;
	private GameObject menuCamera;
	private GameObject tutorialCamera;
	private GameObject backgroundCamera;
	
	private IList<int> resultsDisplayed;
	
	private Pagination pagination;

	private bool isSceneLoaded;

	private bool isPopUpDisplayed;

	private bool isDivisionLobby;
	private bool isEndGameLobby;
	private bool hasWonLastGame;

	private bool waitForPopUp;
	private float timer;

	private bool isEndCompetition;
	private bool isScrolling;
	
	void Update()
	{	
		if(this.waitForPopUp)
		{
			this.timer=timer+Time.deltaTime;
			if(this.timer>2.5f)
			{
				this.waitForPopUp=false;
				this.displayPopUp();
			}
		}
		if(ApplicationDesignRules.isMobileScreen && this.isSceneLoaded)
		{
			isScrolling = this.mainCamera.GetComponent<ScrollingController>().ScrollController();
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewLobbyModel ();
		if(ApplicationModel.gameType==1)
		{
			this.isDivisionLobby=true;
		}
		if(ApplicationModel.launchEndGameSequence)
		{
			this.isEndGameLobby=true;
			ApplicationModel.launchEndGameSequence=false;
		}
		if(ApplicationModel.hasWonLastGame)
		{
			this.hasWonLastGame=true;
			ApplicationModel.hasWonLastGame=false;
		}
		this.timer = 0f;
		this.pagination = new Pagination ();
		this.pagination.nbElementsPerPage= 3;
		this.initializeScene ();
		this.startMenuInitialization ();
	}
	private void startMenuInitialization()
	{
		this.menu = GameObject.Find ("Menu");
		this.menu.AddComponent<LobbyMenuController> ();
	}
	public void endMenuInitialization()
	{
		this.startTutorialInitialization ();
	}
	private void startTutorialInitialization()
	{
		this.tutorial = GameObject.Find ("Tutorial");
		this.tutorial.AddComponent<LobbyTutorialController>();
	}
	public void endTutorialInitialization()
	{
		StartCoroutine(this.initialization ());
	}
	public IEnumerator initialization()
	{
		this.resize ();
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.getLobbyData(this.isDivisionLobby,this.isEndGameLobby));
		this.initializeResults ();
		this.initializeCompetitions ();
		this.initializeStats ();
		if(this.isDivisionLobby)
		{
			this.drawGauge();
		}
		else
		{
			this.drawCup();
		}
		if(this.isEndGameLobby)
		{
			this.initializePopUp ();
		}
		this.initializePlayButton ();
		this.isSceneLoaded = true;
		MenuController.instance.hideLoadingScreen ();
		if(model.player.TutorialStep!=-1)
		{
			TutorialObjectController.instance.startTutorial(model.player.TutorialStep,model.player.displayTutorial);
		}
		else if(model.player.displayTutorial&&!model.player.LobbyTutorial)
		{
			TutorialObjectController.instance.startHelp();
		}
	}
	private void initializeResults()
	{
		this.pagination.chosenPage = 0;
		this.pagination.totalElements= model.lastResults.Count;
		this.paginationButtons.GetComponent<NewLobbyPaginationController> ().p = this.pagination;
		this.paginationButtons.GetComponent<NewLobbyPaginationController> ().setPagination ();
		this.drawResults ();
	}
	public void paginationHandler()
	{
		this.drawResults ();
	}
	private void initializeCompetitions()
	{
		this.drawCompetition ();
	}
	private void initializeStats()
	{
		this.drawStats ();
	}
	public void initializeScene()
	{
		this.mainBlock = Instantiate(this.blockObject) as GameObject;
		this.mainBlockTitle = GameObject.Find ("MainBlockTitle");
		this.mainBlockTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.playButton = GameObject.Find ("PlayButton");
		this.playButton.AddComponent<NewLobbyPlayButtonController> ();
		this.statsBlock = Instantiate(this.blockObject) as GameObject;
		this.statsBlockTitle = GameObject.Find ("StatsBlockTitle");
		this.statsBlockTitle.GetComponent<TextMeshPro> ().text = "Statistiques";
		this.statsBlockTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.lastResultsBlock = Instantiate(this.blockObject) as GameObject;
		this.lastResultsBlockTitle = GameObject.Find ("LastResultsBlockTitle");
		this.lastResultsBlockTitle.GetComponent<TextMeshPro> ().text = "Résultats";
		this.lastResultsBlockTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.competitionBlock = Instantiate(this.blockObject) as GameObject;
		this.competitionBlockTitle = GameObject.Find ("CompetitionBlockTitle");
		this.competitionBlockTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.competitionBlockTitle.GetComponent<TextMeshPro>().text="Récompenses";
		this.paginationButtons = GameObject.Find ("Pagination");
		this.paginationButtons.AddComponent<NewLobbyPaginationController> ();
		this.paginationButtons.GetComponent<NewLobbyPaginationController> ().initialize ();
		this.results=new GameObject[3];
		for(int i=0;i<this.results.Length;i++)
		{
			this.results[i]=GameObject.Find ("Result"+i);
			this.results[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.results[i].transform.FindChild("username").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.results[i].transform.FindChild("username").gameObject.AddComponent<NewLobbyResultsUsernameController>();
			this.results[i].transform.FindChild("username").GetComponent<NewLobbyResultsUsernameController>().setId(i);
			this.results[i].transform.FindChild("picture").gameObject.AddComponent<NewLobbyResultsPictureController>();
			this.results[i].transform.FindChild("picture").GetComponent<NewLobbyResultsPictureController>().setId(i);
			this.results[i].transform.FindChild("line").GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
			this.results[i].SetActive(false);
		}
		this.stats = new GameObject[4];
		for(int i=0;i<this.stats.Length;i++)
		{
			this.stats[i]=GameObject.Find ("Stat"+i);
			this.stats[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		this.stats[0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Victoires";
		this.stats[0].transform.FindChild ("Subvalue").gameObject.SetActive (false);
		this.stats[1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Défaites";
		this.stats[1].transform.FindChild ("Subvalue").gameObject.SetActive (false);
		this.stats[2].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Classement combattant";
		this.stats[3].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= "Classement collectionneur";
		this.popUp = GameObject.Find ("PopUp");
		this.popUp.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Continuer";
		this.popUp.SetActive (false);
		this.profilePopUp = GameObject.Find ("ProfilePopUp");
		this.profilePopUp.SetActive (false);
		this.competitionPicture = GameObject.Find ("CompetitionPicture");
		this.competitionPicture.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.competitionDescription = GameObject.Find ("CompetitionDescription");
		this.competitionDescription.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.divisionProgression = GameObject.Find ("DivisionProgression");
		this.cupProgression = GameObject.Find ("CupProgression");
		this.mainBlockSubTitle=GameObject.Find ("MainBlockSubTitle");
		if(this.isDivisionLobby)
		{
			this.divisionProgression.SetActive(true);
			this.cupProgression.SetActive (false);
			this.divisionProgression.GetComponent<DivisionProgressionController>().initialize();
			this.mainBlockSubTitle.SetActive(true);
			this.mainBlockSubTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		else
		{
			this.divisionProgression.SetActive(false);
			this.mainBlockSubTitle.SetActive(false);
			this.cupProgression.SetActive(true);
			this.cupProgression.transform.FindChild("RemainingRounds").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.cupProgression.transform.FindChild("Status").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}

		this.mainCamera = gameObject;
		this.mainCamera.AddComponent<ScrollingController> ();
		this.menuCamera = GameObject.Find ("MenuCamera");
		this.tutorialCamera = GameObject.Find ("TutorialCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
	}
	public void resize()
	{
		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.menuCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.tutorialCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;

		float mainBlockLeftMargin;
		float mainBlockUpMargin;
		float mainBlockHeight;
		
		float statsBlockLeftMargin;
		float statsBlockUpMargin;
		float statsBlockHeight;
		
		float lastResultsBlockLeftMargin;
		float lastResultsBlockUpMargin;
		float lastResultsBlockHeight;
		
		float competitionBlockLeftMargin;
		float competitionBlockUpMargin;
		float competitionBlockHeight;
		
		mainBlockHeight=ApplicationDesignRules.mediumBlockHeight;
		statsBlockHeight=ApplicationDesignRules.smallBlockHeight;
		lastResultsBlockHeight=ApplicationDesignRules.mediumBlockHeight;
		competitionBlockHeight=ApplicationDesignRules.smallBlockHeight;
		
		if(ApplicationDesignRules.isMobileScreen)
		{
			mainBlockLeftMargin=ApplicationDesignRules.leftMargin;
			mainBlockUpMargin=0f;
			
			statsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			statsBlockUpMargin=mainBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+mainBlockHeight;
			
			lastResultsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			lastResultsBlockUpMargin=statsBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+statsBlockHeight;
			
			competitionBlockLeftMargin=ApplicationDesignRules.leftMargin;
			competitionBlockUpMargin=lastResultsBlockUpMargin+lastResultsBlockHeight;
		}
		else
		{
			mainBlockLeftMargin=ApplicationDesignRules.leftMargin;
			mainBlockUpMargin=ApplicationDesignRules.upMargin;
			
			statsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			statsBlockUpMargin=mainBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+mainBlockHeight;
			
			lastResultsBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			lastResultsBlockUpMargin=ApplicationDesignRules.upMargin;
			
			competitionBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			competitionBlockUpMargin=lastResultsBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+lastResultsBlockHeight;
		}

		this.mainCamera.GetComponent<ScrollingController> ().setViewHeight(ApplicationDesignRules.viewHeight);
		this.mainCamera.GetComponent<ScrollingController> ().setContentHeight(mainBlockHeight + statsBlockHeight + lastResultsBlockHeight + competitionBlockHeight + 3f * ApplicationDesignRules.gapBetweenBlocks);
		//this.mainCamera.transform.position = ApplicationDesignRules.mainCameraStartPosition;
		//this.mainCamera.GetComponent<ScrollingController> ().setStartPositionY (ApplicationDesignRules.mainCameraStartPosition.y);
		this.mainCamera.GetComponent<ScrollingController> ().setEndPositionY ();

		this.mainBlock.GetComponent<NewBlockController> ().resize(mainBlockLeftMargin,mainBlockUpMargin,ApplicationDesignRules.blockWidth,mainBlockHeight);
		Vector3 mainBlockUpperLeftPosition = this.mainBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 mainBlockUpperRightPosition = this.mainBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 mainBlockLowerLeftPosition = this.mainBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 mainBlockLowerRightPosition = this.mainBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 mainBlockOriginPosition = this.mainBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		Vector2 mainBlockSize = this.mainBlock.GetComponent<NewBlockController> ().getSize ();

		this.mainBlockTitle.transform.position = new Vector3 (mainBlockUpperLeftPosition.x + 0.3f, mainBlockUpperLeftPosition.y - 0.2f, 0f);
		this.mainBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.mainBlockSubTitle.transform.localScale=ApplicationDesignRules.subMainTitleScale;
		this.mainBlockSubTitle.transform.position = new Vector3 (0.3f+mainBlockUpperLeftPosition.x, mainBlockUpperLeftPosition.y - 1.2f, 0f);

		this.playButton.transform.localScale = ApplicationDesignRules.button62Scale;
		this.playButton.transform.position = new Vector3 (mainBlockUpperRightPosition.x -0.3f-ApplicationDesignRules.button62WorldSize.x/2f, mainBlockUpperLeftPosition.y - 0.3f - ApplicationDesignRules.button62WorldSize.y/2f, 0f);


		this.competitionBlock.GetComponent<NewBlockController> ().resize(competitionBlockLeftMargin,competitionBlockUpMargin,ApplicationDesignRules.blockWidth,competitionBlockHeight);
		Vector3 competitionBlockUpperLeftPosition = this.competitionBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 competitionBlockUpperRightPosition = this.competitionBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 competitionBlockLowerLeftPosition = this.competitionBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 competitionBlockLowerRightPosition = this.competitionBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 competitionBlockSize = this.competitionBlock.GetComponent<NewBlockController> ().getSize ();

		this.competitionBlockTitle.transform.position = new Vector3 (competitionBlockUpperLeftPosition.x + 0.3f, competitionBlockUpperLeftPosition.y - 0.2f, 0f);
		this.competitionBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.competitionPicture.transform.localScale = ApplicationDesignRules.competitionScale;
		this.competitionPicture.transform.position = new Vector3 (competitionBlockUpperLeftPosition.x + 0.3f + ApplicationDesignRules.competitionWorldSize.x / 2f, competitionBlockUpperLeftPosition.y - 2f, 0f);

		this.competitionDescription.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.competitionDescription.transform.position = new Vector3 (competitionBlockLowerLeftPosition.x + 0.3f + ApplicationDesignRules.competitionWorldSize.x + 0.1f, this.competitionPicture.transform.position.y, 0f);
		this.competitionDescription.GetComponent<TextContainer> ().width = (competitionBlockSize.x - 0.6f - 0.1f - ApplicationDesignRules.competitionWorldSize.x)*1/(ApplicationDesignRules.reductionRatio);

		this.statsBlock.GetComponent<NewBlockController> ().resize(statsBlockLeftMargin,statsBlockUpMargin,ApplicationDesignRules.blockWidth,competitionBlockHeight);
		Vector3 statsBlockUpperLeftPosition = this.statsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 statsBlockLowerLeftPosition = this.statsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector3 statsBlockUpperRightPosition = this.statsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 statsBlockSize = this.statsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 statsOrigin = this.statsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.statsBlockTitle.transform.position = new Vector3 (statsBlockUpperLeftPosition.x + 0.3f, statsBlockUpperLeftPosition.y - 0.2f, 0f);
		this.statsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		Vector3 statsScale = new Vector3 (1f, 1f, 1f);
		Vector2 statBlockSize = new Vector2 ((statsBlockSize.x - 0.6f) / 4f, statsBlockSize.y - 0.5f);
		
		for(int i=0;i<this.stats.Length;i++)
		{
			this.stats[i].transform.position=new Vector3(statsBlockLowerLeftPosition.x+0.3f+statBlockSize.x/2f+i*statBlockSize.x,statsBlockUpperLeftPosition.y-0.8f-statBlockSize.y/2f);
			this.stats[i].transform.localScale= ApplicationDesignRules.reductionRatio*statsScale;
			this.stats[i].transform.FindChild("Title").GetComponent<TextContainer>().width=statBlockSize.x;
		}

		this.lastResultsBlock.GetComponent<NewBlockController> ().resize(lastResultsBlockLeftMargin,lastResultsBlockUpMargin,ApplicationDesignRules.blockWidth,lastResultsBlockHeight);
		Vector3 lastResultsBlockUpperLeftPosition = this.lastResultsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 lastResultsBlockLowerLeftPosition = this.lastResultsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector3 lastResultsBlockUpperRightPosition = this.lastResultsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 lastResultsBlockSize = this.lastResultsBlock.GetComponent<NewBlockController> ().getSize ();
		this.lastResultsBlockTitle.transform.position = new Vector3 (lastResultsBlockUpperLeftPosition.x + 0.3f, lastResultsBlockUpperLeftPosition.y - 0.2f, 0f);
		this.lastResultsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		

		Vector2 lastResultBlockSize = new Vector2 (lastResultsBlockSize.x - 0.6f, (lastResultsBlockSize.y - 1f - 0.6f)/this.results.Length);
		float resultsLineScale = ApplicationDesignRules.getLineScale (lastResultsBlockSize.x);
		
		for(int i=0;i<this.results.Length;i++)
		{
			this.results[i].transform.position=new Vector3(lastResultsBlockUpperLeftPosition.x+0.3f+lastResultBlockSize.x/2f,lastResultsBlockUpperLeftPosition.y-1f-(i+1)*lastResultBlockSize.y,0f);
			this.results[i].transform.FindChild("line").localScale=new Vector3(resultsLineScale,1f,1f);
			this.results[i].transform.FindChild("picture").localScale=ApplicationDesignRules.thumbScale;
			this.results[i].transform.FindChild("picture").localPosition=new Vector3(-lastResultBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(lastResultBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.results[i].transform.FindChild("username").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.results[i].transform.FindChild("username").GetComponent<TextMeshPro>().textContainer.width=(lastResultBlockSize.x/2f)-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.results[i].transform.FindChild("username").localPosition=new Vector3(-lastResultBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,lastResultBlockSize.y-(lastResultBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.results[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.results[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=0.75f*lastResultBlockSize.x-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.results[i].transform.FindChild("description").localPosition=new Vector3(-lastResultBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,lastResultBlockSize.y/2f,0f);
		}

		this.paginationButtons.transform.localPosition=new Vector3 (lastResultsBlockLowerLeftPosition.x + lastResultBlockSize.x / 2, lastResultsBlockLowerLeftPosition.y + 0.3f, 0f);
		this.paginationButtons.GetComponent<NewLobbyPaginationController> ().resize ();

		this.popUp.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x+0, ApplicationDesignRules.menuPosition.y+2f, -3f);

		TutorialObjectController.instance.resize ();

		if(this.isDivisionLobby)
		{
			this.divisionProgression.GetComponent<DivisionProgressionController>().resize(new Rect(mainBlockOriginPosition.x,mainBlockOriginPosition.y,mainBlockSize.x,mainBlockSize.y));
		}
		else
		{
			this.cupProgression.GetComponent<CupProgressionController>().resize(new Rect(mainBlockOriginPosition.x,mainBlockOriginPosition.y,mainBlockSize.x,mainBlockSize.y));
		}
	}
	public void returnPressed()
	{
		if(this.isPopUpDisplayed)
		{
			this.hidePopUp();
		}
	}
	public void escapePressed()
	{
		if(this.isPopUpDisplayed)
		{
			this.hidePopUp();
		}
		else
		{
			MenuController.instance.leaveGame();
		}
	}
	public void closeAllPopUp()
	{
		if(this.isPopUpDisplayed)
		{
			this.hidePopUp();
		}
	}
	public void drawResults()
	{
		this.resultsDisplayed = new List<int> ();
		for(int i =0;i<this.pagination.nbElementsPerPage;i++)
		{
			if(this.pagination.chosenPage*this.pagination.nbElementsPerPage+i<model.lastResults.Count)
			{
				this.resultsDisplayed.Add (this.pagination.chosenPage*this.pagination.nbElementsPerPage+i);
				this.results[i].SetActive(true);


				string description="";
				Color textColor=new Color();
				if(model.lastResults[this.pagination.chosenPage*this.pagination.nbElementsPerPage+i].HasWon)
				{
					description="Victoire le "+model.lastResults[this.pagination.chosenPage*this.pagination.nbElementsPerPage+i].Date.ToString("dd/MM/yyyy");
					textColor=ApplicationDesignRules.blueColor;
				}
				else
				{
					description="Défaite le "+model.lastResults[this.pagination.chosenPage*this.pagination.nbElementsPerPage+i].Date.ToString("dd/MM/yyyy");
					textColor=ApplicationDesignRules.redColor;
				}


				this.results[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=description;
				this.results[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=textColor;
				this.results[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.lastResults[this.pagination.chosenPage*this.pagination.nbElementsPerPage+i].Opponent.idProfilePicture);
				this.results[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.lastResults[this.pagination.chosenPage*this.pagination.nbElementsPerPage+i].Opponent.Username;
				
			}
			else
			{
				this.results[i].SetActive(false);
			}
		}
	}
	public void drawStats()
	{
		this.stats[0].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.player.TotalNbWins.ToString ();
		this.stats[1].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.player.TotalNbLooses.ToString ();
		this.stats[2].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.player.Ranking.ToString ();
		this.stats[2].transform.FindChild ("Subvalue").GetComponent<TextMeshPro> ().text= "("+model.player.RankingPoints.ToString()+" pts)";
		this.stats[3].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= model.player.CollectionRanking.ToString ();
		this.stats[3].transform.FindChild ("Subvalue").GetComponent<TextMeshPro> ().text= "("+model.player.CollectionPoints.ToString()+" pts)";	
	}
	public void drawCompetition()
	{
		if(this.isDivisionLobby)
		{
			this.mainBlockTitle.GetComponent<TextMeshPro>().text=model.currentDivision.Name;
			string description="Hégémonie : "+model.currentDivision.TitlePrize.ToString()+" cristaux";
			if(model.currentDivision.NbWinsForPromotion!=-1)
			{
				description=description+"\nColonisation : "+model.currentDivision.PromotionPrize.ToString()+" cristaux";
			}
			this.competitionDescription.GetComponent<TextMeshPro>().text=description;
			this.competitionPicture.GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnLargeCompetitionPicture(model.currentDivision.IdPicture);
		}
		else
		{
			this.competitionBlockTitle.GetComponent<TextMeshPro>().text=model.currentCup.Name;
			string description="Victoire : "+model.currentCup.CupPrize.ToString()+" cristaux";
			this.competitionDescription.GetComponent<TextMeshPro>().text=description;
			this.competitionPicture.GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnLargeCompetitionPicture(model.currentCup.IdPicture);
		}
	}
	private void displayPopUp()
	{
		this.isPopUpDisplayed = true;
		this.popUp.SetActive (true);
		MenuController.instance.displayTransparentBackground ();
	}
	public void hidePopUp()
	{
		this.isPopUpDisplayed = false;
		this.popUp.SetActive (false);
		MenuController.instance.hideTransparentBackground ();
	}
	public void playHandler()
	{
		if(this.isEndCompetition)
		{
			Application.LoadLevel("NewLobby");
		}
		else
		{
			MenuController.instance.joinRandomRoomHandler();
		}
	}
	public void drawGauge()
	{
		if(this.isEndGameLobby)
		{
			if(hasWonLastGame)
			{
				this.divisionProgression.GetComponent<DivisionProgressionController> ().drawGauge (model.currentDivision,true);
				this.divisionProgression.GetComponent<DivisionProgressionController>().animateGauge();
			}
			else
			{
				this.divisionProgression.GetComponent<DivisionProgressionController> ().drawGauge (model.currentDivision,false);
			}
		}
		else
		{
			this.divisionProgression.GetComponent<DivisionProgressionController> ().drawGauge (model.currentDivision,false);
		}
	}
	public void endGaugeAnimation()
	{
		this.divisionProgression.GetComponent<DivisionProgressionController> ().drawGauge (model.currentDivision,false);
	}
	public void drawCup()
	{
		if(this.isEndGameLobby)
		{
			this.cupProgression.GetComponent<CupProgressionController> ().drawCup (model.currentCup,this.hasWonLastGame);
		}
		else
		{
			this.cupProgression.GetComponent<CupProgressionController> ().drawCup (model.currentCup,true);
		}
	}
	public void initializePopUp()
	{
		bool displayPopUp = false;
		string title = "";
		string content = "";
		if(this.isDivisionLobby)
		{
			if(model.currentDivision.Status==3) // Fin de saison + Promotion + Titre
			{
				title = "Fin de l'exploration";
				content ="Bravo ! Votre domination sur la planète est sans limite ! Commencez dès maintenant l'exploration d'une nouvelle planète !";
				displayPopUp=true;
				this.isEndCompetition=true;
			}
			else if(model.currentDivision.Status==30) // Fin de saison + Titre
			{
				title="Fin de l'exploration";
				content ="Bravo ! Votre domination sur la planète est sans limite ! Prêt à recommencer ?";
				displayPopUp=true;
				this.isEndCompetition=true;
			}
			else if(model.currentDivision.Status==20) // Promotion obtenue au cours du match + Fin de saison
			{
				title = "Fin de l'exploration";
				content="Bravo ! Grâce à cette victoire, vous pouvez dès maintenant commencer l'exploration d'une nouvelle planète !";
				displayPopUp=true;
				this.isEndCompetition=true;
			}
			else if(model.currentDivision.Status==2) // Promotion + Fin de saison
			{
				title = "Fin de l'exploration";
				content="Bravo ! Vous pouvez dès maintenant commencer l'exploration d'une nouvelle planète !";
				displayPopUp=true;
				this.isEndCompetition=true;
			}
			else if(model.currentDivision.Status==21) // Promotion obtenue au cours du match
			{
				title="Félicitations";
				content="Vous pourrez prochainement explorer une nouvelle planète !";
				displayPopUp=true;
			}
			else if(model.currentDivision.Status==10) // Maintien obtenu au cours du match + Fin de saison
			{
				title = "Fin de l'exploration";
				content="Bravo grâce à cette victoire vous pourrez continuer l'exploration de cette planète !";
				displayPopUp=true;
				this.isEndCompetition=true;
			}
			else if(model.currentDivision.Status==1) // Maintien + Fin de saison
			{
				title = "Fin de l'exploration";
				content="Vos efforts ont payé et vous permettent de maintenir votre présence sur cette planète !";
				displayPopUp=true;
				this.isEndCompetition=true;
			}
			else if(model.currentDivision.Status==11) // Maintien obtenu au cours du match
			{
				title="Félicitations";
				content="Votre victoire consolide votre présence sur cette planète !";
				displayPopUp=true;
			}
			else if(model.currentDivision.Status==-1) // Relégation
			{
				title = "Fin de l'exploration";
				content="Malheureusement vos efforts seront insuffisants pour vous maintenir.";
				displayPopUp=true;
			}
		}
		else
		{
			if(model.currentCup.Status==11) // Fin de saison + Promotion + Titre
			{
				title = "Félicitations";
				content ="Bravo ! vous avez remporté la coupe ! Vos résultats en division vous permettent d'accéder à une nouvelle coupe.";
				displayPopUp=true;
				this.isEndCompetition=true;
			}
			else if(model.currentCup.Status==1) // Fin de saison + Titre
			{
				title = "Félicitations";
				content ="Bravo ! vous avez remporté la coupe !";
				displayPopUp=true;
				this.isEndCompetition=true;
			}
			if(model.currentCup.Status==-11) // Fin de saison + Promotion + Titre
			{
				title = "Dommage";
				content ="Vous êtes éliminé... Vos résultats en division vous permettent désormais d'accéder à une nouvelle coupe";
				displayPopUp=true;
				this.isEndCompetition=true;
			}
			else if(model.currentCup.Status==-1) // Fin de saison + Titre
			{
				title = "Dommage";
				content ="Vous êtes élminé...";
				displayPopUp=true;
				this.isEndCompetition=true;
			}
		}
		if(displayPopUp)
		{
			this.waitForPopUp=true;
			this.popUp.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = title;
			this.popUp.transform.FindChild ("Content").GetComponent<TextMeshPro> ().text = content;
		}
	}
	public void initializePlayButton()
	{
		if(this.isEndCompetition)
		{
			this.playButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Continuer";
		}
		else
		{
			this.playButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Jouer";
		}
	}
	public void clickOnResultsProfile(int id)
	{
		ApplicationModel.profileChosen = this.results [id].transform.FindChild ("username").GetComponent<TextMeshPro> ().text;
		Application.LoadLevel("NewProfile");
	}
	public void updateSubMainBlockTitle(string s)
	{
		this.mainBlockSubTitle.GetComponent<TextMeshPro> ().text = s.ToUpper();
	}

	#region TUTORIAL FUNCTIONS
	
	public Vector3 getMainBlockOrigin()
	{
		return this.mainBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getMainBlockSize()
	{
		return this.mainBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getStatsBlockOrigin()
	{
		return this.statsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getStatsBlockSize()
	{
		return this.statsBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getLastResultsBlockOrigin()
	{
		return this.lastResultsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getLastResultsBlockSize()
	{
		return this.lastResultsBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getCompetitionBlockOrigin()
	{
		return this.competitionBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getCompetitionBlockSize()
	{
		return this.competitionBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public IEnumerator endHelp()
	{
		if(!model.player.LobbyTutorial)
		{
			MenuController.instance.displayLoadingScreen();
			yield return StartCoroutine(model.player.setLobbyTutorial(true));
			MenuController.instance.hideLoadingScreen();
		}
	}
	#endregion
}
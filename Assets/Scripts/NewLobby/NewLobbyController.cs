using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.SceneManagement;

public class NewLobbyController : MonoBehaviour
{
	public static NewLobbyController instance;
	private NewLobbyModel model;
	
	public GameObject blockObject;
	public GameObject resultObject;
	
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
	private GameObject backOfficeController;
	private GameObject menu;
	private GameObject help;
	private GameObject[] results;
	private GameObject[] stats;
	private GameObject competitionPicture;
	private GameObject competitionDescription;
	private GameObject playButton;
	private GameObject divisionProgression;
	private GameObject paginationButtons;
	private GameObject mainCamera;
	private GameObject sceneCamera;
	private GameObject helpCamera;
	private GameObject backgroundCamera;
	private GameObject slideLeftButton;
	private GameObject slideRightButton;
	private GameObject statsButton;
	private GameObject lastResultsButton;
	
	private IList<int> resultsDisplayed;
	
	private Pagination pagination;

	private bool isSceneLoaded;

	private bool isPopUpDisplayed;

	private bool isEndGameLobby;
	private bool hasWonLastGame;

	private bool waitForPopUp;
	private float timer;

	private bool isEndCompetition;

	private bool mainContentDisplayed;
	private bool lastResultsDisplayed;
	private bool statsDisplayed;

	private bool toSlideLeft;
	private bool toSlideRight;

	private float mainContentPositionX;
	private float lastResultsPositionX;
	private float statsPositionX;
	private float targetContentPositionX;
	
	void Update()
	{	
		if (Input.touchCount == 1 && this.isSceneLoaded  && HelpController.instance.getCanSwipe() && BackOfficeController.instance.getCanSwipeAndScroll()) 
		{
			if(Input.touches[0].deltaPosition.x<-15f)
			{
				if(this.lastResultsDisplayed || this.mainContentDisplayed || this.toSlideLeft)
				{
					this.slideRight();
					BackOfficeController.instance.setIsSwiping(true);
				}
			}
			if(Input.touches[0].deltaPosition.x>15f)
			{
				if(this.mainContentDisplayed || this.statsDisplayed || this.toSlideRight)
				{
					this.slideLeft();
					BackOfficeController.instance.setIsSwiping(true);
				}
			}
		}
		if(toSlideRight || toSlideLeft)
		{
			Vector3 sceneCameraPosition = this.sceneCamera.transform.position;
			float camerasXPosition = sceneCameraPosition.x;
			if(toSlideRight)
			{
				camerasXPosition=camerasXPosition+Time.deltaTime*40f;
				if(camerasXPosition>this.targetContentPositionX)
				{
					camerasXPosition=this.targetContentPositionX;
					this.toSlideRight=false;
					if(camerasXPosition==this.statsPositionX)
					{
						this.statsDisplayed=true;
					}
					else if(camerasXPosition==this.mainContentPositionX)
					{
						this.mainContentDisplayed=true;
						this.activeGaugeCamera(true);
					}
					BackOfficeController.instance.setIsSwiping(false);
				}
			}
			else if(toSlideLeft)
			{
				camerasXPosition=camerasXPosition-Time.deltaTime*40f;
				if(camerasXPosition<this.targetContentPositionX)
				{
					camerasXPosition=this.targetContentPositionX;
					this.toSlideLeft=false;
					if(camerasXPosition==this.lastResultsPositionX)
					{
						this.lastResultsDisplayed=true;
					}
					else if(camerasXPosition==this.mainContentPositionX)
					{
						this.mainContentDisplayed=true;
						this.activeGaugeCamera(true);
					}
					BackOfficeController.instance.setIsSwiping(false);
				}
			}
			sceneCameraPosition.x=camerasXPosition;
			this.sceneCamera.transform.position=sceneCameraPosition;
		}
		if(this.waitForPopUp)
		{
			this.timer=timer+Time.deltaTime;
			if(this.timer>2.5f)
			{
				this.waitForPopUp=false;
				this.displayPopUp();
			}
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewLobbyModel ();
		if(ApplicationModel.player.ToLaunchEndGameSequence)
		{
			this.isEndGameLobby=true;
			ApplicationModel.player.ToLaunchEndGameSequence=false;
		}
		if(ApplicationModel.player.HasWonLastGame)
		{
			this.hasWonLastGame=true;
			ApplicationModel.player.HasWonLastGame=false;
		}
		this.mainContentDisplayed = true;
		this.timer = 0f;
		this.initializeScene ();
		this.initializeBackOffice();
		this.initializeMenu();
		this.initializeHelp();
		StartCoroutine (this.initialization ());
	}
	private void initializeHelp()
	{
		this.help = GameObject.Find ("HelpController");
		this.help.AddComponent<LobbyHelpController>();
		this.help.GetComponent<LobbyHelpController>().initialize();
		BackOfficeController.instance.setIsHelpLoaded(true);
	}
	private void initializeMenu()
	{
		this.menu = GameObject.Find ("Menu");
		this.menu.AddComponent<MenuController>();
		this.menu.GetComponent<MenuController>().initialize();
		BackOfficeController.instance.setIsMenuLoaded(true);
	}
	private void initializeBackOffice()
	{
		this.backOfficeController = GameObject.Find ("BackOfficeController");
		this.backOfficeController.AddComponent<BackOfficeLobbyController>();
		this.backOfficeController.GetComponent<BackOfficeLobbyController>().initialize();
	}
	public IEnumerator initialization()
	{
		this.resize ();
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.getLobbyData(this.isEndGameLobby));
		this.initializeResults ();
		this.initializeCompetitions ();
		this.initializeStats ();
		this.drawGauge();
		if(this.isEndGameLobby)
		{
			this.initializePopUp ();
		}
		this.initializePlayButton ();
		this.isSceneLoaded = true;
		BackOfficeController.instance.hideLoadingScreen ();
		if(ApplicationModel.player.TutorialStep!=-1)
		{
			HelpController.instance.startTutorial();
		}
		else if(!ApplicationModel.player.LobbyHelp)
		{
			HelpController.instance.startHelp();
		}
	}
	public void initializeResults()
	{
		this.pagination.totalElements= model.lastResults.Count;
		this.paginationButtons.GetComponent<NewLobbyPaginationController> ().p = this.pagination;
		this.paginationButtons.GetComponent<NewLobbyPaginationController> ().setPagination ();
		this.drawResults ();
	}
	public void paginationHandler()
	{
		SoundController.instance.playSound(9);
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
		this.statsBlockTitle.GetComponent<TextMeshPro> ().text = WordingLobby.getReference(0);
		this.statsBlockTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.lastResultsBlock = Instantiate(this.blockObject) as GameObject;
		this.lastResultsBlockTitle = GameObject.Find ("LastResultsBlockTitle");
		this.lastResultsBlockTitle.GetComponent<TextMeshPro> ().text = WordingLobby.getReference(1);
		this.lastResultsBlockTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.competitionBlock = Instantiate(this.blockObject) as GameObject;
		this.competitionBlockTitle = GameObject.Find ("CompetitionBlockTitle");
		this.competitionBlockTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.competitionBlockTitle.GetComponent<TextMeshPro>().text=WordingLobby.getReference(2);
		this.paginationButtons = GameObject.Find ("Pagination");
		this.paginationButtons.AddComponent<NewLobbyPaginationController> ();
		this.paginationButtons.GetComponent<NewLobbyPaginationController> ().initialize ();
		this.results=new GameObject[0];
		this.stats = new GameObject[4];
		for(int i=0;i<this.stats.Length;i++)
		{
			this.stats[i]=GameObject.Find ("Stat"+i);
			this.stats[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		this.stats[0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text=WordingLobby.getReference(3);
		this.stats[0].transform.FindChild ("Subvalue").gameObject.SetActive (false);
		this.stats[1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= WordingLobby.getReference(4);
		this.stats[1].transform.FindChild ("Subvalue").gameObject.SetActive (false);
		this.stats[2].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= WordingLobby.getReference(5);
		this.stats[3].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text= WordingLobby.getReference(6);
		this.popUp = GameObject.Find ("PopUp");
		this.popUp.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingLobby.getReference(7);
		this.popUp.SetActive (false);
		this.competitionPicture = GameObject.Find ("CompetitionPicture");
		this.competitionPicture.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.competitionDescription = GameObject.Find ("CompetitionDescription");
		this.competitionDescription.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.divisionProgression = GameObject.Find ("DivisionProgression");
		this.mainBlockSubTitle=GameObject.Find ("MainBlockSubTitle");
		this.divisionProgression.SetActive(true);
		this.divisionProgression.GetComponent<DivisionProgressionController>().initialize();
		this.mainBlockSubTitle.SetActive(true);
		this.mainBlockSubTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.slideLeftButton = GameObject.Find ("SlideLeftButton");
		this.slideLeftButton.AddComponent<NewLobbySlideLeftButtonController> ();
		this.slideRightButton = GameObject.Find ("SlideRightButton");
		this.slideRightButton.AddComponent<NewLobbySlideRightButtonController> ();
		this.statsButton = GameObject.Find ("StatsButton");
		this.statsButton.AddComponent<NewLobbyStatsButtonController> ();
		this.lastResultsButton = GameObject.Find ("LastResultsButton");
		this.lastResultsButton.AddComponent<NewLobbyLastResultsButtonController> ();

		this.mainCamera = gameObject;
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.helpCamera = GameObject.Find ("HelpCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
	}
	public void resize()
	{
		float mainBlockLeftMargin;
		float mainBlockUpMargin;
		float mainBlockHeight;
		
		float statsBlockLeftMargin;
		float statsBlockUpMargin;
		float statsBlockHeight;
		
		float lastResultsBlockLeftMargin;
		float lastResultsBlockUpMargin;
		float lastResultsBlockHeight;
		float lastResultHeight;
		float lastResultFirstLineY;

		float competitionContentFirstLineY;
		float competitionBlockLeftMargin;
		float competitionBlockUpMargin;
		float competitionBlockHeight;

		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraPosition;
		this.sceneCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraStandardPosition;
		this.helpCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.helpCamera.transform.position = ApplicationDesignRules.helpCameraPositiion;
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;
		this.backgroundCamera.transform.position = ApplicationDesignRules.backgroundCameraPosition;
		this.backgroundCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.helpCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.sceneCamera.GetComponent<Camera> ().rect = new Rect (0f,0f,1f,1f);
		this.mainCamera.GetComponent<Camera>().rect= new Rect (0f,0f,1f,1f);

		this.pagination = new Pagination ();
		this.pagination.chosenPage = 0;
		
		if(ApplicationDesignRules.isMobileScreen)
		{
			mainBlockHeight=5f;
			mainBlockLeftMargin=ApplicationDesignRules.leftMargin;
			mainBlockUpMargin=0f;
			
			statsBlockHeight=ApplicationDesignRules.viewHeight;
			statsBlockLeftMargin=ApplicationDesignRules.worldWidth+ApplicationDesignRules.leftMargin;
			statsBlockUpMargin=0f;
			
			lastResultsBlockHeight=ApplicationDesignRules.viewHeight;
			lastResultsBlockLeftMargin=-ApplicationDesignRules.worldWidth;
			lastResultsBlockUpMargin=0f;
			lastResultHeight=1f;
			lastResultFirstLineY=1f;
			
			competitionContentFirstLineY=1.65f;
			competitionBlockHeight=3f;
			competitionBlockLeftMargin=ApplicationDesignRules.leftMargin;
			competitionBlockUpMargin=mainBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+mainBlockHeight;

			this.pagination.nbElementsPerPage= 6;
			this.slideLeftButton.SetActive(true);
			this.slideRightButton.SetActive(true);
			this.statsButton.SetActive(true);
			this.lastResultsButton.SetActive(true);
		}
		else
		{
			mainBlockHeight=ApplicationDesignRules.mediumBlockHeight;
			mainBlockLeftMargin=ApplicationDesignRules.leftMargin;
			mainBlockUpMargin=ApplicationDesignRules.upMargin;
			
			statsBlockHeight=ApplicationDesignRules.smallBlockHeight;
			statsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			statsBlockUpMargin=mainBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+mainBlockHeight;
			
			lastResultsBlockHeight=ApplicationDesignRules.mediumBlockHeight;
			lastResultsBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			lastResultsBlockUpMargin=ApplicationDesignRules.upMargin;
			lastResultHeight=0.9f;
			lastResultFirstLineY=1f;
			
			competitionContentFirstLineY=2f;
			competitionBlockHeight=ApplicationDesignRules.smallBlockHeight;
			competitionBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			competitionBlockUpMargin=lastResultsBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+lastResultsBlockHeight;

			this.pagination.nbElementsPerPage= 3;
			this.slideLeftButton.SetActive(false);
			this.slideRightButton.SetActive(false);
			this.statsButton.SetActive(false);
			this.lastResultsButton.SetActive(false);
			this.activeGaugeCamera(true);
		}

		this.mainBlock.GetComponent<NewBlockController> ().resize(mainBlockLeftMargin,mainBlockUpMargin,ApplicationDesignRules.blockWidth,mainBlockHeight);
		Vector3 mainBlockUpperLeftPosition = this.mainBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 mainBlockUpperRightPosition = this.mainBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 mainBlockOriginPosition = this.mainBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		Vector2 mainBlockSize = this.mainBlock.GetComponent<NewBlockController> ().getSize ();

		this.mainBlockTitle.transform.position = new Vector3 (mainBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, mainBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.mainBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.mainBlockSubTitle.transform.localScale=ApplicationDesignRules.subMainTitleScale;
		this.mainBlockSubTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+mainBlockUpperLeftPosition.x, mainBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing +0.2f, 0f);
		this.mainBlockSubTitle.transform.GetComponent<TextContainer>().width=mainBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing;

		this.playButton.transform.localScale = ApplicationDesignRules.button62Scale;
		this.playButton.transform.position = new Vector3 (mainBlockUpperRightPosition.x -ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.button62WorldSize.x/2f, mainBlockUpperLeftPosition.y - ApplicationDesignRules.button62WorldSize.y/2f - ApplicationDesignRules.buttonVerticalSpacing-System.Convert.ToInt32(ApplicationDesignRules.isMobileScreen)*(ApplicationDesignRules.roundButtonWorldSize.y +ApplicationDesignRules.buttonVerticalSpacing), 0f);

		this.competitionBlock.GetComponent<NewBlockController> ().resize(competitionBlockLeftMargin,competitionBlockUpMargin,ApplicationDesignRules.blockWidth,competitionBlockHeight);
		Vector3 competitionBlockUpperLeftPosition = this.competitionBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector2 competitionBlockLowerLeftPosition = this.competitionBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 competitionBlockSize = this.competitionBlock.GetComponent<NewBlockController> ().getSize ();

		this.competitionBlockTitle.transform.position = new Vector3 (competitionBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, competitionBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.competitionBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.competitionPicture.transform.localScale = ApplicationDesignRules.competitionScale;
		this.competitionPicture.transform.position = new Vector3 (competitionBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing + ApplicationDesignRules.competitionWorldSize.x / 2f, competitionBlockUpperLeftPosition.y - competitionContentFirstLineY, 0f);

		this.competitionDescription.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.competitionDescription.transform.position = new Vector3 (competitionBlockLowerLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing + ApplicationDesignRules.competitionWorldSize.x + 0.1f, this.competitionPicture.transform.position.y, 0f);
		this.competitionDescription.GetComponent<TextContainer> ().width = (competitionBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing - 0.1f - ApplicationDesignRules.competitionWorldSize.x)*1/(ApplicationDesignRules.subMainTitleScale.x);

		this.statsBlock.GetComponent<NewBlockController> ().resize(statsBlockLeftMargin,statsBlockUpMargin,ApplicationDesignRules.blockWidth,statsBlockHeight);
		Vector3 statsBlockUpperLeftPosition = this.statsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 statsBlockLowerLeftPosition = this.statsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector3 statsBlockUpperRightPosition = this.statsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 statsBlockSize = this.statsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 statsOrigin = this.statsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.statsBlockTitle.transform.position = new Vector3 (statsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, statsBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.statsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.lastResultsBlock.GetComponent<NewBlockController> ().resize(lastResultsBlockLeftMargin,lastResultsBlockUpMargin,ApplicationDesignRules.blockWidth,lastResultsBlockHeight);
		Vector3 lastResultsBlockUpperLeftPosition = this.lastResultsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 lastResultsBlockLowerLeftPosition = this.lastResultsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector3 lastResultsBlockUpperRightPosition = this.lastResultsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 lastResultsBlockSize = this.lastResultsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 lastResultsBlockOrigin = this.lastResultsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.lastResultsBlockTitle.transform.position = new Vector3 (lastResultsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, lastResultsBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.lastResultsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		float lastResultWidth = lastResultsBlockSize.x - 2f * ApplicationDesignRules.blockHorizontalSpacing;
		float lastResultsLineScale = ApplicationDesignRules.getLineScale (lastResultWidth);
		this.results=new GameObject[pagination.nbElementsPerPage];
	
		for(int i=0;i<this.results.Length;i++)
		{
			this.results[i]=Instantiate (this.resultObject) as GameObject;
			this.results[i].transform.position=new Vector3(lastResultsBlockOrigin.x,lastResultsBlockUpperLeftPosition.y-lastResultFirstLineY-(i+1)*lastResultHeight,0f);
			this.results[i].transform.FindChild("line").localScale=new Vector3(lastResultsLineScale,1f,1f);
			this.results[i].transform.FindChild("picture").localScale=ApplicationDesignRules.thumbScale;
			this.results[i].transform.FindChild("picture").localPosition=new Vector3(-lastResultWidth/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(lastResultHeight-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.results[i].transform.FindChild("username").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.results[i].transform.FindChild("username").GetComponent<TextMeshPro>().textContainer.width=(lastResultWidth/2f)-0.2f-ApplicationDesignRules.thumbWorldSize.x;
			this.results[i].transform.FindChild("username").localPosition=new Vector3(-lastResultWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.2f,lastResultHeight-(lastResultHeight-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.results[i].transform.FindChild("divisionIcon").localScale=ApplicationDesignRules.divisionIconScale;
			this.results[i].transform.FindChild("divisionIcon").localPosition=new Vector3(ApplicationDesignRules.divisionIconDistance.x-lastResultWidth/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(lastResultHeight-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f+ApplicationDesignRules.divisionIconDistance.y,0f);
			this.results[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.results[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=0.75f*lastResultWidth-0.2f-ApplicationDesignRules.thumbWorldSize.x;
			this.results[i].transform.FindChild("description").localPosition=new Vector3(-lastResultWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.2f,lastResultHeight-(lastResultHeight-ApplicationDesignRules.thumbWorldSize.y)/2f-0.35f,0f);
		}

		this.setLastResults ();

		this.paginationButtons.transform.localPosition=new Vector3 (lastResultsBlockLowerLeftPosition.x + lastResultWidth / 2, lastResultsBlockLowerLeftPosition.y + 0.3f, 0f);
		this.paginationButtons.GetComponent<NewLobbyPaginationController> ().resize ();

		this.popUp.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.popUp.transform.localScale = ApplicationDesignRules.popUpScale;

		this.slideLeftButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.slideLeftButton.transform.position = new Vector3 (statsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.roundButtonWorldSize.x/2f, statsBlockUpperRightPosition.y -ApplicationDesignRules.buttonVerticalSpacing- ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);
		
		this.slideRightButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.slideRightButton.transform.position = new Vector3 (lastResultsBlockUpperRightPosition.x -ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.roundButtonWorldSize.x/2f, lastResultsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing- ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		this.statsButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.statsButton.transform.position = new Vector3 (mainBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, mainBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);
		
		this.lastResultsButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.lastResultsButton.transform.position = new Vector3 (mainBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 1.5f*ApplicationDesignRules.roundButtonWorldSize.x, mainBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		this.mainContentPositionX = mainBlockOriginPosition.x;
		this.statsPositionX=statsOrigin.x;
		this.lastResultsPositionX = lastResultsBlockOrigin.x;

		this.divisionProgression.GetComponent<DivisionProgressionController>().resize(new Rect(mainBlockOriginPosition.x,mainBlockOriginPosition.y,mainBlockSize.x,mainBlockSize.y));

		if(ApplicationDesignRules.isMobileScreen)
		{
			for(int i=0;i<this.stats.Length;i++)
			{
				this.stats[i].transform.position=new Vector3(statsBlockLowerLeftPosition.x+statsBlockSize.x/2f,statsBlockUpperLeftPosition.y-2f-i*1.5f);
				this.stats[i].transform.localScale= new Vector3(1f,1f,1f);
				this.stats[i].transform.FindChild("Title").GetComponent<TextContainer>().width=statsBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing;
			}
		}
		else
		{
			Vector3 statsScale = new Vector3 (1f, 1f, 1f);
			Vector2 statBlockSize = new Vector2 ((statsBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing) / 4f, statsBlockSize.y - 0.5f);
			
			for(int i=0;i<this.stats.Length;i++)
			{
				this.stats[i].transform.position=new Vector3(statsBlockLowerLeftPosition.x+0.3f+statBlockSize.x/2f+i*statBlockSize.x,statsBlockUpperLeftPosition.y-0.8f-statBlockSize.y/2f);
				this.stats[i].transform.localScale= ApplicationDesignRules.reductionRatio*statsScale;
				this.stats[i].transform.FindChild("Title").GetComponent<TextContainer>().width=statBlockSize.x;
			}
		}
		MenuController.instance.resize();
		MenuController.instance.setCurrentPage(5);
		MenuController.instance.refreshMenuObject();
		HelpController.instance.resize();
	}
	public void returnPressed()
	{
		if(this.isPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hidePopUp();
		}
	}
	public void escapePressed()
	{
		if(this.isPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hidePopUp();
		}
		else
		{
			BackOfficeController.instance.leaveGame();
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
					description=WordingLobby.getReference(10)+model.lastResults[this.pagination.chosenPage*this.pagination.nbElementsPerPage+i].Date.ToString(WordingDates.getDateFormat());
					textColor=ApplicationDesignRules.blueColor;
				}
				else
				{
					description=WordingLobby.getReference(11)+model.lastResults[this.pagination.chosenPage*this.pagination.nbElementsPerPage+i].Date.ToString(WordingDates.getDateFormat());
					textColor=ApplicationDesignRules.redColor;
				}
				this.results[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=description;
				this.results[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=textColor;
				this.results[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnThumbPicture(model.lastResults[this.pagination.chosenPage*this.pagination.nbElementsPerPage+i].Opponent.IdProfilePicture);
				this.results[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.lastResults[this.pagination.chosenPage*this.pagination.nbElementsPerPage+i].Opponent.Username;
				this.results[i].transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setDivision(model.lastResults[this.pagination.chosenPage*this.pagination.nbElementsPerPage+i].Opponent.Division);
			}
			else
			{
				this.results[i].SetActive(false);
			}
		}
	}
	public void drawStats()
	{
		this.stats[0].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= ApplicationModel.player.TotalNbWins.ToString ();
		this.stats[1].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= ApplicationModel.player.TotalNbLooses.ToString ();
		this.stats[2].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= ApplicationModel.player.Ranking.ToString ();
		this.stats[2].transform.FindChild ("Subvalue").GetComponent<TextMeshPro> ().text= WordingLobby.getReference(8)+ApplicationModel.player.RankingPoints.ToString()+WordingLobby.getReference(9);
		this.stats[3].transform.FindChild ("Value").GetComponent<TextMeshPro> ().text= ApplicationModel.player.CollectionRanking.ToString ();
		this.stats[3].transform.FindChild ("Subvalue").GetComponent<TextMeshPro> ().text= WordingLobby.getReference(8)+ApplicationModel.player.CollectionPoints.ToString()+WordingLobby.getReference(9);	
	}
	public void drawCompetition()
	{
		this.mainBlockTitle.GetComponent<TextMeshPro>().text=WordingGameModes.getName(ApplicationModel.player.CurrentDivision.Id);
		string description=WordingLobby.getReference(12)+ApplicationModel.player.CurrentDivision.TitlePrize.ToString()+WordingLobby.getReference(14);
		if(ApplicationModel.player.CurrentDivision.NbWinsForPromotion!=-1)
		{
			description=description+WordingLobby.getReference(13)+ApplicationModel.player.CurrentDivision.PromotionPrize.ToString()+WordingLobby.getReference(14);
		}
		this.competitionDescription.GetComponent<TextMeshPro>().text=description;
		this.competitionPicture.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnLargeCompetitionPicture(ApplicationModel.player.CurrentDivision.getPictureId());
	}
	private void displayPopUp()
	{
		SoundController.instance.playSound(3);
		this.isPopUpDisplayed = true;
		this.popUp.SetActive (true);
		BackOfficeController.instance.displayTransparentBackground ();
	}
	public void hidePopUp()
	{
		this.isPopUpDisplayed = false;
		this.popUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground ();
	}
	public void playHandler()
	{
		SoundController.instance.playSound(9);
		if(this.isEndCompetition)
		{
			SceneManager.LoadScene("NewLobby");
		}
		else
		{
			BackOfficeController.instance.joinRandomRoomHandler();
		}
	}
	public void drawGauge()
	{
		if(this.isEndGameLobby)
		{
			if(hasWonLastGame)
			{
				this.divisionProgression.GetComponent<DivisionProgressionController> ().drawGauge (ApplicationModel.player.CurrentDivision,true);
				this.divisionProgression.GetComponent<DivisionProgressionController>().animateGauge();
			}
			else
			{
				this.divisionProgression.GetComponent<DivisionProgressionController> ().drawGauge (ApplicationModel.player.CurrentDivision,false);
			}
		}
		else
		{
			this.divisionProgression.GetComponent<DivisionProgressionController> ().drawGauge (ApplicationModel.player.CurrentDivision,false);
		}
	}
	public void endGaugeAnimation()
	{
		this.divisionProgression.GetComponent<DivisionProgressionController> ().drawGauge (ApplicationModel.player.CurrentDivision,false);
	}
	public void initializePopUp()
	{
		bool displayPopUp = false;
		string content = "";
		if(ApplicationModel.player.CurrentDivision.Status==3) // Fin de saison + Promotion + Titre
		{
			content =WordingLobby.getReference(16);
			displayPopUp=true;
			this.isEndCompetition=true;
		}
		else if(ApplicationModel.player.CurrentDivision.Status==30) // Fin de saison + Titre
		{
			content =WordingLobby.getReference(17);
			displayPopUp=true;
			this.isEndCompetition=true;
		}
		else if(ApplicationModel.player.CurrentDivision.Status==20) // Promotion obtenue au cours du match + Fin de saison
		{
			content=WordingLobby.getReference(18);
			displayPopUp=true;
			this.isEndCompetition=true;
		}
		else if(ApplicationModel.player.CurrentDivision.Status==2) // Promotion + Fin de saison
		{
			content=WordingLobby.getReference(19);
			displayPopUp=true;
			this.isEndCompetition=true;
		}
		else if(ApplicationModel.player.CurrentDivision.Status==21) // Promotion obtenue au cours du match
		{
			content=WordingLobby.getReference(20);
			displayPopUp=true;
		}
		else if(ApplicationModel.player.CurrentDivision.Status==10) // Maintien obtenu au cours du match + Fin de saison
		{
			content=WordingLobby.getReference(21);
			displayPopUp=true;
			this.isEndCompetition=true;
		}
		else if(ApplicationModel.player.CurrentDivision.Status==1) // Maintien + Fin de saison
		{
			content=WordingLobby.getReference(22);
			displayPopUp=true;
			this.isEndCompetition=true;
		}
		else if(ApplicationModel.player.CurrentDivision.Status==11) // Maintien obtenu au cours du match
		{
			content=WordingLobby.getReference(23);
			displayPopUp=true;
		}
		else if(ApplicationModel.player.CurrentDivision.Status==-1) // Relégation
		{
			content=WordingLobby.getReference(24);
			displayPopUp=true;
		}
		if(displayPopUp)
		{
			this.waitForPopUp=true;
			this.popUp.transform.FindChild ("Content").GetComponent<TextMeshPro> ().text = content;
		}
	}
	public void initializePlayButton()
	{
		if(this.isEndCompetition)
		{
			this.playButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingLobby.getReference(25);
		}
		else
		{
			this.playButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingLobby.getReference(26);
		}
	}
	public void clickOnResultsProfile(int id)
	{
		SoundController.instance.playSound(9);
		ApplicationModel.player.ProfileChosen = this.results [id].transform.FindChild ("username").GetComponent<TextMeshPro> ().text;
		SceneManager.LoadScene("NewProfile");
	}
	public void updateSubMainBlockTitle(string s)
	{
		this.mainBlockSubTitle.GetComponent<TextMeshPro> ().text = s.ToUpper();
	}
	public void activeGaugeCamera(bool value)
	{
		this.divisionProgression.GetComponent<DivisionProgressionController>().activeGaugeCamera(value);
	}
	public void slideRight()
	{
		SoundController.instance.playSound(16);
		if(this.mainContentDisplayed)
		{
			this.activeGaugeCamera(false);
			this.mainContentDisplayed=false;
			this.targetContentPositionX=this.statsPositionX;
		}
		else if(this.lastResultsDisplayed)
		{
			this.lastResultsDisplayed=false;
			this.targetContentPositionX=this.mainContentPositionX;
		}
		else if(this.targetContentPositionX==mainContentPositionX)
		{
			this.targetContentPositionX=this.statsPositionX;
		}
		else if(this.targetContentPositionX==lastResultsPositionX)
		{
			this.targetContentPositionX=this.mainContentPositionX;
		}
		this.toSlideRight=true;
		this.toSlideLeft=false;
	}
	public void slideLeft()
	{
		SoundController.instance.playSound(16);
		if(this.mainContentDisplayed)
		{
			this.activeGaugeCamera(false);
			this.mainContentDisplayed=false;
			this.targetContentPositionX=this.lastResultsPositionX;
		}
		else if(this.statsDisplayed)
		{
			this.lastResultsDisplayed=false;
			this.targetContentPositionX=this.mainContentPositionX;
		}
		else if(this.targetContentPositionX==mainContentPositionX)
		{
			this.targetContentPositionX=this.lastResultsPositionX;
		}
		else if(this.targetContentPositionX==statsPositionX)
		{
			this.targetContentPositionX=this.mainContentPositionX;
		}
		this.toSlideRight=false;
		this.toSlideLeft=true;
	}
	public void cleanLastResults()
	{
		for(int i=0;i<this.results.Length;i++)
		{
			Destroy (this.results[i]);
		}
	}
	public void setLastResults()
	{
		for(int i=0;i<this.results.Length;i++)
		{
			this.results[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.results[i].transform.FindChild("username").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.results[i].transform.FindChild("username").gameObject.AddComponent<NewLobbyResultsUsernameController>();
			this.results[i].transform.FindChild("username").GetComponent<NewLobbyResultsUsernameController>().setId(i);
			this.results[i].transform.FindChild("picture").gameObject.AddComponent<NewLobbyResultsPictureController>();
			this.results[i].transform.FindChild("picture").GetComponent<NewLobbyResultsPictureController>().setId(i);
			this.results[i].transform.FindChild("line").GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
			this.results[i].SetActive(false);
		}
	}
	#region HELP FUNCTIONS
	
	public GameObject returnMainBlock()
	{
		return this.mainBlock;
	}
	public GameObject returnStatsBlock()
	{
		return this.statsBlock;
	}
	public GameObject returnLastResultsBlock()
	{
		return this.lastResultsBlock;
	}
	public GameObject returnCompetitionBlock()
	{
		return this.competitionBlock;
	}
	public IEnumerator endHelp()
	{
		if(!ApplicationModel.player.LobbyHelp)
		{
			BackOfficeController.instance.displayLoadingScreen();
			yield return StartCoroutine(ApplicationModel.player.setLobbyTutorial(true));
			BackOfficeController.instance.hideLoadingScreen();
		}
	}
	public bool getAreStatsDisplayed()
	{
		return statsDisplayed;
	}
	public bool getAreResultsDisplayed()
	{
		return lastResultsDisplayed;
	}
	#endregion
}
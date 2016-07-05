using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class NewHomePageController : MonoBehaviour
{
	public static NewHomePageController instance;
	//private NewHomePageModel model;
	
	public GameObject blockObject;
	public GameObject contentObject;
	public GameObject challengeButtonObject;
	public GameObject friendsStatusButtonObject;
	public Texture2D[] cursorTextures;

	private int refreshInterval;
	private int sliderRefreshInterval;
	private int totalNbResultLimit;
	
	private GameObject backOfficeController;
	private GameObject serverController;
	private GameObject deckBlock;
	private GameObject deckBlockTitle;
	private GameObject deckSelectionButton;
	private GameObject deckTitle;
	private GameObject playBlock;
	private GameObject playBlockTitle;
	private GameObject storeBlock;
	private GameObject storeBlockTitle;
	private GameObject pack;
	private GameObject friendlyGameTitle;
	private GameObject friendlyGamePicture;
	private GameObject friendlyGameButton;
	private GameObject officialGameButton;
	private GameObject divisionGamePicture;
	private GameObject officialGameTitle;
	private GameObject trainingGamePicture;
	private GameObject newsfeedBlock;
	private GameObject newsfeedBlockTitle;
	private GameObject[] tabs;
	private GameObject[] contents;
	private GameObject[] challengeButtons;
	private GameObject[] friendsStatusButtons;
	private GameObject newsfeedPaginationButtons;
	private GameObject[] deckChoices;
	private GameObject[] cardsHalos;
	private GameObject popUp;
	private GameObject mainCamera;
	private GameObject sceneCamera;
	private GameObject helpCamera;
	private GameObject backgroundCamera;

	private GameObject menu;
	private GameObject help;
	private GameObject[] deckCards;

	private GameObject socialButton;
	private GameObject slideRightButton;

	private Rect centralWindow;

	private int activeTab;
	
	private IList<int> newsDisplayed;
	private IList<int> notificationsDisplayed;
	private int packDisplayed;
	private IList<int> friendsDisplayed;
	private IList<int> friendsToBeDisplayed;

	private IList<int> friendsOnline;
	private bool toUpdateFriends;
	
	private Pagination newsfeedPagination;

	private int nbPacks;
	private int displayedPack;

	private float sliderTimer;
	private float checkForFriendsOnlineTimer;
	private bool isSceneLoaded;
	
	private int money;

	private Vector3[] deckCardsPosition;
	private Rect[] deckCardsArea;
	private bool[] deckCardsAreaHovered;
	private bool isHoveringDeckArea;

	private IList<int> decksDisplayed;
	private int[] deckCardsDisplayed;
	private int deckDisplayed;
	
	private GameObject focusedCard;
	private int focusedCardIndex;
	private bool isCardFocusedDisplayed;

	private int idCardClicked;
	private bool isDragging;
	private bool isLeftClicked;
	private bool isHovering;
	private float clickInterval;

	private bool isSearchingDeck;
	private bool isMouseOnSelectDeckButton;

	private bool isMouseOnBuyPackButton;

	private GameObject connectionBonusPopUp;
	private bool isConnectionBonusPopUpDisplayed;
	private GameObject endGamePopUp;
	private bool isEndGamePopUpDisplayed;
	private GameObject trainingPopUp;
	private GameObject wonPackPopUp;
	private GameObject hasLeftRoomPopUp;
    private bool isHasLeftRoomPopUpDisplayed;

	private bool toSlideRight;
	private bool toSlideLeft;
	private bool newsfeedDisplayed;
	private bool isSlidingCursors;
	private bool mainContentDisplayed;

	private float newsfeedPositionX;
	private float mainContentPositionX;

	private int nonReadNotificationsOnCurrentPage;

	void Update()
	{	
		this.sliderTimer += Time.deltaTime;

		if (Input.touchCount == 1 && this.isSceneLoaded &&  HelpController.instance.getCanSwipe() && BackOfficeController.instance.getCanSwipeAndScroll()) 
		{
			if(Mathf.Abs(Input.touches[0].deltaPosition.y)>1f && Mathf.Abs(Input.touches[0].deltaPosition.y)>Mathf.Abs(Input.touches[0].deltaPosition.x))
			{
				this.isLeftClicked=false;
			}
			else if(Input.touches[0].deltaPosition.x<-ApplicationDesignRules.swipeCoefficient	 && !this.isCardFocusedDisplayed && !this.isDragging)
			{
				this.isLeftClicked=false;
				if(this.newsfeedDisplayed || this.toSlideLeft)
				{
					this.slideRight();
					BackOfficeController.instance.setIsSwiping(true);
				}
			}
			else if(Input.touches[0].deltaPosition.x>ApplicationDesignRules.swipeCoefficient && !this.isCardFocusedDisplayed && !this.isDragging)
			{
				this.isLeftClicked=false;
				if(this.mainContentDisplayed || this.toSlideRight)
				{
					this.slideLeft();
					BackOfficeController.instance.setIsSwiping(true);
				}
			}
		}
		this.checkForFriendsOnlineTimer += Time.deltaTime;
		if (checkForFriendsOnlineTimer > refreshInterval && this.isSceneLoaded) 
		{
			this.checkForFriendsOnlineTimer=0;
			this.checkFriendsOnlineStatus();
		}
		if(this.sliderTimer>this.sliderRefreshInterval)
		{
			this.sliderTimer=0;
			if(this.isSceneLoaded && !this.isCardFocusedDisplayed && !this.isMouseOnBuyPackButton)
			{
				if(nbPacks-1>this.displayedPack)
				{
					this.displayedPack++;
					this.drawPack();
				}
				else if(this.displayedPack!=0)
				{
					this.displayedPack=0;
					this.drawPack();
				}
			}
		}
		if(isLeftClicked)
		{
			this.clickInterval=this.clickInterval+Time.deltaTime*10f;
			if(this.clickInterval>2f)
			{
				this.isLeftClicked=false;
				this.startDragging();
			}
		}
		if(this.isSearchingDeck)
		{
			if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))&& !this.isMouseOnSelectDeckButton)
			{
				this.isSearchingDeck=false;
				this.cleanDeckList();
			}
		}
		if(money!=ApplicationModel.player.Money)
		{
			if(isSceneLoaded)
			{
				if(this.isCardFocusedDisplayed)
				{
					this.focusedCard.GetComponent<NewFocusedCardHomePageController>().updateFocusFeatures();
				}
			}
			this.money=ApplicationModel.player.Money;
		}
		if(toSlideRight || toSlideLeft)
		{
			Vector3 sceneCameraPosition = this.sceneCamera.transform.position;
			float camerasXPosition = sceneCameraPosition.x;
			if(toSlideRight)
			{
				camerasXPosition=camerasXPosition+Time.deltaTime*40f;
				if(camerasXPosition>this.mainContentPositionX)
				{
					camerasXPosition=this.mainContentPositionX;
					this.toSlideRight=false;
					this.mainContentDisplayed=true;
					BackOfficeController.instance.setIsSwiping(false);
				}
			}
			else if(toSlideLeft)
			{
				camerasXPosition=camerasXPosition-Time.deltaTime*40f;
				if(camerasXPosition<this.newsfeedPositionX)
				{
					camerasXPosition=this.newsfeedPositionX;
					this.toSlideLeft=false;
					this.newsfeedDisplayed=true;
					this.manageNonReadsNotifications();
					BackOfficeController.instance.setIsSwiping(false);
				}
			}
			sceneCameraPosition.x=camerasXPosition;
			this.sceneCamera.transform.position=sceneCameraPosition;
		}
	}
	void Awake()
	{
		instance = this;
		this.activeTab = 0;
		this.refreshInterval=5;
		this.sliderRefreshInterval=5;
		this.totalNbResultLimit=1000;
		this.mainContentDisplayed = true;
		this.friendsOnline = new List<int> ();
		this.initializeScene ();
		this.initializeBackOffice();
		this.initializeMenu();
		this.initializeHelp();
		this.initialization ();
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
	}
	private void initializeHelp()
	{
		this.help = GameObject.Find ("HelpController");
		this.help.AddComponent<HomePageHelpController>();
		this.help.GetComponent<HomePageHelpController>().initialize();
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
		this.backOfficeController.AddComponent<BackOfficeHomePageController>();
		this.backOfficeController.GetComponent<BackOfficeHomePageController>().initialize();
	}
	public void initialization()
	{
		this.resize ();
		ApplicationModel.player.MyNotifications.writeNotifications();
		ApplicationModel.player.MyNews.writeNews();
		this.selectATab ();
		this.initializePacks ();
		this.retrieveDefaultDeck ();
		this.initializeDecks ();
		this.initializeCompetitions ();
		this.checkFriendsOnlineStatus ();
		if(ApplicationModel.player.ToLaunchEndGameSequence)
		{
			this.showEndGameSequence();
			ApplicationModel.player.ToLaunchEndGameSequence=false;
			ApplicationModel.player.HasWonLastGame=false;
			ApplicationModel.player.ChosenGameType=-1;
		}
		else if(ApplicationModel.player.TutorialStep==2 || ApplicationModel.player.TutorialStep==3 || ApplicationModel.player.TutorialStep==6)
		{
			HelpController.instance.startTutorial();
		}
		else if(ApplicationModel.player.HasToBuyTrainingPack)
		{
			this.displayWonPackPopUp();
		}
		else if(ApplicationModel.player.ConnectionBonus>0)
		{
			this.displayConnectionBonusPopUp();
		}
		else if(ApplicationModel.player.GoToNotifications)
		{
			this.displayNotifications();
		}
		BackOfficeController.instance.hideLoadingScreen();
		this.isSceneLoaded = true;
	}
	public void displayNotifications()
	{
		ApplicationModel.player.GoToNotifications=false;
		if(ApplicationDesignRules.isMobileScreen && !this.newsfeedDisplayed)
		{
			this.slideLeft ();
		}
		if(this.activeTab!=0)
		{
			this.selectATabHandler(0);
		}
	}
	public void selectATabHandler(int idTab)
	{
		SoundController.instance.playSound(9);
		this.activeTab = idTab;
		this.selectATab ();
	}
	private void selectATab()
	{
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.hideActiveTab();
		}
		for(int i=0;i<this.tabs.Length;i++)
		{
			if(i==this.activeTab)
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnTabPicture(1);
				this.tabs[i].GetComponent<NewHomePageTabController>().setIsSelected(true);
				this.tabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnTabPicture(0);
				this.tabs[i].GetComponent<NewHomePageTabController>().reset();
			}
		}
		this.initializeTabContent ();
	}
	public void initializeTabContent()
	{
		switch(this.activeTab)
		{
		case 0:
			this.initializeNotifications();
			break;
		case 1:
			this.initializeNews();
			break;
		case 2:
			this.initializeFriends();
			break;
		}
	}
	private void initializeNotifications()
	{
		this.newsfeedPagination.chosenPage = 0;
		this.newsfeedPagination.totalElements= ApplicationModel.player.MyNotifications.getCount();
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().p = this.newsfeedPagination;
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().setPagination ();
		for(int i=0;i<this.contents.Length;i++)
		{
			this.challengeButtons[i].SetActive(false);
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		this.drawNotifications ();
	}
	private void initializeNews()
	{
		this.newsfeedPagination.chosenPage = 0;
		this.newsfeedPagination.totalElements= ApplicationModel.player.MyNews.getCount();
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().p = this.newsfeedPagination;
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().setPagination ();
		for(int i=0;i<this.contents.Length;i++)
		{
			this.challengeButtons[i].SetActive(false);
			this.contents[i].transform.FindChild("new").gameObject.SetActive(false);
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.friendsStatusButtons[i*2].SetActive(false);
			this.friendsStatusButtons[i*2+1].SetActive(false);
		}
		this.drawNews ();
	}
	private void initializeFriends()
	{
		this.sortFriendsList ();
		this.newsfeedPagination.chosenPage = 0;
		this.newsfeedPagination.totalElements= ApplicationModel.player.MyFriends.Count;
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().p = this.newsfeedPagination;
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().setPagination ();
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i].transform.FindChild("new").gameObject.SetActive(false);
			this.contents[i].transform.FindChild("date").gameObject.SetActive(false);
			this.friendsStatusButtons[i*2].SetActive(false);
			this.friendsStatusButtons[i*2+1].SetActive(false);
		}
		this.drawFriends ();
	}
	private void initializePacks()
	{
		this.nbPacks = ApplicationModel.packs.getCount();
		this.displayedPack = 0;
		this.drawPack ();
	}
	public void paginationHandler()
	{
		SoundController.instance.playSound(8);
		switch(this.activeTab)
		{
		case 0:
			this.drawNotifications();
			break;
		case 1:
			this.drawNews();
			break;
		case 2:
			this.drawFriends();
			break;
		}
	}
	private void initializeCompetitions()
	{
		this.drawCompetitions ();
	}
	private void initializeDecks()
	{
		this.retrieveDecksList ();
		this.drawDeckCards ();
	}
	public void initializeScene()
	{
		this.deckBlock = Instantiate (this.blockObject) as GameObject;
		this.deckBlockTitle = GameObject.Find ("DeckBlockTitle");
		this.deckBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.deckBlockTitle.GetComponent<TextMeshPro> ().text = WordingHomePage.getReference(0);
		this.deckSelectionButton = GameObject.Find ("DeckSelectionButton");
		this.deckSelectionButton.AddComponent<NewHomePageDeckSelectionButtonController> ();
		this.deckTitle = GameObject.Find ("DeckTitle");
		this.deckTitle.AddComponent<newHomePageDeckTitleController>();
		this.deckTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.deckChoices=new GameObject[12];
		for(int i=0;i<this.deckChoices.Length;i++)
		{
			this.deckChoices[i]=GameObject.Find("DeckChoice"+i);
			this.deckChoices[i].AddComponent<NewHomePageDeckChoiceController>();
			this.deckChoices[i].GetComponent<NewHomePageDeckChoiceController>().setId(i);
			this.deckChoices[i].SetActive(false);
		}
		this.deckCards=new GameObject[4];
		for (int i=0;i<4;i++)
		{
			this.deckCards[i]=GameObject.Find("deckCard"+i);
			this.deckCards[i].AddComponent<NewCardHomePageController>();
			this.deckCards[i].SetActive(false);
		}
		this.cardsHalos=new GameObject[4];
		for(int i=0;i<this.cardsHalos.Length;i++)
		{
			this.cardsHalos[i]=GameObject.Find ("CardHalo"+i);
			this.cardsHalos[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		this.cardsHalos [0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDeck.getReference(0);
		this.cardsHalos [1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text =WordingDeck.getReference(1);
		this.cardsHalos [2].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDeck.getReference(2);
		this.cardsHalos [3].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingDeck.getReference(3);
		this.playBlock = Instantiate (this.blockObject) as GameObject;
		this.playBlockTitle = GameObject.Find ("PlayBlockTitle");
		this.playBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.playBlockTitle.GetComponent<TextMeshPro> ().text =  WordingHomePage.getReference(1);
		this.friendlyGameButton = GameObject.Find ("FriendlyGameButton");
		this.friendlyGameButton.AddComponent<NewHomePageCompetitionController> ();
		this.friendlyGameButton.GetComponent<NewHomePageCompetitionController> ().setId (0);
		this.friendlyGamePicture = GameObject.Find ("FriendlyGamePicture");
		this.friendlyGamePicture.GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
		this.friendlyGameTitle = GameObject.Find ("FriendlyGameTitle");
		this.friendlyGameTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.officialGameButton = GameObject.Find ("OfficialGameButton");
		this.officialGameButton.AddComponent<NewHomePageCompetitionController> ();
		this.officialGameButton.GetComponent<NewHomePageCompetitionController> ().setId (1);
		this.divisionGamePicture = GameObject.Find ("DivisionGamePicture");
		this.divisionGamePicture.GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
		this.officialGameTitle = GameObject.Find ("OfficialGameTitle");
		this.officialGameTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.trainingGamePicture = GameObject.Find("TrainingGamePicture");
//		this.cupGameButton = GameObject.Find ("CupGameButton");
//		this.cupGameButton.AddComponent<NewHomePageCompetitionController> ();
//		this.cupGameButton.GetComponent<NewHomePageCompetitionController> ().setId (2);
//		this.cupGamePicture = GameObject.Find ("CupGamePicture");
//		this.cupGamePicture.GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
//		this.cupGameTitle = GameObject.Find ("CupGameTitle");
//		this.cupGameTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;

		this.pack = GameObject.Find ("Pack");
		this.pack.AddComponent<NewPackHomePageController> ();
		this.pack.GetComponent<NewPackHomePageController> ().initialize ();

		this.storeBlock = Instantiate (this.blockObject) as GameObject;
		this.storeBlockTitle = GameObject.Find ("StoreBlockTitle");
		this.storeBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.storeBlockTitle.GetComponent<TextMeshPro> ().text =  WordingHomePage.getReference(2);
		this.newsfeedBlock = Instantiate (this.blockObject) as GameObject;
		this.newsfeedBlockTitle = GameObject.Find ("NewsfeedBlockTitle");
		this.newsfeedBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.tabs=new GameObject[3];
		for(int i=0;i<this.tabs.Length;i++)
		{
			this.tabs[i]=GameObject.Find ("Tab"+i);
			this.tabs[i].AddComponent<NewHomePageTabController>();
			this.tabs[i].GetComponent<NewHomePageTabController>().setId(i);
		}
		this.tabs[0].transform.FindChild("Title").GetComponent<TextMeshPro> ().text =  WordingHomePage.getReference(3);
		this.tabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text =  WordingHomePage.getReference(4);
		this.tabs[2].transform.FindChild("Title").GetComponent<TextMeshPro> ().text =  WordingHomePage.getReference(5);
		this.contents = new GameObject[0];
		this.friendsStatusButtons=new GameObject[0];
		this.challengeButtons = new GameObject[0];
		this.newsfeedPaginationButtons = GameObject.Find("Pagination");
		this.newsfeedPaginationButtons.AddComponent<NewHomePagePaginationController> ();
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().initialize ();

		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardHomePageController> ();
		this.focusedCard.SetActive (false);

		this.endGamePopUp = GameObject.Find ("endGamePopUp");
		this.endGamePopUp.SetActive (false);

		this.trainingPopUp = GameObject.Find("trainingPopUp");
		this.trainingPopUp.SetActive(false);

		this.wonPackPopUp = GameObject.Find("wonPackPopUp");
		this.wonPackPopUp.SetActive(false);

        this.hasLeftRoomPopUp = GameObject.Find("hasLeftRoomPopUp");
        this.hasLeftRoomPopUp.SetActive(false);

		this.mainCamera = gameObject;
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.helpCamera = GameObject.Find ("HelpCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
		this.connectionBonusPopUp = GameObject.Find ("connectionBonusPopUp");
		this.connectionBonusPopUp.SetActive (false);
		this.slideRightButton = GameObject.Find ("SlideRightButton");
		this.slideRightButton.AddComponent<NewHomePageSlideRightButtonController> ();
		this.socialButton = GameObject.Find ("SocialButton");
		this.socialButton.AddComponent<NewHomePageSocialButtonController> ();
	}
	public void resize()
	{
		float playBlockLeftMargin;
		float playBlockUpMargin;
		float playBlockHeight;

		float deckBlockLeftMargin;
		float deckBlockUpMargin;
		float deckBlockHeight;

		float storeBlockLeftMargin;
		float storeBlockUpMargin;
		float storeBlockHeight;

		float newsfeedBlockLeftMargin;
		float newsfeedBlockUpMargin;
		float newsfeedBlockHeight;

		float contentHeight;
		float contentFirstLineY;

		float cardFirstLine;

		storeBlockHeight=ApplicationDesignRules.smallBlockHeight;

		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraPosition;
		this.sceneCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.helpCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.helpCamera.transform.position = ApplicationDesignRules.helpCameraPositiion;
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;
		this.backgroundCamera.transform.position = ApplicationDesignRules.backgroundCameraPosition;
		this.backgroundCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.helpCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.sceneCamera.GetComponent<Camera> ().rect = new Rect (0f,0f,1f,1f);
		this.mainCamera.GetComponent<Camera>().rect= new Rect (0f,0f,1f,1f);

		this.newsfeedPagination = new Pagination ();
		this.newsfeedPagination.chosenPage = 0;

		if(ApplicationDesignRules.isMobileScreen)
		{
			playBlockLeftMargin=ApplicationDesignRules.leftMargin;
			playBlockUpMargin=0f;
			playBlockHeight=3.8f;
			cardFirstLine=2.35f;
			deckBlockHeight=4.2f;
			deckBlockLeftMargin=ApplicationDesignRules.leftMargin;
			deckBlockUpMargin=playBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+playBlockHeight;
			storeBlockLeftMargin=ApplicationDesignRules.worldWidth+ApplicationDesignRules.leftMargin;
			storeBlockUpMargin=0f;
			newsfeedBlockLeftMargin=-ApplicationDesignRules.worldWidth;
			newsfeedBlockUpMargin=ApplicationDesignRules.tabWorldSize.y;
			newsfeedBlockHeight=ApplicationDesignRules.viewHeight-ApplicationDesignRules.tabWorldSize.y;
			contentHeight=1f;
			contentFirstLineY=1f;
			this.socialButton.SetActive(true);
			this.slideRightButton.SetActive(true);
			this.newsfeedBlockTitle.SetActive(true);
			this.newsfeedPagination.nbElementsPerPage= 6;
		}
		else
		{
			cardFirstLine=3f;
			deckBlockHeight=ApplicationDesignRules.mediumBlockHeight;
			deckBlockLeftMargin=ApplicationDesignRules.leftMargin;
			deckBlockUpMargin=ApplicationDesignRules.upMargin;
			playBlockHeight=ApplicationDesignRules.smallBlockHeight;
			playBlockLeftMargin=ApplicationDesignRules.leftMargin;
			playBlockUpMargin=deckBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+deckBlockHeight;
			newsfeedBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			newsfeedBlockUpMargin=deckBlockUpMargin+ApplicationDesignRules.tabWorldSize.y;
			newsfeedBlockHeight=ApplicationDesignRules.mediumBlockHeight-ApplicationDesignRules.tabWorldSize.y;
			contentHeight=0.9f;
			contentFirstLineY=0.3f;
			storeBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			storeBlockUpMargin=newsfeedBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+newsfeedBlockHeight;
			this.socialButton.SetActive(false);
			this.slideRightButton.SetActive(false);
			this.newsfeedBlockTitle.SetActive(false);
			this.newsfeedPagination.nbElementsPerPage= 3;
		}

		if(isCardFocusedDisplayed)
		{
			this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraFocusedCardPosition;
		}
		else
		{
			this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraStandardPosition;
		}

		this.centralWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.25f * ApplicationDesignRules.heightScreen);
		this.playBlock.GetComponent<NewBlockController> ().resize(playBlockLeftMargin,playBlockUpMargin,ApplicationDesignRules.blockWidth,playBlockHeight);
		Vector3 playBlockUpperLeftPosition = this.playBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 playBlockUpperRightPosition = this.playBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 playBlockSize = this.playBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 playBlockOrigin = this.playBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.playBlockTitle.transform.position = new Vector3 (playBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, playBlockUpperLeftPosition.y - 0.2f, 0f);
		this.socialButton.transform.position = new Vector3 (playBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, playBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);
		this.socialButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		float gapBetweenCompetitionsBlock = 0f;
		float competitionsBlockSize = (playBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing - gapBetweenCompetitionsBlock) / 2f;

		this.friendlyGameTitle.transform.localScale=ApplicationDesignRules.subMainTitleScale;
		this.officialGameTitle.transform.localScale=ApplicationDesignRules.subMainTitleScale;
		//this.cupGameTitle.transform.localScale=ApplicationDesignRules.subMainTitleScale;

		this.playBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.officialGameTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f, playBlockUpperLeftPosition.y - 1.2f, 0f);
		this.friendlyGameTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 1f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.2f, 0f);
		//this.cupGameTitle.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 2f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.2f, 0f);
		
		this.divisionGamePicture.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing + playBlockUpperLeftPosition.x + competitionsBlockSize / 2f, playBlockUpperLeftPosition.y - 1.95f, 0f);
		this.trainingGamePicture.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing + playBlockUpperLeftPosition.x + competitionsBlockSize / 2f, playBlockUpperLeftPosition.y - 1.95f, 0f);
		this.friendlyGamePicture.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 1f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.95f, 0f);

		//this.cupGamePicture.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 2f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.95f, 0f);

		this.officialGameButton.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing + playBlockUpperLeftPosition.x + competitionsBlockSize / 2f, playBlockUpperLeftPosition.y - 2.9f, 0f);
		this.friendlyGameButton.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 1f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 2.9f, 0f);

		//this.cupGameButton.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 2f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 2.9f, 0f);
		
		this.friendlyGameButton.transform.localScale = ApplicationDesignRules.button62Scale;
		this.officialGameButton.transform.localScale = ApplicationDesignRules.button62Scale;
		//this.cupGameButton.transform.localScale = ApplicationDesignRules.button62Scale;
		
		this.deckBlock.GetComponent<NewBlockController> ().resize(deckBlockLeftMargin,deckBlockUpMargin,ApplicationDesignRules.blockWidth,deckBlockHeight);
		Vector3 deckBlockUpperLeftPosition = this.deckBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 deckBlockUpperRightPosition = this.deckBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 deckBlockSize = this.deckBlock.GetComponent<NewBlockController> ().getSize ();
		this.deckBlockTitle.transform.position = new Vector3 (deckBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, deckBlockUpperLeftPosition.y - 0.2f, 0f);
		this.deckBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.deckSelectionButton.transform.position = new Vector3 (deckBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, deckBlockUpperRightPosition.y - ApplicationDesignRules.roundButtonWorldSize.y/2f-ApplicationDesignRules.buttonVerticalSpacing, 0f);
		this.deckSelectionButton.transform.localScale = ApplicationDesignRules.roundButtonScale;

		for(int i=0;i<this.deckChoices.Length;i++)
		{
			this.deckChoices[i].transform.localScale=ApplicationDesignRules.listElementScale;
			this.deckChoices[i].transform.position=new Vector3(deckBlockUpperRightPosition.x-ApplicationDesignRules.listElementWorldSize.x/2f,this.deckSelectionButton.transform.position.y-ApplicationDesignRules.button62WorldSize.y/2f-(i+0.5f)*ApplicationDesignRules.listElementWorldSize.y+i*0.02f,-1f);
		}

		this.deckTitle.transform.position = new Vector3 (deckBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, deckBlockUpperLeftPosition.y-ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
		this.deckTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		float gapBetweenCardsHalo = (deckBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing - 4f * ApplicationDesignRules.cardHaloWorldSize.x) / 3f;

		this.deckCardsPosition=new Vector3[this.cardsHalos.Length];
		this.deckCardsArea=new Rect[this.cardsHalos.Length];
		this.deckCardsAreaHovered=new bool[this.cardsHalos.Length];
		
		for(int i=0;i<this.cardsHalos.Length;i++)
		{
			this.cardsHalos[i].transform.localScale=ApplicationDesignRules.cardHaloScale;
			this.cardsHalos[i].transform.position=new Vector3(deckBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+ApplicationDesignRules.cardHaloWorldSize.x/2f+i*(gapBetweenCardsHalo+ApplicationDesignRules.cardHaloWorldSize.x),deckBlockUpperRightPosition.y - cardFirstLine,0);
			this.deckCardsPosition[i]=this.cardsHalos[i].transform.position;
			this.deckCardsArea[i]=new Rect(this.cardsHalos[i].transform.position.x-ApplicationDesignRules.cardHaloWorldSize.x/2f-gapBetweenCardsHalo/2f,this.cardsHalos[i].transform.position.y-ApplicationDesignRules.cardHaloWorldSize.y/2f,ApplicationDesignRules.cardHaloWorldSize.x+gapBetweenCardsHalo,ApplicationDesignRules.cardHaloWorldSize.y);
			this.deckCards[i].transform.position=this.deckCardsPosition[i];
			this.deckCards[i].transform.localScale=ApplicationDesignRules.cardScale;
			this.deckCards[i].transform.GetComponent<NewCardHomePageController>().setId(i);
			this.deckCardsAreaHovered[i]=false;
		}
		
		this.storeBlock.GetComponent<NewBlockController> ().resize(storeBlockLeftMargin,storeBlockUpMargin,ApplicationDesignRules.blockWidth,storeBlockHeight);
		Vector3 storeBlockUpperLeftPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 storeBlockUpperRightPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 storeBlockLowerRightPosition = this.storeBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 storeBlockSize = this.storeBlock.GetComponent<NewBlockController> ().getSize ();
		this.storeBlockTitle.transform.position = new Vector3 (storeBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, storeBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.storeBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.pack.transform.position = new Vector3 (storeBlockLowerRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.packWorldSize.x / 2f, storeBlockLowerRightPosition.y + 0.1f + ApplicationDesignRules.packWorldSize.y / 2f, 0f);
		this.pack.GetComponent<NewPackHomePageController> ().resize ();

		this.newsfeedBlock.GetComponent<NewBlockController> ().resize(newsfeedBlockLeftMargin,newsfeedBlockUpMargin,ApplicationDesignRules.blockWidth,newsfeedBlockHeight);
		Vector3 newsfeedBlockUpperLeftPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 newsfeedBlockUpperRightPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 newsfeedBlockLowerLeftPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 newsfeedBlockLowerRightPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 newsfeedBlockSize = this.newsfeedBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 newsfeedBlockOrigin = this.newsfeedBlock.GetComponent<NewBlockController> ().getOriginPosition ();

		this.newsfeedBlockTitle.transform.position = new Vector3 (newsfeedBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, newsfeedBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);

		this.slideRightButton.transform.position = new Vector3 (newsfeedBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, newsfeedBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);
		this.slideRightButton.transform.localScale = ApplicationDesignRules.roundButtonScale;

		float gapBetweenSelectionsButtons = 0.02f;

		float contentWidth = newsfeedBlockSize.x - 2f * ApplicationDesignRules.blockHorizontalSpacing;
		float lineScale = ApplicationDesignRules.getLineScale (contentWidth);

		this.contents=new GameObject[newsfeedPagination.nbElementsPerPage];

		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i]=Instantiate (this.contentObject) as GameObject;
			this.contents[i].transform.position=new Vector3(newsfeedBlockUpperLeftPosition.x+newsfeedBlockSize.x/2f,newsfeedBlockUpperLeftPosition.y-contentFirstLineY-(i+1)*contentHeight,0f);
			this.contents[i].transform.FindChild("line").localScale=new Vector3(lineScale,1f,1f);
			this.contents[i].transform.FindChild("picture").localScale=ApplicationDesignRules.thumbScale;
			this.contents[i].transform.FindChild("picture").localPosition=new Vector3(-contentWidth/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(contentHeight-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.contents[i].transform.FindChild("divisionIcon").localScale=ApplicationDesignRules.divisionIconScale;
			this.contents[i].transform.FindChild("divisionIcon").localPosition=new Vector3(ApplicationDesignRules.divisionIconDistance.x-contentWidth/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(contentHeight-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f+ApplicationDesignRules.divisionIconDistance.y,0f);
			this.contents[i].transform.FindChild("username").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("username").GetComponent<TextMeshPro>().textContainer.width=(contentWidth)-0.2f-2f*ApplicationDesignRules.button31WorldSize.x;
			this.contents[i].transform.FindChild("username").localPosition=new Vector3(-contentWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.2f,contentHeight-(contentHeight-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.contents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=contentWidth-0.2f-ApplicationDesignRules.thumbWorldSize.x-2f*ApplicationDesignRules.button31WorldSize.x;
			this.contents[i].transform.FindChild("description").localPosition=new Vector3(-contentWidth/2f+ApplicationDesignRules.thumbWorldSize.x+0.2f,contentHeight-(contentHeight-ApplicationDesignRules.thumbWorldSize.y)/2f-0.25f,0f);
			this.contents[i].transform.FindChild("date").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("date").GetComponent<TextMeshPro>().textContainer.width=(contentWidth/4f);
			this.contents[i].transform.FindChild("date").localPosition=new Vector3(contentWidth/2f,contentHeight-(contentHeight-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.contents[i].transform.FindChild("new").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("new").GetComponent<TextMeshPro>().textContainer.width=(contentWidth/4f);
			this.contents[i].transform.FindChild("new").localPosition=new Vector3(contentWidth/2f,contentHeight/2f,0f);
		}
		this.initializeContents ();
		this.challengeButtons=new GameObject[newsfeedPagination.nbElementsPerPage];
		for(int i=0;i<this.challengeButtons.Length;i++)
		{
			this.challengeButtons[i]=Instantiate (this.challengeButtonObject) as GameObject;
			this.challengeButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
			this.challengeButtons[i].transform.position=new Vector3(newsfeedBlockUpperRightPosition.x-ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.button62WorldSize.x/2f,newsfeedBlockUpperRightPosition.y-contentFirstLineY-(i+0.5f)*contentHeight,0f);

		}
		this.initializeChallengeButtons ();
		this.friendsStatusButtons=new GameObject[2*newsfeedPagination.nbElementsPerPage];
		for(int i=0;i<this.newsfeedPagination.nbElementsPerPage;i++)
		{
			this.friendsStatusButtons[i*2]=Instantiate(this.friendsStatusButtonObject) as GameObject;
			this.friendsStatusButtons[i*2+1]=Instantiate(this.friendsStatusButtonObject) as GameObject;
			this.friendsStatusButtons[i*2].transform.localScale = ApplicationDesignRules.button31Scale;
			this.friendsStatusButtons[i*2+1].transform.localScale = ApplicationDesignRules.button31Scale;
			this.friendsStatusButtons[i*2].transform.position=new Vector3(newsfeedBlockUpperRightPosition.x-ApplicationDesignRules.blockHorizontalSpacing-3*ApplicationDesignRules.button31WorldSize.x/2f,newsfeedBlockUpperRightPosition.y-contentFirstLineY-(i+1f)*contentHeight+ApplicationDesignRules.button31WorldSize.y/2f,0f);
			this.friendsStatusButtons[i*2+1].transform.position=new Vector3(newsfeedBlockUpperRightPosition.x-ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.button31WorldSize.x/2f,newsfeedBlockUpperRightPosition.y-contentFirstLineY-(i+1f)*contentHeight+ApplicationDesignRules.button31WorldSize.y/2f,0f);
		}
		this.initializeFriendsStatusButton ();
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().resize ();

		this.focusedCard.transform.localScale = ApplicationDesignRules.cardFocusedScale;
		this.focusedCard.transform.position = ApplicationDesignRules.focusedCardPosition;
		this.focusedCard.GetComponent<NewFocusedCardHomePageController> ().resize ();

		this.mainContentPositionX = playBlockOrigin.x;
		this.newsfeedPositionX = newsfeedBlockOrigin.x;

		this.endGamePopUp.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);

		if(ApplicationDesignRules.isMobileScreen)
		{
			for(int i=0;i<this.tabs.Length;i++)
			{
				this.tabs[i].transform.localScale = ApplicationDesignRules.tabScale;
			}
			this.tabs[0].transform.position = new Vector3 (newsfeedBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f, newsfeedBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
			this.tabs[2].transform.position = new Vector3 (newsfeedBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f+ ApplicationDesignRules.tabWorldSize.x+gapBetweenSelectionsButtons, newsfeedBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
			this.hideActiveTab();
			this.newsfeedPaginationButtons.transform.localPosition=new Vector3 (newsfeedBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 2.5f*ApplicationDesignRules.roundButtonWorldSize.x, newsfeedBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		}
		else
		{
			for(int i=0;i<this.tabs.Length;i++)
			{
				this.tabs[i].SetActive(true);
				this.tabs[i].transform.localScale = ApplicationDesignRules.tabScale;
				this.tabs[i].transform.position = new Vector3 (newsfeedBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f+ i*(ApplicationDesignRules.tabWorldSize.x+gapBetweenSelectionsButtons), newsfeedBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
			}
			this.newsfeedPaginationButtons.transform.localPosition=new Vector3 (newsfeedBlockLowerLeftPosition.x + newsfeedBlockSize.x / 2, newsfeedBlockLowerLeftPosition.y + 0.3f, 0f);
		}
		if(this.isConnectionBonusPopUpDisplayed)
		{
			this.connectionBonusPopUpResize();
		}
		else if(isEndGamePopUpDisplayed)
		{
			this.endGamePopUpResize();
		}

		MenuController.instance.resize();
		MenuController.instance.setCurrentPage(0);
		MenuController.instance.refreshMenuObject();
		HelpController.instance.resize();
	}
	private void retrieveDefaultDeck()
	{
		if(ApplicationModel.player.MyDecks.getCount()>0)
		{
			this.deckDisplayed = 0;
			for(int i=0;i<ApplicationModel.player.MyDecks.getCount();i++)
			{
				if(ApplicationModel.player.MyDecks.getDeck(i).Id==ApplicationModel.player.SelectedDeckId)
				{
					this.deckDisplayed=i;
					break;
				}
			}
		}
		else
		{
			this.deckDisplayed=-1;
		}
	}
	private void retrieveDecksList()
	{
		this.decksDisplayed=new List<int>();
		if(this.deckDisplayed!=-1)
		{
			for(int i=0;i<ApplicationModel.player.MyDecks.getCount();i++)
			{
				if(i!=this.deckDisplayed)
				{
					this.decksDisplayed.Add (i);
				}
			}
		}
	}
	public void drawDeckCards()
	{
		this.deckCardsDisplayed = new int[]{-1,-1,-1,-1};
		if(this.deckDisplayed!=-1)
		{	
			for(int i=0;i<ApplicationModel.player.MyDecks.getDeck(this.deckDisplayed).cards.Count;i++)
			{
				int deckOrder = ApplicationModel.player.MyDecks.getDeck(this.deckDisplayed).cards[i].deckOrder;
				int cardId=ApplicationModel.player.MyDecks.getDeck(this.deckDisplayed).cards[i].Id;
				for(int j=0;j<ApplicationModel.player.MyCards.getCount();j++)
				{
					if(ApplicationModel.player.MyCards.getCard(j).Id==cardId)
					{
						this.deckCardsDisplayed[deckOrder]=j;
						break;
					}
				}
			}
			this.deckTitle.GetComponent<TextMeshPro> ().text = ApplicationModel.player.MyDecks.getDeck(this.deckDisplayed).Name.ToUpper();
			if(ApplicationModel.player.MyDecks.getCount()>1)
			{
				this.deckSelectionButton.SetActive(true);
			}
			else
			{
				this.deckSelectionButton.SetActive(false);
			}
		}
		else
		{
			this.deckTitle.GetComponent<TextMeshPro> ().text = WordingDeck.getReference(4).ToUpper();
			this.deckSelectionButton.gameObject.SetActive(false);
		}
		for(int i=0;i<this.deckCards.Length;i++)
		{
			if(this.deckCardsDisplayed[i]!=-1)
			{
				this.deckCards[i].transform.GetComponent<NewCardController>().c=ApplicationModel.player.MyCards.getCard(this.deckCardsDisplayed[i]);
				this.deckCards[i].transform.GetComponent<NewCardController>().show();
				this.deckCards[i].SetActive(true);
			}
			else
			{
				this.deckCards[i].SetActive(false);
			}
		}
	}
	public void showCardFocused()
	{
		this.isCardFocusedDisplayed = true;
		this.isHovering=false;
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		this.displayBackUI (false);
		this.focusedCard.SetActive (true);
		this.focusedCardIndex=this.deckCardsDisplayed[this.idCardClicked];
		this.focusedCard.GetComponent<NewFocusedCardController>().c=ApplicationModel.player.MyCards.getCard(this.deckCardsDisplayed[this.idCardClicked]);
		this.focusedCard.GetComponent<NewFocusedCardController> ().show ();
	}
	public void hideCardFocused()
	{
		this.isCardFocusedDisplayed = false;
		this.displayBackUI (true);
		this.focusedCard.SetActive (false);
		this.deckCards[this.idCardClicked].GetComponent<NewCardController>().show();
	}
	public void displayBackUI(bool value)
	{
		if(value)
		{
			this.sceneCamera.transform.position=ApplicationDesignRules.sceneCameraStandardPosition;
		}
		else
		{
			this.sceneCamera.transform.position=ApplicationDesignRules.sceneCameraFocusedCardPosition;
		}
	}
	public void selectDeck(int id)
	{
		SoundController.instance.playSound(9);
		this.deckDisplayed = this.decksDisplayed [id];
		this.isSearchingDeck = false;
		this.cleanDeckList ();
		this.initializeDecks ();
		StartCoroutine(ApplicationModel.player.SetSelectedDeck(ApplicationModel.player.MyDecks.getDeck(this.deckDisplayed).Id));
	}
	public void deckSelectionButtonHandler()
	{
		if (HelpController.instance.canAccess (-1)) {
			SoundController.instance.playSound (9);
			if (this.deckDisplayed != -1) {
				this.displayDeckList ();
			} else {
				BackOfficeController.instance.loadScene ("newMyGame");
			}
		}
	}
	public void displayDeckList()
	{
		if(!isSearchingDeck)
		{
			this.setDeckList ();
			this.isSearchingDeck=true;
		}
	}
	private void setDeckList()
	{
		for (int i = 0; i < this.deckChoices.Length; i++) 
		{  
			if(i<this.decksDisplayed.Count)
			{
				this.deckChoices[i].SetActive(true);
				this.deckChoices[i].transform.GetComponent<NewHomePageDeckChoiceController>().reset();
				this.deckChoices[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=ApplicationModel.player.MyDecks.getDeck(this.decksDisplayed[i]).Name;
			}
			else
			{
				this.deckChoices[i].SetActive(false);
			}

		}
	}
	private void cleanDeckList()
	{
		for (int i = 0; i < this.deckChoices.Length; i++) 
		{  
			this.deckChoices[i].SetActive(false);
		}
	}
	public void mouseOnSelectDeckButton(bool value)
	{
		this.isMouseOnSelectDeckButton = value;
	}
	public void mouseOnBuyPackButton(bool value)
	{
		this.isMouseOnBuyPackButton = value;
	}
	public void backOfficeBackgroundClicked()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().escapePressed();
		}
	}
	public void returnPressed()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().returnPressed();
		}
		else if(isEndGamePopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideEndGamePopUp();
		}
		else if(isConnectionBonusPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideConnectionBonusPopUp();
		}
        else if(isHasLeftRoomPopUpDisplayed)
        {
            SoundController.instance.playSound(8);
            this.hideHasLeftRoomPopUp();
        }
	}
	public void escapePressed()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().escapePressed();
		}
		else if(isEndGamePopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideEndGamePopUp();
		}
		else if(isConnectionBonusPopUpDisplayed)
		{
			SoundController.instance.playSound(8);
			this.hideConnectionBonusPopUp();
		}
        else if(isHasLeftRoomPopUpDisplayed)
        {
            SoundController.instance.playSound(8);
            this.hideHasLeftRoomPopUp();
        }
		else
		{
			SoundController.instance.playSound(8);
			BackOfficeController.instance.leaveGame();
		}
	}
	public void closeAllPopUp()
	{
		if(isEndGamePopUpDisplayed)
		{
			this.hideEndGamePopUp();
		}
		else if(isConnectionBonusPopUpDisplayed)
		{
			this.hideConnectionBonusPopUp();
		}
	}
	public void leftClickedHandler(int id)
	{
		if (HelpController.instance.canAccess (-1)) {
			this.idCardClicked = id;
			this.isLeftClicked = true;
			this.clickInterval = 0f;
		}
	}
	public void leftClickReleaseHandler()
	{
		if (HelpController.instance.canAccess (-1)) {
			this.isLeftClicked = false;
			if (isDragging) {
				this.endDragging ();
			} else if (!BackOfficeController.instance.getIsSwiping ()) {
				SoundController.instance.playSound (4);
				this.showCardFocused ();
			}
		}
	}
	public void isHoveringCard()
	{
		if(!isHovering)
		{
			this.isHovering=true;
			if(!this.isDragging)
			{
				Cursor.SetCursor (this.cursorTextures[0], new Vector2(this.cursorTextures[0].width/2f,this.cursorTextures[0].width/2f), CursorMode.Auto);
			}
		}
	}
	public void endHoveringCard()
	{
		if(this.isHovering)
		{
			this.isHovering=false;
			if(!this.isDragging)
			{
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
		}
	}
	public void startDragging()
	{
		if(!this.isDragging)
		{
			SoundController.instance.playSound(2);
			this.isDragging=true;
			Cursor.SetCursor (this.cursorTextures[1], new Vector2(this.cursorTextures[1].width/2f,this.cursorTextures[1].width/2f), CursorMode.Auto);
			this.deckCards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(10,"Foreground");
			this.isHoveringDeckArea=true;
		}
	}
	public void endDragging()
	{
		this.isDragging=false;
		if(this.isHovering)
		{
			Cursor.SetCursor (this.cursorTextures[0], new Vector2(this.cursorTextures[0].width/2f,this.cursorTextures[0].width/2f), CursorMode.Auto);
		}
		else
		{
			Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		}
		this.deckCards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(-10,"Foreground");
		this.deckCards[this.idCardClicked].transform.position=this.deckCardsPosition[this.idCardClicked];
		for(int i=0;i<this.cardsHalos.Length;i++)
		{
			this.cardsHalos[i].GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
		}
		Vector3 cursorPosition = this.sceneCamera.GetComponent<Camera>().ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		for(int i=0;i<deckCardsArea.Length;i++)
		{
			if(this.deckCardsArea[i].Contains(cursorPosition))
			{
				
				SoundController.instance.playSound(1);
				this.moveToDeckCards(i);
				break;
			}
		}
	}
	public void isDraggingCard()
	{
		if(isDragging)
		{
			Vector3 mousePosition = this.sceneCamera.GetComponent<Camera>().ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			float correction=0f;
			if(ApplicationDesignRules.isMobileScreen)
			{
				correction =-ApplicationDesignRules.upMargin;
			}
			Vector3 cardsPosition = new Vector3(mousePosition.x+ApplicationDesignRules.menuPosition.x,mousePosition.y+ApplicationDesignRules.menuPosition.y+correction,0f);
			this.deckCards[this.idCardClicked].transform.position=cardsPosition;
			bool isHoveringDeckCards=false;
			for(int i=0;i<deckCardsArea.Length;i++)
			{
				if(this.deckCardsArea[i].Contains(mousePosition))
				{
					isHoveringDeckCards=true;
					if(!this.deckCardsAreaHovered[i])
					{
						this.deckCardsAreaHovered[i]=true;
						this.cardsHalos[i].GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
						this.cardsHalos[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
					}
				}
				else
				{
					if(this.deckCardsAreaHovered[i])
					{
						this.deckCardsAreaHovered[i]=false;
						this.cardsHalos[i].GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
						this.cardsHalos[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
					}
				}
			}
			if(!isHoveringDeckCards && this.isHoveringDeckArea)
			{
				this.isHoveringDeckArea=false;
				for(int i=0;i<this.cardsHalos.Length;i++)
				{
					this.cardsHalos[i].GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
					this.cardsHalos[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
				}
			}
			else if(isHoveringDeckCards && !this.isHoveringDeckArea)
			{
				this.isHoveringDeckArea=true;
				for(int i=0;i<this.cardsHalos.Length;i++)
				{
					if(!this.deckCardsAreaHovered[i])
					{
						this.cardsHalos[i].GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
						this.cardsHalos[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
					}
				}
			}
		}
	}
	public void moveToDeckCards(int position)
	{
		int idCard1 = ApplicationModel.player.MyCards.getCard(this.deckCardsDisplayed [this.idCardClicked]).Id;
		this.deckCards[position].SetActive(true);
		this.deckCards[position].GetComponent<NewCardController>().c=ApplicationModel.player.MyCards.getCard(this.deckCardsDisplayed [this.idCardClicked]);
		this.deckCards[position].GetComponent<NewCardController>().show();
		if(this.deckCardsDisplayed[position]!=-1)
		{
			int indexCard2=this.deckCardsDisplayed[position];
			int idCard2=ApplicationModel.player.MyCards.getCard(indexCard2).Id;
			this.deckCards[position].GetComponent<NewCardController>().c=ApplicationModel.player.MyCards.getCard(this.deckCardsDisplayed [this.idCardClicked]);
			this.deckCards[position].GetComponent<NewCardController>().show ();
			this.deckCardsDisplayed[position]=this.deckCardsDisplayed[this.idCardClicked];
			this.deckCards[this.idCardClicked].GetComponent<NewCardController>().c=ApplicationModel.player.MyCards.getCard(indexCard2);
			this.deckCards[this.idCardClicked].GetComponent<NewCardController>().show ();
			this.deckCardsDisplayed[this.idCardClicked]=indexCard2;
			StartCoroutine(this.changeDeckCardsOrder(idCard1,position,idCard2,this.idCardClicked));
		}
		else
		{
			this.deckCardsDisplayed[position]=this.deckCardsDisplayed[this.idCardClicked];
			this.deckCards[this.idCardClicked].SetActive(false);
			this.deckCardsDisplayed[this.idCardClicked]=-1;
			StartCoroutine(changeDeckCardsOrder(idCard1,position,-1,-1));
		}
	}
	public IEnumerator changeDeckCardsOrder(int idCard1, int deckOrder1, int idCard2, int deckOrder2)
	{
		yield return StartCoroutine(ApplicationModel.player.MyDecks.getDeck(this.deckDisplayed).changeCardsOrder(idCard1,deckOrder1,idCard2,deckOrder2));
	}
	public void drawNotifications()
	{
		this.notificationsDisplayed = new List<int> ();
		for(int i =0;i<this.newsfeedPagination.nbElementsPerPage;i++)
		{
			if(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i<ApplicationModel.player.MyNotifications.getCount())
			{
				this.notificationsDisplayed.Add (this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i);
				this.contents[i].SetActive(true);
				this.drawContentUser(i,ApplicationModel.player.Users.getUser(ApplicationModel.player.MyNotifications.getNotification(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i).SendingUser));
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=ApplicationModel.player.MyNotifications.getNotification(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i).Content;
				this.contents[i].transform.FindChild("date").GetComponent<TextMeshPro>().text=ApplicationModel.player.MyNotifications.getNotification(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i).Date.ToString(WordingDates.getDateFormat());
				if(!ApplicationModel.player.MyNotifications.getNotification(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i).IsRead)
				{
					this.contents[i].transform.FindChild("new").gameObject.SetActive(true);
				}
				else
				{
					this.contents[i].transform.FindChild("new").gameObject.SetActive(false);
				}
				if(ApplicationModel.player.MyNotifications.getNotification(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i).IdNotificationType==4)
				{
					this.friendsStatusButtons[2*i].SetActive(true);
					this.friendsStatusButtons[2*i+1].SetActive(true);
				}
				else
				{
					this.friendsStatusButtons[2*i].SetActive(false);
					this.friendsStatusButtons[2*i+1].SetActive(false);
				}

			}
			else
			{
				this.contents[i].SetActive(false);
				this.friendsStatusButtons[2*i].SetActive(false);
				this.friendsStatusButtons[2*i+1].SetActive(false);
			}
		}
		this.manageNonReadsNotifications();
	}
	public void drawNews()
	{
		this.newsDisplayed = new List<int> ();
		for(int i =0;i<this.newsfeedPagination.nbElementsPerPage;i++)
		{
			if(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i<ApplicationModel.player.MyNews.getCount())
			{
				this.newsDisplayed.Add (this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i);
				this.contents[i].SetActive(true);
				this.drawContentUser(i,ApplicationModel.player.Users.getUser(ApplicationModel.player.MyNews.getNews(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i).User));
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=ApplicationModel.player.MyNews.getNews(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i).Content;
				this.contents[i].transform.FindChild("date").GetComponent<TextMeshPro>().text=ApplicationModel.player.MyNews.getNews(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i).Date.ToString("dd/MM/yyyy");
			}
			else
			{
				this.contents[i].SetActive(false);
			}
		}
	}
	public void drawFriends()
	{
		this.friendsDisplayed = new List<int> ();
		for(int i =0;i<this.newsfeedPagination.nbElementsPerPage;i++)
		{
			if(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i<this.friendsToBeDisplayed.Count)
			{
				this.friendsDisplayed.Add (this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i);
				this.contents[i].SetActive(true);
				string connectionState="";
				Color connectionStateColor=new Color();
				switch(ApplicationModel.player.Users.getUser(this.friendsToBeDisplayed[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i]).OnlineStatus)
				{
				case 0:
					connectionState = WordingSocial.getReference(3);
					connectionStateColor=ApplicationDesignRules.whiteTextColor;
					this.challengeButtons[i].SetActive(false);
					break;
				case 1:
					connectionState = WordingSocial.getReference(4);
					connectionStateColor=ApplicationDesignRules.blueColor;
					this.challengeButtons[i].SetActive(true);
					break;
				case 2:
					connectionState = WordingSocial.getReference(5);
					connectionStateColor=ApplicationDesignRules.redColor;
					this.challengeButtons[i].SetActive(false);
					break;
				}
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=connectionState;
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=connectionStateColor;
				this.drawContentUser(i,ApplicationModel.player.Users.getUser(this.friendsToBeDisplayed[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i]));
			}
			else
			{
				this.contents[i].SetActive(false);
				this.challengeButtons[i].SetActive(false);
			}
		}
	}
	public void drawContentUser(int contentId, User user)
	{
		this.contents[contentId].transform.FindChild("picture").GetComponent<NewHomePageContentPictureController>().reset();
		this.contents[contentId].transform.FindChild("username").GetComponent<NewHomePageContentUsernameController>().reset();
		if(user.isPublic)
		{
			this.contents[contentId].transform.FindChild("picture").GetComponent<NewHomePageContentPictureController>().setIsActive(true);
			this.contents[contentId].transform.FindChild("username").GetComponent<NewHomePageContentUsernameController>().setIsActive(true);
		}
		else
		{
			this.contents[contentId].transform.FindChild("picture").GetComponent<NewHomePageContentPictureController>().setIsActive(false);
			this.contents[contentId].transform.FindChild("username").GetComponent<NewHomePageContentUsernameController>().setIsActive(false);
		}
        this.contents[contentId].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnLargeProfilePicture(user.IdProfilePicture);
		this.contents[contentId].transform.FindChild("username").GetComponent<TextMeshPro>().text=user.Username;
		if(user.TrainingStatus!=-1)
		{
			this.contents[contentId].transform.FindChild("divisionIcon").gameObject.SetActive(false);
		}
		else
		{
			this.contents[contentId].transform.FindChild("divisionIcon").gameObject.SetActive(true);
			this.contents[contentId].transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setDivision(user.Division);		
		}
	}
	public void drawPack()
	{
		this.pack.GetComponent<NewPackHomePageController> ().show (ApplicationModel.packs.getPack(displayedPack));
	}
	public void drawCompetitions()
	{	
		this.friendlyGameTitle.GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(0).ToUpper ();
		this.friendlyGameButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(2);

		if(ApplicationModel.player.TrainingStatus!=-1)
		{
			this.divisionGamePicture.SetActive(false);
			this.trainingGamePicture.SetActive(true);
			this.trainingGamePicture.GetComponent<TrainingController>().draw(ApplicationModel.player.TrainingStatus);
			this.officialGameTitle.GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(10).ToUpper ();
			this.officialGameButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(11);
		}
		else
		{
			this.divisionGamePicture.SetActive(true);
			this.trainingGamePicture.SetActive(false);
			this.officialGameTitle.GetComponent<TextMeshPro> ().text = WordingGameModes.getReference(1).ToUpper ();
			string divisionState;
			if(ApplicationModel.player.CurrentDivision.GamesPlayed>0)
			{
				divisionState=WordingGameModes.getReference(3);
			}
			else
			{
				divisionState=WordingGameModes.getReference(4);
			}
			this.officialGameButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = divisionState;
		}
	}
	public void buyPackHandler()
	{
		if (HelpController.instance.canAccess (-1)) {
			SoundController.instance.playSound (9);
			ApplicationModel.player.PackToBuy = ApplicationModel.packs.getPack (this.displayedPack).Id;
			BackOfficeController.instance.loadScene ("NewStore");
		}
	}
	private void manageNonReadsNotifications()
	{
		this.nonReadNotificationsOnCurrentPage=0;
		if(!ApplicationDesignRules.isMobileScreen || (this.newsfeedDisplayed && this.activeTab==0))
		{
			this.updateReadNotifications();	
		}
		else
		{
			menu.GetComponent<MenuController>().setNbNotificationsNonRead(ApplicationModel.player.NbNotificationsNonRead);
			menu.GetComponent<MenuController>().refreshMenuObject();
		}
	}
	private void updateReadNotifications()
	{
		IList<int> tempList = new List<int> ();
		for (int i=0;i<this.notificationsDisplayed.Count;i++)
		{
			if(i==ApplicationModel.player.MyNotifications.notificationSystemIndex)
			{
				tempList.Add (this.notificationsDisplayed[i]);
				ApplicationModel.player.MyNotifications.notificationSystemIndex=-1;
			}
			else if(!ApplicationModel.player.MyNotifications.getNotification(this.notificationsDisplayed[i]).IsRead)
			{
				tempList.Add (this.notificationsDisplayed[i]);
			}
		}
		menu.GetComponent<MenuController>().setNbNotificationsNonRead(ApplicationModel.player.NbNotificationsNonRead);
		menu.GetComponent<MenuController>().refreshMenuObject();
		if(tempList.Count>0)
		{
			this.nonReadNotificationsOnCurrentPage=tempList.Count;
			ApplicationModel.player.NbNotificationsNonRead=ApplicationModel.player.NbNotificationsNonRead-tempList.Count;
			StartCoroutine(ApplicationModel.player.MyNotifications.updateReadNotifications (tempList,this.totalNbResultLimit));
		}
	}
	public int getNonReadNotificationsOnCurrentPage()
	{
		return this.nonReadNotificationsOnCurrentPage;
	}
	public int getNbGamesDivision()
	{
		return ApplicationModel.player.CurrentDivision.GamesPlayed;
	}
	private void showEndGameSequence()
	{
		if(ApplicationModel.player.ChosenGameType==0)
		{
			this.displayEndGamePopUp();
		}
		else if(ApplicationModel.player.ChosenGameType>0&&ApplicationModel.player.ChosenGameType<11)
		{
			this.displayTrainingPopUp();
		}
	}
	public void displayConnectionBonusPopUp()
	{
		SoundController.instance.playSound(3);
		BackOfficeController.instance.displayTransparentBackground ();
		this.connectionBonusPopUp.transform.GetComponent<ConnectionBonusPopUpController> ().reset (ApplicationModel.player.ConnectionBonus);
		this.isConnectionBonusPopUpDisplayed = true;
		this.connectionBonusPopUp.SetActive (true);
		this.connectionBonusPopUpResize();
	}
    public void displayHasLeftRoomPopUp()
    {
        SoundController.instance.playSound(3);
        BackOfficeController.instance.displayTransparentBackground ();
        this.hasLeftRoomPopUp.transform.GetComponent<HasLeftRoomPopUpController> ().reset ();
        this.isHasLeftRoomPopUpDisplayed = true;
        this.hasLeftRoomPopUp.SetActive (true);
        this.hasLeftRoomPopUpResize();
    }
	public void displayWonPackPopUp()
	{
		SoundController.instance.playSound(3);
		BackOfficeController.instance.displayTransparentBackground ();
		this.wonPackPopUp.transform.GetComponent<WonPackPopUpController> ().reset ();
		this.wonPackPopUp.SetActive (true);
		this.wonPackPopUpResize();
	}
	private void displayEndGamePopUp()
	{
		BackOfficeController.instance.displayTransparentBackground ();
		this.endGamePopUp.transform.GetComponent<EndGamePopUpController> ().reset (ApplicationModel.player.HasWonLastGame);
		this.isEndGamePopUpDisplayed = true;
		this.endGamePopUp.SetActive (true);
		this.endGamePopUpResize();
	}
	private void displayTrainingPopUp()
	{
		BackOfficeController.instance.displayTransparentBackground ();
		this.trainingPopUp.transform.GetComponent<TrainingPopUpController> ().reset (ApplicationModel.player.HasWonLastGame);
		this.trainingPopUp.SetActive (true);
		this.trainingPopUpResize();
	}
	public void endGamePopUpResize()
	{
		this.endGamePopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.endGamePopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.endGamePopUp.GetComponent<EndGamePopUpController> ().resize ();
	}
    public void hasLeftRoomPopUpResize()
    {
        this.hasLeftRoomPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
        this.hasLeftRoomPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
        this.hasLeftRoomPopUp.GetComponent<HasLeftRoomPopUpController> ().resize ();
    }
	public void trainingPopUpResize()
	{
		this.trainingPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.trainingPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.trainingPopUp.GetComponent<TrainingPopUpController> ().resize ();
	}
	public void connectionBonusPopUpResize()
	{
		this.connectionBonusPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.connectionBonusPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.connectionBonusPopUp.GetComponent<ConnectionBonusPopUpController> ().resize ();
	}
	public void wonPackPopUpResize()
	{
		this.wonPackPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.wonPackPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.wonPackPopUp.GetComponent<WonPackPopUpController> ().resize ();
	}
	public void hideEndGamePopUp()
	{
		this.endGamePopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isEndGamePopUpDisplayed = false;
	}
	public void hideConnectionBonusPopUp()
	{
		this.connectionBonusPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		this.isConnectionBonusPopUpDisplayed = false;
	}
    public void hideHasLeftRoomPopUp()
    {
        this.hasLeftRoomPopUp.SetActive (false);
        BackOfficeController.instance.hideTransparentBackground();
        this.isHasLeftRoomPopUpDisplayed = false;
    }
	public void hideWonPackPopUp()
	{
		this.wonPackPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		BackOfficeController.instance.loadScene("NewStore");
	}
	public void hideTrainingPopUp()
	{
		this.trainingPopUp.SetActive (false);
		BackOfficeController.instance.hideTransparentBackground();
		if(ApplicationModel.player.HasToBuyTrainingPack)
		{
			BackOfficeController.instance.loadScene("NewStore");
		}
		else if(ApplicationModel.player.TutorialStep==6)
		{
			HelpController.instance.startTutorial();
		}
	}
	public void joinGameHandler(int id)
	{
		SoundController.instance.playSound(9);
		if(this.deckDisplayed==-1)
		{
			BackOfficeController.instance.displayErrorPopUp(WordingGameModes.getReference(5));
		}
		else if(id==0)
		{
			if (HelpController.instance.canAccess (-1)) 
			{
				ApplicationModel.player.ChosenGameType = 0;
				StartCoroutine (this.joinGame ());
			}
		}
		else if(ApplicationModel.player.TrainingStatus==-1)
		{
			ApplicationModel.player.ChosenGameType=10+ApplicationModel.player.CurrentDivision.Id;
			StartCoroutine (this.joinGame ());
		}
		else if(!ApplicationModel.player.canAccessTrainingMode())
		{
			BackOfficeController.instance.displayErrorPopUp(WordingGameModes.getReference(12)+" "+WordingCardTypes.getName(ApplicationModel.player.TrainingAllowedCardType));
		}
		else
		{
			ApplicationModel.player.ChosenGameType=1+ApplicationModel.player.TrainingAllowedCardType;
			StartCoroutine (this.joinGame ());
		}
	}
	public IEnumerator joinGame()
	{
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine (ApplicationModel.player.SetSelectedDeck (ApplicationModel.player.MyDecks.getDeck(this.deckDisplayed).Id));
		if(ApplicationModel.player.ChosenGameType>10)
		{
            BackOfficeController.instance.loadScene("NewLobby");
		}
		else
		{
			BackOfficeController.instance.joinRandomRoomHandler();
		}
	}
	public Vector3 getEndGamePopUpButtonPosition()
	{
		return this.endGamePopUp.transform.FindChild ("Button").position;
	}
	public void checkFriendsOnlineStatus()
	{
		if(PhotonNetwork.insideLobby)
		{
			PhotonNetwork.FindFriends (ApplicationModel.player.Users.usernameList);
		}
	}
	public void OnUpdatedFriendList()
	{
		for(int i=0;i<PhotonNetwork.Friends.Count;i++)
		{
			for(int j=0;j<ApplicationModel.player.Users.getCount();j++)
			{
				if(ApplicationModel.player.Users.getUser(j).Username==PhotonNetwork.Friends[i].Name)
				{
					if(PhotonNetwork.Friends[i].IsInRoom)
					{
						if(ApplicationModel.player.MyFriends.Contains(j))
						{
							if(!this.friendsOnline.Contains(j))
							{
								this.friendsOnline.Insert(0,j);
								ApplicationModel.player.Users.getUser(j).OnlineStatus=2;
							}
							else if(ApplicationModel.player.Users.getUser(j).OnlineStatus!=2)
							{
								this.friendsOnline.Remove(j);
								this.friendsOnline.Insert(0,j);
								ApplicationModel.player.Users.getUser(j).OnlineStatus=2;
							}
						}
					}
					else if(PhotonNetwork.Friends[i].IsOnline)
					{
						if(ApplicationModel.player.MyFriends.Contains(j))
						{
							if(!this.friendsOnline.Contains(j))
							{
								this.friendsOnline.Insert(0,j);
								ApplicationModel.player.Users.getUser(j).OnlineStatus=1;
							}
							else if(ApplicationModel.player.Users.getUser(j).OnlineStatus!=1)
							{
								this.friendsOnline.Remove(j);
								this.friendsOnline.Insert(0,j);
								ApplicationModel.player.Users.getUser(j).OnlineStatus=1;
							}
						}
					}
					else
					{
						ApplicationModel.player.Users.getUser(j).OnlineStatus=0;
					}
					break;
				}
			}
		}
		if(this.activeTab == 2 && !this.isCardFocusedDisplayed && this.newsfeedPagination.chosenPage==0)
		{
			this.initializeFriends();
		}
	}
	public void sortFriendsList()
	{
		this.friendsToBeDisplayed = new List<int> ();
		for(int i=0;i<this.friendsOnline.Count;i++)
		{
			this.friendsToBeDisplayed.Add (this.friendsOnline[i]);
		}
		for(int i=0;i<ApplicationModel.player.MyFriends.Count;i++)
		{
			if(!this.friendsToBeDisplayed.Contains(ApplicationModel.player.MyFriends[i]))
			{
				this.friendsToBeDisplayed.Add(ApplicationModel.player.MyFriends[i]);
			}
		}
	}
	public void sendInvitationHandler(int challengeButtonId)
	{
		if (HelpController.instance.canAccess (-1)) {
			SoundController.instance.playSound (9);
			if (this.deckDisplayed == -1) {
				BackOfficeController.instance.displayErrorPopUp (WordingGameModes.getReference (5));
			} else if (ApplicationModel.player.Users.getUser(this.friendsToBeDisplayed [this.friendsDisplayed [challengeButtonId]]).OnlineStatus != 1) {
				BackOfficeController.instance.displayErrorPopUp (WordingGameModes.getReference (6));
			} else {
				StartCoroutine (this.sendInvitation (challengeButtonId));
			}
		}
	}
	public IEnumerator sendInvitation(int challengeButtonId)
	{
		BackOfficeController.instance.displayLoadingScreen ();
		yield return StartCoroutine (ApplicationModel.player.SetSelectedDeck (ApplicationModel.player.MyDecks.getDeck(this.deckDisplayed).Id));
		StartCoroutine (BackOfficeController.instance.sendInvitation (ApplicationModel.player.Users.getUser(this.friendsToBeDisplayed[this.friendsDisplayed[challengeButtonId]]), ApplicationModel.player));
	}
	public void moneyUpdate()
	{
		if(isSceneLoaded)
		{
			if(this.isCardFocusedDisplayed)
			{
				this.focusedCard.GetComponent<NewFocusedCardHomePageController>().updateFocusFeatures();
			}
		}
	}
	public void clickOnContentProfile(int id)
	{
		if (HelpController.instance.canAccess (-1)) {
			SoundController.instance.playSound (9);
			ApplicationModel.player.ProfileChosen = this.contents [id].transform.FindChild ("username").GetComponent<TextMeshPro> ().text;
			BackOfficeController.instance.loadScene ("NewProfile");
		}
	}
	public void acceptFriendsRequestHandler(int id)
	{
		if (HelpController.instance.canAccess (-1)) {
			SoundController.instance.playSound (9);
			StartCoroutine (this.confirmFriendRequest (id));
		}
	}
	public void declineFriendsRequestHandler(int id)
	{
		if (HelpController.instance.canAccess (-1)) {
			SoundController.instance.playSound (9);
			StartCoroutine (this.removeFriendRequest (id));
		}
	}
	public IEnumerator confirmFriendRequest(int id)
	{
		SoundController.instance.playSound(9);
		BackOfficeController.instance.displayLoadingScreen ();
		Connection connection = new Connection ();
		connection.Id = System.Convert.ToInt32(ApplicationModel.player.MyNotifications.getNotification(this.notificationsDisplayed [id]).HiddenParam);
		yield return StartCoroutine (connection.confirm ());
		if(connection.Error=="")
		{
			ApplicationModel.player.moveToFriend(this.notificationsDisplayed[id]);
			this.initializeNotifications();
		}
		else
		{
			BackOfficeController.instance.displayErrorPopUp(connection.Error);
		}
		BackOfficeController.instance.hideLoadingScreen ();
	}
	public IEnumerator removeFriendRequest(int id)
	{
		SoundController.instance.playSound(9);
		BackOfficeController.instance.displayLoadingScreen ();
		Connection connection = new Connection ();
		connection.Id = System.Convert.ToInt32(ApplicationModel.player.MyNotifications.getNotification(this.notificationsDisplayed [id]).HiddenParam);
		yield return StartCoroutine(connection.remove ());
		if(connection.Error=="")
		{
			ApplicationModel.player.MyNotifications.remove(this.notificationsDisplayed[id]);
			this.initializeNotifications();
		}
		else
		{
			BackOfficeController.instance.displayErrorPopUp(connection.Error);
		}
		BackOfficeController.instance.hideLoadingScreen ();
	}
	public Camera returnCurrentCamera()
	{
		return this.sceneCamera.GetComponent<Camera>();
	}
	public void slideRight()
	{
		if (HelpController.instance.canAccess (-1)) {
			SoundController.instance.playSound (16);
			this.toSlideRight = true;
			this.toSlideLeft = false;
			this.newsfeedDisplayed = false;
		}
	}
	public void slideLeft()
	{
		if (HelpController.instance.canAccess (-1)) {
			SoundController.instance.playSound (16);
			this.toSlideLeft = true;
			this.toSlideRight = false;
			this.mainContentDisplayed = false;
		}
	}
	public void hideActiveTab()
	{
		this.newsfeedBlockTitle.GetComponent<TextMeshPro>().text=this.tabs[this.activeTab].transform.FindChild("Title").GetComponent<TextMeshPro>().text;
		switch(this.activeTab)
		{
		case 0:
			this.tabs[0].SetActive(false);
			this.tabs[1].transform.position=this.tabs[0].transform.position;
			this.tabs[1].SetActive(true);
			this.tabs[2].SetActive(true);
			break;
		case 1:
			this.tabs[0].SetActive(true);
			this.tabs[1].SetActive(false);
			this.tabs[2].SetActive(true);
			break;
		case 2:
			this.tabs[0].SetActive(true);
			this.tabs[1].transform.position=this.tabs[2].transform.position;
			this.tabs[1].SetActive(true);
			this.tabs[2].SetActive(false);
			break;
		}
	}
	public void cleanContents()
	{
		for(int i=0;i<this.contents.Length;i++)
		{
			Destroy(this.contents[i]);
		}
	}
	public void cleanChallengeButtons()
	{
		for(int i=0;i<this.challengeButtons.Length;i++)
		{
			Destroy(this.challengeButtons[i]);
		}
	}
	public void cleanFriendsStatusButtons()
	{
		for(int i=0;i<this.friendsStatusButtons.Length;i++)
		{
			Destroy(this.friendsStatusButtons[i]);
		}
	}
	public void initializeContents()
	{
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i].transform.FindChild("new").GetComponent<TextMeshPro>().text=WordingNotifications.getReference(0);
			this.contents[i].transform.FindChild("new").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.contents[i].transform.FindChild("username").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.contents[i].transform.FindChild("username").gameObject.AddComponent<NewHomePageContentUsernameController>();
			this.contents[i].transform.FindChild("username").GetComponent<NewHomePageContentUsernameController>().setId(i);
			this.contents[i].transform.FindChild("picture").gameObject.AddComponent<NewHomePageContentPictureController>();
			this.contents[i].transform.FindChild("picture").GetComponent<NewHomePageContentPictureController>().setId(i);
			this.contents[i].transform.FindChild("date").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.contents[i].transform.FindChild("line").GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
		}
	}
	public void initializeChallengeButtons()
	{
		for(int i=0;i<this.challengeButtons.Length;i++)
		{
			this.challengeButtons[i].AddComponent<NewHomePageChallengeButtonController>();
			this.challengeButtons[i].GetComponent<NewHomePageChallengeButtonController>().setId(i);
			this.challengeButtons[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(0);
			this.challengeButtons[i].SetActive(false);
		}
	}
	public void initializeFriendsStatusButton()
	{
		for(int i=0;i<this.contents.Length;i++)
		{
			this.friendsStatusButtons[2*i].AddComponent<NewHomePageFriendsStatusAcceptButtonController>();
			this.friendsStatusButtons[2*i].GetComponent<NewHomePageFriendsStatusAcceptButtonController>().setId(i);
			this.friendsStatusButtons[2*i+1].AddComponent<NewHomePageFriendsStatusDeclineButtonController>();
			this.friendsStatusButtons[2*i+1].GetComponent<NewHomePageFriendsStatusDeclineButtonController>().setId(i);
			this.friendsStatusButtons[2*i].SetActive(false);
			this.friendsStatusButtons[2*i+1].SetActive(false);
			this.friendsStatusButtons[2*i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(1);
			this.friendsStatusButtons[2*i+1].transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingSocial.getReference(2);
		}
	}
	public void refreshCredits()
	{
		StartCoroutine(BackOfficeController.instance.getUserData ());
	}
	#region TUTORIAL FUNCTIONS

	public GameObject returnStoreBlock()
	{
		return this.storeBlock;
	}
	public GameObject returnPlayBlock()
	{
		return this.playBlock;
	}
	public GameObject returnNewsfeedBlock()
	{
		return this.newsfeedBlock;
	}
	public GameObject returnDeckBlock()
	{
		return this.deckBlock;
	}
	public bool getIsCardFocusedDisplayed()
	{
		return isCardFocusedDisplayed;
	}
	public GameObject returnCardFocused()
	{
		return this.focusedCard;
	}
	public Vector3 getFocusedCardPosition()
	{
		return new Vector3(-ApplicationDesignRules.focusedCardPosition.x+this.focusedCard.transform.FindChild("Card").position.x,-ApplicationDesignRules.focusedCardPosition.y+this.focusedCard.transform.FindChild("Card").position.y,this.focusedCard.transform.FindChild("Card").position.z); 
	}
	public bool getIsMainContentDisplayed()
	{
		return this.mainContentDisplayed;
	}
	public Vector3 getOfficialGameButton()
	{
		return this.officialGameButton.transform.position;
	}
	#endregion
}
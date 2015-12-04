using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class NewHomePageController : MonoBehaviour
{
	public static NewHomePageController instance;
	private NewHomePageModel model;
	
	public GameObject blockObject;
	public GUISkin popUpSkin;
	public Texture2D[] cursorTextures;
	public int refreshInterval;
	public int sliderRefreshInterval;
	public int totalNbResultLimit;
	
	private GameObject deckBlock;
	private GameObject deckBlockTitle;
	private GameObject deckSelectionButton;
	private GameObject deckTitle;
	private GameObject playBlock;
	private GameObject playBlockTitle;
	private GameObject storeBlock;
	private GameObject storeBlockTitle;
	private GameObject packTitle;
	private GameObject packPicture;
	private GameObject packButton;
	private GameObject friendlyGameTitle;
	private GameObject friendlyGamePicture;
	private GameObject friendlyGameButton;
	private GameObject divisionGameButton;
	private GameObject divisionGamePicture;
	private GameObject divisionGameTitle;
	private GameObject cupGameButton;
	private GameObject cupGameTitle;
	private GameObject cupGamePicture;
	private GameObject newsfeedBlock;
	private GameObject[] tabs;
	private GameObject[] contents;
	private GameObject[] challengeButtons;
	private GameObject[] friendsStatusButtons;
	private GameObject newsfeedPaginationButtons;
	private GameObject[] deckChoices;
	private GameObject[] cardsHalos;
	private GameObject popUp;
	private GameObject mainCamera;
	private GameObject menuCamera;
	private GameObject tutorialCamera;
	private GameObject backgroundCamera;

	private GameObject menu;
	private GameObject tutorial;
	private GameObject[] deckCards;
	
	private GameObject endGamePopUp;

	private Rect centralWindow;

	private int activeTab;
	
	private IList<int> newsDisplayed;
	private IList<int> notificationsDisplayed;
	private int packDisplayed;
	private IList<int> friendsDisplayed;
	private IList<int> friendsToBeDisplayed;

	private IList<int> friendsOnline;
	
	private Pagination newsfeedPagination;

	private int nbPacks;
	private int displayedPack;

	private float sliderTimer;
	private float notificationsTimer;
	private bool isSceneLoaded;
	
	private int money;

	private Vector3[] deckCardsPosition;
	private Rect[] deckCardsArea;

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

	private int nbNonReadNotifications;

	private NewHomePageConnectionBonusPopUpView connectionBonusView;
	private bool isConnectionBonusViewDisplayed;

	private bool isEndGamePopUpDisplayed;
	private bool isScrolling;

	void Update()
	{	
		this.sliderTimer += Time.deltaTime;
		this.notificationsTimer += Time.deltaTime;

		if (notificationsTimer > refreshInterval && this.isSceneLoaded) 
		{
			StartCoroutine(this.refreshNonReadsNotifications());
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
		if(money!=ApplicationModel.credits)
		{
			if(isSceneLoaded)
			{
				if(this.isCardFocusedDisplayed)
				{
					this.focusedCard.GetComponent<NewFocusedCardHomePageController>().updateFocusFeatures();
				}
			}
			this.money=ApplicationModel.credits;
		}
		if(ApplicationDesignRules.isMobileScreen && this.isSceneLoaded)
		{
			isScrolling = this.mainCamera.GetComponent<ScrollingController>().ScrollController();
		}
	}
	void Awake()
	{
		instance = this;
		this.activeTab = 0;
		this.model = new NewHomePageModel ();
		this.newsfeedPagination = new Pagination ();
		this.newsfeedPagination.nbElementsPerPage= 3;
		this.friendsOnline = new List<int> ();
		this.initializeScene ();
		this.startMenuInitialization ();
	}
	private void startMenuInitialization()
	{
		this.menu = GameObject.Find ("Menu");
		this.menu.AddComponent<HomePageMenuController> ();
	}
	public void endMenuInitialization()
	{
		this.startTutorialInitialization ();
	}
	private void startTutorialInitialization()
	{
		this.tutorial = GameObject.Find ("Tutorial");
		this.tutorial.AddComponent<HomePageTutorialController>();
	}
	public void endTutorialInitialization()
	{
		StartCoroutine (this.initialization ());
	}
	public IEnumerator initialization()
	{
		this.resize ();
		yield return StartCoroutine (model.getData (this.totalNbResultLimit));
		this.selectATab ();
		this.initializePacks ();
		this.retrieveDefaultDeck ();
		this.initializeDecks ();
		this.initializeCompetitions ();
		this.checkFriendsOnlineStatus ();
		this.isSceneLoaded = true;
		if(ApplicationModel.launchEndGameSequence)
		{
			if(model.player.TutorialStep==-1)
			{
				this.launchEndGameSequence(ApplicationModel.hasWonLastGame);
			}
			ApplicationModel.launchEndGameSequence=false;
			ApplicationModel.hasWonLastGame=false;
		}
		if(model.player.TutorialStep!=-1)
		{
			TutorialObjectController.instance.startTutorial(model.player.TutorialStep,model.player.displayTutorial);
		}
		else if(model.player.ConnectionBonus>0)
		{
			this.displayConnectionBonusPopUp(model.player.ConnectionBonus);
		}
	}
	public void selectATabHandler(int idTab)
	{
		this.activeTab = idTab;
		this.selectATab ();
	}
	private void selectATab()
	{
		for(int i=0;i<this.tabs.Length;i++)
		{
			if(i==this.activeTab)
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(1);
				this.tabs[i].GetComponent<NewHomePageTabController>().setIsSelected(true);
				this.tabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(0);
				this.tabs[i].GetComponent<NewHomePageTabController>().reset();
			}
		}
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
		this.newsfeedPagination.totalElements= model.notifications.Count;
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().p = this.newsfeedPagination;
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().setPagination ();
		for(int i=0;i<this.contents.Length;i++)
		{
			this.challengeButtons[i].SetActive(false);
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		this.drawNotifications (true);
	}
	private void initializeNews()
	{
		this.newsfeedPagination.chosenPage = 0;
		this.newsfeedPagination.totalElements= model.news.Count;
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
		this.newsfeedPagination.totalElements= model.friends.Count;
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
		this.nbPacks = model.packs.Count;
		this.displayedPack = 0;
		this.drawPack ();
	}
	public void paginationHandler()
	{
		switch(this.activeTab)
		{
		case 0:
			this.drawNotifications(false);
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
		StartCoroutine(this.drawDeckCards ());
	}
	public void initializeScene()
	{
		this.deckBlock = Instantiate (this.blockObject) as GameObject;
		this.deckBlockTitle = GameObject.Find ("DeckBlockTitle");
		this.deckBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.deckBlockTitle.GetComponent<TextMeshPro> ().text = "Mon équipe";
		this.deckSelectionButton = GameObject.Find ("DeckSelectionButton");
		this.deckSelectionButton.AddComponent<NewHomePageDeckSelectionButtonController> ();
		this.deckTitle = GameObject.Find ("DeckTitle");
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
		}
		this.playBlock = Instantiate (this.blockObject) as GameObject;
		this.playBlockTitle = GameObject.Find ("PlayBlockTitle");
		this.playBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.playBlockTitle.GetComponent<TextMeshPro> ().text = "Jouer";
		this.friendlyGameButton = GameObject.Find ("FriendlyGameButton");
		this.friendlyGameButton.AddComponent<NewHomePageCompetitionController> ();
		this.friendlyGameButton.GetComponent<NewHomePageCompetitionController> ().setId (0);
		this.friendlyGamePicture = GameObject.Find ("FriendlyGamePicture");
		this.friendlyGamePicture.GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
		this.friendlyGameTitle = GameObject.Find ("FriendlyGameTitle");
		this.friendlyGameTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.divisionGameButton = GameObject.Find ("DivisionGameButton");
		this.divisionGameButton.AddComponent<NewHomePageCompetitionController> ();
		this.divisionGameButton.GetComponent<NewHomePageCompetitionController> ().setId (1);
		this.divisionGamePicture = GameObject.Find ("DivisionGamePicture");
		this.divisionGamePicture.GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
		this.divisionGameTitle = GameObject.Find ("DivisionGameTitle");
		this.divisionGameTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.cupGameButton = GameObject.Find ("CupGameButton");
		this.cupGameButton.AddComponent<NewHomePageCompetitionController> ();
		this.cupGameButton.GetComponent<NewHomePageCompetitionController> ().setId (2);
		this.cupGamePicture = GameObject.Find ("CupGamePicture");
		this.cupGamePicture.GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
		this.cupGameTitle = GameObject.Find ("CupGameTitle");
		this.cupGameTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.packButton = GameObject.Find ("PackButton");
		this.packButton.AddComponent<NewHomePageBuyPackButtonController> ();
		this.packTitle = GameObject.Find ("PackTitle");
		this.packTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.packPicture = GameObject.Find ("PackPicture");
		this.packPicture.GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
		this.storeBlock = Instantiate (this.blockObject) as GameObject;
		this.storeBlockTitle = GameObject.Find ("StoreBlockTitle");
		this.storeBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.storeBlockTitle.GetComponent<TextMeshPro> ().text = "Acheter";
		this.newsfeedBlock = Instantiate (this.blockObject) as GameObject;
		this.tabs=new GameObject[3];
		for(int i=0;i<this.tabs.Length;i++)
		{
			this.tabs[i]=GameObject.Find ("Tab"+i);
			this.tabs[i].AddComponent<NewHomePageTabController>();
			this.tabs[i].GetComponent<NewHomePageTabController>().setId(i);
		}
		this.tabs[0].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Alertes");
		this.tabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("News");
		this.tabs[2].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Amis");
		this.contents = new GameObject[3];
		this.friendsStatusButtons=new GameObject[this.contents.Length*2];
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i]=GameObject.Find("Content"+i);
			this.contents[i].transform.FindChild("new").GetComponent<TextMeshPro>().text="Nouveau !";
			this.contents[i].transform.FindChild("new").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.contents[i].transform.FindChild("username").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.contents[i].transform.FindChild("username").gameObject.AddComponent<NewHomePageContentUsernameController>();
			this.contents[i].transform.FindChild("username").GetComponent<NewHomePageContentUsernameController>().setId(i);
			this.contents[i].transform.FindChild("picture").gameObject.AddComponent<NewHomePageContentPictureController>();
			this.contents[i].transform.FindChild("picture").GetComponent<NewHomePageContentPictureController>().setId(i);
			this.contents[i].transform.FindChild("date").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.contents[i].transform.FindChild("line").GetComponent<SpriteRenderer>().color = ApplicationDesignRules.whiteSpriteColor;
			this.friendsStatusButtons[2*i]=GameObject.Find ("FriendsStatusButton"+(2*i));
			this.friendsStatusButtons[2*i+1]=GameObject.Find ("FriendsStatusButton"+(2*i+1));
			this.friendsStatusButtons[2*i].AddComponent<NewHomePageFriendsStatusAcceptButtonController>();
			this.friendsStatusButtons[2*i].GetComponent<NewHomePageFriendsStatusAcceptButtonController>().setId(i);
			this.friendsStatusButtons[2*i+1].AddComponent<NewHomePageFriendsStatusDeclineButtonController>();
			this.friendsStatusButtons[2*i+1].GetComponent<NewHomePageFriendsStatusDeclineButtonController>().setId(i);
			this.friendsStatusButtons[2*i].SetActive(false);
			this.friendsStatusButtons[2*i+1].SetActive(false);
			this.friendsStatusButtons[2*i].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Oui";
			this.friendsStatusButtons[2*i+1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Non";
		}
		this.challengeButtons = new GameObject[3];
		for(int i=0;i<this.challengeButtons.Length;i++)
		{
			this.challengeButtons[i]=GameObject.Find("ChallengeButton"+i);
			this.challengeButtons[i].AddComponent<NewHomePageChallengeButtonController>();
			this.challengeButtons[i].GetComponent<NewHomePageChallengeButtonController>().setId(i);
			this.challengeButtons[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text="Défier";
		}
		this.newsfeedPaginationButtons = GameObject.Find("Pagination");
		this.newsfeedPaginationButtons.AddComponent<NewHomePagePaginationController> ();
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().initialize ();

		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardHomePageController> ();
		this.focusedCard.SetActive (false);

		this.endGamePopUp = GameObject.Find ("EndGamePopUp");
		this.endGamePopUp.SetActive (false);

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

		playBlockHeight=ApplicationDesignRules.smallBlockHeight;
		deckBlockHeight=ApplicationDesignRules.mediumBlockHeight;
		storeBlockHeight=ApplicationDesignRules.smallBlockHeight;
		newsfeedBlockHeight=ApplicationDesignRules.mediumBlockHeight-ApplicationDesignRules.button62WorldSize.y;

		if(ApplicationDesignRules.isMobileScreen)
		{
			playBlockLeftMargin=ApplicationDesignRules.leftMargin;
			playBlockUpMargin=0f;

			deckBlockLeftMargin=ApplicationDesignRules.leftMargin;
			deckBlockUpMargin=playBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+playBlockHeight;

			storeBlockLeftMargin=ApplicationDesignRules.leftMargin;
			storeBlockUpMargin=deckBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+deckBlockHeight;

			newsfeedBlockLeftMargin=ApplicationDesignRules.leftMargin;
			newsfeedBlockUpMargin=storeBlockUpMargin+storeBlockHeight+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.button62WorldSize.y;
		}
		else
		{
			deckBlockLeftMargin=ApplicationDesignRules.leftMargin;
			deckBlockUpMargin=ApplicationDesignRules.upMargin;

			playBlockLeftMargin=ApplicationDesignRules.leftMargin;
			playBlockUpMargin=deckBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+deckBlockHeight;
			
			newsfeedBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			newsfeedBlockUpMargin=deckBlockUpMargin+ApplicationDesignRules.button62WorldSize.y;
			
			storeBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			storeBlockUpMargin=newsfeedBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+newsfeedBlockHeight;
		}

		this.mainCamera.GetComponent<ScrollingController> ().setViewHeight(ApplicationDesignRules.viewHeight);
		this.mainCamera.GetComponent<ScrollingController> ().setContentHeight(playBlockHeight + deckBlockHeight + storeBlockHeight + newsfeedBlockHeight + 3f * ApplicationDesignRules.gapBetweenBlocks + ApplicationDesignRules.button62WorldSize.y);
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraStartPosition;
		this.mainCamera.GetComponent<ScrollingController> ().setStartPositionY (ApplicationDesignRules.mainCameraStartPosition.y);

		this.centralWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.25f * ApplicationDesignRules.heightScreen);
		
		this.playBlock.GetComponent<NewBlockController> ().resize(playBlockLeftMargin,playBlockUpMargin,ApplicationDesignRules.blockWidth,playBlockHeight);
		Vector3 playBlockUpperLeftPosition = this.playBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 playBlockUpperRightPosition = this.playBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 playBlockSize = this.playBlock.GetComponent<NewBlockController> ().getSize ();
		this.playBlockTitle.transform.position = new Vector3 (playBlockUpperLeftPosition.x + 0.3f, playBlockUpperLeftPosition.y - 0.2f, 0f);
		
		float gapBetweenCompetitionsBlock = 0.05f;
		float competitionsBlockSize = (playBlockSize.x - 0.6f - 2f * gapBetweenCompetitionsBlock) / 3f;

		this.friendlyGameTitle.transform.localScale=ApplicationDesignRules.subMainTitleScale;
		this.divisionGameTitle.transform.localScale=ApplicationDesignRules.subMainTitleScale;
		this.cupGameTitle.transform.localScale=ApplicationDesignRules.subMainTitleScale;

		this.playBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.friendlyGameTitle.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f, playBlockUpperLeftPosition.y - 1.2f, 0f);
		this.divisionGameTitle.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 1f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.2f, 0f);
		this.cupGameTitle.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 2f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.2f, 0f);
		
		this.friendlyGamePicture.transform.position = new Vector3 (0.3f + playBlockUpperLeftPosition.x + competitionsBlockSize / 2f, playBlockUpperLeftPosition.y - 1.95f, 0f);
		this.divisionGamePicture.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 1f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.95f, 0f);
		this.cupGamePicture.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 2f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.95f, 0f);
		
		this.friendlyGameButton.transform.position = new Vector3 (0.3f + playBlockUpperLeftPosition.x + competitionsBlockSize / 2f, playBlockUpperLeftPosition.y - 2.9f, 0f);
		this.divisionGameButton.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 1f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 2.9f, 0f);
		this.cupGameButton.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 2f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 2.9f, 0f);
		
		this.friendlyGameButton.transform.localScale = ApplicationDesignRules.button62Scale;
		this.divisionGameButton.transform.localScale = ApplicationDesignRules.button62Scale;
		this.cupGameButton.transform.localScale = ApplicationDesignRules.button62Scale;
		
		this.deckBlock.GetComponent<NewBlockController> ().resize(deckBlockLeftMargin,deckBlockUpMargin,ApplicationDesignRules.blockWidth,deckBlockHeight);
		Vector3 deckBlockUpperLeftPosition = this.deckBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 deckBlockUpperRightPosition = this.deckBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 deckBlockSize = this.deckBlock.GetComponent<NewBlockController> ().getSize ();
		this.deckBlockTitle.transform.position = new Vector3 (deckBlockUpperLeftPosition.x + 0.3f, deckBlockUpperLeftPosition.y - 0.2f, 0f);
		this.deckBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.deckSelectionButton.transform.position = new Vector3 (deckBlockUpperRightPosition.x - 0.3f - ApplicationDesignRules.button62WorldSize.x / 2f, deckBlockUpperRightPosition.y - 1.2f, 0f);
		this.deckSelectionButton.transform.localScale = ApplicationDesignRules.button62Scale;

		for(int i=0;i<this.deckChoices.Length;i++)
		{
			this.deckChoices[i].transform.localScale=ApplicationDesignRules.listElementScale;
			this.deckChoices[i].transform.position=new Vector3(this.deckSelectionButton.transform.position.x,this.deckSelectionButton.transform.position.y-ApplicationDesignRules.button62WorldSize.y/2f-(i+0.5f)*ApplicationDesignRules.listElementWorldSize.y+i*0.02f,-1f);
		}

		this.deckTitle.transform.position = new Vector3 (deckBlockUpperLeftPosition.x + 0.3f, deckSelectionButton.transform.position.y, 0f);
		this.deckTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		float gapBetweenCardsHalo = (deckBlockSize.x - 0.6f - 4f * ApplicationDesignRules.cardHaloWorldSize.x) / 3f;

		this.deckCardsPosition=new Vector3[this.cardsHalos.Length];
		this.deckCardsArea=new Rect[this.cardsHalos.Length];
		
		for(int i=0;i<this.cardsHalos.Length;i++)
		{
			this.cardsHalos[i].transform.localScale=ApplicationDesignRules.cardHaloScale;
			this.cardsHalos[i].transform.position=new Vector3(deckBlockUpperLeftPosition.x+0.3f+ApplicationDesignRules.cardHaloWorldSize.x/2f+i*(gapBetweenCardsHalo+ApplicationDesignRules.cardHaloWorldSize.x),deckBlockUpperRightPosition.y - 3f,0);
			this.deckCardsPosition[i]=this.cardsHalos[i].transform.position;
			this.deckCardsArea[i]=new Rect(this.cardsHalos[i].transform.position.x-ApplicationDesignRules.cardHaloWorldSize.x/2f,this.cardsHalos[i].transform.position.y-ApplicationDesignRules.cardHaloWorldSize.y/2f,ApplicationDesignRules.cardHaloWorldSize.x,ApplicationDesignRules.cardHaloWorldSize.y);
			this.deckCards[i].transform.position=this.deckCardsPosition[i];
			this.deckCards[i].transform.localScale=ApplicationDesignRules.cardScale;
			this.deckCards[i].transform.GetComponent<NewCardHomePageController>().setId(i);
		}
		
		this.storeBlock.GetComponent<NewBlockController> ().resize(storeBlockLeftMargin,storeBlockUpMargin,ApplicationDesignRules.blockWidth,storeBlockHeight);
		Vector3 storeBlockUpperLeftPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 storeBlockUpperRightPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 storeBlockLowerRightPosition = this.storeBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 storeBlockSize = this.storeBlock.GetComponent<NewBlockController> ().getSize ();
		this.storeBlockTitle.transform.position = new Vector3 (storeBlockUpperLeftPosition.x + 0.3f, storeBlockUpperLeftPosition.y - 0.2f, 0f);
		this.storeBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		float packPictureWidth = 375f;
		float packPictureHeight = 200f;
		float packPictureScale = 1.3f * ApplicationDesignRules.reductionRatio;
		float packPictureWorldWidth = packPictureScale * (packPictureWidth / ApplicationDesignRules.pixelPerUnit);
		float packPictureWorldHeight = packPictureWorldWidth * (packPictureHeight / packPictureWidth);

		this.packPicture.transform.localScale = new Vector3 (packPictureScale,packPictureScale,packPictureScale);
		this.packPicture.transform.position = new Vector3 (storeBlockLowerRightPosition.x - packPictureWorldWidth / 2f, storeBlockLowerRightPosition.y + packPictureWorldHeight/2f+0.25f, 0f);

		this.packButton.transform.localScale = ApplicationDesignRules.button62Scale;
		this.packButton.transform.position = new Vector3 (0.3f + storeBlockUpperLeftPosition.x+ApplicationDesignRules.button62WorldSize.x/2f, storeBlockUpperLeftPosition.y - 2.9f, 0f);
		this.packTitle.transform.position = new Vector3 (storeBlockUpperLeftPosition.x + 0.3f, storeBlockUpperRightPosition.y - 1.2f, 0f);
		this.packTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.packTitle.transform.GetComponent<TextMeshPro> ().textContainer.width = storeBlockSize.x / 2f;

		this.newsfeedBlock.GetComponent<NewBlockController> ().resize(newsfeedBlockLeftMargin,newsfeedBlockUpMargin,ApplicationDesignRules.blockWidth,newsfeedBlockHeight);
		Vector3 newsfeedBlockUpperLeftPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 newsfeedBlockUpperRightPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 newsfeedBlockLowerLeftPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 newsfeedBlockLowerRightPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 newsfeedBlockSize = this.newsfeedBlock.GetComponent<NewBlockController> ().getSize ();

		float gapBetweenSelectionsButtons = 0.02f;
		for(int i=0;i<this.tabs.Length;i++)
		{
			this.tabs[i].transform.localScale = ApplicationDesignRules.button62Scale;
			this.tabs[i].transform.position = new Vector3 (newsfeedBlockUpperLeftPosition.x + ApplicationDesignRules.button62WorldSize.x / 2f+ i*(ApplicationDesignRules.button62WorldSize.x+gapBetweenSelectionsButtons), newsfeedBlockUpperLeftPosition.y+ApplicationDesignRules.button62WorldSize.y/2f,0f);
		}

		Vector2 contentBlockSize = new Vector2 (newsfeedBlockSize.x - 0.6f, (newsfeedBlockSize.y - 0.3f - 0.6f)/this.contents.Length);
		float lineScale = ApplicationDesignRules.getLineScale (contentBlockSize.x);

		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i].transform.position=new Vector3(newsfeedBlockUpperLeftPosition.x+0.3f+contentBlockSize.x/2f,newsfeedBlockUpperLeftPosition.y-0.3f-(i+1)*contentBlockSize.y,0f);
			this.contents[i].transform.FindChild("line").localScale=new Vector3(lineScale,1f,1f);
			this.contents[i].transform.FindChild("picture").localScale=ApplicationDesignRules.thumbScale;
			this.contents[i].transform.FindChild("picture").localPosition=new Vector3(-contentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x/2f,(contentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f+ApplicationDesignRules.thumbWorldSize.y/2f,0f);
			this.contents[i].transform.FindChild("username").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("username").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/2f)-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.contents[i].transform.FindChild("username").localPosition=new Vector3(-contentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,contentBlockSize.y-(contentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.contents[i].transform.FindChild("description").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=0.75f*contentBlockSize.x-0.1f-ApplicationDesignRules.thumbWorldSize.x;
			this.contents[i].transform.FindChild("description").localPosition=new Vector3(-contentBlockSize.x/2f+ApplicationDesignRules.thumbWorldSize.x+0.1f,contentBlockSize.y/2f,0f);
			this.contents[i].transform.FindChild("date").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("date").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
			this.contents[i].transform.FindChild("date").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y-(contentBlockSize.y-ApplicationDesignRules.thumbWorldSize.y)/2f,0f);
			this.contents[i].transform.FindChild("new").localScale=new Vector3(ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio,ApplicationDesignRules.reductionRatio);
			this.contents[i].transform.FindChild("new").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
			this.contents[i].transform.FindChild("new").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y/2f,0f);
		}

		for(int i=0;i<this.challengeButtons.Length;i++)
		{
			this.challengeButtons[i].transform.localScale = ApplicationDesignRules.button62Scale;
			this.challengeButtons[i].transform.position=new Vector3(newsfeedBlockUpperRightPosition.x-0.3f-ApplicationDesignRules.button62WorldSize.x/2f,newsfeedBlockUpperRightPosition.y-0.3f-(i+0.5f)*contentBlockSize.y,0f);
		}

		for(int i=0;i<this.contents.Length;i++)
		{
			this.friendsStatusButtons[i*2].transform.localScale = ApplicationDesignRules.button31Scale;
			this.friendsStatusButtons[i*2+1].transform.localScale = ApplicationDesignRules.button31Scale;
			this.friendsStatusButtons[i*2].transform.position=new Vector3(newsfeedBlockUpperRightPosition.x-0.3f-3*ApplicationDesignRules.button31WorldSize.x/2f-0.05f,newsfeedBlockUpperRightPosition.y-0.3f-(i+1f)*contentBlockSize.y+0.05f+ApplicationDesignRules.button31WorldSize.y/2f,0f);
			this.friendsStatusButtons[i*2+1].transform.position=new Vector3(newsfeedBlockUpperRightPosition.x-0.3f-ApplicationDesignRules.button31WorldSize.x/2f,newsfeedBlockUpperRightPosition.y-0.3f-(i+1f)*contentBlockSize.y+0.05f+ApplicationDesignRules.button31WorldSize.y/2f,0f);
		}

		this.newsfeedPaginationButtons.transform.localPosition=new Vector3 (newsfeedBlockLowerLeftPosition.x + newsfeedBlockSize.x / 2, newsfeedBlockLowerLeftPosition.y + 0.3f, 0f);
		this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController> ().resize ();

		this.focusedCard.transform.localScale = ApplicationDesignRules.cardFocusedScale;
		this.focusedCard.transform.position = new Vector3 (0f, -ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.downMargin+ApplicationDesignRules.cardFocusedWorldSize.y/2f-0.22f, 0f);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setCentralWindow (this.centralWindow);

		TutorialObjectController.instance.resize();

		this.endGamePopUp.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x+0, ApplicationDesignRules.menuPosition.y+2f, -3f);
	}
	private void retrieveDefaultDeck()
	{
		if(model.decks.Count>0)
		{
			this.deckDisplayed = 0;
			for(int i=0;i<model.decks.Count;i++)
			{
				if(model.decks[i].Id==model.player.SelectedDeckId)
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
			for(int i=0;i<model.decks.Count;i++)
			{
				if(i!=this.deckDisplayed)
				{
					this.decksDisplayed.Add (i);
				}
			}
		}
	}
	public IEnumerator drawDeckCards()
	{
		MenuController.instance.displayLoadingScreen ();
		this.deckCardsDisplayed = new int[]{-1,-1,-1,-1};
		if(this.deckDisplayed!=-1)
		{	
			yield return StartCoroutine(model.decks[this.deckDisplayed].RetrieveCards());
			
			for(int i=0;i<model.decks[this.deckDisplayed].cards.Count;i++)
			{
				int deckOrder = model.decks[this.deckDisplayed].cards[i].deckOrder;
				this.deckCardsDisplayed[deckOrder]=i;
			}
			this.deckTitle.GetComponent<TextMeshPro> ().text = model.decks[this.deckDisplayed].Name.ToUpper();
			if(model.decks.Count>1)
			{
				this.deckSelectionButton.SetActive(true);
				this.deckSelectionButton.transform.FindChild("Title").GetComponent<TextMeshPro>().text=("Changer").ToUpper();
			}
			else
			{
				this.deckSelectionButton.SetActive(false);
			}
		}
		else
		{
			this.deckTitle.GetComponent<TextMeshPro> ().text = ("Aucune équipe formée").ToUpper();
			this.deckSelectionButton.transform.FindChild("Title").GetComponent<TextMeshPro>().text=("Créer").ToUpper();
		}
		for(int i=0;i<this.deckCards.Length;i++)
		{
			if(this.deckCardsDisplayed[i]!=-1)
			{
				this.deckCards[i].transform.GetComponent<NewCardController>().c=model.decks[this.deckDisplayed].cards[this.deckCardsDisplayed[i]];
				this.deckCards[i].transform.GetComponent<NewCardController>().show();
				this.deckCards[i].SetActive(true);
			}
			else
			{
				this.deckCards[i].SetActive(false);
			}
		}
		MenuController.instance.hideLoadingScreen ();
	}
	public void showCardFocused()
	{
		this.isCardFocusedDisplayed = true;
		this.isHovering=false;
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		this.displayBackUI (false);
		this.focusedCard.SetActive (true);
		this.focusedCardIndex=this.deckCardsDisplayed[this.idCardClicked];
		this.focusedCard.GetComponent<NewFocusedCardController>().c=model.decks[this.deckDisplayed].cards[this.deckCardsDisplayed[this.idCardClicked]];
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
		this.deckBlock.SetActive (value);
		this.deckBlockTitle.SetActive(value);
		this.deckTitle.SetActive(value);
		this.playBlock.SetActive(value);
		this.playBlockTitle.SetActive(value);
		this.friendlyGameButton.SetActive(value);
		this.friendlyGamePicture.SetActive(value);
		this.friendlyGameTitle.SetActive(value);
		this.divisionGameButton.SetActive(value);
		this.divisionGamePicture.SetActive(value);
		this.divisionGameTitle.SetActive(value);
		this.cupGameButton.SetActive(value);
		this.cupGamePicture.SetActive(value);
		this.cupGameTitle.SetActive(value);
		this.packButton.SetActive(value);
		this.packTitle.SetActive(value);
		this.packPicture.SetActive(value);
		this.storeBlock.SetActive(value);
		this.storeBlockTitle.SetActive(value);
		this.newsfeedBlock.SetActive(value);
		for(int i=0;i<this.tabs.Length;i++)
		{
			this.tabs[i].SetActive(value);
		}
		if(value)
		{
			this.selectATab();
		}
		else
		{
			for(int i=0;i<this.contents.Length;i++)
			{
				this.contents[i].SetActive(value);
			}
		}
		for(int i=0;i<this.cardsHalos.Length;i++)
		{
			this.cardsHalos[i].SetActive(value);
		}
		for(int i=0;i<this.deckCardsDisplayed.Length;i++)
		{
			if(this.deckCardsDisplayed[i]!=-1)
			{
				this.deckCards[i].SetActive(value);
			}
		}
		if(isSearchingDeck&&value)
		{
			for(int i=0;i<this.deckChoices.Length;i++)
			{
				if(i<this.decksDisplayed.Count)
				{
					this.deckChoices[i].SetActive(value);
				}
				else
				{
					this.deckChoices[i].SetActive(false);
				}
			}
		}
		else
		{
			for(int i=0;i<this.deckChoices.Length;i++)
			{
				this.deckChoices[i].SetActive(false);
			}
		}
		if(model.decks.Count>1 && value)
		{
			this.deckSelectionButton.SetActive(true);
		}
		else
		{
			this.deckSelectionButton.SetActive(false);
		}
		if(value)
		{
			this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController>().setPagination();
		}
		else
		{
			this.newsfeedPaginationButtons.GetComponent<NewHomePagePaginationController>().setVisible(false);
		}
	}
	public void selectDeck(int id)
	{
		this.deckDisplayed = this.decksDisplayed [id];
		this.isSearchingDeck = false;
		this.cleanDeckList ();
		this.initializeDecks ();
		StartCoroutine(model.player.SetSelectedDeck(model.decks[this.deckDisplayed].Id));
	}
	public void deckSelectionButtonHandler()
	{
		if(this.deckDisplayed!=-1)
		{
			this.displayDeckList();
		}
		else
		{
			Application.LoadLevel ("newMyGame");
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
				this.deckChoices[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=model.decks[this.decksDisplayed[i]].Name;
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
	public void refreshCredits()
	{
		StartCoroutine(this.menu.GetComponent<MenuController> ().getUserData ());
	}
	public void returnPressed()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().returnPressed();
		}
		else if(isEndGamePopUpDisplayed)
		{
			this.hideEndGamePopUp();
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
			this.hideEndGamePopUp();
		}
	}
	public void closeAllPopUp()
	{
		if(isEndGamePopUpDisplayed)
		{
			this.hideEndGamePopUp();
		}
	}
	public void leftClickedHandler(int id)
	{
		this.idCardClicked = id;
		bool onSale;
		int idOwner;
		this.isLeftClicked = true;
		this.clickInterval = 0f;
	}
	public void leftClickReleaseHandler()
	{
		this.isLeftClicked = false;
		if(isDragging)
		{
			this.endDragging();
		}
		else
		{
			this.showCardFocused ();
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
			this.isDragging=true;
			Cursor.SetCursor (this.cursorTextures[1], new Vector2(this.cursorTextures[1].width/2f,this.cursorTextures[1].width/2f), CursorMode.Auto);
			this.deckCards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(10,"Foreground");
			for(int i=0;i<this.cardsHalos.Length;i++)
			{
				this.cardsHalos[i].GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
			}
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
		
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		for(int i=0;i<deckCardsArea.Length;i++)
		{
			if(this.deckCardsArea[i].Contains(Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z))))
			{
				this.moveToDeckCards(i);
				break;
			}
		}
	}
	public void isDraggingCard()
	{
		if(isDragging)
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			this.deckCards[this.idCardClicked].transform.position=new Vector3(mousePosition.x,mousePosition.y,0f);
		}
	}
	public void moveToDeckCards(int position)
	{
		
		int idCard1 = model.decks [this.deckDisplayed].cards [this.deckCardsDisplayed [this.idCardClicked]].Id;
		this.deckCards[position].SetActive(true);
		this.deckCards[position].GetComponent<NewCardController>().c=model.decks [this.deckDisplayed].cards [this.deckCardsDisplayed [this.idCardClicked]];
		this.deckCards[position].GetComponent<NewCardController>().show();
		if(this.deckCardsDisplayed[position]!=-1)
		{
			int indexCard2=this.deckCardsDisplayed[position];
			int idCard2=model.decks [this.deckDisplayed].cards [indexCard2].Id;
			this.deckCards[position].GetComponent<NewCardController>().c=model.decks [this.deckDisplayed].cards [this.deckCardsDisplayed [this.idCardClicked]];
			this.deckCards[position].GetComponent<NewCardController>().show ();
			this.deckCardsDisplayed[position]=this.deckCardsDisplayed[this.idCardClicked];
			this.deckCards[this.idCardClicked].GetComponent<NewCardController>().c=model.decks [this.deckDisplayed].cards [indexCard2];
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
		yield return StartCoroutine(model.decks[this.deckDisplayed].changeCardsOrder(idCard1,deckOrder1,idCard2,deckOrder2));
	}
	public void drawNotifications(bool firstLoad=false)
	{
		this.notificationsDisplayed = new List<int> ();
		for(int i =0;i<this.newsfeedPagination.nbElementsPerPage;i++)
		{
			if(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i<model.notifications.Count)
			{
				this.notificationsDisplayed.Add (this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i);
				this.contents[i].SetActive(true);
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=model.notifications[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i].Content;
				this.contents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.notifications[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i].SendingUser.idProfilePicture);
				this.contents[i].transform.FindChild("date").GetComponent<TextMeshPro>().text=model.notifications[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i].Notification.Date.ToString("dd/MM/yyyy");
				this.contents[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.notifications[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i].SendingUser.Username;
				if(!model.notifications[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i].Notification.IsRead)
				{
					this.contents[i].transform.FindChild("new").gameObject.SetActive(true);
				}
				else
				{
					this.contents[i].transform.FindChild("new").gameObject.SetActive(false);
				}
				if(model.notifications[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i].Notification.IdNotificationType==4)
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
		this.manageNonReadsNotifications (firstLoad);
	}
	public void drawNews()
	{
		this.newsDisplayed = new List<int> ();
		for(int i =0;i<this.newsfeedPagination.nbElementsPerPage;i++)
		{
			if(this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i<model.news.Count)
			{
				this.newsDisplayed.Add (this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i);
				this.contents[i].SetActive(true);
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=model.news[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i].Content;
				this.contents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.news[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i].User.idProfilePicture);
				this.contents[i].transform.FindChild("date").GetComponent<TextMeshPro>().text=model.news[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i].News.Date.ToString("dd/MM/yyyy");
				this.contents[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.news[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i].User.Username;

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
				switch(model.users[this.friendsToBeDisplayed[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i]].OnlineStatus)
				{
				case 0:
					connectionState = "n'est pas en ligne";
					connectionStateColor=ApplicationDesignRules.whiteTextColor;
					break;
				case 1:
					connectionState = "est disponible pour un défi !";
					connectionStateColor=ApplicationDesignRules.blueColor;
					this.challengeButtons[i].SetActive(true);
					break;
				case 2:
					connectionState = "est entrain de jouer";
					connectionStateColor=ApplicationDesignRules.redColor;
					break;
				}
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=connectionState;
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=connectionStateColor;
				this.contents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.users[this.friendsToBeDisplayed[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i]].idProfilePicture);
				this.contents[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.users[this.friendsToBeDisplayed[this.newsfeedPagination.chosenPage*this.newsfeedPagination.nbElementsPerPage+i]].Username;
			}
			else
			{
				this.contents[i].SetActive(false);
				this.challengeButtons[i].SetActive(false);
			}
		}
	}
	public void drawPack()
	{
		this.packButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = model.packs [this.displayedPack].Price.ToString ();
		this.packPicture.transform.GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnPackPicture (model.packs [this.displayedPack].IdPicture);
		this.packTitle.GetComponent<TextMeshPro> ().text = model.packs [this.displayedPack].Name;
	}
	public void drawCompetitions()
	{	
		this.friendlyGameTitle.GetComponent<TextMeshPro> ().text = "Entrainement".ToUpper ();
		this.divisionGameTitle.GetComponent<TextMeshPro> ().text = model.currentDivision.Name.ToUpper ();
		this.cupGameTitle.GetComponent<TextMeshPro> ().text = model.currentCup.Name.ToUpper ();

		this.friendlyGameButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Jouer".ToUpper ();

		string divisionState;
		if(model.player.NbGamesDivision>0)
		{
			divisionState="Rejoindre";
		}
		else
		{
			divisionState="Démarrer";
		}
		this.divisionGameButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = divisionState.ToUpper ();

		string cupState;
		if(model.player.NbGamesCup>0)
		{
			divisionState="Rejoindre";
		}
		else
		{
			divisionState="Démarrer";
		}
		this.cupGameButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = divisionState.ToUpper ();

	}
	public void buyPackHandler()
	{
		ApplicationModel.packToBuy = model.packs [this.displayedPack].Id;
		PhotonNetwork.Disconnect();
		Application.LoadLevel ("NewStore");
	}
	private IEnumerator refreshNonReadsNotifications()
	{
		this.notificationsTimer=this.notificationsTimer-this.refreshInterval;
		yield return StartCoroutine(model.player.countNonReadsNotifications(this.totalNbResultLimit));
		menu.GetComponent<MenuController>().setNbNotificationsNonRead(model.player.nonReadNotifications);
	}
	private void manageNonReadsNotifications(bool firstload)
	{
		this.computeNonReadsNotifications ();
		StartCoroutine(this.updateReadNotifications (firstload));
	}
	private void computeNonReadsNotifications()
	{
		this.nbNonReadNotifications = 0;
		for(int i=0;i<model.notifications.Count;i++)
		{
			if(i==model.notificationSystemIndex)
			{
				this.nbNonReadNotifications++;
			}
			else if(!model.notifications[i].Notification.IsRead)
			{
				this.nbNonReadNotifications++;
			}
		}
	}
	private IEnumerator updateReadNotifications(bool firstLoad)
	{
		IList<int> tempList = new List<int> ();
		for (int i=0;i<this.notificationsDisplayed.Count;i++)
		{
			if(i==model.notificationSystemIndex)
			{
				tempList.Add (this.notificationsDisplayed[i]);
				model.notificationSystemIndex=-1;
			}
			else if(!model.notifications[this.notificationsDisplayed[i]].Notification.IsRead)
			{
				tempList.Add (this.notificationsDisplayed[i]);
			}
		}
		if(firstLoad)
		{
			menu.GetComponent<MenuController>().setNbNotificationsNonRead(nbNonReadNotifications-tempList.Count);
		}
		if(tempList.Count>0)
		{
			yield return StartCoroutine(model.updateReadNotifications (tempList,this.totalNbResultLimit));
			menu.GetComponent<MenuController>().setNbNotificationsNonRead(model.player.nonReadNotifications);
		}
		yield break;
	}
	public int getNbGamesCup()
	{
		return model.player.NbGamesCup;
	}
	public int getNbGamesDivision()
	{
		return model.player.NbGamesCup;
	}
	public void displayConnectionBonusPopUp(int connectionBonus)
	{
		this.isConnectionBonusViewDisplayed=true;
		this.connectionBonusView= Camera.main.gameObject.AddComponent <NewHomePageConnectionBonusPopUpView>();
		connectionBonusView.connectionBonusPopUpVM.bonus = connectionBonus.ToString();
		connectionBonusView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
		connectionBonusView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		connectionBonusView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		connectionBonusView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		this.connectionBonusPopUpResize ();
	}
	public void hideConnectionBonusPopUp()
	{
		this.isConnectionBonusViewDisplayed = false;
		Destroy (this.connectionBonusView);
	}
	public void connectionBonusPopUpResize()
	{
		connectionBonusView.popUpVM.centralWindow = this.centralWindow;
		connectionBonusView.popUpVM.resize ();
	}
	public void joinGameHandler(int id)
	{
		if(this.deckDisplayed==-1)
		{
			MenuController.instance.displayErrorPopUp("Vous ne pouvez lancer de match sans avoir au préalable créé un deck");
		}
		else
		{
			ApplicationModel.gameType = id;
			StartCoroutine (this.joinGame ());
		}
	}
	public IEnumerator joinGame()
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.player.SetSelectedDeck (model.decks [this.deckDisplayed].Id));
		if(ApplicationModel.gameType==0)
		{

			MenuController.instance.joinRandomRoomHandler();
		}
		else
		{
			Application.LoadLevel("NewLobby");
		}
	}
	private void launchEndGameSequence(bool hasWon)
	{
		if(hasWon)
		{
			this.endGamePopUp.transform.FindChild("Title").GetComponent<TextMeshPro>().text="BRAVO !";
			this.endGamePopUp.transform.FindChild("Content").GetComponent<TextMeshPro>().text="Venez en match officiel vous mesurer aux meilleurs joueurs !";
		}
		else
		{
			this.endGamePopUp.transform.FindChild("Title").GetComponent<TextMeshPro>().text="DOMMAGE !";
			this.endGamePopUp.transform.FindChild("Content").GetComponent<TextMeshPro>().text="C'est en s'entrainant qu'on progresse ! Courage !";
		}
		this.displayEndGamePopUp ();
	}
	private void displayEndGamePopUp()
	{
		this.isEndGamePopUpDisplayed = true;
		this.endGamePopUp.SetActive (true);
		MenuController.instance.displayTransparentBackground ();
	}
	public void hideEndGamePopUp()
	{
		this.isEndGamePopUpDisplayed = false;
		this.endGamePopUp.SetActive (false);
		MenuController.instance.hideTransparentBackground ();
	}
	public Vector3 getEndGamePopUpButtonPosition()
	{
		return this.endGamePopUp.transform.FindChild ("Button").position;
	}
	public void checkFriendsOnlineStatus()
	{
		if(PhotonNetwork.insideLobby)
		{
			PhotonNetwork.FindFriends (model.usernameList);
		}
	}
	public void OnUpdatedFriendList()
	{
		for(int i=0;i<PhotonNetwork.Friends.Count;i++)
		{
			for(int j=0;j<model.users.Count;j++)
			{
				if(model.users[j].Username==PhotonNetwork.Friends[i].Name)
				{
					if(PhotonNetwork.Friends[i].IsInRoom)
					{
						if(model.friends.Contains(j))
						{
							if(!this.friendsOnline.Contains(j))
							{
								this.friendsOnline.Insert(0,j);
								model.users[j].OnlineStatus=2;
							}
							else if(model.users[j].OnlineStatus!=2)
							{
								this.friendsOnline.Remove(j);
								this.friendsOnline.Insert(0,j);
								model.users[j].OnlineStatus=2;
							}
						}
					}
					else if(PhotonNetwork.Friends[i].IsOnline)
					{
						if(model.friends.Contains(j))
						{
							if(!this.friendsOnline.Contains(j))
							{
								this.friendsOnline.Insert(0,j);
								model.users[j].OnlineStatus=1;
							}
							else if(model.users[j].OnlineStatus!=1)
							{
								this.friendsOnline.Remove(j);
								this.friendsOnline.Insert(0,j);
								model.users[j].OnlineStatus=1;
							}
						}
					}
					else
					{
						model.users[j].OnlineStatus=0;
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
		for(int i=0;i<model.friends.Count;i++)
		{
			if(!this.friendsToBeDisplayed.Contains(model.friends[i]))
			{
				this.friendsToBeDisplayed.Add(model.friends[i]);
			}
		}
	}
	public void sendInvitationHandler(int challengeButtonId)
	{
		if(this.deckDisplayed==-1)
		{
			MenuController.instance.displayErrorPopUp("Vous ne pouvez lancer de match sans avoir au préalable créé un deck");
		}
		else if(model.users [this.friendsToBeDisplayed[this.friendsDisplayed[challengeButtonId]]].OnlineStatus!=1)
		{
			MenuController.instance.displayErrorPopUp("Votre adversaire n'est plus disponible");
		}
		else
		{
			StartCoroutine (this.sendInvitation (challengeButtonId));
		}
	}
	public IEnumerator sendInvitation(int challengeButtonId)
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.player.SetSelectedDeck (model.decks [this.deckDisplayed].Id));
		StartCoroutine (MenuController.instance.sendInvitation (model.users [this.friendsToBeDisplayed[this.friendsDisplayed[challengeButtonId]]], model.player));
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
		ApplicationModel.profileChosen = this.contents [id].transform.FindChild ("username").GetComponent<TextMeshPro> ().text;
		Application.LoadLevel("NewProfile");
	}
	public void acceptFriendsRequestHandler(int id)
	{
		StartCoroutine (this.confirmFriendRequest (id));
	}
	public void declineFriendsRequestHandler(int id)
	{
		StartCoroutine (this.removeFriendRequest (id));
	}
	public IEnumerator confirmFriendRequest(int id)
	{
		MenuController.instance.displayLoadingScreen ();
		Connection connection = new Connection ();
		connection.Id = System.Convert.ToInt32(model.notifications [this.notificationsDisplayed [id]].Notification.HiddenParam);
		yield return StartCoroutine (connection.confirm ());
		if(connection.Error=="")
		{
			model.moveToFriend(this.notificationsDisplayed[id]);
			this.initializeNotifications();
		}
		else
		{
			MenuController.instance.displayErrorPopUp(connection.Error);
		}
		MenuController.instance.hideLoadingScreen ();
	}
	public IEnumerator removeFriendRequest(int id)
	{
		MenuController.instance.displayLoadingScreen ();
		Connection connection = new Connection ();
		connection.Id = System.Convert.ToInt32(model.notifications [this.notificationsDisplayed [id]].Notification.HiddenParam);
		yield return StartCoroutine(connection.remove ());
		if(connection.Error=="")
		{
			model.notifications.RemoveAt(this.notificationsDisplayed[id]);
			this.initializeNotifications();
		}
		else
		{
			MenuController.instance.displayErrorPopUp(connection.Error);
		}
		MenuController.instance.hideLoadingScreen ();
	}

	#region TUTORIAL FUNCTIONS

	public bool getIsCardFocusedDisplayed()
	{
		return isCardFocusedDisplayed;
	}
	public Vector3 getDeckBlockOrigin()
	{
		return this.deckBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getDeckBlockSize()
	{
		return this.deckBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getNewsfeedBlockOrigin()
	{
		Vector3 blockOrigin = this.newsfeedBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		blockOrigin.y = blockOrigin.y + ApplicationDesignRules.button62WorldSize.y / 2f;
		return blockOrigin;
	}
	public Vector2 getNewsfeedBlockSize()
	{
		Vector2 blockSize=this.newsfeedBlock.GetComponent<NewBlockController> ().getSize ();
		blockSize.y = blockSize.y + ApplicationDesignRules.button62WorldSize.y;
		return blockSize;
	}
	public Vector3 getStoreBlockOrigin()
	{
		return this.storeBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getStoreBlockSize()
	{
		return this.storeBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getPlayBlockOrigin()
	{
		return this.playBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getPlayBlockSize()
	{
		return this.playBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public GameObject returnCardFocused()
	{
		return this.focusedCard;
	}
	#endregion
}
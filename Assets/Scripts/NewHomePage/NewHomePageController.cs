using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class NewHomePageController : Photon.MonoBehaviour
{
	public static NewHomePageController instance;
	private NewHomePageModel model;
	
	public GameObject tutorialObject;
	public GameObject blockObject;
	public GameObject paginationButtonObject;
	public GameObject packObject;
	public GameObject competitionObject;
	public GameObject deckListObject;
	public GameObject popUpObject;
	public GameObject popUpCompetitionObject;
	public GUISkin popUpSkin;
	public Texture2D[] cursorTextures;
	public int refreshInterval;
	public int sliderRefreshInterval;
	public int totalNbResultLimit;

	private GameObject storeBlock;
	private GameObject notificationsBlock;
	private GameObject competitionsBlock;
	private GameObject statsBlock;
	private GameObject newsBlock;
	private GameObject friendsBlock;
	private GameObject popUp;
	
	private GameObject menu;
	private GameObject tutorial;
	private GameObject storeAssetsTitle;
	private GameObject newsTitle;
	private GameObject notificationsTitle;
	private GameObject competitionsTitle;
	private GameObject connectedPlayersTitle;
	private GameObject friendsTitle;
	private GameObject statsTitle;
	private GameObject deckBoard;
	private GameObject deckBlock;
	private GameObject stats;
	private GameObject collectionButton;
	private GameObject cleanCardsButton;
	private GameObject[] news;
	private GameObject[] notifications;
	private GameObject[] friends;
	private GameObject[] packs;
	private GameObject[] competitions;
	private GameObject[] paginationButtonsNotifications;
	private GameObject[] paginationButtonsNews;
	private GameObject[] paginationButtonsFriends;
	private GameObject[] deckCards;

	private GameObject transparentBackground;
	private GameObject endGamePopUp;
	
	private int widthScreen;
	private int heightScreen;
	private float worldWidth;
	private float worldHeight;
	private float pixelPerUnit;
	private Rect centralWindow;
	private Rect collectionPointsWindow;
	private Rect newSkillsWindow;
	private Rect newCardTypeWindow;
	
	private IList<int> newsDisplayed;
	private IList<int> notificationsDisplayed;
	private IList<int> packsDisplayed;
	private IList<int> friendsDisplayed;

	private IList<int> friendsOnline;
	
	private int nbPagesNotifications;
	private int nbPaginationButtonsLimitNotifications;
	private int elementsPerPageNotifications;
	private int chosenPageNotifications;
	private int pageDebutNotifications;
	private int activePaginationButtonIdNotifications;

	private int nbPagesNews;
	private int nbPaginationButtonsLimitNews;
	private int elementsPerPageNews;
	private int chosenPageNews;
	private int pageDebutNews;
	private int activePaginationButtonIdNews;

	private int nbPagesFriends;
	private int nbPaginationButtonsLimitFriends;
	private int elementsPerPageFriends;
	private int chosenPageFriends;
	private int pageDebutFriends;
	private int activePaginationButtonIdFriends;

	private int nbPagesPacks;
	private int chosenPagePacks;

	private float sliderTimer;
	private float notificationsTimer;
	private bool isSceneLoaded;
	
	private int money;

	private int packsPerLine;
	private int competitionsPerLine;

	private Vector3[] deckCardsPosition;
	private Rect[] deckCardsArea;

	private IList<int> decksDisplayed;
	private int[] deckCardsDisplayed;
	private int deckDisplayed;
	
	private GameObject focusedCard;
	private int focusedCardIndex;
	private bool isCardFocusedDisplayed;
	private IList<GameObject> deckList;

	private int idCardClicked;
	private bool isDragging;
	private bool isLeftClicked;
	private bool isHovering;
	private float clickInterval;

	private bool isSearchingDeck;
	private bool isMouseOnSelectDeckButton;

	private bool isHoveringNotification;
	private bool isHoveringNews;
	private bool isHoveringCompetition;
	private bool isHoveringFriend;
	private bool isHoveringPopUp;
	private bool isPopUpDisplayed;
	private int idNotificationHovered;
	private int idNewsHovered;
	private int idCompetitionHovered;
	private int idFriendHovered;
	private bool toDestroyPopUp;
	private float popUpDestroyInterval;

	private int nbNonReadNotifications;

	private bool areNewsPicturesLoading;
	private bool areNotificationsPicturesLoading;
	private bool areFriendsPicturesLoading;
	private bool arePacksPicturesLoading;
	private bool areCompetitionsPicturesLoading;

	private NewHomePageConnectionBonusPopUpView connectionBonusView;
	private bool isConnectionBonusViewDisplayed;

	private bool isTutorialLaunched;
	private bool isEndGamePopUpDisplayed;
	private bool isLoadingScreenDisplayed;
	private bool isInRoom;

	private NewHomePageErrorPopUpView errorView;
	private bool errorViewDisplayed;
	
	void Update()
	{	
		this.sliderTimer += Time.deltaTime;
		this.notificationsTimer += Time.deltaTime;
		
		if (notificationsTimer > refreshInterval && this.isSceneLoaded) 
		{
			StartCoroutine(this.refreshNonReadsNotifications());
			this.checkFriendsOnlineStatus();
		}
		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();
			this.drawPaginationNews();
			this.drawPaginationNotifications();
			this.initializePacks();
		}
		if(this.sliderTimer>this.sliderRefreshInterval)
		{
			this.sliderTimer=0;
			if(this.isSceneLoaded && !this.isCardFocusedDisplayed)
			{
				if(nbPagesPacks-1>this.chosenPagePacks)
				{
					this.chosenPagePacks++;
					this.drawPacks();
				}
				else if(this.chosenPagePacks!=0)
				{
					this.chosenPagePacks=0;
					this.drawPacks();
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
		if(toDestroyPopUp)
		{
			this.popUpDestroyInterval=this.popUpDestroyInterval+Time.deltaTime;
			if(this.popUpDestroyInterval>0.5f)
			{
				this.toDestroyPopUp=false;
				this.hidePopUp();
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
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			this.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape) && !isTutorialLaunched) 
		{
			this.escapePressed();
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
		if(areNewsPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<newsDisplayed.Count;i++)
			{
				if(!model.news[this.newsDisplayed[i]].User.isThumbPictureLoaded)
				{
					allPicturesLoaded=false;
					break;
				}
			}
			if(allPicturesLoaded)
			{
				this.areNewsPicturesLoading=false;
				for(int i=0;i<newsDisplayed.Count;i++)
				{
					this.news[i].GetComponent<NewsController>().setPicture(model.news[this.newsDisplayed[i]].User.texture);
				}
			}
		}
		if(areNotificationsPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<notificationsDisplayed.Count;i++)
			{
				if(!model.notifications[this.notificationsDisplayed[i]].SendingUser.isThumbPictureLoaded)
				{
					allPicturesLoaded=false;
					break;
				}
			}
			if(allPicturesLoaded)
			{
				this.areNotificationsPicturesLoading=false;
				for(int i=0;i<notificationsDisplayed.Count;i++)
				{
					this.notifications[i].GetComponent<NotificationController>().setPicture(model.notifications[this.notificationsDisplayed[i]].SendingUser.texture);
				}
			}
		}
		if(areFriendsPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<friendsDisplayed.Count;i++)
			{
				if(!model.users[this.friendsOnline[this.friendsDisplayed[i]]].isThumbPictureLoaded)
				{
					allPicturesLoaded=false;
					break;
				}
			}
			if(allPicturesLoaded)
			{
				this.areFriendsPicturesLoading=false;
				for(int i=0;i<friendsDisplayed.Count;i++)
				{
					this.friends[i].GetComponent<OnlineFriendController>().setPicture(model.users[this.friendsOnline[this.friendsDisplayed[i]]].texture);
				}
			}
		}
		if(arePacksPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<packsDisplayed.Count;i++)
			{
				if(!model.packs[this.packsDisplayed[i]].isTextureLoaded)
				{
					allPicturesLoaded=false;
					break;
				}
			}
			if(allPicturesLoaded)
			{
				this.arePacksPicturesLoading=false;
				for(int i=0;i<packsDisplayed.Count;i++)
				{
					this.packs[i].GetComponent<NewPackController>().setPackPicture(model.packs[this.packsDisplayed[i]].texture);
				}
			}
		}
		if(areCompetitionsPicturesLoading)
		{
			bool allPicturesLoaded=true;
			for(int i=0;i<model.competitions.Count;i++)
			{
				if(!model.competitions[i].isTextureLoaded)
				{
					allPicturesLoaded=false;
					break;
				}
			}
			if(allPicturesLoaded)
			{
				this.areCompetitionsPicturesLoading=false;
				for(int i=0;i<model.competitions.Count;i++)
				{
					this.competitions[i+1].GetComponent<CompetitionController>().setPicture(model.competitions[i].texture);
				}
			}
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewHomePageModel ();
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108f;
		this.elementsPerPageNotifications = 3;
		this.elementsPerPageNews = 3;
		this.elementsPerPageFriends = 2;
		this.initializeScene ();
		this.resize ();
	}
	public IEnumerator initialization()
	{
		newMenuController.instance.displayLoadingScreen ();
		if(ApplicationModel.launchEndGameSequence)
		{
			this.launchEndGameSequence(ApplicationModel.hasWonLastGame);
			ApplicationModel.launchEndGameSequence=false;
			ApplicationModel.hasWonLastGame=false;
		}
		yield return StartCoroutine (model.getData (this.totalNbResultLimit));
		this.initializeNotifications ();
		this.initializeNews ();
		this.initializePacks ();
		this.initializeCompetitions ();
		this.initializeStats ();
		this.retrieveDefaultDeck ();
		this.initializeDecks ();
		this.checkFriendsOnlineStatus ();
		this.isSceneLoaded = true;
		if(model.player.TutorialStep==1 || model.player.TutorialStep==4)
		{
			this.tutorial = Instantiate(this.tutorialObject) as GameObject;
			this.tutorial.AddComponent<HomePageTutorialController>();
			this.menu.GetComponent<newMenuController>().setTutorialLaunched(true);
			this.isTutorialLaunched=true;

			if(model.player.TutorialStep==1)
			{
				StartCoroutine(this.tutorial.GetComponent<HomePageTutorialController>().launchSequence(0));
			}
			else if(model.player.TutorialStep==4)
			{
				if(this.isEndGamePopUpDisplayed)
				{
					StartCoroutine(this.tutorial.GetComponent<HomePageTutorialController>().launchSequence(2));
				}
				else
				{
					StartCoroutine(this.tutorial.GetComponent<HomePageTutorialController>().launchSequence(3));
				}
			}
		}
		else if(model.player.ConnectionBonus>0)
		{
			this.displayConnectionBonusPopUp(model.player.ConnectionBonus);
		}
	}
	private void initializeNotifications()
	{
		this.chosenPageNotifications = 0;
		this.pageDebutNotifications = 0 ;
		this.drawPaginationNotifications();
		this.drawNotifications (true);
	}
	private void initializeNews()
	{
		this.chosenPageNews = 0;
		this.pageDebutNews = 0 ;
		this.drawPaginationNews();
		this.drawNews ();
	}
	private void initializeFriends()
	{
		this.chosenPageFriends = 0;
		this.pageDebutFriends = 0 ;
		this.drawPaginationFriends();
		this.drawFriends ();
	}
	private void initializePacks()
	{
		this.chosenPagePacks = 0;
		this.drawPacks ();
	}
	private void initializeCompetitions()
	{
		this.drawCompetitions ();
	}
	private void initializeStats()
	{
		this.drawStats ();
	}
	private void initializeDecks()
	{
		this.retrieveDecksList ();
		StartCoroutine(this.drawDeckCards ());
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("newMenu");
		menu.AddComponent<newHomePageMenuController> ();
		menu.GetComponent<newMenuController> ().setCurrentPage (0);
		this.friendsOnline = new List<int> ();
		this.newsBlock = Instantiate(this.blockObject) as GameObject;
		this.statsBlock = Instantiate(this.blockObject) as GameObject;
		this.deckBlock = Instantiate(this.blockObject) as GameObject;
		this.storeBlock = Instantiate(this.blockObject) as GameObject;
		this.friendsBlock = Instantiate (this.blockObject) as GameObject;
		this.notificationsBlock = Instantiate(this.blockObject) as GameObject;
		this.competitionsBlock = Instantiate(this.blockObject) as GameObject;
		this.deckBoard = GameObject.Find ("deckBoard");
		this.storeAssetsTitle = GameObject.Find ("StoreAssetsTitle");
		this.storeAssetsTitle.GetComponent<TextMeshPro> ().text = "Centre de recrutement";
		this.statsTitle = GameObject.Find ("StatsTitle");
		this.newsTitle = GameObject.Find ("NewsTitle");
		this.newsTitle.GetComponent<TextMeshPro> ().text = "Fil d'actualités";
		this.notificationsTitle = GameObject.Find ("NotificationsTitle");
		this.notificationsTitle.GetComponent<TextMeshPro> ().text = "Notifications";
		this.friendsTitle = GameObject.Find ("FriendsTitle");
		this.friendsTitle.GetComponent<TextMeshPro> ().text = "Amis en ligne";
		this.competitionsTitle = GameObject.Find ("CompetitionsTitle");
		this.connectedPlayersTitle = GameObject.Find ("ConnectedPlayers");
		this.competitionsTitle.GetComponent<TextMeshPro> ().text = "Jouer";
		this.stats = GameObject.Find ("Stats");
		this.paginationButtonsNotifications = new GameObject[0];
		this.paginationButtonsNews = new GameObject[0];
		this.paginationButtonsFriends = new GameObject[0];
		this.packs=new GameObject[0];
		this.notifications=new GameObject[3];
		for(int i=0;i<this.notifications.Length;i++)
		{
			this.notifications[i]=GameObject.Find ("Notification"+i);
			this.notifications[i].GetComponent<NotificationController>().setId(i);
			this.notifications[i].SetActive(false);
		}
		this.competitions=new GameObject[3];
		for(int i=0;i<this.competitions.Length;i++)
		{
			this.competitions[i]=GameObject.Find ("Competition"+i);
		}
		this.news=new GameObject[3];
		for(int i=0;i<this.news.Length;i++)
		{
			this.news[i]=GameObject.Find ("News"+i);
			this.news[i].GetComponent<NewsController>().setId(i);
			this.news[i].SetActive(false);
		}
		this.friends=new GameObject[2];
		for(int i=0;i<this.friends.Length;i++)
		{
			this.friends[i]=GameObject.Find ("Friend"+i);
			this.friends[i].GetComponent<OnlineFriendController>().setId(i);
			this.friends[i].SetActive(false);
		}
		this.deckBoard.transform.FindChild("deckList").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Mes decks";
		this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text="Aucune équipe";
		this.deckList = new List<GameObject> ();
		this.deckCards=new GameObject[4];
		for (int i=0;i<4;i++)
		{
			this.deckCards[i]=GameObject.Find("deckCard"+i);
			this.deckCards[i].AddComponent<NewCardHomePageController>();
			this.deckCards[i].SetActive(false);
		}
		this.collectionButton = GameObject.Find ("CollectionButton");
		this.collectionButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Cristalopedia";
		this.cleanCardsButton = GameObject.Find ("CleanCardsButton");
		this.cleanCardsButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Vider";
		if(!ApplicationModel.isAdmin)
		{
			this.cleanCardsButton.SetActive(false);
		}

		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardHomePageController> ();
		this.deckBoard.transform.FindChild("deckList").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Choisir une équipe";
		this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text="Aucune équipe créé";

		this.transparentBackground = GameObject.Find ("TransparentBackGround");
		this.transparentBackground.SetActive (false);
		this.endGamePopUp = GameObject.Find ("EndGamePopUp");
		this.endGamePopUp.SetActive (false);

	}
	public void resize()
	{
		if(this.isCardFocusedDisplayed)
		{
			this.hideCardFocused();
		}
		this.cleanPacks ();
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.collectionPointsWindow=new Rect(this.widthScreen - this.widthScreen * 0.17f-5,0.1f * this.heightScreen+5,this.widthScreen * 0.17f,this.heightScreen * 0.1f);
		this.newSkillsWindow = new Rect (this.collectionPointsWindow.xMin, this.collectionPointsWindow.yMax + 5,this.collectionPointsWindow.width,this.heightScreen - 0.1f * this.heightScreen - 2 * 5 - this.collectionPointsWindow.height);
		this.newCardTypeWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		float screenRatio = (float)this.widthScreen / (float)this.heightScreen;
		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);

		float storeBlockLeftMargin =3f;
		float storeBlockRightMargin = (this.worldWidth - 3f - 3f) / 2f + 3.1f;
		float storeBlockUpMargin = 6.1f;
		float storeBlockDownMargin = 0.2f;

		float storeBlockHeight = worldHeight - storeBlockUpMargin-storeBlockDownMargin;
		float storeBlockWidth = worldWidth-storeBlockLeftMargin-storeBlockRightMargin;
		Vector2 storeBlockOrigin = new Vector3 (-worldWidth/2f+storeBlockLeftMargin+storeBlockWidth/2f, -worldHeight / 2f + storeBlockDownMargin + storeBlockHeight / 2,0f);
		
		this.storeBlock.GetComponent<BlockController> ().resize(new Rect(storeBlockOrigin.x,storeBlockOrigin.y,storeBlockWidth,storeBlockHeight));
		this.storeAssetsTitle.transform.position = new Vector3 (storeBlockOrigin.x, storeBlockOrigin.y+storeBlockHeight/2f-0.3f, 0);

		float competitionsBlockLeftMargin = (this.worldWidth - 3f - 3f) / 2f + 3.1f;
		float competitionsBlockRightMargin = 3f;
		float competitionsBlockUpMargin = 6.1f;
		float competitionsBlockDownMargin = 0.2f;
		
		float competitionsBlockHeight = worldHeight - competitionsBlockUpMargin-competitionsBlockDownMargin;
		float competitionsBlockWidth = worldWidth-competitionsBlockLeftMargin-competitionsBlockRightMargin;
		Vector2 competitionsBlockOrigin = new Vector3 (-worldWidth/2f+competitionsBlockLeftMargin+competitionsBlockWidth/2f, -worldHeight / 2f + competitionsBlockDownMargin + competitionsBlockHeight / 2,0f);
		
		this.competitionsBlock.GetComponent<BlockController> ().resize(new Rect(competitionsBlockOrigin.x,competitionsBlockOrigin.y,competitionsBlockWidth,competitionsBlockHeight));
		this.competitionsTitle.transform.position = new Vector3 (competitionsBlockOrigin.x, competitionsBlockOrigin.y+competitionsBlockHeight/2f-0.3f, 0);
		this.connectedPlayersTitle.transform.position = new Vector3 (competitionsBlockOrigin.x,  competitionsBlockOrigin.y-competitionsBlockHeight/2f+0.2f, 0);

		float statsBlockLeftMargin = 3f;
		float statsBlockRightMargin = 3f;
		float statsBlockUpMargin = 3.6f;
		float statsBlockDownMargin = 4.1f;
		
		float statsBlockHeight = worldHeight - statsBlockUpMargin-statsBlockDownMargin;
		float statsBlockWidth = worldWidth-statsBlockLeftMargin-statsBlockRightMargin;
		Vector2 statsBlockOrigin = new Vector3 (-worldWidth/2f+statsBlockLeftMargin+statsBlockWidth/2f, -worldHeight / 2f + statsBlockDownMargin + statsBlockHeight / 2,0f);
		
		this.statsBlock.GetComponent<BlockController> ().resize(new Rect(statsBlockOrigin.x,statsBlockOrigin.y,statsBlockWidth,statsBlockHeight));
		this.statsTitle.transform.position = new Vector3 (statsBlockOrigin.x, statsBlockOrigin.y+statsBlockHeight/2f-0.3f, 0);

		this.stats.transform.position = new Vector3 (statsBlockOrigin.x, statsBlockOrigin.y, 0);
		this.stats.transform.FindChild ("nbWins").localPosition = new Vector3 (-1.5f * statsBlockWidth / 5f, 0f, 0);
		this.stats.transform.FindChild ("nbLooses").localPosition = new Vector3 (-0.5f * statsBlockWidth / 5f, 0f, 0);
		this.stats.transform.FindChild ("ranking").localPosition = new Vector3 (0.5f * statsBlockWidth / 5f, 0f, 0);
		this.stats.transform.FindChild ("collectionPoints").localPosition = new Vector3 (1.5f * statsBlockWidth / 5f, 0f, 0);
		this.collectionButton.transform.position = new Vector3 (1.5f * statsBlockWidth / 5f, statsBlockOrigin.y+statsBlockHeight/2f-0.3f, 0f);
		this.cleanCardsButton.transform.position = new Vector3 (1.5f * statsBlockWidth / 5f, statsBlockOrigin.y+statsBlockHeight/2f-0.58f, 0f);
		
		float deckBlockLeftMargin = 3f;
		float deckBlockRightMargin = 3f;
		float deckBlockUpMargin = 0.2f;
		float deckBlockDownMargin = 6.6f;
		
		float deckBlockHeight = worldHeight - deckBlockUpMargin-deckBlockDownMargin;
		float deckBlockWidth = worldWidth-deckBlockLeftMargin-deckBlockRightMargin;
		Vector2 deckBlockOrigin = new Vector3 (-worldWidth/2f+deckBlockLeftMargin+deckBlockWidth/2f, -worldHeight / 2f + deckBlockDownMargin + deckBlockHeight / 2,0f);
		
		this.deckBlock.GetComponent<BlockController> ().resize(new Rect(deckBlockOrigin.x,deckBlockOrigin.y,deckBlockWidth,deckBlockHeight));

		float newsBlockLeftMargin = this.worldWidth-2.8f;
		float newsBlockRightMargin = 0f;
		float newsBlockUpMargin = 4.1f;
		float newsBlockDownMargin = 2.6f;
		
		float newsBlockHeight = worldHeight - newsBlockUpMargin-newsBlockDownMargin;
		float newsBlockWidth = worldWidth-newsBlockLeftMargin-newsBlockRightMargin;
		Vector2 newsBlockOrigin = new Vector3 (-worldWidth/2f+newsBlockLeftMargin+newsBlockWidth/2f, -worldHeight / 2f + newsBlockDownMargin + newsBlockHeight / 2,0f);
		
		this.newsBlock.GetComponent<BlockController> ().resize(new Rect(newsBlockOrigin.x,newsBlockOrigin.y, newsBlockWidth, newsBlockHeight));
		this.newsTitle.transform.position = new Vector3 (newsBlockOrigin.x, newsBlockOrigin.y+newsBlockHeight/2f-0.3f, 0);
		
		float notificationsBlockLeftMargin = this.worldWidth-2.8f;
		float notificationsBlockRightMargin = 0f;
		float notificationsBlockUpMargin = 0.6f;
		float notificationsBlockDownMargin = 6.1f;
		
		float notificationsBlockHeight = worldHeight - notificationsBlockUpMargin-notificationsBlockDownMargin;
		float notificationsBlockWidth = worldWidth-notificationsBlockLeftMargin-notificationsBlockRightMargin;
		Vector2 notificationsBlockOrigin = new Vector3 (-worldWidth/2f+notificationsBlockLeftMargin+notificationsBlockWidth/2f, -worldHeight / 2f + notificationsBlockDownMargin + notificationsBlockHeight / 2,0f);
		
		this.notificationsBlock.GetComponent<BlockController> ().resize(new Rect(notificationsBlockOrigin.x,notificationsBlockOrigin.y,notificationsBlockWidth,notificationsBlockHeight));
		this.notificationsTitle.transform.position = new Vector3 (notificationsBlockOrigin.x, notificationsBlockOrigin.y+notificationsBlockHeight/2f-0.3f, 0);

		float friendsBlockLeftMargin = this.worldWidth-2.8f;
		float friendsBlockRightMargin = 0f;
		float friendsBlockUpMargin = 7.6f;
		float friendsBlockDownMargin = 0.2f;
		
		float friendsBlockHeight = worldHeight - friendsBlockUpMargin-friendsBlockDownMargin;
		float friendsBlockWidth = worldWidth-friendsBlockLeftMargin-friendsBlockRightMargin;
		Vector2 friendsBlockOrigin = new Vector3 (-worldWidth/2f+friendsBlockLeftMargin+friendsBlockWidth/2f, -worldHeight / 2f + friendsBlockDownMargin + friendsBlockHeight / 2,0f);
		
		this.friendsBlock.GetComponent<BlockController> ().resize(new Rect(friendsBlockOrigin.x,friendsBlockOrigin.y,friendsBlockWidth,friendsBlockHeight));
		this.friendsTitle.transform.position = new Vector3 (friendsBlockOrigin.x, friendsBlockOrigin.y+friendsBlockHeight/2f-0.3f, 0);

		float packScale = 0.84f;
		
		float packWidth = 250f;
		float packWorldWidth = (packWidth / pixelPerUnit) * packScale;
		
		this.packsPerLine = Mathf.FloorToInt ((storeBlockWidth-0.5f) / packWorldWidth);

		float packsGapWidth = (storeBlockWidth - (this.packsPerLine * packWorldWidth)) / (this.packsPerLine + 1);
		float packsBoardStartX = storeBlockOrigin.x - storeBlockWidth / 2f-packWorldWidth/2f;

		this.packs=new GameObject[this.packsPerLine];
		
		for(int i =0;i<this.packsPerLine;i++)
		{
			this.packs[i] = Instantiate(this.packObject) as GameObject;
			this.packs[i].transform.localScale= new Vector3(0.7f,0.7f,0.7f);
			this.packs[i].transform.position=new Vector3(packsBoardStartX+(i+1)*(packsGapWidth+packWorldWidth),storeBlockOrigin.y-0.5f,0f);
			this.packs[i].transform.name="Pack"+i;
			this.packs[i].AddComponent<NewPackHomePageController>();
			this.packs[i].transform.GetComponent<NewPackHomePageController>().setId(i);
			this.packs[i].SetActive(false);
		}

		float selectButtonWidth=219f;
		float selectButtonScale = 1.4f;
		float deleteRenameButtonScale = 0.7f;
		float deleteRenameButtonWidth = 219;
		float cardHaloWidth = 740f;
		float cardScale = 0.222f;
		float deckCardsInterval = 1.7f;

		float selectButtonWorldWidth = selectButtonScale*(selectButtonWidth / pixelPerUnit);
		float cardHaloWorldWidth = cardScale * (cardHaloWidth / pixelPerUnit);
		float deckCardsWidth = deckCardsInterval * 3f + cardHaloWorldWidth;
		float deckBoardLeftMargin = 2.9f;
		float deckBoardRightMargin = 2.9f;
		float cardsBoardUpMargin;
		float cardsBoardDownMargin = 0.5f;

		float cardWidth = 194f;
		float cardHeight = 271f;
		float cardWorldWidth = (cardWidth / pixelPerUnit) * cardScale;
		float cardWorldHeight = (cardHeight / pixelPerUnit) * cardScale;

		float tempWidth = worldWidth - deckBoardLeftMargin - deckBoardRightMargin - selectButtonWorldWidth - deckCardsWidth;
		
		if(tempWidth>0.25f)
		{
			this.deckBoard.transform.position=new Vector3(selectButtonWorldWidth/2f +tempWidth/4f,3.22f,0f);
			this.deckBoard.transform.FindChild("deckList").localPosition=new Vector3(-deckCardsWidth/2f-tempWidth/2f-selectButtonWorldWidth/2f,-0.32f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").localPosition=new Vector3(0f,0.27f,0f);
			this.deckBoard.transform.FindChild("deckList").FindChild("Title").localPosition=new Vector3(0,0.86f,0f);
		}
		else
		{
			this.deckBoard.transform.position=new Vector3(0,3.05f,0f);
			this.deckBoard.transform.FindChild("deckList").localPosition=new Vector3(0,1.43f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").localPosition=new Vector3(1.6f,0,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("Title").localPosition=new Vector3(-1.7f,0f,0f);
		}
		
		for(int i =0;i<this.competitions.Length;i++)
		{
			this.competitions[i].transform.position=new Vector3(competitionsBlockOrigin.x,competitionsBlockOrigin.y+0.75f-i*0.85f,0f);
			this.competitions[i].transform.GetComponent<CompetitionController>().setId(i);
		}

		for(int i=0;i<this.notifications.Length;i++)
		{
			this.notifications[i].transform.position=new Vector3(notificationsBlockOrigin.x-0.2f,notificationsBlockOrigin.y+notificationsBlockHeight/2f-0.85f-i*0.77f,0);
		}

		for(int i=0;i<this.news.Length;i++)
		{
			this.news[i].transform.position=new Vector3(newsBlockOrigin.x-0.2f,newsBlockOrigin.y+newsBlockHeight/2f-0.85f-i*0.77f,0);
		}

		for(int i=0;i<this.friends.Length;i++)
		{
			this.friends[i].transform.position=new Vector3(friendsBlockOrigin.x-0.2f,friendsBlockOrigin.y+friendsBlockHeight/2f-0.75f-i*0.65f,0); 
		}

		this.deckCardsPosition=new Vector3[4];
		this.deckCardsArea=new Rect[4];
		
		for(int i=0;i<4;i++)
		{
			this.deckCardsPosition[i]=this.deckBoard.transform.FindChild("Card"+i).position;
			this.deckCardsArea[i]=new Rect(this.deckCardsPosition[i].x-cardWorldWidth/2f,this.deckCardsPosition[i].y-cardWorldHeight/2f,cardWorldWidth,cardWorldHeight);
			this.deckCards[i].transform.position=this.deckCardsPosition[i];
			this.deckCards[i].transform.localScale=new Vector3(cardScale,cardScale,cardScale);
			this.deckCards[i].transform.GetComponent<NewCardHomePageController>().setId(i);
		}
		float focusedCardScale = 3.648985f;
		float focusedCardWidth = 194f;
		float focusedCardHeight = 271f;
		float focusedCardRightMargin = 0.5f;
		float focusedCardLeftMargin = 2.8f;
		float emptyWidth = this.worldWidth - focusedCardRightMargin - focusedCardLeftMargin;
		
		this.focusedCard.transform.position = new Vector3 (focusedCardLeftMargin+emptyWidth/2f-this.worldWidth/2f, -0.25f, 0f);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setCentralWindow (this.centralWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setCollectionPointsWindow (this.collectionPointsWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setNewSkillsWindow (this.newSkillsWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setNewCardTypeWindow (this.newCardTypeWindow);
		this.focusedCard.SetActive (false);

		if(this.isTutorialLaunched)
		{
			this.tutorial.GetComponent<TutorialObjectController>().resize();
		}

		this.transparentBackground.transform.position = new Vector3 (0, 0, -2f);
		this.endGamePopUp.transform.position = new Vector3 (0, 2f, -3f);
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
		newMenuController.instance.displayLoadingScreen ();
		this.deckCardsDisplayed = new int[]{-1,-1,-1,-1};
		if(this.deckDisplayed!=-1)
		{	
			yield return StartCoroutine(model.decks[this.deckDisplayed].RetrieveCards());
			
			for(int i=0;i<model.decks[this.deckDisplayed].Cards.Count;i++)
			{
				int deckOrder = model.decks[this.deckDisplayed].Cards[i].deckOrder;
				this.deckCardsDisplayed[deckOrder]=i;
			}
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text = model.decks[this.deckDisplayed].Name;
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(true);
		}
		else
		{
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text="Aucun deck créé";
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(false);
		}
		for(int i=0;i<this.deckCards.Length;i++)
		{
			if(this.deckCardsDisplayed[i]!=-1)
			{
				this.deckCards[i].transform.GetComponent<NewCardController>().c=model.decks[this.deckDisplayed].Cards[this.deckCardsDisplayed[i]];
				this.deckCards[i].transform.GetComponent<NewCardController>().show();
				this.deckCards[i].SetActive(true);
			}
			else
			{
				this.deckCards[i].SetActive(false);
			}
		}
		newMenuController.instance.hideLoadingScreen ();
	}
	public void showCardFocused()
	{
		this.isCardFocusedDisplayed = true;
		this.isHovering=false;
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		this.displayBackUI (false);
		this.focusedCard.SetActive (true);
		this.focusedCardIndex=this.deckCardsDisplayed[this.idCardClicked];
		this.focusedCard.GetComponent<NewFocusedCardController>().c=model.decks[this.deckDisplayed].Cards[this.deckCardsDisplayed[this.idCardClicked]];
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
		this.deckBoard.SetActive (value);
		this.deckBlock.GetComponent<BlockController>().display(value);
		for(int i=0;i<this.deckCardsDisplayed.Length;i++)
		{
			if(this.deckCardsDisplayed[i]!=-1)
			{
				this.deckCards[i].SetActive(value);
			}
		}
		this.newsBlock.GetComponent<BlockController>().display(value);
		this.newsTitle.SetActive (value);
		for(int i=0;i<this.newsDisplayed.Count;i++)
		{
			this.news[i].SetActive(value);
		}
		this.notificationsBlock.GetComponent<BlockController>().display(value);
		this.notificationsTitle.SetActive (value);
		for(int i=0;i<this.notificationsDisplayed.Count;i++)
		{
			this.notifications[i].SetActive(value);
		}
		this.friendsBlock.GetComponent<BlockController>().display(value);
		this.friendsTitle.SetActive (value);
		for(int i=0;i<this.friendsDisplayed.Count;i++)
		{
			this.friends[i].SetActive(value);
		}
		this.storeBlock.GetComponent<BlockController>().display(value);
		this.storeAssetsTitle.SetActive (value);
		for(int i=0;i<this.packs.Length;i++)
		{
			this.packs[i].SetActive(value);
		}
		this.competitionsBlock.GetComponent<BlockController>().display(value);
		this.competitionsTitle.SetActive (value);
		this.connectedPlayersTitle.SetActive (value);
		for(int i=0;i<this.competitions.Length;i++)
		{
			this.competitions[i].SetActive(value);
		}
		this.stats.SetActive (value);
		this.statsBlock.GetComponent<BlockController>().display(value);
		this.statsTitle.SetActive (value);
		this.cleanCardsButton.SetActive (value);
		this.collectionButton.SetActive (value);
		for(int i=0;i<this.paginationButtonsNews.Length;i++)
		{
			this.paginationButtonsNews[i].SetActive(value);
		}
		for(int i=0;i<this.paginationButtonsNotifications.Length;i++)
		{
			this.paginationButtonsNotifications[i].SetActive(value);
		}
		for(int i=0;i<this.paginationButtonsFriends.Length;i++)
		{
			this.paginationButtonsFriends[i].SetActive(value);
		}
	}
	public void selectDeck(int id)
	{
		this.deckDisplayed = this.decksDisplayed [id];
		this.cleanDeckList ();
		this.isSearchingDeck = false;
		this.initializeDecks ();
		StartCoroutine(model.player.SetSelectedDeck(model.decks[this.deckDisplayed].Id));
	}
	public void displayDeckList()
	{
		this.cleanDeckList ();
		if(!isSearchingDeck)
		{
			this.setDeckList ();
			this.isSearchingDeck=true;
		}
		else
		{
			this.isSearchingDeck=false;
		}
	}
	private void cleanDeckList ()
	{
		for(int i=0;i<this.deckList.Count;i++)
		{
			Destroy(this.deckList[i]);
		}
		this.deckList=new List<GameObject>();
	}
	private void setDeckList()
	{
		for (int i = 0; i < this.decksDisplayed.Count; i++) 
		{  
			this.deckList.Add (Instantiate(this.deckListObject) as GameObject);
			this.deckList[this.deckList.Count-1].transform.parent=this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck");
			this.deckList[this.deckList.Count-1].transform.localScale=new Vector3(1.4f,1.4f,1.4f);
			this.deckList[this.deckList.Count-1].transform.localPosition=new Vector3(0f, -0.45f+(this.deckList.Count-1)*(-0.32f),0f);
			this.deckList[this.deckList.Count-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text = model.decks [this.decksDisplayed[i]].Name;
			this.deckList[this.deckList.Count-1].GetComponent<DeckBoardDeckListHomePageController>().setId(i);
		}
	}
	public void mouseOnSelectDeckButton(bool value)
	{
		this.isMouseOnSelectDeckButton = value;
	}
	public void refreshCredits()
	{
		StartCoroutine(this.menu.GetComponent<newMenuController> ().getUserData ());
	}
	public void returnPressed()
	{
		if(newMenuController.instance.isAPopUpDisplayed())
		{
			newMenuController.instance.returnPressed();
		}
		else if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().returnPressed();
		}
		else if(isEndGamePopUpDisplayed)
		{
			this.hideEndGamePopUp();
		}
		else if(errorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
	}
	public void escapePressed()
	{
		if(newMenuController.instance.isAPopUpDisplayed())
		{
			newMenuController.instance.escapePressed();
		}
		else if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().escapePressed();
		}
		else if(isEndGamePopUpDisplayed)
		{
			this.hideEndGamePopUp();
		}
		else if(errorViewDisplayed)
		{
			this.hideErrorPopUp();
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
			this.deckBoard.GetComponent<DeckBoardController> ().changeCardsColor (new Color (155f / 255f, 220f / 255f, 1f));
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
		this.deckBoard.GetComponent<DeckBoardController> ().changeCardsColor (new Color (1f,1f, 1f));bool toCards=false;
		
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
		
		int idCard1 = model.decks [this.deckDisplayed].Cards [this.deckCardsDisplayed [this.idCardClicked]].Id;
		this.deckCards[position].SetActive(true);
		this.deckCards[position].GetComponent<NewCardController>().c=model.decks [this.deckDisplayed].Cards [this.deckCardsDisplayed [this.idCardClicked]];
		this.deckCards[position].GetComponent<NewCardController>().show();
		if(this.deckCardsDisplayed[position]!=-1)
		{
			int indexCard2=this.deckCardsDisplayed[position];
			int idCard2=model.decks [this.deckDisplayed].Cards [indexCard2].Id;
			this.deckCards[position].GetComponent<NewCardController>().c=model.decks [this.deckDisplayed].Cards [this.deckCardsDisplayed [this.idCardClicked]];
			this.deckCards[position].GetComponent<NewCardController>().show ();
			this.deckCardsDisplayed[position]=this.deckCardsDisplayed[this.idCardClicked];
			this.deckCards[this.idCardClicked].GetComponent<NewCardController>().c=model.decks [this.deckDisplayed].Cards [indexCard2];
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
		bool allPicturesLoaded = true;
		for(int i =0;i<elementsPerPageNotifications;i++)
		{
			if(this.chosenPageNotifications*this.elementsPerPageNotifications+i<model.notifications.Count)
			{
				if(!model.notifications[this.chosenPageNotifications*this.elementsPerPageNotifications+i].SendingUser.isThumbPictureLoaded)
				{
					if(!model.notifications[this.chosenPageNotifications*this.elementsPerPageNotifications+i].SendingUser.isThumbPictureLoading)
					{
						StartCoroutine(model.notifications[this.chosenPageNotifications*this.elementsPerPageNotifications+i].SendingUser.setThumbProfilePicture());
					}
					allPicturesLoaded=false;
				}
				this.notificationsDisplayed.Add (this.chosenPageNotifications*this.elementsPerPageNotifications+i);
				this.notifications[i].GetComponent<NotificationController>().n=model.notifications[this.chosenPageNotifications*this.elementsPerPageNotifications+i];
				this.notifications[i].GetComponent<NotificationController>().show();
				this.notifications[i].SetActive(true);
			}
			else
			{
				this.notifications[i].SetActive(false);
			}
		}
		if(!allPicturesLoaded)
		{
			this.areNotificationsPicturesLoading=true;
		}
		this.manageNonReadsNotifications (firstLoad);
	}
	public void drawNews()
	{
		this.newsDisplayed = new List<int> ();
		bool allPicturesLoaded = true;
		for(int i =0;i<elementsPerPageNews;i++)
		{
			if(this.chosenPageNews*this.elementsPerPageNews+i<model.news.Count)
			{
				if(!model.news[this.chosenPageNews*this.elementsPerPageNews+i].User.isThumbPictureLoaded)
				{
					if(!model.news[this.chosenPageNews*this.elementsPerPageNews+i].User.isThumbPictureLoading)
					{
						StartCoroutine(model.news[this.chosenPageNews*this.elementsPerPageNews+i].User.setThumbProfilePicture());
					}
					allPicturesLoaded=false;
				}
				this.newsDisplayed.Add (this.chosenPageNews*this.elementsPerPageNews+i);
				this.news[i].GetComponent<NewsController>().n=model.news[this.chosenPageNews*this.elementsPerPageNews+i];
				this.news[i].GetComponent<NewsController>().show();
				this.news[i].SetActive(true);
			}
			else
			{
				this.news[i].SetActive(false);
			}
		}
		if(!allPicturesLoaded)
		{
			this.areNewsPicturesLoading=true;
		}
	}
	public void drawFriends()
	{
		this.friendsDisplayed = new List<int> ();
		bool allPicturesLoaded = true;
		for(int i =0;i<elementsPerPageFriends;i++)
		{
			if(this.chosenPageFriends*this.elementsPerPageFriends+i<this.friendsOnline.Count)
			{
				if(!model.users[this.friendsOnline[this.chosenPageFriends*this.elementsPerPageFriends+i]].isThumbPictureLoaded)
				{
					if(!model.users[this.friendsOnline[this.chosenPageFriends*this.elementsPerPageFriends+i]].isThumbPictureLoading)
					{
						StartCoroutine(model.users[this.friendsOnline[this.chosenPageFriends*this.elementsPerPageFriends+i]].setThumbProfilePicture());
					}
					allPicturesLoaded=false;
				}
				this.friendsDisplayed.Add (this.chosenPageFriends*this.elementsPerPageFriends+i);
				this.friends[i].GetComponent<OnlineFriendController>().u=model.users[this.friendsOnline[this.chosenPageFriends*this.elementsPerPageFriends+i]];
				this.friends[i].GetComponent<OnlineFriendController>().show();
				if(!this.isCardFocusedDisplayed)
				{
					this.friends[i].SetActive(true);
				}
				else
				{
					this.friends[i].SetActive(false);
				}
			}
			else
			{
				this.friends[i].SetActive(false);
			}
		}
		if(!allPicturesLoaded)
		{
			this.areFriendsPicturesLoading=true;
		}
	}
	public void drawPacks()
	{
		this.packsDisplayed = new List<int> ();
		int tempInt = this.packsPerLine;
		if(this.chosenPagePacks*(packsPerLine)+packsPerLine>this.model.packs.Count)
		{
			tempInt=model.packs.Count-this.chosenPagePacks*(packsPerLine);
		}
		bool allPicturesLoaded = true;
		for(int i=0;i<packsPerLine;i++)
		{
			if(this.chosenPagePacks*(packsPerLine)+i<this.model.packs.Count)
			{
				if(!model.packs[this.chosenPagePacks*(packsPerLine)+i].isTextureLoaded)
				{
					StartCoroutine(model.packs[this.chosenPagePacks*(packsPerLine)+i].setPicture());
					allPicturesLoaded=false;
				}
				this.packsDisplayed.Add (this.chosenPagePacks*(packsPerLine)+i);
				this.packs[i].SetActive(true);
				this.packs[i].transform.GetComponent<NewPackHomePageController>().p=model.packs[this.chosenPagePacks*(packsPerLine)+i];
				this.packs[i].transform.GetComponent<NewPackHomePageController>().show();
				this.packs[i].transform.GetComponent<NewPackHomePageController>().setId(i);
			}
			else
			{
				this.packs[i].SetActive(false);
			}
		}
		if(!allPicturesLoaded)
		{
			this.arePacksPicturesLoading=true;
		}
		this.nbPagesPacks = Mathf.CeilToInt((float)model.packs.Count / (float)this.packsPerLine);
	}
	public void drawCompetitions()
	{	
		this.competitions [0].transform.GetComponent<CompetitionController> ().show ("Entrainement");
		bool allPicturesLoaded = true;
		for(int i=0;i<competitions.Length;i++)
		{
			if(i>0)
			{
				this.competitions[i].transform.GetComponent<CompetitionController>().show(model.competitions[i-1].Name);
				if(!model.competitions[i-1].isTextureLoaded)
				{
					StartCoroutine(model.competitions[i-1].setPicture());
					this.competitions[i].transform.GetComponent<CompetitionController>().setPicture(model.competitions[i-1].texture);
					allPicturesLoaded=false;
				}
			}
			this.competitions[i].transform.GetComponent<CompetitionController>().setId(i);
		}
		if(!allPicturesLoaded)
		{
			this.areCompetitionsPicturesLoading=true;
		}
	}
	public void drawStats()
	{
		this.stats.transform.FindChild ("nbWins").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.TotalNbWins.ToString ();
		this.stats.transform.FindChild ("nbWins").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Victoires";
		this.stats.transform.FindChild ("nbLooses").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.TotalNbLooses.ToString ();
		this.stats.transform.FindChild ("nbLooses").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Défaites";
		this.stats.transform.FindChild ("ranking").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Classement combattant";
		this.stats.transform.FindChild ("ranking").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.Ranking.ToString ();
		this.stats.transform.FindChild ("ranking").FindChild ("Title2").GetComponent<TextMeshPro> ().text = "("+model.player.RankingPoints.ToString()+" pts)";
		this.stats.transform.FindChild ("collectionPoints").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Classement collectionneur";
		this.stats.transform.FindChild ("collectionPoints").FindChild ("Value").GetComponent<TextMeshPro> ().text = model.player.CollectionRanking.ToString ();
		this.stats.transform.FindChild ("collectionPoints").FindChild ("Title2").GetComponent<TextMeshPro> ().text = "("+model.player.CollectionPoints.ToString()+" pts)";
	}
	public void cleanPacks()
	{
		for (int i=0;i<this.packs.Length;i++)
		{
			Destroy (this.packs[i]);
		}
	}
	private void drawPaginationNotifications()
	{
		for(int i=0;i<this.paginationButtonsNotifications.Length;i++)
		{
			Destroy (this.paginationButtonsNotifications[i]);
		}
		this.paginationButtonsNotifications = new GameObject[0];
		this.activePaginationButtonIdNotifications = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPagesNotifications = Mathf.CeilToInt((float)model.notifications.Count / ((float)this.elementsPerPageNotifications));
		if(this.nbPagesNotifications>1)
		{
			this.nbPaginationButtonsLimitNotifications = Mathf.CeilToInt((2.9f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebutNotifications !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebutNotifications+nbPaginationButtonsLimitNotifications-System.Convert.ToInt32(drawBackButton)<this.nbPagesNotifications-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimitNotifications;
			}
			else
			{
				nbButtonsToDraw=this.nbPagesNotifications-this.pageDebutNotifications;
			}
			this.paginationButtonsNotifications = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsNotifications[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsNotifications[i].AddComponent<HomePageNotificationsPaginationController>();
				this.paginationButtonsNotifications[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),1.35f,0f);
				this.paginationButtonsNotifications[i].name="PaginationNotification"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsNotifications[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutNotifications+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsNotifications[i].GetComponent<HomePageNotificationsPaginationController>().setId(i);
				if(this.pageDebutNotifications+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageNotifications)
				{
					this.paginationButtonsNotifications[i].GetComponent<HomePageNotificationsPaginationController>().setActive(true);
					this.activePaginationButtonIdNotifications=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsNotifications[0].GetComponent<HomePageNotificationsPaginationController>().setId(-2);
				this.paginationButtonsNotifications[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsNotifications[nbButtonsToDraw-1].GetComponent<HomePageNotificationsPaginationController>().setId(-1);
				this.paginationButtonsNotifications[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandlerNotifications(int id)
	{
		if(id==-2)
		{
			this.pageDebutNotifications=this.pageDebutNotifications-this.nbPaginationButtonsLimitNotifications+1+System.Convert.ToInt32(this.pageDebutNotifications-this.nbPaginationButtonsLimitNotifications+1!=0);
			this.drawPaginationNotifications();
		}
		else if(id==-1)
		{
			this.pageDebutNotifications=this.pageDebutNotifications+this.nbPaginationButtonsLimitNotifications-1-System.Convert.ToInt32(this.pageDebutNotifications!=0);
			this.drawPaginationNotifications();
		}
		else
		{
			if(activePaginationButtonIdNotifications!=-1)
			{
				this.paginationButtonsNotifications[this.activePaginationButtonIdNotifications].GetComponent<HomePageNotificationsPaginationController>().setActive(false);
			}
			this.activePaginationButtonIdNotifications=id;
			this.chosenPageNotifications=this.pageDebutNotifications-System.Convert.ToInt32(this.pageDebutNotifications!=0)+id;
			this.drawNotifications();
		}
	}
	private void drawPaginationNews()
	{
		for(int i=0;i<this.paginationButtonsNews.Length;i++)
		{
			Destroy (this.paginationButtonsNews[i]);
		}
		this.paginationButtonsNews = new GameObject[0];
		this.activePaginationButtonIdNews = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPagesNews = Mathf.CeilToInt((float)model.news.Count / ((float)this.elementsPerPageNews));
		if(this.nbPagesNews>1)
		{
			this.nbPaginationButtonsLimitNews = Mathf.CeilToInt((2.9f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebutNews !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebutNews+nbPaginationButtonsLimitNews-System.Convert.ToInt32(drawBackButton)<this.nbPagesNews-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimitNews;
			}
			else
			{
				nbButtonsToDraw=this.nbPagesNews-this.pageDebutNews;
			}
			this.paginationButtonsNews = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsNews[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsNews[i].AddComponent<HomePageNewsPaginationController>();
				this.paginationButtonsNews[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-2.15f,0f);
				this.paginationButtonsNews[i].name="PaginationNews"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsNews[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutNews+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsNews[i].GetComponent<HomePageNewsPaginationController>().setId(i);
				if(this.pageDebutNews+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageNews)
				{
					this.paginationButtonsNews[i].GetComponent<HomePageNewsPaginationController>().setActive(true);
					this.activePaginationButtonIdNews=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsNews[0].GetComponent<HomePageNewsPaginationController>().setId(-2);
				this.paginationButtonsNews[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsNews[nbButtonsToDraw-1].GetComponent<HomePageNewsPaginationController>().setId(-1);
				this.paginationButtonsNews[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandlerNews(int id)
	{
		if(id==-2)
		{
			this.pageDebutNews=this.pageDebutNews-this.nbPaginationButtonsLimitNews+1+System.Convert.ToInt32(this.pageDebutNews-this.nbPaginationButtonsLimitNews+1!=0);
			this.drawPaginationNews();
		}
		else if(id==-1)
		{
			this.pageDebutNews=this.pageDebutNews+this.nbPaginationButtonsLimitNews-1-System.Convert.ToInt32(this.pageDebutNews!=0);
			this.drawPaginationNews();
		}
		else
		{
			if(activePaginationButtonIdNews!=-1)
			{
				this.paginationButtonsNews[this.activePaginationButtonIdNews].GetComponent<HomePageNewsPaginationController>().setActive(false);
			}
			this.activePaginationButtonIdNews=id;
			this.chosenPageNews=this.pageDebutNews-System.Convert.ToInt32(this.pageDebutNews!=0)+id;
			this.drawNews();
		}
	}
	private void drawPaginationFriends()
	{
		for(int i=0;i<this.paginationButtonsFriends.Length;i++)
		{
			Destroy (this.paginationButtonsFriends[i]);
		}
		this.paginationButtonsFriends = new GameObject[0];
		this.activePaginationButtonIdFriends = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPagesFriends = Mathf.CeilToInt((float)this.friendsOnline.Count / ((float)this.elementsPerPageFriends));
		if(this.nbPagesFriends>1)
		{
			this.nbPaginationButtonsLimitFriends = Mathf.CeilToInt((2.9f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebutFriends !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebutFriends+nbPaginationButtonsLimitFriends-System.Convert.ToInt32(drawBackButton)<this.nbPagesFriends-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimitFriends;
			}
			else
			{
				nbButtonsToDraw=this.nbPagesFriends-this.pageDebutFriends;
			}
			this.paginationButtonsFriends = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtonsFriends[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtonsFriends[i].AddComponent<HomePageFriendsPaginationController>();
				this.paginationButtonsFriends[i].transform.position=new Vector3(this.worldWidth/2f-2.9f/2f+(0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
				this.paginationButtonsFriends[i].name="PaginationFriends"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtonsFriends[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebutFriends+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtonsFriends[i].GetComponent<HomePageFriendsPaginationController>().setId(i);
				if(this.pageDebutFriends+i-System.Convert.ToInt32(drawBackButton)==this.chosenPageFriends)
				{
					this.paginationButtonsFriends[i].GetComponent<HomePageFriendsPaginationController>().setActive(true);
					this.activePaginationButtonIdFriends=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtonsFriends[0].GetComponent<HomePageFriendsPaginationController>().setId(-2);
				this.paginationButtonsFriends[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtonsFriends[nbButtonsToDraw-1].GetComponent<HomePageFriendsPaginationController>().setId(-1);
				this.paginationButtonsFriends[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandlerFriends(int id)
	{
		if(id==-2)
		{
			this.pageDebutFriends=this.pageDebutFriends-this.nbPaginationButtonsLimitFriends+1+System.Convert.ToInt32(this.pageDebutFriends-this.nbPaginationButtonsLimitFriends+1!=0);
			this.drawPaginationFriends();
		}
		else if(id==-1)
		{
			this.pageDebutFriends=this.pageDebutFriends+this.nbPaginationButtonsLimitFriends-1-System.Convert.ToInt32(this.pageDebutFriends!=0);
			this.drawPaginationFriends();
		}
		else
		{
			if(activePaginationButtonIdFriends!=-1)
			{
				this.paginationButtonsFriends[this.activePaginationButtonIdFriends].GetComponent<HomePageFriendsPaginationController>().setActive(false);
			}
			this.activePaginationButtonIdNews=id;
			this.chosenPageFriends=this.pageDebutFriends-System.Convert.ToInt32(this.pageDebutFriends!=0)+id;
			this.drawFriends();
		}
	}
	public void buyPackHandler(int id)
	{
		ApplicationModel.packToBuy = model.packs [this.packsDisplayed[id]].Id;
		PhotonNetwork.Disconnect();
		Application.LoadLevel ("NewStore");
	}
	public void collectionButtonHandler()
	{
		PhotonNetwork.Disconnect();
		Application.LoadLevel ("NewSkillBook");
	}
	public void cleanCardsHandler()
	{
		StartCoroutine (this.cleanCards ());
	}
	private IEnumerator cleanCards()
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.player.cleanCards ());
		StartCoroutine(this.initialization ());
	}
	public void startHoveringPopUp()
	{
		this.isHoveringPopUp = true;
		this.toDestroyPopUp = false;
		this.popUpDestroyInterval = 0f;
	}
	public void endHoveringPopUp()
	{
		this.isHoveringPopUp = false;
		this.toDestroyPopUp = true;
		this.popUpDestroyInterval = 0f;
	}
	public void startHoveringNotification (int id)
	{
		this.idNotificationHovered=id;
		this.isHoveringNotification = true;
		if(this.isPopUpDisplayed && this.popUp.GetComponent<PopUpController>().getIsNotification())
		{
			if(this.popUp.GetComponent<PopUpNotificationHomePageController>().getId()!=this.idNotificationHovered);
			{
				this.hidePopUp();
				this.showPopUpNotification();
			}
		}
		else
		{
			if(this.isPopUpDisplayed)
			{
				this.hidePopUp();
			}
			this.showPopUpNotification();
		}
	}
	public void startHoveringNews (int id)
	{
		this.idNewsHovered=id;
		this.isHoveringNews = true;
		if(this.isPopUpDisplayed && this.popUp.GetComponent<PopUpController>().getIsNews())
		{
			if(this.popUp.GetComponent<PopUpNewsHomePageController>().getId()!=this.idNewsHovered);
			{
				this.hidePopUp();
				this.showPopUpNews();
			}
		}
		else
		{
			if(this.isPopUpDisplayed)
			{
				this.hidePopUp();
			}
			this.showPopUpNews();
		}
	}
	public void startHoveringCompetition (int id)
	{
		this.idCompetitionHovered=id;
		this.isHoveringCompetition = true;
		if(this.isPopUpDisplayed && this.popUp.GetComponent<PopUpController>().getIsCompetition())
		{
			if(this.popUp.GetComponent<PopUpCompetitionsHomePageController>().getId()!=this.idCompetitionHovered);
			{
				this.hidePopUp();
				this.showPopUpCompetition();
			}
		}
		else
		{
			if(this.isPopUpDisplayed)
			{
				this.hidePopUp();
			}
			this.showPopUpCompetition();
		}
	}
	public void startHoveringFriend (int id)
	{
		this.idFriendHovered=id;
		this.isHoveringFriend = true;
		if(this.isPopUpDisplayed && this.popUp.GetComponent<PopUpController>().getIsFriend())
		{
			if(this.popUp.GetComponent<PopUpFriendHomePageController>().getId()!=this.idFriendHovered);
			{
				this.hidePopUp();
				this.showPopUpFriend();
			}
		}
		else
		{
			if(this.isPopUpDisplayed)
			{
				this.hidePopUp();
			}
			this.showPopUpFriend();
		}
	}
	public void endHoveringNotification ()
	{
		this.isHoveringNotification = false;
		this.toDestroyPopUp = true;
		this.popUpDestroyInterval = 0f;
	}
	public void endHoveringNews ()
	{
		this.isHoveringNews = false;
		this.toDestroyPopUp = true;
		this.popUpDestroyInterval = 0f;
	}
	public void endHoveringCompetition ()
	{
		this.isHoveringCompetition = false;
		this.toDestroyPopUp = true;
		this.popUpDestroyInterval = 0f;
	}
	public void endHoveringFriend ()
	{
		this.isHoveringNews = false;
		this.toDestroyPopUp = true;
		this.popUpDestroyInterval = 0f;
	}
	public void showPopUpNotification()
	{
		this.popUp = Instantiate(this.popUpObject) as GameObject;
		this.popUp.transform.position=new Vector3(this.notifications[this.idNotificationHovered].transform.position.x-3.1f,this.notifications[this.idNotificationHovered].transform.position.y,-1f);
		this.popUp.AddComponent<PopUpNotificationHomePageController>();
		this.popUp.GetComponent<PopUpNotificationHomePageController> ().setIsNotification (true);
		this.popUp.GetComponent<PopUpNotificationHomePageController> ().setId (this.idNotificationHovered);
		this.popUp.GetComponent<PopUpNotificationHomePageController> ().show (model.notifications [this.notificationsDisplayed [this.idNotificationHovered]]);
		this.isPopUpDisplayed=true;
	}
	public void showPopUpNews()
	{
		this.popUp = Instantiate(this.popUpObject) as GameObject;
		this.popUp.transform.position=new Vector3(this.news[this.idNewsHovered].transform.position.x-3.1f,this.news[this.idNewsHovered].transform.position.y,-1f);
		this.popUp.AddComponent<PopUpNewsHomePageController>();
		this.popUp.GetComponent<PopUpNewsHomePageController> ().setIsNews (true);
		this.popUp.GetComponent<PopUpNewsHomePageController> ().setId (this.idNewsHovered);
		this.popUp.GetComponent<PopUpNewsHomePageController> ().show (model.news [this.newsDisplayed [this.idNewsHovered]]);
		this.isPopUpDisplayed=true;
	}
	public void showPopUpFriend()
	{
		this.popUp = Instantiate(this.popUpObject) as GameObject;
		this.popUp.transform.position=new Vector3(this.friends[this.idFriendHovered].transform.position.x-3.1f,this.friends[this.idFriendHovered].transform.position.y,-1f);
		this.popUp.AddComponent<PopUpFriendHomePageController>();
		this.popUp.GetComponent<PopUpFriendHomePageController> ().setIsFriend (true);
		this.popUp.GetComponent<PopUpFriendHomePageController> ().setId (this.idFriendHovered);
		this.popUp.GetComponent<PopUpFriendHomePageController> ().show (model.users [this.friendsOnline[this.friendsDisplayed[this.idFriendHovered]]]);
		this.isPopUpDisplayed=true;
	}
	public void showPopUpCompetition()
	{
		this.popUp = Instantiate(this.popUpCompetitionObject) as GameObject;
		this.popUp.transform.position=new Vector3(this.competitions[this.idCompetitionHovered].transform.position.x-3.5f,this.competitions[this.idCompetitionHovered].transform.position.y,-1f);
		this.popUp.AddComponent<PopUpCompetitionsHomePageController>();
		this.popUp.GetComponent<PopUpCompetitionsHomePageController> ().setIsCompetition (true);
		this.popUp.GetComponent<PopUpCompetitionsHomePageController> ().setId (this.idCompetitionHovered);
		if(model.competitions[this.idCompetitionHovered-1].GetType()==typeof(Division))
		{
			this.popUp.GetComponent<PopUpCompetitionsHomePageController> ().showDivision (model.currentDivision);
		}
		else if(model.competitions[this.idCompetitionHovered-1].GetType()==typeof(Cup))
		{
			this.popUp.GetComponent<PopUpCompetitionsHomePageController> ().showCup (model.currentCup);
		}
		this.isPopUpDisplayed=true;
	}
	public void hidePopUp()
	{
		this.toDestroyPopUp = false;
		this.popUpDestroyInterval = 0f;
		Destroy (this.popUp);
		this.isPopUpDisplayed=false;
	}
	private IEnumerator refreshNonReadsNotifications()
	{
		this.notificationsTimer=this.notificationsTimer-this.refreshInterval;
		yield return StartCoroutine(model.player.countNonReadsNotifications(this.totalNbResultLimit));
		menu.GetComponent<newMenuController>().setNbNotificationsNonRead(model.player.nonReadNotifications);
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
			menu.GetComponent<newMenuController>().setNbNotificationsNonRead(nbNonReadNotifications-tempList.Count);
		}
		if(tempList.Count>0)
		{
			yield return StartCoroutine(model.updateReadNotifications (tempList,this.totalNbResultLimit));
			menu.GetComponent<newMenuController>().setNbNotificationsNonRead(model.player.nonReadNotifications);
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
	public IEnumerator endTutorial()
	{
		newMenuController.instance.setTutorialLaunched (false);
		//PhotonNetwork.Disconnect();
		if(TutorialObjectController.instance.getSequenceID()==1)
		{
			newMenuController.instance.displayLoadingScreen();
			yield return StartCoroutine (model.player.setTutorialStep (2));
			Application.LoadLevel ("newMyGame");
		}
		else if(TutorialObjectController.instance.getSequenceID()==3)
		{
			newMenuController.instance.displayLoadingScreen();
			yield return StartCoroutine (model.player.setTutorialStep (5));
			Application.LoadLevel ("newStore");
		}
	}
	public void joinGameHandler(int id)
	{
		if(this.deckDisplayed==-1)
		{
			this.displayErrorPopUp("Vous ne pouvez lancer de match sans avoir au préalable créé un deck");
		}
		else
		{
			ApplicationModel.gameType = id;
			StartCoroutine (this.joinGame ());
		}
	}
	public IEnumerator joinGame()
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.player.SetSelectedDeck (model.decks [this.deckDisplayed].Id));
		if(ApplicationModel.gameType==0)
		{

			newMenuController.instance.joinRandomRoom();
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
		this.transparentBackground.SetActive (true);
	}
	public void hideEndGamePopUp()
	{
		this.isEndGamePopUpDisplayed = false;
		this.endGamePopUp.SetActive (false);
		this.transparentBackground.SetActive (false);
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
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
						model.users[j].OnlineStatus=2;
						if(model.friends.Contains(j))
						{
							if(!this.friendsOnline.Contains(j))
							{
								this.friendsOnline.Insert(0,j);
							}
						}
					}
					else if(PhotonNetwork.Friends[i].IsOnline)
					{
						model.users[j].OnlineStatus=1;
						if(model.friends.Contains(j))
						{
							if(!this.friendsOnline.Contains(j))
							{
								this.friendsOnline.Insert(0,j);
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
		if(this.chosenPageFriends == 0)
		{
			this.initializeFriends();
		}
	}
	public void sendInvitationHandler()
	{
		if(this.deckDisplayed==-1)
		{
			this.displayErrorPopUp("Vous ne pouvez lancer de match sans avoir au préalable créé un deck");
		}
		else if(model.users [this.friendsOnline [this.friendsDisplayed [this.idFriendHovered]]].OnlineStatus!=1)
		{
			this.displayErrorPopUp("Votre adversaire n'est plus disponible");
		}
		else
		{
			StartCoroutine (this.sendInvitation ());
		}
	}
	public IEnumerator sendInvitation()
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.player.SetSelectedDeck (model.decks [this.deckDisplayed].Id));
		StartCoroutine (newMenuController.instance.sendInvitation (model.users [this.friendsOnline [this.friendsDisplayed [this.idFriendHovered]]], model.player));
	}
	public void errorPopUpResize()
	{
		errorView.popUpVM.centralWindow = this.centralWindow;
		errorView.popUpVM.resize ();
	}
	public void displayErrorPopUp(string error)
	{
		this.errorViewDisplayed = true;
		this.errorView = Camera.main.gameObject.AddComponent <NewHomePageErrorPopUpView>();
		errorView.errorPopUpVM.error = error;
		errorView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.customStyles[3]);
		errorView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		errorView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		errorView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		this.errorPopUpResize ();
	}
	public void hideErrorPopUp()
	{
		Destroy (this.errorView);
		this.errorViewDisplayed = false;
	}
}
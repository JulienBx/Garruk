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
	
	public GameObject tutorialObject;
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
	private GameObject[] newsfeedPagination;
	private GameObject[] deckChoices;
	private GameObject[] cardsHalos;
	private GameObject popUp;
	
	private GameObject menu;
	private GameObject tutorial;
	private GameObject[] deckCards;
	
	private GameObject endGamePopUp;

	private Rect centralWindow;
	private Rect collectionPointsWindow;
	private Rect newSkillsWindow;
	private Rect newCardTypeWindow;

	private int activeTab;
	
	private IList<int> newsDisplayed;
	private IList<int> notificationsDisplayed;
	private int packDisplayed;
	private IList<int> friendsDisplayed;
	private IList<int> friendsToBeDisplayed;

	private IList<int> friendsOnline;
	
	private int nbPages;
	private int elementsPerPage;
	private int chosenPage;

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

	private bool isTutorialLaunched;
	private bool isEndGamePopUpDisplayed;

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
	}
	void Awake()
	{
		instance = this;
		this.activeTab = 0;
		this.model = new NewHomePageModel ();
		this.elementsPerPage = 3;
		this.initializeScene ();
		this.resize ();
	}
	public IEnumerator initialization()
	{
		MenuController.instance.displayLoadingScreen ();
		if(ApplicationModel.launchEndGameSequence)
		{
			this.launchEndGameSequence(ApplicationModel.hasWonLastGame);
			ApplicationModel.launchEndGameSequence=false;
			ApplicationModel.hasWonLastGame=false;
		}
		yield return StartCoroutine (model.getData (this.totalNbResultLimit));
		this.selectATab ();
		this.initializePacks ();
		this.retrieveDefaultDeck ();
		this.initializeDecks ();
		this.initializeCompetitions ();
		this.checkFriendsOnlineStatus ();
		this.isSceneLoaded = true;
		if(model.player.TutorialStep==1 || model.player.TutorialStep==4)
		{
			this.tutorial = Instantiate(this.tutorialObject) as GameObject;
			this.tutorial.AddComponent<HomePageTutorialController>();
			this.menu.GetComponent<MenuController>().setTutorialLaunched(true);
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
				this.tabs[i].GetComponent<NewHomePageTabController>().setActive(true);
				this.tabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(75f/255f,163f/255f,174f/255f);
			}
			else
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(0);
				this.tabs[i].GetComponent<NewHomePageTabController>().setActive(false);
				this.tabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(196f/255f,196f/255f,196f/255f);
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
		this.chosenPage = 0;
		this.nbPages = Mathf.CeilToInt((float) model.notifications.Count / ((float)this.elementsPerPage));
		this.setPagination ();
		for(int i=0;i<this.contents.Length;i++)
		{
			this.challengeButtons[i].SetActive(false);
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=new Color(218f/255f,218f/255f,218f/255f);
		}
		this.drawNotifications (true);
	}
	private void initializeNews()
	{
		this.chosenPage = 0;
		this.nbPages = Mathf.CeilToInt((float) model.news.Count / ((float)this.elementsPerPage));
		this.setPagination ();
		for(int i=0;i<this.contents.Length;i++)
		{
			this.challengeButtons[i].SetActive(false);
			this.contents[i].transform.FindChild("new").gameObject.SetActive(false);
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=new Color(218f/255f,218f/255f,218f/255f);
		}
		this.drawNews ();
	}
	private void initializeFriends()
	{
		this.chosenPage = 0;
		this.sortFriendsList ();
		this.nbPages = Mathf.CeilToInt((float) model.friends.Count / ((float)this.elementsPerPage));
		this.setPagination ();
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i].transform.FindChild("new").gameObject.SetActive(false);
			this.contents[i].transform.FindChild("date").gameObject.SetActive(false);
		}
		this.drawFriends ();
	}
	private void initializePacks()
	{
		this.nbPacks = model.packs.Count;
		this.displayedPack = 0;
		this.drawPack ();
	}
	public void paginationHandler(int id)
	{
		if(id==0)
		{
			this.chosenPage--;
		}
		else
		{
			this.chosenPage++;
		}
		this.setPagination ();
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
	private void setPagination()
	{
		if(this.chosenPage==0)
		{
			this.newsfeedPagination[0].GetComponent<NewHomePagePaginationButtonController>().setIsHoverd(false);
			this.newsfeedPagination[0].SetActive(false);
		}
		else
		{
			this.newsfeedPagination[0].SetActive(true);
		}
		if(this.nbPages>1 && this.chosenPage!=this.nbPages-1)
		{
			this.newsfeedPagination[1].SetActive(true);
		}
		else
		{
			this.newsfeedPagination[1].GetComponent<NewHomePagePaginationButtonController>().setIsHoverd(false);
			this.newsfeedPagination[1].SetActive(false);
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
		menu = GameObject.Find ("Menu");
		menu.AddComponent<HomePageMenuController> ();
		menu.GetComponent<MenuController> ().setCurrentPage (0);
		this.deckBlock = Instantiate (this.blockObject) as GameObject;
		this.deckBlockTitle = GameObject.Find ("DeckBlockTitle");
		this.deckBlockTitle.GetComponent<TextMeshPro> ().text = "Mes équipes";
		this.deckSelectionButton = GameObject.Find ("DeckSelectionButton");
		this.deckTitle = GameObject.Find ("DeckTitle");
		this.playBlock = Instantiate (this.blockObject) as GameObject;
		this.playBlockTitle = GameObject.Find ("PlayBlockTitle");
		this.playBlockTitle.GetComponent<TextMeshPro> ().text = "Jouer";
		this.friendlyGameButton = GameObject.Find ("FriendlyGameButton");
		this.friendlyGamePicture = GameObject.Find ("FriendlyGamePicture");
		this.friendlyGameTitle = GameObject.Find ("FriendlyGameTitle");
		this.divisionGameButton = GameObject.Find ("DivisionGameButton");
		this.divisionGamePicture = GameObject.Find ("DivisionGamePicture");
		this.divisionGameTitle = GameObject.Find ("DivisionGameTitle");
		this.cupGameButton = GameObject.Find ("CupGameButton");
		this.cupGamePicture = GameObject.Find ("CupGamePicture");
		this.cupGameTitle = GameObject.Find ("CupGameTitle");
		this.packButton = GameObject.Find ("PackButton");
		this.packTitle = GameObject.Find ("PackTitle");
		this.packPicture = GameObject.Find ("PackPicture");
		this.storeBlock = Instantiate (this.blockObject) as GameObject;
		this.storeBlockTitle = GameObject.Find ("StoreBlockTitle");
		this.storeBlockTitle.GetComponent<TextMeshPro> ().text = "Acheter";
		this.newsfeedBlock = Instantiate (this.blockObject) as GameObject;
		this.tabs=new GameObject[3];
		for(int i=0;i<this.tabs.Length;i++)
		{
			this.tabs[i]=GameObject.Find ("Tab"+i);
		}
		this.tabs[0].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Alertes");
		this.tabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("News");
		this.tabs[2].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Amis");
		this.contents = new GameObject[3];
		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i]=GameObject.Find("Content"+i);
			this.contents[i].transform.FindChild("new").GetComponent<TextMeshPro>().text="Nouveau !";
		}
		this.challengeButtons = new GameObject[3];
		for(int i=0;i<this.challengeButtons.Length;i++)
		{
			this.challengeButtons[i]=GameObject.Find("ChallengeButton"+i);
		}
		this.cardsHalos=new GameObject[4];
		for(int i=0;i<this.cardsHalos.Length;i++)
		{
			this.cardsHalos[i]=GameObject.Find ("Card"+i);
		}
		this.newsfeedPagination = new GameObject[2];
		for(int i=0;i<this.newsfeedPagination.Length;i++)
		{
			this.newsfeedPagination[i]=GameObject.Find("NewsfeedPagination"+i);
		}
		this.deckChoices=new GameObject[12];
		for(int i=0;i<this.deckChoices.Length;i++)
		{
			this.deckChoices[i]=GameObject.Find("DeckChoice"+i);
			this.deckChoices[i].SetActive(false);
		}
		this.friendsOnline = new List<int> ();

		this.deckCards=new GameObject[4];
		for (int i=0;i<4;i++)
		{
			this.deckCards[i]=GameObject.Find("deckCard"+i);
			this.deckCards[i].AddComponent<NewCardHomePageController>();
			this.deckCards[i].SetActive(false);
		}
		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardHomePageController> ();

		this.endGamePopUp = GameObject.Find ("EndGamePopUp");
		this.endGamePopUp.SetActive (false);

	}
	public void resize()
	{
		if(this.isCardFocusedDisplayed)
		{
			this.hideCardFocused();
		}
		this.centralWindow = new Rect (MenuController.instance.widthScreen * 0.25f, 0.12f * MenuController.instance.heightScreen, MenuController.instance.widthScreen * 0.50f, 0.25f * MenuController.instance.heightScreen);
		this.centralWindow = new Rect (MenuController.instance.widthScreen * 0.25f, 0.12f * MenuController.instance.heightScreen, MenuController.instance.widthScreen * 0.50f, 0.25f * MenuController.instance.heightScreen);
		this.collectionPointsWindow=new Rect(MenuController.instance.widthScreen - MenuController.instance.widthScreen * 0.17f-5,0.1f * MenuController.instance.heightScreen+5,MenuController.instance.widthScreen * 0.17f,MenuController.instance.heightScreen * 0.1f);
		this.newSkillsWindow = new Rect (this.collectionPointsWindow.xMin, this.collectionPointsWindow.yMax + 5,this.collectionPointsWindow.width,MenuController.instance.heightScreen - 0.1f * MenuController.instance.heightScreen - 2 * 5 - this.collectionPointsWindow.height);
		this.newCardTypeWindow = new Rect (MenuController.instance.widthScreen * 0.25f, 0.12f * MenuController.instance.heightScreen, MenuController.instance.widthScreen * 0.50f, 0.25f * MenuController.instance.heightScreen);

		float reductionRatio=1f;
		float subMainFontScale = 1f;
		float mainFontScale = 1f;
		float buttons62Scale = 0.6f;
		float button62Width = 357f;
		float button62Height = 120f;

		float playBlockLeftMargin = MenuController.instance.leftMargin;
		float playBlockRightMargin = MenuController.instance.gapBetweenBlocks+MenuController.instance.rightMargin+(MenuController.instance.worldWidth-MenuController.instance.leftMargin-MenuController.instance.rightMargin-MenuController.instance.gapBetweenBlocks)/2f;
		float playBlockUpMargin = 6.45f;
		float playBlockDownMargin = MenuController.instance.downMargin;
		
		this.playBlock.GetComponent<NewBlockController> ().resize(playBlockLeftMargin,playBlockRightMargin,playBlockUpMargin,playBlockDownMargin);
		Vector3 playBlockUpperLeftPosition = this.playBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 playBlockUpperRightPosition = this.playBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 playBlockSize = this.playBlock.GetComponent<NewBlockController> ().getSize ();
		this.playBlockTitle.transform.position = new Vector3 (playBlockUpperLeftPosition.x + 0.3f, playBlockUpperLeftPosition.y - 0.2f, 0f);
		
		float gapBetweenCompetitionsBlock = 0.05f;
		float competitionsBlockSize = (playBlockSize.x - 0.6f - 2f * gapBetweenCompetitionsBlock) / 3f;

		this.friendlyGameTitle.transform.localScale=new Vector3(subMainFontScale,subMainFontScale,subMainFontScale);
		this.divisionGameTitle.transform.localScale=new Vector3(subMainFontScale,subMainFontScale,subMainFontScale);
		this.cupGameTitle.transform.localScale=new Vector3(subMainFontScale,subMainFontScale,subMainFontScale);

		if(this.friendlyGameTitle.GetComponent<TextMeshPro>().bounds.size.x>competitionsBlockSize)
		{
			reductionRatio=competitionsBlockSize/this.friendlyGameTitle.GetComponent<TextMeshPro>().bounds.size.x;
		}
		if(this.divisionGameTitle.GetComponent<TextMeshPro>().bounds.size.x>competitionsBlockSize)
		{
			if(reductionRatio>competitionsBlockSize/this.divisionGameTitle.GetComponent<TextMeshPro>().bounds.size.x)
			{
				reductionRatio=competitionsBlockSize/this.divisionGameTitle.GetComponent<TextMeshPro>().bounds.size.x;
			}
		}
		if(this.cupGameTitle.GetComponent<TextMeshPro>().bounds.size.x>competitionsBlockSize)
		{
			if(reductionRatio>competitionsBlockSize/this.cupGameTitle.GetComponent<TextMeshPro>().bounds.size.x)
			{
				reductionRatio=competitionsBlockSize/this.cupGameTitle.GetComponent<TextMeshPro>().bounds.size.x;
			}
		}

		float button62WorldWidth = reductionRatio *buttons62Scale* (button62Width / MenuController.instance.pixelPerUnit);
		float button62WorldHeight = button62WorldWidth * (button62Height / button62Width);

		this.playBlockTitle.transform.localScale = new Vector3(mainFontScale*reductionRatio,mainFontScale*reductionRatio,mainFontScale*reductionRatio);

		this.friendlyGameTitle.transform.localScale= new Vector3(subMainFontScale*reductionRatio,subMainFontScale*reductionRatio,subMainFontScale*reductionRatio);
		this.divisionGameTitle.transform.localScale= new Vector3(subMainFontScale*reductionRatio,subMainFontScale*reductionRatio,subMainFontScale*reductionRatio);
		this.cupGameTitle.transform.localScale= new Vector3(subMainFontScale*reductionRatio,subMainFontScale*reductionRatio,subMainFontScale*reductionRatio);

		this.friendlyGameTitle.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f, playBlockUpperLeftPosition.y - 1.2f, 0f);
		this.divisionGameTitle.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 1f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.2f, 0f);
		this.cupGameTitle.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 2f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.2f, 0f);
		
		this.friendlyGamePicture.transform.position = new Vector3 (0.3f + playBlockUpperLeftPosition.x + competitionsBlockSize / 2f, playBlockUpperLeftPosition.y - 1.95f, 0f);
		this.divisionGamePicture.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 1f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.95f, 0f);
		this.cupGamePicture.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 2f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 1.95f, 0f);
		
		this.friendlyGameButton.transform.position = new Vector3 (0.3f + playBlockUpperLeftPosition.x + competitionsBlockSize / 2f, playBlockUpperLeftPosition.y - 2.9f, 0f);
		this.divisionGameButton.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 1f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 2.9f, 0f);
		this.cupGameButton.transform.position = new Vector3 (0.3f+playBlockUpperLeftPosition.x + competitionsBlockSize / 2f + 2f * (competitionsBlockSize + gapBetweenCompetitionsBlock), playBlockUpperLeftPosition.y - 2.9f, 0f);
		
		this.friendlyGameButton.transform.localScale = new Vector3 (buttons62Scale * reductionRatio, buttons62Scale * reductionRatio, buttons62Scale * reductionRatio);
		this.divisionGameButton.transform.localScale = new Vector3 (buttons62Scale * reductionRatio, buttons62Scale * reductionRatio, buttons62Scale * reductionRatio);
		this.cupGameButton.transform.localScale = new Vector3 (buttons62Scale * reductionRatio, buttons62Scale * reductionRatio, buttons62Scale * reductionRatio);


		float deckBlockLeftMargin = playBlockLeftMargin;
		float deckBlockRightMargin =playBlockRightMargin;
		float deckBlockUpMargin = MenuController.instance.upMargin;
		float deckBlockDownMargin = MenuController.instance.worldHeight-playBlockUpMargin+MenuController.instance.gapBetweenBlocks;
		
		this.deckBlock.GetComponent<NewBlockController> ().resize(deckBlockLeftMargin,deckBlockRightMargin,deckBlockUpMargin,deckBlockDownMargin);
		Vector3 deckBlockUpperLeftPosition = this.deckBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 deckBlockUpperRightPosition = this.deckBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 deckBlockSize = this.deckBlock.GetComponent<NewBlockController> ().getSize ();
		this.deckBlockTitle.transform.position = new Vector3 (deckBlockUpperLeftPosition.x + 0.3f, deckBlockUpperLeftPosition.y - 0.2f, 0f);
		this.deckBlockTitle.transform.localScale = new Vector3(mainFontScale*reductionRatio,mainFontScale*reductionRatio,mainFontScale*reductionRatio);

		this.deckSelectionButton.transform.position = new Vector3 (deckBlockUpperRightPosition.x - 0.3f - button62WorldWidth / 2f, deckBlockUpperRightPosition.y - 1.2f, 0f);
		this.deckSelectionButton.transform.localScale = new Vector3 (buttons62Scale * reductionRatio, buttons62Scale * reductionRatio, buttons62Scale * reductionRatio);

		float deckChoiceWidth = 415f;
		float deckChoiceHeight = 90f;
		float deckChoiceScale = 0.6f * reductionRatio;
		float deckChoiceWorldWidth = deckChoiceScale * (deckChoiceWidth / MenuController.instance.pixelPerUnit);
		float deckChoiceWorldHeight = deckChoiceWorldWidth * deckChoiceHeight / deckChoiceWidth;

		for(int i=0;i<this.deckChoices.Length;i++)
		{
			this.deckChoices[i].transform.localScale=new Vector3(deckChoiceScale,deckChoiceScale,deckChoiceScale);
			this.deckChoices[i].transform.position=new Vector3(this.deckSelectionButton.transform.position.x,this.deckSelectionButton.transform.position.y-button62WorldHeight/2f-(i+0.5f)*deckChoiceWorldHeight+i*0.02f,-1f);
		}

		this.deckTitle.transform.position = new Vector3 (deckBlockUpperLeftPosition.x + 0.3f, deckSelectionButton.transform.position.y, 0f);
		this.deckTitle.transform.localScale= new Vector3(subMainFontScale*reductionRatio,subMainFontScale*reductionRatio,subMainFontScale*reductionRatio);

		float minimalGapBetweenCardHalo = 0.05f;
		float maximalCardWHaloScale = 1f;
		float cardHaloWorldWidth = (deckBlockSize.x - 0.6f - 3f*minimalGapBetweenCardHalo) / 4f;
		float cardHaloWidth = 200f;
		float cardHaloScale = cardHaloWorldWidth / (cardHaloWidth / MenuController.instance.pixelPerUnit);
		if(cardHaloScale>maximalCardWHaloScale)
		{
			cardHaloScale=maximalCardWHaloScale;
			cardHaloWorldWidth=cardHaloScale*(cardHaloWidth/MenuController.instance.pixelPerUnit);
		}
		float gapBetweenCardsHalo = (deckBlockSize.x - 0.6f - 4f * cardHaloWorldWidth) / 3f;
	
		float cardWidth = 720f;
		float cardHeight = 1004f;
		float cardWorldWidth = 0.97f * cardHaloWorldWidth;
		float cardWorldHeight = cardWorldWidth * (cardHeight / cardWidth);
		float cardScale = cardWorldWidth / (cardWidth / MenuController.instance.pixelPerUnit);

		this.deckCardsPosition=new Vector3[4];
		this.deckCardsArea=new Rect[4];
		
		for(int i=0;i<4;i++)
		{
			this.cardsHalos[i].transform.localScale=new Vector3(cardHaloScale,cardHaloScale,cardHaloScale);
			this.cardsHalos[i].transform.position=new Vector3(deckBlockUpperLeftPosition.x+0.3f+cardHaloWorldWidth/2f+i*(gapBetweenCardsHalo+cardHaloWorldWidth),deckBlockUpperRightPosition.y - 3f,0);
			this.deckCardsPosition[i]=this.cardsHalos[i].transform.position;
			this.deckCardsArea[i]=new Rect(this.deckCardsPosition[i].x-cardWorldWidth/2f,this.deckCardsPosition[i].y-cardWorldHeight/2f,cardWorldWidth,cardWorldHeight);
			this.deckCards[i].transform.position=this.deckCardsPosition[i];
			this.deckCards[i].transform.localScale=new Vector3(cardScale,cardScale,cardScale);
			this.deckCards[i].transform.GetComponent<NewCardHomePageController>().setId(i);
		}

		float storeBlockLeftMargin = playBlockRightMargin;
		float storeBlockRightMargin = playBlockLeftMargin;
		float storeBlockUpMargin = playBlockUpMargin;
		float storeBlockDownMargin = playBlockDownMargin;
		
		this.storeBlock.GetComponent<NewBlockController> ().resize(storeBlockLeftMargin,storeBlockRightMargin,storeBlockUpMargin,storeBlockDownMargin);
		Vector3 storeBlockUpperLeftPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 storeBlockUpperRightPosition = this.storeBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 storeBlockLowerRightPosition = this.storeBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 storeBlockSize = this.storeBlock.GetComponent<NewBlockController> ().getSize ();
		this.storeBlockTitle.transform.position = new Vector3 (storeBlockUpperLeftPosition.x + 0.3f, storeBlockUpperLeftPosition.y - 0.2f, 0f);
		this.storeBlockTitle.transform.localScale = new Vector3(mainFontScale*reductionRatio,mainFontScale*reductionRatio,mainFontScale*reductionRatio);

		float packPictureWidth = 375f;
		float packPictureHeight = 200f;
		float packButtonScale = 1.3f * reductionRatio;
		float packPictureWorldWidth = packButtonScale * (packPictureWidth / MenuController.instance.pixelPerUnit);
		float packPictureWorldHeight = packPictureWorldWidth * (packPictureHeight / packPictureWidth);
		this.packPicture.transform.localScale = new Vector3 (packButtonScale, packButtonScale, packButtonScale);
		this.packPicture.transform.position = new Vector3 (storeBlockLowerRightPosition.x - packPictureWorldWidth / 2f, storeBlockLowerRightPosition.y + packPictureWorldHeight/2f+0.25f, 0f);

		this.packButton.transform.localScale = new Vector3 (buttons62Scale * reductionRatio, buttons62Scale * reductionRatio, buttons62Scale * reductionRatio);
		this.packButton.transform.position = new Vector3 (0.3f + storeBlockUpperLeftPosition.x+button62WorldWidth/2f, storeBlockUpperLeftPosition.y - 2.9f, 0f);
		this.packTitle.transform.position = new Vector3 (storeBlockUpperLeftPosition.x + 0.3f, storeBlockUpperRightPosition.y - 1.2f, 0f);
		this.packTitle.transform.localScale= new Vector3(subMainFontScale*reductionRatio,subMainFontScale*reductionRatio,subMainFontScale*reductionRatio);
		this.packTitle.transform.GetComponent<TextMeshPro> ().textContainer.width = storeBlockSize.x / 2f;

		float newsfeedBlockLeftMargin = storeBlockLeftMargin;
		float newsfeedBlockRightMargin = storeBlockRightMargin;
		float newsfeedBlockUpMargin = deckBlockUpMargin+button62WorldHeight;
		float newsfeedBlockDownMargin = deckBlockDownMargin;

		this.newsfeedBlock.GetComponent<NewBlockController> ().resize(newsfeedBlockLeftMargin,newsfeedBlockRightMargin,newsfeedBlockUpMargin,newsfeedBlockDownMargin);
		Vector3 newsfeedBlockUpperLeftPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 newsfeedBlockUpperRightPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 newsfeedBlockLowerLeftPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 newsfeedBlockLowerRightPosition = this.newsfeedBlock.GetComponent<NewBlockController> ().getLowerRightCornerPosition ();
		Vector2 newsfeedBlockSize = this.newsfeedBlock.GetComponent<NewBlockController> ().getSize ();

		float gapBetweenSelectionsButtons = 0.02f;
		for(int i=0;i<this.tabs.Length;i++)
		{
			this.tabs[i].transform.position = new Vector3 (newsfeedBlockUpperLeftPosition.x + button62WorldWidth / 2f+ i*(button62WorldWidth+gapBetweenSelectionsButtons), newsfeedBlockUpperLeftPosition.y+button62WorldHeight/2f,0f);
			this.tabs[i].transform.localScale = new Vector3 (buttons62Scale*reductionRatio,buttons62Scale*reductionRatio,buttons62Scale*reductionRatio);

		}

		float lineWidth = 1500f;
		float thumbWidth = 63f;
		float thumbScale = 1.2f*reductionRatio;
		float thumbWorldWidth = thumbScale*(thumbWidth / MenuController.instance.pixelPerUnit);
		float thumbWorldHeight = thumbWorldWidth;

		Vector2 contentBlockSize = new Vector2 (newsfeedBlockSize.x - 0.6f, (newsfeedBlockSize.y - 0.3f - 0.6f)/this.contents.Length);
		float lineWorldWidth = contentBlockSize.x;
		float lineScale = lineWorldWidth / (lineWidth / MenuController.instance.pixelPerUnit);


		for(int i=0;i<this.contents.Length;i++)
		{
			this.contents[i].transform.position=new Vector3(newsfeedBlockUpperLeftPosition.x+0.3f+contentBlockSize.x/2f,newsfeedBlockUpperLeftPosition.y-0.3f-(i+1)*contentBlockSize.y,0f);
			this.contents[i].transform.FindChild("line").localScale=new Vector3(lineScale,1f,1f);
			this.contents[i].transform.FindChild("picture").localScale=new Vector3(thumbScale,thumbScale,thumbScale);
			this.contents[i].transform.FindChild("picture").localPosition=new Vector3(-contentBlockSize.x/2f+thumbWorldWidth/2f,(contentBlockSize.y-thumbWorldHeight)/2f+thumbWorldHeight/2f,0f);
			this.contents[i].transform.FindChild("username").localScale=new Vector3(reductionRatio,reductionRatio,reductionRatio);
			this.contents[i].transform.FindChild("username").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/2f)-0.1f-thumbWorldWidth;
			this.contents[i].transform.FindChild("username").localPosition=new Vector3(-contentBlockSize.x/2f+thumbWorldWidth+0.1f,contentBlockSize.y-(contentBlockSize.y-thumbWorldHeight)/2f,0f);
			this.contents[i].transform.FindChild("description").localScale=new Vector3(reductionRatio,reductionRatio,reductionRatio);
			this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().textContainer.width=0.75f*contentBlockSize.x-0.1f-thumbWorldWidth;
			this.contents[i].transform.FindChild("description").localPosition=new Vector3(-contentBlockSize.x/2f+thumbWorldWidth+0.1f,contentBlockSize.y/2f,0f);
			this.contents[i].transform.FindChild("date").localScale=new Vector3(reductionRatio,reductionRatio,reductionRatio);
			this.contents[i].transform.FindChild("date").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
			this.contents[i].transform.FindChild("date").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y-(contentBlockSize.y-thumbWorldHeight)/2f,0f);
			this.contents[i].transform.FindChild("new").localScale=new Vector3(reductionRatio,reductionRatio,reductionRatio);
			this.contents[i].transform.FindChild("new").GetComponent<TextMeshPro>().textContainer.width=(contentBlockSize.x/4f);
			this.contents[i].transform.FindChild("new").localPosition=new Vector3(contentBlockSize.x/2f,contentBlockSize.y/2f,0f);
		}

		for(int i=0;i<this.challengeButtons.Length;i++)
		{
			this.challengeButtons[i].transform.localScale = new Vector3 (buttons62Scale * reductionRatio, buttons62Scale * reductionRatio, buttons62Scale * reductionRatio);
			this.challengeButtons[i].transform.position=new Vector3(newsfeedBlockUpperRightPosition.x-0.3f-button62WorldWidth/2f,newsfeedBlockUpperRightPosition.y-0.3f-(i+0.5f)*contentBlockSize.y,0f);
		}

		float paginationNewsfeedWidth = 125f;
		float paginationScale = 0.3f * reductionRatio;
		float paginationNewsfeedWorldWidth = paginationScale*(paginationNewsfeedWidth / MenuController.instance.pixelPerUnit);

		this.newsfeedPagination [0].transform.localScale = new Vector3 (paginationScale, paginationScale, paginationScale);
		this.newsfeedPagination [1].transform.localScale = new Vector3 (paginationScale, paginationScale, paginationScale);

		this.newsfeedPagination [0].transform.position = new Vector3 (newsfeedBlockLowerLeftPosition.x + newsfeedBlockSize.x / 2 - 0.05f - paginationNewsfeedWorldWidth / 2f, newsfeedBlockLowerLeftPosition.y + 0.3f, 0f);
		this.newsfeedPagination [1].transform.position = new Vector3 (newsfeedBlockLowerLeftPosition.x + newsfeedBlockSize.x / 2 + 0.05f + paginationNewsfeedWorldWidth / 2f, newsfeedBlockLowerLeftPosition.y + 0.3f, 0f);


		float focusedCardHeight = 1402f*0.7287152f;
		float focusedCardWorldHeight = MenuController.instance.worldHeight - MenuController.instance.upMargin - MenuController.instance.downMargin;
		float focusedCardScale = focusedCardWorldHeight / (focusedCardHeight / MenuController.instance.pixelPerUnit);

		this.focusedCard.transform.localScale = new Vector3 (focusedCardScale, focusedCardScale, focusedCardScale);
		this.focusedCard.transform.position = new Vector3 (0f, -MenuController.instance.worldHeight/2f+MenuController.instance.downMargin+focusedCardWorldHeight/2f-0.22f, 0f);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setCentralWindow (this.centralWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setCollectionPointsWindow (this.collectionPointsWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setNewSkillsWindow (this.newSkillsWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setNewCardTypeWindow (this.newCardTypeWindow);
		this.focusedCard.SetActive (false);

		if(this.isTutorialLaunched)
		{
			this.tutorial.GetComponent<TutorialObjectController>().resize();
		}

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
			this.deckTitle.GetComponent<TextMeshPro> ().text = ("Aucune équipe créée").ToUpper();
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
				this.cardsHalos[i].GetComponent<SpriteRenderer>().color=new Color (155f / 255f, 220f / 255f, 1f);
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
			this.cardsHalos[i].GetComponent<SpriteRenderer>().color=new Color (1f,1f, 1f);
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
		print (model.notifications.Count);
		this.notificationsDisplayed = new List<int> ();
		for(int i =0;i<elementsPerPage;i++)
		{
			if(this.chosenPage*this.elementsPerPage+i<model.notifications.Count)
			{
				this.notificationsDisplayed.Add (this.chosenPage*this.elementsPerPage+i);
				this.contents[i].SetActive(true);
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=model.notifications[this.chosenPage*this.elementsPerPage+i].Content;
				this.contents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.notifications[this.chosenPage*this.elementsPerPage+i].SendingUser.idProfilePicture);
				this.contents[i].transform.FindChild("date").GetComponent<TextMeshPro>().text=model.notifications[this.chosenPage*this.elementsPerPage+i].Notification.Date.ToString("dd/MM/yyyy");
				this.contents[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.notifications[this.chosenPage*this.elementsPerPage+i].SendingUser.Username;
				if(!model.notifications[this.chosenPage*this.elementsPerPage+i].Notification.IsRead)
				{
					this.contents[i].transform.FindChild("new").gameObject.SetActive(true);
				}
				else
				{
					this.contents[i].transform.FindChild("new").gameObject.SetActive(false);
				}
			}
			else
			{
				this.contents[i].SetActive(false);
			}
		}
		this.manageNonReadsNotifications (firstLoad);
	}
	public void drawNews()
	{
		this.newsDisplayed = new List<int> ();
		for(int i =0;i<elementsPerPage;i++)
		{
			if(this.chosenPage*this.elementsPerPage+i<model.news.Count)
			{
				this.newsDisplayed.Add (this.chosenPage*this.elementsPerPage+i);
				this.contents[i].SetActive(true);
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=model.news[this.chosenPage*this.elementsPerPage+i].Content;
				this.contents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.news[this.chosenPage*this.elementsPerPage+i].User.idProfilePicture);
				this.contents[i].transform.FindChild("date").GetComponent<TextMeshPro>().text=model.news[this.chosenPage*this.elementsPerPage+i].News.Date.ToString("dd/MM/yyyy");
				this.contents[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.news[this.chosenPage*this.elementsPerPage+i].User.Username;

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
		for(int i =0;i<elementsPerPage;i++)
		{
			if(this.chosenPage*this.elementsPerPage+i<this.friendsToBeDisplayed.Count)
			{
				this.friendsDisplayed.Add (this.chosenPage*this.elementsPerPage+i);
				this.contents[i].SetActive(true);
				string connectionState="";
				Color connectionStateColor=new Color();
				switch(model.users[this.friendsToBeDisplayed[this.chosenPage*this.elementsPerPage+i]].OnlineStatus)
				{
				case 0:
					connectionState = "n'est pas en ligne";
					connectionStateColor=new Color(196f/255f,196f/255f,196f/255f);
					break;
				case 1:
					connectionState = "est disponible pour un défi !";
					connectionStateColor=new Color(75f/255f,163f/255f,174f/255f);
					break;
				case 2:
					connectionState = "est entrain de jouer";
					connectionStateColor=new Color(218f/255f,70f/255f,70f/255f);
					break;
				}
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().text=connectionState;
				this.contents[i].transform.FindChild("description").GetComponent<TextMeshPro>().color=connectionStateColor;
				this.contents[i].transform.FindChild("picture").GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnThumbPicture(model.users[this.friendsToBeDisplayed[this.chosenPage*this.elementsPerPage+i]].idProfilePicture);
				this.contents[i].transform.FindChild("username").GetComponent<TextMeshPro>().text=model.users[this.friendsToBeDisplayed[this.chosenPage*this.elementsPerPage+i]].Username;
			}
			else
			{
				this.contents[i].SetActive(false);
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
		if(model.currentDivision.GamesPlayed>0)
		{
			divisionState="Rejoindre";
		}
		else
		{
			divisionState="Démarrer";
		}
		this.divisionGameButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = divisionState.ToUpper ();

		string cupState;
		if(model.currentCup.GamesPlayed>0)
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
	public IEnumerator endTutorial()
	{
		MenuController.instance.setTutorialLaunched (false);
		//PhotonNetwork.Disconnect();
		if(TutorialObjectController.instance.getSequenceID()==1)
		{
			MenuController.instance.displayLoadingScreen();
			yield return StartCoroutine (model.player.setTutorialStep (2));
			Application.LoadLevel ("newMyGame");
		}
		else if(TutorialObjectController.instance.getSequenceID()==3)
		{
			MenuController.instance.displayLoadingScreen();
			yield return StartCoroutine (model.player.setTutorialStep (5));
			Application.LoadLevel ("newStore");
		}
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
		if(this.activeTab == 2 && !this.isCardFocusedDisplayed && this.chosenPage==0)
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
}
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class newMyGameController : MonoBehaviour
{
	public static newMyGameController instance;
	private NewMyGameModel model;

	public GameObject blockObject;
	public GameObject cardObject;
	public Texture2D[] cursorTextures;
	public GUISkin popUpSkin;

	private GameObject menu;
	private GameObject tutorial;
	private GameObject deckBlock;
	private GameObject deckBlockTitle;
	private GameObject deckSelectionButton;
	private GameObject deckCreationButton;
	private GameObject deckDeletionButton;
	private GameObject deckRenameButton;
	private GameObject deckTitle;
	private GameObject[] deckCards;
	private GameObject[] cardsHalos;
	private GameObject[] deckChoices;

	private GameObject cardsBlock;
	private GameObject cardsBlockTitle;
	private GameObject[] cards;
	private GameObject cardsPaginationButtons;
	private GameObject cardsPaginationLine;
	private GameObject cardsScrollLine;
	private GameObject cardsNumberTitle;

	private GameObject filtersBlock;
	private GameObject filtersBlockTitle;
	private GameObject[] cardsTypeFilters;
	private GameObject skillSearchBarTitle;
	private GameObject skillSearchBar;
	private GameObject[] skillChoices;
	private GameObject valueFilterTitle;
	private GameObject[] valueFilters;
	private GameObject cardTypeFilterTitle;
	private GameObject[] cursors;
	private GameObject[] sortButtons;
	private GameObject filterButton;
	private GameObject slideLeftButton;

	private GameObject focusedCard;

	private GameObject mainCamera;
	private GameObject sceneCamera;
	private GameObject upperScrollCamera;
	private GameObject lowerScrollCamera;
	private GameObject tutorialCamera;
	private GameObject backgroundCamera;

	private int focusedCardIndex;
	private bool isCardFocusedDisplayed;

	private Rect centralWindow;

	private bool isSearchingSkill;
	private bool isSearchingDeck;
	private bool isMouseOnSelectDeckButton;
	private bool isSkillChosen;
	private string valueSkill;
	private IList<int> skillsDisplayed;
	
	private int powerVal;
	private int lifeVal;
	private int attackVal;
	private int quicknessVal;

	private IList<int> filtersCardType;
	private int sortingOrder;

	private IList<int> cardsToBeDisplayed;
	private IList<int> cardsDisplayed;
	private IList<int> decksDisplayed;
	private int[] deckCardsDisplayed;
	private int deckDisplayed;
	
	private int nbLines;
	private int cardsPerLine;
	private Pagination cardsPagination;

	private GameObject newDeckPopUp;
	private bool newDeckPopUpDisplayed;
	private GameObject editDeckPopUp;
	private bool editDeckPopUpDisplayed;
	private GameObject deleteDeckPopUp;
	private bool deleteDeckPopUpDisplayed;

	private bool isDragging;
	private bool isLeftClicked;
	private bool isHovering;
	private float clickInterval;
	private int idCardClicked;
	private bool isDeckCardClicked;
	private Vector3[] cardsPosition;
	private Vector3[] deckCardsPosition;
	private Rect[] deckCardsArea;
	private bool[] deckCardsAreaHovered;
	private bool isHoveringDeckArea;
	private Rect cardsArea;
	private Texture2D cursorTexture;
	
	private bool isSceneLoaded;
	private bool isScrolling;
	private bool toScrollCards;
	private float lowerScrollCameraIntermediatePosition;

	private bool toSlideRight;
	private bool toSlideLeft;
	private bool filtersDisplayed;
	private bool isSlidingCursors;
	private bool mainContentDisplayed;
	private float scrollIntersection;

	private float filtersPositionX;
	private float mainContentPositionX;

	void Update()
	{	
		if (Input.touchCount == 1 && this.isSceneLoaded) 
		{
			if(Mathf.Abs(Input.touches[0].deltaPosition.y)>1f && Mathf.Abs(Input.touches[0].deltaPosition.y)>Mathf.Abs(Input.touches[0].deltaPosition.x))
			{
				this.isLeftClicked=false;
			}
			else if(Input.touches[0].deltaPosition.x<-15f && !this.isCardFocusedDisplayed && !this.isDragging && !this.isSlidingCursors)
			{
				this.isLeftClicked=false;
				if(this.mainContentDisplayed || this.toSlideLeft)
				{
					this.slideRight();
				}
			}
			else if(Input.touches[0].deltaPosition.x>15f && !this.isCardFocusedDisplayed && !this.isDragging && !this.isSlidingCursors)
			{
				this.isLeftClicked=false;
				if(this.filtersDisplayed || this.toSlideRight)
				{
					this.slideLeft();
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
		if(isSearchingSkill)
		{
			if(this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().getText().ToLower()!=this.valueSkill.ToLower()	)
			{
				this.valueSkill=this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().getText();
				this.setSkillAutocompletion();
			}
			if(!this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().getIsBeingUsed())
			{
					this.isSearchingSkill=false;
					this.cleanSkillAutocompletion();
					this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().setText("Rechercher");
					this.valueSkill="Rechercher";
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
		if(ApplicationDesignRules.isMobileScreen && this.isSceneLoaded && !this.isLeftClicked && !this.isDragging && this.mainContentDisplayed && !this.isCardFocusedDisplayed)
		{
			if(!toScrollCards)
			{
				isScrolling = this.upperScrollCamera.GetComponent<ScrollingController>().ScrollController();
				isScrolling = this.lowerScrollCamera.GetComponent<ScrollingController>().ScrollController();

				if(this.upperScrollCamera.GetComponent<ScrollingController>().isEndPosition())
				{
					Vector3 cardsCameraPosition = this.lowerScrollCamera.transform.position;
					cardsCameraPosition.y=this.lowerScrollCameraIntermediatePosition;
					this.lowerScrollCamera.transform.position=cardsCameraPosition;
					this.toScrollCards=true;
					this.cardsScrollLine.SetActive(true);
				}
			}
			else
			{
				isScrolling = this.lowerScrollCamera.GetComponent<ScrollingController>().ScrollController();
				if(this.lowerScrollCamera.transform.position.y>this.lowerScrollCameraIntermediatePosition)
				{
					Vector3 cardsCameraPosition = this.lowerScrollCamera.transform.position;
					cardsCameraPosition.y=this.lowerScrollCameraIntermediatePosition;
					this.lowerScrollCamera.transform.position=cardsCameraPosition;
					this.toScrollCards=false;
					this.cardsScrollLine.SetActive(false);
				}
			}
		}
		if(toSlideRight || toSlideLeft)
		{
			Vector3 upperScrollCameraPosition = this.upperScrollCamera.transform.position;
			Vector3 lowerScrollCameraPosition = this.lowerScrollCamera.transform.position;
			float camerasXPosition = upperScrollCameraPosition.x;
			if(toSlideRight)
			{
				camerasXPosition=camerasXPosition+Time.deltaTime*40f;
				if(camerasXPosition>this.filtersPositionX)
				{
					camerasXPosition=this.filtersPositionX;
					this.toSlideRight=false;
					this.filtersDisplayed=true;
					this.skillSearchBar.SetActive(true);
					TutorialObjectController.instance.tutorialTrackPoint();
				}
			}
			else if(toSlideLeft)
			{
				camerasXPosition=camerasXPosition-Time.deltaTime*40f;
				if(camerasXPosition<this.mainContentPositionX)
				{
					camerasXPosition=this.mainContentPositionX;
					this.toSlideLeft=false;
					this.mainContentDisplayed=true;
				}
			}
			upperScrollCameraPosition.x=camerasXPosition;
			lowerScrollCameraPosition.x=camerasXPosition;
			this.upperScrollCamera.transform.position=upperScrollCameraPosition;
			this.lowerScrollCamera.transform.position=lowerScrollCameraPosition;
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewMyGameModel ();
		this.skillSearchBar = GameObject.Find ("SkillSearchBar");
		this.sortingOrder = -1;
		this.scrollIntersection = 3.8f;
		this.mainContentDisplayed = true;
		this.initializeScene ();
		this.startMenuInitialization ();
	}
	private void startMenuInitialization()
	{
		this.menu = GameObject.Find ("Menu");
		this.menu.AddComponent<MyGameMenuController> ();
	}
	public void endMenuInitialization()
	{
		this.startTutorialInitialization ();
	}
	private void startTutorialInitialization()
	{
		this.tutorial = GameObject.Find ("Tutorial");
		this.tutorial.AddComponent<MyGameTutorialController>();
	}
	public void endTutorialInitialization()
	{
		StartCoroutine (this.initialization ());
	}
	public IEnumerator initialization()
	{
		this.resize ();
		yield return StartCoroutine (model.initializeMyGame ());
		this.retrieveDefaultDeck ();
		this.initializeDecks ();
		this.initializeCards ();
		MenuController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
		if(model.player.TutorialStep!=-1)
		{
			TutorialObjectController.instance.startTutorial(model.player.TutorialStep,model.player.displayTutorial);
		}
	}
	private void initializeDecks()
	{
		this.retrieveDecksList ();
		this.drawDeckCards ();
	}
	public void initializeCards()
	{
		this.resetFiltersValue ();
		this.cardsPagination.chosenPage = 0;
		this.applyFilters ();
	}
	private void applyFilters()
	{
		this.computeFilters ();
		this.cardsPagination.totalElements= this.cardsToBeDisplayed.Count;
		this.cardsPaginationButtons.GetComponent<newMyGamePaginationController> ().p = cardsPagination;
		this.cardsPaginationButtons.GetComponent<newMyGamePaginationController> ().setPagination ();
		this.drawPaginationNumber ();
		this.drawCards ();
	}
	public void drawPaginationNumber()
	{
		if(cardsPagination.totalElements>0)
		{
			this.cardsNumberTitle.GetComponent<TextMeshPro>().text=("carte " +this.cardsPagination.elementDebut+" à "+this.cardsPagination.elementFin+" sur "+this.cardsPagination.totalElements ).ToUpper();
		}
		else
		{
			this.cardsNumberTitle.GetComponent<TextMeshPro>().text="aucune carte à afficher".ToUpper();
		}
	}
	public void paginationHandler()
	{
		this.drawPaginationNumber ();
		this.drawCards ();
		if(ApplicationDesignRules.isMobileScreen && toScrollCards)
		{
			Vector3 lowerScrollCameraPosition = this.lowerScrollCamera.transform.position;
			lowerScrollCameraPosition.y=this.lowerScrollCameraIntermediatePosition;
			this.lowerScrollCamera.transform.position=lowerScrollCameraPosition;
		}
	}
	public void initializeScene()
	{
		this.deckBlock = Instantiate (this.blockObject) as GameObject;
		this.deckBlockTitle = GameObject.Find ("DeckBlockTitle");
		this.deckBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.deckBlockTitle.GetComponent<TextMeshPro> ().text = "Mes armées";
		this.deckSelectionButton = GameObject.Find ("DeckSelectionButton");
		this.deckSelectionButton.AddComponent<newMyGameDeckSelectionButtonController> ();
		this.deckCreationButton = GameObject.Find ("DeckCreationButton");
		this.deckCreationButton.AddComponent<newMyGameDeckCreatioButtonController> ();
		this.deckDeletionButton = GameObject.Find ("DeckDeletionButton");
		this.deckDeletionButton.AddComponent<newMyGameDeckDeletionButtonController> ();
		this.deckRenameButton = GameObject.Find ("DeckRenameButton");
		this.deckRenameButton.AddComponent<newMyGameDeckRenameButtonController> ();
		this.deckTitle = GameObject.Find ("DeckTitle");
		this.deckTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.deckChoices=new GameObject[12];
		for(int i=0;i<this.deckChoices.Length;i++)
		{
			this.deckChoices[i]=GameObject.Find("DeckChoice"+i);
			this.deckChoices[i].AddComponent<newMyGameDeckChoiceController>();
			this.deckChoices[i].GetComponent<newMyGameDeckChoiceController>().setId(i);
			this.deckChoices[i].SetActive(false);
		}
		this.deckCards=new GameObject[4];
		for (int i=0;i<4;i++)
		{
			this.deckCards[i]=GameObject.Find("deckCard"+i);
			this.deckCards[i].AddComponent<NewCardMyGameController>();
			this.deckCards[i].SetActive(false);
		}
		this.cardsHalos=new GameObject[4];
		for(int i=0;i<this.cardsHalos.Length;i++)
		{
			this.cardsHalos[i]=GameObject.Find ("CardHalo"+i);
			this.cardsHalos[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
		this.cardsHalos [0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "CAPITAINE \n1er à jouer";
		this.cardsHalos [1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "LIEUTENANT \n2ème à jouer";
		this.cardsHalos [2].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "SERGENT \n3ème à jouer";
		this.cardsHalos [3].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "SOLDAT \n4ème à jouer";
		this.cardsBlock = Instantiate (this.blockObject) as GameObject;
		this.cardsBlockTitle = GameObject.Find ("CardsBlockTitle");
		this.cardsBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.cardsBlockTitle.GetComponent<TextMeshPro> ().text = "Mes unités";
		this.cardsNumberTitle = GameObject.Find ("CardsNumberTitle");
		this.cardsNumberTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.cards=new GameObject[0];

		this.cardsPaginationButtons = GameObject.Find("Pagination");
		this.cardsPaginationButtons.AddComponent<newMyGamePaginationController> ();
		this.cardsPaginationButtons.GetComponent<newMyGamePaginationController> ().initialize ();
		this.cardsPaginationLine = GameObject.Find ("CardsPaginationLine");
		this.cardsPaginationLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.cardsScrollLine = GameObject.Find ("CardsScrollLine");
		this.cardsScrollLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;

		this.filtersBlock = Instantiate (this.blockObject) as GameObject;
		this.filtersBlockTitle = GameObject.Find ("FiltersBlockTitle");
		this.filtersBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.filtersBlockTitle.GetComponent<TextMeshPro> ().text = "Filtrer";
		this.cardsTypeFilters = new GameObject[10];
		for(int i=0;i<this.cardsTypeFilters.Length;i++)
		{
			this.cardsTypeFilters[i]=GameObject.Find("CardTypeFilter"+i);
			this.cardsTypeFilters[i].AddComponent<newMyGameCardTypeFilterController>();
			this.cardsTypeFilters[i].GetComponent<newMyGameCardTypeFilterController>().setId(i);
		}
		this.valueFilters=new GameObject[3];
		for(int i=0;i<this.valueFilters.Length;i++)
		{
			this.valueFilters[i]=GameObject.Find ("ValueFilter"+i);
		}
		this.skillSearchBarTitle = GameObject.Find ("SkillSearchTitle");
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().text = "Compétence".ToUpper ();
		this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController> ().setText ("Rechercher");
		this.skillChoices=new GameObject[3];
		for(int i=0;i<this.skillChoices.Length;i++)
		{
			this.skillChoices[i]=GameObject.Find("SkillChoice"+i);
			this.skillChoices[i].AddComponent<newMyGameSkillChoiceController>();
			this.skillChoices[i].GetComponent<newMyGameSkillChoiceController>().setId(i);
			this.skillChoices[i].SetActive(false);
		}
		this.cardTypeFilterTitle = GameObject.Find ("CardTypeFilterTitle");
		this.cardTypeFilterTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.cardTypeFilterTitle.GetComponent<TextMeshPro> ().text = "Faction".ToUpper ();
		this.valueFilterTitle = GameObject.Find ("ValueFilterTitle");
		this.valueFilterTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.valueFilterTitle.GetComponent<TextMeshPro> ().text = "Attribut".ToUpper ();

		this.cursors=new GameObject[this.valueFilters.Length];
		for (int i=0;i<this.valueFilters.Length;i++)
		{
			this.cursors[i]=this.valueFilters[i].transform.FindChild("Slider").FindChild("Cursor").gameObject;
			this.cursors[i].AddComponent<newMyGameCursorController>();
			this.cursors[i].GetComponent<newMyGameCursorController>().setId(i);
		}
		this.sortButtons=new GameObject[2*this.valueFilters.Length];
		for (int i=0;i<this.valueFilters.Length;i++)
		{
			this.sortButtons[i*2]=this.valueFilters[i].transform.FindChild("Sort0").gameObject;
			this.sortButtons[i*2+1]=this.valueFilters[i].transform.FindChild("Sort1").gameObject;
		}
		for(int i=0;i<this.sortButtons.Length;i++)
		{
			this.sortButtons[i].AddComponent<newMyGameSortButtonController>();
			this.sortButtons[i].GetComponent<newMyGameSortButtonController>().setId(i);
		}
		this.filterButton = GameObject.Find ("FilterButton");
		this.filterButton.AddComponent<newMyGameFilterButtonController> ();
		this.slideLeftButton = GameObject.Find ("SlideLeftButton");
		this.slideLeftButton.AddComponent<newMyGameSlideLeftButtonController> ();

		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardMyGameController> ();
		this.focusedCard.SetActive (false);
		this.mainCamera = gameObject;
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.upperScrollCamera = GameObject.Find ("UpperScrollCamera");
		this.upperScrollCamera.AddComponent<ScrollingController> ();
		this.lowerScrollCamera = GameObject.Find ("LowerScrollCamera");
		this.lowerScrollCamera.AddComponent<ScrollingController> ();
		this.tutorialCamera = GameObject.Find ("TutorialCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
		this.newDeckPopUp = GameObject.Find ("newDeckPopUp");
		this.newDeckPopUp.SetActive (false);
		this.editDeckPopUp = GameObject.Find ("editDeckPopUp");
		this.editDeckPopUp.SetActive (false);
		this.deleteDeckPopUp = GameObject.Find ("deleteDeckPopUp");
		this.deleteDeckPopUp.SetActive (false);
	}
	private void resetFiltersValue()
	{
		this.powerVal = 0;
		this.lifeVal = 0;
		this.attackVal = 0;
		this.quicknessVal = 0;
		this.valueFilters[0].transform.FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(powerVal);
		this.valueFilters[0].transform.FindChild("Icon").GetComponent<SpriteRenderer>().color=getColorFilterIcon(powerVal);
		this.valueFilters[1].transform.FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(attackVal);
		this.valueFilters[1].transform.FindChild("Icon").GetComponent<SpriteRenderer>().color=getColorFilterIcon(attackVal);
		this.valueFilters[2].transform.FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(lifeVal);
		this.valueFilters[2].transform.FindChild("Icon").GetComponent<SpriteRenderer>().color=getColorFilterIcon(lifeVal);
		//this.valueFilters[3].transform.FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(quicknessVal);
		//this.valueFilters[3].transform.FindChild("Icon").GetComponent<SpriteRenderer>().color=getColorFilterIcon(quicknessVal);

		for(int i=0;i<this.cursors.Length;i++)
		{
			Vector3 cursorPosition = this.cursors [i].transform.localPosition;
			cursorPosition.x=-0.67f;
			this.cursors[i].transform.localPosition=cursorPosition;
		}

		this.filtersCardType = new List<int> ();
		for(int i=0;i<this.cardsTypeFilters.Length;i++)
		{
			this.cardsTypeFilters[i].GetComponent<newMyGameCardTypeFilterController>().reset();
		}
		this.valueSkill = "";
		this.isSkillChosen = false;

		this.cleanSkillAutocompletion ();
		this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().setText("Rechercher");
		if(this.sortingOrder!=-1)
		{
			this.sortButtons[this.sortingOrder].GetComponent<newMyGameSortButtonController>().setIsSelected(false);
			this.sortButtons[this.sortingOrder].GetComponent<newMyGameSortButtonController>().reset ();
			this.sortingOrder = -1;
		}
	}
	private void retrieveDefaultDeck()
	{
		this.decksDisplayed=new List<int>();
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
		bool isADeckCompleted = false;
		this.decksDisplayed=new List<int>();
		if(this.deckDisplayed!=-1)
		{
			for(int i=0;i<model.decks.Count;i++)
			{
				if(i!=this.deckDisplayed)
				{
					this.decksDisplayed.Add (i);
				}
				if(model.decks[i].cards.Count==ApplicationModel.nbCardsByDeck)
				{
					isADeckCompleted=true;
				}
			}
		}
		ApplicationModel.hasDeck = isADeckCompleted;
	}
	public void resize()
	{
		float cardsBlockLeftMargin;
		float cardsBlockUpMargin;
		float cardsBlockHeight;
		
		float deckBlockLeftMargin;
		float deckBlockUpMargin;
		float deckBlockHeight;
		
		float filtersBlockLeftMargin;
		float filtersBlockUpMargin;
		float filtersBlockHeight;

		float deckCardsPositionY;
		float firstLineCardsPositionY;


		this.mainCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.mainCamera.transform.position = ApplicationDesignRules.mainCameraPosition;
		this.sceneCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.tutorialCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.cameraSize;
		this.tutorialCamera.transform.position = ApplicationDesignRules.tutorialCameraPositiion;
		this.backgroundCamera.GetComponent<Camera> ().orthographicSize = ApplicationDesignRules.backgroundCameraSize;
		this.backgroundCamera.transform.position = ApplicationDesignRules.backgroundCameraPosition;
		this.backgroundCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.tutorialCamera.GetComponent<Camera> ().rect = new Rect (0f, 0f, 1f, 1f);
		this.sceneCamera.GetComponent<Camera> ().rect = new Rect (0f,0f,1f,1f);
		this.mainCamera.GetComponent<Camera>().rect= new Rect (0f,0f,1f,1f);
		
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.cardsPerLine = 4;
			this.nbLines = 25;
			cardsBlockHeight=2f+this.nbLines*(ApplicationDesignRules.cardWorldSize.y+ApplicationDesignRules.gapBetweenCardsLine);

			deckBlockHeight=3.3f;
			deckBlockLeftMargin=ApplicationDesignRules.leftMargin;
			deckBlockUpMargin=0f;
			deckCardsPositionY = 2.2f;
			
			firstLineCardsPositionY=2.2f;
			cardsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			cardsBlockUpMargin=deckBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+deckBlockHeight;
			
			filtersBlockHeight=ApplicationDesignRules.viewHeight;
			filtersBlockLeftMargin=ApplicationDesignRules.worldWidth+ApplicationDesignRules.leftMargin;
			filtersBlockUpMargin=0f;

			this.upperScrollCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.worldHeight-ApplicationDesignRules.upMargin-this.scrollIntersection)/ApplicationDesignRules.worldHeight,1f,(this.scrollIntersection)/ApplicationDesignRules.worldHeight);this.upperScrollCamera.GetComponent<Camera> ().orthographicSize = this.scrollIntersection/2f;
			this.upperScrollCamera.GetComponent<ScrollingController> ().setViewHeight(this.scrollIntersection);
			this.upperScrollCamera.GetComponent<ScrollingController> ().setContentHeight(this.scrollIntersection+0.7f);
			this.upperScrollCamera.transform.position = new Vector3 (0f, ApplicationDesignRules.worldHeight/2f-this.scrollIntersection/2f, -10f);
			this.upperScrollCamera.GetComponent<ScrollingController> ().setStartPositionY (this.upperScrollCamera.transform.position.y);
			this.upperScrollCamera.GetComponent<ScrollingController>().setEndPositionY();

			this.lowerScrollCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.downMargin)/ApplicationDesignRules.worldHeight,1f,(ApplicationDesignRules.viewHeight-this.scrollIntersection)/ApplicationDesignRules.worldHeight);
			this.lowerScrollCamera.GetComponent<Camera> ().orthographicSize = (ApplicationDesignRules.viewHeight-this.scrollIntersection)/2f;
			this.lowerScrollCamera.GetComponent<ScrollingController> ().setViewHeight(ApplicationDesignRules.viewHeight-this.scrollIntersection);
			this.lowerScrollCamera.transform.position = new Vector3 (0f, ApplicationDesignRules.worldHeight/2f-this.scrollIntersection-(ApplicationDesignRules.viewHeight-this.scrollIntersection)/2f, -10f);
			this.lowerScrollCameraIntermediatePosition=this.lowerScrollCamera.transform.position.y-(this.upperScrollCamera.GetComponent<ScrollingController>().getContentHeight()-this.upperScrollCamera.GetComponent<ScrollingController>().getViewHeight());
			this.lowerScrollCamera.GetComponent<ScrollingController>().setStartPositionY(this.lowerScrollCamera.transform.position.y);

			this.filterButton.SetActive(true);
			this.slideLeftButton.SetActive(true);
			this.cardsPaginationLine.SetActive(false);
			this.lowerScrollCamera.SetActive(true);
			this.toScrollCards=false;
			this.toSlideLeft=false;
			this.toSlideRight=false;
			this.mainContentDisplayed=true;
			this.skillSearchBar.SetActive(false);

			if(isCardFocusedDisplayed)
			{
				this.lowerScrollCamera.SetActive(false);
				this.upperScrollCamera.SetActive(false);
				this.sceneCamera.SetActive(true);
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraFocusedCardPosition;
			}
			else
			{
				this.lowerScrollCamera.SetActive(true);
				this.upperScrollCamera.SetActive(true);
				this.sceneCamera.SetActive(false);
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraStandardPosition;
			}
		}
		else
		{
			cardsBlockHeight=ApplicationDesignRules.largeBlockHeight;

			deckBlockHeight=ApplicationDesignRules.mediumBlockHeight;
			deckBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			deckBlockUpMargin=ApplicationDesignRules.upMargin;
			deckCardsPositionY = 3f;

			filtersBlockHeight=ApplicationDesignRules.smallBlockHeight;
			filtersBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			filtersBlockUpMargin=deckBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+deckBlockHeight;

			firstLineCardsPositionY=3f;
			cardsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			cardsBlockUpMargin=ApplicationDesignRules.upMargin;

			this.cardsPerLine = 4;
			this.nbLines = 2;
			
			this.filterButton.SetActive(false);
			this.slideLeftButton.SetActive(false);
			this.sceneCamera.SetActive(true);
			this.lowerScrollCamera.SetActive(false);
			this.upperScrollCamera.SetActive(false);
			this.cardsPaginationLine.SetActive(true);
			this.skillSearchBar.SetActive(true);

			if(isCardFocusedDisplayed)
			{
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraFocusedCardPosition;
			}
			else
			{
				this.sceneCamera.transform.position = ApplicationDesignRules.sceneCameraStandardPosition;
			}
		}

		this.cardsPagination = new Pagination ();
		this.cardsPagination.chosenPage = 0;
		this.cardsPagination.nbElementsPerPage = this.cardsPerLine * this.nbLines;

		this.filtersBlock.GetComponent<NewBlockController> ().resize(filtersBlockLeftMargin,filtersBlockUpMargin,ApplicationDesignRules.blockWidth,filtersBlockHeight);
		Vector3 filtersBlockUpperLeftPosition = this.filtersBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 filtersBlockUpperRightPosition = this.filtersBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector3 filtersBlockLowerLeftPosition = this.filtersBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 filtersBlockSize = this.filtersBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 filtersBlockOrigin = this.filtersBlock.GetComponent<NewBlockController> ().getOriginPosition ();

		float gapBetweenSubFiltersBlock = 0.05f;
		float filtersSubBlockSize = (filtersBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing - 2f * gapBetweenSubFiltersBlock) / 3f;

		this.filtersBlockTitle.transform.position = new Vector3 (filtersBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, filtersBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.filtersBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;

		this.cardTypeFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.skillSearchBarTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.skillSearchBar.transform.localScale = ApplicationDesignRules.inputTextScale;

		this.valueFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.slideLeftButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.slideLeftButton.transform.position = new Vector3 (filtersBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.roundButtonWorldSize.x/2f, filtersBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		this.centralWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.25f * ApplicationDesignRules.heightScreen);
	
		this.deckBlock.GetComponent<NewBlockController> ().resize(deckBlockLeftMargin,deckBlockUpMargin,ApplicationDesignRules.blockWidth,deckBlockHeight);
		Vector3 deckBlockUpperLeftPosition = this.deckBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 deckBlockUpperRightPosition = this.deckBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 deckBlockSize = this.deckBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 deckBlockOrigin = this.deckBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.deckBlockTitle.transform.position = new Vector3 (deckBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, deckBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.deckBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		this.deckBlockTitle.GetComponent<TextContainer> ().width = deckBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing-3f*ApplicationDesignRules.roundButtonWorldSize.x;

		float gapBetweenDecksButton = 0.05f;

		this.deckCreationButton.transform.position = new Vector3 (deckBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, deckBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y/2f, 0f);
		this.deckCreationButton.transform.localScale = ApplicationDesignRules.roundButtonScale;

		this.deckSelectionButton.transform.position = new Vector3 (deckBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 1.5f*ApplicationDesignRules.roundButtonWorldSize.x, deckBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y/2f, 0f);
		this.deckSelectionButton.transform.localScale = ApplicationDesignRules.roundButtonScale;

		this.deckRenameButton.transform.position = new Vector3 (deckBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 2.5f*ApplicationDesignRules.roundButtonWorldSize.x, deckBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y/2f, 0f);
		this.deckRenameButton.transform.localScale = ApplicationDesignRules.roundButtonScale;

		this.deckDeletionButton.transform.position = new Vector3 (deckBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 3.5f*ApplicationDesignRules.roundButtonWorldSize.x, deckBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y/2f, 0f);
		this.deckDeletionButton.transform.localScale = ApplicationDesignRules.roundButtonScale;

		for(int i=0;i<this.deckChoices.Length;i++)
		{
			this.deckChoices[i].transform.localScale=ApplicationDesignRules.listElementScale;
			this.deckChoices[i].transform.position=new Vector3(this.deckSelectionButton.transform.position.x,this.deckSelectionButton.transform.position.y-ApplicationDesignRules.button61WorldSize.y/2f-(i+0.5f)*ApplicationDesignRules.listElementWorldSize.y+i*0.02f,-1f);
		}
		
		this.deckTitle.transform.position = new Vector3 (deckBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, deckBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
		this.deckTitle.GetComponent<TextContainer> ().width = deckBlockSize.x -2f*ApplicationDesignRules.blockHorizontalSpacing;
		this.deckTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		float gapBetweenCards = (deckBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing - 4f * ApplicationDesignRules.cardWorldSize.x) / 3f;

		this.deckCardsPosition=new Vector3[this.deckCards.Length];
		this.deckCardsArea=new Rect[this.deckCards.Length];
		this.deckCardsAreaHovered=new bool[this.deckCards.Length];
		
		for(int i=0;i<this.cardsHalos.Length;i++)
		{
			this.deckCardsPosition[i]=new Vector3(deckBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+ApplicationDesignRules.cardWorldSize.x/2f+i*(gapBetweenCards+ApplicationDesignRules.cardWorldSize.x),deckBlockUpperRightPosition.y - deckCardsPositionY,0);
			this.deckCards[i].transform.position=this.deckCardsPosition[i];
			this.deckCards[i].transform.localScale=ApplicationDesignRules.cardScale;
			this.deckCards[i].transform.GetComponent<NewCardMyGameController>().setId(i,true);
			this.cardsHalos[i].transform.position=this.deckCardsPosition[i];
			this.cardsHalos[i].transform.localScale=ApplicationDesignRules.cardHaloScale;
			this.deckCardsArea[i]=new Rect(this.cardsHalos[i].transform.position.x-ApplicationDesignRules.cardHaloWorldSize.x/2f-gapBetweenCards/2f,this.cardsHalos[i].transform.position.y-ApplicationDesignRules.cardHaloWorldSize.y/2f,ApplicationDesignRules.cardHaloWorldSize.x+gapBetweenCards,ApplicationDesignRules.cardHaloWorldSize.y);
			this.deckCardsAreaHovered[i]=false;
		}

		this.cardsBlock.GetComponent<NewBlockController> ().resize(cardsBlockLeftMargin,cardsBlockUpMargin,ApplicationDesignRules.blockWidth,cardsBlockHeight);
		Vector3 cardsBlockUpperLeftPosition = this.cardsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 cardsBlockLowerLeftPosition = this.cardsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector3 cardsBlockUpperRightPosition = this.cardsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 cardsBlockSize = this.cardsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 cardsBlockOrigin = this.cardsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.cardsBlockTitle.transform.position = new Vector3 (cardsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, cardsBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.cardsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		this.cardsNumberTitle.transform.position = new Vector3 (cardsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, cardsBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
		this.cardsNumberTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.filterButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.filterButton.transform.position = new Vector3 (cardsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, cardsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		this.mainContentPositionX = deckBlockOrigin.x;
		this.filtersPositionX = filtersBlockOrigin.x;
	
		this.cardsArea = new Rect (cardsBlockUpperLeftPosition.x,cardsBlockLowerLeftPosition.y,cardsBlockSize.x,cardsBlockSize.y);
		this.cardsPosition=new Vector3[this.cardsPerLine*this.nbLines];
		this.cards=new GameObject[this.cardsPerLine*this.nbLines];

		for(int j=0;j<this.nbLines;j++)
		{
			for(int i =0;i<this.cardsPerLine;i++)
			{
				this.cards[j*(cardsPerLine)+i]=Instantiate (this.cardObject) as GameObject;
				this.cards[j*(cardsPerLine)+i].AddComponent<NewCardMyGameController>();
				this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardMyGameController>().setId(j*(cardsPerLine)+i,false);
				this.cards[j*(cardsPerLine)+i].transform.localScale= ApplicationDesignRules.cardScale;
				this.cardsPosition[j*(this.cardsPerLine)+i]=new Vector3(cardsBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+ApplicationDesignRules.cardWorldSize.x/2f+i*(gapBetweenCards+ApplicationDesignRules.cardWorldSize.x),cardsBlockUpperRightPosition.y-firstLineCardsPositionY-j*(ApplicationDesignRules.gapBetweenCardsLine+ApplicationDesignRules.cardWorldSize.y),0f);
				this.cards[j*(cardsPerLine)+i].transform.position=this.cardsPosition[j*(this.cardsPerLine)+i];
				this.cards[j*(this.cardsPerLine)+i].transform.name="Card"+(j*(this.cardsPerLine)+i);
			}
		}

		float lineScale = ApplicationDesignRules.getLineScale (cardsBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing);

		this.cardsScrollLine.SetActive(false);
		this.cardsScrollLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.cardsScrollLine.transform.position = new Vector3 (cardsBlockLowerLeftPosition.x + cardsBlockSize.x / 2, cardsBlockUpperLeftPosition.y - 1.15f, 0f);
		this.cardsPaginationLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.cardsPaginationLine.transform.position = new Vector3 (cardsBlockLowerLeftPosition.x + cardsBlockSize.x / 2, cardsBlockLowerLeftPosition.y + 0.6f, 0f);
		this.cardsPaginationButtons.transform.GetComponent<newMyGamePaginationController> ().resize ();

		this.focusedCard.transform.localScale = ApplicationDesignRules.cardFocusedScale;
		this.focusedCard.transform.position = ApplicationDesignRules.focusedCardPosition;
		this.focusedCard.GetComponent<NewFocusedCardMyGameController> ().resize ();

		if(ApplicationDesignRules.isMobileScreen)
		{
			this.cardsPaginationButtons.transform.localPosition=new Vector3 (cardsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 2.5f*ApplicationDesignRules.roundButtonWorldSize.x, cardsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);
			this.cardTypeFilterTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Left;
			this.cardTypeFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Left;
			this.cardTypeFilterTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x, filtersBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
			float gapBetweenCardTypesFilters = (filtersBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing-5f*ApplicationDesignRules.cardTypeFilterWorldSize.x)/4f;
			for(int i = 0;i<this.cardsTypeFilters.Length;i++)
			{
				Vector3 cardTypeFilterPosition=new Vector3();

				if(i<5)
				{
					cardTypeFilterPosition.y=filtersBlockUpperLeftPosition.y-1.75f;
					cardTypeFilterPosition.x=filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+0.5f*ApplicationDesignRules.cardTypeFilterWorldSize.x+i*(ApplicationDesignRules.cardTypeFilterWorldSize.x+gapBetweenCardTypesFilters);

				}
				else
				{
					cardTypeFilterPosition.y=filtersBlockUpperLeftPosition.y-2.90f;
					cardTypeFilterPosition.x=filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+0.5f*ApplicationDesignRules.cardTypeFilterWorldSize.x+(i-5)*(ApplicationDesignRules.cardTypeFilterWorldSize.x+gapBetweenCardTypesFilters);

				}
				this.cardsTypeFilters[i].transform.position=cardTypeFilterPosition;
				this.cardsTypeFilters[i].transform.localScale=ApplicationDesignRules.cardTypeFilterScale;
			}
			this.skillSearchBarTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Left;
			this.skillSearchBarTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Left;
			this.skillSearchBarTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x, filtersBlockUpperLeftPosition.y - 3.75f, 0f);
			this.skillSearchBar.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x+ApplicationDesignRules.inputTextWorldSize.x/2f, filtersBlockUpperLeftPosition.y - 4.20f, 0f);
			for(int i=0;i<this.skillChoices.Length;i++)
			{
				this.skillChoices[i].transform.localScale=ApplicationDesignRules.listElementScale;
				this.skillChoices[i].transform.position=new Vector3(this.skillSearchBar.transform.position.x,this.skillSearchBar.transform.position.y-ApplicationDesignRules.inputTextWorldSize.y/2f-(i+0.5f)*ApplicationDesignRules.listElementWorldSize.y+i*0.02f,-1f);
			}
			this.valueFilterTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Left;
			this.valueFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Left;
			this.valueFilterTitle.transform.position=new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x, filtersBlockUpperLeftPosition.y - 4.65f, 0f);
			for(int i=0;i<this.valueFilters.Length;i++)
			{
				this.valueFilters[i].transform.localScale=ApplicationDesignRules.valueFilterScale;
				this.valueFilters[i].transform.position=new Vector3(filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+1.45f,filtersBlockUpperLeftPosition.y - 5.20f-i*0.6f,0f);
				this.valueFilters[i].transform.FindChild("Sort0").localScale=new Vector3(0.55f,0.55f,0.55f);
				this.valueFilters[i].transform.FindChild("Sort1").localScale=new Vector3(0.55f,0.55f,0.55f);
				this.valueFilters[i].transform.FindChild("Sort0").localPosition=new Vector3(1.26f,0.135f,0f);
				this.valueFilters[i].transform.FindChild("Sort1").localPosition=new Vector3(1.65f,0.135f,0f);
			}

		}
		else
		{
			this.cardsPaginationButtons.transform.localPosition=new Vector3(cardsBlockLowerLeftPosition.x+cardsBlockSize.x/2f, cardsBlockLowerLeftPosition.y + 0.3f, 0f);
			this.cardTypeFilterTitle.GetComponent<TextContainer>().anchorPosition = TextContainerAnchors.Middle;
			this.cardTypeFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
			this.cardTypeFilterTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f, filtersBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
			float gapBetweenCardTypesFilters = (filtersSubBlockSize - 4f * ApplicationDesignRules.cardTypeFilterWorldSize.x) / 3f;
			float gapBetweenLines;
			if(gapBetweenCardTypesFilters>0.05f)
			{
				gapBetweenLines=0.05f;
			}
			else
			{
				gapBetweenLines=gapBetweenCardTypesFilters;
			}
			for(int i=0;i<this.cardsTypeFilters.Length;i++)
			{
				int column=0;
				int line=0;
				Vector3 position=new Vector3();
				if((i>=0 && i<3)||(i>=7))
				{
					if(i>=7)
					{
						column=i-7;
						line=2;
					}
					else
					{
						column=i;
						line=0;
					}
					position.x=filtersBlockUpperLeftPosition.x+0.3f+ApplicationDesignRules.cardTypeFilterWorldSize.x+column*(gapBetweenCardTypesFilters+ApplicationDesignRules.cardTypeFilterWorldSize.x);
				}
				else if(i>=3&& i<7)
				{
					column=i-3;
					line=1;
					position.x=filtersBlockUpperLeftPosition.x+0.3f+ApplicationDesignRules.cardTypeFilterWorldSize.x/2f+column*(gapBetweenCardTypesFilters+ApplicationDesignRules.cardTypeFilterWorldSize.x);
				}
				position.y=filtersBlockUpperLeftPosition.y-1.7f-line*(ApplicationDesignRules.cardTypeFilterWorldSize.y+gapBetweenLines);
				position.z=0;
				this.cardsTypeFilters[i].transform.localScale=ApplicationDesignRules.cardTypeFilterScale;
				this.cardsTypeFilters[i].transform.position=position;
			}
			this.skillSearchBarTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Middle;
			this.skillSearchBarTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
			this.skillSearchBarTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 2f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 1.2f, 0f);
			this.skillSearchBar.transform.position = new Vector3 (this.skillSearchBarTitle.transform.position.x, filtersBlockUpperLeftPosition.y - 1.6f, 0f);
			for(int i=0;i<this.skillChoices.Length;i++)
			{
				this.skillChoices[i].transform.localScale=ApplicationDesignRules.listElementScale;
				this.skillChoices[i].transform.position=new Vector3(this.skillSearchBar.transform.position.x,this.skillSearchBar.transform.position.y-ApplicationDesignRules.inputTextWorldSize.y/2f-(i+0.5f)*ApplicationDesignRules.listElementWorldSize.y+i*0.02f,-1f);
			}
			this.valueFilterTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Middle;
			this.valueFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
			this.valueFilterTitle.transform.position=new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 1f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 1.2f, 0f);
			for(int i=0;i<this.valueFilters.Length;i++)
			{
				this.valueFilters[i].transform.localScale=ApplicationDesignRules.valueFilterScale;
				this.valueFilters[i].transform.position=new Vector3(valueFilterTitle.transform.position.x,filtersBlockUpperLeftPosition.y - 1.6f-i*0.5f,0f);
				this.valueFilters[i].transform.FindChild("Sort0").localScale=new Vector3(0.32f,0.32f,0.32f);
				this.valueFilters[i].transform.FindChild("Sort1").localScale=new Vector3(0.32f,0.32f,0.32f);
				this.valueFilters[i].transform.FindChild("Sort0").localPosition=new Vector3(0.5594f,-0.0669f,0f);
				this.valueFilters[i].transform.FindChild("Sort1").localPosition=new Vector3(0.8004f,-0.0669f,0f);
			}

		}
		this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().resize();

		if(newDeckPopUpDisplayed)
		{
			this.newDeckPopUpResize();
		}
		else if(editDeckPopUpDisplayed)
		{
			this.editDeckPopUpResize();
		}
		else if(deleteDeckPopUpDisplayed)
		{
			this.deleteDeckPopUpResize();
		}
		TutorialObjectController.instance.resize ();
	}
	public void drawCards()
	{
		this.cardsDisplayed = new List<int> ();

		for(int j=0;j<nbLines;j++)
		{
			for(int i =0;i<cardsPerLine;i++)
			{
				if(this.cardsPagination.chosenPage*(this.nbLines*this.cardsPerLine)+j*(cardsPerLine)+i<this.cardsToBeDisplayed.Count)
				{
					this.cardsDisplayed.Add (this.cardsToBeDisplayed[this.cardsPagination.chosenPage*(this.nbLines*this.cardsPerLine)+j*(cardsPerLine)+i]);
					this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardController>().c=model.cards.getCard(this.cardsDisplayed[j*(cardsPerLine)+i]);
					this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardController>().show();
					this.cards[j*(cardsPerLine)+i].SetActive(true);
				}
				else
				{
					this.cards[j*(cardsPerLine)+i].SetActive(false);
				}
			}
		}
		if(ApplicationDesignRules.isMobileScreen)
		{
			int nbLinesToDisplay = Mathf.CeilToInt ((float)this.cardsDisplayed.Count / (float)this.cardsPerLine);
			float contentHeight = 2f+nbLinesToDisplay*(ApplicationDesignRules.cardWorldSize.y+ApplicationDesignRules.gapBetweenCardsLine)-1f;
			if(this.lowerScrollCamera.GetComponent<ScrollingController>().getViewHeight()>contentHeight)
			{
				contentHeight=this.lowerScrollCamera.GetComponent<ScrollingController>().getViewHeight()+0.7f;
			}
			this.lowerScrollCamera.GetComponent<ScrollingController> ().setContentHeight(contentHeight);
			this.lowerScrollCamera.GetComponent<ScrollingController>().setEndPositionY();
		}
	}
	public void cleanCards()
	{
		for(int i=0;i<this.cards.Length;i++)
		{
			Destroy(this.cards[i]);
		}
	}
	public void drawDeckCards()
	{
		this.deckCardsDisplayed = new int[]{-1,-1,-1,-1};
		if(this.deckDisplayed!=-1)
		{	
			for(int i=0;i<model.decks[this.deckDisplayed].cards.Count;i++)
			{
				int deckOrder = model.decks[this.deckDisplayed].cards[i].deckOrder;
				int cardId=model.decks[this.deckDisplayed].cards[i].Id;
				for(int j=0;j<model.cards.getCount();j++)
				{
					if(model.cards.getCard(j).Id==cardId)
					{
						this.deckCardsDisplayed[deckOrder]=j;
						break;
					}
				}
			}
			this.deckTitle.GetComponent<TextMeshPro> ().text = model.decks[this.deckDisplayed].Name.ToUpper();
			this.deckDeletionButton.gameObject.SetActive(true);
			this.deckRenameButton.gameObject.SetActive(true);
			if(model.decks.Count>1)
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
			this.deckTitle.GetComponent<TextMeshPro> ().text = "Aucune armée créé pour le moment".ToUpper();
			this.deckDeletionButton.gameObject.SetActive(false);
			this.deckRenameButton.gameObject.SetActive(false);
			this.deckSelectionButton.gameObject.SetActive(false);
		}
		for(int i=0;i<this.deckCards.Length;i++)
		{
			if(this.deckCardsDisplayed[i]!=-1)
			{
				this.deckCards[i].transform.GetComponent<NewCardController>().c=model.cards.getCard(this.deckCardsDisplayed[i]);
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
		this.displayBackUI (false);
		this.focusedCard.SetActive (true);
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);

		if(isDeckCardClicked)
		{
			this.focusedCardIndex=this.deckCardsDisplayed[this.idCardClicked];
		}
		else
		{
			this.focusedCardIndex=this.cardsDisplayed[this.idCardClicked];
		}
		this.focusedCard.GetComponent<NewFocusedCardController>().c=model.cards.getCard(this.focusedCardIndex);
		this.focusedCard.GetComponent<NewFocusedCardController> ().show ();
		TutorialObjectController.instance.tutorialTrackPoint();
	}
	public void hideCardFocused()
	{
		this.isCardFocusedDisplayed = false;
		this.focusedCard.SetActive (false);
		this.displayBackUI (true);
		if(isDeckCardClicked)
		{
			this.deckCards[this.idCardClicked].GetComponent<NewCardController>().show();
		}
		else
		{
			this.applyFilters ();
		}
		TutorialObjectController.instance.tutorialTrackPoint();
	}
	public void displayBackUI(bool value)
	{
		if(value)
		{
			if(ApplicationDesignRules.isMobileScreen)
			{
				this.lowerScrollCamera.SetActive(true);
				this.upperScrollCamera.SetActive(true);
				this.sceneCamera.SetActive(false);
			}
			this.sceneCamera.transform.position=ApplicationDesignRules.sceneCameraStandardPosition;
		}
		else
		{
			if(ApplicationDesignRules.isMobileScreen)
			{
				this.lowerScrollCamera.SetActive(false);
				this.upperScrollCamera.SetActive(false);
				this.sceneCamera.SetActive(true);
			}
			this.sceneCamera.transform.position=ApplicationDesignRules.sceneCameraFocusedCardPosition;
		}
		this.skillSearchBar.SetActive(value);
	}
	public void selectDeck(int id)
	{
		this.deckDisplayed = this.decksDisplayed [id];
		this.cleanDeckList ();
		this.isSearchingDeck = false;
		this.initializeDecks ();
		this.applyFilters ();
	}
	public void displayDeckList()
	{
		if(!isSearchingDeck)
		{
			this.setDeckList ();
			this.isSearchingDeck=true;
		}
	}
	private void cleanDeckList ()
	{
		for(int i=0;i<this.deckChoices.Length;i++)
		{
			this.deckChoices[i].SetActive(false);
		}
	}
	private void setDeckList()
	{
		for (int i = 0; i < this.deckChoices.Length; i++) 
		{  
			if(i<this.decksDisplayed.Count)
			{
				this.deckChoices[i].SetActive(true);
				this.deckChoices[i].transform.GetComponent<newMyGameDeckChoiceController>().reset();
				this.deckChoices[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=model.decks[this.decksDisplayed[i]].Name;
			}
			else
			{
				this.deckChoices[i].SetActive(false);
			}
			
		}
	}
	public void searchingSkill()
	{
		if(isSkillChosen)
		{
			this.isSkillChosen=false;
			this.cardsPagination.chosenPage = 0;
			this.applyFilters();
		}
		this.cleanSkillAutocompletion();
		this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().setText("");
		this.valueSkill = "";
		this.isSearchingSkill = true;
	}
	public void cardTypeFilterHandler(int id)
	{
		if(this.filtersCardType.Contains(id))
		{
			this.filtersCardType.Remove(id);
			this.cardsTypeFilters[id].GetComponent<newMyGameCardTypeFilterController>().reset();
		}
		else
		{
			this.filtersCardType.Add (id);
			this.cardsTypeFilters[id].GetComponent<newMyGameCardTypeFilterController>().setIsSelected(true);
			this.cardsTypeFilters[id].GetComponent<newMyGameCardTypeFilterController>().setHoveredState();
		}
		this.cardsPagination.chosenPage = 0;
		this.applyFilters ();
	}
	public void sortButtonHandler(int id)
	{
		if(this.sortingOrder==id)
		{
			this.sortingOrder = -1;
			this.sortButtons[id].GetComponent<newMyGameSortButtonController>().reset();
		}
		else if(this.sortingOrder!=-1)
		{
			this.sortButtons[this.sortingOrder].GetComponent<newMyGameSortButtonController>().reset();
			this.sortButtons[id].GetComponent<newMyGameSortButtonController>().setIsSelected(true);
			this.sortButtons[id].GetComponent<newMyGameSortButtonController>().setHoveredState();
			this.sortingOrder = id;
		}
		else
		{
			this.sortingOrder=id;
			this.sortButtons[id].GetComponent<newMyGameSortButtonController>().setIsSelected(true);
			this.sortButtons[id].GetComponent<newMyGameSortButtonController>().setHoveredState();
		}
		this.cardsPagination.chosenPage = 0;
		this.applyFilters ();
	}
	public void moveCursors(int cursorId)
	{
		this.isSlidingCursors = true;
		float offsetStep = 0f;
		Vector3 mousePosition = new Vector3 ();
		if (ApplicationDesignRules.isMobileScreen) 
		{
			mousePosition = this.lowerScrollCamera.GetComponent<Camera>().ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			offsetStep=0.67f*1.5f;
		}
		else
		{
			mousePosition = this.sceneCamera.GetComponent<Camera>().ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			offsetStep=0.67f;
		}
		float mousePositionX=mousePosition.x;
		Vector3 cursorPosition = this.cursors [cursorId].transform.localPosition;
		float offset = mousePositionX-this.cursors [cursorId].transform.position.x;
	
		int value = -1;
		string label = "";

		bool isMoved = true ;

		if(cursorPosition.x==-0.67f)
		{
			if(offset>offsetStep)
			{
				value = 2;
				cursorPosition.x=+0.67f;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else if(offset>offsetStep/2f)
			{
				value = 1;
				cursorPosition.x=0;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else
			{
				isMoved=false;
			}
		}
		else if(cursorPosition.x==0)
		{
			if(offset>offsetStep/2f)
			{
				value =2;
				cursorPosition.x=+0.67f;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else if(offset<-offsetStep/2f)
			{
				value = 0;
				cursorPosition.x=-0.67f;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else
			{
				isMoved=false;
			}
		}
		else if(cursorPosition.x==0.67f)
		{
			if(offset<-offsetStep)
			{
				value = 0;
				cursorPosition.x=-0.67f;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else if(offset<-offsetStep/2f)
			{
				value = 1;
				cursorPosition.x=0;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else
			{
				isMoved=false;
			}
		}

		if(isMoved)
		{
			switch (cursorId) 
			{
			case 0:
				powerVal=value;
				this.valueFilters[0].transform.FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(powerVal);
				this.valueFilters[0].transform.FindChild("Icon").GetComponent<SpriteRenderer>().color=getColorFilterIcon(powerVal);
				break;
			case 1:
				attackVal=value;
				this.valueFilters[1].transform.FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(attackVal);
				this.valueFilters[1].transform.FindChild("Icon").GetComponent<SpriteRenderer>().color=getColorFilterIcon(attackVal);
				break;
			case 2:
				lifeVal=value;
				this.valueFilters[2].transform.FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(lifeVal);
				this.valueFilters[2].transform.FindChild("Icon").GetComponent<SpriteRenderer>().color=getColorFilterIcon(lifeVal);
				break;
			case 3:
				quicknessVal=value;
				this.valueFilters[3].transform.FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(quicknessVal);
				this.valueFilters[3].transform.FindChild("Icon").GetComponent<SpriteRenderer>().color=getColorFilterIcon(quicknessVal);
				break;
			}
			this.cardsPagination.chosenPage = 0;
			this.applyFilters();
		}
		this.isSlidingCursors = false;
	}
	public string getValueFilterLabel(int value)
	{
		if(value==1)
		{
			return "Rares";
		}
		else if(value==2)
		{
			return "Très rares";
		}
		else
		{
			return "Toutes";
		}
	}
	public Color getColorFilterIcon(int value)
	{
		if(value==1)
		{
			return ApplicationDesignRules.blueColor;
		}
		else if(value==2)
		{
			return ApplicationDesignRules.redColor;
		}
		else
		{
			return ApplicationDesignRules.whiteSpriteColor;
		}
	}
	private void computeFilters() 
	{
		this.cardsToBeDisplayed=new List<int>();
		int nbFilters = this.filtersCardType.Count;
		int max = model.cards.getCount();

		for(int i=0;i<max;i++)
		{
			if(this.isSkillChosen && !model.cards.getCard(i).hasSkill(this.valueSkill.ToLower()))
			{
				continue;
			}
			if(nbFilters>0)
			{
				bool testCardTypes=false;
				for(int j=0;j<nbFilters;j++)
				{
					if (model.cards.getCard(i).IdClass == this.filtersCardType [j])
					{
						testCardTypes=true;
						break;
					}
				}
				if(!testCardTypes)
				{
					continue;
				}
			}
			bool testDeck=true;
			for (int j = 0; j < this.deckCardsDisplayed.Length; j++)
			{
				if (i == this.deckCardsDisplayed [j])
				{
					testDeck = false; 
				}
			}
			if(!testDeck)
			{
				continue;
			}
			if(model.cards.getCard(i).PowerLevel-1>=this.powerVal&&
			   model.cards.getCard(i).AttackLevel-1>=this.attackVal&&
			   model.cards.getCard(i).LifeLevel-1>=this.lifeVal&&
			   model.cards.getCard(i).SpeedLevel-1>=this.quicknessVal)
			{
				this.cardsToBeDisplayed.Add(i);
			}
		}
		
		if(this.sortingOrder!=-1)
		{
			int tempA=new int();
			int tempB=new int();
			
			for (int i = 1; i<this.cardsToBeDisplayed.Count; i++) {
				
				for (int j=0;j<i;j++){
					
					switch (this.sortingOrder)
					{
					case 0:
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Power;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Power;
						break;
					case 1:
						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Power;
						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Power;
						break;
					case 2:
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Attack;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Attack;
						break;
					case 3:
						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Attack;
						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Attack;
						break;
					case 4:
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Life;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Life;
						break;
					case 5:
						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Life;
						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Life;
						break;
//					case 6:
//						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Speed;
//						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Speed;
//						break;
//					case 7:
//						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Speed;
//						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Speed;
//						break;
					default:
						break;
					}
					
					if (tempA<tempB){
						this.cardsToBeDisplayed.Insert (j,this.cardsToBeDisplayed[i]);
						this.cardsToBeDisplayed.RemoveAt(i+1);
						break;
					}
					
				}
			}
		}
	}
	private void setSkillAutocompletion()
	{
		this.skillsDisplayed = new List<int> ();
		this.cleanSkillAutocompletion ();
		this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().setText(this.valueSkill);
		if(this.valueSkill.Length>0)
		{
			for (int i = 0; i < model.skillsList.Count; i++) 
			{  
				if (model.skillsList [i].ToLower ().Contains (this.valueSkill.ToLower())) 
				{
					this.skillsDisplayed.Add (i);
					this.skillChoices[this.skillsDisplayed.Count-1].SetActive(true);
					this.skillChoices[this.skillsDisplayed.Count-1].GetComponent<newMyGameSkillChoiceController>().reset();
					this.skillChoices[this.skillsDisplayed.Count-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text = model.skillsList [i];
				}
				if(this.skillsDisplayed.Count==this.skillChoices.Length)
				{
					break;
				}
			}
		}
	}
	private void cleanSkillAutocompletion ()
	{
		for(int i=0;i<this.skillChoices.Length;i++)
		{
			this.skillChoices[i].SetActive(false);
		}
	}
	public void filterASkill(int id)
	{
		this.isSearchingSkill = false;
		this.valueSkill = this.skillChoices[id].transform.FindChild("Title").GetComponent<TextMeshPro>().text;
		this.isSkillChosen = true;
		this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().setText(this.valueSkill);
		this.cleanSkillAutocompletion ();
		this.cardsPagination.chosenPage = 0;
		this.applyFilters ();
	}
	public void mouseOnSelectDeckButton(bool value)
	{
		this.isMouseOnSelectDeckButton = value;
	}
	public void displayNewDeckPopUp()
	{
		MenuController.instance.displayTransparentBackground ();
		this.newDeckPopUp.transform.GetComponent<NewDeckPopUpController> ().reset ();
		this.newDeckPopUpDisplayed = true;
		this.newDeckPopUp.SetActive (true);
		this.newDeckPopUpResize ();
		TutorialObjectController.instance.tutorialTrackPoint();
	}
	public void displayEditDeckPopUp()
	{
		MenuController.instance.displayTransparentBackground ();
		this.editDeckPopUp.transform.GetComponent<EditDeckPopUpController> ().reset (model.decks[this.deckDisplayed].Name);
		this.editDeckPopUpDisplayed = true;
		this.editDeckPopUp.SetActive (true);
		this.editDeckPopUpResize ();
		TutorialObjectController.instance.tutorialTrackPoint();
	}
	public void displayDeleteDeckPopUp()
	{
		MenuController.instance.displayTransparentBackground ();
		this.deleteDeckPopUp.transform.GetComponent<DeleteDeckPopUpController> ().reset (model.decks[this.deckDisplayed].Name);
		this.deleteDeckPopUpDisplayed = true;
		this.deleteDeckPopUp.SetActive (true);
		this.deleteDeckPopUpResize ();
		TutorialObjectController.instance.tutorialTrackPoint();
	}
	public void hideNewDeckPopUp()
	{
		this.newDeckPopUp.SetActive (false);
		MenuController.instance.hideTransparentBackground();
		this.newDeckPopUpDisplayed = false;
		TutorialObjectController.instance.tutorialTrackPoint();
	}
	public void hideEditDeckPopUp()
	{
		this.editDeckPopUp.SetActive (false);
		MenuController.instance.hideTransparentBackground();
		this.editDeckPopUpDisplayed = false;
		TutorialObjectController.instance.tutorialTrackPoint();
	}
	public void hideDeleteDeckPopUp()
	{
		this.deleteDeckPopUp.SetActive (false);
		MenuController.instance.hideTransparentBackground();
		this.deleteDeckPopUpDisplayed = false;
		TutorialObjectController.instance.tutorialTrackPoint();
	}
	public void newDeckPopUpResize()
	{
		this.newDeckPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.newDeckPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.newDeckPopUp.GetComponent<NewDeckPopUpController> ().resize ();
	}
	public void editDeckPopUpResize()
	{
		this.editDeckPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.editDeckPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.editDeckPopUp.GetComponent<EditDeckPopUpController> ().resize ();
	}
	public void deleteDeckPopUpResize()
	{
		this.deleteDeckPopUp.transform.position= new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -2f);
		this.deleteDeckPopUp.transform.localScale = ApplicationDesignRules.popUpScale;
		this.deleteDeckPopUp.GetComponent<DeleteDeckPopUpController> ().resize ();
	}
	public void createNewDeckHandler()
	{
		StartCoroutine (this.createNewDeck ());
	}
	private IEnumerator createNewDeck()
	{
		string name = this.newDeckPopUp.transform.GetComponent<NewDeckPopUpController> ().getInputText ();
		string error = this.checkDeckName(name);
		if(error=="")
		{
			this.hideNewDeckPopUp();
			MenuController.instance.displayLoadingScreen();
			model.decks.Add(new Deck());
			yield return StartCoroutine(model.decks[model.decks.Count-1].create(name));
			this.deckDisplayed=model.decks.Count-1;
			this.initializeDecks();
			this.initializeCards();
			MenuController.instance.hideLoadingScreen();
			TutorialObjectController.instance.tutorialTrackPoint();
		}
		this.newDeckPopUp.transform.GetComponent<NewDeckPopUpController> ().setError (error);
	}
	public void editDeckHandler()
	{
		StartCoroutine (this.editDeck ());
	}
	public IEnumerator editDeck()
	{
		string newName = this.editDeckPopUp.transform.GetComponent<EditDeckPopUpController> ().getInputText ();
		if(model.decks[this.deckDisplayed].Name!=newName)
		{
			string error=this.checkDeckName(newName);
			this.editDeckPopUp.transform.GetComponent<EditDeckPopUpController>().setError(error);
			if(error=="")
			{
				MenuController.instance.displayLoadingScreen();
				this.hideEditDeckPopUp();
				yield return StartCoroutine(model.decks[this.deckDisplayed].edit(newName));
				this.deckTitle.GetComponent<TextMeshPro> ().text = model.decks[this.deckDisplayed].Name;
				MenuController.instance.hideLoadingScreen();
			}
		}
		else
		{
			this.hideEditDeckPopUp();
		}
	}
	public void deleteDeckHandler()
	{
		StartCoroutine (this.deleteDeck ());
	}
	public IEnumerator deleteDeck()
	{
		this.hideDeleteDeckPopUp();
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.decks[this.deckDisplayed].delete());
		this.removeDeckFromAllCards (model.decks[this.deckDisplayed].Id);
		model.decks.RemoveAt (this.deckDisplayed);
		this.retrieveDefaultDeck ();
		this.initializeDecks ();
		this.initializeCards ();
		MenuController.instance.hideLoadingScreen ();
		TutorialObjectController.instance.tutorialTrackPoint ();
	}
	public void removeDeckFromAllCards(int id)
	{
		for(int i=0;i<model.cards.getCount();i++)
		{
			for(int j=0;j<model.cards.getCard(i).Decks.Count;j++)
			{
				if(model.cards.getCard(i).Decks[j]==id)
				{
					model.cards.getCard(i).Decks.RemoveAt(j);
					break;
				}
			}
		}
	}
	public void removeCardFromAllDecks(int id)
	{
		for(int i=0;i<model.decks.Count;i++)
		{
			for(int j=0;j<model.decks[i].cards.Count;j++)
			{
				if(model.decks[i].cards[j].Id==id)
				{
					model.decks[i].NbCards--;
					model.decks[i].cards.RemoveAt(j);
					break;
				}
			}
		}
	}
	public string checkDeckName(string name)
	{
		if(name.Length>12)
		{
			return "Le nom ne doit pas dépasser 12 caractères";
		}
		if(!Regex.IsMatch(name, @"^[a-zA-Z0-9_\s]+$"))
		{
			return "Vous ne pouvez pas utiliser de caractères spéciaux";
		}
		for(int i=0;i<model.decks.Count;i++)
		{
			if(model.decks[i].Name==name && i!=this.deckDisplayed)
			{
				return "Nom déjà utilisé";
			}
		}
		if(name=="")
		{
			return "Veuillez saisir un nom";
		}
		return "";
	}
	public void leftClickedHandler(int id, bool isDeckCard)
	{
		if(!this.isScrolling)
		{
			this.idCardClicked = id;
			this.isDeckCardClicked = isDeckCard;
			this.isLeftClicked = true;
			this.clickInterval = 0f;
		}
	}
	public void leftClickReleaseHandler()
	{
		if(isLeftClicked)
		{
			this.isLeftClicked=false;
			if(!TutorialObjectController.instance.getIsTutorialDisplayed())
			{
				this.showCardFocused ();
			}
		}
		else if(isDragging)
		{
			this.endDragging();
		}
	}
	public void startDragging()
	{
		if(this.deckDisplayed==-1)
		{
			MenuController.instance.displayErrorPopUp("Vous devez créer un deck avant de sélectionner une carte");
			this.isLeftClicked=false;
		}
		else if(!isDeckCardClicked && this.checkForIdenticalSkills())
		{
			MenuController.instance.displayErrorPopUp("Vous ne pouvez pas posséder dans votre équipe 2 cartes ayant la même compétence passive");
			this.isLeftClicked=false;
		}
		else
		{
			this.isDragging=true;
			Cursor.SetCursor (this.cursorTextures[1], new Vector2(this.cursorTextures[1].width/2f,this.cursorTextures[1].width/2f), CursorMode.Auto);
			if(!isDeckCardClicked)
			{
				this.cards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(10,"Foreground");
				this.isHoveringDeckArea=false;
				for(int i=0;i<this.cardsHalos.Length;i++)
				{
					this.cardsHalos[i].GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
					this.cardsHalos[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
				}
			}
			else
			{
				this.deckCards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(10,"Foreground");
				this.isHoveringDeckArea=true;
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
			if(!isDeckCardClicked)
			{
				this.cards[this.idCardClicked].transform.position=cardsPosition;
			}
			else
			{
				this.deckCards[this.idCardClicked].transform.position=cardsPosition;
			}
			bool isHoveringDeckCards=false;
			if(ApplicationDesignRules.isMobileScreen)
			{
				mousePosition.y=mousePosition.y+this.upperScrollCamera.GetComponent<ScrollingController>().getInterval();
			}
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
	public void moveToCards()
	{
		StartCoroutine(removeCardFromDeck(this.idCardClicked));
		this.deckCards[this.idCardClicked].SetActive(false);
		this.deckCardsDisplayed[this.idCardClicked]=-1;
		this.applyFilters();
		if(ApplicationModel.hasDeck)
		{
			bool isADeckCompleted = false;
			for(int i=0;i<model.decks.Count;i++)
			{
				if(model.decks[i].cards.Count==ApplicationModel.nbCardsByDeck && i!=this.deckDisplayed)
				{
					isADeckCompleted=true;
					break;
				}
			}
			ApplicationModel.hasDeck=isADeckCompleted;
			TutorialObjectController.instance.tutorialTrackPoint();
		}
	}
	public void moveToDeckCards(int position)
	{
		if(!isDeckCardClicked)
		{
			StartCoroutine(addCardToDeck(this.idCardClicked,position));
			if(this.deckCardsDisplayed[position]!=-1)
			{
				StartCoroutine(removeCardFromDeck(position));
			}
			this.deckCards[position].SetActive(true);
			this.deckCards[position].GetComponent<NewCardController>().c=model.cards.getCard(this.cardsDisplayed[this.idCardClicked]);
			this.deckCards[position].GetComponent<NewCardController>().show();
			this.deckCardsDisplayed[position]=this.cardsDisplayed[this.idCardClicked];
			this.applyFilters();
			if(!ApplicationModel.hasDeck)
			{
				bool isDeckCompleted=true;
				for(int i=0;i<this.deckCardsDisplayed.Length;i++)
				{
					if(this.deckCardsDisplayed[i]!=-1)
					{
						continue;
					}
					else
					{
						isDeckCompleted=false;
						break;
					}
				}
				ApplicationModel.hasDeck = isDeckCompleted;
				TutorialObjectController.instance.tutorialTrackPoint();
			}
		}
		else
		{
			int idCard1=model.cards.getCard(deckCardsDisplayed[this.idCardClicked]).Id;
			this.deckCards[position].SetActive(true);
			this.deckCards[position].GetComponent<NewCardController>().c=model.cards.getCard(this.deckCardsDisplayed[this.idCardClicked]);
			this.deckCards[position].GetComponent<NewCardController>().show();
			if(this.deckCardsDisplayed[position]!=-1)
			{
				int indexCard2=this.deckCardsDisplayed[position];
				int idCard2=model.cards.getCard(indexCard2).Id;
				this.deckCards[position].GetComponent<NewCardController>().c=model.cards.getCard(this.deckCardsDisplayed[this.idCardClicked]);
				this.deckCards[position].GetComponent<NewCardController>().show ();
				this.deckCardsDisplayed[position]=this.deckCardsDisplayed[this.idCardClicked];
				this.deckCards[this.idCardClicked].GetComponent<NewCardController>().c=model.cards.getCard(indexCard2);
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
	}
	public IEnumerator removeCardFromDeck(int cardPosition)
	{
		int cardIndex = deckCardsDisplayed[cardPosition];
		model.cards.getCard(cardIndex).Decks.Remove(model.decks[this.deckDisplayed].Id);
		yield return StartCoroutine(model.decks[this.deckDisplayed].removeCard(model.cards.getCard(cardIndex).Id));
	}
	public IEnumerator addCardToDeck(int cardPosition, int deckOrder)
	{
		int cardIndex = this.cardsDisplayed [cardPosition];
		model.cards.getCard(cardIndex).Decks.Add(model.decks[this.deckDisplayed].Id);
		yield return StartCoroutine(model.decks[this.deckDisplayed].addCard(model.cards.getCard(cardIndex).Id,deckOrder));
	}
	public IEnumerator changeDeckCardsOrder(int idCard1, int deckOrder1, int idCard2, int deckOrder2)
	{
		yield return StartCoroutine(model.decks[this.deckDisplayed].changeCardsOrder(idCard1,deckOrder1,idCard2,deckOrder2));
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
		if(!isDeckCardClicked)
		{
			this.cards[this.idCardClicked].transform.position=this.cardsPosition[this.idCardClicked];
			this.cards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(-10,"Foreground");
		}
		else
		{
			this.deckCards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(-10,"Foreground");
			this.deckCards[this.idCardClicked].transform.position=this.deckCardsPosition[this.idCardClicked];
		}
		for(int i=0;i<this.cardsHalos.Length;i++)
		{
			this.cardsHalos[i].GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
			this.cardsHalos[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteSpriteColor;
		}

		Vector3 cursorPosition = this.sceneCamera.GetComponent<Camera>().ScreenToWorldPoint (new Vector2 (Input.mousePosition.x, Input.mousePosition.y));
		if(ApplicationDesignRules.isMobileScreen)
		{
			cursorPosition.y=cursorPosition.y+this.upperScrollCamera.GetComponent<ScrollingController>().getInterval();
		}
		if(this.cardsArea.Contains(cursorPosition) && isDeckCardClicked)
		{
			this.moveToCards();
		}
		else
		{
			for(int i=0;i<deckCardsArea.Length;i++)
			{
				if(this.deckCardsArea[i].Contains(cursorPosition))
				{
					this.moveToDeckCards(i);
					break;
				}
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
	public void deleteCard()
	{
		StartCoroutine(MenuController.instance.getUserData ());
		this.hideCardFocused ();
		this.removeCardFromAllDecks(model.cards.getCard(this.focusedCardIndex).Id);
		model.cards.cards.RemoveAt(this.focusedCardIndex);
		this.drawDeckCards ();
		this.initializeCards ();
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
		else if(this.newDeckPopUpDisplayed)
		{
			this.createNewDeckHandler();
		}
		else if(this.editDeckPopUp)
		{
			this.editDeckHandler();
		}
		else if(this.deleteDeckPopUpDisplayed)
		{
			this.deleteDeckHandler();
		}
	}
	public void escapePressed()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().escapePressed();
		}
		else if(this.newDeckPopUpDisplayed)
		{
			this.hideNewDeckPopUp();
		}
		else if(this.editDeckPopUp)
		{
			this.hideEditDeckPopUp();
		}
		else if(this.deleteDeckPopUpDisplayed)
		{
			this.hideDeleteDeckPopUp();
		}
		else
		{
			MenuController.instance.leaveGame();
		}
	}
	public void closeAllPopUp()
	{
		if(this.newDeckPopUpDisplayed)
		{
			this.hideNewDeckPopUp();
		}
		if(this.editDeckPopUp)
		{
			this.hideEditDeckPopUp();
		}
		if(this.deleteDeckPopUpDisplayed)
		{
			this.hideDeleteDeckPopUp();
		}
	}

	public bool isDeckCompleted()
	{
		for(int i=0;i<this.deckCardsDisplayed.Length;i++)
		{
			if(this.deckCardsDisplayed[i]==-1)
			{
				return false;
			}
		}
		return true;
	}
	public void setDeckListColliders(bool value)
	{
		this.deckCreationButton.GetComponent<BoxCollider2D> ().enabled = value;
		this.deckSelectionButton.GetComponent<BoxCollider2D> ().enabled = value;
		this.deckDeletionButton.GetComponent<BoxCollider2D> ().enabled = value;
		this.deckRenameButton.GetComponent<BoxCollider2D> ().enabled = value;
	}
	public void moneyUpdate()
	{
		if(isSceneLoaded)
		{
			if(this.isCardFocusedDisplayed)
			{
				this.focusedCard.GetComponent<NewFocusedCardMyGameController>().updateFocusFeatures();
			}
		}
	}
	public void slideRight()
	{
		if(this.mainContentDisplayed)
		{
			this.upperScrollCamera.GetComponent<ScrollingController>().reset();
			this.lowerScrollCamera.GetComponent<ScrollingController>().reset();
			this.toScrollCards=false;
		}
		this.toSlideRight=true;
		this.toSlideLeft=false;
		this.mainContentDisplayed=false;
	}
	public void slideLeft()
	{
		this.toSlideLeft=true;
		this.toSlideRight=false;
		this.filtersDisplayed=false;
		this.skillSearchBar.SetActive(false);
		TutorialObjectController.instance.tutorialTrackPoint();
	}
	public Camera returnCurrentCamera(bool isDeckCard)
	{
		if(!ApplicationDesignRules.isMobileScreen)
		{
			return this.sceneCamera.GetComponent<Camera>();
		}
		else
		{
			if(isDeckCard)
			{
				return this.upperScrollCamera.GetComponent<Camera>();
			}
			else
			{
				return this.lowerScrollCamera.GetComponent<Camera>();
			}
		}
	}
	public bool checkForIdenticalSkills()
	{
		for(int i=0;i<this.deckCardsDisplayed.Length;i++)
		{
			if(this.deckCardsDisplayed[i]!=-1)
			{
				if(model.cards.getCard(this.cardsDisplayed[idCardClicked]).Skills[0].Id==model.cards.getCard(this.deckCardsDisplayed[i]).Skills[0].Id)
				{
					return true;
				}
			}
		}
		return false;
	}
	public void setGUI(bool value)
	{
		this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().setGUI(value);
	}
	#region TUTORIAL FUNCTIONS

	public bool getIsCardFocusedDisplayed()
	{
		return isCardFocusedDisplayed;
	}
	public bool isADeckCurrentlySelected()
	{
		if(this.deckDisplayed!=-1)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public Vector3 getNewDeckButtonPosition()
	{
		Vector3 deckCreationButtonPosition = this.deckCreationButton.transform.position;
		if(ApplicationDesignRules.isMobileScreen)
		{
			deckCreationButtonPosition.y=deckCreationButtonPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f-this.upperScrollCamera.GetComponent<ScrollingController>().getInterval();
		}
		return deckCreationButtonPosition;
	}
	public Vector3 getCardsPosition(int id)
	{
		return cards[id].transform.position;
		return new Vector3 ();
	}
	public Vector3 getFiltersBlockOrigin()
	{
		return this.filtersBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getFiltersBlockSize()
	{
		return this.filtersBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getCardsBlockOrigin()
	{
		return this.cardsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getCardsBlockSize()
	{
		return this.cardsBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public Vector3 getDeckBlockOrigin()
	{
		return this.deckBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public Vector2 getDeckBlockSize()
	{
		return this.deckBlock.GetComponent<NewBlockController> ().getSize ();
	}
	public GameObject returnCardFocused()
	{
		return this.focusedCard;
	}
	public bool getIsFocusedCardDisplayed()
	{
		return this.isCardFocusedDisplayed;
	}
	public Vector3 getUpperScrollCameraPosition()
	{
		return this.upperScrollCamera.transform.position;
	}
	public Vector3 getSlideLeftButtonPosition()
	{
		Vector3 buttonPosition = this.slideLeftButton.transform.position;
		buttonPosition.x=buttonPosition.x-ApplicationDesignRules.worldWidth;
		buttonPosition.y=buttonPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f;
		return buttonPosition;
	}
	public bool getFiltersDisplayed()
	{
		return this.filtersDisplayed;
	}
	public bool isAPopUpDisplayed()
	{
		if(this.deleteDeckPopUpDisplayed)
		{
			return true;
		}
		else if(this.editDeckPopUpDisplayed)
		{
			return true;
		}
		else if(this.newDeckPopUpDisplayed)
		{
			return true;
		}
		return false;
	}
	public bool getIsMainContentDisplayed()
	{
		return this.mainContentDisplayed;
	}
	public void resetScrolling()
	{
		this.upperScrollCamera.GetComponent<ScrollingController>().reset();
		this.lowerScrollCamera.GetComponent<ScrollingController>().reset();
		this.toScrollCards=false;
	}
	#endregion
}
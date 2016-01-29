using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class NewMarketController : MonoBehaviour
{
	public static NewMarketController instance;
	private NewMarketModel model;

	public GameObject blockObject;
	public GameObject cardObject;
	public GUISkin popUpSkin;
	public int totalNbResultLimit;
	public int refreshInterval;

	private GameObject menu;
	private GameObject tutorial;
	private GameObject refreshMarketButton;

	private GameObject cardsBlock;
	private GameObject cardsBlockTitle;
	private GameObject[] cards;
	private GameObject cardsPaginationButtons;
	private GameObject cardsPaginationLine;
	private GameObject cardsScrollLine;
	private GameObject cardsNumberTitle;

	private GameObject[] tabs;

	private GameObject marketBlock;
	private GameObject marketBlockTitle;
	private GameObject marketSubtitle;
	
	private GameObject filtersBlock;
	private GameObject filtersBlockTitle;
	private GameObject[] cardsTypeFilters;
	private GameObject skillSearchBarTitle;
	private GameObject skillSearchBar;
	private GameObject[] skillChoices;
	private GameObject valueFilterTitle;
	private GameObject[] valueFilters;
	private GameObject priceFilter;
	private GameObject priceFilterTitle;
	private GameObject cardTypeFilterTitle;
	private GameObject[] cursors;
	private GameObject[] sortButtons;
	private GameObject[] priceCursors;
	
	private GameObject focusedCard;

	private GameObject mainCamera;
	private GameObject lowerScrollCamera;
	private GameObject upperScrollCamera;
	private GameObject sceneCamera;
	private GameObject tutorialCamera;
	private GameObject backgroundCamera;

	private GameObject informationButton;
	private GameObject filterButton;
	private GameObject slideRightButton;
	private GameObject slideLeftButton;
	
	private bool isCardFocusedDisplayed;

	private int activeTab;

	private bool isSearchingSkill;
	private bool isSkillChosen;
	private string valueSkill;
	private IList<int> skillsDisplayed;

	private int powerVal;
	private int lifeVal;
	private int attackVal;
	private int quicknessVal;
	
	private float minPriceVal;
	private float maxPriceVal;
	private float minPriceLimit;
	private float maxPriceLimit;
	private float oldMinPriceVal;
	private float oldMaxPriceVal;

	private IList<int> filtersCardType;
	private int sortingOrder;
	
	private IList<int> cardsToBeDisplayed;
	private IList<int> cardsDisplayed;
	
	private Pagination cardsPagination;
	private int cardsPerLine;
	private int nbLines;

	private int idCardClicked;
	
	private float timer;
	private float refreshMarketButtonTimer;
	private bool isSceneLoaded;

	private bool toUpdateCardsMarketFeatures;
	private bool areNewCardsAvailable;
	private bool isScrolling;
	private float scrollIntersection;

	private bool isLeftClicked;
	private float clickInterval;

	private bool toSlideLeft;
	private bool toSlideRight;
	private bool isSlidingCursors;
	private bool filtersDisplayed;
	private bool mainContentDisplayed;
	private bool marketContentDisplayed;
	
	private float filtersPositionX;
	private float mainContentPositionX;
	private float marketContentPositionX;
	private float targetContentPositionX;

	void Update()
	{	
		this.timer += Time.deltaTime;

		if (Input.touchCount == 1 && this.isSceneLoaded && !this.isSlidingCursors && !this.isCardFocusedDisplayed && TutorialObjectController.instance.getCanSwipe() && MenuController.instance.getCanSwipeAndScroll()) 
		{
			if(Mathf.Abs(Input.touches[0].deltaPosition.y)>1f && Mathf.Abs(Input.touches[0].deltaPosition.y)>Mathf.Abs(Input.touches[0].deltaPosition.x))
			{
				this.isLeftClicked=false;
			}
			else if(Input.touches[0].deltaPosition.x<-15f)
			{
				this.isLeftClicked=false;
				if(this.marketContentDisplayed || this.mainContentDisplayed || this.toSlideLeft)
				{
					this.slideRight();
				}
			}
			else if(Input.touches[0].deltaPosition.x>15f)
			{
				this.isLeftClicked=false;
				if(this.mainContentDisplayed || this.filtersDisplayed || this.toSlideRight)
				{
					this.slideLeft();
				}
			}
		}
		if(isLeftClicked)
		{
			this.clickInterval=this.clickInterval+Time.deltaTime*10f;
			if (Input.touchCount == 1 && Mathf.Abs(Input.touches[0].deltaPosition.y)>1f) 
			{
				this.isLeftClicked=false;
			}
		}
		if (this.timer > this.refreshInterval) 
		{	
			this.timer=this.timer-this.refreshInterval;
			if(this.activeTab==0)
			{
				StartCoroutine(this.refreshMarket());
			}
			else
			{
				StartCoroutine(this.refreshMyCards());
			}
		}
		if(isSearchingSkill)
		{
			if(ApplicationDesignRules.isMobileDevice)
			{
				this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().setText(this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().getText());
			}
			if(this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().getText().ToLower()!=this.valueSkill.ToLower()	)
			{
				this.valueSkill=this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().getText();
				this.setSkillAutocompletion();
			}
			if(!this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().getIsBeingUsed())
			{
					this.isSearchingSkill=false;
					this.cleanSkillAutocompletion();
					this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().setText("Rechercher");
					this.valueSkill="Rechercher";
			}
		}
		if(this.areNewCardsAvailable)
		{
			if(!this.isCardFocusedDisplayed)
			{
				this.refreshMarketButtonTimer += Time.deltaTime;
				if(this.refreshMarketButtonTimer>0.5f)
				{
					this.refreshMarketButtonTimer=0f;
					this.refreshMarketButton.GetComponent<NewMarketRefreshButtonController>().changeColor();
				}
			}
		}
		if(toSlideRight || toSlideLeft)
		{
			Vector3 mainCameraPosition = this.upperScrollCamera.transform.position;
			Vector3 cardsCameraPosition = this.lowerScrollCamera.transform.position;
			float camerasXPosition = mainCameraPosition.x;
			if(toSlideRight)
			{
				camerasXPosition=camerasXPosition+Time.deltaTime*40f;
				if(camerasXPosition>this.targetContentPositionX)
				{
					camerasXPosition=this.targetContentPositionX;
					this.toSlideRight=false;
					if(camerasXPosition==this.filtersPositionX)
					{
						this.filtersDisplayed=true;
						this.skillSearchBar.SetActive(true);
					}
					else if(camerasXPosition==this.mainContentPositionX)
					{
						this.mainContentDisplayed=true;
					}
				}
			}
			else if(toSlideLeft)
			{
				camerasXPosition=camerasXPosition-Time.deltaTime*40f;
				if(camerasXPosition<this.targetContentPositionX)
				{
					camerasXPosition=this.targetContentPositionX;
					this.toSlideLeft=false;
					if(camerasXPosition==this.marketContentPositionX)
					{
						this.marketContentDisplayed=true;
					}
					else if(camerasXPosition==this.mainContentPositionX)
					{
						this.mainContentDisplayed=true;
					}
				}
			}
			mainCameraPosition.x=camerasXPosition;
			cardsCameraPosition.x=camerasXPosition;
			this.upperScrollCamera.transform.position=mainCameraPosition;
			this.lowerScrollCamera.transform.position=cardsCameraPosition;
		}
		if(ApplicationDesignRules.isMobileScreen && this.isSceneLoaded && !this.isLeftClicked && this.mainContentDisplayed && TutorialObjectController.instance.getCanScroll() && MenuController.instance.getCanSwipeAndScroll())
		{
			isScrolling = this.lowerScrollCamera.GetComponent<ScrollingController>().ScrollController();
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewMarketModel ();
		this.skillSearchBar = GameObject.Find ("SkillSearchBar");
		this.sortingOrder = -1;
		this.activeTab = 0;
		this.scrollIntersection = 1.85f;
		this.mainContentDisplayed = true;
		this.initializeScene ();
		this.startMenuInitialization ();
	}
	private void startMenuInitialization()
	{
		this.menu = GameObject.Find ("Menu");
		this.menu.AddComponent<MarketMenuController> ();
	}
	public void endMenuInitialization()
	{
		this.startTutorialInitialization ();
	}
	private void startTutorialInitialization()
	{
		this.tutorial = GameObject.Find ("Tutorial");
		this.tutorial.AddComponent<MarketTutorialController>();
	}
	public void endTutorialInitialization()
	{
		this.initialization ();
	}
	public void initialization()
	{
		this.resize ();
		StartCoroutine(this.selectATab (true));
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
		this.cardsPaginationButtons.GetComponent<NewMarketPaginationController> ().p = cardsPagination;
		this.cardsPaginationButtons.GetComponent<NewMarketPaginationController> ().setPagination ();
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
		if(ApplicationDesignRules.isMobileScreen)
		{
			Vector3 lowerScrollCameraPosition = this.lowerScrollCamera.transform.position;
			lowerScrollCameraPosition.y=this.lowerScrollCamera.GetComponent<ScrollingController>().getStartPositionY();
			this.lowerScrollCamera.transform.position=lowerScrollCameraPosition;
		}
	}
	public void selectATabHandler(int idTab)
	{
		this.activeTab = idTab;
		StartCoroutine(this.selectATab ());
	}
	private IEnumerator selectATab()
	{
		bool firstLoad=false;
		if(ApplicationDesignRules.isMobileScreen)
		{
			this.hideActiveTab();
		}
		for(int i=0;i<this.tabs.Length;i++)
		{
			if(i==this.activeTab)
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(1);
				this.tabs[i].GetComponent<NewMarketTabController>().setIsSelected(true);
				this.tabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(0);
				this.tabs[i].GetComponent<NewMarketTabController>().reset();
			}
		}
		MenuController.instance.displayLoadingScreen ();
		this.isSceneLoaded = false;
		yield return StartCoroutine (model.initializeMarket (this.totalNbResultLimit,this.activeTab,firstLoad));
		this.initializeCards ();
		this.isSceneLoaded = true;
		MenuController.instance.hideLoadingScreen ();
		if(firstLoad)
		{
			if(model.player.TutorialStep!=-1)
			{
				TutorialObjectController.instance.startTutorial(model.player.TutorialStep,model.player.displayTutorial);
			}
			else if(model.player.displayTutorial&&!model.player.MarketTutorial)
			{
				TutorialObjectController.instance.startHelp();
			}
		}
		switch(this.activeTab)
		{
		case 0: case 1:
			this.priceFilterTitle.SetActive(true);
			this.priceFilter.SetActive(true);
			break;
		case 2:
			this.priceFilterTitle.SetActive(false);
			this.priceFilter.SetActive(false);
			break;
		}
	}
	private IEnumerator selectATab(bool firstLoad)
	{
		for(int i=0;i<this.tabs.Length;i++)
		{
			if(i==this.activeTab)
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(1);
				this.tabs[i].GetComponent<NewMarketTabController>().setIsSelected(true);
				this.tabs[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				this.tabs[i].GetComponent<SpriteRenderer>().sprite=MenuController.instance.returnTabPicture(0);
				this.tabs[i].GetComponent<NewMarketTabController>().reset();
			}
		}
		MenuController.instance.displayLoadingScreen ();
		this.isSceneLoaded = false;
		yield return StartCoroutine (model.initializeMarket (this.totalNbResultLimit,this.activeTab,firstLoad));
		this.initializeCards ();
		this.isSceneLoaded = true;
		MenuController.instance.hideLoadingScreen ();
		if(firstLoad)
		{
			if(model.player.TutorialStep!=-1)
			{
				TutorialObjectController.instance.startTutorial(model.player.TutorialStep,model.player.displayTutorial);
			}
			else if(model.player.displayTutorial&&!model.player.MarketTutorial)
			{
				TutorialObjectController.instance.startHelp();
			}
		}
		switch(this.activeTab)
		{
		case 0: case 1:
			this.priceFilterTitle.SetActive(true);
			this.priceFilter.SetActive(true);
			break;
		case 2:
			this.priceFilterTitle.SetActive(false);
			this.priceFilter.SetActive(false);
			break;
		}
	}
	public void initializeScene()
	{
		this.cardsBlock = Instantiate (this.blockObject) as GameObject;
		this.cardsBlockTitle = GameObject.Find ("CardsBlockTitle");
		this.cardsNumberTitle = GameObject.Find ("CardsNumberTitle");
		this.cardsNumberTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.cardsPaginationButtons = GameObject.Find("Pagination");
		this.cardsPaginationButtons.AddComponent<NewMarketPaginationController> ();
		this.cardsPaginationButtons.GetComponent<NewMarketPaginationController> ().initialize ();
		this.cardsPaginationLine = GameObject.Find ("CardsPaginationLine");
		this.cardsPaginationLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.cardsScrollLine = GameObject.Find ("CardsScrollLine");
		this.cardsScrollLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.cards=new GameObject[0];
		this.refreshMarketButton = GameObject.Find ("RefreshMarketButton");
		this.refreshMarketButton.GetComponent<TextMeshPro> ().text = "Actualiser".ToUpper();
		this.refreshMarketButton.AddComponent<NewMarketRefreshButtonController> ();
		this.refreshMarketButton.AddComponent<BoxCollider2D> ();
		this.refreshMarketButton.SetActive (false);
		
		this.filtersBlock = Instantiate (this.blockObject) as GameObject;
		this.filtersBlockTitle = GameObject.Find ("FiltersBlockTitle");
		this.filtersBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.filtersBlockTitle.GetComponent<TextMeshPro> ().text = "Filtrer";

		this.tabs=new GameObject[3];
		for(int i=0;i<this.tabs.Length;i++)
		{
			this.tabs[i]=GameObject.Find ("Tab"+i);
			this.tabs[i].AddComponent<NewMarketTabController>();
			this.tabs[i].GetComponent<NewMarketTabController>().setId(i);
		}
		this.tabs[0].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Offres");
		this.tabs[1].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Mes ventes");
		this.tabs[2].transform.FindChild("Title").GetComponent<TextMeshPro> ().text = ("Mes Unités");
	
		this.cardsTypeFilters = new GameObject[10];
		for(int i=0;i<this.cardsTypeFilters.Length;i++)
		{
			this.cardsTypeFilters[i]=GameObject.Find("CardTypeFilter"+i);
			this.cardsTypeFilters[i].AddComponent<NewMarketCardTypeFilterController>();
			this.cardsTypeFilters[i].GetComponent<NewMarketCardTypeFilterController>().setId(i);
		}
		this.valueFilters=new GameObject[3];
		for(int i=0;i<this.valueFilters.Length;i++)
		{
			this.valueFilters[i]=GameObject.Find ("ValueFilter"+i);
		}
		this.skillSearchBarTitle = GameObject.Find ("SkillSearchTitle");
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().text = "Compétence".ToUpper ();
		this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController> ().setText ("Rechercher");
		this.skillChoices=new GameObject[3];
		for(int i=0;i<this.skillChoices.Length;i++)
		{
			this.skillChoices[i]=GameObject.Find("SkillChoice"+i);
			this.skillChoices[i].AddComponent<NewMarketSkillChoiceController>();
			this.skillChoices[i].GetComponent<NewMarketSkillChoiceController>().setId(i);
			this.skillChoices[i].SetActive(false);
		}
		this.cardTypeFilterTitle = GameObject.Find ("CardTypeFilterTitle");
		this.cardTypeFilterTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.cardTypeFilterTitle.GetComponent<TextMeshPro> ().text = "Faction".ToUpper ();
		this.valueFilterTitle = GameObject.Find ("ValueFilterTitle");
		this.valueFilterTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.valueFilterTitle.GetComponent<TextMeshPro> ().text = "Attribut".ToUpper ();
		this.priceFilterTitle = GameObject.Find ("PriceFilterTitle");
		this.priceFilterTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.priceFilterTitle.GetComponent<TextMeshPro> ().text = "Prix".ToUpper ();
		
		this.cursors=new GameObject[this.valueFilters.Length];
		for (int i=0;i<this.valueFilters.Length;i++)
		{
			this.cursors[i]=this.valueFilters[i].transform.FindChild("Slider").FindChild("Cursor").gameObject;
			this.cursors[i].AddComponent<NewMarketCursorController>();
			this.cursors[i].GetComponent<NewMarketCursorController>().setId(i);
		}
		this.priceFilter = GameObject.Find ("PriceFilter");
		this.priceCursors = new GameObject[2];
		for(int i=0;i<this.priceCursors.Length;i++)
		{
			this.priceCursors[i]=this.priceFilter.transform.FindChild("Slider").FindChild("Cursor"+i).gameObject;
			this.priceCursors[i].AddComponent<NewMarketPriceCursorController>();
			this.priceCursors[i].GetComponent<NewMarketPriceCursorController>().setId(i);
		}
		this.sortButtons=new GameObject[8];
		for(int i=0;i<2;i++)
		{
			this.sortButtons[i]=this.priceFilter.transform.FindChild("Sort"+i).gameObject;
		}
		for (int i=0;i<this.valueFilters.Length;i++)
		{
			this.sortButtons[i*2+2]=this.valueFilters[i].transform.FindChild("Sort0").gameObject;
			this.sortButtons[i*2+3]=this.valueFilters[i].transform.FindChild("Sort1").gameObject;
		}
		for(int i=0;i<this.sortButtons.Length;i++)
		{
			this.sortButtons[i].AddComponent<NewMarketSortButtonController>();
			this.sortButtons[i].GetComponent<NewMarketSortButtonController>().setId(i);
		}
		this.marketBlock = Instantiate (this.blockObject) as GameObject;
		this.marketBlockTitle = GameObject.Find ("MarketBlockTitle");
		this.marketBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.marketBlockTitle.GetComponent<TextMeshPro> ().text = "Le marché";
		this.marketSubtitle = GameObject.Find ("MarketSubtitle");
		this.marketSubtitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.marketSubtitle.GetComponent<TextMeshPro> ().text = "Bienvenue sur le marché. Vous pouvez ici acheter des cartes à d'autres joueurs et vendre vos cartes. Attention seules les cartes qui ne sont pas déjà ratachées à une équipe peuvent être mises en vente";

		this.filterButton = GameObject.Find ("FilterButton");
		this.filterButton.AddComponent<NewMarketFiltersButtonController> ();
		this.informationButton = GameObject.Find ("InformationButton");
		this.informationButton.AddComponent<NewMarketInformationButtonController> ();
		this.slideLeftButton = GameObject.Find ("SlideLeftButton");
		this.slideLeftButton.AddComponent<NewMarketSlideLeftButtonController> ();
		this.slideRightButton = GameObject.Find ("SlideRightButton");
		this.slideRightButton.AddComponent<NewMarketSlideRightButtonController> ();

		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardMarketController> ();
		this.focusedCard.SetActive (false);
		this.mainCamera = gameObject;
		this.sceneCamera = GameObject.Find ("sceneCamera");
		this.upperScrollCamera = GameObject.Find ("UpperScrollCamera");
		this.lowerScrollCamera = GameObject.Find ("LowerScrollCamera");
		this.lowerScrollCamera.AddComponent<ScrollingController> ();
		this.tutorialCamera = GameObject.Find ("TutorialCamera");
		this.backgroundCamera = GameObject.Find ("BackgroundCamera");
	}
	private void resetFiltersValue()
	{
		if(model.cards.getCount()>0)
		{
			this.maxPriceLimit=model.cards.getCard(0).Price;
			this.minPriceLimit=model.cards.getCard(0).Price;
		}
		for(int i=0;i<model.cards.getCount();i++)
		{
			if(model.cards.getCard(i).Price>maxPriceLimit)
			{
				this.maxPriceLimit=model.cards.getCard(i).Price;
			}
			if(model.cards.getCard(i).Price<minPriceLimit)
			{
				this.minPriceLimit=model.cards.getCard(i).Price;
			}
		}
		this.minPriceVal = this.minPriceLimit;
		this.maxPriceVal = this.maxPriceLimit;

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
			this.cardsTypeFilters[i].GetComponent<NewMarketCardTypeFilterController>().reset();
		}
		this.valueSkill = "";
		this.isSkillChosen = false;
		
		this.cleanSkillAutocompletion ();
		this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().setText("Rechercher");
		if(this.sortingOrder!=-1)
		{
			this.sortButtons[this.sortingOrder].GetComponent<NewMarketSortButtonController>().setIsSelected(false);
			this.sortButtons[this.sortingOrder].GetComponent<NewMarketSortButtonController>().reset ();
			this.sortingOrder = -1;
		}
	}
	public void resize()
	{
		float cardsBlockLeftMargin;
		float cardsBlockUpMargin;
		float cardsBlockHeight;
		
		float marketBlockLeftMargin;
		float marketBlockUpMargin;
		float marketBlockHeight;
		
		float filtersBlockLeftMargin;
		float filtersBlockUpMargin;
		float filtersBlockHeight;

		float firstLineY;
		float gapBetweenSelectionsButtons = 0.02f;

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

			firstLineY = 2.2f;

			cardsBlockHeight=5f+this.nbLines*(ApplicationDesignRules.cardWorldSize.y+ApplicationDesignRules.gapBetweenMarketCardsLine);

			marketBlockHeight=ApplicationDesignRules.viewHeight;
			marketBlockLeftMargin=-ApplicationDesignRules.worldWidth;
			marketBlockUpMargin=0f;
			
			cardsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			cardsBlockUpMargin=ApplicationDesignRules.tabWorldSize.y;
			
			filtersBlockHeight=ApplicationDesignRules.viewHeight;
			filtersBlockLeftMargin=ApplicationDesignRules.worldWidth+ApplicationDesignRules.leftMargin;
			filtersBlockUpMargin=0f;

			this.upperScrollCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.worldHeight-ApplicationDesignRules.upMargin-this.scrollIntersection)/ApplicationDesignRules.worldHeight,1f,(this.scrollIntersection)/ApplicationDesignRules.worldHeight);
			this.upperScrollCamera.GetComponent<Camera> ().orthographicSize = this.scrollIntersection/2f;
			this.upperScrollCamera.transform.position = new Vector3 (0f, ApplicationDesignRules.worldHeight/2f-this.scrollIntersection/2f, -10f);
			
			this.lowerScrollCamera.GetComponent<Camera> ().rect = new Rect (0f,(ApplicationDesignRules.downMargin)/ApplicationDesignRules.worldHeight,1f,(ApplicationDesignRules.viewHeight-this.scrollIntersection)/ApplicationDesignRules.worldHeight);
			this.lowerScrollCamera.GetComponent<Camera> ().orthographicSize = (ApplicationDesignRules.viewHeight-this.scrollIntersection)/2f;
			this.lowerScrollCamera.GetComponent<ScrollingController> ().setViewHeight(ApplicationDesignRules.viewHeight-this.scrollIntersection);
			this.lowerScrollCamera.transform.position = new Vector3 (0f, ApplicationDesignRules.worldHeight/2f-this.scrollIntersection-(ApplicationDesignRules.viewHeight-this.scrollIntersection)/2f, -10f);
			this.lowerScrollCamera.GetComponent<ScrollingController> ().setStartPositionY (this.lowerScrollCamera.transform.position.y);

			this.cardsScrollLine.SetActive(true);
			this.cardsBlockTitle.SetActive(true);
			this.lowerScrollCamera.SetActive(true);
			this.cardsPaginationLine.SetActive(false);
			this.toSlideLeft=false;
			this.toSlideRight=false;
			this.mainContentDisplayed=true;
			this.filterButton.SetActive(true);
			this.informationButton.SetActive(true);
			this.slideLeftButton.SetActive(true);
			this.slideRightButton.SetActive(true);
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
			firstLineY = 1.9f;

			cardsBlockHeight=ApplicationDesignRules.largeBlockHeight-ApplicationDesignRules.tabWorldSize.y;

			marketBlockHeight=ApplicationDesignRules.mediumBlockHeight;
			marketBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			marketBlockUpMargin=ApplicationDesignRules.upMargin;
			
			filtersBlockHeight=ApplicationDesignRules.smallBlockHeight;
			filtersBlockLeftMargin=ApplicationDesignRules.leftMargin+ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.blockWidth;
			filtersBlockUpMargin=marketBlockUpMargin+ApplicationDesignRules.gapBetweenBlocks+marketBlockHeight;
			
			cardsBlockLeftMargin=ApplicationDesignRules.leftMargin;
			cardsBlockUpMargin=ApplicationDesignRules.upMargin+ApplicationDesignRules.tabWorldSize.y;

			this.cardsPerLine = 4;
			this.nbLines = 2;
			this.sceneCamera.SetActive(true);
			this.cardsBlockTitle.SetActive(false);
			this.cardsScrollLine.SetActive(false);
			this.lowerScrollCamera.SetActive(false);
			this.upperScrollCamera.SetActive(false);
			this.cardsPaginationLine.SetActive(true);
			this.filterButton.SetActive(false);
			this.informationButton.SetActive(false);
			this.slideLeftButton.SetActive(false);
			this.slideRightButton.SetActive(false);
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

		this.valueFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.skillSearchBarTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.skillSearchBar.transform.localScale = ApplicationDesignRules.inputTextScale;

		this.priceFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
	
		this.priceFilter.transform.localScale=ApplicationDesignRules.valueFilterScale;
		
		this.cardsBlock.GetComponent<NewBlockController> ().resize(cardsBlockLeftMargin,cardsBlockUpMargin,ApplicationDesignRules.blockWidth,cardsBlockHeight);
		Vector3 cardsBlockUpperLeftPosition = this.cardsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 cardsBlockLowerLeftPosition = this.cardsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector3 cardsBlockUpperRightPosition = this.cardsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 cardsBlockSize = this.cardsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 cardsBlockOrigin = this.cardsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.cardsNumberTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.refreshMarketButton.transform.localScale= ApplicationDesignRules.subMainTitleScale;

		this.filterButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.filterButton.transform.position = new Vector3 (cardsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - ApplicationDesignRules.roundButtonWorldSize.x / 2f, cardsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		this.informationButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.informationButton.transform.position = new Vector3 (cardsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 1.5f*ApplicationDesignRules.roundButtonWorldSize.x, cardsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		float gapBetweenCards = (cardsBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing - 4f * ApplicationDesignRules.cardWorldSize.x) / 3f;

		this.cards=new GameObject[this.cardsPerLine*this.nbLines];

		for(int j=0;j<this.nbLines;j++)
		{
			for(int i =0;i<this.cardsPerLine;i++)
			{
				this.cards[j*(cardsPerLine)+i]=Instantiate (this.cardObject) as GameObject;
				this.cards[j*(cardsPerLine)+i].AddComponent<NewCardMarketController>();
				this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardMarketController>().setId(j*(cardsPerLine)+i);
				this.cards[j*(cardsPerLine)+i].transform.localScale= ApplicationDesignRules.cardScale;
				this.cards[j*(cardsPerLine)+i].transform.position=new Vector3(cardsBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+ApplicationDesignRules.cardWorldSize.x/2f+i*(gapBetweenCards+ApplicationDesignRules.cardWorldSize.x),cardsBlockUpperRightPosition.y-firstLineY-j*(ApplicationDesignRules.gapBetweenMarketCardsLine+ApplicationDesignRules.cardHaloWorldSize.y),0f);
			}
		}

		float lineScale = ApplicationDesignRules.getLineScale (cardsBlockSize.x - 2f*ApplicationDesignRules.blockHorizontalSpacing);

		this.marketBlock.GetComponent<NewBlockController> ().resize(marketBlockLeftMargin,marketBlockUpMargin,ApplicationDesignRules.blockWidth,marketBlockHeight);
		Vector3 marketBlockUpperLeftPosition = this.marketBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 marketBlockUpperRightPosition = this.marketBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector3 marketBlockLowerLeftPosition = this.marketBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector2 marketBlockSize = this.marketBlock.GetComponent<NewBlockController> ().getSize ();
		Vector2 marketBlockOrigin = this.marketBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.marketBlockTitle.transform.position = new Vector3 (marketBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, marketBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
		this.marketBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		this.marketSubtitle.transform.position = new Vector3 (marketBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, marketBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
		this.marketSubtitle.transform.GetComponent<TextContainer>().width=marketBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing;
		this.marketSubtitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		this.mainContentPositionX = cardsBlockOrigin.x;
		this.marketContentPositionX=marketBlockOrigin.x;
		this.filtersPositionX = filtersBlockOrigin.x;

		this.cardsScrollLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.cardsScrollLine.transform.position = new Vector3 (cardsBlockLowerLeftPosition.x + cardsBlockSize.x / 2, cardsBlockUpperLeftPosition.y-1.15f, 0f);
		this.cardsPaginationLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.cardsPaginationLine.transform.position = new Vector3 (cardsBlockLowerLeftPosition.x + cardsBlockSize.x / 2, cardsBlockLowerLeftPosition.y + 0.45f, 0f);

		this.cardsPaginationButtons.transform.GetComponent<NewMarketPaginationController> ().resize ();

		this.focusedCard.transform.localScale = ApplicationDesignRules.cardFocusedScale;
		this.focusedCard.transform.position = ApplicationDesignRules.focusedCardPosition;
		this.focusedCard.GetComponent<NewFocusedCardMarketController> ().resize ();

		this.slideLeftButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.slideLeftButton.transform.position = new Vector3 (filtersBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.roundButtonWorldSize.x/2f, filtersBlockUpperRightPosition.y -ApplicationDesignRules.buttonVerticalSpacing- ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		this.slideRightButton.transform.localScale = ApplicationDesignRules.roundButtonScale;
		this.slideRightButton.transform.position = new Vector3 (marketBlockUpperRightPosition.x -ApplicationDesignRules.blockHorizontalSpacing-ApplicationDesignRules.roundButtonWorldSize.x/2f, marketBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing- ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);

		if(ApplicationDesignRules.isMobileScreen)
		{
			this.cardsBlockTitle.transform.position = new Vector3 (cardsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, cardsBlockUpperLeftPosition.y - ApplicationDesignRules.mainTitleVerticalSpacing, 0f);
			this.cardsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
			this.cardsBlockTitle.transform.GetComponent<TextContainer>().width=ApplicationDesignRules.blockWidth-2f*ApplicationDesignRules.blockHorizontalSpacing-3f*ApplicationDesignRules.roundButtonWorldSize.x;
			this.refreshMarketButton.transform.position=new Vector3 (cardsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing, cardsBlockUpperRightPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);

			this.cardsPaginationButtons.transform.localPosition=new Vector3 (cardsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing - 3f*ApplicationDesignRules.roundButtonWorldSize.x, cardsBlockUpperRightPosition.y - ApplicationDesignRules.buttonVerticalSpacing - ApplicationDesignRules.roundButtonWorldSize.y / 2f, 0f);
			this.cardsNumberTitle.transform.position = new Vector3 (cardsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, cardsBlockUpperRightPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
			for(int i=0;i<this.tabs.Length;i++)
			{
				this.tabs[i].transform.localScale = ApplicationDesignRules.tabScale;
			}
			this.tabs[0].transform.position = new Vector3 (cardsBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f, cardsBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
			this.tabs[2].transform.position = new Vector3 (cardsBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f+ ApplicationDesignRules.tabWorldSize.x+gapBetweenSelectionsButtons, cardsBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
			this.hideActiveTab();

			this.skillSearchBarTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Left;
			this.skillSearchBarTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Left;
			this.skillSearchBarTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x, filtersBlockUpperLeftPosition.y - ApplicationDesignRules.subMainTitleVerticalSpacing, 0f);
			this.skillSearchBar.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x+ApplicationDesignRules.inputTextWorldSize.x/2f, filtersBlockUpperLeftPosition.y - 1.375f, 0f);
			for(int i=0;i<this.skillChoices.Length;i++)
			{
				this.skillChoices[i].transform.localScale=ApplicationDesignRules.listElementScale;
				this.skillChoices[i].transform.position=new Vector3(this.skillSearchBar.transform.position.x,this.skillSearchBar.transform.position.y-ApplicationDesignRules.inputTextWorldSize.y/2f-(i+0.5f)*ApplicationDesignRules.listElementWorldSize.y+i*0.02f,-1f);
			}
			this.cardTypeFilterTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Left;
			this.cardTypeFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Left;
			this.cardTypeFilterTitle.transform.position = new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x, filtersBlockUpperLeftPosition.y - 1.9f, 0f);
			float gapBetweenCardTypesFilters = (filtersBlockSize.x-2f*ApplicationDesignRules.blockHorizontalSpacing-5f*ApplicationDesignRules.cardTypeFilterWorldSize.x)/4f;
			for(int i = 0;i<this.cardsTypeFilters.Length;i++)
			{
				Vector3 cardTypeFilterPosition=new Vector3();
				
				if(i<5)
				{
					cardTypeFilterPosition.y=filtersBlockUpperLeftPosition.y-2.65f;
					cardTypeFilterPosition.x=filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+0.5f*ApplicationDesignRules.cardTypeFilterWorldSize.x+i*(ApplicationDesignRules.cardTypeFilterWorldSize.x+gapBetweenCardTypesFilters);
					
				}
				else
				{
					cardTypeFilterPosition.y=filtersBlockUpperLeftPosition.y-3.80f;
					cardTypeFilterPosition.x=filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+0.5f*ApplicationDesignRules.cardTypeFilterWorldSize.x+(i-5)*(ApplicationDesignRules.cardTypeFilterWorldSize.x+gapBetweenCardTypesFilters);
					
				}
				this.cardsTypeFilters[i].transform.position=cardTypeFilterPosition;
				this.cardsTypeFilters[i].transform.localScale=ApplicationDesignRules.cardTypeFilterScale;
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

			this.priceFilterTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Left;
			this.priceFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Left;
			this.priceFilterTitle.transform.position=new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x, filtersBlockUpperLeftPosition.y - 6.9f, 0f);
			this.priceFilter.transform.position=new Vector3(filtersBlockUpperLeftPosition.x+ApplicationDesignRules.blockHorizontalSpacing+1.62f,filtersBlockUpperLeftPosition.y - 7.45f,0f);
			this.priceFilter.transform.FindChild("Sort0").localScale=new Vector3(0.55f,0.55f,0.55f);
			this.priceFilter.transform.FindChild("Sort1").localScale=new Vector3(0.55f,0.55f,0.55f);
			this.priceFilter.transform.FindChild("Sort0").localPosition=new Vector3(1.14f,0.135f,0f);
			this.priceFilter.transform.FindChild("Sort1").localPosition=new Vector3(1.53f,0.135f,0f);
		}
		else
		{
			this.cardsPaginationButtons.transform.localPosition=new Vector3(cardsBlockLowerLeftPosition.x+cardsBlockSize.x/2f, cardsBlockLowerLeftPosition.y + 0.2f, 0f);
			this.cardsNumberTitle.transform.position = new Vector3 (cardsBlockUpperLeftPosition.x + ApplicationDesignRules.blockHorizontalSpacing, cardsBlockUpperLeftPosition.y - 0.25f, 0f);
			this.cardsBlockTitle.transform.GetComponent<TextContainer>().width=ApplicationDesignRules.blockWidth-2f;
			this.refreshMarketButton.transform.position=new Vector3 (cardsBlockUpperRightPosition.x - ApplicationDesignRules.blockHorizontalSpacing, cardsBlockUpperRightPosition.y - 0.25f, 0f);
			for(int i=0;i<this.tabs.Length;i++)
			{
				this.tabs[i].transform.localScale = ApplicationDesignRules.tabScale;
				this.tabs[i].transform.position = new Vector3 (cardsBlockUpperLeftPosition.x + ApplicationDesignRules.tabWorldSize.x / 2f+ i*(ApplicationDesignRules.tabWorldSize.x+gapBetweenSelectionsButtons), cardsBlockUpperLeftPosition.y+ApplicationDesignRules.tabWorldSize.y/2f,0f);
				this.tabs[i].SetActive(true);
			}
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

			this.priceFilterTitle.GetComponent<TextContainer>().anchorPosition =  TextContainerAnchors.Middle;
			this.priceFilterTitle.GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
			this.priceFilterTitle.transform.position=new Vector3 (ApplicationDesignRules.blockHorizontalSpacing+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 2f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 2.25f, 0f);

			this.priceFilter.transform.position=new Vector3(priceFilterTitle.transform.position.x,filtersBlockUpperLeftPosition.y - 3f,0f);
			this.priceFilter.transform.FindChild("Sort0").localScale=new Vector3(0.32f,0.32f,0.32f);
			this.priceFilter.transform.FindChild("Sort1").localScale=new Vector3(0.32f,0.32f,0.32f);
			this.priceFilter.transform.FindChild("Sort0").localPosition=new Vector3(0.5594f,0.38f,0f);
			this.priceFilter.transform.FindChild("Sort1").localPosition=new Vector3(0.8004f,0.38f,0f);

		}
		this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().resize();
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
		this.updateCardsMarketFeatures ();
		if(ApplicationDesignRules.isMobileScreen)
		{
			int nbLinesToDisplay = Mathf.CeilToInt ((float)this.cardsDisplayed.Count / (float)this.cardsPerLine);
			float contentHeight = 1f+(nbLinesToDisplay-1)*(ApplicationDesignRules.cardWorldSize.y+ApplicationDesignRules.gapBetweenMarketCardsLine);
			this.lowerScrollCamera.GetComponent<ScrollingController> ().setContentHeight(contentHeight);
			this.lowerScrollCamera.GetComponent<ScrollingController>().setEndPositionY();
		}
	}
	public void cleanCards()
	{
		for(int i=0;i<this.cards.Length;i++)
		{
			this.cards[i].GetComponent<NewFocusedCardController>().closePopUps();
			Destroy(this.cards[i]);
		}
	}
	public void showCardFocused()
	{
		this.isCardFocusedDisplayed = true;
		this.displayBackUI (false);
		this.focusedCard.SetActive (true);
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		this.focusedCard.GetComponent<NewFocusedCardController>().c=model.cards.getCard(this.cardsDisplayed[this.idCardClicked]);
		this.focusedCard.GetComponent<NewFocusedCardController> ().show ();
	}
	public void hideCardFocused()
	{
		this.isCardFocusedDisplayed = false;
		this.displayBackUI (true);
		this.focusedCard.SetActive (false);
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
				this.skillSearchBar.SetActive(false);
			}
			else
			{
				this.skillSearchBar.SetActive(true);
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
			this.skillSearchBar.SetActive(false);
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
		this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().setText("");
		this.valueSkill = "";
		this.isSearchingSkill = true;
	}
	public void cardTypeFilterHandler(int id)
	{
		if(!ApplicationDesignRules.isMobileScreen || this.filtersDisplayed)
		{
			if(this.filtersCardType.Contains(id))
			{
				this.filtersCardType.Remove(id);
				this.cardsTypeFilters[id].GetComponent<NewMarketCardTypeFilterController>().reset();
			}
			else
			{
				this.filtersCardType.Add (id);
				this.cardsTypeFilters[id].GetComponent<NewMarketCardTypeFilterController>().setIsSelected(true);
				this.cardsTypeFilters[id].GetComponent<NewMarketCardTypeFilterController>().setHoveredState();
			}
			this.cardsPagination.chosenPage = 0;
			this.applyFilters ();
		}
	}
	public void sortButtonHandler(int id)
	{
		if(!ApplicationDesignRules.isMobileScreen || this.filtersDisplayed)
		{
			if(this.sortingOrder==id)
			{
				this.sortingOrder = -1;
				this.sortButtons[id].GetComponent<NewMarketSortButtonController>().reset();
			}
			else if(this.sortingOrder!=-1)
			{
				this.sortButtons[this.sortingOrder].GetComponent<NewMarketSortButtonController>().reset();
				this.sortButtons[id].GetComponent<NewMarketSortButtonController>().setIsSelected(true);
				this.sortButtons[id].GetComponent<NewMarketSortButtonController>().setHoveredState();
				this.sortingOrder = id;
			}
			else
			{
				this.sortingOrder=id;
				this.sortButtons[id].GetComponent<NewMarketSortButtonController>().setIsSelected(true);
				this.sortButtons[id].GetComponent<NewMarketSortButtonController>().setHoveredState();
			}
			this.cardsPagination.chosenPage = 0;
			this.applyFilters ();
		}
	}
	public void startSlidingCursors()
	{
		this.isSlidingCursors=true;
	}
	public void moveCursors(int cursorId)
	{
		this.isSlidingCursors=true;
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
	}
	public void endSlidingCursors()
	{
		this.isSlidingCursors=false;
	}
	public void moveMinMaxCursor(int cursorId)
	{
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
		Vector3 cursorPosition = this.priceCursors [cursorId].transform.position;
		float sliderPositionX = this.priceCursors [cursorId].transform.parent.gameObject.transform.position.x;
		float cursorSizeX = ApplicationDesignRules.cursorWorldSize.x;
		float offset = mousePositionX-cursorPosition.x;
		cursorPosition.x = cursorPosition.x + offset;
		int secondCursorId;
		float secondCursorPositionX;
		float distance;
		if(cursorId%2==0)
		{
			secondCursorId=cursorId+1;
			secondCursorPositionX = this.priceCursors [secondCursorId].transform.position.x;
			if(cursorPosition.x>secondCursorPositionX-cursorSizeX)
			{
				cursorPosition.x=secondCursorPositionX-cursorSizeX;
			}
			else if(cursorPosition.x<-offsetStep+sliderPositionX)
			{
				cursorPosition.x=-offsetStep+sliderPositionX;
			}
			distance = cursorPosition.x -(-offsetStep+sliderPositionX);
		}
		else
		{
			secondCursorId=cursorId-1;
			secondCursorPositionX = this.priceCursors [secondCursorId].transform.position.x;
			if(cursorPosition.x>offsetStep+sliderPositionX)
			{
				cursorPosition.x=offsetStep+sliderPositionX;
			}
			else if(cursorPosition.x<secondCursorPositionX+cursorSizeX)
			{
				cursorPosition.x=secondCursorPositionX+cursorSizeX;
			}
			distance = (offsetStep+sliderPositionX)-cursorPosition.x;
		}
		this.priceCursors [cursorId].transform.position = cursorPosition;
		float maxDistance = 2 * offsetStep-cursorSizeX;
		float ratio = distance / maxDistance;
		if(ratio>0.99f)
		{
			ratio=1f;
		}
		else if(ratio<0.01f)
		{
			ratio=0f;
		}
		bool isMoved = false ;
		switch (cursorId) 
		{
		case 0:
			minPriceVal=minPriceLimit+Mathf.CeilToInt(ratio*(maxPriceLimit-minPriceLimit));
			if(minPriceVal!=oldMinPriceVal)
			{
				this.priceFilter.transform.FindChild ("MinValue").GetComponent<TextMeshPro>().text = minPriceVal.ToString();
				isMoved=true;
			}
			break;
		case 1:
			maxPriceVal=maxPriceLimit-Mathf.FloorToInt(ratio*(maxPriceLimit-minPriceLimit));
			if(maxPriceVal!=oldMaxPriceVal)
			{
				this.priceFilter.transform.FindChild ("MaxValue").GetComponent<TextMeshPro>().text = maxPriceVal.ToString();
				isMoved=true;
			}
			break;
		}
		if(isMoved)
		{
			this.cardsPagination.chosenPage = 0;
			this.applyFilters();
		}
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

			if(this.isSkillChosen && !model.cards.getCard (i).hasSkill(this.valueSkill.ToLower()))
			{
				continue;
			}
			if(nbFilters>0)
			{
				bool testCardTypes=false;
				for(int j=0;j<nbFilters;j++)
				{
					if (model.cards.getCard (i).IdClass == this.filtersCardType [j])
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
			if(model.cards.getCard(i).PowerLevel-1>=this.powerVal&&
			   model.cards.getCard(i).AttackLevel-1>=this.attackVal&&
			   model.cards.getCard(i).LifeLevel-1>=this.lifeVal&&
			   model.cards.getCard(i).SpeedLevel-1>=this.quicknessVal&&
			   model.cards.getCard(i).Price>=this.minPriceVal&&
			   model.cards.getCard(i).Price<=this.maxPriceVal)
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
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Price;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Price;
						break;
					case 1:
						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Price;
						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Price;
						break;
					case 2:
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Power;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Power;
						break;
					case 3:
						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Power;
						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Power;
						break;
					case 4:
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Attack;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Attack;
						break;
					case 5:
						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Attack;
						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Attack;
						break;
					case 6:
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Life;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Life;
						break;
					case 7:
						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Life;
						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Life;
						break;
//					case 8:
//						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Speed;
//						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Speed;
//						break;
//					case 9:
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
		this.priceFilter.transform.FindChild ("MinValue").GetComponent<TextMeshPro>().text = this.minPriceVal.ToString();
		this.priceFilter.transform.FindChild ("MaxValue").GetComponent<TextMeshPro>().text = this.maxPriceVal.ToString();
	}
	private void setSkillAutocompletion()
	{
		this.skillsDisplayed = new List<int> ();
		this.cleanSkillAutocompletion ();
		this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().setText(this.valueSkill);
		if(this.valueSkill.Length>0)
		{
			for (int i = 0; i < model.skillsList.Count; i++) 
			{  
				if (WordingSkills.getName(model.skillsList [i].Id).ToLower ().Contains (this.valueSkill.ToLower())) 
				{
					this.skillsDisplayed.Add (i);
					this.skillChoices[this.skillsDisplayed.Count-1].SetActive(true);
					this.skillChoices[this.skillsDisplayed.Count-1].GetComponent<NewMarketSkillChoiceController>().reset();
					this.skillChoices[this.skillsDisplayed.Count-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text = WordingSkills.getName(model.skillsList [i].Id);
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
		if(ApplicationDesignRules.isMobileDevice)
		{
			this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().closeKeyboard();
		}
		this.valueSkill = this.skillChoices[id].transform.FindChild("Title").GetComponent<TextMeshPro>().text;
		this.isSkillChosen = true;
		this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().setText(this.valueSkill);
		this.cleanSkillAutocompletion ();
		this.cardsPagination.chosenPage = 0;
		this.applyFilters ();
	}
	public void leftClickedHandler(int id)
	{ 
		if(!this.isScrolling)
		{
			this.idCardClicked = id;
			this.isLeftClicked = true;
			this.clickInterval = 0f;
		}
	}
	public void leftClickReleaseHandler()
	{
		if(isLeftClicked)
		{
			this.isLeftClicked=false;
			bool onSale=System.Convert.ToBoolean(model.cards.getCard(this.cardsDisplayed[this.idCardClicked]).onSale);
			bool isMine = model.cards.getCard (this.cardsDisplayed[this.idCardClicked]).isMine;
			int idOwner=model.cards.getCard(this.cardsDisplayed[this.idCardClicked]).IdOWner;
			if(idOwner!=-1 || isMine)
			{
				this.showCardFocused ();
			}
			else
			{
				MenuController.instance.displayErrorPopUp("Cette carte a été vendue, vous ne pouvez plus la consulter");
			}
		}
	}
	public void refreshCredits()
	{
		StartCoroutine(this.menu.GetComponent<MenuController> ().getUserData ());
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
		else
		{
			this.cards[this.idCardClicked].GetComponent<NewCardController>().returnPressed();
		}
	}
	public void escapePressed()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().escapePressed();
		}
		else if(!this.cards[this.idCardClicked].GetComponent<NewCardController>().closePopUps())
		{
			MenuController.instance.leaveGame();
		}
	}
	public void closeAllPopUp()
	{
		this.cards[this.idCardClicked].GetComponent<NewCardController>().escapePressed();
	}
	public void updateCardsMarketFeatures()
	{
		this.toUpdateCardsMarketFeatures=false;
		for (int i=0;i<this.cardsDisplayed.Count;i++)
		{
			this.cards[i].GetComponent<NewCardMarketController>().setMarketFeatures();
		}
	}
	public void communicateCardIndex(int id)
	{
		this.idCardClicked = id;
	}
	public bool canClick()
	{
		if (!toSlideLeft && !toSlideRight && !isScrolling) 
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public void deleteCard()
	{
		StartCoroutine(MenuController.instance.getUserData ());
		if(this.isCardFocusedDisplayed)
		{
			this.hideCardFocused ();
		}
		model.cards.cards.RemoveAt(this.cardsDisplayed[this.idCardClicked]);
		this.initializeCards ();
	}
	public void updateScene()
	{
		if(this.activeTab==1 && model.cards.getCard(this.cardsDisplayed[this.idCardClicked]).onSale==0)
		{
			this.deleteCard();
		}
		else if(this.activeTab==2 && model.cards.getCard(this.cardsDisplayed[this.idCardClicked]).onSale==1)
		{
			this.deleteCard();
		}
		else
		{
			StartCoroutine(MenuController.instance.getUserData ());
			this.cards[this.idCardClicked].GetComponent<NewCardController>().show();
			if(toUpdateCardsMarketFeatures)
			{
				this.updateCardsMarketFeatures();
			}
		}
	}
	public IEnumerator refreshMarket()
	{
		yield return StartCoroutine(model.refreshMarket (this.totalNbResultLimit));
		if(this.activeTab==0)
		{
			if(isCardFocusedDisplayed)
			{
				if(model.cards.getCard(this.cardsDisplayed[this.idCardClicked]).IdOWner==-1 && !model.cards.getCard(this.cardsDisplayed[this.idCardClicked]).isMine)
				{
					this.focusedCard.GetComponent<NewFocusedCardMarketController>().setCardSold();
				}
				this.toUpdateCardsMarketFeatures=true;
			}
			else
			{
				this.updateCardsMarketFeatures();
			}
			if(model.newCards.getCount()>0)
			{
				if(!isCardFocusedDisplayed)
				{
					this.refreshMarketButton.SetActive(true);
					this.refreshMarketButton.GetComponent<NewMarketRefreshButtonController>().reset();
				}
				this.areNewCardsAvailable=true;
				this.refreshMarketButtonTimer=0;
			}
		}
	}
	public IEnumerator refreshMyCards()
	{
		yield return StartCoroutine(model.refreshMyGame());
		if(this.activeTab!=0)
		{
			int index;
			if(isCardFocusedDisplayed && model.cards.getCard(this.cardsDisplayed[this.idCardClicked]).IdOWner==-1)
			{
				this.focusedCard.GetComponent<NewFocusedCardController>().setCardSold();
			}
			else if(this.isSceneLoaded)
			{
				this.updateCardsMarketFeatures();
			}
		}
	}
	public void displayNewCards()
	{
		this.areNewCardsAvailable = false;
		this.refreshMarketButton.SetActive (false);
		for(int i=0;i<model.newCards.getCount();i++)
		{
			model.cards.cards.Insert(i,model.newCards.getCard(i));
		}
		model.newCards = new Cards ();
		for(int i =0;i<model.cards.getCount();i++)
		{
			if(model.cards.getCard(model.cards.getCount()-i-1).onSale==0)
			{
				model.cards.cards.RemoveAt(model.cards.getCount()-i-1);
			}
		}
		this.initializeCards ();
	}
	public bool areSomeCardsDisplayed()
	{
		if(this.cardsDisplayed.Count>0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public Vector3 getFiltersPosition()
	{
		return this.filtersBlock.transform.position;
	}
	public Vector3 getCardsPosition(int id)
	{
		return cards[id].transform.position;
	}
	public void moneyUpdate()
	{
		if(isSceneLoaded)
		{
			if(this.isCardFocusedDisplayed)
			{
				this.focusedCard.GetComponent<NewFocusedCardMarketController>().updateFocusFeatures();
			}
			else
			{
				this.updateCardsMarketFeatures();
			}
		}
	}
	public int returnUserId()
	{
		return model.player.Id;
	}
	public void slideRight()
	{
		if(this.mainContentDisplayed)
		{
			this.lowerScrollCamera.GetComponent<ScrollingController>().reset();
			this.mainContentDisplayed=false;
			this.targetContentPositionX=this.filtersPositionX;
		}
		else if(this.marketContentDisplayed)
		{
			this.marketContentDisplayed=false;
			this.targetContentPositionX=this.mainContentPositionX;
		}
		else if(this.targetContentPositionX==mainContentPositionX)
		{
			this.targetContentPositionX=this.filtersPositionX;
		}
		else if(this.targetContentPositionX==marketContentPositionX)
		{
			this.targetContentPositionX=this.mainContentPositionX;
		}
		this.toSlideRight=true;
		this.toSlideLeft=false;
	}
	public void slideLeft()
	{
		if(this.mainContentDisplayed)
		{
			this.lowerScrollCamera.GetComponent<ScrollingController>().reset();
			this.mainContentDisplayed=false;
			this.targetContentPositionX=this.marketContentPositionX;
		}
		else if(this.filtersDisplayed)
		{
			this.filtersDisplayed=false;
			this.targetContentPositionX=this.mainContentPositionX;
			this.skillSearchBar.SetActive(false);
		}
		else if(this.targetContentPositionX==mainContentPositionX)
		{
			this.targetContentPositionX=this.marketContentPositionX;
		}
		else if(this.targetContentPositionX==filtersPositionX)
		{
			this.targetContentPositionX=this.mainContentPositionX;
		}
		this.toSlideRight=false;
		this.toSlideLeft=true;
	}
	public Camera returnCurrentCamera()
	{
		if(!ApplicationDesignRules.isMobileScreen)
		{
			return this.sceneCamera.GetComponent<Camera>();
		}
		else
		{
			return this.lowerScrollCamera.GetComponent<Camera>();
		}
	}
	public void hideActiveTab()
	{
		this.cardsBlockTitle.GetComponent<TextMeshPro>().text=this.tabs[this.activeTab].transform.FindChild("Title").GetComponent<TextMeshPro>().text;
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
	public void setGUI(bool value)
	{
		this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().setGUI(value);
	}
	#region TUTORIAL FUNCTIONS
	
	public bool getIsCardFocusedDisplayed()
	{
		return isCardFocusedDisplayed;
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
		Vector3 blockOrigin = this.cardsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		blockOrigin.y = blockOrigin.y + ApplicationDesignRules.tabWorldSize.y / 2f;
		return blockOrigin;
	}
	public Vector2 getCardsBlockSize()
	{
		Vector2 blockSize=this.cardsBlock.GetComponent<NewBlockController> ().getSize ();
		blockSize.y = blockSize.y + ApplicationDesignRules.tabWorldSize.y;
		return blockSize;
	}
	public Vector3 getMarketBlockOrigin()
	{
		return this.marketBlock.GetComponent<NewBlockController> ().getOriginPosition ();
	}
	public GameObject returnCardFocused()
	{
		return this.focusedCard;
	}
	public IEnumerator endHelp()
	{
		if(!model.player.MarketTutorial)
		{
			MenuController.instance.displayLoadingScreen();
			yield return StartCoroutine(model.player.setMarketTutorial(true));
			MenuController.instance.hideLoadingScreen();
		}
	}
	public bool getAreFiltersDisplayed()
	{	
		return this.filtersDisplayed;
	}
	public bool getIsMarketContentDisplayed()
	{
		return this.marketContentDisplayed;
	}
	public void resetScrolling()
	{
		this.lowerScrollCamera.GetComponent<ScrollingController>().reset();
	}
	#endregion
}
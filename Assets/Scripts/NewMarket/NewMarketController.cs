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
	public GameObject tutorialObject;
	public GUISkin popUpSkin;
	public int totalNbResultLimit;
	public int refreshInterval;

	private GameObject menu;
	private GameObject tutorial;
	private GameObject refreshMarketButton;

	private GameObject cardsBlock;
	private GameObject cardsBlockTitle;
	private GameObject[] cards;
	private GameObject[] cardsPagination;
	private GameObject cardsPaginationLine;
	private GameObject cardsNumberTitle;
	
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
	
	private int focusedCardIndex;
	private bool isCardFocusedDisplayed;

	private Rect centralWindow;
	private Rect collectionPointsWindow;
	private Rect newSkillsWindow;
	private Rect newCardTypeWindow;
	
	private bool isSearchingSkill;
	private bool isSkillChosen;
	private bool isMouseOnSearchBar;
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
	
	private int nbPages;
	private int nbLines;
	private int cardsPerLine;
	private int chosenPage;

	private int idCardClicked;
	
	private float timer;
	private float refreshMarketButtonTimer;
	private bool isSceneLoaded;

	private bool toUpdateCardsMarketFeatures;
	private bool areNewCardsAvailable;

	private bool isTutorialLaunched;

	void Update()
	{	
		this.timer += Time.deltaTime;
		
		if (this.timer > this.refreshInterval) 
		{	
			this.timer=this.timer-this.refreshInterval;
			StartCoroutine(this.refreshMarket());
		}
		if(isSearchingSkill)
		{
			if(!Input.GetKey(KeyCode.Delete))
			{
				foreach (char c in Input.inputString) 
				{
					if(c==(char)KeyCode.Backspace && this.valueSkill.Length>0)
					{
						this.valueSkill = this.valueSkill.Remove(this.valueSkill.Length - 1);
						this.skillSearchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text = this.valueSkill;
						this.setSkillAutocompletion();
						if(this.valueSkill.Length==0)
						{
							this.isSearchingSkill=false;
							this.skillSearchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text = "Rechercher";
							this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().reset();
						}
					}
					else if (c == "\b"[0])
					{
						if (valueSkill.Length != 0)
						{
							valueSkill= valueSkill.Substring(0, valueSkill.Length - 1);
						}
					}
					else
					{
						if (c == "\n"[0] || c == "\r"[0])
						{
							
						}
						else if(this.valueSkill.Length<12)
						{
							this.valueSkill += c;
							this.valueSkill=this.valueSkill.ToLower();
							this.setSkillAutocompletion();
						}
					}
				}
			}
			if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))&& !this.isMouseOnSearchBar)
			{
				this.isSearchingSkill=false;
				this.cleanSkillAutocompletion();
				this.skillSearchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text = "Rechercher";
				this.skillSearchBar.GetComponent<NewMarketSkillSearchBarController>().reset();
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
	}
	void Awake()
	{
		instance = this;
		this.model = new NewMarketModel ();
		this.cardsPerLine = 4;
		this.nbLines = 2;
		this.initializeScene ();
		this.sortingOrder = -1;
	}
	public IEnumerator initialization()
	{
		this.resize ();
		yield return StartCoroutine (model.initializeMarket (this.totalNbResultLimit));
		this.initializeCards ();
		MenuController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
		if(!model.player.MarketTutorial)
		{
			this.tutorial = Instantiate(this.tutorialObject) as GameObject;
			this.tutorial.AddComponent<MarketTutorialController>();
			StartCoroutine(this.tutorial.GetComponent<MarketTutorialController>().launchSequence(0));
			this.menu.GetComponent<MenuController>().setTutorialLaunched(true);
			this.isTutorialLaunched=true;
		} 
	}
	private void initializeCards()
	{
		this.resetFiltersValue ();
		this.applyFilters ();
	}
	private void applyFilters()
	{
		this.computeFilters ();
		this.chosenPage = 0;
		this.nbPages = Mathf.CeilToInt((float) this.cardsToBeDisplayed.Count / ((float)this.nbLines*(float)this.cardsPerLine));
		this.setPagination ();
		this.drawCards ();
	}
	private void setPagination()
	{
		int carteDebut = 0;
		int carteFin = 0;
		int carteTotal = this.cardsToBeDisplayed.Count;

		if(this.chosenPage==0)
		{
			carteDebut=1;
			this.cardsPagination[0].GetComponent<NewMarketPaginationButtonController>().reset();
			this.cardsPagination[0].SetActive(false);
			if(this.nbPages>1)
			{
				this.cardsPagination[1].SetActive(true);
				carteFin=nbLines*cardsPerLine;
				this.cardsNumberTitle.GetComponent<TextMeshPro>().text=("cartes " +carteDebut+" à "+carteFin+" sur "+carteTotal ).ToUpper();
			}
			else
			{
				this.cardsPagination[1].GetComponent<NewMarketPaginationButtonController>().reset ();
				this.cardsPagination[1].SetActive(false);
				if(carteTotal==0)
				{
					this.cardsNumberTitle.GetComponent<TextMeshPro>().text="Aucune carte à afficher".ToUpper();
				}
				else
				{
					carteFin=carteTotal;
					this.cardsNumberTitle.GetComponent<TextMeshPro>().text=("cartes " +carteDebut+" à "+carteFin+" sur "+carteTotal).ToUpper();
				}
			}
		}
		else
		{
			carteDebut=this.chosenPage*this.nbLines*this.cardsPerLine+1;
			this.cardsPagination[0].SetActive(true);
			if(this.chosenPage!=this.nbPages-1)
			{
				carteFin=carteDebut+this.nbLines*this.cardsPerLine-1;
				this.cardsPagination[1].SetActive(true);
			}
			else
			{
				carteFin=carteTotal;
				this.cardsPagination[1].GetComponent<NewMarketPaginationButtonController>().reset ();
				this.cardsPagination[1].SetActive(false);
			}
			this.cardsNumberTitle.GetComponent<TextMeshPro>().text=("cartes " +carteDebut+" à "+carteFin+" sur "+carteTotal).ToUpper();
		}
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
		this.drawCards ();
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("Menu");
		menu.AddComponent<MarketMenuController> ();

		this.cardsBlock = Instantiate (this.blockObject) as GameObject;
		this.cardsBlockTitle = GameObject.Find ("CardsBlockTitle");
		this.cardsBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.cardsBlockTitle.GetComponent<TextMeshPro> ().text = "En vente";
		this.cardsNumberTitle = GameObject.Find ("CardsNumberTitle");
		this.cardsNumberTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.cards=new GameObject[this.nbLines*this.cardsPerLine];
		for (int i=0;i<this.cards.Length;i++)
		{
			this.cards[i]=GameObject.Find("Card"+i);
			this.cards[i].AddComponent<NewCardMarketController>();
			this.cards[i].transform.GetComponent<NewCardMarketController>().setId(i);
			this.cards[i].SetActive(false);
		}
		this.cardsPagination = new GameObject[2];
		for(int i=0;i<this.cardsPagination.Length;i++)
		{
			this.cardsPagination[i]=GameObject.Find("CardsPagination"+i);
			this.cardsPagination[i].AddComponent<NewMarketPaginationButtonController>();
			this.cardsPagination[i].GetComponent<NewMarketPaginationButtonController>().setId(i);
		}
		this.cardsPaginationLine = GameObject.Find ("CardsPaginationLine");
		this.cardsPaginationLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		this.refreshMarketButton = GameObject.Find ("RefreshMarketButton");
		this.refreshMarketButton.GetComponent<TextMeshPro> ().text = "Actualiser".ToUpper();
		this.refreshMarketButton.AddComponent<NewMarketRefreshButtonController> ();
		this.refreshMarketButton.AddComponent<BoxCollider2D> ();
		this.refreshMarketButton.SetActive (false);
		
		this.filtersBlock = Instantiate (this.blockObject) as GameObject;
		this.filtersBlockTitle = GameObject.Find ("FiltersBlockTitle");
		this.filtersBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.filtersBlockTitle.GetComponent<TextMeshPro> ().text = "Filtrer";
		this.cardsTypeFilters = new GameObject[10];
		for(int i=0;i<this.cardsTypeFilters.Length;i++)
		{
			this.cardsTypeFilters[i]=GameObject.Find("CardTypeFilter"+i);
			this.cardsTypeFilters[i].AddComponent<NewMarketCardTypeFilterController>();
			this.cardsTypeFilters[i].GetComponent<NewMarketCardTypeFilterController>().setId(i);
		}
		this.valueFilters=new GameObject[4];
		for(int i=0;i<this.valueFilters.Length;i++)
		{
			this.valueFilters[i]=GameObject.Find ("ValueFilter"+i);
		}
		this.skillSearchBarTitle = GameObject.Find ("SkillSearchTitle");
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().text = "Compétence".ToUpper ();
		this.skillSearchBar = GameObject.Find ("SkillSearchBar");
		this.skillSearchBar.AddComponent<NewMarketSkillSearchBarController> ();
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
		this.sortButtons=new GameObject[10];
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
		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardMarketController> ();
		this.focusedCard.SetActive (false);

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
		this.valueFilters[3].transform.FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(quicknessVal);
		this.valueFilters[3].transform.FindChild("Icon").GetComponent<SpriteRenderer>().color=getColorFilterIcon(quicknessVal);
		
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
		this.skillSearchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text ="Rechercher";
		if(this.sortingOrder!=-1)
		{
			this.sortButtons[this.sortingOrder].GetComponent<NewMarketSortButtonController>().setIsSelected(false);
			this.sortButtons[this.sortingOrder].GetComponent<NewMarketSortButtonController>().reset ();
			this.sortingOrder = -1;
		}
	}
	public void resize()
	{
		float filtersBlockLeftMargin =  ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.leftMargin+(ApplicationDesignRules.worldWidth-ApplicationDesignRules.rightMargin-ApplicationDesignRules.leftMargin-ApplicationDesignRules.gapBetweenBlocks)/2f;
		float filtersBlockRightMargin = ApplicationDesignRules.rightMargin;
		float filtersBlockUpMargin = 6.45f;
		float filtersBlockDownMargin = ApplicationDesignRules.downMargin;
		
		this.filtersBlock.GetComponent<NewBlockController> ().resize(filtersBlockLeftMargin,filtersBlockRightMargin,filtersBlockUpMargin,filtersBlockDownMargin);
		Vector3 filtersBlockUpperLeftPosition = this.filtersBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 filtersBlockUpperRightPosition = this.filtersBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 filtersBlockSize = this.filtersBlock.GetComponent<NewBlockController> ().getSize ();
		
		float gapBetweenSubFiltersBlock = 0.05f;
		float filtersSubBlockSize = (filtersBlockSize.x - 0.6f - 2f * gapBetweenSubFiltersBlock) / 3f;
		
		this.filtersBlockTitle.transform.position = new Vector3 (filtersBlockUpperLeftPosition.x + 0.3f, filtersBlockUpperLeftPosition.y - 0.2f, 0f);
		this.filtersBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		
		this.cardTypeFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.cardTypeFilterTitle.transform.position = new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f, filtersBlockUpperLeftPosition.y - 1.2f, 0f);
		
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
		
		this.skillSearchBarTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.skillSearchBarTitle.transform.position = new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 2f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 1.2f, 0f);
		
		this.skillSearchBar.transform.localScale = ApplicationDesignRules.inputTextScale;
		this.skillSearchBar.transform.position = new Vector3 (this.skillSearchBarTitle.transform.position.x, filtersBlockUpperLeftPosition.y - 1.6f, 0f);
		
		for(int i=0;i<this.skillChoices.Length;i++)
		{
			this.skillChoices[i].transform.localScale=ApplicationDesignRules.listElementScale;
			this.skillChoices[i].transform.position=new Vector3(this.skillSearchBar.transform.position.x,this.skillSearchBar.transform.position.y-ApplicationDesignRules.inputTextWorldSize.y/2f-(i+0.5f)*ApplicationDesignRules.listElementWorldSize.y+i*0.02f,-1f);
		}
		
		this.valueFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.valueFilterTitle.transform.position=new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 1f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 1.2f, 0f);

		this.priceFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.priceFilterTitle.transform.position=new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 2f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 2.25f, 0f);

		this.priceFilter.transform.localScale=ApplicationDesignRules.valueFilterScale;
		this.priceFilter.transform.position=new Vector3(priceFilterTitle.transform.position.x,filtersBlockUpperLeftPosition.y - 3f,0f);
		
		for(int i=0;i<this.valueFilters.Length;i++)
		{
			this.valueFilters[i].transform.localScale=ApplicationDesignRules.valueFilterScale;
			this.valueFilters[i].transform.position=new Vector3(this.valueFilterTitle.transform.position.x,filtersBlockUpperLeftPosition.y - 1.6f-i*0.5f,0f);
		}
		
		this.centralWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.25f * ApplicationDesignRules.heightScreen);
		this.collectionPointsWindow=new Rect(ApplicationDesignRules.widthScreen -  ApplicationDesignRules.widthScreen * 0.17f-5,0.1f * ApplicationDesignRules.heightScreen+5,ApplicationDesignRules.widthScreen * 0.17f,ApplicationDesignRules.heightScreen * 0.1f);
		this.newSkillsWindow = new Rect (this.collectionPointsWindow.xMin, this.collectionPointsWindow.yMax + 5,this.collectionPointsWindow.width,ApplicationDesignRules.heightScreen - 0.1f * ApplicationDesignRules.heightScreen - 2 * 5 - this.collectionPointsWindow.height);
		this.newCardTypeWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.25f * ApplicationDesignRules.heightScreen);
		
		float cardsBlockLeftMargin = ApplicationDesignRules.leftMargin;
		float cardsBlockRightMargin = ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.rightMargin+(ApplicationDesignRules.worldWidth-ApplicationDesignRules.rightMargin-ApplicationDesignRules.leftMargin-ApplicationDesignRules.gapBetweenBlocks)/2f;
		float cardsBlockUpMargin = ApplicationDesignRules.upMargin;
		float cardsBlockDownMargin = ApplicationDesignRules.downMargin;
		
		this.cardsBlock.GetComponent<NewBlockController> ().resize(cardsBlockLeftMargin,cardsBlockRightMargin,cardsBlockUpMargin,cardsBlockDownMargin);
		Vector3 cardsBlockUpperLeftPosition = this.cardsBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 cardsBlockLowerLeftPosition = this.cardsBlock.GetComponent<NewBlockController> ().getLowerLeftCornerPosition ();
		Vector3 cardsBlockUpperRightPosition = this.cardsBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 cardsBlockSize = this.cardsBlock.GetComponent<NewBlockController> ().getSize ();
		Vector3 cardsBlockOrigin = this.cardsBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.cardsBlockTitle.transform.position = new Vector3 (cardsBlockUpperLeftPosition.x + 0.3f, cardsBlockUpperLeftPosition.y - 0.2f, 0f);
		this.cardsBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		this.cardsNumberTitle.transform.position = new Vector3 (cardsBlockUpperRightPosition.x - 0.3f, cardsBlockUpperRightPosition.y - 0.2f, 0f);
		this.cardsNumberTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.refreshMarketButton.transform.position=new Vector3 (cardsBlockUpperRightPosition.x - 0.3f, cardsBlockUpperRightPosition.y - 0.55f, 0f);
		this.refreshMarketButton.transform.localScale= ApplicationDesignRules.subMainTitleScale;
		
		float gapBetweenCardsLine = 0.55f;
		float gapBetweenCardsHalo = (cardsBlockSize.x - 0.6f - 4f * ApplicationDesignRules.cardWorldSize.x) / 3f;
		float firstLineY = cardsBlockUpperRightPosition.y - 2.35f;
		
		for(int j=0;j<this.nbLines;j++)
		{
			for(int i =0;i<this.cardsPerLine;i++)
			{
				this.cards[j*(cardsPerLine)+i].transform.localScale= ApplicationDesignRules.cardScale;
				this.cards[j*(cardsPerLine)+i].transform.position=new Vector3(cardsBlockUpperLeftPosition.x+0.3f+ApplicationDesignRules.cardHaloWorldSize.x/2f+i*(gapBetweenCardsHalo+ApplicationDesignRules.cardHaloWorldSize.x),firstLineY-j*(gapBetweenCardsLine+ApplicationDesignRules.cardHaloWorldSize.y),0f);
				this.cards[j*(this.cardsPerLine)+i].transform.name="Card"+(j*(this.cardsPerLine)+i);
				this.cards[j*(this.cardsPerLine)+i].transform.GetComponent<NewCardController> ().setCentralWindow (this.centralWindow);
			}
		}
		
		this.cardsPagination [0].transform.localScale = ApplicationDesignRules.paginationButtonScale;
		this.cardsPagination [1].transform.localScale = ApplicationDesignRules.paginationButtonScale;
		
		this.cardsPagination [0].transform.position = new Vector3 (cardsBlockLowerLeftPosition.x + cardsBlockSize.x / 2 - 0.05f - ApplicationDesignRules.paginationButtonWorldSize.x / 2f, cardsBlockLowerLeftPosition.y + 0.3f, 0f);
		this.cardsPagination [1].transform.position = new Vector3 (cardsBlockLowerLeftPosition.x + cardsBlockSize.x / 2 + 0.05f + ApplicationDesignRules.paginationButtonWorldSize.x / 2f, cardsBlockLowerLeftPosition.y + 0.3f, 0f);
		
		float lineScale = ApplicationDesignRules.getLineScale (cardsBlockSize.x - 0.6f);
		this.cardsPaginationLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.cardsPaginationLine.transform.position = new Vector3 (cardsBlockLowerLeftPosition.x + cardsBlockSize.x / 2, cardsBlockLowerLeftPosition.y + 0.6f, 0f);
		
		this.focusedCard.transform.localScale = ApplicationDesignRules.cardFocusedScale;
		this.focusedCard.transform.position = new Vector3 (0f, -ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.downMargin+ApplicationDesignRules.cardFocusedWorldSize.y/2f-0.22f, 0f);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setCentralWindow (this.centralWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setCollectionPointsWindow (this.collectionPointsWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setNewSkillsWindow (this.newSkillsWindow);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setNewCardTypeWindow (this.newCardTypeWindow);

		if(this.isTutorialLaunched)
		{
			this.tutorial.GetComponent<TutorialObjectController>().resize();
		}
	}
	public void drawCards()
	{
		this.cardsDisplayed = new List<int> ();
		
		for(int j=0;j<nbLines;j++)
		{
			for(int i =0;i<cardsPerLine;i++)
			{
				if(this.chosenPage*(this.nbLines*this.cardsPerLine)+j*(cardsPerLine)+i<this.cardsToBeDisplayed.Count)
				{
					this.cardsDisplayed.Add (this.cardsToBeDisplayed[this.chosenPage*(this.nbLines*this.cardsPerLine)+j*(cardsPerLine)+i]);
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
	}
	public void showCardFocused()
	{
		this.isCardFocusedDisplayed = true;
		this.displayBackUI (false);
		this.focusedCard.SetActive (true);
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		this.focusedCardIndex=this.cardsDisplayed[this.idCardClicked];
		this.focusedCard.GetComponent<NewFocusedCardController>().c=model.cards.getCard(this.focusedCardIndex);
		this.focusedCard.GetComponent<NewFocusedCardController> ().show ();
	}
	public void hideCardFocused()
	{
		this.isCardFocusedDisplayed = false;
		this.displayBackUI (true);
		this.focusedCard.SetActive (false);
		this.cards[this.idCardClicked].GetComponent<NewCardController>().show();
		if(toUpdateCardsMarketFeatures)
		{
			this.updateCardsMarketFeatures();
		}
	}
	public void displayBackUI(bool value)
	{
		this.cardsBlock.SetActive (value);
		this.cardsBlockTitle.SetActive (value);
		this.cardsNumberTitle.SetActive (value);
		for (int i=0;i<this.cards.Length;i++)
		{
			if(i<this.cardsDisplayed.Count)
			{
				this.cards[i].SetActive(value);
			}
			else
			{
				this.cards[i].SetActive(false);
			}
		}
		if(value)
		{
			this.setPagination();
		}
		else
		{
			for(int i=0;i<this.cardsPagination.Length;i++)
			{
				this.cardsPagination[i].SetActive(false);
			}
		}
		this.cardsPaginationLine.SetActive (value);
		this.filtersBlock.SetActive (value);
		this.filtersBlockTitle.SetActive (value);
		for(int i=0;i<this.cardsTypeFilters.Length;i++)
		{
			this.cardsTypeFilters[i].SetActive(value);
		}
		for(int i=0;i<this.valueFilters.Length;i++)
		{
			this.valueFilters[i].SetActive(value);
		}
		this.priceFilterTitle.SetActive (value);
		this.priceFilter.SetActive (value);
		this.skillSearchBar.SetActive (value);
		this.skillSearchBarTitle.SetActive (value);
		if(isSearchingSkill&&value)
		{
			for(int i=0;i<this.skillChoices.Length;i++)
			{
				if(i<this.skillsDisplayed.Count)
				{
					this.skillChoices[i].SetActive(value);
				}
				else
				{
					this.skillChoices[i].SetActive(false);
				}
			}
		}
		else
		{
			for(int i=0;i<this.skillChoices.Length;i++)
			{
				this.skillChoices[i].SetActive(false);
			}
		}
		this.cardTypeFilterTitle.SetActive(value);
		this.valueFilterTitle.SetActive(value);
		if(this.areNewCardsAvailable && value)
		{
			this.refreshMarketButton.SetActive(true);
		}
		else
		{
			this.refreshMarketButton.SetActive(false);
		}
		if(value && toUpdateCardsMarketFeatures)
		{
			this.updateCardsMarketFeatures();
		}
	}
	public void searchingSkill()
	{
		if(isSkillChosen)
		{
			this.isSkillChosen=false;
			this.applyFilters();
		}
		this.cleanSkillAutocompletion();
		this.isSearchingSkill = true;
		this.valueSkill = "";
		this.skillSearchBar.transform.FindChild("Title").GetComponent<TextMeshPro>().text = this.valueSkill;
		this.skillSearchBar.transform.GetComponent<NewMarketSkillSearchBarController> ().setIsSelected (true);
		this.skillSearchBar.transform.GetComponent<NewMarketSkillSearchBarController> ().setInitialState ();
	}
	public void cardTypeFilterHandler(int id)
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
		this.applyFilters ();
	}
	public void sortButtonHandler(int id)
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
		this.applyFilters ();
	}
	public void moveCursors(int cursorId)
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		float mousePositionX=mousePosition.x;
		Vector3 cursorPosition = this.cursors [cursorId].transform.localPosition;
		float offset = mousePositionX-this.cursors [cursorId].transform.position.x;
		print (offset);
		
		int value = -1;
		string label = "";
		
		bool isMoved = true ;
		
		if(cursorPosition.x==-0.67f)
		{
			if(offset>0.67f)
			{
				value = 2;
				cursorPosition.x=+0.67f;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else if(offset>0.67/2f)
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
			if(offset>0.67f/2f)
			{
				value =2;
				cursorPosition.x=+0.67f;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else if(offset<-0.67f/2f)
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
			if(offset<-0.67f)
			{
				value = 0;
				cursorPosition.x=-0.67f;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else if(offset<-0.67/2f)
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
			this.applyFilters();
		}
	}
	public void moveMinMaxCursor(int cursorId)
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
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
			else if(cursorPosition.x<-0.67f+sliderPositionX)
			{
				cursorPosition.x=-0.67f+sliderPositionX;
			}
			distance = cursorPosition.x -(-0.67f+sliderPositionX);
		}
		else
		{
			secondCursorId=cursorId-1;
			secondCursorPositionX = this.priceCursors [secondCursorId].transform.position.x;
			if(cursorPosition.x>0.67f+sliderPositionX)
			{
				cursorPosition.x=0.67f+sliderPositionX;
			}
			else if(cursorPosition.x<secondCursorPositionX+cursorSizeX)
			{
				cursorPosition.x=secondCursorPositionX+cursorSizeX;
			}
			distance = (0.67f+sliderPositionX)-cursorPosition.x;
		}
		this.priceCursors [cursorId].transform.position = cursorPosition;
		float maxDistance = 2 * 0.67f-cursorSizeX;
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

			if(this.isSkillChosen && !model.cards.getCard (i).hasSkill(this.valueSkill))
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
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Life;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Life;
						break;
					case 5:
						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Life;
						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Life;
						break;
					case 6:
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Attack;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Attack;
						break;
					case 7:
						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Attack;
						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Attack;
						break;
					case 8:
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Speed;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Speed;
						break;
					case 9:
						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Speed;
						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Speed;
						break;
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
		this.skillSearchBar.transform.FindChild("Title").GetComponent<TextMeshPro>().text = this.valueSkill;
		if(this.valueSkill.Length>0)
		{
			for (int i = 0; i < model.skillsList.Count; i++) 
			{  
				if (model.skillsList [i].ToLower ().Contains (this.valueSkill)) 
				{
					this.skillsDisplayed.Add (i);
					this.skillChoices[this.skillsDisplayed.Count-1].SetActive(true);
					this.skillChoices[this.skillsDisplayed.Count-1].GetComponent<NewMarketSkillChoiceController>().reset();
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
		this.valueSkill = this.skillChoices[id].transform.FindChild("Title").GetComponent<TextMeshPro>().text.ToLower();
		this.isSkillChosen = true;
		this.skillSearchBar.transform.FindChild("Title").GetComponent<TextMeshPro>().text =valueSkill;
		this.cleanSkillAutocompletion ();
		this.applyFilters ();
	}
	public void mouseOnSearchBar(bool value)
	{
		this.isMouseOnSearchBar = value;
	}
	public void leftClickedHandler(int id)
	{
		this.idCardClicked = id;
		bool onSale=System.Convert.ToBoolean(model.cards.getCard(this.cardsDisplayed[id]).onSale);
		bool isMine = model.cards.getCard (this.cardsDisplayed [id]).isMine;
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
		else
		{
			this.cards[this.idCardClicked].GetComponent<NewCardController>().escapePressed();
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
	public void retrieveIdCardClicked(int id)
	{
		this.idCardClicked = id;
	}
	public IEnumerator refreshMarket()
	{
		yield return StartCoroutine(model.refreshMarket (this.totalNbResultLimit));
		if(isCardFocusedDisplayed)
		{
			if(model.cards.getCard(this.focusedCardIndex).IdOWner==-1 && !model.cards.getCard(this.focusedCardIndex).isMine)
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
			}
			this.areNewCardsAvailable=true;
			this.refreshMarketButtonTimer=0;
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
		this.applyFilters ();
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
	public IEnumerator endTutorial(bool toUpdate)
	{
		Destroy (this.tutorial);
		this.isTutorialLaunched = false;
		MenuController.instance.setTutorialLaunched (false);
		if(toUpdate)
		{
			yield return StartCoroutine (model.player.setMarketTutorial(true));
		}
		yield break;
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
		}
	}
}
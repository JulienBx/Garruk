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

	public GameObject tutorialObject;
	public GameObject blockObject;
	public Texture2D[] cursorTextures;
	public GUISkin popUpSkin;
	public int refreshInterval;

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
	private GameObject cardsNumberTitle;

	private GameObject filtersBlock;
	private GameObject filtersBlockTitle;
	private GameObject[] cardsTypeFilters;
	private GameObject skillSearchBarTitle;
	private GameObject skillSearchBar;
	private GameObject[] skillChoices;
	private GameObject valueFilterTitle;
	private GameObject[] valueFilters;
	private GameObject[] availableFilters;
	private GameObject availabilityFilterTitle;
	private GameObject cardTypeFilterTitle;
	private GameObject[] cursors;
	private GameObject[] sortButtons;

	private GameObject focusedCard;

	private int focusedCardIndex;
	private bool isCardFocusedDisplayed;

	private Rect centralWindow;
	private Rect collectionPointsWindow;
	private Rect newSkillsWindow;
	private Rect newCardTypeWindow;

	private bool isSearchingSkill;
	private bool isSearchingDeck;
	private bool isMouseOnSelectDeckButton;
	private bool isSkillChosen;
	private bool isMouseOnSearchBar;
	private bool isOnSaleFilterOn;
	private bool isNotOnSaleFilterOn;
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

	private NewMyGameNewDeckPopUpView newDeckView;
	private bool newDeckViewDisplayed;
	private NewMyGameEditDeckPopUpView editDeckView;
	private bool editDeckViewDisplayed;
	private NewMyGameDeleteDeckPopUpView deleteDeckView;
	private bool deleteDeckViewDisplayed;

	private bool isDragging;
	private bool isLeftClicked;
	private bool isHovering;
	private float clickInterval;
	private int idCardClicked;
	private bool isDeckCardClicked;
	private Vector3[] cardsPosition;
	private Vector3[] deckCardsPosition;
	private Rect[] deckCardsArea;
	private Rect cardsArea;
	private Texture2D cursorTexture;

	private float timer;
	private bool isSceneLoaded;

	private bool isTutorialLaunched;

	void Update()
	{	
		this.timer += Time.deltaTime;
		
		if (this.timer > this.refreshInterval) 
		{	
			this.timer=this.timer-this.refreshInterval;
			StartCoroutine(this.refreshMyGame());
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
							this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().reset();
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
					this.skillSearchBar.GetComponent<newMyGameSkillSearchBarController>().reset();
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
	}
	void Awake()
	{
		instance = this;
		this.model = new NewMyGameModel ();
		this.cardsPerLine = 4;
		this.nbLines = 2;
		this.sortingOrder = -1;
		this.cardsPagination = new Pagination ();
		this.cardsPagination.chosenPage = 0;
		this.cardsPagination.nbElementsPerPage = this.cardsPerLine * this.nbLines;
		this.initializeScene ();
	}
	public IEnumerator initialization()
	{
		this.resize ();
		yield return StartCoroutine (model.initializeMyGame ());
		this.retrieveDefaultDeck ();
		this.initializeDecks ();
		this.resetFiltersValue ();
		if(ApplicationModel.skillChosen!="")
		{
			this.isSkillChosen=true;
			this.valueSkill=ApplicationModel.skillChosen.ToLower();
			this.skillSearchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text =valueSkill;
		   	ApplicationModel.skillChosen="";
		}
		if(ApplicationModel.cardTypeChosen!=-1)
		{
			this.filtersCardType.Add (ApplicationModel.cardTypeChosen);
		   	ApplicationModel.cardTypeChosen=-1;
		}
		this.applyFilters ();
		MenuController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
		if(model.player.TutorialStep==2 || model.player.TutorialStep==3)
		{
			this.tutorial = Instantiate(this.tutorialObject) as GameObject;
			this.tutorial.AddComponent<MyGameTutorialController>();
			this.menu.GetComponent<MenuController>().setTutorialLaunched(true);
			if(model.player.TutorialStep==2)
			{
				StartCoroutine(this.tutorial.GetComponent<MyGameTutorialController>().launchSequence(0));
			}
			else if(model.player.TutorialStep==3)
			{
				StartCoroutine(this.tutorial.GetComponent<MyGameTutorialController>().launchSequence(18));
			}
			this.isTutorialLaunched=true;
		} 
	}
	private void initializeDecks()
	{
		this.retrieveDecksList ();
		this.drawDeckCards ();
	}
	private void initializeCards()
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
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("Menu");
		menu.AddComponent<MyGameMenuController> ();

		this.deckBlock = Instantiate (this.blockObject) as GameObject;
		this.deckBlockTitle = GameObject.Find ("DeckBlockTitle");
		this.deckBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.deckBlockTitle.GetComponent<TextMeshPro> ().text = "Mes équipes";
		this.deckSelectionButton = GameObject.Find ("DeckSelectionButton");
		this.deckSelectionButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Changer".ToUpper ();
		this.deckSelectionButton.AddComponent<newMyGameDeckSelectionButtonController> ();
		this.deckCreationButton = GameObject.Find ("DeckCreationButton");
		this.deckCreationButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Nouveau".ToUpper ();
		this.deckCreationButton.AddComponent<newMyGameDeckCreatioButtonController> ();
		this.deckDeletionButton = GameObject.Find ("DeckDeletionButton");
		this.deckDeletionButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Supprimer".ToUpper ();
		this.deckDeletionButton.AddComponent<newMyGameDeckDeletionButtonController> ();
		this.deckRenameButton = GameObject.Find ("DeckRenameButton");
		this.deckRenameButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Renommer".ToUpper ();
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
		}
		this.cardsBlock = Instantiate (this.blockObject) as GameObject;
		this.cardsBlockTitle = GameObject.Find ("CardsBlockTitle");
		this.cardsBlockTitle.GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		this.cardsBlockTitle.GetComponent<TextMeshPro> ().text = "Mes cartes";
		this.cardsNumberTitle = GameObject.Find ("CardsNumberTitle");
		this.cardsNumberTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.cards=new GameObject[this.nbLines*this.cardsPerLine];
		for (int i=0;i<this.cards.Length;i++)
		{
			this.cards[i]=GameObject.Find("Card"+i);
			this.cards[i].AddComponent<NewCardMyGameController>();
			this.cards[i].transform.GetComponent<NewCardMyGameController>().setId(i,false);
			this.cards[i].SetActive(false);
		}
		this.cardsPaginationButtons = GameObject.Find("Pagination");
		this.cardsPaginationButtons.AddComponent<newMyGamePaginationController> ();
		this.cardsPaginationButtons.GetComponent<newMyGamePaginationController> ().initialize ();
		this.cardsPaginationLine = GameObject.Find ("CardsPaginationLine");
		this.cardsPaginationLine.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;

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
		this.valueFilters=new GameObject[4];
		for(int i=0;i<this.valueFilters.Length;i++)
		{
			this.valueFilters[i]=GameObject.Find ("ValueFilter"+i);
		}
		this.availableFilters = new GameObject[2];
		for (int i=0; i<this.availableFilters.Length; i++) 
		{
			this.availableFilters[i]=GameObject.Find("AvailableFilter"+i);
			this.availableFilters[i].transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.availableFilters[i].AddComponent<newMyGameAvailabilityFilterController>();
			this.availableFilters[i].GetComponent<newMyGameAvailabilityFilterController>().setId(i);
		}
		this.availableFilters [0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "disponibles".ToUpper ();
		this.availableFilters [1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "en vente".ToUpper();
		this.skillSearchBarTitle = GameObject.Find ("SkillSearchTitle");
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		this.skillSearchBarTitle.GetComponent<TextMeshPro> ().text = "Compétence".ToUpper ();
		this.skillSearchBar = GameObject.Find ("SkillSearchBar");
		this.skillSearchBar.AddComponent<newMyGameSkillSearchBarController> ();
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
		this.availabilityFilterTitle = GameObject.Find ("AvailabilityFilterTitle");
		this.availabilityFilterTitle.GetComponent<TextMeshPro> ().text = "Disponibilité".ToUpper ();
		this.availabilityFilterTitle.GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;

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
		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardMyGameController> ();
		this.focusedCard.SetActive (false);
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
			this.cardsTypeFilters[i].GetComponent<newMyGameCardTypeFilterController>().reset();
		}
		this.valueSkill = "";
		this.isSkillChosen = false;
		this.isOnSaleFilterOn = false;
		this.isNotOnSaleFilterOn = false;
		for(int i=0;i<this.availableFilters.Length;i++)
		{
			this.availableFilters[i].GetComponent<newMyGameAvailabilityFilterController>().reset();
		}

		this.cleanSkillAutocompletion ();
		this.skillSearchBar.transform.FindChild ("Title").GetComponent<TextMeshPro>().text ="Rechercher";
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

		this.availabilityFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.availabilityFilterTitle.transform.position=new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 2f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 2.25f, 0f);

		this.valueFilterTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;
		this.valueFilterTitle.transform.position=new Vector3 (0.3f+filtersBlockUpperLeftPosition.x + filtersSubBlockSize / 2f + 1f*(filtersSubBlockSize+gapBetweenSubFiltersBlock), filtersBlockUpperLeftPosition.y - 1.2f, 0f);

		for(int i=0;i<this.valueFilters.Length;i++)
		{
			this.valueFilters[i].transform.localScale=ApplicationDesignRules.valueFilterScale;
			this.valueFilters[i].transform.position=new Vector3(valueFilterTitle.transform.position.x,filtersBlockUpperLeftPosition.y - 1.6f-i*0.5f,0f);
		}

		for(int i=0;i<this.availableFilters.Length;i++)
		{
			this.availableFilters[i].transform.localScale=ApplicationDesignRules.button62Scale;
			this.availableFilters[i].transform.position=new Vector3(availabilityFilterTitle.transform.position.x, filtersBlockUpperLeftPosition.y-2.65f-i*0.45f,0f);
		}

		this.centralWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.25f * ApplicationDesignRules.heightScreen);
		this.collectionPointsWindow=new Rect(ApplicationDesignRules.widthScreen -  ApplicationDesignRules.widthScreen * 0.17f-5,0.1f * ApplicationDesignRules.heightScreen+5,ApplicationDesignRules.widthScreen * 0.17f,ApplicationDesignRules.heightScreen * 0.1f);
		this.newSkillsWindow = new Rect (this.collectionPointsWindow.xMin, this.collectionPointsWindow.yMax + 5,this.collectionPointsWindow.width,ApplicationDesignRules.heightScreen - 0.1f * ApplicationDesignRules.heightScreen - 2 * 5 - this.collectionPointsWindow.height);
		this.newCardTypeWindow = new Rect (ApplicationDesignRules.widthScreen * 0.25f, 0.12f * ApplicationDesignRules.heightScreen, ApplicationDesignRules.widthScreen * 0.50f, 0.25f * ApplicationDesignRules.heightScreen);

		float deckBlockLeftMargin = ApplicationDesignRules.gapBetweenBlocks+ApplicationDesignRules.leftMargin+(ApplicationDesignRules.worldWidth-ApplicationDesignRules.rightMargin-ApplicationDesignRules.leftMargin-ApplicationDesignRules.gapBetweenBlocks)/2f;;
		float deckBlockRightMargin = ApplicationDesignRules.rightMargin;
		float deckBlockUpMargin = ApplicationDesignRules.upMargin;
		float deckBlockDownMargin = ApplicationDesignRules.worldHeight-6.45f+ApplicationDesignRules.gapBetweenBlocks;

		this.deckBlock.GetComponent<NewBlockController> ().resize(deckBlockLeftMargin,deckBlockRightMargin,deckBlockUpMargin,deckBlockDownMargin);
		Vector3 deckBlockUpperLeftPosition = this.deckBlock.GetComponent<NewBlockController> ().getUpperLeftCornerPosition ();
		Vector3 deckBlockUpperRightPosition = this.deckBlock.GetComponent<NewBlockController> ().getUpperRightCornerPosition ();
		Vector2 deckBlockSize = this.deckBlock.GetComponent<NewBlockController> ().getSize ();
		this.deckBlockTitle.transform.position = new Vector3 (deckBlockUpperLeftPosition.x + 0.3f, deckBlockUpperLeftPosition.y - 0.2f, 0f);
		this.deckBlockTitle.transform.localScale = ApplicationDesignRules.mainTitleScale;
		
		float gapBetweenDecksButton = 0.1f;

		this.deckCreationButton.transform.position = new Vector3 (deckBlockUpperRightPosition.x - 0.3f - ApplicationDesignRules.button61WorldSize.x / 2f, deckBlockUpperRightPosition.y - 0.3f - 0.5f*ApplicationDesignRules.button61WorldSize.y/2f, 0f);
		this.deckCreationButton.transform.localScale = ApplicationDesignRules.button61Scale;

		this.deckSelectionButton.transform.position = new Vector3 (deckBlockUpperRightPosition.x - 0.3f - ApplicationDesignRules.button61WorldSize.x / 2f, deckBlockUpperRightPosition.y - 0.3f - 0.5f*ApplicationDesignRules.button61WorldSize.y/2f - (ApplicationDesignRules.button61WorldSize.y+gapBetweenDecksButton), 0f);
		this.deckSelectionButton.transform.localScale = ApplicationDesignRules.button61Scale;

		this.deckRenameButton.transform.position = new Vector3 (deckBlockUpperRightPosition.x - 0.3f - ApplicationDesignRules.button61WorldSize.x / 2f, deckBlockUpperRightPosition.y - 0.3f - 0.5f*ApplicationDesignRules.button61WorldSize.y/2f - 2*(ApplicationDesignRules.button61WorldSize.y+gapBetweenDecksButton), 0f);
		this.deckRenameButton.transform.localScale = ApplicationDesignRules.button61Scale;

		this.deckDeletionButton.transform.position = new Vector3 (this.deckRenameButton.transform.position.x - gapBetweenDecksButton - ApplicationDesignRules.button61WorldSize.x, this.deckRenameButton.transform.position.y, 0f);
		this.deckDeletionButton.transform.localScale = ApplicationDesignRules.button61Scale;

		for(int i=0;i<this.deckChoices.Length;i++)
		{
			this.deckChoices[i].transform.localScale=ApplicationDesignRules.listElementScale;
			this.deckChoices[i].transform.position=new Vector3(this.deckSelectionButton.transform.position.x,this.deckSelectionButton.transform.position.y-ApplicationDesignRules.button61WorldSize.y/2f-(i+0.5f)*ApplicationDesignRules.listElementWorldSize.y+i*0.02f,-1f);
		}
		
		this.deckTitle.transform.position = new Vector3 (deckBlockUpperLeftPosition.x + 0.3f, deckBlockUpperLeftPosition.y - 1.2f, 0f);
		this.deckTitle.GetComponent<TextContainer> ().width = deckBlockSize.x - 0.6f - gapBetweenDecksButton - 2f * ApplicationDesignRules.button61WorldSize.x;
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
			this.deckCards[i].transform.GetComponent<NewCardMyGameController>().setId(i,true);
		}

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
		this.cardsNumberTitle.transform.position = new Vector3 (cardsBlockUpperLeftPosition.x + 0.3f, cardsBlockUpperLeftPosition.y - 1.2f, 0f);
		this.cardsNumberTitle.transform.localScale = ApplicationDesignRules.subMainTitleScale;

		float gapBetweenCardsLine = 0.25f;
		float gapBetweenCard = gapBetweenCardsHalo;
		float firstLineY = deckCardsPosition [0].y;

		this.cardsArea = new Rect (cardsBlockUpperLeftPosition.x,cardsBlockLowerLeftPosition.y,cardsBlockSize.x,cardsBlockSize.y);

		this.cardsPosition=new Vector3[this.cardsPerLine*this.nbLines];
		
		for(int j=0;j<this.nbLines;j++)
		{
			for(int i =0;i<this.cardsPerLine;i++)
			{
				this.cards[j*(cardsPerLine)+i].transform.localScale= ApplicationDesignRules.cardScale;
				this.cardsPosition[j*(this.cardsPerLine)+i]=new Vector3(cardsBlockUpperLeftPosition.x+0.3f+ApplicationDesignRules.cardHaloWorldSize.x/2f+i*(gapBetweenCardsHalo+ApplicationDesignRules.cardHaloWorldSize.x),firstLineY-j*(gapBetweenCardsLine+ApplicationDesignRules.cardHaloWorldSize.y),0f);
				this.cards[j*(cardsPerLine)+i].transform.position=this.cardsPosition[j*(this.cardsPerLine)+i];
				this.cards[j*(this.cardsPerLine)+i].transform.name="Card"+(j*(this.cardsPerLine)+i);
			}
		}
		
		this.cardsPaginationButtons.transform.localPosition=new Vector3(cardsBlockLowerLeftPosition.x+cardsBlockSize.x/2f, cardsBlockLowerLeftPosition.y + 0.3f, 0f);
		this.cardsPaginationButtons.transform.GetComponent<newMyGamePaginationController> ().resize ();

		float lineScale = ApplicationDesignRules.getLineScale (cardsBlockSize.x - 0.6f);
		this.cardsPaginationLine.transform.localScale = new Vector3 (lineScale, 1f, 1f);
		this.cardsPaginationLine.transform.position = new Vector3 (cardsBlockLowerLeftPosition.x + cardsBlockSize.x / 2, cardsBlockLowerLeftPosition.y + 0.6f, 0f);

		this.focusedCard.transform.localScale = ApplicationDesignRules.cardFocusedScale;
		this.focusedCard.transform.position = new Vector3 (0f, -ApplicationDesignRules.worldHeight/2f+ApplicationDesignRules.downMargin+ApplicationDesignRules.cardFocusedWorldSize.y/2f-0.22f, 0f);
		this.focusedCard.transform.GetComponent<NewFocusedCardController> ().setCentralWindow (this.centralWindow);

		if(newDeckViewDisplayed)
		{
			this.newDeckPopUpResize();
		}
		else if(editDeckViewDisplayed)
		{
			this.editDeckPopUpResize();
		}
		else if(deleteDeckViewDisplayed)
		{
			this.deleteDeckPopUpResize();
		}
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
			this.deckTitle.GetComponent<TextMeshPro> ().text = "Aucune équipe créé pour le moment".ToUpper();
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
	}
	public void displayBackUI(bool value)
	{
		this.deckBlock.SetActive(value);
		this.deckBlockTitle.SetActive (value);
		this.deckCreationButton.SetActive (value);
		this.deckTitle.SetActive (value);
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
		if(deckDisplayed!=-1 && value)
		{
			this.deckDeletionButton.SetActive(true);
			this.deckRenameButton.SetActive(true);
		}
		else
		{
			this.deckDeletionButton.SetActive (false);
			this.deckRenameButton.SetActive(false);
		}
		for (int i=0;i<this.deckCards.Length;i++)
		{
			if(this.deckCardsDisplayed[i]!=-1)
			{
				this.deckCards[i].SetActive(value);
			}
		}
		for(int i=0;i<this.cardsHalos.Length;i++)
		{
			this.cardsHalos[i].SetActive(value);
		}
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
			this.cardsPaginationButtons.GetComponent<newMyGamePaginationController>().setPagination();
		}
		else
		{
			this.cardsPaginationButtons.GetComponent<newMyGamePaginationController>().setVisible(false);
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
		for (int i=0; i<this.availableFilters.Length; i++) 
		{
			this.availableFilters[i].SetActive(value);
		}
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
		this.availabilityFilterTitle.SetActive(value);
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
		this.isSearchingSkill = true;
		this.valueSkill = "";
		this.skillSearchBar.transform.FindChild("Title").GetComponent<TextMeshPro>().text = this.valueSkill;
		this.skillSearchBar.transform.GetComponent<newMyGameSkillSearchBarController> ().setIsSelected (true);
		this.skillSearchBar.transform.GetComponent<newMyGameSkillSearchBarController> ().setInitialState ();
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
	public void availabilityFilterHandler(int id)
	{
		if(id==0)
		{
			if(isOnSaleFilterOn)
			{
				isOnSaleFilterOn=false;
				this.availableFilters[0].GetComponent<newMyGameAvailabilityFilterController>().reset();
			}
			else
			{
				isOnSaleFilterOn=true;
				if(isNotOnSaleFilterOn)
				{
					isNotOnSaleFilterOn=false;
					this.availableFilters[1].GetComponent<newMyGameAvailabilityFilterController>().reset();
				}
				this.availableFilters[0].GetComponent<newMyGameAvailabilityFilterController>().setIsSelected(true);
				this.availableFilters[0].GetComponent<newMyGameAvailabilityFilterController>().setHoveredState();
			}
		}
		else if(id==1)
		{
			if(isNotOnSaleFilterOn)
			{
				isNotOnSaleFilterOn=false;
				this.availableFilters[1].GetComponent<newMyGameAvailabilityFilterController>().reset();
			}
			else
			{
				isNotOnSaleFilterOn=true;
				if(isOnSaleFilterOn)
				{
					isOnSaleFilterOn=false;
					this.availableFilters[0].GetComponent<newMyGameAvailabilityFilterController>().reset();
				}
				this.availableFilters[1].GetComponent<newMyGameAvailabilityFilterController>().setIsSelected(true);
				this.availableFilters[1].GetComponent<newMyGameAvailabilityFilterController>().setHoveredState();
			}
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

			if(this.isOnSaleFilterOn && model.cards.getCard(i).onSale == 1)
			{
				continue;
			}
			if(this.isNotOnSaleFilterOn && model.cards.getCard(i).onSale == 0)
			{
				continue;
			}
			if(this.isSkillChosen && !model.cards.getCard(i).hasSkill(this.valueSkill))
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
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Life;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Life;
						break;
					case 3:
						tempB = model.cards.getCard(this.cardsToBeDisplayed[i]).Life;
						tempA = model.cards.getCard(this.cardsToBeDisplayed[j]).Life;
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
						tempA = model.cards.getCard(this.cardsToBeDisplayed[i]).Speed;
						tempB = model.cards.getCard(this.cardsToBeDisplayed[j]).Speed;
						break;
					case 7:
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
		this.valueSkill = this.skillChoices[id].transform.FindChild("Title").GetComponent<TextMeshPro>().text.ToLower();
		this.isSkillChosen = true;
		this.skillSearchBar.transform.FindChild("Title").GetComponent<TextMeshPro>().text =valueSkill;
		this.cleanSkillAutocompletion ();
		this.cardsPagination.chosenPage = 0;
		this.applyFilters ();
	}
	public void mouseOnSearchBar(bool value)
	{
		this.isMouseOnSearchBar = value;
		print (value);
	}
	public void mouseOnSelectDeckButton(bool value)
	{
		this.isMouseOnSelectDeckButton = value;
	}
	public void displayNewDeckPopUp()
	{
		this.newDeckViewDisplayed = true;
		this.newDeckView = Camera.main.gameObject.AddComponent <NewMyGameNewDeckPopUpView>();
		newDeckView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
		newDeckView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		newDeckView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		newDeckView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (this.popUpSkin.textField);
		newDeckView.popUpVM.centralWindowErrorStyle = new GUIStyle (this.popUpSkin.customStyles [1]);
		newDeckView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		this.newDeckPopUpResize ();
	}
	public void displayEditDeckPopUp()
	{
		this.editDeckViewDisplayed = true;
		this.editDeckView = Camera.main.gameObject.AddComponent <NewMyGameEditDeckPopUpView>();
		editDeckView.editDeckPopUpVM.oldName = model.decks[this.deckDisplayed].Name;
		editDeckView.editDeckPopUpVM.newName = model.decks[this.deckDisplayed].Name;
		editDeckView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
		editDeckView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		editDeckView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		editDeckView.popUpVM.centralWindowTextfieldStyle = new GUIStyle (this.popUpSkin.textField);
		editDeckView.popUpVM.centralWindowErrorStyle = new GUIStyle (this.popUpSkin.customStyles [1]);
		editDeckView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		this.editDeckPopUpResize ();
	}
	public void displayDeleteDeckPopUp()
	{
		this.deleteDeckViewDisplayed = true;
		this.deleteDeckView = Camera.main.gameObject.AddComponent <NewMyGameDeleteDeckPopUpView>();
		deleteDeckView.deleteDeckPopUpVM.name = model.decks[this.deckDisplayed].Name;
		deleteDeckView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.window);
		deleteDeckView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		deleteDeckView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		deleteDeckView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		this.deleteDeckPopUpResize ();
	}
	public void hideNewDeckPopUp()
	{
		Destroy (this.newDeckView);
		this.newDeckViewDisplayed = false;
	}
	public void hideEditDeckPopUp()
	{
		Destroy (this.editDeckView);
		this.editDeckViewDisplayed = false;
	}
	public void hideDeleteDeckPopUp()
	{
		Destroy (this.deleteDeckView);
		this.deleteDeckViewDisplayed = false;
	}
	public void newDeckPopUpResize()
	{
		newDeckView.popUpVM.centralWindow = this.centralWindow;
		newDeckView.popUpVM.resize ();
	}
	public void editDeckPopUpResize()
	{
		editDeckView.popUpVM.centralWindow = this.centralWindow;
		editDeckView.popUpVM.resize ();
	}
	public void deleteDeckPopUpResize()
	{
		deleteDeckView.popUpVM.centralWindow = this.centralWindow;
		deleteDeckView.popUpVM.resize ();
	}
	public void createNewDeckHandler()
	{
		StartCoroutine (this.createNewDeck ());
	}
	private IEnumerator createNewDeck()
	{
		newDeckView.newDeckPopUpVM.error=this.checkDeckName(newDeckView.newDeckPopUpVM.name);
		if(newDeckView.newDeckPopUpVM.error=="")
		{
			this.hideNewDeckPopUp();
			MenuController.instance.displayLoadingScreen();
			this.newDeckView.popUpVM.guiEnabled=false;
			model.decks.Add(new Deck());
			yield return StartCoroutine(model.decks[model.decks.Count-1].create(newDeckView.newDeckPopUpVM.name));
			this.deckDisplayed=model.decks.Count-1;
			this.initializeDecks();
			this.initializeCards();
			if(this.isTutorialLaunched)
			{
				TutorialObjectController.instance.actionIsDone();
			}
			MenuController.instance.hideLoadingScreen();
		}
	}
	public void editDeckHandler()
	{
		StartCoroutine (this.editDeck ());
	}
	public IEnumerator editDeck()
	{
		if(editDeckView.editDeckPopUpVM.newName!=editDeckView.editDeckPopUpVM.oldName)
		{
			editDeckView.editDeckPopUpVM.error=checkDeckName(editDeckView.editDeckPopUpVM.newName);
			if(editDeckView.editDeckPopUpVM.error=="")
			{
				MenuController.instance.displayLoadingScreen();
				this.hideEditDeckPopUp();
				yield return StartCoroutine(model.decks[this.deckDisplayed].edit(editDeckView.editDeckPopUpVM.newName));
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
		if(!Regex.IsMatch(name, @"^[a-zA-Z0-9_\s]+$"))
		{
			return "Vous ne pouvez pas utiliser de caractères spéciaux";
		}
		for(int i=0;i<model.decks.Count;i++)
		{
			if(model.decks[i].Name==name)
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
		this.idCardClicked = id;
		this.isDeckCardClicked = isDeckCard;
		this.isLeftClicked = true;
		this.clickInterval = 0f;
	}
	public void leftClickReleaseHandler()
	{
		if(isLeftClicked)
		{
			this.isLeftClicked=false;
			bool onSale;
			int idOwner;
			if(this.isDeckCardClicked)
			{
				onSale=System.Convert.ToBoolean(model.cards.getCard(this.deckCardsDisplayed[this.idCardClicked]).onSale);
				idOwner=model.cards.getCard(this.deckCardsDisplayed[this.idCardClicked]).onSale;
			}
			else
			{
				onSale=System.Convert.ToBoolean(model.cards.getCard(this.cardsDisplayed[this.idCardClicked]).onSale);
				idOwner = model.cards.getCard(this.cardsDisplayed[this.idCardClicked]).IdOWner;
			}
			if(idOwner==-1)
			{
				MenuController.instance.displayErrorPopUp("Cette carte a été vendue, vous ne pouvez plus la consulter");
			}
			else if(isTutorialLaunched)
			{
				if(TutorialObjectController.instance.getSequenceID()==1)
				{
					this.showCardFocused();
					TutorialObjectController.instance.actionIsDone();
				}
			}
			else
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
	
		bool onSale;
		int idOwner;
		if(this.isDeckCardClicked)
		{
			onSale=System.Convert.ToBoolean(model.cards.getCard(this.deckCardsDisplayed[this.idCardClicked]).onSale);
			idOwner=model.cards.getCard(this.deckCardsDisplayed[this.idCardClicked]).onSale;
		}
		else
		{
			onSale=System.Convert.ToBoolean(model.cards.getCard(this.cardsDisplayed[this.idCardClicked]).onSale);
			idOwner = model.cards.getCard(this.cardsDisplayed[this.idCardClicked]).IdOWner;
		}

		if(this.deckDisplayed==-1)
		{
			MenuController.instance.displayErrorPopUp("Vous devez créer un deck avant de sélectionner une carte");
			this.isLeftClicked=false;
		}
		else if(onSale)
		{
			MenuController.instance.displayErrorPopUp("Vous ne pouvez pas ajouter à votre deck une carte qui est en vente");
			this.isLeftClicked=false;
		}
		else if(idOwner==-1)
		{
			MenuController.instance.displayErrorPopUp("Cette carte a été vendue, vous ne pouvez plus l'ajouter");
			this.isLeftClicked=false;
		}
		else if(!this.isTutorialLaunched || TutorialObjectController.instance.getSequenceID()==13)
		{
			this.isDragging=true;
			Cursor.SetCursor (this.cursorTextures[1], new Vector2(this.cursorTextures[1].width/2f,this.cursorTextures[1].width/2f), CursorMode.Auto);
			if(!isDeckCardClicked)
			{
				this.cards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(10,"Foreground");
			}
			else
			{
				this.deckCards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(10,"Foreground");
			}
			for(int i=0;i<this.cardsHalos.Length;i++)
			{
				this.cardsHalos[i].GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
			}
		}
	}
	public void isDraggingCard()
	{
		if(isDragging)
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			if(!isDeckCardClicked)
			{
				this.cards[this.idCardClicked].transform.position=new Vector3(mousePosition.x,mousePosition.y,0f);
			}
			else
			{
				this.deckCards[this.idCardClicked].transform.position=new Vector3(mousePosition.x,mousePosition.y,0f);
			}
		}
	}
	public void moveToCards()
	{
		StartCoroutine(removeCardFromDeck(this.idCardClicked));
		this.deckCards[this.idCardClicked].SetActive(false);
		this.deckCardsDisplayed[this.idCardClicked]=-1;
		this.applyFilters();
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
		}

		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector2 (Input.mousePosition.x, Input.mousePosition.y));
		if(this.cardsArea.Contains(cursorPosition) && isDeckCardClicked)
		{
			this.moveToCards();
		}
		else
		{
			print(cursorPosition);
			for(int i=0;i<deckCardsArea.Length;i++)
			{
				print (deckCardsArea[i]);
				if(this.deckCardsArea[i].Contains(cursorPosition))
				{
					this.moveToDeckCards(i);
					break;
				}
			}
		}
		if(this.isTutorialLaunched)
		{
			TutorialObjectController.instance.actionIsDone();
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
	public void returnPressed()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().returnPressed();
		}
		else if(this.newDeckViewDisplayed)
		{
			this.createNewDeckHandler();
		}
		else if(this.editDeckView)
		{
			this.editDeckHandler();
		}
		else if(this.deleteDeckViewDisplayed)
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
		else if(this.newDeckViewDisplayed)
		{
			this.hideNewDeckPopUp();
		}
		else if(this.editDeckView)
		{
			this.hideEditDeckPopUp();
		}
		else if(this.deleteDeckViewDisplayed)
		{
			this.hideDeleteDeckPopUp();
		}
	}
	public void closeAllPopUp()
	{
		if(this.newDeckViewDisplayed)
		{
			this.hideNewDeckPopUp();
		}
		if(this.editDeckView)
		{
			this.hideEditDeckPopUp();
		}
		if(this.deleteDeckViewDisplayed)
		{
			this.hideDeleteDeckPopUp();
		}
	}
	private IEnumerator refreshMyGame()
	{
		yield return StartCoroutine(model.refreshMyGame ());
		int index;
		if(isCardFocusedDisplayed && model.cards.getCard(this.focusedCardIndex).IdOWner==-1)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().setCardSold();
		}
		else if(this.isSceneLoaded)
		{
			for(int i = 0 ; i < this.cardsDisplayed.Count ; i++)
			{
				if(model.cards.getCard(this.cardsDisplayed[i]).IdOWner==-1)
				{
					this.cards[i].GetComponent<NewCardController>().displayPanelSold();
				}
			}
		}
	}
	public bool getIsTutorialLaunched()
	{
		return isTutorialLaunched;
	}
	public Vector3 getCardsPosition(int id)
	{
		return cards[id].transform.position;
		return new Vector3 ();
	}
	public Vector3 getFocusedCardHealthPointsPosition()
	{
		return new Vector3((this.focusedCard.transform.FindChild ("Life").FindChild ("Picto").position.x+this.focusedCard.transform.FindChild ("Life").FindChild ("Text").position.x)/2f,
		                   this.focusedCard.transform.FindChild ("Life").FindChild ("Text").position.y,
		                   this.focusedCard.transform.FindChild ("Life").FindChild ("Text").position.z);
	}
	public Vector3 getFocusedCardAttackPointsPosition()
	{
		return new Vector3((this.focusedCard.transform.FindChild ("Attack").FindChild ("Picto").position.x+this.focusedCard.transform.FindChild ("Attack").FindChild ("Text").position.x)/2f,
		                   this.focusedCard.transform.FindChild ("Attack").FindChild ("Text").position.y,
		                   this.focusedCard.transform.FindChild ("Attack").FindChild ("Text").position.z);
	}
	public Vector3 getFocusedCardQuicknessPointsPosition()
	{
		return new Vector3((this.focusedCard.transform.FindChild ("Quickness").FindChild ("Picto").position.x+this.focusedCard.transform.FindChild ("Quickness").FindChild ("Text").position.x)/2f,
		                   this.focusedCard.transform.FindChild ("Quickness").FindChild ("Text").position.y,
		                   this.focusedCard.transform.FindChild ("Quickness").FindChild ("Text").position.z);
	}
	public Vector3 getFocusedCardMovePointsPosition()
	{
		return new Vector3(this.focusedCard.transform.FindChild ("Move").FindChild ("Text").position.x,
		                   (this.focusedCard.transform.FindChild ("Move").FindChild ("Picto").position.y+this.focusedCard.transform.FindChild ("Move").FindChild ("Text").position.y)/2f,
		                   this.focusedCard.transform.FindChild ("Move").FindChild ("Text").position.z);
	}
	public Vector3 getFocusedCardSkill1Position()
	{
		return new Vector3(this.focusedCard.transform.FindChild ("Face").position.x,
		                   this.focusedCard.transform.FindChild ("Skill1").FindChild("Description").position.y,
		                   this.focusedCard.transform.FindChild ("Skill1").position.z);
	}
	public Vector3 getFocusedCardSkill0Position()
	{
		return new Vector3(this.focusedCard.transform.FindChild ("Face").position.x,
		                   this.focusedCard.transform.FindChild ("Skill0").FindChild("Description").position.y,
		                   this.focusedCard.transform.FindChild ("Skill0").position.z);
	}
	public Vector3 getFocusedCardExperienceLevelPosition()
	{
		return new Vector3(this.focusedCard.transform.FindChild ("Experience").FindChild("ExperienceLevel").position.x,
		                   this.focusedCard.transform.FindChild ("Experience").FindChild("ExperienceLevel").position.y,
		                   this.focusedCard.transform.FindChild ("Experience").FindChild("ExperienceLevel").position.z);
	}
	public Vector3 getFocusedCardExperienceGaugePosition()
	{
		return new Vector3(this.focusedCard.transform.FindChild ("Face").position.x,
		                    this.focusedCard.transform.FindChild ("Experience").FindChild("ExperienceGauge").position.y,
		                    this.focusedCard.transform.FindChild ("Experience").FindChild("ExperienceGauge").position.z);
	}
	public Vector3 getFocusedCardFeaturePosition(int id)
	{
		return this.focusedCard.transform.FindChild ("FocusFeature"+id).position;
	}
	public Vector3 getNewDeckButtonPosition()
	{
		return this.deckCreationButton.transform.position;
	}
	public Vector3 getFiltersPosition()
	{
		return this.filtersBlock.transform.position;
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
	public IEnumerator endTutorial()
	{
		MenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.player.setTutorialStep (3));
		//MenuController.instance.setTutorialLaunched (false);
		ApplicationModel.launchGameTutorial = true;
		ApplicationModel.gameType = 0;
		MenuController.instance.joinRandomRoomHandler();
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
}
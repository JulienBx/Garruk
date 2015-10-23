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
	public GameObject cardObject;
	public GameObject skillListObject;
	public GameObject paginationButtonObject;
	public GameObject deckListObject;
	public Texture2D[] cursorTextures;
	public GUISkin popUpSkin;
	public int refreshInterval;

	private GameObject menu;
	private GameObject tutorial;
	private GameObject deckBoard;
	private GameObject cardsBlock;
	private GameObject deckBlock;
	private GameObject filtersBlock;
	private GameObject filters;
	private GameObject[] deckCards;
	private GameObject[] cards;
	private GameObject[] cursors;
	private GameObject[] paginationButtons;
	private GameObject[] sortButtons;
	private GameObject[] toggleButtons;
	private GameObject focusedCard;
	private int focusedCardIndex;
	private bool isCardFocusedDisplayed;
	private IList<GameObject> matchValues;
	private IList<GameObject> deckList;
	
	private int widthScreen;
	private int heightScreen;
	private float worldWidth;
	private float worldHeight;
	private float pixelPerUnit;
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

	private int nbPages;
	private int nbPaginationButtonsLimit;
	private int nbLines;
	private int cardsPerLine;
	private int chosenPage;
	private int pageDebut;
	private int activePaginationButtonId;

	private NewMyGameNewDeckPopUpView newDeckView;
	private bool newDeckViewDisplayed;
	private NewMyGameEditDeckPopUpView editDeckView;
	private bool editDeckViewDisplayed;
	private NewMyGameDeleteDeckPopUpView deleteDeckView;
	private bool deleteDeckViewDisplayed;
	private NewMyGameErrorPopUpView errorView;
	private bool errorViewDisplayed;

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

	private int money;

	private bool isTutorialLaunched;
	private bool toResizeBackUI;

	private bool isLoadingScreenDisplayed;

	void Update()
	{	
		this.timer += Time.deltaTime;
		
		if (this.timer > this.refreshInterval) 
		{	
			this.timer=this.timer-this.refreshInterval;
			StartCoroutine(this.refreshMyGame());
		}

		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			if(toResizeBackUI)
			{
				this.toResizeBackUI=false;
			}
			this.resize();
			if(!toResizeBackUI)
			{
				this.initializeDecks();
				this.initializeCards();
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
			if(!Input.GetKey(KeyCode.Delete))
			{
				foreach (char c in Input.inputString) 
				{
					if(c==(char)KeyCode.Backspace && this.valueSkill.Length>0)
					{
						this.valueSkill = this.valueSkill.Remove(this.valueSkill.Length - 1);
						this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text = this.valueSkill;
						this.setSkillAutocompletion();
						if(this.valueSkill.Length==0)
						{
							this.isSearchingSkill=false;
							this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text ="Rechercher";
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
					this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text ="Rechercher";
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
					this.focusedCard.GetComponent<NewFocusedCardMyGameController>().updateFocusFeatures();
				}
			}
			this.money=ApplicationModel.credits;
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewMyGameModel ();
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108f;
		this.sortingOrder = -1;
		this.initializeScene ();
		this.resize ();
	}
	public IEnumerator initialization()
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.initializeMyGame ());
		this.retrieveDefaultDeck ();
		this.initializeFilters ();
		this.initializeDecks ();
		this.resetFiltersValue ();
		if(ApplicationModel.skillChosen!="")
		{
			this.isSkillChosen=true;
			this.valueSkill=ApplicationModel.skillChosen.ToLower();
			this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text=valueSkill;
		   	ApplicationModel.skillChosen="";
		}
		if(ApplicationModel.cardTypeChosen!=-1)
		{
			this.filtersCardType.Add (ApplicationModel.cardTypeChosen);
		   	ApplicationModel.cardTypeChosen=-1;
		}
		this.applyFilters ();
		newMenuController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
		this.money = ApplicationModel.credits;
		if(model.player.TutorialStep==2 || model.player.TutorialStep==3)
		{
			this.tutorial = Instantiate(this.tutorialObject) as GameObject;
			this.tutorial.AddComponent<MyGameTutorialController>();
			this.menu.GetComponent<newMenuController>().setTutorialLaunched(true);
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
	private void initializeFilters()
	{
		for(int i=2;i<12;i++)
		{
			this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle"+i).GetComponent<TextMeshPro>().text = model.cardTypeList[i-2];
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
		this.applyFilters ();
	}
	private void applyFilters()
	{
		this.computeFilters ();
		this.chosenPage = 0;
		this.pageDebut = 0 ;
		this.drawPagination();
		this.drawCards ();
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("newMenu");
		menu.AddComponent<newMyGameMenuController> ();
		menu.GetComponent<newMenuController> ().setCurrentPage (1);
		this.cardsBlock = Instantiate(this.blockObject) as GameObject;
		this.deckBlock = Instantiate(this.blockObject) as GameObject;
		this.filtersBlock = Instantiate(this.blockObject) as GameObject;
		this.deckBoard = GameObject.Find ("deckBoard");
		this.filters = GameObject.Find ("myGameFilters");
		this.deckCards=new GameObject[4];
		this.cards = new GameObject[0];
		this.matchValues=new List<GameObject>();
		this.deckList = new List<GameObject> ();
		this.paginationButtons = new GameObject[0];
		for (int i=0;i<4;i++)
		{
			this.deckCards[i]=GameObject.Find("deckCard"+i);
			this.deckCards[i].AddComponent<NewCardMyGameController>();
		}
		this.cursors=new GameObject[4];
		for (int i=0;i<4;i++)
		{
			this.cursors[i]=GameObject.Find("Cursor"+i);
		}
		this.sortButtons=new GameObject[8];
		for (int i=0;i<8;i++)
		{
			this.sortButtons[i]=GameObject.Find("Sort"+i);
		}
		this.toggleButtons=new GameObject[12];
		for (int i=0;i<12;i++)
		{
			this.toggleButtons[i]=GameObject.Find("Toggle"+i);
		}
		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardMyGameController> ();
		this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").FindChild("Title").GetComponent<TextMeshPro>().text = "Renommer";
		this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").FindChild("Title").GetComponent<TextMeshPro>().text = "Supprimer";
		this.deckBoard.transform.FindChild("deckList").FindChild("newDeckButton").FindChild("Title").GetComponent<TextMeshPro>().text = "Créer une équipe";
		this.deckBoard.transform.FindChild("deckList").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Mes équipes";
		this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text="Aucune équipe";
		this.filters.transform.FindChild("Title").GetComponent<TextMeshPro>().text = "Filtres";
		this.filters.transform.FindChild ("onSaleFilters").FindChild("Toggle0").GetComponent<TextMeshPro>().text = "Unités disponibles";
		this.filters.transform.FindChild ("onSaleFilters").FindChild("Toggle1").GetComponent<TextMeshPro>().text = "Unités indisponibles";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Title").GetComponent<TextMeshPro>().text = "Factions";
		this.filters.transform.FindChild("skillSearch").FindChild("Title").GetComponent<TextMeshPro>().text = "Compétences";
		this.filters.transform.FindChild("powerFilter").FindChild ("Title").GetComponent<TextMeshPro>().text = "Puissance";
		this.filters.transform.FindChild("lifeFilter").FindChild ("Title").GetComponent<TextMeshPro>().text = "Vie";
		this.filters.transform.FindChild("attackFilter").FindChild ("Title").GetComponent<TextMeshPro>().text = "Attaque";
		this.filters.transform.FindChild("quicknessFilter").FindChild ("Title").GetComponent<TextMeshPro>().text = "Rapidité";
	}
	private void resetFiltersValue()
	{
		this.powerVal = 0;
		this.lifeVal = 0;
		this.attackVal = 0;
		this.quicknessVal = 0;
		this.filters.transform.FindChild("powerFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(powerVal);
		this.filters.transform.FindChild("lifeFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(lifeVal);
		this.filters.transform.FindChild("attackFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(attackVal);
		this.filters.transform.FindChild("quicknessFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(quicknessVal);

		for(int i=0;i<this.cursors.Length;i++)
		{
			Vector3 cursorPosition = this.cursors [i].transform.localPosition;
			cursorPosition.x=-0.975f;
			this.cursors[i].transform.localPosition=cursorPosition;
		}


		this.filtersCardType = new List<int> ();
		for(int i=0;i<this.toggleButtons.Length;i++)
		{
			this.toggleButtons[i].GetComponent<MyGameFiltersToggleController>().setActive(false);
		}
		this.valueSkill = "";
		this.isSkillChosen = false;
		this.isOnSaleFilterOn = false;
		this.isNotOnSaleFilterOn = false;
		this.cleanSkillAutocompletion ();
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text ="Rechercher";
		if(this.sortingOrder!=-1)
		{
			this.sortButtons[this.sortingOrder].GetComponent<MyGameFiltersSortController>().setActive(false);
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
		if(!toResizeBackUI)
		{
			this.resizeMainParameters();
			this.resizeFocusedCard();
		}
		if(this.isCardFocusedDisplayed)
		{
			toResizeBackUI=true;
			this.focusedCard.GetComponent<NewFocusedCardController>().resize();
		}
		else
		{
			this.resizeBackUI();
		}
		if(this.isTutorialLaunched)
		{
			this.tutorial.GetComponent<TutorialObjectController>().resize();
		}
	}
	public void resizeMainParameters()
	{
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.collectionPointsWindow=new Rect(this.widthScreen - this.widthScreen * 0.17f-5,0.1f * this.heightScreen+5,this.widthScreen * 0.17f,this.heightScreen * 0.1f);
		this.newSkillsWindow = new Rect (this.collectionPointsWindow.xMin, this.collectionPointsWindow.yMax + 5,this.collectionPointsWindow.width,this.heightScreen - 0.1f * this.heightScreen - 2 * 5 - this.collectionPointsWindow.height);
		this.newCardTypeWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);
	}
	public void resizeBackUI()
	{
		this.cleanCards ();
		float screenRatio = (float)this.widthScreen / (float)this.heightScreen;
		float selectButtonWidth=219f;
		float selectButtonScale = 1.4f;
		float deleteRenameButtonScale = 0.7f;
		float deleteRenameButtonWidth = 219;
		float cardHaloWidth = 740f;
		float cardScale = 0.222f;
		float deckCardsInterval = 1.7f;
		
		float selectButtonWorldWidth = selectButtonScale*(selectButtonWidth / pixelPerUnit);
		float deleteRenameButtonWorldWidth = deleteRenameButtonScale*(deleteRenameButtonWidth / pixelPerUnit);
		float cardHaloWorldWidth = cardScale * (cardHaloWidth / pixelPerUnit);
		float deckCardsWidth = deckCardsInterval * 3f + cardHaloWorldWidth;
		float cardsBoardLeftMargin = 3f;
		float cardsBoardRightMargin = 3f;
		float cardsBoardUpMargin;
		float deckBlockDownMargin;
		float cardsBoardDownMargin = 0.2f;
		
		float tempWidth = worldWidth - cardsBoardLeftMargin - cardsBoardRightMargin - selectButtonWorldWidth - deckCardsWidth;
		
		if(tempWidth>0.25f)
		{
			this.deckBoard.transform.position=new Vector3(selectButtonWorldWidth/2f +tempWidth/4f,3.645f,0f);
			this.deckBoard.transform.FindChild("deckList").localPosition=new Vector3(-deckCardsWidth/2f-tempWidth/2f-selectButtonWorldWidth/2f,0,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").localPosition=new Vector3(0f,0.27f,0f);
			this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").localPosition=new Vector3(-selectButtonWorldWidth/2f+deleteRenameButtonWorldWidth/2f,-0.27f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").localPosition=new Vector3(selectButtonWorldWidth/2f-deleteRenameButtonWorldWidth/2f,-0.27f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("newDeckButton").localPosition=new Vector3(-0.93f,-0.74f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("Title").localPosition=new Vector3(0,0.86f,0f);
			deckBlockDownMargin = 7.5f;
		}
		else
		{
			this.deckBoard.transform.position=new Vector3(0,2.25f,0f);
			this.deckBoard.transform.FindChild("deckList").localPosition=new Vector3(0,1.6f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").localPosition=new Vector3(0.34f,0,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").localPosition=new Vector3(2.5f,0.15f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").localPosition=new Vector3(2.5f,-0.15f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("newDeckButton").localPosition=new Vector3(-3f,0,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("Title").localPosition=new Vector3(0,0.69f,0f);
			deckBlockDownMargin = 6f;
		}
		
		float deckBlockLeftMargin = 3f;
		float deckBlockRightMargin = 3f;
		float deckBlockUpMargin = 0.2f;
		
		float deckBlockHeight = worldHeight - deckBlockUpMargin-deckBlockDownMargin;
		float deckBlockWidth = worldWidth-deckBlockLeftMargin-deckBlockRightMargin;
		Vector2 deckBlockOrigin = new Vector3 (-worldWidth/2f+deckBlockLeftMargin+deckBlockWidth/2f, -worldHeight / 2f + deckBlockDownMargin + deckBlockHeight / 2,0f);
		
		this.deckBlock.GetComponent<BlockController> ().resize(new Rect(deckBlockOrigin.x,deckBlockOrigin.y,deckBlockWidth,deckBlockHeight));
		
		cardsBoardUpMargin= worldHeight-deckBlockDownMargin+0.2f; 
		float cardsBoardHeight = worldHeight - cardsBoardUpMargin-cardsBoardDownMargin;
		float cardsBoardWidth = worldWidth-cardsBoardLeftMargin-cardsBoardRightMargin;
		Vector2 cardsBoardOrigin = new Vector3 (-worldWidth/2f+cardsBoardLeftMargin+cardsBoardWidth/2f, -worldHeight / 2f + cardsBoardDownMargin + cardsBoardHeight / 2,0f);
		
		this.cardsArea = new Rect (cardsBoardOrigin.x-cardsBoardWidth/2f, cardsBoardOrigin.y-cardsBoardHeight/2f, cardsBoardWidth, cardsBoardHeight);
		
		float cardWidth = 720f;
		float cardHeight = 1004f;
		float cardWorldWidth = (cardWidth / pixelPerUnit) * cardScale;
		float cardWorldHeight = (cardHeight / pixelPerUnit) * cardScale;
		
		this.cardsBlock.GetComponent<BlockController> ().resize(new Rect (cardsBoardOrigin.x, cardsBoardOrigin.y, cardsBoardWidth, cardsBoardHeight));
		
		this.deckCardsPosition=new Vector3[4];
		this.deckCardsArea=new Rect[4];
		
		for(int i=0;i<4;i++)
		{
			this.deckCardsPosition[i]=this.deckBoard.transform.FindChild("Card"+i).position;
			this.deckCardsArea[i]=new Rect(this.deckCardsPosition[i].x-cardWorldWidth/2f,this.deckCardsPosition[i].y-cardWorldHeight/2f,cardWorldWidth,cardWorldHeight);
			this.deckCards[i].transform.position=this.deckCardsPosition[i];
			this.deckCards[i].transform.localScale=new Vector3(cardScale,cardScale,cardScale);
			this.deckCards[i].transform.GetComponent<NewCardMyGameController>().setId(i,true);
			this.deckCards[i].SetActive(false);
		}
		
		this.cardsPerLine = Mathf.FloorToInt ((cardsBoardWidth-0.5f) / cardWorldWidth);
		this.nbLines = Mathf.FloorToInt ((cardsBoardHeight-0.6f) / cardWorldHeight);
		
		float gapWidth = (cardsBoardWidth - (this.cardsPerLine * cardWorldWidth)) / (this.cardsPerLine + 1);
		float gapHeight = (cardsBoardHeight - 0.45f - (this.nbLines * cardWorldHeight)) / (this.nbLines + 1);
		float cardBoardStartX = cardsBoardOrigin.x - cardsBoardWidth / 2f-cardWorldWidth/2f;
		float cardBoardStartY = cardsBoardOrigin.y + cardsBoardHeight / 2f+cardWorldHeight/2f;
		
		this.cards=new GameObject[this.cardsPerLine*this.nbLines];
		this.cardsPosition=new Vector3[this.cardsPerLine*this.nbLines];
		
		for(int j=0;j<this.nbLines;j++)
		{
			for(int i =0;i<this.cardsPerLine;i++)
			{
				this.cards[j*(cardsPerLine)+i] = Instantiate(this.cardObject) as GameObject;
				this.cards[j*(cardsPerLine)+i].AddComponent<NewCardMyGameController>();
				this.cards[j*(cardsPerLine)+i].transform.localScale= new Vector3(cardScale,cardScale,cardScale);
				this.cardsPosition[j*(this.cardsPerLine)+i]=new Vector3(cardBoardStartX+(i+1)*(gapWidth+cardWorldWidth),cardBoardStartY-(j+1)*(gapHeight+cardWorldHeight),0f);
				this.cards[j*(cardsPerLine)+i].transform.position=this.cardsPosition[j*(this.cardsPerLine)+i];
				this.cards[j*(this.cardsPerLine)+i].transform.name="Card"+(j*(this.cardsPerLine)+i);
				this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardMyGameController>().setId(j*(cardsPerLine)+i,false);
				this.cards[j*(this.cardsPerLine)+i].SetActive(false);
			}
		}
		
		float filtersBlockLeftMargin = this.worldWidth-2.8f;
		float filtersBlockRightMargin = 0f;
		float filtersBlockUpMargin = 0.6f;
		float filtersBlockDownMargin = 0.2f;
		
		float filtersBlockHeight = worldHeight - filtersBlockUpMargin-filtersBlockDownMargin;
		float filtersBlockWidth = worldWidth-filtersBlockLeftMargin-filtersBlockRightMargin;
		Vector2 filtersBlockOrigin = new Vector3 (-worldWidth/2f+filtersBlockLeftMargin+filtersBlockWidth/2f, -worldHeight / 2f + filtersBlockDownMargin + filtersBlockHeight / 2,0f);
		
		this.filtersBlock.GetComponent<BlockController> ().resize(new Rect(filtersBlockOrigin.x,filtersBlockOrigin.y, filtersBlockWidth, filtersBlockHeight));
		
		this.filters.transform.position = new Vector3 (worldWidth/2f - 1.4f, 0f, 0f);
		this.focusedCard.SetActive (false);
		
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
		else if(errorViewDisplayed)
		{
			this.errorPopUpResize();
		}
	}
	public void resizeFocusedCard()
	{
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
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text = model.decks[this.deckDisplayed].Name;
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(true);
			this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").gameObject.SetActive(true);
			this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").gameObject.SetActive(true);
		}
		else
		{
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text="Aucun deck créé";
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(false);
			this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").gameObject.SetActive(false);
			this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").gameObject.SetActive(false);
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
		if(this.toResizeBackUI)
		{
			this.resize();
			this.toResizeBackUI=false;
			this.initializeDecks();
			this.initializeCards();
		}
		else
		{
			this.focusedCard.SetActive (false);
		}
		this.displayBackUI (true);
		if(isDeckCardClicked)
		{
			this.deckCards[this.idCardClicked].GetComponent<NewCardController>().show();
		}
		else
		{
			this.cards[this.idCardClicked].GetComponent<NewCardController>().show();
		}
	}
	public void displayBackUI(bool value)
	{
		this.deckBoard.SetActive (value);
		this.cardsBlock.GetComponent<BlockController> ().display (value);
		this.deckBlock.GetComponent<BlockController> ().display (value);
		this.filtersBlock.GetComponent<BlockController> ().display (value);
		this.filters.SetActive (value);
		for(int i=0;i<this.cardsDisplayed.Count;i++)
		{
			this.cards[i].SetActive(value);
		}
		for(int i=0;i<this.deckCardsDisplayed.Length;i++)
		{
			if(this.deckCardsDisplayed[i]!=-1)
			{
				this.deckCards[i].SetActive(value);
			}
		}
		for(int i=0;i<this.paginationButtons.Length;i++)
		{
			this.paginationButtons[i].SetActive(value);
		}
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
			this.deckList[this.deckList.Count-1].GetComponent<DeckBoardDeckListMyGameController>().setId(i);
		}
	}
	public void cleanCards()
	{
		for (int i=0;i<this.cards.Length;i++)
		{
			Destroy (this.cards[i]);
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
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text = this.valueSkill;
	}
	public void changeToggle(int toggleId)
	{
		if(toggleId>1)
		{
			toggleId=toggleId-2;
			if(this.filtersCardType.Contains(toggleId))
			{
				this.filtersCardType.Remove(toggleId);
			}
			else
			{
				this.filtersCardType.Add (toggleId);
			}
		}
		else if(toggleId==0)
		{
			if(isOnSaleFilterOn)
			{
				isOnSaleFilterOn=false;
			}
			else
			{
				isOnSaleFilterOn=true;
				if(isNotOnSaleFilterOn)
				{
					isNotOnSaleFilterOn=false;
					this.filters.transform.FindChild("onSaleFilters").FindChild("Toggle1").GetComponent<MyGameFiltersToggleController>().setActive(false);
				}
			}
		}
		else if(toggleId==1)
		{
			if(isNotOnSaleFilterOn)
			{
				isNotOnSaleFilterOn=false;
			}
			else
			{
				isNotOnSaleFilterOn=true;
				if(isOnSaleFilterOn)
				{
					isOnSaleFilterOn=false;
					this.filters.transform.FindChild("onSaleFilters").FindChild("Toggle0").GetComponent<MyGameFiltersToggleController>().setActive(false);
				}
			}
		}
		this.applyFilters ();
	}
	public void changeSort(int id)
	{
		if(this.sortingOrder==id)
		{
			this.sortingOrder = -1;
		}
		else if(this.sortingOrder!=-1)
		{
			this.sortButtons[this.sortingOrder].GetComponent<MyGameFiltersSortController>().setActive(false);
			this.sortingOrder = id;
		}
		else
		{
			this.sortingOrder=id;
		}
		this.applyFilters ();
	}
	public void moveCursors(int cursorId)
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		float mousePositionX=mousePosition.x;
		Vector3 cursorPosition = this.cursors [cursorId].transform.localPosition;
		float offset = mousePositionX-this.cursors [cursorId].transform.position.x;

		int value = -1;
		string label = "";

		bool isMoved = true ;

		if(cursorPosition.x==-0.975f)
		{
			if(offset>0.975f)
			{
				value = 2;
				cursorPosition.x=+0.975f;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else if(offset>0.975/2f)
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
			if(offset>0.975f/2f)
			{
				value =2;
				cursorPosition.x=+0.975f;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else if(offset<-0.975f/2f)
			{
				value = 0;
				cursorPosition.x=-0.975f;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else
			{
				isMoved=false;
			}
		}
		else if(cursorPosition.x==0.975f)
		{
			if(offset<-0.975f)
			{
				value = 0;
				cursorPosition.x=-0.975f;
				this.cursors [cursorId].transform.localPosition = cursorPosition;
			}
			else if(offset<-0.975/2f)
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
				this.filters.transform.FindChild("powerFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(powerVal);
				break;
			case 1:
				lifeVal=value;
				this.filters.transform.FindChild("lifeFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(lifeVal);
				break;
			case 2:
				attackVal=value;
				this.filters.transform.FindChild("attackFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(attackVal);
				break;
			case 3:
				quicknessVal=value;
				this.filters.transform.FindChild("quicknessFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(quicknessVal);
				break;
			}
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
		this.cleanSkillAutocompletion ();
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text = this.valueSkill;
		if(this.valueSkill.Length>0)
		{
			for (int i = 0; i < model.skillsList.Count; i++) 
			{  
				if (model.skillsList [i].ToLower ().Contains (this.valueSkill)) 
				{
					this.matchValues.Add (Instantiate(this.skillListObject) as GameObject);
					this.matchValues[this.matchValues.Count-1].transform.parent=this.filters.transform.FindChild("skillSearch");
					this.matchValues[this.matchValues.Count-1].transform.localPosition=new Vector3(0, -0.55f+(this.matchValues.Count-1)*(-0.27f),0f);
					this.matchValues[this.matchValues.Count-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text = model.skillsList [i];
				}
			}
		}
	}
	private void cleanSkillAutocompletion ()
	{
		for(int i=0;i<this.matchValues.Count;i++)
		{
			Destroy(this.matchValues[i]);
		}
		this.matchValues=new List<GameObject>();
	}
	public void filterASkill(string skill)
	{
		this.isSearchingSkill = false;
		this.valueSkill = skill.ToLower();
		this.isSkillChosen = true;
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text =valueSkill;
		this.cleanSkillAutocompletion ();
		this.applyFilters ();
	}
	public void mouseOnSearchBar(bool value)
	{
		this.isMouseOnSearchBar = value;
	}
	public void mouseOnSelectDeckButton(bool value)
	{
		this.isMouseOnSelectDeckButton = value;
	}
	private void drawPagination()
	{
		for(int i=0;i<this.paginationButtons.Length;i++)
		{
			Destroy (this.paginationButtons[i]);
		}
		this.paginationButtons = new GameObject[0];
		this.activePaginationButtonId = -1;
		float paginationButtonWidth = 0.34f;
		float gapBetweenPaginationButton = 0.2f * paginationButtonWidth;
		this.nbPages = Mathf.CeilToInt((float)this.cardsToBeDisplayed.Count / ((float)this.nbLines*(float)this.cardsPerLine));
		if(this.nbPages>1)
		{
			this.nbPaginationButtonsLimit = Mathf.CeilToInt((this.worldWidth/2f)/(paginationButtonWidth+gapBetweenPaginationButton));
			int nbButtonsToDraw=0;
			bool drawBackButton=false;
			if (this.pageDebut !=0)
			{
				drawBackButton=true;
			}
			bool drawNextButton=false;
			if (this.pageDebut+nbPaginationButtonsLimit-System.Convert.ToInt32(drawBackButton)<this.nbPages-1)
			{
				drawNextButton=true;
				nbButtonsToDraw=nbPaginationButtonsLimit;
			}
			else
			{
				nbButtonsToDraw=this.nbPages-this.pageDebut;
				if(drawBackButton)
				{
					nbButtonsToDraw++;
				}
			}
			this.paginationButtons = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtons[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtons[i].AddComponent<MyGamePaginationController>();
				this.paginationButtons[i].transform.position=new Vector3((0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
				this.paginationButtons[i].name="Pagination"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtons[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebut+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtons[i].GetComponent<MyGamePaginationController>().setId(i);
				if(this.pageDebut+i-System.Convert.ToInt32(drawBackButton)==this.chosenPage)
				{
					this.paginationButtons[i].GetComponent<MyGamePaginationController>().setActive(true);
					this.activePaginationButtonId=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtons[0].GetComponent<MyGamePaginationController>().setId(-2);
				this.paginationButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtons[nbButtonsToDraw-1].GetComponent<MyGamePaginationController>().setId(-1);
				this.paginationButtons[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
		}
	}
	public void paginationHandler(int id)
	{
		if(id==-2)
		{
			this.pageDebut=this.pageDebut-this.nbPaginationButtonsLimit+1+System.Convert.ToInt32(this.pageDebut-this.nbPaginationButtonsLimit+1!=0);
			this.drawPagination();
		}
		else if(id==-1)
		{
			this.pageDebut=this.pageDebut+this.nbPaginationButtonsLimit-1-System.Convert.ToInt32(this.pageDebut!=0);
			this.drawPagination();
		}
		else
		{
			if(activePaginationButtonId!=-1)
			{
				this.paginationButtons[this.activePaginationButtonId].GetComponent<MyGamePaginationController>().setActive(false);
			}
			this.activePaginationButtonId=id;
			this.chosenPage=this.pageDebut-System.Convert.ToInt32(this.pageDebut!=0)+id;
			this.drawCards();
		}
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
	public void displayErrorPopUp(string error)
	{
		this.errorViewDisplayed = true;
		this.errorView = Camera.main.gameObject.AddComponent <NewMyGameErrorPopUpView>();
		errorView.errorPopUpVM.error = error;
		errorView.popUpVM.centralWindowStyle = new GUIStyle(this.popUpSkin.customStyles[3]);
		errorView.popUpVM.centralWindowTitleStyle = new GUIStyle (this.popUpSkin.customStyles [0]);
		errorView.popUpVM.centralWindowButtonStyle = new GUIStyle (this.popUpSkin.button);
		errorView.popUpVM.transparentStyle = new GUIStyle (this.popUpSkin.customStyles [2]);
		this.errorPopUpResize ();
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
	public void hideErrorPopUp()
	{
		Destroy (this.errorView);
		this.errorViewDisplayed = false;
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
	public void errorPopUpResize()
	{
		errorView.popUpVM.centralWindow = this.centralWindow;
		errorView.popUpVM.resize ();
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
			newMenuController.instance.displayLoadingScreen();
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
			newMenuController.instance.hideLoadingScreen();
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
				newMenuController.instance.displayLoadingScreen();
				this.hideEditDeckPopUp();
				yield return StartCoroutine(model.decks[this.deckDisplayed].edit(editDeckView.editDeckPopUpVM.newName));
				this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text = model.decks[this.deckDisplayed].Name;
				newMenuController.instance.hideLoadingScreen();
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
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine(model.decks[this.deckDisplayed].delete());
		this.removeDeckFromAllCards (model.decks[this.deckDisplayed].Id);
		model.decks.RemoveAt (this.deckDisplayed);
		this.retrieveDefaultDeck ();
		this.initializeDecks ();
		//this.drawCards ();
		this.initializeCards ();
		newMenuController.instance.hideLoadingScreen ();
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
//	public void rightClickedHandler(int id, bool isDeckCard)
//	{
//		this.idCardClicked = id;
//		this.isDeckCardClicked = isDeckCard;
//		bool onSale;
//		int idOwner;
//		if(isDeckCard)
//		{
//			onSale=System.Convert.ToBoolean(model.cards[this.deckCardsDisplayed[id]].onSale);
//			idOwner=model.cards[this.deckCardsDisplayed[id]].onSale;
//		}
//		else
//		{
//			onSale=System.Convert.ToBoolean(model.cards[this.cardsDisplayed[id]].onSale);
//			idOwner = model.cards[this.cardsDisplayed[id]].IdOWner;
//		}
//		if(idOwner==-1)
//		{
//			this.displayErrorPopUp("Cette carte a été vendue, vous ne pouvez plus la consulter");
//		}
//		else if(isTutorialLaunched)
//		{
//			if(TutorialObjectController.instance.getSequenceID()==1)
//			{
//				this.showCardFocused();
//				TutorialObjectController.instance.actionIsDone();
//			}
//		}
//		else
//		{
//			this.showCardFocused ();
//		}
//	}
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
				this.displayErrorPopUp("Cette carte a été vendue, vous ne pouvez plus la consulter");
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
			this.displayErrorPopUp("Vous devez créer un deck avant de sélectionner une carte");
			this.isLeftClicked=false;
		}
		else if(onSale)
		{
			this.displayErrorPopUp("Vous ne pouvez pas ajouter à votre deck une carte qui est en vente");
			this.isLeftClicked=false;
		}
		else if(idOwner==-1)
		{
			this.displayErrorPopUp("Cette carte a été vendue, vous ne pouvez plus l'ajouter");
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
				//this.cardsBoard.GetComponent<BoardController> ().changeColor (new Color (155f / 255f, 220f / 255f, 1f));
			}
			this.deckBoard.GetComponent<DeckBoardController> ().changeCardsColor (new Color (155f / 255f, 220f / 255f, 1f));
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
			//print (this.cards[0].transform.FindChild("Face").GetComponent<SpriteRenderer>().sortingLayerID);
			this.cards[this.idCardClicked].transform.position=this.cardsPosition[this.idCardClicked];
			this.cards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(-10,"Foreground");
		}
		else
		{
			this.deckCards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(-10,"Foreground");
			this.deckCards[this.idCardClicked].transform.position=this.deckCardsPosition[this.idCardClicked];
			//this.cardsBoard.GetComponent<BoardController> ().changeColor (new Color (1f,1f, 1f));
		}
		this.deckBoard.GetComponent<DeckBoardController> ().changeCardsColor (new Color (1f,1f, 1f));bool toCards=false;

		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		if(this.cardsArea.Contains(cursorPosition) && isDeckCardClicked)
		{
			this.moveToCards();
		}
		else
		{
			for(int i=0;i<deckCardsArea.Length;i++)
			{
				if(this.deckCardsArea[i].Contains(Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z))))
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
	public void refreshCredits()
	{
		StartCoroutine(this.menu.GetComponent<newMenuController> ().getUserData ());
	}
	public void deleteCard()
	{
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
		else if(this.errorViewDisplayed)
		{
			this.hideErrorPopUp();
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
		else if(this.errorViewDisplayed)
		{
			this.hideErrorPopUp();
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
		if(this.errorViewDisplayed)
		{
			this.hideErrorPopUp();
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
		return new Vector3(this.deckBoard.transform.FindChild ("deckList").FindChild ("newDeckButton").FindChild ("Title").position.x+this.deckBoard.transform.FindChild("deckList").FindChild ("newDeckButton").FindChild ("Title").GetComponent<TextContainer>().width/3f,
		                   this.deckBoard.transform.FindChild ("deckList").FindChild ("newDeckButton").FindChild("Title").position.y,
		                   this.deckBoard.transform.FindChild ("deckList").FindChild ("newDeckButton").FindChild("Title").position.z);
	}
	public Vector3 getFiltersPosition()
	{
		return this.filters.transform.position;
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
		this.deckBoard.transform.FindChild ("deckList").FindChild ("newDeckButton").GetComponent<BoxCollider2D> ().enabled = value;
		this.deckBoard.transform.FindChild ("deckList").FindChild ("deleteDeckButton").GetComponent<BoxCollider2D> ().enabled = value;
		this.deckBoard.transform.FindChild ("deckList").FindChild ("renameDeckButton").GetComponent<BoxCollider2D> ().enabled = value;
		this.deckBoard.transform.FindChild ("deckList").FindChild ("currentDeck").FindChild ("selectButton").GetComponent<BoxCollider2D> ().enabled = value;

	}
	public IEnumerator endTutorial()
	{
		newMenuController.instance.displayLoadingScreen ();
		yield return StartCoroutine (model.player.setTutorialStep (3));
		//newMenuController.instance.setTutorialLaunched (false);
		ApplicationModel.launchGameTutorial = true;
		ApplicationModel.gameType = 0;
		newMenuController.instance.joinRandomRoomHandler();
	}
}
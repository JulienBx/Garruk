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
	
	public GameObject cardObject;
	public GameObject skillListObject;
	public GameObject paginationButtonObject;
	public GameObject blockObject;
	public GameObject tutorialObject;
	public GUISkin popUpSkin;
	public int totalNbResultLimit;
	public int refreshInterval;

	private GameObject menu;
	private GameObject tutorial;
	private GameObject filters;
	private GameObject[] cards;
	private GameObject[] paginationButtons;
	private GameObject[] sortButtons;
	private GameObject[] cursors;
	private GameObject[] toggleButtons;
	private GameObject focusedCard;
	private GameObject refreshMarketButton;
	private GameObject filtersBlock;
	private GameObject cardsBlock;
	private int focusedCardIndex;
	private bool isCardFocusedDisplayed;
	private IList<GameObject> matchValues;
	
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
	private bool isSkillChosen;
	private bool isMouseOnSearchBar;
	private string valueSkill;

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
	private int nbPaginationButtonsLimit;
	private int nbLines;
	private int cardsPerLine;
	private int chosenPage;
	private int pageDebut;
	private int activePaginationButtonId;

	private NewMarketErrorPopUpView errorView;
	private bool errorViewDisplayed;

	private int idCardClicked;
	
	private float timer;
	private float refreshMarketButtonTimer;
	private bool isSceneLoaded;

	private int money;

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

		if(money!=ApplicationModel.credits)
		{
			if(isSceneLoaded)
			{
				if(this.isCardFocusedDisplayed)
				{
					this.toUpdateCardsMarketFeatures=true;
					this.focusedCard.GetComponent<NewFocusedCardMarketController>().updateFocusFeatures();
				}
				else
				{
					this.updateCardsMarketFeatures();
				}
			}
			this.money=ApplicationModel.credits;
		}

		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();
			this.initializeCards();
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
		if(this.areNewCardsAvailable)
		{
			if(!this.isCardFocusedDisplayed)
			{
				this.refreshMarketButtonTimer += Time.deltaTime;
				if(this.refreshMarketButtonTimer>0.5f)
				{
					this.refreshMarketButtonTimer=0f;
					this.refreshMarketButton.GetComponent<RefreshMarketController>().changeColor();
				}
			}
		}
	}
	void Awake()
	{
		instance = this;
		this.model = new NewMarketModel ();
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
		yield return StartCoroutine (model.initializeMarket (this.totalNbResultLimit));
		this.initializeFilters ();
		this.initializeCards ();
		newMenuController.instance.hideLoadingScreen ();
		this.isSceneLoaded = true;
		this.money = ApplicationModel.credits;
		if(!model.player.MarketTutorial)
		{
			this.tutorial = Instantiate(this.tutorialObject) as GameObject;
			this.tutorial.AddComponent<MarketTutorialController>();
			StartCoroutine(this.tutorial.GetComponent<MarketTutorialController>().launchSequence(0));
			this.menu.GetComponent<newMenuController>().setTutorialLaunched(true);
			this.isTutorialLaunched=true;
		} 
	}
	private void initializeCards()
	{
		this.resetFiltersValue ();
		this.applyFilters ();
	}
	private void initializeFilters()
	{
		for(int i=0;i<10;i++)
		{
			this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle"+i).GetComponent<TextMeshPro>().text = model.cardTypeList[i];
		}
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
		menu.AddComponent<newMarketMenuController> ();
		menu.GetComponent<newMenuController> ().setCurrentPage (3);
		this.filters = GameObject.Find ("marketFilters");
		this.filtersBlock = Instantiate(this.blockObject) as GameObject;
		this.cards = new GameObject[0];
		this.cardsBlock = Instantiate(this.blockObject) as GameObject;
		this.matchValues=new List<GameObject>();
		this.paginationButtons = new GameObject[0];
		this.cursors=new GameObject[6];
		for (int i=0;i<6;i++)
		{
			this.cursors[i]=GameObject.Find("Cursor"+i);
		}
		this.sortButtons=new GameObject[10];
		for (int i=0;i<10;i++)
		{
			this.sortButtons[i]=GameObject.Find("Sort"+i);
		}
		this.toggleButtons=new GameObject[10];
		for (int i=0;i<10;i++)
		{
			this.toggleButtons[i]=GameObject.Find("Toggle"+i);
		}
		this.refreshMarketButton = GameObject.Find ("RefreshMarket");
		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardMarketController> ();
		this.filters.transform.FindChild("Title").GetComponent<TextMeshPro>().text = "Filtres";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Title").GetComponent<TextMeshPro>().text = "Factions";
		this.filters.transform.FindChild("skillSearch").FindChild("Title").GetComponent<TextMeshPro>().text = "Compétences";
		this.filters.transform.FindChild("priceFilter").FindChild ("Title").GetComponent<TextMeshPro>().text = "Prix";
		this.filters.transform.FindChild("powerFilter").FindChild ("Title").GetComponent<TextMeshPro>().text = "Puissance";
		this.filters.transform.FindChild("lifeFilter").FindChild ("Title").GetComponent<TextMeshPro>().text = "Vie";
		this.filters.transform.FindChild("attackFilter").FindChild ("Title").GetComponent<TextMeshPro>().text = "Attaque";
		this.filters.transform.FindChild("quicknessFilter").FindChild ("Title").GetComponent<TextMeshPro>().text = "Rapidité";
		this.refreshMarketButton.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Actualiser";
		this.refreshMarketButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Nouvelles cartes disponibles";
	}
	private void resetFiltersValue()
	{

		if(model.cards.Count>0)
		{
			this.maxPriceLimit=model.cards[0].Price;
			this.minPriceLimit=model.cards[0].Price;
		}

		for(int i=0;i<model.cards.Count;i++)
		{
			if(model.cards[i].Price>maxPriceLimit)
			{
				this.maxPriceLimit=model.cards[i].Price;
			}
			if(model.cards[i].Price<minPriceLimit)
			{
				this.minPriceLimit=model.cards[i].Price;
			}
		}
		this.minPriceVal = this.minPriceLimit;
		this.maxPriceVal = this.maxPriceLimit;

		this.filters.transform.FindChild("priceFilter").FindChild ("MinValue").GetComponent<TextMeshPro>().text = this.minPriceVal.ToString();
		this.filters.transform.FindChild("priceFilter").FindChild ("MaxValue").GetComponent<TextMeshPro>().text = this.maxPriceVal.ToString();

		Vector3 cursor0Position = this.cursors [0].transform.localPosition;
		cursor0Position.x=-0.975f;
		this.cursors[0].transform.localPosition=cursor0Position;
		
		Vector3 cursor1Position = this.cursors [1].transform.localPosition;
		cursor1Position.x=0.975f;
		this.cursors[1].transform.localPosition=cursor1Position;

		this.powerVal = 0;
		this.lifeVal = 0;
		this.attackVal = 0;
		this.quicknessVal = 0;
		this.filters.transform.FindChild("powerFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(powerVal);
		this.filters.transform.FindChild("lifeFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(lifeVal);
		this.filters.transform.FindChild("attackFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(attackVal);
		this.filters.transform.FindChild("quicknessFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(quicknessVal);
		
		for(int i=2;i<this.cursors.Length;i++)
		{
			Vector3 cursorPosition = this.cursors [i].transform.localPosition;
			cursorPosition.x=-0.975f;
			this.cursors[i].transform.localPosition=cursorPosition;
		}

		this.filtersCardType = new List<int> ();
		for(int i=0;i<this.toggleButtons.Length;i++)
		{
			this.toggleButtons[i].GetComponent<MarketFiltersToggleController>().setActive(false);
		}
		this.valueSkill = "";
		this.isSkillChosen = false;
		this.cleanSkillAutocompletion ();
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMeshPro>().text ="Rechercher";
		if(this.sortingOrder!=-1)
		{
			this.sortButtons[this.sortingOrder].GetComponent<MarketFiltersSortController>().setActive(false);
			this.sortingOrder = -1;
		}
	}
	public void resize()
	{
		if(this.isCardFocusedDisplayed)
		{
			this.hideCardFocused();
		}
		this.cleanCards ();
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.collectionPointsWindow=new Rect(this.widthScreen - this.widthScreen * 0.17f-5,0.1f * this.heightScreen+5,this.widthScreen * 0.17f,this.heightScreen * 0.1f);
		this.newSkillsWindow = new Rect (this.collectionPointsWindow.xMin, this.collectionPointsWindow.yMax + 5,this.collectionPointsWindow.width,this.heightScreen - 0.1f * this.heightScreen - 2 * 5 - this.collectionPointsWindow.height);
		this.newCardTypeWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		float screenRatio = (float)this.widthScreen / (float)this.heightScreen;
		float cardScale = 0.222f;
		
		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);

		float cardsBoardLeftMargin = 2.9f;
		float cardsBoardRightMargin = 2.9f;
		float cardsBoardUpMargin=0.5f;
		float cardsBoardDownMargin = 0.5f;
		
		float cardsBoardHeight = worldHeight - cardsBoardUpMargin-cardsBoardDownMargin;
		float cardsBoardWidth = worldWidth-cardsBoardLeftMargin-cardsBoardRightMargin;
		Vector2 cardsBoardOrigin = new Vector3 (-worldWidth/2f+cardsBoardLeftMargin+cardsBoardWidth/2f, -worldHeight / 2f + cardsBoardDownMargin + cardsBoardHeight / 2,0f);
		
		float cardWidth = 720f;
		float cardHeight = 1004f;
		float panelMarketHeight = cardHeight / 6f;
		float cardWorldWidth = (cardWidth / pixelPerUnit) * cardScale;
		float cardWorldHeight = ((cardHeight+panelMarketHeight) / pixelPerUnit) * cardScale;
		
		this.cardsPerLine = Mathf.FloorToInt ((cardsBoardWidth-0.5f) / cardWorldWidth);
		this.nbLines = Mathf.FloorToInt ((cardsBoardHeight-0.5f) / cardWorldHeight);
		
		float gapWidth = (cardsBoardWidth - (this.cardsPerLine * cardWorldWidth)) / (this.cardsPerLine + 1);
		float gapHeight = (cardsBoardHeight - (this.nbLines * cardWorldHeight)) / (this.nbLines + 1);
		float cardBoardStartX = cardsBoardOrigin.x - cardsBoardWidth / 2f-cardWorldWidth/2f;
		float cardBoardStartY = cardsBoardOrigin.y + cardsBoardHeight / 2f+cardWorldHeight/2f;
		
		this.cards=new GameObject[this.cardsPerLine*this.nbLines];
		
		for(int j=0;j<this.nbLines;j++)
		{
			for(int i =0;i<this.cardsPerLine;i++)
			{
				this.cards[j*(cardsPerLine)+i] = Instantiate(this.cardObject) as GameObject;
				this.cards[j*(cardsPerLine)+i].AddComponent<NewCardMarketController>();
				this.cards[j*(cardsPerLine)+i].transform.localScale= new Vector3(cardScale,cardScale,cardScale);
				this.cards[j*(cardsPerLine)+i].transform.position=new Vector3(cardBoardStartX+(i+1)*(gapWidth+cardWorldWidth),cardBoardStartY+1/12f*cardWorldHeight-(j+1)*(gapHeight+cardWorldHeight),0f);
				this.cards[j*(this.cardsPerLine)+i].transform.name="Card"+(j*(this.cardsPerLine)+i);
				this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardMarketController>().setId(j*(cardsPerLine)+i);
				this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardMarketController> ().setCentralWindow (this.centralWindow);
				this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardMarketController> ().setCollectionPointsWindow (this.collectionPointsWindow);
				this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardMarketController> ().setNewSkillsWindow (this.newSkillsWindow);
				this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardMarketController> ().setNewCardTypeWindow (this.newCardTypeWindow);
				this.cards[j*(this.cardsPerLine)+i].SetActive(false);
			}
		}
		
		this.filters.transform.position = new Vector3 (worldWidth/2f - 1.4f, 0f, 0f);

		float filtersBlockLeftMargin = this.worldWidth-2.8f;
		float filtersBlockRightMargin = 0f;
		float filtersBlockUpMargin = 0.6f;
		float filtersBlockDownMargin = 0.2f;
		
		float filtersBlockHeight = worldHeight - filtersBlockUpMargin-filtersBlockDownMargin;
		float filtersBlockWidth = worldWidth-filtersBlockLeftMargin-filtersBlockRightMargin;
		Vector2 filtersBlockOrigin = new Vector3 (-worldWidth/2f+filtersBlockLeftMargin+filtersBlockWidth/2f, -worldHeight / 2f + filtersBlockDownMargin + filtersBlockHeight / 2,0f);
		
		this.filtersBlock.GetComponent<BlockController> ().resize(new Rect(filtersBlockOrigin.x,filtersBlockOrigin.y, filtersBlockWidth, filtersBlockHeight));


		float cardsBlockLeftMargin = 3f;
		float cardsBlockRightMargin = 3f;
		float cardsBlockUpMargin = 0.6f;
		float cardsBlockDownMargin = 0.2f;
		
		float cardsBlockHeight = worldHeight - cardsBlockUpMargin-cardsBlockDownMargin;
		float cardsBlockWidth = worldWidth-cardsBlockLeftMargin-cardsBlockRightMargin;
		Vector2 cardsBlockOrigin = new Vector3 (-worldWidth/2f+cardsBlockLeftMargin+cardsBlockWidth/2f, -worldHeight / 2f + cardsBlockDownMargin + cardsBlockHeight / 2,0f);
		
		this.cardsBlock.GetComponent<BlockController> ().resize(new Rect(cardsBlockOrigin.x,cardsBlockOrigin.y, cardsBlockWidth, cardsBlockHeight));

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

		this.refreshMarketButton.transform.position = new Vector3 (this.cards [this.cardsPerLine - 1].transform.position.x + cardWorldWidth / 2f, this.worldHeight/2f-cardsBoardUpMargin, 0);
		if(this.areNewCardsAvailable)
		{
			this.refreshMarketButton.SetActive(true);
		}
		else
		{
			this.refreshMarketButton.SetActive(false);
		}
		if(errorViewDisplayed)
		{
			this.errorPopUpResize();
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
				if(this.chosenPage*(this.nbLines*this.cardsPerLine)+j*(cardsPerLine)+i<this.cardsToBeDisplayed.Count)
				{
					this.cardsDisplayed.Add (this.cardsToBeDisplayed[this.chosenPage*(this.nbLines*this.cardsPerLine)+j*(cardsPerLine)+i]);
					this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardController>().c=model.cards[this.cardsDisplayed[j*(cardsPerLine)+i]];
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
		this.focusedCard.GetComponent<NewFocusedCardController>().c=model.cards[this.focusedCardIndex];
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
		this.filters.SetActive (value);
		this.filtersBlock.GetComponent<BlockController> ().display (value);
		this.cardsBlock.GetComponent<BlockController> ().display (value);
		for(int i=0;i<this.cardsDisplayed.Count;i++)
		{
			this.cards[i].SetActive(value);
		}
		for(int i=0;i<this.paginationButtons.Length;i++)
		{
			this.paginationButtons[i].SetActive(value);
		}
		if(areNewCardsAvailable)
		{
			this.refreshMarketButton.SetActive(value);
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
		if(this.filtersCardType.Contains(toggleId))
		{
			this.filtersCardType.Remove(toggleId);
		}
		else
		{
			this.filtersCardType.Add (toggleId);
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
			this.sortButtons[this.sortingOrder].GetComponent<MarketFiltersSortController>().setActive(false);
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
			case 2:
				powerVal=value;
				this.filters.transform.FindChild("powerFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(powerVal);
				break;
			case 3:
				lifeVal=value;
				this.filters.transform.FindChild("lifeFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(lifeVal);
				break;
			case 4:
				attackVal=value;
				this.filters.transform.FindChild("attackFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(attackVal);
				break;
			case 5:
				quicknessVal=value;
				this.filters.transform.FindChild("quicknessFilter").FindChild ("Value").GetComponent<TextMeshPro>().text = getValueFilterLabel(quicknessVal);
				break;
			}
			this.applyFilters();
		}
	}
	public void moveMinMaxCursor(int cursorId)
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		float mousePositionX=mousePosition.x;
		Vector3 cursorPosition = this.cursors [cursorId].transform.position;
		float sliderPositionX = this.cursors [cursorId].transform.parent.gameObject.transform.position.x;
		float cursorSizeX = (78f / 108f) * 0.3f;
		float offset = mousePositionX-cursorPosition.x;
		cursorPosition.x = cursorPosition.x + offset;
		int secondCursorId;
		float secondCursorPositionX;
		float distance;
		if(cursorId%2==0)
		{
			secondCursorId=cursorId+1;
			secondCursorPositionX = this.cursors [secondCursorId].transform.position.x;
			if(cursorPosition.x>secondCursorPositionX-cursorSizeX)
			{
				cursorPosition.x=secondCursorPositionX-cursorSizeX;
			}
			else if(cursorPosition.x<-0.975f+sliderPositionX)
			{
				cursorPosition.x=-0.975f+sliderPositionX;
			}
			distance = cursorPosition.x -(-0.975f+sliderPositionX);
		}
		else
		{
			secondCursorId=cursorId-1;
			secondCursorPositionX = this.cursors [secondCursorId].transform.position.x;
			if(cursorPosition.x>0.975f+sliderPositionX)
			{
				cursorPosition.x=0.975f+sliderPositionX;
			}
			else if(cursorPosition.x<secondCursorPositionX+cursorSizeX)
			{
				cursorPosition.x=secondCursorPositionX+cursorSizeX;
			}
			distance = (0.975f+sliderPositionX)-cursorPosition.x;
		}
		this.cursors [cursorId].transform.position = cursorPosition;
		float maxDistance = 2 * 0.975f-cursorSizeX;
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
				this.filters.transform.FindChild("priceFilter").FindChild ("MinValue").GetComponent<TextMeshPro>().text = minPriceVal.ToString();
				isMoved=true;
			}
			break;
		case 1:
			maxPriceVal=maxPriceLimit-Mathf.FloorToInt(ratio*(maxPriceLimit-minPriceLimit));
			if(maxPriceVal!=oldMaxPriceVal)
			{
				this.filters.transform.FindChild("priceFilter").FindChild ("MaxValue").GetComponent<TextMeshPro>().text = maxPriceVal.ToString();
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
	private void computeFilters() 
	{
		this.cardsToBeDisplayed=new List<int>();
		int nbFilters = this.filtersCardType.Count;
		int max = model.cards.Count;
		
		for(int i=0;i<max;i++)
		{

			if(this.isSkillChosen && !model.cards [i].hasSkill(this.valueSkill))
			{
				continue;
			}
			if(nbFilters>0)
			{
				bool testCardTypes=false;
				for(int j=0;j<nbFilters;j++)
				{
					if (model.cards [i].IdClass == this.filtersCardType [j])
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
			if(model.cards[i].PowerLevel-1>=this.powerVal&&
			   model.cards[i].AttackLevel-1>=this.attackVal&&
			   model.cards[i].LifeLevel-1>=this.lifeVal&&
			   model.cards[i].SpeedLevel-1>=this.quicknessVal&&
			   model.cards[i].Price>=this.minPriceVal&&
			   model.cards[i].Price<=this.maxPriceVal)
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
						tempA = model.cards[this.cardsToBeDisplayed[i]].Price;
						tempB = model.cards[this.cardsToBeDisplayed[j]].Price;
						break;
					case 1:
						tempB = model.cards[this.cardsToBeDisplayed[i]].Price;
						tempA = model.cards[this.cardsToBeDisplayed[j]].Price;
						break;
					case 2:
						tempA = model.cards[this.cardsToBeDisplayed[i]].Power;
						tempB = model.cards[this.cardsToBeDisplayed[j]].Power;
						break;
					case 3:
						tempB = model.cards[this.cardsToBeDisplayed[i]].Power;
						tempA = model.cards[this.cardsToBeDisplayed[j]].Power;
						break;
					case 4:
						tempA = model.cards[this.cardsToBeDisplayed[i]].Life;
						tempB = model.cards[this.cardsToBeDisplayed[j]].Life;
						break;
					case 5:
						tempB = model.cards[this.cardsToBeDisplayed[i]].Life;
						tempA = model.cards[this.cardsToBeDisplayed[j]].Life;
						break;
					case 6:
						tempA = model.cards[this.cardsToBeDisplayed[i]].Attack;
						tempB = model.cards[this.cardsToBeDisplayed[j]].Attack;
						break;
					case 7:
						tempB = model.cards[this.cardsToBeDisplayed[i]].Attack;
						tempA = model.cards[this.cardsToBeDisplayed[j]].Attack;
						break;
					case 8:
						tempA = model.cards[this.cardsToBeDisplayed[i]].Speed;
						tempB = model.cards[this.cardsToBeDisplayed[j]].Speed;
						break;
					case 9:
						tempB = model.cards[this.cardsToBeDisplayed[i]].Speed;
						tempA = model.cards[this.cardsToBeDisplayed[j]].Speed;
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
		
		this.filters.transform.FindChild("priceFilter").FindChild ("MinValue").GetComponent<TextMeshPro>().text = this.minPriceVal.ToString();
		this.filters.transform.FindChild("priceFilter").FindChild ("MaxValue").GetComponent<TextMeshPro>().text = this.maxPriceVal.ToString();

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
				this.paginationButtons[i].AddComponent<MarketPaginationController>();
				this.paginationButtons[i].transform.position=new Vector3((0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.55f,0f);
				this.paginationButtons[i].name="Pagination"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtons[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=(this.pageDebut+i-System.Convert.ToInt32(drawBackButton)).ToString();
				this.paginationButtons[i].GetComponent<MarketPaginationController>().setId(i);
				if(this.pageDebut+i-System.Convert.ToInt32(drawBackButton)==this.chosenPage)
				{
					this.paginationButtons[i].GetComponent<MarketPaginationController>().setActive(true);
					this.activePaginationButtonId=i;
				}
			}
			if(drawBackButton)
			{
				this.paginationButtons[0].GetComponent<MarketPaginationController>().setId(-2);
				this.paginationButtons[0].transform.FindChild("Title").GetComponent<TextMeshPro>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtons[nbButtonsToDraw-1].GetComponent<MarketPaginationController>().setId(-1);
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
				this.paginationButtons[this.activePaginationButtonId].GetComponent<MarketPaginationController>().setActive(false);
			}
			this.activePaginationButtonId=id;
			this.chosenPage=this.pageDebut-System.Convert.ToInt32(this.pageDebut!=0)+id;
			this.drawCards();
		}
	}
	public void displayErrorPopUp(string error)
	{
		this.errorViewDisplayed = true;
		this.errorView = Camera.main.gameObject.AddComponent <NewMarketErrorPopUpView>();
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
	public void errorPopUpResize()
	{
		errorView.popUpVM.centralWindow = this.centralWindow;
		errorView.popUpVM.resize ();
	}
	public void leftClickedHandler(int id)
	{
		this.idCardClicked = id;
		bool onSale=System.Convert.ToBoolean(model.cards[this.cardsDisplayed[id]].onSale);
		int idOwner=model.cards[this.cardsDisplayed[this.idCardClicked]].IdOWner;
		if(idOwner!=-1)
		{
			this.showCardFocused ();
		}
		else
		{
			this.displayErrorPopUp("Cette carte a été vendue, vous ne pouvez plus la consulter");
		}
	}
	public void refreshCredits()
	{
		StartCoroutine(this.menu.GetComponent<newMenuController> ().getUserData ());
	}
	public void returnPressed()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().returnPressed();
		}
		else if(this.errorViewDisplayed)
		{
			this.hideErrorPopUp();
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
		else if(this.errorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
		else
		{
			this.cards[this.idCardClicked].GetComponent<NewCardController>().escapePressed();
		}
	}
	public void closeAllPopUp()
	{
		if(this.errorViewDisplayed)
		{
			this.hideErrorPopUp();
		}
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
			if(model.cards[this.focusedCardIndex].IdOWner==-1)
			{
				this.focusedCard.GetComponent<NewFocusedCardMarketController>().setCardSold();
			}
			this.toUpdateCardsMarketFeatures=true;
		}
		else
		{
			this.updateCardsMarketFeatures();
		}
		if(model.newCards.Count>0)
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
		for(int i=0;i<model.newCards.Count;i++)
		{
			model.cards.Insert(i,model.newCards[i]);
		}
		model.newCards = new List<Card> ();
		for(int i =0;i<model.cards.Count;i++)
		{
			if(model.cards[model.cards.Count-i-1].onSale==0)
			{
				model.cards.RemoveAt(model.cards.Count-i-1);
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
		return this.filters.transform.position;
	}
	public IEnumerator endTutorial(bool toUpdate)
	{
		Destroy (this.tutorial);
		this.isTutorialLaunched = false;
		newMenuController.instance.setTutorialLaunched (false);
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
}
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class newMyGameController : MonoBehaviour
{
	public static newMyGameController instance;
	private NewMyGameModel model;

	public GameObject cardObject;
	public GameObject skillListObject;
	public GameObject paginationButtonObject;
	public GameObject deckListObject;
	public Texture2D[] cursorTextures;
	public GUISkin popUpSkin;

	private GameObject menu;
	private GameObject deckBoard;
	private GameObject cardsBoard;
	private GameObject filters;
	private GameObject deckBoardTitle;
	private GameObject[] deckCards;
	private GameObject[] cards;
	private GameObject[] cursors;
	private GameObject[] paginationButtons;
	private GameObject[] sortButtons;
	private GameObject draggedCard;
	private IList<GameObject> matchValues;
	private IList<GameObject> deckList;
	
	private int widthScreen;
	private int heightScreen;
	private float worldWidth;
	private float worldHeight;
	private float pixelPerUnit;
	private Rect centralWindow;

	private bool isSearchingSkill;
	private bool isSearchingDeck;
	private bool isMouseOnSelectDeckButton;
	private bool isSkillChosen;
	private bool isMouseOnSearchBar;
	private bool isOnSaleFilterOn;
	private bool isNotOnSaleFilterOn;
	private string valueSkill;

	private float minPowerVal;
	private float maxPowerVal;
	private float minPowerLimit;
	private float maxPowerLimit;
	private float minLifeVal;
	private float maxLifeVal;
	private float minLifeLimit;
	private float maxLifeLimit;
	private float minAttackVal;
	private float maxAttackVal;
	private float minAttackLimit;
	private float maxAttackLimit;
	private float minQuicknessVal;
	private float maxQuicknessVal;
	private float minQuicknessLimit;
	private float maxQuicknessLimit;
	private float oldMinPowerVal;
	private float oldMaxPowerVal;
	private float oldMinLifeVal;
	private float oldMaxLifeVal;
	private float oldMinAttackVal;
	private float oldMaxAttackVal;
	private float oldMinQuicknessVal;
	private float oldMaxQuicknessVal;
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

	private bool isDragging;
	private bool isLeftClicked;
	private bool isHovering;
	private float clickInterval;
	private int idCardClicked;
	private Vector3[] cardsPosition;
	private Vector3[] deckCardsPosition;
	private Texture2D cursorTexture;
	void Update()
	{	
		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();
			this.initializeDecks();
			this.initializeCards();
		}
		if(isLeftClicked)
		{
			this.clickInterval=this.clickInterval+Time.deltaTime*10f;
			if(this.clickInterval>2f)
			{
				this.isLeftClicked=false;
				this.isDragging=true;
				Cursor.SetCursor (this.cursorTextures[1], new Vector2(this.cursorTextures[1].width/2f,this.cursorTextures[1].width/2f), CursorMode.Auto);
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
						this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text = this.valueSkill;
						this.setSkillAutocompletion();
						if(this.valueSkill.Length==0)
						{
							this.isSearchingSkill=false;
							this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text ="Rechercher";
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
					this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text ="Rechercher";
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
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108f;
		this.sortingOrder = -1;
		this.initializeScene ();
	}
	void Start()
	{
		instance = this;
		this.model = new NewMyGameModel ();
		this.resize ();
		StartCoroutine (this.initialization ());
	}
	private IEnumerator initialization()
	{
		yield return StartCoroutine (model.initializeMyGame ());
		this.retrieveDefaultDeck ();
		this.initializeDecks ();
		this.initializeCards ();
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
		menu.GetComponent<newMenuController> ().setCurrentPage (1);
		this.deckBoard = GameObject.Find ("deckBoard");
		this.cardsBoard = GameObject.Find ("cardsBoard");
		this.filters = GameObject.Find ("myGameFilters");
		this.deckBoardTitle = GameObject.Find ("deckBoardTitle");
		this.deckCards=new GameObject[4];
		this.cards = new GameObject[0];
		this.matchValues=new List<GameObject>();
		this.deckList = new List<GameObject> ();
		this.paginationButtons = new GameObject[0];
		for (int i=0;i<4;i++)
		{
			this.deckCards[i]=GameObject.Find("deckCard"+i);
		}
		this.cursors=new GameObject[8];
		for (int i=0;i<8;i++)
		{
			this.cursors[i]=GameObject.Find("Cursor"+i);
		}
		this.sortButtons=new GameObject[8];
		for (int i=0;i<8;i++)
		{
			this.sortButtons[i]=GameObject.Find("Sort"+i);
		}
		this.draggedCard = GameObject.Find ("draggedCard");

		this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").FindChild("Title").GetComponent<TextMesh>().text = "Renommer";
		this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").FindChild("Title").GetComponent<TextMesh>().text = "Supprimer";
		this.deckBoard.transform.FindChild("deckList").FindChild("newDeckButton").FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.deckBoard.transform.FindChild("deckList").FindChild("newDeckButton").FindChild("Title").GetComponent<TextMesh>().text = "Nouveau deck";
		this.deckBoard.transform.FindChild("deckList").FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.deckBoard.transform.FindChild("deckList").FindChild ("Title").GetComponent<TextMesh> ().text = "Mes decks";
		this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMesh> ().text="Aucun deck créé";
		this.filters.transform.FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("Title").GetComponent<TextMesh>().text = "Filtres";
		this.filters.transform.FindChild ("onSaleFilters").FindChild("Toggle0").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild ("onSaleFilters").FindChild("Toggle0").GetComponent<TextMesh>().text = "Cartes mises en vente";
		this.filters.transform.FindChild ("onSaleFilters").FindChild("Toggle1").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild ("onSaleFilters").FindChild("Toggle1").GetComponent<TextMesh>().text = "Cartes non mises en vente";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Title").GetComponent<TextMesh>().text = "Classes";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle2").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle3").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle4").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle5").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle6").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle7").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle8").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle9").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle10").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle11").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle2").GetComponent<TextMesh>().text = "Enchanteur";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle3").GetComponent<TextMesh>().text = "Roublard";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle4").GetComponent<TextMesh>().text = "Berserk";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle5").GetComponent<TextMesh>().text = "Artificier";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle6").GetComponent<TextMesh>().text = "Mentaliste";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle7").GetComponent<TextMesh>().text = "Androide";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle8").GetComponent<TextMesh>().text = "Metamorphe";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle9").GetComponent<TextMesh>().text = "Pretre";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle10").GetComponent<TextMesh>().text = "Animiste";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle11").GetComponent<TextMesh>().text = "Geomancien";
		this.filters.transform.FindChild("skillSearch").FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("skillSearch").FindChild("Title").GetComponent<TextMesh>().text = "Compétences";
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("powerFilter").FindChild ("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("powerFilter").FindChild ("Title").GetComponent<TextMesh>().text = "Puissance";
		this.filters.transform.FindChild("powerFilter").FindChild ("MinValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("powerFilter").FindChild ("MaxValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("lifeFilter").FindChild ("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("lifeFilter").FindChild ("Title").GetComponent<TextMesh>().text = "Vie";
		this.filters.transform.FindChild("lifeFilter").FindChild ("MinValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("lifeFilter").FindChild ("MaxValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("attackFilter").FindChild ("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("attackFilter").FindChild ("Title").GetComponent<TextMesh>().text = "Attaque";
		this.filters.transform.FindChild("attackFilter").FindChild ("MinValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("attackFilter").FindChild ("MaxValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("quicknessFilter").FindChild ("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("quicknessFilter").FindChild ("Title").GetComponent<TextMesh>().text = "Rapidité";
		this.filters.transform.FindChild("quicknessFilter").FindChild ("MinValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("quicknessFilter").FindChild ("MaxValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
	}
	private void resetFiltersValue()
	{
		this.minPowerVal=0;
		this.maxPowerVal=100;
		this.minPowerLimit=0;
		this.maxPowerLimit=100;
		this.minLifeVal = 0;
		this.maxLifeVal = 100;
		this.minLifeLimit=0;
		this.maxLifeLimit=100;
		this.minAttackVal=0;
		this.maxAttackVal=100;
		this.minAttackLimit=0;
		this.maxAttackLimit=100;
		this.minQuicknessVal=0;
		this.maxQuicknessVal=100;
		this.minQuicknessLimit=0;
		this.maxQuicknessLimit=100;
		this.oldMinPowerVal=0;
		this.oldMaxPowerVal=100;
		this.oldMinLifeVal=0;
		this.oldMaxLifeVal=100;
		this.oldMinAttackVal=0;
		this.oldMaxAttackVal=100;
		this.oldMinQuicknessVal=0;
		this.oldMaxQuicknessVal=100;
		this.filtersCardType = new List<int> ();
		this.valueSkill = "";
		this.isSkillChosen = false;
		this.isOnSaleFilterOn = false;
		this.isNotOnSaleFilterOn = false;
		this.cleanSkillAutocompletion ();
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text ="Rechercher";
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
		this.cleanCards ();
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		float screenRatio = (float)this.widthScreen / (float)this.heightScreen;
		float selectButtonWidth=219f;
		float selectButtonScale = 1.4f;
		float deleteRenameButtonScale = 0.7f;
		float deleteRenameButtonWidth = 219;
		float cardHaloWidth = 200f;
		float cardScale = 0.83f;
		float deckCardsInterval = 1.7f;

		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);

		//deckBoard.GetComponent<deckBoardController>().resize(screenRatio);


		float selectButtonWorldWidth = selectButtonScale*(selectButtonWidth / pixelPerUnit);
		float deleteRenameButtonWorldWidth = deleteRenameButtonScale*(deleteRenameButtonWidth / pixelPerUnit);
		float cardHaloWorldWidth = cardScale * (cardHaloWidth / pixelPerUnit);
		float deckCardsWidth = deckCardsInterval * 3f + cardHaloWorldWidth;
		float cardsBoardLeftMargin = 2.9f;
		float cardsBoardRightMargin = 2.9f;
		float cardsBoardUpMargin;
		float cardsBoardDownMargin = 0.5f;

		float tempWidth = worldWidth - cardsBoardLeftMargin - cardsBoardRightMargin - selectButtonWorldWidth - deckCardsWidth;

		if(tempWidth>0.25f)
		{
			this.deckBoard.transform.position=new Vector3(selectButtonWorldWidth/2f +tempWidth/4f,3.4f,0);
			this.deckBoard.transform.FindChild("deckList").localPosition=new Vector3(-deckCardsWidth/2f-tempWidth/2f-selectButtonWorldWidth/2f,0,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").localPosition=new Vector3(0f,0.27f,0f);
			this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").localPosition=new Vector3(-selectButtonWorldWidth/2f+deleteRenameButtonWorldWidth/2f,-0.27f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").localPosition=new Vector3(selectButtonWorldWidth/2f-deleteRenameButtonWorldWidth/2f,-0.27f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("newDeckButton").localPosition=new Vector3(-0.93f,-0.74f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("Title").localPosition=new Vector3(0,0.86f,0);
			this.deckBoard.transform.FindChild("3stars").localPosition=new Vector3(-2.55f,1.29f,0);
			this.deckBoard.transform.FindChild("2stars").localPosition=new Vector3(-0.85f,1.29f,0);
			this.deckBoard.transform.FindChild("1star").localPosition=new Vector3(0.85f,1.29f,0);

			cardsBoardUpMargin = 2.75f;
		}
		else
		{
			this.deckBoard.transform.position=new Vector3(0,2.25f,0);
			this.deckBoard.transform.FindChild("deckList").localPosition=new Vector3(0,1.6f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").localPosition=new Vector3(0.34f,0,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").localPosition=new Vector3(2.6f,0.15f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").localPosition=new Vector3(2.6f,-0.15f,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("newDeckButton").localPosition=new Vector3(-3.169f,0,0);
			this.deckBoard.transform.FindChild("deckList").FindChild("Title").localPosition=new Vector3(0,0.69f,0);
			this.deckBoard.transform.FindChild("3stars").localPosition=new Vector3(-2.55f,-1.35f,0);
			this.deckBoard.transform.FindChild("2stars").localPosition=new Vector3(-0.85f,-1.35f,0);
			this.deckBoard.transform.FindChild("1star").localPosition=new Vector3(0.85f,-1.35f,0);
			cardsBoardUpMargin = 4.5f;
		}
		
		float cardsBoardHeight = worldHeight - cardsBoardUpMargin-cardsBoardDownMargin;
		float cardsBoardWidth = worldWidth-cardsBoardLeftMargin-cardsBoardRightMargin;
		Vector2 cardsBoardOrigin = new Vector3 (-worldWidth/2f+cardsBoardLeftMargin+cardsBoardWidth/2f, -worldHeight / 2f + cardsBoardDownMargin + cardsBoardHeight / 2,0);

		this.cardsBoard.GetComponent<CardsBoardController> ().resize(cardsBoardWidth,cardsBoardHeight,cardsBoardOrigin);
		this.deckCardsPosition=new Vector3[4];

		for(int i=0;i<4;i++)
		{
			this.deckCardsPosition[i]=this.deckBoard.transform.FindChild("Card"+i).position;
			this.deckCards[i].transform.position=this.deckCardsPosition[i];
			this.deckCards[i].transform.localScale=new Vector3(1f,1f,1f);
			this.deckCards[i].SetActive(false);
		}
		
		float cardWidth = 194f;
		float cardHeight = 271f;
		float cardWorldWidth = (cardWidth / pixelPerUnit) * cardScale;
		float cardWorldHeight = (cardHeight / pixelPerUnit) * cardScale;

		this.cardsPerLine = Mathf.FloorToInt ((cardsBoardWidth-0.5f) / cardWorldWidth);
		this.nbLines = Mathf.FloorToInt ((cardsBoardHeight-0.5f) / cardWorldHeight);

		float gapWidth = (cardsBoardWidth - (this.cardsPerLine * cardWorldWidth)) / (this.cardsPerLine + 1);
		float gapHeight = (cardsBoardHeight - (this.nbLines * cardWorldHeight)) / (this.nbLines + 1);
		float cardBoardStartX = cardsBoardOrigin.x - cardsBoardWidth / 2f-cardWorldWidth/2f;
		float cardBoardStartY = cardsBoardOrigin.y + cardsBoardHeight / 2f+cardWorldHeight/2f;

		this.cards=new GameObject[this.cardsPerLine*this.nbLines];
		this.cardsPosition=new Vector3[this.cardsPerLine*this.nbLines];

		for(int j=0;j<this.nbLines;j++)
		{
			for(int i =0;i<this.cardsPerLine;i++)
			{
				this.cards[j*(cardsPerLine)+i] = Instantiate(this.cardObject) as GameObject;
				this.cards[j*(cardsPerLine)+i].transform.localScale= new Vector3(1f,1f,1f);
				this.cardsPosition[j*(this.cardsPerLine)+i]=new Vector3(cardBoardStartX+(i+1)*(gapWidth+cardWorldWidth),cardBoardStartY-(j+1)*(gapHeight+cardWorldHeight),0);
				this.cards[j*(cardsPerLine)+i].transform.position=this.cardsPosition[j*(this.cardsPerLine)+i];
				this.cards[j*(this.cardsPerLine)+i].transform.name="Card"+(j*(this.cardsPerLine)+i);
				this.cards[j*(this.cardsPerLine)+i].SetActive(false);
			}
		}

		this.filters.transform.position = new Vector3 (worldWidth/2f - 1.4f, 0f, 0f);

		if(newDeckViewDisplayed)
		{
			this.newDeckPopUpResize();
		}
		if(editDeckViewDisplayed)
		{
			this.editDeckPopUpResize();
		}
		if(deleteDeckViewDisplayed)
		{
			this.deleteDeckPopUpResize();
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
					this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardController>().setCard(model.cards[this.cardsDisplayed[j*(cardsPerLine)+i]]);
					this.cards[j*(cardsPerLine)+i].transform.GetComponent<NewCardController>().setId(j*(cardsPerLine)+i);
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
			for(int i=0;i<model.decks[this.deckDisplayed].Cards.Count;i++)
			{
				int deckOrder = model.decks[this.deckDisplayed].Cards[i].deckOrder;
				int cardId=model.decks[this.deckDisplayed].Cards[i].Id;
				for(int j=0;j<model.cards.Count;j++)
				{
					if(model.cards[j].Id==cardId)
					{
						this.deckCardsDisplayed[deckOrder]=j;
						break;
					}
				}
			}
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMesh> ().text = model.decks[this.deckDisplayed].Name;
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(true);
			this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").gameObject.SetActive(true);
			this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").gameObject.SetActive(true);
		}
		else
		{
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMesh> ().text="Aucun deck créé";
			this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("selectButton").gameObject.SetActive(false);
			this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").gameObject.SetActive(false);
			this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").gameObject.SetActive(false);
		}
		for(int i=0;i<this.deckCards.Length;i++)
		{
			if(this.deckCardsDisplayed[i]!=-1)
			{
				this.deckCards[i].transform.GetComponent<NewCardController>().setCard(model.cards[this.deckCardsDisplayed[i]]);
				this.deckCards[i].transform.GetComponent<NewCardController>().setId(-i);
				this.deckCards[i].SetActive(true);
			}
			else
			{
				this.deckCards[i].SetActive(false);
			}
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
			this.deckList[this.deckList.Count-1].transform.localPosition=new Vector3(0f, -0.45f+(this.deckList.Count-1)*(-0.32f),0);
			this.deckList[this.deckList.Count-1].transform.FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI3";
			this.deckList[this.deckList.Count-1].transform.FindChild("Title").GetComponent<TextMesh>().text = model.decks [this.decksDisplayed[i]].Name;
			this.deckList[this.deckList.Count-1].GetComponent<DeckBoardDeckListController>().setId(i);
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
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text = this.valueSkill;
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
			distance = cursorPosition.x -(-0.975f+sliderPositionX)-0.01f;
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
			distance = (0.975f+sliderPositionX)-cursorPosition.x+0.01f;
		}
		this.cursors [cursorId].transform.position = cursorPosition;
		float maxDistance = 2 * 0.975f-cursorSizeX;
		float ratio = distance / maxDistance;
		bool isMoved = false ;
		switch (cursorId) 
		{
		case 0:
			minPowerVal=minPowerLimit+Mathf.CeilToInt(ratio*(maxPowerLimit-minPowerLimit));
			if(minPowerVal!=oldMinPowerVal)
			{
				this.filters.transform.FindChild("powerFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = minPowerVal.ToString();
				isMoved=true;
			}
			break;
		case 1:
			maxPowerVal=maxPowerLimit-Mathf.FloorToInt(ratio*(maxPowerLimit-minPowerLimit));
			if(maxPowerVal!=oldMaxPowerVal)
			{
				this.filters.transform.FindChild("powerFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = maxPowerVal.ToString();
				isMoved=true;
			}
			break;
		case 2:
			minLifeVal=minLifeLimit+Mathf.CeilToInt(ratio*(maxLifeLimit-minLifeLimit));
			if(minLifeVal!=oldMinLifeVal)
			{
				this.filters.transform.FindChild("lifeFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = minLifeVal.ToString();
				isMoved=true;
			}
			break;
		case 3:
			maxLifeVal=maxLifeLimit-Mathf.FloorToInt(ratio*(maxLifeLimit-minLifeLimit));
			if(maxLifeVal!=oldMaxLifeVal)
			{
				this.filters.transform.FindChild("lifeFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = maxLifeVal.ToString();
				isMoved=true;
			}
			break;
		case 4:
			minAttackVal=minAttackLimit+Mathf.CeilToInt(ratio*(maxAttackLimit-minAttackLimit));
			if(minAttackVal!=oldMinAttackVal)
			{
				this.filters.transform.FindChild("attackFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = minAttackVal.ToString();
				isMoved=true;
			}
			break;
		case 5:
			maxAttackVal=maxAttackLimit-Mathf.FloorToInt(ratio*(maxAttackLimit-minAttackLimit));
			if(maxAttackVal!=oldMaxAttackVal)
			{
				this.filters.transform.FindChild("attackFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = maxAttackVal.ToString();
				isMoved=true;
			}
			break;
		case 6:
			minQuicknessVal=minQuicknessLimit+Mathf.CeilToInt(ratio*(maxQuicknessLimit-minQuicknessLimit));
			if(minQuicknessVal!=oldMinQuicknessVal)
			{
				this.filters.transform.FindChild("quicknessFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = minQuicknessVal.ToString();
				isMoved=true;
			}
			break;
		case 7:
			maxQuicknessVal=maxQuicknessLimit-Mathf.FloorToInt(ratio*(maxQuicknessLimit-minQuicknessLimit));
			if(maxQuicknessVal!=oldMaxQuicknessVal)
			{
				this.filters.transform.FindChild("quicknessFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = maxQuicknessVal.ToString();
				isMoved=true;
			}
			break;
		}
		if(isMoved)
		{
			this.applyFilters();
		}
	}
	private void computeFilters() 
	{
		this.cardsToBeDisplayed=new List<int>();
		IList<int> tempCardsToBeDisplayed = new List<int>();
		int nbFilters = this.filtersCardType.Count;
		
		bool testFilters = false;
		bool testDeck = false;
		bool test ;

		bool minPowerBool = (this.minPowerLimit==this.minPowerVal);
		bool maxPowerBool = (this.maxPowerLimit==this.maxPowerVal);
		bool minLifeBool = (this.minLifeLimit==this.minLifeVal);
		bool maxLifeBool = (this.maxLifeLimit==this.maxLifeVal);
		bool minQuicknessBool = (this.minQuicknessLimit==this.minQuicknessVal);
		bool maxQuicknessBool = (this.maxQuicknessLimit==this.maxQuicknessVal);
		bool minAttackBool = (this.minAttackLimit==this.minAttackVal);
		bool maxAttackBool = (this.maxAttackLimit==this.maxAttackVal);

		if (this.isSkillChosen)
		{
			int max = model.cards.Count;
			if (nbFilters == 0)
			{
				max = model.cards.Count;
				if (this.isOnSaleFilterOn)
				{
					for (int i = 0; i < max; i++)
					{
						if (model.cards [i].hasSkill(this.valueSkill) && model.cards [i].onSale == 1)
						{
							testDeck = false;
							for (int j = 0; j < this.deckCardsDisplayed.Length; j++)
							{
								if (i == this.deckCardsDisplayed [j])
								{
									testDeck = true;
								}
							}
							if (!testDeck)
							{
								tempCardsToBeDisplayed.Add(i);
							}
						}
					}
				}
				else if (this.isNotOnSaleFilterOn)
				{
					for (int i = 0; i < max; i++)
					{
						if (model.cards [i].hasSkill(this.valueSkill) && model.cards [i].onSale == 0)
						{
							testDeck = false;
							for (int j = 0; j < this.deckCardsDisplayed.Length; j++)
							{
								if (i == this.deckCardsDisplayed [j])
								{
									testDeck = true;
								}
							}
							if (!testDeck)
							{
								tempCardsToBeDisplayed.Add(i);
							}
						}
					}
				}
				else
				{
					for (int i = 0; i < max; i++)
					{
						if (model.cards [i].hasSkill(this.valueSkill))
						{
							testDeck = false;
							for (int j = 0; j < this.deckCardsDisplayed.Length; j++)
							{
								if (i == this.deckCardsDisplayed [j])
								{
									testDeck = true;
								}
							}
							if (!testDeck)
							{
								tempCardsToBeDisplayed.Add(i);
							}
						}
					}
				}
			} else
			{
				for (int i = 0; i < max; i++)
				{
					test = false;
					int j = 0;
					if (this.isNotOnSaleFilterOn)
					{
						while (!test && j != nbFilters)
						{
							if (model.cards [i].IdClass == this.filtersCardType [j])
							{
								test = true;
								if (model.cards [i].hasSkill(this.valueSkill) && model.cards [i].onSale == 1)
								{
									testDeck = false;
									for (int k = 0; k < this.deckCardsDisplayed.Length; k++)
									{
										if (i == this.deckCardsDisplayed [k])
										{
											testDeck = true; 
										}
									}
									if (!testDeck)
									{
										tempCardsToBeDisplayed.Add(i);
									}
								}
							}
							j++;
						}
					} 
					else if (this.isNotOnSaleFilterOn)
					{
						while (!test && j != nbFilters)
						{
							if (model.cards [i].IdClass == this.filtersCardType [j])
							{
								test = true;
								if (model.cards [i].hasSkill(this.valueSkill) && model.cards [i].onSale == 0)
								{
									testDeck = false;
									for (int k = 0; k < this.deckCardsDisplayed.Length; k++)
									{
										if (i == this.deckCardsDisplayed [k])
										{
											testDeck = true; 
										}
									}
									if (!testDeck)
									{
										tempCardsToBeDisplayed.Add(i);
									}
								}
							}
							j++;
						}
					}
					else
					{
						while (!test && j != nbFilters)
						{
							if (model.cards [i].IdClass == this.filtersCardType [j])
							{
								test = true;
								if (model.cards [i].hasSkill(this.valueSkill))
								{
									testDeck = false;
									for (int k = 0; k < this.deckCardsDisplayed.Length; k++)
									{
										if (i == this.deckCardsDisplayed [k])
										{
											testDeck = true; 
										}
									}
									if (!testDeck)
									{
										tempCardsToBeDisplayed.Add(i);
									}
								}
							}
							j++;
						}
					}
				}
			}
		} 
		else
		{
			int max = model.cards.Count;
			if (nbFilters == 0)
			{
				if (this.isOnSaleFilterOn)
				{
					for (int i = 0; i < max; i++)
					{
						if (model.cards [i].onSale == 1)
						{
							testDeck = false;
							for (int j = 0; j < this.deckCardsDisplayed.Length; j++)
							{
								if (i == this.deckCardsDisplayed [j])
								{
									testDeck = true;
								}
							}
							if (!testDeck)
							{
								tempCardsToBeDisplayed.Add(i);
							}
						}
					}
				}
				else if(this.isNotOnSaleFilterOn)
				{
					for (int i = 0; i < max; i++)
					{
						if (model.cards [i].onSale == 0)
						{
							testDeck = false;
							for (int j = 0; j < this.deckCardsDisplayed.Length; j++)
							{
								if (i == this.deckCardsDisplayed [j])
								{
									testDeck = true;
								}
							}
							if (!testDeck)
							{
								tempCardsToBeDisplayed.Add(i);
							}
						}
					}
				}
				else
				{
					for (int i = 0; i < max; i++)
					{
						testDeck = false;
						for (int j = 0; j < this.deckCardsDisplayed.Length; j++)
						{
							if (i == this.deckCardsDisplayed [j])
							{
								testDeck = true;
							}
						}
						if (!testDeck)
						{
							tempCardsToBeDisplayed.Add(i);
						}
					}
				}
			} 
			else
			{
				if (this.isOnSaleFilterOn)
				{
					for (int i = 0; i < max; i++)
					{
						test = false;
						int j = 0;
						while (!test && j != nbFilters)
						{
							if (model.cards [i].IdClass == this.filtersCardType [j])
							{
								if (model.cards [i].onSale == 1)
								{
									test = true;
									testDeck = false;
									for (int k = 0; k < this.deckCardsDisplayed.Length; k++)
									{
										if (i == this.deckCardsDisplayed [k])
										{
											testDeck = true; 
										}
									}
									if (!testDeck)
									{
										tempCardsToBeDisplayed.Add(i);
									}
								}
							}
							j++;
						}
					}
				}
				else if(this.isNotOnSaleFilterOn)
				{
					for (int i = 0; i < max; i++)
					{
						test = false;
						int j = 0;
						while (!test && j != nbFilters)
						{
							if (model.cards [i].IdClass == this.filtersCardType [j])
							{
								if (model.cards [i].onSale == 0)
								{
									test = true;
									testDeck = false;
									for (int k = 0; k < this.deckCardsDisplayed.Length; k++)
									{
										if (i == this.deckCardsDisplayed [k])
										{
											testDeck = true; 
										}
									}
									if (!testDeck)
									{
										tempCardsToBeDisplayed.Add(i);
									}
								}
							}
							j++;
						}
					}
				}
				else
				{
					for (int i = 0; i < max; i++)
					{
						test = false;
						int j = 0;
						while (!test && j != nbFilters)
						{
							if (model.cards [i].IdClass == this.filtersCardType [j])
							{
								test = true;
								testDeck = false;
								for (int k = 0; k < this.deckCardsDisplayed.Length; k++)
								{
									if (i == this.deckCardsDisplayed [k])
									{
										testDeck = true;
									}
								}
								if (!testDeck)
								{
									tempCardsToBeDisplayed.Add(i);
								}
							}
							j++;
						}
					}
				}
			}
		}
		if (tempCardsToBeDisplayed.Count>0){
			this.minPowerLimit=10000;
			this.maxPowerLimit=0;
			this.minLifeLimit=10000;
			this.maxLifeLimit=0;
			this.minAttackLimit=10000;
			this.maxAttackLimit=0;
			this.minQuicknessLimit=10000;
			this.maxQuicknessLimit=0;
			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++){
				if (model.cards[tempCardsToBeDisplayed[i]].Power<this.minPowerLimit){
					this.minPowerLimit = model.cards[tempCardsToBeDisplayed[i]].Power;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Power>this.maxPowerLimit){
					this.maxPowerLimit = model.cards[tempCardsToBeDisplayed[i]].Power;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Life<this.minLifeLimit){
					this.minLifeLimit = model.cards[tempCardsToBeDisplayed[i]].Life;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Life>this.maxLifeLimit){
					this.maxLifeLimit = model.cards[tempCardsToBeDisplayed[i]].Life;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Attack<this.minAttackLimit){
					this.minAttackLimit = model.cards[tempCardsToBeDisplayed[i]].Attack;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Attack>this.maxAttackLimit){
					this.maxAttackLimit = model.cards[tempCardsToBeDisplayed[i]].Attack;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Speed<this.minQuicknessLimit){
					this.minQuicknessLimit = model.cards[tempCardsToBeDisplayed[i]].Speed;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Speed>this.maxQuicknessLimit){
					this.maxQuicknessLimit = model.cards[tempCardsToBeDisplayed[i]].Speed;
				}
			}
			if (minPowerBool && this.maxPowerVal>this.minPowerLimit){
				this.minPowerVal = this.minPowerLimit;
			}
			else{
				if (this.minPowerVal<this.minPowerLimit){
					this.minPowerLimit = this.minPowerVal;
				}
			}
			if (maxPowerBool && this.minPowerVal<this.maxPowerLimit){
				this.maxPowerVal = this.maxPowerLimit;
			}
			else{
				if (this.maxPowerVal>this.maxPowerLimit){
					this.maxPowerLimit = this.maxPowerVal;
				}
			}
			if (minLifeBool && this.maxLifeVal>this.minLifeLimit){
				this.minLifeVal = this.minLifeLimit;
			}
			else{
				if (this.minLifeVal<this.minLifeLimit){
					this.minLifeLimit = this.minLifeVal;
				}
			}
			if (maxLifeBool && this.minLifeVal<this.maxLifeLimit){
				this.maxLifeVal = this.maxLifeLimit;
			}
			else{
				if (this.maxLifeVal>this.maxLifeLimit){
					this.maxLifeLimit = this.maxLifeVal;
				}
			}
			if (minAttackBool && this.maxAttackVal>this.minAttackLimit){
				this.minAttackVal = this.minAttackLimit;
			}
			else{
				if (this.minAttackVal<this.minAttackLimit){
					this.minAttackLimit = this.minAttackVal;
				}
			}
			if (maxAttackBool && this.minAttackVal<this.maxAttackLimit){
				this.maxAttackVal = this.maxAttackLimit;
			}
			else{
				if (this.maxAttackVal>this.maxAttackLimit){
					this.maxAttackLimit = this.maxAttackVal;
				}
			}
			if (minQuicknessBool && this.maxQuicknessVal>this.minQuicknessLimit){
				this.minQuicknessVal = this.minQuicknessLimit;
			}
			else{
				if (this.minQuicknessVal<this.minQuicknessLimit){
					this.minQuicknessLimit = this.minQuicknessVal;
				}
			}
			if (maxQuicknessBool && this.minQuicknessVal<this.maxQuicknessLimit){
				this.maxQuicknessVal = this.maxQuicknessLimit;
			}
			else{
				if (this.maxQuicknessVal>this.maxQuicknessLimit){
					this.maxQuicknessLimit = this.maxQuicknessVal;
				}
			}
			this.oldMinPowerVal = this.minPowerVal ;
			this.oldMaxPowerVal = this.maxPowerVal ;
			this.oldMinLifeVal = this.minLifeVal ;
			this.oldMaxLifeVal = this.maxLifeVal ;
			this.oldMinQuicknessVal = this.minQuicknessVal ;
			this.oldMaxQuicknessVal = this.maxQuicknessVal ;
			this.oldMinAttackVal = this.minAttackVal ;
			this.oldMaxAttackVal = this.maxAttackVal ;
		}
		if(this.minPowerVal!=this.minPowerLimit){
			testFilters=true;
		}
		else if(this.maxPowerVal!=maxPowerLimit){
			testFilters=true;
		}
		else if (this.minLifeVal!=this.minLifeLimit){
			testFilters = true ;
		}
		else if (this.maxLifeVal!=this.maxLifeLimit){
			testFilters = true ;
		}
		else if (this.minAttackVal!=this.minAttackLimit){
			testFilters = true ;
		}
		else if (this.maxAttackVal!=this.maxAttackLimit){
			testFilters = true ;
		}
		else if (this.minQuicknessVal!=this.minQuicknessLimit){
			testFilters = true ;
		}
		else if (this.maxQuicknessVal!=this.maxQuicknessLimit){
			testFilters = true ;
		}
		
		if (testFilters == true){
			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++)
			{
				if (model.cards[tempCardsToBeDisplayed[i]].verifyC(this.minPowerVal,
				                                                   this.maxPowerVal,
				                                                   this.minLifeVal,
				                                                   this.maxLifeVal,
				                                                   this.minAttackVal,
				                                                   this.maxAttackVal,
				                                                   this.minQuicknessVal,
				                                                   this.maxQuicknessVal)){
					this.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
				}
			}
		}
		else
		{
			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++)
			{
				this.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
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
						tempA = model.cards[this.cardsToBeDisplayed[i]].Power;
						tempB = model.cards[this.cardsToBeDisplayed[j]].Power;
						break;
					case 1:
						tempB = model.cards[this.cardsToBeDisplayed[i]].Power;
						tempA = model.cards[this.cardsToBeDisplayed[j]].Power;
						break;
					case 2:
						tempA = model.cards[this.cardsToBeDisplayed[i]].Life;
						tempB = model.cards[this.cardsToBeDisplayed[j]].Life;
						break;
					case 3:
						tempB = model.cards[this.cardsToBeDisplayed[i]].Life;
						tempA = model.cards[this.cardsToBeDisplayed[j]].Life;
						break;
					case 4:
						tempA = model.cards[this.cardsToBeDisplayed[i]].Attack;
						tempB = model.cards[this.cardsToBeDisplayed[j]].Attack;
						break;
					case 5:
						tempB = model.cards[this.cardsToBeDisplayed[i]].Attack;
						tempA = model.cards[this.cardsToBeDisplayed[j]].Attack;
						break;
					case 6:
						tempA = model.cards[this.cardsToBeDisplayed[i]].Speed;
						tempB = model.cards[this.cardsToBeDisplayed[j]].Speed;
						break;
					case 7:
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

		this.filters.transform.FindChild("powerFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = this.minPowerVal.ToString();
		this.filters.transform.FindChild("powerFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = this.maxPowerVal.ToString();
		this.filters.transform.FindChild("lifeFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = this.minLifeVal.ToString();
		this.filters.transform.FindChild("lifeFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = this.maxLifeVal.ToString();
		this.filters.transform.FindChild("attackFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = this.minAttackVal.ToString();
		this.filters.transform.FindChild("attackFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = this.maxAttackVal.ToString();
		this.filters.transform.FindChild("quicknessFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = this.minQuicknessVal.ToString();
		this.filters.transform.FindChild("quicknessFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = this.maxQuicknessVal.ToString();
	}
	private void setSkillAutocompletion()
	{
		this.cleanSkillAutocompletion ();
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text = this.valueSkill;
		if(this.valueSkill.Length>0)
		{
			for (int i = 0; i < model.skillsList.Count; i++) 
			{  
				if (model.skillsList [i].ToLower ().Contains (this.valueSkill)) 
				{
					this.matchValues.Add (Instantiate(this.skillListObject) as GameObject);
					this.matchValues[this.matchValues.Count-1].transform.parent=this.filters.transform.FindChild("skillSearch");
					this.matchValues[this.matchValues.Count-1].transform.localPosition=new Vector3(0, -0.55f+(this.matchValues.Count-1)*(-0.27f),0);
					this.matchValues[this.matchValues.Count-1].transform.FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI3";
					this.matchValues[this.matchValues.Count-1].transform.FindChild("Title").GetComponent<TextMesh>().text = model.skillsList [i];
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
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text =valueSkill;
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
			}
			this.paginationButtons = new GameObject[nbButtonsToDraw];
			for(int i =0;i<nbButtonsToDraw;i++)
			{
				this.paginationButtons[i] = Instantiate(this.paginationButtonObject) as GameObject;
				this.paginationButtons[i].transform.position=new Vector3((0.5f+i-nbButtonsToDraw/2f)*(paginationButtonWidth+gapBetweenPaginationButton),-4.7f,0);
				this.paginationButtons[i].transform.FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
				this.paginationButtons[i].name="Pagination"+i.ToString();
			}
			for(int i=System.Convert.ToInt32(drawBackButton);i<nbButtonsToDraw-System.Convert.ToInt32(drawNextButton);i++)
			{
				this.paginationButtons[i].transform.FindChild("Title").GetComponent<TextMesh>().text=(this.pageDebut+i-System.Convert.ToInt32(drawBackButton)).ToString();
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
				this.paginationButtons[0].transform.FindChild("Title").GetComponent<TextMesh>().text="...";
			}
			if(drawNextButton)
			{
				this.paginationButtons[nbButtonsToDraw-1].GetComponent<MyGamePaginationController>().setId(-1);
				this.paginationButtons[nbButtonsToDraw-1].transform.FindChild("Title").GetComponent<TextMesh>().text="...";
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
			this.newDeckView.newDeckPopUpVM.guiEnabled=false;
			model.decks.Add(new Deck());
			yield return StartCoroutine(model.decks[model.decks.Count-1].create(newDeckView.newDeckPopUpVM.name));
			this.deckDisplayed=model.decks.Count-1;
			this.initializeDecks();
			this.hideNewDeckPopUp();
//			if(this.isTutorialLaunched)
//			{
//				this.tutorial.GetComponent<MyGameTutorialController>().actionIsDone();
//			}
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
				this.editDeckView.editDeckPopUpVM.guiEnabled=false;
				yield return StartCoroutine(model.decks[this.deckDisplayed].edit(editDeckView.editDeckPopUpVM.newName));
				this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMesh> ().text = model.decks[this.deckDisplayed].Name;
				this.hideEditDeckPopUp();
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
		this.deleteDeckView.deleteDeckPopUpVM.guiEnabled=false;
		yield return StartCoroutine(model.decks[this.deckDisplayed].delete());
		this.removeDeckFromAllCards (model.decks[this.deckDisplayed].Id);
		model.decks.RemoveAt (this.deckDisplayed);
		this.hideDeleteDeckPopUp();
		this.retrieveDefaultDeck ();
		this.initializeDecks ();
		this.drawCards ();
	}
	public void removeDeckFromAllCards(int id)
	{
		for(int i=0;i<model.cards.Count;i++)
		{
			for(int j=0;j<model.cards[i].Decks.Count;j++)
			{
				if(model.cards[i].Decks[j]==id)
				{
					model.cards[i].Decks.RemoveAt(j);
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
	public void leftClickedHandler(int id)
	{
		this.idCardClicked = id;
		this.isLeftClicked = true;
		this.clickInterval = 0f;
	}
	public void leftClickReleaseHandler()
	{
		if(isLeftClicked)
		{
			this.isLeftClicked=false;
		}
		else
		{
			this.endDragging();
		}
	}
	public void isDraggingCard()
	{
		if(isDragging)
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			if(this.idCardClicked>-1)
			{
				this.cards[this.idCardClicked].transform.position=new Vector3(mousePosition.x,mousePosition.y,0);
			}
			else
			{
				this.deckCards[-this.idCardClicked].transform.position=new Vector3(mousePosition.x,mousePosition.y,0);
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

		if(this.idCardClicked>-1)
		{
			this.cards[this.idCardClicked].transform.position=this.cardsPosition[this.idCardClicked];
		}
		else
		{
			this.deckCards[-this.idCardClicked].transform.position=this.deckCardsPosition[-this.idCardClicked];
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
}
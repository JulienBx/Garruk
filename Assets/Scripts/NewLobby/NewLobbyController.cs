using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class NewLobbyController : Photon.MonoBehaviour
{
	public static NewLobbyController instance;
	public Sprite friendlyGameButtonTexture;
	public Texture2D[] cursorTextures;
	private NewLobbyModel model;

	public GameObject deckListObject;
	
	private GameObject menu;
	private GameObject deckBoard;
	private GameObject[] deckCards;
	private GameObject focusedCard;
	private GameObject[] buttons;
	private GameObject selectGameTypeTitle;
	private GameObject connectedPlayerTitle;
	private int focusedCardIndex;
	private bool isCardFocusedDisplayed;
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
	
	private bool isSearchingDeck;
	private bool isMouseOnSelectDeckButton;

	private IList<int> decksDisplayed;
	private int[] deckCardsDisplayed;
	private int deckDisplayed;
	
	private Vector3[] deckCardsPosition;
	private Rect[] deckCardsArea;
	
	private bool isSceneLoaded;

	private int money;

	private int idCardClicked;
	private bool isDragging;
	private bool isLeftClicked;
	private bool isHovering;
	private float clickInterval;

	private Texture2D[] buttonsPictures;
	private Rect[] atlasButtonsPicturesRects;
	private Texture2D atlasButtonsPictures;
	private bool arePicturesLoading;

	private bool[] toRotateButtons;
	private bool[] toShowButtonsFront;
	private float[] buttonsAngle;
	
	private float speed;

	public int countPlayers;
	public Dictionary<int, string> playersName;
	private const string roomName="GarrukLobby";
	private bool attemptToPlay;
	
	void Update()
	{	
		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();
			this.initializeDecks();
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
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			this.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			this.escapePressed();
		}
		if(money!=ApplicationModel.credits)
		{
			if(isSceneLoaded)
			{
				if(this.isCardFocusedDisplayed)
				{
					this.focusedCard.GetComponent<NewFocusedCardLobbyController>().updateFocusFeatures();
				}
			}
			this.money=ApplicationModel.credits;
		}
		if(arePicturesLoading)
		{
			bool createAtlas=true;
			if(!model.currentDivision.isTextureLoaded)
			{
				createAtlas=false;
			}
			if(!model.currentCup.isTextureLoaded)
			{
				createAtlas=false;
			}
			if(createAtlas)
			{
				this.arePicturesLoading=false;
				this.atlasButtonsPictures = new Texture2D(8192, 8192);
				this.atlasButtonsPicturesRects = atlasButtonsPictures.PackTextures(buttonsPictures, 2, 8192);
				
				this.buttons[1].GetComponent<LobbyButtonController> ().setDefaultSprite(Sprite.Create(this.atlasButtonsPictures,new Rect(this.atlasButtonsPicturesRects[0].x*atlasButtonsPictures.width,this.atlasButtonsPicturesRects[0].y*atlasButtonsPictures.height,this.atlasButtonsPicturesRects[0].width*atlasButtonsPictures.width,this.atlasButtonsPicturesRects[0].height*atlasButtonsPictures.height),new Vector2(0.5f,0.5f)));
				this.buttons[2].GetComponent<LobbyButtonController> ().setDefaultSprite(Sprite.Create(this.atlasButtonsPictures,new Rect(this.atlasButtonsPicturesRects[1].x*atlasButtonsPictures.width,this.atlasButtonsPicturesRects[1].y*atlasButtonsPictures.height,this.atlasButtonsPicturesRects[1].width*atlasButtonsPictures.width,this.atlasButtonsPicturesRects[1].height*atlasButtonsPictures.height),new Vector2(0.5f,0.5f)));
			}
		}
		for(int i=0;i<3;i++)
		{
			if (this.toRotateButtons[i])
			{
				if(!toShowButtonsFront[i])
				{
					this.buttonsAngle[i] = this.buttonsAngle[i]+ this.speed * Time.deltaTime;
				}
				else
				{
					this.buttonsAngle[i] = this.buttonsAngle[i] - this.speed * Time.deltaTime;
				}
				
				if(this.buttonsAngle[i]>90)
				{
					this.buttons[i].GetComponent<LobbyButtonController>().askForBackSide();
				}
				else
				{
					this.buttons[i].GetComponent<LobbyButtonController>().askForFrontSide();
				}
				if(this.buttonsAngle[i]<0)
				{
					this.buttonsAngle[i]=0;
					this.toRotateButtons[i]=false;
					this.toShowButtonsFront[i]=false;
				}
				if(this.buttonsAngle[i]>=180)
				{
					this.buttonsAngle[i]=180;
					this.toRotateButtons[i]=false;
				}
				Quaternion target = Quaternion.Euler(0, this.buttonsAngle[i], 0);
				this.buttons[i].transform.FindChild("Button").localRotation = target;
			}
		}
	}
	void Awake()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.pixelPerUnit = 108f;
		this.speed = 500.0f;
		this.countPlayers = 0;
		this.attemptToPlay = false;
		this.playersName= new Dictionary<int, string>();
		this.initializeScene ();
	}
	void Start()
	{
		instance = this;
		this.model = new NewLobbyModel ();
		this.resize ();
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		StartCoroutine (this.initialization ());
	}
	private IEnumerator initialization()
	{
		yield return StartCoroutine (model.getLobbyData());
		this.retrieveDefaultDeck ();
		this.initializeDecks ();
		this.initializeButtons ();
		this.updateNbPlayersLabel ();
		this.isSceneLoaded = true;
	}
	private void initializeDecks()
	{
		this.retrieveDecksList ();
		StartCoroutine(this.drawDeckCards ());
	}
	public void initializeScene()
	{
		menu = GameObject.Find ("newMenu");
		menu.GetComponent<newMenuController> ().setCurrentPage (4);
		this.buttonsPictures=new Texture2D[2];
		this.deckBoard = GameObject.Find ("deckBoard");
		this.buttons = new GameObject[3];
		this.buttons[0] = GameObject.Find ("FriendlyGameButton");
		this.buttons [0].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Amical";
		this.buttons[1] = GameObject.Find ("DivisionGameButton");
		this.buttons [1].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "League";
		this.buttons[2] = GameObject.Find ("CupGameButton");
		this.buttons [2].transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Coupe";
		this.selectGameTypeTitle = GameObject.Find ("SelectGameTypeTitle");
		this.selectGameTypeTitle.GetComponent<TextMeshPro> ().text = "Choisir un type de match";
		this.connectedPlayerTitle = GameObject.Find ("ConnectedPlayersTitle");
		this.toRotateButtons=new bool[3];
		this.toShowButtonsFront=new bool[3];
		this.buttonsAngle=new float[3];
		this.deckCards=new GameObject[4];
		this.deckList = new List<GameObject> ();
		for (int i=0;i<4;i++)
		{
			this.deckCards[i]=GameObject.Find("deckCard"+i);
			this.deckCards[i].AddComponent<NewCardLobbyController>();
		}
		this.focusedCard = GameObject.Find ("FocusedCard");
		this.focusedCard.AddComponent<NewFocusedCardLobbyController> ();
		this.deckBoard.transform.FindChild("deckList").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Choisir un deck";
		this.deckBoard.transform.FindChild("deckList").FindChild("currentDeck").FindChild("deckName").GetComponent<TextMeshPro> ().text="Aucun deck créé";
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
	private void initializeButtons()
	{
		bool clickable = false;
		if(this.deckDisplayed!=-1)
		{
			clickable=true;
		}
		for(int i=0;i<this.buttons.Length;i++)
		{
			this.buttons[i].GetComponent<LobbyButtonController>().setClickable(clickable);
			this.buttons[i].GetComponent<LobbyButtonController>().setId(i);
		}
		this.buttons [0].GetComponent<LobbyButtonController> ().setDefaultSprite (this.friendlyGameButtonTexture);
		StartCoroutine(model.currentDivision.setPicture());
		//this.buttonsPictures [0] = model.currentDivision.texture;
		StartCoroutine (model.currentCup.setPicture ());
		this.buttons [1].GetComponent<DivisionGameButtonController> ().setDivision (model.currentDivision);
		//this.buttonsPictures [1] = model.currentCup.texture;
		this.buttons [2].GetComponent<CupGameButtonController> ().setCup (model.currentCup);
		this.arePicturesLoading = true;
	}
	public void resize()
	{
		if(this.isCardFocusedDisplayed)
		{
			this.hideCardFocused();
		}
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.collectionPointsWindow=new Rect(this.widthScreen - this.widthScreen * 0.17f-5,0.1f * this.heightScreen+5,this.widthScreen * 0.17f,this.heightScreen * 0.1f);
		this.newSkillsWindow = new Rect (this.collectionPointsWindow.xMin, this.collectionPointsWindow.yMax + 5,this.collectionPointsWindow.width,this.heightScreen - 0.1f * this.heightScreen - 2 * 5 - this.collectionPointsWindow.height);
		this.newCardTypeWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		this.worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		float screenRatio = (float)this.widthScreen / (float)this.heightScreen;
		float cardHaloWidth = 200f;
		float cardScale = 0.83f;
		float cardWidth = 194f;
		float cardHeight = 271f;
		float cardWorldWidth = (cardWidth / pixelPerUnit) * cardScale;
		float cardWorldHeight = (cardHeight / pixelPerUnit) * cardScale;
		float deckBoardLeftMargin = 2.9f;
		float deckBoardRightMargin = 0.5f;

		float buttonsLeftMargin = 3.4f + ((screenRatio / 1.25f)-1) * 2.5f;
		float buttonsRightMargin = 1f+ ((screenRatio / 1.25f)-1) * 2.5f;
		float buttonsScale = 0.77f;
		float buttonsWidth = 325f;
		float buttonsWorldWidth = (buttonsWidth / pixelPerUnit) * buttonsScale;
		float buttonsTotalWidth = this.worldWidth - buttonsLeftMargin - buttonsRightMargin;
		float gapBetweenButtons = (buttonsTotalWidth - this.buttons.Length * buttonsWorldWidth) / (this.buttons.Length+1);

		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);
		this.deckBoard.transform.position=new Vector3(deckBoardLeftMargin/2f-deckBoardRightMargin/2f,2.5f,0f);
		this.selectGameTypeTitle.transform.position = new Vector3 (buttonsLeftMargin / 2f - buttonsRightMargin / 2f, 0f, 0f);

		for(int i=0;i<this.buttons.Length;i++)
		{
			this.buttons[i].transform.position=new Vector3(buttonsLeftMargin / 2f - buttonsRightMargin / 2f-buttonsTotalWidth/2f+gapBetweenButtons+buttonsWorldWidth/2f+i*(gapBetweenButtons+buttonsWorldWidth),-1.65f,0);
		}
		
		this.deckCardsPosition=new Vector3[4];
		this.deckCardsArea=new Rect[4];

		for(int i=0;i<4;i++)
		{
			this.deckCardsPosition[i]=this.deckBoard.transform.FindChild("Card"+i).position;
			this.deckCardsArea[i]=new Rect(this.deckCardsPosition[i].x-cardWorldWidth/2f,this.deckCardsPosition[i].y-cardWorldHeight/2f,cardWorldWidth,cardWorldHeight);
			this.deckCards[i].transform.position=this.deckBoard.transform.FindChild("Card"+i).position;
			this.deckCards[i].transform.localScale=new Vector3(1f,1f,1f);
			this.deckCards[i].transform.GetComponent<NewCardLobbyController>().setId(i);
			this.deckCards[i].SetActive(false);
		}

		this.connectedPlayerTitle.transform.position = new Vector3 (this.worldWidth / 2f, -this.worldHeight / 2f, 0);
		
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
	}
	public IEnumerator drawDeckCards()
	{
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
		for(int i=0;i<this.deckCardsDisplayed.Length;i++)
		{
			if(this.deckCardsDisplayed[i]!=-1)
			{
				this.deckCards[i].SetActive(value);
			}
		}
		for(int i=0;i<this.buttons.Length;i++)
		{
			this.buttons[i].SetActive(value);
		}
		this.connectedPlayerTitle.SetActive (value);
		this.selectGameTypeTitle.SetActive (value);
	}
	public void selectDeck(int id)
	{
		this.deckDisplayed = this.decksDisplayed [id];
		this.cleanDeckList ();
		this.isSearchingDeck = false;
		this.initializeDecks ();
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
			this.deckList[this.deckList.Count-1].GetComponent<DeckBoardDeckListLobbyController>().setId(i);
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
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().returnPressed();
		}
	}
	public void escapePressed()
	{
		if(isCardFocusedDisplayed)
		{
			this.focusedCard.GetComponent<NewFocusedCardController>().escapePressed();
		}
	}
	public void rightClickedHandler(int id)
	{
		this.idCardClicked = id;
		this.showCardFocused ();
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
			else
			{
				this.isDragging=false;
			}
		}
	}
	public void startDragging()
	{
		if(!this.isDragging)
		{
			this.isDragging=true;
			Cursor.SetCursor (this.cursorTextures[1], new Vector2(this.cursorTextures[1].width/2f,this.cursorTextures[1].width/2f), CursorMode.Auto);
			this.deckCards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(4);
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
		this.deckCards[this.idCardClicked].GetComponent<NewCardController>().changeLayer(-4);
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
	public int getNbGamesDivision()
	{
		return model.player.NbGamesDivision;
	}
	public int getNbGamesCup()
	{
		return model.player.NbGamesCup;
	}
	public void startFriendlyGameButtonHovering(int id)
	{
		this.toRotateButtons [id] = true;
		this.toShowButtonsFront [id] = false;
		Cursor.SetCursor (this.cursorTextures[2], new Vector2(this.cursorTextures[2].width/2f,this.cursorTextures[2].width/2f), CursorMode.Auto);
	}
	public void endFriendlyGameButtonHovering(int id)
	{
		this.toRotateButtons [id] = true;
		this.toShowButtonsFront [id] = true;
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
	}
	public void joinGame(int id)
	{
		ApplicationModel.gameType = id;
		StartCoroutine (this.setSelectedDeck ());
		//if(this.isTutorialLaunched)
		//{
		//	this.endTutorial();
		//}
	}
	private IEnumerator setSelectedDeck()
	{
		yield return StartCoroutine(model.player.SetSelectedDeck(model.decks[this.deckDisplayed].Id));
		attemptToPlay = true;
		PhotonNetwork.Disconnect();
	}
	private void updateNbPlayersLabel()
	{
		if (countPlayers>0)
		{
			if (countPlayers==1)
			{
				this.connectedPlayerTitle.GetComponent<TextMeshPro>().text="Vous etes le seul utilisateur connecté";
			}
			else
			{
				this.connectedPlayerTitle.GetComponent<TextMeshPro>().text=countPlayers+" utilisateurs connectés";
			}
		}
	}
	void RemovePlayerFromList(int id)
	{
		playersName.Remove(id);
		countPlayers--;
		this.updateNbPlayersLabel ();
	}
	void OnJoinedLobby()
	{
		TypedLobby sqlLobby = new TypedLobby("lobby", LobbyType.SqlLobby);    
		string sqlLobbyFilter = "C0 = 0";
		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}
	void OnPhotonRandomJoinFailed()
	{
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 50;
		
		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", 0 } };
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		TypedLobby sqlLobby = new TypedLobby("lobby", LobbyType.SqlLobby);
		
		PhotonNetwork.CreateRoom(roomName, newRoomOptions, sqlLobby);
		//Debug.Log("Creating room");
	}
	
	void OnReceivedRoomListUpdate()
	{
		//roomsList = PhotonNetwork.GetRoomList();
	}
	void OnJoinedRoom()
	{
		//Debug.Log("Connected to Room");
		photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);
	}
	
	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		RemovePlayerFromList(player.ID);
	}
	void OnDisconnectedFromPhoton()
	{
		if (attemptToPlay)
		{
			if(ApplicationModel.gameType==1)
			{
				Application.LoadLevel("DivisionLobby");
			}
			else if(ApplicationModel.gameType==2)
			{
				Application.LoadLevel("CupLobby");
			}
			else
			{
				Application.LoadLevel("Game");
			}
		}
	}
	[RPC]
	void AddPlayerToList(int id, string loginName)
	{
		//print ("I add a player");
		playersName.Add(id, loginName);
		countPlayers++;
		this.updateNbPlayersLabel ();
	}
}
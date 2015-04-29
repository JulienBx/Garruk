using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameView : MonoBehaviour
{
//	public GameObject[] characters;

//	// Profiles des joueurs
//	public Texture2D bottomUserPicture;
//	public Texture2D topUserPicture;
//	public string bottomUserName;
//	public string topUserName;
//	public bool isBottomPlayerLoaded = false ;
//	public bool isTopPlayerLoaded = false ;
//	
//	public GUIStyle[] statsZoneStyles;
//	public GUIStyle[] characterNameStyles;
//	public GUIStyle[] characterLifeStyle;
//	public GUIStyle[] lifeBarStyle;
//	public GUIStyle[] attackStyle;
//	public GUIStyle[] quicknessStyle;
//	public GUIStyle[] moveStyle;
//	
//	public GUIStyle message1Style;
//	public GUIStyle message2Style;
//	public GUIStyle buttonStyle;
//	public GUIStyle skillInfoStyle;
//	public GUIStyle mySkillInfoStyle;
//	public GUIStyle myPlayerNameStyle ;
//	public GUIStyle playerNameStyle ;
//	public GUIStyle enAttenteStyle ;
//	public GUIStyle areaPlayer1Style ;
//	public GUIStyle profilePictureStyle ;
//
//	string labelMessage1;
//	string labelMessage2;
//	
//	public Texture2D attackIcon;
//	public Texture2D quicknessIcon;
//	public Texture2D moveIcon;
//	public GameObject Card;
//	public GameObject Hex;
//
//	private Rect areaPlayer1;
//	public GUIStyle areaPlayer2Style;
//	private Rect areaPlayer2;
//
//	public bool isDragging = false;         // Pendant la phase de positionnement
//	public bool isMoving = false;           // Pendant la phase de combat
//	public bool droppedCard = false;        
//	public bool TimeOfPositionning;         // false : phase de positionnement, true : phase de combat
//	public GameNetworkCard CardSelected;           // Carte sélectionnée dans la phase de positionnement et la phase de combat 
//	public GameCard CardHovered;
//	public static GameView instance = null;
//	public int gridWidthInHexes = 5, gridHeightInHexes = 0;
//	public int nbPlayer = 0;
//	public int MyPlayerNumber {get {return nbPlayer;}}
//	public int nbCardsPlayer1 = 0, nbCardsPlayer2 = 0;
//	public int nbTurn;
//	private User[] users ;
//	Texture2D[] playerPictures ;
//	
//	bool displayedHex = false ;
//	bool isStart = false ;
//	public int GridLayerMask = 1 << 8;
//	
//	private int widthScreen;
//	private int heightScreen;
//	private float scaleTile ;
//	public IList<Tile> board = new List<Tile>();
//	public IList<GameObject> myCards = new List<GameObject>();
//	public IList<GameObject> opponentCards = new List<GameObject>();
	public BottomZoneViewModel bottomZoneVM;
	public TopZoneViewModel topZoneVM ;
	public GameScreenViewModel gameScreenVM ;

	void Awake()
	{
		gameScreenVM = new GameScreenViewModel();
		gameScreenVM.recalculate();
		bottomZoneVM = new BottomZoneViewModel();
		topZoneVM = new TopZoneViewModel();
	}
	
	void Start()
	{	
		//scaleTile = 1.2f * (8f / gridHeightInHexes);
		//users = new User[2];
		//users[0] = new User();
		//users[1] = new User();
		//playerPictures = new Texture2D[2];
		//setSizes();
	}

	void Update() 
	{
//		if (Screen.width != widthScreen || Screen.height != heightScreen) 
//		{
//			if (displayedHex)
//			{
//				this.setSizes();
//				this.displayGrid();
//			}
//		}
	}

	void OnGUI()
	{
		GUILayout.BeginArea(gameScreenVM.bottomZoneRect);
		{
			GUILayout.BeginHorizontal(bottomZoneVM.backgroundStyle,GUILayout.Width(gameScreenVM.widthScreen), GUILayout.Height(gameScreenVM.bottomZoneRect.height));
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Box(bottomZoneVM.userPicture, bottomZoneVM.imageStyle, GUILayout.Width(gameScreenVM.widthScreen*7/100), GUILayout.Height(gameScreenVM.bottomZoneRect.height*80/100));
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.Label(bottomZoneVM.userName, bottomZoneVM.nameTextStyle, GUILayout.Width(gameScreenVM.bottomZoneRect.width*12/100));
				GUILayout.FlexibleSpace();	
				GUILayout.Label(bottomZoneVM.message, bottomZoneVM.messageTextStyle, GUILayout.Width(gameScreenVM.bottomZoneRect.width*58/100));
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical(GUILayout.Width(gameScreenVM.bottomZoneRect.width*18/100));
				{
					if (bottomZoneVM.displayStartButton){
						if (GUILayout.Button("Commencer le match", bottomZoneVM.buttonTextStyle)){
							GameController.instance.StartFight();
						}
					}
					if (GUILayout.Button("Attaquer", bottomZoneVM.buttonTextStyle))
					{
						GameController.instance.setStateOfAttack(true);

					}
					if (GUILayout.Button("Quitter le match", bottomZoneVM.buttonTextStyle))
					{
						PhotonNetwork.Disconnect();
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();

		GUILayout.BeginArea(gameScreenVM.topZoneRect);
		{
			GUILayout.BeginHorizontal(topZoneVM.backgroundStyle,GUILayout.Width(gameScreenVM.widthScreen), GUILayout.Height(gameScreenVM.topZoneRect.height));
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Box(topZoneVM.userPicture, topZoneVM.imageStyle, GUILayout.Width(gameScreenVM.widthScreen*7/100), GUILayout.Height(gameScreenVM.topZoneRect.height*80/100));
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.Label(topZoneVM.userName, topZoneVM.nameTextStyle, GUILayout.Width(gameScreenVM.topZoneRect.width*12/100));
				GUILayout.FlexibleSpace();	
				GUILayout.Label(topZoneVM.message, topZoneVM.messageTextStyle, GUILayout.Width(gameScreenVM.topZoneRect.width*58/100));
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical(GUILayout.Width(gameScreenVM.topZoneRect.width*18/100));
				{
					if (topZoneVM.toDisplayRedStatus){
						GUILayout.Label(topZoneVM.status, topZoneVM.redStatusTextStyle);
					}
					else if (topZoneVM.toDisplayGreenStatus){
						GUILayout.Label(topZoneVM.status, topZoneVM.greenStatusTextStyle);
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
		
		//		if (playersName.Count > 1)
		//		{
		//			GUI.Label(new Rect(10, 0, 500, 50), labelText);
		//			if (!hasClicked && GUI.Button(new Rect(10, 20, 200, 35), "Commencer le combat"))
		//			{
		//				hasClicked = true;
		//				labelText = "En attente d'actions de l'autre joueur";
		//				photonView.RPC("StartFight", PhotonTargets.AllBuffered);
		//			}
		//			if (!GameBoard.instance.TimeOfPositionning)
		//			{
		//				GUI.Label(new Rect(220, 0, 500, 50), "tour " + GameBoard.instance.nbTurn);
		//			}
		//		}
		//		else
		//		{
		//			GUI.Label(new Rect(10, 0, 500, 50), labelInfo);
		//		}
		//		if (GUI.Button(new Rect(220, 20, 150, 35), "Quitter le match"))
		//		{
		//			PhotonNetwork.Disconnect();
		//		}
		
	}
	
	void setSizes()
	{
//		heightScreen = Screen.height;
//		widthScreen = Screen.width;
//		
//		areaPlayer1 = new Rect(0, 0.87f * heightScreen, widthScreen, heightScreen);
//		areaPlayer2 = new Rect(0, 0, widthScreen, 0.13f * heightScreen);
//		
//		playerNameStyle.fixedHeight =       heightScreen * 13 / 100;
//		playerNameStyle.fontSize =          heightScreen * 3 / 100;
//		
//		myPlayerNameStyle.fixedHeight =     heightScreen * 13 / 100;
//		myPlayerNameStyle.fontSize =        heightScreen * 3 / 100;
//		
//		enAttenteStyle.fixedHeight =        heightScreen * 13 / 100;
//		enAttenteStyle.fontSize =           heightScreen * 2 / 100;
//		
//		message1Style.fixedHeight =         heightScreen * 13 / 100;
//		message1Style.fontSize =            heightScreen * 2 / 100;
//		message2Style.fontSize =            heightScreen * 2 / 100;
	}

	public void initGrid()
	{

//		int decalage;
//		int xmax = gridWidthInHexes;
//		int ymax = gridHeightInHexes;
//		
//		for (int x = -1 * xmax / 2 ; x <= 1 * xmax / 2; x++)
//		{
//			if ((xmax - x) % 2 == 0)
//			{
//				decalage = 0;
//			}
//			else
//			{
//				decalage = 1;
//			}
//			for (int y = -1 * ymax / 2; y <= 1 * ymax / 2 - decalage; y++)
//			{
//				int type = Mathf.RoundToInt (UnityEngine.Random.Range (1, 25));
//				if (type > 4)
//				{
//					type = 0;
//				}
//				photonView.RPC("AddTile", PhotonTargets.AllBuffered, x, y, type);
//			}
//		}
	}

	
	void displayGrid()
	{
//		int decalage;
//		Debug.Log ("je redimensionne");
//		CharacterScript gCard; 
//		Vector3 pos;
//		for (int i = 0 ; i < myCards.Count ; i++)
//		{
//			gCard = myCards[i].GetComponentInChildren<CharacterScript>();
//			if (gCard.x % 2 == 0)
//			{
//				decalage = 0;
//			}
//			else
//			{
//				decalage = 1;
//			}
//			if (gCard.y > 0)
//			{
//				if (GameController.instance.isFirstPlayer)
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				}
//				else
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				}
//			}
//			else
//			{
//				if (GameController.instance.isFirstPlayer)
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0));
//				}
//				else
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.4f, 0));
//				}
//			}
//			gCard.setRectStats(pos.x, heightScreen-pos.y, heightScreen *12/100, heightScreen*4/100);
//		}
//		for (int i = 0 ; i < opponentCards.Count ; i++)
//		{
//			gCard = opponentCards[i].GetComponentInChildren<CharacterScript>();
//			if (gCard.x%2==0)
//			{
//				decalage = 0;
//			}
//			else
//			{
//				decalage = 1;
//			}
//			if (gCard.y>0)
//			{
//				if (GameController.instance.isFirstPlayer)
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				}
//				else
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				}
//			}
//			else
//			{
//				if (GameController.instance.isFirstPlayer)
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0));
//				}
//				else
//				{
//					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.4f, 0));
//				}
//			}
//			gCard.setRectStats(pos.x, heightScreen-pos.y, heightScreen *12/100, heightScreen*4/100);
//		}
//		//setRectStats(float x, float y, float sizeX, float sizeY);
//		
	}
	
	public void ArrangeCards(bool player1, Deck deck)
	{
//		int y; 
//		if (player1)
//		{
//			y = -1*gridHeightInHexes/2;
//		}
//		else
//		{
//			y = gridHeightInHexes/2;
//		}
//		for (int x = 0 ; x < 5 ; x++){
//			int viewID = PhotonNetwork.AllocateViewID();
//			photonView.RPC("SpawnCard", PhotonTargets.AllBuffered, x-2, y, deck.Cards[x].ArtIndex, viewID, deck.Cards[x].Id);
//		}
	}
	
//	[RPC]
//	IEnumerator SpawnCard(int x, int y, int type, int viewID, int cardID)
//	{
//		int decalage ;
//		GameObject clone;
//		Vector3 pos ;
//		PlayingCardView playingCardV;
//		BoxCollider bCard ;
//		
//		if (x % 2 == 0)
//		{
//			decalage = 0;
//		}
//		else
//		{
//			decalage = 1;
//		}
//		
//		if ( y > 0)
//		{
//			clone = Instantiate(characters[type], new Vector3(x*scaleTile*0.71f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0), Quaternion.identity) as GameObject;
//		}
//		else
//		{
//			clone = Instantiate(characters[type], new Vector3(x*scaleTile*0.71f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0), Quaternion.identity) as GameObject;
//		}
//		playingCardV = clone.GetComponentInChildren<PlayingCardView>();
//		bCard = clone.GetComponentInChildren<BoxCollider>();
//		PlayingCardController playingCardController = clone.GetComponent<PlayingCardController>();
//		playingCardV.setGnCard(playingCardV);
//		
//		if (y > 0)
//		{
//			clone.transform.localRotation =  Quaternion.Euler(90,180,0);
//			//clone.name = gnCard.card.Title + "-2";
//			if (!GameController.instance.isFirstPlayer){
//				//gCard.setStyles(myCharacterNameStyle, myCharacterLifeStyle, myLifeBarStyle, myAttackStyle, myMoveStyle, myQuicknessStyle, myStatsZoneStyle, attackIcon, quicknessIcon, moveIcon, mySkillInfoStyle);
//				myCards.Add(clone);
//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((x+1)*scaleTile*0.71f-scaleTile*0.27f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				playingCardController.isMine = true;
//				
//			}
//			else
//			{
//				//gCard.setStyles(characterNameStyle, characterLifeStyle, lifeBarStyle, attackStyle, moveStyle, quicknessStyle, statsZoneStyle, attackIcon, quicknessIcon, moveIcon, skillInfoStyle);
//				opponentCards.Add(clone);
//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(x*scaleTile*0.71f-scaleTile*0.42f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
//				playingCardController.isMine = false;
//			}
//		}
//		else
//		{
//			clone.transform.localRotation =  Quaternion.Euler(-90,0,0);
//			//clone.name = gnCard.card.Title + "-1";
//			if (GameController.instance.isFirstPlayer){
//				//gCard.setStyles(myCharacterNameStyle, myCharacterLifeStyle, myLifeBarStyle, myAttackStyle, myMoveStyle, myQuicknessStyle, myStatsZoneStyle, attackIcon, quicknessIcon, moveIcon, mySkillInfoStyle);
//				myCards.Add(clone);
//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(x*scaleTile*0.71f-scaleTile*0.42f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0));
//				playingCardController.isMine = true;
//			}
//			else
//			{
//				//gCard.setStyles(characterNameStyle, characterLifeStyle, lifeBarStyle, attackStyle, moveStyle, quicknessStyle, statsZoneStyle, attackIcon, quicknessIcon, moveIcon, skillInfoStyle);
//				opponentCards.Add(clone);
//				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((x+1)*scaleTile*0.71f-scaleTile*0.27f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.4f, 0));
//				playingCardController.isMine = false;
//			}
//		}
//		
//		yield return StartCoroutine(playingCardController.instance.retrieveCard(cardID));
//		
//		PhotonView nView;
//		nView = clone.GetComponentInChildren<PhotonView>();
//		nView.viewID = viewID;
//		clone.transform.localScale = new Vector3(1, 1, 1);
//		
//		//		if (gCard.photonView.isMine && GameBoard.instance.nbPlayer == 1 || !gCard.photonView.isMine && GameBoard.instance.nbPlayer == 2)
//		//		{
//		//			gnCard.ownerNumber = 1;
//		//			nbCardsPlayer1++;
//		//
//		//		}
//		//		else
//		//		{
//		//			gnCard.ownerNumber = 2;
//		//			nbCardsPlayer2++;
//		//		}
//		
//		
//		// JBU gCard.setRectStats(pos.x, heightScreen-pos.y, heightScreen *12/100, heightScreen*4/100);
//		// JBU gCard.x = x ;
//		// JBU gCard.y = y ;
//		// JBU gCard.showInformations();
//		clone.tag = "PlayableCard";
//		// JBU gnCard.ShowFace();
//		
//		//GameTimeLine.instance.GameCards.Add(gnCard);
//		//GameTimeLine.instance.SortCardsBySpeed();
//		//GameTimeLine.instance.removeBarLife();
//		//GameTimeLine.instance.Arrange();
//	}

//	[RPC]
//	public void addTile(int x, int y, int type)
//	{
//		int decalage ;
//		if ((this.gridWidthInHexes-x)%2==0){
//			decalage = 0;
//		}
//		else{
//			decalage = 1;
//		}
//		GameObject hex = (GameObject)Instantiate(Hex);
//		hex.name = "hex " + (x) + "-" + (y) ;
//		//hex.layer = GridLayerMask;
//		
//		hex.transform.localScale= new Vector3(scaleTile,scaleTile,scaleTile);
//		hex.transform.position = new Vector3(x*scaleTile*0.71f, (y+(decalage/2f))*scaleTile*0.81f, 0);
//		
//		var tb = (GameTile)hex.GetComponent("GameTile");
//		tb.tile = new Tile(x, y, type);
//		tb.ShowFace();
//		this.board.Add(tb.tile);
//		
//		//foreach(Tile tile in GameBoard.instance.board.Values){
//		//tile.FindNeighbours(GameBoard.instance.board, new Vector2(gridHeightInHexes, gridWidthInHexes));
//		//}
//		
//		displayedHex=true ;
//	}
}

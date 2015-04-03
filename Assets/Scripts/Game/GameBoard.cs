using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBoard : Photon.MonoBehaviour 
{
	private string URLGetUserGameProfile = "http://54.77.118.214/GarrukServer/get_user_game_profile.php";
	private string URLDefaultProfilePicture = "http://54.77.118.214/GarrukServer/img/profile/defautprofilepicture.png";

	public GameObject[] characters;

	public GUIStyle[] statsZoneStyles;
	public GUIStyle[] characterNameStyles;
	public GUIStyle[] characterLifeStyle;
	public GUIStyle[] lifeBarStyle;
	public GUIStyle[] attackStyle;
	public GUIStyle[] quicknessStyle;
	public GUIStyle[] moveStyle;

	public GUIStyle message1Style;
	public GUIStyle message2Style;
	public GUIStyle buttonStyle;
	public GUIStyle skillInfoStyle;
	public GUIStyle mySkillInfoStyle;

	string labelMessage1;
	string labelMessage2;

	public Texture2D attackIcon;
	public Texture2D quicknessIcon;
	public Texture2D moveIcon;
	public GameObject Card;
	public GameObject Hex;
	public GUIStyle myPlayerNameStyle ;
	public GUIStyle playerNameStyle ;
	public GUIStyle enAttenteStyle ;
	public GUIStyle areaPlayer1Style ;
	public GUIStyle profilePictureStyle ;
	private Rect areaPlayer1 ;
	public GUIStyle areaPlayer2Style ;
	private Rect areaPlayer2 ;
	public bool isDragging = false;         // Pendant la phase de positionnement
	public bool isMoving = false;           // Pendant la phase de combat
	public bool droppedCard = false;        
	public bool TimeOfPositionning;         // false : phase de positionnement, true : phase de combat
	public GameNetworkCard CardSelected;           // Carte sélectionnée dans la phase de positionnement et la phase de combat 
	public GameCard CardHovered;
	public static GameBoard instance = null;
	public int gridWidthInHexes = 5, gridHeightInHexes = 0;
	public int nbPlayer = 0;
	public int MyPlayerNumber {get {return nbPlayer;}}
	public int nbCardsPlayer1 = 0, nbCardsPlayer2 = 0;
	public static Deck deck;
	public int nbTurn;
	private User[] users ;
	Texture2D[] playerPictures ;
	bool player1Loaded = false ;
	bool player2Loaded = false ;
	bool displayedHex = false ;
	bool isStart = false ;
	public int GridLayerMask = 1 << 8;

	private int widthScreen;
	private int heightScreen;
	private float scaleTile ;

	private int nbPlayerReadyToFight = 0;
	public IList<Tile> board = new List<Tile>();
	public IList<GameObject> mesCartes = new List<GameObject>();
	public IList<GameObject> sesCartes = new List<GameObject>();

	void Awake()
	{
		instance = this;
	}
	
	// Update is called once per frame

	void Start()
	{
		labelMessage2 = "En attente du joueur 2";
		labelMessage1 = "Positionnez vos héros";
		
		scaleTile = 1.2f * (8f/gridHeightInHexes);
		users = new User[2];
		users[0]=new User();
		users[1]=new User();
		playerPictures = new Texture2D[2];
		setSizes();
	}

	void OnGUI()
	{
		GUILayout.BeginArea(areaPlayer1);
		{
			GUILayout.BeginHorizontal();
			{
				if (player1Loaded){
					GUILayout.BeginHorizontal(areaPlayer1Style,GUILayout.Width(widthScreen*20/100), GUILayout.Height(heightScreen*13/100));
					{

							GUILayout.FlexibleSpace();
							GUILayout.BeginVertical();
							{
								GUILayout.FlexibleSpace();
								GUILayout.Box(playerPictures[0],profilePictureStyle, GUILayout.Height(heightScreen*10/100), GUILayout.Width(widthScreen*8/100));
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
							GUILayout.FlexibleSpace();
							GUILayout.Label(users[0].Username, myPlayerNameStyle);
							
							GUILayout.FlexibleSpace();
						
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.Label(labelMessage1, message1Style);

				GUILayout.BeginVertical(GUILayout.Width(20*widthScreen/100), GUILayout.Height(13*heightScreen/100));
				{
					if (!isStart){
						GUILayout.Button("Commencer le match", buttonStyle);
					}
					if (GUILayout.Button("Quitter le match", buttonStyle))
					{
						PhotonNetwork.Disconnect();
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
		
		GUILayout.BeginArea(areaPlayer2);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginHorizontal(areaPlayer2Style,GUILayout.Width(widthScreen*20/100), GUILayout.Height(heightScreen*13/100));
				{
					if (player2Loaded){
						GUILayout.FlexibleSpace();
						GUILayout.BeginVertical();
						{
							GUILayout.FlexibleSpace();
							GUILayout.Box(playerPictures[1],profilePictureStyle, GUILayout.Height(heightScreen*10/100), GUILayout.Width(widthScreen*8/100));
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
						GUILayout.FlexibleSpace();

						GUILayout.Label(users[1].Username, playerNameStyle);
						
						GUILayout.FlexibleSpace();
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.Label(labelMessage2, message2Style);


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
		heightScreen = Screen.height;
		widthScreen = Screen.width;

		areaPlayer1 = new Rect(0,0.87f*heightScreen,this.widthScreen, heightScreen);
		areaPlayer2 = new Rect(0,0,this.widthScreen,0.13f*heightScreen);

		playerNameStyle.fixedHeight=this.heightScreen*13/100;
		playerNameStyle.fontSize=this.heightScreen*3/100;

		myPlayerNameStyle.fixedHeight=this.heightScreen*13/100;
		myPlayerNameStyle.fontSize=this.heightScreen*3/100;

		enAttenteStyle.fixedHeight=this.heightScreen*13/100;
		enAttenteStyle.fontSize=this.heightScreen*2/100;

		message1Style.fixedHeight=this.heightScreen*13/100;
		message1Style.fontSize=this.heightScreen*2/100;
		message2Style.fontSize=this.heightScreen*2/100;
	}

	public void addTile(int x, int y, int type){
		int decalage ;
		if ((this.gridWidthInHexes-x)%2==0){
			decalage = 0;
		}
		else{
			decalage = 1;
		}
		GameObject hex = (GameObject)Instantiate(Hex);
		hex.name = "hex " + (x) + "-" + (y) ;
		//hex.layer = GridLayerMask;
		
		hex.transform.localScale= new Vector3(scaleTile,scaleTile,scaleTile);
		hex.transform.position = new Vector3(x*scaleTile*0.71f, (y+(decalage/2f))*scaleTile*0.81f, 0);
		
		var tb = (GameTile)hex.GetComponent("GameTile");
		tb.tile = new Tile(x, y, type);
		tb.ShowFace();
		this.board.Add(tb.tile);

		//foreach(Tile tile in GameBoard.instance.board.Values){
			//tile.FindNeighbours(GameBoard.instance.board, new Vector2(gridHeightInHexes, gridWidthInHexes));
		//}

		displayedHex=true ;
	}

	void displayGrid()
	{
		int decalage ;
		print ("je redimensionne");
		CharacterScript gCard ; 
		Vector3 pos ;
		for (int i = 0 ; i < mesCartes.Count ; i++){
			gCard = mesCartes[i].GetComponentInChildren<CharacterScript>();
			if (gCard.x%2==0){
				decalage = 0;
			}
			else{
				decalage = 1;
			}
			if (gCard.y>0){
				if (GameScript.instance.isFirstPlayer){
					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
				}
				else{
					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
				}
			}
			else{
				if (GameScript.instance.isFirstPlayer){
					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0));
				}
				else{
					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.4f, 0));
				}
			}
			gCard.setRectStats(pos.x, heightScreen-pos.y, heightScreen *12/100, heightScreen*4/100);
		}
		for (int i = 0 ; i < sesCartes.Count ; i++){
			gCard = sesCartes[i].GetComponentInChildren<CharacterScript>();
			if (gCard.x%2==0){
				decalage = 0;
			}
			else{
				decalage = 1;
			}
			if (gCard.y>0){
				if (GameScript.instance.isFirstPlayer){
					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
				}
				else{
					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
				}
			}
			else{
				if (GameScript.instance.isFirstPlayer){
					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(gCard.x*scaleTile*0.71f-scaleTile*0.42f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0));
				}
				else{
					pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((gCard.x+1)*scaleTile*0.71f-scaleTile*0.27f, (gCard.y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.4f, 0));
				}
			}
			gCard.setRectStats(pos.x, heightScreen-pos.y, heightScreen *12/100, heightScreen*4/100);
		}
		//setRectStats(float x, float y, float sizeX, float sizeY);

	}
	
	// Update is called once per frame
	void Update () {
		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			if (displayedHex){
				this.setSizes();
				this.displayGrid() ;
			}
		}
	}

	void ArrangeCards(bool player1)
	{
		int y ; 
		if (player1){
			y = -1*gridHeightInHexes/2;
		}
		else{
			y = gridHeightInHexes/2;
		}
		for (int x = 0 ; x < 5 ; x++){
			int viewID = PhotonNetwork.AllocateViewID();
			photonView.RPC("SpawnCard", PhotonTargets.AllBuffered, x-2, y, deck.Cards[x].ArtIndex, viewID, deck.Cards[x].Id);
		}
	}

	public IEnumerator AddCardToBoard()
	{
		GameBoard.deck = new Deck(ApplicationModel.username);
		yield return StartCoroutine(GameBoard.deck.LoadSelectedDeck());
		yield return StartCoroutine(deck.RetrieveCards());
	
		ArrangeCards(GameScript.instance.isFirstPlayer);
	}

	public IEnumerator setUser(string name){

		if (users[0].Username!=name || users[1].Username!=name){
			WWWForm form = new WWWForm(); 											// Création de la connexion
			form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
			form.AddField("myform_nick", name);
			
			WWW w = new WWW(URLGetUserGameProfile, form); 				// On envoie le formulaire à l'url sur le serveur 
			yield return w;
			if (w.error != null) 
				print (w.error); 
			else 
			{
				//print(w.text); 											// donne le retour
				if (ApplicationModel.username==name){
					users[0] = new User(name, w.text);
					StartCoroutine (setProfilePicture(0));
					player1Loaded = true ;
				}
				else{
					users[1] = new User(name, w.text);
					StartCoroutine (setProfilePicture(1));
					player2Loaded = true ;
				}
			}
		}
		else{
			print ("Utilisateur déjà ajouté");
		}
	}

	private IEnumerator setProfilePicture(int id){
		
		playerPictures[id] = new Texture2D (4, 4, TextureFormat.DXT1, false);
		
		if (users[id].Picture.StartsWith("http")){
			var www = new WWW(users[id].Picture);
			yield return www;
			www.LoadImageIntoTexture(playerPictures[id]);
		}
		else {
			var www = new WWW(URLDefaultProfilePicture);
			yield return www;
			www.LoadImageIntoTexture(playerPictures[id]);
		}
	}

	public void StartFight()
	{
		nbPlayerReadyToFight++;

//		if (nbPlayerReadyToFight == 2)
//		{  
//			nbTurn = 1;
//			TimeOfPositionning = false;
//			GameTimeLine.instance.PlayingCard.transform.Find("Yellow Outline").renderer.enabled = true;
//			if (GameTimeLine.instance.PlayingCard.ownerNumber == MyPlayerNumber)
//			{
//				GameScript.instance.labelText = "A vous de jouer";
//			}
//			else
//			{
//				GameScript.instance.labelText = "Au joueur adverse de jouer";
//			}
//		}
	}

	[RPC]
	IEnumerator SpawnCard(int x, int y, int type, int viewID, int cardID)
	{
		int decalage ;
		GameObject clone;
		Vector3 pos ;
		CharacterScript gCard ;
		BoxCollider bCard ;

		if (x%2==0){
			decalage = 0;
		}
		else{
			decalage = 1;
		}
		
		if (y>0){
			clone = Instantiate(characters[type], new Vector3(x*scaleTile*0.71f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0), Quaternion.identity) as GameObject;
		}
		else{
			clone = Instantiate(characters[type], new Vector3(x*scaleTile*0.71f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0), Quaternion.identity) as GameObject;
		}
		gCard = clone.GetComponentInChildren<CharacterScript>();
		bCard = clone.GetComponentInChildren<BoxCollider>();
		GameNetworkCard gnCard = clone.GetComponent<GameNetworkCard>();
		gCard.setGnCard(gnCard);
		
		if (y>0){
			clone.transform.localRotation =  Quaternion.Euler(90,180,0);
			//clone.name = gnCard.card.Title + "-2";
			if (!GameScript.instance.isFirstPlayer){
				//gCard.setStyles(myCharacterNameStyle, myCharacterLifeStyle, myLifeBarStyle, myAttackStyle, myMoveStyle, myQuicknessStyle, myStatsZoneStyle, attackIcon, quicknessIcon, moveIcon, mySkillInfoStyle);
				mesCartes.Add(clone);
				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((x+1)*scaleTile*0.71f-scaleTile*0.27f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
				gnCard.isMine = true;

			}
			else{
				//gCard.setStyles(characterNameStyle, characterLifeStyle, lifeBarStyle, attackStyle, moveStyle, quicknessStyle, statsZoneStyle, attackIcon, quicknessIcon, moveIcon, skillInfoStyle);
				sesCartes.Add(clone);
				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(x*scaleTile*0.71f-scaleTile*0.42f, (y+(decalage/2f))*scaleTile*0.81f-scaleTile*0.4f, 0));
				gnCard.isMine = false;
			}
		}
		else{
			clone.transform.localRotation =  Quaternion.Euler(-90,0,0);
			//clone.name = gnCard.card.Title + "-1";
			if (GameScript.instance.isFirstPlayer){
				//gCard.setStyles(myCharacterNameStyle, myCharacterLifeStyle, myLifeBarStyle, myAttackStyle, myMoveStyle, myQuicknessStyle, myStatsZoneStyle, attackIcon, quicknessIcon, moveIcon, mySkillInfoStyle);
				mesCartes.Add(clone);
				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3(x*scaleTile*0.71f-scaleTile*0.42f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.2f, 0));
				gnCard.isMine = true;
			}
			else{
				//gCard.setStyles(characterNameStyle, characterLifeStyle, lifeBarStyle, attackStyle, moveStyle, quicknessStyle, statsZoneStyle, attackIcon, quicknessIcon, moveIcon, skillInfoStyle);
				sesCartes.Add(clone);
				pos = UnityEngine.Camera.main.WorldToScreenPoint(new Vector3((x+1)*scaleTile*0.71f-scaleTile*0.27f, (y-(decalage/2f))*scaleTile*0.81f+scaleTile*0.4f, 0));
				gnCard.isMine = false;
			}
		}

		yield return StartCoroutine(gnCard.RetrieveCard(cardID));

		PhotonView nView;
		nView = clone.GetComponentInChildren<PhotonView>();
		nView.viewID = viewID;
		clone.transform.localScale = new Vector3(1, 1, 1);

//		if (gCard.photonView.isMine && GameBoard.instance.nbPlayer == 1 || !gCard.photonView.isMine && GameBoard.instance.nbPlayer == 2)
//		{
//			gnCard.ownerNumber = 1;
//			nbCardsPlayer1++;
//
//		}
//		else
//		{
//			gnCard.ownerNumber = 2;
//			nbCardsPlayer2++;
//		}
		

		gCard.setRectStats(pos.x, heightScreen-pos.y, heightScreen *12/100, heightScreen*4/100);
		gCard.x = x ;
		gCard.y = y ;
		gCard.showInformations();
		clone.tag = "PlayableCard";
		gnCard.ShowFace();

		//GameTimeLine.instance.GameCards.Add(gnCard);
		//GameTimeLine.instance.SortCardsBySpeed();
		//GameTimeLine.instance.removeBarLife();
		//GameTimeLine.instance.Arrange();
	}
}


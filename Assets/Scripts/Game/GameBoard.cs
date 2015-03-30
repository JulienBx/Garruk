using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBoard : Photon.MonoBehaviour 
{
	private string URLGetUserGameProfile = "http://54.77.118.214/GarrukServer/get_user_game_profile.php";
	private string URLDefaultProfilePicture = "http://54.77.118.214/GarrukServer/img/profile/defautprofilepicture.png";

	public GameObject[] locations;
	public GameObject Card;
	public GameObject Hex;
	public GUIStyle myPlayerNameStyle ;
	public GUIStyle playerNameStyle ;
	public GUIStyle playerName ;
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
	public int gridWidthInHexes, gridHeightInHexes;
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

	private int widthScreen;
	private int heightScreen;
	private float scaleTile ;
	Vector3 offsetTile ;

	private int nbPlayerReadyToFight = 0;
	public Dictionary<Point, Tile> board = new Dictionary<Point, Tile>();

	void Awake()
	{
		instance = this;
	}
	
	void Start()
	{
		scaleTile = 1f * (8f/gridHeightInHexes);
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
				GUILayout.BeginHorizontal(areaPlayer1Style,GUILayout.Width(widthScreen*20/100), GUILayout.Height(heightScreen*13/100));
				{
					if (player1Loaded){
						GUILayout.FlexibleSpace();
						GUILayout.BeginVertical();
						{
							GUILayout.FlexibleSpace();
							GUILayout.Box(playerPictures[0],profilePictureStyle, GUILayout.Height(heightScreen*13/100), GUILayout.Width(widthScreen*8/100));
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
						GUILayout.FlexibleSpace();
						GUILayout.Label(users[0].Username, myPlayerNameStyle);

						GUILayout.FlexibleSpace();
					}
				}
				GUILayout.EndHorizontal();
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
					if (player1Loaded){
						GUILayout.FlexibleSpace();
						GUILayout.BeginVertical();
						{
							GUILayout.FlexibleSpace();
							GUILayout.Box(playerPictures[1],profilePictureStyle, GUILayout.Height(heightScreen*13/100), GUILayout.Width(widthScreen*8/100));
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
						GUILayout.FlexibleSpace();
						if (users[1].Username.Length>2){
							GUILayout.Label(users[1].Username, playerNameStyle);
						}
						else{
							GUILayout.Label("En attente du joueur 2...", enAttenteStyle, GUILayout.Width(widthScreen*12/100));
						}

						GUILayout.FlexibleSpace();
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
		
//		GUI.Label(new Rect(530, 0, 800, 50), labelMessage);
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

		float boardsize = Camera.main.WorldToScreenPoint(new Vector3(offsetTile.x-(scaleTile/2f),0,0)).x;

		areaPlayer1 = new Rect(0,0.87f*heightScreen,this.widthScreen, heightScreen);
		areaPlayer2 = new Rect(0,0,this.widthScreen,0.13f*heightScreen);

		playerNameStyle.fixedHeight=this.heightScreen*13/100;
		playerNameStyle.fontSize=this.heightScreen*3/100;

		myPlayerNameStyle.fixedHeight=this.heightScreen*13/100;
		myPlayerNameStyle.fontSize=this.heightScreen*3/100;

		enAttenteStyle.fixedHeight=this.heightScreen*13/100;
		enAttenteStyle.fontSize=this.heightScreen*2/100;
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
		hex.transform.position = new Vector3(x*scaleTile*0.71f, (y+(decalage/2f))*scaleTile*0.81f, 10);
		
		var tb = (GameTile)hex.GetComponent("GameTile");
		tb.tile = new Tile(x, y, type);
		tb.ShowFace();
		this.board.Add(tb.tile.Location, tb.tile);

		foreach(Tile tile in GameBoard.instance.board.Values){
			//tile.FindNeighbours(GameBoard.instance.board, new Vector2(gridHeightInHexes, gridWidthInHexes));
		}

		displayedHex=true ;
	}

	void displayGrid()
	{
		int decalage ;
		print ("je redimensionne");
		
		for (int x = 0; x < gridWidthInHexes; x++)
		{
			if (x%2==0){
				decalage = 0;
			}
			else{
				decalage = 1;
			}
			for (int y = 0; y < gridHeightInHexes-decalage; y++)
			{
				string name = "hex " + (x) + "-" + (y) ;
				GameObject hex = GameObject.Find(name);
				//hex.layer = GridLayerMask;
				
				hex.transform.localScale= new Vector3(scaleTile,scaleTile,scaleTile);
				hex.transform.position = new Vector3(offsetTile.x+x*scaleTile*0.71f, offsetTile.y+y*scaleTile*0.82f+scaleTile*0.43f*decalage, 10);
			}
		}

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

	void ArrangeCards()
	{
		for (int i = 0 ; i < 5 ; i++){
			int viewID = PhotonNetwork.AllocateViewID();
			photonView.RPC("SpawnCard", PhotonTargets.AllBuffered, 0, 0, viewID, deck.Cards[i].Id);
		}
	}

	public IEnumerator AddCardToBoard()
	{
		GameBoard.deck = new Deck(ApplicationModel.username);
		yield return StartCoroutine(GameBoard.deck.LoadSelectedDeck());
		yield return StartCoroutine(deck.RetrieveCards());
		
		ArrangeCards();
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

		if (nbPlayerReadyToFight == 2)
		{  
			nbTurn = 1;
			TimeOfPositionning = false;
			GameTimeLine.instance.PlayingCard.transform.Find("Yellow Outline").renderer.enabled = true;
			if (GameTimeLine.instance.PlayingCard.ownerNumber == MyPlayerNumber)
			{
				GameScript.instance.labelText = "A vous de jouer";
			}
			else
			{
				GameScript.instance.labelText = "Au joueur adverse de jouer";
			}
		}
	}

	[RPC]
	IEnumerator SpawnCard(int x, int y, int viewID, int cardID)
	{
		GameObject clone;
		clone = Instantiate(Card, new Vector3(x,y,0), Quaternion.identity) as GameObject;
		PhotonView nView;

		nView = clone.GetComponent<PhotonView>();
		nView.viewID = viewID;
		GameCard gCard = clone.GetComponent<GameCard>();
		GameNetworkCard gnCard = clone.GetComponent<GameNetworkCard>();
		clone.transform.localScale = new Vector3(10, 10, 1);
		if (gCard.photonView.isMine && GameBoard.instance.nbPlayer == 1 || !gCard.photonView.isMine && GameBoard.instance.nbPlayer == 2)
		{
			gnCard.ownerNumber = 1;
			nbCardsPlayer1++;
		}
		else
		{
			gnCard.ownerNumber = 2;
			nbCardsPlayer2++;
		}
		if (gCard.photonView.isMine)
		{
			clone.transform.Find("Green Outline").renderer.enabled = true;
		} 
		else
		{
			clone.transform.Find("Red Outline").renderer.enabled = true;
		}
		yield return StartCoroutine(gCard.RetrieveCard(cardID));
		clone.name = gCard.Card.Title + "-" + gnCard.ownerNumber;
		clone.tag = "PlayableCard";
		gnCard.ShowFace();
		GameTimeLine.instance.GameCards.Add(gnCard);
		GameTimeLine.instance.SortCardsBySpeed();
		GameTimeLine.instance.removeBarLife();
		GameTimeLine.instance.Arrange();
	}
}


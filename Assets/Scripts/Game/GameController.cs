using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : Photon.MonoBehaviour 
{	
	//Variables UNITY
	public bool isReconnecting;
	public GameObject hex ;
	public int boardWidth ;
	public int boardHeight ;
	public GameObject[] characters;
	public GUIStyle[] bottomZoneStyles ;
	public GUIStyle[] topZoneStyles ;

	//Variables du controlleur
	public static GameController instance;
	private bool isFirstPlayer = false;
	private bool isGameOver = false;
	private Deck myDeck ;
	private Deck hisDeck ;
	GameObject[,] tiles ;
	List<GameObject> myCharacters ;
	List<GameObject> hisCharacters ;
	const string roomNamePrefix = "GarrukGame";
	private int nbPlayers = 0 ;
	User[] users;
	GameView gameView;

	int characterDragged = -1;
	int mouseX, mouseY ;
	int nbPlayersReadyToFight ;

	//string URLStat = ApplicationModel.dev + "updateResult.php";
	
	void Awake()
	{
		instance = this;
		this.gameView = Camera.main.gameObject.AddComponent <GameView>();
		tiles = new GameObject[boardWidth,boardHeight];
		myCharacters = new List<GameObject>();
		hisCharacters = new List<GameObject>();
		this.nbPlayersReadyToFight = 0 ;
	}
	
	void Start()
	{	
		users = new User[2];
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		PhotonNetwork.autoCleanUpPlayerObjects = false ;
		//scaleTile = 1.2f * (8f/gridHeightInHexes);
	}	

	void Update()
	{	
		if (gameView.gameScreenVM.widthScreen!= Screen.width || gameView.gameScreenVM.widthScreen!= Screen.width)
		{
			this.gameView.gameScreenVM.recalculate();
			int h = this.gameView.gameScreenVM.heightScreen;
			this.gameView.bottomZoneVM.resize(h);
			this.gameView.topZoneVM.resize(h);
		}
	}	
	
	public void setCharacterDragged(int c){
		this.characterDragged = c ;
		this.myCharacters[characterDragged].GetComponentInChildren<PlayingCardController>().tile.GetComponentInChildren<TileController>().setOccupationType(-1);
	}

	public void dropCharacter(){
		this.myCharacters[characterDragged].GetComponentInChildren<PlayingCardController>().tile.GetComponentInChildren<TileController>().setOccupationType(0);
		this.characterDragged = -1 ;
	}

	public void moveCharacter(int x, int y){
		if (this.characterDragged!=-1){
			photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, x, y, this.characterDragged, this.isFirstPlayer);
		}
	}

	[RPC]
	public void moveCharacterRPC(int x, int y, int c, bool isFirstPlayer){
		if (this.isFirstPlayer==isFirstPlayer){
			this.myCharacters[c].GetComponentInChildren<PlayingCardController>().changeTile(tiles[x,y]);
		}
		else{
			this.hisCharacters[c].GetComponentInChildren<PlayingCardController>().changeTile(tiles[x,y]);
		}
	}

	public void EndOfGame(int player)
	{
		isGameOver = true;
	}
	
	private IEnumerator returnToLobby()
	{
//		if (gameView.MyPlayerNumber == 1)
//		{
//			yield return new WaitForSeconds(5);
//		} 
//		else
//		{
//			yield return new WaitForSeconds(7);
//		}
//		PhotonNetwork.Disconnect();
		yield break ;
	}
	
	public void addStat(int user1, int user2)
	{
		//StartCoroutine(sendStat(playersName[user1], playersName[user2]));
	}
	
	IEnumerator sendStat(string user1, string user2)
	{
		//		WWWForm form = new WWWForm(); 								// Création de la connexion
		//		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		//		form.AddField("myform_nick1", user1); 	                    // Pseudo de l'utilisateur victorieux
		//		form.AddField("myform_nick2", user2); 	                    // Pseudo de l'autre utilisateur
		//		
		//		WWW w = new WWW(URLStat, form); 							// On envoie le formulaire à l'url sur le serveur 
		//		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		//		if (w.error != null)
		//		{
		//			print(w.error); 										// donne l'erreur eventuelle
		//		} else
		//		{
		//			print(w.text);
		//		}
		yield break ;
	}

	private void initGrid(){
		
		print ("J'initialise le terrain de jeu");
		int decalage ;
		
		for (int x = 0; x < boardWidth; x++)
		{
			if ((boardWidth-x)%2==0){
				decalage = 1;
			}
			else{
				decalage = 0;
			}
			for (int y = 0; y < boardHeight-decalage; y++)
			{
				int type = Mathf.RoundToInt (UnityEngine.Random.Range (1,25));
				if (type>4){
					type = 0 ;
				}
				photonView.RPC("AddTileToBoard",PhotonTargets.AllBuffered,x,y,type);
			}
		}
	}

	public IEnumerator loadMyDeck()
	{
		this.myDeck = new Deck(ApplicationModel.username);
		yield return StartCoroutine(this.myDeck.LoadDeck());

		photonView.RPC("SpawnCharacter", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, myDeck.Id);
	}
	
	// RPC
	[RPC]
	IEnumerator AddPlayerToList(int id, string loginName)
	{
		print ("J'ajoute "+loginName+" ,ID = "+id);

		users[id-1] = new User(loginName);	
		yield return StartCoroutine(users[id-1].retrievePicture());
		yield return StartCoroutine(users[id-1].setProfilePicture());
		
		if (ApplicationModel.username==loginName){
			this.gameView.bottomZoneVM.setValues(users[id-1], bottomZoneStyles, this.gameView.gameScreenVM.heightScreen);
		}
		else{
			this.gameView.topZoneVM.setValues(users[id-1], topZoneStyles, this.gameView.gameScreenVM.heightScreen);
		}
		nbPlayers++;

		if (this.isReconnecting){
			if (ApplicationModel.username==loginName){
				if (nbPlayers==1){
					this.isFirstPlayer = true ;
				}
				else{
					Camera.main.transform.localRotation=Quaternion.Euler(30,0,180);
					Camera.main.transform.localPosition=new Vector3(0,5.75f,-10f);
				}
			}
		}
		else
		{
			if (this.isFirstPlayer && nbPlayers==1){
				this.initGrid();
				StartCoroutine(this.loadMyDeck());
			}
			else if(!this.isFirstPlayer && nbPlayers==2){
				StartCoroutine(this.loadMyDeck());
			}
		}
	}

	[RPC]
	void AddTileToBoard(int x, int y, int type)
	{
		tiles[x,y] = (GameObject)Instantiate(hex);
		hex.name = "Tile " + (x) + "-" + (y) ;

		tiles[x,y].GetComponent<TileController>().setTile(x, y , this.boardWidth, this.boardHeight, type, 1.2f * (8f/boardHeight));
	}

	[RPC]
	IEnumerator SpawnCharacter(int idPlayer, int idDeck)
	{
		int decalage = 0;
		print ("Je spawne le deck "+idDeck);

		if ((idPlayer==1 && this.isFirstPlayer) || (idPlayer==2 && !this.isFirstPlayer)){
			this.myDeck = new Deck(idDeck);
			yield return StartCoroutine (this.myDeck.RetrieveCards());
			for (int i = 0 ; i < 5 ; i++){
				if (idPlayer==2){
					if (i%2==0){
						decalage = 1;
					}
					else{
						decalage = 0;
					}
				}
				myCharacters.Add((GameObject)Instantiate(this.characters[myDeck.Cards[i].ArtIndex]));
				myCharacters[i].GetComponentInChildren<PlayingCardController>().setCard(myDeck.Cards[i]);
				myCharacters[i].GetComponentInChildren<PlayingCardController>().setID(i);
				myCharacters[i].GetComponentInChildren<PlayingCardController>().setStyles(true);
				myCharacters[i].GetComponentInChildren<PlayingCardController>().setTile(tiles[this.boardWidth/2-2+i,(idPlayer-1)*(this.boardHeight-1)-decalage], (idPlayer==2), this.isFirstPlayer);
				tiles[this.boardWidth/2-2+i,(idPlayer-1)*(this.boardHeight-1)-decalage].GetComponent<TileController>().setOccupationType(0);
				myCharacters[i].GetComponentInChildren<PlayingCardController>().resize(this.gameView.gameScreenVM.heightScreen);
			}

			for (int i = 0 ; i < this.boardWidth ; i++){
				if (i%2==1){
					decalage = 1;
				}
				else{
					decalage = 0;
				}
				for (int j = 0 ; j < 2 - decalage ; j++){
					if (this.isFirstPlayer){
						print ("je set en destination "+i+","+j);
						this.tiles[i,j].GetComponent<TileController>().setDestination();
					}
					else{
						this.tiles[i,this.boardHeight-1-decalage-j].GetComponent<TileController>().setDestination();
					}
				}
			}
		}
		else{
			this.hisDeck = new Deck(idDeck);
			yield return StartCoroutine (this.hisDeck.RetrieveCards());
			for (int i = 0 ; i < 5 ; i++){
				if (idPlayer==2){
					if (i%2==0){
						decalage = 1;
					}
					else{
						decalage = 0;
					}
				}
				hisCharacters.Add((GameObject)Instantiate(this.characters[hisDeck.Cards[i].ArtIndex]));
				hisCharacters[i].GetComponentInChildren<PlayingCardController>().setCard(hisDeck.Cards[i]);
				hisCharacters[i].GetComponentInChildren<PlayingCardController>().setStyles(false);
				hisCharacters[i].GetComponentInChildren<PlayingCardController>().setTile(tiles[this.boardWidth/2-2+i,(idPlayer-1)*(this.boardHeight-1)-decalage], (idPlayer==2), this.isFirstPlayer);
				tiles[this.boardWidth/2-2+i,(idPlayer-1)*(this.boardHeight-1)-decalage].GetComponent<TileController>().setOccupationType(1);
				hisCharacters[i].GetComponentInChildren<PlayingCardController>().resize(this.gameView.gameScreenVM.heightScreen);
			}
		}
		yield break ;
	}

	[RPC]
	public void StartFightRPC()
	{
		this.nbPlayersReadyToFight++;
	}

	public void StartFight()
	{
		photonView.RPC("StartFightRPC", PhotonTargets.AllBuffered, PhotonNetwork.player.ID);
	}
	
	// Photon
	void OnJoinedLobby()
	{
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);    
		string sqlLobbyFilter = "C0 = " + ApplicationModel.gameType;
		PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
	}
	
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room! - creating a new room");
		RoomOptions newRoomOptions = new RoomOptions();
		newRoomOptions.isOpen = true;
		newRoomOptions.isVisible = true;
		newRoomOptions.maxPlayers = 2;
		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", ApplicationModel.gameType } }; // CO pour une partie simple
		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // C0 est récupérable dans le lobby
		
		TypedLobby sqlLobby = new TypedLobby("rankedGame", LobbyType.SqlLobby);
		PhotonNetwork.CreateRoom(roomNamePrefix + Guid.NewGuid().ToString("N"), newRoomOptions, sqlLobby);
		this.isFirstPlayer=true;
	}
	
	void OnJoinedRoom()
	{
		Debug.Log("Connected to a room");
		if (!isReconnecting)
		{
			if (!this.isFirstPlayer){
				Camera.main.transform.localRotation=Quaternion.Euler(30,0,180);
				Camera.main.transform.localPosition=new Vector3(0,5.75f,-10f);
			}
			photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);

		}
		else
		{
			Debug.Log("Reconnecting...");
		}
	}
	
	void OnDisconnectedFromPhoton()
	{
		Application.LoadLevel("LobbyPage");
	}
}

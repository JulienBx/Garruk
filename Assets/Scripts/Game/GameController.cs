using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : Photon.MonoBehaviour
{	
	//Variables UNITY
	public bool isReconnecting;
	public GameObject tile ;
	public int boardWidth ;
	public int boardHeight ;
	public GameObject[] characters;
	public GameObject playingCard;

	public GUIStyle[] gameScreenStyles;

	public Texture2D[] cursors ;
	public GameObject gameEvent;

	//Variables du controlleur
	public static GameController instance;
	public bool isFirstPlayer = false;
	GameObject[,] tiles ;
	GameObject[] playingCards ;
	List<GameObject> gameEvents;

	Tile currentHoveredTile ;
	Tile currentClickedTile ;

	int hoveredPlayingCard = -1;
	int clickedPlayingCard = -1;
	bool isHovering = false ;
	
	const string roomNamePrefix = "GarrukGame";
	private int nbPlayers = 0 ;
	User[] users;
	GameView gameView;

	bool isDragging = false;
	bool isLookingForTarget = false;
	int nbPlayersReadyToFight;

	int currentPlayingCard;
	public int eventMax = 10;
	int nbActionPlayed = 0;
	int nbTurns = 0 ;

	List<int> rankedPlayingCardsID; 


	int myNextPlayer ;
	int hisNextPlayer ;

	bool gameStarted = false;
	bool timeElapsed = false;

	//string URLStat = ApplicationModel.dev + "updateResult.php";
	
	void Awake()
	{
		instance = this;
		this.gameView = Camera.main.gameObject.AddComponent <GameView>();
		tiles = new GameObject[boardWidth, boardHeight];
		playingCards = new GameObject[10];
		gameEvents = new List<GameObject>();

		this.nbPlayersReadyToFight = 0;
		this.currentPlayingCard = -1;
	}
	
	void Start()
	{	
		users = new User[2];
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		PhotonNetwork.autoCleanUpPlayerObjects = false;
		//testTimeline();
		//scaleTile = 1.2f * (8f/gridHeightInHexes);
	}	

	void Update()
	{	
		if (gameView.gameScreenVM.widthScreen != Screen.width || gameView.gameScreenVM.widthScreen != Screen.width)
		{
			this.gameView.gameScreenVM.recalculate();
			int h = this.gameView.gameScreenVM.heightScreen;
			this.gameView.bottomZoneVM.resize(h);
			this.gameView.topZoneVM.resize(h);
			this.recalculateGameEvents();
		}
		if (gameStarted)
		{
			gameView.gameScreenVM.timer -= Time.deltaTime;

			if (timeElapsed)
			{
				timeElapsed = false;
				gameView.gameScreenVM.timer -= 1;
				gameView.gameScreenVM.hasAMessage = true;
				gameView.gameScreenVM.centerMessageRect.width = 100;
				gameView.gameScreenVM.centerMessageRect.x = (Screen.width / 2 - 53);
				gameView.gameScreenVM.messageToDisplay = "temps ecoule";
				currentPlayingCard = 1;
				pass();
			}
			if (gameView.gameScreenVM.timer < 0 && gameView.gameScreenVM.timer > -1)
			{
				timeElapsed = true;
			}
			if (gameView.gameScreenVM.timer < -5)
			{
				gameView.gameScreenVM.hasAMessage = false;
				gameView.gameScreenVM.timer = 10f;
			}
		}
	}	

	public void hideHoveredTile()
	{
		this.tiles [currentHoveredTile.x, currentHoveredTile.y].GetComponent<TileController>().hideHover();
		this.isHovering = false;
	}

	public void hideHoveredPlayingCard()
	{
		this.playingCards [this.hoveredPlayingCard].GetComponent<PlayingCardController>().hideHover();
		this.isHovering = false;
		this.hoveredPlayingCard = -1;
	}

	public void hoverTile(Tile t)
	{
		this.tiles [t.x, t.y].GetComponent<TileController>().displayHover();
		this.currentHoveredTile = t;
	}

	public void hoverPlayingCard(int idPlayingCard)
	{
		this.hoveredPlayingCard = idPlayingCard;
		this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().displayHover();
	}

	public void clickTile(Tile t)
	{

	}

	public void clickPlayingCard(int idPlayingCard)
	{
		this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().displaySelected();
		this.clickedPlayingCard = idPlayingCard;
	}

	public void activatePlayingCard()
	{
		this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().displayPlaying();
	}

	public void moveCharacter()
	{
		this.hideHoveredTile();
		if (this.nbTurns == 0)
		{
			this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().hideSelected();
		}
		this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().setTile(this.currentHoveredTile);
		this.hideHoveredTile();
	}

	public void hideClickedTile()
	{
		this.tiles [currentClickedTile.x, currentClickedTile.y].GetComponent<TileController>().hideSelected();
	}

	public void hideClickedPlayingCard()
	{
		this.playingCards [this.clickedPlayingCard].GetComponent<PlayingCardController>().hideSelected();
		this.clickedPlayingCard = -1;
	}

	public void hideActivatedPlayingCard()
	{
		this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().hidePlaying();
		this.currentPlayingCard = -1;
	}

	public void hoverPlayingCardHandler(int idPlayingCard)
	{

	}

	public void hoverTileHandler(Tile t)
	{
		if (t.x == this.currentClickedTile.x && t.y == this.currentClickedTile.y)
		{

		} else if (t.x == this.currentHoveredTile.x && t.y == this.currentHoveredTile.y)
		{
			
		} else
		{
			if (this.isHovering)
			{
				this.hideHoveredTile();
			}
			this.hoverTile(t);
			if (this.currentPlayingCard != -1 && this.isDragging)
			{
				if (tiles [t.x, t.y].GetComponent<TileController>().isDestination)
				{
					this.gameView.gameScreenVM.setCursor(this.cursors [0], 0);
				} else
				{
					this.gameView.gameScreenVM.setCursor(this.cursors [2], 2);
				}
			} else
			{
				this.gameView.gameScreenVM.SetCursorToDefault();
			}
		}
	}

	public void passHandler()
	{
		this.findNextPlayer();
	}

	public void clickPlayingCardHandler(int idPlayingCard)
	{
		if (this.clickedPlayingCard != idPlayingCard)
		{
			if (this.clickedPlayingCard != -1)
			{
				this.hideClickedPlayingCard();
			}
			this.clickPlayingCard(idPlayingCard);
			this.clickedPlayingCard = idPlayingCard;

			if (nbTurns == 0)
			{
				this.currentPlayingCard = idPlayingCard;
			}
		} else
		{
			this.hideClickedPlayingCard();
		}
	}

	public void releaseClickPlayingCardHandler(int idPlayingCard)
	{
		if (this.currentPlayingCard != -1 && this.isHovering)
		{
			photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, currentClickedTile.x, currentClickedTile.y, idPlayingCard, this.isFirstPlayer, false);
			photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, currentHoveredTile.x, currentHoveredTile.y, this.currentPlayingCard, this.isFirstPlayer, false);
		}
	}

	public void releaseClickTileHandler()
	{
		if (this.currentPlayingCard != -1 && this.isHovering)
		{
			this.hideHoveredTile();
			if (this.gameView.bottomZoneVM.nbTurns == 0)
			{
				this.hideClickedTile();
			}
			if (this.tiles [currentHoveredTile.x, currentHoveredTile.y].GetComponent<TileController>().isDestination)
			{
				photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, currentHoveredTile.x, currentHoveredTile.y, this.currentPlayingCard, this.isFirstPlayer, true);
			}
		}
	}

	public void findNextPlayer()
	{
		bool newTurn = true;
		int nextPlayingCard = -1;
		int i = 0;

		while (i < this.rankedPlayingCardsID.Count && newTurn == true)
		{
			if (!this.playingCards [rankedPlayingCardsID [i]].GetComponentInChildren<PlayingCardController>().hasPlayed)
			{
				nextPlayingCard = rankedPlayingCardsID [i];
				newTurn = false;
			}
			i++;
		}

		if (newTurn)
		{
			this.nbTurns++;
			for (i = 0; i < 10; i++)
			{
				if (!this.playingCards [i].GetComponentInChildren<PlayingCardController>().isDead)
				{
					this.playingCards [i].GetComponentInChildren<PlayingCardController>().hasPlayed = false;
				}
			}
			nextPlayingCard = 0;
		}
		
		photonView.RPC("initPlayer", PhotonTargets.AllBuffered, nextPlayingCard, newTurn, this.isFirstPlayer);
	}



	[RPC]
	public void initPlayer(int id, bool newTurn, bool isFirstP)
	{
		if (this.currentPlayingCard != -1)
		{
			this.hideActivatedPlayingCard();
		}

		if (newTurn)
		{
			this.nbTurns++;
		}
		if (isFirstP != this.isFirstPlayer)
		{
			id = 9 - id;
		}

		this.currentPlayingCard = id;
		
		this.activatePlayingCard();
		this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile.setNeighbours(this.getCharacterTilesArray(), this.playingCards [id].GetComponentInChildren<PlayingCardController>().card.Move);
		this.setDestinations(currentPlayingCard);
		this.isDragging = true;
			
		print("Je passe la main au personnage " + currentPlayingCard);
	}

	public int[,] getCharacterTilesArray()
	{
		int width = GameController.instance.boardWidth;
		int height = GameController.instance.boardHeight;
		int[,] characterTiles = new int[width, height]; 
		for (int i = 0; i < width; i ++)
		{
			for (int j = 0; j < height; j ++)
			{
				characterTiles [i, j] = -1;
			}
		}
		int debut;
		if (this.isFirstPlayer)
		{
			debut = 0;
		} else
		{
			debut = 5;
		}
		characterTiles [this.playingCards [debut].GetComponentInChildren<PlayingCardController>().tile.x, this.playingCards [debut].GetComponentInChildren<PlayingCardController>().tile.y] = 8;
		characterTiles [this.playingCards [debut + 1].GetComponentInChildren<PlayingCardController>().tile.x, this.playingCards [debut + 1].GetComponentInChildren<PlayingCardController>().tile.y] = 8;
		characterTiles [this.playingCards [debut + 2].GetComponentInChildren<PlayingCardController>().tile.x, this.playingCards [debut + 2].GetComponentInChildren<PlayingCardController>().tile.y] = 8;
		characterTiles [this.playingCards [debut + 3].GetComponentInChildren<PlayingCardController>().tile.x, this.playingCards [debut + 3].GetComponentInChildren<PlayingCardController>().tile.y] = 8;
		characterTiles [this.playingCards [debut + 4].GetComponentInChildren<PlayingCardController>().tile.x, this.playingCards [debut + 4].GetComponentInChildren<PlayingCardController>().tile.y] = 8;
		return characterTiles;
	}

	public void setDestinations(int idPlayer)
	{
		List<Tile> nt = this.playingCards [idPlayer].GetComponentInChildren<PlayingCardController>().tile.neighbours.tiles;
		foreach (Tile t in nt)
		{
			this.tiles [t.x, t.y].GetComponentInChildren<TileController>().setDestination();
		}
	}

//	public void setStateOfAttack(bool state)
//	{
//		this.onGoingAttack = state;
//	}

	public void inflictDamage(int targetCharacter)
	{
		photonView.RPC("inflictDamageRPC", PhotonTargets.AllBuffered, targetCharacter, isFirstPlayer);
	}

	public void EndOfGame(bool isFirstPlayerWin)
	{
		gameView.gameScreenVM.hasAMessage = true;
		gameView.gameScreenVM.centerMessageRect.width = 100;
		gameView.gameScreenVM.centerMessageRect.x = (Screen.width / 2 - 30);
		if (isFirstPlayerWin == this.isFirstPlayer)
		{
			gameView.gameScreenVM.messageToDisplay = "gagné";
		} else
		{
			gameView.gameScreenVM.messageToDisplay = "perdu";
		}
	}
	public void pass()
	{
		GameEventType ge = new PassType();
		addGameEvent(this.playingCards [this.currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.Title, ge, "");
		nbActionPlayed = 0;
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
		yield break;
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
		yield break;
	}

	private void sortPlayingCard(int idPlayingCard)
	{
		int speed = this.playingCards [idPlayingCard].GetComponentInChildren<PlayingCardController>().card.Speed;
		this.rankedPlayingCardsID.Remove(idPlayingCard);
		int i = 0;
		bool isInserted = false;

		while (!isInserted && i<this.rankedPlayingCardsID.Count)
		{
			if (speed >= this.playingCards [this.rankedPlayingCardsID [i]].GetComponentInChildren<PlayingCardController>().card.Speed)
			{
				this.rankedPlayingCardsID.Insert(i, idPlayingCard);
				isInserted = true;
			}
			i++;
		}
		if (!isInserted)
		{
			this.rankedPlayingCardsID.Add(idPlayingCard);
		}
	}

	private void initGrid()
	{
		print("J'initialise le terrain de jeu");
		int decalage;
		
		for (int x = 0; x < boardWidth; x++)
		{
			for (int y = 0; y < boardHeight; y++)
			{
				int type = Mathf.RoundToInt(UnityEngine.Random.Range(1, 25));
				if (type > 4)
				{
					type = 0;
				}
				photonView.RPC("AddTileToBoard", PhotonTargets.AllBuffered, x, y, type);
			}
		}
	}

	public IEnumerator loadMyDeck()
	{
		Deck myDeck = new Deck(ApplicationModel.username);
		yield return StartCoroutine(myDeck.LoadDeck());

		photonView.RPC("SpawnCharacter", PhotonTargets.AllBuffered, this.isFirstPlayer, myDeck.Id);
	}
	
	// RPC
	[RPC]
	IEnumerator AddPlayerToList(int id, string loginName)
	{
		print("J'ajoute " + loginName + " ,ID = " + id);

		users [id - 1] = new User(loginName);	
		yield return StartCoroutine(users [id - 1].retrievePicture());
		yield return StartCoroutine(users [id - 1].setProfilePicture());
		gameStarted = true;
		this.gameView.gameScreenVM.setValues(gameScreenStyles);

//		if (ApplicationModel.username == loginName)
//		{
//			this.gameView.bottomZoneVM.setValues(users [id - 1], bottomZoneStyles, this.gameView.gameScreenVM.heightScreen);
//		} else
//		{
//			this.gameView.topZoneVM.setValues(users [id - 1], topZoneStyles, this.gameView.gameScreenVM.heightScreen);
//		}
		nbPlayers++;

		if (this.isReconnecting)
		{
			if (ApplicationModel.username == loginName)
			{
				if (nbPlayers == 1)
				{
					this.isFirstPlayer = true;
				} else
				{
					Camera.main.transform.localRotation = Quaternion.Euler(30, 0, 180);
					Camera.main.transform.localPosition = new Vector3(0, 5.75f, -10f);
				}
			}
		} else
		{
			if (this.isFirstPlayer && nbPlayers == 1)
			{
				this.initGrid();
				StartCoroutine(this.loadMyDeck());
			} else if (!this.isFirstPlayer && nbPlayers == 2)
			{
				StartCoroutine(this.loadMyDeck());
			}
		}
	}

	[RPC]
	void AddTileToBoard(int x, int y, int type)
	{
		tiles [x, y] = (GameObject)Instantiate(this.tile);
		tiles [x, y].name = "Tile " + (x) + "-" + (y);

		tiles [x, y].GetComponent<TileController>().setTile(x, y, this.boardWidth, this.boardHeight, type, 1.1f * (8f / boardHeight));
	}

	[RPC]
	IEnumerator SpawnCharacter(bool isFirstP, int idDeck)
	{
		Deck deck;
		deck = new Deck(idDeck);
		yield return StartCoroutine(deck.RetrieveCards());
		int debut;

		if (isFirstP)
		{

			debut = 0;
		} else
		{
			debut = 5;
		}

		for (int i = 0; i < 5; i++)
		{
			this.playingCards [debut + i] = (GameObject)Instantiate(this.playingCard);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setCard(deck.Cards [i]);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setIDCharacter(debut + i);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setStyles((isFirstP == this.isFirstPlayer));
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setTile(new Tile(i, 0), !isFirstP);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().resize(this.gameView.gameScreenVM.heightScreen);
		}
		
		yield break;
	}

	[RPC]
	public void StartFightRPC(bool isFirst)
	{
		this.nbPlayersReadyToFight++;

		if (this.nbPlayersReadyToFight == 2)
		{
			for (int i = 0; i < this.boardWidth; i++)
			{
				for (int j = 0; j < this.boardHeight; j++)
				{
					this.tiles [i, j].GetComponent<TileController>().setStandard();
				}
			}
			this.nbTurns = 1;

			if (this.isFirstPlayer)
			{
				this.sortAllCards();
				this.findNextPlayer();
			}
		} 
	}

	private void sortAllCards()
	{

	}

	[RPC]
	public void moveCharacterRPC(int x, int y, int c, bool isFirstPlayer, bool isEmpty)
	{
		this.playingCards [c].GetComponentInChildren<PlayingCardController>().changeTile(new Tile(x, y), isEmpty);
	}

	[RPC]
	public void inflictDamageRPC(int targetCharacter, bool isFisrtPlayerCharacter)
	{
//		PlayingCardController temp = GameController.instance.getPlayingCharacter(this.isFirstPlayer == isFisrtPlayerCharacter);
//		int damage = temp.card.GetAttack();
//		this.myPlayingCards [targetCharacter].GetComponentInChildren<PlayingCardController>().damage += damage;
//		List<GameObject> tempList;
//		if (isFisrtPlayerCharacter == isFirstPlayer)
//		{
//			tempList = myPlayingCards;
//		} else
//		{
//			tempList = hisPlayingCards;
//		}
//		PlayingCardController pcc = tempList [targetCharacter].GetComponent<PlayingCardController>();
//		int tempCounter = 0;
//		if (pcc.damage >= pcc.card.GetLife())
//		{
//			pcc.isDead = true;
//
//			for (int i = 0; i < 5; i++)
//			{
//				if (tempList [i].GetComponentInChildren<PlayingCardController>().isDead)
//				{
//					tempCounter++;
//				}
//			}
//		}
//		if (tempCounter > 4)
//		{
//			EndOfGame(isFirstPlayer);
//		}
	}
	
	public void StartFight()
	{
		photonView.RPC("StartFightRPC", PhotonTargets.AllBuffered, this.isFirstPlayer);
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
		this.isFirstPlayer = true;
	}
	
	void OnJoinedRoom()
	{
		Debug.Log("Connected to a room");
		if (!isReconnecting)
		{
			if (!this.isFirstPlayer)
			{
				Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 180);
			}
			photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);

		} else
		{
			Debug.Log("Reconnecting...");
		}
	}
	
	void OnDisconnectedFromPhoton()
	{
		Application.LoadLevel("Lobby");
	}

	public void testTimeline()
	{
		this.currentPlayingCard = 1;
		addMovementEvent(this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.Title, tiles [4, 3], tiles [4, 4]);
		string targetName = "coincoin";
		List<GameSkill> temp = this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().skills;
		if (temp.Count > 0)
		{
			//addGameEvent("", new SkillType(temp [0].Skill.Action), targetName);
		}
		pass();
		this.currentPlayingCard = 0;
		pass();

		inflictDamage(0);
		inflictDamage(1);
		inflictDamage(2);
		inflictDamage(3);
		inflictDamage(4);
	}
	
	public void addGameEvent(string name, GameEventType type, string targetName)
	{
		setGameEvent(name, type);
		if (targetName != "")
		{
			gameEvents [0].GetComponent<GameEventController>().addAction(" sur " + targetName);
		}
	}

	public void addMovementEvent(string name, GameObject origin, GameObject destination)
	{
		GameObject go = setGameEvent(name, new MovementType());

		go.GetComponent<GameEventController>().setMovement(origin, destination);
	}

	GameObject setGameEvent(string name, GameEventType type)
	{
		GameObject go;
		if (nbActionPlayed == 0)
		{
			if (gameEvents.Count < eventMax)
			{
				go = (GameObject)Instantiate(gameEvent);
				gameEvents.Add(go);
				go.GetComponent<GameEventController>().setScreenPosition(gameEvents.Count);
			} 
			changeGameEvents();
			go = gameEvents [0];
			go.GetComponent<GameEventController>().setCharacterName(name);
			go.GetComponent<GameEventController>().setAction(type.toString());
			Texture2D t2 = this.playingCards [currentPlayingCard].GetComponent<PlayingCardController>().getPicture();
			Texture2D temp = getImageResized(t2);

			go.GetComponent<GameEventController>().setArt(temp);
			go.GetComponent<GameEventController>().gameEventView.show();
			nbActionPlayed++;
		} else if (nbActionPlayed < 2)
		{
			go = gameEvents [0];
			go.GetComponent<GameEventController>().addAction(type.toString());
			nbActionPlayed++;
		} else
		{
			go = gameEvents [0];
		}

		return go;
	}
			           
	Texture2D getImageResized(Texture2D t)
	{
		int size;
		Color[] pix;
		if (t.width > t.height)
		{
			size = t.height;
			pix = t.GetPixels((t.width - size) / 2, 0, size, size);
		} else
		{
			size = t.width;
			pix = t.GetPixels(0, (t.height - size) / 2, size, size);
		}
		Texture2D temp = new Texture2D(size, size);
		temp.SetPixels(pix);
		temp.Apply();

		return temp;
	}

	void changeGameEvents()
	{
		for (int i = gameEvents.Count - 1; i > 0; i--)
		{
			string title = gameEvents [i - 1].GetComponent<GameEventController>().getCharacterName();
			string action = gameEvents [i - 1].GetComponent<GameEventController>().getAction();
			GameObject[] movement = gameEvents [i - 1].GetComponent<GameEventController>().getMovement();
			Texture2D t2 = gameEvents [i - 1].GetComponent<GameEventController>().getArt();

			gameEvents [i].GetComponent<GameEventController>().setCharacterName(title);
			gameEvents [i].GetComponent<GameEventController>().setAction(action);
			gameEvents [i].GetComponent<GameEventController>().setMovement(movement [0], movement [1]);
			gameEvents [i].GetComponent<GameEventController>().setArt(t2);
			gameEvents [i].GetComponent<GameEventController>().gameEventView.show();
			gameEvents [i - 1].GetComponent<GameEventController>().setMovement(null, null);

		}
	}

	void recalculateGameEvents()
	{
		int i = 1;

		foreach (GameObject go in gameEvents)
		{
			go.GetComponent<GameEventController>().setScreenPosition(i++);
		}
	}
}


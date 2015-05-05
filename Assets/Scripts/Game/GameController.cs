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
	public GameObject playingCard;
	public GUIStyle[] bottomZoneStyles ;
	public GUIStyle[] topZoneStyles ;
	public Texture2D[] cursors ;
	public GameObject gameEvent;

	//Variables du controlleur
	public static GameController instance;
	private bool isFirstPlayer = false;
	private bool isGameOver = false;
	private Deck myDeck ;
	private Deck hisDeck ;
	GameObject[,] tiles ;
	List<GameObject> myCharacters ;
	List<GameObject> hisCharacters ;
	List<GameObject> myPlayingCards ;
	List<GameObject> hisPlayingCards ;
	List<GameObject> gameEvents;

	int currentHoveredTileX = -1 ;
	int currentHoveredTileY = -1 ;

	int currentClickedTileX = -1 ;
	int currentClickedTileY = -1 ;

	int hoveredCharacter = -1;
	
	const string roomNamePrefix = "GarrukGame";
	private int nbPlayers = 0 ;
	User[] users;
	GameView gameView;

	int characterDragged = -1;
	int playingCharacter = 2;
	public bool onGoingAttack = false;
	int mouseX, mouseY ;
	int nbPlayersReadyToFight ;

	private List<int> hasPlayed ;
	int currentPlayer;

	int speed ;
	int eventMax = 10;
	//string URLStat = ApplicationModel.dev + "updateResult.php";
	
	void Awake()
	{
		instance = this;
		this.gameView = Camera.main.gameObject.AddComponent <GameView>();
		tiles = new GameObject[boardWidth, boardHeight];
		myCharacters = new List<GameObject>();
		hisCharacters = new List<GameObject>();
		myPlayingCards = new List<GameObject>();
		hisPlayingCards = new List<GameObject>();
		gameEvents = new List<GameObject>();
		this.nbPlayersReadyToFight = 0;
		this.currentPlayer = -1;
		this.speed = 100;
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
		}
	}	

	public void hideHoveredTile()
	{
		if (currentHoveredTileX != -1)
		{
			this.tiles [currentHoveredTileX, currentHoveredTileY].GetComponent<TileController>().hideHover();
			if (this.hoveredCharacter >= 0 && this.hoveredCharacter < 5)
			{
				this.myPlayingCards [this.hoveredCharacter].GetComponentInChildren<PlayingCardController>().hideHover();
			} else
			{
				
			}
			this.currentHoveredTileX = -1;
			this.currentHoveredTileY = -1;
			this.hoveredCharacter = -1;
		}
	}

	public void hoverTile(int idCharacter, int x, int y)
	{
		this.hoveredCharacter = idCharacter;
		this.tiles [x, y].GetComponent<TileController>().displayHover();
		if (idCharacter >= 0 && idCharacter < 5)
		{
			this.myPlayingCards [idCharacter].GetComponentInChildren<PlayingCardController>().displayHover();
		} else
		{
			
		}
		this.currentHoveredTileX = x;
		this.currentHoveredTileY = y;
	}

	public void clickTile(int idCharacter, int x, int y)
	{
		this.hoveredCharacter = -1;
		this.currentHoveredTileX = -1;
		this.currentHoveredTileY = -1;
		this.currentClickedTileX = x;
		this.currentClickedTileY = y;
		this.tiles [x, y].GetComponent<TileController>().displaySelected();
		if (idCharacter < 5)
		{
			this.myPlayingCards [idCharacter].GetComponentInChildren<PlayingCardController>().displayClick();
			this.myPlayingCards [idCharacter].GetComponentInChildren<PlayingCardController>().isSelected = true;
			this.myPlayingCards [idCharacter].GetComponentInChildren<PlayingCardController>().resizeInfoRect();
			int r = this.myPlayingCards [idCharacter].GetComponentInChildren<PlayingCardController>().sortID;
			
			for (int i = 0; i < 5; i++)
			{
				if (i != idCharacter)
				{
					if (r < this.myPlayingCards [i].GetComponentInChildren<PlayingCardController>().sortID)
					{
						this.myPlayingCards [i].GetComponentInChildren<PlayingCardController>().isMoved = true;
						this.myPlayingCards [i].GetComponentInChildren<PlayingCardController>().resizeInfoRect();
					}
				}
			}
		} else
		{
			
		}
	}

	public void hideClickedTile()
	{
		this.tiles [currentClickedTileX, currentClickedTileY].GetComponent<TileController>().hideSelected();
		if (this.characterDragged >= 0 && this.characterDragged < 5)
		{
			this.myPlayingCards [this.characterDragged].GetComponentInChildren<PlayingCardController>().hideHover();
			this.myPlayingCards [this.characterDragged].GetComponentInChildren<PlayingCardController>().isSelected = false;
			this.myPlayingCards [this.characterDragged].GetComponentInChildren<PlayingCardController>().resizeInfoRect();
			for (int i = 0; i < 5; i++)
			{
				this.myPlayingCards [i].GetComponentInChildren<PlayingCardController>().isMoved = false;
				this.myPlayingCards [i].GetComponentInChildren<PlayingCardController>().resizeInfoRect();
			}
		} else
		{
			
		}
		this.currentClickedTileX = -1;
		this.currentClickedTileY = -1;
		this.characterDragged = -1;
	}

	public void hoverTileHandler(int x, int y, int idCharacter, bool isDestination)
	{
		if (currentClickedTileX != x || currentClickedTileY != y)
		{
			if (currentHoveredTileX != x || currentHoveredTileY != y)
			{
				this.hideHoveredTile();
				this.hoverTile(idCharacter, x, y);

				if (this.characterDragged != -1)
				{
					if (isDestination)
					{
						if (idCharacter == -1)
						{
							this.gameView.gameScreenVM.cursor = this.cursors [0];
						} else
						{
							this.gameView.gameScreenVM.cursor = this.cursors [1];
						}
					} else
					{
						this.gameView.gameScreenVM.cursor = this.cursors [2];
					}
					this.gameView.changeCursor();
				}
			}
		} else
		{
			this.hideHoveredTile();
		}
	}

	public void hoverPlayingCard(int idCharacter)
	{
		if (idCharacter < 5)
		{
			this.hoverTileHandler(myCharacters [idCharacter].GetComponentInChildren<PlayingCharacterController>().tile.GetComponent<TileController>().x, myCharacters [idCharacter].GetComponentInChildren<PlayingCharacterController>().tile.GetComponent<TileController>().y, idCharacter, myCharacters [idCharacter].GetComponentInChildren<PlayingCharacterController>().tile.GetComponent<TileController>().isDestination);
		} else
		{
			//this.hoverTile(myCharacters[idCharacter].GetComponentInChildren<PlayingCharacterController>().tile.GetComponent<TileController>().x, myCharacters[idCharacter].GetComponentInChildren<PlayingCharacterController>().tile.GetComponent<TileController>().y, idCharacter, myCharacters[idCharacter].GetComponentInChildren<PlayingCharacterController>().tile.GetComponent<TileController>().isDestination);
		}
	}

	public void clickPlayingCard(int idCharacter)
	{
		if (idCharacter < 5)
		{
			this.clickTileHandler(myCharacters [idCharacter].GetComponentInChildren<PlayingCharacterController>().tile.GetComponent<TileController>().x, myCharacters [idCharacter].GetComponentInChildren<PlayingCharacterController>().tile.GetComponent<TileController>().y, idCharacter);
			this.releaseClick();
		}
	}

	public void clickTileHandler(int x, int y, int idCharacter)
	{
		if (idCharacter != -1)
		{
			if (currentClickedTileX != x || currentClickedTileY != y)
			{
				if (this.characterDragged == -1)
				{
					if (idCharacter < 5)
					{
						if (this.myCharacters [idCharacter].GetComponentInChildren<PlayingCharacterController>().isMovable)
						{
							this.characterDragged = idCharacter;
						}
					} else
					{
					
					}
					if (currentClickedTileX != -1)
					{
						this.hideClickedTile();
					}
					this.clickTile(idCharacter, x, y);
				}
			} else
			{
				if (this.characterDragged == -1)
				{
					this.gameView.SetCursorToDefault();
				}
				this.hideClickedTile();
			}
		}
	}

	public void releaseClick()
	{
		if (this.characterDragged != -1 && currentHoveredTileX != -1)
		{
			if (this.tiles [currentHoveredTileX, currentHoveredTileY].GetComponent<TileController>().isDestination)
			{
				if (this.tiles [currentHoveredTileX, currentHoveredTileY].GetComponent<TileController>().characterID != -1)
				{
					photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, currentClickedTileX, currentClickedTileY, this.tiles [currentHoveredTileX, currentHoveredTileY].GetComponent<TileController>().characterID, this.isFirstPlayer, false);
					photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, currentHoveredTileX, currentHoveredTileY, this.characterDragged, this.isFirstPlayer, false);
				} else
				{
					photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, currentHoveredTileX, currentHoveredTileY, this.characterDragged, this.isFirstPlayer, true);
				}
				this.hideHoveredTile();
				this.hideClickedTile();
				this.gameView.SetCursorToDefault();
			}
		}
	}

	public void initTurns()
	{
		this.gameView.bottomZoneVM.nbTurns = 1;
		this.hasPlayed = new List<int>();
		this.findNextPlayer();
	}

	public void findNextPlayer()
	{
		bool newTurn = false;
		int quickness = -10000;
		int tempQuickness;
		int nextPlayer = -1;

		if (this.hasPlayed.Count == 10)
		{
			newTurn = true; 
			hasPlayed = new List<int>();
			for (int i = 0; i < 10; i++)
			{
				if (myCharacters [i].GetComponentInChildren<PlayingCardController>().isDead)
				{
					this.hasPlayed.Add(i);
				}
			}
		}

		for (int i = 0; i < 10; i++)
		{
			if (!hasPlayed.Contains(i))
			{
				if (i < 5)
				{
					tempQuickness = myCharacters [i].GetComponentInChildren<PlayingCardController>().card.Speed;
				} else
				{
					tempQuickness = hisCharacters [i - 5].GetComponentInChildren<PlayingCardController>().card.Speed;
				}
				if (tempQuickness >= quickness)
				{
					quickness = tempQuickness;
					nextPlayer = i;
				}
			}
		}
		this.currentPlayer = nextPlayer;
		this.initNextPlayer(newTurn);
	}

	public void initNextPlayer(bool newTurn)
	{
		photonView.RPC("initPlayer", PhotonTargets.AllBuffered, this.currentPlayer, newTurn);
	}

	[RPC]
	public void initPlayer(int id, bool newTurn)
	{
		print("au personnage " + id + " de jouer... " + newTurn);
		if (newTurn)
		{
			this.gameView.bottomZoneVM.nbTurns++;
		}
		if (this.isFirstPlayer == (id < 5))
		{
			this.gameView.bottomZoneVM.message = "A votre tour de jouer";
			if (id < 5)
			{
				this.gameView.bottomZoneVM.setCharacter(this.myCharacters [id].GetComponentInChildren<PlayingCardController>().card);
			} else
			{
				this.gameView.bottomZoneVM.setCharacter(this.myCharacters [id - 5].GetComponentInChildren<PlayingCardController>().card);
			}

		} else
		{
			this.gameView.bottomZoneVM.message = "Au tour du joueur adverse";
		}
	}
	
	public void setCharacterDragged(int c)
	{
		this.characterDragged = c;
		this.myCharacters [characterDragged].GetComponentInChildren<PlayingCharacterController>().tile.GetComponentInChildren<TileController>().setCharacterID(characterDragged);
	}

	public void dropCharacter()
	{
		this.myCharacters [characterDragged].GetComponentInChildren<PlayingCharacterController>().tile.GetComponentInChildren<TileController>().setCharacterID(-1);
		this.characterDragged = -1;
	}

	public void setStateOfAttack(bool state)
	{
		this.onGoingAttack = state;
	}

	public PlayingCardController getPlayingCharacter(bool myCharacter=true)
	{
		if (myCharacter)
		{
			return myCharacters [playingCharacter].GetComponentInChildren<PlayingCardController>();
		} else
		{
			return hisCharacters [playingCharacter].GetComponentInChildren<PlayingCardController>();
		}
	}

	public void inflictDamage(int targetCharacter)
	{
		photonView.RPC("inflictDamageRPC", PhotonTargets.AllBuffered, targetCharacter, isFirstPlayer);
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

	private void sortMyCards()
	{
		int[] ranks = new int[5];
		float[] quicknesses = new float[5];
		for (int i = 0; i < 5; i++)
		{
			quicknesses [i] = myPlayingCards [i].GetComponentInChildren<PlayingCardController>().card.Speed;
		}

		for (int i = 0; i < 5; i++)
		{
			ranks [i] = 1;
			for (int j = 0; j < 5; j++)
			{
				if (i != j)
				{
					if (quicknesses [i] <= quicknesses [j])
					{
						ranks [i]++;
						quicknesses [j] += 0.1f;
					}
				}
			}
			myPlayingCards [i].GetComponentInChildren<PlayingCardController>().setSortID(ranks [i], 100 - (this.speed - (int)quicknesses [i]));
			myPlayingCards [i].GetComponentInChildren<PlayingCardController>().resize(this.gameView.gameScreenVM.heightScreen);
		}
	}

	private void initGrid()
	{
		
		print("J'initialise le terrain de jeu");
		int decalage;
		
		for (int x = 0; x < boardWidth; x++)
		{
			if ((boardWidth - x) % 2 == 0)
			{
				decalage = 1;
			} else
			{
				decalage = 0;
			}
			for (int y = 0; y < boardHeight-decalage; y++)
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
		this.myDeck = new Deck(ApplicationModel.username);
		yield return StartCoroutine(this.myDeck.LoadDeck());

		photonView.RPC("SpawnCharacter", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, myDeck.Id);
	}
	
	// RPC
	[RPC]
	IEnumerator AddPlayerToList(int id, string loginName)
	{
		print("J'ajoute " + loginName + " ,ID = " + id);

		users [id - 1] = new User(loginName);	
		yield return StartCoroutine(users [id - 1].retrievePicture());
		yield return StartCoroutine(users [id - 1].setProfilePicture());
		
		if (ApplicationModel.username == loginName)
		{
			this.gameView.bottomZoneVM.setValues(users [id - 1], bottomZoneStyles, this.gameView.gameScreenVM.heightScreen);
		} else
		{
			this.gameView.topZoneVM.setValues(users [id - 1], topZoneStyles, this.gameView.gameScreenVM.heightScreen);
		}
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
		tiles [x, y] = (GameObject)Instantiate(hex);
		tiles [x, y].name = "Tile " + (x) + "-" + (y);

		tiles [x, y].GetComponent<TileController>().setTile(x, y, this.boardWidth, this.boardHeight, type, 1.2f * (8f / boardHeight));
	}

	[RPC]
	IEnumerator SpawnCharacter(int idPlayer, int idDeck)
	{
		int decalage = 0;
		print("Je spawne le deck " + idDeck);

		if ((idPlayer == 1 && this.isFirstPlayer) || (idPlayer == 2 && !this.isFirstPlayer))
		{
			this.myDeck = new Deck(idDeck);
			yield return StartCoroutine(this.myDeck.RetrieveCards());
			for (int i = 0; i < 5; i++)
			{
				if (idPlayer == 2)
				{
					if (i % 2 == 0)
					{
						decalage = 1;
					} else
					{
						decalage = 0;
					}
				}
				myPlayingCards.Add((GameObject)Instantiate(this.playingCard));
				myPlayingCards [i].GetComponentInChildren<PlayingCardController>().setCard(myDeck.Cards [i]);
				myPlayingCards [i].GetComponentInChildren<PlayingCardController>().setSkills();
				myPlayingCards [i].GetComponentInChildren<PlayingCardController>().setIDCharacter(i);
				myPlayingCards [i].GetComponentInChildren<PlayingCardController>().setStyles(true);

				print(myDeck.Cards [i].ArtIndex);
				myCharacters.Add((GameObject)Instantiate(this.characters [myDeck.Cards [i].ArtIndex]));
				myCharacters [i].GetComponentInChildren<PlayingCharacterController>().setID(i);
				myCharacters [i].GetComponentInChildren<PlayingCharacterController>().setName(myDeck.Cards [i].Title);
				myCharacters [i].GetComponentInChildren<PlayingCharacterController>().setStyles(true);
				myCharacters [i].GetComponentInChildren<PlayingCharacterController>().setTile(tiles [this.boardWidth / 2 - 2 + i, (idPlayer - 1) * (this.boardHeight - 1) - decalage], (idPlayer == 2), this.isFirstPlayer);
				tiles [this.boardWidth / 2 - 2 + i, (idPlayer - 1) * (this.boardHeight - 1) - decalage].GetComponent<TileController>().setCharacterID(i);
				myCharacters [i].GetComponentInChildren<PlayingCharacterController>().resize(this.gameView.gameScreenVM.heightScreen);
			}

			for (int i = 0; i < this.boardWidth; i++)
			{
				if (i % 2 == 1)
				{
					decalage = 1;
				} else
				{
					decalage = 0;
				}
				for (int j = 0; j < 2 - decalage; j++)
				{
					if (this.isFirstPlayer)
					{
						this.tiles [i, j].GetComponent<TileController>().setDestination();
					} else
					{
						this.tiles [i, this.boardHeight - 1 - decalage - j].GetComponent<TileController>().setDestination();
					}
				}
			}
			this.sortMyCards();
		} else
		{
			this.hisDeck = new Deck(idDeck);
			yield return StartCoroutine(this.hisDeck.RetrieveCards());
			for (int i = 0; i < 5; i++)
			{
				if (idPlayer == 2)
				{
					if (i % 2 == 0)
					{
						decalage = 1;
					} else
					{
						decalage = 0;
					}
				}
				hisPlayingCards.Add((GameObject)Instantiate(this.playingCard));
				hisPlayingCards [i].GetComponentInChildren<PlayingCardController>().setCard(hisDeck.Cards [i]);
				hisPlayingCards [i].GetComponentInChildren<PlayingCardController>().setSkills();
				hisPlayingCards [i].GetComponentInChildren<PlayingCardController>().setIDCharacter(i + 5);
				hisPlayingCards [i].GetComponentInChildren<PlayingCardController>().setStyles(false);
				hisPlayingCards [i].GetComponentInChildren<PlayingCardController>().resize(this.gameView.gameScreenVM.heightScreen);

				hisCharacters.Add((GameObject)Instantiate(this.characters [hisDeck.Cards [i].ArtIndex]));
				hisCharacters [i].GetComponentInChildren<PlayingCharacterController>().setName(myDeck.Cards [i].Title);
				hisCharacters [i].GetComponentInChildren<PlayingCharacterController>().setStyles(false);
				hisCharacters [i].GetComponentInChildren<PlayingCharacterController>().setTile(tiles [this.boardWidth / 2 - 2 + i, (idPlayer - 1) * (this.boardHeight - 1) - decalage], (idPlayer == 2), this.isFirstPlayer);
				hisCharacters [i].GetComponentInChildren<PlayingCharacterController>().setID(i + 5);

				tiles [this.boardWidth / 2 - 2 + i, (idPlayer - 1) * (this.boardHeight - 1) - decalage].GetComponent<TileController>().setCharacterID(i + 5);
				hisCharacters [i].GetComponentInChildren<PlayingCharacterController>().resize(this.gameView.gameScreenVM.heightScreen);
			}
		}
		yield break;
	}

	[RPC]
	public void StartFightRPC(bool isFirst)
	{
		this.nbPlayersReadyToFight++;

		if (this.nbPlayersReadyToFight == 2)
		{
			if (this.isFirstPlayer)
			{
				this.initTurns();
			}
		} else
		{
			if (isFirst == this.isFirstPlayer)
			{
				this.gameView.bottomZoneVM.message = "En attente du joueur adverse pour démarrer la partie";
				this.gameView.bottomZoneVM.displayStartButton = false;
			} else
			{
				this.gameView.topZoneVM.message = "A terminé de positionner ses héros";
				this.gameView.topZoneVM.status = "Pret a jouer";
				this.gameView.topZoneVM.toDisplayGreenStatus = true;
				this.gameView.topZoneVM.toDisplayRedStatus = false;
			}
		}

		if (isFirst == this.isFirstPlayer)
		{
			int decalage;
			for (int i = 0; i < this.boardWidth; i++)
			{
				if (i % 2 == 1)
				{
					decalage = 1;
				} else
				{
					decalage = 0;
				}
				for (int j = 0; j < 2 - decalage; j++)
				{
					if (this.isFirstPlayer)
					{
						print("je set en destination " + i + "," + j);
						this.tiles [i, j].GetComponent<TileController>().setStandard();
					} else
					{
						this.tiles [i, this.boardHeight - 1 - decalage - j].GetComponent<TileController>().setStandard();
					}
				}
			}
		}
	}

	[RPC]
	public void moveCharacterRPC(int x, int y, int c, bool isFirstPlayer, bool isEmpty)
	{
		if (this.isFirstPlayer == isFirstPlayer)
		{
			myCharacters [c].GetComponentInChildren<PlayingCharacterController>().changeTile(tiles [x, y], isEmpty);
		} else
		{
			hisCharacters [c].GetComponentInChildren<PlayingCharacterController>().changeTile(tiles [x, y], isEmpty);
		}
	}
	[RPC]
	public void inflictDamageRPC(int targetCharacter, bool isFirstPlayer)
	{
		//int damage = GameController.instance.getPlayingCharacter(this.isFirstPlayer == isFirstPlayer).card.GetAttack();
		//this.myCharacters[targetCharacter].GetComponentInChildren<PlayingCardController>().damage += damage;
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
				Camera.main.transform.localRotation = Quaternion.Euler(30, 0, 180);
				Camera.main.transform.localPosition = new Vector3(0, 5.75f, -10f);
			}
			photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);

		} else
		{
			Debug.Log("Reconnecting...");
		}
	}
	
	void OnDisconnectedFromPhoton()
	{
		Application.LoadLevel("LobbyPage");
	}

	public void testTimeline()
	{
		Card card = new Card("test");
		GameEventType ge = new PassType();
		addGameEvent(card, ge);
		addGameEvent(card, ge);
		addGameEvent(card, ge);
	}
	
	public void addGameEvent(Card card, GameEventType type)
	{
		GameObject go;
		if (gameEvents.Count < eventMax)
		{
			go = (GameObject)Instantiate(gameEvent);
			gameEvents.Add(go);
			go.GetComponent<GameEventController>().setScreenPosition(gameEvents.Count);
		} else
		{
			changeGameEvents();
			go = gameEvents [0];
		}
		go.GetComponent<GameEventController>().setCharacterName(card.Title);
		go.GetComponent<GameEventController>().setAction(type.toString());
		//go.GetComponent<GameEventController>().setArt(card.);
	}

	void changeGameEvents()
	{
		for (int i = eventMax - 1; i > 0; i--)
		{
			string title = gameEvents [i - 1].GetComponent<GameEventController>().getCharacterName();
			string action = gameEvents [i - 1].GetComponent<GameEventController>().getAction();
			gameEvents [i].GetComponent<GameEventController>().setCharacterName(title);
			gameEvents [i].GetComponent<GameEventController>().setAction(action);
		}
	}
}


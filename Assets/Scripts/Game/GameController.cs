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
	public GameObject verticalBorder;
	public GameObject horizontalBorder;
	public GameObject backGO;
	public Texture2D[] backgroundGO ;
	public int nbFreeStartRows ;
	public GUIStyle[] gameScreenStyles;

	public Texture2D[] cursors ;
	public GameObject gameEvent;
	public GameObject skillObject;

	//Variables du controlleur
	public static GameController instance;
	public bool isFirstPlayer = false;
	public Deck myDeck;
	GameObject[,] tiles ;
	GameObject[] playingCards ;
	GameObject[] verticalBorders ;
	GameObject[] horizontalBorders ;
	GameObject background ;
	List<GameObject> gameEvents;

	GameObject selectedPlayingCard ;
	GameObject[] skillsScripts ;

	GameObject selectedOpponentCard ;
	GameObject[] opponentSkillsScripts ;

	Tile currentHoveredTile ;
	Tile currentClickedTile ;
	Tile currentOpponentClickedTile ;
	Tile currentPlayingTile ;
	int hoveredPlayingCard = -1;
	int clickedPlayingCard = -1;
	int clickedOpponentPlayingCard = -1;
	bool isHovering = false ;

	public float borderSize ;

	int widthScreen ; 
	int heightScreen ;
	float tileScale ; 
	int backgroundType = -1 ;
	
	const string roomNamePrefix = "GarrukGame";
	private int nbPlayers = 0 ;
	public User[] users;
	GameView gameView;

	bool isDragging = false;
	bool isLookingForTarget = false;
	int nbPlayersReadyToFight;

	int currentPlayingCard = -1;
	public int eventMax;
	int nbActionPlayed = 0;
	int nbTurns = 0 ;

	int[] rankedPlayingCardsID; 

	int myNextPlayer ;
	int hisNextPlayer ;

	bool gameStarted = false;
	bool timeElapsed = false;
	bool timeElapsedPopUp = true;
	bool popUpDisplay = false;

	float[] popupPosition = {0.95f, 0.05f};

	string URLStat = ApplicationModel.host + "updateResult.php";
	
	void Awake()
	{
		instance = this;
		this.gameView = Camera.main.gameObject.AddComponent <GameView>();
		this.gameView.gameScreenVM.setStyles(gameScreenStyles);

		tiles = new GameObject[boardWidth, boardHeight];
		playingCards = new GameObject[10];
		gameEvents = new List<GameObject>();

		this.nbPlayersReadyToFight = 0;
		this.currentPlayingCard = -1;
		this.eventMax = 11;
		this.verticalBorders = new GameObject[this.boardWidth + 1];
		this.horizontalBorders = new GameObject[this.boardHeight + 1];
		this.createBackground();
		this.resize();
		this.initGameEvents();
	}
	
	void Start()
	{	
		users = new User[2];
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		PhotonNetwork.autoCleanUpPlayerObjects = false;
		//scaleTile = 1.2f * (8f/gridHeightInHexes);
	}	

	void Update()
	{	
		if (this.widthScreen != Screen.width || this.heightScreen != Screen.height)
		{
			this.resize();
		}
		if (gameStarted)
		{
			gameView.gameScreenVM.timer -= Time.deltaTime;
			if (popUpDisplay)
			{
				gameView.gameScreenVM.timerPopUp -= Time.deltaTime;
			}

			if (gameView.gameScreenVM.timerPopUp < 0)
			{
				popUpDisplay = false;
				gameView.gameScreenVM.hasAMessage = false;
			}

			if (timeElapsed)
			{
				timeElapsed = false;
				gameView.gameScreenVM.timer -= 1;
				displayPopUpMessage("Temps ecoulé", 5f);
				//currentPlayingCard = 1; // provisoire
			}
			if (gameView.gameScreenVM.timer < 0 && gameView.gameScreenVM.timer > -1)
			{
				timeElapsed = true;
				pass();
			}
			if (gameView.gameScreenVM.timer < -5)
			{
				gameView.gameScreenVM.timer = 10f;
			}
		}
	}

	public void resize()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		if (this.widthScreen * 10f / 6f > this.heightScreen)
		{
			this.tileScale = 1f;
		} else
		{
			this.tileScale = 1f * (1.0f * widthScreen / heightScreen) * 10f / 6f;
		}

		this.gameView.gameScreenVM.recalculate(widthScreen, heightScreen);
		this.resizeBackground();
		int h = this.gameView.gameScreenVM.heightScreen;
		this.recalculateGameEvents();
	}

	public void displayPopUpMessage(string message, float time)
	{
		gameView.gameScreenVM.hasAMessage = true;
		gameView.gameScreenVM.messageToDisplay = message;
		popUpDisplay = true;
		gameView.gameScreenVM.timerPopUp = time;
	}

	public void displayPopUpMessageMyPlayer(string message, float time)
	{
		gameView.gameScreenVM.hasAMessage = true;
		gameView.gameScreenVM.messageToDisplay = message;
		popUpDisplay = true;
		gameView.gameScreenVM.timerPopUp = time;
	}

	public void displayPopUpMessageOpponent(string message, float time)
	{
		gameView.gameScreenVM.hasAMessage = true;
		gameView.gameScreenVM.messageToDisplay = message;
		popUpDisplay = true;
		gameView.gameScreenVM.timerPopUp = time;
	}

	public void createBackground()
	{
		if (this.widthScreen > this.heightScreen)
		{
			this.background = (GameObject)Instantiate(this.backGO);
		} else
		{
			this.background = (GameObject)Instantiate(this.backGO);
		}

		for (int i = 0; i < this.verticalBorders.Length; i++)
		{
			this.verticalBorders [i] = (GameObject)Instantiate(this.verticalBorder);
		}
		for (int i = 0; i < this.horizontalBorders.Length; i++)
		{
			this.horizontalBorders [i] = (GameObject)Instantiate(this.horizontalBorder);
		}

		this.selectedPlayingCard = (GameObject)Instantiate(this.playingCard);
		this.selectedPlayingCard.GetComponent<PlayingCardController>().setControlActive(false);
		this.selectedOpponentCard = (GameObject)Instantiate(this.playingCard);
		this.selectedOpponentCard.GetComponent<PlayingCardController>().setControlActive(false);

		this.skillsScripts = new GameObject[6];
		this.opponentSkillsScripts = new GameObject[6];
		for (int i = 0; i < 6 ; i++)
		{
			this.skillsScripts[i] = (GameObject)Instantiate(this.skillObject);
			this.opponentSkillsScripts[i] = (GameObject)Instantiate(this.skillObject);
		}
	}

	public void resizeBackground()
	{
		if (this.widthScreen > this.heightScreen && this.backgroundType != 1)
		{
			this.background.renderer.materials [0].mainTexture = this.backgroundGO [1];
			this.backgroundType = 1;
		} else if (this.widthScreen <= this.heightScreen && this.backgroundType != 0)
		{
			this.background.renderer.materials [0].mainTexture = this.backgroundGO [0];
			this.backgroundType = 0;
		}
		
		if (this.widthScreen > this.heightScreen)
		{
			this.background.transform.localScale = new Vector3(20f, 10f, 0.5f);
		} else
		{
			if (this.tileScale == 1f)
			{
				this.background.transform.localScale = new Vector3(10f * tileScale * (1.0f * widthScreen / heightScreen), 10 * tileScale, 0.5f);
			} else
			{
				this.background.transform.localScale = new Vector3(6 * tileScale, 12 * tileScale, 0.5f);
			}
		}
		
		Vector3 position;
		Vector3 scale;
		for (int i = 0; i < this.horizontalBorders.Length; i++)
		{
			position = new Vector3(0, (5f - 4f * this.tileScale) + tileScale * i - 5f, -1);
			this.horizontalBorders [i].transform.localPosition = position;
			this.horizontalBorders [i].transform.localScale = new Vector3(this.tileScale * 6f, this.borderSize, this.borderSize);
		}

		for (int i = 0; i < this.verticalBorders.Length; i++)
		{
			position = new Vector3((-3 * this.tileScale) + i * this.tileScale, 0f, -1f);
			this.verticalBorders [i].transform.localPosition = position;
			this.verticalBorders [i].transform.localScale = new Vector3(this.verticalBorders [i].transform.localScale.x, 8f * tileScale, this.verticalBorders [i].transform.localScale.z);
		}

		position = new Vector3((-2.6f * this.tileScale), -4.5f * this.tileScale, -1f);
		scale = new Vector3(0.8f, 0.8f, 0.8f);
		this.selectedPlayingCard.GetComponent<PlayingCardController>().setPosition(position, scale);
		this.selectedPlayingCard.GetComponent<PlayingCardController>().setActive(false);

		position = new Vector3((-2.6f * this.tileScale), 4.5f * this.tileScale, -1f);
		scale = new Vector3(0.8f, 0.8f, 0.8f);
		this.selectedOpponentCard.GetComponent<PlayingCardController>().setPosition(position, scale);
		this.selectedOpponentCard.GetComponent<PlayingCardController>().setActive(false);

		for (int i = 0; i < this.skillsScripts.Length; i++)
		{
			position = new Vector3((-1.5f * this.tileScale) + i * this.tileScale*0.8f, -4.5f * this.tileScale, -1f);
			scale = new Vector3(0.8f, 0.8f, 0.8f);
			this.skillsScripts [i].GetComponent<SkillObjectController>().setPosition(position, scale);
			this.skillsScripts [i].GetComponent<SkillObjectController>().show();
			this.skillsScripts [i].GetComponent<SkillObjectController>().setActive(false);
		}

		for (int i = 0; i < this.skillsScripts.Length; i++)
		{
			position = new Vector3((-1.5f * this.tileScale) + i * this.tileScale*0.8f, 4.5f * this.tileScale, -1f);
			scale = new Vector3(0.8f, 0.8f, 0.8f);
			this.opponentSkillsScripts [i].GetComponent<SkillObjectController>().setPosition(position, scale);
			this.opponentSkillsScripts [i].GetComponent<SkillObjectController>().show();
			this.opponentSkillsScripts [i].GetComponent<SkillObjectController>().setActive(false);
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
		this.isHovering = true;
		this.hoveredPlayingCard = -1;
	}

	public void hoverPlayingCard(int idPlayingCard)
	{
		this.hoveredPlayingCard = idPlayingCard;
		this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().displayHover();
		this.currentHoveredTile = this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile;
		this.isHovering = true;
	}

	public void activatePlayingCard(int idPlayingCard)
	{
		if (this.hoveredPlayingCard!=-1){
			this.hideHoveredPlayingCard();
		}
		this.currentPlayingCard = idPlayingCard;

		if (this.isFirstPlayer==(idPlayingCard<5)){
			this.isDragging=true;
			this.clickedPlayingCard = idPlayingCard;
		}
		else{
			this.clickedOpponentPlayingCard = idPlayingCard;
		}
		
		if (this.isFirstPlayer==(idPlayingCard<5)){
			this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().displayPlaying();
			this.currentPlayingTile = this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().tile;
			this.currentClickedTile = this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().tile;
			
			this.selectedPlayingCard.GetComponent<PlayingCardController>().setCard(this.playingCards[this.clickedPlayingCard].GetComponent<PlayingCardController>().card);
			this.selectedPlayingCard.GetComponent<PlayingCardController>().show();
			this.selectedPlayingCard.GetComponent<PlayingCardController>().setActive(true);


			List<Skill> skills = this.playingCards[this.clickedPlayingCard].GetComponent<PlayingCardController>().card.Skills;
			for (int i = 0 ; i < 4 ; i++){
				if (i < skills.Count){
					this.skillsScripts[i].GetComponent<SkillObjectController>().setSkill(skills[i]);
					this.skillsScripts[i].GetComponent<SkillObjectController>().show();
					this.skillsScripts[i].SetActive(true);
				}
				else{
					this.skillsScripts[i].SetActive(false);
				}
			}
			if (this.nbTurns!=0){
				this.skillsScripts[4].GetComponent<SkillObjectController>().setAttack();
				this.skillsScripts[4].GetComponent<SkillObjectController>().show();
				this.skillsScripts[4].SetActive(true);
			}
			else{
				this.skillsScripts[4].SetActive(false);
			}

			if (this.nbTurns!=0){
				this.skillsScripts[5].GetComponent<SkillObjectController>().setPass();
				this.skillsScripts[5].GetComponent<SkillObjectController>().show();
				this.skillsScripts[5].SetActive(true);
			}
			else{
				this.skillsScripts[5].SetActive(false);
			}
		}
		else{
			List<Skill> skills = this.playingCards[this.clickedOpponentPlayingCard].GetComponent<PlayingCardController>().card.Skills;
			for (int i = 0 ; i < 4 ; i++){
				if (i < skills.Count){
					this.opponentSkillsScripts[i].GetComponent<SkillObjectController>().setSkill(skills[i]);
					this.opponentSkillsScripts[i].GetComponent<SkillObjectController>().show();
					this.opponentSkillsScripts[i].SetActive(true);
				}
				else{
					this.opponentSkillsScripts[i].SetActive(false);
				}
			}
			if (this.nbTurns!=0){
				this.opponentSkillsScripts[4].GetComponent<SkillObjectController>().setAttack();
				this.opponentSkillsScripts[4].GetComponent<SkillObjectController>().show();
				this.opponentSkillsScripts[4].SetActive(true);
			}
			else{
				this.opponentSkillsScripts[4].SetActive(false);
			}
			
			if (this.nbTurns!=0){
				this.opponentSkillsScripts[5].GetComponent<SkillObjectController>().setPass();
				this.opponentSkillsScripts[5].GetComponent<SkillObjectController>().show();
				this.opponentSkillsScripts[5].SetActive(true);
			}
			else{
				this.opponentSkillsScripts[5].SetActive(false);
			}
		}
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

	public void hoverPlayingCardHandler(int idPlayingCard)
	{
		bool toHover = true;
		bool toHide = false;

		if (this.isHovering)
		{
			if (this.currentHoveredTile.x != this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile.x || this.currentHoveredTile.y != this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile.y)
			{
				toHide = true;
			} else
			{
				toHover = false;
			}
		}
		if (this.clickedPlayingCard != -1)
		{
			if (clickedPlayingCard == idPlayingCard)
			{
				toHover = false;
			}
		}
		if (this.clickedOpponentPlayingCard != -1)
		{
			if (clickedOpponentPlayingCard == idPlayingCard)
			{
				toHover = false;
			}
		}
		if (this.currentPlayingCard != -1)
		{
			if (currentPlayingCard == idPlayingCard)
			{
				toHover = false;
			}
		}

		if (toHide)
		{
			if (this.hoveredPlayingCard == -1)
			{
				this.hideHoveredTile();
			} else
			{
				this.hideHoveredPlayingCard();
			}
		}
		if (toHover)
		{
			this.hoverPlayingCard(idPlayingCard);
		}
	}

	public void hoverTileHandler(Tile t)
	{
		bool toHover = true;
		bool toHide = false;
		if (this.isHovering)
		{
			if (t.x != this.currentHoveredTile.x || t.y != this.currentHoveredTile.y)
			{
				toHide = true;
			} 
			else
			{
				toHover = false;
			}
		}
		if (this.currentPlayingCard != -1)
		{

		}

		if (toHide)
		{
			if (this.hoveredPlayingCard == -1)
			{
				this.hideHoveredTile();
			} else
			{
				this.hideHoveredPlayingCard();
			}
		}
		if (toHover)
		{
			this.hoverTile(t);
		}
	}
	
	public void passHandler()
	{
		//this.findNextPlayer();
	}

	public void hideClickedPlayingCard()
	{
		this.playingCards [this.clickedPlayingCard].GetComponent<PlayingCardController>().hideSelected();
		this.clickedPlayingCard = -1;
	}

	public void hideOpponentClickedPlayingCard()
	{
		this.playingCards [this.clickedOpponentPlayingCard].GetComponent<PlayingCardController>().hideSelected();
		this.clickedOpponentPlayingCard = -1;
	}

	public void clickPlayingCard(int idPlayingCard)
	{
		this.hideHoveredPlayingCard();
		this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().displaySelected();
		this.clickedPlayingCard = idPlayingCard;
		this.currentClickedTile = this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile;
	}

	public void clickOpponentPlayingCard(int idPlayingCard)
	{
		this.hideHoveredPlayingCard();
		this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().displayOpponentSelected();
		this.clickedOpponentPlayingCard = idPlayingCard;
		this.currentClickedTile = this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile;
	}

	public void hideActivatedPlayingCard()
	{
		this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().hidePlaying();
		this.currentPlayingCard = -1;
		this.isDragging = false;
		this.gameView.gameScreenVM.SetCursorToDefault();
	}

	public void clickPlayingCardHandler(int idPlayingCard)
	{
		bool toClick = false;
		bool toClickOpponent = false;
		bool toHideClick = false;
		bool toHideOpponentClick = false;
		bool toHidePlay = false;
		bool toPlay = false;

		if ((idPlayingCard < 5) == (this.isFirstPlayer))
		{

			if (idPlayingCard == this.clickedPlayingCard)
			{
				if (idPlayingCard != this.currentPlayingCard||nbTurns==0)
				{
					if (nbTurns==0){
						toHidePlay = true;
						this.clickedPlayingCard=-1;
					}
					else{
						toHideClick = true;
					}
				}
			} 
			else if (nbTurns==0 && this.isDragging){
				
			}
			else
			{
				if (nbTurns == 0)
				{
					if (idPlayingCard != this.currentPlayingCard)
					{
						if (this.currentPlayingCard != -1)
						{
							toHidePlay = true;
						}
						toPlay = true;
					}
				} 
				else
				{
					if (clickedPlayingCard != -1)
					{
						if (clickedPlayingCard != this.currentPlayingCard)
						{
							toHideClick = true;
						}
					}
					if (this.clickedPlayingCard != idPlayingCard)
					{
						toClick = true;
					}
				}
			}
		} else
		{
			if (idPlayingCard == this.clickedOpponentPlayingCard)
			{
				if (idPlayingCard != this.currentPlayingCard)
				{
					toHideOpponentClick = true;
				}
			} else
			{
				if (clickedOpponentPlayingCard != -1)
				{
					if (clickedOpponentPlayingCard != this.currentPlayingCard)
					{
						toHideOpponentClick = true;
					}
				}
				if (this.clickedOpponentPlayingCard != idPlayingCard)
				{
					toClickOpponent = true;
				}
			}
		}

		if (toHideClick)
		{
			this.hideClickedPlayingCard();
		}
		if (toHideOpponentClick)
		{
			this.hideOpponentClickedPlayingCard();
		}
		if (toHidePlay)
		{
			this.hideActivatedPlayingCard();
		}
		if (toClick)
		{
			this.clickPlayingCard(idPlayingCard);
		}
		if (toClickOpponent)
		{
			this.clickOpponentPlayingCard(idPlayingCard);
		}
		if (toPlay)
		{
			this.activatePlayingCard(idPlayingCard);
		}
	}

	public void releaseClickPlayingCardHandler(int idPlayingCard)
	{
		if (isDragging){
			if (idPlayingCard!=this.currentPlayingCard){
				if (this.tiles[currentHoveredTile.x,currentHoveredTile.y].GetComponentInChildren<TileController>().isDestination){
					if (nbTurns == 0 && this.tiles[currentHoveredTile.x, currentHoveredTile.y].GetComponentInChildren<TileController>().characterID!=-1){
						int x = currentHoveredTile.x ;
						int y = currentHoveredTile.y ;
						Tile t = this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().tile;

						photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, t.x, t.y, this.hoveredPlayingCard, this.isFirstPlayer, true);
						this.hideHoveredPlayingCard();
						photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, x, y, this.currentPlayingCard, this.isFirstPlayer, true);

					}
					else{
						int x = currentHoveredTile.x ;
						int y = currentHoveredTile.y ;
						this.hideHoveredTile();
						photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, x, y, this.currentPlayingCard, this.isFirstPlayer, false);
					}
					if (nbTurns==0){
						this.hideActivatedPlayingCard();
					}
				}
			}
			else {
				if (this.tiles[currentHoveredTile.x, currentHoveredTile.y].GetComponentInChildren<TileController>().characterID!=this.currentPlayingCard){
					if (this.tiles[currentHoveredTile.x, currentHoveredTile.y].GetComponentInChildren<TileController>().characterID!=-1){
						int x = currentHoveredTile.x ;
						int y = currentHoveredTile.y ;
						Tile t = this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().tile;
						
						photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, t.x, t.y, this.hoveredPlayingCard, this.isFirstPlayer, true);
						this.hideHoveredPlayingCard();
						photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, x, y, this.currentPlayingCard, this.isFirstPlayer, true);
					}
					else{
						print ("blabla");
						int x = currentHoveredTile.x ;
						int y = currentHoveredTile.y ;
						this.hideHoveredTile();
						photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, x, y, this.currentPlayingCard, this.isFirstPlayer, false);
					}
					if (nbTurns==0){
						this.hideActivatedPlayingCard();
					}
				}
			}
		}
	}

	public void releaseClickTileHandler(Tile t)
	{
		if (isDragging){
			if (this.tiles[t.x,t.y].GetComponentInChildren<TileController>().isDestination){
				int x = currentHoveredTile.x ;
				int y = currentHoveredTile.y ;
				this.hideHoveredTile();
				photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, x, y, this.currentPlayingCard, this.isFirstPlayer, false);
				if (nbTurns==0){
					this.hideActivatedPlayingCard();
				}
			}
		}

	}

	public void findNextPlayer()
	{
		bool newTurn = true;
		int nextPlayingCard = -1;
		int i = 0;

		while (i < 10 && newTurn == true)
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

		this.currentPlayingCard = id ;
		this.currentPlayingTile = this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile;
		
		this.activatePlayingCard(id);
		this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile.setNeighbours(this.getCharacterTilesArray(), this.playingCards [id].GetComponentInChildren<PlayingCardController>().card.Move);
		this.setDestinations(currentPlayingCard);
		this.isDragging = true;

		if ((currentPlayingCard < 5 && this.isFirstPlayer) || (currentPlayingCard >= 5 && !this.isFirstPlayer))
		{
			displayPopUpMessage("A votre tour de jouer", 2f);
		} else
		{
			displayPopUpMessage("Tour du joueur adverse", 2f);
		}
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
		addGameEvent(ge, "");
		nbActionPlayed = 0;
		changeGameEvents();
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
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick1", user1); 	                    // Pseudo de l'utilisateur victorieux
		form.AddField("myform_nick2", user2); 	                    // Pseudo de l'autre utilisateur
				
		WWW w = new WWW(URLStat, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null)
			{
				print(w.error); 										// donne l'erreur eventuelle
			} else
			{
				//print(w.text);
			}
		yield break;
	}

	private void sortPlayingCard(int idPlayingCard)
	{
//		int speed = this.playingCards [idPlayingCard].GetComponentInChildren<PlayingCardController>().card.Speed;
//		this.rankedPlayingCardsID.Remove(idPlayingCard);
//		int i = 0;
//		bool isInserted = false;
//
//		while (!isInserted && i<this.rankedPlayingCardsID.Count)
//		{
//			if (speed >= this.playingCards [this.rankedPlayingCardsID [i]].GetComponentInChildren<PlayingCardController>().card.Speed)
//			{
//				this.rankedPlayingCardsID.Insert(i, idPlayingCard);
//				isInserted = true;
//			}
//			i++;
//		}
//		if (!isInserted)
//		{
//			this.rankedPlayingCardsID.Add(idPlayingCard);
//		}
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
		users [id - 1] = new User(loginName);	
		yield return StartCoroutine(users [id - 1].retrievePicture());
		yield return StartCoroutine(users [id - 1].setProfilePicture());

		if (ApplicationModel.username == loginName)
		{
			this.gameView.gameScreenVM.myPlayerName = loginName;
		} else
		{
			this.gameView.gameScreenVM.hisPlayerName = loginName;
			this.gameView.gameScreenVM.connectOtherPlayer();
		}
		nbPlayers++;

		if (this.isReconnecting)
		{
			if (ApplicationModel.username == loginName)
			{
				if (nbPlayers == 1)
				{
					this.isFirstPlayer = true;
				}
			}
		} 
		else
		{
			if (this.isFirstPlayer && nbPlayers == 1)
			{
				this.initGrid();
				StartCoroutine(this.loadMyDeck());
			} 
			else if (!this.isFirstPlayer && nbPlayers == 2)
			{
				StartCoroutine(this.loadMyDeck());
			}
		}

		if (ApplicationModel.username == loginName)
		{
			if (this.isFirstPlayer){
				for (int i = 0 ; i < this.nbFreeStartRows ; i++){
					for (int j = 0 ; j < this.boardWidth ; j++){
						this.tiles[j,i].GetComponent<TileController>().setDestination();
					}
				}
			}
			else{
				for (int i = this.boardHeight-1 ; i > this.boardHeight-1-this.nbFreeStartRows ; i--){
					for (int j = 0 ; j < this.boardWidth ; j++){
						this.tiles[j,i].GetComponent<TileController>().setDestination();
					}
				}
			}
		}
	}

	[RPC]
	void AddTileToBoard(int x, int y, int type)
	{
		tiles [x, y] = (GameObject)Instantiate(this.tile);
		tiles [x, y].name = "Tile " + (x) + "-" + (y);

		tiles [x, y].GetComponent<TileController>().setTile(x, y, this.boardWidth, this.boardHeight, type, this.tileScale);
	}

	[RPC]
	IEnumerator SpawnCharacter(bool isFirstP, int idDeck)
	{
		Deck deck;
		deck = new Deck(idDeck);
		yield return StartCoroutine(deck.RetrieveCards());
		int debut;
		int hauteur;

		if (isFirstP)
		{
			debut = 0;
			hauteur = 0;
		} else
		{
			debut = 5;
			hauteur = 7;
		}
		if(isFirstP=this.isFirstPlayer)
		{
			this.myDeck=deck;
		}

		for (int i = 0; i < 5; i++)
		{
			this.playingCards [debut + i] = (GameObject)Instantiate(this.playingCard);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setStyles((isFirstP == this.isFirstPlayer));
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setCard(deck.Cards [i]);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setIDCharacter(debut + i);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setTile(new Tile(i, hauteur), tiles [i, hauteur].GetComponent<TileController>().tileView.tileVM.position, !isFirstP);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().resize(this.gameView.gameScreenVM.heightScreen);
			this.tiles[i, hauteur].GetComponent<TileController>().characterID = debut + i;
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().show();
		}
		//testTimeline();
		yield break;
	}

	[RPC]
	public void StartFightRPC(bool isFirst)
	{
		this.nbPlayersReadyToFight++;

		if (this.nbPlayersReadyToFight == 2)
		{
			this.gameView.gameScreenVM.toDisplayStartWindows = false ;
			this.displayPopUpMessage("Le combat commence", 2);
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
		} else
		{
			if (isFirst == this.isFirstPlayer)
			{
				this.gameView.gameScreenVM.startMyPlayer();
			} else
			{
				this.gameView.gameScreenVM.startOtherPlayer();
			}
		}
	}

	private void sortAllCards()
	{
		List <int> cardsToRank = new List<int>() ;
		List <int> quicknessesToRank = new List<int>() ;
		int indexToRank ;

		for (int i = 0 ; i < 10 ; i++){
			cardsToRank.Add(i);	
			quicknessesToRank.Add(this.playingCards[i].GetComponentInChildren<PlayingCardController>().card.Speed);
		}
		for (int i = 0 ; i < 10 ; i++){
			indexToRank = this.FindMaxQuicknessIndex(quicknessesToRank);
			quicknessesToRank.RemoveAt(indexToRank);
			photonView.RPC("addRankedCharacter", PhotonTargets.AllBuffered, cardsToRank[indexToRank], i, (i==0));
		}
	}

	public int FindMaxQuicknessIndex(List<int> list)
	{
		if (list.Count == 0)
		{
			throw new InvalidOperationException("Liste vide !");
		}
		int max = -1;
		int index = -1 ;
		for (int i = 0 ; i < list.Count ; i++)
		{
			if (list[i] > max)
			{
				max = list[i];
				index = i;
			}
		}
		return index;
	}
	
	[RPC]
	public void addRankedCharacter(int id, int rank, bool toCreate)
	{
		if (toCreate){
			this.rankedPlayingCardsID = new int[10] ;
		}
		this.rankedPlayingCardsID[rank]=id;
	}

	[RPC]
	public void moveCharacterRPC(int x, int y, int c, bool isFirstPlayer, bool isSwap)
	{
		print ("Je move "+c+" vers "+x+","+y);
		if (!isSwap){
			this.tiles[this.playingCards [c].GetComponentInChildren<PlayingCardController>().tile.x, this.playingCards [c].GetComponentInChildren<PlayingCardController>().tile.y].GetComponent<TileController>().characterID = -1 ;
		}

		this.tiles[x, y].GetComponent<TileController>().characterID = c ;
		this.playingCards [c].GetComponentInChildren<PlayingCardController>().changeTile(new Tile(x, y), this.tiles[x, y].GetComponent<TileController>().getPosition());

		if (!photonView.isMine)
		{
			displayPopUpMessage(this.playingCards [c].GetComponentInChildren<PlayingCardController>().card.Title + " s'est déplacé", 2f);
		}
	}

	[RPC]
	public void inflictDamageRPC(int targetCharacter, bool isFisrtPlayerCharacter)
	{
		if (!photonView.isMine)
		{
			displayPopUpMessage(this.playingCards [targetCharacter].GetComponentInChildren<PlayingCardController>().card.Title + " attaque", 2f);
		}
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
			photonView.RPC("AddPlayerToList", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, ApplicationModel.username);

		} else
		{
			Debug.Log("Reconnecting...");
		}
	}
	
	void OnDisconnectedFromPhoton()
	{
		//Application.LoadLevel("Lobby");
	}

	public void quitGameHandler()
	{
		photonView.RPC("quitGameRPC", PhotonTargets.AllBuffered, this.isFirstPlayer);
	}

	[RPC]
	public void quitGameRPC(bool isFirstP){
		StartCoroutine (quitGame(isFirstP));
	}

	public IEnumerator quitGame(bool isFirstP)
	{
		if (isFirstP==this.isFirstPlayer)
		{
			yield return (StartCoroutine(this.sendStat(this.users[0].Username, this.users[1].Username)));
			print ("J'ai perdu comme un gros con");
		}
		else
		{
			yield return (StartCoroutine(this.sendStat(this.users[1].Username, this.users[0].Username)));
			print ("Mon adversaire a lachement abandonné comme une merde");
		}
		PhotonNetwork.Disconnect();

		//EndSceneController.instance.displayEndScene (false);
	}

	public void testTimeline()
	{
		/*this.currentPlayingCard = 1;
		//addMovementEvent(this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.Title, tiles [4, 3], tiles [4, 4]);
		string targetName = "coincoin";
		List<GameSkill> temp = this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().skills;
		if (temp.Count > 0)
		{
			//addGameEvent("", new SkillType(temp [0].Skill.Action), targetName);
		}
		pass();
		this.currentPlayingCard = 0;
		pass();
		this.currentPlayingCard = 1;
		pass();
		this.currentPlayingCard = 2;
		pass();
		this.currentPlayingCard = 3;
		pass();
		this.currentPlayingCard = 4;
		pass();
		this.currentPlayingCard = 1;
		pass();
		this.currentPlayingCard = 2;
		pass();
		this.currentPlayingCard = 3;
		pass();
		this.currentPlayingCard = 4;
		pass();
		displayPopUpMessage("test", 1f, 0);
		inflictDamage(0);
		inflictDamage(1);
		inflictDamage(2);
		inflictDamage(3);
		inflictDamage(4);
		*/

		for (int i = 0; i < 6; i++)
		{
			addCardEvent(i % 5, i);
		}
		addGameEvent(new SkillType("a lancé test"), "vilain");
		gameStarted = true;
	}
	
	public void addGameEvent(GameEventType type, string targetName)
	{
		setGameEvent(type);
		if (targetName != "")
		{
			int midTimeline = (int)Math.Floor((double)eventMax / 2);
			gameEvents [midTimeline].GetComponent<GameEventController>().setTarget(targetName);
		}
	}

	public void addMovementEvent(GameObject origin, GameObject destination)
	{
		GameObject go = setGameEvent(new MovementType());

		go.GetComponent<GameEventController>().setMovement(origin, destination);
	}

	GameObject setGameEvent(GameEventType type)
	{
		int midTimeline = (int)Math.Floor((double)eventMax / 2);
		GameObject go;
		if (nbActionPlayed == 0)
		{ 
			go = gameEvents [midTimeline];
			go.GetComponent<GameEventController>().setAction(type.toString());
			nbActionPlayed++;
		} else if (nbActionPlayed < 2)
		{
			go = gameEvents [midTimeline];
			go.GetComponent<GameEventController>().addAction(type.toString());
			nbActionPlayed++;
		} else
		{
			go = gameEvents [0];
		}

		return go;
	}

	void addCardEvent(int idCharacter, int position)
	{
		GameObject go = gameEvents [position];
		PlayingCardController pcc = playingCards [idCharacter].GetComponent<PlayingCardController>();
		go.GetComponent<GameEventController>().setCharacterName(pcc.card.Title);
		Texture t2 = playingCards [idCharacter].GetComponent<PlayingCardController>().getPicture();
		Texture2D temp = getImageResized(t2 as Texture2D);
		go.GetComponent<GameEventController>().setAction("");
		go.GetComponent<GameEventController>().setArt(temp);
		go.GetComponent<GameEventController>().setBorder(position);
		go.GetComponent<GameEventController>().gameEventView.show();
		go.renderer.enabled = true;
	}

	void initGameEvents()
	{
		GameObject go;
		while (gameEvents.Count < eventMax)
		{
			go = (GameObject)Instantiate(gameEvent);
			gameEvents.Add(go);
			go.GetComponent<GameEventController>().setScreenPosition(gameEvents.Count, boardWidth, boardHeight, tileScale);
			go.renderer.enabled = false;
		}
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
			if (title != "")
			{
				gameEvents [i].renderer.enabled = true;
			}

			gameEvents [i].GetComponent<GameEventController>().setCharacterName(title);
			gameEvents [i].GetComponent<GameEventController>().setAction(action);
			gameEvents [i].GetComponent<GameEventController>().setMovement(movement [0], movement [1]);
			gameEvents [i].GetComponent<GameEventController>().setArt(t2);
			gameEvents [i].GetComponent<GameEventController>().setBorder(i);
			gameEvents [i].GetComponent<GameEventController>().gameEventView.show();
			gameEvents [i - 1].GetComponent<GameEventController>().setMovement(null, null);
		}
	}

	void recalculateGameEvents()
	{
		int i = 1;

		foreach (GameObject go in gameEvents)
		{
			go.GetComponent<GameEventController>().setScreenPosition(i++, boardWidth, boardHeight, tileScale);
		}
	}
}


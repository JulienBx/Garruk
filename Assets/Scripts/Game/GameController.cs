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
	bool isRunningSkill = false ;
	bool playingCardHasMoved = false ;

	int numberOfExpectedArgs ;
	int numberOfArgs ;

	public int[] skillArgs ;

	public Texture2D[] cursors ;
	public GameObject gameEvent;
	public GameObject skillObject;

	public bool playindCardHasPlayed = false ;

	//Variables du controlleur
	public bool isTriggeringSkill = false;
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
	GameObject[] skillsObjects ;

	GameObject selectedOpponentCard ;
	GameObject[] opponentSkillsObjects ;

	Tile currentHoveredTile ;
	Tile currentClickedTile ;
	Tile currentOpponentClickedTile ;
	Tile currentPlayingTile ;
	int hoveredPlayingCard = -1;
	int clickedPlayingCard = -1;
	int clickedOpponentPlayingCard = -1;
	bool isHovering = false ;
	bool isDestinationDrawn = true ;

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

	GameSkill skillToBeCast;
	int nbPlayersReadyToFight; 

	public int currentPlayingCard = -1;
	public int eventMax;
	int nbActionPlayed = 0;
	int nbTurns = 0 ;

	int[] rankedPlayingCardsID; 

	int myNextPlayer ;
	int hisNextPlayer ;

	float timerTurn = 600;
	bool startTurn = false;
	bool timeElapsed = false;
	bool popUpDisplay = false;

	int nextCharacterPositionTimeline;

	GameSkill[] gameskills ;

	string URLStat = ApplicationModel.host + "updateResult.php";

	int clickedSkill ;

	void Awake()
	{
		instance = this;
		this.gameView = Camera.main.gameObject.AddComponent <GameView>();
		this.gameView.gameScreenVM.setStyles(gameScreenStyles);
		tiles = new GameObject[boardWidth, boardHeight];
		playingCards = new GameObject[10];
		gameEvents = new List<GameObject>();
		this.skillArgs = new int[10];

		this.nbPlayersReadyToFight = 0;
		this.currentPlayingCard = -1;
		this.eventMax = 11;
		this.verticalBorders = new GameObject[this.boardWidth + 1];
		this.horizontalBorders = new GameObject[this.boardHeight + 1];

		this.createBackground();
		this.resize();
		this.initSkills();
	}
	
	void Start()
	{	
		users = new User[2];
		PhotonNetwork.ConnectUsingSettings(ApplicationModel.photonSettings);
		PhotonNetwork.autoCleanUpPlayerObjects = false;
	}	

	void Update()
	{	
		if (this.widthScreen != Screen.width || this.heightScreen != Screen.height)
		{
			this.resize();
		}
		for (int i = 0; i < gameView.gameScreenVM.messagesToDisplay.Count; i++)
		{
			gameView.gameScreenVM.timersPopUp [i] -= Time.deltaTime;
			if (gameView.gameScreenVM.timersPopUp [i] < 0)
			{
				gameView.gameScreenVM.messagesToDisplay.RemoveAt(i);
				gameView.gameScreenVM.timersPopUp.RemoveAt(i);
				gameView.gameScreenVM.centerMessageRects.RemoveAt(i);
				gameView.gameScreenVM.resizePopUps();
			}
		}

		
		if (startTurn)
		{
			gameView.gameScreenVM.timer -= Time.deltaTime;
			if (timeElapsed)
			{
				timeElapsed = false;
				gameView.gameScreenVM.timer -= 1;
				displayPopUpMessage("Temps ecoulé", 5f);
			}
			if (gameView.gameScreenVM.timer < 0 && gameView.gameScreenVM.timer > -1)
			{
				timeElapsed = true;
				clickSkillHandler(5);
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
		if (EndSceneController.instance != null)
		{
			EndSceneController.instance.resize();
		}
	}

	public void displayPopUpMessage(string message, float time)
	{
		gameView.gameScreenVM.messagesToDisplay.Add(message);
		gameView.gameScreenVM.timersPopUp.Add(time);
		gameView.gameScreenVM.centerMessageRects.Add(new Rect());
		gameView.gameScreenVM.resizePopUps();
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
		this.selectedPlayingCard.GetComponent<PlayingCardController>().isMine = true;
		this.selectedPlayingCard.GetComponent<PlayingCardController>().setControlActive(false);
		this.selectedPlayingCard.GetComponent<PlayingCardController>().setActive(false);

		this.selectedOpponentCard = (GameObject)Instantiate(this.playingCard);
		this.selectedOpponentCard.GetComponent<PlayingCardController>().setControlActive(false);
		this.selectedOpponentCard.GetComponent<PlayingCardController>().setActive(false);

		this.skillsObjects = new GameObject[6];
		this.opponentSkillsObjects = new GameObject[6];
		for (int i = 0; i < 6; i++)
		{
			this.skillsObjects [i] = (GameObject)Instantiate(this.skillObject);
			this.skillsObjects [i].GetComponent<SkillObjectController>().setOwner(true);
			this.skillsObjects [i].GetComponent<SkillObjectController>().setID(i);
			this.skillsObjects [i].GetComponent<SkillObjectController>().setActive(false);
			this.opponentSkillsObjects [i] = (GameObject)Instantiate(this.skillObject);
			this.opponentSkillsObjects [i].GetComponent<SkillObjectController>().setOwner(false);
			this.opponentSkillsObjects [i].GetComponent<SkillObjectController>().setID(i);
			this.opponentSkillsObjects [i].GetComponent<SkillObjectController>().setActive(false);
		}
		this.skillsObjects [4].GetComponent<SkillObjectController>().setAttack();
		this.skillsObjects [5].GetComponent<SkillObjectController>().setPass();
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
			position = new Vector3(0, (5f - 4f * this.tileScale) + tileScale * i - 5f, -1.2f);
			this.horizontalBorders [i].transform.localPosition = position;
			this.horizontalBorders [i].transform.localScale = new Vector3(this.tileScale * 6f, this.borderSize, this.borderSize);
		}

		for (int i = 0; i < this.verticalBorders.Length; i++)
		{
			position = new Vector3((-3 * this.tileScale) + i * this.tileScale, 0f, -1.2f);
			this.verticalBorders [i].transform.localPosition = position;
			this.verticalBorders [i].transform.localScale = new Vector3(this.verticalBorders [i].transform.localScale.x, 8f * tileScale, this.verticalBorders [i].transform.localScale.z);
		}

		position = new Vector3((-2.6f * this.tileScale), -4.5f * this.tileScale, -1f);
		scale = new Vector3(0.8f, 0.8f, 0.8f);
		this.selectedPlayingCard.GetComponent<PlayingCardController>().setPosition(position, scale);

		position = new Vector3((-2.6f * this.tileScale), 4.5f * this.tileScale, -1f);
		scale = new Vector3(0.8f, 0.8f, 0.8f);
		this.selectedOpponentCard.GetComponent<PlayingCardController>().setPosition(position, scale);

		for (int i = 0; i < this.skillsObjects.Length; i++)
		{
			position = new Vector3((-1.5f * this.tileScale) + i * this.tileScale * 0.8f, -4.4f * this.tileScale, -1f);
			scale = new Vector3(0.6f, 0.6f, 0.6f);
			this.skillsObjects [i].GetComponent<SkillObjectController>().setPosition(position, scale);
			this.skillsObjects [i].GetComponent<SkillObjectController>().resize();
			this.skillsObjects [i].GetComponent<SkillObjectController>().show();
		}

		for (int i = 0; i < this.opponentSkillsObjects.Length; i++)
		{
			position = new Vector3((-1.5f * this.tileScale) + i * this.tileScale * 0.8f, 4.4f * this.tileScale, -1f);
			scale = new Vector3(0.6f, 0.6f, 0.6f);
			this.opponentSkillsObjects [i].GetComponent<SkillObjectController>().setPosition(position, scale);
			this.opponentSkillsObjects [i].GetComponent<SkillObjectController>().resize();
			this.opponentSkillsObjects [i].GetComponent<SkillObjectController>().show();
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
		if (this.hoveredPlayingCard != -1)
		{
			this.hideHoveredPlayingCard();
		}
		this.currentPlayingCard = idPlayingCard;

		if (this.isFirstPlayer == (idPlayingCard < 5))
		{
			this.isDragging = true;
			this.clickedPlayingCard = idPlayingCard;
		} else
		{
			this.clickedOpponentPlayingCard = idPlayingCard;
		}

		this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().displayPlaying();
		this.currentPlayingTile = this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().tile;
		this.currentClickedTile = this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().tile;

		if (this.isFirstPlayer == (idPlayingCard < 5))
		{
			this.showMyPlayingSkills(this.currentPlayingCard);
		} else
		{
			this.showOpponentSkills(this.currentPlayingCard);
		}
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
			} else
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

	public void hideClickedPlayingCard()
	{
		this.hideMySkills();

		this.playingCards [this.clickedPlayingCard].GetComponent<PlayingCardController>().hideSelected();
		print("Je hide " + this.clickedPlayingCard);
		this.clickedPlayingCard = -1;
	}

	public void hideOpponentClickedPlayingCard()
	{
		this.hideOpponentSkills();

		this.playingCards [this.clickedOpponentPlayingCard].GetComponent<PlayingCardController>().hideSelected();
		this.clickedOpponentPlayingCard = -1;
	}

	public void clickPlayingCard(int idPlayingCard)
	{
		if (this.hoveredPlayingCard != -1)
		{
			this.hideHoveredPlayingCard();
		}
		this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().displaySelected();
		this.clickedPlayingCard = idPlayingCard;
		this.currentClickedTile = this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile;
		this.showMyPlayingSkills(this.clickedPlayingCard);
	}

	public void clickOpponentPlayingCard(int idPlayingCard)
	{
		if (this.hoveredPlayingCard != -1)
		{
			this.hideHoveredPlayingCard();
		}
		this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().displayOpponentSelected();
		this.clickedOpponentPlayingCard = idPlayingCard;
		this.currentClickedTile = this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().tile;

		this.showOpponentSkills(this.clickedOpponentPlayingCard);
	}

	void setValidationTexts(string regularText, string buttonText)
	{
		this.gameView.gameScreenVM.toDisplayValidationWindows = true;
		this.gameView.gameScreenVM.toDisplayValidationButton = false;
		this.gameView.gameScreenVM.validationRegularText = regularText;
		this.gameView.gameScreenVM.validationButtonText = buttonText;
	}

	public void lookForTarget(string regularText, string buttonText)
	{
		this.numberOfExpectedArgs = 1;
		setValidationTexts(regularText, buttonText);

		for (int i = 0; i < 10; i++)
		{
			if (!this.playingCards [i].GetComponent<PlayingCardController>().isDead && this.playingCards [i].GetComponent<PlayingCardController>().cannotBeTargeted == -1)
			{
				this.playingCards [i].GetComponent<PlayingCardController>().activateTargetHalo();
			}
		}
	}
	
	public void lookForAnyTile(string regularText, string buttonText)
	{
		this.numberOfExpectedArgs = 2;
		setValidationTexts(regularText, buttonText);
		
		for (int i = 0 ; i < this.boardWidth ; i++){
			for (int j = 0 ; j < this.boardHeight ; j++){
				this.tiles[i,j].GetComponent<TileController>().setLookingForTileZone(true);
			}
		}
	}
	
	public void changeZoneTargetTile(Tile t){
		int x = t.x;
		int y = t.y;
		for (int i = 0 ; i < this.boardWidth ; i++){
			for (int j = 0 ; j < this.boardHeight ; j++){
				this.tiles[i,j].GetComponent<TileController>().removeHalo();
			}
		}
		
		if (x>0){
			if (y>0){
				this.tiles[x-1,y-1].GetComponent<TileController>().activateEffectZoneHalo();
			}
			if (y<this.boardHeight-1){
				this.tiles[x-1,y+1].GetComponent<TileController>().activateEffectZoneHalo();
			}
			this.tiles[x-1,y].GetComponent<TileController>().activateEffectZoneHalo();
		}
		if (x<this.boardWidth-1){
			if (y>0){
				this.tiles[x+1,y-1].GetComponent<TileController>().activateEffectZoneHalo();
			}
			if (y<this.boardHeight-1){
				this.tiles[x+1,y+1].GetComponent<TileController>().activateEffectZoneHalo();
			}
			this.tiles[x+1,y].GetComponent<TileController>().activateEffectZoneHalo();
		}
		if (y>0){
			this.tiles[x,y-1].GetComponent<TileController>().activateEffectZoneHalo();
		}
		if (y<this.boardHeight-1){
			this.tiles[x,y+1].GetComponent<TileController>().activateEffectZoneHalo();
		}
		this.tiles[x,y].GetComponent<TileController>().activateEffectZoneHalo();
	}
	
	public void addTargetTile(int x, int y){
	
	}

	public void lookForAdjacentTarget(string regularText, string buttonText)
	{
		this.numberOfExpectedArgs = 1;
		setValidationTexts(regularText, buttonText);

		List<TileController> tempTiles;
		tempTiles = new List<TileController>();
		foreach (Tile t in this.getCurrentPCC().tile.getImmediateNeighbouringTiles())
		{
			if (this.tiles [t.x, t.y].GetComponent<TileController>().characterID != -1)
			{
				tempTiles.Add(this.tiles [t.x, t.y].GetComponent<TileController>());
			}
		}
		foreach (TileController tc in tempTiles)
		{
			if (!this.playingCards [tc.characterID].GetComponent<PlayingCardController>().isDead && this.playingCards [tc.characterID].GetComponent<PlayingCardController>().cannotBeTargeted == -1)
			{
				this.playingCards [tc.characterID].GetComponent<PlayingCardController>().activateTargetHalo();
			}
		}
	}
	
	public void lookForAdjacentAllyTarget(string regularText, string buttonText)
	{
		this.numberOfExpectedArgs = 1;
		
		setValidationTexts(regularText, buttonText);
		
		List<TileController> tempTiles;
		tempTiles = new List<TileController>();
		foreach (Tile t in this.getCurrentPCC().tile.getImmediateNeighbouringTiles())
		{
			if (this.currentPlayingCard < 5)
			{
				if (this.tiles [t.x, t.y].GetComponent<TileController>().characterID != -1 && this.tiles [t.x, t.y].GetComponent<TileController>().characterID < 5)
				{
					tempTiles.Add(this.tiles [t.x, t.y].GetComponent<TileController>());
				}
			} else
			{
				if (this.tiles [t.x, t.y].GetComponent<TileController>().characterID != -1 && this.tiles [t.x, t.y].GetComponent<TileController>().characterID > 4)
				{
					tempTiles.Add(this.tiles [t.x, t.y].GetComponent<TileController>());
				}
			}
		}
		foreach (TileController tc in tempTiles)
		{
			if (!this.playingCards [tc.characterID].GetComponent<PlayingCardController>().isDead && this.playingCards [tc.characterID].GetComponent<PlayingCardController>().cannotBeTargeted == -1)
			{
				this.playingCards [tc.characterID].GetComponent<PlayingCardController>().activateTargetHalo();
			}
		}
	}
	
	public void lookForEmptyAdjacentTile(string regularText, string buttonText)
	{
		this.numberOfExpectedArgs = 2;
		
		setValidationTexts(regularText, buttonText);
		
		foreach (Tile t in this.getCurrentPCC().tile.getImmediateNeighbouringTiles())
		{
			if (this.tiles [t.x, t.y].GetComponent<TileController>().characterID == -1)
			{
				this.tiles [t.x, t.y].GetComponent<TileController>().activateWolfTrapTarget();
			}
		}
	}

	public void lookForTileTarget(string regularText, string buttonText)
	{
		this.numberOfExpectedArgs = 1;
		setValidationTexts(regularText, buttonText);
		for (int i = 0; i < this.boardWidth; i++)
		{
			for (int j = 0; j < this.boardHeight; j++)
			{
				this.tiles [i, j].GetComponent<TileController>().activatePotentielTarget();
			}
		}
	}

	public void addTarget(int id)
	{
		this.skillArgs [numberOfArgs] = id;
		this.numberOfArgs++;

		if (this.numberOfExpectedArgs <= this.numberOfArgs)
		{
			this.gameView.gameScreenVM.toDisplayValidationButton = true;
			this.gameView.gameScreenVM.validationRegularText = "Cible choisie !";
			for (int i = 0; i < 10; i++)
			{
				this.playingCards [i].GetComponent<PlayingCardController>().removeTargetHalo();
			}
			this.validateSkill();
		}
	}

	public void addBuffTileTarget(Tile t)
	{
		this.skillArgs [numberOfArgs] = t.x;
		this.numberOfArgs++;
		this.skillArgs [numberOfArgs] = t.y;
		this.numberOfArgs++;

		if (this.numberOfExpectedArgs <= this.numberOfArgs)
		{
			this.gameView.gameScreenVM.toDisplayValidationButton = true;
			this.gameView.gameScreenVM.validationRegularText = "Cible choisie !";
			for (int i = 0; i < this.boardWidth; i++)
			{
				for (int j = 0; j < this.boardHeight; j++)
				{
					this.tiles [i, j].GetComponent<TileController>().removePotentielTarget();
				}
			}
			this.validateSkill();
		}
	}
	
	public void addTileTarget(Tile t)
	{
		this.skillArgs [numberOfArgs] = t.x;
		this.numberOfArgs++;
		this.skillArgs [numberOfArgs] = t.y;
		this.numberOfArgs++;
		
		if (this.numberOfExpectedArgs <= this.numberOfArgs)
		{
			this.gameView.gameScreenVM.toDisplayValidationButton = true;
			this.gameView.gameScreenVM.validationRegularText = "Cible choisie !";
			for (int i = 0; i < this.boardWidth; i++)
			{
				for (int j = 0; j < this.boardHeight; j++)
				{
					this.tiles [i, j].GetComponent<TileController>().removeHalo();
					this.tiles [i, j].GetComponent<TileController>().removeZoneEffect();
				}
			}
			this.validateSkill();
		}
	}

	public void deactivateMySkills()
	{
		for (int i = 0; i < 6; i++)
		{
			this.skillsObjects [i].GetComponent<SkillObjectController>().setControlActive(false);
		}
	}
	public void lookForValidation(bool toDisplayButton, string regularText, string buttonText)
	{
		setValidationTexts(regularText, buttonText);
		this.gameView.gameScreenVM.toDisplayValidationButton = toDisplayButton;
	}

	public void cancelSkill()
	{
		this.gameView.gameScreenVM.toDisplayValidationWindows = false;
		this.isRunningSkill = false;
		this.playindCardHasPlayed = false;
		for (int i = 0; i < 10; i++)
		{
			this.getPCC(i).removeTargetHalo();	
		}
		this.showMyPlayingSkills(this.currentPlayingCard);
	}

	public void validateSkill()
	{
		this.gameView.gameScreenVM.toDisplayValidationWindows = false;
		this.gameskills [this.getCurrentSkillID()].resolve(this.skillArgs);
	}

	public void hideActivatedPlayingCard()
	{
		if (this.isFirstPlayer == (this.currentPlayingCard < 5))
		{
			this.hideMySkills();
		} else
		{
			this.hideOpponentSkills();
		}

		this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().hidePlaying();
		if (this.currentPlayingCard == this.clickedPlayingCard)
		{
			this.clickedPlayingCard = -1;
		}
		this.currentPlayingCard = -1;
		this.isDragging = false;
	}

	public void showMyPlayingSkills(int idc)
	{
		this.selectedPlayingCard.GetComponent<PlayingCardController>().setCard(this.playingCards [idc].GetComponent<PlayingCardController>().card);
		this.selectedPlayingCard.GetComponent<PlayingCardController>().show();
		this.selectedPlayingCard.GetComponent<PlayingCardController>().setActive(true);

		List<Skill> skills = this.playingCards [idc].GetComponent<PlayingCardController>().card.Skills;
		for (int i = 0; i < 4; i++)
		{
			if (i < skills.Count)
			{
				this.skillsObjects [i].GetComponent<SkillObjectController>().setSkill(skills [i]);
				this.skillsObjects [i].GetComponent<SkillObjectController>().setActive(true);
			} else
			{
				this.skillsObjects [i].GetComponent<SkillObjectController>().setActive(false);
			}
		}
		
		if (nbTurns != 0)
		{
			this.updateStatusMySkills(idc);
		}
		
		bool isActive = !(nbTurns == 0) && (idc == this.currentPlayingCard);
		this.skillsObjects [4].GetComponent<SkillObjectController>().setActive(isActive);
		this.skillsObjects [5].GetComponent<SkillObjectController>().setActive(isActive);
		
	}
		
	public void updateStatusMySkills(int idc)
	{
		bool controlActive;
		bool isActive = !(nbTurns == 0) && (idc == this.currentPlayingCard);
		List<Skill> skills = this.playingCards [idc].GetComponent<PlayingCardController>().card.Skills;
		for (int i = 0; i < 4; i++)
		{
			if (i < skills.Count)
			{
				this.skillsObjects [i].GetComponent<SkillObjectController>().setActiveStatus(isActive);
				controlActive = this.gameskills [skills [i].Id].isLaunchable(skills [i]) && !this.playindCardHasPlayed && !this.isRunningSkill;
				this.skillsObjects [i].GetComponent<SkillObjectController>().setControlStatus(controlActive);
				this.skillsObjects [i].GetComponent<SkillObjectController>().show();
			}
		}
		this.skillsObjects [4].GetComponent<SkillObjectController>().setActive(isActive);
		if (isActive)
		{
			this.skillsObjects [4].GetComponent<SkillObjectController>().setAttackValue(this.playingCards [idc].GetComponent<PlayingCardController>().card.Attack);
		}
		this.skillsObjects [4].GetComponent<SkillObjectController>().setActiveStatus(isActive);
		controlActive = this.gameskills [0].isLaunchable(new Skill()) && !this.playindCardHasPlayed && !this.isRunningSkill;
		this.skillsObjects [4].GetComponent<SkillObjectController>().setControlStatus(controlActive);
		this.skillsObjects [4].GetComponent<SkillObjectController>().show();
		this.skillsObjects [5].GetComponent<SkillObjectController>().setActive(isActive);
		this.skillsObjects [5].GetComponent<SkillObjectController>().setActiveStatus(isActive);
		this.skillsObjects [5].GetComponent<SkillObjectController>().setControlStatus(true);
		this.skillsObjects [5].GetComponent<SkillObjectController>().show();
	}

	public void showOpponentSkills(int idc)
	{
		this.selectedOpponentCard.GetComponent<PlayingCardController>().setCard(this.playingCards [idc].GetComponent<PlayingCardController>().card);
		this.selectedOpponentCard.GetComponent<PlayingCardController>().show();
		this.selectedOpponentCard.GetComponent<PlayingCardController>().setActive(true);
		
		List<Skill> skills = this.playingCards [idc].GetComponent<PlayingCardController>().card.Skills;
		for (int i = 0; i < 4; i++)
		{
			if (i < skills.Count)
			{
				this.opponentSkillsObjects [i].GetComponent<SkillObjectController>().setSkill(skills [i]);
				this.opponentSkillsObjects [i].SetActive(true);
			} else
			{
				this.opponentSkillsObjects [i].SetActive(false);
			}
		}
		this.opponentSkillsObjects [4].SetActive(false);
		this.opponentSkillsObjects [5].SetActive(false);
	}

	public void hideMySkills()
	{
		this.selectedPlayingCard.SetActive(false);
		for (int i = 0; i < 6; i++)
		{
			this.skillsObjects [i].SetActive(false);
		}
	}

	public void hideOpponentSkills()
	{
		for (int i = 0; i < 6; i++)
		{
			this.selectedOpponentCard.SetActive(false);
			this.opponentSkillsObjects [i].SetActive(false);
		}
	}

	public void clickPlayingCardHandler(int idPlayingCard)
	{
		bool toClick = false;
		bool toClickOpponent = false;
		bool toHideClick = false;
		bool toHideOpponentClick = false;
		bool toHidePlay = false;
		bool toPlay = false;


		if (idPlayingCard == this.currentPlayingCard)
		{
			if (this.nbTurns == 0)
			{
				toHidePlay = true;
			} else if (this.isFirstPlayer == (idPlayingCard < 5))
			{
				if (this.clickedPlayingCard != idPlayingCard)
				{
					this.hideClickedPlayingCard();
					this.clickedPlayingCard = idPlayingCard;
					this.showMyPlayingSkills(idPlayingCard);
				}
			} else
			{
				if (this.clickedOpponentPlayingCard != idPlayingCard)
				{
					this.hideOpponentClickedPlayingCard();
					this.clickedOpponentPlayingCard = idPlayingCard;
					this.showOpponentSkills(idPlayingCard);
				}
			}
		} else if (idPlayingCard == this.clickedPlayingCard)
		{
			toHideClick = true;
		} else if (idPlayingCard == this.clickedOpponentPlayingCard)
		{
			toHideOpponentClick = true;
		} else
		{
			if (this.nbTurns == 0)
			{
				if (this.isFirstPlayer == (idPlayingCard < 5))
				{
					if (this.currentPlayingCard != -1)
					{
						toHidePlay = true;
					}
					toPlay = true;
				} else
				{
					if (clickedOpponentPlayingCard != -1)
					{
						toHideOpponentClick = true;
					}
					toClickOpponent = true;
				}
			} else
			{
				if (this.isFirstPlayer == (idPlayingCard < 5))
				{
					if (this.clickedPlayingCard != -1 && this.clickedPlayingCard != this.currentPlayingCard)
					{
						toHideClick = true;
					}
					toClick = true;
				} else
				{
					if (clickedOpponentPlayingCard != -1 && this.clickedOpponentPlayingCard != this.currentPlayingCard)
					{
						toHideOpponentClick = true;
					}
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
		if (isDragging)
		{
			if (this.isHovering && this.tiles [currentHoveredTile.x, currentHoveredTile.y].GetComponentInChildren<TileController>().characterID == -1 && this.tiles [currentHoveredTile.x, currentHoveredTile.y].GetComponentInChildren<TileController>().isDestination)
			{
				int x = currentHoveredTile.x;
				int y = currentHoveredTile.y;
				this.hideHoveredTile();
				photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, x, y, this.currentPlayingCard, this.isFirstPlayer, false);
				
			}
		}
	}

	public void releaseClickTileHandler(Tile t)
	{
		if (isDragging)
		{
			if (this.tiles [t.x, t.y].GetComponentInChildren<TileController>().isDestination && this.tiles [currentHoveredTile.x, currentHoveredTile.y].GetComponentInChildren<TileController>().characterID == -1)
			{
				int x = currentHoveredTile.x;
				int y = currentHoveredTile.y;
				this.hideHoveredTile();
				photonView.RPC("moveCharacterRPC", PhotonTargets.AllBuffered, x, y, this.currentPlayingCard, this.isFirstPlayer, false);
			}
		}
	}

	[RPC]
	public void resolveSkill(int idSkill, int[] args)
	{
		this.clickedSkill = idSkill;
		this.gameskills [this.getCurrentSkillID()].resolve(args);
	}

	[RPC]
	public void castSkillOnTarget(int idPlayingCard)
	{
		PlayingCardController pcc = this.playingCards [idPlayingCard].GetComponent<PlayingCardController>();
		skillToBeCast.setTarget(pcc);
		//this.addGameEvent(new SkillType(skillToBeCast.skill.Action), pcc.card.Title);
		//this.displayPopUpMessage(this.playingCards [currentPlayingCard].GetComponent<PlayingCardController>().card.Title +
		//	" " + skillToBeCast.skill.Action + " sur " + pcc.card.Title + " " + currentModifier.Amount + " " + convertStatToString(currentModifier.Stat), 2f);
		this.playingCards [idPlayingCard].GetComponent<PlayingCardController>().show();
	}

	public void clickSkillHandler(int ids)
	{
		this.isRunningSkill = true;
		this.updateStatusMySkills(this.currentPlayingCard);
		this.clickedSkill = ids;
		if (ids > 3)
		{
			if (ids == 4)
			{
				this.gameskills [0].launch();
			} else
			{
				this.gameskills [1].launch();
			}
		} else
		{
			this.gameskills [this.playingCards [this.currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.Skills [ids].Id].launch();
		}
		this.skillArgs = new int[10];
		for (int i = 0; i < 10; i++)
		{
			this.skillArgs [i] = -1;
		}
		this.numberOfArgs = 0;
	}

	public void findNextPlayer()
	{
		if (this.currentPlayingCard != -1)
		{
			this.playingCards [this.currentPlayingCard].GetComponentInChildren<PlayingCardController>().hasPlayed = true;
		}
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
			for (i = 0; i < 10; i++)
			{
				if (!this.playingCards [i].GetComponentInChildren<PlayingCardController>().isDead)
				{
					this.playingCards [i].GetComponentInChildren<PlayingCardController>().hasPlayed = false;
				}
			}
			int j = 0;
			while (j < 10)
			{
				if (!this.playingCards [rankedPlayingCardsID [j]].GetComponentInChildren<PlayingCardController>().hasPlayed)
				{
					nextPlayingCard = rankedPlayingCardsID [j];
					j = 11;
				}
				j++;
			}
		}
		
		photonView.RPC("initPlayer", PhotonTargets.AllBuffered, nextPlayingCard, newTurn, this.isFirstPlayer);
	}

	[RPC]
	public void initPlayer(int id, bool newTurn, bool isFirstP)
	{
		print("Au tour de " + id);
		if (newTurn)
		{
			for (int i = 0; i < 10; i++)
			{
				if (!this.playingCards [i].GetComponentInChildren<PlayingCardController>().isDead)
				{
					this.playingCards [i].GetComponentInChildren<PlayingCardController>().hasPlayed = false;
					this.reloadCard(i);
					//reloadDestinationTiles();
				}
			}
		}
		if (this.currentPlayingCard != -1)
		{
			this.playingCards [this.currentPlayingCard].GetComponentInChildren<PlayingCardController>().hasPlayed = true;
		}

		if (this.currentPlayingCard != -1)
		{
			this.hideActivatedPlayingCard();
		}
		this.resetDestinations();

		if (newTurn)
		{
			this.nbTurns++;
		}

		this.currentPlayingCard = id;
		
		if (this.getCurrentCard().isParalyzed())
		{
			this.playindCardHasPlayed = true;
			this.displayPopUpMessage(this.getCurrentCard().Title + " est paralysé", 3f);
		} else
		{
			this.playindCardHasPlayed = false;
		}
		this.isRunningSkill = false;
		
		this.playingCardHasMoved = false;

		this.currentPlayingTile = this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile;

		this.activatePlayingCard(id);

		if (this.isFirstPlayer == (id < 5))
		{
			this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile.setNeighbours(this.getCharacterTilesArray(), this.playingCards [id].GetComponentInChildren<PlayingCardController>().card.GetMove());
			this.setDestinations(currentPlayingCard);
			this.isDragging = true;
		}
		//this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.changeModifiers();
		loadTileModifierToCharacter(getCurrentPCC().tile.x, getCurrentPCC().tile.y);

		this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().show();

		if ((currentPlayingCard < 5 && this.isFirstPlayer) || (currentPlayingCard >= 5 && !this.isFirstPlayer))
		{
			displayPopUpMessage("A votre tour de jouer", 3f);
			this.showMyPlayingSkills(this.currentPlayingCard);
		} else
		{
			displayPopUpMessage("Tour du joueur adverse", 3f);
			this.showOpponentSkills(this.currentPlayingCard);
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
			debut = 5;
		} else
		{
			debut = 0;
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
			this.tiles [t.x, t.y].GetComponentInChildren<TileController>().setDestination(true);
		}
		this.isDestinationDrawn = true;
	}

//	public void setStateOfAttack(bool state)
//	{
//		this.onGoingAttack = state;
//	}

	public void inflictDamage(int targetCharacter)
	{
		photonView.RPC("inflictDamageRPC", PhotonTargets.AllBuffered, targetCharacter, isFirstPlayer);
	}

	public void resolvePass()
	{
		this.isRunningSkill = false;
		
		photonView.RPC("checkModyfiersRPC", PhotonTargets.AllBuffered);
		
		findNextPlayer();
		photonView.RPC("timeRunsOut", PhotonTargets.AllBuffered, timerTurn);
		photonView.RPC("addPassEvent", PhotonTargets.AllBuffered);
	}
	
	[RPC]
	public void checkModyfiersRPC()
	{
		this.getCurrentPCC().checkModyfiers();
	}

	[RPC]
	public void addPassEvent()
	{
		GameEventType ge = new PassType();
		addGameEvent(ge, "");
		nbActionPlayed = 0;
		changeGameEvents();
		fillTimeline();
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
		form.AddField("myform_gametype", ApplicationModel.gameType);		

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

	private void initGrid()
	{
		print("J'initialise le terrain de jeu");

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
		print("J'add " + loginName);
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
		
		if (ApplicationModel.username == loginName)
		{
			if (this.isFirstPlayer)
			{
				for (int i = 0; i < this.nbFreeStartRows; i++)
				{
					for (int j = 0; j < this.boardWidth; j++)
					{
						this.tiles [j, i].GetComponent<TileController>().setDestination(true);
					}
				}
			} else
			{
				for (int i = this.boardHeight-1; i > this.boardHeight-1-this.nbFreeStartRows; i--)
				{
					for (int j = 0; j < this.boardWidth; j++)
					{
						this.tiles [j, i].GetComponent<TileController>().setDestination(true);
					}
				}
			}
		}
	}
				
	public void resetDestinations()
	{
		if (isDestinationDrawn)
		{
			for (int i = 0; i < this.boardHeight; i++)
			{
				for (int j = 0; j < this.boardWidth; j++)
				{
					this.tiles [j, i].GetComponent<TileController>().setDestination(false);
				}
			}
			this.isDestinationDrawn = false;
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
		if (isFirstP == this.isFirstPlayer)
		{
			this.myDeck = deck;
		}

		for (int i = 0; i < 5; i++)
		{
			this.playingCards [debut + i] = (GameObject)Instantiate(this.playingCard);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setStyles((isFirstP == this.isFirstPlayer));
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setCard(deck.Cards [i]);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setIDCharacter(debut + i);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().setTile(new Tile(i, hauteur), tiles [i, hauteur].GetComponent<TileController>().tileView.tileVM.position, !isFirstP);
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().resize();
			this.tiles [i, hauteur].GetComponent<TileController>().characterID = debut + i;
			this.playingCards [debut + i].GetComponentInChildren<PlayingCardController>().show();
		}
		yield break;
	}

	public void playerReady()
	{
		photonView.RPC("playerReadyRPC", PhotonTargets.AllBuffered, this.isFirstPlayer);
	}

	public void StartFight()
	{
		if (this.isFirstPlayer)
		{
			this.sortAllCards();
			photonView.RPC("timeRunsOut", PhotonTargets.AllBuffered, timerTurn);
			this.findNextPlayer();
		}
	}
	
	public void reloadDestinationTiles()
	{
		if (this.isFirstPlayer == (currentPlayingCard < 5))
		{
			resetDestinations();
			this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile.setNeighbours(this.getCharacterTilesArray(), this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.GetMove());
			this.setDestinations(currentPlayingCard);
		}
	}
	
	[RPC]
	public void playerReadyRPC(bool isFirst)
	{
		if (isFirst == this.isFirstPlayer)
		{
			this.gameView.gameScreenVM.startMyPlayer();
		} else
		{
			this.gameView.gameScreenVM.startOtherPlayer();
		}

		if (this.isFirstPlayer)
		{
			for (int i = 0; i < this.nbFreeStartRows; i++)
			{
				for (int j = 0; j < this.boardWidth; j++)
				{
					this.tiles [j, i].GetComponent<TileController>().setDestination(true);
				}
			}
		} else
		{
			for (int i = this.boardHeight-1; i > this.boardHeight-1-this.nbFreeStartRows; i--)
			{
				for (int j = 0; j < this.boardWidth; j++)
				{
					this.tiles [j, i].GetComponent<TileController>().setDestination(true);
				}
			}
		}
		
		nbPlayersReadyToFight++;
		
		if (nbPlayersReadyToFight == 2)
		{
			this.gameView.gameScreenVM.toDisplayStartWindows = false;
			this.displayPopUpMessage("Le combat commence", 2);
			this.resetDestinations();	
			this.nbTurns = 1;
			
			if (this.currentPlayingCard != -1)
			{
				this.hideActivatedPlayingCard();
			}
			
			if (this.isFirstPlayer)
			{
				this.StartFight();
			}
		}
	}

	public void reloadSortedList()
	{
		if (this.isFirstPlayer)
		{
			sortAllCards();
			photonView.RPC("reloadTimeline", PhotonTargets.AllBuffered);

		}
	}

	public void sortAllCards()
	{
		List <int> cardsToRank = new List<int>();
		List <int> quicknessesToRank = new List<int>();
		int indexToRank;

		for (int i = 0; i < 10; i++)
		{
			cardsToRank.Add(i);	
			quicknessesToRank.Add(this.playingCards [i].GetComponentInChildren<PlayingCardController>().card.GetSpeed());
		}
		for (int i = 0; i < 10; i++)
		{
			indexToRank = this.FindMaxQuicknessIndex(quicknessesToRank);
			print("j'add " + cardsToRank [indexToRank] + " au rang " + i + " avec la vitesse " + quicknessesToRank [indexToRank]);

			quicknessesToRank.RemoveAt(indexToRank);
			photonView.RPC("addRankedCharacter", PhotonTargets.AllBuffered, cardsToRank [indexToRank], i);
			cardsToRank.RemoveAt(indexToRank);
		}
	}

	public int FindMaxQuicknessIndex(List<int> list)
	{
		if (list.Count == 0)
		{
			throw new InvalidOperationException("Liste vide !");
		}
		int max = -1;
		int index = -1;
		for (int i = 0; i < list.Count; i++)
		{
			if (list [i] > max)
			{
				max = list [i];
				index = i;
			}
		}
		return index;
	}
	
	[RPC]
	public void addRankedCharacter(int id, int rank)
	{
		if (rank == 0)
		{
			this.rankedPlayingCardsID = new int[10];
		}
		this.rankedPlayingCardsID [rank] = id;
		if (rank == 9)
		{
			initGameEvents();
		}
	}

	[RPC]
	public void moveCharacterRPC(int x, int y, int c, bool isFirstP, bool isSwap)
	{
		if (!isSwap)
		{
			this.tiles [this.playingCards [c].GetComponentInChildren<PlayingCardController>().tile.x, this.playingCards [c].GetComponentInChildren<PlayingCardController>().tile.y].GetComponent<TileController>().characterID = -1;
		}

		getTile(x, y).characterID = c;
		getTile(x, y).statModifierActive = true;

		getPCC(c).changeTile(new Tile(x, y), getTile(x, y).getPosition());
		getPCC(c).card.TileModifiers.Clear();
		loadTileModifierToCharacter(x, y);

		if (this.isFirstPlayer == isFirstP && nbTurns != 0)
		{
			this.playingCards [currentPlayingCard].GetComponentInChildren<PlayingCardController>().tile.setNeighbours(this.getCharacterTilesArray(), this.playingCards [this.currentPlayingCard].GetComponentInChildren<PlayingCardController>().card.GetMove());
			this.isDragging = false;
			this.resetDestinations();
			if (this.clickedPlayingCard != this.currentPlayingCard)
			{
				this.showMyPlayingSkills(this.currentPlayingCard);
			} else
			{
				this.updateStatusMySkills(this.currentPlayingCard);
			}
			this.tiles [x, y].GetComponentInChildren<TileController>().checkTrap(this.currentPlayingCard);
			if (this.playindCardHasPlayed)
			{
				this.gameskills [1].launch();
			}
		}
		
		if (!photonView.isMine)
		{
			displaySkillEffect(c, "se déplace", 2, 2);
		}
		playingCardHasMoved = true;
	}

	[RPC]
	public void inflictDamageRPC(int targetCharacter, bool isFisrtPlayerCharacter)
	{
		if (!photonView.isMine)
		{
			displayPopUpMessage(this.playingCards [targetCharacter].GetComponentInChildren<PlayingCardController>().card.Title + " attaque", 2f);
		}
	}

	[RPC]
	public void timeRunsOut(float time)
	{
		startTurn = true;
		gameView.gameScreenVM.timer = time;
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
		StartCoroutine(this.quitGame());
	}

	public IEnumerator quitGame()
	{
		if (isFirstPlayer)
		{
			yield return (StartCoroutine(this.sendStat(this.users [1].Username, this.users [0].Username)));
		} else
		{
			yield return (StartCoroutine(this.sendStat(this.users [0].Username, this.users [1].Username)));
		}
		photonView.RPC("quitGameRPC", PhotonTargets.AllBuffered, this.isFirstPlayer);
	}
	[RPC]
	public void quitGameRPC(bool isFirstP)
	{
		gameView.gameScreenVM.toDisplayGameScreen = false;
		if (isFirstP == this.isFirstPlayer)
		{
			//print("J'ai perdu comme un gros con");
			EndSceneController.instance.displayEndScene(false);
		} else
		{
			//print("Mon adversaire a lachement abandonné comme une merde");
			EndSceneController.instance.displayEndScene(true);
		}
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

	void fillTimeline()
	{
		int rankedPlayingCardID = 0;
		bool nextChara = true;
		bool findCharactersHaveNoAlreadyPlayed = false;

		if (nextCharacterPositionTimeline < 7)
		{
			findCharactersHaveNoAlreadyPlayed = true;
		}

		while (nextChara)
		{
			rankedPlayingCardID = rankedPlayingCardsID [nextCharacterPositionTimeline];
			PlayingCardController pcc = this.playingCards [rankedPlayingCardID] .GetComponentInChildren<PlayingCardController>();
			if (!pcc.isDead && (!pcc.hasPlayed && !findCharactersHaveNoAlreadyPlayed || findCharactersHaveNoAlreadyPlayed))
			{
				nextChara = false;
			}
			if (++nextCharacterPositionTimeline > 9)
			{
				nextCharacterPositionTimeline = 0;
				findCharactersHaveNoAlreadyPlayed = true;
			}
		}
		addCardEvent(rankedPlayingCardID, 0);
	}

	[RPC]
	public void reloadTimeline()
	{
		bool findCharactersHaveNoAlreadyPlayed = false;
		nextCharacterPositionTimeline = 0;
		for (int i = 5; i >= 0; i--)
		{
			int rankedPlayingCardID = 0;
			bool nextChara = true;
			while (nextChara)
			{
				rankedPlayingCardID = rankedPlayingCardsID [nextCharacterPositionTimeline];
				PlayingCardController pcc = this.playingCards [rankedPlayingCardID] .GetComponentInChildren<PlayingCardController>();
				
				if (!pcc.isDead && (!pcc.hasPlayed && !findCharactersHaveNoAlreadyPlayed || findCharactersHaveNoAlreadyPlayed))
				{
					nextChara = false;
				}
				if (++nextCharacterPositionTimeline > 9)
				{
					nextCharacterPositionTimeline = 0;
					findCharactersHaveNoAlreadyPlayed = true;
				}
			}
			addCardEvent(rankedPlayingCardID, i);
		}

	}

	void addCardEvent(int idCharacter, int position)
	{
		GameObject go = gameEvents [position];
		PlayingCardController pcc = playingCards [idCharacter].GetComponent<PlayingCardController>();
		go.GetComponent<GameEventController>().setCharacterName(pcc.card.Title);
		Texture t2 = playingCards [idCharacter].GetComponent<PlayingCardController>().getPicture();
		Texture2D temp = getImageResized(t2 as Texture2D);
		go.GetComponent<GameEventController>().IDCharacter = idCharacter;
		go.GetComponent<GameEventController>().setAction("");
		go.GetComponent<GameEventController>().setArt(temp);
		go.GetComponent<GameEventController>().setBorder(isThisCharacterMine(idCharacter));
		go.GetComponent<GameEventController>().gameEventView.show();
		go.renderer.enabled = true;
	}

	void initGameEvents()
	{
		GameObject go;
		int i = 1;
		while (gameEvents.Count < eventMax)
		{
			go = (GameObject)Instantiate(gameEvent);
			gameEvents.Add(go);
			go.GetComponent<GameEventController>().setScreenPosition(gameEvents.Count, boardWidth, boardHeight, tileScale);
			go.GetComponent<GameEventController>().setBorder(0);
			if (i < 7)
			{
				GameObject child = (GameObject)Instantiate(go.GetComponent<GameEventController>().transparentImage);
				child.name = "TransparentEvent";
				child.transform.parent = go.transform;
				child.transform.localPosition = new Vector3(0f, 0f, -5f);
				child.transform.localScale = new Vector3(0.9f, 0.9f, 10f);
			}
			if (i == 6)
			{
				go.transform.localScale = new Vector3(0.95f, 0.95f, 10f);
			} 

			if (i > 6)
			{
				GameObject child = (GameObject)Instantiate(go.GetComponent<GameEventController>().darkImage);
				child.name = "DarkEvent";
				child.transform.parent = go.transform;
				child.transform.localPosition = new Vector3(0f, 0f, -5f);
				child.transform.localScale = new Vector3(0.9f, 0.9f, 10f);
				child.renderer.enabled = false;
			}
			go.renderer.enabled = false;
			i++;
		}
		for (i = 0; i < 6; i++)
		{
			addCardEvent(rankedPlayingCardsID [5 - i], i);
		}
		nextCharacterPositionTimeline = 6;
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
	
	public Card getCurrentCard()
	{
		return this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().card;
	}

	void changeGameEvents()
	{
		for (int i = gameEvents.Count - 1; i > 0; i--)
		{
			int id = gameEvents [i - 1].GetComponent<GameEventController>().IDCharacter;
			string title = gameEvents [i - 1].GetComponent<GameEventController>().getCharacterName();
			string action = gameEvents [i - 1].GetComponent<GameEventController>().getAction();
			GameObject[] movement = gameEvents [i - 1].GetComponent<GameEventController>().getMovement();
			Texture2D t2 = gameEvents [i - 1].GetComponent<GameEventController>().getArt();
			if (title != "" && i > 5)
			{
				gameEvents [i].renderer.enabled = true;
				gameEvents [i].transform.Find("DarkEvent").renderer.enabled = true;
			}

			gameEvents [i].GetComponent<GameEventController>().IDCharacter = id;
			gameEvents [i].GetComponent<GameEventController>().setCharacterName(title);
			gameEvents [i].GetComponent<GameEventController>().setAction(action);
			gameEvents [i].GetComponent<GameEventController>().setMovement(movement [0], movement [1]);
			gameEvents [i].GetComponent<GameEventController>().setArt(t2);
			if (i < 6)
			{
				gameEvents [i].GetComponent<GameEventController>().setBorder(isThisCharacterMine(id));
			}

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

	public void disconnect()
	{
		PhotonNetwork.Disconnect();
	}

	int isThisCharacterMine(int id)
	{
		return (isFirstPlayer == (id < 5)) ? 1 : -1;
	}

	void initSkills()
	{
		this.gameskills = new GameSkill[52];
		this.gameskills [0] = new Attack();
		this.gameskills [1] = new Pass();
		this.gameskills [2] = new GameSkill();
		this.gameskills [3] = new Reflexe();
		this.gameskills [4] = new Apathie();
		this.gameskills [5] = new Renforcement();
		this.gameskills [6] = new Sape();
		this.gameskills [7] = new Lenteur();
		this.gameskills [8] = new Rapidite();
		this.gameskills [9] = new Dissipation();
		this.gameskills [10] = new TirALarc();
		this.gameskills [11] = new Furtivite();
		this.gameskills [12] = new Assassinat();
		this.gameskills [13] = new AttaquePrecise();
		this.gameskills [14] = new AttaqueRapide();
		this.gameskills [15] = new PiegeALoups();
		this.gameskills [16] = new Agilite();
		this.gameskills [17] = new Paralyser();
		this.gameskills [18] = new AttaqueFrontale();
		this.gameskills [19] = new AttaqueCirculaire();
		this.gameskills [20] = new Frenesie();
		this.gameskills [21] = new Rugissement();
		this.gameskills [22] = new Terreur();
		this.gameskills [23] = new SacrificeTribal();
		this.gameskills [24] = new RayonEnergie();
		this.gameskills [25] = new BouleEnergie();
		this.gameskills [26] = new TempeteEnergie();
		this.gameskills [27] = new EnergieQuantique();
		this.gameskills [28] = new PuissanceIncontrolable();
		this.gameskills [29] = new GameSkill();
		this.gameskills [30] = new GameSkill();
		this.gameskills [31] = new GameSkill();
		this.gameskills [32] = new GameSkill();
		this.gameskills [33] = new GameSkill();
		this.gameskills [34] = new GameSkill();
		this.gameskills [35] = new GameSkill();
		this.gameskills [36] = new GameSkill();
		this.gameskills [37] = new GameSkill();
		this.gameskills [38] = new GameSkill();
		this.gameskills [39] = new GameSkill();
		this.gameskills [40] = new GameSkill();
		this.gameskills [41] = new GameSkill();
		this.gameskills [42] = new TempleSacre();
		this.gameskills [43] = new ForetDeLianes();
		this.gameskills [44] = new SablesMouvants();
		this.gameskills [45] = new GameSkill();
		this.gameskills [46] = new FontaineDeJouvence();
		this.gameskills [47] = new GameSkill();
		this.gameskills [48] = new GameSkill();
		this.gameskills [49] = new GameSkill();
		this.gameskills [50] = new GameSkill();
		this.gameskills [51] = new GameSkill();
	}

	public PlayingCardController getCurrentPCC()
	{
		return this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>();
	}

	public PlayingCardController getPCC(int id)
	{
		return this.playingCards [id].GetComponent<PlayingCardController>();
	}

	public TileController getTile(int x, int y)
	{
		return this.tiles [x, y].GetComponent<TileController>();
	}

	public Card getCard(int id)
	{
		return this.playingCards [id].GetComponent<PlayingCardController>().card;
	}

	public Skill getCurrentSkill()
	{
		return this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().card.Skills [this.clickedSkill];
	}

	public int getCurrentSkillID()
	{
		if (this.clickedSkill == 4)
		{
			return 0;
		} else if (this.clickedSkill == 5)
		{
			return 1;
		}
		return this.playingCards [this.currentPlayingCard].GetComponent<PlayingCardController>().card.Skills [this.clickedSkill].Id;
	}

	public void reloadCard(int id)
	{
		this.playingCards [id].GetComponent<PlayingCardController>().show();
	}


	public void play()
	{
		if (this.getCurrentPCC().cannotBeTargeted != -1 && this.getCurrentSkillID() != 11)
		{
			photonView.RPC("removeFurtivityRPC", PhotonTargets.AllBuffered, this.currentPlayingCard);
		}
		this.isRunningSkill = false;
		this.playindCardHasPlayed = true;
		
		if (this.clickedPlayingCard != this.currentPlayingCard && this.clickedPlayingCard != -1)
		{
			this.showMyPlayingSkills(this.currentPlayingCard);
		} else
		{
			this.updateStatusMySkills(this.currentPlayingCard);
		}
		
		if (this.playingCardHasMoved)
		{
			this.gameskills [1].launch();
		}
	}
	
	public void play(string s)
	{
		if (this.getCurrentPCC().cannotBeTargeted != -1 && this.getCurrentSkillID() != 11)
		{
			photonView.RPC("removeFurtivityRPC", PhotonTargets.AllBuffered, this.currentPlayingCard);
		}
		this.isRunningSkill = false;
		this.playindCardHasPlayed = true;
		
		if (this.clickedPlayingCard != this.currentPlayingCard && this.clickedPlayingCard != -1)
		{
			this.showMyPlayingSkills(this.currentPlayingCard);
		} else
		{
			this.updateStatusMySkills(this.currentPlayingCard);
		}
		
		if (this.playingCardHasMoved)
		{
			this.gameskills [1].launch();
		}
	}
	
	[RPC]
	public void playRPC(string message)
	{
		this.displayPopUpMessage(message, 3);
	}
	
	public void displaySkillEffect(int target, string s, float timer, int colorindex)
	{
		photonView.RPC("displaySkillEffectRPC", PhotonTargets.AllBuffered, target, s, timer, colorindex);
	}
	
	[RPC]
	public void displaySkillEffectRPC(int target, string s, float timer, int colorindex)
	{
		this.getPCC(target).addSkillResult(s, timer, colorindex);
	}
	
	[RPC]
	public void removeFurtivityRPC(int id)
	{
		this.getPCC(id).removeFurtivity();
	}
	
	public void updateTimeline()
	{
		this.sortAllCards();
	}
	
	public void emptyTile(int x, int y)
	{
		this.tiles [x, y].GetComponent<TileController>().characterID = -1;
		this.areMyHeroesDead();
	}
	
	public void areMyHeroesDead()
	{
		bool areTheyAllDead = true;
		int debut;
		int i = 0;
		if (this.isFirstPlayer)
		{
			debut = 0;
		} else
		{
			debut = 5;
		}
		while (areTheyAllDead && i<5)
		{
			if (!this.getPCC(debut + i).isDead)
			{
				areTheyAllDead = false;
			}
			i++;
		}
		if (areTheyAllDead)
		{
			StartCoroutine(this.quitGame());
		}
	}
	
	public void addModifier(int target, int type)
	{ 
		photonView.RPC("addModifierRPC", PhotonTargets.AllBuffered, target, 0, type, 0, -1);
	}
	
	public void addModifier(int target, int amount, int type, int stat)
	{ 
		photonView.RPC("addModifierRPC", PhotonTargets.AllBuffered, target, amount, type, stat, -1);
	}
	
	public void addModifier(int target, int amount, int type, int stat, int duration)
	{ 
		photonView.RPC("addModifierRPC", PhotonTargets.AllBuffered, target, amount, type, stat, duration);
	}
	
	[RPC]
	public void addModifierRPC(int target, int amount, int type, int stat, int duration)
	{
		this.getCard(target).modifiers.Add(new StatModifier(amount, (ModifierType)type, (ModifierStat)stat, duration));
		if (stat == (int)ModifierStat.Stat_Dommage)
		{
			
			if (this.getCard(target).GetLife() <= 0)
			{
				StartCoroutine(this.kill(target));
				this.reloadTimeline();
			}
			this.reloadCard(target);
		} else if (stat == (int)ModifierStat.Stat_Move || stat == (int)ModifierStat.Stat_Attack)
		{
			this.reloadCard(target);
		}
	}
	
	public IEnumerator kill(int target)
	{
		yield return new WaitForSeconds(2f);
		this.getPCC(target).kill();
	}

	public void addTileModifier(int modifierType, int amount, int tileX, int tileY)
	{
		photonView.RPC("addTileModifierRPC", PhotonTargets.AllBuffered, modifierType, amount, tileX, tileY, this.isFirstPlayer);
	}	

	[RPC]
	public void addTileModifierRPC(int modifierType, int amount, int tileX, int tileY, bool isFirstP)
	{
		int c = getTile(tileX, tileY).characterID;
		if (c != -1)
		{
			getPCC(c).card.TileModifiers.Clear();
		}
		switch (modifierType)
		{
			case 0:
				this.tiles [tileX, tileY].GetComponent<TileController>().addTemple(amount);
				loadTileModifierToCharacter(tileX, tileY);
				break;
			case 1:
				TileController tileController1 = this.tiles [tileX, tileY].GetComponent<TileController>();
				tileController1.addForetIcon(amount);
				loadTileModifierToCharacter(tileX, tileY);
				foreach (Tile til in tileController1.tile.getImmediateNeighbouringTiles())
				{
					this.getTile(til.x, til.y).addForetIcon(amount);
					loadTileModifierToCharacter(til.x, til.y);
				}
				break;
			case 2: 
				getTile(tileX, tileY).addSable((this.isFirstPlayer == isFirstP));
				loadTileModifierToCharacter(tileX, tileY);
				break;
			case 3:
				getTile(tileX, tileY).addFontaine(amount);
				loadTileModifierToCharacter(tileX, tileY);
				break;
			default:
				break;
		}
	}
	public void loadTileModifierToCharacter(int x, int y)
	{
		TileController tileController = this.getTile(x, y).GetComponent<TileController>();
		if (tileController.characterID != -1)
		{
			if (tileController.statModifierActive == true)
			{
				foreach (StatModifier sm in tileController.tile.StatModifier)
				{
					this.getPCC(tileController.characterID).card.TileModifiers.Add(sm);
				}
				if (tileController.tileModification == TileModification.Sables_Mouvants)
				{
					tileController.tileView.tileVM.toDisplayIcon = true;
					playRPC(this.getPCC(tileController.characterID).card.Title + " est pris dans un sable mouvant");
				}
				if (!tileController.statModifierEachTurn)
				{
					tileController.statModifierActive = false;
				}
				reloadCard(tileController.characterID);
			}
		}
	}
	
	public void addTrap(int tileX, int tileY, int type, int amount)
	{ 
		photonView.RPC("addTrapRPC", PhotonTargets.AllBuffered, tileX, tileY, type, amount, this.isFirstPlayer);
	}
	
	[RPC]
	public void addTrapRPC(int tileX, int tileY, int type, int amount, bool isFirstP)
	{
		this.tiles [tileX, tileY].GetComponent<TileController>().setTrap(new Trap(amount, type, (this.isFirstPlayer == isFirstP)));
	}
	
	public void setParalyzed(int target, int duration)
	{
		photonView.RPC("setParalyzedRPC", PhotonTargets.AllBuffered, target, duration);
	}
	
	[RPC]
	public void setParalyzedRPC(int target, int duration)
	{
		this.getPCC(target).setParalyzed(duration);
	}
	
	public void setCannotBeTargeted()
	{
		photonView.RPC("setCannotBeTargetedRPC", PhotonTargets.AllBuffered, this.currentPlayingCard);
	}
	
	[RPC]
	public void setCannotBeTargetedRPC(int target)
	{
		this.getPCC(target).setCannotBeTargeted("Invisible", "Le héros ne peut pas etre ciblé tant qu'il n'a pas activé une de ses compétences");
	}
	
	public void setEsquive(int amount)
	{
		photonView.RPC("setEsquiveRPC", PhotonTargets.AllBuffered, this.currentPlayingCard, amount);
	}
	
	[RPC]
	public void setEsquiveRPC(int target, int amount)
	{
		this.getPCC(target).changeEsquive("Esquive", "Le héros possède " + amount + " % de chances d'esquiver les dégats");
		this.addModifier(target, amount, (int)ModifierType.Type_EsquivePercentage, -1);
	}
	
	public void removeTrap(Tile t)
	{
		photonView.RPC("removeTrapRPC", PhotonTargets.AllBuffered, t.x, t.y);
	}
	
	[RPC]
	public void removeTrapRPC(int x, int y)
	{
		this.tiles [x, y].GetComponent<TileController>().removeTrap();
	}
	
	public void useSkill()
	{
		photonView.RPC("useSkillRPC", PhotonTargets.AllBuffered, this.currentPlayingCard, this.clickedSkill);
	}
	
	[RPC]
	public void useSkillRPC(int target, int nbSkill)
	{
		this.getCard(target).Skills [nbSkill].lowerNbLeft();
	}
}


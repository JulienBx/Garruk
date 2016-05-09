using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using TMPro ;

public class GameView : MonoBehaviour
{
	//URL pour les appels en BDD
	string URLStat = ApplicationModel.host + "updateResult.php";
	
	public static GameView instance;
	public GameObject loadingScreenObject;
	public GameObject tileModel ;
	public GameObject verticalBorderModel;
	public GameObject horizontalBorderModel;
	public GameObject playingCardModel;
	public Sprite[] sprites;
	public Sprite[] factionSprites;
	public Sprite[] skillSprites;
	public Sprite[] skillTypeSprites;
	public int boardWidth ;
	public int boardHeight ;
	int nbCardsPerPlayer ;
	public int nbFreeRowsAtBeginning ;

	bool isLoadingScreenDisplayed = false ;

	GameObject serverController;
	GameObject loadingScreen;
	GameObject[,] tiles ;
	GameObject myHoveredRPC ;
	GameObject hisHoveredRPC ;
	GameObject[] verticalBorders ;
	GameObject[] horizontalBorders ;
	GameObject[] playingCards ;
	public GameObject timerFront;
	public GameObject choicePopUp;
	GameObject validationSkill;
	TimelineController timeline;
	public GameTutoController gameTutoController;

	GameObject tutorial;
	public GameObject SB;
	GameObject interlude;
	public GameObject passZone;
	public GameObject skillZone;
	
	int heightScreen = -1;
	int widthScreen = -1;
	
	int currentPlayingCard = -1;
	bool isFirstPlayer = false;
	
	public Deck myDeck ;
	int nbPlayersReadyToFight = 0 ;
	
	public bool hasFightStarted = false ;
	bool isBackgroundLoaded ;
	
	public float realwidth ;
	
	public bool isDisplayedMyDestination;
	public int runningSkill ;
	
	public List<Tile> anims ;
	public List<Tile> skillEffects ;
	public List<Tile> targets ;
	public List<int> deads ;
	TargetTileHandler targetTileHandler ;
	
	public bool amIReadyToFight= false ;
	public bool isHeReadyToFight= false ;

	public bool blockFury ;
	bool toPassDead = false ;

	public bool isFreezed = false ;
	public bool isDisplayedPopUp = false ;
	public int hoveringZone = -1 ;

	public int draggingCard ;
	public int draggingSkillButton ;
	public int nbTurns ;
	int numberDeckLoaded ;

	int nbCards = 8 ;
	bool isFirstPlayerStarting ;

	public bool isMobile;
	public float stepButton;
	public float timeDragging = -1 ;
	public int clickedCharacterId=-1;

	List<int> orderCards ; 
	int meteoritesCounter = 8 ; 
	int meteoritesStep = 1 ; 
	int lastPlayingCard = -1 ;

	public int sequenceID = 0 ;
	public bool toLaunchCardCreation = false ; 
	public bool isGameskillOK;

	int indexPlayer ; 
	public float tileScale;

	public TimerController myTimer, hisTimer;
	public ArtificialIntelligence IA ;

	bool areTilesLoaded ; 
	BackOfficeController backOfficeController ;
	public bool isChangingTurn ;

	void Awake()
	{

	}

	void Start()
	{
		this.init();
	}

	public void init(){
		instance = this;		
		this.isChangingTurn = false;
		areTilesLoaded = false ;
		this.numberDeckLoaded = 0 ;
		this.initializeServerController();
		this.displayLoadingScreen ();
		this.tiles = new GameObject[this.boardWidth, this.boardHeight];
		this.playingCards = new GameObject[100];
		this.verticalBorders = new GameObject[this.boardWidth+1];
		this.horizontalBorders = new GameObject[this.boardHeight+1];
		this.myHoveredRPC = GameObject.Find("MyHoveredPlayingCard");
		this.hisHoveredRPC = GameObject.Find("HisHoveredPlayingCard");
		this.toLaunchCardCreation = false ;
		this.isGameskillOK = false ;
		this.SB = GameObject.Find("SB");
		this.interlude = GameObject.Find("Interlude");
		this.passZone = GameObject.Find("PassZone");
		this.skillZone = GameObject.Find("SkillZone");
		this.choicePopUp = GameObject.Find("PopUpChoice");
		this.timeline = GameObject.Find("Timeline").GetComponent<TimelineController>();
		this.hisTimer = GameObject.Find("HisPlayerName").transform.FindChild("Time").GetComponent<TimerController>();
		this.myTimer = GameObject.Find("MyPlayerBox").transform.FindChild("Time").GetComponent<TimerController>();
		this.timerFront = GameObject.Find("TimerFront");
		this.validationSkill = GameObject.Find("ValidationAutoSkill");
		this.validationSkill.GetComponent<SkillValidationController>().show(false);
		this.gameTutoController = GameObject.Find("HelpController").GetComponent<GameTutoController>();
		this.SB.GetComponent<StartButtonController>().show(false);
		this.setMyPlayerName(ApplicationModel.myPlayerName);
		this.setHisPlayerName(ApplicationModel.hisPlayerName);
		this.isFirstPlayer = ApplicationModel.player.IsFirstPlayer;
		this.runningSkill=-1;
		this.isGameskillOK = true ;
		this.createBackground();
		this.targets = new List<Tile>();
		this.skillEffects = new List<Tile>();
		this.anims = new List<Tile>();
		this.deads = new List<int>();
		this.orderCards = new List<int>();
		if (this.isFirstPlayer)
		{
			if(ApplicationModel.player.ToLaunchGameTutorial){
				this.gameTutoController.initialize();
				this.gameTutoController.setCompanion("Bienvenue dans le simulateur de combat Alpha-B49 ! Mon nom est Mudo et je serai votre guide pour cette première bataille.", true, false, true, 0);
				this.gameTutoController.setBackground(true, new Rect(0f, 0f, 20f, 10f), 0f, 0f);
				this.gameTutoController.showSequence(true, true, false);
			}
			else{
				this.initGrid();
			}
		}
		this.hasFightStarted = false ;
		this.blockFury = false;
		draggingCard = -1 ;
		draggingSkillButton = -1 ;
		this.nbTurns = 0 ;
		this.isFirstPlayerStarting=true;
		this.indexPlayer = 0 ; 
		this.IA = GameObject.Find("Main Camera").GetComponent<ArtificialIntelligence>();
	}

	private void initializeServerController()
	{
		this.serverController = GameObject.Find ("ServerController");
		this.serverController.GetComponent<ServerController>().initialize();
	}

	public void displayLoadingScreen()
	{
		if(!isLoadingScreenDisplayed)
		{
			this.loadingScreen=Instantiate(this.loadingScreenObject) as GameObject;
			this.isLoadingScreenDisplayed=true;
		}
	}
	
	public void setMyPlayerName(string s){
		GameObject tempGO = GameObject.Find("MyPlayerName");
		tempGO.GetComponent<TextMeshPro>().text = s ;
	}
	
	public void setHisPlayerName(string s){
		GameObject tempGO = GameObject.Find("HisPlayerName");
		tempGO.GetComponent<TextMeshPro>().text = s ;
	}
	
	public PassController getPassZoneController(){
		return this.passZone.GetComponent<PassController>();
	}
	
	public SkillZoneController getSkillZoneController(){
		return this.skillZone.GetComponent<SkillZoneController>();
	}
	
	void initGrid()
	{
		bool isRock = false;
		List<Tile> rocks = new List<Tile>();
		Tile t = new Tile(0,0) ;

		if(ApplicationModel.player.ToLaunchGameTutorial){
			rocks.Add(new Tile(2,2));
			rocks.Add(new Tile(0,2));
			rocks.Add(new Tile(5,4));
		}
		else{
			int nbRocksToAdd = UnityEngine.Random.Range(3, 6);
			int compteurRocks = 0;
			bool isOk = true;
			while (compteurRocks<nbRocksToAdd)
			{
				isOk = false;
				while (!isOk)
				{
					t = GameView.instance.getRandomRock(1);
					isOk = true;
					for (int a = 0; a < rocks.Count && isOk; a++)
					{
						if (rocks [a].x == t.x && rocks [a].y == t.y)
						{
							isOk = false;
						}
					}
				}
				rocks.Add(t);
				compteurRocks++;
			}
		}
		
		for (int x = 0; x < this.boardWidth; x++)
		{
			for (int y = 0; y < this.boardHeight; y++)
			{
				isRock = false;
				for (int z = 0; z < rocks.Count && !isRock; z++)
				{
					if (rocks [z].x == x && rocks [z].y == y)
					{
						isRock = true;
					}
				}
				
				if (!isRock)
				{
					GameController.instance.createTile(x, y, 0);	
				}
				else{
					GameController.instance.createTile(x, y, 1);
				}
			}
		}
	}
	
	public void loadMyDeck()
	{
		if(ApplicationModel.player.ToLaunchGameTutorial){
			List<Skill> skills = new List<Skill>();
			skills.Add (new Skill("Lâche", 65, 1, 1, 2, 0, "", 0, 0));
			skills.Add (new Skill("Vitamines", 6, 1, 2, 6, 0, "", 0, 100));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			Card c1 = new Card(-1, "Flash", 35, 2, 0, 7, 16, skills);
			c1.deckOrder=0;
			GameCard g1 = new GameCard(c1);
			g1.LifeLevel=1;
			g1.AttackLevel=1;
			g1.PowerLevel=1;
			this.createPlayingCard(g1, true);
			print(this.getCard(0).Life);
			this.getPlayingCardController(0).updateLife(0);
			this.getPlayingCardController(0).updateAttack(0);

			skills = new List<Skill>();
			skills.Add (new Skill("Paladin", 73, 1, 1, 3, 0, "", 0, 0));
			skills.Add (new Skill("PistoSoin", 2, 1, 1, 6, 0, "", 0, 100));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			c1 = new Card(-1, "Arthur", 51, 1, 0, 3, 14, skills);
			c1.deckOrder=1;
			g1 = new GameCard(c1);
			g1.LifeLevel=2;
			g1.AttackLevel=3;
			g1.PowerLevel=1;
			this.createPlayingCard(g1, true);
			this.getPlayingCardController(1).updateLife(0);
			this.getPlayingCardController(1).updateAttack(0);

			
			skills = new List<Skill>();
			skills.Add (new Skill("Cuirassé", 70, 1, 1, 4, 0, "", 0, 0));
			skills.Add (new Skill("Attaque 360", 17, 1, 1, 8, 0, "", 0, 100));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			c1 = new Card(-1, "Psycho", 52, 2, 0, 3, 28, skills);
			c1.deckOrder=2;
			g1 = new GameCard(c1);
			g1.LifeLevel=2;
			g1.AttackLevel=1;
			g1.PowerLevel=1;
			this.createPlayingCard(g1, true);
			this.getPlayingCardController(2).updateLife(0);
			this.getPlayingCardController(2).updateAttack(0);


			skills = new List<Skill>();
			skills.Add (new Skill("Agile", 66, 1, 1, 2, 0, "", 0, 0));
			skills.Add (new Skill("Assassinat", 18, 1, 2, 10, 0, "", 0, 80));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			c1 = new Card(-1, "Slayer", 35, 2, 0, 3, 16, skills);
			c1.deckOrder=3;
			g1 = new GameCard(c1);
			g1.LifeLevel=1;
			g1.AttackLevel=1;
			g1.PowerLevel=1;
			this.createPlayingCard(g1, true);
			this.getPlayingCardController(3).updateLife(0);
			this.getPlayingCardController(3).updateAttack(0);

			skills = new List<Skill>();
			skills.Add (new Skill("Tank", 70, 1, 1, 2, 0, "", 0, 0));
			skills.Add (new Skill("Attaque 360", 17, 1, 2, 6, 0, "", 0, 80));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			c1 = new Card(-1, "Brute", 26, 2, 0, 2, 17, skills);
			c1.deckOrder=0;
			g1 = new GameCard(c1);
			g1.LifeLevel=2;
			g1.AttackLevel=1;
			g1.PowerLevel=1;
			this.createPlayingCard(g1, false);
			this.getPlayingCardController(4).changeTile(new Tile(1,6), this.tiles[1,6].GetComponentInChildren<TileController>().getPosition());
			this.tiles[4,7].GetComponentInChildren<TileController>().setCharacterID(-1);
			this.tiles[1,6].GetComponentInChildren<TileController>().setCharacterID(4);

			skills = new List<Skill>();
			skills.Add (new Skill("Leader", 76, 1, 1, 3, 0, "", 0, 0));
			skills.Add (new Skill("PistoSoin", 2, 1, 1, 1, 0, "", 0, 80));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			c1 = new Card(-1, "Flash", 14, 1, 0, 6, 11, skills);
			c1.deckOrder=1;
			g1 = new GameCard(c1);
			g1.LifeLevel=2;
			g1.AttackLevel=3;
			g1.PowerLevel=1;
			this.createPlayingCard(g1, false);
			this.getPlayingCardController(5).changeTile(new Tile(3,6), this.tiles[3,6].GetComponentInChildren<TileController>().getPosition());
			this.tiles[3,7].GetComponentInChildren<TileController>().setCharacterID(-1);
			this.tiles[3,6].GetComponentInChildren<TileController>().setCharacterID(5);

			skills = new List<Skill>();
			skills.Add (new Skill("Rapide", 71, 1, 1, 4, 0, "", 0, 0));
			skills.Add (new Skill("Massue", 63, 1, 1, 1, 0, "", 0, 100));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			c1 = new Card(-1, "Alien", 19, 2, 0, 3, 21, skills);
			c1.deckOrder=2;
			g1 = new GameCard(c1);
			g1.LifeLevel=2;
			g1.AttackLevel=1;
			g1.PowerLevel=1;
			this.createPlayingCard(g1, false);
			
			skills = new List<Skill>();
			skills.Add (new Skill("Tank", 70, 1, 1, 2, 0, "", 0, 0));
			skills.Add (new Skill("Attaque 360", 17, 1, 2, 6, 0, "", 0, 80));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			c1 = new Card(-1, "Tank", 24, 2, 0, 2, 17, skills);
			c1.deckOrder=3;
			g1 = new GameCard(c1);
			g1.LifeLevel=2;
			g1.AttackLevel=1;
			g1.PowerLevel=1;
			this.createPlayingCard(g1, false);

			this.numberDeckLoaded = 2 ; 
			this.setInitialDestinations(this.isFirstPlayer);
			this.showStartButton();
		}
		else if(ApplicationModel.player.ToLaunchGameIA){
			this.loadDeck(ApplicationModel.player.MyDeck, this.isFirstPlayer);
			this.createPlayingCard(ApplicationModel.opponentDeck.getGameCard(0), false);
			this.createPlayingCard(ApplicationModel.opponentDeck.getGameCard(1), false);
			this.createPlayingCard(ApplicationModel.opponentDeck.getGameCard(2), false);
			this.createPlayingCard(ApplicationModel.opponentDeck.getGameCard(3), false);

			this.numberDeckLoaded = 2 ; 
			this.setInitialDestinations(this.isFirstPlayer);
			this.placeIACards();
			this.showStartButton();
		}
		else{
			this.loadDeck(ApplicationModel.player.MyDeck, this.isFirstPlayer);
			this.loadDeck(ApplicationModel.opponentDeck, !this.isFirstPlayer);
		}
		this.nbCards=8;
		this.myDeck = ApplicationModel.player.MyDeck;
		this.setInitialDestinations(this.isFirstPlayer);
		this.showStartButton();
	}
	
	public void createTile(int x, int y, int type)
	{
		this.tiles [x, y] = (GameObject)Instantiate(this.tileModel);
		this.tiles [x, y].GetComponent<TileController>().initTileController(new Tile(x,y), type);
		if(x==boardWidth-1&&y==boardHeight-1){
			areTilesLoaded = true ;
			if(!ApplicationModel.player.ToLaunchGameTutorial){
				this.toLaunchCardCreation = true ;
			}
		}
	}
	
	public void createPlayingCard(GameCard c, bool isFirstP)
	{
		int hauteur = 0 ;
		int baseIndex = 0 ;
		
		if (!isFirstP){
			hauteur = this.boardHeight-1 ;
			baseIndex = 4;
		}
		int index = baseIndex+c.deckOrder;
		this.playingCards[c.deckOrder+baseIndex] = (GameObject)Instantiate(this.playingCardModel);

		this.playingCards [index].GetComponentInChildren<PlayingCardController>().setCard(c, (isFirstP==isFirstPlayer), index);

		if (!isFirstP){
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().setTile(new Tile(4-c.deckOrder, hauteur));
			this.tiles [4-c.deckOrder, hauteur].GetComponent<TileController>().setCharacterID(index);
		}
		else{
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().setTile(new Tile(1+c.deckOrder, hauteur));
			this.tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().setCharacterID(index);
		}

		this.playingCards [index].GetComponentInChildren<PlayingCardController>().resize();

		if (isFirstP==isFirstPlayer){
			this.tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().setDestination(5);
		}

		this.playingCards [index].GetComponentInChildren<PlayingCardController>().show();

		this.playingCards [index].GetComponentInChildren<PlayingCardController>().checkPassiveSkills(isFirstP==this.isFirstPlayer);
		GameCard gc = this.getCard(index);
		GameSkills.instance.initialize();
		if(gc.isPiegeur() && this.isFirstPlayer){
			List<Tile> tiles2 = ((Piegeur)GameSkills.instance.getSkill(64)).getTiles(gc.getPassiveSkillLevel(), this.boardWidth, this.boardHeight, this.nbFreeRowsAtBeginning);
			for (int i = 0 ; i < tiles2.Count ; i++){
				GameController.instance.addPiegeurTrap(tiles2[i], 5+2*gc.getPassiveSkillLevel(), gc.isMine);
			}
		}
	}
	
	public void addTrap(Trap trap, Tile tile)
	{
		this.getTileController(tile.x, tile.y).setTrap(trap);
	}
	
	public void loadDeck(Deck deck, bool isFirstP){
		for (int i = 0; i < ApplicationModel.nbCardsByDeck; i++){
			this.createPlayingCard(deck.getGameCard(i), isFirstP);
		}
		this.numberDeckLoaded++;
		if(this.numberDeckLoaded==2){
			GameView.instance.hideLoadingScreen ();
		}
		
		if(this.numberDeckLoaded==2 || ApplicationModel.player.ToLaunchGameTutorial){
			int level;
			int attackValue ;
			int pvValue ;
			for(int i = 0 ; i < this.nbCards ; i++){
				if(this.getCard(i).isMine){
					if(this.getCard(i).isLeader()){
						level = this.getCard(i).getSkills()[0].Power;
						GameView.instance.getPlayingCardController(i).addDamagesModifyer(new Modifyer(Mathf.RoundToInt(this.getCard(i).GetTotalLife()/2f), -1, 23, base.name, 5+" dégats subis"), false, -1);
						for(int j = 0 ; j < this.nbCards ; j++){
							if(this.getCard(j).isMine && i!=j){
								attackValue = level+2;
								pvValue = 2*level+5;
								this.getPlayingCardController(j).addAttackModifyer(new Modifyer(attackValue, -1, 76, "Leader", ". Permanent"));
								this.getPlayingCardController(j).addPVModifyer(new Modifyer(pvValue, -1, 76, "Leader", ". Permanent"));
								this.getPlayingCardController(j).show();
								this.getPlayingCardController(j).updateLife(0);
								this.getPlayingCardController(j).updateAttack(0);

							}
						}
						if(!ApplicationModel.player.ToLaunchGameTutorial){
							GameView.instance.displaySkillEffect(i, "Leader\nrenforce les alliés", 1);	
							GameView.instance.addAnim(0,GameView.instance.getTile(i));
						}
					}
					if(this.getCard(i).isProtector()){
						level = this.getCard(i).getSkills()[0].Power*2+10;
						for(int j = 0 ; j < this.nbCards ; j++){
							if(this.getCard(j).isMine && i!=j){
								this.getPlayingCardController(j).addShieldModifyer(new Modifyer(level, -1, 111, base.name, "Bouclier "+level+"%"));
								this.getPlayingCardController(j).show();
							}
						}
						if(!ApplicationModel.player.ToLaunchGameTutorial){
							GameView.instance.displaySkillEffect(i, "Protecteur\nProtège les alliés adjacents", 1);	
							GameView.instance.addAnim(0,GameView.instance.getTile(i));
						}
					}
				}
				else{
					if(this.getCard(i).isLeader()){
						level = this.getCard(i).getSkills()[0].Power;
						GameView.instance.getPlayingCardController(i).addDamagesModifyer(new Modifyer(Mathf.RoundToInt(this.getCard(i).GetTotalLife()/2f), -1, 23, base.name, 5+" dégats subis"), false,-1);
	
						for(int j = 0 ; j < this.nbCards ; j++){
							if(!this.getCard(j).isMine && i!=j){
								attackValue = level+2;
								pvValue = 2*level+5;
								this.getPlayingCardController(j).addAttackModifyer(new Modifyer(attackValue, -1, 76, "Leader", ". Permanent"));
								this.getPlayingCardController(j).addPVModifyer(new Modifyer(pvValue, -1, 76, "Leader", ". Permanent"));
								this.getPlayingCardController(j).show();
								this.getPlayingCardController(j).updateLife(0);
								this.getPlayingCardController(j).updateAttack(0);
							}
						}
					}
					if(this.getCard(i).isProtector()){
						level = this.getCard(i).getSkills()[0].Power*2+10;
						for(int j = 0 ; j < this.nbCards ; j++){
							if(!this.getCard(j).isMine && i!=j){
								this.getPlayingCardController(j).addShieldModifyer(new Modifyer(level, -1, 111, base.name, "Bouclier "+level+"%"));
								this.getPlayingCardController(j).show();
								this.getPlayingCardController(j).updateLife(0);
								this.getPlayingCardController(j).updateAttack(0);
							}
						}
						if(!ApplicationModel.player.ToLaunchGameTutorial){
							GameView.instance.displaySkillEffect(i, "Protecteur\nProtège les alliés adjacents", 1);	
							GameView.instance.addAnim(0,GameView.instance.getTile(i));
						}
					}
				}
			}
		}
	}
	
	public void setInitialDestinations(bool isFirstP)
	{
		int debut = 0 ;
		if (!isFirstP){
			debut = this.boardHeight-this.nbFreeRowsAtBeginning;
		}
		for (int i = debut ; i < debut + this.nbFreeRowsAtBeginning; i++){
			for (int j = 0 ; j < this.boardWidth; j++){
				if (this.getTileController(j,i).canBeDestination()){
					this.getTileController(j,i).setDestination(0);
				}
			}
		}
	}
	
	public void showStartButton(){
		this.SB.GetComponent<StartButtonController>().show(true);
	}
	
	public void playerReady()
	{
		this.removeDestinations();
	}
	
	public void removeDestinations()
	{
		this.isDisplayedMyDestination = false ;
		for (int i = 0; i < this.boardWidth; i++)
		{
			for (int j = 0; j < this.boardHeight; j++)
			{
				if(this.getTileController(i,j).getIsDestination()!=-1 && this.getTileController(i,j).getIsDestination()!=5 && this.getTileController(i,j).getIsDestination()!=6){
					this.getTileController(i,j).removeDestination();
				}
			}
		}
	}
	
	public void playerReadyR(bool b){
		if(this.isFirstPlayer==b){
			this.amIReadyToFight = true;
		}
		else{
			this.isHeReadyToFight = true;
		}
		nbPlayersReadyToFight++;

		if (nbPlayersReadyToFight == 2 ||ApplicationModel.player.ToLaunchGameTutorial ||ApplicationModel.player.ToLaunchGameIA)
		{
			this.SB.GetComponent<StartButtonController>().show(false);
			this.removeDestinations();
			this.displayOpponentCards();
			this.updateCristoEater();
			if(this.isMobile){
				GameObject tempGO = GameObject.Find("MyPlayerBox");
				tempGO.transform.FindChild("MyPlayerName").GetComponent<MeshRenderer>().enabled = false ;

			    tempGO = GameObject.Find("HisPlayerName");
				tempGO.transform.GetComponent<MeshRenderer>().enabled = false ;
			}
			if(ApplicationModel.player.ToLaunchGameTutorial){
				if(this.sequenceID==5){
					this.gameTutoController.unslide();
				}
			}
			this.setNextPlayer(false);
		}
		else{
			this.isFirstPlayerStarting = b;
		}
	}

	public void authorizeDrag(){
		for (int i = 0 ; i < this.nbCards ; i++){
			if (this.getCard(i).isMine){
				this.getPlayingCardController(i).canBeDragged = true ;
			}
		}
	}
	
	public void displayOpponentCards(){
		for (int i = 0; i < this.nbCards; i++)
		{
			if(!this.getCard(i).isMine){
				this.playingCards [i].GetComponentInChildren<PlayingCardController>().display();
				this.playingCards [i].GetComponentInChildren<PlayingCardController>().show ();
				this.getTileController(i).GetComponent<TileController>().setDestination(6);
			}
		}
	}
	
	public void hoverCharacter(int characterID){
		if(!ApplicationModel.player.ToLaunchGameTutorial || sequenceID>6){
			if (this.getPlayingCardController(characterID).getIsMine()){	
				this.getMyHoveredCardController().setNextDisplayedCharacter(characterID, this.getCard(characterID));
			}
			else{
				this.getHisHoveredCardController().setNextDisplayedCharacter(characterID, this.getCard(characterID));
				if(ApplicationModel.player.ToLaunchGameTutorial){
					if(this.sequenceID==7){
						this.hitNextTutorial();
					}
				}
			}

			if(!ApplicationModel.player.ToLaunchGameTutorial || this.sequenceID>15){
				if(this.currentPlayingCard!=-1){
					if(!this.getCard(characterID).isMine && this.getCard(this.currentPlayingCard).isMine){
						this.getMyHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
					}
					else if(this.getCard(characterID).isMine && !this.getCard(this.currentPlayingCard).isMine){
						this.getHisHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
					}
				}
			}
			
			if(this.hasFightStarted && this.hoveringZone==-1 && this.runningSkill==-1){
				this.removeDestinations();
				if(!this.getCard(characterID).isSniperActive()){
					this.displayDestinations(characterID);
				}
			}
		}
	}

	public void mobileClick(int characterID){
		if (this.getPlayingCardController(characterID).getIsMine()){	
			this.getMyHoveredCardController().setNextDisplayedCharacter(characterID, this.getCard(characterID));
		}
		else{
			this.getHisHoveredCardController().setNextDisplayedCharacter(characterID, this.getCard(characterID));
		}
		
		if(this.hasFightStarted && this.hoveringZone==-1){
			this.removeDestinations();
			if(!this.getCard(characterID).isSniperActive()){
				this.displayDestinations(characterID);
			}
		}
	}
	
	public void clickCharacter(int characterID){
		Tile origine = this.getPlayingCardController(characterID).getTile();
		if(!hasFightStarted){
			this.tiles[origine.x, origine.y].GetComponentInChildren<TileController>().setDestination(1);
		}
		else{
			this.setLaunchability("Déplacement en cours !");
			if(ApplicationModel.player.ToLaunchGameTutorial){
				this.hideTuto();
			}
		}
		this.clickedCharacterId=characterID;
		this.draggingCard = this.clickedCharacterId;
		this.getPlayingCardController(draggingCard).moveForward();
		this.timeDragging=0f;
		SoundController.instance.playSound(26);
	}

	public void clickMobileCharacter(int characterID){
		this.clickedCharacterId=characterID;
		this.timeDragging=0f;
	}

	public void dropCharacter(int characterID, Tile t, bool isFirstP, bool toDisplayMove){
		SoundController.instance.playSound(27);
		Tile origine = this.getPlayingCardController(characterID).getTile();
		this.removeSE(origine);
		if(this.hasFightStarted){
			this.tiles[origine.x, origine.y].GetComponentInChildren<TileController>().setDestination(0);
			this.removeDestinations();
		}
		else{
			if(this.getCard(characterID).isMine){
				this.tiles[origine.x, origine.y].GetComponentInChildren<TileController>().setDestination(0);
			}
		}
		if(isFirstP!=this.isFirstPlayer || toDisplayMove){
			if(GameView.instance.hasFightStarted){
				this.setLaunchability("Déplacement en cours !");
			}
			this.getPlayingCardController(characterID).moveForward();
			this.getPlayingCardController(characterID).changeTile(new Tile(t.x,t.y), this.tiles[t.x,t.y].GetComponentInChildren<TileController>().getPosition());
		}
		else{
			this.draggingCard=-1;
			this.getPlayingCardController(characterID).setTile(t);
			if(GameView.instance.hasFightStarted){
				if(!this.getPlayingCardController(characterID).getIsMine()){
					this.tiles[t.x, t.y].GetComponentInChildren<TileController>().setDestination(6);
				}
			}
			if(this.getPlayingCardController(characterID).getIsMine()){
				this.tiles[t.x, t.y].GetComponentInChildren<TileController>().setDestination(5);
			}
			if(!this.hasFightStarted){
				this.getPlayingCardController(characterID).moveBackward();
			}
		}
		this.tiles[origine.x, origine.y].GetComponentInChildren<TileController>().setCharacterID(-1);
		this.tiles[t.x, t.y].GetComponentInChildren<TileController>().setCharacterID(characterID);

		if(GameView.instance.hasFightStarted){
			if(!this.getCard(characterID).isGolem()){
				this.getCard(characterID).hasMoved=true;
			}
			else{
				int pv = 11-this.getCard(characterID).getSkills()[0].Power;
				GameView.instance.displaySkillEffect(characterID, "Golem\n-"+pv+"PV", 0);
				GameView.instance.getPlayingCardController(characterID).addDamagesModifyer(new Modifyer(pv,-1,1,"Attaque","10 dégats subis"), true, -1);
				GameView.instance.addAnim(4,GameView.instance.getTile(characterID));
				SoundController.instance.playSound(34);
			}
			this.removeDestinations();
			this.recalculateDestinations();
			this.updateActionStatus();
			if(this.getCard(characterID).isMine){
				if(this.getCard(characterID).isCreator()){
					int level = this.getCard(characterID).getSkills()[0].Power * 5 + 30;
					if(UnityEngine.Random.Range(1,101)<level){
						GameController.instance.addRock(origine, 140);
					}
				}
			}
		}
		int tempInt ;
		int bonus ;
		bool isM ;
		if(this.getCard(characterID).isProtector()){
			isM = this.getCard(characterID).isMine;
			List<Tile> neighbours = origine.getImmediateNeighbourTiles();
			for (int i = 0 ; i < neighbours.Count ; i++){
				tempInt = this.getTileController(neighbours[i]).getCharacterID();
				if(tempInt!=-1){
					if(this.getCard(tempInt).isMine==isM){
						this.getCard(tempInt).isProtected(true);
					}
				}
			}

			neighbours = t.getImmediateNeighbourTiles();
			for (int i = 0 ; i < neighbours.Count ; i++){
				tempInt = this.getTileController(neighbours[i]).getCharacterID();
				if(tempInt!=-1){
					if(this.getCard(tempInt).isMine==isM){
						this.getCard(tempInt).isProtected(true);
						bonus = this.getCard(i).getSkills()[0].Power*2+10;
						this.getPlayingCardController(i).addShieldModifyer(new Modifyer(bonus, -1, 111, base.name, "Bouclier "+bonus+"%"));
					}
				}
			}
		}
		else{
			this.getCard(characterID).isProtected(true);
			isM = this.getCard(characterID).isMine;

			List<Tile> neighbours = t.getImmediateNeighbourTiles();
			for (int i = 0 ; i < neighbours.Count ; i++){
				tempInt = this.getTileController(neighbours[i]).getCharacterID();

				if(tempInt!=-1){
					if(this.getCard(tempInt).isProtector()){
						if(this.getCard(i).isMine==isM){
							this.getCard(tempInt).isProtected(true);
							bonus = this.getCard(i).getSkills()[0].Power*2+10;
							this.getPlayingCardController(tempInt).addShieldModifyer(new Modifyer(bonus, -1, 111, base.name, "Bouclier "+bonus+"%"));
						}
					}
				}
			}
		}

		if(ApplicationModel.player.ToLaunchGameTutorial){
			if(this.sequenceID==4){
				this.hitNextTutorial();
			}
		}
	}

	public void dropCharacter(int characterID){
		SoundController.instance.playSound(19);
		this.draggingCard=-1;
		Tile t = this.getPlayingCardTile(characterID);
		this.getPlayingCardController(characterID).setTile(t);
		if(!hasFightStarted){
			if(this.getPlayingCardController(characterID).getIsMine()){
				this.tiles[t.x, t.y].GetComponentInChildren<TileController>().setDestination(5);
			}
		}
		this.getPlayingCardController(characterID).moveBackward();
		if(GameView.instance.hasFightStarted){
			this.updateActionStatus();
		}
	}

	public void dropMobileCharacter(int characterID){
		Tile t = this.getPlayingCardTile(characterID);
		this.getPlayingCardController(characterID).setTile(t);
		if(!hasFightStarted){
			if(this.getPlayingCardController(characterID).getIsMine()){
				this.tiles[t.x, t.y].GetComponentInChildren<TileController>().setDestination(5);
			}
		}
		this.getPlayingCardController(characterID).moveBackward();
		if(GameView.instance.hasFightStarted){
			this.updateActionStatus();
		}
	}
	
	public void changeCurrentClickedCard(int characterID){

		if(this.currentPlayingCard!=-1){
			this.getPlayingCardController(this.currentPlayingCard).stopAnim ();
			this.getPlayingCardController(this.currentPlayingCard).moveBackward();
		}
		this.currentPlayingCard = characterID ;
		this.getPlayingCardController(this.currentPlayingCard).moveForward();
		this.getPlayingCardController(this.currentPlayingCard).run ();
		if(this.hasFightStarted){
			if(this.isMobile){

			}
			else{
				if(this.getCard(this.currentPlayingCard).isMine){
					this.getMyHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
				}
				else{
					this.getHisHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
				}
			}
		}
	}
	
	public void clickEmpty(){
		if(!this.hasFightStarted){
			this.currentPlayingCard = -1 ;
			this.getMyHoveredCardController().setNextDisplayedCharacter(-1, new GameCard());
			this.getHisHoveredCardController().setNextDisplayedCharacter(-1, new GameCard());
		}
	}

	public IEnumerator checkDestination(int c){
		bool isSuccess ;
		if(GameView.instance.hasFightStarted){
			isSuccess = this.getTileController(c).checkTrap();
			this.removeDestinations();
			
			if(this.getCard(this.currentPlayingCard).hasPlayed && this.getCard(this.currentPlayingCard).hasMoved){
				this.getPassZoneController().show(false);
			}

			if(this.getCard(c).isMine){
				if(this.getCard(this.currentPlayingCard).hasPlayed && this.getCard(this.currentPlayingCard).hasMoved){
					if(isSuccess){
						yield return new WaitForSeconds(2f);
					}
					if(!this.deads.Contains(this.currentPlayingCard) && !this.getCurrentCard().isFurious()){
						yield return new WaitForSeconds(1f);
						GameController.instance.findNextPlayer (true);
					}
				}
			}
		}
		else{
			this.setInitialDestinations(this.isFirstPlayer);
		}
	}

	public void setLaunchability(string s){
		this.getSkillZoneController().setLaunchability(s);
		this.getPassZoneController().setLaunchability(s);
	}

	public int getCurrentPlayingCard(){
		return this.currentPlayingCard;
	}
	
	public void hoverTile(){
		
		if(!this.interlude.GetComponent<InterludeController>().getIsRunning()){
			if(!this.isMobile){
				if(this.currentPlayingCard!=-1){
					if(this.getCard(this.currentPlayingCard).isMine){
						this.getMyHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
					}
					else{
						this.getHisHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
					}
				}

				if(this.getMyHoveredCardController().getStatus()==0){
					if(this.getMyHoveredCardController().getCurrentCharacter()!=-1){
						if(this.getMyHoveredCardController().getCurrentCharacter()!=this.currentPlayingCard){
							this.getMyHoveredCardController().empty();
						}
					}
					if(this.getHisHoveredCardController().getCurrentCharacter()!=-1){
						if(this.getHisHoveredCardController().getCurrentCharacter()!=this.currentPlayingCard){	
							this.getHisHoveredCardController().empty();
						}
					}
				}
				else{
					if(this.getMyHoveredCardController().getNextDisplayedCharacter()!=-1){
						if(this.getMyHoveredCardController().getNextDisplayedCharacter()!=-1){
							if(this.getMyHoveredCardController().getNextDisplayedCharacter()!=this.currentPlayingCard){		
								this.getMyHoveredCardController().empty();
							}
						}
						if(this.getHisHoveredCardController().getNextDisplayedCharacter()!=-1){
							if(this.getHisHoveredCardController().getNextDisplayedCharacter()!=this.currentPlayingCard){			
								this.getHisHoveredCardController().empty();
							}
						}
					}
				}
			}
			if(this.hasFightStarted){
				if(this.runningSkill==-1){
					this.removeDestinations();
					if(this.currentPlayingCard!=-1){
						if(!this.isDisplayedMyDestination && !this.getCard(this.currentPlayingCard).hasMoved){
							this.displayDestinations(this.currentPlayingCard);
						}
					}
				}
			}
		}
	}
	
	public int findCardWithDO(int o, bool isM){
		int i = 0 ;
		int card = -1;
		while(i < this.nbCards ){ 
			if(this.getCard(i).isMine==isM && this.getCard(i).deckOrder==o){
				card = i ;
				i = 100 ;
			}
			i++;
		}
		return card ;
	}

	public void hideSkillEffects(){
		for (int i = 0 ; i < this.boardWidth ; i++){
			for (int j = 0 ; j < this.boardHeight ; j++){
				GameView.instance.removeSE(new Tile(i,j));
			}
		}
	}

	public void setNextPlayer(bool isEndMeteor){
		
		if(!this.isChangingTurn){
			this.isChangingTurn = true;
			isFreezed = true ;
			this.hideButtons();
			//this.hideSkillEffects();
			if(this.sequenceID==19){
				this.hitNextTutorial();
			}
			this.hoveringZone=-1 ;
			if(this.hasFightStarted){
				this.meteoritesCounter--;
				if(this.meteoritesCounter==0){
					StartCoroutine(this.endTurnEffects(false));
				}
				else{
					if(!isEndMeteor){
						StartCoroutine(endTurnEffects(true));
					}
					else{
						this.launchEndTurnEffects();
					}
				}
			}
			else{
				GameObject tempGO = GameObject.Find("mainLogo");
				tempGO.GetComponent<SpriteRenderer>().enabled = false ;
			
				this.launchEndTurnEffects();
			}
		}
	}

	public IEnumerator endTurnEffects(bool toLaunchEndTurn){
		if(this.hasFightStarted){
			bool isSuccess = false ;
			if(!GameView.instance.getCurrentCard().isDead){
				if(GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).isPoisoned()){
					int value = Mathf.Min(GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).getPoisonAmount(), GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).getLife());
					GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), "Poison\nPerd "+value+"PV", 0);
					GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer(value,-1,94,"Poison",value+" dégats subis"), false, -1);
					GameView.instance.addAnim(4,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
					SoundController.instance.playSound(34);
					isSuccess = true ;
				}
				if(this.getCurrentTileController().getIsTrapped()){
					if(this.getCurrentTileController().trap.getType()==4){
						int soin = Mathf.Min(this.getCurrentCard().GetTotalLife()-this.getCurrentCard().getLife(), this.getCurrentTileController().trap.getAmount());
						if(soin==0){
							GameView.instance.displaySkillEffect(this.currentPlayingCard, "Soin sans effet", 1);	
							GameView.instance.addAnim(8,GameView.instance.getTile(this.currentPlayingCard));
						}
						else{
							this.getPlayingCardController(this.currentPlayingCard).addDamagesModifyer(new Modifyer(-1*soin, -1, 44, "Fontaine", "+"+(soin)+"PV"), false, -1);
							GameView.instance.displaySkillEffect(this.currentPlayingCard, "+"+soin+"PV", 2);	
							GameView.instance.addAnim(1,GameView.instance.getTile(this.currentPlayingCard));
							SoundController.instance.playSound(37);
						}
						isSuccess = true ;
					}
					else if(this.getCurrentTileController().trap.getType()==4){
						this.getPlayingCardController(this.currentPlayingCard).addAttackModifyer(new Modifyer(this.getCurrentTileController().trap.getAmount(), -1, 46, "Caserne", "+"+(this.getCurrentTileController().trap.getAmount())+"ATK"));
						GameView.instance.displaySkillEffect(this.currentPlayingCard, "+"+this.getCurrentTileController().trap.getAmount()+"ATK", 2);	
						GameView.instance.addAnim(7,GameView.instance.getTile(this.currentPlayingCard));
						SoundController.instance.playSound(37);
						isSuccess = true ;
					}
				}
				if(this.getCard(this.currentPlayingCard).isNurse()){
					int power = 3+2*this.getCurrentCard().Skills[0].Power;
					List<Tile> neighbourTiles = this.getNeighbours(this.getPlayingCardController(this.currentPlayingCard).getTile());
					this.targets = new List<Tile>();
					int playerID;
					int soin ;
					foreach (Tile t in neighbourTiles)
					{
						playerID = this.getTileController(t.x, t.y).getCharacterID();
						if (playerID != -1)
						{
							if (this.getCard(playerID).isMine==this.getCard(this.currentPlayingCard).isMine){
								soin = Mathf.Min(this.getCard(playerID).GetTotalLife()-this.getCard(playerID).getLife(), power);
								isSuccess = true ;
								if(soin==0){
									GameView.instance.displaySkillEffect(playerID, "Soin sans effet", 1);	
									GameView.instance.addAnim(8,GameView.instance.getTile(playerID));
								}
								else{
									this.getPlayingCardController(playerID).addDamagesModifyer(new Modifyer(-1*soin, -1, 75, "Infirmier", "+"+(soin)+"PV"), false, -1);
									GameView.instance.displaySkillEffect(playerID, "+"+soin+"PV", 2);	
									GameView.instance.addAnim(1,GameView.instance.getTile(playerID));
									SoundController.instance.playSound(37);
								}
							}
						}
					}
					if(isSuccess){
						GameView.instance.displaySkillEffect(this.currentPlayingCard, "Infirmier", 1);	
						GameView.instance.addAnim(8,GameView.instance.getTile(this.currentPlayingCard));
					}
				}
				else if(this.getCard(this.currentPlayingCard).isPurificateur() && this.getCurrentCard().isMine){
					int proba = 34+4*this.getCurrentCard().Skills[0].Power;
					List<Tile> neighbourTiles = this.getNeighbours(this.getPlayingCardController(this.currentPlayingCard).getTile());
					this.targets = new List<Tile>();
					int playerID;
					foreach (Tile t in neighbourTiles)
					{
						playerID = this.getTileController(t.x, t.y).getCharacterID();
						if (playerID != -1)
						{
							if (UnityEngine.Random.Range(1,101)<proba){
								GameController.instance.purify(playerID, true);
								isSuccess = true ;
							}
							else{
								GameController.instance.purify(playerID, false);
								isSuccess = true ;
							}
						}
					}
				}
				else if(this.getCard(this.currentPlayingCard).isMissionary() && this.getCurrentCard().isMine){
					int proba = 5+2*this.getCurrentCard().Skills[0].Power;
					List<Tile> neighbourTiles = this.getNeighbours(this.getPlayingCardController(this.currentPlayingCard).getTile());
					this.targets = new List<Tile>();
					int playerID;
					foreach (Tile t in neighbourTiles)
					{
						playerID = this.getTileController(t.x, t.y).getCharacterID();
						if (playerID != -1)
						{
							if (UnityEngine.Random.Range(1,101)<=proba){
								GameController.instance.convert(playerID);
								isSuccess = true ;
							}
						}
					}
				}
				else if(this.getCard(this.currentPlayingCard).isFrenetique()){
					int level = GameView.instance.getCurrentCard().Skills[0].Power+2;
					int target = GameView.instance.getCurrentPlayingCard();

					GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(10,-1,69,"Frenetique","10 dégats subis"), false, -1);
					GameView.instance.getPlayingCardController(target).updateAttack(this.getCard(target).getAttack());
					GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(level, -1, 18, "Frenetique", ". Permanent."));
					GameView.instance.displaySkillEffect(target, "Frénétique\n+"+level+" ATK\n-10PV", 1);
					GameView.instance.addAnim(0,GameView.instance.getTile(target));
					SoundController.instance.playSound(28);
					isSuccess = true ;
				}
				if(isSuccess){
					yield return new WaitForSeconds(2f);
				}
			}
		}

		if(toLaunchEndTurn){
			this.launchEndTurnEffects();
		}
		else{
			print("Je lance météorites");
		
			List<int> idCards = new List<int>();
			idCards.Add(currentPlayingCard);
			int i = 0 ; 
			int j = this.meteoritesCounter ; 
			int l = this.meteoritesStep ; 
			while (idCards.Count<9){
				if(j==0){
					idCards.Add(-1*l);
					if(l==1){
						j = 7 ;
						l = 2 ;
					}
					else if(l==2){
						j = 6 ;
						l = 3 ;
					}
					else if(l==3){
						j = 5 ;
						l = 4 ;
					}
					else{
						j = 4 ;
					}
				}
				else{
					idCards.Add(orderCards[i]);
					i++;
					j--;
				}
			}

			this.timeline.changeFaces(idCards);

			this.isFreezed = true ;
			this.interlude.GetComponent<InterludeController>().set("Fin du tour - Météorites !", 3);
			
			nbTurns++;
			if(meteoritesStep==1){
				meteoritesCounter = 7 ;
				meteoritesStep = 2 ;
			}
			else if(meteoritesStep==2){
				meteoritesCounter = 6 ;
				meteoritesStep = 3 ;
			}
			else if(meteoritesStep==3){
				meteoritesCounter = 5 ;
				meteoritesStep = 4 ;
			}
			else{
				meteoritesCounter = 4 ;
			}
		}
	}

	public void launchEndTurnEffects(){
		
		if(!this.hasFightStarted){
			this.changePlayer(-1);
		
			if (this.getCurrentCard().isMine){
				this.interlude.GetComponent<InterludeController>().set("A votre tour de jouer !", 1);
			}
			else{
				this.interlude.GetComponent<InterludeController>().set("Tour de l'adversaire !", 2);
			}
		}
		else{
			this.getPlayingCardController(this.currentPlayingCard).checkModyfiers();
			int i = this.indexPlayer+1;
			if(i==this.nbCards){
				i=0;
			}
			while(this.getCard(this.orderCards[i]).isDead || this.deads.Contains(this.orderCards[i])){
				i++;
				if(i==this.nbCards){
					i=0;
				}
			}

			if (this.getCard(this.orderCards[i]).isMine){
				this.interlude.GetComponent<InterludeController>().set("A votre tour de jouer !", 1);
			}
			else{
				this.interlude.GetComponent<InterludeController>().set("Tour de l'adversaire !", 2);
			}
		}
	}

	public void purify(int target, bool b){
		if(b || !b){
			this.getPlayingCardController(target).emptyModifiers();
			GameView.instance.displaySkillEffect(target, "Purifié!", 1);	
			GameView.instance.addAnim(0,GameView.instance.getTile(target));
		}
		else{
			GameView.instance.displaySkillEffect(target, "Echec purification", 0);	
			GameView.instance.addAnim(8,GameView.instance.getTile(target));
		}
	}

	public void updateTimeline(){
		List<int> idCards = new List<int>();
		idCards.Add(this.lastPlayingCard);
		int i = this.indexPlayer ; 
		int j = this.meteoritesCounter ; 
		int l = this.meteoritesStep ; 
		while (idCards.Count<8){
			if(j==0){
				idCards.Add(-1*l);
				if(l==1){
					j = 7 ;
					l = 2 ;
				}
				else if(l==2){
					j = 6 ;
					l = 3 ;
				}
				else if(l==3){
					j = 5 ;
					l = 4 ;
				}
				else{
					j = 4 ;
				}
			}
			else{
				if(!this.getCard(this.orderCards[i]).isDead && !this.deads.Contains(this.orderCards[i])){
					idCards.Add(orderCards[i]);
					j--;
				}
				i++;
				if(i==this.nbCards){
					i=0;
				}
			}
		}
		this.timeline.changeFaces(idCards);
		this.timeline.show(true);
	}

	public void changePlayer(int idToChange){
		//print("Change");
		if(this.hasFightStarted){
			if(this.getCurrentCard().isMutant()){
				this.getPlayingCardController(this.currentPlayingCard).nbTurns++;
				if(this.getPlayingCardController(this.currentPlayingCard).nbTurns==3){
					int level = this.getCurrentCard().getSkills()[0].Power;
					this.getPlayingCardController(this.currentPlayingCard).emptyModifiers();
					this.getCard(this.currentPlayingCard).emptyDamageModifyers();
					this.getPlayingCardController(this.currentPlayingCard).updateLife(this.getCard(this.currentPlayingCard).getLife());
					this.getPlayingCardController(this.currentPlayingCard).updateAttack(this.getCard(this.currentPlayingCard).getAttack());
					this.getCard(this.currentPlayingCard).Attack = 15+level*5;

					this.getCard(this.currentPlayingCard).Life = 45+level*5;
					this.getCard(this.currentPlayingCard).getSkills()[0].Id = 144;
					this.getPlayingCardController(this.currentPlayingCard).show();
					GameView.instance.displaySkillEffect(this.currentPlayingCard, "Mutant\nse transforme!", 2);
					GameView.instance.addAnim(4,GameView.instance.getTile(this.currentPlayingCard));
					SoundController.instance.playSound(31);
				}
			}
			this.lastPlayingCard = currentPlayingCard;
		}
		else{
			this.lastPlayingCard = -10;
			if(this.isFirstPlayer==this.isFirstPlayerStarting){
				orderCards.Add(this.findCardWithDO(0, true));
				orderCards.Add(this.findCardWithDO(0, false));
				orderCards.Add(this.findCardWithDO(1, true));
				orderCards.Add(this.findCardWithDO(1, false));
				orderCards.Add(this.findCardWithDO(2, true));
				orderCards.Add(this.findCardWithDO(2, false));
				orderCards.Add(this.findCardWithDO(3, true));
				orderCards.Add(this.findCardWithDO(3, false));
			}
			else{
				orderCards.Add(this.findCardWithDO(0, false));
				orderCards.Add(this.findCardWithDO(0, true));
				orderCards.Add(this.findCardWithDO(1, false));
				orderCards.Add(this.findCardWithDO(1, true));
				orderCards.Add(this.findCardWithDO(2, false));
				orderCards.Add(this.findCardWithDO(2, true));
				orderCards.Add(this.findCardWithDO(3, false));
				orderCards.Add(this.findCardWithDO(3, true));
			}
			this.updateTimeline();
		}

		int nextPlayingCard ;
		if(idToChange==-1){
			if(this.hasFightStarted){
				this.indexPlayer++;
				if(indexPlayer == this.nbCards){
					indexPlayer = 0 ;
				}
			}
			this.updateTimeline();
			while(this.getCard(this.orderCards[indexPlayer]).isDead || this.deads.Contains(this.orderCards[indexPlayer])){
				this.indexPlayer++;
				if(indexPlayer == this.nbCards){
					indexPlayer = 0 ;
				}
			}
			nextPlayingCard = this.orderCards[indexPlayer];
		}
		else{
			nextPlayingCard = idToChange;
		}

		if(this.hasFightStarted){
			this.hideTargets();
		}
		bool hasMoved = false ;
		bool hasPlayed = false ;
		
		if (this.getCard(nextPlayingCard).isSniperActive()){
			hasMoved = true ;
		}
		else if (this.getCard(nextPlayingCard).isEffraye()){
			hasPlayed = true ;
		}
		else if (this.getCard(nextPlayingCard).isFurious()){
			hasPlayed = true ;
			hasMoved = true ;
		}
	
		this.getCard(nextPlayingCard).setHasMoved(hasMoved);
		this.getCard(nextPlayingCard).setHasPlayed(hasPlayed);
		this.changeCurrentClickedCard(nextPlayingCard) ;
	}
	
	public IEnumerator launchFury(){
		
		if(ApplicationModel.player.ToLaunchGameTutorial){
			while(!this.blockFury){
				yield return new WaitForSeconds(1f);
			}
		}
		yield return new WaitForSeconds(2f);
		
		int enemy = GameView.instance.attackClosestCharacter();
		
		yield return new WaitForSeconds(1.2f);
		
		if(enemy!=-1){
			GameController.instance.play(0);
			
			if (UnityEngine.Random.Range(1,101) <= this.getCard(enemy).getEsquive())
			{                             
				GameController.instance.esquive(enemy,1);
			}
			else{
				GameController.instance.applyOn(enemy);
			}
			GameView.instance.displaySkillEffect(this.currentPlayingCard, "Attaque", 0);
			yield return new WaitForSeconds(2.5f);
			
		}

		GameController.instance.findNextPlayer(true);
	}

	public IEnumerator launchTourelle(){
		
		yield return new WaitForSeconds(2f);
		List<int> ennemis = this.getOpponents(this.getCurrentCard().isMine);
		Tile tile ;
		Tile currentTile = this.getCurrentCardTile();
		for(int i = 0 ; i < ennemis.Count; i++){
			GameController.instance.play(38);

			tile = this.getTile(ennemis[i]);
			if(Mathf.Abs(tile.x-currentTile.x)+Math.Abs(tile.y-currentTile.y)<=2){
				if (UnityEngine.Random.Range(1,101) <= this.getCard(ennemis[i]).getMagicalEsquive())
				{                             
					GameController.instance.esquive(ennemis[i],1);
				}
				else{
					GameController.instance.applyOn(ennemis[i]);
				}
			}
		}

		yield return new WaitForSeconds(2f);
		GameController.instance.findNextPlayer(true);
	}

	public IEnumerator launchIABourrin(){
		
		if(ApplicationModel.player.ToLaunchGameTutorial){
			while(!this.blockFury){
				yield return new WaitForSeconds(1f);
			}
		}
		yield return new WaitForSeconds(2f);
		
		int enemy = GameView.instance.attackClosestEnnemy();
		
		yield return new WaitForSeconds(1.2f);
		
		if(enemy!=-1){
			GameController.instance.play(0);
			
			if (UnityEngine.Random.Range(1,101) <= this.getCard(enemy).getEsquive())
			{                             
				GameController.instance.esquive(enemy,1);
			}
			else{
				GameController.instance.applyOn(enemy);
			}
			GameView.instance.displaySkillEffect(this.currentPlayingCard, "Attaque", 0);
			yield return new WaitForSeconds(2.5f);
			
		}
		GameController.instance.findNextPlayer(true);
	}
	
	public void emptyTile(int c){
		this.getTileController(this.getPlayingCardTile(c).x,this.getPlayingCardTile(c).y).setCharacterID(-1);
		this.getTileController(this.getPlayingCardTile(c).x,this.getPlayingCardTile(c).y).setDestination(-1);
		this.getTileController(this.getPlayingCardTile(c).x,this.getPlayingCardTile(c).y).showEffect(false);
	}
	
	public void updateActionStatus(){
		if(this.getCurrentCard().isMine){
			this.getPassZoneController().updateButtonStatus (this.getCard(this.currentPlayingCard));
			this.getPassZoneController().getLaunchability ();
			this.getSkillZoneController().updateButtonStatus (this.getCard(this.currentPlayingCard));
		}
	}

	public void hideButtons(){
		this.getPassZoneController().show (false);
		this.getSkillZoneController().showCancelButton (false);
		this.getSkillZoneController().showSkillButtons(false);
	}
	
	public void setRankedCharacter(int id, int rank){
		this.getCard(id).setNbTurnsToWait(rank);
	}
	
	public bool getIsFirstPlayer(){
		return this.isFirstPlayer ;
	}
	
	public GameCard getCard(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getCard();
	}
	
	public Deck getMyDeck(){
		return this.myDeck;
	}
	
	public TileController getTileController(int x, int y){
		return this.tiles[x,y].GetComponent<TileController>();
	}
	
	public TileController getTileController(Tile t){
		return this.tiles[t.x,t.y].GetComponent<TileController>();
	}

	public TileController getCurrentTileController(){
		return this.tiles[this.getCurrentCardTile().x,this.getCurrentCardTile().y].GetComponent<TileController>();
	}
	
	public TileController getTileController(int c){
		return this.tiles[this.getPlayingCardController(c).getTile().x,this.getPlayingCardController(c).getTile().y].GetComponent<TileController>();
	}
	
	public PlayingCardController getPlayingCardController(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>();
	}
	
	public void displayCurrentMove(){
		this.displayDestinations(this.currentPlayingCard);
	}
	
	void Update()
	{
		if (this.widthScreen!=-1){
			if (this.widthScreen!=Screen.width){
				this.resize();
			}
		}

		if(this.timerFront.GetComponent<TimerFrontController>().isShowing){
			this.timerFront.GetComponent<TimerFrontController>().addTime(Time.deltaTime);
		}

		if(this.myTimer.isShowing){
			this.myTimer.addTime(Time.deltaTime);
		}

		if(this.hisTimer.isShowing){
			this.hisTimer.addTime(Time.deltaTime);
		}

		if(this.toLaunchCardCreation && this.isGameskillOK){
			this.toLaunchCardCreation = false ; 
			this.loadMyDeck();
		}

		if(this.draggingSkillButton!=-1){
			Vector3 mousePos = Input.mousePosition;
			this.getSkillZoneController().getSkillButtonController(draggingSkillButton).setPosition3(Camera.main.ScreenToWorldPoint(mousePos));
		}

		if(this.timeDragging>=0){
			timeDragging+=Time.deltaTime;
		}

		if(this.draggingCard!=-1){
			Vector3 mousePos = Input.mousePosition;
			this.getPlayingCardController(draggingCard).setPosition(Camera.main.ScreenToWorldPoint(mousePos));
		}

		if(anims.Count>0){
			for(int i = 0 ; i < anims.Count ; i++){
				this.getTileController(anims[i].x, anims[i].y).addAnimTime(Time.deltaTime);
			}
		}

		if(skillEffects.Count>0){
			for(int i = 0 ; i < skillEffects.Count ; i++){
				this.getTileController(skillEffects[i].x, skillEffects[i].y).addSETime(Time.deltaTime);
			}
		}
		
		if (this.SB.GetComponent<StartButtonController>().getIsPushed()){
			this.SB.GetComponent<StartButtonController>().addTime(Time.deltaTime);
		}
		
		if (this.getMyHoveredCardController().getStatus()!=0){
			this.getMyHoveredCardController().addTime(Time.deltaTime);
		}
		
		if(this.getMyHoveredCardController().getIsRunning() && !this.isMobile){
			this.getMyHoveredCardController().addTimeC(Time.deltaTime);
		}
		
		if (this.getHisHoveredCardController().getStatus()!=0){
			this.getHisHoveredCardController().addTime(Time.deltaTime);
		}
		
		if(this.getHisHoveredCardController().getIsRunning() && !this.isMobile){
			this.getHisHoveredCardController().addTimeC(Time.deltaTime);
		}

		if(this.numberDeckLoaded==2 || (ApplicationModel.player.ToLaunchGameTutorial && this.numberDeckLoaded==1)){
			for(int i = 0 ; i < this.nbCards ; i++){
				if(this.getPlayingCardController(i).isUpdatingLife){
					this.getPlayingCardController(i).addLifeTime(Time.deltaTime);
				}
				if(this.getPlayingCardController(i).isUpdatingAttack){
					this.getPlayingCardController(i).addAttackTime(Time.deltaTime);
				}
				if(!this.getCard(i).isDead){
					if(this.getPlayingCardController(i).getIsRunning()){
						this.getPlayingCardController(i).addTime(Time.deltaTime);
					}
					if(this.getPlayingCardController(i).getIsMoving()){
						this.getPlayingCardController(i).addMoveTime(Time.deltaTime);
					}
				}
			}
		}

		if(deads.Count>0){
			for(int i = 0 ; i < deads.Count ; i++){
				this.getPlayingCardController(deads[i]).addDeadTime(Time.deltaTime);
			}
		}
		
		if(this.interlude.GetComponent<InterludeController>().getIsRunning()){
			this.interlude.GetComponent<InterludeController>().addTime(Time.deltaTime);
		}
		
		for (int i = 0 ; i < this.targets.Count; i++){
			this.getTileController(this.targets[i].x, this.targets[i].y).addTargetTime(Time.deltaTime);
		}
	}
	
	public MyHoveredCardController getMyHoveredCardController(){
		return this.myHoveredRPC.GetComponent<MyHoveredCardController>();
	}
	
	public HisHoveredCardController getHisHoveredCardController(){
		return this.hisHoveredRPC.GetComponent<HisHoveredCardController>();
	}
	
	public Sprite getSprite(int i){
		return this.sprites[i];
	}

	public Sprite getFactionSprite(int i){
		return this.factionSprites[i];
	}
	
	public void loadHisHoveredPC(int c){
		Card card = this.playingCards[c].GetComponent<PlayingCardController>().getCard();
		List<Skill> skills = this.playingCards[c].GetComponent<PlayingCardController>().getCard().getSkills();
		GameObject.Find("Title").GetComponent<TextMeshPro>().text = card.Title;
		GameObject.Find("HisSpecialite").GetComponent<TextMeshPro>().text = WordingSkills.getName(skills[0].Id);
		GameObject.Find("HisSpecialiteDescription").GetComponent<TextMeshPro>().text = WordingSkills.getDescription(skills[0].Id, skills[0].Power);
		
		this.hisHoveredRPC.GetComponent<SpriteRenderer>().sprite = this.sprites[card.ArtIndex];
		
		for (int i = 1 ; i < skills.Count ; i++){
			GameObject.Find("Skill"+(i-1)+"Title").GetComponent<TextMeshPro>().text = WordingSkills.getName(skills[i].Id);
			GameObject.Find(("Skill"+(i-1)+"Description")).GetComponent<TextMeshPro>().text =  WordingSkills.getDescription(skills[i].Id, skills[i].Power);
		}
		for (int i = skills.Count ; i < 4 ; i++){
			GameObject.Find("Skill"+(i-1)+"Title").GetComponent<TextMeshPro>().text = "";
			GameObject.Find(("Skill"+(i-1)+"Description")).GetComponent<TextMeshPro>().text = "";
		}
	}
	
	public void createBackground()
	{
		for (int i = 0; i < this.verticalBorders.Length; i++)
		{
			this.verticalBorders [i] = (GameObject)Instantiate(this.verticalBorderModel);
		}
		for (int i = 0; i < this.horizontalBorders.Length; i++)
		{
			this.horizontalBorders [i] = (GameObject)Instantiate(this.horizontalBorderModel);
		}
		
		this.isBackgroundLoaded = true ;
		this.resize();
	}

	public List<Tile> getFreeCenterTiles(){
		List<Tile> freeCenterTiles = new List<Tile>();
//		for(int x = 0 ; x < this.boardWidth ; x++){
//			for(int y = 2 ; y < this.boardHeight-2 ; y++){
//				if(this.tiles[x,y].GetComponent<TileController>().getTileType()==0 && this.tiles[x,y].GetComponent<TileController>().getTrapId()==-1){
//					freeCenterTiles.Add (new Tile(x,y));
//				}
//			}
//		}
		return freeCenterTiles;
	}
	
	public void hideTrap(int x, int y){
		this.tiles [x, y].GetComponent<TileController>().removeTrap();
	}
	
	public void resize()
	{
		this.widthScreen = Screen.width ;
		this.heightScreen = Screen.height ;
		
		if (this.isBackgroundLoaded){
			this.resizeBackground();
		}
	}
	
	public void resizeBackground()
	{		
		Vector3 position;
		Vector3 scale;
		Transform tempTransform;
		this.realwidth = 10f*this.widthScreen/this.heightScreen;
		this.isMobile = (this.widthScreen<this.heightScreen);
		if(this.isMobile){
			this.myHoveredRPC.GetComponent<MyHoveredCardController>().activateCollider(true);
			this.hisHoveredRPC.GetComponent<HisHoveredCardController>().activateCollider(true);
		}
		else{
			this.myHoveredRPC.GetComponent<MyHoveredCardController>().activateCollider(false);
			this.hisHoveredRPC.GetComponent<HisHoveredCardController>().activateCollider(false);
		}

		this.tileScale = Mathf.Min (realwidth/6.05f, 8f / this.boardHeight);
		for (int i = 0; i < this.horizontalBorders.Length; i++)
		{
			position = new Vector3(0, (-4*tileScale) + tileScale * i, -1f);
			this.horizontalBorders [i].transform.localPosition = position;
			this.horizontalBorders [i].transform.localScale = new Vector3(1f,0.5f,1f);
		}
		
		for (int i = 0; i < this.verticalBorders.Length; i++)
		{
			position = new Vector3((-this.boardWidth/2f+i)*tileScale, 0f, -1f);
			this.verticalBorders [i].transform.localPosition = position;
			this.verticalBorders [i].transform.localScale = new Vector3(0.5f,tileScale,1f);
		}
		
		GameObject tempGO = GameObject.Find("MyPlayerBox");
		position = tempGO.transform.position ;
		if(this.isMobile){
			tempGO.transform.FindChild("MyPlayerName").GetComponent<MeshRenderer>().enabled = false ;
			tempGO.transform.FindChild("Forfeit").GetComponent<SpriteRenderer>().enabled = false ;
		}
		else{
			tempGO.transform.FindChild("MyPlayerName").GetComponent<MeshRenderer>().enabled = true ;
			tempGO.transform.FindChild("Forfeit").GetComponent<SpriteRenderer>().enabled = true ;
		}
		position.x=-realwidth/2f;
		tempGO.transform.position = position;
		tempGO.transform.FindChild("MyPlayerName").GetComponent<TextContainer>().width = realwidth/2f-4 ;
		tempGO.transform.FindChild("Time").transform.localPosition = new Vector3(realwidth/2f-4f,0f,0f) ;

		tempGO = GameObject.Find("HisPlayerName");
		position = tempGO.transform.position ;
		position.x = 0.48f*this.realwidth;
		tempGO.transform.position = position;
		tempGO.GetComponent<TextContainer>().width = realwidth/2f-4 ;
		tempGO.transform.FindChild("Time").transform.localPosition = new Vector3(-realwidth/2f+4f,0f,0f) ;
		if(this.isMobile){
			tempGO.GetComponent<MeshRenderer>().enabled = false ;
		}
		else{
			tempGO.GetComponent<MeshRenderer>().enabled = true ;
		}

		tempGO = GameObject.Find("mainLogo");
		position = tempGO.transform.position ;
		position.y = 2.5f+2*tileScale;
		tempGO.transform.position = position;
		if(this.hasFightStarted){
			tempGO.GetComponent<SpriteRenderer>().enabled = false ;
		}
		else{
			tempGO.GetComponent<SpriteRenderer>().enabled = true ;
		}

		tempGO = GameObject.Find("SB");
		position = tempGO.transform.position ;
		position.y = -2.5f-2*tileScale;
		tempGO.transform.position = position;

		tempTransform = this.interlude.transform.FindChild("Bar1");
		scale = tempTransform.transform.localScale ;
		scale.x = 1f*realwidth/20f;
		scale.y = 1f*realwidth/20f;
		scale.z = 1f*realwidth/20f;
		tempTransform.transform.localScale = scale;
		position = tempTransform.transform.position ;
		position.y = 0.75f*(realwidth/20f);
		tempTransform.transform.position = position;

		tempTransform = this.interlude.transform.FindChild("Bar2");
		scale = tempTransform.transform.localScale ;
		scale.x = 1f*realwidth/20f;
		scale.y = 1f*realwidth/20f;
		scale.z = 1f*realwidth/20f;
		tempTransform.transform.localScale = scale;

		tempTransform = this.interlude.transform.FindChild("Bar3");
		scale = tempTransform.transform.localScale ;
		scale.x = 1f*realwidth/20f;
		scale.y = 1f*realwidth/20f;
		scale.z = 1f*realwidth/20f;
		tempTransform.transform.localScale = scale;
		tempTransform.transform.localScale = scale;
		position = tempTransform.transform.position ;
		position.y = -0.75f*(realwidth/20f);
		tempTransform.transform.position = position;

		tempTransform = this.interlude.transform.FindChild("Text");
		tempTransform.GetComponent<TextContainer>().width = realwidth ;
		tempTransform.GetComponent<TextContainer>().height = 2*(realwidth/20f) ;

		tempTransform = this.passZone.transform;
		position = tempTransform.position ;
		position.x = Mathf.Min(2.2f, 0.5f*this.realwidth-0.8f);
		tempTransform.position = position;
	
		if(this.isMobile){
			tempTransform = this.skillZone.transform.FindChild("CancelZone");
			position = tempTransform.position ;
			position.x = 2.5f * (realwidth/6f);
			tempTransform.position = position;

			tempTransform = this.skillZone.transform.FindChild("CancelZone").FindChild("Text");
			tempTransform.GetComponent<TextContainer>().width = 3.5f*(realwidth/6f) ;
		}
		this.getMyHoveredCardController().resize(realwidth, tileScale);
		this.getHisHoveredCardController().resize(realwidth, tileScale);
		this.interlude.GetComponent<InterludeController>().resize(realwidth);

		if(this.areTilesLoaded){
			for(int i = 0 ; i < boardWidth ; i++){
				for(int j = 0 ; j < boardHeight ; j++){
					this.getTileController(i,j).resize();
				}
			}
			for(int i = 0 ; i < this.nbCards ; i++){
				this.getPlayingCardController(i).resize();
			}
		}
	}
	
	public void hitExternalCollider(){
		GameView.instance.hoverTile();

		if(GameView.instance.isMobile){
			if(GameView.instance.draggingSkillButton!=-1){
				GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).shiftCenter();
				GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setRed();
				GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).setDescription(GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power)));
				GameView.instance.getSkillZoneController().getSkillButtonController(GameView.instance.draggingSkillButton).showDescription(true);
			}
		}
	}
	
	public int getNbPlayingCards(){
		return this.nbCards;
	}
	
	public Tile getPlayingCardTile(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getTile();
	}
	
	public Tile getTile(int x, int y){
		return this.tiles[x,y].GetComponent<TileController>().getTile();
	}
	
	public int getTileCharacterID(int x, int y){
		return this.tiles[x,y].GetComponent<TileController>().getCharacterID();
	}

	public List<Tile> getRocks(){
		List<Tile> rocks = new List<Tile>();
		for(int i = 0 ; i < boardWidth ;i++){
			for(int j = 0 ; j < boardHeight ;j++){
				if(this.getTileController(i,j).isRock()){
					rocks.Add(new Tile(i,j));
				}
			}
		}
		return rocks ;
	}
	
	public void calculateDestinations(int i){
		List<Tile> destinations = new List<Tile>();
		if(this.getCard(i).isGolem()){
			List<Tile> rocks = this.getRocks();
			List<Tile> voisins ;
			bool[,] isDestination = new bool[this.boardWidth, this.boardHeight];
			for(int l = 0 ; l < this.boardWidth ; l++){
				for(int k = 0 ; k < this.boardHeight ; k++){
					isDestination[l,k]=false;
				}
			}
			for(int m = 0 ; m < rocks.Count ; m++){
				voisins = rocks[m].getImmediateNeighbourTiles();
				for (int n = 0 ; n < voisins.Count ; n++){
					if(!isDestination[voisins[n].x, voisins[n].y]){
						if(this.getTileController(voisins[n].x, voisins[n].y).canBeDestination()){
							destinations.Add(voisins[n]);
						}
					}
				}
			}
		}
		else{
			bool[,] hasBeenPassages = new bool[this.boardWidth, this.boardHeight];
			bool[,] isDestination = new bool[this.boardWidth, this.boardHeight];
			for(int l = 0 ; l < this.boardWidth ; l++){
				for(int k = 0 ; k < this.boardHeight ; k++){
					hasBeenPassages[l,k]=false;
					isDestination[l,k]=false;
				}
			}
			List<Tile> baseTiles = new List<Tile>();
			List<Tile> tempTiles = new List<Tile>();
			List<Tile> tempNeighbours ;
			baseTiles.Add(this.getPlayingCardTile(i));
			int move = this.getCard(i).getMove();
			
			int j = 0 ;
			
			if(this.getCard(i).isMine){		
				while (j < move){
					tempTiles = new List<Tile>();
					
					for(int k = 0 ; k < baseTiles.Count ; k++){
						tempNeighbours = this.getDestinationImmediateNeighbours(baseTiles[k], true);
						for(int l = 0 ; l < tempNeighbours.Count ; l++){
							
							if(!hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]){
								tempTiles.Add(tempNeighbours[l]);
								hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]=true;
							}
							if(this.tiles[tempNeighbours[l].x, tempNeighbours[l].y].GetComponent<TileController>().getCharacterID()==-1){
								if(!isDestination[tempNeighbours[l].x, tempNeighbours[l].y]){
									destinations.Add(tempNeighbours[l]);
									isDestination[tempNeighbours[l].x, tempNeighbours[l].y]=true;
								}
							}
						}	
					}
					baseTiles = new List<Tile>();
					for(int l = 0 ; l < tempTiles.Count ; l++){
						baseTiles.Add(tempTiles[l]);
					}
					j++;
				}
			}
			else{
				while (j < move){
					tempTiles = new List<Tile>();
					
					for(int k = 0 ; k < baseTiles.Count ; k++){
						tempNeighbours = this.getDestinationHisImmediateNeighbours(baseTiles[k],true);
						for(int l = 0 ; l < tempNeighbours.Count ; l++){
							if(!hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]){
								tempTiles.Add(tempNeighbours[l]);
								hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]=true;
							}
							if(this.tiles[tempNeighbours[l].x, tempNeighbours[l].y].GetComponent<TileController>().getCharacterID()==-1){
								if(!isDestination[tempNeighbours[l].x, tempNeighbours[l].y]){
									destinations.Add(tempNeighbours[l]);
									isDestination[tempNeighbours[l].x, tempNeighbours[l].y]=true;
								}
							}
						}	
					}
					baseTiles = new List<Tile>();
					for(int l = 0 ; l < tempTiles.Count ; l++){
						baseTiles.Add(tempTiles[l]);
					}
					j++;
				}
			}
		}
		this.playingCards[i].GetComponent<PlayingCardController>().setDestinations(destinations);
	}
	
	public void recalculateDestinations(){
		
		for (int i = 0 ; i < this.nbCards ; i++){
			if(!this.getCard(i).isDead){
				this.calculateDestinations(i);
			}
		}
		GameView.instance.hoverTile();
	}
	
	public Tile getRandomRock(int nbForbiddenRows){
		return new Tile(UnityEngine.Random.Range(0, this.boardWidth), UnityEngine.Random.Range(0, this.boardHeight - 2*nbForbiddenRows) + 1);
	}
	
	public int getBoardWidth(){
		return this.boardWidth;
	}
	
	public int getBoardHeight(){
		return this.boardHeight;
	}
	
	public List<Tile> getMyPlayingCardsTiles(){
		List<Tile> tiles = new List<Tile>();
		for (int i = 0 ; i < this.nbCards ; i++){
			if (this.getCard(i).isMine){
				tiles.Add(this.getPlayingCardTile(i));
			}
		}
		return tiles;
	}
	
	public void displayDestinations(int c)
	{
		if(!this.getPlayingCardController(this.currentPlayingCard).getIsMoving()){
			int i = -1;
			if(this.currentPlayingCard==c && !this.getCard(c).hasMoved){
				if(this.getCard(c).isMine){
					i = 1 ;
				}
				else{
					i = 9;
				}
			}
			else{
				i = 10 ;
			}
			
			List<Tile> destinations = this.playingCards[c].GetComponent<PlayingCardController>().getDestinations();
			foreach (Tile t in destinations)
			{
				if (this.getTileController(t.x,t.y).canBeDestination()){
					this.getTileController(t.x,t.y).setDestination(i);
				}
			}
		}
	}

	public List<Tile> getAdjacentOpponentsTargets(Tile t, bool isM){
		List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(t, isM);
		List<Tile> cibles = new List<Tile>();
		int playerID;
		foreach (Tile t2 in neighbourTiles)
		{
			playerID = this.getTileController(t2.x, t2.y).getCharacterID();
			if (playerID != -1)
			{
				if (this.getPlayingCardController(playerID).canBeTargeted() && this.getCard(playerID).isMine!=isM){
					cibles.Add(t2);
				}
			}
		}
		return cibles;
	}

	public List<Tile> getMyUnitTarget(){
		PlayingCardController pcc;
		List<Tile> cibles = new List<Tile>();
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (i == this.currentPlayingCard)
			{
				cibles.Add(this.getTile(i));
			}
		}
		return cibles ;
	}

	public List<Tile> getAdjacentUnitsTargets(Tile t){
		List<Tile> neighbourTiles = this.getCharacterImmediateNeighbours(t);
		List<Tile> cibles = new List<Tile>();
		int playerID;
		foreach (Tile t2 in neighbourTiles)
		{
			playerID = this.getTileController(t2.x, t2.y).getCharacterID();
			if (playerID != -1)
			{
				if (playerID!=this.currentPlayingCard && this.getPlayingCardController(playerID).canBeTargeted()){
					cibles.Add(t2);
				}
			}
		}
		return cibles ;
	}
	
	public List<Tile> getAdjacentAllyTargets(Tile t, bool isM)
	{
		List<Tile> neighbourTiles = this.getAllyImmediateNeighbours(t, isM);
		List<Tile> cibles = new List<Tile>();
		int playerID;
		foreach (Tile t2 in neighbourTiles)
		{
			playerID = this.getTileController(t2.x, t2.y).getCharacterID();
			if (playerID != -1)
			{
				if (this.getPlayingCardController(playerID).canBeTargeted() && this.getCard(playerID).isMine==isM){
					cibles.Add(t2);
				}
			}
		}
		return cibles;
	}
	
	public List<Tile> getAllysButMeTargets(bool isM)
	{
		PlayingCardController pcc;
		List<Tile> cibles = new List<Tile>();

		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine==isM && pcc.canBeTargeted() &&  i != this.currentPlayingCard)
			{
				cibles.Add(this.getTile(i));
			}
		}
		return cibles;
	}

	public List<Tile> getWoundedAllysButMeTargets(bool isM)
	{
		PlayingCardController pcc;
		List<Tile> cibles = new List<Tile>();

		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine==isM && pcc.canBeTargeted() &&  i != this.currentPlayingCard)
			{
				if (this.getCard(i).getLife() != this.getCard(i).GetTotalLife())
				{
					cibles.Add(this.getTile(i));
				}
			}
		}

		return cibles ;
	}
	
	public List<Tile> getAllButMeTargets()
	{
		PlayingCardController pcc;
		List<Tile> cibles = new List<Tile>();
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (pcc.canBeTargeted() &&  i != this.currentPlayingCard)
			{
				cibles.Add(this.getTile(i));
			}
		}	
		return cibles;
	}
	
	public List<Tile> getOpponentsTargets(bool isM)
	{
		PlayingCardController pcc;
		List<Tile> cibles = new List<Tile>();
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine!=isM && pcc.canBeTargeted())
			{
				cibles.Add(this.getTile(i));
			}
		}
		return cibles;
	}

	public List<Tile> getAdjacentCristoidOpponents(bool isM){
		List<Tile> cibles = new List<Tile>();

		for(int i = 0 ; i < this.nbCards ; i++){
			if(i!=this.currentPlayingCard && this.getCard(i).CardType.Id==6 && this.getCard(i).isMine && !this.getCard(i).isDead){
				List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardTile(i), isM);
				int playerID;
				foreach (Tile t in neighbourTiles)
				{
					playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
					if (playerID != -1)
					{
						if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getCard(playerID).isMine!=isM)
						{
							cibles.Add(t);
						}
					}
				}
			}
		}

		return cibles;
	}

	public List<Tile> getAdjacentDroidOpponents(bool isM){
		List<Tile> cibles = new List<Tile>();

		for(int i = 0 ; i < this.nbCards ; i++){
			if(i!=this.currentPlayingCard && this.getCard(i).CardType.Id==5 && this.getCard(i).isMine && !this.getCard(i).isDead){
				List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardTile(i), isM);
				int playerID;
				foreach (Tile t in neighbourTiles)
				{
					playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
					if (playerID != -1)
					{
						if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getCard(playerID).isMine!=isM)
						{
							cibles.Add(t);
						}
					}
				}
			}
		}

		return cibles;
	}
	
	public List<Tile> getAdjacentTileTargets(Tile t)
	{
		List<Tile> neighbourTiles = this.getFreeImmediateNeighbours(t);
		List<Tile> cibles = new List<Tile>();
				
		foreach (Tile t2 in neighbourTiles){
			cibles.Add(t2);
		}

		return cibles ;
	}
	
	public List<Tile> get1TileAwayOpponentsTargets(Tile tile, bool isM)
	{
		int playerID;
		List<Tile> cibles = new List<Tile>();
		
		if(tile.x>1){
			playerID = this.tiles [tile.x-2, tile.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(this.getCard(playerID).isMine!=isM){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						cibles.Add(this.getTile(playerID));
					}
				}
			}
		}
		if(tile.x<this.boardWidth-2){
			playerID = this.tiles [tile.x+2, tile.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(this.getCard(playerID).isMine!=isM){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						cibles.Add(this.getTile(playerID));
					}
				}
			}
		}
		if(tile.y>1){
			playerID = this.tiles [tile.x, tile.y-2].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(this.getCard(playerID).isMine!=isM){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						cibles.Add(this.getTile(playerID));
					}
				}
			}
		}
		if(tile.y<this.boardHeight-2){
			playerID = this.tiles [tile.x, tile.y+2].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(this.getCard(playerID).isMine!=isM){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						cibles.Add(this.getTile(playerID));
					}
				}
			}
		}
		return cibles;
	}
	
	public string canLaunch1TileAwayOpponents(Tile tile, bool isM)
	{
		string isLaunchable = "Aucun ennemi à portée de lance";
		int playerID;
		this.targets = new List<Tile>();
		
		if(tile.x>1){
			playerID = this.tiles [tile.x-2, tile.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(this.getCard(playerID).isMine!=isM){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						isLaunchable="";
					}
				}
			}
		}
		if(tile.x<this.boardWidth-2){
			playerID = this.tiles [tile.x+2, tile.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(this.getCard(playerID).isMine!=isM){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						isLaunchable="";
					}
				}
			}
		}
		if(tile.y>1){
			playerID = this.tiles [tile.x, tile.y-2].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(this.getCard(playerID).isMine!=isM){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						isLaunchable="";
					}
				}
			}
		}
		if(tile.y<this.boardHeight-2){
			playerID = this.tiles [tile.x, tile.y+2].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(this.getCard(playerID).isMine!=isM){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						isLaunchable="";
					}
				}
			}
		}
		return isLaunchable;
	}
	
	public void hideTargets(){
		for (int i = 0 ; i < this.targets.Count ; i++){
			this.getTileController(this.targets[i].x, this.targets[i].y).displayTarget(false);
		}
		this.targets = new List<Tile>();
		this.getSkillZoneController().showCancelButton(false);
		this.getSkillZoneController().isRunningSkill = false ;
	}

	public void hideAllTargets(){
		for (int a = 0; a < this.boardWidth; a++){
			for (int b = 0; b < this.boardHeight; b++){
				this.getTileController(a,b).displayTarget(false);
				this.getTileController(a,b).showDescription(false);
			}
		}
	}
	
	public string canLaunchAdjacentOpponents(Tile tile, bool isM)
	{
		string isLaunchable = "Aucun ennemi à proximité";
		
		List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(tile, isM);
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getCard(playerID).isMine!=isM)
				{
					isLaunchable = "";
				}
			}
		}
		return isLaunchable;
	}

	public string canLaunchAdjacentCristoidOpponents(Tile tile, bool isM)
	{
		string isLaunchable = "Aucun ennemi à proximité de cristoides alliés";
		
		for(int i = 0 ; i < this.nbCards ; i++){
			if(i!=this.currentPlayingCard && this.getCard(i).CardType.Id==6 && this.getCard(i).isMine){
				List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(tile, isM);
				int playerID;
				foreach (Tile t in neighbourTiles)
				{
					playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
					if (playerID != -1)
					{
						if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getCard(playerID).isMine!=isM)
						{
							isLaunchable = "";
						}
					}
				}
			}
		}
		return isLaunchable;
	}

	public string canLaunchAdjacentDroidOpponents(Tile tile, bool isM)
	{
		string isLaunchable = "Aucun ennemi à proximité de cristoides alliés";
		
		for(int i = 0 ; i < this.nbCards ; i++){
			if(i!=this.currentPlayingCard && this.getCard(i).CardType.Id==5 && this.getCard(i).isMine){
				List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(tile, isM);
				int playerID;
				foreach (Tile t in neighbourTiles)
				{
					playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
					if (playerID != -1)
					{
						if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getCard(playerID).isMine!=isM)
						{
							isLaunchable = "";
						}
					}
				}
			}
		}
		return isLaunchable;
	}

	public string canLaunchAdjacentUnits(Tile tile)
	{
		string isLaunchable = "Aucune unité à proximité";
		
		List<Tile> neighbourTiles = this.getCharacterImmediateNeighbours(tile);
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if (playerID!=this.currentPlayingCard && this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
				{
					isLaunchable = "";
				}
			}
		}
		return isLaunchable;
	}

	public string canLaunchAdjacentRock(Tile tile)
	{
		string isLaunchable = "Aucun cristal à proximité";
		
		List<Tile> neighbourTiles = tile.getImmediateNeighbourTiles();
		foreach (Tile t in neighbourTiles)
		{
			if (this.getTileController(t).isRock())
			{
				isLaunchable = "";
			}
		}
		return isLaunchable;
	}

	public List<Tile> getAdjacentRockTargets(Tile t)
	{
		List<Tile> neighbourTiles = t.getImmediateNeighbourTiles();
		List<Tile> cibles = new List<Tile>();
				
		foreach (Tile t2 in neighbourTiles){
			if (this.getTileController(t2).isRock())
			{
				cibles.Add(t2);
			}
		}

		return cibles;
	}

	public string canLaunchMyUnit()
	{
		string isLaunchable = "";
		return isLaunchable;
	}
	
	public string canLaunchAdjacentTileTargets(Tile tile)
	{
		string isLaunchable = "Aucun terrain ne peut etre ciblé";
		
		List<Tile> neighbourTiles = this.getFreeImmediateNeighbours(tile);
		this.targets = new List<Tile>();
		
		if(neighbourTiles.Count>0){
			isLaunchable = "";
		}
		return isLaunchable;
	}
	
	public string canLaunchAdjacentAllys(Tile tile, bool isM)
	{
		string isLaunchable = "Aucun allié à proximité";
		
		List<Tile> neighbourTiles = this.getAllyImmediateNeighbours(tile, isM);
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getCard(playerID).isMine==isM)
				{
					isLaunchable = "";
				}
			}
		}
		return isLaunchable;
	}
	
	public string canLaunchOpponentsTargets(bool isM)
	{
		string isLaunchable = "Aucun ennemi ne peut etre atteint";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine!=isM && pcc.canBeTargeted())
			{
				isLaunchable = "";
			}
		}
		return isLaunchable;
	}
	
	public string canLaunchAllButMeTargets()
	{
		string isLaunchable = "Aucune unité ne peut etre atteint";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (pcc.canBeTargeted() && i != this.currentPlayingCard)
			{
				isLaunchable = "";
			}
		}
		return isLaunchable;
	}
	
	public string canLaunchAllysButMeTargets(bool isM)
	{
		string isLaunchable = "Aucun allié ne peut etre atteint";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine==isM && pcc.canBeTargeted() && i != this.currentPlayingCard)
			{
				isLaunchable = "";
			}
		}
		return isLaunchable;
	}

	public string canLaunchWoundedAllysButMeTargets(bool isM)
	{
		string isLaunchable = "Aucun allié n'est blessé";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine==isM && pcc.canBeTargeted() && i != this.currentPlayingCard)
			{
				if(this.getCard(i).getLife()!=this.getCard(i).GetTotalLife()){
					isLaunchable = "";
				}
			}
		}
		return isLaunchable;
	}

	public string canLaunchDead()
	{
		string isLaunchable = "Aucun allié n'est blessé n'est mort";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine && this.getCard(i).isDead)
			{
				isLaunchable = "";
			}
		}
		return isLaunchable;
	}
	
	public string canLaunchAnyone()
	{
		string isLaunchable = "Aucun ennemi ne peut etre atteint";
		
//		PlayingCardController pcc;
//		
//		for (int i = 0; i < this.playingCards.Count; i++)
//		{
//			pcc = this.getPCC(i);
//			if (pcc.canBeTargeted() && i != GameController.instance.getCurrentPlayingCard())
//			{
//				isLaunchable = "";
//			}
//		}
		return isLaunchable;
	}
	
	public void show(int target, bool showTR){
		this.playingCards[target].GetComponent<PlayingCardController>().show();
	}
	
	public void emptyTile(int x, int y)
	{
		this.tiles [x, y].GetComponent<TileController>().setCharacterID(-1);
	}

	public void displaySkillEffect(int target, string text, int type){
		this.getTileController(target).setSkillEffect(text,type);
	}

	public void displaySkillEffect(Tile t, string text, int type){
		this.getTileController(t).setSkillEffect(text,type);
	}
	
	public void removeLeaderEffect(int target, bool b){
		if(b){
			for(int j = 0 ; j < this.nbCards ; j++){
				if(this.getCard(j).isMine && target!=j){
					this.getCard(j).removeLeaderEffect();
					this.getPlayingCardController(j).updateLife(this.getCard(j).getLife());
					this.getPlayingCardController(j).show();
					this.displaySkillEffect(j, "Perd les bonus leader", 0);
					GameView.instance.addAnim(4,GameView.instance.getTile(j));
				}
			}
		}
		else{
			for(int j = 0 ; j < this.nbCards ; j++){
				if(!this.getCard(j).isMine && target!=j){
					this.getCard(j).removeLeaderEffect();
					this.getPlayingCardController(j).updateLife(this.getCard(j).getLife());
					this.getPlayingCardController(j).show();
					this.displaySkillEffect(j, "Perd les bonus leader", 0);
					GameView.instance.addAnim(4,GameView.instance.getTile(j));
				}
			}
		}
	}
	
	public void killHandle(int c, bool endTurn){
		this.toPassDead = endTurn ;

		if(this.getCard(this.currentPlayingCard).isSanguinaire()){
			GameCard currentCard = GameView.instance.getCurrentCard();
			int target = GameView.instance.getCurrentPlayingCard();
			int bonus = GameView.instance.getCurrentCard().Skills[0].Power*4;

			GameView.instance.getPlayingCardController(target).addBonusModifyer(new Modifyer(bonus, -1, 34, "Sanguinaire", ". Permanent."));
			GameView.instance.displaySkillEffect(target, "Dégats à distance +"+bonus+"%", 2);
			GameView.instance.addAnim(7,GameView.instance.getTile(target));
		}

		this.getPlayingCardController(c).displayDead(true);
		this.deads.Add(c);

		if(this.areAllMyPlayersDead()){
			GameView.instance.quitGameHandler();
		}
		else{
			this.updateTimeline();
		}
	}
	
	public IEnumerator sendStat(string idUser1, string idUser2, int rankingPoints1, int rankingPoints2, int gameType, int percentageTotalDamages, int currentGameid, bool hasWon, bool connectionLost)
	{
        int hasWonInt = 0;
        if(hasWon)
        {
            hasWonInt=1;
        }
        int isConnectionLostInt = 0;
        if(connectionLost)
        {
            isConnectionLostInt=1;
        }
		
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick1", idUser1); 	                    // Pseudo de l'utilisateur victorieux
		form.AddField("myform_nick2", idUser2); 	                    // Pseudo de l'autre utilisateur
        form.AddField("myform_rp1", rankingPoints1);                       // Pseudo de l'utilisateur victorieux
        form.AddField("myform_rp2", rankingPoints2);
		form.AddField("myform_gametype", ApplicationModel.player.ChosenGameType);
		form.AddField("myform_percentagelooser",percentageTotalDamages);
        form.AddField("myform_currentgameid",currentGameid);
        form.AddField("myform_haswon",hasWonInt);
        form.AddField("myform_connectionlost",isConnectionLostInt);

        ServerController.instance.setRequest(URLStat, form);
        yield return ServerController.instance.StartCoroutine("executeRequest");

        if(ServerController.instance.getError()!="")
        {
            Debug.Log(ServerController.instance.getError());
            ServerController.instance.lostConnection();
        }
	}
    public void quitGameHandler()
    {
        GameController.instance.quitGameHandler(this.isFirstPlayer);
    }
	public IEnumerator quitGame(bool hasFirstPlayerLost, bool isConnectionLost)
	{		
        ApplicationModel.player.MyDeck=GameView.instance.getMyDeck();
        if(ApplicationModel.player.ToLaunchGameTutorial)
		{
            if(hasFirstPlayerLost==this.isFirstPlayer)
			{
                ApplicationModel.player.HasWonLastGame=false;
                yield return (StartCoroutine(ApplicationModel.player.setTutorialStep(3)));
			}
			else
			{
                ApplicationModel.player.HasWonLastGame=true;
                yield return (StartCoroutine(ApplicationModel.player.setTutorialStep(2)));
			}
		}
		else
		{
            if(hasFirstPlayerLost==this.isFirstPlayer)
            {
                ApplicationModel.player.HasWonLastGame=false;
                ApplicationModel.player.PercentageLooser=GameView.instance.getPercentageTotalDamages(false);
            }
            else
            {
                ApplicationModel.player.HasWonLastGame=true;
                ApplicationModel.player.PercentageLooser=GameView.instance.getPercentageTotalDamages(true);
            }
            yield return (StartCoroutine(this.sendStat(ApplicationModel.myPlayerName,ApplicationModel.hisPlayerName,ApplicationModel.player.RankingPoints,ApplicationModel.hisRankingPoints,ApplicationModel.player.ChosenGameType,ApplicationModel.player.PercentageLooser,ApplicationModel.currentGameId,ApplicationModel.player.HasWonLastGame,isConnectionLost)));
		}
        GameController.instance.quitGame();
	}
	public List<Tile> getFreeImmediateNeighbours(Tile t){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getTileType()==0 && this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()==-1){
				freeNeighbours.Add(neighbours[i]);
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getDestinationImmediateNeighbours(Tile t, bool isM){
		bool b ; 
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			b = false ;
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(this.getCard(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()).isMine==isM){
					b = true ;
				}
			}
			else{
				b = true ; 
			}
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getTileType()==0 && b){
				freeNeighbours.Add(neighbours[i]);
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getDestinationHisImmediateNeighbours(Tile t, bool isM){
		bool b ; 
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			b = false ;
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(this.getCard(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()).isMine!=isM){
					b = true ;
				}
			}
			else{
				b = true ; 
			}
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getTileType()==0 && b){
				freeNeighbours.Add(neighbours[i]);
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getOpponentImmediateNeighbours(Tile t, bool isM){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(this.getCard(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()).isMine!=isM){
					freeNeighbours.Add(neighbours[i]);
				}
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getCharacterImmediateNeighbours(Tile t){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				freeNeighbours.Add(neighbours[i]);
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getAllyImmediateNeighbours(Tile t, bool isM){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(this.getCard(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()).isMine==isM){
					freeNeighbours.Add(neighbours[i]);
				}
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getNeighbours(Tile t){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				freeNeighbours.Add(neighbours[i]);
			}
		}
		return freeNeighbours ;
	}
	
	public List<int> getAllys(bool isM){
		List<int> allys = new List<int>();
		int CPC = GameView.instance.getCurrentPlayingCard();
		for(int i = 0 ; i < this.nbCards; i++){
			if(i!=CPC && !this.getCard(i).isDead && this.getCard(i).isMine==isM){
				allys.Add(i);
			}
		}
		return allys;
	}

	public List<int> getOpponents(bool isM){
		List<int> opponents = new List<int>();
		for(int i = 0 ; i < this.nbCards;i++){
			if(!this.getCard(i).isDead && this.getCard(i).isMine!=isM){
				opponents.Add(i);
			}
		}
		return opponents;
	}
	
	public List<int> getEveryone(){
		List<int> everyone = new List<int>();
		for(int i = 0 ; i < this.nbCards;i++){
			if(!this.getCard(i).isDead){
				everyone.Add(i);
			}
		}
		return everyone;
	}
	
	public List<int> getEveryoneButMe(){
		List<int> everyone = new List<int>();
		for(int i = 0 ; i < this.nbCards;i++){
			if(!this.getCard(i).isDead && i!=this.currentPlayingCard){
				everyone.Add(i);
			}
		}
		return everyone;
	}

	public List<int> getEveryoneCardtype(int value){
		List<int> everyone = new List<int>();
		for(int i = 0 ; i < this.nbCards ; i++){
			if(!this.getCard(i).isDead && i!=this.currentPlayingCard && this.getCard(i).CardType.Id==value){
				everyone.Add(i);
			}
		}
		return everyone;
	}

	public List<int> getEveryoneNextCristal(){
		List<int> everyone = new List<int>();
		List<Tile> neighbours = new List<Tile>();
		bool hasFoundCristal ;
		for(int i = 0 ; i < this.nbCards;i++){
			if(!this.getCard(i).isDead && i!=this.currentPlayingCard){
				neighbours = this.getTile(i).getImmediateNeighbourTiles();
				hasFoundCristal = false ;
				for(int j = 0 ; j < neighbours.Count ; j++){
					if(this.getTileController(neighbours[j]).isRock()){
						hasFoundCristal = true ;
					}
				}
				if(hasFoundCristal){
					everyone.Add(i);
				}
			}
		}
		return everyone;
	}

	public string canLaunchNextCristal(){
		string s = "Pas d'ennemis à proximité de cristaux";
		List<Tile> neighbours = new List<Tile>();
		for(int i = 0 ; i < this.nbCards;i++){
			if(!this.getCard(i).isDead && i!=this.currentPlayingCard){
				neighbours = this.getTile(i).getImmediateNeighbourTiles();
				for(int j = 0 ; j < neighbours.Count ; j++){
					if(this.getTileController(neighbours[j]).isRock()){
						s = "";
					}
				}
			}
		}
		return s;
	}
	
	public int countAlive(){
		int compteur = 0 ;
		for (int i = 0 ; i < this.nbCards ; i++){
			if (!this.getCard(i).isDead){
				compteur++;
			}
		}
		return compteur ;
	}
	
	public bool areAllMyPlayersDead(){
		bool areMyPlayersDead = true ;
		if(ApplicationModel.player.ToLaunchGameTutorial){
			areMyPlayersDead = false ;
			for (int i = 0 ; i < 8 ; i++){
				if (this.getCard(i).isMine){
					if (!this.getCard(i).isDead){
						areMyPlayersDead = false ;
					}
				}
			}
			if(!areMyPlayersDead){
				areMyPlayersDead = true ;
				for (int i = 0 ; i < 8 ; i++){
					if (!this.getCard(i).isMine){
						if (!this.getCard(i).isDead){
							areMyPlayersDead = false ;
						}
					}
				}
			}
		}
		else{
			areMyPlayersDead = true ;
			for (int i = 0 ; i < 8 ; i++){
				if (this.getCard(i).isMine){
					if (!this.getCard(i).isDead){
						areMyPlayersDead = false ;
					}
				}
			}
		}
		return areMyPlayersDead ;
	}

	public int getPercentageTotalDamages(bool isMe){
		int damages = 0;
		int total = 0;
		for(int i = 0 ; i < this.nbCards ; i++){
			if(this.getCard(i).isMine==isMe){
				damages+=(this.getCard(i).GetTotalLife()-this.getCard(i).getLife());
				total+=this.getCard(i).GetTotalLife();
			}
		}
		return Mathf.FloorToInt(100*damages/total);
	}

	public int attackClosestCharacter(){
		List<Tile> destinations = this.getPlayingCardController(this.currentPlayingCard).getDestinations();
		destinations.Add(this.getTile(this.currentPlayingCard));
		List<Tile> tempTiles ;
		Tile chosenTile = null;
		int bestDistance = 20 ;
		int distance = 0;
		int character = -1;
		Tile t;
		for(int i = 0 ; i < destinations.Count ; i++){
			t = destinations[i];
			tempTiles = t.getImmediateNeighbourTiles();
			for(int j = 0 ; j < tempTiles.Count ; j++){
				if (this.getTileController(tempTiles[j]).getCharacterID()!=-1 && this.getTileController(tempTiles[j]).getCharacterID()!=this.currentPlayingCard){
					distance = Mathf.Abs(tempTiles[j].x-t.x)+Mathf.Abs(tempTiles[j].y-t.y);
					if(distance < bestDistance){
						bestDistance = distance ;
						chosenTile = t;
						character = this.getTileController(tempTiles[j]).getCharacterID();
					}
				}
			}
		}

		if(character!=-1){
			if(distance > 0){
				GameController.instance.clickDestination(chosenTile, this.currentPlayingCard, true);
			}
		}
		else{
			List<int> opponents = GameView.instance.getOpponents(this.getCurrentCard().isMine);
			Tile t2 ;
			bestDistance = 20;
			for(int i = 0 ; i < destinations.Count ; i++){
				t = destinations[i];
				for(int j = 0 ; j < opponents.Count ; j++){
					t2 = this.getTile(opponents[j]);
					distance = Mathf.Abs(t2.x-t.x)+Mathf.Abs(t2.y-t.y);
					if(distance < bestDistance){
						bestDistance = distance ;
						chosenTile = t;
					}
				}
			}
			GameController.instance.clickDestination(chosenTile, this.currentPlayingCard, true);
		}
		return character ;
	}

	public int attackClosestEnnemy(){
		List<Tile> destinations = this.getPlayingCardController(this.currentPlayingCard).getDestinations();
		destinations.Add(this.getTile(this.currentPlayingCard));
		List<Tile> tempTiles ;
		Tile chosenTile = null;
		int bestDistance = 20 ;
		int distance = 0;
		int character = -1;
		Tile t;
		for(int i = 0 ; i < destinations.Count ; i++){
			t = destinations[i];
			tempTiles = t.getImmediateNeighbourTiles();
			for(int j = 0 ; j < tempTiles.Count ; j++){
				if (this.getTileController(tempTiles[j]).getCharacterID()!=-1 && this.getTileController(tempTiles[j]).getCharacterID()!=this.currentPlayingCard){
					if (this.getCard(this.getTileController(tempTiles[j]).getCharacterID()).isMine){
						distance = Mathf.Abs(tempTiles[j].x-t.x)+Mathf.Abs(tempTiles[j].y-t.y);
						if(distance < bestDistance){
							bestDistance = distance ;
							chosenTile = t;
							character = this.getTileController(tempTiles[j]).getCharacterID();
						}
					}
				}
			}
		}

		if(character!=-1){
			if(distance > 0){
				GameController.instance.clickDestination(chosenTile, this.currentPlayingCard, true);
			}
		}
		else{
			List<int> opponents = GameView.instance.getAllys(this.getCurrentCard().isMine);
			Tile t2 ;
			distance = 0 ;
			bestDistance = 20;
			for(int i = 0 ; i < destinations.Count ; i++){
				t = destinations[i];
				for(int j = 0 ; j < opponents.Count ; j++){
					if (this.getCard(opponents[j]).isMine){
						t2 = this.getTile(opponents[j]);
						distance = Mathf.Abs(t2.x-t.x)+Mathf.Abs(t2.y-t.y);
						if(distance < bestDistance){
							bestDistance = distance ;
							chosenTile = t;
						}
					}
				}
			}
			GameController.instance.clickDestination(chosenTile, this.currentPlayingCard, true);
		}
		return character ;
	}
	
	public void hideLoadingScreen()
	{
		if(isLoadingScreenDisplayed)
		{
			Destroy (this.loadingScreen);
			this.isLoadingScreenDisplayed=false;
		}
	}
	
	public Sprite getSkillSprite(int i){
		return this.skillSprites[i];
	}

	public Sprite getSkillTypeSprite(int i){
		return this.skillTypeSprites[i];
	}
	
	public Sprite getCharacterSprite(int i){
		return this.sprites[i];
	}
	
	public void play(int r)
	{	
		this.setLaunchability("Compétence en cours");
		this.runningSkill = r ;

		this.getPassZoneController().show(false);

		if(this.getCurrentCard().isMine){
			this.getSkillZoneController().showCancelButton(true);
		}
		this.getSkillZoneController().showSkillButtons(false);

		this.removeDestinations();
	}
	
	public IEnumerator endPlay()
	{
		this.getCard(this.currentPlayingCard).hasPlayed = true ;
		this.skillZone.GetComponent<SkillZoneController>().showCancelButton(false);
		this.getSkillZoneController().isRunningSkill=false;
		if(this.runningSkill==9){
			this.getCurrentSkill().hasBeenPlayed = true ;
		}
		else if(this.runningSkill==93){
			this.getCard(this.currentPlayingCard).hasMoved = true ;
		}
		else if(this.runningSkill==15){
			this.getCard(this.currentPlayingCard).hasMoved = false ;
		}
		this.runningSkill = -1;
		if(this.getCard(this.currentPlayingCard).isMine){
			if(this.getCard(this.currentPlayingCard).hasPlayed && this.getCard(this.currentPlayingCard).hasMoved){
				yield return new WaitForSeconds(3f);
				if(!this.deads.Contains(this.currentPlayingCard)){
					if(!ApplicationModel.player.ToLaunchGameTutorial || this.sequenceID>14){
						GameController.instance.findNextPlayer (true);
					}
				}
			}
			else{
				this.updateActionStatus();
			}
		}
		else{
			this.updateActionStatus();
		}
		yield break;
	}
	
	public GameCard getCurrentCard(){
		return this.getCard(this.currentPlayingCard);
	}

	public Tile getCurrentCardTile(){
		return this.getTile(this.currentPlayingCard);
	}
	
	public void initTileTargetHandler(int numberOfExpectedTargets)
	{
		this.targetTileHandler = new TargetTileHandler(numberOfExpectedTargets);
		this.hideSkillEffects();
	}
	
	public void hitTarget(Tile t){
		this.targetTileHandler.addTargetTile(t);
	}
	
	public Skill getCurrentSkill(){
		return this.getCurrentCard().findSkill(this.runningSkill);
	}
	
	public void hideTuto(){
		this.interlude.GetComponent<InterludeController>().unPause();
	}
	
	public void addAnim(int i, Tile t){
		this.anims.Add(t);
		this.getTileController(t).setAnimIndex(i);
		this.getTileController(t).displayAnim(true);
	}
	
	public void removeAnim(Tile t){
		this.getTileController(t).displayAnim(false);
		for(int i = anims.Count-1 ; i >= 0 ; i--){
			if(anims[i].x==t.x && anims[i].y==t.y){
				anims.RemoveAt(i);
			}
		}
		this.addSE(t);
	}

	public void addSE(Tile t){
		this.getTileController(t).isFinishedTransi = false ;
		this.skillEffects.Add(t);
		this.getTileController(t).showEffect(true);
	}

	public void removeSE(Tile t){
		for(int i = skillEffects.Count-1 ; i >= 0 ; i--){
			if(skillEffects[i].x==t.x && skillEffects[i].y==t.y){
				skillEffects.RemoveAt(i);
			}
		}
		this.getTileController(t.x,t.y).showEffect(false);
	}

	public void removeDead(int c){
		for(int i = deads.Count-1 ; i >= 0 ; i--){
			if(deads[i]==c){
				deads.RemoveAt(i);
			}
		}
		if(toPassDead){
			toPassDead = false ;
			this.setNextPlayer(false);
		}
	}
	
	public Tile getTile(int c){
		return this.getPlayingCardController(c).getTile();
	}

	public void launchValidationButton(string s, string d){
		if(!GameView.instance.isMobile){
			this.validationSkill.GetComponent<SkillValidationController>().setTexts(s,d,"Lancer");
			this.validationSkill.GetComponent<SkillValidationController>().show(true);
			this.isDisplayedPopUp = true ;
		}
	}

	public void hideValidationButton(){
		this.validationSkill.GetComponent<SkillValidationController>().show(false);
		this.isDisplayedPopUp = false ;
	}

	public void setHoveringZone(int i, string t, string d){
		this.hoveringZone = i ;
		if(i>0){
			for (int a = 0; a < this.boardWidth; a++){
				for (int b = 0; b < this.boardHeight; b++){
					if(this.getTileController(a,b).getCharacterID()!=-1){
						this.getTileController(a,b).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(this.getTileController(a,b).getCharacterID()));
					}
				}
			}
		}
	}

	public int getBonusMeteorites(){
		int bonus = 100 ; 
		for (int i = 0 ; i < this.nbCards ; i++){
			if(this.getCard(i).isAstronome() && !this.getCard(i).isDead){
				bonus += 50+10*this.getCard(i).Skills[0].Power;
			}
		}
		return bonus;
	}

	public void hideEndTurnPopUp(){
		this.isFreezed = false ;
		Tile t ;
		int amount = 10 ;
		int amount2 ; 
		int bonus = this.getBonusMeteorites();

		amount = Mathf.RoundToInt(amount * bonus / 100f);
		SoundController.instance.playSound(25);

		for(int i = 0 ; i < boardWidth ; i++){
			t = new Tile(i,0);
			if(this.getTileController(t).getCharacterID()!=-1){
				if(this.getCard(this.getTileController(t).getCharacterID()).isSniperActive()){
					amount2 = Mathf.RoundToInt((0.5f-0.05f*this.getCard(this.getTileController(t).getCharacterID()).Skills[0].Power)*amount*nbTurns);
				}
				else{
					amount2 = amount*nbTurns ;
				}
				GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount2+"PV", 0);
				GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount2,-1,1,"Attaque",amount2+" dégats subis"), false, -1);
				GameView.instance.addAnim(3,GameView.instance.getTile(this.getTileController(t).getCharacterID()));
			}
			else{
				GameView.instance.displaySkillEffect(t, "", 0);
				GameView.instance.addAnim(3,t);
			}
			t = new Tile(i,boardHeight-1);
			if(this.getTileController(t).getCharacterID()!=-1){
				if(this.getCard(this.getTileController(t).getCharacterID()).isSniperActive()){
					amount2 = Mathf.RoundToInt((0.5f-0.05f*this.getCard(this.getTileController(t).getCharacterID()).Skills[0].Power)*amount*nbTurns);
				}
				else{
					amount2 = amount*nbTurns ;
				}
				GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount2+"PV", 0);
				GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount2,-1,1,"Attaque",amount2+" dégats subis"), false, -1);
				GameView.instance.addAnim(3,GameView.instance.getTile(this.getTileController(t).getCharacterID()));
			}
			else{
				GameView.instance.displaySkillEffect(t, "", 0);
				GameView.instance.addAnim(3,t);
			}
		}

		if(nbTurns>=2){
			for(int i = 0 ; i < boardWidth ; i++){
				t = new Tile(i,1);
					if(this.getTileController(t).getCharacterID()!=-1){
					if(this.getCard(this.getTileController(t).getCharacterID()).isSniperActive()){
						amount2 = Mathf.RoundToInt((0.5f-0.05f*this.getCard(this.getTileController(t).getCharacterID()).Skills[0].Power)*amount*(nbTurns-1));
					}
					else{
						amount2 = amount*(nbTurns-1) ;
					}
					GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount2+"PV", 0);
					GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount2,-1,1,"Attaque",amount2+" dégats subis"), false, -1);
					GameView.instance.addAnim(3,GameView.instance.getTile(this.getTileController(t).getCharacterID()));
				}
				else{
					GameView.instance.displaySkillEffect(t, "", 0);
					GameView.instance.addAnim(3,t);
				}
				t = new Tile(i,boardHeight-2);
				if(this.getTileController(t).getCharacterID()!=-1){
					if(this.getCard(this.getTileController(t).getCharacterID()).isSniperActive()){
						amount2 = Mathf.RoundToInt((0.5f-0.05f*this.getCard(this.getTileController(t).getCharacterID()).Skills[0].Power)*amount*(nbTurns-1));
					}
					else{
						amount2 = amount*(nbTurns-1) ;
					}
					GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount2+"PV", 0);
					GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount2,-1,1,"Attaque",amount2+" dégats subis"), false, -1);
					GameView.instance.addAnim(3,GameView.instance.getTile(this.getTileController(t).getCharacterID()));
				}
				else{
					GameView.instance.displaySkillEffect(t, "", 0);
					GameView.instance.addAnim(3,t);
				}
			}
		}

		if(nbTurns>=3){
			for(int i = 0 ; i < boardWidth ; i++){
				t = new Tile(i,2);
				if(this.getTileController(t).getCharacterID()!=-1){
					if(this.getCard(this.getTileController(t).getCharacterID()).isSniperActive()){
						amount2 = Mathf.RoundToInt((0.5f-0.05f*this.getCard(this.getTileController(t).getCharacterID()).Skills[0].Power)*amount*(nbTurns-2));
					}
					else{
						amount2 = amount *(nbTurns-2);
					}
					GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount2+"PV", 0);
					GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount2,-1,1,"Attaque",amount2+" dégats subis"), false, -1);
					GameView.instance.addAnim(3,GameView.instance.getTile(this.getTileController(t).getCharacterID()));
				}
				else{
					GameView.instance.displaySkillEffect(t, "", 0);
					GameView.instance.addAnim(3,t);
				}
				t = new Tile(i,boardHeight-3);
				if(this.getTileController(t).getCharacterID()!=-1){
					if(this.getCard(this.getTileController(t).getCharacterID()).isSniperActive()){
						amount2 = Mathf.RoundToInt((0.5f-0.05f*this.getCard(this.getTileController(t).getCharacterID()).Skills[0].Power)*amount*(nbTurns-2));
					}
					else{
						amount2 = amount*(nbTurns-2) ;
					}
					GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount2+"PV", 0);
					GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount2,-1,1,"Attaque",amount2+" dégats subis"), false, -1);
					GameView.instance.addAnim(3,GameView.instance.getTile(this.getTileController(t).getCharacterID()));
				}
				else{
					GameView.instance.displaySkillEffect(t, "", 0);
					GameView.instance.addAnim(3,t);
				}
			}
		}
		if(ApplicationModel.player.ToLaunchGameTutorial && GameView.instance.sequenceID<24){
				
		}
		else{
			StartCoroutine(this.waitThenLaunchEDE());
		}
	}

	public IEnumerator waitThenLaunchEDE(){
		yield return new WaitForSeconds(2f);
		this.setNextPlayer(true);
		yield break ;
	}

	public void clickSkillButton(int i){
		this.draggingSkillButton=i;
		this.getSkillZoneController().getSkillButtonController(draggingSkillButton).showCollider(false);
	}

	public void dropSkillButton(int i){
		Vector3 mousePos;
		if(i==0){
			mousePos = new Vector3(1.6f+stepButton, -4.4f, 0f);
		}
		else if(i==1){
			mousePos = new Vector3(2.6f+stepButton, -4.4f, 0f);
		}
		else if(i==2){
			mousePos = new Vector3(3.6f+stepButton, -4.4f, 0f);
		}
		else {
			mousePos = new Vector3(0.5f+stepButton, -4.4f, 0f);
		}

		this.getSkillZoneController().getSkillButtonController(draggingSkillButton).setPosition4(mousePos);
		this.getSkillZoneController().getSkillButtonController(draggingSkillButton).showDescription(false);
		this.hideTargets();
		this.cancelSkill();
		this.draggingSkillButton=-1;
	}

	public void cancelSkill(){
		this.getSkillZoneController().isRunningSkill = false ;
		this.runningSkill = -1;
		this.hoveringZone = -1;
		this.getSkillZoneController().updateButtonStatus(this.getCurrentCard());
		GameView.instance.passZone.GetComponent<PassController>().setLaunchability("");
		GameView.instance.passZone.GetComponent<PassController>().show(true);
	}

	public List<Tile> getAllTilesWithin(Tile t, int r){
		List<Tile> tiles = new List<Tile>();
		Tile tempTile ;
		for (int i = 0 ; i < boardWidth ; i++){
			for (int j = 0 ; j < boardHeight ; j++){
				tempTile = new Tile(i,j);
				if(this.getDistanceBetweenTiles(t,tempTile)<=r){
					if(this.getTileController(tempTile).getCharacterID()==-1 && !this.getTileController(tempTile).isRock()){
						tiles.Add(tempTile);
					}
				}
			}
		}
		return tiles ;
	}

	public List<Tile> getTrappedTiles(){
		List<Tile> tiles = new List<Tile>();
		Tile tempTile ;
		for (int i = 0 ; i < boardWidth ; i++){
			for (int j = 0 ; j < boardHeight ; j++){
				tempTile = new Tile(i,j);
				if(this.getTileController(tempTile).getIsTrapped()){
					if(!this.getTileController(tempTile).trap.getIsVisible()){
						tiles.Add(tempTile);
					}
				}
			}
		}
		return tiles ;
	}

	public int getDistanceBetweenTiles(Tile t1, Tile t2){
		return (Math.Abs(t1.x-t2.x)+Math.Abs(t1.y-t2.y));
	}

	public int countCristals(){
		int compteur = 0 ;
		for(int i = 0; i < this.boardWidth ; i++){
			for(int j = 0; j < this.boardHeight ; j++){
				if(this.getTileController(i,j).isRock()){
					compteur++;
				}
			}
		}
		return compteur ;
	}

	public void updateCristoEater(){
		int nbCristals = this.countCristals();
		int amount ;
		for(int i = 0 ; i < this.nbCards ; i++){
			if(this.getCard(i).isCristoMaster() && !this.getCard(i).isDead){
				amount = Mathf.Max(1,Mathf.RoundToInt(nbCristals*this.getCard(i).Skills[0].Power*this.getCard(i).Attack/100f));
				GameView.instance.getPlayingCardController(i).updateAttack(GameView.instance.getCard(i).getAttack());
				this.getCard(i).replaceCristoMasterModifyer(new Modifyer(amount,-1,139,"Cristomaster",amount+". Permanent."));
				GameView.instance.displaySkillEffect(i, "Cristomaitre\n+"+amount+" ATK", 2);
				GameView.instance.addAnim(0,GameView.instance.getTile(i));
			}
		}
	}

	public void hitNextTutorial(){
		print("nexttuto");
		if(this.sequenceID==0){
			this.initGrid();
			this.gameTutoController.setCompanion("Les champs de bataille cristaliens sont constitués de cases sur lesquelles les combattants se déplacent.", true, false, false, 0);
			this.gameTutoController.setBackground(true, new Rect(0f, 0f, 6f, 8f), 1f, 1f);
			this.gameTutoController.showSequence(true, true, false);
		}
		else if(this.sequenceID==1){
			this.gameTutoController.setCompanion("Certaines cases spéciales comme les cristaux possèdent des propriétés particulières. Cliquez sur un cristal pour découvrir sa fonction!", true, false, false, 0);
			this.gameTutoController.setBackground(true, new Rect(-0.5f, -1.5f, 1f, 1f), 1f, 1f);
			this.gameTutoController.setArrow("down",new Vector3(-0.5f,-0.8f,0f));
			this.gameTutoController.showSequence(true, true, true);
		}
		else if(this.sequenceID==2){
			this.loadMyDeck();
			this.gameTutoController.setCompanion("Voici vos unités! Les combats cristaliens opposent des équipes de 4 combattants. Chacun possède des points d'attaque (à gauche) et des points de vie (à droite).", true, false, false, 0);
			this.gameTutoController.setBackground(true, new Rect(0f, -3.5f, 4f, 1f), 0f, 0f);
			this.gameTutoController.setArrow("down",new Vector3(-0.5f,-3.2f,0f));
			this.gameTutoController.showSequence(true, true, true);
		}
		else if(this.sequenceID==3){
			this.gameTutoController.setCompanion("Avant le début de la bataille, vous pouvez positionner vos unités sur les deux premières rangées du terrain. Déplacez une unité pour continuer!", false, false, false, 0);
			this.gameTutoController.setBackground(true, new Rect(0f, -3f, 6f, 2f), 1f, 1f);
			this.gameTutoController.showSequence(true, true, false);
		}
		else if(this.sequenceID==4){
			this.gameTutoController.setCompanion("Bravo ! Positionnez maintenant le reste de vos troupes avant de démarrer le combat. N'oubliez pas de protéger vos unités possédant le moins de points de vie.", false, false, false, 0);
			this.gameTutoController.setBackground(true, new Rect(0f, -3.5f, 6f, 3f), 1f, 1f);
			this.gameTutoController.showSequence(true, true, false);
		}
		else if(this.sequenceID==5){
			this.gameTutoController.setCompanion("Votre adversaire a positionné ses unités, la bataille peut démarrer! Le premier joueur à avoir placé ses troupes commence le combat (Code de guerre Cristalien, article 2).", true, true, true, 1);
			this.gameTutoController.setBackground(true, new Rect(0f, 3f, 6f, 2f), 0f, 0f);
			this.gameTutoController.showSequence(true, true, false);
		}
		else if(this.sequenceID==6){
			string text = "";
			if(this.isMobile){
				text = "Il semblerait que votre adversaire dispose d'un LEADER, unité dangereuse car elle renforce toute son équipe. Vérifions en touchant l'unité.";
			}
			else{
				text = "Il semblerait que votre adversaire dispose d'un LEADER, unité dangereuse car elle renforce toute son équipe. Vérifions en survolant l'unité.";
			}
			this.gameTutoController.setCompanion(text, false, true, false, 1);
			this.gameTutoController.setBackground(true, new Rect(0.5f, 2.5f, 1f, 1f), 1f, 1f);
			this.gameTutoController.setArrow("up",new Vector3(0.5f,2.2f,0f));
			this.gameTutoController.showSequence(true, true, true);
		}
		else if(this.sequenceID==7){
			this.gameTutoController.setCompanion("Chaque unité dispose de compétences ACTIVES et PASSIVES. Les compétences actives, sur fond gris, sont utilisables par le joueur pendant le tour de l'unité", true, true, false, 1);
			this.gameTutoController.setBackground(true, new Rect(this.realwidth/4f+1.53f, 0f, this.realwidth/2f-3f, 10f), 0f, 0f);
			this.gameTutoController.setArrow("right",new Vector3(3.8f,-2.9f,0f));
			this.gameTutoController.showSequence(true, true, true);
		}
		else if(this.sequenceID==8){
			this.gameTutoController.setCompanion("Les compétences PASSIVES s'affichent sur un fond noir. Elles sont propres à chaque type d'unité et confèrent des bonus permanents pendant le combat.", true, true, false, 1);
			this.gameTutoController.setBackground(true, new Rect(this.realwidth/4f+1.53f, 0f, this.realwidth/2f-3f, 10f), 0f, 0f);
			this.gameTutoController.setArrow("right",new Vector3(3.8f,-3.5f,0f));
			this.gameTutoController.showSequence(true, true, true);
		}
		else if(this.sequenceID==9){
			this.gameTutoController.showArrow(false);
			this.gameTutoController.setCompanion("Une unité peut à chaque tour SE DEPLACER et DECLENCHER UNE COMPETENCE, dans n'importe quel ordre. Commencez par déplacer votre unité près de l'ennemi!", false, true, false, 1);
			this.hoverTile();
			this.gameTutoController.setBackground(true, new Rect(0f, 0f, 6f, 8f), 1f, 1f);
			this.gameTutoController.setArrow("up",new Vector3(0.5f,1.2f,0f));
			this.gameTutoController.showSequence(true, true, true);
		}
		else if(this.sequenceID==10){
			string text = "";
			if(this.isMobile){
				text = "Choisissez la compétence ATTAQUE dans le menu d'action et déplacer la sur la cible ennemie. Une unité ne peut attaquer en diagonale!";
			}
			else{
				text = "Cliquez sur la compétence ATTAQUE puis sur l'unité ennemie. Une unité ne peut attaquer en diagonale!";
			}
			this.gameTutoController.setCompanion(text, false, false, true, 2);
			this.gameTutoController.setBackground(true, new Rect(-this.realwidth/4f+1.53f, 0f, this.realwidth/2f+3f, 10f), 1f, 1f);
			this.gameTutoController.setArrow("right",new Vector3(-2.9f,-4.5f,0f));
			this.gameTutoController.showSequence(true, true, true);
		}
		else if(this.sequenceID==11){
			this.gameTutoController.setCompanion("Une fois la compétence utilisée, les effets sur les unités sont affichés. La destruction du leader a permis d'affaiblir les unités ennemies!", true, false, false, 2);
			this.gameTutoController.setBackground(true, new Rect(-this.realwidth/4f+1.53f, 0f, this.realwidth/2f+3f, 10f), 1f, 1f);
			this.gameTutoController.showSequence(true, true, false);
		}
		else if(this.sequenceID==12){
			this.hideSkillEffects();
			this.gameTutoController.setCompanion("Vous avez utilisé une compétence après vous être déplacé, votre tour est donc terminé! La timeline vous permet de consulter l'ordre des tours et de savoir quelles seront les prochaines unités à jouer", true, false, false, 2);
			this.gameTutoController.setBackground(true, new Rect(0f, 4.51f, 6f, 1f), 0f, 0f);
			this.gameTutoController.setArrow("up",new Vector3(0f,4.2f,0f));
			this.gameTutoController.showSequence(true, true, true);
		}
		else if(this.sequenceID==13){
			this.gameTutoController.showSequence(false, false, false);
			GameController.instance.findNextPlayer (true);
		}
		else if(this.sequenceID==14){
			this.gameTutoController.setCompanion("C'est maintenant le tour de votre adversaire! Profitez de son tour pour consulter ses unités ou planifier votre stratégie", true, true, true, 1);
			this.gameTutoController.setBackground(true, new Rect(this.realwidth/4f-1.53f, 0f, this.realwidth/2f+3f, 10f), 0f, 0f);
			this.gameTutoController.showSequence(true, true, false);
		}
		else if(this.sequenceID==15){
			this.gameTutoController.showSequence(false, false, false);
			StartCoroutine(launchIABourrin());
		}
		else if(this.sequenceID==16){
			this.gameTutoController.setCompanion("C'est maintenant le tour de votre paladin. Appartenant à la faction des MEDIC, cette unité peut soigner vos alliés grâce à sa compétence PISTOSOIN", true, false, true, 2);
			this.gameTutoController.setBackground(true, new Rect(-this.realwidth/4f+1.53f, 0f, this.realwidth/2f+3f, 10f), 0f, 0f);
			this.gameTutoController.showSequence(true, true, false);
		}
		else if(this.sequenceID==17){
			this.gameTutoController.setCompanion("Utilisez votre compétence PISTOSOIN pour rendre des points de vie (PV) à votre unité avancée", false, false, false, 2);
			this.gameTutoController.setBackground(true, new Rect(-this.realwidth/4f+1.53f, 0f, this.realwidth/2f+3f, 10f), 1f, 1f);
			this.gameTutoController.setArrow("right",new Vector3(-1.4f,-4.2f,0f));
			this.gameTutoController.showSequence(true, true, true);
		}
		else if(this.sequenceID==18){
			this.gameTutoController.setCompanion("Félicitations. Votre unité est soignée ! Vous pouvez terminer votre tour en déplaçant votre personnage ou en cliquant sur le bouton de fin de tour.", false, false, false, 2);
			this.gameTutoController.setBackground(true, new Rect(-this.realwidth/4f+1.53f, 0f, this.realwidth/2f+3f, 10f), 1f, 1f);
			this.gameTutoController.setArrow("left",new Vector3(2.2f,-4.2f,0f));
			this.gameTutoController.showSequence(true, true, true);
		}
		else if(this.sequenceID==19){
			this.gameTutoController.showSequence(false, false, false);
		}
		else if(this.sequenceID==20){
			this.gameTutoController.setCompanion("Au fil du combat, des météorites s'abattent sur le champ de bataille, infligeant des dégats aux unités sur les bords du champ de bataille", true, false, false, 0);
			this.gameTutoController.setBackground(true, new Rect(0f, 0f, 6f, 10f), 0f, 0f);
			this.gameTutoController.showSequence(true, true, false);
		}
		else if(this.sequenceID==21){
			this.hideSkillEffects();
			this.gameTutoController.setCompanion("Les météorites sont indiquées sur la timeline, ainsi que le nombre de rangées qu'elles toucheront de chaque coté du champ de bataille", true, false, false, 0);
			this.gameTutoController.setBackground(true, new Rect(0f, 4.5f, 6f, 1f), 0f, 0f);
			this.gameTutoController.showSequence(true, true, false);
		}
		else if(this.sequenceID==22){
			this.gameTutoController.setCompanion("Il ne vous reste plus à présent qu'à terminer ce combat d'entrainement avant d'arriver sur Cristalia. Bon courage à vous, terrien!", true, false, false, 0);
			this.gameTutoController.setBackground(true, new Rect(0f, 0f, 6f, 10f), 0f, 0f);
			this.gameTutoController.showSequence(true, true, false);
		}
		else if(this.sequenceID==23){
			this.gameTutoController.showSequence(false, false, false);
			StartCoroutine(this.waitThenLaunchEDE());
		}

		sequenceID++;
	}

	public void advanceTurn(int character){
		
		for(int i = 0 ; i < orderCards.Count ; i++){
			if(orderCards[i]==character){
				this.orderCards.RemoveAt(i);
			}
		} 

		for(int i = 0 ; i < orderCards.Count ; i++){
			if(orderCards[i]==this.currentPlayingCard){
				this.orderCards.Insert(i+1,character);
			}
		} 
		
		this.updateTimeline();
	}

	public int getMaxPVCard(){
		int pv = 0 ; 
		int chosenCard = -1 ;

		for(int i = 0 ; i < nbCards ; i++){
			if(!this.getCard(i).isDead){
				if(this.getCard(i).getLife()>pv){
					pv = this.getCard(i).getLife();
					chosenCard = i ;
				}
			}
		}
		return chosenCard;
	}

	public int getMinPVCard(){
		int pv = 1000 ; 
		int chosenCard = -1 ;

		for(int i = 0 ; i < nbCards ; i++){
			if(!this.getCard(i).isDead){
				if(this.getCard(i).getLife()<pv){
					pv = this.getCard(i).getLife();
					chosenCard = i ;
				}
			}
		}
		return chosenCard;
	}

	public void convert(int target){
		this.getCard(target).isMine = !this.getCard(target).isMine ;
		this.getPlayingCardController(target).show();
	}

	public int countTraps(){
		int nbTraps = 0;
		for(int i = 0 ; i < boardWidth ;i++){
			for(int j = 0 ; j < boardHeight ;j++){
				if(this.getTileController(i,j).getIsTrapped()){
					if(this.getTileController(i,j).trap.isVisible){
						nbTraps++;
					}
				}
			}
		}
		return nbTraps;
	}

	public int hasSoutien(bool isM){
		int soutien = -1 ;
		for(int i = 0 ; i < nbCards ; i++){
			if(!this.getCard(i).isDead){
				if(this.getCard(i).isMine==isM){
					if(this.getCard(i).isSoutien()){
						soutien = i ;
					}
				}
			}
		}
		return soutien;
	}

	public int getMinDistanceOpponent(Tile myTile, int j){
		bool isM = this.getCard(j).isMine;
		int minDistance = 99;
		int distance ;
		Tile tile ;
		List<int> everyone = this.getEveryone();
		for(int i = 0 ; i < everyone.Count ; i++){
			if(this.getCard(everyone[i]).isMine!=isM || this.getCard(everyone[i]).isFurious()){
				tile = this.getTile(everyone[i]);
				distance = Mathf.Abs(tile.x-myTile.x)+Mathf.Abs(tile.y-myTile.y);
				if(distance<minDistance){
					minDistance = distance ;
				}
			}
		}
		return minDistance ;
	}

	public void placeIACards(){
		int strategy = UnityEngine.Random.Range(1,11);
		Tile startingTile = new Tile(-1,-1) ;
		int tempInt ;
		List<int> units = this.getOpponents(true);
		int[,] tilesOccupancy = new int[6,2];
		for(int i = 0 ; i < this.boardWidth ; i++){
			for(int j = 6 ; j < this.boardHeight ; j++){
				if(this.getTileController(new Tile(i,j)).isRock()){
					tilesOccupancy[i,j-6]=-1;
				}
				else{
					tilesOccupancy[i,j-6]=0;
				}
			}
		}

		for(int i = 0 ; i < units.Count ; i++){
			if(this.getCard(units[i]).CardType.Id==0){
				tempInt = UnityEngine.Random.Range(1,101);
				if(tempInt<=25){
					startingTile = new Tile(3,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(3,1);
				}
				else if(tempInt<=75){
					startingTile = new Tile(2,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(2,1);
				}
			}
			else if(this.getCard(units[i]).CardType.Id==1){
				tempInt = UnityEngine.Random.Range(1,101);
				if(tempInt<=25){
					startingTile = new Tile(0,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(5,0);
				}
				else if(tempInt<=75){
					startingTile = new Tile(1,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,0);
				}
			}
			else if(this.getCard(units[i]).CardType.Id==2){
				tempInt = UnityEngine.Random.Range(1,101);
				if(tempInt<=25){
					startingTile = new Tile(2,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(1,0);
				}
				else if(tempInt<=75){
					startingTile = new Tile(3,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,0);
				}
			}
			else if(this.getCard(units[i]).CardType.Id==3){
				tempInt = UnityEngine.Random.Range(1,101);
				if(tempInt<=25){
					startingTile = new Tile(0,1);
				}
				else if(tempInt<=50){
					startingTile = new Tile(1,1);
				}
				else if(tempInt<=75){
					startingTile = new Tile(4,1);
				}
				else if(tempInt<=100){
					startingTile = new Tile(5,1);
				}
			}
			else if(this.getCard(units[i]).CardType.Id==4){
				tempInt = UnityEngine.Random.Range(1,101);
				if(tempInt<=25){
					startingTile = new Tile(1,1);
				}
				else if(tempInt<=50){
					startingTile = new Tile(3,1);
				}
				else if(tempInt<=75){
					startingTile = new Tile(5,1);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,0);
				}
			}
			else if(this.getCard(units[i]).CardType.Id==5){
				tempInt = UnityEngine.Random.Range(1,101);
				if(tempInt<=25){
					startingTile = new Tile(1,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(2,0);
				}
				else if(tempInt<=75){
					startingTile = new Tile(3,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,0);
				}
			}
			else if(this.getCard(units[i]).CardType.Id==6){
				tempInt = UnityEngine.Random.Range(1,101);
				if(tempInt<=25){
					startingTile = new Tile(2,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(3,0);
				}
				else if(tempInt<=75){
					startingTile = new Tile(3,1);
				}
				else if(tempInt<=100){
					startingTile = new Tile(2,1);
				}
			}
			else if(this.getCard(units[i]).CardType.Id==7){
				tempInt = UnityEngine.Random.Range(1,101);
				if(tempInt<=25){
					startingTile = new Tile(1,1);
				}
				else if(tempInt<=50){
					startingTile = new Tile(0,1);
				}
				else if(tempInt<=75){
					startingTile = new Tile(5,1);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,1);
				}
			}
			else if(this.getCard(units[i]).CardType.Id==8){
				tempInt = UnityEngine.Random.Range(1,101);
				if(tempInt<=25){
					startingTile = new Tile(0,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(1,0);
				}
				else if(tempInt<=75){
					startingTile = new Tile(4,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(5,0);
				}
			}
			else if(this.getCard(units[i]).CardType.Id==9){
				tempInt = UnityEngine.Random.Range(1,101);
				if(tempInt<=25){
					startingTile = new Tile(1,1);
				}
				else if(tempInt<=50){
					startingTile = new Tile(4,1);
				}
				else if(tempInt<=75){
					startingTile = new Tile(1,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,0);
				}
			}

			while(tilesOccupancy[startingTile.x, startingTile.y]!=0){
				startingTile = new Tile(UnityEngine.Random.Range(0,6), UnityEngine.Random.Range(0,2));
			}
			tilesOccupancy[startingTile.x, startingTile.y]=10+i;


		}

		for(int i = 0 ; i < this.boardWidth ; i++){
			for(int j = 0 ; j < 2 ; j++){
				this.tiles[i,j+6].GetComponentInChildren<TileController>().setCharacterID(-1);
			}
		}

		for(int i = 0 ; i < this.boardWidth ; i++){
			for(int j = 0 ; j < 2 ; j++){
				if(tilesOccupancy[i,j]>9){
					this.getPlayingCardController(units[tilesOccupancy[i,j]-10]).changeTile(new Tile(i,j+6), this.tiles[i,j+6].GetComponentInChildren<TileController>().getPosition());
					this.tiles[i,j+6].GetComponentInChildren<TileController>().setCharacterID(units[tilesOccupancy[i,j]-10]);
				}
			}
		}
	}

	public int getMaxAttack(bool isM){
		int maxAttack = 0 ; 
		List<int> ennemis = this.getOpponents(isM);
		for(int i = 0 ; i < ennemis.Count ; i++){
			if(this.getCard(ennemis[i]).getAttack()>maxAttack){
				maxAttack = this.getCard(ennemis[i]).getAttack();
			}
		}
		return maxAttack ;
	}
}
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
	public GameObject backgroundImageModel;
	public GameObject playingCardModel;
	public GameObject skillButtonModel;
	public GameObject TutorialObject;
	public Sprite[] sprites;
	public Sprite[] factionSprites;
	public Sprite[] skillSprites;
	public Sprite[] skillTypeSprites;
	public Sprite[] cardTypeSprites;
	public Sprite[] iconSprites;

	public int boardWidth ;
	public int boardHeight ;
	public int nbCardsPerPlayer ;
	public int nbFreeRowsAtBeginning ;

	bool isLoadingScreenDisplayed = false ;
	
	GameObject loadingScreen;
	GameObject[,] tiles ;
	GameObject myHoveredRPC ;
	GameObject hisHoveredRPC ;
	GameObject[] verticalBorders ;
	GameObject[] horizontalBorders ;
	GameObject[] playingCards ;
	GameObject popUp;
	GameObject endTurnPopUp;
	public GameObject choicePopUp;
	GameObject validationSkill;
	TimelineController timeline;

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
	TargetPCCHandler targetPCCHandler ;
	TargetTileHandler targetTileHandler ;
	
	public bool amIReadyToFight= false ;
	public bool isHeReadyToFight= false ;

	public bool hasStep3 ;
	public bool hasStep2 ;
	public bool blockFury ;
	bool toPassDead = false ;

	public bool isFreezed = false ;
	public bool isDisplayedPopUp = false ;
	public int hoveringZone = -1 ;
	public bool toCountTime = true ;

	public int draggingCard ;
	public int draggingSkillButton ;
	int nbTurns ;
	int numberDeckLoaded ;

	int nbCards = 8 ;
	bool isFirstPlayerStarting ;

	float timerTurn ; 
	public float turnTime = 30f;
	public float tileScale;

	public bool isMobile;
	public float stepButton;
	public float timeDragging = -1 ;
	public int clickedCharacterId=-1;

	List<int> orderCards ; 
	int meteoritesCounter = 8 ; 
	int meteoritesStep = 1 ; 
	int lastPlayingCard = -1 ;
	
	void Awake()
	{
		instance = this;
		this.displayLoadingScreen ();
		this.tiles = new GameObject[this.boardWidth, this.boardHeight];
		this.playingCards = new GameObject[100];
		this.verticalBorders = new GameObject[this.boardWidth+1];
		this.horizontalBorders = new GameObject[this.boardHeight+1];
		this.myHoveredRPC = GameObject.Find("MyHoveredPlayingCard");
		this.hisHoveredRPC = GameObject.Find("HisHoveredPlayingCard");
		
		this.SB = GameObject.Find("SB");
		this.interlude = GameObject.Find("Interlude");
		this.passZone = GameObject.Find("PassZone");
		this.skillZone = GameObject.Find("SkillZone");
		this.popUp = GameObject.Find("PopUp");
		this.endTurnPopUp = GameObject.Find("EndTurnPopUp");
		this.choicePopUp = GameObject.Find("PopUpChoice");
		this.timeline = GameObject.Find("Timeline").GetComponent<TimelineController>();

		this.validationSkill = GameObject.Find("ValidationAutoSkill");
		this.popUp.GetComponent<PopUpGameController>().show (false);
		this.validationSkill.GetComponent<SkillValidationController>().show(false);

		this.SB.GetComponent<StartButtonController>().show(false);
		this.setMyPlayerName(ApplicationModel.myPlayerName);
		this.setHisPlayerName(ApplicationModel.hisPlayerName);
		this.isFirstPlayer = ApplicationModel.player.IsFirstPlayer;
		this.runningSkill=-1;
		this.createBackground();
		this.targets = new List<Tile>();
		this.skillEffects = new List<Tile>();
		this.anims = new List<Tile>();
		this.deads = new List<int>();
		this.orderCards = new List<int>();
		//this.destinations

		if (this.isFirstPlayer)
		{
			this.initGrid();
		}
		this.hasFightStarted = false ;
		this.hasStep3 = false ;
		this.hasStep2 = false;
		this.blockFury = false;

		if(ApplicationModel.player.ToLaunchGameTutorial){
			this.turnTime = 1200;
		}
		else{
			this.turnTime = 480;
		}
		toCountTime = true ;

		draggingCard = -1 ;
		draggingSkillButton = -1 ;
		this.nbTurns = 0 ;
		this.numberDeckLoaded = 0 ;
		this.isFirstPlayerStarting=true;

		if(ApplicationModel.player.ToLaunchGameTutorial){
			this.hideTuto();
				
			List<Skill> skills = new List<Skill>();
			skills.Add (new Skill("Aguerri", 68, 1, 1, 2, 0, "", 0, 0));
			skills.Add (new Skill("Frénésie", 18, 1, 2, 6, 0, "", 0, 80));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			Card c1 = new Card(-1, "Predator", 35, 2, 0, 3, 16, skills);
			c1.deckOrder=0;
			GameCard g1 = new GameCard(c1);
			g1.LifeLevel=1;
			g1.AttackLevel=1;
			g1.PowerLevel=1;
			this.createPlayingCard(g1, false);
			
			skills = new List<Skill>();
			skills.Add (new Skill("Furtif", 66, 1, 1, 3, 0, "", 0, 0));
			skills.Add (new Skill("Estoc", 11, 1, 1, 1, 0, "", 0, 80));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			c1 = new Card(-1, "Flash", 24, 1, 0, 6, 11, skills);
			c1.deckOrder=1;
			g1 = new GameCard(c1);
			g1.LifeLevel=2;
			g1.AttackLevel=3;
			g1.PowerLevel=1;
			this.createPlayingCard(g1, false);
			
			skills = new List<Skill>();
			skills.Add (new Skill("Rapide", 71, 1, 1, 4, 0, "", 0, 0));
			skills.Add (new Skill("Massue", 63, 1, 1, 1, 0, "", 0, 100));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			skills.Add (new Skill("Aguerri", 68, 0, 0, 2, 0, "", 0, 0));
			c1 = new Card(-1, "Alien", 38, 2, 0, 3, 21, skills);
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
			c1 = new Card(-1, "Psycho", 42, 2, 0, 2, 17, skills);
			c1.deckOrder=3;
			g1 = new GameCard(c1);
			g1.LifeLevel=2;
			g1.AttackLevel=1;
			g1.PowerLevel=1;
			this.createPlayingCard(g1, false);
		}
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
		GameController.instance.launchCardCreation();
	}
	
	public IEnumerator loadMyDeck()
	{
		Deck myDeck = new Deck(ApplicationModel.myPlayerName);
		yield return StartCoroutine(myDeck.LoadDeck());
		
		GameController.instance.spawnCharacter(myDeck.Id);
		if(ApplicationModel.player.ToLaunchGameTutorial){
			this.launchTutoStep(1);
		}
	}
	
	public void createTile(int x, int y, int type, bool isFirstP)
	{
		Vector3 position ;
		
		this.tiles [x, y] = (GameObject)Instantiate(this.tileModel);
		this.tiles [x, y].GetComponent<TileController>().initTileController(new Tile(x,y), type);
		
		if (isFirstP){
			position = new Vector3((-2.5f+x)*(tileScale), (-3.5f+y)*(tileScale), 0);
		}
		else{
			position = new Vector3((2.5f-x)*(tileScale), (3.5f-y)*(tileScale), 0);
		}
		
		Vector3 scale = new Vector3(0.25f*tileScale, 0.25f*tileScale, 0.25f*tileScale);
		this.tiles [x, y].GetComponent<TileController>().resize(position, scale);
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
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().setTile(new Tile(4-c.deckOrder, hauteur), tiles [4-c.deckOrder, hauteur].GetComponent<TileController>().getPosition());
			this.tiles [4-c.deckOrder, hauteur].GetComponent<TileController>().setCharacterID(index);
		}
		else{
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().setTile(new Tile(1+c.deckOrder, hauteur), tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().getPosition());
			this.tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().setCharacterID(index);

		}

		if (isFirstP==isFirstPlayer){
			this.tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().setDestination(5);
		}

		this.playingCards [index].GetComponentInChildren<PlayingCardController>().show();

		this.playingCards [index].GetComponentInChildren<PlayingCardController>().checkPassiveSkills(isFirstP==this.isFirstPlayer);
		GameCard gc = this.getCard(index);
		if(gc.isPiegeur() && this.isFirstPlayer){
			List<Tile> tiles2 = ((Piegeur)GameSkills.instance.getSkill(64)).getTiles(gc.getPassiveSkillLevel(), this.boardWidth, this.boardHeight, this.nbFreeRowsAtBeginning);
			for (int i = 0 ; i < tiles2.Count ; i++){
				GameController.instance.addPiegeurTrap(tiles2[i], 5+2*gc.getPassiveSkillLevel(), gc.isMine);
			}
			GameView.instance.displaySkillEffect(index, "Piégeur\npose 4 pièges!", 2);
			GameView.instance.addAnim(GameView.instance.getTile(index), 0);
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
			bool hasFoundMine = false;
			bool hasFoundHis = false;
			int level;
			int attackValue ;
			int pvValue ;
			for(int i = 0 ; i < this.nbCards ; i++){
				if(this.getCard(i).isMine){
					if(!hasFoundMine){
						if(this.getCard(i).isLeader()){
							level = this.getCard(i).getSkills()[0].Power;
							GameView.instance.getPlayingCardController(i).addDamagesModifyer(new Modifyer(Mathf.RoundToInt(this.getCard(i).GetTotalLife()/2f), -1, 23, base.name, 5+" dégats subis"), false);
							for(int j = 0 ; j < this.nbCards ; j++){
								if(this.getCard(j).isMine && i!=j){
									attackValue = level+2;
									pvValue = 2*level+5;
									this.getCard(j).attackModifyers.Add(new Modifyer(attackValue, -1, 76, "Leader", "+"+attackValue+"ATK. Permanent"));
									this.getCard(j).pvModifyers.Add(new Modifyer(pvValue, -1, 76, "Leader", "+"+pvValue+"PV. Permanent"));
									this.getPlayingCardController(j).show();
									this.getPlayingCardController(j).updateLife(0);
								}
							}

							GameView.instance.displaySkillEffect(i, "Leader\nrenforce les alliés", 1);	
							GameView.instance.addAnim(GameView.instance.getTile(i), 76);
							hasFoundMine = true;	
						}
					}	
				}
				else{
					if(!hasFoundHis){
						if(this.getCard(i).isLeader()){
							level = this.getCard(i).getSkills()[0].Power;
							GameView.instance.getPlayingCardController(i).addDamagesModifyer(new Modifyer(Mathf.RoundToInt(this.getCard(i).GetTotalLife()/2f), -1, 23, base.name, 5+" dégats subis"), false);
		
							for(int j = 0 ; j < this.nbCards ; j++){
								if(!this.getCard(j).isMine && i!=j){
									attackValue = level+2;
									pvValue = 2*level+5;
									this.getCard(j).attackModifyers.Add(new Modifyer(attackValue, -1, 76, "Leader", "+"+attackValue+"ATK. Permanent"));
									this.getCard(j).pvModifyers.Add(new Modifyer(pvValue, -1, 76, "Leader", "+"+pvValue+"PV. Permanent"));
									this.getPlayingCardController(j).show();
									this.getPlayingCardController(j).updateLife(0);
								}
							}
							hasFoundHis = true;
						}
					}
				}
			}
		}
		
		if (isFirstP == this.isFirstPlayer)
		{
			this.myDeck = deck;
			this.setInitialDestinations(this.isFirstPlayer);
			this.showStartButton();
		}
		else{

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

		if (nbPlayersReadyToFight == 2 ||ApplicationModel.player.ToLaunchGameTutorial)
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
			this.setNextPlayer();
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
		if (this.getPlayingCardController(characterID).getIsMine()){	
			this.getMyHoveredCardController().setNextDisplayedCharacter(characterID, this.getCard(characterID));
		}
		else{
			this.getHisHoveredCardController().setNextDisplayedCharacter(characterID, this.getCard(characterID));
		}

		if(this.currentPlayingCard!=-1){
			if(!this.getCard(characterID).isMine && this.getCard(this.currentPlayingCard).isMine){
				this.getMyHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
			}
			else if(this.getCard(characterID).isMine && !this.getCard(this.currentPlayingCard).isMine){
				this.getHisHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
			}
		}
		
		if(this.hasFightStarted && this.hoveringZone==-1){
			this.removeDestinations();
			if(!this.getCard(characterID).isSniperActive()){
				this.displayDestinations(characterID);
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
	}

	public void clickMobileCharacter(int characterID){
		this.clickedCharacterId=characterID;
		this.timeDragging=0f;
	}

	public void dropCharacter(int characterID, Tile t, bool isFirstP, bool toDisplayMove){
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
			this.getPlayingCardController(characterID).setTile(t, tiles [t.x, t.y].GetComponent<TileController>().getPosition());
			if(GameView.instance.hasFightStarted){
				if(!this.getPlayingCardController(characterID).getIsMine()){
					this.tiles[t.x, t.y].GetComponentInChildren<TileController>().setDestination(6);
				}
			}
			if(this.getPlayingCardController(characterID).getIsMine()){
				this.tiles[t.x, t.y].GetComponentInChildren<TileController>().setDestination(5);
			}
			this.getPlayingCardController(characterID).moveBackward();
		}
		this.tiles[origine.x, origine.y].GetComponentInChildren<TileController>().setCharacterID(-1);
		this.tiles[t.x, t.y].GetComponentInChildren<TileController>().setCharacterID(characterID);

		if(GameView.instance.hasFightStarted){
			if(!this.getCard(characterID).isGolem()){
				this.getCard(characterID).hasMoved=true;
			}
			else{
				GameView.instance.displaySkillEffect(characterID, "Golem\n-10PV", 0);
				GameView.instance.getPlayingCardController(characterID).addDamagesModifyer(new Modifyer(10,-1,1,"Attaque","10 dégats subis"), true);
				GameView.instance.addAnim(GameView.instance.getTile(characterID), 141);
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
	}

	public void dropCharacter(int characterID){
		this.draggingCard=-1;
		Tile t = this.getPlayingCardTile(characterID);
		this.getPlayingCardController(characterID).setTile(t, tiles [t.x, t.y].GetComponent<TileController>().getPosition());
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
		this.getPlayingCardController(characterID).setTile(t, tiles [t.x, t.y].GetComponent<TileController>().getPosition());
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
						GameController.instance.findNextPlayer ();
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
				this.removeDestinations();
				if(this.currentPlayingCard!=-1){
					if(!this.isDisplayedMyDestination && !this.getCard(this.currentPlayingCard).hasMoved){
						this.displayDestinations(this.currentPlayingCard);
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
	
	public int findNextAlivePlayer(int o, bool isM){
		int i = o ;
		if(i==3){
			i=0;
		}
		else{
			i++;
		}
		int card = -1;
		while(i != o && card==-1){ 
			if(!this.getCard(this.findCardWithDO(i,isM)).isDead && !this.deads.Contains(this.findCardWithDO(i,isM))){
				card = this.findCardWithDO(i,isM) ;
			}
			if(card==-1){
				if(i==3){
					i=0;
				}
				else{
					i++;
				}
			}
		}
		if(i==o){
			card = this.findCardWithDO(o, isM);
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

	public void setNextPlayer(){

		isFreezed = true ;
		this.hideButtons();
		this.hideSkillEffects();
		this.hoveringZone=-1 ;
		if(this.hasFightStarted){
			this.meteoritesCounter--;
			if(this.meteoritesCounter==0){
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
				if(ApplicationModel.player.ToLaunchGameTutorial && this.nbTurns==0){
					this.endTurnPopUp.GetComponent<EndTurnPopUpController>().display(this.nbTurns);
				}
				else{
					this.interlude.GetComponent<InterludeController>().set("Fin du tour - Météorites !", 3);
				}
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
			else{
				StartCoroutine(launchEndTurnEffects());
			}
		}
		else{
			StartCoroutine(launchEndTurnEffects());
		}
	}

	public IEnumerator launchEndTurnEffects(){
		if(this.hasFightStarted){
			bool isSuccess = false ;
			if(!GameView.instance.getCurrentCard().isDead){
				if(GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).isPoisoned()){
					int value = Mathf.Min(GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).getPoisonAmount(), GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).getLife());
					GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), "Poison\nPerd "+value+"PV", 0);
					GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer(value,-1,94,"Poison",value+" dégats subis"), false);
					GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 94);
					isSuccess = true ;
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
									GameView.instance.addAnim(GameView.instance.getTile(playerID), 75);
								}
								else{
									this.getPlayingCardController(playerID).addDamagesModifyer(new Modifyer(-1*soin, -1, 75, "Infirmier", "+"+(soin)+"PV"), false);
									GameView.instance.displaySkillEffect(playerID, "+"+soin+"PV", 2);	
									GameView.instance.addAnim(GameView.instance.getTile(playerID), 75);
								}
							}
						}
					}
					if(isSuccess){
						GameView.instance.displaySkillEffect(this.currentPlayingCard, "Infirmier", 1);	
						GameView.instance.addAnim(GameView.instance.getTile(this.currentPlayingCard), 75);
					}
				}
				else if(this.getCard(this.currentPlayingCard).isFrenetique()){
					int level = GameView.instance.getCurrentCard().Skills[0].Power+2;
					int target = GameView.instance.getCurrentPlayingCard();

					GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(10,-1,69,"Frenetique","10 dégats subis"), false);
					GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(level, -1, 18, "Frenetique", "+"+level+"ATK. Permanent"));
					GameView.instance.getPlayingCardController(target).updateAttack();
					GameView.instance.displaySkillEffect(target, "Frénétique\n+"+level+" ATK\n-10PV", 1);
					GameView.instance.addAnim(GameView.instance.getTile(target), 69);
					isSuccess = true ;
				}
				if(isSuccess){
					yield return new WaitForSeconds(2f);
				}
			}
		}
		if(!this.hasFightStarted){
			this.changePlayer();
			if (this.getCurrentCard().isMine){
				this.interlude.GetComponent<InterludeController>().set("A votre tour de jouer !", 1);
			}
			else{
				this.interlude.GetComponent<InterludeController>().set("Tour de l'adversaire !", 2);
				if(ApplicationModel.player.ToLaunchGameTutorial){
					if(!this.hasStep3){
						interlude.GetComponent<InterludeController>().pause();
						this.launchTutoStep(3);
						this.blockFury = false;
						this.hasStep3 = true;
					}
				}
			}
		}
		else{
			if (!this.getCurrentCard().isMine){
				this.interlude.GetComponent<InterludeController>().set("A votre tour de jouer !", 1);
			}
			else{
				this.interlude.GetComponent<InterludeController>().set("Tour de l'adversaire !", 2);
				if(ApplicationModel.player.ToLaunchGameTutorial){
					if(!this.hasStep3){
						interlude.GetComponent<InterludeController>().pause();
						this.launchTutoStep(3);
						this.blockFury = false;
						this.hasStep3 = true;
					}
				}
			}
		}

		this.getPlayingCardController(this.currentPlayingCard).checkModyfiers();
		yield break ; 
	}

	public void updateTimeline(){
		List<int> idCards = new List<int>();
		idCards.Add(this.lastPlayingCard);
		int i = 0 ; 
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
				idCards.Add(orderCards[i]);
				i++;
				j--;
			}
		}
		this.timeline.changeFaces(idCards);
		this.timeline.show(true);
	}

	public void changePlayer(){
		if(this.hasFightStarted){
			int tempInt=-1;
			this.orderCards.RemoveAt(0);
			if(this.getCurrentCard().isMutant()){
				this.getPlayingCardController(this.currentPlayingCard).nbTurns++;
				if(this.getPlayingCardController(this.currentPlayingCard).nbTurns==3){
					int level = this.getCurrentCard().getSkills()[0].Power;
					this.getCard(this.currentPlayingCard).emptyModifiers();
					this.getCard(this.currentPlayingCard).Attack = 15+level*5;
					this.getCard(this.currentPlayingCard).Life = 45+level*5;
					this.getCard(this.currentPlayingCard).getSkills()[0].Id = 144;
					this.getPlayingCardController(this.currentPlayingCard).show();
					GameView.instance.displaySkillEffect(this.currentPlayingCard, "Mutant\nse transforme!", 2);
					GameView.instance.addAnim(GameView.instance.getTile(this.currentPlayingCard), 0);
				}
			}
			this.lastPlayingCard = currentPlayingCard;
			if(this.getCard(this.orderCards[5]).isMine){
				tempInt = this.findNextAlivePlayer(this.getCard(this.orderCards[4]).deckOrder, false);
			}
			else{
				tempInt = this.findNextAlivePlayer(this.getCard(this.orderCards[4]).deckOrder, true);
			}

			this.orderCards.Add(tempInt);
			this.updateTimeline();
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
			}
			else{
				orderCards.Add(this.findCardWithDO(0, false));
				orderCards.Add(this.findCardWithDO(0, true));
				orderCards.Add(this.findCardWithDO(1, false));
				orderCards.Add(this.findCardWithDO(1, true));
				orderCards.Add(this.findCardWithDO(2, false));
				orderCards.Add(this.findCardWithDO(2, true));
				orderCards.Add(this.findCardWithDO(3, false));
			}
			this.updateTimeline();
		}
		int nextPlayingCard = orderCards[0];
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

		GameController.instance.findNextPlayer();
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
		GameController.instance.findNextPlayer();
	}
	
	public void emptyTile(int c){
		this.getTileController(this.getPlayingCardTile(c).x,this.getPlayingCardTile(c).y).setCharacterID(-1);
		this.getTileController(this.getPlayingCardTile(c).x,this.getPlayingCardTile(c).y).setDestination(-1);
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

		if(this.draggingSkillButton!=-1){
			Vector3 mousePos = Input.mousePosition;
			this.getSkillZoneController().getSkillButtonController(draggingSkillButton).setPosition2(Camera.main.ScreenToWorldPoint(mousePos));
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
		
		if(this.getMyHoveredCardController().getIsRunning()){
			this.getMyHoveredCardController().addTimeC(Time.deltaTime);
		}
		
		if (this.getHisHoveredCardController().getStatus()!=0){
			this.getHisHoveredCardController().addTime(Time.deltaTime);
		}
		
		if(this.getHisHoveredCardController().getIsRunning()){
			this.getHisHoveredCardController().addTimeC(Time.deltaTime);
		}

		if(this.numberDeckLoaded==2 || (ApplicationModel.player.ToLaunchGameTutorial && numberDeckLoaded==1)){
			for(int i = 0 ; i < this.nbCards ; i++){
				if(this.getPlayingCardController(i).isUpdatingLife){
					this.getPlayingCardController(i).addLifeTime(Time.deltaTime);
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

	public Sprite getIconSprite(int i){
		return this.iconSprites[i];
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
	
	public void hidePopUp(){
		this.popUp.transform.position = new Vector3(0, -10, 0);
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
		position.x = -0.5f*this.realwidth;
		tempGO.transform.position = position;
		tempGO.transform.FindChild("MyPlayerName").GetComponent<TextContainer>().width = realwidth/2f-1 ;

		tempGO = GameObject.Find("HisPlayerName");
		position = tempGO.transform.position ;
		position.x = 0.48f*this.realwidth;
		tempGO.transform.position = position;
		tempGO.GetComponent<TextContainer>().width = realwidth/2f-1 ;

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

		tempTransform = this.skillZone.transform;
		position = tempTransform.position ;
		position.x = Mathf.Max(-3,-0.5f*this.realwidth);
		this.stepButton = Mathf.Max(-3,-0.5f*this.realwidth);
		tempTransform.position = position;

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
	}
	
	public void hitExternalCollider(){
		GameView.instance.hoverTile();
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
						tempNeighbours = this.getDestinationImmediateNeighbours(baseTiles[k]);
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
						tempNeighbours = this.getDestinationHisImmediateNeighbours(baseTiles[k]);
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
	
	public void displayAdjacentOpponentsTargets(){
		List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardController(this.currentPlayingCard).getTile());
		this.targets = new List<Tile>();
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.getTileController(t.x, t.y).getCharacterID();
			if (playerID != -1)
			{
				if (this.getPlayingCardController(playerID).canBeTargeted() && !this.getCard(playerID).isMine){
					this.targets.Add(t);
					this.getTileController(t.x,t.y).displayTarget(true);
					this.getTileController(playerID).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(playerID));
				}
			}
		}
	}

	public void displayMyUnitTarget(){
		PlayingCardController pcc;
		Tile tile ;
		this.targets = new List<Tile>();
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (i == this.currentPlayingCard)
			{
				tile = this.getPlayingCardTile(i);
				this.targets.Add(tile);
				this.getTileController(tile.x,tile.y).displayTarget(true);
				this.getTileController(tile).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(i));
			}
		}
	}

	public void displayAdjacentUnitsTargets(){
		List<Tile> neighbourTiles = this.getCharacterImmediateNeighbours(this.getPlayingCardController(this.currentPlayingCard).getTile());
		this.targets = new List<Tile>();
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.getTileController(t.x, t.y).getCharacterID();
			if (playerID != -1)
			{
				if (this.getPlayingCardController(playerID).canBeTargeted()){
					this.targets.Add(t);
					this.getTileController(t.x,t.y).displayTarget(true);
					this.getTileController(playerID).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(playerID));
				}
			}
		}
	}
	
	public void displayAdjacentAllyTargets()
	{
		List<Tile> neighbourTiles = this.getAllyImmediateNeighbours(this.getPlayingCardController(this.currentPlayingCard).getTile());
		this.targets = new List<Tile>();
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.getTileController(t.x, t.y).getCharacterID();
			if (playerID != -1)
			{
				if (this.getPlayingCardController(playerID).canBeTargeted() && this.getCard(playerID).isMine){
					this.targets.Add(t);
					this.getTileController(t.x,t.y).displayTarget(true);
					this.getTileController(playerID).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(playerID));
				}
			}
		}
	}
	
	public void displayAllysButMeTargets()
	{
		PlayingCardController pcc;
		Tile tile ;
		this.targets = new List<Tile>();
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine && pcc.canBeTargeted() &&  i != this.currentPlayingCard)
			{
				tile = this.getPlayingCardTile(i);
				this.targets.Add(tile);
				this.getTileController(tile.x,tile.y).displayTarget(true);
				this.getTileController(tile).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(i));
			}
		}
	}

	public void displayWoundedAllysButMeTargets()
	{
		PlayingCardController pcc;
		Tile tile ;
		this.targets = new List<Tile>();
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine && pcc.canBeTargeted() &&  i != this.currentPlayingCard)
			{
				if (this.getCard(i).getLife() != this.getCard(i).GetTotalLife())
				{
					tile = this.getPlayingCardTile(i);
					this.targets.Add(tile);
					this.getTileController(tile.x,tile.y).displayTarget(true);
					this.getTileController(tile).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(i));
				}
			}
		}
	}
	
	public void displayAllButMeModifiersTargets()
	{
		PlayingCardController pcc;
		Tile tile ;
		this.targets = new List<Tile>();
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (pcc.canBeTargeted() &&  i != this.currentPlayingCard)
			{
				tile = this.getPlayingCardTile(i);
				this.targets.Add(tile);
				this.getTileController(tile.x,tile.y).displayTarget(true);
				this.getTileController(tile).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(i));
			}
		}	
	}
	
	public void displayOpponentsTargets()
	{
		PlayingCardController pcc;
		Tile tile ;
		this.targets = new List<Tile>();
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (!this.getCard(i).isMine && pcc.canBeTargeted())
			{
				tile = this.getPlayingCardTile(i);
				this.targets.Add(tile);
				this.getTileController(tile.x,tile.y).displayTarget(true);
				this.getTileController(tile).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(i));
			}
		}
	}

	public void displayAdjacentCristoidOpponents(){
		Tile tile ;
		this.targets = new List<Tile>();

		for(int i = 0 ; i < this.nbCards ; i++){
			if(this.getCard(i).CardType.Id==6 && this.getCard(i).isMine){
				List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardTile(i));
				int playerID;
				foreach (Tile t in neighbourTiles)
				{
					playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
					if (playerID != -1)
					{
						if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getCard(playerID).isMine)
						{
							tile = this.getPlayingCardTile(i);
							this.targets.Add(tile);
							this.getTileController(tile.x,tile.y).displayTarget(true);
							this.getTileController(tile).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(i));
						}
					}
				}
			}
		}
	}
	
	public void displayAdjacentTileTargets()
	{
		List<Tile> neighbourTiles = this.getFreeImmediateNeighbours(this.getPlayingCardTile(this.getCurrentPlayingCard()));
		this.targets = new List<Tile>();
				
		foreach (Tile t in neighbourTiles){
			this.targets.Add(t);
			this.getTileController(t.x,t.y).displayTarget(true);
			this.getTileController(t).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText());
		}
	}
	
	public void display1TileAwayOpponentsTargets()
	{
		int playerID;
		Tile tile = this.getPlayingCardTile(this.currentPlayingCard) ;
		this.targets = new List<Tile>();
		
		if(tile.x>1){
			playerID = this.tiles [tile.x-2, tile.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getCard(playerID).isMine){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						this.targets.Add(new Tile(tile.x-2, tile.y));
						this.getTileController(tile.x-2, tile.y).displayTarget(true);
						this.getTileController(playerID).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(playerID));
					}
				}
			}
		}
		if(tile.x<this.boardWidth-2){
			playerID = this.tiles [tile.x+2, tile.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getCard(playerID).isMine){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						this.targets.Add(new Tile(tile.x+2, tile.y));
						this.getTileController(tile.x+2, tile.y).displayTarget(true);
						this.getTileController(playerID).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(playerID));
					}
				}
			}
		}
		if(tile.y>1){
			playerID = this.tiles [tile.x, tile.y-2].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getCard(playerID).isMine){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						this.targets.Add(new Tile(tile.x, tile.y-2));
						this.getTileController(tile.x, tile.y-2).displayTarget(true);
						this.getTileController(playerID).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(playerID));
					}
				}
			}
		}
		if(tile.y<this.boardHeight-2){
			playerID = this.tiles [tile.x, tile.y+2].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getCard(playerID).isMine){
					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
					{
						this.targets.Add(new Tile(tile.x, tile.y+2));
						this.getTileController(tile.x, tile.y+2).displayTarget(true);
						this.getTileController(playerID).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText(playerID));
					}
				}
			}
		}
	}
	
	public string canLaunch1TileAwayOpponents()
	{
		string isLaunchable = "Aucun ennemi à portée de lance";
		int playerID;
		Tile tile = this.getPlayingCardTile(this.currentPlayingCard) ;
		this.targets = new List<Tile>();
		
		if(tile.x>1){
			playerID = this.tiles [tile.x-2, tile.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if(!this.getCard(playerID).isMine){
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
				if(!this.getCard(playerID).isMine){
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
				if(!this.getCard(playerID).isMine){
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
				if(!this.getCard(playerID).isMine){
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
	
	public string canLaunchAdjacentOpponents()
	{
		string isLaunchable = "Aucun ennemi à proximité";
		
		List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardTile(this.currentPlayingCard));
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getCard(playerID).isMine)
				{
					isLaunchable = "";
				}
			}
		}
		return isLaunchable;
	}

	public string canLaunchAdjacentCristoidOpponents()
	{
		string isLaunchable = "Aucun ennemi à proximité de cristoides alliés";
		
		for(int i = 0 ; i < this.nbCards ; i++){
			if(this.getCard(i).CardType.Id==6 && this.getCard(i).isMine){
				List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardTile(i));
				int playerID;
				foreach (Tile t in neighbourTiles)
				{
					playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
					if (playerID != -1)
					{
						if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getCard(playerID).isMine)
						{
							isLaunchable = "";
						}
					}
				}
			}
		}
		return isLaunchable;
	}

	public string canLaunchAdjacentUnits()
	{
		string isLaunchable = "Aucune unité à proximité";
		
		List<Tile> neighbourTiles = this.getCharacterImmediateNeighbours(this.getPlayingCardTile(this.currentPlayingCard));
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted())
				{
					isLaunchable = "";
				}
			}
		}
		return isLaunchable;
	}

	public string canLaunchAdjacentRock()
	{
		string isLaunchable = "Aucun cristal à proximité";
		
		List<Tile> neighbourTiles = this.getPlayingCardTile(this.currentPlayingCard).getImmediateNeighbourTiles();
		foreach (Tile t in neighbourTiles)
		{
			if (this.getTileController(t).isRock())
			{
				isLaunchable = "";
			}
		}
		return isLaunchable;
	}

	public void displayAdjacentRockTargets()
	{
		List<Tile> neighbourTiles = this.getPlayingCardTile(this.currentPlayingCard).getImmediateNeighbourTiles();
		this.targets = new List<Tile>();
				
		foreach (Tile t in neighbourTiles){
			if (this.getTileController(t).isRock())
			{
				this.targets.Add(t);
				this.getTileController(t.x,t.y).displayTarget(true);
				this.getTileController(t).setTargetText(GameSkills.instance.getSkill(this.runningSkill).name, GameSkills.instance.getCurrentGameSkill().getTargetText());
			}
		}
	}

	public string canLaunchMyUnit()
	{
		string isLaunchable = "";
		return isLaunchable;
	}
	
	public string canLaunchAdjacentTileTargets()
	{
		string isLaunchable = "Aucun terrain ne peut etre ciblé";
		
		List<Tile> neighbourTiles = this.getFreeImmediateNeighbours(this.getPlayingCardTile(this.currentPlayingCard));
		this.targets = new List<Tile>();
		
		if(neighbourTiles.Count>0){
			isLaunchable = "";
		}
		return isLaunchable;
	}
	
	public string canLaunchAdjacentAllys()
	{
		string isLaunchable = "Aucun allié à proximité";
		
		List<Tile> neighbourTiles = this.getAllyImmediateNeighbours(this.getPlayingCardTile(this.currentPlayingCard));
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
			if (playerID != -1)
			{
				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getCard(playerID).isMine)
				{
					isLaunchable = "";
				}
			}
		}
		return isLaunchable;
	}
	
	public string canLaunchOpponentsTargets()
	{
		string isLaunchable = "Aucun ennemi ne peut etre atteint";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (!this.getCard(i).isMine && pcc.canBeTargeted())
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
	
	public string canLaunchAllysButMeTargets()
	{
		string isLaunchable = "Aucun allié ne peut etre atteint";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine && pcc.canBeTargeted() && i != this.currentPlayingCard)
			{
				isLaunchable = "";
			}
		}
		return isLaunchable;
	}

	public string canLaunchWoundedAllysButMeTargets()
	{
		string isLaunchable = "Aucun allié n'est blessé";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.nbCards; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine && pcc.canBeTargeted() && i != this.currentPlayingCard)
			{
				if(this.getCard(i).getLife()!=this.getCard(i).GetTotalLife()){
					isLaunchable = "";
				}
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
					GameView.instance.addAnim(GameView.instance.getTile(j), 76);
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
					GameView.instance.addAnim(GameView.instance.getTile(j), 76);
				}
			}
		}
	}
	
	public void killHandle(int c, bool endTurn){
		if(this.areAllMyPlayersDead()){
			StartCoroutine(quitGame());
		}
		else{
			List<int> newOrderCards = new List<int>();
			List<int> idCards = new List<int>();
			
			int i = 0 ; 
			int jMine = 0 ;
			int jHis = 0 ;
			while (i<orderCards.Count && orderCards[i]!=c){
				newOrderCards.Add(orderCards[i]);
				i++;
			}
			if (i==0){
				
			}
			else{
				i--;
				jMine = i ;
				jHis = i;
				while(i<6){
					if(i==0){
						if(this.getCard(newOrderCards[0]).isMine){
							if(lastPlayingCard==-10){
								newOrderCards.Add(this.findNextAlivePlayer(3,false));
							}
							else{
								print(lastPlayingCard);
								newOrderCards.Add(this.findNextAlivePlayer(this.getCard(lastPlayingCard).deckOrder,false));
							}
						}
						else{
							if(lastPlayingCard==-10){
								newOrderCards.Add(this.findNextAlivePlayer(3,true));
							}
							else{
								newOrderCards.Add(this.findNextAlivePlayer(this.getCard(lastPlayingCard).deckOrder,true));
							}
						}
					}
					else{
						if(this.getCard(newOrderCards[i]).isMine){
							newOrderCards.Add(this.findNextAlivePlayer(this.getCard(newOrderCards[i-1]).deckOrder,false));
							print(i+" - MINE "+newOrderCards[newOrderCards.Count-1]);
						}
						else{
							newOrderCards.Add(this.findNextAlivePlayer(this.getCard(newOrderCards[i-1]).deckOrder,true));
							print(i+ " - HIS "+newOrderCards[newOrderCards.Count-1]);
						}
					}
					i++;
				}
				orderCards = new List<int>();
				for(int k = 0 ; k < newOrderCards.Count ; k++){
					orderCards.Add(newOrderCards[k]);
				}
				this.updateTimeline();
			}
		}

		if(this.getCard(this.currentPlayingCard).isSanguinaire()){
			GameCard currentCard = GameView.instance.getCurrentCard();
			int target = GameView.instance.getCurrentPlayingCard();
			int bonus = GameView.instance.getCurrentCard().Skills[0].Power*4;

			GameView.instance.getCard(target).magicalBonusModifyers.Add(new Modifyer(bonus, -1, 34, "Sanguinaire", "+"+bonus+"% dégats à distance"));
			GameView.instance.getPlayingCardController(target).updateAttack();
			GameView.instance.displaySkillEffect(target, "Dégats à distance +"+bonus+"%", 2);
			GameView.instance.addAnim(GameView.instance.getTile(target), 34);
		}

		this.getPlayingCardController(c).displayDead(true);
		this.deads.Add(c);
	}
	
	IEnumerator sendStat(string user1, string user2, bool isTutorialGame, int percentageTotalDamages)
	{
		int isTutorialGameInt = 0;
		if(isTutorialGame)
		{
			isTutorialGameInt=1;
		}
		
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick1", user1); 	                    // Pseudo de l'utilisateur victorieux
		form.AddField("myform_nick2", user2); 	                    // Pseudo de l'autre utilisateur
		form.AddField("myform_gametype", ApplicationModel.player.ChosenGameType);
		form.AddField ("myform_istutorialgame", isTutorialGameInt);
		form.AddField("myform_percentagelooser",percentageTotalDamages);
		
		WWW w = new WWW(URLStat, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null)
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else if(w.text.Contains("#ERROR#"))
		{
			string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
			Debug.Log (errors[1]);
		}
		yield break;
	}
	public IEnumerator quitGame()
	{
		bool hasFirstPlayerWon=false;
		
		if(ApplicationModel.player.ToLaunchGameTutorial)
		{
			if(this.areAllMyPlayersDead())
			{
				hasFirstPlayerWon=true;
				yield return (StartCoroutine(ApplicationModel.player.setTutorialStep(2)));
			}
			else
			{
				yield return (StartCoroutine(ApplicationModel.player.setTutorialStep(3)));
			}
		}
		else
		{
			if(!isFirstPlayer)
			{
				hasFirstPlayerWon=true;
			}
			yield return (StartCoroutine(this.sendStat(ApplicationModel.hisPlayerName, ApplicationModel.myPlayerName, false,getPercentageTotalDamages(false))));
		}
		
		GameController.instance.quitGame(hasFirstPlayerWon);
		
		yield break;
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
	
	public List<Tile> getDestinationImmediateNeighbours(Tile t){
		bool b ; 
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			b = false ;
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(this.getCard(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()).isMine){
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
	
	public List<Tile> getDestinationHisImmediateNeighbours(Tile t){
		bool b ; 
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			b = false ;
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(!this.getCard(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()).isMine){
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
	
	public List<Tile> getOpponentImmediateNeighbours(Tile t){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(!this.getCard(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()).isMine){
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
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1 && this.getCard(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()).isMine){
				freeNeighbours.Add(neighbours[i]);
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getAllyImmediateNeighbours(Tile t){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(this.getCard(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()).isMine){
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
	
	public List<int> getAllys(){
		List<int> allys = new List<int>();
		int CPC = GameView.instance.getCurrentPlayingCard();
		for(int i = 0 ; i < this.nbCards; i++){
			if(i!=CPC && !GameView.instance.getCard(i).isDead && GameView.instance.getCard(i).isMine){
				allys.Add(i);
			}
		}
		return allys;
	}
	
	public List<int> getOpponents(){
		List<int> opponents = new List<int>();
		for(int i = 0 ; i < this.nbCards;i++){
			if(!this.getCard(i).isDead && !this.getCard(i).isMine){
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
			List<int> opponents = GameView.instance.getOpponents();
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
			List<int> opponents = GameView.instance.getAllys();
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

	public Sprite getCardTypeSprite(int i){
		return this.cardTypeSprites[i];
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
		this.getSkillZoneController().showCancelButton(false);
		this.getSkillZoneController().showSkillButtons(false);
	}
	
	public IEnumerator endPlay()
	{
		this.getCard(this.currentPlayingCard).hasPlayed = true ;
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
					GameController.instance.findNextPlayer ();
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
	
	public void initPCCTargetHandler(int numberOfExpectedTargets)
	{
		this.targetPCCHandler = new TargetPCCHandler(numberOfExpectedTargets);
		this.hideSkillEffects();
	}
	
	public void initTileTargetHandler(int numberOfExpectedTargets)
	{
		this.targetTileHandler = new TargetTileHandler(numberOfExpectedTargets);
		this.hideSkillEffects();
	}
	
	public void hitTarget(int c){
		this.targetPCCHandler.addTargetPCC(c);
	}
	
	public void hitTarget(Tile t){
		this.targetTileHandler.addTargetTile(t);
	}
	
	public Skill getCurrentSkill(){
		return this.getCurrentCard().findSkill(this.runningSkill);
	}
	
	public void launchTutoStep(int i){
		if (i==1){
			this.popUp.GetComponent<PopUpGameController>().setTexts("Etape 1 : L'arène", "Bienvenue dans l'arène de combat de Cristalia!\n\nL'arène est constitué de cases sur lesquels vos personnages peuvent se déplacer (sauf les cases cristal). Attention, certaines cases peuvent être piégées!\n\n Vous pouvez positionner vos unités avant le combat (le premier à terminer son positionnement démarre le combat), et elles pourront se déplacer 1 fois par tour de jeu\n\nLe positionnement des unités est important, les attaques en diagonale étant interdites (Code de guerre de Cristalia, article 1)");
			this.popUp.GetComponent<PopUpGameController>().changePosition(new Vector3(-0.05f, 0f, 0f));
			this.popUp.GetComponent<PopUpGameController>().show(true);
		}
		else if (i==2){
			this.popUp.GetComponent<PopUpGameController>().setTexts("Etape 2 : Chacun son tour", "Les colons jouent chacun leur tour, selon l'ordre des unités dans leurs équipes.\n\nUne unité peut SE DEPLACER et UTILISER UNE COMPETENCE par tour, dans n'importe quel ordre (Code de Guerre cristalien, article 2)\n\nLorsque que vous utilisez une compétence ciblant une unité, pensez à vérifier les effets de la compétence en survolant votre cible!");
			this.popUp.GetComponent<PopUpGameController>().changePosition(new Vector3(-0.05f, 0f, 0f));
			this.popUp.GetComponent<PopUpGameController>().show(true);
		}
		else if (i==3){
			this.popUp.GetComponent<PopUpGameController>().setTexts("Etape 3 : Règles du combat", "Félicitations, vous avez donné vos premiers ordres, chef de guerre. Mais la bataille n'est pas terminée !\n\nLe combat se termine quand un colon ne dispose plus d'unités pour se battre.\n\nJe vous laisse désormais seul pour terminer le combat, bon courage !");
			this.popUp.GetComponent<PopUpGameController>().changePosition(new Vector3(-0.05f, 0f, 0f));
			this.popUp.GetComponent<PopUpGameController>().show(true);
		}
	}
	
	public void hideTuto(){
		this.popUp.GetComponent<PopUpGameController>().show(false);
		this.interlude.GetComponent<InterludeController>().unPause();
	}
	
	public void addAnim(Tile t, int i){
		this.anims.Add(t);
		this.getTileController(t).setAnimIndex(i);
		this.getTileController(t).displayAnim(true);
	}
	
	public void removeAnim(Tile t){
		for(int i = anims.Count-1 ; i >= 0 ; i--){
			if(anims[i].x==t.x && anims[i].y==t.y){
				anims.RemoveAt(i);
			}
		}
		this.getTileController(t).displayAnim(false);
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
			this.setNextPlayer();
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

	public void addCharacter(int id, int atk, int pv, int x, int y, bool isFirstP){
		this.playingCards[nbCards]=(GameObject)Instantiate(this.playingCardModel);
		this.nbCards++;
		int index = this.nbCards-1;
		GameCard c = null ;
		if(id==6){
			c = new GameCard(atk, pv, "Bouclier", 0, 11, 0);
			c.Skills = new List<Skill>();
			c.Skills.Add(new Skill("Protection",6));
		}
		this.playingCards [index].GetComponentInChildren<PlayingCardController>().setCard(c, true, index);
		if ((isFirstP==isFirstPlayer)){
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().setTile(new Tile(x,y), tiles [x,y].GetComponent<TileController>().getPosition());
			this.tiles [x,y].GetComponent<TileController>().setCharacterID(index);
			this.tiles [x,y].GetComponent<TileController>().setDestination(5);
		}
		else{
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().setTile(new Tile(x,y), tiles [x,y].GetComponent<TileController>().getPosition());
			this.tiles [x,y].GetComponent<TileController>().setCharacterID(index);
			this.tiles [x,y].GetComponent<TileController>().setDestination(6);
		}

		this.playingCards [index].GetComponentInChildren<PlayingCardController>().show();
		if(id==6){
			GameView.instance.displaySkillEffect(index, "Protection", 2);
			GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 29);
		}
	}

	public void stopCountingTime(){
		this.toCountTime = false ;
	}

	public void hideEndTurnPopUp(){
		this.isFreezed = false ;
		Tile t ;
		int amount ;

		for(int i = 0 ; i < boardWidth ; i++){
			t = new Tile(i,0);
			if(this.getTileController(t).getCharacterID()!=-1){
				amount = 5*(this.nbTurns);
				if(this.getCard(this.getTileController(t).getCharacterID()).isSniperActive()){
					amount = Mathf.RoundToInt((0.5f-0.05f*this.getCard(this.getTileController(t).getCharacterID()).Skills[0].Power)*amount);
				}
				GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount+"PV", 0);
				GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount,-1,1,"Attaque",amount+" dégats subis"), false);
				GameView.instance.addAnim(GameView.instance.getTile(this.getTileController(t).getCharacterID()), 0);
			}
			else{
				GameView.instance.displaySkillEffect(t, "", 0);
				GameView.instance.addAnim(t, 0);
			}
			t = new Tile(i,boardHeight-1);
			if(this.getTileController(t).getCharacterID()!=-1){
				amount = 5*(this.nbTurns);
				if(this.getCard(this.getTileController(t).getCharacterID()).isSniperActive()){
					amount = Mathf.RoundToInt((0.5f-0.05f*this.getCard(this.getTileController(t).getCharacterID()).Skills[0].Power)*amount);
				}
				GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount+"PV", 0);
				GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount,-1,1,"Attaque",amount+" dégats subis"), false);
				GameView.instance.addAnim(GameView.instance.getTile(this.getTileController(t).getCharacterID()), 0);
			}
			else{
				GameView.instance.displaySkillEffect(t, "", 0);
				GameView.instance.addAnim(t, 0);
			}
		}

		if(nbTurns>=2){
			for(int i = 0 ; i < boardWidth ; i++){
				t = new Tile(i,1);
				if(this.getTileController(t).getCharacterID()!=-1){
					amount = 5*(this.nbTurns-1);
					GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount+"PV", 0);
					GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount,-1,1,"Attaque",amount+" dégats subis"), false);
					GameView.instance.addAnim(GameView.instance.getTile(this.getTileController(t).getCharacterID()), 0);
				}
				else{
					GameView.instance.displaySkillEffect(t, "", 0);
					GameView.instance.addAnim(t, 0);
				}
				t = new Tile(i,boardHeight-2);
				if(this.getTileController(t).getCharacterID()!=-1){
					amount = 5*(this.nbTurns-1);
					GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount+"PV", 0);
					GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount,-1,1,"Attaque",amount+" dégats subis"), false);
					GameView.instance.addAnim(GameView.instance.getTile(this.getTileController(t).getCharacterID()), 0);
				}
				else{
					GameView.instance.displaySkillEffect(t, "", 0);
					GameView.instance.addAnim(t, 0);
				}
			}
		}

		if(nbTurns>=3){
			for(int i = 0 ; i < boardWidth ; i++){
				t = new Tile(i,2);
				if(this.getTileController(t).getCharacterID()!=-1){
					amount = 5*(this.nbTurns-2);
					GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount+"PV", 0);
					GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount,-1,1,"Attaque",amount+" dégats subis"), false);
					GameView.instance.addAnim(GameView.instance.getTile(this.getTileController(t).getCharacterID()), 0);
				}
				else{
					GameView.instance.displaySkillEffect(t, "", 0);
					GameView.instance.addAnim(t, 0);
				}
				t = new Tile(i,boardHeight-3);
				if(this.getTileController(t).getCharacterID()!=-1){
					amount = 5*(this.nbTurns-2);
					GameView.instance.displaySkillEffect(this.getTileController(t).getCharacterID(), "Météorite\n-"+amount+"PV", 0);
					GameView.instance.getPlayingCardController(this.getTileController(t).getCharacterID()).addDamagesModifyer(new Modifyer(amount,-1,1,"Attaque",amount+" dégats subis"), false);
					GameView.instance.addAnim(GameView.instance.getTile(this.getTileController(t).getCharacterID()), 0);
				}
				else{
					GameView.instance.displaySkillEffect(t, "", 0);
					GameView.instance.addAnim(t, 0);
				}
			}
		}

		StartCoroutine(this.waitThenLaunchEDE());
	}

	public IEnumerator waitThenLaunchEDE(){
		yield return new WaitForSeconds(2f);
		StartCoroutine(this.launchEndTurnEffects());
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

		this.getSkillZoneController().getSkillButtonController(draggingSkillButton).setPosition2(mousePos);
		this.getSkillZoneController().getSkillButtonController(draggingSkillButton).showDescription(false);
		this.hideTargets();
		this.cancelSkill();
		this.draggingSkillButton=-1;
	}

	public void cancelSkill(){
		this.getSkillZoneController().updateButtonStatus(this.getCurrentCard());
		this.getSkillZoneController().isRunningSkill = false ;
		this.runningSkill = -1;
		this.hoveringZone = -1;
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
			if(this.getCard(i).isCristoMaster()){
				amount = Mathf.Max(1,Mathf.RoundToInt(nbCristals*this.getCard(i).Skills[0].Power*this.getCard(i).Attack/100f));
				this.getCard(i).replaceCristoMasterModifyer(new Modifyer(amount,-1,139,"Cristomaster",amount+" ATK. Permanent"));
				this.getPlayingCardController(i).updateAttack();
				GameView.instance.displaySkillEffect(i, "Cristomaitre\n+"+amount+" ATK", 2);
				GameView.instance.addAnim(GameView.instance.getTile(i), 0);
			}
		}
	}
}



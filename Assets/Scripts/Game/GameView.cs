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
	public Sprite[] skillSprites;
	public Sprite[] skillTypeSprites;
	public Sprite[] cardTypeSprites;

	public int boardWidth ;
	public int boardHeight ;
	public int nbCardsPerPlayer ;
	public int nbFreeRowsAtBeginning ;
	public int turnTime ;
	
	bool isLoadingScreenDisplayed = false ;
	
	GameObject loadingScreen;
	GameObject[,] tiles ;
	GameObject attackButton ;
	GameObject passButton ;
	GameObject myHoveredRPC ;
	GameObject hisHoveredRPC ;
	GameObject[] verticalBorders ;
	GameObject[] horizontalBorders ;
	List<GameObject> playingCards ;
	GameObject popUp;
	GameObject validationSkill;

	GameObject tutorial;
	public GameObject myTimerGO;
	public GameObject hisTimerGO;
	
	public GameObject SB;
	GameObject interlude;
	public GameObject passZone;
	public GameObject skillZone;
	
	int heightScreen = -1;
	int widthScreen = -1;
	
	AudioSource audioEndTurn;
	
	int currentPlayingCard = -1;
	bool isFirstPlayer = false;
	
	public Deck myDeck ;
	int nbPlayersReadyToFight = 0 ;
	
	public bool hasFightStarted = false ;
	bool isBackgroundLoaded ;
	
	float realwidth ;
	
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
	
	int lastMyPlayingCardDeckOrder = 3 ; 
	int lastHisPlayingCardDeckOrder = 3 ; 
	
	public bool hasStep3 ;
	public bool hasStep2 ;
	public bool blockFury ;
	bool toPassDead = false ;

	public bool isFreezed = false ;
	public bool isDisplayedPopUp = false ;
	public int hoveringZone = -1 ;
	public bool toCountTime = true ;
	
	void Awake()
	{
		instance = this;
		this.displayLoadingScreen ();
		this.tiles = new GameObject[this.boardWidth, this.boardHeight];
		this.playingCards = new List<GameObject>();
		this.verticalBorders = new GameObject[this.boardWidth+1];
		this.horizontalBorders = new GameObject[this.boardHeight+1];
		this.attackButton = GameObject.Find("AttackButton");
		this.passButton = GameObject.Find("PassButton");
		this.myHoveredRPC = GameObject.Find("MyHoveredPlayingCard");
		this.hisHoveredRPC = GameObject.Find("HisHoveredPlayingCard");
		this.myTimerGO = GameObject.Find("MyTimer");
		this.myTimerGO.GetComponent<TimerController>().setIsMine(true);
		this.hisTimerGO = GameObject.Find("HisTimer");
		this.hisTimerGO.GetComponent<TimerController>().setIsMine(false);
		
		this.SB = GameObject.Find("SB");
		this.interlude = GameObject.Find("Interlude");
		this.passZone = GameObject.Find("PassZone");
		this.skillZone = GameObject.Find("SkillZone");
		this.popUp = GameObject.Find("PopUp");
		this.validationSkill = GameObject.Find("ValidationAutoSkill");
		this.popUp.GetComponent<PopUpGameController>().show (false);
		this.validationSkill.GetComponent<SkillValidationController>().show(false);

		this.SB.GetComponent<StartButtonController>().show(false);
		this.audioEndTurn = GetComponent<AudioSource>();
		this.setMyPlayerName(ApplicationModel.myPlayerName);
		this.setHisPlayerName(ApplicationModel.hisPlayerName);
		this.isFirstPlayer = ApplicationModel.player.IsFirstPlayer;
		this.runningSkill=-1;
		this.createBackground();
		this.targets = new List<Tile>();
		this.skillEffects = new List<Tile>();
		this.anims = new List<Tile>();
		this.deads = new List<int>();
		//this.destinations

		this.lastMyPlayingCardDeckOrder = 3 ; 
		this.lastHisPlayingCardDeckOrder = 3 ; 
		
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
		GameView.instance.hideLoadingScreen ();
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
			position = new Vector3(-2.5f+x, -3.5f+y, 0);
		}
		else{
			position = new Vector3(2.5f-x, 3.5f-y, 0);
		}
		
		Vector3 scale = new Vector3(0.25f, 0.25f, 0.25f);
		this.tiles [x, y].GetComponent<TileController>().resize(position, scale);
	}
	
	public void createPlayingCard(GameCard c, bool isFirstP)
	{
		int hauteur = 0 ;
		
		if (!isFirstP){
			hauteur = this.boardHeight-1 ;
		}
		this.playingCards.Add((GameObject)Instantiate(this.playingCardModel));
		int index = this.playingCards.Count-1;
		
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
				GameController.instance.addPiegeurTrap(tiles2[i], 2*gc.getPassiveSkillLevel(), gc.isMine);
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
		
		if(this.playingCards.Count==8 || ApplicationModel.player.ToLaunchGameTutorial){
			bool hasFoundMine = false;
			bool hasFoundHis = false;
			int level;
			int attackValue ;
			int pvValue ;
			for(int i = 0 ; i < playingCards.Count ; i++){
				if(this.getCard(i).isMine){
					if(!hasFoundMine){
						if(this.getCard(i).isLeader()){
							level = this.getCard(i).getSkills()[0].Power;
							for(int j = 0 ; j < playingCards.Count ; j++){
								if(this.getCard(j).isMine && i!=j){
									attackValue = Mathf.RoundToInt(level*3f*this.getCard(j).getAttack()/100f);
									pvValue = Mathf.RoundToInt(level*3f*this.getCard(j).GetTotalLife()/100f);
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
							for(int j = 0 ; j < playingCards.Count ; j++){
								if(!this.getCard(j).isMine && i!=j){
									attackValue = Mathf.RoundToInt(level*3f*this.getCard(j).getAttack()/100f);
									pvValue = Mathf.RoundToInt(level*3f*this.getCard(j).GetTotalLife()/100f);
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
			this.myTimerGO.GetComponent<TimerController>().setIsMyTurn(true);
		}
		else{
			this.hisTimerGO.GetComponent<TimerController>().setIsMyTurn(true);
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
			this.myTimerGO.GetComponent<TimerController>().setIsMyTurn(false);
			this.amIReadyToFight = true;
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
				
				this.SB.GetComponent<StartButtonController>().show(false);
				this.removeDestinations();
				this.displayOpponentCards();
			}
		}
		else{
			this.hisTimerGO.GetComponent<TimerController>().setIsMyTurn(false);
			this.isHeReadyToFight = true;
		}
		nbPlayersReadyToFight++;
		if (nbPlayersReadyToFight == 2)
		{
			this.SB.GetComponent<StartButtonController>().show(false);
			this.removeDestinations();
			this.displayOpponentCards();
			this.setNextPlayer();
		}
	}
	
	public void displayOpponentCards(){
		for (int i = 0; i < this.playingCards.Count; i++)
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
		this.getPlayingCardController(characterID).showHover(true);
		
		if(this.currentPlayingCard!=-1){
			if(!this.getCard(characterID).isMine && this.getCard(this.currentPlayingCard).isMine){
				this.getMyHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
			}
			else if(this.getCard(characterID).isMine && !this.getCard(this.currentPlayingCard).isMine){
				this.getHisHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
			}
		}
		
		if(this.hasFightStarted){
			this.removeDestinations();
			this.displayDestinations(characterID);
		}
	}
	
	public void clickCharacter(int characterID){
		if(!this.hasFightStarted){
			if(characterID==this.currentPlayingCard){
				this.getPlayingCardController(this.currentPlayingCard).stopAnim();
				if(this.getCard(this.currentPlayingCard).isMine){
					this.getMyHoveredCardController().stopAnim();
				}
				else{
					this.getHisHoveredCardController().stopAnim();
				}
				this.currentPlayingCard = -1 ;
			}
			else{
				this.changeCurrentClickedCard(characterID);
				if(this.getCard(this.currentPlayingCard).isMine){
					this.getMyHoveredCardController().toRun();
				}
				else{
					this.getHisHoveredCardController().toRun();
				}
			}
		}
		else{
			if(this.currentPlayingCard==characterID && ApplicationModel.player.ToLaunchGameTutorial){
				this.hideTuto();
			}
		}
	}
	
	public void changeCurrentClickedCard(int characterID){
		if(this.currentPlayingCard!=-1){
			this.getPlayingCardController(this.currentPlayingCard).stopAnim ();
			this.getPlayingCardController(this.currentPlayingCard).moveBackward();
			if(this.hasFightStarted){
				if(this.getCard(this.currentPlayingCard).isMine){
					this.getMyHoveredCardController().setNextDisplayedCharacter(-1, new GameCard());
				}
				else{
					this.getHisHoveredCardController().setNextDisplayedCharacter(-1, new GameCard());
				}
			}
		}
		this.currentPlayingCard = characterID ;
		this.getPlayingCardController(this.currentPlayingCard).moveForward();
		this.getPlayingCardController(this.currentPlayingCard).run ();
		if(this.hasFightStarted){
			if(this.getCard(this.currentPlayingCard).isMine){
				this.getMyHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
			}
			else{
				this.getHisHoveredCardController().setNextDisplayedCharacter(this.currentPlayingCard, this.getCard(this.currentPlayingCard));
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

	public void clickDestination(Tile destination, int c){
		if(c!=-1 && !this.isFreezed){
			this.setLaunchability("Déplacement en cours !");
			Tile origine = this.getPlayingCardController(c).getTile();
			this.tiles[origine.x, origine.y].GetComponentInChildren<TileController>().setDestination(1);
			if(this.hasFightStarted){
				this.removeDestinations();
			}
			this.tiles[origine.x, origine.y].GetComponentInChildren<TileController>().setCharacterID(-1);

			this.getPlayingCardController(c).changeTile(new Tile(destination.x,destination.y), this.tiles[destination.x,destination.y].GetComponentInChildren<TileController>().getPosition());
			this.tiles[destination.x, destination.y].GetComponentInChildren<TileController>().setCharacterID(c);

			if(GameView.instance.hasFightStarted){
				this.getCard(GameView.instance.getCurrentPlayingCard()).setHasMoved(true);
				this.getCard(c).hasMoved = true ;

				if(this.getPlayingCardController(c).getIsMine()){
					this.tiles[destination.x, destination.y].GetComponentInChildren<TileController>().setDestination(5);
				}
				else{
					this.tiles[destination.x, destination.y].GetComponentInChildren<TileController>().setDestination(6);
				}
			}
			else{
				if(this.getPlayingCardController(c).getIsMine()){
					this.tiles[destination.x, destination.y].GetComponentInChildren<TileController>().setDestination(5);
					this.tiles[origine.x, origine.y].GetComponentInChildren<TileController>().setDestination(1);
				}
			}
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
			
			yield return new WaitForSeconds(1f);
			
			if(this.getCard(c).isMine){
				if(this.getCard(this.currentPlayingCard).hasPlayed && this.getCard(this.currentPlayingCard).hasMoved){
					if(isSuccess){
						yield return new WaitForSeconds(1f);
					}
					if(!this.deads.Contains(this.currentPlayingCard) && !this.getCurrentCard().isFurious()){
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
			if(this.hoveringZone!=-1){
				if(this.hoveringZone==1){
					
				}
			}
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
						print("EMPTY");
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
		while(i < this.playingCards.Count ){ 
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
			if(!this.getCard(this.findCardWithDO(i,isM)).isDead){
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
	
	public void setNextPlayer(){
		isFreezed = true ;
		int length = this.playingCards.Count;
		this.hideButtons();

		StartCoroutine(launchEndTurnEffects());
	}

	public IEnumerator launchEndTurnEffects(){
		int nextPlayingCard = -1;
		if(this.hasFightStarted){
			bool isSuccess = false ;
				
			if(!GameView.instance.getCurrentCard().isDead){
				if(GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).isPoisoned()){
					int value = Mathf.Min(GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).state.amount, GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).getLife());
					GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), "Poison\nPerd "+value+"PV", 0);
					GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer(value,-1,94,"Poison",value+" dégats subis"));
					GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 94);
					isSuccess = true ;
				}
				if(this.getCard(this.currentPlayingCard).isNurse()){
					int power = this.getCurrentCard().Skills[0].Power;
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
									GameView.instance.displaySkillEffect(playerID, "Infirmier\nSans effet", 0);	
									GameView.instance.addAnim(GameView.instance.getTile(playerID), 75);
								}
								else{
									this.getPlayingCardController(playerID).addDamagesModifyer(new Modifyer(-1*soin, -1, 75, "Infirmier", "+"+(soin)+"PV"));
									GameView.instance.displaySkillEffect(playerID, "Infirmier\n+"+soin+"PV", 1);	
									GameView.instance.addAnim(GameView.instance.getTile(playerID), 75);
								}
							}
						}
					}

				}
				else if(this.getCard(this.currentPlayingCard).isFrenetique()){
					int level = GameView.instance.getCurrentCard().Skills[0].Power;
					int target = GameView.instance.getCurrentPlayingCard();

					GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(5,-1,69,"Frenetique","5 dégats subis"));
					GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(level, -1, 18, "Frenetique", "+"+level+"ATK. Permanent"));
					GameView.instance.getPlayingCardController(target).updateAttack();
					GameView.instance.displaySkillEffect(target, "+"+level+" ATK\n-5PV", 1);
					GameView.instance.addAnim(GameView.instance.getTile(target), 69);
					isSuccess = true ;
				}
				if(isSuccess){
					yield return new WaitForSeconds(2f);
				}
			}
		}

		if(this.hasFightStarted){
			if(this.getCurrentCard().isMine){
				nextPlayingCard = this.findNextAlivePlayer(this.lastHisPlayingCardDeckOrder, false);
				this.lastHisPlayingCardDeckOrder = this.getCard(nextPlayingCard).deckOrder;
			}
			else{
				nextPlayingCard = this.findNextAlivePlayer(this.lastMyPlayingCardDeckOrder, true);
				this.lastMyPlayingCardDeckOrder = this.getCard(nextPlayingCard).deckOrder;
			}
			this.checkField();
			print("NextTurn "+nextPlayingCard);
		}
		else{
			nextPlayingCard = this.findCardWithDO(0, this.isFirstPlayer);
			if(this.isFirstPlayer){
				this.lastMyPlayingCardDeckOrder = 0;
			}
			else{
				this.lastHisPlayingCardDeckOrder = 0;
			}
		}
		bool hasMoved = false ;
		bool hasPlayed = false ;
		
		if (this.getCard(nextPlayingCard).isMine){
			this.interlude.GetComponent<InterludeController>().set("A votre tour de jouer !", true);
		}
		else{
			this.interlude.GetComponent<InterludeController>().set("Tour de l'adversaire !", false);
			if(ApplicationModel.player.ToLaunchGameTutorial){
				if(!this.hasStep3){
					interlude.GetComponent<InterludeController>().pause();
					this.launchTutoStep(3);
					this.blockFury = false;
					this.hasStep3 = true;
				}
			}
		}
		
		if(this.hasFightStarted){
			this.hideTargets();
			hasMoved = false ;
			hasPlayed = false ;
			
			if (this.getCard(nextPlayingCard).isParalyzed()){
				hasPlayed = true ;
			}
			else if (this.getCard(nextPlayingCard).isFurious()){
				hasPlayed = true ;
				hasMoved = true ;
			}
		
			this.getPlayingCardController(this.currentPlayingCard).checkModyfiers();
			this.getCard(nextPlayingCard).setHasMoved(hasMoved);
			this.getCard(nextPlayingCard).setHasPlayed(hasPlayed);
			
			if(this.getCurrentCard().isMine){
				this.myTimerGO.GetComponent<TimerController>().setIsMyTurn(false);
			}
			else{
				this.hisTimerGO.GetComponent<TimerController>().setIsMyTurn(false);
			}
		}
		else{
			this.hasFightStarted = true ;
		}
		this.changeCurrentClickedCard(nextPlayingCard) ;
		yield break ; 
	}
	
	public void checkField(){
		bool toDestroy = false ;
		bool isDestroyed = true ;
		int i = 0 ;
		while(isDestroyed && !toDestroy){
			isDestroyed = true ;
			toDestroy = true ;
			for(int j = 0 ; j < 6 ; j++){
				if(this.getTileController(j,i).getTileType()!=1){
					isDestroyed = false ;
				}
				if(this.getTileController(j,i).getCharacterID()!=-1){
					toDestroy = false ;
				}
			}
			if(toDestroy){
				this.getTileController(0,i).changeType(2);
				this.getTileController(1,i).changeType(2);
				this.getTileController(2,i).changeType(2);
				this.getTileController(3,i).changeType(2);
				this.getTileController(4,i).changeType(2);
				this.getTileController(5,i).changeType(2);
				isDestroyed = true;
				toDestroy = false ;
			}
			i++;
		}
		i--;
		
		
		toDestroy = false ;
		isDestroyed = true ;
		i = 7 ;
		while(isDestroyed && !toDestroy){
			isDestroyed = true ;
			toDestroy = true ;
			for(int j = 0 ; j < 6 ; j++){
				if(this.getTileController(j,i).getTileType()!=1){
					isDestroyed = false ;
				}
				if(this.getTileController(j,i).getCharacterID()!=-1){
					toDestroy = false ;
				}
			}
			if(toDestroy){
				this.getTileController(0,i).changeType(1);
				this.getTileController(1,i).changeType(1);
				this.getTileController(2,i).changeType(1);
				this.getTileController(3,i).changeType(1);
				this.getTileController(4,i).changeType(1);
				this.getTileController(5,i).changeType(1);
				isDestroyed = true;
				toDestroy = false ;
			}
			i--;
		}
		i++;	
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
	
	public void setTurn(int id, int rank){
		this.getCard(id).setNbTurnsToWait(rank);
		if(id == this.playingCards.Count-1){
			this.setNextPlayer();
		}
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
		
		for(int i = 0 ; i < this.playingCards.Count ; i++){
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
		
		if(toCountTime){
			if(this.hasFightStarted){
				if(this.getCard(currentPlayingCard).isMine){
					this.myTimerGO.GetComponent<TimerController>().addTime(Time.deltaTime);
				}
				else{
					this.hisTimerGO.GetComponent<TimerController>().addTime(Time.deltaTime);
				}
			}
			else{
				if(!amIReadyToFight){
					this.myTimerGO.GetComponent<TimerController>().addTime(Time.deltaTime);
				}
				if(!isHeReadyToFight){
					this.hisTimerGO.GetComponent<TimerController>().addTime(Time.deltaTime);
				}
			}
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
	
	public void loadHisHoveredPC(int c){
		Card card = this.playingCards[c].GetComponent<PlayingCardController>().getCard();
		List<Skill> skills = this.playingCards[c].GetComponent<PlayingCardController>().getCard().getSkills();
		GameObject.Find("Title").GetComponent<TextMeshPro>().text = card.Title;
		GameObject.Find("HisSpecialite").GetComponent<TextMeshPro>().text = skills[0].Name;
		GameObject.Find("HisSpecialiteDescription").GetComponent<TextMeshPro>().text = skills[0].Description;
		
		this.hisHoveredRPC.GetComponent<SpriteRenderer>().sprite = this.sprites[card.ArtIndex];
		
		for (int i = 1 ; i < skills.Count ; i++){
			GameObject.Find("Skill"+(i-1)+"Title").GetComponent<TextMeshPro>().text = skills[i].Name;
			GameObject.Find(("Skill"+(i-1)+"Description")).GetComponent<TextMeshPro>().text = skills[i].Description;
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
		
		GameObject go = GameObject.Find("toDisplay");
		foreach (Transform child in go.transform)
		{
			child.gameObject.SetActive(true);
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
	
	public void updateMyTimeBar(float percentage){
		GameObject llbr = GameObject.Find("LLBRight");	
		GameObject leb = GameObject.Find("LLBRightEnd");
		GameObject llbl = GameObject.Find("LLBLeft");	
		GameObject lcb = GameObject.Find("LLBLeftEnd");
		lcb.transform.position = new Vector3(llbr.transform.position.x-0.5f+(percentage)*(-llbr.transform.position.x+0.5f+(llbl.transform.position.x+0.1f))/100, 4.5f, 0);
		
		GameObject llbb = GameObject.Find("LLBBar");
		llbb.transform.position = new Vector3((leb.transform.position.x+lcb.transform.position.x)/2f, 4.5f, 0);
		llbb.transform.localScale = new Vector3((leb.transform.position.x-lcb.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
	}
	
	public void updateHisTimeBar(float percentage){
		GameObject rlbl = GameObject.Find("RLBLeft");
		GameObject rlbr = GameObject.Find("RLBRight");
		GameObject rlbc = GameObject.Find("RLBCenter");		
		GameObject reb = GameObject.Find("RLBRightEnd");
		reb.transform.position = new Vector3(rlbl.transform.position.x+0.5f+(percentage)*(-rlbl.transform.position.x-0.5f+(rlbr.transform.position.x-0.1f))/100, 4.5f, 0);
		
		GameObject rcb = GameObject.Find("RLBLeftEnd");
		rcb.transform.position = new Vector3(1.20f, 4.5f, 0);
		
		GameObject rlbb = GameObject.Find("RLBBar");
		rlbb.transform.position = new Vector3((reb.transform.position.x+rcb.transform.position.x)/2f, 4.5f, 0);
		rlbb.transform.localScale = new Vector3((reb.transform.position.x-rcb.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
	}
	
	
	public void resizeBackground()
	{		
		Vector3 position;
		this.realwidth = 10f*this.widthScreen/this.heightScreen;
		float tileScale = 8f / this.boardHeight;
		
		for (int i = 0; i < this.horizontalBorders.Length; i++)
		{
			position = new Vector3(0, -4f + tileScale * i, -1f);
			this.horizontalBorders [i].transform.localPosition = position;
			this.horizontalBorders [i].transform.localScale = new Vector3(1f,0.5f,1f);
		}
		
		for (int i = 0; i < this.verticalBorders.Length; i++)
		{
			position = new Vector3((-this.boardWidth/2f+i)*tileScale, 0f, -1f);
			this.verticalBorders [i].transform.localPosition = position;
			this.verticalBorders [i].transform.localScale = new Vector3(0.5f,1f,1f);
		}
		
		GameObject tempGO = GameObject.Find("MyPlayerName");
		position = tempGO.transform.position ;
		position.x = -0.48f*this.realwidth;
		tempGO.transform.position = position;
		tempGO.GetComponent<TextContainer>().width = 0.48f*this.realwidth-3f ;
		
		tempGO = GameObject.Find("HisPlayerName");
		position = tempGO.transform.position ;
		position.x = 0.48f*this.realwidth;
		tempGO.transform.position = position;
		tempGO.GetComponent<TextContainer>().width = 0.48f*this.realwidth-3f ;
		
		GameObject llbl = GameObject.Find("LLBLeft");
		llbl.transform.position = new Vector3(-this.realwidth/2f+0.25f, 4.5f, 0);
		
		GameObject llbr = GameObject.Find("LLBRight");
		llbr.transform.position = new Vector3(-1.2f, 4.5f, 0);
		
		GameObject llbc = GameObject.Find("LLBCenter");
		llbc.transform.position = new Vector3((llbl.transform.position.x+llbr.transform.position.x)/2f, 4.5f, 0);
		llbc.transform.localScale = new Vector3((-llbl.transform.position.x+llbr.transform.position.x-0.49f)/10f, 0.5f, 1f);
		
		GameObject leb = GameObject.Find("LLBRightEnd");
		leb.transform.position = new Vector3(-1.2f, 4.5f, 0);
		
		GameObject lcb = GameObject.Find("LLBLeftEnd");
		lcb.transform.position = new Vector3(llbr.transform.position.x-0.5f+100f*(-llbr.transform.position.x+0.5f+(llbl.transform.position.x+0.1f))/100, 4.5f, 0);
		
		GameObject llbb = GameObject.Find("LLBBar");
		llbb.transform.position = new Vector3((leb.transform.position.x+lcb.transform.position.x)/2f, 4.5f, 0);
		llbb.transform.localScale = new Vector3((leb.transform.position.x-lcb.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
		
		GameObject rlbl = GameObject.Find("RLBLeft");
		rlbl.transform.position = new Vector3(1.2f, 4.5f, 0);
		
		GameObject rlbr = GameObject.Find("RLBRight");
		rlbr.transform.position = new Vector3(this.realwidth/2f-0.25f, 4.5f, 0);
		
		GameObject rlbc = GameObject.Find("RLBCenter");
		rlbc.transform.position = new Vector3((rlbl.transform.position.x+rlbr.transform.position.x)/2f, 4.5f, 0);
		rlbc.transform.localScale = new Vector3((-rlbl.transform.position.x+rlbr.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
		
		GameObject reb = GameObject.Find("RLBRightEnd");
		reb.transform.position = new Vector3(rlbl.transform.position.x+0.5f+100f*(-rlbl.transform.position.x-0.5f+(rlbr.transform.position.x-0.1f))/100, 4.5f, 0);
		
		GameObject rcb = GameObject.Find("RLBLeftEnd");
		rcb.transform.position = new Vector3(1.20f, 4.5f, 0);
		
		GameObject rlbb = GameObject.Find("RLBBar");
		rlbb.transform.position = new Vector3((reb.transform.position.x+rcb.transform.position.x)/2f, 4.5f, 0);
		rlbb.transform.localScale = new Vector3((reb.transform.position.x-rcb.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
		
		this.getMyHoveredCardController().resize(realwidth, tileScale);
		this.getHisHoveredCardController().resize(realwidth, tileScale);
		this.interlude.GetComponent<InterludeController>().resize(realwidth);
		
	}
	
	public void hitExternalCollider(){
		GameView.instance.hoverTile();
	}
	
	public int getNbPlayingCards(){
		return this.playingCards.Count;
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
	
	public void calculateDestinations(int i){
		bool[,] hasBeenPassages = new bool[this.boardWidth, this.boardHeight];
		bool[,] isDestination = new bool[this.boardWidth, this.boardHeight];
		for(int l = 0 ; l < this.boardWidth ; l++){
			for(int k = 0 ; k < this.boardHeight ; k++){
				hasBeenPassages[l,k]=false;
				isDestination[l,k]=false;
			}
		}
		List<Tile> destinations = new List<Tile>();
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
		
		this.playingCards[i].GetComponent<PlayingCardController>().setDestinations(destinations);
	}
	
	public void recalculateDestinations(){
		for (int i = 0 ; i < playingCards.Count ; i++){
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
		for (int i = 0 ; i < this.playingCards.Count ; i++){
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

	public void displayAdjacentUnitsTargets(){
		List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardController(this.currentPlayingCard).getTile());
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
		
		for (int i = 0; i < this.playingCards.Count; i++)
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
	
	public void displayAllButMeModifiersTargets()
	{
		PlayingCardController pcc;
		Tile tile ;
		this.targets = new List<Tile>();
		
		for (int i = 0; i < this.playingCards.Count; i++)
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
		
		for (int i = 0; i < this.playingCards.Count; i++)
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

	public string canLaunchAdjacentUnits()
	{
		string isLaunchable = "Aucune unité à proximité";
		
		List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardTile(this.currentPlayingCard));
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
		
		for (int i = 0; i < this.playingCards.Count; i++)
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
		
		for (int i = 0; i < this.playingCards.Count; i++)
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
		
		for (int i = 0; i < this.playingCards.Count; i++)
		{
			pcc = this.getPlayingCardController(i);
			if (this.getCard(i).isMine && pcc.canBeTargeted() && i != this.currentPlayingCard)
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
	
	public void removeLeaderEffect(int target, bool b){
		if(b){
			for(int j = 0 ; j < playingCards.Count ; j++){
				if(this.getCard(j).isMine && target!=j){
					this.getCard(j).removeLeaderEffect();
					this.getPlayingCardController(j).updateLife(this.getCard(j).getLife());
					this.getPlayingCardController(j).show();
					this.displaySkillEffect(j, "Leader mort\nPerd ses bonus", 0);
					GameView.instance.addAnim(GameView.instance.getTile(j), 76);
				}
			}
		}
		else{
			for(int j = 0 ; j < playingCards.Count ; j++){
				if(!this.getCard(j).isMine && target!=j){
					this.getCard(j).removeLeaderEffect();
					this.getPlayingCardController(j).updateLife(this.getCard(j).getLife());
					this.getPlayingCardController(j).show();
					this.displaySkillEffect(j, "Leader\nPerd ses bonus", 0);
					GameView.instance.addAnim(GameView.instance.getTile(j), 76);
				}
			}
		}
	}
	
	public void killHandle(int c){
		if(this.areAllMyPlayersDead()){
			StartCoroutine(quitGame());
		}
		else{
			if(c!=this.currentPlayingCard){
				for (int i = 0 ; i < this.playingCards.Count ; i++){
					if(this.getCard(i).nbTurnsToWait>this.getCard(c).nbTurnsToWait){
						this.getCard(i).nbTurnsToWait--;
						if(i!=this.currentPlayingCard){
							this.getPlayingCardController(i).show();
						}
						else{
							this.getPlayingCardController(i).show();
						}
					}
				}
			}
			else{
				this.toPassDead = true ;
			}
		}

		if(this.getCard(this.currentPlayingCard).isSanguinaire()){
			GameCard currentCard = GameView.instance.getCurrentCard();
			int target = GameView.instance.getCurrentPlayingCard();
			int bonus = GameView.instance.getCurrentSkill().Power*3;

			GameView.instance.getCard(target).magicalBonusModifyers.Add(new Modifyer(bonus, -1, 34, "Sanguinaire", "+"+bonus+"% aux dégats à distance. Permanent"));
			GameView.instance.getPlayingCardController(target).updateAttack();
			GameView.instance.displaySkillEffect(target, "Dégats +"+bonus+"%", 1);
			GameView.instance.addAnim(GameView.instance.getTile(target), 34);
		}

		this.getPlayingCardController(c).displayDead(true);
		this.deads.Add(c);
	}
	
	IEnumerator sendStat(string user1, string user2, bool isTutorialGame)
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
				yield return (StartCoroutine(this.sendStat(ApplicationModel.myPlayerName, ApplicationModel.myPlayerName, true)));
			}
			else
			{
				yield return (StartCoroutine(this.sendStat(ApplicationModel.myPlayerName, "Garruk", true)));
			}
		}
		else
		{
			if(!isFirstPlayer)
			{
				hasFirstPlayerWon=true;
			}
			yield return (StartCoroutine(this.sendStat(ApplicationModel.hisPlayerName, ApplicationModel.myPlayerName, false)));
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
		for(int i = 0 ; i < this.playingCards.Count; i++){
			if(i!=CPC && !GameView.instance.getCard(i).isDead && GameView.instance.getCard(i).isMine){
				allys.Add(i);
			}
		}
		return allys;
	}
	
	public List<int> getOpponents(){
		List<int> opponents = new List<int>();
		for(int i = 0 ; i < this.playingCards.Count;i++){
			if(!this.getCard(i).isDead && !this.getCard(i).isMine){
				opponents.Add(i);
			}
		}
		return opponents;
	}
	
	public List<int> getEveryone(){
		List<int> everyone = new List<int>();
		for(int i = 0 ; i < this.playingCards.Count;i++){
			if(!this.getCard(i).isDead){
				everyone.Add(i);
			}
		}
		return everyone;
	}
	
	public List<int> getEveryoneButMe(){
		List<int> everyone = new List<int>();
		for(int i = 0 ; i < this.playingCards.Count;i++){
			if(!this.getCard(i).isDead && i!=this.currentPlayingCard){
				everyone.Add(i);
			}
		}
		return everyone;
	}
	
	public int countAlive(){
		int compteur = 0 ;
		for (int i = 0 ; i < this.playingCards.Count ; i++){
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
			for (int i = 0 ; i < this.playingCards.Count ; i++){
				if (this.getCard(i).isMine){
					if (!this.getCard(i).isDead){
						areMyPlayersDead = false ;
					}
				}
			}
			if(!areMyPlayersDead){
				areMyPlayersDead = true ;
				for (int i = 0 ; i < this.playingCards.Count ; i++){
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
			for (int i = 0 ; i < this.playingCards.Count ; i++){
				if (this.getCard(i).isMine){
					if (!this.getCard(i).isDead){
						areMyPlayersDead = false ;
					}
				}
			}
		}
		return areMyPlayersDead ;
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
				this.clickDestination(chosenTile, this.currentPlayingCard);
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
			this.clickDestination(chosenTile, this.currentPlayingCard);
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
				this.clickDestination(chosenTile, this.currentPlayingCard);
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
							print("Je trouve ("+chosenTile.x+","+chosenTile.y+")");
						}
					}
				}
			}
			this.clickDestination(chosenTile, this.currentPlayingCard);
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
		string s = GameSkills.instance.getSkill(this.runningSkill).name;
		this.getPassZoneController().show(false);
		this.getSkillZoneController().showCancelButton(false);
		this.getSkillZoneController().showSkillButtons(false);
		this.hoverTile();
		if(GameSkills.instance.getSkill(this.runningSkill).ciblage>0){
			this.displaySkillEffect(this.currentPlayingCard,s,1);
			this.addSE(this.getTile(this.currentPlayingCard));
		}
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
		this.runningSkill = -1;
		if(this.getCard(this.currentPlayingCard).isMine){
			if(this.getCard(this.currentPlayingCard).hasPlayed && this.getCard(this.currentPlayingCard).hasMoved){
				yield return new WaitForSeconds(3f);
				if(this.getCard(this.currentPlayingCard).getLife()>0){
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
	}
	
	public void initTileTargetHandler(int numberOfExpectedTargets)
	{
		this.targetTileHandler = new TargetTileHandler(numberOfExpectedTargets);
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
			this.popUp.GetComponent<PopUpGameController>().setTexts("Etape 1 : L'arène", "Bienvenue dans l'arène de combat de Cristalia.\nChaque case peut etre normale, infranchissable, ou meme piégée. Prenez garde où vous mettez vos pieds ! (ou plutot ceux de vos unités)\n\nVos unités sont disposées à l'entrée de l'arène, vous pouvez les déplacer avant le début du combat en cliquant dessus!");
			this.popUp.GetComponent<PopUpGameController>().changePosition(new Vector3(-0.05f, 0f, 0f));
			this.popUp.GetComponent<PopUpGameController>().show(true);
		}
		else if (i==2){
			this.popUp.GetComponent<PopUpGameController>().setTexts("Etape 2 : Chacun son tour", "Chaque colon joue à son tour, l'ordre des personnages étant déterminé par leur position dans l'équipe. Un personnage peut à chaque tour se déplacer et/ou utiliser une compétence, dans n'importe quel ordre.\n\nCristalia et ses lunes ne possèdent pas d'atmosphère, ce qui rend les combats difficiles : prenez bien garde à ne pas épuiser vos réserves d'oxygène ou vous unités perdront le combat !\n\nA vous de jouer désormais, profitez de votre tour pour vous rapprocher des unités du colon ennemi.");
			this.popUp.GetComponent<PopUpGameController>().changePosition(new Vector3(-0.05f, 0f, 0f));
			this.popUp.GetComponent<PopUpGameController>().show(true);
		}
		else if (i==3){
			this.popUp.GetComponent<PopUpGameController>().setTexts("Etape 3 : Règles du combat", "Félicitations, vous avez donné vos premiers ordres de chef de guerre. Mais la bataille n'est pas terminée !\n\nLe combat se termine quand un colon ne dispose plus d'unités pour se battre OU s'il a épuisé le temps mis à sa disposition. Je vous laisse désormais seul pour terminer le combat, bon courage !");
			this.popUp.GetComponent<PopUpGameController>().changePosition(new Vector3(-0.05f, 0f, 0f));
			this.popUp.GetComponent<PopUpGameController>().show(true);
		}
	}
	
	public void hideTuto(){
		this.popUp.GetComponent<PopUpGameController>().show(false);
		this.interlude.GetComponent<InterludeController>().unPause();
	}
	
	public void showResult(bool isSuccess)
	{
		if(isSuccess){
			this.interlude.GetComponent<InterludeController>().setUnderText("Succès !");
		}
		else{
			this.interlude.GetComponent<InterludeController>().setUnderText("Echec !");
		}
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
		this.skillEffects.Add(t);
		this.getTileController(t).showEffect(true);
	}

	public void removeSE(Tile t){
		for(int i = skillEffects.Count-1 ; i >= 0 ; i--){
			if(skillEffects[i].x==t.x && skillEffects[i].y==t.y){
				skillEffects.RemoveAt(i);
			}
		}
		this.getTileController(t).showEffect(false);
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
		this.validationSkill.GetComponent<SkillValidationController>().setTexts(s,d,"Lancer");
		this.validationSkill.GetComponent<SkillValidationController>().show(true);
		this.isDisplayedPopUp = true ;
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
		this.playingCards.Add((GameObject)Instantiate(this.playingCardModel));
		int index = this.playingCards.Count-1;
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
	}

	public void stopCountingTime(){
		this.toCountTime = false ;
	}
}



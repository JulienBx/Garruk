using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using TMPro ;

public class GameView : MonoBehaviour
{
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
	
	public int boardWidth ;
	public int boardHeight ;
	public int nbCardsPerPlayer ;
	public int nbFreeRowsAtBeginning ;
	public int turnTime ;
	
	bool isLoadingScreenDisplayed = false ;
	bool isTutorialLaunched = false;
	
	GameObject loadingScreen;
	GameObject[,] tiles ;
	GameObject[] skillButtons ;
	GameObject attackButton ;
	GameObject passButton ;
	GameObject myHoveredRPC ;
	GameObject hisHoveredRPC ;
	GameObject[] verticalBorders ;
	GameObject[] horizontalBorders ;
	List<GameObject> playingCards ;
	GameObject popUp;
	GameObject tutorial;
	GameObject timerGO;
	GameObject SB;
	
	int heightScreen = -1;
	int widthScreen = -1;
	
	AudioSource audioEndTurn;
	
	public bool isTargeting = false ;
	float timerTargeting ;
	float targetingTime = 0.5f ;
	bool isTargetingHaloOn = false ;
	
	int currentPlayingCard = -1;
	bool isFirstPlayer = false;
	
	public Deck myDeck ;
	int nbPlayersReadyToFight = 0 ;
	
	bool hasFightStarted = false ;
	bool isBackgroundLoaded ;
	
	float realwidth ;
		
	void Awake()
	{
		instance = this;
		this.displayLoadingScreen ();
		this.tiles = new GameObject[this.boardWidth, this.boardHeight];
		this.playingCards = new List<GameObject>();
		this.verticalBorders = new GameObject[this.boardWidth+1];
		this.horizontalBorders = new GameObject[this.boardHeight+1];
		this.skillButtons = new GameObject[3];
		this.attackButton = GameObject.Find("AttackButton");
		this.passButton = GameObject.Find("PassButton");
		this.skillButtons[0] = GameObject.Find("SkillButton0");
		this.skillButtons[1] = GameObject.Find("SkillButton1");
		this.skillButtons[2] = GameObject.Find("SkillButton2");
		this.myHoveredRPC = GameObject.Find("MyHoveredPlayingCard");
		this.hisHoveredRPC = GameObject.Find("HisHoveredPlayingCard");
		this.popUp = GameObject.Find("PopUp");
		this.timerGO = GameObject.Find("Timer");
		this.SB = GameObject.Find("SB");
		this.SB.GetComponent<StartButtonController>().show(false);
		this.audioEndTurn = GetComponent<AudioSource>();
		this.setMyPlayerName(ApplicationModel.myPlayerName);
		this.setHisPlayerName(ApplicationModel.hisPlayerName);
		this.isFirstPlayer = ApplicationModel.isFirstPlayer;
		this.createBackground();
		
		if (ApplicationModel.launchGameTutorial)
		{
			this.isTutorialLaunched = true ;
			ApplicationModel.launchGameTutorial=false;
			this.launchTuto();
		}
		else{
			this.isTutorialLaunched = false ;
		}
		
		if (this.isFirstPlayer)
		{
			this.initGrid();
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
		
		if (isFirstP){
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().setTile(new Tile(c.deckOrder + 1, hauteur), tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().getPosition());
			this.tiles [c.deckOrder + 1, hauteur].GetComponent<TileController>().setCharacterID(index);
		}
		else{
			this.playingCards [index].GetComponentInChildren<PlayingCardController>().setTile(new Tile(4-c.deckOrder, hauteur), tiles [4-c.deckOrder, hauteur].GetComponent<TileController>().getPosition());
			this.tiles [4-c.deckOrder, hauteur].GetComponent<TileController>().setCharacterID(index);
		}
		
		GameCard gc = this.getCard(index);
		gc.checkPassiveSkills();
		if(gc.isPiegeur()){
			if(isFirstP==this.isFirstPlayer){
				List<Tile> tiles = ((Piegeur)GameSkills.instance.getSkill(64)).getTiles(gc.getPassiveSkillLevel(), this.boardWidth, this.boardHeight, this.nbFreeRowsAtBeginning);
				for (int i = 0 ; i < tiles.Count ; i++){
					GameController.instance.addPiegeurTrap(tiles[i], gc.getPassiveSkillLevel());
				}
			}
		}
		
		this.playingCards [index].GetComponentInChildren<PlayingCardController>().show(false);
	}
	
	public void addTrap(Trap trap, Tile tile)
	{
		this.getTileController(tile.x, tile.y).setTrap(trap);
	}
	
	public void loadDeck(Deck deck, bool isFirstP){
		for (int i = 0; i < ApplicationModel.nbCardsByDeck; i++)
		{
			this.createPlayingCard(deck.getGameCard(i), isFirstP);
		}
		
		if (isFirstP == this.isFirstPlayer)
		{
			this.myDeck = deck;
			this.setInitialDestinations(this.isFirstPlayer);
			this.showStartButton();
			GameView.instance.checkPassiveSkills(true);
		}
		else{
			GameView.instance.checkPassiveSkills(false);
		}
		
		if (GameView.instance.getIsTutorialLaunched())
		{
			TutorialObjectController.instance.actionIsDone();
		}
		
		//		if (this.nbPlayers==2){
		//			this.bothPlayerLoaded = true ;
		//		}
		
		if (GameView.instance.getIsTutorialLaunched())
		{
			
		}
	}
	
	public void setInitialDestinations(bool isFirstP)
	{
		print ("DESTINATIONS");
		int debut = 0 ;
		if (!isFirstP){
			debut = this.boardHeight-this.nbFreeRowsAtBeginning;
		}
		for (int i = debut ; i < debut + this.nbFreeRowsAtBeginning; i++){
			for (int j = 0 ; j < this.boardWidth; j++){
				print ("je teste "+j+","+j);
				
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
		for (int i = 0; i < this.boardWidth; i++)
		{
			for (int j = 0; j < this.boardHeight; j++)
			{
				if(this.getTileController(i,j).getIsDestination()!=-1){
					this.getTileController(i,j).removeDestination();
				}
			}
		}
	}
	
	public void playerReadyR(){
		nbPlayersReadyToFight++;
		if (nbPlayersReadyToFight == 2)
		{
			this.SB.GetComponent<StartButtonController>().show(false);
			this.displayOpponentCards();
			if (this.isFirstPlayer)
			{
				this.StartFight();
			}
		}
	}
	
	public void displayOpponentCards(){
		for (int i = 0; i < this.playingCards.Count; i++)
		{
			this.playingCards [i].GetComponentInChildren<PlayingCardController>().display();
			this.playingCards [i].GetComponentInChildren<PlayingCardController>().show (true);
		}
	}
	
	public void StartFight()
	{		
		this.sortAllCards();
		GameController.instance.findNextPlayer();
	}
	
	public void sortAllCards()
	{
		List <int> quicknessesToRank = new List<int>();
		int length = GameView.instance.getNbPlayingCards();
		int nbTurnsToWait ;
		
		for (int i = 0; i < playingCards.Count; i++)
		{
			quicknessesToRank.Add(this.getCard(i).getGameSpeed());
		}
		
		for (int i = 0; i < playingCards.Count; i++)
		{
			nbTurnsToWait = 1; 
			for (int j = 0; j < playingCards.Count; j++)
			{
				if(i==j){
				
				}
				else if(quicknessesToRank[i]<quicknessesToRank[j]){
					nbTurnsToWait++;
				}
				else if(quicknessesToRank[i]==quicknessesToRank[j]){
					if(i<j){
						nbTurnsToWait++;
					}
				}
			}
			GameController.instance.addRankedCharacter(i, nbTurnsToWait);
		}
	}
	
	public void hoverCharacter(int characterID){
		
	}
	
	public void setNextPlayer(){
//		if(this.hasFightStarted){
//			if(this.getCard(this.currentPlayingCard).isNurse()){
//				List<Tile> targets = new List<Tile>();
//				if(GameView.instance.getIsMine(this.currentPlayingCard)){
//					targets = GameView.instance.getAllyImmediateNeighbours(GameView.instance.getPlayingCardTile(this.currentPlayingCard));
//				}
//				else{
//					targets = GameView.instance.getOpponentImmediateNeighbours(GameView.instance.getPlayingCardTile(this.currentPlayingCard));
//				}
//				for (int i = 0 ; i < targets.Count ; i++){
//					int target = GameView.instance.getTileCharacterID(targets[i].x, targets[i].y);
//					int amount = Mathf.CeilToInt(GameView.instance.getCard(this.currentPlayingCard).getPassiveManacost()*GameView.instance.getCard(target).GetTotalLife()/100f);
//					this.addCardModifier(target, -1*amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//					GameView.instance.displaySkillEffect(target, "INFIRMIER\n+"+amount+" PV", 4);
//				}
//			}
//			else if(GameView.instance.getCard(this.currentPlayingCard).isFrenetique()){
//				int amount = GameView.instance.getCard(this.currentPlayingCard).getPassiveManacost();
//				int amountAttack = Mathf.CeilToInt(GameView.instance.getCard(this.currentPlayingCard).GetAttack()*amount / 100f);
//				
//				GameView.instance.getCard(this.currentPlayingCard).addModifier(amountAttack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 20, "FRENESIE", "+"+amount+" ATK. Permanent.", "Permanent");
//				GameView.instance.show(this.currentPlayingCard, false);
//			}
//		}
//		else{
//			this.hasFightStarted = true ;
//		}
//		
//		this.timeRunsOut(2);
//		
//		this.turnsToWait = 100;
//		
//		bool newTurn = true;
//		int nextPlayingCard = -1;
//		int i2 = 0;
//		int length = this.playingCardTurnsToWait.Count;
//		
//		while (i2 < length && newTurn == true)
//		{
//			if(!GameView.instance.isDead(i2)){
//				this.playingCardTurnsToWait[i2]--;
//				
//				if(GameView.instance.getIsMine(i2)){
//					if(this.playingCardTurnsToWait[i2]<this.turnsToWait){
//						this.turnsToWait = this.playingCardTurnsToWait[i2];
//					}
//				}
//				
//				if (this.playingCardTurnsToWait[i2]==0)
//				{
//					this.playingCardTurnsToWait[i2]=GameView.instance.countAlive();
//					nextPlayingCard = i2;
//				}
//			}
//			i2++;
//		}
//		
//		GameView.instance.recalculateDestinations();
//		this.initPlayer(nextPlayingCard)
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
	
	public PlayingCardController getPlayingCardController(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>();
	}
	
	public void launchTuto(){
		//this.tutorial = Instantiate(this.TutorialObject) as GameObject;
		//this.tutorial.AddComponent<GameTutorialController>();
		//StartCoroutine(this.tutorial.GetComponent<GameTutorialController>().launchSequence(0));
	}
	
	
	
	
	
	
	
	
	void Update()
	{
		if (this.widthScreen!=-1){
			if (this.widthScreen!=Screen.width){
				this.resize();
			}
		}
		
		if (this.SB.GetComponent<StartButtonController>().getIsPushed()){
			this.SB.GetComponent<StartButtonController>().addTime(Time.deltaTime);
		}
		
//		if(GameController.instance.getCurrentPlayingCard()!=-1){
//			this.playingCards[GameController.instance.getCurrentPlayingCard()].GetComponent<PlayingCardController>().addTime(Time.deltaTime);
//		}
//		
//		if(this.SkillWindowLeft.GetComponent<SkillWindowLeft>().getTimeToDisplay()!=0){
//			this.SkillWindowLeft.GetComponent<SkillWindowLeft>().addTime(Time.deltaTime);
//		}
//		if(this.SkillWindowRight.GetComponent<SkillWindowRight>().getTimeToDisplay()!=0){
//			this.SkillWindowRight.GetComponent<SkillWindowRight>().addTime(Time.deltaTime);
//		}
//		if(this.Thunder.GetComponent<SkillThunder>().getTimeToDisplay()!=0){
//			this.Thunder.GetComponent<SkillThunder>().addTime(Time.deltaTime);
//		}
//		
//		if(timerTurn>0){
//			this.timerTurn += Time.deltaTime; 
//			if (this.timerTurn<=0 && this.getIsMine(GameController.instance.getCurrentPlayingCard())){
//				GameController.instance.resolvePass();
//			}
//			else{
//				if(this.timerSeconds != Mathf.Min(Mathf.FloorToInt(this.timerTurn))){
//					this.timerSeconds = Mathf.Min(Mathf.FloorToInt(this.timerTurn));
//					if(this.timerSeconds>9){
//						this.timerGO.GetComponent<TextMeshPro>().text = this.timerSeconds.ToString();
//					}
//					else{
//						this.timerGO.GetComponent<TextMeshPro>().text = "0"+this.timerSeconds.ToString();
//					}
//				}
//			}
//		}
//		
//		if (this.getMyHoveredCardController().getStatus()!=0){
//			this.getMyHoveredCardController().addTime(Time.deltaTime);
//		}
//		
//		if (this.getHisHoveredCardController().getStatus()!=0){
//			this.getHisHoveredCardController().addTime(Time.deltaTime);
//		}
//		
//		if (statusSkill==1){
//			this.timerSkill += Time.deltaTime;
//			this.skillPosition.x = (5f+(realwidth/2f)-8.25f)-(Mathf.Min(1,this.timerSkill/this.animationTime))*(5f+(realwidth/2f)-8.25f);
//			this.skillRPC.transform.localPosition = this.skillPosition ;
//			if (timerSkill>animationTime){
//				statusSkill = 0 ;
//				this.isDisplayedSkill = true ;
//			}
//		}
//		else if (statusSkill<0){
//			this.timerSkill += Time.deltaTime;
//			this.skillPosition.x = (Mathf.Min(1,this.timerSkill/this.animationTime))*(5f+(realwidth/2f)-8.25f);
//			this.skillRPC.transform.localPosition = this.skillPosition ;
//			if (timerSkill>animationTime){
//				if (statusSkill==-2){
//					this.loadSkill();
//					statusSkill = 1 ;
//					this.timerSkill = 0 ;
//				}
//				else{
//					statusSkill = 0 ;
//				}
//				this.isDisplayedSkill = false ;
//			}
//		}
//		
//		if (this.isTargeting){
//			this.timerTargeting += Time.deltaTime;
//			Tile t ;
//			
//			if (this.timerTargeting>this.targetingTime){
//				if(this.isTargetingHaloOn){
//					for (int i = 0 ; i < this.targets.Count ; i++){	
//						t = this.targets[i];
//						if(!this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().getIsHovered()){
//							this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().disable() ;
//						}
//					}
//				}
//				else{
//					for (int i = 0 ; i < this.targets.Count ; i++){	
//						t = this.targets[i];
//						if(!this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().getIsHovered()){
//							this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().enable() ;
//						}
//					}
//				}
//				this.isTargetingHaloOn = !this.isTargetingHaloOn;
//				this.timerTargeting = 0;
//			}
//		}
//		
//		if(this.toDisplayDeadHalos){
//			for (int i = this.displayedDeads.Count-1 ; i >= 0 ; i--){
//				this.displayedDeadsTimer[i] -= Time.deltaTime;
//				if (this.displayedDeadsTimer[i]<=0f){
//					Tile t = this.getPlayingCardTile(this.displayedDeads[i]);
//					this.disappear(this.displayedDeads[i]);
//					this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().setCharacterID(-1);
//					this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().disable();
//					this.recalculateDestinations();
//					this.displayDestinations(GameController.instance.getCurrentPlayingCard());
//					this.displayedDeads.RemoveAt(i);
//					this.displayedDeadsTimer.RemoveAt(i);
//				}
//			}
//			if (this.displayedDeads.Count==0){
//				this.toDisplayDeadHalos = false ;
//			}
//		}
//		
//		if(this.toDisplaySE){
//			for (int i = this.displayedSE.Count-1 ; i >= 0 ; i--){
//				this.displayedSETimer[i] -= Time.deltaTime;
//				if (this.displayedSETimer[i]<=0f){
//					Tile t = this.getPlayingCardTile(this.displayedSE[i]);
//					this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().moveBack();
//					this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().disable();
//					this.displayedSE.RemoveAt(i);
//					this.displayedSETimer.RemoveAt(i);
//				}
//			}
//			if (this.displayedSE.Count==0){
//				this.toDisplaySE = false ;
//			}
//		}
	}
	
	public bool getIsTargetingHaloOn(){
		return this.isTargetingHaloOn ;
	}
	
	public bool getIsTargeting(){
		return this.isTargeting ;
	}
	
	
	public void setSkillPopUp(string texte, Card launcher, List<Card> receivers, List<string> textsReceivers){
//		if(launcher.isMine){
//			this.SkillWindowLeft.GetComponent<SkillWindowLeft>().setLauncher(launcher.Title+" Niv."+launcher.ExperienceLevel+texte, this.getCharacterSprite(launcher.IdClass));
//		}
//		else{
//		
//		}
//		this.SkillWindowLeft.GetComponent<SkillWindowLeft>().setTimeToDisplay(7f);
//		this.SkillWindowRight.GetComponent<SkillWindowRight>().setTimeToDisplay(7f);
//		this.Thunder.GetComponent<SkillThunder>().setTimeToDisplay(7f);
//		this.Thunder.GetComponent<SkillThunder>().show();
//		
//		//this.skillPopUp.GetComponent<SkillPopUpController>().setPopUp(texte, launcher, receivers, textsReceivers);
		
	}
	
	public void setSkillPopUp(string texte, Card launcher, List<Card> receivers, List<string> textesPV, List<string> textesAttack,  List<string> textesMove,  List<string> textesEffects, List<int> status){
//		if(launcher.isMine){
//			this.SkillWindowLeft.GetComponent<SkillWindowLeft>().setLauncher(launcher.Title+" lance...", this.getCharacterSprite(launcher.IdClass));
//			this.SkillWindowRight.GetComponent<SkillWindowRight>().setReceivers(receivers, textesPV, textesAttack,textesMove, textesEffects,status);
//		}
//		else{
//			this.SkillWindowRight.GetComponent<SkillWindowRight>().setLauncher(launcher.Title+" lance...", this.getCharacterSprite(launcher.IdClass));
//			this.SkillWindowLeft.GetComponent<SkillWindowLeft>().setReceivers(receivers, textesPV, textesAttack,textesMove, textesEffects,status);
//		}
//		this.SkillWindowLeft.GetComponent<SkillWindowLeft>().setTimeToDisplay(7f);
//		this.SkillWindowRight.GetComponent<SkillWindowRight>().setTimeToDisplay(7f);
//		this.Thunder.GetComponent<SkillThunder>().setTimeToDisplay(7f);
//		this.Thunder.GetComponent<SkillThunder>().show();
		
		//this.skillPopUp.GetComponent<SkillPopUpController>().setPopUp(texte, launcher, receivers, textsReceivers);
		
	}
	
	public void displaySkills(bool b){
		this.skillButtons[0].SetActive(b);
		this.skillButtons[1].SetActive(b);
		this.skillButtons[2].SetActive(b);
		this.skillButtons[3].SetActive(b);
		this.skillButtons[4].SetActive(b);
	}
	
	public void showSkills(){
		this.skillButtons[0].GetComponent<BoxCollider>().enabled = true;
		this.skillButtons[1].GetComponent<BoxCollider>().enabled = true;
		this.skillButtons[2].GetComponent<BoxCollider>().enabled = true;
		this.skillButtons[3].GetComponent<BoxCollider>().enabled = true;
		this.skillButtons[4].GetComponent<BoxCollider>().enabled = true;
		//this.actionButtons.GetComponent<TextMeshPro>().text = "" ;
	}
	
	public void overloadSkills(string s){
		this.skillButtons[0].GetComponent<BoxCollider>().enabled = false;
		this.skillButtons[1].GetComponent<BoxCollider>().enabled = false;
		this.skillButtons[2].GetComponent<BoxCollider>().enabled = false;
		this.skillButtons[3].GetComponent<BoxCollider>().enabled = false;
		this.skillButtons[4].GetComponent<BoxCollider>().enabled = false;
		//this.actionButtons.GetComponent<TextMeshPro>().text = s ;
	}
	
	public void changeOverloadText(string s){
		//this.actionButtons.GetComponent<TextMeshPro>().text = s ;
	}
	
	public void unSelectPC(int p){
		this.playingCards[p].GetComponent<PlayingCardController>().resetTimer();
	}
	
	public void unClickPC(int p){
		Tile t = this.getPlayingCardTile(p);
		//this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().disable();
	}
	
	public MyHoveredCardController getMyHoveredCardController(){
		return this.myHoveredRPC.GetComponent<MyHoveredCardController>();
	}
	
	public HisHoveredCardController getHisHoveredCardController(){
		return this.hisHoveredRPC.GetComponent<HisHoveredCardController>();
	}
	
	public void initTimer(){
//		this.timerTurn = this.turnTime ;
//		this.timerSeconds = Mathf.FloorToInt(this.timerTurn) ;
//		this.timerGO.GetComponent<TextMeshPro>().text = this.timerSeconds.ToString();
	}
	
	public void selectSkill(int id){
//		this.unSelectSkill();
//		GameObject.Find ("Skill"+id+"Title").GetComponent<TextMeshPro>().color = Color.yellow;
//		GameObject.Find ("Skill"+id+"Description").GetComponent<TextMeshPro>().color = Color.yellow;
	}
	
	public void unSelectSkill(){
//		for(int i = 0 ; i < 4 ; i++){
//			GameObject.Find ("Skill"+i+"Title").GetComponent<TextMeshPro>().color = Color.white;
//			GameObject.Find ("Skill"+i+"Description").GetComponent<TextMeshPro>().color = Color.white;
//		}
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
	
	public void loadClickedPC(){
//		int currentPlayingCard = GameController.instance.getCurrentPlayingCard();
//		Card c = this.playingCards[currentPlayingCard].GetComponent<PlayingCardController>().getCard();
//		
//		this.skillButtons[0].SetActive(true);
//		this.skillButtons[0].GetComponent<SkillButtonController>().setSkill(c.GetAttackSkill(), this.skillSprites[this.skillSprites.Length-4]);
//		this.skillButtons[4].SetActive(true);
//		this.skillButtons[4].GetComponent<SkillButtonController>().setSkill(new Skill("Fin du tour","Termine son tour et passe la main au personnage suivant",1), this.skillSprites[this.skillSprites.Length-3]);
//		
//		int count = c.Skills.Count ;
//		
//		for (int i = 1 ; i < 4 ; i++){
//			this.skillButtons[i].SetActive(true);
//			if (i<count){
//				this.skillButtons[i].GetComponent<SkillButtonController>().setSkill(c.Skills[i], this.skillSprites[c.Skills[i].Id]);
//				GameObject.Find ("Description"+i).GetComponent<TextMeshPro>().text = c.Skills[i].Name;
//			}
//			else{
//				if(i==1){
//					this.skillButtons[i].GetComponent<SkillButtonController>().setSkill(new Skill("Non disponible","Niveau 4 requis pour débloquer cette compétence",-99), this.skillSprites[this.skillSprites.Length-2]);
//					GameObject.Find ("Description"+i).GetComponent<TextMeshPro>().text = "?";
//				}
//				else{
//					this.skillButtons[i].GetComponent<SkillButtonController>().setSkill(new Skill("Non disponible","Niveau 8 requis pour débloquer cette compétence",-99), this.skillSprites[this.skillSprites.Length-1]);
//					GameObject.Find ("Description"+i).GetComponent<TextMeshPro>().text = "?";
//				}
//			}
//		}
	}
	
	public void loadSkill(){
//		int currentPlayingCard = GameController.instance.getCurrentPlayingCard();
//		GameObject.Find("TitleSD").GetComponent<TextMeshPro>().text = this.currentHoveredSkill.Name;
//		GameObject.Find("DescriptionSD").GetComponent<TextMeshPro>().text = this.currentHoveredSkill.Description;
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
	
	public void displayPopUp(string s, Vector3 position, string t){
//		this.popUpText.GetComponent<TextMeshPro>().text = s ;
//		this.popUpTitle.GetComponent<TextMeshPro>().text = t ;
//		
//		this.popUp.transform.position = position;
	}
	
	public void hidePopUp(){
		this.popUp.transform.position = new Vector3(0, -10, 0);
	}
	
	
	public void displaySkill(){
		
//		if (!this.isDisplayedSkill){
//			this.loadSkill();
//			statusSkill = 1 ;
//			this.timerSkill= 0 ;
//		}
//		else{
//			statusSkill = -2 ;
//			this.timerSkill = 0 ;
//		}
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
	
	public void showTR(int i)
	{
		this.playingCards [i].GetComponent<PlayingCardController>().showTR(true);
	}
	
	public void movePlayingCard(int x, int y, int c)
	{
//		Tile t = this.playingCards [c].GetComponentInChildren<PlayingCardController>().getTile();
//		this.tiles[t.x, t.y].GetComponentInChildren<TileController>().setCharacterID(-1);
//		this.tileHandlers[t.x, t.y].GetComponentInChildren<TileHandlerController>().setCharacterID(-1);
//		this.playingCards [c].GetComponentInChildren<PlayingCardController>().setTile(new Tile(x,y), this.tiles[x,y].GetComponentInChildren<TileController>().getPosition());
//		this.tiles[x, y].GetComponentInChildren<TileController>().setCharacterID(c);
//		this.tileHandlers[x, y].GetComponentInChildren<TileHandlerController>().setCharacterID(c);
//		
//		if(this.getIsMine(c)){
//			this.tiles[x, y].GetComponentInChildren<TileController>().checkTrap(c);
//		}
//		
//		if (GameController.instance.hasGameStarted() && this.getIsMine(GameController.instance.getCurrentPlayingCard())){
//			this.checkSkillsLaunchability();
//		}
//		this.moveCard(c, true);
//		this.recalculateDestinations();
	}
	
	public void hideTrap(int x, int y){
		this.tiles [x, y].GetComponent<TileController>().removeTrap();
	}
	
	public void checkSkillsLaunchability(){
//		List<Skill> skills = this.getCard(GameController.instance.getCurrentPlayingCard()).getSkills();
//		this.skillButtons[0].GetComponentInChildren<SkillButtonController>().checkLaunchability();
//		for (int i = 0 ; i < skills.Count ; i++){
//			this.skillButtons[1+i].GetComponentInChildren<SkillButtonController>().checkLaunchability();
//		}
	}
	
	public void resize()
	{
		this.widthScreen = Screen.width ;
		this.heightScreen = Screen.height ;
		
		if (this.isBackgroundLoaded){
			this.resizeBackground();
		}
	}
	
	public void updateMyLifeBar(){
//		GameObject llbr = GameObject.Find("LLBRight");
//		llbr.transform.position = new Vector3(-1.2f, 4.5f, 0);
//		
//		GameObject leb = GameObject.Find("LLBRightEnd");
//		leb.transform.position = new Vector3(-1.2f, 4.5f, 0);
//		
//		GameObject llbl = GameObject.Find("LLBLeft");
//		llbl.transform.position = new Vector3(-this.realwidth/2f+0.25f, 4.5f, 0);
//		
//		GameObject lcb = GameObject.Find("LLBLeftEnd");
//		lcb.transform.position = new Vector3(llbr.transform.position.x-0.5f+(this.getPercentageLifeMyPlayer())*(-llbr.transform.position.x+0.5f+(llbl.transform.position.x+0.1f))/100, 4.5f, 0);
//		
//		GameObject llbb = GameObject.Find("LLBBar");
//		llbb.transform.position = new Vector3((leb.transform.position.x+lcb.transform.position.x)/2f, 4.5f, 0);
//		llbb.transform.localScale = new Vector3((leb.transform.position.x-lcb.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
	}
	
	public void updateHisLifeBar(){
//		GameObject rlbl = GameObject.Find("RLBLeft");
//		rlbl.transform.position = new Vector3(1.2f, 4.5f, 0);
//		
//		GameObject rlbr = GameObject.Find("RLBRight");
//		rlbr.transform.position = new Vector3(this.realwidth/2f-0.25f, 4.5f, 0);
//		
//		GameObject rlbc = GameObject.Find("RLBCenter");
//		rlbc.transform.position = new Vector3((rlbl.transform.position.x+rlbr.transform.position.x)/2f, 4.5f, 0);
//		rlbc.transform.localScale = new Vector3((-rlbl.transform.position.x+rlbr.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
//		
//		GameObject reb = GameObject.Find("RLBRightEnd");
//		reb.transform.position = new Vector3(rlbl.transform.position.x+0.5f+(this.getPercentageLifeHisPlayer())*(-rlbl.transform.position.x-0.5f+(rlbr.transform.position.x-0.1f))/100, 4.5f, 0);
//		
//		GameObject rcb = GameObject.Find("RLBLeftEnd");
//		rcb.transform.position = new Vector3(1.20f, 4.5f, 0);
//		
//		GameObject rlbb = GameObject.Find("RLBBar");
//		rlbb.transform.position = new Vector3((reb.transform.position.x+rcb.transform.position.x)/2f, 4.5f, 0);
//		rlbb.transform.localScale = new Vector3((reb.transform.position.x-rcb.transform.position.x-0.49f)/10f, 0.5f, 0.5f);
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
	}
	
	public void hoverTargetTile(int c, Tile t){
//		if (this.isTargeting && this.currentTargetingTileHandler!=-1 && this.currentTargetingTileHandler!=c){	
//			Tile tile = this.getPlayingCardTile(currentTargetingTileHandler);
//			this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
//			this.tileHandlers[tile.x, tile.y].GetComponentInChildren<TextMeshPro>().text = "";
//			this.currentTargetingTileHandler = -1 ;
//		}
	}
	
	public void hoverTile(int c, Tile t){
		
//		int currentPlayingCard = GameController.instance.getCurrentPlayingCard();
//		
//		if(c!=-1){
//			if(GameController.instance.getCurrentPlayingCard()!=-1){		
//				if (GameController.instance.getCurrentPlayingCard()!=c){
//					this.clearDestinations();
//					this.isDisplayedItsDestinations = true ;
//					this.displayDestinations(c);	
//				}
//				else{
//					if(this.isDisplayedItsDestinations){
//						this.clearDestinations();
//					}
//						
//					this.displayDestinations(GameController.instance.getCurrentPlayingCard());
//					if(!hasMoved(currentPlayingCard)){
//						this.isDisplayedItsDestinations=false;
//					}
//					else{
//						this.isDisplayedItsDestinations=true;
//					}
//				}
//			}
//			if (this.getIsMine(c)){
//				if(this.getMyHoveredCardController().getIsDisplayed()){
//					if(c!=this.getMyHoveredCardController().getCurrentCharacter()){
//						if(this.getMyHoveredCardController().getStatus()==-1){
//							
//						}
//						else{
//							this.getMyHoveredCardController().hide();
//						}
//						this.getMyHoveredCardController().setNextDisplayedCharacter(c) ;
//					}
//					else{
//						if(this.getMyHoveredCardController().getStatus()==-1){
//							this.getMyHoveredCardController().reverse(1);
//							this.getMyHoveredCardController().setNextDisplayedCharacter(-1) ;
//						}
//					}
//				}
//				else{
//					if(this.getMyHoveredCardController().getStatus()==1){
//						if(c!=this.getMyHoveredCardController().getCurrentCharacter()){
//							this.getMyHoveredCardController().reverse(-1);
//							this.getMyHoveredCardController().setNextDisplayedCharacter(c) ;
//						}
//					}
//					else{
//						this.getMyHoveredCardController().setNextDisplayedCharacter(c) ;
//						this.getMyHoveredCardController().launchNextMove();
//					}
//				}
//			}
//			else{
//				if(this.getHisHoveredCardController().getIsDisplayed()){
//					if(c!=this.getHisHoveredCardController().getCurrentCharacter()){
//						if(this.getHisHoveredCardController().getStatus()==-1){
//							
//						}
//						else{
//							this.getHisHoveredCardController().hide();
//						}
//						this.getHisHoveredCardController().setNextDisplayedCharacter(c) ;
//					}
//					else{
//						if(this.getHisHoveredCardController().getStatus()==-1){
//							this.getHisHoveredCardController().reverse(1);
//							this.getHisHoveredCardController().setNextDisplayedCharacter(-1);
//						}
//					}
//				}
//				else{
//					if(this.getHisHoveredCardController().getStatus()==1){
//						if(c!=this.getHisHoveredCardController().getCurrentCharacter()){
//							this.getHisHoveredCardController().reverse(-1);
//							this.getHisHoveredCardController().setNextDisplayedCharacter(c) ;
//						}
//					}
//					else{
//						this.getHisHoveredCardController().setNextDisplayedCharacter(c) ;
//						this.getHisHoveredCardController().launchNextMove();
//					}
//				}
//			}
//		}
//		else{
//			if(GameController.instance.getCurrentPlayingCard()!=-1){
//				if(this.isDisplayedItsDestinations){
//					this.clearDestinations();
//					
//					if(!hasMoved(GameController.instance.getCurrentPlayingCard())){
//						this.displayDestinations(GameController.instance.getCurrentPlayingCard());	
//					}
//					this.isDisplayedItsDestinations=false;
//				}
//			}
//			if(currentPlayingCard!=-1){
//				if(this.getIsMine(currentPlayingCard)){
//					if(this.getMyHoveredCardController().getIsDisplayed()){
//						if(this.getMyHoveredCardController().getStatus()==-1){
//							this.getMyHoveredCardController().reverse(1);
//							this.getMyHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
//					 	}
//						else if (this.getMyHoveredCardController().getCurrentCharacter()!=currentPlayingCard){
//							if(this.getMyHoveredCardController().getStatus()==1){
//								this.getMyHoveredCardController().reverse(-1);
//								this.getMyHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
//							}
//							else{
//								this.getMyHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
//								this.getMyHoveredCardController().launchNextMove();
//							}
//					 	}
//					}
//					else{
//						if(this.getMyHoveredCardController().getStatus()==1){
//							if(this.getMyHoveredCardController().getCurrentCharacter()!=currentPlayingCard){
//								this.getMyHoveredCardController().reverse(-1);
//								this.getMyHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
//							}
//						}
//						else{
//							this.getMyHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
//							this.getMyHoveredCardController().launchNextMove();
//						}
//					}
//					if(!this.getHisHoveredCardController().getIsDisplayed()){
//						if(this.getHisHoveredCardController().getStatus()==1){
//							this.getHisHoveredCardController().reverse(-1);
//							this.getHisHoveredCardController().setNextDisplayedCharacter(-1);
//						}
//					}
//					else{
//						this.getHisHoveredCardController().setNextDisplayedCharacter(-1);
//						this.getHisHoveredCardController().hide();
//					}
//				}
//				else{
//					if(this.getHisHoveredCardController().getIsDisplayed()){
//						if (this.getHisHoveredCardController().getStatus()==1){
//							this.getHisHoveredCardController().reverse(1);
//							this.getHisHoveredCardController().setNextDisplayedCharacter(-1);
//						}
//						else{
//							if(this.getHisHoveredCardController().getStatus()==-1){
//								this.getHisHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
//							}
//							else if(currentPlayingCard!=this.getHisHoveredCardController().getCurrentCharacter()){
//								this.getHisHoveredCardController().hide();
//								this.getHisHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
//							}
//						}
//					}
//					else{
//						if(this.getHisHoveredCardController().getStatus()==1){
//							if(this.getHisHoveredCardController().getCurrentCharacter()==currentPlayingCard){
//							
//							}
//							else{
//								this.getHisHoveredCardController().reverse(-1);
//								this.getHisHoveredCardController().setNextDisplayedCharacter(currentPlayingCard) ;
//							}
//						}
//						else{
//							this.getHisHoveredCardController().setNextDisplayedCharacter(currentPlayingCard);
//							this.getHisHoveredCardController().launchNextMove();
//						}
//					}
//				}
//			}
//			else{
//				int clickedCard = GameController.instance.getClickedCard();
//				if (clickedCard!=-1){
//					if (this.getMyHoveredCardController().getIsDisplayed()){
//						if(this.getMyHoveredCardController().getCurrentCharacter()!=clickedCard){
//							if(this.getMyHoveredCardController().getStatus()==0){
//								this.getMyHoveredCardController().hide();
//							}
//							this.getMyHoveredCardController().setNextDisplayedCharacter(clickedCard);
//						}
//					}
//					else{
//						if(this.getMyHoveredCardController().getStatus()==1){
//							this.getMyHoveredCardController().reverse(-1);
//							this.getMyHoveredCardController().setNextDisplayedCharacter(clickedCard);
//						}
//						else{
//							this.getMyHoveredCardController().setNextDisplayedCharacter(clickedCard);
//							this.getMyHoveredCardController().launchNextMove();
//						}
//					}
//				}
//				else{
//					if (this.getMyHoveredCardController().getIsDisplayed()){
//						if(this.getMyHoveredCardController().getStatus()==0){
//							this.getMyHoveredCardController().hide();
//						}
//						this.getMyHoveredCardController().setNextDisplayedCharacter(-1);
//					}
//					else{
//						if(this.getMyHoveredCardController().getStatus()==1){
//							this.getMyHoveredCardController().reverse(-1);
//							this.getMyHoveredCardController().setNextDisplayedCharacter(-1);
//						}
//					}
//				}
//			}
//		}
//		
//		GameObject tempGO = GameObject.Find("Hover");
//		Vector3 pos = tiles[t.x, t.y].GetComponent<TileController>().getPosition();
//		pos.z = -1 ;
//		tempGO.transform.position = pos ;
	}
	
	public void changePlayingCard(int c){
		Tile t = this.getPlayingCardTile(c);
		if(this.getIsMine(c)){
			this.loadClickedPC();
		}
		this.playingCards[c].GetComponent<PlayingCardController>().moveForward();
		this.hoverTile(c, t);
	}
	
	public void changeClickedCard(int c){
//		Tile t = this.getPlayingCardTile(c);
//		this.tileHandlers[t.x,t.y].GetComponent<TileHandlerController>().setCharacterID(c);
//		this.tileHandlers[t.x,t.y].GetComponent<TileHandlerController>().changeType(7);
//		this.tileHandlers[t.x,t.y].GetComponent<TileHandlerController>().enable();
	}
	
	public void loadPlayInformation(){
//		this.currentHoveredSkill = new Skill("C'est votre tour !", "Choisissez l'action à effectuer par votre héros ", 1) ;
//		GameObject.Find("AdditionnalInfo").GetComponent<TextMeshPro>().text = "";
//		this.loadSkill();
//		this.statusSkill = 1 ;
	}
	
	public void hoverTileHandler(int c, Tile t){
		
//		this.hoverTile(c,t);
//		
//		this.tileHandlers[t.x,t.y].GetComponent<TileHandlerController>().changeType(2);
//		if(c==-1){
//			this.tileHandlers[t.x,t.y].GetComponentInChildren<TextMeshPro>().text = GameSkills.instance.getCurrentGameSkill().getTargetText(-1, new Card());
//		}
//		else{
//			this.tileHandlers[t.x,t.y].GetComponentInChildren<TextMeshPro>().text = GameSkills.instance.getCurrentGameSkill().getTargetText(c, this.getCard(c));
//		}
//		
//		this.currentTargetingTileHandler = c ;
	}
	
	public void hitExternalCollider(){
//		GameObject tempGO = GameObject.Find("Hover");
//		tempGO.transform.position = new Vector3(-10, -10, 1) ;
//		
//		if(GameController.instance.getCurrentPlayingCard()!=-1){
//			if(this.isDisplayedItsDestinations){
//				this.clearDestinations();
//				if(!this.hasMoved(GameController.instance.getCurrentPlayingCard())){
//					this.displayDestinations(GameController.instance.getCurrentPlayingCard());
//					this.isDisplayedItsDestinations=false;
//				}
//			}
//		}
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
	
	public bool hasPlayed(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getHasPlayed();
	}
	
	public bool hasMoved(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getHasMoved();
	}
	
	public bool getIsMine(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getIsMine();
	}
	
	public void playCard(int i, bool b){
		this.playingCards[i].GetComponent<PlayingCardController>().play(b);
	}
	
	public void checkModifyers(int i){
		this.playingCards[i].GetComponent<PlayingCardController>().checkModyfiers();
	}
	
	public void moveCard(int i, bool b){
		this.playingCards[i].GetComponent<PlayingCardController>().move(b);
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
		int move = this.getCard(i).GetMove();
		
		int j = 0 ;
		
		if(this.getIsMine(i)){		
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
			this.calculateDestinations(i);
		}
	}
	
	public bool isDead(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>().getIsDead();
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
			if (this.getIsMine(i)){
				tiles.Add(this.getPlayingCardTile(i));
			}
		}
		return tiles;
	}
	
	public void clearDestinations(){
//		for (int i = 0 ; i < GameView.instance.boardWidth ; i++){
//			for (int j = 0 ; j < GameView.instance.boardHeight ; j++){
//				if(this.tileHandlers[i,j].activeSelf){
//					if(this.tileHandlers[i,j].GetComponent<TileHandlerController>().getTypeNumber()!=6){
//						this.tileHandlers[i, j].GetComponent<TileHandlerController>().disable();
//					}
//				}
//			}
//		}
	}
	
	public void displayDestinations(int c)
	{
//		int i = -1;
//		if(GameController.instance.getCurrentPlayingCard()==c && !this.hasMoved(GameController.instance.getCurrentPlayingCard())){
//			if(this.getIsMine(c)){
//				i = 1 ;
//			}
//			else{
//				i = 9;
//			}
//		}
//		else{
//			i = 10 ;
//		}
//		
//		List<Tile> destinations = this.playingCards[c].GetComponent<PlayingCardController>().getDestinations();
//		foreach (Tile t in destinations)
//		{
//			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().changeType(i);
//			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().enable();
//		}
	}
	
	public void displayAdjacentOpponentsTargets()
	{
//		Tile tile ;
//		this.targets = new List<Tile>();
//		List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
//		int playerID;
//		foreach (Tile t in neighbourTiles)
//		{
//			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
//				{
//					tile = this.getPlayingCardTile(playerID);
//					this.targets.Add(tile);
//					
//					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
//					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
//					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
//					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().enable();
//				}
//			}
//		}
//		this.timerTargeting = 0 ;
//		this.currentTargetingTileHandler = -1;
//		this.isTargeting = true ;
	}
	
	public void display1TileAwayOpponentsTargets()
	{
//		int playerID;
//		Tile tile = this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()) ;
//		if(tile.x>1){
//			playerID = this.tiles [tile.x-2, tile.y].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if(!this.getIsMine(this.getTileCharacterID(tile.x-2, tile.y))){
//					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
//					{
//						tile = this.getPlayingCardTile(playerID);
//						this.targets.Add(tile);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().enable();
//					}
//				}
//			}
//		}
//		if(tile.x<this.boardWidth-2){
//			playerID = this.tiles [tile.x+2, tile.y].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if(!this.getIsMine(this.getTileCharacterID(tile.x+2, tile.y))){
//					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
//					{
//						tile = this.getPlayingCardTile(playerID);
//						this.targets.Add(tile);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().enable();
//					}
//				}
//			}
//		}
//		if(tile.y>1){
//			playerID = this.tiles [tile.x, tile.y-2].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if(!this.getIsMine(this.getTileCharacterID(tile.x, tile.y-2))){
//					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
//					{
//						tile = this.getPlayingCardTile(playerID);
//						this.targets.Add(tile);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().enable();
//					}
//				}
//			}
//		}
//		if(tile.y<this.boardHeight-2){
//			playerID = this.tiles [tile.x, tile.y+2].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if(!this.getIsMine(this.getTileCharacterID(tile.x, tile.y+2))){
//					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
//					{
//						tile = this.getPlayingCardTile(playerID);
//						this.targets.Add(tile);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
//						this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().enable();
//					}
//				}
//			}
//		}
//		
//		this.timerTargeting = 0 ;
//		this.currentTargetingTileHandler = -1;
//		this.isTargeting = true ;
	}
	
	public string canLaunch1TileAwayOpponents()
	{
		string isLaunchable = "Aucun ennemi à portée de lance";
//		
//		int playerID;
//		Tile tile = this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()) ;
//		
//		if(tile.x>1){
//			playerID = this.tiles [tile.x-2, tile.y].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if(!this.getIsMine(this.getTileCharacterID(tile.x-2, tile.y))){
//					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
//					{
//						isLaunchable="";
//					}
//				}
//			}
//		}
//		if(tile.x<this.boardWidth-2){
//			playerID = this.tiles [tile.x+2, tile.y].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if(!this.getIsMine(this.getTileCharacterID(tile.x+2, tile.y))){
//					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
//					{
//						isLaunchable="";
//					}
//				}
//			}
//		}
//		if(tile.y>1){
//			playerID = this.tiles [tile.x, tile.y-2].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if(!this.getIsMine(this.getTileCharacterID(tile.x, tile.y-2))){
//					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
//					{
//						isLaunchable="";
//					}
//				}
//			}
//		}
//		if(tile.y<this.boardHeight-2){
//			playerID = this.tiles [tile.x, tile.y+2].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if(!this.getIsMine(this.getTileCharacterID(tile.x, tile.y+2))){
//					if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
//					{
//						isLaunchable="";
//					}
//				}
//			}
//		}
//		
		return isLaunchable;
	}
	
	public void displayAdjacentAllyTargets()
	{
//		Tile tile ;
//		this.targets = new List<Tile>();
//		List<Tile> neighbourTiles = this.getAllyImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
//		int playerID;
//		foreach (Tile t in neighbourTiles)
//		{
//			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getIsMine(playerID))
//				{
//					tile = this.getPlayingCardTile(playerID);
//					this.targets.Add(tile);
//					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
//					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
//					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(playerID);
//					this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().enable();
//				}
//			}
//		}
//		this.timerTargeting = 0 ;
//		this.currentTargetingTileHandler = -1;
//		this.isTargeting = true ;
	}
	
	public void displayAdjacentTileTargets()
	{
//		List<Tile> neighbourTiles = this.getFreeImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
//		this.targets = new List<Tile>();
//		
//		foreach (Tile t in neighbourTiles){
//			this.targets.Add(t);
//			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().changeType(6);
//			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().setText("");
//			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().setCharacterID(-1);
//			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().enable();
//		}
//		this.isTargeting = true ;
	}
	
	public void displayAllButMeModifiersTargets()
	{
//		PlayingCardController pcc;
//		this.targets = new List<Tile>();
//		Tile tile ;
//		
//		for (int i = 0; i < this.playingCards.Count; i++)
//		{
//			pcc = this.getPCC(i);
//			if (pcc.getCard().hasModifiers() && pcc.canBeTargeted() && i != GameController.instance.getCurrentPlayingCard())
//			{
//				tile = this.getPlayingCardTile(i);
//				this.targets.Add(tile);
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(i);
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().enable();
//			}
//		}		
	}
	
	public void displayOpponentsTargets()
	{
//		PlayingCardController pcc;
//		Tile tile ;
//		this.targets = new List<Tile>();
//		
//		for (int i = 0; i < this.playingCards.Count; i++)
//		{
//			pcc = this.getPCC(i);
//			if (!this.getIsMine(i) && pcc.canBeTargeted())
//			{
//				tile = this.getPlayingCardTile(i);
//				this.targets.Add(tile);
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(i);
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().enable();
//			}
//		}
//		this.timerTargeting = 0 ;
//		this.currentTargetingTileHandler = -1;
//		this.isTargeting = true ;	
	}
	
	public void displayAllysButMeTargets()
	{
//		PlayingCardController pcc;
//		Tile tile ;
//		this.targets = new List<Tile>();
//		
//		for (int i = 0; i < this.playingCards.Count; i++)
//		{
//			pcc = this.getPCC(i);
//			if (this.getIsMine(i) && pcc.canBeTargeted() &&  i != GameController.instance.getCurrentPlayingCard())
//			{
//				tile = this.getPlayingCardTile(i);
//				this.targets.Add(tile);
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().changeType(6);
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setText("");
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().setCharacterID(i);
//				this.tileHandlers[tile.x, tile.y].GetComponent<TileHandlerController>().enable();
//			}
//		}
//		this.timerTargeting = 0 ;
//		this.currentTargetingTileHandler = -1;
//		this.isTargeting = true ;	
	}
	
	public PlayingCardController getPCC(int i){
		return this.playingCards[i].GetComponent<PlayingCardController>();
	}
	
	public void hideTargets(){
//		this.isTargeting = false ;
//		this.currentTargetingTileHandler = -1;
//		for (int i = 0 ; i < this.targets.Count ; i++){
//			this.tileHandlers[this.targets[i].x, this.targets[i].y].GetComponent<TileHandlerController>().disable();
//		}
	}
	
	public string canLaunchAdjacentOpponents()
	{
		string isLaunchable = "Aucun ennemi à proximité";
		
//		List<Tile> neighbourTiles = this.getOpponentImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
//		int playerID;
//		foreach (Tile t in neighbourTiles)
//		{
//			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && !this.getIsMine(playerID))
//				{
//					isLaunchable = "";
//				}
//			}
//		}
		return isLaunchable;
	}
	
	public string canLaunchAdjacentAllys()
	{
		string isLaunchable = "Aucun allié à proximité";
		
//		List<Tile> neighbourTiles = this.getAllyImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
//		int playerID;
//		foreach (Tile t in neighbourTiles)
//		{
//			playerID = this.tiles [t.x, t.y].GetComponent<TileController>().getCharacterID();
//			if (playerID != -1)
//			{
//				if (this.playingCards [playerID].GetComponent<PlayingCardController>().canBeTargeted() && this.getIsMine(playerID))
//				{
//					isLaunchable = "";
//				}
//			}
//		}
		return isLaunchable;
	}
	
	public string canLaunchAdjacentTileTargets()
	{
		string isLaunchable = "Aucun terrain ne peut etre ciblé";
		
//		List<Tile> neighbourTiles = this.getFreeImmediateNeighbours(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
//		this.targets = new List<Tile>();
//		
//		if(neighbourTiles.Count>0){
//			isLaunchable = "";
//		}
			
		return isLaunchable;
	}
	
	public string canLaunchAllButMeModifiersTargets()
	{
		string isLaunchable = "Aucun personnage ne peut etre ciblé";
		
//		PlayingCardController pcc;
//		this.targets = new List<Tile>();
//		
//		for (int i = 0; i < this.playingCards.Count; i++)
//		{
//			pcc = this.getPCC(i);
//			if (pcc.getCard().hasModifiers() && pcc.canBeTargeted() && i != GameController.instance.getCurrentPlayingCard())
//			{
//				isLaunchable = "";
//			}
//		}		
		return isLaunchable ;
	}
	
	public string canLaunchOpponentsTargets()
	{
		string isLaunchable = "Aucun ennemi ne peut etre atteint";
		
		PlayingCardController pcc;
		
		for (int i = 0; i < this.playingCards.Count; i++)
		{
			pcc = this.getPCC(i);
			if (!this.getIsMine(i) && pcc.canBeTargeted())
			{
				isLaunchable = "";
			}
		}
		return isLaunchable;
	}
	
	public string canLaunchAllysButMeTargets()
	{
		string isLaunchable = "Aucun ennemi ne peut etre atteint";
		
//		PlayingCardController pcc;
//		
//		for (int i = 0; i < this.playingCards.Count; i++)
//		{
//			pcc = this.getPCC(i);
//			if (this.getIsMine(i) && pcc.canBeTargeted() && i != GameController.instance.getCurrentPlayingCard())
//			{
//				isLaunchable = "";
//			}
//		}
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
	
//	public void setModifier(Tile tile, int amount, ModifierType type, ModifierStat stat, int duration, int idIcon, string t, string d, string a, bool b){
//		this.tiles [tile.x, tile.y].GetComponent<TileController>().getTile().setModifier(amount, type, stat, duration, idIcon, t, d, a, b);
//		this.tiles [tile.x, tile.y].GetComponent<TileController>().changeTrapId(idIcon);
//		this.tiles [tile.x, tile.y].GetComponent<TileController>().show();
//	}
	
	public void show(int target, bool showTR){
		this.playingCards[target].GetComponent<PlayingCardController>().show(showTR);
	}
	
	public void kill(int target){
		
//		this.playingCards[target].GetComponent<PlayingCardController>().kill();
//		GameController.instance.killHandle (target);
//		
//		Tile t = this.getPlayingCardTile(target);
//		this.emptyTile(t.x, t.y);
//		
//		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().setText("");
//		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().changeType(3);
//		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().enable();
//		this.displayedDeads.Add(target);
//		this.displayedDeadsTimer.Add(1);
//		
//		if(this.getCard(target).isLeader()){
//			bool isMine = this.getIsMine(target);
//			for (int i = 0 ; i < this.playingCards.Count ; i++){
//				if(i!=target && this.getIsMine(i)==isMine){
//					this.getCard(i).removeLeaderEffect();
//					this.show(i,true);
//				}
//			}
//			
//		}	
//		this.toDisplayDeadHalos = true ;
	}
	
	public void emptyTile(int x, int y)
	{
		this.tiles [x, y].GetComponent<TileController>().setCharacterID(-1);
	}
	
	public void disappear(int target){
		this.playingCards[target].GetComponent<PlayingCardController>().disappear();		
	}
	
	public void displaySkillEffect(int target, string text, int type){
//		Tile t = this.getPlayingCardTile(target);
//		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().enable();
//		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().changeType(type);
//		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().setText(text);
//		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().moveForward();
//		this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().enable();
//		
//		this.displayedSE.Add(target);
//		this.displayedSETimer.Add(2);
//		
//		this.toDisplaySE = true ;
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
				if(this.getIsMine(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID())){
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
				if(!this.getIsMine(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID())){
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
				if(!this.getIsMine(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID())){
					freeNeighbours.Add(neighbours[i]);
				}
			}
		}
		return freeNeighbours ;
	}
	
	public List<Tile> getCharacterImmediateNeighbours(Tile t){
		List<Tile> freeNeighbours = new List<Tile>();
//		List<Tile> neighbours = t.getImmediateNeighbourTiles();
//		for (int i = 0 ; i < neighbours.Count ; i++){
//			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1 && this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=GameController.instance.getCurrentPlayingCard()){
//				freeNeighbours.Add(neighbours[i]);
//			}
//		}
		return freeNeighbours ;
	}
	
	public List<Tile> getAllyImmediateNeighbours(Tile t){
		List<Tile> freeNeighbours = new List<Tile>();
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID()!=-1){
				if(this.getIsMine(this.tiles[neighbours[i].x, neighbours[i].y].GetComponent<TileController>().getCharacterID())){
					freeNeighbours.Add(neighbours[i]);
				}
			}
		}
		return freeNeighbours ;
	}
	
	public void removeClickedCard(int c){
//		if(c!=-1){
//			Tile t = this.getPlayingCardTile(c);
//			this.tileHandlers[t.x, t.y].GetComponent<TileHandlerController>().disable();
//		}
	}
	
	public List<int> getAllys(){
		List<int> allys = new List<int>();
//		int CPC = GameController.instance.getCurrentPlayingCard();
//		for(int i = 0 ; i < this.playingCards.Count;i++){
//			if(i!=CPC && !GameView.instance.isDead(i) && GameView.instance.getIsMine(i)){
//				allys.Add(i);
//			}
//		}
		return allys;
	}
	
	public List<int> getOpponents(){
		List<int> allys = new List<int>();
		for(int i = 0 ; i < this.playingCards.Count;i++){
			if(!GameView.instance.isDead(i) && !GameView.instance.getIsMine(i)){
				allys.Add(i);
			}
		}
		return allys;
	}
	
	public List<int> getEveryone(){
		List<int> everyone = new List<int>();
		for(int i = 0 ; i < this.playingCards.Count;i++){
			if(!GameView.instance.isDead(i)){
				everyone.Add(i);
			}
		}
		return everyone;
	}
	
	public List<int> getEveryoneButMe(){
		List<int> everyone = new List<int>();
//		for(int i = 0 ; i < this.playingCards.Count;i++){
//			if(!GameView.instance.isDead(i) && GameController.instance.getCurrentPlayingCard()!=i){
//				everyone.Add(i);
//			}
//		}
		return everyone;
	}
	
	public int countAlive(){
		int compteur = 0 ;
		for (int i = 0 ; i < this.playingCards.Count ; i++){
			if (!this.isDead(i)){
				compteur++;
			}
		}
		return compteur ;
	}
	
	public void checkPassiveSkills(bool mine){
//		bool isFoundLeader = false ;
//		for (int i = 0 ; i < this.playingCards.Count ; i++){
//			if (this.getIsMine(i)==mine){
//				if(this.getCard(i).isLeader() && !isFoundLeader){
//					isFoundLeader = true ;
//					int amount = this.getCard(i).getPassiveManacost();
//					for (int j = 0 ; j < this.playingCards.Count ; j++){
//						if(i!=j){
//							if (this.getIsMine(j)==mine){
//								int amountLife = Mathf.CeilToInt(amount*this.getCard(j).GetTotalLife()/100f);
//								int amountAttack = Mathf.CeilToInt(amount*this.getCard(j).GetAttack()/100f);
//								this.getCard(j).addModifier(amountLife, ModifierType.Type_BonusMalus, ModifierStat.Stat_Life, -1, 17, "LEADER ACTIF", "+"+amountLife+"PV. Tant que le leader est en vie.", "");
//								this.getCard(j).addModifier(amountAttack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 28, "LEADER ACTIF", "+"+amountAttack+"ATK. Tant que le leader est en vie.", "");
//								this.show (j, false);
//							}
//						}
//					}
//				}
//			}
//		}
	}
	
	public bool getIsTutorialLaunched()
	{
		return isTutorialLaunched;
	}
	public Vector3 getTilesPosition(int x, int y)
	{
		return this.tiles [x, y].transform.position;
	} 
	public Vector3 getPlayingCardsPosition(int index)
	{
		return this.playingCards [index].transform.position;
	}
	public Vector3 getPlayingCardsAttackZonePosition(int index)
	{
		return this.playingCards [index].transform.FindChild("AttackZone").position;
	}
	public Vector3 getPlayingCardsQuicknessZonePosition(int index)
	{
		return this.playingCards [index].transform.FindChild("WaitTime").position;
	}
	public Vector3 getPlayingCardsLifeZonePosition(int index)
	{
		return this.playingCards [index].transform.FindChild("LifeBar").FindChild("PVValue").position;
	}
	public Vector3 getMyHoveredRPCPosition()
	{
		return this.myHoveredRPC.transform.position;
	}
	public Vector3 getHisHoveredRPCPosition()
	{
		return this.hisHoveredRPC.transform.position;
	}
	public Vector3 getStartButtonPosition()
	{
		return GameObject.Find ("StartButton").transform.position;
	}
	public Vector3 getTimerGoPosition()
	{
		return this.timerGO.transform.position;
	}
	public Vector3 getSkillButtonPosition(int id)
	{
		return GameObject.Find ("ActionButtons").gameObject.transform.FindChild ("SkillButton"+id).transform.position;
	}
	public Vector3 getAttackButtonPosition()
	{
		return GameObject.Find ("ActionButtons").gameObject.transform.FindChild ("AttackButton").transform.position;
	}
	public Vector3 getPassButtonPosition()
	{
		return GameObject.Find ("ActionButtons").gameObject.transform.FindChild ("PassButton").transform.position;
	}
	
	public bool areAllMyPlayersDead(){
		bool areMyPlayersDead = true ;
		for (int i = 0 ; i < this.playingCards.Count ; i++){
			if (this.getIsMine(i)){
				if (!this.isDead(i)){
					areMyPlayersDead = false ;
				}
				else{
					print ("Dead "+i);
				}
			}
		}
		return areMyPlayersDead ;
	}
	
	public int attackClosestEnnemy(){
//		bool[,] hasBeenPassages = new bool[this.boardWidth, this.boardHeight];
//		bool hasFoundEnnemy = false ;
		int idEnnemyToAttack = -1 ;
//		Tile idPlaceToMoveTo = new Tile(-1,-1) ;
//		for(int l = 0 ; l < this.boardWidth ; l++){
//			for(int k = 0 ; k < this.boardHeight ; k++){
//				hasBeenPassages[l,k]=false;
//			}
//		}
//		
//		int move = this.getCard(GameController.instance.getCurrentPlayingCard()).GetMove();
//		
//		List<Tile> destinations = new List<Tile>();
//		List<Tile> baseTiles = new List<Tile>();
//		List<Tile> tempTiles = new List<Tile>();
//		List<Tile> tempNeighbours = new List<Tile>();
//		baseTiles.Add(this.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()));
//		
//		int j = 0 ;
//		while (!hasFoundEnnemy && j<move){
//			tempTiles = new List<Tile>();
//			
//			for(int k = 0 ; k < baseTiles.Count ; k++){
//				tempNeighbours = this.getCharacterImmediateNeighbours(baseTiles[k]);
//				if(tempNeighbours.Count>0){
//					idEnnemyToAttack = GameView.instance.getTileCharacterID(tempNeighbours[0].x, tempNeighbours[0].y);
//					idPlaceToMoveTo = baseTiles[k];
//					hasFoundEnnemy = true ;
//				}
//				else{
//					tempNeighbours = this.getDestinationImmediateNeighbours(baseTiles[k]);
//					for(int l = 0 ; l < tempNeighbours.Count ; l++){
//						if(!hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]){
//							tempTiles.Add(tempNeighbours[l]);
//							hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]=true;
//						}
//					}
//				}
//			}
//			baseTiles = new List<Tile>();
//			for(int l = 0 ; l < tempTiles.Count ; l++){
//				baseTiles.Add(tempTiles[l]);
//			}
//			j++;
//		}
//		
//		if(hasFoundEnnemy==false){
//			GameController.instance.moveToDestination(baseTiles[0]);
//		}
//		else{
//			GameController.instance.moveToDestination(idPlaceToMoveTo);
//		}
		
		return idEnnemyToAttack;
	}
	
	public float getPercentageLifeMyPlayer(){
		float life = 0; 
		float totalLife = 0;
		for(int i = 0 ; i < this.playingCards.Count ; i++){
			if(this.getIsMine(i)){
				totalLife += this.getCard(i).GetTotalLife();
				if(!this.isDead(i)){
					life += this.getCard(i).GetLife();
				}
			}
		}
		return (100f*(life/totalLife));
	}
	
	public float getPercentageLifeHisPlayer(){
		float life = 0; 
		float totalLife = 0;
		for(int i = 0 ; i < this.playingCards.Count ; i++){
			if(!this.getIsMine(i)){
				totalLife += this.getCard(i).GetTotalLife();
				if(!this.isDead(i)){
					life += this.getCard(i).GetLife();
				}
			}
		}
		return (100f*(life/totalLife));
	}
	
	public void hideLoadingScreen()
	{
		if(isLoadingScreenDisplayed)
		{
			Destroy (this.loadingScreen);
			this.isLoadingScreenDisplayed=false;
		}
	}
	
	public Sprite getSkillTypeSprite(int i){
		return this.skillTypeSprites[i];
	}
	
	public Sprite getCharacterSprite(int i){
		return this.sprites[i];
	}
	
}



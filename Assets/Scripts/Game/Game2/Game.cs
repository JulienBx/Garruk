using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public static Game instance;
	public Board board ;
	public Gamecards gamecards ;

	public GameObject verticalBorderModel;
	public GameObject horizontalBorderModel;
	public GameObject tileModel;
	public GameObject cardModel;

	public Sprite[] skillSprites;
	public Sprite[] bigCharacterSprites;
	public Sprite[] animSprites;

	GameObject myHoveredCard ;
	GameObject hisHoveredCard ;
	GameObject startButton ;
	GameObject interlude ;
	GameObject passButton ;
	GameObject[] skillsButton ;
	GameObject popUpChoice ;
	GameObject timeline ;
	GameObject timer ;
	GameObject frontTimer ;
	GameObject popUpValidation ;
	GameObject forfeitButton ;
	GameObject myPlayerName ;
	GameObject hisPlayerName ;
	GameObject cancelButton ;

	bool ignoreNextMoveOn;

	SkillsM skills ;

	bool mobile ;
	bool firstPlayer ;
	bool ia;

	Intelligence intelligence ;
	int currentCardID ;
	int draggingCardID ;
	int draggingSBID ;
	float tileScale;
	int nbPlayersReadyToStart;

	List<int> cardsToPlay;

	int lastToPlay;
	int indexPlayingCard;
	int indexMeteores;

	string URLStat = ApplicationModel.host + "updateResult.php";
	bool SE ;
	bool toLaunchNextTurn;

	void Awake(){
		instance = this;
		this.currentCardID = -1;
		this.draggingCardID = -1;
		this.draggingSBID = -1;
		this.nbPlayersReadyToStart = 0;
		this.indexPlayingCard = -1;
		this.lastToPlay = -10;
		this.indexMeteores = 1;
		this.ignoreNextMoveOn = false ;
		this.skills = new SkillsM();
		this.firstPlayer = false ;
		this.cardsToPlay = new List<int>();
		this.ia = false ;
		GameObject.Find("PhotonController").GetComponent<PhotonController>().isOk = false ;

		this.board = new Board();
		this.gamecards = new Gamecards();
		SoundController.instance.playMusic(new int[]{4,5,6});

		this.myHoveredCard = GameObject.Find("MyHoveredPlayingCard");
		this.hisHoveredCard = GameObject.Find("HisHoveredPlayingCard");
		this.startButton = GameObject.Find("SB");
		this.interlude = GameObject.Find("Interlude");
		this.passButton = GameObject.Find("PassButton");
		this.skillsButton = new GameObject[4];
		this.skillsButton[0] = GameObject.Find("SkillButton1");
		this.skillsButton[1] = GameObject.Find("SkillButton2");
		this.skillsButton[2] = GameObject.Find("SkillButton3");
		this.skillsButton[3] = GameObject.Find("SkillButton4");
		this.popUpChoice = GameObject.Find("PopUpChoice");
		this.timeline = GameObject.Find("Timeline");
		this.timer = GameObject.Find("Timer");
		this.frontTimer = GameObject.Find("FrontTimer");
		this.popUpValidation = GameObject.Find("PopUpValidation");
		this.forfeitButton = GameObject.Find("ForfeitButton");
		this.myPlayerName = GameObject.Find("MyPlayerName");
		this.hisPlayerName = GameObject.Find("HisPlayerName");
		this.cancelButton = GameObject.Find("CancelButton");

		PhotonC.instance.findRoom();
		GameObject.Find("Logo").GetComponent<SpriteRenderer>().enabled = false;
	}

	void Update(){
		this.SE = false ;
		if(PhotonC.instance.isWaiting){
			PhotonC.instance.addTime(Time.deltaTime);
		}

		if(GameRPC.instance.isFailing()){
			if(!PhotonC.instance.isReconnecting()){
				GameRPC.instance.addTime(Time.deltaTime);
			}
		}

		if(this.getInterlude().isDisplaying()){
			this.getInterlude().addTime(Time.deltaTime);
		}

		if(this.getMyHoveredCard().isClignoting()){
			this.getMyHoveredCard().addTimeClignoting(Time.deltaTime);
		}

		if(this.getMyHoveredCard().isMoving()){
			this.getMyHoveredCard().addTime(Time.deltaTime);
		}

		if(this.getMyHoveredCard().isMoving()){
			this.getMyHoveredCard().addTime(Time.deltaTime);
		}

		if(this.getMyHoveredCard().isMoving()){
			this.getMyHoveredCard().addTime(Time.deltaTime);
		}

		if(this.getHisHoveredCard().isMoving()){
			this.getHisHoveredCard().addTime(Time.deltaTime);
		}

		if(this.getHisHoveredCard().isClignoting()){
			this.getHisHoveredCard().addTimeClignoting(Time.deltaTime);
		}

		if(this.gamecards.isLoaded()){
			for (int i = 0 ; i < 8 ; i++){
				if(this.gamecards.getCardC(i).isClignoting()){
					this.gamecards.getCardC(i).addClignoteTime(Time.deltaTime);
				}
				if(this.gamecards.getCardC(i).isMoving()){
					this.gamecards.getCardC(i).addMoveTime(Time.deltaTime);
				}
				if(this.gamecards.getCardC(i).isMeasuringPush()){
					this.gamecards.getCardC(i).addPushTime(Time.deltaTime);
				}
				if(this.gamecards.getCardC(i).isUpgradingAttack()){
					this.gamecards.getCardC(i).addTimeAttack(Time.deltaTime);
				}
				if(this.gamecards.getCardC(i).isUpgradingLife()){
					this.gamecards.getCardC(i).addTimeLife(Time.deltaTime);
				}
				if(this.gamecards.getCardC(i).isSkillEffect()){
					this.gamecards.getCardC(i).addSETime(Time.deltaTime);
					SE = true ;
				}
			}
		}

		if(this.getBoard().isLoaded()){
			for (int i = 0 ; i < this.getBoard().getBoardWidth() ; i++){
				for (int j = 0 ; j < this.getBoard().getBoardHeight() ; j++){
					if(this.getBoard().getTileC(i,j).isSkillEffect()){
						this.getBoard().getTileC(i,j).addSETime(Time.deltaTime);
						SE = true ;
					}
					if(this.getBoard().getTileC(i,j).isTarget()){
						this.getBoard().getTileC(i,j).addTargetTime(Time.deltaTime);
					}
				}
			}
		}

		if(this.draggingCardID!=-1){
			Vector3 mousePos = Input.mousePosition;
			this.getCards().getCardC(draggingCardID).setPosition(Camera.main.ScreenToWorldPoint(mousePos));
		}

		if(this.draggingSBID!=-1){
			Vector3 mousePos = Input.mousePosition;
			this.getSkillButton(this.draggingSBID).setPosition(Camera.main.ScreenToWorldPoint(mousePos));
		}

		if(this.ia){
			if(this.intelligence.isStarting()){
				this.intelligence.addStartTime(Time.deltaTime);
			}
		}

		if(this.getTimer().isVisible()){
			this.getTimer().addTime(Time.deltaTime);
		}

		if(this.getTimer().isVisibleFront()){
			this.getTimer().addFrontTime(Time.deltaTime);
		}

		for (int i = 0 ; i < 4 ; i++){
			if(this.getSkillButton(i).isMoving()){
				this.getSkillButton(i).addMoveTime(Time.deltaTime);
			}
		}

		if(!SE){
			if(this.toLaunchNextTurn){
				this.toLaunchNextTurn=false;
				this.hitPassButton();
			}
			else{
				this.getPassButton().white();
			}
		}
		else{
			this.getPassButton().grey();
		}
	}

	public int getCurrentCardID(){
		return this.currentCardID;
	}

	public float getTileScale(){
		return this.tileScale;
	}

	public CardC getCurrentCard(){
		return this.gamecards.getCardC(this.currentCardID);
	}

	public bool getIgnoreNextMoveOn(){
		return this.ignoreNextMoveOn;
	}

	public void setIgnoreNextMoveOn(bool b){
		this.ignoreNextMoveOn = b;
	}

	public Sprite getSkillSprite(int i){
		return this.skillSprites[i];
	}

	public Sprite getAnimSprite(int i){
		return this.animSprites[i];
	}

	public Sprite getBigCharacterSprite(int i){
		return this.bigCharacterSprites[i];
	}

	public void setMyName(string s){
		this.myPlayerName.GetComponent<TextMeshPro>().text = s;
	}

	public Board getBoard(){
		return this.board ;
	}

	public void setHisName(string s){
		this.hisPlayerName.GetComponent<TextMeshPro>().text = s;
	}

	public void createBackground(){
		print("createBack "+this.getBoard().getBoardWidth());
		for (int i = 0; i < this.board.getBoardWidth()+1; i++)
		{
			this.board.addVerticalBorder(i, (GameObject)Instantiate(verticalBorderModel));
		}
		for (int i = 0; i < this.board.getBoardHeight()+1; i++)
		{
			this.board.addHorizontalBorder(i, (GameObject)Instantiate(horizontalBorderModel));
		}

		if(this.isFirstPlayer()){
			this.createTiles();

			if(this.ia || this.isTutorial()){
				this.createCards();
			}
			else{
				//StartCoroutine(GameRPC.instance.launchRPC("createCardsRPC"));
				//GameRPC.instance.sendMyDeckID();
			}
		}
	}

	public void createCards(){
		print("CardsCreation");

		if(this.ia){
			this.gamecards.GenerateIADeck();
		}
			
		int card0Index = ApplicationModel.player.MyCards.getCardIndex(ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [0].Id);
		int card1Index = ApplicationModel.player.MyCards.getCardIndex(ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [1].Id);
		int card2Index = ApplicationModel.player.MyCards.getCardIndex(ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [2].Id);
		int card3Index = ApplicationModel.player.MyCards.getCardIndex(ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [3].Id);

		int card0DeckOrder = ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [0].deckOrder;
		int card1DeckOrder = ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [1].deckOrder;
		int card2DeckOrder = ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [2].deckOrder;
		int card3DeckOrder = ApplicationModel.player.MyDecks.getDeck (ApplicationModel.player.SelectedDeckIndex).cards [3].deckOrder;

		if (this.isFirstPlayer()){
			this.gamecards.createPlayingCard(card0DeckOrder, ApplicationModel.player.MyCards.getCardM(card0Index), true, (GameObject)Instantiate(cardModel),card0DeckOrder);
			this.gamecards.createPlayingCard(card1DeckOrder, ApplicationModel.player.MyCards.getCardM(card1Index), true, (GameObject)Instantiate(cardModel),card1DeckOrder);
			this.gamecards.createPlayingCard(card2DeckOrder, ApplicationModel.player.MyCards.getCardM(card2Index), true, (GameObject)Instantiate(cardModel),card2DeckOrder);
			this.gamecards.createPlayingCard(card3DeckOrder, ApplicationModel.player.MyCards.getCardM(card3Index), true, (GameObject)Instantiate(cardModel),card3DeckOrder);
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(0).getDeckOrder()+4, ApplicationModel.opponentDeck.getCardM(0), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(0).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(1).getDeckOrder()+4, ApplicationModel.opponentDeck.getCardM(1), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(1).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(2).getDeckOrder()+4, ApplicationModel.opponentDeck.getCardM(2), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(2).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(3).getDeckOrder()+4, ApplicationModel.opponentDeck.getCardM(3), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(3).getDeckOrder());
		}
		else{
			this.gamecards.createPlayingCard(card0DeckOrder+4, ApplicationModel.player.MyCards.getCardM(card0Index), true, (GameObject)Instantiate(cardModel),card0DeckOrder);
			this.gamecards.createPlayingCard(card1DeckOrder+4, ApplicationModel.player.MyCards.getCardM(card1Index), true, (GameObject)Instantiate(cardModel),card1DeckOrder);
			this.gamecards.createPlayingCard(card2DeckOrder+4, ApplicationModel.player.MyCards.getCardM(card2Index), true, (GameObject)Instantiate(cardModel),card2DeckOrder);
			this.gamecards.createPlayingCard(card3DeckOrder+4, ApplicationModel.player.MyCards.getCardM(card3Index), true, (GameObject)Instantiate(cardModel),card3DeckOrder);
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(0).getDeckOrder(), ApplicationModel.opponentDeck.getCardM(0), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(0).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(1).getDeckOrder(), ApplicationModel.opponentDeck.getCardM(1), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(1).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(2).getDeckOrder(), ApplicationModel.opponentDeck.getCardM(2), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(2).getDeckOrder());
			this.gamecards.createPlayingCard(ApplicationModel.opponentDeck.getCardM(3).getDeckOrder(), ApplicationModel.opponentDeck.getCardM(3), false, (GameObject)Instantiate(cardModel),ApplicationModel.opponentDeck.getCardM(3).getDeckOrder());
		}

		this.intelligence.placeCards();
		if(this.ia || this.isTutorial()){
			this.resize();
		}
		else{
			GameRPC.instance.launchRPC("resizeRPC");
		}
		PhotonNetwork.Reconnect();
	}

	void createTiles()
	{
		bool isRock = false;
		List<TileM> rocks = new List<TileM>();
		TileM t = new TileM(0,0) ;

		if(ApplicationModel.player.ToLaunchGameTutorial){
			rocks.Add(new TileM(2,2));
			rocks.Add(new TileM(0,2));
			rocks.Add(new TileM(5,4));
		}
		else{
			int nbRocksToAdd = UnityEngine.Random.Range(3, 6);
			int compteurRocks = 0;
			bool isOk = true;
			while (compteurRocks<nbRocksToAdd){
				isOk = false;
				while (!isOk)
				{
					t = new TileM();
					t.randomize(this.board.getBoardWidth(), this.board.getBoardHeight());
					isOk = true;
					for (int a = 0; a < rocks.Count && isOk; a++){
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
		
		for (int x = 0; x < this.board.getBoardWidth(); x++){
			for (int y = 0; y < this.board.getBoardHeight(); y++){
				isRock = false;
				for (int z = 0; z < rocks.Count && !isRock; z++){
					if (rocks [z].x == x && rocks [z].y == y){
						isRock = true;
					}
				}
				if(this.ia || this.isTutorial()){
					this.createTile(x, y, isRock);
				}
				else{
					GameRPC.instance.launchRPC("createTileRPC", x, y, isRock);
				}
			}
		}
	}

	public CancelButtonC getCancelButton(){
		return this.cancelButton.GetComponent<CancelButtonC>();
	}

	public StartButtonC getStartButton(){
		return this.startButton.GetComponent<StartButtonC>();
	}

	public TimerC getTimer(){
		return this.timer.GetComponent<TimerC>();
	}

	public ForfeitC getForfeitButton(){
		return this.forfeitButton.GetComponent<ForfeitC>();
	}

	public LeftSlider getMyHoveredCard(){
		return this.myHoveredCard.GetComponent<LeftSlider>();
	}

	public RightSlider getHisHoveredCard(){
		return this.hisHoveredCard.GetComponent<RightSlider>();
	}

	public PassButtonC getPassButton(){
		return this.passButton.GetComponent<PassButtonC>();
	}

	public SkillButtonC getSkillButton(int i){
		return this.skillsButton[i].GetComponent<SkillButtonC>();
	}

	public TimelineC getTimeline(){
		return this.timeline.GetComponent<TimelineC>();
	}

	public InterludeC getInterlude(){
		return this.interlude.GetComponent<InterludeC>();
	}

	public Gamecards getCards(){
		return this.gamecards;
	}

	public SkillsM getSkills(){
		return this.skills;
	}

	public bool isFirstPlayer(){
		return this.firstPlayer;
	}

	public bool isIA(){
		return this.ia;
	}

	public void setFirstPlayer(bool b){
		this.firstPlayer = b;
	}

	public void setIA(bool b){
		if(b){
			this.intelligence = new Intelligence();
		}
		this.ia = b;
	}

	public bool isTutorial(){
		return ApplicationModel.player.ToLaunchGameTutorial;
	}

	public void resize(){
		print("Resizing");

		int width = Screen.width ;
		int height = Screen.height ;

		float realwidth = 10f*width/height;

		this.getMyHoveredCard().putToStartPosition(realwidth);
		this.getHisHoveredCard().putToStartPosition(realwidth);
		this.mobile = (width<height);

		this.getMyHoveredCard().activateCollider(this.mobile);
		this.getHisHoveredCard().activateCollider(this.mobile);
		float TileScale = Mathf.Min (realwidth/6.05f, 8f/this.board.getBoardHeight());
		this.tileScale = TileScale ;

		for (int i = 0; i < this.board.getBoardHeight()+1; i++){
			this.board.sizeHorizontalBorder(i, new Vector3(0,(-4*tileScale)+tileScale*i,-1f), new Vector3(1f,0.5f,1f));
		}
		
		for (int i = 0; i < this.board.getBoardWidth()+1; i++){
			this.board.sizeVerticalBorder(i, new Vector3((-this.board.getBoardWidth()/2f+i)*tileScale, 0f, -1f), new Vector3(0.5f,tileScale,1f));
		}

		if(this.mobile){
			this.getForfeitButton().size(new Vector3(-realwidth/2f+0.25f, 4.75f, 0f), new Vector3(0.4f, 0.4f, 0.4f));
			GameObject.Find("MyPlayerName").GetComponent<MeshRenderer>().enabled = false;
			GameObject.Find("HisPlayerName").GetComponent<MeshRenderer>().enabled = false;
		}
		else{
			this.getForfeitButton().size(new Vector3(-realwidth/2f+0.5f, 4.5f, 0f), new Vector3(0.8f, 0.8f, 0.8f));
			GameObject.Find("MyPlayerName").transform.localPosition = new Vector3(-realwidth/2f+1f, 4.5f, 0f);
			GameObject.Find("HisPlayerName").transform.localPosition = new Vector3(realwidth/2f, 4.5f, 0f);
		}

		this.getTimer().size(new Vector3(0f, 4*tileScale+0.1f, 0f));

		GameObject.Find("Logo").transform.position = new Vector3(0f, 2.5f+2*tileScale, 0f);
		this.getStartButton().size(new Vector3(0f, -2.5f-2*tileScale, 0f));

		this.getPassButton().size(new Vector3(Mathf.Min(2.2f, 0.5f*realwidth-0.8f), -2.4f-2*tileScale, 0f));
		this.getSkillButton(0).size(new Vector3(-2.5f*tileScale, -2.4f-2*tileScale, 0f));
		this.getSkillButton(1).size(new Vector3(-1.5f*tileScale, -2.4f-2*tileScale, 0f));
		this.getSkillButton(2).size(new Vector3(-0.5f*tileScale, -2.4f-2*tileScale, 0f));
		this.getSkillButton(3).size(new Vector3(0.5f*tileScale, -2.4f-2*tileScale, 0f));

		this.getCancelButton().size(new Vector3(0f, -2.4f-2*tileScale, 0f));
		this.getInterlude().size(realwidth);

		for(int i = 0 ; i < this.board.getBoardWidth() ; i++){
			for(int j = 0 ; j < this.board.getBoardHeight() ; j++){
				this.board.getTileC(i,j).size(tileScale);
			}
		}

		for(int i = 0 ; i < this.board.getBoardWidth() ; i++){
			for(int j = 0 ; j < this.board.getBoardHeight() ; j++){
				this.board.getTileC(i,j).size(tileScale);
			}
		}

		for(int i = 0 ; i < this.gamecards.getNumberOfCards() ; i++){
			this.gamecards.getCardC(i).resize(tileScale);
		}

		PhotonC.instance.hideLoadingScreen();
		this.launchPreGame();
	}

	public void launchPreGame(){
		if(this.ia){
			this.intelligence.launch();
		}
		this.setInitialDestinations();
		this.getTimer().setTimer(30);
		this.getStartButton().show(true);
		this.getForfeitButton().show(true);
	}

	public void createTile(int x, int y, bool type)
	{
		this.board.addTile(x,y,type,(GameObject)Instantiate(this.tileModel));
	}

	public void setInitialDestinations(){
		if(this.firstPlayer){
			for(int i = 0 ; i < this.board.getBoardWidth() ; i++){
				for(int j = 0 ; j < 2 ; j++){
					if(this.board.getTileC(i,j).isEmpty()){
						this.board.getTileC(i,j).displayDestination(0);
					}
					else{
						this.board.getTileC(i,j).showDestination(false);
					}
				}
			}
		}
		else{
			for(int i = 0 ; i < this.board.getBoardWidth() ; i++){
				for(int j = 6 ; j < 8 ; j++){
					if(this.board.getTileC(i,j).isEmpty()){
						this.board.getTileC(i,j).displayDestination(0);
					}
					else{
						this.board.getTileC(i,j).showDestination(false);
					}
				}
			}
		}
	}

	public void hoverMyCard(int i){
		this.getMyHoveredCard().setNextCard(i);
		if(this.currentCardID!=-1){
			this.displayDestinations(i);
		}
	}

	public void hoverHisCard(int i){
		this.getHisHoveredCard().setNextCard(i);
		if(this.currentCardID!=-1){
			this.displayDestinations(i);
		}
	}

	public void hoverTile(){
		if(this.currentCardID==-1){
			this.getMyHoveredCard().setNextCard(-1);
			this.getHisHoveredCard().setNextCard(-1);
		}
		else{
			this.displayDestinations(this.currentCardID);
			if(this.getCurrentCard().getCardM().isMine()){
				this.getMyHoveredCard().setNextCard(this.currentCardID);
				this.getHisHoveredCard().setNextCard(-1);
			}
			else{
				this.getHisHoveredCard().setNextCard(this.currentCardID);
				this.getMyHoveredCard().setNextCard(-1);
			}
		}
	}

	public bool isMobile(){
		return this.mobile;
	}

	public void failToReconnect(){
		print("Pas réussi à me reconnecter...");
		SceneManager.LoadScene("Home");
	}

	public void hitCard(int i){
		if(this.currentCardID==-1){
			this.startDragging(i);
		}
		else if(this.getCurrentCard().getCardM().isMine() && i==this.currentCardID && this.getCurrentCard().canMove()){
			this.startDragging(i);
		}
	}

	public void startDragging(int i){
		this.draggingCardID = i ;
		this.gamecards.getCardC(i).moveForward();
		this.gamecards.getCardC(i).showCollider(false);
		this.gamecards.getCardC(i).showHover(false);
		this.gamecards.getCardC(i).displayBackTile(false);
		this.getBoard().getTileC(this.getCards().getCardC(i).getTileM()).free();
	}

	public int getDraggingCardID(){
		return this.draggingCardID;
	}

	public int getDraggingSBID(){
		return this.draggingSBID;
	}

	public void dropOnTile(int x, int y){
		if(this.getBoard().getTileC(x,y).getDestination()==0){
			if(currentCardID!=-1){
				if(this.draggingCardID==this.getCurrentCardID()){
					this.getCurrentCard().move(true);
				}
				this.displayDestinations(this.draggingCardID);
			}

			if(this.ia || this.isTutorial()){
				this.moveOn(x,y,draggingCardID);
			}
			else{
				if(this.draggingCardID==this.currentCardID){
					this.moveOn(x,y,draggingCardID);
					this.setIgnoreNextMoveOn(true);
				}
				GameRPC.instance.launchRPC("moveOnRPC",x,y,draggingCardID);
			}
		}
		else{
			if(this.getBoard().getTileC(x,y).getCharacterID()==-1){
				this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(68),2);
			}
			else{
				if(this.draggingCardID==this.getBoard().getTileC(x,y).getCharacterID()){
					//this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(71),2);
				}
				else{
					this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(69),2);
				}
			}
			this.dropOutsideBoard();
		}
		this.draggingCardID = -1;
	}

	public void dropSBOnTile(int x, int y){
		if(this.getBoard().getTileC(x,y).isTarget()){
			this.getCurrentCard().play(true);
			this.useSkill(x,y, draggingSBID);
		}
		else{
			if(this.getBoard().getTileC(x,y).getCharacterID()==-1){
				this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(73),2);
			}
			else{
				this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(74),2);
			}
		}
		this.dropSBOutsideBoard();
	}

	public void useSkill(int x, int y, int id){
		
		this.getSkillButton(id).getSkillC().resolve(x,y,this.getSkillButton(id).getSkill());
	}

	public void moveOn(int x, int y, int i){
		if(this.currentCardID==-1){
			if(this.gamecards.getCardC(i).getCardM().isMine()){
				this.gamecards.getCardC(i).startMove(this.gamecards.getCardC(i).getPosition(), this.getBoard().getTileC(x,y).getPosition());
			}
		}
		else{
			this.gamecards.getCardC(i).startMove(this.gamecards.getCardC(i).getPosition(), this.getBoard().getTileC(x,y).getPosition());
		}

		this.updateBoard(x,y,i);
	}

	public void updateBoard(int x, int y, int i){
		if(this.currentCardID!=-1){
			if(i==this.getCurrentCardID()){
				this.getCurrentCard().move(true);
			}
		}

		this.getBoard().getTileC(this.gamecards.getCardC(i).getTileM()).setCharacterID(-1);
		this.getBoard().getTileC(this.gamecards.getCardC(i).getTileM()).showCollider(true);
		this.gamecards.getCardC(i).setTile(new TileM(x,y));
		this.gamecards.getCardC(i).showCollider(true);
		this.getBoard().getTileC(x,y).showCollider(false);
		this.getBoard().getTileC(x,y).BruteStopSE();
		this.getBoard().getTileC(x,y).setCharacterID(i);

		if(this.currentCardID!=-1){
			this.loadDestinations();
			this.actuController();
		}
	}

	public void dropOutsideBoard(){
		if(this.currentCardID==-1){

		}
		else{
			this.getBoard().getTileC(this.gamecards.getCardC(this.draggingCardID).getTileM()).showDestination(false);
		}
		this.gamecards.getCardC(this.draggingCardID).startMove(this.gamecards.getCardC(this.draggingCardID).getPosition(), this.getBoard().getTileC(this.gamecards.getCardC(this.draggingCardID).getTileM()).getPosition());
		this.draggingCardID = -1;
	}

	public void dropSBOutsideBoard(){
		this.getSkillButton(this.draggingSBID).startMove(this.getSkillButton(this.draggingSBID).getPosition(), this.getSkillButton(this.draggingSBID).getInitialPosition());
		this.getBoard().stopTargets(this.getCurrentSkillButtonC().getTargets());
		this.actuController();
		this.draggingSBID = -1;
	}

	public void endMove(){
		if(this.currentCardID==-1){
			this.setInitialDestinations();
		}
	}

	public void pushStartButton(){
		if(this.ia || this.isTutorial()){
			this.addStartGame(true);
		}
		else{
			GameRPC.instance.launchRPC("startGameRPC");
		}
		this.getStartButton().setText(WordingGame.getText(70));
		this.getStartButton().hideButton();
	}

	public void addStartGame(bool b){
		this.nbPlayersReadyToStart++;
		if(nbPlayersReadyToStart==2){
			this.startGame(!b);
		}
	}

	public void deleteDestinations(){
		for(int i = 0 ; i < this.getBoard().getBoardWidth() ; i++){
			for(int j = 0 ; j < this.getBoard().getBoardHeight() ; j++){
				this.getBoard().getTileC(i,j).showDestination(false);
			}
		}
	}

	public void startGame(bool b){
		this.getStartButton().show(false);
		this.showOpponentCards();
		if(b){
			this.cardsToPlay.Add(0);
			this.cardsToPlay.Add(4);
			this.cardsToPlay.Add(1);
			this.cardsToPlay.Add(5);
			this.cardsToPlay.Add(2);
			this.cardsToPlay.Add(6);
			this.cardsToPlay.Add(3);
			this.cardsToPlay.Add(7);
		}
		else{
			this.cardsToPlay.Add(4);
			this.cardsToPlay.Add(0);
			this.cardsToPlay.Add(5);
			this.cardsToPlay.Add(1);
			this.cardsToPlay.Add(6);
			this.cardsToPlay.Add(2);
			this.cardsToPlay.Add(7);
			this.cardsToPlay.Add(3);
		}
		this.getTimeline().show(true);
		this.firstTurn();
	}

	public void showOpponentCards(){
		if(this.firstPlayer){
			this.gamecards.getCardC(4).show(true);
			this.gamecards.getCardC(5).show(true);
			this.gamecards.getCardC(6).show(true);
			this.gamecards.getCardC(7).show(true);
		}
		else{
			this.gamecards.getCardC(0).show(true);
			this.gamecards.getCardC(1).show(true);
			this.gamecards.getCardC(2).show(true);
			this.gamecards.getCardC(3).show(true);
		}
	}

	public void firstTurn(){
		this.indexPlayingCard = 0 ;
		this.giveHandTo(this.cardsToPlay[0]);
	}

	public void handleBeginningTurnEffects(){
		if(this.getCurrentCard().getCardM().getCharacterType()==69){
			this.getSkills().skills[69].resolve(this.getCurrentCard().getCardM().getSkill(0));
		}
	}

	public void launchNextTurn(){
		this.getCurrentCard().stopClignote();
		this.getCurrentCard().displayBackTile(true);
		this.getCurrentCard().play(false);
		this.getCurrentCard().move(false);
		this.indexPlayingCard++;
		if(this.indexPlayingCard>=this.getCards().getNumberOfCards()){
			this.indexPlayingCard = 0 ;
		}
		while(this.getCards().getCardC(this.cardsToPlay[indexPlayingCard]).isDead()){
			this.indexPlayingCard++;
		}

		this.giveHandTo(this.cardsToPlay[this.indexPlayingCard]);
	}

	public void updateTimeline(){
		List<int> liste = new List<int>();
		liste.Add(this.lastToPlay);
		int tempInt = this.indexPlayingCard;
		int tempIndex = this.indexMeteores;

		while(liste.Count<8){
			if(!this.getCards().getCardC(this.cardsToPlay[tempInt]).isDead()){
				liste.Add(this.cardsToPlay[tempInt]);
			}
			if(liste.Count<8){
				tempInt++;
				if(tempInt==this.getCards().getNumberOfCards()){
					tempInt = 0;
					liste.Add(-1*tempIndex);
					tempIndex++;
				}
			}
		}
		this.getTimeline().changeFaces(liste);
	}

	public void giveHandTo(int i){
		print("AU TOUR DE "+i);
		if(this.getCards().getCardC(i).getCardM().isMine()){
			this.getInterlude().launchType(0);
		}
		else{
			this.getInterlude().launchType(1);
		}
	
		this.lastToPlay = this.currentCardID;
		this.currentCardID = i;
		this.updateTimeline();
		this.getCards().getCardC(this.currentCardID).startClignote();
			
		if(this.getCards().getCardC(this.currentCardID).getCardM().isMine()){
			this.getMyHoveredCard().setNextCard(this.currentCardID);
			this.loadController();
		}
		else{
			this.getHisHoveredCard().setNextCard(this.currentCardID);
			this.displayController(false);
		}

		this.loadDestinations();
		this.displayDestinations();
	}

	public void startActions(){
		if(this.ia){
			if(!this.getCards().getCardC(this.currentCardID).getCardM().isMine()){
				StartCoroutine(this.intelligence.play());
			}
		}
		this.getTimer().setTimer(30);
		this.handleBeginningTurnEffects();
	}

	public void loadController(){
		CardC c = this.getCurrentCard();
		this.getSkillButton(0).setCard(c);
		for(int i = 1 ; i < 4 ; i++){
			if (c.getCardM().getSkill(i).IsActivated==1){
				this.getSkillButton(i).setCard(c);
			}
		}

		this.actuController();

		this.getSkillButton(0).show(true);
		for(int i = 1 ; i < 4 ; i++){
			if (c.getCardM().getSkill(i).IsActivated==1){
				this.getSkillButton(i).show(true);
			}
		}
		this.getPassButton().setLaunchable(true);
		this.getPassButton().show(true);
	}

	public void setSE(bool b){
		this.SE = b ;
	}

	public void actuController(){
		CardC c = this.getCurrentCard();

		if(this.getCurrentCard().hasPlayed()){
			this.getSkillButton(0).forbid();
			for(int i = 1 ; i < 4 ; i++){
				if (c.getCardM().getSkill(i).IsActivated==1){
					this.getSkillButton(i).forbid();
				}
			}
			if(this.getCurrentCard().hasMoved()){
				if(SE){
					this.toLaunchNextTurn = true;
				}
				else{
					this.hitPassButton();
				}
			}
		}
		else{
			this.getSkillButton(0).update();
			for(int i = 1 ; i < 4 ; i++){
				if (c.getCardM().getSkill(i).IsActivated==1){
					this.getSkillButton(i).update();
				}
			}	
		}

		if(SE){
			this.getPassButton().grey();
		}
		else{
			this.getPassButton().white();
		}
	}

	public void displayDestinations(){
		TileM tile = this.board.getMouseTile();
		int characterID;

		if(tile.x>=0 && tile.x<Game.instance.getBoard().getBoardWidth() && tile.y>=0 && tile.y<Game.instance.getBoard().getBoardHeight()){
			characterID = this.getBoard().getTileC(tile).getCharacterID();
		}
		else{
			characterID = -1;
		}

		if(characterID==-1){
			this.displayDestinations(this.currentCardID);
		}
		else{
			this.displayDestinations(characterID);
		}
	}

	public void displayDestinations(int c){
		this.deleteDestinations();
		int type = 0 ;
		if(c==this.currentCardID && !this.getCurrentCard().hasMoved()){
			if(this.getCurrentCard().getCardM().isMine()){
				type = 0;
			}
			else{
				type = 1 ;
			}
		}
		else{
			type = 2 ;
		}
		for (int i = 0 ; i < this.getBoard().getBoardWidth();i++){
			for (int j = 0 ; j < this.getBoard().getBoardHeight();j++){
				if(this.gamecards.getCardC(c).canMoveOn(i,j)){
					this.getBoard().getTileC(i,j).displayDestination(type);
				}
				else{
					this.getBoard().getTileC(i,j).showDestination(false);
				}
			}
		}
	}

	public void loadDestinations(){
		for (int i = 0 ; i < this.gamecards.getNumberOfCards() ; i++){
			if(!this.gamecards.getCardC(i).isDead()){
				this.gamecards.getCardC(i).setDestinations(this.getDestinations(i));
			}
		}
		this.displayDestinations();
	}

	public bool[,] getDestinations(int i){
		int boardWidth = this.getBoard().getBoardWidth();
		int boardHeight = this.getBoard().getBoardHeight();
		/*
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
		*/
		bool[,] hasBeenPassages = new bool[boardWidth, boardHeight];
		bool[,] isDestination = new bool[boardWidth, boardHeight];
		for(int l = 0 ; l < boardWidth ; l++){
			for(int k = 0 ; k < boardHeight ; k++){
				hasBeenPassages[l,k]=false;
				isDestination[l,k]=false;
			}
		}
		List<TileM> baseTiles = new List<TileM>();
		List<TileM> tempTiles = new List<TileM>();
		List<TileM> tempNeighbours ;
		baseTiles.Add(this.gamecards.getCardC(i).getTileM());
		int move = this.gamecards.getCardC(i).getMove();
		
		int j = 0 ;
		bool mine = this.gamecards.getCardC(i).getCardM().isMine()	;
		while (j < move){
			tempTiles = new List<TileM>();
			for(int k = 0 ; k < baseTiles.Count ; k++){
				tempNeighbours = this.getBoard().getTileNeighbours(baseTiles[k]);
				for(int l = 0 ; l < tempNeighbours.Count ; l++){
					if(!hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]){
						if(this.getBoard().getTileC(tempNeighbours[l].x, tempNeighbours[l].y).canPassOver(mine)){
							tempTiles.Add(tempNeighbours[l]);
							hasBeenPassages[tempNeighbours[l].x, tempNeighbours[l].y]=true;
						}
					}
					if(this.getBoard().getTileC(tempNeighbours[l].x, tempNeighbours[l].y).isEmpty()){
						if(!isDestination[tempNeighbours[l].x, tempNeighbours[l].y]){
							isDestination[tempNeighbours[l].x, tempNeighbours[l].y]=true;
						}
					}
				}	
			}
			baseTiles = new List<TileM>();
			for(int l = 0 ; l < tempTiles.Count ; l++){
				baseTiles.Add(tempTiles[l]);
			}
			j++;
		}

		return isDestination;
	}

	public void displayController(bool b){
		this.getSkillButton(0).show(b);
		this.getSkillButton(1).show(b);
		this.getSkillButton(2).show(b);
		this.getSkillButton(3).show(b);
		this.getPassButton().show(b);
	}

	public void timeOver(){
		if(this.currentCardID==-1){
			this.pushStartButton();
		}
		else{
			if(this.getCurrentCard().getCardM().isMine()){
				
			}
			else{

			}
		}
	}

	public void hitSkillButton(int id){
		this.draggingSBID = id;

		this.getSkillButton(id).showTitle(false);
		this.getSkillButton(id).showCollider(false);
		this.getSkillButton(id).red();
	}

	public void hitPassButton(){
		if(!this.areUnitsDead(true) && !this.areUnitsDead(false)){
			this.displayController(false);
			if(this.ia || this.isTutorial()){
				this.launchNextTurn();
			}
			else{
				GameRPC.instance.launchRPC("launchNextTurnRPC");
			}
		}
	}

	public SkillButtonC getCurrentSkillButtonC(){
		return this.getSkillButton(this.draggingSBID);
	}

	public bool areUnitsDead(bool b){
		bool areTheyDead = true ;

		for (int i = 0 ; i < 8 ; i++){
			if (this.getCards().getCardC(i).getCardM().isMine()==b){
				if (!this.getCards().getCardC(i).isDead()){
					areTheyDead = false ;
				}
			}
		}
		if(areTheyDead){
			if(this.ia || this.isTutorial()){
				StartCoroutine(quitGameHandler(b));
			}
			else{
				GameRPC.instance.launchRPC("LostRPC",b);
			}
		}

		return areTheyDead;
	}

	public IEnumerator quitGameHandler(bool hasFirstPlayerLost)
	{		
		this.getHisHoveredCard().moveCharacterBackward();
		this.getMyHoveredCard().moveCharacterBackward();
		int type = 2;
		if(hasFirstPlayerLost==this.firstPlayer){
			type = 3 ;
		}
		this.getInterlude().launchType(type);

		if(this.isTutorial())
		{
            if(hasFirstPlayerLost==this.firstPlayer)
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
            if(hasFirstPlayerLost==this.firstPlayer)
            {
                ApplicationModel.player.HasWonLastGame=false;
                ApplicationModel.player.PercentageLooser=Game.instance.getPercentageTotalDamages();
            }
            else
            {
                ApplicationModel.player.HasWonLastGame=true;
            }
            yield return (StartCoroutine(this.sendStat(ApplicationModel.player.PercentageLooser,ApplicationModel.currentGameId,ApplicationModel.player.HasWonLastGame,ApplicationModel.player.IsFirstPlayer)));
		}
	}

	public int getPercentageTotalDamages(){
		int damages = 0;
		int total = 0;
		for(int i = 0 ; i < this.getCards().getNumberOfCards(); i++){
			if(!this.getCards().getCardC(i).getCardM().isMine()){
				damages+=Mathf.Max(0,(this.getCards().getCardC(i).getCardM().getLife()-this.getCards().getCardC(i).getLife()));
				total+=this.getCards().getCardC(i).getCardM().getLife();
			}
		}
		return Mathf.FloorToInt(100*damages/total);
	}

	public IEnumerator sendStat(int percentageTotalDamages, int currentGameid, bool hasWon, bool isFirstPlayer)
	{
        int hasWonInt = 0;
        if(hasWon)
        {
            hasWonInt=1;
        }
        int isFirstPlayerInt =0;
        if(isFirstPlayer)
        {
            isFirstPlayerInt=1;
        }
		
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
        form.AddField("myform_percentagelooser",percentageTotalDamages);
        form.AddField("myform_currentgameid",currentGameid);
        form.AddField("myform_haswon",hasWonInt);
        form.AddField("myform_isfirstplayer",isFirstPlayerInt);

        ServerController.instance.setRequest(this.URLStat, form);
        yield return ServerController.instance.StartCoroutine("executeRequest");

        if(ServerController.instance.getError()!="")
        {
            Debug.Log(ServerController.instance.getError());
            //ServerController.instance.lostConnection();
        }
	}

	public void hitForfeit(){
		if(this.ia || this.isTutorial()){
			StartCoroutine(quitGameHandler(true));
		}
		else{
			GameRPC.instance.launchRPC("LostRPC",true);
		}
	}

	public void updateModifyers(int i){
		if(this.getMyHoveredCard().getCurrentCard()==i){
			this.getMyHoveredCard().setCard(i);
		}
		if(this.getHisHoveredCard().getCurrentCard()==i){
			this.getHisHoveredCard().setCard(i);
		}
		this.actuController();
	}

	public int getIndexMeteores(){
		return this.indexMeteores;
	}
}
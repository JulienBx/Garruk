using System;
using UnityEngine;
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

	GameObject myHoveredCard ;
	GameObject hisHoveredCard ;
	GameObject startButton ;
	GameObject interlude ;
	GameObject passButton ;
	GameObject[] skillsButton ;
	GameObject popUpChoice ;
	GameObject timeline ;
	GameObject myTimer ;
	GameObject hisTimer ;
	GameObject frontTimer ;
	GameObject popUpValidation ;
	GameObject forfeitButton ;
	GameObject myPlayerName ;
	GameObject hisPlayerName ;
	GameObject cancelButton ;

	SkillsM skills ;

	bool mobile ;
	bool firstPlayer ;
	bool ia;

	Intelligence intelligence ;
	int currentCardID ;
	int draggingCardID ;
	float tileScale;
	int nbPlayersReadyToStart;

	List<int> cardsToPlay;

	void Awake(){
		instance = this;
		this.currentCardID = -1;
		this.draggingCardID = -1;
		this.nbPlayersReadyToStart = 0;
		this.skills = new SkillsM();
		this.firstPlayer = false ;
		this.cardsToPlay = new List<int>();
		this.ia = false ;
		GameObject.Find("PhotonController").GetComponent<PhotonController>().isOk = false ;
		PhotonC.instance.findRoom();

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
		this.myTimer = GameObject.Find("MyTimer");
		this.hisTimer = GameObject.Find("HisTimer");
		this.frontTimer = GameObject.Find("FrontTimer");
		this.popUpValidation = GameObject.Find("PopUpValidation");
		this.forfeitButton = GameObject.Find("ForfeitButton");
		this.myPlayerName = GameObject.Find("MyPlayerName");
		this.hisPlayerName = GameObject.Find("HisPlayerName");
		this.cancelButton = GameObject.Find("CancelButton");

		GameObject.Find("Logo").GetComponent<SpriteRenderer>().enabled = false;
	}

	void Update(){
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

		if(this.getHisHoveredCard().isMoving()){
			this.getHisHoveredCard().addTime(Time.deltaTime);
		}

		if(this.getHisHoveredCard().isClignoting()){
			this.getHisHoveredCard().addTimeClignoting(Time.deltaTime);
		}

		if(this.gamecards.isLoaded()){
			for (int i = 0 ; i < 8 ; i++){
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
			}
		}

		if(this.getBoard().isLoaded()){
			for (int i = 0 ; i < this.getBoard().getBoardWidth() ; i++){
				for (int j = 0 ; j < this.getBoard().getBoardHeight() ; j++){
					if(this.getBoard().getTileC(i,j).isSkillEffect()){
						this.getBoard().getTileC(i,j).addSETime(Time.deltaTime);
					}
				}
			}
		}

		if(this.draggingCardID!=-1){
			Vector3 mousePos = Input.mousePosition;
			this.getCards().getCardC(draggingCardID).setPosition(Camera.main.ScreenToWorldPoint(mousePos));
		}

		if(this.ia){
			if(this.intelligence.isStarting()){
				this.intelligence.addStartTime(Time.deltaTime);
			}
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

	public Sprite getSkillSprite(int i){
		return this.skillSprites[i];
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
		print("BackgroundCreation");
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

		if (this.isFirstPlayer()){
			this.gamecards.createPlayingCard(0, ApplicationModel.player.MyDeck.getCardM(0), true, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(1, ApplicationModel.player.MyDeck.getCardM(1), true, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(2, ApplicationModel.player.MyDeck.getCardM(2), true, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(3, ApplicationModel.player.MyDeck.getCardM(3), true, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(4, ApplicationModel.opponentDeck.getCardM(0), false, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(5, ApplicationModel.opponentDeck.getCardM(1), false, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(6, ApplicationModel.opponentDeck.getCardM(2), false, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(7, ApplicationModel.opponentDeck.getCardM(3), false, (GameObject)Instantiate(cardModel));
		}
		else{
			this.gamecards.createPlayingCard(4, ApplicationModel.player.MyDeck.getCardM(0), true, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(5, ApplicationModel.player.MyDeck.getCardM(1), true, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(6, ApplicationModel.player.MyDeck.getCardM(2), true, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(7, ApplicationModel.player.MyDeck.getCardM(3), true, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(0, ApplicationModel.opponentDeck.getCardM(0), false, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(1, ApplicationModel.opponentDeck.getCardM(1), false, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(2, ApplicationModel.opponentDeck.getCardM(2), false, (GameObject)Instantiate(cardModel));
			this.gamecards.createPlayingCard(3, ApplicationModel.opponentDeck.getCardM(3), false, (GameObject)Instantiate(cardModel));
		}

		this.intelligence.placeCards();
		if(this.ia || this.isTutorial()){
			this.resize();
		}
		else{
			GameRPC.instance.resize();
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

	public MyTimerC getMyTimer(){
		return this.myTimer.GetComponent<MyTimerC>();
	}

	public HisTimerC getHisTimer(){
		return this.hisTimer.GetComponent<HisTimerC>();
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
			this.getMyTimer().size(new Vector3(-realwidth/2f+0.02f, 4*tileScale+0.25f, 0f));
			this.getHisTimer().size(new Vector3(realwidth/2f-0.52f, 4*tileScale+0.25f, 0f));
			GameObject.Find("MyPlayerName").GetComponent<MeshRenderer>().enabled = false;
			GameObject.Find("HisPlayerName").GetComponent<MeshRenderer>().enabled = false;
		}
		else{
			this.getForfeitButton().size(new Vector3(-realwidth/2f+0.5f, 4.5f, 0f), new Vector3(0.8f, 0.8f, 0.8f));
			this.getMyTimer().size(new Vector3(-2.98f, 4*tileScale+0.25f, 0f));
			this.getHisTimer().size(new Vector3(2.48f, 4*tileScale+0.25f, 0f));
			GameObject.Find("MyPlayerName").transform.localPosition = new Vector3(-realwidth/2f+1f, 4.5f, 0f);
			GameObject.Find("HisPlayerName").transform.localPosition = new Vector3(realwidth/2f, 4.5f, 0f);
		}

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
	}

	public void hoverHisCard(int i){
		this.getHisHoveredCard().setNextCard(i);
	}

	public void hoverTile(){
		if(this.currentCardID==-1){
			this.getMyHoveredCard().setNextCard(-1);
			this.getHisHoveredCard().setNextCard(-1);
		}
		else{
			if(this.getCurrentCard().getCardM().isMine()){
				this.getMyHoveredCard().setNextCard(this.currentCardID);
			}
			else{
				this.getHisHoveredCard().setNextCard(this.currentCardID);
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
		else if(i==this.currentCardID && this.getCurrentCard().canMove()){
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

	public void dropOnTile(int x, int y){
		if(this.getBoard().getTileC(x,y).getDestination()==0){
			if(this.ia || this.isTutorial()){
				this.moveOn(x,y,draggingCardID);
			}
			else{
				GameRPC.instance.launchRPC("moveOnRPC",x,y,draggingCardID);
			}
		}
		else{
			if(this.getBoard().getTileC(x,y).getCharacterID()==-1){
				this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(68),2);
			}
			else{
				this.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(69),2);
			}
			this.dropOutsideBoard();
		}
		this.draggingCardID = -1;
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
		this.getBoard().getTileC(this.gamecards.getCardC(i).getTileM()).setCharacterID(-1);
		this.gamecards.getCardC(i).setTile(new TileM(x,y));
		this.getBoard().getTileC(x,y).setCharacterID(i);
	}

	public void dropOutsideBoard(){
		this.gamecards.getCardC(this.draggingCardID).startMove(this.gamecards.getCardC(this.draggingCardID).getPosition(), this.getBoard().getTileC(this.gamecards.getCardC(this.draggingCardID).getTileM()).getPosition());
		this.draggingCardID = -1;
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

		this.firstTurn();
	}

	public void firstTurn(){
		this.giveHandTo(this.cardsToPlay[0]);
	}

	public void giveHandTo(int i){
		if(this.currentCardID==-1){
			this.getInterlude().launchType(0);
		}
		else{
			if(this.getCards().getCardC(i).getCardM().isMine()){
				this.getInterlude().launchType(1);
			}
			else{
				this.getInterlude().launchType(1);
			}
		}
		this.currentCardID = i;
		if(this.getCards().getCardC(this.currentCardID).getCardM().isMine()){
			this.getMyHoveredCard().setNextCard(this.currentCardID);
			this.loadController();
		}
		else{
			this.getHisHoveredCard().setNextCard(this.currentCardID);
			this.displayController(false);
		}
	}

	public void loadController(){
		CardC c = this.getCurrentCard();
		this.getSkillButton(0).init(c, 0);
		for(int i = 1 ; i < 4 ; i++){
			if (c.getCardM().getSkill(i).IsActivated==1){
				this.getSkillButton(i).init(c, i);
			}
		}
		//this.getPassButton(0).init(c);
	}

	public void displayController(bool b){
		this.getSkillButton(0).show(b);
		this.getSkillButton(1).show(b);
		this.getSkillButton(2).show(b);
		this.getSkillButton(3).show(b);
		this.getPassButton().show(b);
	}
}
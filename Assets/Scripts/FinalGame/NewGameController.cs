using System;
using UnityEngine;

public class NewGameController : MonoBehaviour
{
	public int boardWidth ;
	public int boardHeight ;

	public GameObject verticalBorderModel;
	public GameObject horizontalBorderModel;
	public GameObject tileModel;
	public GameObject tutorialModel;
	public GameObject cardModel;

	public static NewGameController instance;

	GameObject Background ;
	GameObject CentralMessage ;
	GameObject StartButton ;
	GameObject Interlude ;
	GameObject Timer ;
	GameObject TimerFront ;
	GameObject OpponentStatus ;

	GameCardsController gameCardsController;

	ScreenDimensions screenDimensions ;
	TilesController tilesController ;
	BoardController boardController ;

	bool firstPlayer ;
	bool useTutorial ;
	bool useRPC ;
	bool objectsCreated ;
	int draggingCardID;
	bool haveIStarted;
	bool hasHeStarted;
	IntelligenceController intelligence;

	TutorielsController tutorielsController ;
	TileModel hoveredTile;

	void Awake(){
		instance = this;
		this.firstPlayer = false ;
		this.useTutorial = false ;
		this.useRPC = false ;
		this.objectsCreated = false ;
		this.haveIStarted = false;
		this.hasHeStarted = false;
		this.intelligence = new IntelligenceController(1);
		this.draggingCardID=-1;
		this.hoveredTile = new TileModel(-1,-1);
		this.Background = GameObject.Find("Background");

		this.CentralMessage = GameObject.Find("CentralMessage");
		this.StartButton = GameObject.Find("StartButton");
		this.Interlude = GameObject.Find("Interlude");
		this.Timer = GameObject.Find("Timer");
		this.TimerFront = GameObject.Find("TimerFront");
		this.OpponentStatus = GameObject.Find("OpponentStatus");
		this.getCentralMessageController().setTexts(WordingGame.getText(139),WordingGame.getText(140));
		this.getCentralMessageController().show(true);

		this.boardController = new BoardController(this.boardWidth,this.boardHeight);
		this.tilesController = new TilesController(this.boardWidth, this.boardHeight);
		this.tutorielsController = new TutorielsController((GameObject)Instantiate(tutorialModel));
		this.gameCardsController = new GameCardsController();

		this.screenDimensions = new ScreenDimensions(0,0);
	}

	void Start(){
		try{
			PhotonC.instance.findRoom();
		}
		catch(Exception e){
			Debug.Log(e.ToString());
			this.getCentralMessageController().setDescription(WordingGame.getText(141));
			this.firstPlayer = true;
			this.useRPC = false;
			this.useTutorial = true;

			this.createBackground();
//			this.tutorielsController.getTutorielController().setPosition(new Vector3(0f,0f,0f));
//			this.tutorielsController.getTutorielController().setTexts("Titre tutoriel","Blablacar",WordingGame.getText(142));
//			this.tutorielsController.getTutorielController().show(true);
//			this.tutorielsController.getTutorielController().startAnimation();

			this.objectsCreated = true ;
			this.gameCardsController.createMyFakeHand(this.firstPlayer,(GameObject)Instantiate(cardModel),(GameObject)Instantiate(cardModel),(GameObject)Instantiate(cardModel),(GameObject)Instantiate(cardModel));
			this.gameCardsController.createHisFakeHand(this.firstPlayer,(GameObject)Instantiate(cardModel),(GameObject)Instantiate(cardModel),(GameObject)Instantiate(cardModel),(GameObject)Instantiate(cardModel));
		}

		this.gameCardsController.initCards();
		this.resize();

		if(this.useTutorial){
			this.tutorielsController.launchNextTutorial();
		}
		else{
			this.loadPreGameDestinations();
		}

		this.getCentralMessageController().show(false);
	}

	void Update(){
		if(this.objectsCreated){
			if(this.screenDimensions.hasChanged(Screen.width,Screen.height)){
				this.resize();
			}
		}

		if(this.draggingCardID!=-1){
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0;
			this.getGameCardsController().getCardController(this.draggingCardID).setPosition(mousePos);
		}

		if(this.Interlude.GetComponent<InterludeController>().isDisplaying()){
			this.Interlude.GetComponent<InterludeController>().addTime(Time.deltaTime);
		}

		if(this.Timer.GetComponent<TimerController>().isCounting()){
			this.Timer.GetComponent<TimerController>().addTime(Time.deltaTime);
		}

		if(this.TimerFront.GetComponent<TimerFrontController>().isCounting()){
			this.TimerFront.GetComponent<TimerFrontController>().addTime(Time.deltaTime);
		}

		if(this.intelligence.isCounting()){
			this.intelligence.addTime(Time.deltaTime);
		}

		for(int i = 0; i < 8 ; i++){
			if(this.gameCardsController.getCardController(i).isMoving()){
				this.gameCardsController.getCardController(i).addMoveTime(Time.deltaTime);
			}
			if(this.gameCardsController.getCardController(i).isBlinking()){
				this.gameCardsController.getCardController(i).addBlinkTime(Time.deltaTime);
			}
		}
	}

	public void createBackground(){
		for (int i = 0; i < this.boardWidth+1; i++)
		{
			this.boardController.addVerticalBorder(i,(GameObject)Instantiate(verticalBorderModel));
		}
		for (int i = 0; i < this.boardHeight+1; i++)
		{
			this.boardController.addHorizontalBorder(i,(GameObject)Instantiate(horizontalBorderModel));
		}

		if(this.firstPlayer){
			this.tilesController.createTiles(this.boardWidth,this.boardHeight);
		}
	}

	public void resize(){
		this.screenDimensions.setDimensions(Screen.width, Screen.height);

		float realwidth = this.screenDimensions.getRealWidth();
		this.Interlude.GetComponent<InterludeController>().resize(realwidth);
		this.boardController.resize(realwidth, this.boardWidth, this.boardHeight);
		this.tilesController.resize(realwidth, this.boardWidth, this.boardHeight);
		this.gameCardsController.resize();
		this.Timer.GetComponent<TimerController>().resize(realwidth);
	}

	public GameBackgroundController getBackground(){
		return this.Background.GetComponent<GameBackgroundController>();
	}

	public ScreenDimensions getScreenDimensions(){
		return this.screenDimensions;
	}

	public void setDraggingCardID(int i){
		this.draggingCardID = i;
	}

	public int getDraggingCardID(){
		return this.draggingCardID ;
	}

	public MessageController getCentralMessageController(){
		return this.CentralMessage.GetComponent<MessageController>();
	}

	public TutorielsController getTutorielsController(){
		return this.tutorielsController;
	}

	public TilesController getTilesController(){
		return this.tilesController;
	}

	public bool isUsingRPC(){
		return this.useRPC;
	}

	public void hitTutorial(){
		this.tutorielsController.hitTutorial();
	}

	public void createTile(int x, int y, bool isRock)
	{
		this.tilesController.createTile(x,y,isRock,(GameObject)Instantiate(this.tileModel));
	}

	public bool isFirstPlayer(){
		return this.firstPlayer;
	}

	public void setCharacterToTile(int characterID, TileModel t){
		if(this.gameCardsController.getCardController(characterID).getTileModel().getX()!=-1){
			TileModel previousTile = this.gameCardsController.getCardController(characterID).getTileModel();
			this.getTilesController().getTileController(previousTile).setCharacterID(-1);
		}
		this.gameCardsController.getCardController(characterID).setTileModel(t);
		this.getTilesController().getTileController(t).setCharacterID(characterID);
		this.updateDesti();
	}

	public GameCardsController getGameCardsController(){
		return this.gameCardsController;
	}

	public void loadPreGameDestinations(){
		this.tilesController.loadPreGameDestinations(this.firstPlayer);

		if(this.useTutorial){
			this.tutorielsController.launchNextTutorial();
		}
		else{
			this.showStartButton();
		}
	}

	public void hitStartButton(){
		this.StartButton.GetComponent<StartButtonController>().show(false);
		if(!this.useRPC){
			this.startGame(true);
		}
		else{

		}
	}

	public void showStartButton(){
		float scale = Mathf.Min(1f,NewGameController.instance.getScreenDimensions().getRealWidth()/6.05f);
		this.StartButton.GetComponent<StartButtonController>().setTexts(WordingGame.getText(143),WordingGame.getText(144));
		this.StartButton.GetComponent<StartButtonController>().setPosition(new Vector3(0f,-1*(4*scale+(5f-4f*scale)/2f)));
		this.Timer.GetComponent<TimerController>().startTimer(15);
		this.OpponentStatus.GetComponent<OpponentStatusController>().setText(WordingGame.getText(145),new Color(71f/255f,150f/255f,189f/255f, 1f));
		this.OpponentStatus.GetComponent<OpponentStatusController>().show(true);
		this.StartButton.GetComponent<StartButtonController>().show(true);

		if(!this.useRPC){
			this.intelligence.startTimer();
		}
	}

	public void handleEndInterlude(){
		this.gameCardsController.startBlinking();
		this.gameCardsController.loadDestinations();
		this.Interlude.GetComponent<InterludeController>().show(false);
		this.getTilesController().showAllColliders(true);
		this.updateDesti();
	}

	public void updateDesti(){
		if(this.hoveredTile.getX()!=-1){
			this.getTilesController().getTileController(this.hoveredTile).displayDestinations();
		}
		else{
			this.getTilesController().getTileController(NewGameController.instance.getGameCardsController().getCurrentCard().getTileModel()).displayDestinations();
		}
	}

	public void endTimer(){

	}

	public void displayTimerFront(int i, Color c){
		this.TimerFront.GetComponent<TimerFrontController>().setTimer(i, c);
	}

	public void startGame(bool isFirstP){
		if(isFirstP){
			if(this.firstPlayer){
				this.haveIStarted = true;
			}
			else{
				this.hasHeStarted = true;
				this.OpponentStatus.GetComponent<OpponentStatusController>().setText(WordingGame.getText(146),new Color(231f/255f, 0f, 66f/255f, 1f));
			}
		}
		else{
			if(!this.firstPlayer){
				this.haveIStarted = true;
			}
			else{
				this.hasHeStarted = true;
				this.OpponentStatus.GetComponent<OpponentStatusController>().setText(WordingGame.getText(146),new Color(231f/255f, 0f, 66f/255f, 1f));
			}
		}

		if(this.haveIStarted && this.hasHeStarted){
			this.OpponentStatus.GetComponent<OpponentStatusController>().show(false);
			this.Timer.GetComponent<TimerController>().stopTimer();
			if(isFirstP){
				if(this.firstPlayer){
					this.gameCardsController.initOrder(false);
				}
				else{
					this.gameCardsController.initOrder(true);
				}
			}
			else{
				if(!this.firstPlayer){
					this.gameCardsController.initOrder(false);
				}
				else{
					this.gameCardsController.initOrder(true);
				}
			}
			this.gameCardsController.incrementIndex();
		}
	}

	public InterludeController getInterludeController(){
		return this.Interlude.GetComponent<InterludeController>();
	}
}
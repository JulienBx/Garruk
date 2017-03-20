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

	GameCardsController gameCardsController;

	ScreenDimensions screenDimensions ;
	TilesController tilesController ;
	BoardController boardController ;

	bool firstPlayer ;
	bool useTutorial ;
	bool useRPC ;
	bool objectsCreated ;
	int draggingCardID;

	TutorielsController tutorielsController ;

	void Awake(){
		instance = this;
		this.firstPlayer = false ;
		this.useTutorial = false ;
		this.useRPC = false ;
		this.objectsCreated = false ;
		this.draggingCardID=-1;
		this.Background = GameObject.Find("Background");

		this.CentralMessage = GameObject.Find("CentralMessage");
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
			this.tilesController.loadPreGameDestinations(this.firstPlayer);
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
		this.boardController.resize(realwidth, this.boardWidth, this.boardHeight);
		this.tilesController.resize(realwidth, this.boardWidth, this.boardHeight);
		this.gameCardsController.resize();
	}

	public GameBackgroundController getBackground(){
		return this.Background.GetComponent<GameBackgroundController>();
	}

	public void setDraggingCardID(int i){
		this.draggingCardID = i;
	}

	public int getDraggingCardID(int i){
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
		this.gameCardsController.getCardController(characterID).setTileModel(t);
		this.getTilesController().getTileController(t).setCharacterID(characterID);
	}

	public GameCardsController getGameCardsController(){
		return this.gameCardsController;
	}
}


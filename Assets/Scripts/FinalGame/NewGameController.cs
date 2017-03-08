using System;
using UnityEngine;

public class NewGameController : MonoBehaviour
{
	public int boardWidth ;
	public int boardHeight ;

	public GameObject verticalBorderModel;
	public GameObject horizontalBorderModel;
	public GameObject tileModel;

	public static NewGameController instance;

	GameObject Background ;
	GameObject CentralMessage ;

	ScreenDimensions screenDimensions ;
	TilesController tilesController ;
	BoardController boardController ;

	bool firstPlayer ;
	bool useRPC ;

	void Awake(){
		this.firstPlayer = false ;
		this.Background = GameObject.Find("Background");

		this.CentralMessage = GameObject.Find("CentralMessage");
		this.getCentralMessageController().setTexts(WordingGame.getText(139),WordingGame.getText(140));
		this.getCentralMessageController().display(true);

		this.boardController = new BoardController(this.boardWidth,this.boardHeight);
		this.tilesController = new TilesController(this.boardWidth, this.boardHeight);

		this.screenDimensions = new ScreenDimensions(0,0);
	}

	void Start(){
		this.resize();
		try{
			PhotonC.instance.findRoom();
		}
		catch(Exception e){
			Debug.Log(e.ToString());
			this.getCentralMessageController().setDescription(WordingGame.getText(141));
			this.firstPlayer = true;
			this.useRPC = false;
			this.createBackground();
		}
	}

	public void createBackground(){
		for (int i = 0; i < this.boardWidth+1; i++)
		{
			this.boardController.addVerticalBorder(i,(GameObject)Instantiate(verticalBorderModel));
		}
		for (int i = 0; i < this.boardHeight+1; i++)
		{
			this.boardController.addVerticalBorder(i,(GameObject)Instantiate(horizontalBorderModel));
		}

		if(this.firstPlayer){
			this.tilesController.createTiles(this.boardWidth,this.boardHeight);
		}
	}

	void Update(){
		if(this.screenDimensions.hasChanged(Screen.width,Screen.height)){
			this.resize();
		}
	}

	public void resize(){
		this.screenDimensions.setDimensions(Screen.width, Screen.height);

		float realwidth = this.screenDimensions.getRealWidth();
	}

	public GameBackgroundController getBackground(){
		return this.Background.GetComponent<GameBackgroundController>();
	}

	public MessageController getCentralMessageController(){
		return this.CentralMessage.GetComponent<MessageController>();
	}

	public bool isUsingRPC(){
		return this.useRPC;
	}

	public void createTile(int x, int y, bool isRock)
	{
		this.tilesController.createTile(x,y,isRock,(GameObject)Instantiate(this.tileModel));
	}
}


using UnityEngine;

public class GameScreenViewModel
{
	//styles

	//informations � afficher
	public int heightScreen;
	public int widthScreen;

	public Rect bottomZoneRect ;
	public Rect topZoneRect ;

	public Rect rightMessageRect;
	public GUIStyle centerMessageTextStyle;
	public GUIStyle rightMessageTextStyle;

	public Texture2D cursor ;
	int cursorID = -1 ; 
	public bool hasAMessage;


	public bool toDisplayStartWindows = true ;
	public bool iHaveStarted = false;
	public bool heHasStarted = false ;
	public string messageStartWindow = "Pr�paration du champ de bataille";
	public string messageStartWindowButton = "Je suis pret !" ;
	public Rect startButtonRect ;
	public GUIStyle startWindowStyle ;
	public GUIStyle quitButtonStyle ;

	public string messageOpponentStartWindow = "En attente du second joueur" ;
	public string messageOpponentStartWindowButton = "Pret � jouer !" ;
	public Rect opponentStartButtonRect ;
	public GUIStyle opponentStartWindowStyle ;

	public GUIStyle whiteSmallTextStyle ;
	public GUIStyle buttonTextStyle ;
	public GUIStyle greenInformationTextStyle ;

	public string messageToDisplay;

	public float timer;
	public float timerPopUp;

	public bool popUpDisplay;
	public bool toDisplayGameScreen = false;

	public string myPlayerName ;
	public string hisPlayerName ;
	public string quitButtonText = "Quitter la partie" ;
	public Rect quitButtonRect ;

	public Rect centerMessageRect;

	public GameScreenViewModel()
	{
		this.centerMessageTextStyle = new GUIStyle();
		this.rightMessageTextStyle = new GUIStyle();
		this.startWindowStyle = new GUIStyle();
		this.opponentStartWindowStyle = new GUIStyle();
		this.whiteSmallTextStyle = new GUIStyle();
		this.buttonTextStyle = new GUIStyle();
		this.greenInformationTextStyle = new GUIStyle();
		messageToDisplay = "";
		hasAMessage = false;
		timer = 10f;
		timerPopUp = 5f;
	}

	public void setStyles(GUIStyle[] gameScreenStyles)
	{
		this.centerMessageTextStyle = gameScreenStyles [0];
		this.rightMessageTextStyle = gameScreenStyles [1];
		this.startWindowStyle = gameScreenStyles [2];
		this.opponentStartWindowStyle = gameScreenStyles [3];
		this.whiteSmallTextStyle = gameScreenStyles [4];
		this.buttonTextStyle = gameScreenStyles [5];
		this.greenInformationTextStyle = gameScreenStyles [6];
		this.quitButtonStyle = gameScreenStyles [5] ;
	}

	public void startMyPlayer(){
		this.iHaveStarted = true ;
		this.messageStartWindowButton = "Pret � jouer !" ;
	}

	public void startOtherPlayer(){
		this.heHasStarted = true ;
	}

	public void connectOtherPlayer(){
		this.messageOpponentStartWindow = this.hisPlayerName+" pr�pare ses h�ros";
	}

	public void recalculate(int w, int h)
	{
		this.centerMessageRect = new Rect((w/2f)-h * 1f / 4f, h * 0.45f, h * 5 / 10, h * 1 / 10);
		this.rightMessageRect = new Rect(w * 0.9f, h * 0.5f, 30, 30);
		this.startButtonRect = new Rect((w/2f)-h * 1f / 4f , (h/2f)+h * 5f / 100f, h * 5 / 10, h * 1 / 10);
		this.opponentStartButtonRect = new Rect((w/2f)-h * 1f / 4f , (h/2f)- h * 15f / 100f, h * 5 / 10, h * 1 / 10);
		this.quitButtonRect = new Rect(w/2f, h*25f/1000f, w/8f, h*5f/100f  );
		
		this.centerMessageTextStyle.fontSize = h* 20 / 1000 ;
		this.rightMessageTextStyle.fontSize = h* 20 / 1000;
		this.whiteSmallTextStyle.fontSize = h* 22 / 1000;
		this.buttonTextStyle.fontSize = h* 22 / 1000;
		this.greenInformationTextStyle.fontSize = h* 22 / 1000;
		this.quitButtonStyle.fontSize = h* 22 / 1000 ;
		
		toDisplayGameScreen = true ;
	}

	public void setCursor(Texture2D c, int i)
	{
		if (i != this.cursorID)
		{
			this.cursorID = i;
			this.cursor = c;
		}
		this.changeCursor();
	}

	public void changeCursor()
	{
		Cursor.SetCursor(this.cursor, new Vector2(0, 0), CursorMode.Auto);
	}
	
	public void SetCursorToDefault()
	{
		if (this.cursorID!=-1){
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			this.cursorID = -1 ;
		}
	}
}



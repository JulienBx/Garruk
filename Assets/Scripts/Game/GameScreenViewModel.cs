using UnityEngine;
using System.Collections.Generic;

public class GameScreenViewModel
{
	//styles

	//informations à afficher
	public int heightScreen;
	public int widthScreen;

	public Rect bottomZoneRect ;
	public Rect topZoneRect ;

	public Rect rightMessageRect;
	public GUIStyle centerMessageTextStyle;
	public GUIStyle rightMessageTextStyle;

	public Texture2D cursor ;
	int cursorID = -1 ; 
	
	public bool toDisplayStartWindows = true ;
	public bool toDisplayValidationWindows = false ;
	public bool toDisplayValidationButton = false ;

	public string validationButtonText ;
	public string validationRegularText ;

	public bool iHaveStarted = false;
	public bool heHasStarted = false ;
	public string messageStartWindow = "Préparation du champ de bataille";
	public string messageStartWindowButton = "Je suis pret !" ;
	public Rect startButtonRect ;
	public GUIStyle startWindowStyle ;
	public GUIStyle quitButtonStyle ;

	public string messageOpponentStartWindow = "En attente du second joueur" ;
	public string messageOpponentStartWindowButton = "Pret à jouer !" ;
	public Rect opponentStartButtonRect ;
	public GUIStyle opponentStartWindowStyle ;

	public GUIStyle whiteSmallTextStyle ;
	public GUIStyle buttonTextStyle ;
	public GUIStyle greenInformationTextStyle ;

	public List<string> messagesToDisplay;

	public float timer;
	public List<float> timersPopUp;

	public bool toDisplayGameScreen = false;
	public bool toDisplayQuitButton = false; 

	public string myPlayerName ;
	public string hisPlayerName ;
	public string quitButtonText = "Quitter la partie" ;
	public Rect quitButtonRect ;

	public List<Rect> centerMessageRects;
	public Rect namePlayingCardRect;
	public Rect nameOpponentPlayingCardRect;

	public bool toDisplayPlayingCard;
	public bool toDisplayOpponentPlayingCard;

	public GUIStyle namePlayingCardTextStyle ;
	public GUIStyle nameOpponentPlayingCardTextStyle ;

	public string myPlayingCardName ; 
	public string hisPlayingCardName ;
	
	public Rect validationWindowRect;


	public GameScreenViewModel()
	{
		this.centerMessageTextStyle = new GUIStyle();
		this.rightMessageTextStyle = new GUIStyle();
		this.startWindowStyle = new GUIStyle();
		this.opponentStartWindowStyle = new GUIStyle();
		this.whiteSmallTextStyle = new GUIStyle();
		this.buttonTextStyle = new GUIStyle();
		this.greenInformationTextStyle = new GUIStyle();
		this.namePlayingCardTextStyle = new GUIStyle();
		this.nameOpponentPlayingCardTextStyle = new GUIStyle();
		this.messagesToDisplay = new List<string>();
		timer = 10f;
		this.timersPopUp = new List<float>();
		this.centerMessageRects = new List<Rect>();
		toDisplayGameScreen = true;
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
		this.quitButtonStyle = gameScreenStyles [5];
		this.namePlayingCardTextStyle = gameScreenStyles [7];
		this.nameOpponentPlayingCardTextStyle = gameScreenStyles [8];
	}

	public void startMyPlayer()
	{
		this.iHaveStarted = true;
		this.messageStartWindowButton = "Pret à jouer !";
	}

	public void startOtherPlayer()
	{
		this.heHasStarted = true;
	}

	public void connectOtherPlayer()
	{
		this.messageOpponentStartWindow = this.hisPlayerName + " prépare ses héros";
		this.toDisplayQuitButton = true;
	}

	public void recalculate(int w, int h)
	{
		this.rightMessageRect = new Rect(w * 0.9f, h * 0.5f, 30, 30);
		this.startButtonRect = new Rect((w / 2f) - h * 1f / 4f, (h / 2f) + h * 5f / 100f, h * 5 / 10, h * 1 / 10);
		this.opponentStartButtonRect = new Rect((w / 2f) - h * 1f / 4f, (h / 2f) - h * 15f / 100f, h * 5 / 10, h * 1 / 10);
		this.namePlayingCardRect = new Rect(-3, 4.8f, 1, 0.2f);
		this.nameOpponentPlayingCardRect = new Rect(-3, -5f, 1, 0.2f);
		this.quitButtonRect = new Rect(3 * w / 4f, h * 25f / 1000f, w / 8f, h * 5f / 100f);
		this.validationWindowRect = new Rect((w / 2f) - h * 1f / 4f, 0.91f*h, h * 5 / 10, h * 8 / 100);
		
		this.centerMessageTextStyle.fontSize = h * 18 / 1000;
		this.rightMessageTextStyle.fontSize = h * 20 / 1000;
		this.whiteSmallTextStyle.fontSize = h * 22 / 1000;
		this.buttonTextStyle.fontSize = h * 22 / 1000;
		this.greenInformationTextStyle.fontSize = h * 22 / 1000;
		this.quitButtonStyle.fontSize = h * 22 / 1000;
		this.namePlayingCardTextStyle.fontSize = h * 20/1000 ;
		this.nameOpponentPlayingCardTextStyle.fontSize = h * 22/1000 ;
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
		if (this.cursorID != -1)
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			this.cursorID = -1;
		}
	}
	
	public void resizePopUps()
	{
		int h = Screen.height ;
		int w = Screen.width ;
		
		for (int i = 0 ; i < this.messagesToDisplay.Count ; i++){
			this.centerMessageRects[i] = new Rect((w / 2f) - h * 1f / 4f, h * (0.51f - 0.05f*(this.messagesToDisplay.Count) + 0.1f*i), h * 5 / 10, h * 8 / 100);
		}
	}
}



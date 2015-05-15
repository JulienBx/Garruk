using UnityEngine;

public class GameScreenViewModel
{
	//styles

	//informations à afficher
	public int heightScreen;
	public int widthScreen;

	public Rect bottomZoneRect ;
	public Rect topZoneRect ;

	public Rect centerMessageRect;
	public Rect rightMessageRect;
	public GUIStyle centerMessageTextStyle;
	public GUIStyle rightMessageTextStyle;

	public Texture2D cursor ;
	int cursorID = -1 ; 
	public bool hasAMessage;


	public bool toDisplayStartWindows = true ;
	public string messageStartWindow = "Positionnez vos héros sur le champ de bataille";
	public string messageStartWindowButton = "Je suis pret !" ;
	public Rect startButtonRect ;
	public GUIStyle startWindowStyle ;

	public string messageOpponentStartWindow = "L'adversaire positionne ses héros";
	public Rect opponentStartButtonRect ;
	public GUIStyle opponentStartWindowStyle ;


	public string messageToDisplay;

	public float timer;
	public float timerPopUp;

	public bool popUpDisplay;

	public GameScreenViewModel()
	{
		centerMessageTextStyle = new GUIStyle();
		rightMessageTextStyle = new GUIStyle();
		messageToDisplay = "";
		hasAMessage = false;
		timer = 10f;
		timerPopUp = 5f;
	}

	public void setValues(GUIStyle[] gameScreenStyles)
	{
		centerMessageTextStyle = gameScreenStyles [0];
		rightMessageTextStyle = gameScreenStyles [1];
	}

	public void recalculate()
	{

		this.centerMessageRect = new Rect(Screen.width / 2 - 100, Screen.height * 0.95f, 200, 35);
		this.rightMessageRect = new Rect(Screen.width * 0.9f, Screen.height * 0.5f, 30, 30);
		this.startButtonRect = new Rect((Screen.width/2f)-Screen.height * 1f / 4f , (Screen.height/2f)+Screen.height * 5f / 100f, Screen.height * 5 / 10, Screen.height * 1 / 10);
		this.opponentStartButtonRect = new Rect((Screen.width/2f)-Screen.height * 1f / 4f , (Screen.height/2f)-Screen.height * 15f / 100f, Screen.height * 5 / 10, Screen.height * 1 / 10);
	}

	public void recalculate(int w, int h)
	{
		
		this.centerMessageRect = new Rect(w / 2 - 100, h * 0.95f, 200, 35);
		this.rightMessageRect = new Rect(w * 0.9f, w * 0.5f, 30, 30);
		this.startButtonRect = new Rect((Screen.width/2f)-Screen.height * 6 / 10 , (Screen.height/2f)-Screen.height * 3 / 10, Screen.height * 6 / 10, Screen.height * 3 / 10);
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
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
}



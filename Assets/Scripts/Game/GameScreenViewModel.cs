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
	public string messageToDisplay;

	public float timer;

	public GameScreenViewModel()
	{
		centerMessageTextStyle = new GUIStyle();
		rightMessageTextStyle = new GUIStyle();
		messageToDisplay = "";
		hasAMessage = false;
		timer = 10f;
	}

	public void setValues(GUIStyle[] gameScreenStyles)
	{
		centerMessageTextStyle = gameScreenStyles [0];
		rightMessageTextStyle = gameScreenStyles [1];
	}

	public void recalculate()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		this.bottomZoneRect = new Rect(0, this.heightScreen * 0.86f, this.widthScreen * 0.2f, this.heightScreen * 0.14f);
		this.topZoneRect = new Rect(this.widthScreen * 0.8f, 0, this.widthScreen * 0.2f, this.heightScreen * 0.14f);
		this.centerMessageRect = new Rect(Screen.width / 2 - 30, Screen.height / 2, 100, 35);
		this.rightMessageRect = new Rect(Screen.width * 0.9f, Screen.height * 0.5f, 30, 30);
	}

	public void setCursor(Texture2D c, int i){
		if (i!=this.cursorID){
			this.cursorID = i ;
			this.cursor = c ;
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



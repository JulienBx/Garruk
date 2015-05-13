using UnityEngine;

public class GameScreenViewModel
{
	//styles

	//informations à afficher
	public int heightScreen;
	public int widthScreen;

	public Rect bottomZoneRect ;
	public Rect topZoneRect ;

	public Texture2D cursor ;
	int cursorID = -1 ; 
	public bool hasAMessage;
	public string messageToDisplay;

	public GameScreenViewModel()
	{
		messageToDisplay = "";
		hasAMessage = false;
	}

	public void recalculate()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		this.bottomZoneRect = new Rect(0, this.heightScreen * 0.9f, this.widthScreen * 0.2f, this.heightScreen * 0.1f);
		this.topZoneRect = new Rect(this.widthScreen * 0.8f, 0, this.widthScreen * 0.2f, this.heightScreen * 0.1f);
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



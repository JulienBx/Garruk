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
		this.bottomZoneRect = new Rect(0, this.heightScreen * 0.86f, this.widthScreen * 0.2f, this.heightScreen * 0.14f);
		this.topZoneRect = new Rect(this.widthScreen * 0.8f, 0, this.widthScreen * 0.2f, this.heightScreen * 0.14f);
	}
}



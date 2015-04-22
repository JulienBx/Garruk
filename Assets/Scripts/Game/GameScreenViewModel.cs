using UnityEngine;

public class GameScreenViewModel
{
	//styles

	//informations à afficher
	public int heightScreen;
	public int widthScreen;

	public Rect bottomZoneRect ;
	public Rect topZoneRect ;

	public GameScreenViewModel ()
	{

	}

	public void recalculate ()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		this.bottomZoneRect = new Rect(0, this.heightScreen*0.9f, this.widthScreen, this.heightScreen*0.1f) ;
		this.topZoneRect = new Rect(0, 0, this.widthScreen, this.heightScreen*0.1f);
	}
}



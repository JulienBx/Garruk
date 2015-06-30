using UnityEngine;
using System.Collections.Generic;

public class GameViewModel
{
	public int heightScreen;
	public int widthScreen;

	public Texture2D cursor ;
	
	public bool toDisplayStartWindows = true ;
	public bool haveIStarted = false;
	public bool hasHeStarted = false ;
	
	public string messageStartWindow ;
	public string messageStartWindowButton ;
	public string messageOpponentStartWindow ;
	public string messageOpponentStartWindowButton ;
	
	public Rect startButtonRect ;
	public Rect opponentStartButtonRect ;
	public GUIStyle startWindowStyle ;
	public GUIStyle startWindowTextStyle ;
	public GUIStyle startButtonTextStyle ;
	public GUIStyle opponentStartWindowStyle ;
	public GUIStyle opponentStartWindowTextStyle ;
	
	public Rect quitButtonRect ;
	public GUIStyle quitButtonStyle ;
	public string quitButtonText ;

	public float timer ;
	public int timerSeconds ;
	public Rect timerRect ;
	public GUIStyle timerStyle ;
	
	public GameViewModel()
	{
		this.timer = -1;
	}

	public void setStyles(GUIStyle[] gameStyles)
	{
		
	}

	public void resize(int w, int h)
	{
		
	}
}



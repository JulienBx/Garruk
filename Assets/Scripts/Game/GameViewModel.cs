using UnityEngine;
using System.Collections.Generic;

public class GameViewModel
{
	public int heightScreen;
	public int widthScreen;

	public Texture2D cursor;
	
	public bool toDisplayStartWindows = true;
	public bool haveIStarted = false;
	public bool hasHeStarted = false;
	
	public string messageStartWindow = "";
	public string messageStartWindowButton = "";
	public string messageOpponentStartWindow = "";
	
	public Rect startButtonRect;
	public Rect opponentStartButtonRect;
	public GUIStyle startWindowStyle;
	public GUIStyle startWindowTextStyle;
	public GUIStyle startButtonTextStyle;
	public GUIStyle opponentStartWindowStyle;
	public GUIStyle opponentStartWindowTextStyle;
	
	public Rect topCenterRect;
	public GUIStyle quitButtonStyle;

	public float timer;
	public int timerSeconds;
	public Rect timerRect;
	public GUIStyle timerStyle;
	
	public string myPlayerName ; 
	public string hisPlayerName ;
	
	public GameViewModel()
	{
		this.timer = -1;
		this.timerStyle = new GUIStyle();
		this.quitButtonStyle = new GUIStyle();
		this.startWindowStyle = new GUIStyle();
		this.startWindowTextStyle = new GUIStyle();
		this.startButtonTextStyle = new GUIStyle();
		this.opponentStartWindowStyle = new GUIStyle();
		this.opponentStartWindowTextStyle = new GUIStyle();
	}

	public void setStyles(GUISkin mainSkin, GUISkin titlesSkin, GUISkin gameSkin)
	{
		this.quitButtonStyle = new GUIStyle(gameSkin.customStyles[0]);
		this.timerStyle = titlesSkin.label;
		this.timerStyle.fontStyle=FontStyle.Italic;
	}
	
	public void setTexts(string[] gameTexts)
	{
		
	}

	public void resize(int w, int h)
	{
		this.topCenterRect = new Rect((w/2f)-0.1f*h,0,0.2f*h,0.1f*h);
		this.timerStyle.fontSize = h*60/1000;
	}
}



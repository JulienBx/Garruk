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
	
	public float myLifePercentage = 100 ; 
	public float hisLifePercentage = 100 ;
	
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
		float realwidth = 10f*w/h;
		
		GameObject llbl = GameObject.Find("LLBLeft");
		llbl.transform.position = new Vector3(-realwidth/2f+0.25f, 4.5f, 0);
		
		GameObject llbr = GameObject.Find("LLBRight");
		llbr.transform.position = new Vector3(-1.2f, 4.5f, 0);
		
		GameObject llbc = GameObject.Find("LLBCenter");
		llbc.transform.position = new Vector3((llbl.transform.position.x+llbr.transform.position.x)/2f, 4.5f, 0);
		llbc.transform.localScale = new Vector3((-llbl.transform.position.x+llbr.transform.position.x-0.48f)*54f, 0.5f,0);
		
		GameObject leb = GameObject.Find("LLBRightEnd");
		leb.transform.position = new Vector3(-1.2f, 4.5f, 0);
		
		GameObject lcb = GameObject.Find("LLBLeftEnd");
		lcb.transform.position = new Vector3(llbl.transform.position.x+0.2f, 4.5f, 0);
		
		GameObject llbb = GameObject.Find("LLBBar");
		llbb.transform.position = new Vector3((leb.transform.position.x+lcb.transform.position.x)/2f, 4.5f, 0);
		llbb.transform.localScale = new Vector3((leb.transform.position.x-lcb.transform.position.x-0.48f)*54f, 0.5f,0);
		
		GameObject rlbl = GameObject.Find("RLBLeft");
		rlbl.transform.position = new Vector3(1.2f, 4.5f, 0);
		
		GameObject rlbr = GameObject.Find("RLBRight");
		rlbr.transform.position = new Vector3(realwidth/2f-0.25f, 4.5f, 0);
		
		GameObject rlbc = GameObject.Find("RLBCenter");
		rlbc.transform.position = new Vector3((rlbl.transform.position.x+rlbr.transform.position.x)/2f, 4.5f, 0);
		rlbc.transform.localScale = new Vector3((-rlbl.transform.position.x+rlbr.transform.position.x-0.48f)*54f, 0.5f,0);
		
		GameObject reb = GameObject.Find("RLBRightEnd");
		reb.transform.position = new Vector3(rlbr.transform.position.x-0.2f, 4.5f, 0);
		
		GameObject rcb = GameObject.Find("RLBLeftEnd");
		rcb.transform.position = new Vector3(1.2f, 4.5f, 0);
		
		GameObject rlbb = GameObject.Find("RLBBar");
		rlbb.transform.position = new Vector3((reb.transform.position.x+rcb.transform.position.x)/2f, 4.5f, 0);
		rlbb.transform.localScale = new Vector3((reb.transform.position.x-rcb.transform.position.x-0.48f)*54f, 0.5f,0);		
		
		this.topCenterRect = new Rect((w/2f)-0.1f*h,0,0.2f*h,0.1f*h);
		this.timerStyle.fontSize = h*60/1000;
	}
	
	public void setMyLife(){
	
	}
}



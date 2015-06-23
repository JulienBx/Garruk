using System;
using UnityEngine;

public class AdminBoardViewModel
{
	
	public GUIStyle[] styles;
	public GUIStyle titleStyle;
	public GUIStyle valueStyle;
	public GUIStyle textStyle;
	public GUIStyle buttonStyle;
	public GUIStyle textfieldStyle;
	public GUIStyle errorStyle;
	public GUIStyle toggleStyle;
	public int heightScreen;
	public int widthScreen;
	public string connectionsToday;
	public string playersToday;
	public string cardsRenamedToday;
	public string packBoughtToday;
	public string xpBoughtToday;
	public string cardBoughtToday;
	public string cardSoldToday;
	public string connectionsOnPeriod;
	public string playersOnPeriod;
	public string cardsRenamedOnPeriod;
	public string packBoughtOnPeriod;
	public string xpBoughtOnPeriod;
	public string cardBoughtOnPeriod;
	public string cardSoldOnPeriod;
	public string startPeriod;
	public string endPeriod;
	public string error;
	public bool filteredStats;

	public AdminBoardViewModel ()
	{
		this.styles=new GUIStyle[0];	
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		this.titleStyle = new GUIStyle ();
		this.valueStyle = new GUIStyle ();
		this.textStyle = new GUIStyle ();
		this.buttonStyle = new GUIStyle ();
		this.textfieldStyle = new GUIStyle ();
		this.errorStyle = new GUIStyle ();
		this.toggleStyle = new GUIStyle ();
		this.connectionsToday="";
		this.playersToday = "";
		this.cardsRenamedToday="";
		this.packBoughtToday="";
		this.xpBoughtToday="";
		this.cardBoughtToday="";
		this.cardSoldToday = "";
		this.connectionsOnPeriod="";
		this.playersOnPeriod = "";
		this.cardsRenamedOnPeriod="";
		this.packBoughtOnPeriod="";
		this.xpBoughtOnPeriod="";
		this.cardBoughtOnPeriod="";
		this.cardSoldOnPeriod = "";
		this.startPeriod = "";
		this.endPeriod = "";
		this.error = "";
		this.filteredStats = true;
	}
	public void initStyles()
	{
		this.titleStyle = this.styles [0];
		this.valueStyle = this.styles [1];
		this.textStyle = this.styles [2];
		this.textfieldStyle = this.styles [3];
		this.buttonStyle = this.styles [4];
		this.errorStyle = this.styles [5];
		this.toggleStyle = this.styles [6];
	}
	public void resize()
	{
		heightScreen=Screen.height;
		widthScreen=Screen.width;
		this.titleStyle.fontSize = this.heightScreen * 3 / 100;
		this.valueStyle.fontSize = this.heightScreen * 4 / 100;
		this.textStyle.fontSize = this.heightScreen * 2 / 100;
		this.textfieldStyle.fontSize = this.heightScreen * 2 / 100;
		this.buttonStyle.fontSize = this.heightScreen * 2 / 100;
		this.errorStyle.fontSize = this.heightScreen * 2 / 100;
	}
}


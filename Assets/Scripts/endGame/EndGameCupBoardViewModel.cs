using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class EndGameCupBoardViewModel 
{
	public IList<GUIStyle> roundsStyle;
	public bool winCup;
	public bool stillInCup;
	public bool endCup;
	public IList<string> roundsName;
	public string name;
	public int nbRounds;
	public int cupPrize;

	public GUIStyle[] styles;
	public GUIStyle cupLabelStyle;
	public GUIStyle winRoundStyle;
	public GUIStyle looseRoundStyle;
	public GUIStyle notPlayedRoundStyle;

	public EndGameCupBoardViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.roundsName = new List<string> ();
		this.roundsStyle = new List<GUIStyle> ();
		this.cupLabelStyle = new GUIStyle ();
		this.winRoundStyle = new GUIStyle ();
		this.notPlayedRoundStyle = new GUIStyle ();
		this.looseRoundStyle = new GUIStyle ();
		this.name = "";
	}
	public void initStyles()
	{
		this.cupLabelStyle = this.styles [0];
		this.winRoundStyle = this.styles [1];
		this.notPlayedRoundStyle = this.styles [2];
		this.looseRoundStyle = this.styles [3];
	}
	public void resize(int heightScreen)
	{
		this.cupLabelStyle.fontSize = heightScreen * 3 / 100;
	}
}

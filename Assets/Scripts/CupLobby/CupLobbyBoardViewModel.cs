using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class CupLobbyBoardViewModel
{
	public GUIStyle[] styles;
	public IList<GUIStyle> roundsStyle=new List<GUIStyle>();
	public IList<string> roundsName;
	public string name;
	public int nbRounds;

	
	public GUIStyle cupLabelStyle;
	public GUIStyle winRoundStyle;
	public GUIStyle notPlayedRoundStyle;
	
	public CupLobbyBoardViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.roundsName = new List<string> ();
		this.roundsStyle = new List<GUIStyle> ();
		this.cupLabelStyle = new GUIStyle ();
		this.winRoundStyle = new GUIStyle ();
		this.notPlayedRoundStyle = new GUIStyle ();
		this.name = "";
	}
	public void initStyles()
	{
		this.cupLabelStyle = this.styles [0];
		this.winRoundStyle = this.styles [1];
		this.notPlayedRoundStyle = this.styles [2];
	}
	public void resize(int heightScreen)
	{
		this.cupLabelStyle.fontSize = heightScreen * 3 / 100;
	}
}
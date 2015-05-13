using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class CupLobbyOpponentViewModel
{
	
	public GUIStyle[] styles;
	public GUIStyle backgroundStyle;
	public GUIStyle profilePictureStyle;
	public GUIStyle usernameStyle;
	public GUIStyle informationsStyle;
	public string username;
	public int totalNbWins;
	public int totalNbLooses;
	public int division;
	public int ranking;
	public int rankingPoints;
	
	public CupLobbyOpponentViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.backgroundStyle=new GUIStyle();
		this.profilePictureStyle=new GUIStyle();
		this.usernameStyle = new GUIStyle();
		this.informationsStyle = new GUIStyle();
		this.username = "";
	}
	public void initStyles()
	{
		this.backgroundStyle=this.styles[0];
		this.profilePictureStyle=this.styles[1];
		this.usernameStyle = this.styles[2];
		this.informationsStyle = this.styles[3];
	}
	public void resize(int heightScreen)
	{
		this.usernameStyle.fontSize = heightScreen * 3 / 100;
		this.informationsStyle.fontSize = heightScreen * 2 / 100;
	}
}
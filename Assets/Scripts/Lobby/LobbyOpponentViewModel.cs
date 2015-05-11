using System;
using UnityEngine;
public class LobbyOpponentViewModel
{
	public GUIStyle[] styles;
	public string username;
	public int totalNbWins;
	public int totalNbLooses;
	public int ranking;
	public int rankingPoints;
	public int currentDivision;
	public GUIStyle profilePictureButtonStyle;
	public GUIStyle usernameStyle;
	public GUIStyle informationsStyle;
	public GUIStyle backgroundStyle;
	
	public LobbyOpponentViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.username = "";
		this.profilePictureButtonStyle = new GUIStyle ();
		this.usernameStyle = new GUIStyle ();
		this.informationsStyle = new GUIStyle ();
		this.backgroundStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.profilePictureButtonStyle = this.styles [0];
		this.usernameStyle = this.styles [1];
		this.informationsStyle = this.styles [2];
		this.backgroundStyle = this.styles [3];
	}
	public void resize(int heightScreen)
	{
		this.usernameStyle.fontSize = heightScreen * 3 / 100;
		this.informationsStyle.fontSize = heightScreen * 2 / 100;
	}
}


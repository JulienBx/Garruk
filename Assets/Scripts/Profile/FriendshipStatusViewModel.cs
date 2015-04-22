using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class FriendshipStatusViewModel {

	public int status;
	public GUIStyle[] styles;
	public GUIStyle buttonStyle;
	public GUIStyle situationStyle;
	public string username;
	public int indexConnection;

	public FriendshipStatusViewModel ()
	{

	}
	public void initStyles()
	{
		this.buttonStyle = styles [0];
		this.situationStyle = styles [1];
	}
	public void resize(int heightScreen)
	{
		this.buttonStyle.fontSize = heightScreen * 2 / 100;
	}
}

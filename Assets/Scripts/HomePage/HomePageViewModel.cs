using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageViewModel {

	public GUIStyle[] styles;
	public GUIStyle paginationStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle titleStyle;
	public GUIStyle labelNoStyle;
	public GUIStyle profileBackgroundStyle;
	public GUIStyle profileUsernameStyle;
	public GUIStyle profileInformationsStyle;
	public bool guiEnabled;

	
	public HomePageViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.paginationStyle = new GUIStyle ();
		this.paginationActivatedStyle = new GUIStyle ();
		this.titleStyle = new GUIStyle ();
		this.labelNoStyle = new GUIStyle ();
		this.profileBackgroundStyle = new GUIStyle ();
		this.profileUsernameStyle = new GUIStyle ();
		this.profileInformationsStyle = new GUIStyle ();
		this.guiEnabled = true;
	}
	public void initStyles()
	{	
		this.paginationStyle = this.styles [0];
		this.paginationActivatedStyle = this.styles [1];
		this.titleStyle = this.styles [2];
		this.labelNoStyle = this.styles [3];
		this.profileBackgroundStyle = this.styles [4];
		this.profileUsernameStyle = this.styles [5];
		this.profileInformationsStyle = this.styles [6];
	}
	public void resize(int heightScreen)
	{	
		this.paginationStyle.fontSize = heightScreen * 2 / 100;
		this.paginationActivatedStyle.fontSize = heightScreen * 2 / 100;
		this.profileUsernameStyle.fontSize = heightScreen * 2 / 100;
		this.profileInformationsStyle.fontSize = heightScreen * 15 / 1000;
		this.labelNoStyle.fontSize = heightScreen * 2 / 100;
		this.titleStyle.fontSize = heightScreen * 25 / 1000;

	}
}
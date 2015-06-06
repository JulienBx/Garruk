using System;
using UnityEngine;

public class TutorialViewModel
{
	public GUIStyle[] styles;
	public GUIStyle blockBakgroundStyle;
	public GUIStyle titleStyle;
	public GUIStyle descriptionStyle;
	public GUIStyle buttonStyle;
	public GUIStyle mainPictureStyle;
	public string title = "";
	public string description ="";
	public int selectedPage;
	public bool guiEnabled;
	
	public TutorialViewModel ()
	{
		this.styles = new GUIStyle[0];
		this.blockBakgroundStyle = new GUIStyle ();
		this.titleStyle = new GUIStyle ();
		this.descriptionStyle = new GUIStyle ();
		this.buttonStyle = new GUIStyle ();
		this.mainPictureStyle = new GUIStyle ();
		this.selectedPage = 0;
		this.guiEnabled = true;
	}
	public void initStyles()
	{
		this.blockBakgroundStyle = this.styles [0];
		this.titleStyle = this.styles [1];
		this.descriptionStyle = this.styles [2];
		this.buttonStyle = this.styles [3];
		this.mainPictureStyle = this.styles [4];
	}
	public void resize(int heightScreen)
	{
		this.titleStyle.fontSize = heightScreen * 3 / 100;
		this.descriptionStyle.fontSize = heightScreen * 2 / 100;
		this.buttonStyle.fontSize = heightScreen * 2 / 100;
	}
}


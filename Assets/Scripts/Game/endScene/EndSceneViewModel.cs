using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
public class EndSceneViewModel
{
	public int credits;
	public int startCredits;
	public int endCredits;
	public int creditsToAdd;
	public int collectionPoints;
	public int collectionPointsRanking;
	public string title;
	public IList<string> newSkills;
	public bool guiEnabled;
	public GUIStyle[] styles;
	public GUIStyle titleStyle;
	public GUIStyle creditStyle;
	public GUIStyle labelStyle;
	public GUIStyle buttonStyle;
	public GUIStyle newSkillsStyle;
	public string newCardType;

	public EndSceneViewModel ()
	{
		this.titleStyle = new GUIStyle ();
		this.creditStyle = new GUIStyle ();
		this.labelStyle = new GUIStyle ();
		this.buttonStyle = new GUIStyle ();
		this.newSkillsStyle = new GUIStyle ();
		this.guiEnabled = false;
		this.title = "";
		this.newCardType = "";
		this.newSkills = new List<string> ();
	}
	public void initStyles()
	{
		this.titleStyle = this.styles [0];
		this.creditStyle = this.styles [1];
		this.labelStyle = this.styles [2];
		this.buttonStyle = this.styles [3];
		this.newSkillsStyle = this.styles [4];
	}
	public void resize(int heightScreen)
	{
		this.titleStyle.fontSize = heightScreen * 4 / 100;
		this.creditStyle.fontSize = heightScreen * 3 / 100;
		this.labelStyle.fontSize = heightScreen * 3 / 100;
		this.buttonStyle.fontSize = heightScreen * 3 / 100;
		this.newSkillsStyle.fontSize = heightScreen * 2 / 100;
	}
}
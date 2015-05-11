using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class LobbyDecksViewModel
{
	public IList<GUIStyle> myDecksButtonGuiStyle;
	public IList<int> decksToBeDisplayed;
	public IList<string> decksName;
	public string decksTitle;
	public int chosenDeck;
	public Vector2 scrollPosition;
	
	public GUIStyle[] styles;
	public GUIStyle decksTitleStyle;
	public GUIStyle deckButtonStyle;
	public GUIStyle deckButtonChosenStyle;
	
	
	public LobbyDecksViewModel()
	{
		this.chosenDeck = 0;
		this.styles=new GUIStyle[0];
		this.decksToBeDisplayed = new List<int>();
		this.decksName=new List<string>();
		this.scrollPosition= new Vector2(0, 0);
		this.decksTitle = "Sélectionnez votre deck";
		this.decksTitleStyle=new GUIStyle();
		this.deckButtonStyle=new GUIStyle();
		this.deckButtonChosenStyle=new GUIStyle();
		this.myDecksButtonGuiStyle = new List<GUIStyle> ();
	}
	
	public void initStyles()
	{
		this.decksTitleStyle = this.styles [0];
		this.deckButtonStyle = this.styles [1];
		this.deckButtonChosenStyle = this.styles [2];
	}
	public void resize(int heightScreen)
	{
		this.decksTitleStyle.fontSize = heightScreen * 2 / 100;
		this.deckButtonStyle.fontSize = heightScreen * 2 / 100;
		this.deckButtonChosenStyle.fontSize = heightScreen * 2 / 100;
	}
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MyGameDecksViewModel
{
	public GUIStyle[] myDecksGuiStyle;
	public GUIStyle[] myDecksButtonGuiStyle;
	public IList<Deck> myDecks;
	public Rect rectDeck;
	public Rect rectFocus;
	public Rect rectInsideScrollDeck;
	public Rect rectOutsideScrollDeck;
	public string decksTitle;
	public string myNewDeckButtonTitle;
	public string newTitle;
	public float scaleDeck;
	public int chosenDeck;
	public int chosenIdDeck;
	public Vector2 scrollPosition;

	public GUIStyle[] styles;
	public GUIStyle decksTitleStyle;
	public GUIStyle deckStyle;
	public GUIStyle deckChosenStyle;
	public GUIStyle deckButtonStyle;
	public GUIStyle deckButtonChosenStyle;
	public GUIStyle mySuppressButtonStyle;
	public GUIStyle myEditButtonStyle;
	public GUIStyle myNewDeckButtonStyle;
	

	public MyGameDecksViewModel()
	{
		this.chosenDeck = 0;
		this.chosenIdDeck = -1;
		this.myDecks = new List<Deck> ();
		this.scrollPosition= new Vector2(0, 0);
		this.decksTitle = "Mes decks";
		this.myNewDeckButtonTitle = "Nouveau";
		this.decksTitleStyle=new GUIStyle();
		this.deckStyle=new GUIStyle();
		this.deckChosenStyle=new GUIStyle();
		this.deckButtonStyle=new GUIStyle();
		this.deckButtonChosenStyle=new GUIStyle();
		this.mySuppressButtonStyle=new GUIStyle();
		this.myEditButtonStyle=new GUIStyle();
		this.myNewDeckButtonStyle=new GUIStyle();
	}

	public void initStyles()
	{
		this.decksTitleStyle = this.styles [0];
		this.deckStyle = this.styles [1];
		this.deckChosenStyle = this.styles [2];
		this.deckButtonStyle = this.styles [3];
		this.deckButtonChosenStyle = this.styles [4];
		this.mySuppressButtonStyle = this.styles [5];
		this.myEditButtonStyle = this.styles [6];
		this.myNewDeckButtonStyle = this.styles [7];
	}
	public void resize(int heightScreen)
	{
		this.decksTitleStyle.fontSize = heightScreen * 2 / 100;
		this.deckButtonStyle.fontSize = heightScreen * 2 / 100;
		this.deckButtonChosenStyle.fontSize = heightScreen * 2 / 100;
		this.myNewDeckButtonStyle.fontSize = heightScreen * 2 / 100;
	}
}

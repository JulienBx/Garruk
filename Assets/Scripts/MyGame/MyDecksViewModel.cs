using UnityEngine;
using System.Collections;

public class MyDecksViewModel
{
	public GUIStyle[] myDecksGuiStyle;
	public GUIStyle[] myDecksButtonGuiStyle;
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

	public GUIStyle[] styles;
	public GUIStyle decksTitleStyle;
	public GUIStyle deckStyle;
	public GUIStyle deckChosenStyle;
	public GUIStyle deckButtonStyle;
	public GUIStyle deckButtonChosenStyle;
	public GUIStyle mySuppressButtonStyle;
	public GUIStyle myEditButtonStyle;
	public GUIStyle myNewDeckButton;

	public Texture2D[] textures;
	public Texture2D backNewDeckButton;
	public Texture2D backHoveredNewDeckButton;

	public MyDecksViewModel()
	{
		chosenDeck = 0;
		chosenIdDeck = -1;
	}

	public void initStyles()
	{
		decksTitleStyle = styles [0];
		deckStyle = styles [1];
		deckChosenStyle = styles [2];
		deckButtonStyle = styles [3];
		deckButtonChosenStyle = styles [4];
		mySuppressButtonStyle = styles [5];
		myEditButtonStyle = styles [6];
		myNewDeckButton = styles [7];
	}

	public void initTextures()
	{
		backNewDeckButton = textures [0];
		backHoveredNewDeckButton = textures [1];
	}
}

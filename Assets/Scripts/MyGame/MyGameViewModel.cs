using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyGameViewModel
{
	public GUIStyle monLoaderStyle;

	public IList<Deck> myDecks;
	public IList<Card> cards;
	public IList<int> cardsToBeDisplayed;
	public IList<int> cardsIds;
	public IList<int> deckCardsIds;

	public GameObject[] displayedCards;
	public GameObject[] displayedDeckCards;
	public string[] skillsList;
	public string[] cardTypeList;
	public bool[] togglesCurrentStates;

	public GameObject cardFocused;
	public bool areDecksRetrieved;
	public bool isLoadedCards;
	public bool isLoadedDeck;
	public bool soldCard;
	public bool toReloadAll;
	public bool areCreatedDeckCards;
	public bool toReload;
	public bool destroyAll;
	public bool displayDecks;
	public bool isCreatedDeckCards;
	public bool isCreatedCards;
	public bool displayLoader;
	public bool isUpEscape;
	public bool isDisplayedCards;
	public bool isBeingDragged;
	public bool confirmSuppress;
	public bool isMarketed;
	public int idFocused;
	public int nbCardsPerRow;
	public int cardId;
	public string textMarket;
	public string tempPrice; 

	public Texture2D[] textures;
	public Texture2D backButton;
	public Texture2D backActivatedButton;

	public MyGameViewModel()
	{
		cards = new List<Card>();
		areDecksRetrieved = false;
		isLoadedCards = false;
		isLoadedDeck = false;
		soldCard = false;
		toReloadAll = false;
		areCreatedDeckCards = false; 
		toReload = false;
		destroyAll = false;
		displayDecks = false;
		isCreatedDeckCards = false;
		isCreatedCards = false;
		isDisplayedCards = true;
		isBeingDragged = false;
		nbCardsPerRow = 1;
	}

	public void initTextures()
	{
		backButton = textures [0];
		backActivatedButton = textures [1];
	}
}

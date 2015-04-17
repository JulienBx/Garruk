using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class myGameScript : MonoBehaviour 
{
	public static myGameScript instance;
	public MyGameView myGameView;

	private User userModel;
	public GameObject MenuObject;

	void Start() 
	{
		userModel = new User(ApplicationModel.username);
		instance = this;
		myGameView = Camera.main.gameObject.GetComponent<MyGameView>();
		myGameView.setStyles(); 
		MenuObject = Instantiate(MenuObject) as GameObject;
		myGameView.toReloadAll = true;
	}
		
	public IEnumerator addDeck(string decksName) 
	{
		yield return StartCoroutine(Deck.create(decksName));
		StartCoroutine(retrieveDecks());
	}

	public IEnumerator deleteDeck(int deckId) 
	{
		myGameView.areDecksRetrieved = false;
		Deck deck = new Deck(deckId);

		yield return StartCoroutine(deck.delete());
		StartCoroutine(retrieveDecks()); 
	}

	public IEnumerator editDeck(int deckToEdit, string decksName)
	{
		Deck deck = new Deck(deckToEdit);

		yield return StartCoroutine(deck.edit(decksName));
		StartCoroutine(retrieveDecks()); 
	}

	public IEnumerator retrieveDecks() 
	{
		myGameView.myDecks = new List<Deck>();
		yield return StartCoroutine(userModel.getDecks(parseDecks));

		myGameView.areDecksRetrieved = true;
	}

	public void getCards()
	{
		myGameView.cardsToBeDisplayed = new List<int>();
		
		StartCoroutine(userModel.getCards(parseCards));
	}

	public IEnumerator retrieveCardsFromDeck(int idDeck)
	{
		Deck deck = new Deck(idDeck);

		yield return StartCoroutine(deck.retrieveCards(parseCardsFromDeck));
		
		myGameView.isLoadedDeck = true;
	}
	
	public void AddCardToDeck(int idDeck, int idCard)
	{		
		Deck deck = new Deck(idDeck);

		StartCoroutine(deck.addCard(idCard));
	}
	
	public void RemoveCardFromDeck(int idDeck, int idCard)
	{		
		Deck deck = new Deck(idDeck);
		StartCoroutine(deck.removeCard(idCard));
	}
	
	public IEnumerator sellCard(int idCard, int cost)
	{	
		ApplicationModel.credits += cost;
		Card card = new Card(idCard);
		yield return StartCoroutine(card.sellCard(cost));
		
		myGameView.soldCard = true ;
	}
	
	public void putOnMarket(int idCard, int price)
	{
		myGameView.cards[myGameView.idFocused].onSale = 1;
		myGameView.cards[myGameView.idFocused].Price = price;
		Card card = new Card(idCard);
		
		StartCoroutine(card.toSell(price));
	}
	
	public void removeFromMarket(int idCard)
	{	
		myGameView.cards[myGameView.idFocused].onSale = 0;
		Card card = new Card(idCard);

		StartCoroutine(card.notToSell());
	}
	
	public void changeMarketPrice(int idCard, int price)
	{	
		myGameView.cards[myGameView.idFocused].Price = price;
		Card card = new Card(idCard);

		StartCoroutine(card.changePriceCard(price));
	}
	
	public void renameCard(int idCard, string newTitle, int renameCost)
	{
		myGameView.newTitle = myGameView.newTitle.Replace("\n", "");
		myGameView.newTitle = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(myGameView.newTitle.ToLower());
		Card card = new Card(idCard);

		StartCoroutine(card.renameCard(newTitle, renameCost));
		
		ApplicationModel.credits -= renameCost;
		myGameView.cards[myGameView.idFocused].Title = myGameView.newTitle;
		myGameView.cardFocused.GetComponent<GameCard>().Card.Title = myGameView.newTitle;
		myGameView.cardFocused.GetComponent<GameCard>().ShowFace();
	}

	public void parseDecks(string text)
	{
		string[] decksInformation = text.Split('\n'); 
		string[] deckInformation;
		
		myGameView.myDecksGuiStyle = new GUIStyle[decksInformation.Length - 1];
		myGameView.myDecksButtonGuiStyle = new GUIStyle[decksInformation.Length - 1];
		
		for (int i = 0 ; i < decksInformation.Length - 1 ; i++) 		// On boucle sur les attributs d'un deck
		{
			if (i > 0)
			{
				myGameView.myDecksGuiStyle[i] = myGameView.deckStyle;
				myGameView.myDecksButtonGuiStyle[i] = myGameView.deckButtonStyle;
			}
			else
			{
				myGameView.myDecksGuiStyle[i] = myGameView.deckChosenStyle;
				myGameView.myDecksButtonGuiStyle[i] = myGameView.deckButtonChosenStyle;
			}
			deckInformation = decksInformation[i].Split('\\');
			myGameView.myDecks.Add(new Deck(System.Convert.ToInt32(deckInformation[0]), deckInformation[1], System.Convert.ToInt32(deckInformation[2])));
			
			if (i == 0)
			{
				myGameView.chosenIdDeck = System.Convert.ToInt32(deckInformation[0]);
				myGameView.chosenDeck = 0 ;
			}
		}
	}

	public void parseCards(string text)
	{
		string[] data = text.Split(new string[] { "END" }, System.StringSplitOptions.None);
		string[] cardsIDS = data[2].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
		string[] skillsIds = data[1].Split('\n');
		
		myGameView.cardTypeList = data[0].Split('\n');
		myGameView.togglesCurrentStates = new bool[myGameView.cardTypeList.Length];
		
		for(int i = 0 ; i < myGameView.cardTypeList.Length - 1 ; i++)
		{
			myGameView.togglesCurrentStates[i] = false;
		}
		myGameView.skillsList = new string[skillsIds.Length-1];
		
		string[] tempString;
		for(int i = 0 ; i < skillsIds.Length - 1 ; i++)
		{
			tempString = skillsIds[i].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
			if (i > 0)
			{
				myGameView.skillsList[i - 1] = tempString[0];
			}
		}
		
		myGameView.cards = new List<Card>();
		myGameView.cardsIds = new List<int>();
		
		string[] cardInfo;
		string[] cardInfo2;
		for (int i = 0 ; i < cardsIDS.Length - 1 ; i++)
		{
			cardInfo = cardsIDS[i].Split('\n');
			for (int j = 1 ; j < cardInfo.Length - 1 ; j++)
			{
				cardInfo2 = cardInfo[j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
				if (j == 1)
				{
					myGameView.cards.Add(
						new Card(System.Convert.ToInt32(cardInfo2[0]), // id
					         cardInfo2[1], // title
					         System.Convert.ToInt32(cardInfo2[2]), // life
					         System.Convert.ToInt32(cardInfo2[3]), // attack
					         System.Convert.ToInt32(cardInfo2[4]), // speed
					         System.Convert.ToInt32(cardInfo2[5]), // move
					         System.Convert.ToInt32(cardInfo2[6]), // artindex
					         System.Convert.ToInt32(cardInfo2[7]), // idclass
					         myGameView.cardTypeList[System.Convert.ToInt32(cardInfo2[7])], // titleclass
					         System.Convert.ToInt32(cardInfo2[8]), // lifelevel
					         System.Convert.ToInt32(cardInfo2[9]), // movelevel
					         System.Convert.ToInt32(cardInfo2[10]),
					         System.Convert.ToInt32(cardInfo2[11]),
					         System.Convert.ToInt32(cardInfo2[12]),
					         System.Convert.ToInt32(cardInfo2[13]),
					         System.Convert.ToInt32(cardInfo2[14]),
					         System.Convert.ToInt32(cardInfo2[15]),
					         System.Convert.ToInt32(cardInfo2[16]))); 
					
					myGameView.cards[i].Skills = new List<Skill>();
					myGameView.cardsIds.Add(System.Convert.ToInt32(cardInfo2[0]));
					myGameView.cardsToBeDisplayed.Add(i);
				}
				else
				{
					myGameView.cards[i].Skills.Add(
						new Skill (myGameView.skillsList[System.Convert.ToInt32(cardInfo2[0])], 
					           System.Convert.ToInt32(cardInfo2[0]),
					           System.Convert.ToInt32(cardInfo2[1]),
					           System.Convert.ToInt32(cardInfo2[2]),
					           System.Convert.ToInt32(cardInfo2[3]),
					           System.Convert.ToInt32(cardInfo2[4]),
					           cardInfo2[5]));
				}
			}
		}
	
		myGameView.isLoadedCards = true;
		myGameView.isDisplayedCards = true;
	}

	public void parseCardsFromDeck(string text)
	{
		int tempInt;
		int tempInt2 = myGameView.cards.Count;
		bool tempBool;
		int j = 0;
		string[] cardDeckEntries =  text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
		myGameView.deckCardsIds = new List<int>();
		for(int i = 0 ; i < cardDeckEntries.Length - 1 ; i++)
		{
			tempInt = System.Convert.ToInt32(cardDeckEntries[i]);
			tempBool = true; 
			j = 0 ;
			while (tempBool && j < tempInt2)
			{
				if (myGameView.cards[j].Id == tempInt)
				{
					tempBool = false;
				}
				j++;
			}
			j--;
			myGameView.deckCardsIds.Add(j);
		}
	}
}

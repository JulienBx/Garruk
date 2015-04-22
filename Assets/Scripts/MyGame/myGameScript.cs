using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class myGameScript : MonoBehaviour 
{
	public static myGameScript instance;
	public MyGameView myGameView;
	public GameObject MenuObject;
	public GameObject CardObject;
	private User userModel;

	public GUIStyle[] FilterVMStyle;
	public GUIStyle[] PopUpVMStyle;
	public GUIStyle[] MydecksVMStyle;
	public GUIStyle[] SortVMStyle;
	public GUIStyle[] PaginationVMStyle;
	public GUIStyle[] FocusVMStyle;
	public Texture2D[] MyDecksVMTexture;
	public Texture2D[] MyGameVMTexture;

	void Start() 
	{
		instance                            = this;
		myGameView                          = Camera.main.gameObject.AddComponent<MyGameView>();
		initViewModels();
		initStyles();
		userModel                           = new User(ApplicationModel.username);
		myGameView.myGameVM.toReloadAll     = true;
	}
	private void initViewModels()
	{
		myGameView.filterVM                            = new FilterViewModel();
		myGameView.popupVM                             = new MyGamePopUpViewModel();
		myGameView.myDecksVM                           = new MyDecksViewModel();
		myGameView.sortVM                              = new SortViewModel();
		myGameView.focusVM                             = new FocusViewModel();
		myGameView.paginationVM                        = new PaginationViewModel();
		myGameView.myGameVM                            = new MyGameViewModel();
	}

	private void initStyles()
	{
		myGameView.filterVM.styles = new GUIStyle[FilterVMStyle.Length];
		for(int i = 0 ; i < FilterVMStyle.Length ; i++)
		{
			myGameView.filterVM.styles[i] = FilterVMStyle[i];
		}
		myGameView.filterVM.initStyles();

		myGameView.popupVM.styles = new GUIStyle[PopUpVMStyle.Length];
		for(int i = 0 ; i < PopUpVMStyle.Length ; i++)
		{
			myGameView.popupVM.styles[i] = PopUpVMStyle[i];
		}
		myGameView.popupVM.initStyles();

		myGameView.myDecksVM.styles = new GUIStyle[MydecksVMStyle.Length];
		for(int i = 0 ; i < MydecksVMStyle.Length ; i++)
		{
			myGameView.myDecksVM.styles[i] = MydecksVMStyle[i];
		}
		myGameView.myDecksVM.initStyles();

		myGameView.sortVM.styles = new GUIStyle[SortVMStyle.Length];
		for(int i = 0 ; i < SortVMStyle.Length ; i++)
		{
			myGameView.sortVM.styles[i] = SortVMStyle[i];
		}
		myGameView.sortVM.initStyles();

		myGameView.focusVM.styles = new GUIStyle[FocusVMStyle.Length];
		for(int i = 0 ; i < FocusVMStyle.Length ; i++)
		{
			myGameView.focusVM.styles[i] = FocusVMStyle[i];
		}
		myGameView.focusVM.initStyles();

		myGameView.paginationVM.styles = new GUIStyle[PaginationVMStyle.Length];
		for(int i = 0 ; i < PaginationVMStyle.Length ; i++)
		{
			myGameView.paginationVM.styles[i] = PaginationVMStyle[i];
		}
		myGameView.paginationVM.initStyles();

		myGameView.myDecksVM.textures = new Texture2D[MyDecksVMTexture.Length];
		for(int i = 0 ; i < MyDecksVMTexture.Length ; i++)
		{
			myGameView.myDecksVM.textures[i] = MyDecksVMTexture[i];
		}
		myGameView.myDecksVM.initTextures();

		myGameView.myGameVM.textures = new Texture2D[MyGameVMTexture.Length];
		for(int i = 0 ; i < MyGameVMTexture.Length ; i++)
		{
			myGameView.myGameVM.textures[i] = MyGameVMTexture[i];
		}
		myGameView.myGameVM.initTextures();
	}

	public IEnumerator addDeck(string decksName) 
	{
		yield return StartCoroutine(Deck.create(decksName));
		StartCoroutine(retrieveDecks());
	}

	public IEnumerator deleteDeck(int deckId) 
	{
		myGameView.myGameVM.areDecksRetrieved = false;
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
		myGameView.myGameVM.myDecks = new List<Deck>();
		yield return StartCoroutine(userModel.getDecks(parseDecks));

		myGameView.myGameVM.areDecksRetrieved = true;
	}

	public void getCards()
	{
		myGameView.myGameVM.cardsToBeDisplayed = new List<int>();
		
		StartCoroutine(userModel.getCards(parseCards));
	}

	public IEnumerator retrieveCardsFromDeck(int idDeck)
	{
		Deck deck = new Deck(idDeck);

		yield return StartCoroutine(deck.retrieveCards(parseCardsFromDeck));

		myGameView.myGameVM.isLoadedDeck = true;
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

		myGameView.myGameVM.soldCard = true ;
	}
	
	public void putOnMarket(int idCard, int price)
	{
		myGameView.myGameVM.cards[myGameView.myGameVM.idFocused].onSale = 1;
		myGameView.myGameVM.cards[myGameView.myGameVM.idFocused].Price = price;
		Card card = new Card(idCard);
		
		StartCoroutine(card.toSell(price));
	}
	
	public void removeFromMarket(int idCard)
	{	
		myGameView.myGameVM.cards[myGameView.myGameVM.idFocused].onSale = 0;
		Card card = new Card(idCard);

		StartCoroutine(card.notToSell());
	}
	
	public void changeMarketPrice(int idCard, int price)
	{	
		myGameView.myGameVM.cards[myGameView.myGameVM.idFocused].Price = price;
		Card card = new Card(idCard);

		StartCoroutine(card.changePriceCard(price));
	}
	
	public void renameCard(int idCard, string newTitle, int renameCost)
	{

		myGameView.myDecksVM.newTitle = myGameView.myDecksVM.newTitle.Replace("\n", "");
		myGameView.myDecksVM.newTitle = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo
			.ToTitleCase(myGameView.myDecksVM.newTitle.ToLower());
		Card card = new Card(idCard);

		StartCoroutine(card.renameCard(newTitle, renameCost));
		
		ApplicationModel.credits -= renameCost;

		myGameView.myGameVM.cards[myGameView.myGameVM.idFocused].Title = myGameView.myDecksVM.newTitle;
		myGameView.GetComponent<GameCard>().Card.Title = myGameView.myDecksVM.newTitle;
		myGameView.myGameVM.cardFocused.GetComponent<GameCard>().ShowFace();
	}

	public void parseDecks(string text)
	{
		string[] decksInformation = text.Split('\n'); 
		string[] deckInformation;

		myGameView.myDecksVM.myDecksGuiStyle = new GUIStyle[decksInformation.Length - 1];
		myGameView.myDecksVM.myDecksButtonGuiStyle = new GUIStyle[decksInformation.Length - 1];
		
		for (int i = 0 ; i < decksInformation.Length - 1 ; i++) 		// On boucle sur les attributs d'un deck
		{
			if (i > 0)
			{
				myGameView.myDecksVM.myDecksGuiStyle[i] = myGameView.myDecksVM.deckStyle;
				myGameView.myDecksVM.myDecksButtonGuiStyle[i] = myGameView.myDecksVM.deckButtonStyle;
			}
			else
			{
				myGameView.myDecksVM.myDecksGuiStyle[i] = myGameView.myDecksVM.deckChosenStyle;
				myGameView.myDecksVM.myDecksButtonGuiStyle[i] = myGameView.myDecksVM.deckButtonChosenStyle;
			}
			deckInformation = decksInformation[i].Split('\\');
			myGameView.myGameVM.myDecks.Add(new Deck(System.Convert.ToInt32(deckInformation[0]), deckInformation[1], System.Convert.ToInt32(deckInformation[2])));
			
			if (i == 0)
			{
				myGameView.myDecksVM.chosenIdDeck = System.Convert.ToInt32(deckInformation[0]);
				myGameView.myDecksVM.chosenDeck = 0 ;
			}
		}
	}

	public void parseCards(string text)
	{
		string[] data           = text.Split(new string[] { "END" }, System.StringSplitOptions.None);
		string[] cardsIDS       = data[2].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
		string[] skillsIds      = data[1].Split('\n');
		
		myGameView.myGameVM.cardTypeList = data[0].Split('\n');
		myGameView.myGameVM.togglesCurrentStates = new bool[myGameView.myGameVM.cardTypeList.Length];
		
		for(int i = 0 ; i < myGameView.myGameVM.cardTypeList.Length - 1 ; i++)
		{
			myGameView.myGameVM.togglesCurrentStates[i] = false;
		}
		myGameView.myGameVM.skillsList = new string[skillsIds.Length-1];
		
		string[] tempString;
		for (int i = 0 ; i < skillsIds.Length - 1 ; i++)
		{
			tempString = skillsIds[i].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
			if (i > 0)
			{
				myGameView.myGameVM.skillsList[i - 1] = tempString[0];
			}
		}
		
		myGameView.myGameVM.cards = new List<Card>();
		myGameView.myGameVM.cardsIds = new List<int>();
		
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
					myGameView.myGameVM.cards.Add(
						new Card(System.Convert.ToInt32(cardInfo2[0]), // id
					         cardInfo2[1], // title
					         System.Convert.ToInt32(cardInfo2[2]), // life
					         System.Convert.ToInt32(cardInfo2[3]), // attack
					         System.Convert.ToInt32(cardInfo2[4]), // speed
					         System.Convert.ToInt32(cardInfo2[5]), // move
					         System.Convert.ToInt32(cardInfo2[6]), // artindex
					         System.Convert.ToInt32(cardInfo2[7]), // idclass
					         myGameView.myGameVM.cardTypeList[System.Convert.ToInt32(cardInfo2[7])], // titleclass
					         System.Convert.ToInt32(cardInfo2[8]), // lifelevel
					         System.Convert.ToInt32(cardInfo2[9]), // movelevel
					         System.Convert.ToInt32(cardInfo2[10]),
					         System.Convert.ToInt32(cardInfo2[11]),
					         System.Convert.ToInt32(cardInfo2[12]),
					         System.Convert.ToInt32(cardInfo2[13]),
					         System.Convert.ToInt32(cardInfo2[14]),
					         System.Convert.ToInt32(cardInfo2[15]),
					         System.Convert.ToInt32(cardInfo2[16]))); 
					
					myGameView.myGameVM.cards[i].Skills = new List<Skill>();
					myGameView.myGameVM.cardsIds.Add(System.Convert.ToInt32(cardInfo2[0]));
					myGameView.myGameVM.cardsToBeDisplayed.Add(i);
				}
				else
				{
					myGameView.myGameVM.cards[i].Skills.Add(
						new Skill (myGameView.myGameVM.skillsList[System.Convert.ToInt32(cardInfo2[0])], 
					           System.Convert.ToInt32(cardInfo2[0]),
					           System.Convert.ToInt32(cardInfo2[1]),
					           System.Convert.ToInt32(cardInfo2[2]),
					           System.Convert.ToInt32(cardInfo2[3]),
					           System.Convert.ToInt32(cardInfo2[4]),
					           cardInfo2[5]));
				}
			}
		}
	
		myGameView.myGameVM.isLoadedCards = true;
		myGameView.myGameVM.isDisplayedCards = true;
	}

	public void parseCardsFromDeck(string text)
	{
		int tempInt;
		int tempInt2 = myGameView.myGameVM.cards.Count;
		bool tempBool;
		int j = 0;
		string[] cardDeckEntries =  text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
		myGameView.myGameVM.deckCardsIds = new List<int>();
		for (int i = 0 ; i < cardDeckEntries.Length - 1 ; i++)
		{
			tempInt = System.Convert.ToInt32(cardDeckEntries[i]);
			tempBool = true; 
			j = 0 ;
			while (tempBool && j < tempInt2)
			{
				if (myGameView.myGameVM.cards[j].Id == tempInt)
				{
					tempBool = false;
				}
				j++;
			}
			j--;
			myGameView.myGameVM.deckCardsIds.Add(j);
		}
	}
}

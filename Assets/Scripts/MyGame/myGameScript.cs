using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class myGameScript : MonoBehaviour 
{
	public static myGameScript instance;
	public MyGameView myGameView;

	private Deck deckModel;
	private User userModel;

	#region variables

	//URL des fichiers PHP appelés par cette classe
	private string URLGetCardsByDeck = "http://54.77.118.214/GarrukServer/get_cards_by_deck.php";

	//La fonction pour charger les decks est-elle terminée ?

	//GUIStyle du titre de la zone de gestion des decks
	public GUIStyle decksTitleStyle ;
	string decksTitle ;
	//GUIStyle du bouton d'ajout de deck
	public GUIStyle myNewDeckButton ;
	//Texte du bouton d'ajout de deck
	private string myNewDeckButtonTitle ;
	//Images du bouton en mode smartphone
	public Texture2D backNewDeckButton ;
	public Texture2D backHoveredNewDeckButton ;
	//Images du bouton en mode normal
	public Texture2D backButton ;
	public Texture2D backActivatedButton ;
	//Le style et les dimensions de la pop up qui s'affiche au centre de l'écran
	public GUIStyle centralWindowStyle ;
	Rect centralWindow ;
	Rect centralFocus ;
	
	public GUIStyle centralWindowTitleStyle ;
	public GUIStyle centralWindowTextFieldStyle ;
	public GUIStyle centralWindowButtonStyle ;
	public GUIStyle smallCentralWindowButtonStyle ;
	public GUIStyle focusedWindowStyle ;
	public GUIStyle focusedWindowTitleStyle ;
	public GUIStyle focusedWindowButtonTitleStyle ;
	//public GUIStyle deckStyle ;
	//public GUIStyle deckChosenStyle ;
	//public GUIStyle deckButtonStyle ;
	//public GUIStyle deckButtonChosenStyle ;
	public GUIStyle mySuppressButtonStyle ;
	public GUIStyle myEditButtonStyle ;
	public GUIStyle paginationStyle ;
	public GUIStyle paginationActivatedStyle ;
	public GUIStyle filterTitleStyle ;
	public GUIStyle toggleStyle;
	public GUIStyle filterTextFieldStyle;
	public GUIStyle myStyle;
	public GUIStyle smallPoliceStyle;
	public GUIStyle focusButtonStyle;
	public GUIStyle cantBuyStyle;
	public GUIStyle sortDefaultButtonStyle;
	public GUIStyle sortActivatedButtonStyle;

	float scaleDeck ;



	public GameObject MenuObject;

	//Si l'utilisateur sélectionne une action (edit ou suppress) sur un des deck, donne à cette variable l'ID du deck en question
	//int IDDeckToEdit = -1;

	Rect rectDeck ;
	Rect rectFocus ;
	Rect rectInsideScrollDeck ;
	Rect rectOutsideScrollDeck ;

	#endregion

	#region variablesAClasser

	GUIStyle[] paginatorGuiStyle;
	GUIStyle[] sortButtonStyle=new GUIStyle[10];
	//private int chosenDeck = 0 ;
	//private int chosenIdDeck = -1 ;
	private int chosenPage ;

	string[] cardTypeList;
	private IList<string> matchValues;



	private IList<int> filtersCardType ;
	public GameObject CardObject;	

	int nbCardsPerRow = 0 ;
	int widthScreen = Screen.width ; 
	int heightScreen = Screen.height ;
	int nbPages ;
	int pageDebut ; 
	int pageFin ;
	private string valueSkill="";
	bool isSkillToDisplay = false ;
	bool isSkillChosen = false ;

	GUIStyle monLoaderStyle;
	float minLifeVal = 0;
	float maxLifeVal = 200;
	float minAttackVal = 0;
	float maxAttackVal = 100;
	float minMoveVal = 0;
	float maxMoveVal = 10;
	float minQuicknessVal = 0;
	float maxQuicknessVal = 100;
	float minLifeLimit = 0;
	float maxLifeLimit = 200;
	float minAttackLimit = 0;
	float maxAttackLimit = 100;
	float minMoveLimit = 0;
	float maxMoveLimit = 10;
	float minQuicknessLimit = 0;
	float maxQuicknessLimit = 100;
	float oldMinLifeVal = 0;
	float oldMaxLifeVal = 200;
	float oldMinAttackVal = 0;
	float oldMaxAttackVal = 100;
	float oldMinMoveVal = 0;
	float oldMaxMoveVal = 10;
	float oldMinQuicknessVal = 0;
	float oldMaxQuicknessVal = 100;



	bool isBeingDragged = false;

	bool confirmSuppress ;
	Vector2 scrollPosition = new Vector2(0,0) ;
	bool displayCreationDeckWindow  = false ;

	int deckToEdit = -1;

	int cardId ;

	string textMarket ;
	bool isMarketed ;
	int idFocused ;

	string tempPrice ; 


	bool enVente = false ;

	// jbu a mettre dans gameview GameObject[] displayedCards ;
	GameObject[] displayedDeckCards ;

	RaycastHit hit;
	Ray ray ;

	#endregion

	void Start() 
	{
		userModel = new User(ApplicationModel.username);
		deckModel = new Deck(1);
		instance = this;
		myGameView = Camera.main.gameObject.GetComponent<MyGameView>();
		myGameView.setStyles(); 
		MenuObject = Instantiate(MenuObject) as GameObject;
		filtersCardType = new List<int>();
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
		yield return StartCoroutine(deckModel.delete(deckId));
		StartCoroutine(retrieveDecks()); 
	}

	public IEnumerator editDeck(int deckToEdit, string decksName)
	{
		yield return StartCoroutine(deckModel.edit(deckToEdit, decksName));
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

	public IEnumerator RetrieveCardsFromDeck()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", myGameView.chosenIdDeck);      // id du deck courant
		WWW w = new WWW(URLGetCardsByDeck, form); 					// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			//print(w.text); 
			int tempInt;
			int tempInt2 = myGameView.cards.Count;
			bool tempBool;
			int j = 0;
			string[] cardDeckEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
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
		myGameView.isLoadedDeck = true;
	}

	public void AddCardToDeck(int idDeck, int idCard)
	{		
		StartCoroutine(deckModel.addCard(idDeck, idCard));
	}

	public void RemoveCardFromDeck(int idDeck, int idCard)
	{		
		StartCoroutine(deckModel.removeCard(idDeck, idCard));
	}

	public IEnumerator sellCard(int idCard, int cost)
	{	
		ApplicationModel.credits += cost ;
		yield return StartCoroutine(userModel.sellCard(idCard, cost));

		myGameView.soldCard = true ;
	}

	public void putOnMarket(int cardId, int price)
	{
		myGameView.cards[idFocused].onSale=1;
		myGameView.cards[idFocused].Price=price;

		StartCoroutine(userModel.toSell(cardId, price));
	}

	public void removeFromMarket(int cardId)
	{	
		myGameView.cards[idFocused].onSale = 0;
		StartCoroutine(userModel.notToSell(cardId));
	}

	public void changeMarketPrice(int cardId, int price)
	{	
		myGameView.cards[idFocused].Price = price;
		StartCoroutine(userModel.changePriceCard(cardId, price));
	}

	public void renameCard(int cardId, string newTitle, int renameCost)
	{
		myGameView.newTitle = myGameView.newTitle.Replace("\n", "");
		myGameView.newTitle = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(myGameView.newTitle.ToLower());
		StartCoroutine(userModel.renameCard(cardId, newTitle, renameCost));

		ApplicationModel.credits -= renameCost;
		myGameView.cards[idFocused].Title = myGameView.newTitle;
		myGameView.cardFocused.GetComponent<GameCard>().Card.Title = myGameView.newTitle;
		myGameView.cardFocused.GetComponent<GameCard>().ShowFace();
	}
}

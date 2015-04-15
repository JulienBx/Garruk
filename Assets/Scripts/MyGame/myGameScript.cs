using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myGameScript : MonoBehaviour 
{
	public static myGameScript instance;
	public MyGameView myGameView;

	#region variables

	//URL des fichiers PHP appelés par cette classe
	private string URLGetDecks = "http://54.77.118.214/GarrukServer/get_decks_by_user.php";
	private string URLGetCardsByDeck = "http://54.77.118.214/GarrukServer/get_cards_by_deck.php";
	private string URLGetMyCardsPage = ApplicationModel.host + "get_mycardspage_data.php";
	private string URLAddNewDeck = "http://54.77.118.214/GarrukServer/add_new_deck.php";
	private string URLDeleteDeck = "http://54.77.118.214/GarrukServer/delete_deck.php";
	private string URLEditDeck = "http://54.77.118.214/GarrukServer/update_deck_name.php";
	private string URLAddCardToDeck = "http://54.77.118.214/GarrukServer/add_card_to_deck_by_user.php";
	private string URLRemoveCardFromDeck = "http://54.77.118.214/GarrukServer/remove_card_from_deck_by_user.php";
	private string URLSellCard = "http://54.77.118.214/GarrukServer/sellRandomCard.php";
	private string URLPutOnMarket = "http://54.77.118.214/GarrukServer/putonmarket.php";
	private string URLRemoveFromMarket = "http://54.77.118.214/GarrukServer/removeFromMarket.php";
	private string URLChangeMarketPrice = "http://54.77.118.214/GarrukServer/changeMarketPrice.php";
	private string URLRenameCard = "http://54.77.118.214/GarrukServer/renameCard.php";

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
	int IDDeckToEdit = -1;

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
	string tempText = "Nouveau deck" ;
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
		instance = this;
		myGameView = Camera.main.gameObject.GetComponent<MyGameView>();
		myGameView.setStyles(); 
		MenuObject = Instantiate(MenuObject) as GameObject;
		filtersCardType = new List<int>();
		myGameView.toReloadAll = true;
	}
		
	public IEnumerator addDeck() 
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_name", tempText);
		WWW w = new WWW(URLAddNewDeck, form);						// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		}
		else 
		{
			StartCoroutine(RetrieveDecks()); 
		}
	}

	public IEnumerator deleteDeck() 
	{
		myGameView.areDecksRetrived = false;

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_id", IDDeckToEdit);
		WWW w = new WWW(URLDeleteDeck, form);						// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			StartCoroutine(RetrieveDecks()); 
		}
	}

	public IEnumerator editDeck()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_id", this.deckToEdit);
		form.AddField("myform_name", this.tempText);
		WWW w = new WWW(URLEditDeck, form); 						// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			StartCoroutine(RetrieveDecks());
		}
	}

	public IEnumerator RetrieveDecks() 
	{
		myGameView.myDecks = new List<Deck>();

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté	
		WWW w = new WWW(URLGetDecks, form); 						// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] decksInformation = w.text.Split('\n'); 
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
		myGameView.areDecksRetrived = true;
	}

	public IEnumerator getCards()
	{
		myGameView.cardsToBeDisplayed = new List<int>();
			
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);		
		WWW w = new WWW(URLGetMyCardsPage, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;

		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			print (w.text);
			
			string[] data = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
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

	public IEnumerator AddCardToDeck(int idCard, int cardsCount)
	{		
		WWWForm form = new WWWForm (); 						
		form.AddField ("myform_hash", ApplicationModel.hash);
		form.AddField ("myform_nick", ApplicationModel.username);
		form.AddField ("myform_deck", myGameView.chosenIdDeck);
		form.AddField ("myform_idCard", idCard);
		WWW w = new WWW (URLAddCardToDeck, form); 								
		yield return w; 						

		if (w.error != null) 
		{
			print (w.error); 										
		} 
		else 
		{
			//print ("deck : "+this.chosenIdDeck+ " idcard : "+idCard+" compteurCartes : "+cardsCount+w.text);
		}
	}

	public IEnumerator RemoveCardFromDeck(int idCard, int cardsCount)
	{		
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField ("myform_deck", myGameView.chosenIdDeck);
		form.AddField ("myform_idCard", idCard);
		WWW w = new WWW (URLRemoveCardFromDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			// print ("REUSSI : "+w.text);
		}
	}

	public IEnumerator sellCard(int idCard, int cost){
		
		ApplicationModel.credits += cost ;
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", idCard);
		form.AddField("myform_cost", cost);
		
		WWW w = new WWW(URLSellCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null)
		{
			print(w.error); 
		}
		else 
		{
			myGameView.soldCard = true ;
		}	
	}
	
	
	public IEnumerator putOnMarket(int cardId, int price){

		myGameView.cards[idFocused].onSale=1;
		myGameView.cards[idFocused].Price=price;

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", cardId);
		form.AddField("myform_price", price);
		form.AddField("myform_date",  System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss").ToString());
		
		WWW w = new WWW(URLPutOnMarket, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			//print (w.text);											// donne le retour
		}
	}

	public IEnumerator removeFromMarket(int cardId){
		
		myGameView.cards[idFocused].onSale = 0;
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", cardId);

		WWW w = new WWW(URLRemoveFromMarket, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			//print (w.text);											// donne le retour
		}
	}

	public IEnumerator changeMarketPrice(int cardId, int price){
		
		myGameView.cards[idFocused].Price=price;
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", cardId);
		form.AddField("myform_price", price);
		
		WWW w = new WWW(URLChangeMarketPrice, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			print (w.text);											// donne le retour
		}
	}

	public IEnumerator renameCard()
	{
		myGameView.newTitle = myGameView.newTitle.Replace("\n", "");
		myGameView.newTitle = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(myGameView.newTitle.ToLower());

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", myGameView.cardFocused.GetComponent<GameCard>().Card.Id);
		form.AddField("myform_title", myGameView.newTitle);
		form.AddField("myform_cost", myGameView.renameCost.ToString());
		
		WWW w = new WWW(URLRenameCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			print(w.text); 											// donne le retour
			ApplicationModel.credits = System.Convert.ToInt32(w.text);
			myGameView.cards[idFocused].Title = myGameView.newTitle;
			myGameView.cardFocused.GetComponent<GameCard>().Card.Title = myGameView.newTitle;
			myGameView.cardFocused.GetComponent<GameCard>().ShowFace();
		}	
	}
}

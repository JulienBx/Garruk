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
	private bool areDecksRetrived = false ;
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
	public GUIStyle deckStyle ;
	public GUIStyle deckChosenStyle ;
	public GUIStyle deckButtonStyle ;
	public GUIStyle deckButtonChosenStyle ;
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
	bool areCreatedDeckCards = false;
	float scaleDeck ;
	GameObject cardFocused ;
	bool displayFilters = false ;

	public GameObject MenuObject;

	//Si l'utilisateur sélectionne une action (edit ou suppress) sur un des deck, donne à cette variable l'ID du deck en question
	int IDDeckToEdit = -1;
	int renameCost = 200;

	Rect rectDeck ;
	Rect rectFocus ;
	Rect rectInsideScrollDeck ;
	Rect rectOutsideScrollDeck ;

	#endregion

	#region variablesAClasser
	private IList<Deck> myDecks;
	GUIStyle[] myDecksGuiStyle ;
	GUIStyle[] myDecksButtonGuiStyle ;
	GUIStyle[] paginatorGuiStyle;
	GUIStyle[] sortButtonStyle=new GUIStyle[10];
	private int chosenDeck = 0 ;
	private int chosenIdDeck = -1 ;
	private int chosenPage ;

	string[] skillsList;
	string[] cardTypeList;
	private IList<string> matchValues;
	// jbu dans gameview private IList<Card> cards ;
	private IList<int> cardsIds ;
	private IList<int> deckCardsIds ;
	private IList<int> cardsToBeDisplayed ;
	private IList<int> filtersCardType ;
	public GameObject CardObject;	
	bool isLoadedCards = false ;
	bool isLoadedDeck = false ;
	bool isDisplayedCards = true ;
	int nbCardsPerRow = 0 ;
	int widthScreen = Screen.width ; 
	int heightScreen = Screen.height ;
	int nbPages ;
	int pageDebut ; 
	int pageFin ;
	private bool[] togglesCurrentStates;
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
	int focusedCard = -1 ;
	int focusedCardPrice ;
	bool isSellingCard = false ; 
	bool isUpgradingCard = false ;
	bool isMarketingCard = false ; 
	bool isRenamingCard = false;
	bool toReloadAll = false ;

	bool isBeingDragged = false;
	bool toReload = false ;
	bool confirmSuppress ;
	Vector2 scrollPosition = new Vector2(0,0) ;
	bool displayCreationDeckWindow  = false ;
	string tempText = "Nouveau deck" ;
	int deckToEdit = -1;
	bool destroyAll = false ;
	bool displayDecks = false ;
	int cardId ;
	bool isCreatedDeckCards=false;
	bool displayLoader;
	bool isCreatedCards = false ;
	bool destroySellingCardWindow = false ;
	bool destroyUpgradingCardWindow = false ;
	bool destroyRenamingCardWindow = false;
	bool destroyFocus = false ;
	bool soldCard = false ;
	string textMarket ;
	bool isMarketed ;
	int idFocused ;
	bool isChangingPrice ;
	string tempPrice ; 
	string newTitle;
	bool isEscDown = false ;
	bool isUpEscape;
	bool enVente = false ;

	// jbu a mettre dans gameview GameObject[] displayedCards ;
	GameObject[] displayedDeckCards ;

	RaycastHit hit;
	Ray ray ;

	int oldSortSelected = 10;
	int sortSelected = 10;

	#endregion

	void Start() 
	{
		instance = this;
		myGameView = Camera.main.gameObject.GetComponent<MyGameView>();
		myGameView.setStyles(); 
		MenuObject = Instantiate(MenuObject) as GameObject;
		filtersCardType = new List<int>();
		toReloadAll = true;
	}

	void Update () 
	{	
		if (Screen.width != widthScreen || Screen.height != heightScreen)
		{
			myGameView.setStyles();
			this.applyFilters ();
			if (this.focusedCard != -1)
			{
				Destroy (cardFocused);
				this.focusedCard=-1;
			}
			this.clearCards();
			this.clearDeckCards();
			this.createCards();
			this.createDeckCards();
			this.displayFilters = true ;
			this.displayDecks = true ;

		}
		if (toReload) {
			this.applyFilters ();
			if (sortSelected!=10){
				this.sortCards();
			}
			this.displayPage();
			toReload = false ;
		}
		if (destroyAll){
			this.clearCards();
			this.clearDeckCards();
			isCreatedDeckCards=false;
			toReloadAll=true;
			destroyAll=false;
		}
		if (toReloadAll) {
			displayLoader = true ;
			displayFilters = false ;

			areDecksRetrived=false ;
			areCreatedDeckCards = false ;
			isLoadedCards = false ;
			isLoadedDeck = false ;
				
			StartCoroutine(getCards());
			toReloadAll = false ;
		}
		if (isLoadedCards){
			this.createCards();
			StartCoroutine(RetrieveDecks());
			isLoadedCards=false ;
			isCreatedCards=true;
		}
		if (areDecksRetrived && isCreatedCards){
			StartCoroutine(this.RetrieveCardsFromDeck ());
			areDecksRetrived=false ;
		}
		if (isLoadedDeck){
			if (isCreatedDeckCards){
				this.displayDeckCards();
			}
			else{
				this.createDeckCards();
				isCreatedDeckCards=true;
			}
			this.applyFilters ();
			this.displayPage();
			displayDecks = true ;
			isLoadedDeck = false ;
			displayLoader = false ;
			displayFilters = true ;
		}
		
		if (Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out hit))
			{
				if (hit.collider.name.StartsWith("DeckCard")){
					StartCoroutine(this.RemoveCardFromDeck(myGameView.cards[this.deckCardsIds[System.Convert.ToInt32(hit.collider.gameObject.name.Substring(8))]].Id, this.deckCardsIds.Count-1));
					int tempInt = System.Convert.ToInt32(hit.collider.gameObject.name.Substring(8));
					deckCardsIds.RemoveAt (tempInt);
					myDecks[chosenDeck].NbCards--;
					this.displayDeckCards();
					this.applyFilters ();
					this.displayPage ();
				}
				else if (hit.collider.name.StartsWith("Card")){
					if (this.deckCardsIds.Count!=5){
						int tempInt = System.Convert.ToInt32(hit.collider.gameObject.name.Substring(4));
						deckCardsIds.Add (tempInt);
						myDecks[chosenDeck].NbCards++;
						this.displayDeckCards();
						StartCoroutine(this.AddCardToDeck(myGameView.cards[System.Convert.ToInt32(hit.collider.gameObject.name.Substring(4))].Id, this.deckCardsIds.Count));
						this.applyFilters ();
						this.displayPage ();
					}
				}
			}
		}

		if (Input.GetMouseButtonDown(1)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out hit))
			{
				if (hit.collider.name.Contains("DeckCard") || hit.collider.name.StartsWith("Card")){
					displayDecks=false ;
					displayFilters=false ;
					if (hit.collider.name.Contains("DeckCard")){
						focusedCard = System.Convert.ToInt32(hit.collider.gameObject.name.Substring(8));
					}
					else{
						focusedCard = System.Convert.ToInt32(hit.collider.gameObject.name.Substring(4));
					}

					int finish = 3 * nbCardsPerRow;
					for(int i = 0 ; i < finish ; i++){
						myGameView.displayedCards[i].SetActive(false);
					}
					for(int i = 0 ; i < displayedDeckCards.Length ; i++){
						displayedDeckCards[i].SetActive(false);
					}

					cardFocused = Instantiate(CardObject) as GameObject;
					Destroy(cardFocused.GetComponent<GameNetworkCard>());
					Destroy(cardFocused.GetComponent<PhotonView>());
					float scale = heightScreen/120f;
					cardFocused.transform.localScale = new Vector3(scale,scale,scale); 
					Vector3 vec = Camera.main.WorldToScreenPoint(cardFocused.collider.bounds.size);
					cardFocused.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0.50f*widthScreen ,0.45f*heightScreen-1 , 10)); 
					cardFocused.gameObject.name = "FocusedCard";	

					if (hit.collider.name.Contains("DeckCard")){
						idFocused = deckCardsIds[focusedCard];

					}
					else{
						idFocused = focusedCard;
					}

					cardId = myGameView.cards[idFocused].Id;
					cardFocused.GetComponent<GameCard>().Card = myGameView.cards[idFocused]; 
					focusedCardPrice = myGameView.cards[idFocused].getCost();
					if (myGameView.cards[idFocused].onSale==0){
						textMarket = "Mettre la carte en vente sur le bazar";
						isMarketed = false ;
					}
					else{
						textMarket = "La carte est mise en vente sur le bazar pour "+myGameView.cards[idFocused].Price+" crédits. Modifier ?";
						isMarketed = true ;
					}

					cardFocused.GetComponent<GameCard>().ShowFace();
					cardFocused.GetComponent<GameCard>().setTextResolution(2f);
					cardFocused.SetActive (true);
					cardFocused.transform.Find("texturedGameCard").FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();
					cardFocused.transform.Find("texturedGameCard").FindChild("ExperienceArea").GetComponent<GameCard_experience>().setTextResolution(2f);
					rectFocus = new Rect(0.50f*widthScreen+(vec.x-widthScreen/2f)/2f, 0.15f*heightScreen, 0.25f*widthScreen, 0.8f*heightScreen);
				}
			}
		}

		if (destroySellingCardWindow){
			isSellingCard=false;
			destroySellingCardWindow = false ;
		}

		if (destroyUpgradingCardWindow){
			isUpgradingCard=false;
			destroyUpgradingCardWindow = false ;
		}

		if (destroyRenamingCardWindow){
			isRenamingCard=false;
			destroyRenamingCardWindow = false ;
		}

		if (destroyFocus){
			isSellingCard=false;
			isMarketingCard=false;
			isUpgradingCard=false;
			isRenamingCard = false;
			Destroy(cardFocused);
			this.focusedCard=-1;
			displayFilters = true ;
			displayDecks = true ;
			this.displayPage();
			this.displayDeckCards();
			destroyFocus = false ;
		}

		if (soldCard){
			isSellingCard=false;
			Destroy(cardFocused);
			this.focusedCard=-1;
			destroyAll = true ;
			soldCard = false;
		}

		if (isUpEscape){
			isEscDown = false ;
			isUpEscape = false ;
		}

		if (isUpgradingCard){
			if(Input.GetKeyDown(KeyCode.Return)) {
				destroyUpgradingCardWindow = true ;
				cardFocused.transform
					.Find("texturedGameCard")
						.FindChild("ExperienceArea")
						.GetComponent<GameCard_experience>()
						.addXp(myGameView.cards[idFocused].getPriceForNextLevel(),myGameView.cards[idFocused].getPriceForNextLevel());
				
			}
			else if(Input.GetKeyDown(KeyCode.Escape)) {
				isUpgradingCard = false ;
				isEscDown = true ;
				}
		}
		else if(isRenamingCard){
			if(Input.GetKeyDown(KeyCode.Return)) {
				isRenamingCard = false ;
				destroyRenamingCardWindow = true ;
				StartCoroutine (this.renameCard());
			}
			else if(Input.GetKeyDown(KeyCode.Escape)){
				isRenamingCard = false ;
				isEscDown = true ;
			}
			else if(newTitle.Contains("\n")){
				isRenamingCard = false ;
				destroyRenamingCardWindow = true ;
				StartCoroutine (this.renameCard());
			}
		}
		else if(isSellingCard){
			if(Input.GetKeyDown(KeyCode.Return)) {
				isSellingCard = false ;
				destroySellingCardWindow = true ;
				StartCoroutine (this.sellCard(cardId, focusedCardPrice));
				}
			else if(Input.GetKeyDown(KeyCode.Escape)){
				isSellingCard = false ;
				isEscDown = true ;
			}
		}
		else if (isMarketingCard){
			if (isChangingPrice){
				if(Input.GetKeyDown(KeyCode.Return)) {
					destroyFocus = true ;
					isChangingPrice = false ;
					StartCoroutine (this.changeMarketPrice(cardId, focusedCardPrice));
				}
				else if(Input.GetKeyDown(KeyCode.Escape)) {
					isMarketingCard = false ;
					isEscDown = true ;
				}
			}

		}

		if (oldSortSelected!=sortSelected){
			if(oldSortSelected!=10){
				this.sortButtonStyle[oldSortSelected]=this.sortDefaultButtonStyle;
			}
			this.sortButtonStyle[sortSelected]=this.sortActivatedButtonStyle;
			oldSortSelected=sortSelected;
		}

	}
		
	public IEnumerator addDeck() {
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_name", tempText);

		WWW w = new WWW(URLAddNewDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) {
			print (w.error); 										// donne l'erreur eventuelle
		} else {
//			print (w.text);
			StartCoroutine(RetrieveDecks()); 
		}
	}

	public IEnumerator deleteDeck() {
		areDecksRetrived=false ;
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_id", IDDeckToEdit);
		
		WWW w = new WWW(URLDeleteDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) {
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			//print (w.text);
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
		
		WWW w = new WWW(URLEditDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) {
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			//print (w.text);
			StartCoroutine(RetrieveDecks());
		}
	}

	private void displaySkills(){
		this.matchValues = new List<string> ();	
		if (this.valueSkill != "") {
			this.matchValues = new List<string> ();
			for (int i = 0; i < skillsList.Length-1; i++) {  
				if (skillsList [i].ToLower ().Contains (valueSkill)) {
					matchValues.Add (skillsList [i]);
				}
			}
		}
	}

	private void applyFilters() {
		this.cardsToBeDisplayed=new List<int>();
		IList<int> tempCardsToBeDisplayed = new List<int>();
		int nbFilters = this.filtersCardType.Count;
		bool testFilters = false;
		bool testDeck = false;
		bool test ;
		
			bool minLifeBool = (minLifeLimit==minLifeVal);
			bool maxLifeBool = (maxLifeLimit==maxLifeVal);
			bool minMoveBool = (minMoveLimit==minMoveVal);
			bool maxMoveBool = (maxMoveLimit==maxMoveVal);
			bool minQuicknessBool = (minQuicknessLimit==minQuicknessVal);
			bool maxQuicknessBool = (maxQuicknessLimit==maxQuicknessVal);
			bool minAttackBool = (minAttackLimit==minAttackVal);
			bool maxAttackBool = (maxAttackLimit==maxAttackVal);
			if (this.isSkillChosen){
				int max = myGameView.cards.Count;
				if (nbFilters==0){
				max = myGameView.cards.Count;
					if (enVente){
						for (int i = 0; i < max ; i++) {
							if (myGameView.cards[i].hasSkill(this.valueSkill) && myGameView.cards[i].onSale==1){
								testDeck = false ;
								for (int j = 0 ; j < deckCardsIds.Count ; j++){
									if (i==deckCardsIds[j]){
										testDeck = true ; 
									}
								}
								if (!testDeck){
									tempCardsToBeDisplayed.Add(i);
								}
							}
						}
					}
					else{
						for (int i = 0; i < max ; i++) {
							if (myGameView.cards[i].hasSkill(this.valueSkill)){
								testDeck = false ;
								for (int j = 0 ; j < deckCardsIds.Count ; j++){
									if (i==deckCardsIds[j]){
										testDeck = true ; 
									}
								}
								if (!testDeck){
									tempCardsToBeDisplayed.Add(i);
								}
							}
						}
					}
				}
				else{
					for (int i = 0; i < max ; i++) {
						test = false ;
						int j = 0 ;
						if(enVente){	
							while (!test && j!=nbFilters){
								if (myGameView.cards[i].IdClass==this.filtersCardType[j])
								{
									test=true ;
									if (myGameView.cards[i].hasSkill(this.valueSkill) && myGameView.cards[i].onSale == 1)
									{
										testDeck = false ;
										for (int k = 0 ; k < deckCardsIds.Count ; k++){
											if (i==deckCardsIds[k]){
												testDeck = true ; 
											}
										}
										if (!testDeck){
											tempCardsToBeDisplayed.Add(i);
										}
									}
								}
								j++;
							}
						}
						else
						{
							while (!test && j!=nbFilters)
							{
								if (myGameView.cards[i].IdClass==this.filtersCardType[j])
								{
									test=true ;
									if (myGameView.cards[i].hasSkill(this.valueSkill))
									{
										testDeck = false ;
										for (int k = 0 ; k < deckCardsIds.Count ; k++)
										{
											if (i==deckCardsIds[k])
											{
												testDeck = true ; 
											}
										}
										if (!testDeck){
											tempCardsToBeDisplayed.Add(i);
										}
									}
								}
								j++;
							}
						}
					}
				}
			}
			else
			{
				int max = myGameView.cards.Count;
				if (nbFilters==0)
				{
					if (enVente)
					{
						for (int i = 0; i < max ; i++) 
						{
							if(myGameView.cards[i].onSale == 1)
							{
								testDeck = false;
								for (int j = 0 ; j < deckCardsIds.Count ; j++)
								{
									if (i == deckCardsIds[j])
									{
										testDeck = true ; 
									}
								}
								if (!testDeck){
									tempCardsToBeDisplayed.Add(i);
								}
							}
						}
					}
					else
					{
						for (int i = 0; i < max ; i++) 
						{
							testDeck = false;
							for (int j = 0 ; j < deckCardsIds.Count ; j++){
								if (i==deckCardsIds[j]){
									testDeck = true ; 
								}
							}
							if (!testDeck){
								tempCardsToBeDisplayed.Add(i);
							}
						}
					}
				}
				else{
					if (enVente){
						for (int i = 0; i < max ; i++) {
							test = false ;
							int j = 0 ;
							while (!test && j!=nbFilters){
								if (myGameView.cards[i].IdClass==this.filtersCardType[j]){
									if(myGameView.cards[i].onSale == 1)
									{
										test=true ;
										testDeck = false ;
										for (int k = 0 ; k < deckCardsIds.Count ; k++)
										{
											if (i == deckCardsIds[k])
											{
												testDeck = true ; 
											}
										}
										if (!testDeck){
											tempCardsToBeDisplayed.Add(i);
										}
									}
								}
								j++;
							}
						}
					}
					else{
						for (int i = 0; i < max ; i++) {
							test = false ;
							int j = 0 ;
							while (!test && j!=nbFilters){
								if (myGameView.cards[i].IdClass==this.filtersCardType[j]){
									test=true ;
									testDeck = false ;
									for (int k = 0 ; k < deckCardsIds.Count ; k++){
										if (i==deckCardsIds[k]){
											testDeck = true ; 
										}
									}
									if (!testDeck){
										tempCardsToBeDisplayed.Add(i);
									}
								}
								j++;
							}
						}
					}
				}
			}
			if (tempCardsToBeDisplayed.Count>0){
				minLifeLimit=10000;
				maxLifeLimit=0;
				minAttackLimit=10000;
				maxAttackLimit=0;
				minMoveLimit=10000;
				maxMoveLimit=0;
				minQuicknessLimit=10000;
				maxQuicknessLimit=0;
				for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++){
					if (myGameView.cards[tempCardsToBeDisplayed[i]].Life<minLifeLimit){
						minLifeLimit = myGameView.cards[tempCardsToBeDisplayed[i]].Life;
					}
					if (myGameView.cards[tempCardsToBeDisplayed[i]].Life>maxLifeLimit){
						maxLifeLimit = myGameView.cards[tempCardsToBeDisplayed[i]].Life;
					}
					if (myGameView.cards[tempCardsToBeDisplayed[i]].Attack<minAttackLimit){
						minAttackLimit = myGameView.cards[tempCardsToBeDisplayed[i]].Attack;
					}
					if (myGameView.cards[tempCardsToBeDisplayed[i]].Attack>maxAttackLimit){
						maxAttackLimit = myGameView.cards[tempCardsToBeDisplayed[i]].Attack;
					}
					if (myGameView.cards[tempCardsToBeDisplayed[i]].Move<minMoveLimit){
						minMoveLimit = myGameView.cards[tempCardsToBeDisplayed[i]].Move;
					}
					if (myGameView.cards[tempCardsToBeDisplayed[i]].Move>maxMoveLimit){
						maxMoveLimit = myGameView.cards[tempCardsToBeDisplayed[i]].Move;
					}
					if (myGameView.cards[tempCardsToBeDisplayed[i]].Speed<minQuicknessLimit){
						minQuicknessLimit = myGameView.cards[tempCardsToBeDisplayed[i]].Speed;
					}
					if (myGameView.cards[tempCardsToBeDisplayed[i]].Speed>maxQuicknessLimit){
						maxQuicknessLimit = myGameView.cards[tempCardsToBeDisplayed[i]].Speed;
					}
				}
				if (minLifeBool && maxLifeVal>minLifeLimit){
					minLifeVal = minLifeLimit;
				}
				else{
					if (minLifeVal<minLifeLimit){
						minLifeLimit = minLifeVal;
					}
				}
				if (maxLifeBool && minLifeVal<maxLifeLimit){
					maxLifeVal = maxLifeLimit;
					print ("Max "+maxLifeVal);
				}
				else{
					if (maxLifeVal>maxLifeLimit){
						maxLifeLimit = maxLifeVal;
					}
					print ("Max2 "+maxLifeVal);
				}
				if (minAttackBool && maxAttackVal>minAttackLimit){
					minAttackVal = minAttackLimit;
				}
				else{
					if (minAttackVal<minAttackLimit){
						minAttackLimit = minAttackVal;
					}
				}
				if (maxAttackBool && minAttackVal<maxAttackLimit){
					maxAttackVal = maxAttackLimit;
				}
				else{
					if (maxAttackVal>maxAttackLimit){
						maxAttackLimit = maxAttackVal;
					}
				}
				if (minMoveBool && maxMoveVal>minMoveLimit){
					minMoveVal = minMoveLimit;
				}
				else{
					if (minMoveVal<minMoveLimit){
						minMoveLimit = minMoveVal;
					}
				}
				if (maxMoveBool && minMoveVal<maxMoveLimit){
					maxMoveVal = maxMoveLimit;
				}
				else{
					if (maxMoveVal>maxMoveLimit){
						maxMoveLimit = maxMoveVal;
					}
				}
				if (minQuicknessBool && maxQuicknessVal>minQuicknessLimit){
					minQuicknessVal = minQuicknessLimit;
				}
				else{
					if (minQuicknessVal<minQuicknessLimit){
						minQuicknessLimit = minQuicknessVal;
					}
				}
				if (maxQuicknessBool && minQuicknessVal<maxQuicknessLimit){
					maxQuicknessVal = maxQuicknessLimit;
				}
				else{
					if (maxQuicknessVal>maxQuicknessLimit){
						maxQuicknessLimit = maxQuicknessVal;
					}
				}

				oldMinLifeVal = minLifeVal ;
				oldMaxLifeVal = maxLifeVal ;
				oldMinQuicknessVal = minQuicknessVal ;
				oldMaxQuicknessVal = maxQuicknessVal ;
				oldMinMoveVal = minMoveVal ;
				oldMaxMoveVal = maxMoveVal ;
				oldMinAttackVal = minAttackVal ;
				oldMaxAttackVal = maxAttackVal ;
			}

		if (this.minLifeVal!=this.minLifeLimit){
			testFilters = true ;
		}
		else if (this.maxLifeVal!=this.maxLifeLimit){
			testFilters = true ;
		}
		else if (this.minAttackVal!=this.minAttackLimit){
			testFilters = true ;
		}
		else if (this.maxAttackVal!=this.maxAttackLimit){
			testFilters = true ;
		}
		else if (this.minMoveVal!=this.minMoveLimit){
			testFilters = true ;
		}
		else if (this.maxMoveVal!=this.maxMoveLimit){
			testFilters = true ;
		}
		else if (this.minQuicknessVal!=this.minQuicknessLimit){
			testFilters = true ;
		}
		else if (this.maxQuicknessVal!=this.maxQuicknessLimit){
			testFilters = true ;
		}

		if (testFilters == true){
			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++){
				if (myGameView.cards[tempCardsToBeDisplayed[i]].verifyC(minLifeVal,maxLifeVal,minAttackVal,maxAttackVal,minMoveVal,maxMoveVal,minQuicknessVal,maxQuicknessVal)){
					this.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
				}
			}
		}
		else{
			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++){
				this.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
			}
		}

		nbPages = Mathf.CeilToInt(cardsToBeDisplayed.Count / (3.0f*nbCardsPerRow));
		pageDebut = 0 ;
		if (nbPages>15){
			pageFin = 14 ;
		}
		else{
			pageFin = nbPages ;
		}
		this.chosenPage = 0;
		paginatorGuiStyle = new GUIStyle[nbPages];
		for (int i = 0; i < nbPages; i++) { 
			if (i==0){
				paginatorGuiStyle[i]=paginationActivatedStyle;
			}
			else{
				paginatorGuiStyle[i]=paginationStyle;
			}
		}
	}

	public void setFilters(){
		minLifeLimit=10000;
		maxLifeLimit=0;
		minAttackLimit=10000;
		maxAttackLimit=0;
		minMoveLimit=10000;
		maxMoveLimit=0;
		minQuicknessLimit=10000;
		maxQuicknessLimit=0;
						
		int max = myGameView.cards.Count;
		for (int i = 0; i < max ; i++) {
			if (myGameView.cards[i].Life<minLifeLimit){
				minLifeLimit = myGameView.cards[i].Life;
			}
			if (myGameView.cards[i].Life>maxLifeLimit){
				maxLifeLimit = myGameView.cards[i].Life;
			}
			if (myGameView.cards[i].Attack<minAttackLimit){
				minAttackLimit = myGameView.cards[i].Attack;
			}
			if (myGameView.cards[i].Attack>maxAttackLimit){
				maxAttackLimit = myGameView.cards[i].Attack;
			}
			if (myGameView.cards[i].Move<minMoveLimit){
				minMoveLimit = myGameView.cards[i].Move;
			}
			if (myGameView.cards[i].Move>maxMoveLimit){
				maxMoveLimit = myGameView.cards[i].Move;
			}
			if (myGameView.cards[i].Speed<minQuicknessLimit){
				minQuicknessLimit = myGameView.cards[i].Speed;
			}
			if (myGameView.cards[i].Speed>maxQuicknessLimit){
				maxQuicknessLimit = myGameView.cards[i].Speed;
			}
		}
		minLifeVal = minLifeLimit;
		maxLifeVal = maxLifeLimit;
		minAttackVal = minAttackLimit;
		maxAttackVal = maxAttackLimit;
		minMoveVal = minMoveLimit;
		maxMoveVal = maxMoveLimit;
		minQuicknessVal = minQuicknessLimit;
		maxQuicknessVal = maxQuicknessLimit;
	}

	public IEnumerator RetrieveDecks() {
		myDecks = new List<Deck> ();

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW(URLGetDecks, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
		//	print(w.text); 											// donne le retour
			
			string[] decksInformation = w.text.Split('\n'); 
			string[] deckInformation;

			myDecksGuiStyle = new GUIStyle[decksInformation.Length - 1];
			myDecksButtonGuiStyle = new GUIStyle[decksInformation.Length - 1];

			for(int i = 0 ; i < decksInformation.Length - 1 ; i++) 		// On boucle sur les attributs d'un deck
			{
				if (i>0){
					myDecksGuiStyle[i] = this.deckStyle;
					myDecksButtonGuiStyle[i] = this.deckButtonStyle;
				}
				else{
					myDecksGuiStyle[i] = this.deckChosenStyle;
					myDecksButtonGuiStyle[i] = this.deckButtonChosenStyle;
				}
				deckInformation = decksInformation[i].Split('\\');
				myDecks.Add(new Deck(System.Convert.ToInt32(deckInformation[0]), deckInformation[1], System.Convert.ToInt32(deckInformation[2])));

				if (i==0){
					chosenIdDeck = System.Convert.ToInt32(deckInformation[0]);
					chosenDeck = 0 ;
				}
			}
		}
		areDecksRetrived = true ;
	}

	public IEnumerator getCards()
	{
		string[] cardsIDS = null;
		string[] skillsIds = null;
		string[] cardInfo = null;
		string[] cardInfo2 = null;
		string[] tempString = null;
		this.cardsToBeDisplayed = new List<int> ();
		int tempInt ;
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLGetMyCardsPage, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			print (w.text);
			
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			cardsIDS=data[2].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
			skillsIds = data[1].Split('\n');

			this.cardTypeList = data[0].Split('\n');
			togglesCurrentStates = new bool[this.cardTypeList.Length];
			for(int i = 0 ; i < this.cardTypeList.Length-1 ; i++){
				togglesCurrentStates[i] = false;
			}
			this.skillsList = new string[skillsIds.Length-1];
			for(int i = 0 ; i < skillsIds.Length-1 ; i++){
				tempString = skillsIds[i].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
				if (i>0){
					this.skillsList[i-1]=tempString[0];
				}
			}
	
			myGameView.cards = new List<Card>();
			this.cardsIds = new List<int>();

			for(int i = 0 ; i < cardsIDS.Length-1 ; i++){
				cardInfo = cardsIDS[i].Split('\n');
				for(int j = 1 ; j < cardInfo.Length-1 ; j++){
					cardInfo2 = cardInfo[j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
					if (j==1){
						myGameView.cards.Add(new Card(System.Convert.ToInt32(cardInfo2[0]), // id
						                        cardInfo2[1], // title
						                        System.Convert.ToInt32(cardInfo2[2]), // life
						                        System.Convert.ToInt32(cardInfo2[3]), // attack
						                        System.Convert.ToInt32(cardInfo2[4]), // speed
						                        System.Convert.ToInt32(cardInfo2[5]), // move
						                        System.Convert.ToInt32(cardInfo2[6]), // artindex
						                        System.Convert.ToInt32(cardInfo2[7]), // idclass
						                        this.cardTypeList[System.Convert.ToInt32(cardInfo2[7])], // titleclass
						                        System.Convert.ToInt32(cardInfo2[8]), // lifelevel
						                        System.Convert.ToInt32(cardInfo2[9]), // movelevel
						                        System.Convert.ToInt32(cardInfo2[10]),
						                        System.Convert.ToInt32(cardInfo2[11]),
						                        System.Convert.ToInt32(cardInfo2[12]),
						                        System.Convert.ToInt32(cardInfo2[13]),
						                        System.Convert.ToInt32(cardInfo2[14]),
						                        System.Convert.ToInt32(cardInfo2[15]),
						                        System.Convert.ToInt32(cardInfo2[16])
						                        )); 
						myGameView.cards[i].Skills = new List<Skill>();
						this.cardsIds.Add(System.Convert.ToInt32(cardInfo2[0]));
						this.cardsToBeDisplayed.Add(i);
					}
					else{
						tempInt = System.Convert.ToInt32(cardInfo2[0]);
						myGameView.cards[i].Skills.Add(new Skill (skillsList[System.Convert.ToInt32(cardInfo2[0])], 
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
		isLoadedCards = true;
		isDisplayedCards = true ;
	}

	private void createCards(){
		
		float tempF = 10f*widthScreen/heightScreen;
		float width = tempF * 0.78f;
		nbCardsPerRow = Mathf.FloorToInt(width/1.6f);
		float debutLargeur = -0.49f * tempF + 0.8f + (width - 1.6f * nbCardsPerRow)/2 ;
		myGameView.displayedCards = new GameObject[3*nbCardsPerRow];
		int nbCardsToDisplay = this.cardsToBeDisplayed.Count;
		for(int i = 0 ; i < 3*nbCardsPerRow ; i++){
			myGameView.displayedCards[i] = Instantiate(CardObject) as GameObject;
			Destroy(myGameView.displayedCards[i].GetComponent<GameNetworkCard>());
			Destroy(myGameView.displayedCards[i].GetComponent<PhotonView>());
			myGameView.displayedCards[i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
			myGameView.displayedCards[i].transform.localPosition = new Vector3(debutLargeur + 1.6f*(i%nbCardsPerRow), 0.8f-(i-i%nbCardsPerRow)/nbCardsPerRow*2.2f, 0); 
			myGameView.displayedCards[i].gameObject.name = "Card" + i + "";	
			if (i<nbCardsToDisplay){
				myGameView.displayedCards[i].GetComponent<GameCard>().Card = myGameView.cards[this.cardsToBeDisplayed[i]]; 
				myGameView.displayedCards[i].GetComponent<GameCard>().ShowFace();
				myGameView.displayedCards[i].transform.Find("texturedGameCard").FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();
			}   
			else{
				myGameView.displayedCards[i].SetActive (false);
			}
		}
		nbPages = Mathf.CeilToInt(cardsToBeDisplayed.Count / (3.0f*nbCardsPerRow));
		pageDebut = 0 ;
		if (nbPages>15){
			pageFin = 14 ;
		}
		else{
			pageFin = nbPages ;
		}
		this.chosenPage = 0;
		paginatorGuiStyle = new GUIStyle[nbPages];
		for (int i = 0; i < nbPages; i++) { 
			if (i==0){
				paginatorGuiStyle[i]=paginationActivatedStyle;
			}
			else{
				paginatorGuiStyle[i]=paginationStyle;
			}
		}
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		this.setFilters ();
	}

	private void displayPage(){
		
		int start = 3 * nbCardsPerRow * chosenPage;
		int finish = start + 3 * nbCardsPerRow;
		for(int i = start ; i < finish ; i++){
			//displayedCards[i].GetComponent<GameCard>().setTextResolution (1f);
			int nbCardsToDisplay = this.cardsToBeDisplayed.Count;
			if (i<nbCardsToDisplay){
				myGameView.displayedCards[i-start].gameObject.name = "Card" + this.cardsToBeDisplayed[i] + "";
				myGameView.displayedCards[i-start].SetActive(true);
				myGameView.displayedCards[i-start].GetComponent<GameCard>().Card = myGameView.cards[this.cardsToBeDisplayed[i]]; 
				myGameView.displayedCards[i-start].GetComponent<GameCard>().ShowFace();
				myGameView.displayedCards[i-start].transform.Find("texturedGameCard").FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();
			}
			else{
				myGameView.displayedCards[i-start].SetActive(false);
			}
		}
	}

	private void clearCards(){
		for (int i = 0; i < 3*nbCardsPerRow; i++) {
			Destroy(myGameView.displayedCards[i]);
		}
	}

	private void clearDeckCards(){
		for (int i = 0; i < 5; i++) {
			Destroy(displayedDeckCards[i]);
		}
	}

	public IEnumerator RetrieveCardsFromDeck()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", this.chosenIdDeck); // id du deck courant
		
		WWW w = new WWW(URLGetCardsByDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			//print(w.text); 
			int tempInt ;
			int tempInt2 = myGameView.cards.Count;
			bool tempBool ;
			int j = 0 ;
			string[] cardDeckEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
			deckCardsIds = new List<int>();
			for(int i = 0 ; i < cardDeckEntries.Length-1 ; i++){
				tempInt = System.Convert.ToInt32(cardDeckEntries[i]);
				tempBool = true ; 
				j = 0 ;
				while (tempBool && j<tempInt2){
					if (myGameView.cards[j].Id == tempInt){
						tempBool = false;
					}
					j++;		
				}
				j--;
				deckCardsIds.Add(j);
			}
		}
		this.isLoadedDeck=true ;
	}

	private void createDeckCards(){
		float tempF = 10f*widthScreen/heightScreen;
		float width = tempF * 0.6f;
		scaleDeck = Mathf.Min (1.6f, width / 6f);
		float pas = (width - 5f * scaleDeck) / 6f;
		float debutLargeur = -0.3f * tempF + pas + scaleDeck/2 ;

		displayedDeckCards = new GameObject[5];
		int nbDeckCardsToDisplay = this.deckCardsIds.Count;
		for(int i = 0 ; i < 5 ; i++){
			displayedDeckCards[i] = Instantiate(CardObject) as GameObject;
			Destroy(displayedDeckCards[i].GetComponent<GameNetworkCard>());
			Destroy(displayedDeckCards[i].GetComponent<PhotonView>());
			displayedDeckCards[i].transform.localScale = new Vector3(scaleDeck,scaleDeck,scaleDeck); 
			displayedDeckCards[i].transform.localPosition = new Vector3(debutLargeur + (scaleDeck+pas)*i , 2.9f, 0); 
			displayedDeckCards[i].gameObject.name = "DeckCard" + i + "";	
			if (i<nbDeckCardsToDisplay){
				displayedDeckCards[i].GetComponent<GameCard>().Card = myGameView.cards[this.deckCardsIds[i]]; 
				displayedDeckCards[i].GetComponent<GameCard>().ShowFace();
				displayedDeckCards[i].transform.Find("texturedGameCard").FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();
			}   
			else{
				displayedDeckCards[i].SetActive (false);
			}
		}
		areCreatedDeckCards = true; 
	}

	private void displayDeckCards(){
		int nbDeckCardsToDisplay = this.deckCardsIds.Count;
		for(int i = 0 ; i < 5 ; i++){
			if (i<nbDeckCardsToDisplay){
				displayedDeckCards[i].SetActive (true);
				displayedDeckCards[i].GetComponent<GameCard>().Card = myGameView.cards[this.deckCardsIds[i]]; 
				displayedDeckCards[i].GetComponent<GameCard>().ShowFace();
				displayedDeckCards[i].transform.Find("texturedGameCard").FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();
			}   
			else{
				displayedDeckCards[i].SetActive (false);
			}
		}
	}

	public IEnumerator AddCardToDeck(int idCard, int cardsCount)
	{		
		WWWForm form = new WWWForm (); 						
		form.AddField ("myform_hash", ApplicationModel.hash);
		form.AddField ("myform_nick", ApplicationModel.username);
		form.AddField ("myform_deck", this.chosenIdDeck);
		form.AddField ("myform_idCard", idCard);

		WWW w = new WWW (URLAddCardToDeck, form); 								
		yield return w; 											
		if (w.error != null) 
		{
			print (w.error); 										
		} else {
			//print ("deck : "+this.chosenIdDeck+ " idcard : "+idCard+" compteurCartes : "+cardsCount+w.text);
		}
	}

	public IEnumerator RemoveCardFromDeck(int idCard, int cardsCount)
	{		
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField ("myform_deck", this.chosenIdDeck);
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
			print (w.error); 
		else 
		{
			this.soldCard = true ;
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
		newTitle = newTitle.Replace("\n", "");
		newTitle=System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(newTitle.ToLower());

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", cardFocused.GetComponent<GameCard>().Card.Id);
		form.AddField("myform_title", newTitle);
		form.AddField("myform_cost", renameCost.ToString());
		
		WWW w = new WWW(URLRenameCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			print(w.text); 											// donne le retour
			ApplicationModel.credits = System.Convert.ToInt32(w.text);
			myGameView.cards[idFocused].Title=newTitle;
			cardFocused.GetComponent<GameCard>().Card.Title=newTitle;
			cardFocused.GetComponent<GameCard>().ShowFace();
		}
		
	}


	public void sortCards (){
		
		int tempA=new int();
		int tempB=new int();
		
		
		for (int i = 1; i<cardsToBeDisplayed.Count; i++) {
			
			for (int j=0;j<i;j++){
				
				
				switch (sortSelected)
				{
				case 0:
					tempA = myGameView.cards[cardsToBeDisplayed[i]].Life;
					tempB = myGameView.cards[cardsToBeDisplayed[j]].Life;
					break;
				case 1:
					tempB = myGameView.cards[cardsToBeDisplayed[i]].Life;
					tempA = myGameView.cards[cardsToBeDisplayed[j]].Life;
					break;
				case 2:
					tempA = myGameView.cards[cardsToBeDisplayed[i]].Attack;
					tempB = myGameView.cards[cardsToBeDisplayed[j]].Attack;
					break;
				case 3:
					tempB = myGameView.cards[cardsToBeDisplayed[i]].Attack;
					tempA = myGameView.cards[cardsToBeDisplayed[j]].Attack;
					break;
				case 4:
					tempA = myGameView.cards[cardsToBeDisplayed[i]].Move;
					tempB = myGameView.cards[cardsToBeDisplayed[j]].Move;
					break;
				case 5:
					tempB = myGameView.cards[cardsToBeDisplayed[i]].Move;
					tempA = myGameView.cards[cardsToBeDisplayed[j]].Move;
					break;
				case 6:
					tempA = myGameView.cards[cardsToBeDisplayed[i]].Speed;
					tempB = myGameView.cards[cardsToBeDisplayed[j]].Speed;
					break;
				case 7:
					tempB = myGameView.cards[cardsToBeDisplayed[i]].Speed;
					tempA = myGameView.cards[cardsToBeDisplayed[j]].Speed;
					break;
				default:
					break;
				}

				if (tempA < tempB)
				{
					cardsToBeDisplayed.Insert(j,cardsToBeDisplayed[i]);
					cardsToBeDisplayed.RemoveAt(i+1);
					break;
				}		
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myGameScript : MonoBehaviour {

	#region variables

	//URL des fichiers PHP appelés par cette classe
	private string URLGetDecks = "http://54.77.118.214/GarrukServer/get_decks_by_user.php";
	private string URLGetCardsByDeck = "http://54.77.118.214/GarrukServer/get_cards_by_deck.php";
	private string URLGetMyCardsPage = "http://54.77.118.214/GarrukServer/get_mycardspage_data.php";
	private string URLAddNewDeck = "http://54.77.118.214/GarrukServer/add_new_deck.php";
	private string URLDeleteDeck = "http://54.77.118.214/GarrukServer/delete_deck.php";
	private string URLEditDeck = "http://54.77.118.214/GarrukServer/update_deck_name.php";
	private string URLAddCardToDeck = "http://54.77.118.214/GarrukServer/add_card_to_deck_by_user.php";
	private string URLRemoveCardFromDeck = "http://54.77.118.214/GarrukServer/remove_card_from_deck_by_user.php";
	private string URLSellCard = "http://54.77.118.214/GarrukServer/sellRandomCard.php";
	private string URLPutOnMarket = "http://54.77.118.214/GarrukServer/putonmarket.php";

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
	bool areCreatedDeckCards = false;
	float scaleDeck ;
	GameObject cardFocused ;
	bool displayFilters = false ;

	//Si l'utilisateur sélectionne une action (edit ou suppress) sur un des deck, donne à cette variable l'ID du deck en question
	int IDDeckToEdit = -1;

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
	private int chosenDeck = 0 ;
	private int chosenIdDeck = -1 ;
	private int chosenPage ;

	string[] skillsList;
	string[] cardTypeList;
	private IList<string> matchValues;
	private IList<Card> cards ;
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
	bool isMarketingCard = false ; 
	bool toReloadAll = false ;

	bool isBeingDragged = false;
	bool toReload = false ;
	bool recalculeFiltres = false;
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
	bool destroyFocus = false ;
	bool isDeckCardFocused = false ;
	bool isMarketingCardWindow = false ;

	GameObject[] displayedCards ;
	GameObject[] displayedDeckCards ;

	RaycastHit hit;
	Ray ray ;
	bool step1 = false ;

	#endregion

	void Update () {
		
		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			this.setStyles();
			this.applyFilters ();
			this.clearCards();
			this.clearDeckCards();
			this.createCards();
			this.createDeckCards();
		}
		if (toReload) {
			this.applyFilters ();
			this.displayPage();
			toReload = false ;
		}
		if (destroyAll){
			this.clearCards();
			this.clearDeckCards();
			isCreatedDeckCards = false ;
			isCreatedCards = false ;
			destroyAll=true;
			toReloadAll=true;
		}
		if (toReloadAll) {
			displayLoader = true ;
			displayFilters = false ;
			toReloadAll = false ;

			areDecksRetrived=false ;
			areCreatedDeckCards = false ;
			isLoadedCards = false ;
			isLoadedDeck = false ;
				
			StartCoroutine(getCards());
			StartCoroutine(RetrieveDecks());
		}
		if (isLoadedCards){
			this.createCards();
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
					myDecks[this.chosenDeck].NbCards--;
					StartCoroutine(this.RemoveCardFromDeck(this.cards[this.deckCardsIds[System.Convert.ToInt32(hit.collider.gameObject.name.Substring(8))]].Id, this.deckCardsIds.Count-1));
					deckCardsIds.RemoveAt (System.Convert.ToInt32(hit.collider.gameObject.name.Substring(8)));
					this.displayDeckCards();
					this.applyFilters ();
					this.displayPage ();
				}
				else if (hit.collider.name.StartsWith("Card")){
					if (this.deckCardsIds.Count!=5){
						deckCardsIds.Add (System.Convert.ToInt32(hit.collider.gameObject.name.Substring(4)));
						this.displayDeckCards();
						myDecks[this.chosenDeck].NbCards++;
						StartCoroutine(this.AddCardToDeck(this.cards[System.Convert.ToInt32(hit.collider.gameObject.name.Substring(4))].Id, this.deckCardsIds.Count));
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
						displayedCards[i].SetActive(false);
					}
					for(int i = 0 ; i < displayedDeckCards.Length ; i++){
						displayedDeckCards[i].SetActive(false);
					}

					cardFocused = Instantiate(CardObject) as GameObject;
					float scale = heightScreen/120f;
					cardFocused.transform.localScale = new Vector3(scale,scale,scale); 
					Vector3 vec = Camera.main.WorldToScreenPoint(cardFocused.collider.bounds.size);
					cardFocused.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0.50f*widthScreen ,0.45f*heightScreen-1 , 10)); 
					cardFocused.gameObject.name = "FocusedCard";	

					if (hit.collider.name.Contains("DeckCard")){
						cardId = cards[deckCardsIds[focusedCard]].Id;
						cardFocused.GetComponent<GameCard>().Card = cards[deckCardsIds[focusedCard]]; 
						focusedCardPrice = cards[deckCardsIds[focusedCard]].getCost();
					}
					else{
						cardId = cards[focusedCard].Id;
						cardFocused.GetComponent<GameCard>().Card = cards[focusedCard]; 
						focusedCardPrice = cards[focusedCard].getCost();
					}


					cardFocused.GetComponent<GameCard>().ShowFace();
					cardFocused.SetActive (true);
					rectFocus = new Rect(0.50f*widthScreen+(vec.x-widthScreen/2f)/2f, 0.15f*heightScreen, 0.25f*widthScreen, 0.8f*heightScreen);
				}
			}
		}

		if (destroySellingCardWindow){
			isSellingCard=false;
			destroySellingCardWindow = false ;
		}

		if (destroyFocus){
			isSellingCard=false;
			Destroy(cardFocused);
			this.focusedCard=-1;
			destroyFocus = false ;
			isLoadedDeck = true ;
			displayDecks=true ;
			displayFilters=true ;
		}
	}

	void Start() {
		this.setStyles(); 
		filtersCardType = new List<int> ();
		toReloadAll = true ;
	}

	void OnGUI()
	{
		if (this.focusedCard!=-1){
			if(isSellingCard){
				if(Event.current.keyCode==KeyCode.Return) {
					//this.destroyFocus();
					//StartCoroutine (this.sellCard(cardId, focusedCardPrice));
				}
				else{
				GUILayout.BeginArea(centralWindow);
				{
					GUILayout.BeginVertical(centralWindowStyle);
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label("Confirmer la vente de la carte pour la somme de "+focusedCardPrice+ " crédits", centralWindowTitleStyle);
						GUILayout.Space(0.02f*heightScreen);
						GUILayout.BeginHorizontal();
						{
							GUILayout.Space(0.03f*widthScreen);
							if (GUILayout.Button("Confirmer la vente",centralWindowButtonStyle)) // also can put width here
							{
								destroySellingCardWindow = true ;
								StartCoroutine (this.sellCard(cardId, focusedCardPrice));
							}
							GUILayout.Space(0.04f*widthScreen);
							if (GUILayout.Button("Annuler",centralWindowButtonStyle)) // also can put width here
							{
								destroySellingCardWindow = true ;
							}
							GUILayout.Space(0.03f*widthScreen);
						}
						GUILayout.EndHorizontal();
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
				}
			}
			else if(isMarketingCard){
				StartCoroutine (this.putOnMarket(cards[focusedCard].Id, cards[focusedCard].getCost()));
			}
			else{
				if(Event.current.keyCode==KeyCode.Escape) {
					//this.destroyFocus();
				}
				else{
				GUILayout.BeginArea(rectFocus);
				{
						GUILayout.BeginVertical();
						{
							if (GUILayout.Button("Vendre (+"+focusedCardPrice+" crédits)",focusButtonStyle)){
								isSellingCard = true ; 
							}
							if (GUILayout.Button("Mettre sur le marché",focusButtonStyle))
							{
								isMarketingCard = true ; 	
							}
						}
						GUILayout.EndVertical();
				}
				GUILayout.EndArea();
			}
		}
		}

		if (displayDecks) {
			if (IDDeckToEdit!=-1) {
				if(Event.current.keyCode==KeyCode.Escape) {
					IDDeckToEdit=-1;
					tempText = "Nouveau deck";
				}
				else if(Event.current.keyCode==KeyCode.Return) {
					StartCoroutine(this.deleteDeck());
					tempText = "Nouveau deck";
					IDDeckToEdit=-1;
				}
				else{
					GUILayout.BeginArea(centralWindow);
					{
						GUILayout.BeginVertical(centralWindowStyle);
						{
							GUILayout.FlexibleSpace();
							GUILayout.Label("Voulez-vous supprimer le deck ?", centralWindowTitleStyle);
							GUILayout.Space(0.02f*heightScreen);
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.03f*widthScreen);
								if (GUILayout.Button("Confirmer la suppression",centralWindowButtonStyle)) // also can put width here
								{
									StartCoroutine(this.deleteDeck());
									tempText = "Nouveau deck";
									IDDeckToEdit=-1;
								}
								GUILayout.Space(0.04f*widthScreen);
								if (GUILayout.Button("Annuler",centralWindowButtonStyle)) // also can put width here
								{
									displayCreationDeckWindow = false ; 
									IDDeckToEdit=-1;
								}
								GUILayout.Space(0.03f*widthScreen);
							}
							GUILayout.EndHorizontal();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndArea();
				}
			}
			if (displayCreationDeckWindow) {

				if(Event.current.keyCode==KeyCode.Escape) {
					displayCreationDeckWindow = false ;
					tempText = "Nouveau deck";
				}
				else if(Event.current.keyCode==KeyCode.Return) {
					StartCoroutine(this.addDeck());
					tempText = "Nouveau deck";
					displayCreationDeckWindow = false ;
				}
				else{
					GUILayout.BeginArea(centralWindow);
					{
						GUILayout.BeginVertical(centralWindowStyle);
						{
							GUILayout.FlexibleSpace();
							GUILayout.Label("Choisissez le nom de votre nouveau deck", centralWindowTitleStyle);
							GUILayout.Space(0.02f*heightScreen);
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.05f*widthScreen);
								tempText = GUILayout.TextField(tempText, centralWindowTextFieldStyle);
							}
							GUILayout.EndHorizontal();
							GUILayout.Space(0.02f*heightScreen);
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.03f*widthScreen);
								if (GUILayout.Button("Créer le deck",this.centralWindowButtonStyle)) // also can put width here
								{
									StartCoroutine(this.addDeck());
									tempText = "Nouveau deck";
									displayCreationDeckWindow = false ;
								}
								GUILayout.Space(0.04f*widthScreen);
								if (GUILayout.Button("Annuler",this.centralWindowButtonStyle)) // also can put width here
								{
									displayCreationDeckWindow = false ; 
									tempText = "Nouveau deck";
								}
								GUILayout.Space(0.03f*widthScreen);
							}
							GUILayout.EndHorizontal();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndArea();
				}
			}
			if (deckToEdit!=-1) {
				
				if(Event.current.keyCode==KeyCode.Escape) {
					deckToEdit=-1;
					tempText = "Nouveau deck";
				}
				else if(Event.current.keyCode==KeyCode.Return) {
					StartCoroutine(this.editDeck());
					tempText = "Nouveau deck";
					deckToEdit=-1;
				}
				else{
					GUILayout.BeginArea(centralWindow);
					{
						GUILayout.BeginVertical(centralWindowStyle);
						{
							GUILayout.FlexibleSpace();
							GUILayout.Label("Modifiez le nom de votre deck", centralWindowTitleStyle);
							GUILayout.Space(0.02f*heightScreen);
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.05f*widthScreen);
								tempText = GUILayout.TextField(tempText, centralWindowTextFieldStyle);
							}
							GUILayout.EndHorizontal();
							GUILayout.Space(0.02f*heightScreen);
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.03f*widthScreen);
								if (GUILayout.Button("Modifier",centralWindowButtonStyle)) // also can put width here
								{
									StartCoroutine(this.editDeck());
									tempText = "Nouveau deck";
									deckToEdit=-1;
								}
								GUILayout.Space(0.04f*widthScreen);
								if (GUILayout.Button("Annuler",centralWindowButtonStyle)) // also can put width here
								{
									deckToEdit=-1; 
									tempText = "Nouveau deck";
								}
								GUILayout.Space(0.03f*widthScreen);
							}
							GUILayout.EndHorizontal();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndArea();
				}
			}
			GUILayout.BeginArea(rectDeck);
			{
				GUILayout.BeginVertical();
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label(decksTitle,decksTitleStyle);
						GUILayout.FlexibleSpace();
						if (GUILayout.Button(myNewDeckButtonTitle, myNewDeckButton)){
							displayCreationDeckWindow = true ;
						}
					}
					GUILayout.EndHorizontal();

					GUILayout.Space(0.005f*heightScreen);

					scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(0.19f*widthScreen), GUILayout.Height(0.17f*heightScreen));

					for(int i = 0 ; i < myDecks.Count ; i++){	
						GUILayout.BeginHorizontal(myDecksGuiStyle[i]);
							{
							if (GUILayout.Button("("+myDecks[i].NbCards+") "+myDecks[i].Name,myDecksButtonGuiStyle[i]))
							{
								if (chosenDeck != i){
									myDecksGuiStyle[chosenDeck]=this.deckStyle;
									myDecksButtonGuiStyle[chosenDeck]=this.deckButtonStyle;
									chosenDeck = i;
									myDecksGuiStyle[i]=this.deckChosenStyle;
									myDecksButtonGuiStyle[i]=this.deckButtonChosenStyle;
									chosenIdDeck=myDecks[i].Id;
									StartCoroutine(this.RetrieveCardsFromDeck ());
								}
							}
							GUILayout.FlexibleSpace();
							if (GUILayout.Button("",myEditButtonStyle))
							{
								tempText = myDecks[i].Name;
								deckToEdit = myDecks[i].Id;
							}
						
							if (GUILayout.Button("",mySuppressButtonStyle))
								{
									IDDeckToEdit = myDecks[i].Id;
								}
							}
							GUILayout.EndHorizontal();
						}

					GUILayout.EndScrollView();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
	
		if (displayLoader){
			GUILayout.BeginArea(new Rect(widthScreen * 0.22f,0.26f*heightScreen,widthScreen * 0.78f,0.64f*heightScreen));
			{
				GUILayout.BeginVertical(); // also can put width in here
				{
					GUILayout.Label("Cartes en cours de chargement...   "+cardsToBeDisplayed.Count+" carte(s) chargee(s)");
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
		if (displayFilters){
			GUILayout.BeginArea(new Rect(widthScreen * 0.01f,0.965f*heightScreen,widthScreen * 0.78f,0.03f*heightScreen));
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (pageDebut>0){
						if (GUILayout.Button("...",paginationStyle)){
							pageDebut = pageDebut-15;
							pageFin = pageDebut+15;
						}
					}
					GUILayout.Space(widthScreen*0.01f);
					for (int i = pageDebut ; i < pageFin ; i++){
						if (GUILayout.Button(""+(i+1),paginatorGuiStyle[i])){
							paginatorGuiStyle[chosenPage]=this.paginationStyle;
							chosenPage=i;
							paginatorGuiStyle[i]=this.paginationActivatedStyle;
							displayPage();
						}
						GUILayout.Space(widthScreen*0.01f);
					}
					if (nbPages>pageFin){
						if (GUILayout.Button("...",paginationStyle)){
							pageDebut = pageDebut+15;
							pageFin = Mathf.Min(pageFin+15, nbPages);
						}
					}
					GUILayout.FlexibleSpace();
				}
			
			GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(0.80f*widthScreen,0.11f*heightScreen,widthScreen * 0.19f,0.85f*heightScreen));
			{
				bool toggle;
				string tempString ;
				GUILayout.BeginVertical();
				{
					GUILayout.Label ("Filtrer par classe",filterTitleStyle);
					for (int i=0; i<this.cardTypeList.Length-1; i++) {		
						toggle = GUILayout.Toggle (togglesCurrentStates [i], this.cardTypeList[i],this.toggleStyle);
						if (toggle != togglesCurrentStates [i]) {
							togglesCurrentStates [i] = toggle;
							if (toggle){
								filtersCardType.Add(i);
							}
							else{
								filtersCardType.Remove(i);
							}
							recalculeFiltres = true ;
								toReload = true ;
						}
					}
						
					GUILayout.FlexibleSpace();
					
					GUILayout.Label ("Filtrer une capacité",filterTitleStyle);
					tempString = GUILayout.TextField (this.valueSkill, this.filterTextFieldStyle);
					if (tempString != valueSkill) {
						if (tempString.Length > 0) {
							this.isSkillToDisplay=true;
							valueSkill = tempString.ToLower ();
							displaySkills ();
						} 
						else {
							this.isSkillToDisplay=false;
							valueSkill = "";
						}
						if (this.isSkillChosen){
							this.isSkillChosen=false ;
							recalculeFiltres = true ;
								toReload = true ;
						}
					}
					if (isSkillToDisplay){
						GUILayout.Space(-3);
						for (int j=0; j<matchValues.Count; j++) {
							if (GUILayout.Button (matchValues [j], myStyle)) {
								valueSkill = matchValues [j].ToLower ();
								this.isSkillChosen=true ;
								this.matchValues = new List<string>();
								recalculeFiltres = true ;
									toReload = true ;
							}
						}
					}
						
					GUILayout.FlexibleSpace();

					if (this.cardsToBeDisplayed.Count>0){
					GUILayout.Label ("Filtrer par Vie",filterTitleStyle);
					GUILayout.Space(-8);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:"+ Mathf.Round(minLifeVal),smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:"+ Mathf.Round(maxLifeVal),smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-5);
					MyGUI.MinMaxSlider (ref minLifeVal, ref maxLifeVal, minLifeLimit, maxLifeLimit);
						
					GUILayout.FlexibleSpace();
					
					GUILayout.Label ("Filtrer par Attaque",filterTitleStyle);
					GUILayout.Space(-8);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:"+ Mathf.Round(minAttackVal),smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:"+ Mathf.Round(maxAttackVal),smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-5);
					MyGUI.MinMaxSlider (ref minAttackVal, ref maxAttackVal, minAttackLimit, maxAttackLimit);

					GUILayout.FlexibleSpace();
					
					GUILayout.Label ("Filtrer par Mouvement",filterTitleStyle);
					GUILayout.Space(-8);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:"+ Mathf.Round(minMoveVal),smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:"+ Mathf.Round(maxMoveVal),smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-5);
					MyGUI.MinMaxSlider (ref minMoveVal, ref maxMoveVal, minMoveLimit, maxMoveLimit);

					GUILayout.FlexibleSpace();

					GUILayout.Label ("Filtrer par Rapidité",filterTitleStyle);
					GUILayout.Space(-8);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:"+ Mathf.Round(minQuicknessVal),smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:"+ Mathf.Round(maxQuicknessVal),smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-5);
					MyGUI.MinMaxSlider (ref minQuicknessVal, ref maxQuicknessVal, minQuicknessLimit, maxQuicknessLimit);

					if (Input.GetMouseButtonDown(0)){
						isBeingDragged = true;
					}
					
					if (Input.GetMouseButtonUp(0)){
						isBeingDragged = false;
					}

					if (!isBeingDragged){
						bool isMoved = false ;
						if (oldMaxLifeVal != maxLifeVal){
							oldMaxLifeVal = maxLifeVal;
							isMoved = true ; 
						}
						if (oldMinLifeVal != minLifeVal){
							oldMinLifeVal = minLifeVal;
							isMoved = true ; 
						}
						if (oldMaxAttackVal != maxAttackVal){
							oldMaxAttackVal = maxAttackVal;
							isMoved = true ; 
						}
						if (oldMinAttackVal != minAttackVal){
							oldMinAttackVal = minAttackVal;
							isMoved = true ; 
						}
						if (oldMaxMoveVal != maxMoveVal){
							oldMaxMoveVal = maxMoveVal;
							isMoved = true ; 
						}
						if (oldMinMoveVal != minMoveVal){
							oldMinMoveVal = minMoveVal;
							isMoved = true ; 
						}
						if (oldMaxQuicknessVal != maxQuicknessVal){
							oldMaxQuicknessVal = maxQuicknessVal;
							isMoved = true ; 
						}
						if (oldMinQuicknessVal != minQuicknessVal){
							oldMinQuicknessVal = minQuicknessVal;
							isMoved = true ; 
						}
//						if(isMoved){
//							toReload = true ;
//						}
						}
					}
				}
				GUILayout.EndVertical();	
			}
			GUILayout.EndArea();
		}
		
	}

	private IEnumerator addDeck() {
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

	private IEnumerator deleteDeck() {
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
			print (w.text);
			StartCoroutine(RetrieveDecks()); 
		}
	}

	private IEnumerator editDeck() {
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
			print (w.text);
			StartCoroutine(RetrieveDecks());
		}
	}

	private void setStyles() {

		heightScreen = Screen.height;
		widthScreen = Screen.width;

		if (heightScreen < widthScreen) {
			this.decksTitleStyle.fontSize = heightScreen*2/100;
			this.decksTitleStyle.fixedHeight = (int)heightScreen*3/100;
			this.decksTitleStyle.fixedWidth = (int)widthScreen*9/100;
			this.decksTitle = "Mes decks";

			this.myNewDeckButton.fontSize = heightScreen*2/100;
			this.myNewDeckButton.fixedHeight = heightScreen*3/100;
			this.myNewDeckButton.fixedWidth = widthScreen*9/100;
			this.myNewDeckButton.normal.background = this.backButton ;
			this.myNewDeckButton.hover.background = this.backActivatedButton ;
			this.myNewDeckButtonTitle = "Nouveau";

			this.centralWindow = new Rect (widthScreen * 0.25f, 0.12f * heightScreen, widthScreen * 0.50f, 0.18f * heightScreen);

			this.centralWindowStyle.fixedWidth = widthScreen*0.5f-5;

			this.centralWindowTitleStyle.fontSize = heightScreen*2/100;
			this.centralWindowTitleStyle.fixedHeight = heightScreen*3/100;
			this.centralWindowTitleStyle.fixedWidth = widthScreen*5/10;

			this.centralWindowTextFieldStyle.fontSize = heightScreen*2/100;
			this.centralWindowTextFieldStyle.fixedHeight = heightScreen*3/100;
			this.centralWindowTextFieldStyle.fixedWidth = widthScreen*4/10;

			this.centralWindowButtonStyle.fontSize = heightScreen*2/100;
			this.centralWindowButtonStyle.fixedHeight = heightScreen*3/100;
			this.centralWindowButtonStyle.fixedWidth = widthScreen*2/10;

			rectDeck = new Rect (widthScreen * 0.005f, 0.105f * heightScreen, widthScreen * 0.19f, 0.21f * heightScreen);
			this.rectInsideScrollDeck = new Rect (widthScreen * 0.005f, 0.12f * heightScreen, widthScreen * 0.18f, 0.18f * heightScreen);
			this.rectOutsideScrollDeck = new Rect (widthScreen * 0.005f, 0.12f * heightScreen, widthScreen * 0.19f, 0.18f * heightScreen);

			this.deckStyle.fixedHeight = heightScreen*3/100;
			this.deckStyle.fixedWidth = widthScreen*17/100;

			this.deckChosenStyle.fixedHeight = heightScreen*3/100;
			this.deckChosenStyle.fixedWidth = widthScreen*17/100;

			this.deckButtonStyle.fontSize = heightScreen*2/100;
			this.deckButtonStyle.fixedHeight = heightScreen*3/100;
			this.deckButtonStyle.fixedWidth = widthScreen*12/100;
			
			this.deckButtonChosenStyle.fontSize = heightScreen*2/100;
			this.deckButtonChosenStyle.fixedHeight = heightScreen*3/100;
			this.deckButtonChosenStyle.fixedWidth = widthScreen*12/100;

			this.myEditButtonStyle.fixedHeight = heightScreen*3/100;
			this.myEditButtonStyle.fixedWidth = heightScreen*3/100;

			this.mySuppressButtonStyle.fixedHeight = heightScreen*3/100;
			this.mySuppressButtonStyle.fixedWidth = heightScreen*3/100;

			this.paginationStyle.fontSize = heightScreen*2/100;
			this.paginationStyle.fixedWidth = widthScreen*3/100;
			this.paginationStyle.fixedHeight = heightScreen*3/100;
			this.paginationActivatedStyle.fontSize = heightScreen*2/100;
			this.paginationActivatedStyle.fixedWidth = widthScreen*3/100;
			this.paginationActivatedStyle.fixedHeight = heightScreen*3/100;

			this.filterTitleStyle.fixedWidth = widthScreen*19/100;
			this.filterTitleStyle.fixedHeight = heightScreen*3/100;
			this.filterTitleStyle.fontSize = heightScreen*2/100;

			this.toggleStyle.fixedWidth = widthScreen*19/100;
			this.toggleStyle.fixedHeight = heightScreen*20/1000;
			this.toggleStyle.fontSize = heightScreen*15/1000;

			this.filterTextFieldStyle.fontSize = heightScreen*2/100;
			this.filterTextFieldStyle.fixedHeight = heightScreen*3/100;
			this.filterTextFieldStyle.fixedWidth = widthScreen*19/100;

			this.myStyle.fontSize = heightScreen*15/1000;
			this.myStyle.fixedHeight = heightScreen*20/1000;
			this.myStyle.fixedWidth = widthScreen*19/100;

			this.smallPoliceStyle.fontSize = heightScreen*15/1000;
			this.smallPoliceStyle.fixedHeight = heightScreen*20/1000;
			this.smallPoliceStyle.fixedWidth = widthScreen*5/100;

			this.focusButtonStyle.fontSize = heightScreen*3/100;
			this.focusButtonStyle.fixedHeight = heightScreen*6/100;
			this.focusButtonStyle.fixedWidth = widthScreen*25/100;
		}
		else{
			this.decksTitleStyle.fontSize = heightScreen*2/100;
			this.decksTitleStyle.fixedHeight = heightScreen*3/100;
			this.decksTitleStyle.fixedWidth = widthScreen*12/100;
			this.decksTitle = "Decks";
			
			this.myNewDeckButton.fontSize = heightScreen*2/100;
			this.myNewDeckButton.fixedHeight = heightScreen*3/100;
			this.myNewDeckButton.fixedWidth = heightScreen*3/100;
			this.myNewDeckButton.normal.background = this.backNewDeckButton ;
			this.myNewDeckButton.hover.background = this.backHoveredNewDeckButton ;
			this.myNewDeckButtonTitle = "";

			this.centralWindow = new Rect (widthScreen * 0.10f, 0.10f * heightScreen, widthScreen * 0.80f, 0.80f * heightScreen);
			this.centralWindowTitleStyle.fontSize = heightScreen*2/100;
			this.centralWindowTitleStyle.fixedHeight = heightScreen*3/100;
			this.centralWindowTitleStyle.fixedWidth = widthScreen*5/10;

			this.centralWindowTextFieldStyle.fontSize = heightScreen*1/100;
			this.centralWindowTextFieldStyle.fixedHeight = heightScreen*3/100;
			this.centralWindowTextFieldStyle.fixedWidth = widthScreen*7/10;
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
		int nbFilters = this.filtersCardType.Count;
		bool testFilters = false;
		bool testDeck = false;
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
		if (this.isSkillChosen){
			int max = this.cards.Count;
			if (nbFilters==0){
				max = this.cards.Count;
				if (testFilters){
					for (int i = 0; i < max ; i++) {
						if (cards[i].hasSkill(this.valueSkill)){
							if (cards[i].verifyC(minLifeVal,maxLifeVal,minAttackVal,maxAttackVal,minMoveVal,maxMoveVal,minQuicknessVal,maxQuicknessVal)){
								testDeck = false ;
								for (int j = 0 ; j < deckCardsIds.Count ; j++){
									if (i==deckCardsIds[j]){
										testDeck = true ; 
									}
								}
								if (!testDeck){
									this.cardsToBeDisplayed.Add(i);
								}
							}
						}
					}
				}
				else{
					for (int i = 0; i < max ; i++) {
						if (cards[i].hasSkill(this.valueSkill)){
							testDeck = false ;
							for (int j = 0 ; j < deckCardsIds.Count ; j++){
								if (i==deckCardsIds[j]){
									testDeck = true ; 
								}
							}
							if (!testDeck){
								this.cardsToBeDisplayed.Add(i);
							}
						}
					}
				}
			}
			else{
				bool test ;
				int j;
				if (testFilters){
					for (int i = 0; i < max ; i++) {
						test = false ;
						j = 0 ;
						while (!test && j!=nbFilters){
							if (cards[i].IdClass==this.filtersCardType[j]){
								test=true ;
								if (cards[i].hasSkill(this.valueSkill)){
									if (cards[i].verifyC(minLifeVal,maxLifeVal,minAttackVal,maxAttackVal,minMoveVal,maxMoveVal,minQuicknessVal,maxQuicknessVal)){
										testDeck = false ;
										for (int k = 0 ; k < deckCardsIds.Count ; k++){
											if (i==deckCardsIds[k]){
												testDeck = true ; 
											}
										}
										if (!testDeck){
											this.cardsToBeDisplayed.Add(i);
										}
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
						j = 0 ;
						while (!test && j!=nbFilters){
							if (cards[i].IdClass==this.filtersCardType[j]){
								test=true ;
								if (cards[i].hasSkill(this.valueSkill)){
									testDeck = false ;
									for (int k = 0 ; k < deckCardsIds.Count ; k++){
										if (i==deckCardsIds[k]){
											testDeck = true ; 
										}
									}
									if (!testDeck){
										this.cardsToBeDisplayed.Add(i);
									}
								}
							}
							j++;
						}
					}
				}
			}
		}
		else{
			if (nbFilters==0){
				int max = this.cards.Count;
				if (testFilters){
					for (int i = 0; i < max ; i++) {
						if (cards[i].verifyC(minLifeVal,maxLifeVal,minAttackVal,maxAttackVal,minMoveVal,maxMoveVal,minQuicknessVal,maxQuicknessVal)){
							testDeck = false ;
							for (int k = 0 ; k < deckCardsIds.Count ; k++){
								if (i==deckCardsIds[k]){
									testDeck = true ; 
								}
							}
							if (!testDeck){
								this.cardsToBeDisplayed.Add(i);
							}
						}
					}
				}
				else{
					for (int i = 0; i < max ; i++) {
						testDeck = false ;
						for (int j = 0 ; j < deckCardsIds.Count ; j++){
							if (i==deckCardsIds[j]){
								testDeck = true ; 
							}
						}
						if (!testDeck){
							this.cardsToBeDisplayed.Add(i);
						}
					}
				}
			}
			else{
				int max = this.cards.Count;
				bool test ;
				int j;
				if (testFilters){
					for (int i = 0; i < max ; i++) {
						test = false ;
						j = 0 ;
						while (!test && j!=nbFilters){
							if (cards[i].IdClass==this.filtersCardType[j]){
								test=true ;
								if (cards[i].verifyC(minLifeVal,maxLifeVal,minAttackVal,maxAttackVal,minMoveVal,maxMoveVal,minQuicknessVal,maxQuicknessVal)){
									testDeck = false ;
									for (int k = 0 ; k < deckCardsIds.Count ; k++){
										if (i==deckCardsIds[k]){
											testDeck = true ; 
										}
									}
									if (!testDeck){
										this.cardsToBeDisplayed.Add(i);
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
						j = 0 ;
						while (!test && j!=nbFilters){
							if (cards[i].IdClass==this.filtersCardType[j]){
								test=true ;
								testDeck = false ;
								for (int k = 0 ; k < deckCardsIds.Count ; k++){
									if (i==deckCardsIds[k]){
										testDeck = true ; 
									}
								}
								if (!testDeck){
									this.cardsToBeDisplayed.Add(i);
								}
							}
							j++;
						}
					}
				}
			}
		}

		if (recalculeFiltres) {
			bool minLifeBool = (minLifeLimit==minLifeVal);
			bool maxLifeBool = (maxLifeLimit==maxLifeVal);
			bool minMoveBool = (minMoveLimit==minMoveVal);
			bool maxMoveBool = (maxMoveLimit==maxMoveVal);
			bool minQuicknessBool = (minQuicknessLimit==minQuicknessVal);
			bool maxQuicknessBool = (maxQuicknessLimit==maxQuicknessVal);
			bool minAttackBool = (minAttackLimit==minAttackVal);
			bool maxAttackBool = (maxAttackLimit==maxAttackVal);
			minLifeLimit=10000;
			maxLifeLimit=0;
			minAttackLimit=10000;
			maxAttackLimit=0;
			minMoveLimit=10000;
			maxMoveLimit=0;
			minQuicknessLimit=10000;
			maxQuicknessLimit=0;
			if (this.isSkillChosen){
				int max = this.cards.Count;
				if (nbFilters==0){
					max = this.cards.Count;
					for (int i = 0; i < max ; i++) {
						if (cards[i].hasSkill(this.valueSkill)){
							if (this.cards[i].Life<minLifeLimit){
								minLifeLimit = this.cards[i].Life;
							}
							if (this.cards[i].Life>maxLifeLimit){
								maxLifeLimit = this.cards[i].Life;
							}
							if (this.cards[i].Attack<minAttackLimit){
								minAttackLimit = this.cards[i].Attack;
							}
							if (this.cards[i].Attack>maxAttackLimit){
								maxAttackLimit = this.cards[i].Attack;
							}
							if (this.cards[i].Move<minMoveLimit){
								minMoveLimit = this.cards[i].Move;
							}
							if (this.cards[i].Move>maxMoveLimit){
								maxMoveLimit = this.cards[i].Move;
							}
							if (this.cards[i].Speed<minQuicknessLimit){
								minQuicknessLimit = this.cards[i].Speed;
							}
							if (this.cards[i].Speed>maxQuicknessLimit){
								maxQuicknessLimit = this.cards[i].Speed;
							}
						}
					}
				}
				else{
					bool test ;
					int j;
					for (int i = 0; i < max ; i++) {
						test = false ;
						j = 0 ;
						while (!test && j!=nbFilters){
							if (cards[i].IdClass==this.filtersCardType[j]){
								test=true ;
								if (cards[i].hasSkill(this.valueSkill)){
									if (minLifeBool){
										minLifeVal=minLifeLimit;
									}
									else if (this.cards[i].Life<minLifeLimit){
										minLifeLimit = this.cards[i].Life;
									}
									if (maxLifeBool){
										maxLifeVal=maxLifeLimit;
									}
									else if (this.cards[i].Life>maxLifeLimit){
										maxLifeLimit = this.cards[i].Life;
									}
									if (minAttackBool){
										minAttackVal=minAttackLimit;
									}
									else if (this.cards[i].Attack<minAttackLimit){
										minAttackLimit = this.cards[i].Attack;
									}
									if (maxAttackBool){
										maxAttackVal=maxAttackLimit;
									}
									else if (this.cards[i].Attack>maxAttackLimit){
										maxAttackLimit = this.cards[i].Attack;
									}
									if (minMoveBool){
										minMoveVal=minMoveLimit;
									}
									else if (this.cards[i].Move<minMoveLimit){
										minMoveLimit = this.cards[i].Move;
									}
									if (maxMoveBool){
										maxMoveVal=maxMoveLimit;
									}
									else if (this.cards[i].Move>maxMoveLimit){
										maxMoveLimit = this.cards[i].Move;
									}
									if (minQuicknessBool){
										minQuicknessVal=minQuicknessLimit;
									}
									else if (this.cards[i].Speed<minQuicknessLimit){
										minQuicknessLimit = this.cards[i].Speed;
									}
									if(maxQuicknessBool){
										maxQuicknessVal=maxQuicknessLimit;
									}
									else if (this.cards[i].Speed>maxQuicknessLimit){
										maxQuicknessLimit = this.cards[i].Speed;
									}
								}
							}
							j++;
						}
					}
				}
			}
			else{
				if (nbFilters==0){
					int max = this.cards.Count;
					for (int i = 0; i < max ; i++) {
						if (this.cards[i].Life<minLifeLimit){
							minLifeLimit = this.cards[i].Life;
						}
						if (this.cards[i].Life>maxLifeLimit){
							maxLifeLimit = this.cards[i].Life;
						}
						if (this.cards[i].Attack<minAttackLimit){
							minAttackLimit = this.cards[i].Attack;
						}
						if (this.cards[i].Attack>maxAttackLimit){
							maxAttackLimit = this.cards[i].Attack;
						}
						if (this.cards[i].Move<minMoveLimit){
							minMoveLimit = this.cards[i].Move;
						}
						if (this.cards[i].Move>maxMoveLimit){
							maxMoveLimit = this.cards[i].Move;
						}
						if (this.cards[i].Speed<minQuicknessLimit){
							minQuicknessLimit = this.cards[i].Speed;
						}
						if (this.cards[i].Speed>maxQuicknessLimit){
							maxQuicknessLimit = this.cards[i].Speed;
						}
					}
				}
				else{
					int max = this.cards.Count;
					bool test ;
					int j;
					for (int i = 0; i < max ; i++) {
						test = false ;
						j = 0 ;
						while (!test && j!=nbFilters){
							if (cards[i].IdClass==this.filtersCardType[j]){
								test=true ;
								if (this.cards[i].Life<minLifeLimit){
									minLifeLimit = this.cards[i].Life;
								}
								if (this.cards[i].Life>maxLifeLimit){
									maxLifeLimit = this.cards[i].Life;
								}
								if (this.cards[i].Attack<minAttackLimit){
									minAttackLimit = this.cards[i].Attack;
								}
								if (this.cards[i].Attack>maxAttackLimit){
									maxAttackLimit = this.cards[i].Attack;
								}
								if (this.cards[i].Move<minMoveLimit){
									minMoveLimit = this.cards[i].Move;
								}
								if (this.cards[i].Move>maxMoveLimit){
									maxMoveLimit = this.cards[i].Move;
								}
								if (this.cards[i].Speed<minQuicknessLimit){
									minQuicknessLimit = this.cards[i].Speed;
								}
								if (this.cards[i].Speed>maxQuicknessLimit){
									maxQuicknessLimit = this.cards[i].Speed;
								}
							}
							j++;
						}
					}
				}
			}
			recalculeFiltres=false;

			if (minLifeBool || minLifeVal<minLifeLimit){
				minLifeVal = minLifeLimit;
			}
			if (maxLifeBool || maxLifeVal>maxLifeLimit){
				maxLifeVal = maxLifeLimit;
			}
			if (minAttackBool || minAttackVal<minAttackLimit){
				minAttackVal = minAttackLimit;
			}
			if (maxAttackBool || maxAttackVal>maxAttackLimit){
				maxAttackVal = maxAttackLimit;
			}
			if (minMoveBool || minMoveVal<minMoveLimit){
				minMoveVal = minMoveLimit;
			}
			if (maxMoveBool || maxMoveVal>maxMoveLimit){
				maxMoveVal = maxMoveLimit;
			}
			if (minQuicknessBool || minQuicknessVal<minQuicknessLimit){
				minQuicknessVal = minQuicknessLimit;
			}
			if (maxQuicknessBool || maxQuicknessVal>maxQuicknessLimit){
				maxQuicknessVal = maxQuicknessLimit;
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
						
		int max = this.cards.Count;
		for (int i = 0; i < max ; i++) {
			if (this.cards[i].Life<minLifeLimit){
				minLifeLimit = this.cards[i].Life;
			}
			if (this.cards[i].Life>maxLifeLimit){
				maxLifeLimit = this.cards[i].Life;
			}
			if (this.cards[i].Attack<minAttackLimit){
				minAttackLimit = this.cards[i].Attack;
			}
			if (this.cards[i].Attack>maxAttackLimit){
				maxAttackLimit = this.cards[i].Attack;
			}
			if (this.cards[i].Move<minMoveLimit){
				minMoveLimit = this.cards[i].Move;
			}
			if (this.cards[i].Move>maxMoveLimit){
				maxMoveLimit = this.cards[i].Move;
			}
			if (this.cards[i].Speed<minQuicknessLimit){
				minQuicknessLimit = this.cards[i].Speed;
			}
			if (this.cards[i].Speed>maxQuicknessLimit){
				maxQuicknessLimit = this.cards[i].Speed;
			}
		}
	}

	IEnumerator RetrieveDecks() {
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
			//print(w.text); 											// donne le retour
			
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

	private IEnumerator getCards() {

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
//			print (w.text);
			
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			cardsIDS=data[2].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
			skillsIds = data[1].Split('\n');

			this.cardTypeList = data[0].Split('\n');
			togglesCurrentStates = new bool[this.cardTypeList.Length];
			for(int i = 0 ; i < this.cardTypeList.Length-1 ; i++){
				togglesCurrentStates[i] = false;
			}
			this.skillsList = new string[skillsIds.Length];
			for(int i = 0 ; i < skillsIds.Length-1 ; i++){
				tempString = skillsIds[i].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
				if (i>0){
					this.skillsList[i-1]=tempString[0];
				}
			}
	
			this.cards = new List<Card>();
			this.cardsIds = new List<int>();

			for(int i = 0 ; i < cardsIDS.Length-1 ; i++){
				cardInfo = cardsIDS[i].Split('\n');
				for(int j = 1 ; j < cardInfo.Length-1 ; j++){
					cardInfo2 = cardInfo[j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
					if (j==1){
						this.cards.Add(new Card(System.Convert.ToInt32(cardInfo2[0]), // id
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
						                        System.Convert.ToInt32(cardInfo2[10]), // speedlevel
						                        System.Convert.ToInt32(cardInfo2[11]))); // attacklevel;
						this.cards[i].Skills = new List<Skill>();
						this.cardsIds.Add(System.Convert.ToInt32(cardInfo2[0]));
						this.cardsToBeDisplayed.Add(i);
					}
					else{
						tempInt = System.Convert.ToInt32(cardInfo2[0]);
						this.cards[i].Skills.Add(new Skill (skillsList[System.Convert.ToInt32(cardInfo2[0])], 
						                                      System.Convert.ToInt32(cardInfo2[0]), // idskill
						                                      System.Convert.ToInt32(cardInfo2[1]), // isactivated
						                                      System.Convert.ToInt32(cardInfo2[2]), // level
						                                      System.Convert.ToInt32(cardInfo2[3]), // power
						                                      System.Convert.ToInt32(cardInfo2[4]),
						                                      cardInfo2[5])); // costmana
					
					}

				}
			}
		}
		isLoadedCards=true;
		isDisplayedCards = true ;
	}

	private void createCards(){
		
		float tempF = 10f*widthScreen/heightScreen;
		float width = tempF * 0.78f;
		nbCardsPerRow = Mathf.FloorToInt(width/1.6f);
		float debutLargeur = -0.49f * tempF + 0.8f + (width - 1.6f * nbCardsPerRow)/2 ;
		displayedCards = new GameObject[3*nbCardsPerRow];
		int nbCardsToDisplay = this.cardsToBeDisplayed.Count;
		for(int i = 0 ; i < 3*nbCardsPerRow ; i++){
			displayedCards[i] = Instantiate(CardObject) as GameObject;
			displayedCards[i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
			displayedCards[i].transform.localPosition = new Vector3(debutLargeur + 1.6f*(i%nbCardsPerRow), 0.8f-(i-i%nbCardsPerRow)/nbCardsPerRow*2.2f, 0); 
			displayedCards[i].gameObject.name = "Card" + i + "";	
			if (i<nbCardsToDisplay){
				displayedCards[i].GetComponent<GameCard>().Card = cards[this.cardsToBeDisplayed[i]]; 
				displayedCards[i].GetComponent<GameCard>().ShowFace();
			}   
			else{
				displayedCards[i].SetActive (false);
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
				displayedCards[i-start].gameObject.name = "Card" + this.cardsToBeDisplayed[i] + "";
				displayedCards[i-start].SetActive(true);
				displayedCards[i-start].GetComponent<GameCard>().Card = cards[this.cardsToBeDisplayed[i]]; 
				displayedCards[i-start].GetComponent<GameCard>().ShowFace();
			}
			else{
				displayedCards[i-start].SetActive(false);
			}
		}
	}

	private void clearCards(){
		for (int i = 0; i < 3*nbCardsPerRow; i++) {
			Destroy(displayedCards[i]);
		}
	}

	private void clearDeckCards(){
		for (int i = 0; i < 5; i++) {
			Destroy(displayedDeckCards[i]);
		}
	}

	private IEnumerator RetrieveCardsFromDeck(){
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
			int tempInt2 = this.cards.Count;
			bool tempBool ;
			int j = 0 ;
			string[] cardDeckEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
			deckCardsIds = new List<int>();
			for(int i = 0 ; i < cardDeckEntries.Length-1 ; i++){
				tempInt = System.Convert.ToInt32(cardDeckEntries[i]);
				tempBool = true ; 
				j = 0 ;
				while (tempBool && j<tempInt2){
					if (cards[j].Id == tempInt){
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
			displayedDeckCards[i].transform.localScale = new Vector3(scaleDeck,scaleDeck,scaleDeck); 
			displayedDeckCards[i].transform.localPosition = new Vector3(debutLargeur + (scaleDeck+pas)*i , 2.9f, 0); 
			displayedDeckCards[i].gameObject.name = "DeckCard" + i + "";	
			if (i<nbDeckCardsToDisplay){
				displayedDeckCards[i].GetComponent<GameCard>().Card = cards[this.deckCardsIds[i]]; 
				displayedDeckCards[i].GetComponent<GameCard>().ShowFace();
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
				displayedDeckCards[i].GetComponent<GameCard>().Card = cards[this.deckCardsIds[i]]; 
				displayedDeckCards[i].GetComponent<GameCard>().ShowFace();
			}   
			else{
				displayedDeckCards[i].SetActive (false);
			}
		}
	}

	private IEnumerator AddCardToDeck(int idCard, int cardsCount)
	{		
		WWWForm form = new WWWForm (); 						
		form.AddField ("myform_hash", ApplicationModel.hash);
		form.AddField ("myform_nick", ApplicationModel.username);
		form.AddField ("myform_deck", this.chosenIdDeck);
		form.AddField ("myform_idCard", idCard);
		form.AddField ("myform_nbCards", cardsCount);
		
		WWW w = new WWW (URLAddCardToDeck, form); 								
		yield return w; 											
		if (w.error != null) 
		{
			print (w.error); 										
		} else {
			//print ("deck : "+this.chosenIdDeck+ " idcard : "+idCard+" compteurCartes : "+cardsCount+w.text);
		}
	}

	private IEnumerator RemoveCardFromDeck(int idCard, int cardsCount)
	{		
		WWWForm form = new WWWForm (); 								// Création de la connexion
		form.AddField ("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField ("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField ("myform_deck", this.chosenIdDeck);
		form.AddField ("myform_idCard", idCard);
		form.AddField ("myform_nbCards", cardsCount);				// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW (URLRemoveCardFromDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			// print ("REUSSI : "+w.text);
		}
	}

	private IEnumerator sellCard(int idCard, int cost){
		
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
			//print(w.text); 											// donne le retour
			//money = System.Convert.ToInt32(w.text);
			this.focusedCard=-1;
			destroyAll=true ;
		}	
	}
	
	
	private IEnumerator putOnMarket(int cardId, int price){
		
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
			print(w.text); 											// donne le retour
		}
		
	}
}

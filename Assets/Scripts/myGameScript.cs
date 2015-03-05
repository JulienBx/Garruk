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
	public GUIStyle centralWindowTitleStyle ;
	public GUIStyle centralWindowTextFieldStyle ;
	public GUIStyle centralWindowButtonStyle ;
	public GUIStyle deckStyle ;
	public GUIStyle deckChosenStyle ;
	public GUIStyle mySuppressButtonStyle ;
	public GUIStyle myEditButtonStyle ;
	public GUIStyle textDeckStyle ;

	//Si l'utilisateur sélectionne une action (edit ou suppress) sur un des deck, donne à cette variable l'ID du deck en question
	int IDDeckToEdit = -1;

	Rect rectDeck ;

	#endregion

	#region variablesAClasser
	private IList<Deck> myDecks;
	public Texture2D backCard ;
	GameObject myDecksPanel ;
	GUIStyle[] myDecksGuiStyle ;
	GUIStyle[] paginatorGuiStyle;
	private int chosenDeck = 0 ;
	private int chosenIdDeck ;
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
	int isLoadedCards = 0 ;
	bool isDisplayedCards = true ;
	int nbCardsPerRow = 0 ;
	int widthScreen = Screen.width ; 
	int heightScreen = Screen.height ;
	int nbPages ;
	private bool[] togglesCurrentStates;
	private string valueSkill="";
	bool isSkillToDisplay = false ;
	bool isSkillChosen = false ;
	GUIStyle smallPoliceStyle;
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
	GUIStyle myStyle;
	bool isBeingDragged = false;
	bool toReload = false ;
	bool recalculeFiltres = false;
	bool confirmSuppress ;
	Vector2 scrollPosition = new Vector2(0,0) ;
	bool displayCreationDeckWindow  = false ;
	string tempText = "Nouveau deck" ;


	int deckToEdit = -1;

	GameObject[] displayedCards ;
	GameObject[] displayedDeckCards ;

	GameObject clickedCard = null;
	GameObject zoomedCard = null;
	bool cardIsMoving = false;
	Vector3 destination;
	Vector3 destinationScale;
	Vector3 origin;
	float speed = 55f;
	RaycastHit hit;
	Ray ray ;

	int idCardHovered = -1 ;
	int selectedCard = -1 ;

	#endregion

	void Update () {
		
		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			this.applyFilters();
			StartCoroutine(this.setStyles());
			StartCoroutine(clearCards());
			StartCoroutine(clearDeckCards());
			this.createCards();
			this.createDeckCards();
		}
		if (toReload) {
			StartCoroutine(this.applyFilters ());
			StartCoroutine(this.displayPage ());
			toReload = false ;
		}

		if (idCardHovered == -1) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if(Physics.Raycast(ray, out hit))
			{
				if (hit.collider.name.StartsWith("Card") || hit.collider.name.StartsWith("DeckCard") || hit.collider.name.StartsWith("Skill")){
					if (hit.collider.name.StartsWith("Skill")){
						clickedCard = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
					}
					else{
						clickedCard = hit.collider.gameObject;
					}
					idCardHovered = clickedCard.GetComponent<GameCard>().Card.Id;
					zoomedCard = Instantiate(CardObject) as GameObject;
					zoomedCard.transform.localScale= new Vector3(1f,1f,1f); 
					zoomedCard.GetComponent<GameCard>().Card=clickedCard.GetComponent<GameCard>().Card;
					origin = clickedCard.transform.localPosition;
					zoomedCard.transform.localPosition=origin;
					zoomedCard.GetComponent<GameCard>().ShowFace();
					zoomedCard.gameObject.name = "ZoomedCard";
					cardIsMoving = true;
				}
			}
		}
		else{
			if (Input.GetMouseButtonDown(0)) {
				if (selectedCard!=idCardHovered) {
					selectedCard = idCardHovered;
				}
				else{
					selectedCard = -1;
				}
			}
			else{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if(Physics.Raycast(ray, out hit)){
					if (hit.collider.name.StartsWith("Card") || hit.collider.name.StartsWith("DeckCard") || hit.collider.name.StartsWith("Skill")){
						if (hit.collider.name.StartsWith("Skill")){
							clickedCard = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
						}
						else{
							clickedCard = hit.collider.gameObject;
						}
						if (clickedCard.GetComponent<GameCard>().Card.Id!=idCardHovered && selectedCard==-1){
							zoomedCard.transform.localScale= new Vector3(1f,1f,1f); 
							zoomedCard.GetComponent<GameCard>().Card=clickedCard.GetComponent<GameCard>().Card;
							origin = clickedCard.transform.localPosition;
							zoomedCard.transform.localPosition=origin;
							zoomedCard.GetComponent<GameCard>().ShowFace();
							zoomedCard.gameObject.name = "ZoomedCard";
							cardIsMoving = true;

							idCardHovered = clickedCard.GetComponent<GameCard>().Card.Id;
						}
					}
				}
			}
		}

		if (cardIsMoving) {
			zoomedCard.transform.position = Vector3.MoveTowards(zoomedCard.transform.position, destination, speed * Time.deltaTime);
			float percentageDistance = 1f-(Vector3.Distance(zoomedCard.transform.position, destination)/Vector3.Distance(origin, destination));
			zoomedCard.transform.localScale = new Vector3(destinationScale.x*percentageDistance, destinationScale.y*percentageDistance,destinationScale.z*percentageDistance) ;
			if(Vector3.Distance(zoomedCard.transform.position, destination) < 0.001f){
				cardIsMoving=false;
			}
		}
	}

	void Start() {
		StartCoroutine(setStyles());
		StartCoroutine(RetrieveDecks()); 
		
		filtersCardType = new List<int> ();
		StartCoroutine(getCards()); 
	}

	void OnGUI()
	{
		if (areDecksRetrived) {
			float heightDeckElements = heightScreen/30f;

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
							GUILayout.Label("Voulez-vous supprimer le deck ?"+tempText, decksTitleStyle, GUILayout.Width(0.40f*widthScreen), GUILayout.Height(0.04f*heightScreen));
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.01f*widthScreen);
								if (GUILayout.Button("Confirmer la suppression",myNewDeckButton, GUILayout.Width(0.18f*widthScreen), GUILayout.Height(0.03f*heightScreen))) // also can put width here
								{
									StartCoroutine(this.deleteDeck());
									tempText = "Nouveau deck";
									IDDeckToEdit=-1;
								}
								GUILayout.Space(0.02f*widthScreen);
								if (GUILayout.Button("Annuler",myNewDeckButton, GUILayout.Width(0.18f*widthScreen), GUILayout.Height(0.03f*heightScreen))) // also can put width here
								{
									displayCreationDeckWindow = false ; 
									IDDeckToEdit=-1;
								}
								GUILayout.Space(0.01f*widthScreen);
							}
							GUILayout.EndHorizontal();
							GUILayout.Space(0.01f*heightScreen);
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
								if (GUILayout.Button("Creer le deck",this.centralWindowButtonStyle)) // also can put width here
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
							GUILayout.Label("Modifiez le nom de votre deck", decksTitleStyle, GUILayout.Width(0.40f*widthScreen), GUILayout.Height(0.04f*heightScreen));
							tempText = GUILayout.TextField(tempText, GUILayout.Width(0.39f*widthScreen), GUILayout.Height(0.04f*heightScreen));
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.01f*widthScreen);
								if (GUILayout.Button("Modifier",myNewDeckButton, GUILayout.Width(0.18f*widthScreen), GUILayout.Height(0.03f*heightScreen))) // also can put width here
								{
									StartCoroutine(this.editDeck());
									tempText = "Nouveau deck";
									deckToEdit=-1;
								}
								GUILayout.Space(0.02f*widthScreen);
								if (GUILayout.Button("Annuler",myNewDeckButton, GUILayout.Width(0.18f*widthScreen), GUILayout.Height(0.03f*heightScreen))) // also can put width here
								{
									deckToEdit=-1; 
									tempText = "Nouveau deck";
								}
								GUILayout.Space(0.01f*widthScreen);
							}
							GUILayout.EndHorizontal();
							GUILayout.Space(0.01f*heightScreen);
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

					scrollPosition = GUILayout.BeginScrollView(scrollPosition);

					for(int i = 0 ; i < myDecks.Count ; i++){	
						GUILayout.BeginHorizontal(myDecksGuiStyle[i]);
							{
							if (GUILayout.Button("("+myDecks[i].NbCards+") "+myDecks[i].Name,textDeckStyle)) // also can put width here
							{
								if (chosenDeck != i){
									myDecksGuiStyle[chosenDeck]=this.deckStyle;
									chosenDeck = i;
									myDecksGuiStyle[i]=this.deckChosenStyle;
									chosenIdDeck=myDecks[i].Id;
								}
							}
							GUILayout.Space(0.01f*widthScreen);
							if (GUILayout.Button("",myEditButtonStyle)) // also can put width here
							{
								tempText = myDecks[i].Name;
								deckToEdit = myDecks[i].Id;
							}
						
							GUILayout.Space(0.01f*widthScreen);
							if (GUILayout.Button("",mySuppressButtonStyle)) // also can put width here
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
		if (isLoadedCards<2){
			GUILayout.BeginArea(new Rect(widthScreen * 0.22f,0.26f*heightScreen,widthScreen * 0.78f,0.64f*heightScreen));
			{
				GUILayout.BeginVertical(); // also can put width in here
				{
					GUILayout.Label("Cartes en cours de chargement...   "+cardsToBeDisplayed.Count+" carte(s) chargée(s)",monLoaderStyle);
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
		else {
			if (isDisplayedCards){
				isDisplayedCards=false ;
				createCards();
				createDeckCards();
			}

			for (int i = 0 ; i < nbPages ; i++){
				if (GUI.Button(new Rect(widthScreen*0.505f-widthScreen*0.015f*(nbPages)+i*widthScreen*0.03f,0.965f*heightScreen,0.02f*widthScreen,0.03f*heightScreen),""+(i+1),paginatorGuiStyle[i])){
					paginatorGuiStyle[chosenPage].normal.background=backButton;
					paginatorGuiStyle[chosenPage].normal.textColor=Color.black;
					chosenPage=i;
					paginatorGuiStyle[i].normal.background=backActivatedButton;
					paginatorGuiStyle[i].normal.textColor=Color.white;
					StartCoroutine(displayPage());
				}
			}

			GUILayout.BeginArea(new Rect(0.82f*widthScreen,0.12f*heightScreen,widthScreen * 0.16f,0.85f*heightScreen));
			{
				bool toggle;
				string tempString ;
				GUILayout.BeginVertical();
				{
					GUILayout.Label ("Filtrer par classe",decksTitleStyle,GUILayout.Height(20));
					GUILayout.Space(-5);
					for (int i=0; i<this.cardTypeList.Length-1; i++) {		
						toggle = GUILayout.Toggle (togglesCurrentStates [i], this.cardTypeList[i],GUILayout.Height(16));
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
						GUILayout.Space(-5);
					}
						
					GUILayout.Space(8);
					GUILayout.Label ("Filtrer une compétence",decksTitleStyle,GUILayout.Height(20));
					GUILayout.Space(-5);
					tempString = GUILayout.TextField (this.valueSkill, GUILayout.Height(18));
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
						GUILayout.Space(-5);
						for (int j=0; j<matchValues.Count; j++) {
							if (GUILayout.Button (matchValues [j], myStyle,GUILayout.Width(widthScreen/7))) {
								valueSkill = matchValues [j].ToLower ();
								this.isSkillChosen=true ;
								this.matchValues = new List<string>();
								recalculeFiltres = true ;
								toReload = true ;
							}
						}
					}
						
					GUILayout.Space(5);
						
					GUILayout.Label ("Filtrer par points de vie",decksTitleStyle,GUILayout.Height(20));
					GUILayout.Space(-1);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:"+ Mathf.Round(minLifeVal),smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:"+ Mathf.Round(maxLifeVal),smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-5);
					MyGUI.MinMaxSlider (ref minLifeVal, ref maxLifeVal, minLifeLimit, maxLifeLimit);

					GUILayout.Space(5);
					
					GUILayout.Label ("Filtrer par pts d'attaque",decksTitleStyle,GUILayout.Height(20));
					GUILayout.Space(-1);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:"+ Mathf.Round(minAttackVal),smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:"+ Mathf.Round(maxAttackVal),smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-5);
					MyGUI.MinMaxSlider (ref minAttackVal, ref maxAttackVal, minAttackLimit, maxAttackLimit);

					GUILayout.Space(5);
					
					GUILayout.Label ("Filtrer par déplacement",decksTitleStyle,GUILayout.Height(20));
					GUILayout.Space(-1);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:"+ Mathf.Round(minMoveVal),smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:"+ Mathf.Round(maxMoveVal),smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-5);
					MyGUI.MinMaxSlider (ref minMoveVal, ref maxMoveVal, minMoveLimit, maxMoveLimit);

					GUILayout.Space(5);
					
					GUILayout.Label ("Filtrer par rapidité",decksTitleStyle,GUILayout.Height(20));
					GUILayout.Space(-1);
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
						if(isMoved){
							toReload = true ;
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
			print (w.text);
			StartCoroutine(RetrieveDecks()); 
			StartCoroutine(clearDeckCards());
			this.createDeckCards();
		}
	}

	private IEnumerator deleteDeck() {
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
			StartCoroutine(clearDeckCards());
			this.createDeckCards();
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
			StartCoroutine(clearDeckCards());
			this.createDeckCards();
		}
	}

	private IEnumerator setStyles() {

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

			this.deckStyle.fixedHeight = heightScreen*3/100;
			this.deckStyle.fixedWidth = widthScreen*19/100;

			this.deckChosenStyle.fixedHeight = heightScreen*3/100;
			this.deckChosenStyle.fixedWidth = widthScreen*19/100;

			this.textDeckStyle.fontSize = heightScreen*2/100;
			this.textDeckStyle.fixedHeight = heightScreen*3/100;
			this.textDeckStyle.fixedWidth = widthScreen*19/100;

			this.myEditButtonStyle.fixedHeight = heightScreen*2/100;
			this.myEditButtonStyle.fixedWidth = heightScreen*2/100;

			this.mySuppressButtonStyle.fixedHeight = heightScreen*2/100;
			this.mySuppressButtonStyle.fixedWidth = heightScreen*2/100;
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

		this.monLoaderStyle = new GUIStyle ();
		monLoaderStyle.normal.textColor = Color.black;
		monLoaderStyle.alignment = TextAnchor.MiddleCenter;

		this.smallPoliceStyle = new GUIStyle ();
		smallPoliceStyle.normal.textColor = Color.gray;
		smallPoliceStyle.alignment = TextAnchor.MiddleCenter;
		smallPoliceStyle.fontSize = 10;
		
		this.myStyle = new GUIStyle ();
		myStyle.normal.textColor = Color.black;
		myStyle.fontSize = 10;
		myStyle.fixedHeight = 12;
		myStyle.alignment = TextAnchor.MiddleCenter;
		myStyle.normal.background = this.backButton;



		if (zoomedCard != null) {
			print("x : "+zoomedCard.transform.localScale.x);
			print("x : "+zoomedCard.transform);
		}
		//float scale = Math.min (3.5f, heigh);
//		destinationScale = new Vector3(scale, scale, scale);

//		float debutLargeur = scale/2f;

//		destination = new Vector3(debutLargeur,-1f,-1);

		yield break;
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

	private IEnumerator applyFilters() {
		this.cardsToBeDisplayed=new List<int>();
		int nbFilters = this.filtersCardType.Count;
		bool testFilters = false;
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
								this.cardsToBeDisplayed.Add(i);
							}
						}
					}
				}
				else{
					for (int i = 0; i < max ; i++) {
						if (cards[i].hasSkill(this.valueSkill)){
							this.cardsToBeDisplayed.Add(i);
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
								if (cards[i].hasSkill(this.valueSkill)){
									this.cardsToBeDisplayed.Add(i);
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
							this.cardsToBeDisplayed.Add(i);
						}
					}
				}
				else{
					for (int i = 0; i < max ; i++) {
						this.cardsToBeDisplayed.Add(i);
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
									this.cardsToBeDisplayed.Add(i);

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
								this.cardsToBeDisplayed.Add(i);
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
		this.chosenPage = 0;
		paginatorGuiStyle = new GUIStyle[nbPages];
		for (int i = 0; i < nbPages; i++) { 
			paginatorGuiStyle [i] = new GUIStyle ();
			paginatorGuiStyle [i].alignment = TextAnchor.MiddleCenter;
			paginatorGuiStyle [i].fontSize = 12;
			if (i==0){
				paginatorGuiStyle[i].normal.background=backActivatedButton;
				paginatorGuiStyle[i].normal.textColor=Color.white;
			}
			else{
				paginatorGuiStyle[i].normal.background=backButton;
				paginatorGuiStyle[i].normal.textColor=Color.black;
			}
		}
		yield break;
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
		myDecksPanel = GameObject.Find("MyDecks");

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

			for(int i = 0 ; i < decksInformation.Length - 1 ; i++) 		// On boucle sur les attributs d'un deck
			{
				if (i>0){
					myDecksGuiStyle[i] = this.deckStyle;
				}
				else{
					myDecksGuiStyle[i] = this.deckChosenStyle;
				}
				deckInformation = decksInformation[i].Split('\\');
				myDecks.Add(new Deck(System.Convert.ToInt32(deckInformation[0]), deckInformation[1], System.Convert.ToInt32(deckInformation[2])));

				if (i==0){
					chosenIdDeck = System.Convert.ToInt32(deckInformation[0]);
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
				this.skillsList[i]=tempString[0];
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
		isLoadedCards++;
		StartCoroutine(RetrieveCardsFromDeck ());
	}

	private void createCards(){
		
		float tempF = 10f*widthScreen/heightScreen;
		float width = tempF * 0.6f;
		nbCardsPerRow = Mathf.FloorToInt(width/1.6f);
		float debutLargeur = -0.3f * tempF+0.8f + (width - 1.6f * nbCardsPerRow)/2 ;
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
		nbPages = Mathf.CeilToInt(nbCardsToDisplay / (3*nbCardsPerRow))+1;
		this.chosenPage = 0;
		paginatorGuiStyle = new GUIStyle[nbPages];
		for (int i = 0; i < nbPages; i++) { 
			paginatorGuiStyle [i] = new GUIStyle ();
			paginatorGuiStyle [i].alignment = TextAnchor.MiddleCenter;
			paginatorGuiStyle [i].fontSize = 12;
			if (i==0){
				paginatorGuiStyle[i].normal.background=backActivatedButton;
				paginatorGuiStyle[i].normal.textColor=Color.white;
			}
			else{
				paginatorGuiStyle[i].normal.background=backButton;
				paginatorGuiStyle[i].normal.textColor=Color.black;
			}
		}
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		this.setFilters ();
	}

	private IEnumerator displayPage(){

		int start = 3 * nbCardsPerRow * chosenPage;
		int finish = start + 3 * nbCardsPerRow;
		for(int i = start ; i < finish ; i++){
			//displayedCards[i].GetComponent<GameCard>().setTextResolution (1f);
			int nbCardsToDisplay = this.cardsToBeDisplayed.Count;
			if (i<nbCardsToDisplay){
				displayedCards[i-start].SetActive(true);
				displayedCards[i-start].GetComponent<GameCard>().Card = cards[this.cardsToBeDisplayed[i]]; 
				displayedCards[i-start].GetComponent<GameCard>().ShowFace();
			}
			else{
				displayedCards[i-start].SetActive(false);
			}
		}
		yield break;
	}

	private IEnumerator clearCards(){
		for (int i = 0; i < 3*nbCardsPerRow; i++) {
			Destroy(displayedCards[i]);
		}
		yield break;
	}

	private IEnumerator clearDeckCards(){
		for (int i = 0; i < 5; i++) {
			Destroy(displayedDeckCards[i]);
		}
		yield break;
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
		this.isLoadedCards++ ;
	}

	private void createDeckCards(){
		float tempF = 10f*widthScreen/heightScreen;
		float width = tempF * 0.6f;
		float scale = Mathf.Min (1.6f, width / 6f);
		float pas = (width - 5f * scale) / 6f;
		float debutLargeur = -0.3f * tempF + pas + scale/2 ;

		displayedDeckCards = new GameObject[5];
		int nbDeckCardsToDisplay = this.deckCardsIds.Count;
		for(int i = 0 ; i < 5 ; i++){
			displayedDeckCards[i] = Instantiate(CardObject) as GameObject;
			displayedDeckCards[i].transform.localScale = new Vector3(scale,scale,scale); 
			displayedDeckCards[i].transform.localPosition = new Vector3(debutLargeur + (scale+pas)*i , 2.9f, 0); 
			displayedDeckCards[i].gameObject.name = "DeckCard" + i + "";	
			if (i<nbDeckCardsToDisplay){
				displayedDeckCards[i].GetComponent<GameCard>().Card = cards[this.deckCardsIds[i]]; 
				displayedDeckCards[i].GetComponent<GameCard>().ShowFace();
			}   
			else{
				displayedDeckCards[i].SetActive (false);
			}
		}
	}
}

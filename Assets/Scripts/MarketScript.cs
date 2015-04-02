using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

public class MarketScript : MonoBehaviour {

	public int refreshInterval;

	public Texture2D backButton ;
	public Texture2D backActivatedButton ;
	public Texture2D soldCardTexture;

	int widthScreen = Screen.width ; 
	int heightScreen = Screen.height ;
	int nbCardsPerRow = 0 ;

	public GUIStyle filterTitleStyle;
	public GUIStyle nbResultsStyle;
	public GUIStyle sortTitleStyle;
	public GUIStyle cantBuyPricePoliceStyle;
	public GUIStyle smallPoliceStyle;
	public GUIStyle pricePoliceStyle;
	public GUIStyle sellerPoliceStyle;
	public GUIStyle usernameStyle;
	public GUIStyle skillListStyle;
	public GUIStyle minmaxPriceStyle;
	public GUIStyle deleteButtonStyle;
	public GUIStyle buyButtonStyle;
	public GUIStyle sortDefaultButtonStyle;
	public GUIStyle sortActivatedButtonStyle;
	public GUIStyle searchButtonStyle;
	public GUIStyle toggleStyle;
	public GUIStyle textFieldStyle;
	public GUIStyle skillsChosenStyle;
	public GUIStyle paginationActivatedStyle;
	public GUIStyle paginationStyle;

	public GUIStyle newCardsStyle;
	public GUIStyle newCardsButtonStyle;


	public GameObject MenuObject;

	GUIStyle[] paginatorGuiStyle;
	GUIStyle[] sortButtonStyle=new GUIStyle[10];

	private int chosenPage =0 ;

	int nbPages ;
	int pageDebut ; 
	int pageFin ;
	int newCardsNb ;

	public GameObject CardObject;
	WWW w;

	private IList<int> cardsToBeDisplayed ;
	string[] cardTypeList;
	string[] skillsList;

	string[] cardsIDS = null;

	private IList<Card> cards ;
	private IList<int> filtersCardType ;
	private string valueSkill="";
	private IList<string> skillsChosen = new List<string> ();
	private IList<string> matchValues;
	private IList<string> usersList;
	private IList<int> cardsSold= new List<int> ();

	private bool[] togglesCurrentStates;
	bool displayLoader;
	bool displayCards = false;
	bool isBeingDragged = false;
	bool toReload = false ;
	bool isSkillToDisplay = false ;
	bool isSkillChosen = false ;
	bool displayPopUp = false;

	string cardsToSearch="";

	private string URLGetMarketData = "http://54.77.118.214/GarrukServer/get_market_data.php";
	private string URLGetMarketCards = "http://54.77.118.214/GarrukServer/get_market_cards.php";
	private string URLBuyCard = "http://54.77.118.214/GarrukServer/buyCard.php";
	private string URLRefreshMarket = "http://54.77.118.214/GarrukServer/refresh_market.php";

	GameObject[] displayedCards ;
	Vector3[] cardsPosition ;
	Vector3 cardsDimensionX;


	float minLifeVal = 0;
	float maxLifeVal = 200;
	float minAttackVal = 0;
	float maxAttackVal = 100;
	float minMoveVal = 0;
	float maxMoveVal = 10;
	float minQuicknessVal = 0;
	float maxQuicknessVal = 100;
	float minLifeLimit;
	float maxLifeLimit;
	float minAttackLimit;
	float maxAttackLimit;
	float minMoveLimit;
	float maxMoveLimit;
	float minQuicknessLimit;
	float maxQuicknessLimit;
	float oldMinLifeVal = 0;
	float oldMaxLifeVal = 200;
	float oldMinAttackVal = 0;
	float oldMaxAttackVal = 100;
	float oldMinMoveVal = 0;
	float oldMaxMoveVal = 10;
	float oldMinQuicknessVal = 0;
	float oldMaxQuicknessVal = 100;
	
	int idCardToBuy;
	int priceCardToBuy;
	int start;
	int finish;

	int totalNbResult;
	int newTotalNbResult;
	int totalNbResultLimit = 5000;

	string minPrice ="";
	string maxPrice="";
	
	float timer;

	DateTime dateLimit;
	DateTime newDateLimit;

	int oldSortSelected = 10;
	int sortSelected = 10;
	
	int idFocused=-1;
	int cardId ;
	Rect rectFocus ;
	GameObject cardFocused ;

	RaycastHit hit;
	Ray ray ;

	public GUIStyle centralWindowStyle ;
	public GUIStyle centralWindowTitleStyle ;
	public GUIStyle centralWindowButtonStyle ;
	public GUIStyle smallCentralWindowButtonStyle ;
	public GUIStyle focusButtonStyle;
	Rect centralWindow ;
	Rect centralWindowCardNotFocused;

	bool isBuyingCard=false;
	bool destroyFocus = false ;
	bool isEscDown = false ;
	bool isUpEscape;


	// Use this for initialization
	void Start () {
		MenuObject = Instantiate(MenuObject) as GameObject;
		setStyles();
		displayLoader = true;
		StartCoroutine(initializeMarket());
		filtersCardType = new List<int> ();
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		if (timer > refreshInterval) {

			timer=timer-refreshInterval;

			if (displayCards || idFocused!=-1)
			StartCoroutine(refreshMarket());
		}

		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			if (this.idFocused != -1){
				isBuyingCard=false;
				Destroy(cardFocused);
				this.idFocused=-1;
				displayCards = true ;
				destroyFocus = false ;
			}
			this.setStyles();
			clearCards();
			this.createCards();
			chosenPage=0;
		}
		if (toReload) {
			this.applyFilters ();
			if (sortSelected!=10){
				this.sortCards();
			}
			this.displayPage ();
			toReload = false ;
		}

		if (oldSortSelected!=sortSelected){
			if(oldSortSelected!=10){
				this.sortButtonStyle[oldSortSelected]=this.sortDefaultButtonStyle;
			}
			this.sortButtonStyle[sortSelected]=this.sortActivatedButtonStyle;
			oldSortSelected=sortSelected;
			sortCards();
		}


		if (Input.GetMouseButtonDown(1)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out hit))
			{
				if (hit.collider.name.StartsWith("Card")){
					displayCards=false ;
					idFocused = System.Convert.ToInt32(hit.collider.gameObject.name.Substring(4));
					
					int finish = 3 * nbCardsPerRow;
					for(int i = 0 ; i < finish ; i++){
						displayedCards[i].SetActive(false);
					}
					
					cardFocused = Instantiate(CardObject) as GameObject;
					Destroy(cardFocused.GetComponent<GameNetworkCard>());
					Destroy(cardFocused.GetComponent<PhotonView>());

					float scale = heightScreen/120f;
					cardFocused.transform.localScale = new Vector3(scale,scale,scale); 
					Vector3 vec = Camera.main.WorldToScreenPoint(cardFocused.collider.bounds.size);
					cardFocused.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0.50f*widthScreen ,0.45f*heightScreen-1 , 10)); 
					cardFocused.gameObject.name = "FocusedCard";	
					
					cardId = cards[idFocused].Id;
					cardFocused.GetComponent<GameCard>().Card = cards[idFocused]; 
					
					cardFocused.GetComponent<GameCard>().ShowFace();

					if (cardsSold.Contains(idFocused)){
							cardFocused.transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;
					}

					cardFocused.GetComponent<GameCard>().setTextResolution(2f);
					cardFocused.SetActive (true);
					cardFocused.transform.Find("texturedGameCard").FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();
					cardFocused.transform.Find("texturedGameCard").FindChild("ExperienceArea").GetComponent<GameCard_experience>().setTextResolution(2f);
					rectFocus = new Rect(0.50f*widthScreen+(vec.x-widthScreen/2f)/2f, 0.15f*heightScreen, 0.25f*widthScreen, 0.8f*heightScreen);
				}
			}
		}


		if (destroyFocus){
			isBuyingCard=false;
			Destroy(cardFocused);
			this.idFocused=-1;
			displayCards = true ;
			this.displayPage ();
			destroyFocus = false ;
		}
		
		if (isUpEscape){
			isEscDown = false ;
			isUpEscape = false ;
		}
		
		if(isBuyingCard){
			if(Input.GetKeyDown(KeyCode.Return)) {
				isBuyingCard = false ;
				if(idFocused!=-1){
					StartCoroutine(buyCard(cards[cardsToBeDisplayed[idFocused]].Id.ToString(),cards[cardsToBeDisplayed[idFocused]].Price.ToString()));
					cardsSold.Add (cardsToBeDisplayed[idFocused]);
					cardFocused.transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;
					displayedCards[idFocused].transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;
				}
				else{
					StartCoroutine(buyCard(cards[cardsToBeDisplayed[idCardToBuy]].Id.ToString(),cards[cardsToBeDisplayed[idCardToBuy]].Price.ToString()));
					cardsSold.Add (cardsToBeDisplayed[idCardToBuy]);
					displayedCards[idCardToBuy-start].transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;
				}
			}
			else if(Input.GetKeyDown(KeyCode.Escape)){
				isBuyingCard=false;
				isEscDown = true ;
			}
		}

		if(displayPopUp){
			if(Input.GetKeyDown(KeyCode.Return)) {
				displayPopUp=false;
			}
			else if(Input.GetKeyDown(KeyCode.Escape)) {
				isEscDown = true ;
				displayPopUp=false;
			}
		}

	}

	void OnGUI () {

		if(isBuyingCard){

			if (idFocused!=-1){
				GUILayout.BeginArea(centralWindow);
			} else {
				GUILayout.BeginArea(centralWindowCardNotFocused);
			}
				{
				GUILayout.BeginVertical(centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					if(idFocused!=-1){
						GUILayout.Label("Confirmer l'achat de la carte (coûte "+cards[cardsToBeDisplayed[idFocused]].Price+ " crédits)", centralWindowTitleStyle);
					}else{
						GUILayout.Label("Confirmer l'achat de la carte (coûte "+cards[cardsToBeDisplayed[idCardToBuy]].Price+ " crédits)", centralWindowTitleStyle);
					}
					GUILayout.Space(0.02f*heightScreen);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space(0.03f*widthScreen);
						if (GUILayout.Button("Acheter",centralWindowButtonStyle)) // also can put width here
						{
							isBuyingCard = false ;
							if(idFocused!=-1){
								StartCoroutine(buyCard(cards[cardsToBeDisplayed[idFocused]].Id.ToString(),cards[cardsToBeDisplayed[idFocused]].Price.ToString()));
								cardsSold.Add (cardsToBeDisplayed[idFocused]);
								cardFocused.transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;
								displayedCards[idFocused].transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;
							} else{
								StartCoroutine(buyCard(cards[cardsToBeDisplayed[idCardToBuy]].Id.ToString(),cards[cardsToBeDisplayed[idCardToBuy]].Price.ToString()));
								cardsSold.Add (cardsToBeDisplayed[idCardToBuy]);
								displayedCards[idCardToBuy-start].transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;
							}
						}
						GUILayout.Space(0.04f*widthScreen);
						if (GUILayout.Button("Annuler",centralWindowButtonStyle)) // also can put width here
						{
							isBuyingCard = false ;
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

		if(displayPopUp){
			if (idFocused!=-1){
				GUILayout.BeginArea(centralWindow);
			} else {
				GUILayout.BeginArea(centralWindowCardNotFocused);
			}
			{
				GUILayout.BeginVertical(centralWindowStyle);
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label("La carte a déjà été vendue, dommage !", centralWindowTitleStyle);
					
					GUILayout.Space(0.02f*heightScreen);
					
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("OK",centralWindowButtonStyle)) // also can put width here
						{
							displayPopUp=false;
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
		if (idFocused!=-1 && !displayPopUp && !isBuyingCard){
			if(isEscDown) {
				if(Input.GetKeyUp(KeyCode.Escape)) {
					isUpEscape = true ;
				}
			}
			else if(Input.GetKeyDown(KeyCode.Escape)) {
				this.destroyFocus=true;
			}
			else{
				GUILayout.BeginArea(rectFocus);
				{
					GUILayout.BeginVertical();
					{
						if (ApplicationModel.credits >= cards[idFocused].Price && !cardsSold.Contains(idFocused)){
							if (GUILayout.Button("Acheter (-"+cards[idFocused].Price+" crédits)",focusButtonStyle)){
								isBuyingCard = true ; 
							}
						}
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("Revenir à mes cartes",focusButtonStyle))
						{
							this.destroyFocus=true;
						}
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
			}
		}
		if (displayLoader){
			GUILayout.BeginArea(new Rect(0,0.20f*heightScreen,widthScreen * 0.85f,0.80f*heightScreen));
			{
				GUILayout.BeginVertical(); // also can put width in here
				{
					GUILayout.Label("Cartes en cours de chargement...   "+cardsToBeDisplayed.Count+" carte(s) chargée(s)");
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
		else if (displayCards){
			if (newCardsNb>0){
				GUILayout.BeginArea(new Rect(0.01f*widthScreen,0.10f*heightScreen,widthScreen * 0.78f,0.04f*heightScreen));
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label(newCardsNb+" nouvelles cartes sur le marché"+"\u00A0",newCardsStyle);
						if(GUILayout.Button("Rafraichir",newCardsButtonStyle))
						{
							newCardsNb=0;
							cardsSold= new List<int> ();
							totalNbResult=newTotalNbResult;
							dateLimit=newDateLimit;
							displayCards=false;
							displayLoader=true;
							clearCards();
							getCards();
							toReload=true;
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndArea();
			}
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
			GUILayout.BeginArea(new Rect(0.01f*widthScreen,0.10f*heightScreen,widthScreen * 0.78f,0.04f*heightScreen));
			{
				if (cardsToBeDisplayed.Count != totalNbResult)
					GUILayout.Label (cardsToBeDisplayed.Count +" / " + totalNbResult + " cartes affichées",nbResultsStyle);;
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(new Rect(0.80f*widthScreen,0.10f*heightScreen,widthScreen * 0.19f,0.90f*heightScreen));
			{
				bool toggle;
				string tempString ;
				string tempMinPrice ;
				string tempMaxPrice ;
				GUILayout.BeginVertical();
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Prix",filterTitleStyle);
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("^",sortButtonStyle[0])) {
							sortSelected=0;
						}
						if(GUILayout.Button ("v",sortButtonStyle[1])) {
							sortSelected=1;
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.Label ("Prix min :",minmaxPriceStyle);
					tempMinPrice = GUILayout.TextField(minPrice, 9,textFieldStyle);
					if (tempMinPrice != minPrice) {
						minPrice= Regex.Replace(tempMinPrice, "[^0-9]", "");
						toReload=true;
					}
					GUILayout.Label ("Prix max :",minmaxPriceStyle);
					tempMaxPrice = GUILayout.TextField(maxPrice, 9,textFieldStyle);
					if (tempMaxPrice != maxPrice) {
						maxPrice= Regex.Replace(tempMaxPrice, "[^0-9]", "");
						toReload=true;
					}
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Filtrer par classe",filterTitleStyle);
					for (int i=0; i<this.cardTypeList.Length-1; i++) {		
						toggle = GUILayout.Toggle (togglesCurrentStates [i], this.cardTypeList[i],toggleStyle);
						if (toggle != togglesCurrentStates [i]) {
							togglesCurrentStates [i] = toggle;
							if (toggle){
								filtersCardType.Add(i);
							}
							else{
								filtersCardType.Remove(i);
							}
							toReload = true ;
						}
					}
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Filtrer une capacité",filterTitleStyle);
					tempString = GUILayout.TextField (this.valueSkill, this.textFieldStyle);
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
							toReload = true ;
						}
					}
					if (isSkillToDisplay){
						GUILayout.Space(-3);
						for (int j=0; j<matchValues.Count; j++) {
							if (GUILayout.Button (matchValues [j], skillListStyle)) {
								valueSkill = matchValues [j].ToLower ();
								this.isSkillChosen=true ;
								this.matchValues = new List<string>();
								toReload = true ;
							}
						}
					}
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Vie",filterTitleStyle);
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("^",sortButtonStyle[2])) {
							sortSelected=2;
						}
						if(GUILayout.Button ("v",sortButtonStyle[3])) {
							sortSelected=3;
						}
					}
					GUILayout.EndHorizontal();
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
					
					GUILayout.FlexibleSpace();
					
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Attaque",filterTitleStyle);
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("^",sortButtonStyle[4])) {
							sortSelected=4;
						}
						if(GUILayout.Button ("v",sortButtonStyle[5])) {
							sortSelected=5;
						}
					}
					GUILayout.EndHorizontal();
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
					
					GUILayout.FlexibleSpace();
					
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Mouvement",filterTitleStyle);
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("^",sortButtonStyle[6])) {
							sortSelected=6;
						}
						if(GUILayout.Button ("v",sortButtonStyle[7])) {
							sortSelected=7;
						}
					}
					GUILayout.EndHorizontal();
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
					
					GUILayout.FlexibleSpace();
					
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Rapidité",filterTitleStyle);
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("^",sortButtonStyle[8])) {
							sortSelected=8;
						}
						if(GUILayout.Button ("v",sortButtonStyle[9])) {
							sortSelected=9;
						}
					}
					GUILayout.EndHorizontal();
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

					GUILayout.FlexibleSpace();

					GUILayout.FlexibleSpace();


					if (Input.GetMouseButtonDown(0)){
						isBeingDragged = true;
					}
					
					if (Input.GetMouseButtonUp(0)){
						isBeingDragged = false;
					}
					
					if (!isBeingDragged){
						bool isMoved = false ;
						maxLifeVal=Mathf.RoundToInt(maxLifeVal);
						minLifeVal=Mathf.RoundToInt(minLifeVal);
						maxAttackVal=Mathf.RoundToInt(maxAttackVal);
						minAttackVal=Mathf.RoundToInt(minAttackVal);
						maxMoveVal=Mathf.RoundToInt(maxMoveVal);
						minMoveVal=Mathf.RoundToInt(minMoveVal);
						maxQuicknessVal=Mathf.RoundToInt(maxQuicknessVal);
						minQuicknessVal=Mathf.RoundToInt(minQuicknessVal);

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

			for (int i=start; i<finish;i++){

				cardsDimensionX = new Vector3(camera.WorldToScreenPoint(new Vector3(displayedCards[i-start].transform.FindChild("texturedGameCard").renderer.bounds.size.x,0,0)).x-Screen.width/2,0,0);
				cardsPosition[i-start]=camera.WorldToScreenPoint(new Vector3 (displayedCards[i-start].transform.position.x,1.65f-((i-start)-(i-start)%nbCardsPerRow)/nbCardsPerRow*2.75f,0));

				GUILayout.BeginArea(new Rect(cardsPosition[i-start].x-cardsDimensionX.x/2,heightScreen - cardsPosition[i-start].y,cardsDimensionX.x,heightScreen*7/100));
				{

					GUILayout.BeginVertical(); // also can put width in here
					{	

						if (ApplicationModel.credits >= cards[cardsToBeDisplayed[i]].Price && !cardsSold.Contains(cardsToBeDisplayed[i])){
							GUILayout.Label ("Prix : "+cards[cardsToBeDisplayed[i]].Price+" $",pricePoliceStyle);

							GUILayout.BeginHorizontal();
							{

								GUILayout.Label ("Joueur : ",sellerPoliceStyle);
								if (GUILayout.Button(usersList[cardsToBeDisplayed[i]],usernameStyle))
								{
									ApplicationModel.profileChosen =usersList[cardsToBeDisplayed[i]];
									Application.LoadLevel("Profile");

								}
							}
							GUILayout.EndHorizontal();

							if (GUILayout.Button("Acheter",buyButtonStyle))
							{
							
								isBuyingCard = true ; 
								idCardToBuy=i;

							}
						}
						else if (ApplicationModel.credits < cards[cardsToBeDisplayed[i]].Price){
							GUILayout.Label ("Prix : "+cards[cardsToBeDisplayed[i]].Price+" $",cantBuyPricePoliceStyle);
							GUILayout.BeginHorizontal();
							{
								
								GUILayout.Label ("Joueur : ",sellerPoliceStyle);
								if (GUILayout.Button(usersList[cardsToBeDisplayed[i]],usernameStyle))
								{
									ApplicationModel.profileChosen =usersList[cardsToBeDisplayed[i]];
									Application.LoadLevel("Profile");
									
								}
							}
							GUILayout.EndHorizontal();
							//GUILayout.Space (15);
						}
						else {
							GUILayout.Label ("Prix : "+cards[cardsToBeDisplayed[i]].Price+" $",pricePoliceStyle);
							GUILayout.BeginHorizontal();
							{
								GUILayout.Label ("Joueur : ",sellerPoliceStyle);
								if (GUILayout.Button(usersList[cardsToBeDisplayed[i]],usernameStyle))
								{
									ApplicationModel.profileChosen =usersList[cardsToBeDisplayed[i]];
									Application.LoadLevel("Profile");
																}
							}
							GUILayout.EndHorizontal();
							//GUILayout.Space (15);
						}
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
			}
		}
	}
	
	private void setStyles() {
		
		heightScreen = Screen.height;
		widthScreen = Screen.width;

		// Style utilisé pour les titre des filtres de tri

		this.sortTitleStyle.fontSize = heightScreen*15/1000;
		this.sortTitleStyle.fixedHeight = (int)heightScreen*2/100;

		// Style utilisé pour les bouttons de tri

		this.sortDefaultButtonStyle.fontSize=heightScreen*2/100;
		this.sortDefaultButtonStyle.fixedHeight = (int)heightScreen*3/100;
		this.sortDefaultButtonStyle.fixedWidth = (int)widthScreen*12/1000;

		this.sortActivatedButtonStyle.fontSize=heightScreen*2/100;
		this.sortActivatedButtonStyle.fixedHeight = (int)heightScreen*3/100;
		this.sortActivatedButtonStyle.fixedWidth = (int)widthScreen*12/1000;

		for (int i =0;i<10;i++){

			if(sortSelected==10){
				sortButtonStyle[i]=this.sortDefaultButtonStyle;
			}
		}


		//Style utilisé pour les titres des filtres de filtre

		this.filterTitleStyle.fontSize = heightScreen*19/1000;
		this.filterTitleStyle.fixedHeight = (int)heightScreen*25/1000;
		//this.filterTitleStyle.fixedWidth = (int)widthScreen*12/100;

		// Style utilisé pour le filtre prix min/max 

		this.minmaxPriceStyle.fontSize = heightScreen*15/1000;
		this.minmaxPriceStyle.fixedHeight = (int)heightScreen*2/100;
	
		// Style utilisé pour le bouton de suppression des compétences (et le bouton profil)

		this.deleteButtonStyle.fontSize = heightScreen*15/1000;
		this.deleteButtonStyle.fixedHeight = (int)heightScreen*2/100;
		//this.deleteButtonStyle.fixedWidth = (int)widthScreen*6/100;

		// Style utilisé pour le bouton d'achat des cartes

		this.buyButtonStyle.fontSize = heightScreen*15/1000;
		this.buyButtonStyle.fixedHeight = (int)heightScreen*2/100;

		// Style utilisé pour les filtre "classes" et pour les valeurs min/max des sliders

		this.smallPoliceStyle.fontSize = heightScreen*15/1000;
		this.smallPoliceStyle.fixedHeight = (int)heightScreen*2/100;

		// Style utilisé pour l'affichage du nombre de cartes

		this.nbResultsStyle.fontSize = heightScreen*2/100;
		this.nbResultsStyle.fixedHeight = (int)heightScreen*25/1000;

		// Style utilisé pour l'affichage du nombre de nouvelles cartes
		
		this.newCardsStyle.fontSize = heightScreen*2/100;
		this.newCardsStyle.fixedHeight = (int)heightScreen*25/1000;

		// Style utilisé pour l'affichage du bouton permettant l'actualisation des cartes
		
		this.newCardsButtonStyle.fontSize = heightScreen*2/100;
		this.newCardsButtonStyle.fixedHeight = (int)heightScreen*25/1000;
		this.newCardsButtonStyle.fixedWidth = (int)widthScreen * 10 / 100;

		// Style utilisé pour l'affichage du prix des cartes

		this.pricePoliceStyle.fontSize= heightScreen*2/100;
		this.pricePoliceStyle.fixedHeight = (int)heightScreen*3/100;

		// Style utilisé pour l'affichage d'un prix trop élevé

		this.cantBuyPricePoliceStyle.fontSize= heightScreen*2/100;
		this.cantBuyPricePoliceStyle.fixedHeight = (int)heightScreen*3/100;

		// Style utilisé pour l'affichage du libellé joueur

		this.sellerPoliceStyle.fontSize=heightScreen*15/1000;
		this.sellerPoliceStyle.fixedHeight= (int)heightScreen*2/100;

		// Style utilisé pour l'affichage des users
		
		this.usernameStyle.fontSize = heightScreen * 15/1000;
		this.usernameStyle.fixedHeight = (int)heightScreen * 2/100;

		// Style utilisé pour les bouttons de compétence

		this.skillListStyle.fontSize=heightScreen*15/1000;
		this.skillListStyle.fixedHeight=(int)heightScreen*2/100;

		// Style utilisé pour les filtres sur les classes
		
		this.toggleStyle.fontSize=heightScreen*15/1000;
		this.toggleStyle.fixedHeight=(int)heightScreen*2/100;

		// Style utilisé pour le bouton de recherche

		this.searchButtonStyle.fontSize=heightScreen*2/100;
		this.searchButtonStyle.fixedHeight=(int)heightScreen*3/100;

		// Style utilisé pour les textfields
		
		this.textFieldStyle.fontSize=heightScreen*2/100;
		this.textFieldStyle.fixedHeight=(int)heightScreen*25/1000;

		// Style utilisé pour les skills à filtrer

		this.skillsChosenStyle.fontSize = heightScreen* 15/1000;
		this.skillsChosenStyle.fixedHeight = (int)heightScreen * 2/100;

		// Styles utilisés pour la pagination

		this.paginationStyle.fontSize = heightScreen*2/100;
		this.paginationStyle.fixedWidth = widthScreen*3/100;
		this.paginationStyle.fixedHeight = heightScreen*3/100;
		this.paginationActivatedStyle.fontSize = heightScreen*2/100;
		this.paginationActivatedStyle.fixedWidth = widthScreen*3/100;
		this.paginationActivatedStyle.fixedHeight = heightScreen*3/100;

		this.centralWindow = new Rect (widthScreen * 0.25f, 0.12f * heightScreen, widthScreen * 0.50f, 0.18f * heightScreen);
		this.centralWindowCardNotFocused = new Rect (widthScreen * 0.175f, 0.41f * heightScreen, widthScreen * 0.50f, 0.18f * heightScreen);
		
		this.centralWindowStyle.fixedWidth = widthScreen*0.5f-5;
		
		this.centralWindowTitleStyle.fontSize = heightScreen*2/100;
		this.centralWindowTitleStyle.fixedHeight = heightScreen*3/100;
		this.centralWindowTitleStyle.fixedWidth = widthScreen*5/10;

		this.centralWindowButtonStyle.fontSize = heightScreen*2/100;
		this.centralWindowButtonStyle.fixedHeight = heightScreen*3/100;
		this.centralWindowButtonStyle.fixedWidth = widthScreen*2/10;

		this.focusButtonStyle.fontSize = heightScreen*2/100;
		this.focusButtonStyle.fixedHeight = heightScreen*6/100;
		this.focusButtonStyle.fixedWidth = widthScreen*25/100;

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


	private void clearCards(){
		for (int i = 0; i < 3*nbCardsPerRow; i++) {
			Destroy(displayedCards[i]);
		}
	}


	private void displayPage(){
		
		start = 3 * nbCardsPerRow * chosenPage;
		finish = start + 3 * nbCardsPerRow;
		for(int i = start ; i < finish ; i++){
			//displayedCards[i].GetComponent<GameCard>().setTextResolution (1f);
			int nbCardsToDisplay = this.cardsToBeDisplayed.Count;
			if (i<nbCardsToDisplay){
				displayedCards[i-start].gameObject.name = "Card" + this.cardsToBeDisplayed[i] + "";
				displayedCards[i-start].GetComponent<GameCard>().Card = cards[this.cardsToBeDisplayed[i]];
				displayedCards[i-start].GetComponent<GameCard>().ShowFace();
				displayedCards[i-start].transform.Find("texturedGameCard").FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();
				displayedCards[i-start].SetActive(true);
				
				if (cardsSold.Contains(cardsToBeDisplayed[i])){
					displayedCards[i-start].transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;
				}

			}
			else{
				displayedCards[i-start].SetActive(false);
			}
		}

		if (chosenPage == nbPages-1){
			finish = cardsToBeDisplayed.Count;
		}
		else{
			finish = start + 3 * nbCardsPerRow;
		}
		if (cardsToBeDisplayed.Count==0){
			finish = 0;
		}
	
	}

	private void createCards(){

		displayCards=true;
		displayLoader = false;

		float tempF = 10f*widthScreen/heightScreen;
		float width = tempF * 0.78f;
		nbCardsPerRow = Mathf.FloorToInt(width/1.6f);
		float debutLargeur = -0.49f * tempF+0.8f + (width - 1.6f * nbCardsPerRow)/2 ;
		displayedCards = new GameObject[3*nbCardsPerRow];
		cardsPosition = new Vector3[3 * nbCardsPerRow];
		int nbCardsToDisplay = this.cardsToBeDisplayed.Count;

		for(int i = 0 ; i < 3*nbCardsPerRow ; i++){

			displayedCards[i] = Instantiate(CardObject) as GameObject;
			Destroy(displayedCards[i].GetComponent<GameNetworkCard>());
			Destroy(displayedCards[i].GetComponent<PhotonView>());
			displayedCards[i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
			displayedCards[i].transform.localPosition = new Vector3(debutLargeur + 1.6f*(i%nbCardsPerRow), 2.625f-(i-i%nbCardsPerRow)/nbCardsPerRow*2.75f, 0); 
			displayedCards[i].gameObject.name = "Card" + i + "";	

			if (i<nbCardsToDisplay){

				displayedCards[i].GetComponent<GameCard>().Card = cards[this.cardsToBeDisplayed[i]];
				displayedCards[i].GetComponent<GameCard>().ShowFace();
				displayedCards[i].transform.Find("texturedGameCard").FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();

				

				if (cardsSold.Contains(cardsToBeDisplayed[i])){
					displayedCards[i].transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;

				}

			}   
			else{
				displayedCards[i].SetActive (false);
			}
		}

		nbPages = Mathf.CeilToInt((nbCardsToDisplay-1) / (3*nbCardsPerRow))+1;
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

		start = 3 * nbCardsPerRow * chosenPage;
		if (nbCardsToDisplay < 3*nbCardsPerRow){
		finish = nbCardsToDisplay;
		}
		else{
		finish = 3 * nbCardsPerRow;
		}
	}


	private IEnumerator initializeMarket () {


		string[] skillsIds = null;
		string[] tempString = null;

		this.cardsToBeDisplayed = new List<int> ();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		
		w = new WWW(URLGetMarketData, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			//			print (w.text);
			
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			ApplicationModel.credits = System.Convert.ToInt32(data[3]);
			cardsIDS=data[2].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
			skillsIds = data[1].Split('\n');
			totalNbResult = System.Convert.ToInt32(data[4]);
			dateLimit=DateTime.ParseExact(data[5], "yyyy-MM-dd hh:mm:ss", null);
			
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


			getCards ();
			setFilters ();
			


		}

	}

	public void getCards() {
		
		
		string[] cardInfo = null;
		string[] cardInfo2 = null;
		int tempInt ;
		
			this.usersList = new List<string> ();
			this.cards = new List<Card>();
			
			for(int i = 0 ; i < cardsIDS.Length-1 ; i++){
				cardInfo = cardsIDS[i].Split('\n');
				for(int j = 1 ; j < cardInfo.Length-1 ; j++){
					cardInfo2 = cardInfo[j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
					if (j==1){
						//Debug.Log (cardInfo2[12]);
						//Debug.Log (cardInfo2[13]);
						
						
						
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
						                        System.Convert.ToInt32(cardInfo2[11]), // attackleve
						                        System.Convert.ToInt32(cardInfo2[12]), // price
					                       		DateTime.ParseExact(cardInfo2[13], "yyyy-MM-dd hh:mm:ss", null),
					                        	System.Convert.ToInt32(cardInfo2[15]))); // onSaleDate
											
						this.cards[i].Skills = new List<Skill>();
						this.cardsToBeDisplayed.Add(i);
						this.usersList.Add (cardInfo2[14]);

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
		createCards ();
	}


	private IEnumerator buyCard(string idcard, string cost){
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", idcard);
		form.AddField("myform_cost", cost);
		form.AddField("myform_date",  System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss").ToString());
		
		WWW w = new WWW(URLBuyCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			print(w.text); // donne le retour
			if (w.text != "ALREADYSOLD")
			ApplicationModel.credits = System.Convert.ToInt32(w.text);
			else{
				displayPopUp = true;
				//for (int i = 0; i < 3*nbCardsPerRow; i++) {
				//	displayedCards[i].collider.enabled=false;
				//}
			}
		}
	}
	

	private IEnumerator enableGameObjects(){

		yield return new WaitForSeconds (0.01f);
		for (int i = 0; i < 3*nbCardsPerRow; i++) {
			displayedCards[i].collider.enabled=true;
		}	
	}
	

	private IEnumerator refreshMarket (){
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_datelimit",  dateLimit.ToString("yyyy-MM-dd hh:mm:ss").ToString());
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());

		
		w = new WWW(URLRefreshMarket, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		}
		else {
			bool find = false;
			string[] data = null;
			string[] newCardsIDS = null;
			data = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			newCardsIDS = data[0].Split(new char[] { '\n' }, System.StringSplitOptions.None);
			cardsIDS=data[1].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
			newTotalNbResult=System.Convert.ToInt32(data[2]);
			newDateLimit=DateTime.ParseExact(data[3], "yyyy-MM-dd hh:mm:ss", null);

			for (int i=0; i<cardsToBeDisplayed.Count;i++){

				find=false;
				for (int j=0 ; j<newCardsIDS.Length-1; j++){

					if(System.Convert.ToInt32(newCardsIDS[j])==cards[cardsToBeDisplayed[i]].Id){
						find=true;
						break;
					}

				}
				if (!find && !cardsSold.Contains(cardsToBeDisplayed[i])){
					cardsSold.Add (cardsToBeDisplayed[i]);
					displayedCards[i-start].transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;
				}

			}

			if (cardsSold.Contains(idFocused)){
				cardFocused.transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;
			}
			newCardsNb=newTotalNbResult-(cards.Count-cardsSold.Count);
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
					tempA = cards[cardsToBeDisplayed[i]].Price;
					tempB = cards[cardsToBeDisplayed[j]].Price;
					break;
				case 1:
					tempB = cards[cardsToBeDisplayed[i]].Price;
					tempA = cards[cardsToBeDisplayed[j]].Price;
					break;
				case 2:
					tempA = cards[cardsToBeDisplayed[i]].Life;
					tempB = cards[cardsToBeDisplayed[j]].Life;
					break;
				case 3:
					tempB = cards[cardsToBeDisplayed[i]].Life;
					tempA = cards[cardsToBeDisplayed[j]].Life;
					break;
				case 4:
					tempA = cards[cardsToBeDisplayed[i]].Attack;
					tempB = cards[cardsToBeDisplayed[j]].Attack;
					break;
				case 5:
					tempB = cards[cardsToBeDisplayed[i]].Attack;
					tempA = cards[cardsToBeDisplayed[j]].Attack;
					break;
				case 6:
					tempA = cards[cardsToBeDisplayed[i]].Move;
					tempB = cards[cardsToBeDisplayed[j]].Move;
					break;
				case 7:
					tempB = cards[cardsToBeDisplayed[i]].Move;
					tempA = cards[cardsToBeDisplayed[j]].Move;
					break;
				case 8:
					tempA = cards[cardsToBeDisplayed[i]].Speed;
					tempB = cards[cardsToBeDisplayed[j]].Speed;
					break;
				case 9:
					tempB = cards[cardsToBeDisplayed[i]].Speed;
					tempA = cards[cardsToBeDisplayed[j]].Speed;
					break;
				default:

					break;
				}

				if (tempA<tempB){
					cardsToBeDisplayed.Insert (j,cardsToBeDisplayed[i]);
					cardsToBeDisplayed.RemoveAt(i+1);
					break;
				}
				
			}
		}

		clearCards();
		createCards();
		this.displayPage ();
		
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
		minLifeVal = minLifeLimit;
		maxLifeVal = maxLifeLimit;
		minAttackVal = minAttackLimit;
		maxAttackVal = maxAttackLimit;
		minMoveVal = minMoveLimit;
		maxMoveVal = maxMoveLimit;
		minQuicknessVal = minQuicknessLimit;
		maxQuicknessVal = maxQuicknessLimit;
	}

	private void applyFilters() {
		this.cardsToBeDisplayed=new List<int>();
		IList<int> tempCardsToBeDisplayed = new List<int>();
		int nbFilters = this.filtersCardType.Count;
		int tempMinPrice=0;
		int tempMaxPrice=999999999;

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
			int max = this.cards.Count;
			if (nbFilters==0){
				max = this.cards.Count;
				for (int i = 0; i < max ; i++) {
					if (cards[i].hasSkill(this.valueSkill)){
						tempCardsToBeDisplayed.Add(i);
					}
				}
			}
			else{
				for (int i = 0; i < max ; i++) {
					test = false ;
					int j = 0 ;
					while (!test && j!=nbFilters){
						if (cards[i].IdClass==this.filtersCardType[j]){
							test=true ;
							if (cards[i].hasSkill(this.valueSkill)){
									tempCardsToBeDisplayed.Add(i);
							}
						}
						j++;
					}
				}
			}
		}
		else{
			int max = this.cards.Count;
			if (nbFilters==0){
				for (int i = 0; i < max ; i++) {
						tempCardsToBeDisplayed.Add(i);
				}
			}
			else{
				for (int i = 0; i < max ; i++) {
					test = false ;
					int j = 0 ;
					while (!test && j!=nbFilters){
						if (cards[i].IdClass==this.filtersCardType[j]){
							test=true ;
								tempCardsToBeDisplayed.Add(i);
						}
						j++;
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
				if (this.cards[tempCardsToBeDisplayed[i]].Life<minLifeLimit){
					minLifeLimit = this.cards[tempCardsToBeDisplayed[i]].Life;
				}
				if (this.cards[tempCardsToBeDisplayed[i]].Life>maxLifeLimit){
					maxLifeLimit = this.cards[tempCardsToBeDisplayed[i]].Life;
				}
				if (this.cards[tempCardsToBeDisplayed[i]].Attack<minAttackLimit){
					minAttackLimit = this.cards[tempCardsToBeDisplayed[i]].Attack;
				}
				if (this.cards[tempCardsToBeDisplayed[i]].Attack>maxAttackLimit){
					maxAttackLimit = this.cards[tempCardsToBeDisplayed[i]].Attack;
				}
				if (this.cards[tempCardsToBeDisplayed[i]].Move<minMoveLimit){
					minMoveLimit = this.cards[tempCardsToBeDisplayed[i]].Move;
				}
				if (this.cards[tempCardsToBeDisplayed[i]].Move>maxMoveLimit){
					maxMoveLimit = this.cards[tempCardsToBeDisplayed[i]].Move;
				}
				if (this.cards[tempCardsToBeDisplayed[i]].Speed<minQuicknessLimit){
					minQuicknessLimit = this.cards[tempCardsToBeDisplayed[i]].Speed;
				}
				if (this.cards[tempCardsToBeDisplayed[i]].Speed>maxQuicknessLimit){
					maxQuicknessLimit = this.cards[tempCardsToBeDisplayed[i]].Speed;
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
		if(this.minPrice!=""){
			testFilters=true;
			tempMinPrice = System.Convert.ToInt32(minPrice);
		}
		if(this.maxPrice!=""){
			testFilters=true;
			tempMaxPrice = System.Convert.ToInt32(maxPrice);
		}
		
		if (testFilters == true){
			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++){
				if (cards[tempCardsToBeDisplayed[i]].verifyC2(minLifeVal,maxLifeVal,minAttackVal,maxAttackVal,minMoveVal,maxMoveVal,minQuicknessVal,maxQuicknessVal,tempMinPrice,tempMaxPrice)){
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
}

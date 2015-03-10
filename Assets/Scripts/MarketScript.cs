using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MarketScript : MonoBehaviour {


	public Texture2D backButton ;
	public Texture2D backActivatedButton ;
	public Texture2D soldCardTexture;


	int widthScreen = Screen.width ; 
	int heightScreen = Screen.height ;
	int nbCardsPerRow = 0 ;


	GUIStyle mesDecksTitleStyle;
	GUIStyle nbResultsStyle;
	GUIStyle sortTitleStyle;
	GUIStyle cantBuyPricePoliceStyle;
	GUIStyle smallPoliceStyle;
	GUIStyle moneyPoliceStyle;
	GUIStyle pricePoliceStyle;
	GUIStyle sellerPoliceStyle;
	GUIStyle myStyle;
	GUIStyle minmaxPriceStyle;
	GUIStyle minmaxPriceFieldStyle;
	GUIStyle deleteButtonStyle;
	GUIStyle monLoaderStyle;
	GUIStyle[] myDecksGuiStyle ;
	GUIStyle[] paginatorGuiStyle;
	GUIStyle buyButtonStyle;
	GUIStyle sortButtonStyle;

	public Rect windowRect ;

	private int chosenPage =0 ;

	int nbPages ;
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
	int isLoadedCards = 0 ;
	bool isBeingDragged = false;
	bool toReload = false ;
	bool isSkillToDisplay = false ;
	bool isSkillChosen = false ;
	bool isDisplayedCards = true ;
	bool createNewCards = false;
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

	int money;
	int idCardToBuy;
	int priceCardToBuy;
	int start;
	int finish;

	int totalNbResult;
	int totalNbResultLimit = 1000;

	string minPrice ="";
	string maxPrice="";

	RaycastHit hit;
	GameObject clickedCard = null;
	GameObject zoomedCard = null;
	bool cardIsMoving = false;
	Vector3 destination;
	float speed =55f;
	float timer;

	DateTime dateLimit;

	int oldSelGridInt = 10;
	int selGridInt = 10;
	string[] selStrings = new string[] {"^", "v", "^", "v", "^", "v", "^", "v", "^", "v"};


	// Use this for initialization
	void Start () {

		StartCoroutine(setStyles());
		StartCoroutine(initializeMarket());
		
		filtersCardType = new List<int> ();


	
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		if (timer > 10) {

			timer=timer-10;

			if (isLoadedCards!=0)
			StartCoroutine(refreshMarket());


		}

		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			StartCoroutine(this.setStyles());
			StartCoroutine(clearCards());
			this.createCards();

		}
		if (toReload) {
			//StartCoroutine(this.applyFilters ());
			StartCoroutine(this.displayPage ());
			toReload = false ;
		}

		if(createNewCards) {
			createNewCards=false;
			StartCoroutine(clearCards());
			getCards();
			createCards();

		}


		if (oldSelGridInt!=selGridInt){
			oldSelGridInt=selGridInt;
			sortCards();
		}


		if (Input.GetMouseButtonDown(0)) {
			

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if(Physics.Raycast(ray, out hit))
			{
				if (hit.collider.name.StartsWith("Card") || hit.collider.name.StartsWith("DeckCard") || hit.collider.name.StartsWith("Skill")){
					
					if (zoomedCard != null)
						Destroy (zoomedCard);
					
					
					if (hit.collider.name.StartsWith("Skill"))
						clickedCard = hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject;
					
					else
						clickedCard = hit.collider.gameObject;
					
					zoomedCard = Instantiate(CardObject) as GameObject;
					zoomedCard.transform.localScale= new Vector3(1.5f,1.5f,1.5f); 
					zoomedCard.GetComponent<GameCard>().Card=clickedCard.GetComponent<GameCard>().Card;
					zoomedCard.transform.localPosition=clickedCard.transform.localPosition;
					zoomedCard.GetComponent<GameCard>().ShowFace();
					zoomedCard.gameObject.name = "ZoomedCard";
					cardIsMoving = true;
					destination = camera.ScreenToWorldPoint(new Vector3(0.1f*widthScreen,0.5f*heightScreen,0));
					destination = new Vector3(destination.x,destination.y,-1);
					
				}
				
				
			}
		}

		if (cardIsMoving) {
			
			zoomedCard.transform.position = Vector3.MoveTowards(zoomedCard.transform.position, destination, speed * Time.deltaTime);
			
			if(Vector3.Distance(zoomedCard.transform.position, destination) < 0.001f)
				cardIsMoving=false;
		}
		
		if (zoomedCard!=null && !cardIsMoving) {
			
			zoomedCard.transform.position = destination;
			zoomedCard.transform.localScale= new Vector3(3f,3f,3f); 
		}



	
		
	}


	void OnGUI () {

		if (displayPopUp)
			windowRect = GUI.Window(0, new Rect(Screen.width/2-100, Screen.height/2-50, 300, 80), DoMyWindow, "Transaction abandonnée");

		
		
		if (isLoadedCards<1){

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

			if (money>0)
				GUI.Label(new Rect(10, 10, 130+ money.ToString().Length, 20), "Vous avez "+ money + " crédits",moneyPoliceStyle);
			else
				GUI.Label(new Rect(10,10, 130, 20), "Vous avez 0 crédit",mesDecksTitleStyle);

			
			
			if (isDisplayedCards){
				isDisplayedCards=false ;
				GUI.skin.toggle.normal.textColor = Color.gray;
				GUI.skin.toggle.onNormal.textColor = Color.black;
				GUI.skin.toggle.onHover.textColor = Color.blue;
				GUI.skin.toggle.hover.textColor = Color.blue;
				GUI.skin.toggle.fontSize = 10;
				GUI.skin.toggle.alignment = TextAnchor.LowerLeft;
				createCards();
				//createDeckCards();
				
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


			GUILayout.BeginArea(new Rect(widthScreen * 0.22f,0.02f*heightScreen,widthScreen * 0.56f,0.2f*heightScreen));
			{
				GUILayout.Label ("Trier les résultats",mesDecksTitleStyle,GUILayout.Height(20));

				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Prix",sortTitleStyle,GUILayout.Width(widthScreen * 0.56f/6f),GUILayout.Height(20));
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vie",sortTitleStyle,GUILayout.Width(widthScreen * 0.56f/6f),GUILayout.Height(20));
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Attaque",sortTitleStyle,GUILayout.Width(widthScreen * 0.56f/6f),GUILayout.Height(20));
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Vitesse",sortTitleStyle,GUILayout.Width(widthScreen * 0.56f/6f),GUILayout.Height(20));
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Mouvement",sortTitleStyle,GUILayout.Width(widthScreen * 0.56f/6f),GUILayout.Height(20));
					GUILayout.FlexibleSpace();

				}
				GUILayout.EndHorizontal();

				selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 10);

				if (cardsToBeDisplayed.Count != totalNbResult)
					GUILayout.Label (cardsToBeDisplayed.Count +" / " + totalNbResult + " cartes affichées",nbResultsStyle,GUILayout.Height(20));

			}
			GUILayout.EndArea();


			
			GUILayout.BeginArea(new Rect(0.82f*widthScreen,0.02f*heightScreen,widthScreen * 0.16f,0.98f*heightScreen));
			{

				bool toggle;
				string tempString ;
				GUILayout.BeginVertical();
				{

					GUILayout.Label ("Filtrer par prix",mesDecksTitleStyle,GUILayout.Height(20));
					GUILayout.Label ("Prix min :",minmaxPriceStyle);
					minPrice = GUILayout.TextField(minPrice, 9);
					GUILayout.Label ("Prix max :",minmaxPriceStyle);
					maxPrice = GUILayout.TextField(maxPrice, 9);


					GUILayout.Label ("Filtrer par classe",mesDecksTitleStyle,GUILayout.Height(20));
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
							toReload = true ;
						}
						GUILayout.Space(-5);
					}
					
					GUILayout.Space(8);
					GUILayout.Label ("Filtrer une compétence",mesDecksTitleStyle,GUILayout.Height(20));
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
							toReload = true ;
						}
					}
					if (isSkillToDisplay){
						GUILayout.Space(-5);
						for (int j=0; j<matchValues.Count; j++) {
							if (GUILayout.Button (matchValues [j], myStyle,GUILayout.Width(widthScreen/7))) {
								valueSkill = matchValues [j].ToLower ();
								skillsChosen.Add (valueSkill);
								this.isSkillChosen=true ;
								this.matchValues = new List<string>();
								toReload = true ;
								valueSkill="";
							}
						}
					}

					if (skillsChosen.Count >0){
						for (int i=0; i<this.skillsChosen.Count; i++) {	
							GUILayout.BeginHorizontal();
							{
								GUILayout.Label (skillsChosen[i],smallPoliceStyle,GUILayout.Height(20));
								GUILayout.FlexibleSpace();
								if (GUILayout.Button("Supprimer",deleteButtonStyle, GUILayout.Height(15))){
									skillsChosen.RemoveAt(i);
								}
							}
							GUILayout.EndHorizontal();
						}
					}
					
					GUILayout.Space(5);
					
					GUILayout.Label ("Filtrer par points de vie",mesDecksTitleStyle,GUILayout.Height(20));
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
					
					GUILayout.Label ("Filtrer par pts d'attaque",mesDecksTitleStyle,GUILayout.Height(20));
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
					
					GUILayout.Label ("Filtrer par déplacement",mesDecksTitleStyle,GUILayout.Height(20));
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
					
					GUILayout.Label ("Filtrer par rapidité",mesDecksTitleStyle,GUILayout.Height(20));
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

					GUILayout.Space(5);
					
					if (GUILayout.Button("Lancer une recherche")){

						selGridInt=11;
						oldSelGridInt=11;
						cardsToSearch=" AND (";
						for (int i = 0; i < togglesCurrentStates.Length; i++){

							if (togglesCurrentStates[i])
							cardsToSearch=cardsToSearch+"c.idcardType = '"+ i + "' OR ";
						}

						if (cardsToSearch==" AND (")
							cardsToSearch="";
						else
							cardsToSearch = cardsToSearch.Remove(cardsToSearch.Length - 3) + ")";

						if (skillsChosen.Count >0){

							cardsToSearch = cardsToSearch + " AND cs.idcard = c.id AND cs.isactivated='1' AND (";

							for (int i=0;i<skillsChosen.Count;i++){
								for (int j = 0; j < skillsList.Length-1; j++) {  
									if (skillsList [j].ToLower ().Contains (skillsChosen[i])) {
										cardsToSearch=cardsToSearch+" cs.idskill='"+j+"' OR";
									}
								}
							}
							cardsToSearch = cardsToSearch.Remove(cardsToSearch.Length - 2) + ")";
						
						}


						if (minLifeVal!=minLifeLimit)
							cardsToSearch= cardsToSearch + " AND (c.life >= '" + minLifeVal + "' )";

						if (maxLifeVal!=maxLifeLimit)
							cardsToSearch= cardsToSearch + " AND ('" +maxLifeVal+ "' >= c.life )";

						if (minMoveVal!=minMoveLimit)
							cardsToSearch= cardsToSearch + " AND (c.move >= '" + minMoveVal + "' )";
						
						if (maxMoveVal!=maxMoveLimit)
							cardsToSearch= cardsToSearch + " AND ('" +maxMoveVal+ "' >= c.move )";

						if (minQuicknessVal!=minQuicknessLimit)
							cardsToSearch= cardsToSearch + " AND (c.speed >= '" + minQuicknessVal + "' )";
						
						if (maxQuicknessVal!=maxQuicknessLimit)
							cardsToSearch= cardsToSearch + " AND ('" +maxQuicknessVal+ "' >= c.speed )";

						if (minAttackVal!=minAttackLimit)
							cardsToSearch= cardsToSearch + " AND (c.attack >= '" + minAttackVal + "' )";
						
						if (maxAttackVal!=maxAttackLimit)
							cardsToSearch= cardsToSearch + " AND ('" +maxAttackVal+ "' >= c.attack )";

						if (minPrice!="")
							cardsToSearch= cardsToSearch + " AND (c.price >= '" + minPrice + "' )";

						if (maxPrice!="")
							cardsToSearch= cardsToSearch + " AND ('" +maxPrice+ "' >= c.price )";

						isLoadedCards=0;
						start=0;
						finish=0;
						StartCoroutine(clearCards());
						StartCoroutine(searchForCards());
					}


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

			 


			for (int i=start; i<finish;i++){

				cardsDimensionX = new Vector3(camera.WorldToScreenPoint(new Vector3(displayedCards[i-start].transform.FindChild("texturedGameCard").renderer.bounds.size.x,0,0)).x-Screen.width/2,0,0);
				cardsPosition[i-start]=camera.WorldToScreenPoint(new Vector3 (displayedCards[i-start].transform.position.x,1.65f-((i-start)-(i-start)%nbCardsPerRow)/nbCardsPerRow*2.6f,0));


				GUILayout.BeginArea(new Rect(cardsPosition[i-start].x-cardsDimensionX.x/2,heightScreen - cardsPosition[i-start].y+10,cardsDimensionX.x,40));
				{

					GUILayout.BeginVertical(); // also can put width in here
					{	



						if (money >= cards[cardsToBeDisplayed[i]].Price && !cardsSold.Contains(cardsToBeDisplayed[i])){
							GUILayout.Label ("Prix : "+cards[cardsToBeDisplayed[i]].Price+" $",pricePoliceStyle,GUILayout.Height(15));

							GUILayout.BeginHorizontal();
							{

								GUILayout.Label ("Joueur : ",sellerPoliceStyle,GUILayout.Height(10));
								if (GUILayout.Button(usersList[cardsToBeDisplayed[i]],deleteButtonStyle, GUILayout.Height(10)))
								{
									ApplicationModel.profileChosen =usersList[cardsToBeDisplayed[i]];
									Application.LoadLevel("Profile");

								}
							}
							GUILayout.EndHorizontal();

							if (GUILayout.Button("Acheter",buyButtonStyle, GUILayout.Height(15)))
							{
							
								StartCoroutine(buyCard(cards[cardsToBeDisplayed[i]].Id.ToString(),cards[cardsToBeDisplayed[i]].Price.ToString()));
								cardsSold.Add (cardsToBeDisplayed[i]);
								displayedCards[i-start].transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;

							}
						}
						else if (money < cards[cardsToBeDisplayed[i]].Price){
							GUILayout.Label ("Prix : "+cards[cardsToBeDisplayed[i]].Price+" $",cantBuyPricePoliceStyle,GUILayout.Height(15));
							GUILayout.BeginHorizontal();
							{
								
								GUILayout.Label ("Joueur : ",sellerPoliceStyle,GUILayout.Height(10));
								if (GUILayout.Button(usersList[cardsToBeDisplayed[i]],deleteButtonStyle, GUILayout.Height(10)))
								{
									ApplicationModel.profileChosen =usersList[cardsToBeDisplayed[i]];
									Application.LoadLevel("Profile");
									
								}
							}
							GUILayout.EndHorizontal();
							GUILayout.Space (15);
						}
						else {
							GUILayout.Label ("Prix : "+cards[cardsToBeDisplayed[i]].Price+" $",pricePoliceStyle,GUILayout.Height(15));
							GUILayout.BeginHorizontal();
							{
								
								GUILayout.Label ("Joueur : ",sellerPoliceStyle,GUILayout.Height(10));
								if (GUILayout.Button(usersList[cardsToBeDisplayed[i]],deleteButtonStyle, GUILayout.Height(10)))
								{
									ApplicationModel.profileChosen =usersList[cardsToBeDisplayed[i]];
									Application.LoadLevel("Profile");
									
								}
							}
							GUILayout.EndHorizontal();
							GUILayout.Space (15);
						}

					}
					GUILayout.EndVertical();

				}
				GUILayout.EndArea();

			}



		
		}


	
	}
	
	
	private IEnumerator setStyles() {
		
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		
		this.sortTitleStyle = new GUIStyle ();
		sortTitleStyle.normal.textColor = Color.black;
		sortTitleStyle.fontSize = heightScreen/60;
		sortTitleStyle.alignment = TextAnchor.MiddleCenter;

		this.mesDecksTitleStyle = new GUIStyle ();
		mesDecksTitleStyle.normal.textColor = Color.black;
		mesDecksTitleStyle.fontSize = heightScreen/40;
		mesDecksTitleStyle.alignment = TextAnchor.MiddleCenter;

		this.minmaxPriceStyle = new GUIStyle ();
		minmaxPriceStyle.normal.textColor = Color.black;
		minmaxPriceStyle.fontSize = heightScreen/60;
		minmaxPriceStyle.alignment = TextAnchor.MiddleLeft;

		this.moneyPoliceStyle = new GUIStyle ();
		moneyPoliceStyle.normal.textColor = Color.black;
		moneyPoliceStyle.fontSize = heightScreen/40;
		moneyPoliceStyle.alignment = TextAnchor.MiddleLeft;

		this.deleteButtonStyle = new GUIStyle ();
		deleteButtonStyle.normal.textColor = Color.black;
		deleteButtonStyle.fontSize = heightScreen/80;
		deleteButtonStyle.alignment = TextAnchor.MiddleCenter;
		deleteButtonStyle.normal.background=backButton;

		this.buyButtonStyle = new GUIStyle ();
		buyButtonStyle.normal.textColor = Color.black;
		buyButtonStyle.fontSize = heightScreen/80;
		buyButtonStyle.alignment = TextAnchor.MiddleCenter;
		buyButtonStyle.normal.background=backButton;
		
		this.monLoaderStyle = new GUIStyle ();
		monLoaderStyle.normal.textColor = Color.black;
		monLoaderStyle.alignment = TextAnchor.MiddleCenter;
		
		this.smallPoliceStyle = new GUIStyle ();
		smallPoliceStyle.normal.textColor = Color.gray;
		smallPoliceStyle.alignment = TextAnchor.MiddleCenter;
		smallPoliceStyle.fontSize = 10;

		this.nbResultsStyle = new GUIStyle ();
		sortTitleStyle.normal.textColor = Color.black;
		sortTitleStyle.fontSize = heightScreen/60;
		nbResultsStyle.alignment = TextAnchor.MiddleLeft;

		this.pricePoliceStyle = new GUIStyle ();
		pricePoliceStyle.normal.textColor = Color.black;
		pricePoliceStyle.alignment = TextAnchor.MiddleCenter;
		pricePoliceStyle.fontSize = 10;

		this.cantBuyPricePoliceStyle = new GUIStyle ();
		cantBuyPricePoliceStyle.normal.textColor = Color.red;
		cantBuyPricePoliceStyle.alignment = TextAnchor.MiddleCenter;
		cantBuyPricePoliceStyle.fontSize = 10;

		this.sellerPoliceStyle = new GUIStyle ();
		sellerPoliceStyle.normal.textColor = Color.black;
		sellerPoliceStyle.alignment = TextAnchor.MiddleCenter;
		sellerPoliceStyle.fontSize = 9;
		
		this.myStyle = new GUIStyle ();
		myStyle.normal.textColor = Color.black;
		myStyle.fontSize = 10;
		myStyle.fixedHeight = 12;
		myStyle.alignment = TextAnchor.MiddleCenter;
		myStyle.normal.background = this.backButton;
		
		
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


	private IEnumerator clearCards(){
		for (int i = 0; i < 3*nbCardsPerRow; i++) {
			Destroy(displayedCards[i]);
		}
		yield break;
	}


	private IEnumerator displayPage(){
		
		start = 3 * nbCardsPerRow * chosenPage;
		finish = start + 3 * nbCardsPerRow;
		for(int i = start ; i < finish ; i++){
			//displayedCards[i].GetComponent<GameCard>().setTextResolution (1f);
			int nbCardsToDisplay = this.cardsToBeDisplayed.Count;
			if (i<nbCardsToDisplay){
				displayedCards[i-start].SetActive(true);
				displayedCards[i-start].GetComponent<GameCard>().Card = cards[this.cardsToBeDisplayed[i]];
				displayedCards[i-start].GetComponent<GameCard>().ShowFace();
				
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
	

		yield break;
	}




	private void createCards(){
		
		float tempF = 10f*widthScreen/heightScreen;
		float width = tempF * 0.6f;
		nbCardsPerRow = Mathf.FloorToInt(width/1.6f);
		float debutLargeur = -0.3f * tempF+0.8f + (width - 1.6f * nbCardsPerRow)/2 ;
		displayedCards = new GameObject[3*nbCardsPerRow];
		cardsPosition = new Vector3[3 * nbCardsPerRow];
		int nbCardsToDisplay = this.cardsToBeDisplayed.Count;

		for(int i = 0 ; i < 3*nbCardsPerRow ; i++){

			displayedCards[i] = Instantiate(CardObject) as GameObject;
			displayedCards[i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
			displayedCards[i].transform.localPosition = new Vector3(debutLargeur + 1.6f*(i%nbCardsPerRow), 2.5f-(i-i%nbCardsPerRow)/nbCardsPerRow*2.6f, 0); 
			displayedCards[i].gameObject.name = "Card" + i + "";	

			if (i<nbCardsToDisplay){

				displayedCards[i].GetComponent<GameCard>().Card = cards[this.cardsToBeDisplayed[i]];
				displayedCards[i].GetComponent<GameCard>().ShowFace();
				

				if (cardsSold.Contains(cardsToBeDisplayed[i])){
					displayedCards[i].transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;

				}

			}   
			else{
				displayedCards[i].SetActive (false);
			}
		}

		nbPages = Mathf.CeilToInt((nbCardsToDisplay-1) / (3*nbCardsPerRow))+1;

		if (chosenPage > nbPages - 1 && chosenPage !=0)
		chosenPage = chosenPage - 1;

		paginatorGuiStyle = new GUIStyle[nbPages];
		for (int i = 0; i < nbPages; i++) { 
			paginatorGuiStyle [i] = new GUIStyle ();
			paginatorGuiStyle [i].alignment = TextAnchor.MiddleCenter;
			paginatorGuiStyle [i].fontSize = 12;
			if (i==chosenPage){
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

		start = 3 * nbCardsPerRow * chosenPage;
		if (nbCardsToDisplay < 3*nbCardsPerRow){
		finish = nbCardsToDisplay;
		}
		else{
		finish = 3 * nbCardsPerRow;
		}

		//this.setFilters ();




	

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
			money = System.Convert.ToInt32(data[3]);
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
			


		}

	}

	private IEnumerator searchForCards (){
		
		this.cardsSold = new List<int> ();
		this.cardsToBeDisplayed = new List<int> ();
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_cardstosearch", cardsToSearch);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		
		w = new WWW(URLGetMarketCards, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		}
		else{

			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			cardsIDS=data[0].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
			totalNbResult = System.Convert.ToInt32(data[1]);
			dateLimit=DateTime.ParseExact(data[2], "yyyy-MM-dd hh:mm:ss", null);
			createNewCards = true;
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
						                        DateTime.ParseExact(cardInfo2[13], "yyyy-MM-dd hh:mm:ss", null))); // onSaleDate
											
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
		isLoadedCards++;
		//StartCoroutine(RetrieveCardsFromDeck ());
	}


	private IEnumerator buyCard(string idcard, string cost){
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", idcard);
		form.AddField("myform_cost", cost);
		
		WWW w = new WWW(URLBuyCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			print(w.text); // donne le retour
			if (w.text != "ALREADYSOLD")
			money = System.Convert.ToInt32(w.text);
			else{
				displayPopUp = true;
				for (int i = 0; i < 3*nbCardsPerRow; i++) {
					displayedCards[i].collider.enabled=false;
				}
			}

			
		}
		
	}

	void DoMyWindow(int windowID) {
		
		GUI.Label (new Rect(10,20,280,20),"La carte a déjà été vendue, dommage !");
		if (GUI.Button(new Rect(110,50,80,20), "Quitter")){
			displayPopUp=false;
			StartCoroutine(enableGameObjects());
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
		form.AddField ("myform_cardstosearch", cardsToSearch);
		form.AddField("myform_datelimit",  dateLimit.ToString("yyyy-MM-dd hh:mm:ss").ToString());
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());

		
		w = new WWW(URLRefreshMarket, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		}
		else if(isLoadedCards==0){
		}
		else{
			bool find = false;
			string[] cardsIDS = null;
			cardsIDS = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			cardsIDS = cardsIDS[0].Split(new char[] { '\n' }, System.StringSplitOptions.None);
			print(cardsIDS.Length-1);
			for (int i=0; i<cardsToBeDisplayed.Count;i++){


				find=false;
				for (int j=0 ; j<cardsIDS.Length-1; j++){

					if(System.Convert.ToInt32(cardsIDS[j])==cards[cardsToBeDisplayed[i]].Id){
						find=true;
						break;
					}

				}
				if (!find && !cardsSold.Contains(cardsToBeDisplayed[i])){
					cardsSold.Add (cardsToBeDisplayed[i]);
					displayedCards[i-start].transform.FindChild("texturedGameCard").renderer.material.mainTexture = soldCardTexture;
				}

			}
		}
		
	}



	public void sortCards (){

		int tempA=new int();
		int tempB=new int();


		for (int i = 1; i<cardsToBeDisplayed.Count; i++) {

			for (int j=0;j<i;j++){


				switch (selGridInt)
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
					tempA = cards[cardsToBeDisplayed[i]].Speed;
					tempB = cards[cardsToBeDisplayed[j]].Speed;
					break;
				case 7:
					tempB = cards[cardsToBeDisplayed[i]].Speed;
					tempA = cards[cardsToBeDisplayed[j]].Speed;
					break;
				case 8:
					tempA = cards[cardsToBeDisplayed[i]].Move;
					tempB = cards[cardsToBeDisplayed[j]].Move;
					break;
				case 9:
					tempB = cards[cardsToBeDisplayed[i]].Move;
					tempA = cards[cardsToBeDisplayed[j]].Move;
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

		StartCoroutine(clearCards());
		createCards();
		chosenPage=0;
		StartCoroutine(this.displayPage ());
		
	}
	



	
}

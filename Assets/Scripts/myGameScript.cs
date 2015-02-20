using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myGameScript : MonoBehaviour {
	private string URLGetDecks = "http://54.77.118.214/GarrukServer/get_decks_by_user.php";
	private string URLGetCardsByDeck = "http://54.77.118.214/GarrukServer/get_cards_by_deck.php";
//	private string URLGetCardsFullByDeck = "http://localhost/GarrukServer/get_cards_full_by_deck.php";
	private string URLGetMyCardsPage = "http://54.77.118.214/GarrukServer/get_mycardspage_data.php";

	private IList<Deck> myDecks;
	private bool isDeckEnabled = false ;
	public Texture2D backButton ;
	public Texture2D backCard ;
	public Texture2D backActivatedButton ;
	GameObject myDecksPanel ;
	GUIStyle[] myDecksGuiStyle;
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
	GUIStyle mesDecksTitleStyle;
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

	GameObject[] displayedCards ;
	void Update () {
		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			this.applyFilters();
			StartCoroutine(clearCards());
			this.createCards();
		}
		if (toReload) {
			StartCoroutine(this.applyFilters ());
			StartCoroutine(this.displayPage ());
			toReload = false ;
		}
	}

	void Start() {
		StartCoroutine(setStyles()); 
		filtersCardType = new List<int> ();
		StartCoroutine(RetrieveDecks()); 
		StartCoroutine(getCards()); 
		//StartCoroutine(createDeckCards()); 
	}

	void OnGUI()
	{
		if (isDeckEnabled) {
			GUILayout.BeginArea(new Rect(Screen.width * 0.02f,0.12f*Screen.height,Screen.width * 0.16f,0.9f*Screen.height));
			{
				GUILayout.BeginVertical(); // also can put width in here
				{
					GUILayout.Label("Mes decks",mesDecksTitleStyle,GUILayout.Height(20));
					for(int i = 0 ; i < myDecks.Count ; i++){	
						if (GUILayout.Button(myDecks[i].Name+" ("+myDecks[i].NbCards+")",myDecksGuiStyle[i], GUILayout.Height(20))) // also can put width here
						{
							if (chosenDeck != i){
								myDecksGuiStyle[chosenDeck].normal.background=backButton;
								myDecksGuiStyle[chosenDeck].normal.textColor=Color.black;
								chosenDeck = i;
								myDecksGuiStyle[i].normal.background=backActivatedButton;
								myDecksGuiStyle[i].normal.textColor=Color.white;
								chosenIdDeck=myDecks[i].Id;
							}
						}
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
		if (isLoadedCards==0){
			GUILayout.BeginArea(new Rect(Screen.width * 0.22f,0.26f*Screen.height,Screen.width * 0.78f,0.64f*Screen.height));
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
				GUI.skin.toggle.normal.textColor = Color.gray;
				GUI.skin.toggle.onNormal.textColor = Color.black;
				GUI.skin.toggle.onHover.textColor = Color.blue;
				GUI.skin.toggle.hover.textColor = Color.blue;
				GUI.skin.toggle.fontSize = 10;
				GUI.skin.toggle.alignment = TextAnchor.LowerLeft;

				createCards();
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

			GUILayout.BeginArea(new Rect(0.82f*Screen.width,0.12f*Screen.height,Screen.width * 0.16f,0.85f*Screen.height));
			{
				bool toggle;
				string tempString ;
				GUILayout.BeginVertical();
				{
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
							recalculeFiltres = true ;
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

	private IEnumerator setStyles() {
		this.mesDecksTitleStyle = new GUIStyle ();
		mesDecksTitleStyle.normal.textColor = Color.black;
		mesDecksTitleStyle.fontSize = 14;
		mesDecksTitleStyle.alignment = TextAnchor.MiddleCenter;
		
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
			print ("test : "+minLifeBool);
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
			//print ("filtres recalculés");
			//print ("minLife "+minLifeLimit);
			//print ("maxLife "+maxLifeLimit);
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
				myDecksGuiStyle[i] = new GUIStyle();
				myDecksGuiStyle[i].margin=new RectOffset(0,0,0,0);
				myDecksGuiStyle[i].alignment= TextAnchor.MiddleCenter;
				myDecksGuiStyle[i].fontSize=12;

				if (i>0){
					myDecksGuiStyle[i].normal.background=backButton;
				}
				else{
					myDecksGuiStyle[i].normal.background=backActivatedButton;
					myDecksGuiStyle[i].normal.textColor=Color.white;
				}

				deckInformation = decksInformation[i].Split('\\');
				myDecks.Add(new Deck(System.Convert.ToInt32(deckInformation[0]), deckInformation[1], System.Convert.ToInt32(deckInformation[2])));

				if (i==0){
					chosenIdDeck = System.Convert.ToInt32(deckInformation[0]);
				}
			}
		}
		isDeckEnabled = true ;
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

			for(int i = 1 ; i < cardsIDS.Length-1 ; i++){
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
						this.cards[i-1].Skills = new List<Skill>();
						this.cardsIds.Add(System.Convert.ToInt32(cardInfo2[0]));
						this.cardsToBeDisplayed.Add(i-1);
					}
					else{
						tempInt = System.Convert.ToInt32(cardInfo2[0]);
						this.cards[i-1].Skills.Add(new Skill (skillsList[System.Convert.ToInt32(cardInfo2[0])], 
						                                      System.Convert.ToInt32(cardInfo2[0]), // idskill
						                                      System.Convert.ToInt32(cardInfo2[1]), // isactivated
						                                      System.Convert.ToInt32(cardInfo2[2]), // level
						                                      System.Convert.ToInt32(cardInfo2[3]), // power
						                                      System.Convert.ToInt32(cardInfo2[4]),
						                                      cardInfo2[5])); // costmana
					}
				}
			}
			this.isLoadedCards = 2 ;
		}
	}

	private void createCards(){
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		nbCardsPerRow = 7 * 377 * widthScreen / (711*heightScreen) ;
		displayedCards = new GameObject[3*nbCardsPerRow];
		int nbCardsToDisplay = this.cardsToBeDisplayed.Count;
		for(int i = 0 ; i < 3*nbCardsPerRow ; i++){
			displayedCards[i] = Instantiate(CardObject) as GameObject;
			displayedCards[i].transform.localScale = new Vector3(0.15f, 0.02f, 0.20f); 
			displayedCards[i].transform.localPosition = new Vector3(-5.6f + (8-nbCardsPerRow)*0.8f + (1.6f * (i%nbCardsPerRow)), 0.8f-(i-i%nbCardsPerRow)/nbCardsPerRow*2.2f, 0); 
			//displayedCards[i].GetComponent<GameCard>().setTextResolution (1f);
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
			displayedCards[i].SetActive(false);
		}
		yield break;
	}

	private IEnumerator RetrieveCardsFromDeck()
	{
		deckCardsIds = new List<int>();

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
//			print(w.text); 											// donne le retour
			
			string[] cardDeckEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
			
			for(int i = 0 ; i < cardDeckEntries.Length-1 ; i++) 		// On boucle sur les attributs d'une carte
			{
				int j = 0 ;
				int id = -1 ;
				while(id==-1 && j<10000){

					if (cardsIds[j]==System.Convert.ToInt32(cardDeckEntries[i])){
						id = j;
					}
					j++;
				}

				GameObject instance = GameObject.Find("CardDeck"+i);
				instance.renderer.enabled=true ;
				instance.GetComponent<GameCard>().Card = cards[id];        					// ... et la carte qu'elle représente
				instance.GetComponent<GameCard>().ShowFace();  
			}
			for(int i = cardDeckEntries.Length-1 ; i < 5 ; i++) 		// On boucle sur les attributs d'une carte
			{
				GameObject instance = GameObject.Find("CardDeck"+i);
				instance.renderer.enabled=false ;
			}
		}
	}
}

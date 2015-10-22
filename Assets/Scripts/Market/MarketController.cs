//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Text.RegularExpressions;
//
//public class MarketController : MonoBehaviour 
//{
//	public GameObject MenuObject;
//	public GameObject CardObject;
//	public GameObject TutorialObject;
//	public int totalNbResultLimit;
//	public int refreshInterval;
//	public GUIStyle[] marketScreenVMStyle;
//	public GUIStyle[] marketVMStyle;
//	public GUIStyle[] marketFiltersVMStyle;
//	public GUIStyle[] marketCardsVMStyle;
//	
//	private MarketView view;
//	public static MarketController instance;
//	private MarketModel model;
//
//	private GameObject[] displayedCards ;
//	private GameObject cardFocused;
//	private GameObject cardPopUpBelongTo;
//	private GameObject tutorial;
//
//	private float timer;
//	private bool isTutorialLaunched;
//
//	
//	void Start () 
//	{	
//		instance = this;
//		this.view = Camera.main.gameObject.AddComponent <MarketView>();
//		this.model = new MarketModel ();
//		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
//		StartCoroutine (this.initialization ());
//	}
//	void Update()
//	{
//		this.timer += Time.deltaTime;
//		
//		if (this.timer > this.refreshInterval) 
//		{	
//			this.timer=this.timer-this.refreshInterval;
//			if(!isTutorialLaunched)
//			{
//				StartCoroutine(this.refreshMarket());
//			}
//		}
//	}
//	private IEnumerator initialization()
//	{
//		yield return StartCoroutine (model.initializeMarket (this.totalNbResultLimit));
//		this.initVM ();
//		this.initStyles ();
//		this.resize ();
//		this.createCards ();
//		this.setPagination ();
//		this.initializeToggles ();
//		this.initializeSortButtons ();
//		this.setFilters ();
//		if(!model.player.MarketTutorial)
//		{
//			this.tutorial = Instantiate(this.TutorialObject) as GameObject;
//			MenuObject.GetComponent<MenuController>().setTutorialLaunched(true);
//			this.tutorial.AddComponent<MarketTutorialController>();
//			this.tutorial.GetComponent<MarketTutorialController>().launchSequence(0);
//			this.isTutorialLaunched=true;
//		}
//	}
//	public void loadData()
//	{
//		this.resize ();
//		this.clearCards ();
//		this.createCards ();
//		this.setPagination ();
//		view.marketVM.displayView = true;
//		if(isTutorialLaunched)
//		{
//			tutorial.GetComponent<MarketTutorialController>().resize();
//		}
//	}
//	private void initVM()
//	{
//		view.marketCardsVM.nbCards=model.cards.Count;
//		view.marketCardsVM.cardsToBeDisplayed = new List<int> ();
//		for (int i=0;i<model.cards.Count;i++)
//		{
//			view.marketCardsVM.cardsToBeDisplayed.Add (i);
//		}
//	}
//	private void eraseSoldCards()
//	{
//		for(int i =0;i<model.cards.Count;i++)
//		{
//			if(model.cards[model.cards.Count-i-1].onSale==0)
//			{
//				model.cards.RemoveAt(model.cards.Count-i-1);
//			}
//		}
//	}
//	public IEnumerator refreshMarket()
//	{
//		yield return StartCoroutine(model.refreshMarket (this.totalNbResultLimit));
//		if(cardFocused!=null)
//		{
//			int tempInt = System.Convert.ToInt32(this.cardFocused.name.Substring(4))+view.marketCardsVM.start;
//			if(model.cardsSold.Contains(this.cardFocused.GetComponent<CardController>().card.Id))
//			{
//				this.cardFocused.GetComponent<CardMarketController>().resetFocusedMarketCard(model.cards[view.marketCardsVM.cardsToBeDisplayed[tempInt]]);
//			}
//		}
//		int finish = view.marketCardsVM.nbCardsToDisplay;
//		for(int i = 0 ; i < finish ; i++)
//		{
//			if(model.cardsSold.Contains(this.displayedCards[i].GetComponent<CardController>().card.Id))
//			{
//				this.displayedCards[i].GetComponent<CardMarketController>().resetMarketCard(model.cards[view.marketCardsVM.cardsToBeDisplayed[i+view.marketCardsVM.start]]);
//			}
//		}
//		if(model.newCards.Count>0)
//		{
//			view.marketCardsVM.newCardsToDisplay=true;
//			if(model.newCards.Count==1)
//			{
//				view.marketCardsVM.newCardsLabel="1 nouvelle carte - Actualiser";
//			}
//			else
//			{
//				view.marketCardsVM.newCardsLabel=model.newCards.Count+" nouvelles cartes Actualiser";
//			}
//		}
//	}
//	public IEnumerator buyCard(GameObject gameObject)
//	{
//		int tempInt = System.Convert.ToInt32(gameObject.name.Substring(4))+view.marketCardsVM.start;
//		yield return StartCoroutine(model.cards[view.marketCardsVM.cardsToBeDisplayed[tempInt]].buyCard());
//		this.displayedCards [tempInt-view.marketCardsVM.start].GetComponent<CardMarketController> ().resetMarketCard (model.cards[view.marketCardsVM.cardsToBeDisplayed[tempInt]]);
//		if(cardFocused!=null)
//		{
//			this.cardFocused.GetComponent<CardMarketController>().resetFocusedMarketCard(model.cards[view.marketCardsVM.cardsToBeDisplayed[tempInt]]);
//		}
//		if(model.cards[view.marketCardsVM.cardsToBeDisplayed[tempInt]].CollectionPoints>0)
//		{
//			StartCoroutine(this.displayedCards [tempInt-view.marketCardsVM.start].GetComponent<CardController> ().displayCollectionPointsPopUp());
//		}
//		if(model.cards[view.marketCardsVM.cardsToBeDisplayed[tempInt]].NewSkills.Count>0)
//		{
//			StartCoroutine(this.displayedCards [tempInt-view.marketCardsVM.start].GetComponent<CardController> ().displayNewSkillsPopUp());
//		}
//		if(model.cards[view.marketCardsVM.cardsToBeDisplayed[tempInt]].Error=="")
//		{
//			this.setGUI (true);
//			this.popUpDisplayed (false, gameObject);
//			this.refreshCredits();
//			int idOwner = model.cards [tempInt].IdOWner;
//			int idCard = model.cards [tempInt].Id;
//			int price = model.cards [tempInt].Price;
//			Notification tempNotification = new Notification(idOwner,model.player.Id,false,2,idCard.ToString(),price.ToString());
//			StartCoroutine(tempNotification.add ());
//		}
//		else
//		{
//			if(cardFocused!=null)
//			{
//				this.cardFocused.GetComponent<CardController>().setError();
//			}
//			else
//			{
//				this.displayedCards [tempInt].GetComponent<CardController>().setError();
//			}
//			model.cards[view.marketCardsVM.cardsToBeDisplayed[tempInt]].Error="";
//		}
//	}
//	public void displayNewCards()
//	{
//		view.marketCardsVM.newCardsToDisplay = false;
//		for(int i=0;i<model.newCards.Count;i++)
//		{
//			model.cards.Insert(i,model.newCards[i]);
//		}
//		view.marketCardsVM.nbCards=model.cards.Count;
//		model.newCards = new List<Card> ();
//		this.eraseSoldCards ();
//		this.initializeFilters ();
//	}
//	public void refreshCredits()
//	{
//		StartCoroutine(this.MenuObject.GetComponent<MenuController> ().getUserData ());
//	}
//	public void popUpDisplayed(bool value, GameObject gameObject)
//	{
//		this.cardPopUpBelongTo = gameObject;
//		view.marketVM.isPopUpDisplayed = value;
//	}
//	public void returnPressed()
//	{
//		if(view.marketVM.isPopUpDisplayed)
//		{
//			this.cardPopUpBelongTo.GetComponent<CardController> ().confirmPopUp ();
//		}
//	}
//	public void escapePressed()
//	{
//		if(view.marketVM.isPopUpDisplayed)
//		{
//			this.cardPopUpBelongTo.GetComponent<CardController> ().exitPopUp ();
//		}
//		else if(this.cardFocused!=null)
//		{
//			this.exitCard();
//		}
//	}
//	public void clickedCard(GameObject gameObject)
//	{
//		if(!view.marketVM.isPopUpDisplayed && !isTutorialLaunched)
//		{
//			view.marketVM.displayView=false ;
//			
//			int finish = 3 * view.marketCardsVM.nbCardsPerRow;
//			for(int i = 0 ; i < finish ; i++)
//			{
//				this.displayedCards[i].SetActive(false);
//			}
//			String name = "Fcus" + gameObject.name.Substring (4);
//			Vector3 scale = new Vector3(view.marketScreenVM.heightScreen / 120f,view.marketScreenVM.heightScreen / 120f,view.marketScreenVM.heightScreen / 120f);
//			Vector3 position = Camera.main.ScreenToWorldPoint (new Vector3 (0.4f * view.marketScreenVM.widthScreen, 0.45f * view.marketScreenVM.heightScreen - 1, 10));
//			this.cardFocused = Instantiate(CardObject) as GameObject;
//			this.cardFocused.AddComponent<CardMarketController> ();
//			this.cardFocused.GetComponent<CardController> ().setGameObject (name, scale, position);
//			this.cardFocused.GetComponent<CardMarketController> ().setFocusedMarketCard (gameObject.GetComponent<CardController> ().card);
//			this.cardFocused.GetComponent<CardController> ().setCentralWindowRect (view.marketScreenVM.centralWindow);
//		}
//	}
//	public void exitCard()
//	{
//		Destroy (this.cardFocused);
//		int finish = view.marketCardsVM.nbCardsToDisplay;
//		for(int i = 0 ; i < finish ; i++)
//		{
//			this.displayedCards[i].SetActive(true);
//		}
//		view.marketVM.displayView=true ;
//	}
//	public void setGUI(bool value)
//	{
//		view.marketVM.guiEnabled = value;
//		if(cardFocused==null)
//		{
//			int finish = view.marketCardsVM.nbCardsToDisplay;
//			for(int i = 0 ; i < finish ; i++)
//			{
//				this.displayedCards[i].GetComponent<CardController>().setMyGUI(value);
//			}
//		}
//		else
//		{
//			this.cardFocused.GetComponent<CardController>().setMyGUI(value);
//		}
//	}
//	public void setButtonGUI(bool value)
//	{
//		view.marketVM.guiEnabled = value;
//	}
//	private void createCards()
//	{
//		string name;
//		Vector3 scale;
//		Vector3 position;
//		float tempF = 10f*view.marketScreenVM.widthScreen/view.marketScreenVM.heightScreen;
//		float width = 10f*0.85f*view.marketScreenVM.blockLeftWidth/view.marketScreenVM.blockLeftHeight;
//		view.marketCardsVM.nbCardsPerRow = Mathf.FloorToInt(width/1.6f);
//		float debutLargeur = -0.49f * tempF+1f + (width - 1.6f * view.marketCardsVM.nbCardsPerRow)/2 ;
//		this.displayedCards = new GameObject[3*view.marketCardsVM.nbCardsPerRow];
//		view.marketCardsVM.nbCardsToDisplay = 0;
//		
//		for(int i = 0 ; i < 3*view.marketCardsVM.nbCardsPerRow ; i++)
//		{
//			name = "Card" + i;
//			scale = new Vector3(1.5f, 1.5f, 1.5f);
//			position = new Vector3(debutLargeur + 1.6f*(i%view.marketCardsVM.nbCardsPerRow), 2.625f-(i-i%view.marketCardsVM.nbCardsPerRow)/view.marketCardsVM.nbCardsPerRow*2.75f, 0); 
//			this.displayedCards[i] = Instantiate(CardObject) as GameObject;
//			this.displayedCards[i].AddComponent<CardMarketController>();
//			this.displayedCards[i].GetComponent<CardController>().setGameObject(name, scale,position);
//			
//			if (i<view.marketCardsVM.cardsToBeDisplayed.Count)
//			{
//				this.displayedCards[i].GetComponent<CardMarketController>().setMarketCard(model.cards[view.marketCardsVM.cardsToBeDisplayed[i]]);
//				this.displayedCards[i].GetComponent<CardController> ().setCentralWindowRect (view.marketScreenVM.centralWindow);
//				this.displayedCards[i].GetComponent<CardController>().setCollectionPointsWindowRect(view.marketScreenVM.collectionPointsWindow);
//				this.displayedCards[i].GetComponent<CardController>().setNewSkillsWindowRect(view.marketScreenVM.newSkillsWindow);
//				view.marketCardsVM.nbCardsToDisplay++;
//			}   
//			else{
//				this.displayedCards[i].SetActive (false);
//			}
//		}
//	}
//	private void setPagination()
//	{
//		view.marketCardsVM.nbPages = Mathf.CeilToInt((view.marketCardsVM.cardsToBeDisplayed.Count-1) / (3*view.marketCardsVM.nbCardsPerRow))+1;
//		view.marketCardsVM.pageDebut = 0 ;
//		if (view.marketCardsVM.nbPages>15)
//		{
//			view.marketCardsVM.pageFin = 15 ;
//		}
//		else
//		{
//			view.marketCardsVM.pageFin = view.marketCardsVM.nbPages ;
//		}
//		view.marketCardsVM.chosenPage = 0;
//		view.marketCardsVM.paginatorGuiStyle = new GUIStyle[view.marketCardsVM.nbPages];
//		for (int i = 0; i < view.marketCardsVM.nbPages; i++) 
//		{ 
//			if (i==0)
//			{
//				view.marketCardsVM.paginatorGuiStyle[i]=view.marketVM.paginationActivatedStyle;
//			}
//			else
//			{
//				view.marketCardsVM.paginatorGuiStyle[i]=view.marketVM.paginationStyle;
//			}
//		}
//	}	
//	private void displayPage()
//	{
//		view.marketCardsVM.start = 3 * view.marketCardsVM.nbCardsPerRow * view.marketCardsVM.chosenPage;
//		view.marketCardsVM.finish = view.marketCardsVM.start + 3 * view.marketCardsVM.nbCardsPerRow;
//		view.marketCardsVM.nbCardsToDisplay = 0;
//		for(int i = view.marketCardsVM.start ; i < view.marketCardsVM.finish ; i++)
//		{
//			if (i<view.marketCardsVM.cardsToBeDisplayed.Count)
//			{
//				this.displayedCards[i-view.marketCardsVM.start].SetActive(true);
//				this.displayedCards[i-view.marketCardsVM.start].GetComponent<CardMarketController>().resetMarketCard(model.cards[view.marketCardsVM.cardsToBeDisplayed[i]]);
//				view.marketCardsVM.nbCardsToDisplay++;
//			}
//			else
//			{
//				displayedCards[i-view.marketCardsVM.start].SetActive(false);
//			}
//		}
//	}
//	private void initStyles()
//	{
//		view.marketScreenVM.styles=new GUIStyle[this.marketScreenVMStyle.Length];
//		for(int i=0;i<this.marketScreenVMStyle.Length;i++)
//		{
//			view.marketScreenVM.styles[i]=this.marketScreenVMStyle[i];
//		}
//		view.marketScreenVM.initStyles();
//		view.marketVM.styles=new GUIStyle[this.marketVMStyle.Length];
//		for(int i=0;i<this.marketVMStyle.Length;i++)
//		{
//			view.marketVM.styles[i]=this.marketVMStyle[i];
//		}
//		view.marketVM.initStyles();
//		view.marketFiltersVM.styles=new GUIStyle[this.marketFiltersVMStyle.Length];
//		for(int i=0;i<this.marketFiltersVMStyle.Length;i++)
//		{
//			view.marketFiltersVM.styles[i]=this.marketFiltersVMStyle[i];
//		}
//		view.marketFiltersVM.initStyles();
//		view.marketCardsVM.styles=new GUIStyle[this.marketCardsVMStyle.Length];
//		for(int i=0;i<this.marketCardsVMStyle.Length;i++)
//		{
//			view.marketCardsVM.styles[i]=this.marketCardsVMStyle[i];
//		}
//		view.marketCardsVM.initStyles();
//	}
//	private void resize()
//	{
//		view.marketScreenVM.resize ();
//		view.marketFiltersVM.resize (view.marketScreenVM.heightScreen);
//		view.marketVM.resize (view.marketScreenVM.heightScreen);
//		view.marketCardsVM.resize (view.marketScreenVM.heightScreen);
//	}
//	private void clearCards()
//	{
//		if(this.cardFocused!=null)
//		{
//			Destroy (this.cardFocused);
//		}
//		for (int i = 0; i < 3*view.marketCardsVM.nbCardsPerRow; i++) 
//		{
//			Destroy(this.displayedCards[i]);
//		}
//	}
//	private void initializeSortButtons()
//	{
//		for (int i =0;i<10;i++)
//		{
//			view.marketFiltersVM.sortButtonStyle[i]=view.marketVM.sortDefaultButtonStyle;
//		}
//	}
//	private void initializeToggles()
//	{
//		view.marketFiltersVM.cardTypeList=new string[model.cardTypeList.Length];
//		for(int i=0;i<model.cardTypeList.Length-1;i++)
//		{
//			view.marketFiltersVM.cardTypeList[i] = model.cardTypeList[i];
//		}
//		view.marketFiltersVM.togglesCurrentStates = new bool[model.cardTypeList.Length];
//		for(int i = 0 ; i < model.cardTypeList.Length-1 ; i++)
//		{
//			view.marketFiltersVM.togglesCurrentStates[i] = false;
//		}
//	}
//	private void setFilters()
//	{
//		view.marketFiltersVM.minLifeLimit=10000;
//		view.marketFiltersVM.maxLifeLimit=0;
//		view.marketFiltersVM.minAttackLimit=10000;
//		view.marketFiltersVM.maxAttackLimit=0;
//		view.marketFiltersVM.minQuicknessLimit=10000;
//		view.marketFiltersVM.maxQuicknessLimit=0;
//		
//		int max = model.cards.Count;
//		for (int i = 0; i < max ; i++) {
//			if (model.cards[i].Life<view.marketFiltersVM.minLifeLimit){
//				view.marketFiltersVM.minLifeLimit = model.cards[i].Life;
//			}
//			if (model.cards[i].Life>view.marketFiltersVM.maxLifeLimit){
//				view.marketFiltersVM.maxLifeLimit = model.cards[i].Life;
//			}
//			if (model.cards[i].Attack<view.marketFiltersVM.minAttackLimit){
//				view.marketFiltersVM.minAttackLimit = model.cards[i].Attack;
//			}
//			if (model.cards[i].Attack>view.marketFiltersVM.maxAttackLimit){
//				view.marketFiltersVM.maxAttackLimit = model.cards[i].Attack;
//			}
//			if (model.cards[i].Speed<view.marketFiltersVM.minQuicknessLimit){
//				view.marketFiltersVM.minQuicknessLimit = model.cards[i].Speed;
//			}
//			if (model.cards[i].Speed>view.marketFiltersVM.maxQuicknessLimit){
//				view.marketFiltersVM.maxQuicknessLimit = model.cards[i].Speed;
//			}
//		}
//		if(view.marketFiltersVM.minLifeLimit>view.marketFiltersVM.maxLifeLimit)
//		{
//			view.marketFiltersVM.minLifeLimit=0;
//			view.marketFiltersVM.maxLifeLimit=0;
//		}
//		if(view.marketFiltersVM.minAttackLimit>view.marketFiltersVM.maxAttackLimit)
//		{
//			view.marketFiltersVM.minAttackLimit=0;
//			view.marketFiltersVM.maxAttackLimit=0;
//		}
//		if(view.marketFiltersVM.minQuicknessLimit>view.marketFiltersVM.maxQuicknessLimit)
//		{
//			view.marketFiltersVM.minQuicknessLimit=0;
//			view.marketFiltersVM.maxQuicknessLimit=0;
//		}
//		view.marketFiltersVM.minLifeVal = view.marketFiltersVM.minLifeLimit;
//		view.marketFiltersVM.maxLifeVal = view.marketFiltersVM.maxLifeLimit;
//		view.marketFiltersVM.oldMinLifeVal = view.marketFiltersVM.minLifeLimit;
//		view.marketFiltersVM.oldMaxLifeVal = view.marketFiltersVM.maxLifeLimit;
//		view.marketFiltersVM.minAttackVal = view.marketFiltersVM.minAttackLimit;
//		view.marketFiltersVM.maxAttackVal = view.marketFiltersVM.maxAttackLimit;
//		view.marketFiltersVM.oldMinAttackVal = view.marketFiltersVM.minAttackLimit;
//		view.marketFiltersVM.oldMaxAttackVal = view.marketFiltersVM.maxAttackLimit;
//		view.marketFiltersVM.minQuicknessVal = view.marketFiltersVM.minQuicknessLimit;
//		view.marketFiltersVM.maxQuicknessVal = view.marketFiltersVM.maxQuicknessLimit;
//		view.marketFiltersVM.oldMinQuicknessVal = view.marketFiltersVM.minQuicknessLimit;
//		view.marketFiltersVM.oldMaxQuicknessVal = view.marketFiltersVM.maxQuicknessLimit;
//	}
//	public void paginationBack()
//	{
//		view.marketCardsVM.pageDebut = view.marketCardsVM.pageDebut-15;
//		view.marketCardsVM.pageFin = view.marketCardsVM.pageDebut+15;
//	}
//	public void paginationSelect(int chosenPage)
//	{
//		view.marketCardsVM.paginatorGuiStyle[view.marketCardsVM.chosenPage]=view.marketVM.paginationStyle;
//		view.marketCardsVM.chosenPage=chosenPage;
//		view.marketCardsVM.paginatorGuiStyle[chosenPage]=this.view.marketVM.paginationActivatedStyle;
//		this.displayPage();
//	}
//	public void paginationNext()
//	{
//		view.marketCardsVM.pageDebut = view.marketCardsVM.pageDebut+15;
//		view.marketCardsVM.pageFin = Mathf.Min(view.marketCardsVM.pageFin+15, view.marketCardsVM.nbPages);
//	}
//
//	private void applyFilters() {
//		view.marketCardsVM.cardsToBeDisplayed=new List<int>();
//		IList<int> tempCardsToBeDisplayed = new List<int>();
//		int nbFilters = view.marketFiltersVM.filtersCardType.Count;
//		int tempMinPrice=0;
//		int tempMaxPrice=999999999;
//		
//		bool testFilters = false;
//		bool test ;
//		
//		bool minLifeBool = (view.marketFiltersVM.minLifeLimit==view.marketFiltersVM.minLifeVal);
//		bool maxLifeBool = (view.marketFiltersVM.maxLifeLimit==view.marketFiltersVM.maxLifeVal);
//		bool minQuicknessBool = (view.marketFiltersVM.minQuicknessLimit==view.marketFiltersVM.minQuicknessVal);
//		bool maxQuicknessBool = (view.marketFiltersVM.maxQuicknessLimit==view.marketFiltersVM.maxQuicknessVal);
//		bool minAttackBool = (view.marketFiltersVM.minAttackLimit==view.marketFiltersVM.minAttackVal);
//		bool maxAttackBool = (view.marketFiltersVM.maxAttackLimit==view.marketFiltersVM.maxAttackVal);
//		
//		
//		if (view.marketFiltersVM.isSkillChosen)
//		{
//			int max = model.cards.Count;
//			if (nbFilters==0){
//				max = model.cards.Count;
//				for (int i = 0; i < max ; i++) {
//					if (model.cards[i].hasSkill(view.marketFiltersVM.valueSkill)){
//						tempCardsToBeDisplayed.Add(i);
//					}
//				}
//			}
//			else{
//				for (int i = 0; i < max ; i++) {
//					test = false ;
//					int j = 0 ;
//					while (!test && j!=nbFilters){
//						if (model.cards[i].IdClass==view.marketFiltersVM.filtersCardType[j]){
//							test=true ;
//							if (model.cards[i].hasSkill(view.marketFiltersVM.valueSkill)){
//								tempCardsToBeDisplayed.Add(i);
//							}
//						}
//						j++;
//					}
//				}
//			}
//		}
//		else{
//			int max = model.cards.Count;
//			if (nbFilters==0){
//				for (int i = 0; i < max ; i++) {
//					tempCardsToBeDisplayed.Add(i);
//				}
//			}
//			else{
//				for (int i = 0; i < max ; i++) {
//					test = false ;
//					int j = 0 ;
//					while (!test && j!=nbFilters){
//						if (model.cards[i].IdClass==view.marketFiltersVM.filtersCardType[j]){
//							test=true ;
//							tempCardsToBeDisplayed.Add(i);
//						}
//						j++;
//					}
//				}
//			}
//		}
//		if (tempCardsToBeDisplayed.Count>0){
//			view.marketFiltersVM.minLifeLimit=10000;
//			view.marketFiltersVM.maxLifeLimit=0;
//			view.marketFiltersVM.minAttackLimit=10000;
//			view.marketFiltersVM.maxAttackLimit=0;
//			view.marketFiltersVM.minQuicknessLimit=10000;
//			view.marketFiltersVM.maxQuicknessLimit=0;
//			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++){
//				if (model.cards[tempCardsToBeDisplayed[i]].Life<view.marketFiltersVM.minLifeLimit){
//					view.marketFiltersVM.minLifeLimit = model.cards[tempCardsToBeDisplayed[i]].Life;
//				}
//				if (model.cards[tempCardsToBeDisplayed[i]].Life>view.marketFiltersVM.maxLifeLimit){
//					view.marketFiltersVM.maxLifeLimit = model.cards[tempCardsToBeDisplayed[i]].Life;
//				}
//				if (model.cards[tempCardsToBeDisplayed[i]].Attack<view.marketFiltersVM.minAttackLimit){
//					view.marketFiltersVM.minAttackLimit = model.cards[tempCardsToBeDisplayed[i]].Attack;
//				}
//				if (model.cards[tempCardsToBeDisplayed[i]].Attack>view.marketFiltersVM.maxAttackLimit){
//					view.marketFiltersVM.maxAttackLimit = model.cards[tempCardsToBeDisplayed[i]].Attack;
//				}
//				if (model.cards[tempCardsToBeDisplayed[i]].Speed<view.marketFiltersVM.minQuicknessLimit){
//					view.marketFiltersVM.minQuicknessLimit = model.cards[tempCardsToBeDisplayed[i]].Speed;
//				}
//				if (model.cards[tempCardsToBeDisplayed[i]].Speed>view.marketFiltersVM.maxQuicknessLimit){
//					view.marketFiltersVM.maxQuicknessLimit = model.cards[tempCardsToBeDisplayed[i]].Speed;
//				}
//			}
//			if (minLifeBool && view.marketFiltersVM.maxLifeVal>view.marketFiltersVM.minLifeLimit){
//				view.marketFiltersVM.minLifeVal = view.marketFiltersVM.minLifeLimit;
//			}
//			else{
//				if (view.marketFiltersVM.minLifeVal<view.marketFiltersVM.minLifeLimit){
//					view.marketFiltersVM.minLifeLimit = view.marketFiltersVM.minLifeVal;
//				}
//			}
//			if (maxLifeBool && view.marketFiltersVM.minLifeVal<view.marketFiltersVM.maxLifeLimit){
//				view.marketFiltersVM.maxLifeVal = view.marketFiltersVM.maxLifeLimit;
//			}
//			else{
//				if (view.marketFiltersVM.maxLifeVal>view.marketFiltersVM.maxLifeLimit){
//					view.marketFiltersVM.maxLifeLimit = view.marketFiltersVM.maxLifeVal;
//				}
//			}
//			if (minAttackBool && view.marketFiltersVM.maxAttackVal>view.marketFiltersVM.minAttackLimit){
//				view.marketFiltersVM.minAttackVal = view.marketFiltersVM.minAttackLimit;
//			}
//			else{
//				if (view.marketFiltersVM.minAttackVal<view.marketFiltersVM.minAttackLimit){
//					view.marketFiltersVM.minAttackLimit = view.marketFiltersVM.minAttackVal;
//				}
//			}
//			if (maxAttackBool && view.marketFiltersVM.minAttackVal<view.marketFiltersVM.maxAttackLimit){
//				view.marketFiltersVM.maxAttackVal = view.marketFiltersVM.maxAttackLimit;
//			}
//			else{
//				if (view.marketFiltersVM.maxAttackVal>view.marketFiltersVM.maxAttackLimit){
//					view.marketFiltersVM.maxAttackLimit = view.marketFiltersVM.maxAttackVal;
//				}
//			}
//			if (minQuicknessBool && view.marketFiltersVM.maxQuicknessVal>view.marketFiltersVM.minQuicknessLimit){
//				view.marketFiltersVM.minQuicknessVal = view.marketFiltersVM.minQuicknessLimit;
//			}
//			else{
//				if (view.marketFiltersVM.minQuicknessVal<view.marketFiltersVM.minQuicknessLimit){
//					view.marketFiltersVM.minQuicknessLimit = view.marketFiltersVM.minQuicknessVal;
//				}
//			}
//			if (maxQuicknessBool && view.marketFiltersVM.minQuicknessVal<view.marketFiltersVM.maxQuicknessLimit){
//				view.marketFiltersVM.maxQuicknessVal = view.marketFiltersVM.maxQuicknessLimit;
//			}
//			else{
//				if (view.marketFiltersVM.maxQuicknessVal>view.marketFiltersVM.maxQuicknessLimit){
//					view.marketFiltersVM.maxQuicknessLimit = view.marketFiltersVM.maxQuicknessVal;
//				}
//			}
//			
//			view.marketFiltersVM.oldMinLifeVal = view.marketFiltersVM.minLifeVal ;
//			view.marketFiltersVM.oldMaxLifeVal = view.marketFiltersVM.maxLifeVal ;
//			view.marketFiltersVM.oldMinQuicknessVal = view.marketFiltersVM.minQuicknessVal ;
//			view.marketFiltersVM.oldMaxQuicknessVal = view.marketFiltersVM.maxQuicknessVal ;
//			view.marketFiltersVM.oldMinAttackVal = view.marketFiltersVM.minAttackVal ;
//			view.marketFiltersVM.oldMaxAttackVal = view.marketFiltersVM.maxAttackVal ;
//		}
//		
//		if (view.marketFiltersVM.minLifeVal!=view.marketFiltersVM.minLifeLimit){
//			testFilters = true ;
//		}
//		else if (view.marketFiltersVM.maxLifeVal!=view.marketFiltersVM.maxLifeLimit){
//			testFilters = true ;
//		}
//		else if (view.marketFiltersVM.minAttackVal!=view.marketFiltersVM.minAttackLimit){
//			testFilters = true ;
//		}
//		else if (view.marketFiltersVM.maxAttackVal!=view.marketFiltersVM.maxAttackLimit){
//			testFilters = true ;
//		}
//		else if (view.marketFiltersVM.minQuicknessVal!=view.marketFiltersVM.minQuicknessLimit){
//			testFilters = true ;
//		}
//		else if (view.marketFiltersVM.maxQuicknessVal!=view.marketFiltersVM.maxQuicknessLimit){
//			testFilters = true ;
//		}
//		if(view.marketFiltersVM.minPrice!=""){
//			testFilters=true;
//			tempMinPrice = System.Convert.ToInt32(view.marketFiltersVM.minPrice);
//		}
//		if(view.marketFiltersVM.maxPrice!=""){
//			testFilters=true;
//			tempMaxPrice = System.Convert.ToInt32(view.marketFiltersVM.maxPrice);
//		}
//		
//		if (testFilters == true){
//			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++)
//			{
//				if (model.cards[tempCardsToBeDisplayed[i]].verifyC2((float)tempMinPrice, 
//				                                                    (float)tempMaxPrice,
//				                                                    0,
//				                                                    100,
//				                                                    view.marketFiltersVM.minLifeVal,
//				                                                    view.marketFiltersVM.maxLifeVal,
//				                                                    view.marketFiltersVM.minAttackVal,
//				                                                    view.marketFiltersVM.maxAttackVal,
//				                                                    view.marketFiltersVM.minQuicknessVal,
//				                                                    view.marketFiltersVM.maxQuicknessVal)){
//					view.marketCardsVM.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
//				}
//			}
//		}
//		else
//		{
//			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++)
//			{
//				view.marketCardsVM.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
//			}
//		}
//		view.marketCardsVM.nbPages = Mathf.CeilToInt(view.marketCardsVM.cardsToBeDisplayed.Count / (3.0f*view.marketCardsVM.nbCardsPerRow));
//		view.marketCardsVM.pageDebut = 0 ;
//		if (view.marketCardsVM.nbPages>15){
//			view.marketCardsVM.pageFin = 15 ;
//		}
//		else
//		{
//			view.marketCardsVM.pageFin = view.marketCardsVM.nbPages ;
//		}
//		view.marketCardsVM.chosenPage = 0;
//		view.marketCardsVM.start = 0;
//		view.marketCardsVM.paginatorGuiStyle = new GUIStyle[view.marketCardsVM.nbPages];
//		for (int i = 0; i < view.marketCardsVM.nbPages; i++) { 
//			if (i==0){
//				view.marketCardsVM.paginatorGuiStyle[i]=view.marketVM.paginationActivatedStyle;
//			}
//			else{
//				view.marketCardsVM.paginatorGuiStyle[i]=view.marketVM.paginationStyle;
//			}
//		}
//	}
//	public void filterCards()
//	{
//		this.applyFilters ();
//		if (view.marketFiltersVM.oldSortSelected!=10)
//		{
//			this.sortCards(view.marketFiltersVM.oldSortSelected);
//		}
//		this.clearCards ();
//		this.createCards ();
//		this.setPagination ();
//	}
//	public void isBeingDragged()
//	{
//		view.marketVM.isBeingDragged = true;
//	}
//	public void isNotBeingDragged()
//	{
//		if(view.marketVM.isBeingDragged)
//		{
//			view.marketVM.isBeingDragged = false;
//			this.refreshMinMaxFilters();
//		}
//	}
//	public void refreshMinMaxFilters()
//	{
//		bool isMoved = false ;
//		view.marketFiltersVM.maxLifeVal=Mathf.RoundToInt(view.marketFiltersVM.maxLifeVal);
//		view.marketFiltersVM.minLifeVal=Mathf.RoundToInt(view.marketFiltersVM.minLifeVal);
//		view.marketFiltersVM.maxAttackVal=Mathf.RoundToInt(view.marketFiltersVM.maxAttackVal);
//		view.marketFiltersVM.minAttackVal=Mathf.RoundToInt(view.marketFiltersVM.minAttackVal);
//		view.marketFiltersVM.maxQuicknessVal=Mathf.RoundToInt(view.marketFiltersVM.maxQuicknessVal);
//		view.marketFiltersVM.minQuicknessVal=Mathf.RoundToInt(view.marketFiltersVM.minQuicknessVal);
//		
//		if (view.marketFiltersVM.oldMaxLifeVal != view.marketFiltersVM.maxLifeVal){
//			view.marketFiltersVM.oldMaxLifeVal = view.marketFiltersVM.maxLifeVal;
//			isMoved = true ; 
//		}
//		if (view.marketFiltersVM.oldMinLifeVal != view.marketFiltersVM.minLifeVal){
//			view.marketFiltersVM.oldMinLifeVal = view.marketFiltersVM.minLifeVal;
//			isMoved = true ; 
//		}
//		if (view.marketFiltersVM.oldMaxAttackVal != view.marketFiltersVM.maxAttackVal){
//			view.marketFiltersVM.oldMaxAttackVal = view.marketFiltersVM.maxAttackVal;
//			isMoved = true ; 
//		}
//		if (view.marketFiltersVM.oldMinAttackVal != view.marketFiltersVM.minAttackVal){
//			view.marketFiltersVM.oldMinAttackVal = view.marketFiltersVM.minAttackVal;
//			isMoved = true ; 
//		}
//		if (view.marketFiltersVM.oldMaxQuicknessVal != view.marketFiltersVM.maxQuicknessVal){
//			view.marketFiltersVM.oldMaxQuicknessVal = view.marketFiltersVM.maxQuicknessVal;
//			isMoved = true ; 
//		}
//		if (view.marketFiltersVM.oldMinQuicknessVal != view.marketFiltersVM.minQuicknessVal){
//			view.marketFiltersVM.oldMinQuicknessVal = view.marketFiltersVM.minQuicknessVal;
//			isMoved = true ; 
//		}
//		if(isMoved){
//			this.filterCards();
//		}
//	}
//	public void editMinPrice(string value)
//	{
//		view.marketFiltersVM.minPrice= Regex.Replace(value, "[^0-9]", "");
//		this.filterCards ();
//	}
//	public void editMaxPrice(string value)
//	{
//		view.marketFiltersVM.maxPrice= Regex.Replace(value, "[^0-9]", "");
//		this.filterCards ();
//	}
//	public void selectCardType(bool value, int id)
//	{
//		view.marketFiltersVM.togglesCurrentStates [id] = value;
//		if (value)
//		{
//			view.marketFiltersVM.filtersCardType.Add(id);
//		}
//		else
//		{
//			view.marketFiltersVM.filtersCardType.Remove(id);
//		}
//		this.filterCards ();
//	}
//	public void selectSkills(string value)
//	{
//		if (value.Length > 0) 
//		{
//			view.marketFiltersVM.isSkillToDisplay=true;
//			view.marketFiltersVM.valueSkill = value.ToLower ();
//			view.marketFiltersVM.matchValues = new List<string> ();	
//			if (view.marketFiltersVM.valueSkill != "") 
//			{
//				view.marketFiltersVM.matchValues = new List<string> ();
//				for (int i = 0; i < model.skillsList.Count; i++) 
//				{  
//					if (model.skillsList [i].ToLower ().Contains (view.marketFiltersVM.valueSkill)) 
//					{
//						view.marketFiltersVM.matchValues.Add (model.skillsList [i]);
//					}
//				}
//			}
//		} 
//		else 
//		{
//			view.marketFiltersVM.isSkillToDisplay=false;
//			view.marketFiltersVM.valueSkill = "";
//		}
//		if (view.marketFiltersVM.isSkillChosen)
//		{
//			view.marketFiltersVM.isSkillChosen=false ;
//			this.filterCards() ;
//		}
//	}
//	public void filterASkill(string value)
//	{
//		view.marketFiltersVM.valueSkill = value.ToLower ();
//		view.marketFiltersVM.isSkillChosen=true ;
//		view.marketFiltersVM.matchValues = new List<string>();
//		this.filterCards ();
//	}
//	public void sortCards(int id)
//	{
//		this.applySorts (id);
//		this.loadData ();
//	}
//	public void applySorts(int id)
//	{
//		int tempA=new int();
//		int tempB=new int();
//		
//		if(view.marketFiltersVM.oldSortSelected!=10)
//		{
//			view.marketFiltersVM.sortButtonStyle[view.marketFiltersVM.oldSortSelected]=view.marketVM.sortDefaultButtonStyle;
//		}
//		view.marketFiltersVM.sortButtonStyle[id]=view.marketVM.sortActivatedButtonStyle;
//		view.marketFiltersVM.oldSortSelected=id;
//		
//		for (int i = 1; i<view.marketCardsVM.cardsToBeDisplayed.Count; i++) {
//			
//			for (int j=0;j<i;j++){
//				
//				
//				switch (id)
//				{
//				case 0:
//					tempA = model.cards[view.marketCardsVM.cardsToBeDisplayed[i]].Price;
//					tempB = model.cards[view.marketCardsVM.cardsToBeDisplayed[j]].Price;
//					break;
//				case 1:
//					tempB = model.cards[view.marketCardsVM.cardsToBeDisplayed[i]].Price;
//					tempA = model.cards[view.marketCardsVM.cardsToBeDisplayed[j]].Price;
//					break;
//				case 2:
//					tempA = model.cards[view.marketCardsVM.cardsToBeDisplayed[i]].Life;
//					tempB = model.cards[view.marketCardsVM.cardsToBeDisplayed[j]].Life;
//					break;
//				case 3:
//					tempB = model.cards[view.marketCardsVM.cardsToBeDisplayed[i]].Life;
//					tempA = model.cards[view.marketCardsVM.cardsToBeDisplayed[j]].Life;
//					break;
//				case 4:
//					tempA = model.cards[view.marketCardsVM.cardsToBeDisplayed[i]].Attack;
//					tempB = model.cards[view.marketCardsVM.cardsToBeDisplayed[j]].Attack;
//					break;
//				case 5:
//					tempB = model.cards[view.marketCardsVM.cardsToBeDisplayed[i]].Attack;
//					tempA = model.cards[view.marketCardsVM.cardsToBeDisplayed[j]].Attack;
//					break;
//				case 6:
//					tempA = model.cards[view.marketCardsVM.cardsToBeDisplayed[i]].Speed;
//					tempB = model.cards[view.marketCardsVM.cardsToBeDisplayed[j]].Speed;
//					break;
//				case 7:
//					tempB = model.cards[view.marketCardsVM.cardsToBeDisplayed[i]].Speed;
//					tempA = model.cards[view.marketCardsVM.cardsToBeDisplayed[j]].Speed;
//					break;
//				default:
//					
//					break;
//				}
//				
//				if (tempA<tempB){
//					view.marketCardsVM.cardsToBeDisplayed.Insert (j,view.marketCardsVM.cardsToBeDisplayed[i]);
//					view.marketCardsVM.cardsToBeDisplayed.RemoveAt(i+1);
//					break;
//				}
//				
//			}
//		}
//	}
//	public int getNbCardsDisplayed()
//	{
//		return view.marketCardsVM.cardsToBeDisplayed.Count;
//	}
//	public Vector2 getCardsPosition(int index)
//	{
//		return this.displayedCards[index].GetComponent<CardController>().GOPosition;
//	}
//	public Vector2 getCardsSize(int index)
//	{
//		return this.displayedCards[index].GetComponent<CardController>().GOSize;
//	}
//	public void initializeFilters()
//	{
//		this.initVM ();
//		this.clearCards ();
//		this.createCards ();
//		this.setPagination ();
//		view.marketFiltersVM.setFilters ();
//		this.initializeToggles ();
//		this.initializeSortButtons ();
//		this.setFilters ();
//	}
//	public IEnumerator endTutorial(bool toUpdate)
//	{
//		if(toUpdate)
//		{
//			yield return StartCoroutine (model.player.setMarketTutorial(true));
//		}
//		MenuController.instance.setButtonsGui (true);
//		Destroy (this.tutorial);
//		this.isTutorialLaunched = false;
//		MenuController.instance.isTutorialLaunched = false;
//		this.setGUI (true);
//		yield break;
//	}
//}
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MyGameController : MonoBehaviour
{
	public static MyGameController instance;
	private MyGameView view;
	private MyGameModel model;
	public GameObject MenuObject;
	public GameObject CardObject;
	private GameObject[] displayedCards;
	private GameObject cardFocused;
	
	public GUIStyle[] myGameScreenVMStyle;
	public GUIStyle[] myGameVMStyle;
	public GUIStyle[] myGameFiltersVMStyle;
	public GUIStyle[] myGameCardsVMStyle;
	public GUIStyle[] myGameDecksVMStyle;

	void Start()
	{
		instance = this;
		this.view = Camera.main.gameObject.AddComponent <MyGameView>();
		this.model = new MyGameModel ();
		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
		StartCoroutine (this.initialization ());
	}
	public IEnumerator initialization()
	{
		yield return StartCoroutine (model.initializeMyGame ());
		this.initStyles ();
		this.initMyGameCardsVM ();
		this.initMyGameDecksVM ();
		this.resize ();
		this.createCards ();
		this.setPagination ();
		this.initializeSortButtons ();
		this.initializeToggles ();
		this.setFilters ();
	}
	public void loadData()
	{
		this.resize ();
		this.clearCards ();
		this.createCards ();
		this.setPagination ();
		view.myGameVM.displayView = true;
	}
	public void resetAll()
	{
		this.initMyGameCardsVM ();
		this.clearCards ();
		this.createCards ();
		this.setPagination ();
		this.initializeSortButtons ();
		this.initializeToggles ();
		this.setFilters ();
		view.myGameVM.displayView = true;
		this.setGUI (true);
	}
	public void clickedCard(GameObject gameObject)
	{
		view.myGameVM.displayView=false ;
		
		int finish = 3 * view.myGameCardsVM.nbCardsPerRow;
		for(int i = 0 ; i < finish ; i++)
		{
			this.displayedCards[i].SetActive(false);
		}
		float scale = view.myGameScreenVM.heightScreen / 120f;
		this.cardFocused = Instantiate(CardObject) as GameObject;
		this.cardFocused.transform.localScale = new Vector3(scale, scale, scale); 
		this.cardFocused.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0.4f*view.myGameScreenVM.widthScreen ,0.45f*view.myGameScreenVM.heightScreen-1 , 10));  
		this.cardFocused.gameObject.name = "Fcus"+gameObject.name.Substring(4);	
		this.cardFocused.AddComponent<CardMyGameController> ();
		this.cardFocused.GetComponent<CardMyGameController> ().setFocusedMyGameCard (gameObject.GetComponent<CardController> ().card);
		this.cardFocused.GetComponent<CardController> ().setCentralWindowRect (view.myGameScreenVM.centralWindow);
	}
	public void exitCard()
	{
		Destroy (this.cardFocused);
		int finish = view.myGameCardsVM.nbCardsToDisplay;
		for(int i = 0 ; i < finish ; i++)
		{
			this.displayedCards[i].SetActive(true);
		}
		view.myGameVM.displayView=true ;
	}
	public IEnumerator sellCard(GameObject gameobject)
	{
		int tempInt = System.Convert.ToInt32(gameobject.name.Substring(4))+view.myGameCardsVM.start;
		yield return StartCoroutine (model.cards[view.myGameCardsVM.cardsToBeDisplayed[tempInt]].sellCard());
		this.refreshCredits();
		if(model.cards[view.myGameCardsVM.cardsToBeDisplayed[tempInt]].Error=="")
		{
			model.cards.RemoveAt(view.myGameCardsVM.cardsToBeDisplayed[tempInt]);
			this.resetAll();
		}
		else
		{
			this.cardFocused.GetComponent<CardMyGameController>().resetFocusedMyGameCard(model.cards[view.myGameCardsVM.cardsToBeDisplayed[tempInt]]);
			this.cardFocused.GetComponent<CardController>().setError();
			model.cards[view.myGameCardsVM.cardsToBeDisplayed[tempInt]].Error="";
		}
	}
	public IEnumerator buyXpCard(GameObject gameobject)
	{
		int tempInt = System.Convert.ToInt32(gameobject.name.Substring(4))+view.myGameCardsVM.start;
		int tempPrice = model.cards [view.myGameCardsVM.cardsToBeDisplayed [tempInt]].getPriceForNextLevel();
		yield return StartCoroutine(model.cards[view.myGameCardsVM.cardsToBeDisplayed[tempInt]].addXp(tempPrice,tempPrice));
		this.refreshCredits();
		if(model.cards[view.myGameCardsVM.cardsToBeDisplayed[tempInt]].Error=="")
		{
			this.setGUI (true);
			this.cardFocused.GetComponent<CardController>().animateExperience (model.cards[view.myGameCardsVM.cardsToBeDisplayed[tempInt]]);
		}
		else
		{
			this.cardFocused.GetComponent<CardMyGameController>().resetFocusedMyGameCard(model.cards[view.myGameCardsVM.cardsToBeDisplayed[tempInt]]);
			this.cardFocused.GetComponent<CardController>().setError();
			model.cards[view.myGameCardsVM.cardsToBeDisplayed[tempInt]].Error="";
		}
		this.displayedCards [tempInt - view.myGameCardsVM.start].GetComponent<CardMyGameController> ().resetMyGameCard (model.cards [view.myGameCardsVM.cardsToBeDisplayed [tempInt]]);
	}
	public IEnumerator renameCard(string value, GameObject gameobject)
	{
		int tempInt = System.Convert.ToInt32(gameobject.name.Substring(4))+view.myGameCardsVM.start;
		int tempPrice = model.cards [view.myGameCardsVM.cardsToBeDisplayed [tempInt]].RenameCost;
		yield return StartCoroutine(model.cards [view.myGameCardsVM.cardsToBeDisplayed [tempInt]].renameCard(value,tempPrice));
		this.updateScene (tempInt);
	}
	public IEnumerator putOnMarketCard(int price, GameObject gameobject)
	{
		int tempInt = System.Convert.ToInt32(gameobject.name.Substring(4))+view.myGameCardsVM.start;
		yield return StartCoroutine (model.cards [view.myGameCardsVM.cardsToBeDisplayed [tempInt]].toSell (price));
		this.updateScene (tempInt);
	}
	public IEnumerator editSellPriceCard(int price, GameObject gameobject)
	{
		int tempInt = System.Convert.ToInt32(gameobject.name.Substring(4))+view.myGameCardsVM.start;
		yield return StartCoroutine (model.cards [view.myGameCardsVM.cardsToBeDisplayed [tempInt]].changePriceCard (price));
		this.updateScene (tempInt);
	}
	public IEnumerator unsellCard(GameObject gameobject)
	{
		int tempInt = System.Convert.ToInt32(gameobject.name.Substring(4))+view.myGameCardsVM.start;
		yield return StartCoroutine (model.cards [view.myGameCardsVM.cardsToBeDisplayed [tempInt]].notToSell ());
		this.updateScene (tempInt);
	}
	public void updateScene(int tempInt)
	{
		this.displayedCards [tempInt - view.myGameCardsVM.start].GetComponent<CardMyGameController> ().resetMyGameCard (model.cards [view.myGameCardsVM.cardsToBeDisplayed [tempInt]]);
		this.cardFocused.GetComponent<CardMyGameController> ().resetFocusedMyGameCard (model.cards [view.myGameCardsVM.cardsToBeDisplayed [tempInt]]);
		this.refreshCredits();
		if(model.cards [view.myGameCardsVM.cardsToBeDisplayed [tempInt]].Error=="")
		{
			this.setGUI (true);
		}
		else
		{
			this.cardFocused.GetComponent<CardController>().setError();
			model.cards [view.myGameCardsVM.cardsToBeDisplayed [tempInt]].Error="";
		}
	}
	public void refreshCredits()
	{
		StartCoroutine(this.MenuObject.GetComponent<MenuController> ().getUserData ());
	}
	public void setGUI(bool value)
	{
		view.myGameVM.guiEnabled = value;
		if(this.cardFocused!=null)
		{
			this.cardFocused.GetComponent<CardController>().setMyGUI(value);
		}
		int finish = view.myGameCardsVM.nbCardsToDisplay;
		for(int i = 0 ; i < finish ; i++)
		{
			this.displayedCards[i].GetComponent<CardController>().setMyGUI(value);
		}
	}
	private void initMyGameCardsVM()
	{
		view.myGameCardsVM.nbCards=model.cards.Count;
		view.myGameCardsVM.cardsToBeDisplayed = new List<int> ();
		for (int i=0;i<model.cards.Count;i++)
		{
			view.myGameCardsVM.cardsToBeDisplayed.Add (i);
		}
	}
	private void initMyGameDecksVM()
	{
		view.myGameDecksVM.myDecks = new List<Deck> ();
		view.myGameDecksVM.myDecksGuiStyle=new GUIStyle[model.decks.Count];
		view.myGameDecksVM.myDecksButtonGuiStyle=new GUIStyle[model.decks.Count];
		for (int i=0;i<model.decks.Count;i++)
		{
			if(model.decks[i].Id==model.idSelectedDeck)
			{
				view.myGameDecksVM.myDecks.Insert(0,model.decks[i]);
			}
			else
			{
				view.myGameDecksVM.myDecks.Add (model.decks[i]);
			}
			view.myGameDecksVM.myDecksGuiStyle[i]=view.myGameDecksVM.deckStyle;
			view.myGameDecksVM.myDecksButtonGuiStyle[i]=view.myGameDecksVM.deckButtonStyle;
		}
		view.myGameDecksVM.myDecksGuiStyle[view.myGameDecksVM.chosenDeck]=view.myGameDecksVM.deckChosenStyle;
		view.myGameDecksVM.myDecksButtonGuiStyle[view.myGameDecksVM.chosenDeck]=view.myGameDecksVM.deckButtonChosenStyle;
	}
	private void initStyles()
	{
		view.myGameScreenVM.styles=new GUIStyle[this.myGameScreenVMStyle.Length];
		for(int i=0;i<this.myGameScreenVMStyle.Length;i++)
		{
			view.myGameScreenVM.styles[i]=this.myGameScreenVMStyle[i];
		}
		view.myGameScreenVM.initStyles();
		view.myGameVM.styles=new GUIStyle[this.myGameVMStyle.Length];
		for(int i=0;i<this.myGameVMStyle.Length;i++)
		{
			view.myGameVM.styles[i]=this.myGameVMStyle[i];
		}
		view.myGameVM.initStyles();
		view.myGameFiltersVM.styles=new GUIStyle[this.myGameFiltersVMStyle.Length];
		for(int i=0;i<this.myGameFiltersVMStyle.Length;i++)
		{
			view.myGameFiltersVM.styles[i]=this.myGameFiltersVMStyle[i];
		}
		view.myGameFiltersVM.initStyles();
		view.myGameCardsVM.styles=new GUIStyle[this.myGameCardsVMStyle.Length];
		for(int i=0;i<this.myGameCardsVMStyle.Length;i++)
		{
			view.myGameCardsVM.styles[i]=this.myGameCardsVMStyle[i];
		}
		view.myGameCardsVM.initStyles();
		view.myGameDecksVM.styles=new GUIStyle[this.myGameDecksVMStyle.Length];
		for(int i=0;i<this.myGameDecksVMStyle.Length;i++)
		{
			view.myGameDecksVM.styles[i]=this.myGameDecksVMStyle[i];
		}
		view.myGameDecksVM.initStyles();
	}
	private void resize()
	{
		view.myGameScreenVM.resize ();
		view.myGameFiltersVM.resize (view.myGameScreenVM.heightScreen);
		view.myGameVM.resize (view.myGameScreenVM.heightScreen);
		view.myGameCardsVM.resize (view.myGameScreenVM.heightScreen);
		view.myGameDecksVM.resize(view.myGameScreenVM.heightScreen);
	}
	public void returnPressed()
	{
		if(view.myGameVM.isPopUpDisplayed)
		{
			//this.cardPopUpBelongTo.GetComponent<CardController> ().confirmPopUp ();
		}
	}
	public void escapePressed()
	{
		if(view.myGameVM.isPopUpDisplayed)
		{
			//this.cardPopUpBelongTo.GetComponent<CardController> ().exitPopUp ();
		}
		else if(this.cardFocused!=null)
		{
			//this.exitCard();
		}
	}
	private void createCards()
	{
		float tempF = 10f*view.myGameScreenVM.widthScreen/view.myGameScreenVM.heightScreen;
		float width = 10f*0.85f*view.myGameScreenVM.blockCardsWidth/(view.myGameScreenVM.blockCardsHeight+view.myGameScreenVM.blockDecksHeight+view.myGameScreenVM.gapBetweenblocks);
		view.myGameCardsVM.nbCardsPerRow = Mathf.FloorToInt(width/1.6f);
		float debutLargeur = -0.49f * tempF+1f + (width - 1.6f * view.myGameCardsVM.nbCardsPerRow)/2 ;
		this.displayedCards = new GameObject[3*view.myGameCardsVM.nbCardsPerRow];
		view.myGameCardsVM.nbCardsToDisplay = 0;
		
		for(int i = 0 ; i < 3*view.myGameCardsVM.nbCardsPerRow ; i++)
		{
			this.displayedCards[i] = Instantiate(CardObject) as GameObject;
			this.displayedCards[i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
			this.displayedCards[i].transform.localPosition = new Vector3(debutLargeur + 1.6f * (i % view.myGameCardsVM.nbCardsPerRow), 0.8f - (i - i % view.myGameCardsVM.nbCardsPerRow) / view.myGameCardsVM.nbCardsPerRow * 2.2f,0);
			this.displayedCards[i].gameObject.name = "Card" + i + "";
			
			if (i<view.myGameCardsVM.cardsToBeDisplayed.Count)
			{
				this.displayedCards[i].AddComponent<CardMyGameController>();
				this.displayedCards[i].GetComponent<CardMyGameController>().setMyGameCard(model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]]);
				this.displayedCards[i].GetComponent<CardController> ().setCentralWindowRect (view.myGameScreenVM.centralWindow);
				view.myGameCardsVM.nbCardsToDisplay++;
			}   
			else{
				this.displayedCards[i].SetActive (false);
			}
		}
	}
	private void setPagination()
	{
		view.myGameCardsVM.nbPages = Mathf.CeilToInt((view.myGameCardsVM.cardsToBeDisplayed.Count-1) / (3*view.myGameCardsVM.nbCardsPerRow))+1;
		view.myGameCardsVM.pageDebut = 0 ;
		if (view.myGameCardsVM.nbPages>15)
		{
			view.myGameCardsVM.pageFin = 14 ;
		}
		else
		{
			view.myGameCardsVM.pageFin = view.myGameCardsVM.nbPages ;
		}
		view.myGameCardsVM.chosenPage = 0;
		view.myGameCardsVM.paginatorGuiStyle = new GUIStyle[view.myGameCardsVM.nbPages];
		for (int i = 0; i < view.myGameCardsVM.nbPages; i++) 
		{ 
			if (i==0)
			{
				view.myGameCardsVM.paginatorGuiStyle[i]=view.myGameVM.paginationActivatedStyle;
			}
			else
			{
				view.myGameCardsVM.paginatorGuiStyle[i]=view.myGameVM.paginationStyle;
			}
		}
	}
	private void clearCards()
	{
		if(this.cardFocused!=null)
		{
			Destroy (this.cardFocused);
		}
		for (int i = 0; i < 3*view.myGameCardsVM.nbCardsPerRow; i++) 
		{
			Destroy(this.displayedCards[i]);
		}
	}
	private void displayPage()
	{
		view.myGameCardsVM.start = 3 * view.myGameCardsVM.nbCardsPerRow * view.myGameCardsVM.chosenPage;
		view.myGameCardsVM.finish = view.myGameCardsVM.start + 3 * view.myGameCardsVM.nbCardsPerRow;
		view.myGameCardsVM.nbCardsToDisplay = 0;
		for(int i = view.myGameCardsVM.start ; i < view.myGameCardsVM.finish ; i++)
		{
			if (i<view.myGameCardsVM.cardsToBeDisplayed.Count)
			{
				this.displayedCards[i-view.myGameCardsVM.start].SetActive(true);
				this.displayedCards[i-view.myGameCardsVM.start].GetComponent<CardMyGameController>().resetMyGameCard(model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]]);
				view.myGameCardsVM.nbCardsToDisplay++;
			}
			else
			{
				displayedCards[i-view.myGameCardsVM.start].SetActive(false);
			}
		}
	}
	private void initializeSortButtons()
	{
		for (int i =0;i<8;i++)
		{
			view.myGameFiltersVM.sortButtonStyle[i]=view.myGameVM.sortDefaultButtonStyle;
		}
	}
	private void initializeToggles()
	{
		view.myGameFiltersVM.cardTypeList=new string[model.cardTypeList.Length];
		for(int i=0;i<model.cardTypeList.Length-1;i++)
		{
			view.myGameFiltersVM.cardTypeList[i] = model.cardTypeList[i];
		}
		view.myGameFiltersVM.togglesCurrentStates = new bool[model.cardTypeList.Length];
		for(int i = 0 ; i < model.cardTypeList.Length-1 ; i++)
		{
			view.myGameFiltersVM.togglesCurrentStates[i] = false;
		}
	}
	private void setFilters()
	{
		view.myGameFiltersVM.minLifeLimit=10000;
		view.myGameFiltersVM.maxLifeLimit=0;
		view.myGameFiltersVM.minAttackLimit=10000;
		view.myGameFiltersVM.maxAttackLimit=0;
		view.myGameFiltersVM.minMoveLimit=10000;
		view.myGameFiltersVM.maxMoveLimit=0;
		view.myGameFiltersVM.minQuicknessLimit=10000;
		view.myGameFiltersVM.maxQuicknessLimit=0;
		
		int max = model.cards.Count;
		for (int i = 0; i < max ; i++) {
			if (model.cards[i].Life<view.myGameFiltersVM.minLifeLimit){
				view.myGameFiltersVM.minLifeLimit = model.cards[i].Life;
			}
			if (model.cards[i].Life>view.myGameFiltersVM.maxLifeLimit){
				view.myGameFiltersVM.maxLifeLimit = model.cards[i].Life;
			}
			if (model.cards[i].Attack<view.myGameFiltersVM.minAttackLimit){
				view.myGameFiltersVM.minAttackLimit = model.cards[i].Attack;
			}
			if (model.cards[i].Attack>view.myGameFiltersVM.maxAttackLimit){
				view.myGameFiltersVM.maxAttackLimit = model.cards[i].Attack;
			}
			if (model.cards[i].Move<view.myGameFiltersVM.minMoveLimit){
				view.myGameFiltersVM.minMoveLimit = model.cards[i].Move;
			}
			if (model.cards[i].Move>view.myGameFiltersVM.maxMoveLimit){
				view.myGameFiltersVM.maxMoveLimit = model.cards[i].Move;
			}
			if (model.cards[i].Speed<view.myGameFiltersVM.minQuicknessLimit){
				view.myGameFiltersVM.minQuicknessLimit = model.cards[i].Speed;
			}
			if (model.cards[i].Speed>view.myGameFiltersVM.maxQuicknessLimit){
				view.myGameFiltersVM.maxQuicknessLimit = model.cards[i].Speed;
			}
		}
		view.myGameFiltersVM.minLifeVal = view.myGameFiltersVM.minLifeLimit;
		view.myGameFiltersVM.maxLifeVal = view.myGameFiltersVM.maxLifeLimit;
		view.myGameFiltersVM.oldMinLifeVal = view.myGameFiltersVM.minLifeLimit;
		view.myGameFiltersVM.oldMaxLifeVal = view.myGameFiltersVM.maxLifeLimit;
		view.myGameFiltersVM.minAttackVal = view.myGameFiltersVM.minAttackLimit;
		view.myGameFiltersVM.maxAttackVal = view.myGameFiltersVM.maxAttackLimit;
		view.myGameFiltersVM.oldMinAttackVal = view.myGameFiltersVM.minAttackLimit;
		view.myGameFiltersVM.oldMaxAttackVal = view.myGameFiltersVM.maxAttackLimit;
		view.myGameFiltersVM.minMoveVal = view.myGameFiltersVM.minMoveLimit;
		view.myGameFiltersVM.maxMoveVal = view.myGameFiltersVM.maxMoveLimit;
		view.myGameFiltersVM.oldMinMoveVal = view.myGameFiltersVM.minMoveLimit;
		view.myGameFiltersVM.oldMaxMoveVal = view.myGameFiltersVM.maxMoveLimit;
		view.myGameFiltersVM.minQuicknessVal = view.myGameFiltersVM.minQuicknessLimit;
		view.myGameFiltersVM.maxQuicknessVal = view.myGameFiltersVM.maxQuicknessLimit;
		view.myGameFiltersVM.oldMinQuicknessVal = view.myGameFiltersVM.minQuicknessLimit;
		view.myGameFiltersVM.oldMaxQuicknessVal = view.myGameFiltersVM.maxQuicknessLimit;
	}
	public void paginationBack()
	{
		view.myGameCardsVM.pageDebut = view.myGameCardsVM.pageDebut-15;
		view.myGameCardsVM.pageFin = view.myGameCardsVM.pageDebut+15;
	}
	public void paginationSelect(int chosenPage)
	{
		view.myGameCardsVM.paginatorGuiStyle[view.myGameCardsVM.chosenPage]=view.myGameVM.paginationStyle;
		view.myGameCardsVM.chosenPage=chosenPage;
		view.myGameCardsVM.paginatorGuiStyle[chosenPage]=this.view.myGameVM.paginationActivatedStyle;
		this.displayPage();
	}
	public void paginationNext()
	{
		view.myGameCardsVM.pageDebut = view.myGameCardsVM.pageDebut+15;
		view.myGameCardsVM.pageFin = Mathf.Min(view.myGameCardsVM.pageFin+15, view.myGameCardsVM.nbPages);
	}
	
	private void applyFilters() 
	{
		view.myGameCardsVM.cardsToBeDisplayed=new List<int>();
		IList<int> tempCardsToBeDisplayed = new List<int>();
		int nbFilters = view.myGameFiltersVM.filtersCardType.Count;
		int tempMinPrice=0;
		int tempMaxPrice=999999999;
		
		bool testFilters = false;
		bool testDeck = false;
		bool test ;
		
		bool minLifeBool = (view.myGameFiltersVM.minLifeLimit==view.myGameFiltersVM.minLifeVal);
		bool maxLifeBool = (view.myGameFiltersVM.maxLifeLimit==view.myGameFiltersVM.maxLifeVal);
		bool minMoveBool = (view.myGameFiltersVM.minMoveLimit==view.myGameFiltersVM.minMoveVal);
		bool maxMoveBool = (view.myGameFiltersVM.maxMoveLimit==view.myGameFiltersVM.maxMoveVal);
		bool minQuicknessBool = (view.myGameFiltersVM.minQuicknessLimit==view.myGameFiltersVM.minQuicknessVal);
		bool maxQuicknessBool = (view.myGameFiltersVM.maxQuicknessLimit==view.myGameFiltersVM.maxQuicknessVal);
		bool minAttackBool = (view.myGameFiltersVM.minAttackLimit==view.myGameFiltersVM.minAttackVal);
		bool maxAttackBool = (view.myGameFiltersVM.maxAttackLimit==view.myGameFiltersVM.maxAttackVal);
		
		
		if (view.myGameFiltersVM.isSkillChosen)
		{
			int max = model.cards.Count;
			if (nbFilters == 0)
			{
				max = model.cards.Count;
				if (view.myGameFiltersVM.onSale)
				{
					for (int i = 0; i < max; i++)
					{
						if (model.cards [i].hasSkill(view.myGameFiltersVM.valueSkill) && model.cards [i].onSale == 1)
						{
							testDeck = false;
							for (int j = 0; j < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; j++)
							{
								if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [j])
								{
									testDeck = true;
								}
							}
							if (!testDeck)
							{
								tempCardsToBeDisplayed.Add(i);
							}
						}
					}
				} else
				{
					for (int i = 0; i < max; i++)
					{
						if (model.cards [i].hasSkill(view.myGameFiltersVM.valueSkill))
						{
							testDeck = false;
							for (int j = 0; j < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; j++)
							{
								if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [j])
								{
									testDeck = true;
								}
							}
							if (!testDeck)
							{
								tempCardsToBeDisplayed.Add(i);
							}
						}
					}
				}
			} else
			{
				for (int i = 0; i < max; i++)
				{
					test = false;
					int j = 0;
					if (view.myGameFiltersVM.onSale)
					{
						while (!test && j != nbFilters)
						{
							if (model.cards [i].IdClass == view.myGameFiltersVM.filtersCardType [j])
							{
								test = true;
								if (model.cards [i].hasSkill(view.myGameFiltersVM.valueSkill) && model.cards [i].onSale == 1)
								{
									testDeck = false;
									for (int k = 0; k < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; k++)
									{
										if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [k])
										{
											testDeck = true; 
										}
									}
									if (!testDeck)
									{
										tempCardsToBeDisplayed.Add(i);
									}
								}
							}
							j++;
						}
					} else
					{
						while (!test && j != nbFilters)
						{
							if (model.cards [i].IdClass == view.myGameFiltersVM.filtersCardType [j])
							{
								test = true;
								if (model.cards [i].hasSkill(view.myGameFiltersVM.valueSkill))
								{
									testDeck = false;
									for (int k = 0; k < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; k++)
									{
										if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [k])
										{
											testDeck = true; 
										}
									}
									if (!testDeck)
									{
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
			int max = model.cards.Count;
			if (nbFilters == 0)
			{
				if (view.myGameFiltersVM.onSale)
				{
					for (int i = 0; i < max; i++)
					{
						if (model.cards [i].onSale == 1)
						{
							testDeck = false;
							for (int j = 0; j < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; j++)
							{
								if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [j])
								{
									testDeck = true;
								}
							}
							if (!testDeck)
							{
								tempCardsToBeDisplayed.Add(i);
							}
						}
					}
				} 
				else
				{
					for (int i = 0; i < max; i++)
					{
						testDeck = false;
						for (int j = 0; j < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; j++)
						{
							if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [j])
							{
								testDeck = true;
							}
						}
						if (!testDeck)
						{
							tempCardsToBeDisplayed.Add(i);
						}
					}
				}
			} 
			else
			{
				if (view.myGameFiltersVM.onSale)
				{
					for (int i = 0; i < max; i++)
					{
						test = false;
						int j = 0;
						while (!test && j != nbFilters)
						{
							if (model.cards [i].IdClass == view.myGameFiltersVM.filtersCardType [j])
							{
								if (model.cards [i].onSale == 1)
								{
									test = true;
									testDeck = false;
									for (int k = 0; k < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; k++)
									{
										if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [k])
										{
											testDeck = true; 
										}
									}
									if (!testDeck)
									{
										tempCardsToBeDisplayed.Add(i);
									}
								}
							}
							j++;
						}
					}
				} else
				{
					for (int i = 0; i < max; i++)
					{
						test = false;
						int j = 0;
						while (!test && j != nbFilters)
						{
							if (model.cards [i].IdClass == view.myGameFiltersVM.filtersCardType [j])
							{
								test = true;
								testDeck = false;
								for (int k = 0; k < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; k++)
								{
									if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [k])
									{
										testDeck = true;
									}
								}
								if (!testDeck)
								{
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
			view.myGameFiltersVM.minLifeLimit=10000;
			view.myGameFiltersVM.maxLifeLimit=0;
			view.myGameFiltersVM.minAttackLimit=10000;
			view.myGameFiltersVM.maxAttackLimit=0;
			view.myGameFiltersVM.minMoveLimit=10000;
			view.myGameFiltersVM.maxMoveLimit=0;
			view.myGameFiltersVM.minQuicknessLimit=10000;
			view.myGameFiltersVM.maxQuicknessLimit=0;
			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++){
				if (model.cards[tempCardsToBeDisplayed[i]].Life<view.myGameFiltersVM.minLifeLimit){
					view.myGameFiltersVM.minLifeLimit = model.cards[tempCardsToBeDisplayed[i]].Life;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Life>view.myGameFiltersVM.maxLifeLimit){
					view.myGameFiltersVM.maxLifeLimit = model.cards[tempCardsToBeDisplayed[i]].Life;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Attack<view.myGameFiltersVM.minAttackLimit){
					view.myGameFiltersVM.minAttackLimit = model.cards[tempCardsToBeDisplayed[i]].Attack;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Attack>view.myGameFiltersVM.maxAttackLimit){
					view.myGameFiltersVM.maxAttackLimit = model.cards[tempCardsToBeDisplayed[i]].Attack;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Move<view.myGameFiltersVM.minMoveLimit){
					view.myGameFiltersVM.minMoveLimit = model.cards[tempCardsToBeDisplayed[i]].Move;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Move>view.myGameFiltersVM.maxMoveLimit){
					view.myGameFiltersVM.maxMoveLimit = model.cards[tempCardsToBeDisplayed[i]].Move;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Speed<view.myGameFiltersVM.minQuicknessLimit){
					view.myGameFiltersVM.minQuicknessLimit = model.cards[tempCardsToBeDisplayed[i]].Speed;
				}
				if (model.cards[tempCardsToBeDisplayed[i]].Speed>view.myGameFiltersVM.maxQuicknessLimit){
					view.myGameFiltersVM.maxQuicknessLimit = model.cards[tempCardsToBeDisplayed[i]].Speed;
				}
			}
			if (minLifeBool && view.myGameFiltersVM.maxLifeVal>view.myGameFiltersVM.minLifeLimit){
				view.myGameFiltersVM.minLifeVal = view.myGameFiltersVM.minLifeLimit;
			}
			else{
				if (view.myGameFiltersVM.minLifeVal<view.myGameFiltersVM.minLifeLimit){
					view.myGameFiltersVM.minLifeLimit = view.myGameFiltersVM.minLifeVal;
				}
			}
			if (maxLifeBool && view.myGameFiltersVM.minLifeVal<view.myGameFiltersVM.maxLifeLimit){
				view.myGameFiltersVM.maxLifeVal = view.myGameFiltersVM.maxLifeLimit;
			}
			else{
				if (view.myGameFiltersVM.maxLifeVal>view.myGameFiltersVM.maxLifeLimit){
					view.myGameFiltersVM.maxLifeLimit = view.myGameFiltersVM.maxLifeVal;
				}
			}
			if (minAttackBool && view.myGameFiltersVM.maxAttackVal>view.myGameFiltersVM.minAttackLimit){
				view.myGameFiltersVM.minAttackVal = view.myGameFiltersVM.minAttackLimit;
			}
			else{
				if (view.myGameFiltersVM.minAttackVal<view.myGameFiltersVM.minAttackLimit){
					view.myGameFiltersVM.minAttackLimit = view.myGameFiltersVM.minAttackVal;
				}
			}
			if (maxAttackBool && view.myGameFiltersVM.minAttackVal<view.myGameFiltersVM.maxAttackLimit){
				view.myGameFiltersVM.maxAttackVal = view.myGameFiltersVM.maxAttackLimit;
			}
			else{
				if (view.myGameFiltersVM.maxAttackVal>view.myGameFiltersVM.maxAttackLimit){
					view.myGameFiltersVM.maxAttackLimit = view.myGameFiltersVM.maxAttackVal;
				}
			}
			if (minMoveBool && view.myGameFiltersVM.maxMoveVal>view.myGameFiltersVM.minMoveLimit){
				view.myGameFiltersVM.minMoveVal = view.myGameFiltersVM.minMoveLimit;
			}
			else{
				if (view.myGameFiltersVM.minMoveVal<view.myGameFiltersVM.minMoveLimit){
					view.myGameFiltersVM.minMoveLimit = view.myGameFiltersVM.minMoveVal;
				}
			}
			if (maxMoveBool && view.myGameFiltersVM.minMoveVal<view.myGameFiltersVM.maxMoveLimit){
				view.myGameFiltersVM.maxMoveVal = view.myGameFiltersVM.maxMoveLimit;
			}
			else{
				if (view.myGameFiltersVM.maxMoveVal>view.myGameFiltersVM.maxMoveLimit){
					view.myGameFiltersVM.maxMoveLimit = view.myGameFiltersVM.maxMoveVal;
				}
			}
			if (minQuicknessBool && view.myGameFiltersVM.maxQuicknessVal>view.myGameFiltersVM.minQuicknessLimit){
				view.myGameFiltersVM.minQuicknessVal = view.myGameFiltersVM.minQuicknessLimit;
			}
			else{
				if (view.myGameFiltersVM.minQuicknessVal<view.myGameFiltersVM.minQuicknessLimit){
					view.myGameFiltersVM.minQuicknessLimit = view.myGameFiltersVM.minQuicknessVal;
				}
			}
			if (maxQuicknessBool && view.myGameFiltersVM.minQuicknessVal<view.myGameFiltersVM.maxQuicknessLimit){
				view.myGameFiltersVM.maxQuicknessVal = view.myGameFiltersVM.maxQuicknessLimit;
			}
			else{
				if (view.myGameFiltersVM.maxQuicknessVal>view.myGameFiltersVM.maxQuicknessLimit){
					view.myGameFiltersVM.maxQuicknessLimit = view.myGameFiltersVM.maxQuicknessVal;
				}
			}
			
			view.myGameFiltersVM.oldMinLifeVal = view.myGameFiltersVM.minLifeVal ;
			view.myGameFiltersVM.oldMaxLifeVal = view.myGameFiltersVM.maxLifeVal ;
			view.myGameFiltersVM.oldMinQuicknessVal = view.myGameFiltersVM.minQuicknessVal ;
			view.myGameFiltersVM.oldMaxQuicknessVal = view.myGameFiltersVM.maxQuicknessVal ;
			view.myGameFiltersVM.oldMinMoveVal = view.myGameFiltersVM.minMoveVal ;
			view.myGameFiltersVM.oldMaxMoveVal = view.myGameFiltersVM.maxMoveVal ;
			view.myGameFiltersVM.oldMinAttackVal = view.myGameFiltersVM.minAttackVal ;
			view.myGameFiltersVM.oldMaxAttackVal = view.myGameFiltersVM.maxAttackVal ;
		}
		
		if (view.myGameFiltersVM.minLifeVal!=view.myGameFiltersVM.minLifeLimit){
			testFilters = true ;
		}
		else if (view.myGameFiltersVM.maxLifeVal!=view.myGameFiltersVM.maxLifeLimit){
			testFilters = true ;
		}
		else if (view.myGameFiltersVM.minAttackVal!=view.myGameFiltersVM.minAttackLimit){
			testFilters = true ;
		}
		else if (view.myGameFiltersVM.maxAttackVal!=view.myGameFiltersVM.maxAttackLimit){
			testFilters = true ;
		}
		else if (view.myGameFiltersVM.minMoveVal!=view.myGameFiltersVM.minMoveLimit){
			testFilters = true ;
		}
		else if (view.myGameFiltersVM.maxMoveVal!=view.myGameFiltersVM.maxMoveLimit){
			testFilters = true ;
		}
		else if (view.myGameFiltersVM.minQuicknessVal!=view.myGameFiltersVM.minQuicknessLimit){
			testFilters = true ;
		}
		else if (view.myGameFiltersVM.maxQuicknessVal!=view.myGameFiltersVM.maxQuicknessLimit){
			testFilters = true ;
		}
		
		if (testFilters == true){
			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++)
			{
				if (model.cards[tempCardsToBeDisplayed[i]].verifyC2(view.myGameFiltersVM.minLifeVal,
				                                                    view.myGameFiltersVM.maxLifeVal,
				                                                    view.myGameFiltersVM.minAttackVal,
				                                                    view.myGameFiltersVM.maxAttackVal,
				                                                    view.myGameFiltersVM.minMoveVal,
				                                                    view.myGameFiltersVM.maxMoveVal,
				                                                    view.myGameFiltersVM.minQuicknessVal,
				                                                    view.myGameFiltersVM.maxQuicknessVal,
				                                                    tempMinPrice,tempMaxPrice)){
					view.myGameCardsVM.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
				}
			}
		}
		else
		{
			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++)
			{
				view.myGameCardsVM.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
			}
		}
		view.myGameCardsVM.nbPages = Mathf.CeilToInt(view.myGameCardsVM.cardsToBeDisplayed.Count / (3.0f*view.myGameCardsVM.nbCardsPerRow));
		view.myGameCardsVM.pageDebut = 0 ;
		if (view.myGameCardsVM.nbPages>15){
			view.myGameCardsVM.pageFin = 14 ;
		}
		else
		{
			view.myGameCardsVM.pageFin = view.myGameCardsVM.nbPages ;
		}
		view.myGameCardsVM.chosenPage = 0;
		view.myGameCardsVM.paginatorGuiStyle = new GUIStyle[view.myGameCardsVM.nbPages];
		for (int i = 0; i < view.myGameCardsVM.nbPages; i++) { 
			if (i==0){
				view.myGameCardsVM.paginatorGuiStyle[i]=view.myGameVM.paginationActivatedStyle;
			}
			else{
				view.myGameCardsVM.paginatorGuiStyle[i]=view.myGameVM.paginationStyle;
			}
		}
	}
	public void filterCards()
	{
		this.applyFilters ();
		if (view.myGameFiltersVM.oldSortSelected!=10)
		{
			this.sortCards(view.myGameFiltersVM.oldSortSelected);
		}
		this.clearCards ();
		this.createCards ();
		this.setPagination ();
	}
	public void isBeingDragged()
	{
		view.myGameVM.isBeingDragged = true;
	}
	public void isNotBeingDragged()
	{
		if(view.myGameVM.isBeingDragged)
		{
			view.myGameVM.isBeingDragged = false;
			this.refreshMinMaxFilters();
		}
	}
	public void refreshMinMaxFilters()
	{
		bool isMoved = false ;
		view.myGameFiltersVM.maxLifeVal=Mathf.RoundToInt(view.myGameFiltersVM.maxLifeVal);
		view.myGameFiltersVM.minLifeVal=Mathf.RoundToInt(view.myGameFiltersVM.minLifeVal);
		view.myGameFiltersVM.maxAttackVal=Mathf.RoundToInt(view.myGameFiltersVM.maxAttackVal);
		view.myGameFiltersVM.minAttackVal=Mathf.RoundToInt(view.myGameFiltersVM.minAttackVal);
		view.myGameFiltersVM.maxMoveVal=Mathf.RoundToInt(view.myGameFiltersVM.maxMoveVal);
		view.myGameFiltersVM.minMoveVal=Mathf.RoundToInt(view.myGameFiltersVM.minMoveVal);
		view.myGameFiltersVM.maxQuicknessVal=Mathf.RoundToInt(view.myGameFiltersVM.maxQuicknessVal);
		view.myGameFiltersVM.minQuicknessVal=Mathf.RoundToInt(view.myGameFiltersVM.minQuicknessVal);
		
		if (view.myGameFiltersVM.oldMaxLifeVal != view.myGameFiltersVM.maxLifeVal){
			view.myGameFiltersVM.oldMaxLifeVal = view.myGameFiltersVM.maxLifeVal;
			isMoved = true ; 
		}
		if (view.myGameFiltersVM.oldMinLifeVal != view.myGameFiltersVM.minLifeVal){
			view.myGameFiltersVM.oldMinLifeVal = view.myGameFiltersVM.minLifeVal;
			isMoved = true ; 
		}
		if (view.myGameFiltersVM.oldMaxAttackVal != view.myGameFiltersVM.maxAttackVal){
			view.myGameFiltersVM.oldMaxAttackVal = view.myGameFiltersVM.maxAttackVal;
			isMoved = true ; 
		}
		if (view.myGameFiltersVM.oldMinAttackVal != view.myGameFiltersVM.minAttackVal){
			view.myGameFiltersVM.oldMinAttackVal = view.myGameFiltersVM.minAttackVal;
			isMoved = true ; 
		}
		if (view.myGameFiltersVM.oldMaxMoveVal != view.myGameFiltersVM.maxMoveVal){
			view.myGameFiltersVM.oldMaxMoveVal = view.myGameFiltersVM.maxMoveVal;
			isMoved = true ; 
		}
		if (view.myGameFiltersVM.oldMinMoveVal != view.myGameFiltersVM.minMoveVal){
			view.myGameFiltersVM.oldMinMoveVal = view.myGameFiltersVM.minMoveVal;
			isMoved = true ; 
		}
		if (view.myGameFiltersVM.oldMaxQuicknessVal != view.myGameFiltersVM.maxQuicknessVal){
			view.myGameFiltersVM.oldMaxQuicknessVal = view.myGameFiltersVM.maxQuicknessVal;
			isMoved = true ; 
		}
		if (view.myGameFiltersVM.oldMinQuicknessVal != view.myGameFiltersVM.minQuicknessVal){
			view.myGameFiltersVM.oldMinQuicknessVal = view.myGameFiltersVM.minQuicknessVal;
			isMoved = true ; 
		}
		if(isMoved){
			this.filterCards();
		}
	}
	public void selectCardType(bool value, int id)
	{
		view.myGameFiltersVM.togglesCurrentStates [id] = value;
		if (value)
		{
			view.myGameFiltersVM.filtersCardType.Add(id);
		}
		else
		{
			view.myGameFiltersVM.filtersCardType.Remove(id);
		}
		this.filterCards ();
	}
	public void selectOnSale(bool value)
	{
		view.myGameFiltersVM.onSale = value;
		this.filterCards ();
	}
	public void selectSkills(string value)
	{
		if (value.Length > 0) 
		{
			view.myGameFiltersVM.isSkillToDisplay=true;
			view.myGameFiltersVM.valueSkill = value.ToLower ();
			view.myGameFiltersVM.matchValues = new List<string> ();	
			if (view.myGameFiltersVM.valueSkill != "") 
			{
				view.myGameFiltersVM.matchValues = new List<string> ();
				for (int i = 0; i < model.skillsList.Count; i++) 
				{  
					if (model.skillsList [i].ToLower ().Contains (view.myGameFiltersVM.valueSkill)) 
					{
						view.myGameFiltersVM.matchValues.Add (model.skillsList [i]);
					}
				}
			}
		} 
		else 
		{
			view.myGameFiltersVM.isSkillToDisplay=false;
			view.myGameFiltersVM.valueSkill = "";
		}
		if (view.myGameFiltersVM.isSkillChosen)
		{
			view.myGameFiltersVM.isSkillChosen=false ;
			this.filterCards() ;
		}
	}
	public void filterASkill(string value)
	{
		view.myGameFiltersVM.valueSkill = value.ToLower ();
		view.myGameFiltersVM.isSkillChosen=true ;
		view.myGameFiltersVM.matchValues = new List<string>();
		this.filterCards ();
	}
	public void sortCards(int id)
	{
		this.applySorts (id);
		this.loadData ();
	}
	public void applySorts(int id)
	{
		int tempA=new int();
		int tempB=new int();
		
		if(view.myGameFiltersVM.oldSortSelected!=10)
		{
			view.myGameFiltersVM.sortButtonStyle[view.myGameFiltersVM.oldSortSelected]=view.myGameVM.sortDefaultButtonStyle;
		}
		view.myGameFiltersVM.sortButtonStyle[id]=view.myGameVM.sortActivatedButtonStyle;
		view.myGameFiltersVM.oldSortSelected=id;
		
		for (int i = 1; i<view.myGameCardsVM.cardsToBeDisplayed.Count; i++) {
			
			for (int j=0;j<i;j++){
				
				
				switch (id)
				{
				case 0:
					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Life;
					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Life;
					break;
				case 1:
					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Life;
					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Life;
					break;
				case 2:
					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Attack;
					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Attack;
					break;
				case 3:
					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Attack;
					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Attack;
					break;
				case 4:
					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Move;
					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Move;
					break;
				case 5:
					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Move;
					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Move;
					break;
				case 6:
					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Speed;
					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Speed;
					break;
				case 7:
					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Speed;
					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Speed;
					break;
				default:
					break;
				}
				
				if (tempA<tempB){
					view.myGameCardsVM.cardsToBeDisplayed.Insert (j,view.myGameCardsVM.cardsToBeDisplayed[i]);
					view.myGameCardsVM.cardsToBeDisplayed.RemoveAt(i+1);
					break;
				}
				
			}
		}
	}
}

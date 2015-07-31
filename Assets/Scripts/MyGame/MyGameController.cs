//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;
//
//public class MyGameController : MonoBehaviour
//{
//	public static MyGameController instance;
//	private MyGameModel model;
//	public GameObject MenuObject;
//	public GameObject CardObject;
//	public GameObject TutorialObject;
//	public int refreshInterval;
//	public GUIStyle[] myGameScreenVMStyle;
//	public GUIStyle[] myGameVMStyle;
//	public GUIStyle[] myGameFiltersVMStyle;
//	public GUIStyle[] myGameCardsVMStyle;
//	public GUIStyle[] myGameDecksVMStyle;
//	public GUIStyle[] popUpVMStyle;
//	public GUIStyle[] myGameDeckCardsVMStyle;
//	private MyGameView view;
//	private NewMyGameErrorPopUpView errorPopUpView;
//	private MyGameNewDeckPopUpView newDeckPopUpView;
//	private MyGameEditDeckPopUpView editDeckPopUpView;
//	private MyGameDeleteDeckPopUpView deleteDeckPopUpView;
//	private GameObject[] displayedCards;
//	private GameObject[] displayedDeckCards;
//	private GameObject cardFocused;
//	private GameObject cardPopUpBelongTo;
//	private GameObject tutorial;
//	private float timer;
//	private bool isTutorialLaunched;
//	private bool isFocus;
//
//	void Start()
//	{
//		instance = this;
//		this.view = Camera.main.gameObject.AddComponent <MyGameView>();
//		this.model = new MyGameModel ();
//		this.MenuObject = Instantiate(this.MenuObject) as GameObject;
//		Debug.Log ("toto");
//		StartCoroutine (this.initialization ());
//	}
//	void Update()
//	{
//		this.timer += Time.deltaTime;
//		
//		if (this.timer > this.refreshInterval) 
//		{	
//			this.timer=this.timer-this.refreshInterval;
//			StartCoroutine(this.refreshMyGame());
//		}
//	}
//	public IEnumerator initialization()
//	{
//		yield return StartCoroutine (model.initializeMyGame ());
//		this.initStyles ();
//		this.initMyGameCardsVM ();
//		this.initMyGameDecksVM ();
//		this.initMyGameDeckCardsVM ();
//		this.resize ();
//		this.createCards ();
//		this.createDeckCards ();
//		this.setPagination ();
//		this.initializeSortButtons ();
//		this.initializeToggles ();
//		this.setFilters ();
//		if(ApplicationModel.skillChosen!="")
//		{
//			this.filterASkill(ApplicationModel.skillChosen);
//			ApplicationModel.skillChosen="";
//		}
//		else if(ApplicationModel.cardTypeChosen!=-1)
//		{
//			this.selectCardType(true,ApplicationModel.cardTypeChosen);
//			ApplicationModel.cardTypeChosen=-1;
//		}
//		else
//		{
//			this.filterCards ();
//		}
//		if(model.player.TutorialStep==2)
//		{
//			this.tutorial = Instantiate(this.TutorialObject) as GameObject;
//			MenuObject.GetComponent<MenuController>().setTutorialLaunched(true);
//			this.tutorial.AddComponent<MyGameTutorialController>();
//			this.tutorial.GetComponent<MyGameTutorialController>().launchSequence(0);
//			this.isTutorialLaunched=true;
//		}
//	}
//	public void loadAll()
//	{
//		this.resize ();
//		this.loadCards ();
//		this.loadDeckCards ();
//		if(this.cardFocused!=null)
//		{
//			this.resizeFocus();
//		}
//		if(isTutorialLaunched)
//		{
//			tutorial.GetComponent<MyGameTutorialController>().resize();
//		}
//	}
//	public void loadDeckCards()
//	{
//		this.clearDeckCards ();
//		this.createDeckCards ();
//	}
//	public void loadCards()
//	{
//		this.clearCards ();
//		this.createCards ();
//		this.setPagination ();
//	}
//	public void resetAll()
//	{
//		this.initMyGameCardsVM ();
//		this.initMyGameDecksVM ();
//		this.initMyGameDeckCardsVM ();
//		this.clearFocus ();
//		this.loadCards ();
//		this.loadDeckCards ();
//		this.initializeSortButtons ();
//		this.initializeToggles ();
//		this.setFilters ();
//		this.filterCards ();
//		this.setGUI (true);
//		view.myGameVM.displayView = true;
//	}
//	private IEnumerator refreshMyGame()
//	{
//		yield return StartCoroutine(model.refreshMyGame ());
//		int index;
//		if(cardFocused!=null)
//		{
//			if(model.cardsSold.Contains(this.cardFocused.GetComponent<CardController>().card.Id))
//			{
//				index = retrieveCardIndex (this.cardFocused.name);
//				this.cardFocused.GetComponent<CardMyGameController>().resetFocusedMyGameCard(model.cards[index]);
//			}
//		}
//		int finish = view.myGameCardsVM.nbCardsToDisplay;
//		for(int i = 0 ; i < finish ; i++)
//		{
//			if(model.cardsSold.Contains(this.displayedCards[i].GetComponent<CardController>().card.Id))
//			{
//				index = retrieveCardIndex (this.displayedCards[i].name);
//				this.displayedCards[i].GetComponent<CardMyGameController>().resetMyGameCard(model.cards[index]);
//			}
//		}
//	}
//	public void leftClickedCard(GameObject gameobject)
//	{
//		string name = gameobject.name;
//
//		if(!view.myGameVM.isPopUpDisplayed && 
//		   this.errorPopUpView==null && 
//		   this.newDeckPopUpView==null && 
//		   this.deleteDeckPopUpView==null && 
//		   this.editDeckPopUpView==null &&
//		   !this.isTutorialLaunched)
//		{
//			if(model.decks.Count<1)
//			{
//				this.displayErrorPopUp("Vous devez créer un deck avant de sélectionner une carte");
//			}
//			else
//			{
//				StartCoroutine(this.moveCards(name));
//			}
//		}
//		else if(isTutorialLaunched)
//		{
//			if(this.tutorial.GetComponent<MyGameTutorialController>().getSequenceID()==12 && name=="Card0")
//			{
//				StartCoroutine(this.moveCards(name));
//				this.tutorial.GetComponent<MyGameTutorialController>().actionIsDone();
//			}
//			else if(this.tutorial.GetComponent<MyGameTutorialController>().getSequenceID()==13 && name=="Card2")
//			{
//				StartCoroutine(this.moveCards(name));
//				this.tutorial.GetComponent<MyGameTutorialController>().actionIsDone();
//			}
//			else if(this.tutorial.GetComponent<MyGameTutorialController>().getSequenceID()==14 && name=="Card1")
//			{
//				StartCoroutine(this.moveCards(name));
//				this.tutorial.GetComponent<MyGameTutorialController>().actionIsDone();
//			}
//			else if(this.tutorial.GetComponent<MyGameTutorialController>().getSequenceID()==15 && name=="Card0")
//			{
//				StartCoroutine(this.moveCards(name));
//				this.tutorial.GetComponent<MyGameTutorialController>().actionIsDone();
//			}
//		}
//	}
//	public IEnumerator moveCards(string name)
//	{
//		if(name.StartsWith("Card")&&view.myGameDeckCardsVM.deckCardsOrder.Count<ApplicationModel.nbCardsByDeck)
//		{
//			int cardIndex = retrieveCardIndex (name);
//			if(model.cards[cardIndex].onSale==0 && model.cards[cardIndex].IdOWner!=-1)
//			{
//				int deckIndex = view.myGameDecksVM.decksToBeDisplayed[view.myGameDecksVM.chosenDeck];
//				int deckOrder=0;
//				while(view.myGameDeckCardsVM.deckCardsOrder.Contains(deckOrder))
//				{
//					deckOrder++;
//				}
//				this.displayedDeckCards[deckOrder].SetActive(true);
//				this.displayedDeckCards[deckOrder].GetComponent<CardMyGameController>().resetMyGameCard(model.cards[cardIndex]);
//				this.displayedDeckCards[deckOrder].GetComponent<CardController>().setDeckOrderFeatures(deckOrder);
//				view.myGameDeckCardsVM.deckCardsToBeDisplayed.Add (cardIndex);
//				view.myGameDeckCardsVM.deckCardsOrder.Add (deckOrder);
//				view.myGameDecksVM.decksNbCards[view.myGameDecksVM.chosenDeck]++;
//				model.cards[cardIndex].Decks.Add (model.decks[deckIndex].Id);
//				this.filterCards();
//				yield return StartCoroutine(model.decks[deckIndex].addCard(model.cards[cardIndex].Id,deckOrder));
//			}
//			else if(model.cards[cardIndex].onSale==1)
//			{
//				this.displayErrorPopUp("Vous ne pouvez pas ajouter à votre deck une carte qui est en vente");
//				yield break;
//			}
//			else if(model.cards[cardIndex].IdOWner==-1)
//			{
//				this.displayErrorPopUp("Cette carte a été vendue, vous ne pouvez plus l'ajouter");
//				yield break;
//			}
//		}
//		else if(name.StartsWith("DCrd"))
//		{
//			int cardIndex = retrieveCardIndex (name);
//			int deckIndex = view.myGameDecksVM.decksToBeDisplayed[view.myGameDecksVM.chosenDeck];
//			model.cards[cardIndex].Decks.Remove(model.decks[deckIndex].Id);
//			view.myGameDecksVM.decksNbCards[view.myGameDecksVM.chosenDeck]--;
//			int deckOrder = view.myGameDeckCardsVM.deckCardsOrder.IndexOf(System.Convert.ToInt32(name.Substring(4)));
//			view.myGameDeckCardsVM.deckCardsToBeDisplayed.RemoveAt(deckOrder);
//			view.myGameDeckCardsVM.deckCardsOrder.RemoveAt(deckOrder);
//			this.displayedDeckCards[System.Convert.ToInt32(name.Substring(4))].SetActive(false);
//			this.filterCards();
//			yield return StartCoroutine(model.decks[deckIndex].removeCard(model.cards[cardIndex].Id));
//		}
//		yield break;
//	}
//	public IEnumerator changeDeckOrder(GameObject gameobject, bool moveLeft)
//	{
//		string name = gameobject.name;
//		int deckIndex = view.myGameDecksVM.decksToBeDisplayed[view.myGameDecksVM.chosenDeck];
//		int deckOrder2=System.Convert.ToInt32 (name.Substring (4));
//		int deckOrder1;
//		if(moveLeft)
//		{
//			deckOrder1 = System.Convert.ToInt32 (name.Substring (4))-1;
//		}
//		else
//		{
//			deckOrder1 = System.Convert.ToInt32 (name.Substring (4))+1;
//		}
//		int indexVMClickedCard = view.myGameDeckCardsVM.deckCardsOrder.IndexOf(deckOrder2);
//		int indexVMCardToMove = view.myGameDeckCardsVM.deckCardsOrder.IndexOf(deckOrder1);
//
//		view.myGameDeckCardsVM.deckCardsOrder [indexVMClickedCard] =deckOrder1;
//		this.displayedDeckCards[deckOrder1].GetComponent<CardMyGameController>().resetMyGameCard(model.cards[view.myGameDeckCardsVM.deckCardsToBeDisplayed[indexVMClickedCard]]);
//		this.displayedDeckCards[deckOrder1].GetComponent<CardController>().setDeckOrderFeatures(deckOrder1);
//		int idCard1 = model.cards [view.myGameDeckCardsVM.deckCardsToBeDisplayed [indexVMClickedCard]].Id;
//
//		int idCard2 = -1;
//		if(indexVMCardToMove==-1)
//		{
//			this.displayedDeckCards[deckOrder2].SetActive(false);
//			this.displayedDeckCards[deckOrder1].SetActive(true);
//		}
//		else
//		{
//			view.myGameDeckCardsVM.deckCardsOrder [indexVMCardToMove] = deckOrder2;
//			this.displayedDeckCards[deckOrder2].GetComponent<CardMyGameController>().resetMyGameCard(model.cards[view.myGameDeckCardsVM.deckCardsToBeDisplayed[indexVMCardToMove]]);
//			this.displayedDeckCards[deckOrder2].GetComponent<CardController>().setDeckOrderFeatures(deckOrder2);
//			idCard2 = model.cards[view.myGameDeckCardsVM.deckCardsToBeDisplayed[indexVMCardToMove]].Id;
//		}
//		yield return StartCoroutine(model.decks[deckIndex].changeCardsOrder(idCard1,deckOrder1,idCard2,deckOrder2));
//	}
//	public void rightClickedCard(GameObject gameobject)
//	{
//		string name = gameobject.name;
//		if(!view.myGameVM.isPopUpDisplayed && 
//		   this.errorPopUpView==null && 
//		   this.newDeckPopUpView==null && 
//		   this.deleteDeckPopUpView==null && 
//		   this.editDeckPopUpView==null && 
//		   !isTutorialLaunched && !this.isFocus)
//		{
//			this.focus (name);
//		}
//		else if(isTutorialLaunched && !this.isFocus)
//		{
//			if(name.Substring(4)=="3" && this.tutorial.GetComponent<MyGameTutorialController>().getSequenceID()==1)
//			{
//				this.focus (name);
//				this.cardFocused.GetComponent<CardMyGameController>().setIsTutorialLaunched(true);
//				this.tutorial.GetComponent<MyGameTutorialController>().actionIsDone();
//			}
//		}
//	}
//	public void focus(string name)
//	{
//		this.isFocus = true;
//		Card tempCard = new Card ();
//		int index = System.Convert.ToInt32 (name.Substring (4));
//		if(name.StartsWith("Card"))
//		{
//			tempCard=this.displayedCards[index].GetComponent<CardController> ().card;
//			name = "Fcrd"+index.ToString();
//		}
//		else
//		{
//			tempCard=this.displayedDeckCards[index].GetComponent<CardController> ().card;
//			name = "Fdcd"+index.ToString();
//		}
//		view.myGameVM.displayView=false ;
//		int finish = 3 * view.myGameCardsVM.nbCardsPerRow;
//		for(int i = 0 ; i < finish ; i++)
//		{
//			this.displayedCards[i].SetActive(false);
//		}
//		for(int i = 0 ; i < ApplicationModel.nbCardsByDeck ; i++)
//		{
//			this.displayedDeckCards[i].SetActive(false);
//		}
//		if(this.cardFocused==null)
//		{
//			Vector3 scale = new Vector3(view.myGameScreenVM.heightScreen / 120f,view.myGameScreenVM.heightScreen / 120f,view.myGameScreenVM.heightScreen / 120f);
//			Vector3 position = Camera.main.ScreenToWorldPoint (new Vector3 (0.4f * view.myGameScreenVM.widthScreen, 0.45f * view.myGameScreenVM.heightScreen - 1, 10));
//			this.cardFocused = Instantiate(CardObject) as GameObject;
//			this.cardFocused.AddComponent<CardMyGameController> ();
//			this.cardFocused.GetComponent<CardController> ().setGameObjectScaleAndPosition(scale,position);
//			this.cardFocused.GetComponent<CardController> ().setCentralWindowRect (view.myGameScreenVM.centralWindow);
//			this.cardFocused.GetComponent<CardController>().setCollectionPointsWindowRect(view.myGameScreenVM.collectionPointsWindow);
//			this.cardFocused.GetComponent<CardController>().setNewSkillsWindowRect(view.myGameScreenVM.newSkillsWindow);
//			this.cardFocused.GetComponent<CardController>().setNewCardTypeWindowRect(view.myGameScreenVM.centralWindow);
//		}
//		this.cardFocused.GetComponent<CardController> ().setGameObjectName (name);
//		this.cardFocused.GetComponent<CardMyGameController> ().resetFocusedMyGameCard (tempCard);
//		this.cardFocused.SetActive (true);
//	}
//	public void resizeFocus()
//	{
//		Vector3 scale = new Vector3(view.myGameScreenVM.heightScreen / 120f,view.myGameScreenVM.heightScreen / 120f,view.myGameScreenVM.heightScreen / 120f);
//		Vector3 position = Camera.main.ScreenToWorldPoint (new Vector3 (0.4f * view.myGameScreenVM.widthScreen, 0.45f * view.myGameScreenVM.heightScreen - 1, 10));
//		this.cardFocused.GetComponent<CardController> ().setGameObjectScaleAndPosition(scale,position);
//		this.cardFocused.GetComponent<CardController> ().setCentralWindowRect (view.myGameScreenVM.centralWindow);
//		this.cardFocused.GetComponent<CardController>().setCollectionPointsWindowRect(view.myGameScreenVM.collectionPointsWindow);
//		this.cardFocused.GetComponent<CardController>().setNewSkillsWindowRect(view.myGameScreenVM.newSkillsWindow);
//		this.cardFocused.GetComponent<CardController> ().setNewCardTypeWindowRect (view.myGameScreenVM.centralWindow);
//		this.cardFocused.GetComponent<CardController> ().resize ();
//	}
//	public void displayErrorPopUp(string error)
//	{
//		this.setGUI (false);
//		this.errorPopUpView = Camera.main.gameObject.AddComponent <NewMyGameErrorPopUpView>();
//		errorPopUpView.errorPopUpVM.error = error;
//		//errorPopUpView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
//		for(int i=0;i<this.popUpVMStyle.Length;i++)
//		{
//			//errorPopUpView.popUpVM.styles[i]=this.popUpVMStyle[i];
//		}
//		//errorPopUpView.popUpVM.initStyles();
//		this.errorPopUpResize ();
//	}
//	public void displayNewDeckPopUp()
//	{
//		this.setGUI (false);
//		this.newDeckPopUpView = Camera.main.gameObject.AddComponent <MyGameNewDeckPopUpView>();
//		newDeckPopUpView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
//		for(int i=0;i<this.popUpVMStyle.Length;i++)
//		{
//			newDeckPopUpView.popUpVM.styles[i]=this.popUpVMStyle[i];
//		}
//		newDeckPopUpView.popUpVM.initStyles();
//		this.newDeckPopUpResize ();
//	}
//	public void displayEditDeckPopUp(int chosenDeck)
//	{
//		this.setGUI (false);
//		this.editDeckPopUpView = Camera.main.gameObject.AddComponent <MyGameEditDeckPopUpView>();
//		editDeckPopUpView.editDeckPopUpVM.oldName = view.myGameDecksVM.decksName [chosenDeck];
//		editDeckPopUpView.editDeckPopUpVM.newName = view.myGameDecksVM.decksName [chosenDeck];
//		editDeckPopUpView.editDeckPopUpVM.chosenDeck=chosenDeck;
//		editDeckPopUpView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
//		for(int i=0;i<this.popUpVMStyle.Length;i++)
//		{
//			editDeckPopUpView.popUpVM.styles[i]=this.popUpVMStyle[i];
//		}
//		editDeckPopUpView.popUpVM.initStyles();
//		this.editDeckPopUpResize ();
//	}
//	public void displayDeleteDeckPopUp(int chosenDeck)
//	{
//		this.setGUI (false);
//		this.deleteDeckPopUpView = Camera.main.gameObject.AddComponent <MyGameDeleteDeckPopUpView>();
//		deleteDeckPopUpView.deleteDeckPopUpVM.name = view.myGameDecksVM.decksName [chosenDeck];
//		deleteDeckPopUpView.deleteDeckPopUpVM.chosenDeck=chosenDeck;
//		deleteDeckPopUpView.popUpVM.styles=new GUIStyle[this.popUpVMStyle.Length];
//		for(int i=0;i<this.popUpVMStyle.Length;i++)
//		{
//			deleteDeckPopUpView.popUpVM.styles[i]=this.popUpVMStyle[i];
//		}
//		deleteDeckPopUpView.popUpVM.initStyles();
//		this.deleteDeckPopUpResize ();
//	}
//	public void hideNewDeckPopUp()
//	{
//		this.setGUI (true);
//		Destroy (this.newDeckPopUpView);
//	}
//	public void hideEditDeckPopUp()
//	{
//		this.setGUI (true);
//		Destroy (this.editDeckPopUpView);
//	}
//	public void hideDeleteDeckPopUp()
//	{
//		this.setGUI (true);
//		Destroy (this.deleteDeckPopUpView);
//	}
//	public void hideErrorPopUp()
//	{
//		this.setGUI (true);
//		Destroy (this.errorPopUpView);
//	}
//	public void errorPopUpResize()
//	{
//		errorPopUpView.popUpVM.centralWindow = view.myGameScreenVM.centralWindow;
//		errorPopUpView.popUpVM.resize ();
//	}
//	public void newDeckPopUpResize()
//	{
//		newDeckPopUpView.popUpVM.centralWindow = view.myGameScreenVM.centralWindow;
//		newDeckPopUpView.popUpVM.resize ();
//	}
//	public void editDeckPopUpResize()
//	{
//		editDeckPopUpView.popUpVM.centralWindow = view.myGameScreenVM.centralWindow;
//		editDeckPopUpView.popUpVM.resize ();
//	}
//	public void deleteDeckPopUpResize()
//	{
//		deleteDeckPopUpView.popUpVM.centralWindow = view.myGameScreenVM.centralWindow;
//		deleteDeckPopUpView.popUpVM.resize ();
//	}
//	public IEnumerator createNewDeck()
//	{
//		newDeckPopUpView.newDeckPopUpVM.error=this.checkDeckName(newDeckPopUpView.newDeckPopUpVM.name);
//		if(newDeckPopUpView.newDeckPopUpVM.error=="")
//		{
//			model.decks.Add(new Deck());
//			yield return StartCoroutine(model.decks[model.decks.Count-1].create(newDeckPopUpView.newDeckPopUpVM.name));
//			view.myGameDecksVM.decksToBeDisplayed.Add (model.decks.Count-1);
//			view.myGameDecksVM.decksName.Add (newDeckPopUpView.newDeckPopUpVM.name);
//			view.myGameDecksVM.decksNbCards.Add (0);
//			view.myGameDecksVM.myDecksButtonGuiStyle.Add(new GUIStyle());
//			view.myGameDecksVM.myDecksGuiStyle.Add(new GUIStyle());
//			this.displayDeck(model.decks.Count-1);
//			this.hideNewDeckPopUp();
//			if(this.isTutorialLaunched)
//			{
//				this.tutorial.GetComponent<MyGameTutorialController>().actionIsDone();
//			}
//		}
//	}
//	public string checkDeckName(string name)
//	{
//		if(!Regex.IsMatch(name, @"^[a-zA-Z0-9_\s]+$"))
//		{
//			return "Vous ne pouvez pas utiliser de caractères spéciaux";
//		}
//		for(int i=0;i<model.decks.Count;i++)
//		{
//			if(model.decks[i].Name==name)
//			{
//				return "Nom déjà utilisé";
//			}
//		}
//		if(name=="")
//		{
//			return "Veuillez saisir un nom";
//		}
//		return "";
//	}
//	public IEnumerator editDeck()
//	{
//		if(editDeckPopUpView.editDeckPopUpVM.newName!=editDeckPopUpView.editDeckPopUpVM.oldName)
//		{
//			editDeckPopUpView.editDeckPopUpVM.error=checkDeckName(editDeckPopUpView.editDeckPopUpVM.newName);
//			if(editDeckPopUpView.editDeckPopUpVM.error=="")
//			{
//				yield return StartCoroutine(model.decks[view.myGameDecksVM.decksToBeDisplayed[editDeckPopUpView.editDeckPopUpVM.chosenDeck]].edit(editDeckPopUpView.editDeckPopUpVM.newName));
//				view.myGameDecksVM.decksName[editDeckPopUpView.editDeckPopUpVM.chosenDeck]=editDeckPopUpView.editDeckPopUpVM.newName;
//				this.hideEditDeckPopUp();
//			}
//		}
//		else
//		{
//			this.hideEditDeckPopUp();
//		}
//	}
//	public IEnumerator deleteDeck()
//	{
//		int deckId = model.decks [view.myGameDecksVM.decksToBeDisplayed [deleteDeckPopUpView.deleteDeckPopUpVM.chosenDeck]].Id;
//		yield return StartCoroutine(model.decks[view.myGameDecksVM.decksToBeDisplayed[deleteDeckPopUpView.deleteDeckPopUpVM.chosenDeck]].delete());
//		model.decks.RemoveAt (view.myGameDecksVM.decksToBeDisplayed [deleteDeckPopUpView.deleteDeckPopUpVM.chosenDeck]);
//		this.removeDeckFromAllCards (deckId);
//		this.hideDeleteDeckPopUp();
//		if(view.myGameDecksVM.chosenDeck==deleteDeckPopUpView.deleteDeckPopUpVM.chosenDeck)
//		{
//			view.myGameDecksVM.chosenDeck=0;
//			this.resetAll();
//		}
//		else
//		{
//			if(deleteDeckPopUpView.deleteDeckPopUpVM.chosenDeck<view.myGameDecksVM.chosenDeck)
//			{
//				view.myGameDecksVM.chosenDeck--;
//			}
//			this.initMyGameDecksVM ();
//			this.initMyGameDeckCardsVM ();
//			this.loadDeckCards ();
//		}
//		view.myGameDeckCardsVM.labelNoDecks=computeLabelNoDeck();
//	}
//	public void exitCard()
//	{
//		this.clearFocus ();
//		int finish = view.myGameCardsVM.nbCardsToDisplay;
//		for(int i = 0 ; i < finish ; i++)
//		{
//			this.displayedCards[i].SetActive(true);
//		}
//		for(int i = 0 ; i < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count ; i++)
//		{
//			this.displayedDeckCards[view.myGameDeckCardsVM.deckCardsOrder[i]].SetActive(true);
//		}
//		view.myGameVM.displayView=true ;
//	}
//	public int retrieveCardIndex(string name)
//	{
//		if(name.StartsWith("Card")||name.StartsWith("Fcrd"))
//		{
//			return view.myGameCardsVM.cardsToBeDisplayed[System.Convert.ToInt32(name.Substring(4))+view.myGameCardsVM.start];
//		}
//		else
//		{
//			return view.myGameDeckCardsVM.deckCardsToBeDisplayed[view.myGameDeckCardsVM.deckCardsOrder.IndexOf(System.Convert.ToInt32(name.Substring(4)))];
//		}
//	}
//	public IEnumerator sellCard(GameObject gameobject)
//	{
//		int index = retrieveCardIndex (gameobject.name);
//		yield return StartCoroutine (model.cards[index].sellCard());
//		this.refreshCredits();
//		if(model.cards[index].Error=="")
//		{
//			this.removeCardFromAllDecks(model.cards[index].Id);
//			model.cards.RemoveAt(index);
//			this.resetAll();
//		}
//		else
//		{
//			this.cardFocused.GetComponent<CardMyGameController>().resetFocusedMyGameCard(model.cards[index]);
//			this.cardFocused.GetComponent<CardController>().setError();
//			model.cards[index].Error="";
//		}
//	}
//	public IEnumerator buyXpCard(GameObject gameobject)
//	{
//		int index = retrieveCardIndex (gameobject.name);
//		yield return StartCoroutine(model.cards[index].addXpLevel());
//		this.refreshCredits();
//		if(model.cards[index].Error=="")
//		{
//			this.setGUI (true);
//			this.cardFocused.GetComponent<CardController>().animateExperience (model.cards[index]);
//			if(model.cards[index].CollectionPoints>0)
//			{
//				StartCoroutine(this.cardFocused.GetComponent<CardController>().displayCollectionPointsPopUp());
//			}
//			if(model.cards[index].NewSkills.Count>0)
//			{
//				StartCoroutine(this.cardFocused.GetComponent<CardController>().displayNewSkillsPopUp());
//			}
//			if(model.cards[index].IdCardTypeUnlocked!=-1)
//			{
//				this.cardFocused.GetComponent<CardController>().displayNewCardTypePopUp();
//			}
//		}
//		else
//		{
//			this.cardFocused.GetComponent<CardMyGameController>().resetFocusedMyGameCard(model.cards[index]);
//			this.cardFocused.GetComponent<CardController>().setError();
//			model.cards[index].Error="";
//		}
//		if(gameobject.name.StartsWith("Card")||gameobject.name.StartsWith("Fcrd"))
//		{
//			int tempInt = System.Convert.ToInt32(gameobject.name.Substring(4))+view.myGameCardsVM.start;
//			this.displayedCards [tempInt - view.myGameCardsVM.start].GetComponent<CardMyGameController> ().resetMyGameCard (model.cards [index]);
//		}
//		else
//		{
//			int tempInt = System.Convert.ToInt32(gameobject.name.Substring(4));
//			this.displayedDeckCards [tempInt].GetComponent<CardMyGameController> ().resetMyGameCard (model.cards [index]);
//			this.displayedDeckCards [tempInt].GetComponent<CardController> ().setDeckOrderFeatures(tempInt);
//		}
//	}
//	public IEnumerator renameCard(string value, GameObject gameobject)
//	{
//		int index = retrieveCardIndex (gameobject.name);
//		int tempPrice = model.cards [index].RenameCost;
//		yield return StartCoroutine(model.cards [index].renameCard(value,tempPrice));
//		this.updateScene (index,gameobject.name);
//	}
//	public IEnumerator putOnMarketCard(int price, GameObject gameobject)
//	{
//		int index = retrieveCardIndex (gameobject.name);
//		yield return StartCoroutine (model.cards [index].toSell (price));
//		this.updateScene (index,gameobject.name);
//	}
//	public IEnumerator editSellPriceCard(int price, GameObject gameobject)
//	{
//		int index = retrieveCardIndex (gameobject.name);
//		yield return StartCoroutine (model.cards [index].changePriceCard (price));
//		this.updateScene (index,gameobject.name);
//	}
//	public IEnumerator unsellCard(GameObject gameobject)
//	{
//		int index = retrieveCardIndex (gameobject.name);
//		yield return StartCoroutine (model.cards [index].notToSell ());
//		this.updateScene (index,gameobject.name);
//	}
//	public void updateScene(int index, string name)
//	{
//		if(name.StartsWith("Card")||name.StartsWith("Fcrd"))
//		{
//			int tempInt = System.Convert.ToInt32(name.Substring(4))+view.myGameCardsVM.start;
//			this.displayedCards [tempInt - view.myGameCardsVM.start].GetComponent<CardMyGameController> ().resetMyGameCard (model.cards [index]);
//		}
//		else
//		{
//			int tempInt = System.Convert.ToInt32(name.Substring(4));
//			this.displayedDeckCards [tempInt].GetComponent<CardMyGameController> ().resetMyGameCard (model.cards [index]);
//			this.displayedDeckCards [tempInt].GetComponent<CardController> ().setDeckOrderFeatures(tempInt);
//		}
//		this.cardFocused.GetComponent<CardMyGameController> ().resetFocusedMyGameCard (model.cards [index]);
//		this.refreshCredits();
//		if(model.cards [index].Error=="")
//		{
//			this.setGUI (true);
//		}
//		else
//		{
//			this.cardFocused.GetComponent<CardController>().setError();
//			model.cards [index].Error="";
//		}
//	}
//	public void removeCardFromAllDecks(int id)
//	{
//		for(int i=0;i<model.decks.Count;i++)
//		{
//			for(int j=0;j<model.decks[i].Cards.Count;j++)
//			{
//				if(model.decks[i].Cards[j].Id==id)
//				{
//					model.decks[i].NbCards--;
//					model.decks[i].Cards.RemoveAt(j);
//					break;
//				}
//			}
//		}
//	}
//	public void removeDeckFromAllCards(int id)
//	{
//		for(int i=0;i<model.cards.Count;i++)
//		{
//			for(int j=0;j<model.cards[i].Decks.Count;j++)
//			{
//				if(model.cards[i].Decks[j]==id)
//				{
//					model.cards[i].Decks.RemoveAt(j);
//					break;
//				}
//			}
//		}
//	}
//	public void refreshCredits()
//	{
//		StartCoroutine(this.MenuObject.GetComponent<MenuController> ().getUserData ());
//	}
//	public void setGUI(bool value)
//	{
//		if(this.isTutorialLaunched)
//		{
//			if(this.tutorial.GetComponent<MyGameTutorialController>().getSequenceID()!=9)
//			{
//				view.myGameVM.guiEnabled = value;
//				if(this.cardFocused!=null)
//				{
//					this.cardFocused.GetComponent<CardController>().setMyGUI(value);
//				}
//				int finish = view.myGameCardsVM.nbCardsToDisplay;
//				this.setButtonsGui (value);
//			}
//		}
//		else
//		{
//			view.myGameVM.guiEnabled = value;
//			if(this.cardFocused!=null)
//			{
//				this.cardFocused.GetComponent<CardController>().setMyGUI(value);
//			}
//			int finish = view.myGameCardsVM.nbCardsToDisplay;
////			for(int i = 0 ; i < finish ; i++)
////			{
////				this.displayedCards[i].GetComponent<CardController>().setMyGUI(value);
////			}
//			for(int i = 0 ; i < ApplicationModel.nbCardsByDeck ; i++)
//			{
//				this.displayedDeckCards[i].GetComponent<CardController>().setMyGUI(value);
//			}
//			this.setButtonsGui (value);
//		}
//	}
//	private void initMyGameCardsVM()
//	{
//		view.myGameCardsVM.nbCards=model.cards.Count;
//		view.myGameCardsVM.cardsToBeDisplayed = new List<int> ();
//		for (int i=0;i<model.cards.Count;i++)
//		{
//			view.myGameCardsVM.cardsToBeDisplayed.Add (i);
//		}
//	}
//	private void initMyGameDecksVM()
//	{
//		view.myGameDecksVM.decksToBeDisplayed = new List<int> ();
//		view.myGameDecksVM.decksName = new List<string> ();
//		view.myGameDecksVM.decksNbCards = new List<int> ();
//		view.myGameDecksVM.myDecksGuiStyle=new List<GUIStyle>();
//		view.myGameDecksVM.myDecksButtonGuiStyle=new List<GUIStyle>();
//		for (int i=0;i<model.decks.Count;i++)
//		{
//			if(model.decks[i].Id==model.player.SelectedDeckId)
//			{
//				view.myGameDecksVM.decksToBeDisplayed.Insert(0,i);
//				view.myGameDecksVM.decksName.Insert(0,model.decks[i].Name);
//				view.myGameDecksVM.decksNbCards.Insert(0,model.decks[i].NbCards);
//			}
//			else
//			{
//				view.myGameDecksVM.decksToBeDisplayed.Add (i);
//				view.myGameDecksVM.decksName.Add (model.decks[i].Name);
//				view.myGameDecksVM.decksNbCards.Add(model.decks[i].NbCards);
//			}
//			view.myGameDecksVM.myDecksGuiStyle.Add (new GUIStyle());
//			view.myGameDecksVM.myDecksButtonGuiStyle.Add (new GUIStyle());
//			view.myGameDecksVM.myDecksGuiStyle[i]=view.myGameDecksVM.deckStyle;
//			view.myGameDecksVM.myDecksButtonGuiStyle[i]=view.myGameDecksVM.deckButtonStyle;
//		}
//		if(model.decks.Count>0)
//		{
//			view.myGameDecksVM.myDecksGuiStyle[view.myGameDecksVM.chosenDeck]=view.myGameDecksVM.deckChosenStyle;
//			view.myGameDecksVM.myDecksButtonGuiStyle[view.myGameDecksVM.chosenDeck]=view.myGameDecksVM.deckButtonChosenStyle;
//		}
//	}
//	private void initMyGameDeckCardsVM()
//	{
//		view.myGameDeckCardsVM.labelNoDecks=computeLabelNoDeck();
//		view.myGameDeckCardsVM.deckCardsToBeDisplayed = new List<int> ();
//		view.myGameDeckCardsVM.deckCardsOrder = new List<int> ();
//		if(model.decks.Count>0)
//		{
//			for(int i=0;i<model.decks[view.myGameDecksVM.decksToBeDisplayed[view.myGameDecksVM.chosenDeck]].Cards.Count;i++)
//			{
//				for(int j=0;j<model.cards.Count;j++)
//				{
//					if(model.decks[view.myGameDecksVM.decksToBeDisplayed[view.myGameDecksVM.chosenDeck]].Cards[i].Id==model.cards[j].Id && 
//					   model.decks[view.myGameDecksVM.decksToBeDisplayed[view.myGameDecksVM.chosenDeck]].Cards[i].deckOrder<ApplicationModel.nbCardsByDeck)
//					{
//						view.myGameDeckCardsVM.deckCardsToBeDisplayed.Add(j);
//						view.myGameDeckCardsVM.deckCardsOrder.Add (model.decks[view.myGameDecksVM.decksToBeDisplayed[view.myGameDecksVM.chosenDeck]].Cards[i].deckOrder);
//						break;
//					}
//				}
//			}
//		}
//	}
//	private string computeLabelNoDeck()
//	{
//		string text = "";
//		if(model.decks.Count<1)
//		{
//			text="Aucun deck disponible, cliquez sur 'Nouveau' pour ajouter un deck";
//		}
//		return text;
//	}
//	private void initStyles()
//	{
//		view.myGameScreenVM.styles=new GUIStyle[this.myGameScreenVMStyle.Length];
//		for(int i=0;i<this.myGameScreenVMStyle.Length;i++)
//		{
//			view.myGameScreenVM.styles[i]=this.myGameScreenVMStyle[i];
//		}
//		view.myGameScreenVM.initStyles();
//		view.myGameVM.styles=new GUIStyle[this.myGameVMStyle.Length];
//		for(int i=0;i<this.myGameVMStyle.Length;i++)
//		{
//			view.myGameVM.styles[i]=this.myGameVMStyle[i];
//		}
//		view.myGameVM.initStyles();
//		view.myGameFiltersVM.styles=new GUIStyle[this.myGameFiltersVMStyle.Length];
//		for(int i=0;i<this.myGameFiltersVMStyle.Length;i++)
//		{
//			view.myGameFiltersVM.styles[i]=this.myGameFiltersVMStyle[i];
//		}
//		view.myGameFiltersVM.initStyles();
//		view.myGameCardsVM.styles=new GUIStyle[this.myGameCardsVMStyle.Length];
//		for(int i=0;i<this.myGameCardsVMStyle.Length;i++)
//		{
//			view.myGameCardsVM.styles[i]=this.myGameCardsVMStyle[i];
//		}
//		view.myGameCardsVM.initStyles();
//		view.myGameDecksVM.styles=new GUIStyle[this.myGameDecksVMStyle.Length];
//		for(int i=0;i<this.myGameDecksVMStyle.Length;i++)
//		{
//			view.myGameDecksVM.styles[i]=this.myGameDecksVMStyle[i];
//		}
//		view.myGameDecksVM.initStyles();
//		view.myGameDeckCardsVM.styles=new GUIStyle[this.myGameDeckCardsVMStyle.Length];
//		for(int i=0;i<this.myGameDeckCardsVMStyle.Length;i++)
//		{
//			view.myGameDeckCardsVM.styles[i]=this.myGameDeckCardsVMStyle[i];
//		}
//		view.myGameDeckCardsVM.initStyles();
//	}
//	private void resize()
//	{
//		view.myGameScreenVM.resize ();
//		view.myGameFiltersVM.resize (view.myGameScreenVM.heightScreen);
//		view.myGameVM.resize (view.myGameScreenVM.heightScreen);
//		view.myGameCardsVM.resize (view.myGameScreenVM.heightScreen);
//		view.myGameDecksVM.resize(view.myGameScreenVM.heightScreen);
//		view.myGameDeckCardsVM.resize (view.myGameScreenVM.heightScreen);
//		if(this.errorPopUpView!=null)
//		{
//			this.errorPopUpResize();
//		}
//		if(this.newDeckPopUpView!=null)
//		{
//			this.newDeckPopUpResize();
//		}
//		if(this.editDeckPopUpView!=null)
//		{
//			this.editDeckPopUpResize();
//		}
//		if(this.deleteDeckPopUpView!=null)
//		{
//			this.deleteDeckPopUpResize();
//		}
//	}
//	public void popUpDisplayed(bool value, GameObject gameObject)
//	{
//		this.cardPopUpBelongTo = gameObject;
//		view.myGameVM.isPopUpDisplayed = value;
//	}
//	public void returnPressed()
//	{
//		if(view.myGameVM.isPopUpDisplayed)
//		{
//			this.cardPopUpBelongTo.GetComponent<CardController> ().confirmPopUp ();
//		}
//		else if(this.errorPopUpView!=null)
//		{
//			this.hideErrorPopUp();
//		}
//		else if(this.newDeckPopUpView!=null)
//		{
//			StartCoroutine(this.createNewDeck());
//		}
//		else if(this.editDeckPopUpView!=null)
//		{
//			StartCoroutine(this.editDeck());
//		}
//		else if(this.deleteDeckPopUpView!=null)
//		{
//			StartCoroutine(this.deleteDeck());
//		}
//	}
//	public void escapePressed()
//	{
//		if(view.myGameVM.isPopUpDisplayed)
//		{
//			this.cardPopUpBelongTo.GetComponent<CardController> ().exitPopUp ();
//		}
//		else if(this.cardFocused!=null && !this.isTutorialLaunched)
//		{
//			this.cardFocused.GetComponent<CardMyGameController> ().exitFocus();
//		}
//		else if(this.errorPopUpView!=null)
//		{
//			this.hideErrorPopUp();
//		}
//		else if(this.newDeckPopUpView!=null)
//		{
//			this.hideNewDeckPopUp();
//		}
//		else if(this.editDeckPopUpView!=null)
//		{
//			this.hideEditDeckPopUp();
//		}
//		else if(this.deleteDeckPopUpView!=null)
//		{
//			this.hideDeleteDeckPopUp();
//		}
//	}
//	private void createCards()
//	{
//		string name;
//		Vector3 scale;
//		Vector3 position;
//		float tempF = 10f*view.myGameScreenVM.widthScreen/view.myGameScreenVM.heightScreen;
//		float width = 10f*0.85f*view.myGameScreenVM.blockCardsWidth/(view.myGameScreenVM.blockCardsHeight+view.myGameScreenVM.blockDecksHeight+view.myGameScreenVM.gapBetweenblocks);
//		view.myGameCardsVM.nbCardsPerRow = Mathf.FloorToInt(width/1.6f);
//		float debutLargeur = -0.49f * tempF+1f + (width - 1.6f * view.myGameCardsVM.nbCardsPerRow)/2 ;
//		this.displayedCards = new GameObject[3*view.myGameCardsVM.nbCardsPerRow];
//		view.myGameCardsVM.nbCardsToDisplay = 0;
//		
//		for(int i = 0 ; i < 3*view.myGameCardsVM.nbCardsPerRow ; i++)
//		{
//			name = "Card" + i;
//			scale = new Vector3(1.5f, 1.5f, 1.5f);
//			position=new Vector3(debutLargeur + 1.6f * (i % view.myGameCardsVM.nbCardsPerRow), 0.35f - (i - i % view.myGameCardsVM.nbCardsPerRow) / view.myGameCardsVM.nbCardsPerRow * 1.98f,0);
//			this.displayedCards[i] = Instantiate(CardObject) as GameObject;
//			this.displayedCards[i].AddComponent<CardMyGameController>();
//			this.displayedCards[i].GetComponent<CardController>().setGameObject(name,scale,position);
//			
//			if (i<view.myGameCardsVM.cardsToBeDisplayed.Count)
//			{
//				this.displayedCards[i].GetComponent<CardMyGameController>().setMyGameCard(model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]]);
//				this.displayedCards[i].GetComponent<CardController> ().setCentralWindowRect (view.myGameScreenVM.centralWindow);
//				view.myGameCardsVM.nbCardsToDisplay++;
//				if(this.isFocus)
//				{
//					this.displayedCards[i].SetActive (false);
//				}
//			}   
//			else{
//				this.displayedCards[i].SetActive (false);
//			}
//		}
//	}
//	public void createDeckCards()
//	{
//		string name;
//		Vector3 scale;
//		Vector3 position;
//		float tempF = 10f*view.myGameScreenVM.widthScreen/view.myGameScreenVM.heightScreen;
//		float width = 10f*0.85f*view.myGameScreenVM.blockDeckCardsWidth/(view.myGameScreenVM.blockCardsHeight+view.myGameScreenVM.blockDecksHeight+view.myGameScreenVM.gapBetweenblocks);
//		float scaleDeck = Mathf.Min(1.6f, width / 6f);
//		float pas = (width - 5f * scaleDeck) / 6f;
//		float debutLargeur = -0.3f * tempF + pas + scaleDeck / 2;
//		this.displayedDeckCards = new GameObject[ApplicationModel.nbCardsByDeck];
//		
//		for (int i = 0; i < ApplicationModel.nbCardsByDeck; i++)
//		{
//			name="DCrd" + i;
//			scale = new Vector3(scaleDeck,scaleDeck,scaleDeck);
//			position = new Vector3(debutLargeur + (scaleDeck + pas) * i, 2.675f, 0); 
//			this.displayedDeckCards [i] = Instantiate(this.CardObject) as GameObject;
//			this.displayedDeckCards [i].AddComponent<CardMyGameController>();
//			this.displayedDeckCards [i].GetComponent<CardController>().setGameObject(name,scale,position);
//			this.displayedDeckCards[i].SetActive (false);
//		}
//		for(int i =0; i<view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count;i++)
//		{
//			this.displayedDeckCards[view.myGameDeckCardsVM.deckCardsOrder[i]].GetComponent<CardMyGameController>().setMyGameCard(model.cards[view.myGameDeckCardsVM.deckCardsToBeDisplayed[i]]);
//			this.displayedDeckCards[view.myGameDeckCardsVM.deckCardsOrder[i]].GetComponent<CardController> ().setCentralWindowRect (view.myGameScreenVM.centralWindow);
//			this.displayedDeckCards[view.myGameDeckCardsVM.deckCardsOrder[i]].GetComponent<CardController>().setDeckOrderFeatures(view.myGameDeckCardsVM.deckCardsOrder[i]);
//			if(!this.isFocus)
//			{
//				this.displayedDeckCards[view.myGameDeckCardsVM.deckCardsOrder[i]].SetActive (true);
//			}
//		}
//	}
//	private void setPagination()
//	{
//		view.myGameCardsVM.nbPages = Mathf.CeilToInt((view.myGameCardsVM.cardsToBeDisplayed.Count-1) / (3*view.myGameCardsVM.nbCardsPerRow))+1;
//		view.myGameCardsVM.pageDebut = 0 ;
//		if (view.myGameCardsVM.nbPages>15)
//		{
//			view.myGameCardsVM.pageFin = 15 ;
//		}
//		else
//		{
//			view.myGameCardsVM.pageFin = view.myGameCardsVM.nbPages ;
//		}
//		view.myGameCardsVM.chosenPage = 0;
//		view.myGameCardsVM.paginatorGuiStyle = new GUIStyle[view.myGameCardsVM.nbPages];
//		for (int i = 0; i < view.myGameCardsVM.nbPages; i++) 
//		{ 
//			if (i==0)
//			{
//				view.myGameCardsVM.paginatorGuiStyle[i]=view.myGameVM.paginationActivatedStyle;
//			}
//			else
//			{
//				view.myGameCardsVM.paginatorGuiStyle[i]=view.myGameVM.paginationStyle;
//			}
//		}
//	}
//	private void clearCards()
//	{
//		for (int i = 0; i < 3*view.myGameCardsVM.nbCardsPerRow; i++) 
//		{
//			Destroy(this.displayedCards[i]);
//		}
//	}
//	private void clearFocus()
//	{
//		if(this.isFocus)
//		{
//			this.cardFocused.SetActive (false);
//		}
//		this.isFocus = false;
//	}
//	private void clearDeckCards()
//	{
//		for (int i = 0; i < ApplicationModel.nbCardsByDeck; i++) 
//		{
//			Destroy(this.displayedDeckCards[i]);
//		}
//	}
//	private void displayPage()
//	{
//		view.myGameCardsVM.start = 3 * view.myGameCardsVM.nbCardsPerRow * view.myGameCardsVM.chosenPage;
//		view.myGameCardsVM.finish = view.myGameCardsVM.start + 3 * view.myGameCardsVM.nbCardsPerRow;
//		view.myGameCardsVM.nbCardsToDisplay = 0;
//		for(int i = view.myGameCardsVM.start ; i < view.myGameCardsVM.finish ; i++)
//		{
//			if (i<view.myGameCardsVM.cardsToBeDisplayed.Count)
//			{
//				this.displayedCards[i-view.myGameCardsVM.start].SetActive(true);
//				this.displayedCards[i-view.myGameCardsVM.start].GetComponent<CardMyGameController>().resetMyGameCard(model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]]);
//				view.myGameCardsVM.nbCardsToDisplay++;
//			}
//			else
//			{
//				displayedCards[i-view.myGameCardsVM.start].SetActive(false);
//			}
//		}
//	}
//	public void displayDeck(int chosenDeck)
//	{
//		view.myGameDecksVM.myDecksButtonGuiStyle [view.myGameDecksVM.chosenDeck] = view.myGameDecksVM.deckButtonStyle;
//		view.myGameDecksVM.myDecksButtonGuiStyle [chosenDeck] = view.myGameDecksVM.deckButtonChosenStyle;
//
//		view.myGameDecksVM.myDecksGuiStyle [view.myGameDecksVM.chosenDeck] = view.myGameDecksVM.deckStyle;
//		view.myGameDecksVM.myDecksGuiStyle [chosenDeck] = view.myGameDecksVM.deckChosenStyle;
//
//		view.myGameDecksVM.chosenDeck = chosenDeck;
//		this.initMyGameDeckCardsVM ();
//		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
//		{
//			this.displayedDeckCards[i].SetActive (false);
//		}
//		for(int i =0;i<view.myGameDeckCardsVM.deckCardsOrder.Count;i++)
//		{
//			this.displayedDeckCards[view.myGameDeckCardsVM.deckCardsOrder[i]].SetActive (true);
//			this.displayedDeckCards[view.myGameDeckCardsVM.deckCardsOrder[i]].GetComponent<CardMyGameController>().resetMyGameCard(model.cards[view.myGameDeckCardsVM.deckCardsToBeDisplayed[i]]);
//			this.displayedDeckCards[view.myGameDeckCardsVM.deckCardsOrder[i]].GetComponent<CardController>().setDeckOrderFeatures(view.myGameDeckCardsVM.deckCardsOrder[i]);
//		}
//		this.filterCards ();
//	}
//	private void initializeSortButtons()
//	{
//		for (int i =0;i<8;i++)
//		{
//			view.myGameFiltersVM.sortButtonStyle[i]=view.myGameVM.sortDefaultButtonStyle;
//		}
//	}
//	private void initializeToggles()
//	{
//		view.myGameFiltersVM.cardTypeList=new string[model.cardTypeList.Length];
//		for(int i=0;i<model.cardTypeList.Length-1;i++)
//		{
//			view.myGameFiltersVM.cardTypeList[i] = model.cardTypeList[i];
//		}
//		view.myGameFiltersVM.togglesCurrentStates = new bool[model.cardTypeList.Length];
//		for(int i = 0 ; i < model.cardTypeList.Length-1 ; i++)
//		{
//			view.myGameFiltersVM.togglesCurrentStates[i] = false;
//		}
//	}
//	private void setFilters()
//	{
//		view.myGameFiltersVM.minLifeLimit=10000;
//		view.myGameFiltersVM.maxLifeLimit=0;
//		view.myGameFiltersVM.minAttackLimit=10000;
//		view.myGameFiltersVM.maxAttackLimit=0;
//		view.myGameFiltersVM.minQuicknessLimit=10000;
//		view.myGameFiltersVM.maxQuicknessLimit=0;
//		
//		int max = model.cards.Count;
//		for (int i = 0; i < max ; i++) 
//		{
//			if (model.cards[i].Life<view.myGameFiltersVM.minLifeLimit)
//			{
//				view.myGameFiltersVM.minLifeLimit = model.cards[i].Life;
//			}
//			if (model.cards[i].Life>view.myGameFiltersVM.maxLifeLimit)
//			{
//				view.myGameFiltersVM.maxLifeLimit = model.cards[i].Life;
//			}
//			if (model.cards[i].Attack<view.myGameFiltersVM.minAttackLimit)
//			{
//				view.myGameFiltersVM.minAttackLimit = model.cards[i].Attack;
//			}
//			if (model.cards[i].Attack>view.myGameFiltersVM.maxAttackLimit)
//			{
//				view.myGameFiltersVM.maxAttackLimit = model.cards[i].Attack;
//			}
//			if (model.cards[i].Speed<view.myGameFiltersVM.minQuicknessLimit)
//			{
//				view.myGameFiltersVM.minQuicknessLimit = model.cards[i].Speed;
//			}
//			if (model.cards[i].Speed>view.myGameFiltersVM.maxQuicknessLimit)
//			{
//				view.myGameFiltersVM.maxQuicknessLimit = model.cards[i].Speed;
//			}
//		}
//		if(view.myGameFiltersVM.minLifeLimit>view.myGameFiltersVM.maxLifeLimit)
//		{
//			view.myGameFiltersVM.minLifeLimit=0;
//			view.myGameFiltersVM.maxLifeLimit=0;
//		}
//		if(view.myGameFiltersVM.minAttackLimit>view.myGameFiltersVM.maxAttackLimit)
//		{
//			view.myGameFiltersVM.minAttackLimit=0;
//			view.myGameFiltersVM.maxAttackLimit=0;
//		}
//		if(view.myGameFiltersVM.minQuicknessLimit>view.myGameFiltersVM.maxQuicknessLimit)
//		{
//			view.myGameFiltersVM.minQuicknessLimit=0;
//			view.myGameFiltersVM.maxQuicknessLimit=0;
//		}
//		view.myGameFiltersVM.minLifeVal = view.myGameFiltersVM.minLifeLimit;
//		view.myGameFiltersVM.maxLifeVal = view.myGameFiltersVM.maxLifeLimit;
//		view.myGameFiltersVM.oldMinLifeVal = view.myGameFiltersVM.minLifeLimit;
//		view.myGameFiltersVM.oldMaxLifeVal = view.myGameFiltersVM.maxLifeLimit;
//		view.myGameFiltersVM.minAttackVal = view.myGameFiltersVM.minAttackLimit;
//		view.myGameFiltersVM.maxAttackVal = view.myGameFiltersVM.maxAttackLimit;
//		view.myGameFiltersVM.oldMinAttackVal = view.myGameFiltersVM.minAttackLimit;
//		view.myGameFiltersVM.oldMaxAttackVal = view.myGameFiltersVM.maxAttackLimit;
//		view.myGameFiltersVM.minQuicknessVal = view.myGameFiltersVM.minQuicknessLimit;
//		view.myGameFiltersVM.maxQuicknessVal = view.myGameFiltersVM.maxQuicknessLimit;
//		view.myGameFiltersVM.oldMinQuicknessVal = view.myGameFiltersVM.minQuicknessLimit;
//		view.myGameFiltersVM.oldMaxQuicknessVal = view.myGameFiltersVM.maxQuicknessLimit;
//	}
//	public void paginationBack()
//	{
//		view.myGameCardsVM.pageDebut = view.myGameCardsVM.pageDebut-15;
//		view.myGameCardsVM.pageFin = view.myGameCardsVM.pageDebut+15;
//	}
//	public void paginationSelect(int chosenPage)
//	{
//		view.myGameCardsVM.paginatorGuiStyle[view.myGameCardsVM.chosenPage]=view.myGameVM.paginationStyle;
//		view.myGameCardsVM.chosenPage=chosenPage;
//		view.myGameCardsVM.paginatorGuiStyle[chosenPage]=this.view.myGameVM.paginationActivatedStyle;
//		this.displayPage();
//	}
//	public void paginationNext()
//	{
//		view.myGameCardsVM.pageDebut = view.myGameCardsVM.pageDebut+15;
//		view.myGameCardsVM.pageFin = Mathf.Min(view.myGameCardsVM.pageFin+15, view.myGameCardsVM.nbPages);
//	}
//	
//	private void applyFilters() 
//	{
//		view.myGameCardsVM.cardsToBeDisplayed=new List<int>();
//		IList<int> tempCardsToBeDisplayed = new List<int>();
//		int nbFilters = view.myGameFiltersVM.filtersCardType.Count;
//		int tempMinPrice=0;
//		int tempMaxPrice=999999999;
//		
//		bool testFilters = false;
//		bool testDeck = false;
//		bool test ;
//		
//		bool minLifeBool = (view.myGameFiltersVM.minLifeLimit==view.myGameFiltersVM.minLifeVal);
//		bool maxLifeBool = (view.myGameFiltersVM.maxLifeLimit==view.myGameFiltersVM.maxLifeVal);
//		bool minQuicknessBool = (view.myGameFiltersVM.minQuicknessLimit==view.myGameFiltersVM.minQuicknessVal);
//		bool maxQuicknessBool = (view.myGameFiltersVM.maxQuicknessLimit==view.myGameFiltersVM.maxQuicknessVal);
//		bool minAttackBool = (view.myGameFiltersVM.minAttackLimit==view.myGameFiltersVM.minAttackVal);
//		bool maxAttackBool = (view.myGameFiltersVM.maxAttackLimit==view.myGameFiltersVM.maxAttackVal);
//		
//		
//		if (view.myGameFiltersVM.isSkillChosen)
//		{
//			int max = model.cards.Count;
//			if (nbFilters == 0)
//			{
//				max = model.cards.Count;
//				if (view.myGameFiltersVM.onSale)
//				{
//					for (int i = 0; i < max; i++)
//					{
//						if (model.cards [i].hasSkill(view.myGameFiltersVM.valueSkill) && model.cards [i].onSale == 1)
//						{
//							testDeck = false;
//							for (int j = 0; j < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; j++)
//							{
//								if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [j])
//								{
//									testDeck = true;
//								}
//							}
//							if (!testDeck)
//							{
//								tempCardsToBeDisplayed.Add(i);
//							}
//						}
//					}
//				}
//				else if (view.myGameFiltersVM.notOnSale)
//				{
//					for (int i = 0; i < max; i++)
//					{
//						if (model.cards [i].hasSkill(view.myGameFiltersVM.valueSkill) && model.cards [i].onSale == 0)
//						{
//							testDeck = false;
//							for (int j = 0; j < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; j++)
//							{
//								if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [j])
//								{
//									testDeck = true;
//								}
//							}
//							if (!testDeck)
//							{
//								tempCardsToBeDisplayed.Add(i);
//							}
//						}
//					}
//				}
//				else
//				{
//					for (int i = 0; i < max; i++)
//					{
//						if (model.cards [i].hasSkill(view.myGameFiltersVM.valueSkill))
//						{
//							testDeck = false;
//							for (int j = 0; j < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; j++)
//							{
//								if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [j])
//								{
//									testDeck = true;
//								}
//							}
//							if (!testDeck)
//							{
//								tempCardsToBeDisplayed.Add(i);
//							}
//						}
//					}
//				}
//			} else
//			{
//				for (int i = 0; i < max; i++)
//				{
//					test = false;
//					int j = 0;
//					if (view.myGameFiltersVM.onSale)
//					{
//						while (!test && j != nbFilters)
//						{
//							if (model.cards [i].IdClass == view.myGameFiltersVM.filtersCardType [j])
//							{
//								test = true;
//								if (model.cards [i].hasSkill(view.myGameFiltersVM.valueSkill) && model.cards [i].onSale == 1)
//								{
//									testDeck = false;
//									for (int k = 0; k < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; k++)
//									{
//										if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [k])
//										{
//											testDeck = true; 
//										}
//									}
//									if (!testDeck)
//									{
//										tempCardsToBeDisplayed.Add(i);
//									}
//								}
//							}
//							j++;
//						}
//					} 
//					else if (view.myGameFiltersVM.notOnSale)
//					{
//						while (!test && j != nbFilters)
//						{
//							if (model.cards [i].IdClass == view.myGameFiltersVM.filtersCardType [j])
//							{
//								test = true;
//								if (model.cards [i].hasSkill(view.myGameFiltersVM.valueSkill) && model.cards [i].onSale == 0)
//								{
//									testDeck = false;
//									for (int k = 0; k < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; k++)
//									{
//										if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [k])
//										{
//											testDeck = true; 
//										}
//									}
//									if (!testDeck)
//									{
//										tempCardsToBeDisplayed.Add(i);
//									}
//								}
//							}
//							j++;
//						}
//					}
//					else
//					{
//						while (!test && j != nbFilters)
//						{
//							if (model.cards [i].IdClass == view.myGameFiltersVM.filtersCardType [j])
//							{
//								test = true;
//								if (model.cards [i].hasSkill(view.myGameFiltersVM.valueSkill))
//								{
//									testDeck = false;
//									for (int k = 0; k < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; k++)
//									{
//										if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [k])
//										{
//											testDeck = true; 
//										}
//									}
//									if (!testDeck)
//									{
//										tempCardsToBeDisplayed.Add(i);
//									}
//								}
//							}
//							j++;
//						}
//					}
//				}
//			}
//		} 
//		else
//		{
//			int max = model.cards.Count;
//			if (nbFilters == 0)
//			{
//				if (view.myGameFiltersVM.onSale)
//				{
//					for (int i = 0; i < max; i++)
//					{
//						if (model.cards [i].onSale == 1)
//						{
//							testDeck = false;
//							for (int j = 0; j < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; j++)
//							{
//								if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [j])
//								{
//									testDeck = true;
//								}
//							}
//							if (!testDeck)
//							{
//								tempCardsToBeDisplayed.Add(i);
//							}
//						}
//					}
//				}
//				else if(view.myGameFiltersVM.notOnSale)
//				{
//					for (int i = 0; i < max; i++)
//					{
//						if (model.cards [i].onSale == 0)
//						{
//							testDeck = false;
//							for (int j = 0; j < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; j++)
//							{
//								if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [j])
//								{
//									testDeck = true;
//								}
//							}
//							if (!testDeck)
//							{
//								tempCardsToBeDisplayed.Add(i);
//							}
//						}
//					}
//				}
//				else
//				{
//					for (int i = 0; i < max; i++)
//					{
//						testDeck = false;
//						for (int j = 0; j < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; j++)
//						{
//							if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [j])
//							{
//								testDeck = true;
//							}
//						}
//						if (!testDeck)
//						{
//							tempCardsToBeDisplayed.Add(i);
//						}
//					}
//				}
//			} 
//			else
//			{
//				if (view.myGameFiltersVM.onSale)
//				{
//					for (int i = 0; i < max; i++)
//					{
//						test = false;
//						int j = 0;
//						while (!test && j != nbFilters)
//						{
//							if (model.cards [i].IdClass == view.myGameFiltersVM.filtersCardType [j])
//							{
//								if (model.cards [i].onSale == 1)
//								{
//									test = true;
//									testDeck = false;
//									for (int k = 0; k < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; k++)
//									{
//										if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [k])
//										{
//											testDeck = true; 
//										}
//									}
//									if (!testDeck)
//									{
//										tempCardsToBeDisplayed.Add(i);
//									}
//								}
//							}
//							j++;
//						}
//					}
//				}
//				else if(view.myGameFiltersVM.notOnSale)
//				{
//					for (int i = 0; i < max; i++)
//					{
//						test = false;
//						int j = 0;
//						while (!test && j != nbFilters)
//						{
//							if (model.cards [i].IdClass == view.myGameFiltersVM.filtersCardType [j])
//							{
//								if (model.cards [i].onSale == 0)
//								{
//									test = true;
//									testDeck = false;
//									for (int k = 0; k < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; k++)
//									{
//										if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [k])
//										{
//											testDeck = true; 
//										}
//									}
//									if (!testDeck)
//									{
//										tempCardsToBeDisplayed.Add(i);
//									}
//								}
//							}
//							j++;
//						}
//					}
//				}
//				else
//				{
//					for (int i = 0; i < max; i++)
//					{
//						test = false;
//						int j = 0;
//						while (!test && j != nbFilters)
//						{
//							if (model.cards [i].IdClass == view.myGameFiltersVM.filtersCardType [j])
//							{
//								test = true;
//								testDeck = false;
//								for (int k = 0; k < view.myGameDeckCardsVM.deckCardsToBeDisplayed.Count; k++)
//								{
//									if (i == view.myGameDeckCardsVM.deckCardsToBeDisplayed [k])
//									{
//										testDeck = true;
//									}
//								}
//								if (!testDeck)
//								{
//									tempCardsToBeDisplayed.Add(i);
//								}
//							}
//							j++;
//						}
//					}
//				}
//			}
//		}
//		if (tempCardsToBeDisplayed.Count>0){
//			view.myGameFiltersVM.minLifeLimit=10000;
//			view.myGameFiltersVM.maxLifeLimit=0;
//			view.myGameFiltersVM.minAttackLimit=10000;
//			view.myGameFiltersVM.maxAttackLimit=0;
//			view.myGameFiltersVM.minQuicknessLimit=10000;
//			view.myGameFiltersVM.maxQuicknessLimit=0;
//			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++){
//				if (model.cards[tempCardsToBeDisplayed[i]].Life<view.myGameFiltersVM.minLifeLimit){
//					view.myGameFiltersVM.minLifeLimit = model.cards[tempCardsToBeDisplayed[i]].Life;
//				}
//				if (model.cards[tempCardsToBeDisplayed[i]].Life>view.myGameFiltersVM.maxLifeLimit){
//					view.myGameFiltersVM.maxLifeLimit = model.cards[tempCardsToBeDisplayed[i]].Life;
//				}
//				if (model.cards[tempCardsToBeDisplayed[i]].Attack<view.myGameFiltersVM.minAttackLimit){
//					view.myGameFiltersVM.minAttackLimit = model.cards[tempCardsToBeDisplayed[i]].Attack;
//				}
//				if (model.cards[tempCardsToBeDisplayed[i]].Attack>view.myGameFiltersVM.maxAttackLimit){
//					view.myGameFiltersVM.maxAttackLimit = model.cards[tempCardsToBeDisplayed[i]].Attack;
//				}
//				if (model.cards[tempCardsToBeDisplayed[i]].Speed<view.myGameFiltersVM.minQuicknessLimit){
//					view.myGameFiltersVM.minQuicknessLimit = model.cards[tempCardsToBeDisplayed[i]].Speed;
//				}
//				if (model.cards[tempCardsToBeDisplayed[i]].Speed>view.myGameFiltersVM.maxQuicknessLimit){
//					view.myGameFiltersVM.maxQuicknessLimit = model.cards[tempCardsToBeDisplayed[i]].Speed;
//				}
//			}
//			if (minLifeBool && view.myGameFiltersVM.maxLifeVal>view.myGameFiltersVM.minLifeLimit){
//				view.myGameFiltersVM.minLifeVal = view.myGameFiltersVM.minLifeLimit;
//			}
//			else{
//				if (view.myGameFiltersVM.minLifeVal<view.myGameFiltersVM.minLifeLimit){
//					view.myGameFiltersVM.minLifeLimit = view.myGameFiltersVM.minLifeVal;
//				}
//			}
//			if (maxLifeBool && view.myGameFiltersVM.minLifeVal<view.myGameFiltersVM.maxLifeLimit){
//				view.myGameFiltersVM.maxLifeVal = view.myGameFiltersVM.maxLifeLimit;
//			}
//			else{
//				if (view.myGameFiltersVM.maxLifeVal>view.myGameFiltersVM.maxLifeLimit){
//					view.myGameFiltersVM.maxLifeLimit = view.myGameFiltersVM.maxLifeVal;
//				}
//			}
//			if (minAttackBool && view.myGameFiltersVM.maxAttackVal>view.myGameFiltersVM.minAttackLimit){
//				view.myGameFiltersVM.minAttackVal = view.myGameFiltersVM.minAttackLimit;
//			}
//			else{
//				if (view.myGameFiltersVM.minAttackVal<view.myGameFiltersVM.minAttackLimit){
//					view.myGameFiltersVM.minAttackLimit = view.myGameFiltersVM.minAttackVal;
//				}
//			}
//			if (maxAttackBool && view.myGameFiltersVM.minAttackVal<view.myGameFiltersVM.maxAttackLimit){
//				view.myGameFiltersVM.maxAttackVal = view.myGameFiltersVM.maxAttackLimit;
//			}
//			else{
//				if (view.myGameFiltersVM.maxAttackVal>view.myGameFiltersVM.maxAttackLimit){
//					view.myGameFiltersVM.maxAttackLimit = view.myGameFiltersVM.maxAttackVal;
//				}
//			}
//			if (minQuicknessBool && view.myGameFiltersVM.maxQuicknessVal>view.myGameFiltersVM.minQuicknessLimit){
//				view.myGameFiltersVM.minQuicknessVal = view.myGameFiltersVM.minQuicknessLimit;
//			}
//			else{
//				if (view.myGameFiltersVM.minQuicknessVal<view.myGameFiltersVM.minQuicknessLimit){
//					view.myGameFiltersVM.minQuicknessLimit = view.myGameFiltersVM.minQuicknessVal;
//				}
//			}
//			if (maxQuicknessBool && view.myGameFiltersVM.minQuicknessVal<view.myGameFiltersVM.maxQuicknessLimit){
//				view.myGameFiltersVM.maxQuicknessVal = view.myGameFiltersVM.maxQuicknessLimit;
//			}
//			else{
//				if (view.myGameFiltersVM.maxQuicknessVal>view.myGameFiltersVM.maxQuicknessLimit){
//					view.myGameFiltersVM.maxQuicknessLimit = view.myGameFiltersVM.maxQuicknessVal;
//				}
//			}
//			
//			view.myGameFiltersVM.oldMinLifeVal = view.myGameFiltersVM.minLifeVal ;
//			view.myGameFiltersVM.oldMaxLifeVal = view.myGameFiltersVM.maxLifeVal ;
//			view.myGameFiltersVM.oldMinQuicknessVal = view.myGameFiltersVM.minQuicknessVal ;
//			view.myGameFiltersVM.oldMaxQuicknessVal = view.myGameFiltersVM.maxQuicknessVal ;
//			view.myGameFiltersVM.oldMinAttackVal = view.myGameFiltersVM.minAttackVal ;
//			view.myGameFiltersVM.oldMaxAttackVal = view.myGameFiltersVM.maxAttackVal ;
//		}
//		
//		if (view.myGameFiltersVM.minLifeVal!=view.myGameFiltersVM.minLifeLimit){
//			testFilters = true ;
//		}
//		else if (view.myGameFiltersVM.maxLifeVal!=view.myGameFiltersVM.maxLifeLimit){
//			testFilters = true ;
//		}
//		else if (view.myGameFiltersVM.minAttackVal!=view.myGameFiltersVM.minAttackLimit){
//			testFilters = true ;
//		}
//		else if (view.myGameFiltersVM.maxAttackVal!=view.myGameFiltersVM.maxAttackLimit){
//			testFilters = true ;
//		}
//		else if (view.myGameFiltersVM.minQuicknessVal!=view.myGameFiltersVM.minQuicknessLimit){
//			testFilters = true ;
//		}
//		else if (view.myGameFiltersVM.maxQuicknessVal!=view.myGameFiltersVM.maxQuicknessLimit){
//			testFilters = true ;
//		}
//		
//		if (testFilters == true){
//			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++)
//			{
//				if (model.cards[tempCardsToBeDisplayed[i]].verifyC(0,100,view.myGameFiltersVM.minLifeVal,
//				                                                    view.myGameFiltersVM.maxLifeVal,
//				                                                    view.myGameFiltersVM.minAttackVal,
//				                                                    view.myGameFiltersVM.maxAttackVal,
//				                                                    view.myGameFiltersVM.minQuicknessVal,
//				                                                    view.myGameFiltersVM.maxQuicknessVal)){
//					view.myGameCardsVM.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
//				}
//			}
//		}
//		else
//		{
//			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++)
//			{
//				view.myGameCardsVM.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
//			}
//		}
//		view.myGameCardsVM.nbPages = Mathf.CeilToInt(view.myGameCardsVM.cardsToBeDisplayed.Count / (3.0f*view.myGameCardsVM.nbCardsPerRow));
//		view.myGameCardsVM.pageDebut = 0 ;
//		if (view.myGameCardsVM.nbPages>15){
//			view.myGameCardsVM.pageFin = 15 ;
//		}
//		else
//		{
//			view.myGameCardsVM.pageFin = view.myGameCardsVM.nbPages ;
//		}
//		view.myGameCardsVM.chosenPage = 0;
//		view.myGameCardsVM.start = 0;
//		view.myGameCardsVM.paginatorGuiStyle = new GUIStyle[view.myGameCardsVM.nbPages];
//		for (int i = 0; i < view.myGameCardsVM.nbPages; i++) { 
//			if (i==0){
//				view.myGameCardsVM.paginatorGuiStyle[i]=view.myGameVM.paginationActivatedStyle;
//			}
//			else{
//				view.myGameCardsVM.paginatorGuiStyle[i]=view.myGameVM.paginationStyle;
//			}
//		}
//	}
//	public void filterCards()
//	{
//		this.applyFilters ();
//		if (view.myGameFiltersVM.oldSortSelected!=10)
//		{
//			this.sortCards(view.myGameFiltersVM.oldSortSelected);
//		}
//		this.clearCards ();
//		this.createCards ();
//		this.setPagination ();
//	}
//	public void isBeingDragged()
//	{
//		view.myGameVM.isBeingDragged = true;
//	}
//	public void isNotBeingDragged()
//	{
//		if(view.myGameVM.isBeingDragged)
//		{
//			view.myGameVM.isBeingDragged = false;
//			this.refreshMinMaxFilters();
//		}
//	}
//	public void refreshMinMaxFilters()
//	{
//		bool isMoved = false ;
//		view.myGameFiltersVM.maxLifeVal=Mathf.RoundToInt(view.myGameFiltersVM.maxLifeVal);
//		view.myGameFiltersVM.minLifeVal=Mathf.RoundToInt(view.myGameFiltersVM.minLifeVal);
//		view.myGameFiltersVM.maxAttackVal=Mathf.RoundToInt(view.myGameFiltersVM.maxAttackVal);
//		view.myGameFiltersVM.minAttackVal=Mathf.RoundToInt(view.myGameFiltersVM.minAttackVal);
//		view.myGameFiltersVM.maxQuicknessVal=Mathf.RoundToInt(view.myGameFiltersVM.maxQuicknessVal);
//		view.myGameFiltersVM.minQuicknessVal=Mathf.RoundToInt(view.myGameFiltersVM.minQuicknessVal);
//		
//		if (view.myGameFiltersVM.oldMaxLifeVal != view.myGameFiltersVM.maxLifeVal){
//			view.myGameFiltersVM.oldMaxLifeVal = view.myGameFiltersVM.maxLifeVal;
//			isMoved = true ; 
//		}
//		if (view.myGameFiltersVM.oldMinLifeVal != view.myGameFiltersVM.minLifeVal){
//			view.myGameFiltersVM.oldMinLifeVal = view.myGameFiltersVM.minLifeVal;
//			isMoved = true ; 
//		}
//		if (view.myGameFiltersVM.oldMaxAttackVal != view.myGameFiltersVM.maxAttackVal){
//			view.myGameFiltersVM.oldMaxAttackVal = view.myGameFiltersVM.maxAttackVal;
//			isMoved = true ; 
//		}
//		if (view.myGameFiltersVM.oldMinAttackVal != view.myGameFiltersVM.minAttackVal){
//			view.myGameFiltersVM.oldMinAttackVal = view.myGameFiltersVM.minAttackVal;
//			isMoved = true ; 
//		}
//		if (view.myGameFiltersVM.oldMaxQuicknessVal != view.myGameFiltersVM.maxQuicknessVal){
//			view.myGameFiltersVM.oldMaxQuicknessVal = view.myGameFiltersVM.maxQuicknessVal;
//			isMoved = true ; 
//		}
//		if (view.myGameFiltersVM.oldMinQuicknessVal != view.myGameFiltersVM.minQuicknessVal){
//			view.myGameFiltersVM.oldMinQuicknessVal = view.myGameFiltersVM.minQuicknessVal;
//			isMoved = true ; 
//		}
//		if(isMoved){
//			this.filterCards();
//		}
//	}
//	public void selectCardType(bool value, int id)
//	{
//		view.myGameFiltersVM.togglesCurrentStates [id] = value;
//		if (value)
//		{
//			view.myGameFiltersVM.filtersCardType.Add(id);
//		}
//		else
//		{
//			view.myGameFiltersVM.filtersCardType.Remove(id);
//		}
//		this.filterCards ();
//	}
//	public void selectOnSale(bool value)
//	{
//		if(value && view.myGameFiltersVM.notOnSale)
//		{
//			view.myGameFiltersVM.notOnSale=false;
//		}
//		view.myGameFiltersVM.onSale = value;
//		this.filterCards ();
//	}
//	public void selectNotOnSale(bool value)
//	{
//		if(value && view.myGameFiltersVM.onSale)
//		{
//			view.myGameFiltersVM.onSale=false;
//		}
//		view.myGameFiltersVM.notOnSale = value;
//		this.filterCards ();
//	}
//	public void selectSkills(string value)
//	{
//		if (value.Length > 0) 
//		{
//			view.myGameFiltersVM.isSkillToDisplay=true;
//			view.myGameFiltersVM.valueSkill = value.ToLower ();
//			view.myGameFiltersVM.matchValues = new List<string> ();	
//			if (view.myGameFiltersVM.valueSkill != "") 
//			{
//				view.myGameFiltersVM.matchValues = new List<string> ();
//				for (int i = 0; i < model.skillsList.Count; i++) 
//				{  
//					if (model.skillsList [i].ToLower ().Contains (view.myGameFiltersVM.valueSkill)) 
//					{
//						view.myGameFiltersVM.matchValues.Add (model.skillsList [i]);
//					}
//				}
//			}
//		} 
//		else 
//		{
//			view.myGameFiltersVM.isSkillToDisplay=false;
//			view.myGameFiltersVM.valueSkill = "";
//		}
//		if (view.myGameFiltersVM.isSkillChosen)
//		{
//			view.myGameFiltersVM.isSkillChosen=false ;
//			this.filterCards() ;
//		}
//	}
//	public void filterASkill(string value)
//	{
//		view.myGameFiltersVM.valueSkill = value.ToLower ();
//		view.myGameFiltersVM.isSkillChosen=true ;
//		view.myGameFiltersVM.matchValues = new List<string>();
//		this.filterCards ();
//	}
//	public void sortCards(int id)
//	{
//		this.applySorts (id);
//		this.loadCards ();
//	}
//	public void applySorts(int id)
//	{
//		int tempA=new int();
//		int tempB=new int();
//		
//		if(view.myGameFiltersVM.oldSortSelected!=10)
//		{
//			view.myGameFiltersVM.sortButtonStyle[view.myGameFiltersVM.oldSortSelected]=view.myGameVM.sortDefaultButtonStyle;
//		}
//		view.myGameFiltersVM.sortButtonStyle[id]=view.myGameVM.sortActivatedButtonStyle;
//		view.myGameFiltersVM.oldSortSelected=id;
//		
//		for (int i = 1; i<view.myGameCardsVM.cardsToBeDisplayed.Count; i++) {
//			
//			for (int j=0;j<i;j++){
//				
//				
//				switch (id)
//				{
//				case 0:
//					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Life;
//					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Life;
//					break;
//				case 1:
//					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Life;
//					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Life;
//					break;
//				case 2:
//					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Attack;
//					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Attack;
//					break;
//				case 3:
//					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Attack;
//					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Attack;
//					break;
//				case 4:
//					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Speed;
//					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Speed;
//					break;
//				case 5:
//					tempB = model.cards[view.myGameCardsVM.cardsToBeDisplayed[i]].Speed;
//					tempA = model.cards[view.myGameCardsVM.cardsToBeDisplayed[j]].Speed;
//					break;
//				default:
//					break;
//				}
//				
//				if (tempA<tempB){
//					view.myGameCardsVM.cardsToBeDisplayed.Insert (j,view.myGameCardsVM.cardsToBeDisplayed[i]);
//					view.myGameCardsVM.cardsToBeDisplayed.RemoveAt(i+1);
//					break;
//				}
//				
//			}
//		}
//	}
//	public void setButtonsGui(bool value)
//	{
//		for(int i=0;i<view.myGameVM.buttonsEnabled.Length;i++)
//		{
//			view.myGameVM.buttonsEnabled[i]=value;
//		}
//		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
//		{
//			this.displayedDeckCards[i].GetComponent<CardMyGameController>().setButtonsGui(value);
//		}
//	}
//	public void setButtonGui(int index, bool value)
//	{
//		view.myGameVM.buttonsEnabled[index]=value;
//	}
//	public Vector2 getCardsPosition(int index)
//	{
//		return this.displayedCards[index].GetComponent<CardController>().GOPosition;
//	}
//	public Vector2 getCardsSize(int index)
//	{
//		return this.displayedCards[index].GetComponent<CardController>().GOSize;
//	}
//	public Vector2 getFocusCardsPosition()
//	{
//		return this.cardFocused.GetComponent<CardController>().GOPosition;
//	}
//	public Vector2 getFocusCardsSize()
//	{
//		return this.cardFocused.GetComponent<CardController>().GOSize;
//	}
//	public void setButtonGuiOnFocusedCard(int index, bool value)
//	{
//		this.cardFocused.GetComponent<CardMyGameController> ().setButtonGui (index, value);
//	}
//	public void tutorialCardUpgrated()
//	{
//		this.tutorial.GetComponent<MyGameTutorialController> ().actionIsDone ();
//	}
//	public void tutorialCardLeaved()
//	{
//		this.cardFocused.GetComponent<CardMyGameController>().setIsTutorialLaunched(false);
//		this.tutorial.GetComponent<MyGameTutorialController> ().actionIsDone ();
//	}
//	public IEnumerator endTutorial()
//	{
//		MenuController.instance.setButtonsGui (false);
//		yield return StartCoroutine (model.player.setTutorialStep (3));
//		Application.LoadLevel ("Lobby");
//	}
//}

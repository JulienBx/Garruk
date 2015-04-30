using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class myGameScript : MonoBehaviour
{
	public static myGameScript instance;
	public MyGameView myGameView;
	public GameObject MenuObject;
	public GameObject CardObject;

	private User userModel;

	public Rect centralWindow;

	public GUIStyle[] FilterVMStyle;
	public GUIStyle[] PopUpVMStyle;
	public GUIStyle[] MydecksVMStyle;
	public GUIStyle[] SortVMStyle;
	public GUIStyle[] PaginationVMStyle;
	public GUIStyle[] FocusVMStyle;
	public Texture2D[] MyDecksVMTexture;
	public Texture2D[] MyGameVMTexture;

	public int widthScreen = Screen.width; 
	public int heightScreen = Screen.height;

	void Start()
	{
		instance = this;
		resize();
		myGameView = Camera.main.gameObject.AddComponent<MyGameView>();
		userModel = new User(ApplicationModel.username);
	}

	private void resize()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.centralWindow = new Rect(this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
	}

	public void setGUI(bool value)
	{
		/*myGameView.marketVM.guiEnabled = value;
		if(cardFocused==null)
		{
			int finish = 3 * view.marketCardsVM.nbCardsPerRow;
			for(int i = 0 ; i < finish ; i++)
			{
				this.displayedCards[i].GetComponent<CardController>().setMyGUI(value);
			}
		}
		else
		{
			this.cardFocused.GetComponent<CardController>().setMyGUI(value);
		}*/
	}
	
	public void Update()
	{
		if (Screen.width != widthScreen || Screen.height != heightScreen)
		{
			setStyles();
			//applyFilters();
			if (myGameView.focusVM.focusedCard != -1)
			{
				Destroy(myGameView.myGameVM.cardFocused);
				myGameView.focusVM.focusedCard = -1;
			}
			this.clearCards();
			this.clearDeckCards();
			this.createCards();
			this.createDeckCards();
			myGameView.filterVM.displayFilters = true;
			myGameView.myGameVM.displayDecks = true;
			
		}
		if (myGameView.myGameVM.toReload)
		{
			//applyFilters();
			if (myGameView.sortVM.sortSelected != 10)
			{
				this.sortCards();
			}
			this.displayPage();
			myGameView.myGameVM.toReload = false;
		}
		if (myGameView.myGameVM.destroyAll)
		{
			this.clearCards();
			this.clearDeckCards();
			myGameView.myGameVM.isCreatedDeckCards = false;
			myGameView.myGameVM.toReloadAll = true;
			myGameView.myGameVM.destroyAll = false;
		}
		if (myGameView.myGameVM.toReloadAll)
		{
			myGameView.myGameVM.displayLoader = true;
			myGameView.filterVM.displayFilters = false;
			
			myGameView.myGameVM.areDecksRetrieved = false;
			myGameView.myGameVM.areCreatedDeckCards = false;
			myGameView.myGameVM.isLoadedCards = false;
			myGameView.myGameVM.isLoadedDeck = false;
			
			myGameScript.instance.getCards();
			myGameView.myGameVM.toReloadAll = false;
		}
		if (myGameView.myGameVM.isLoadedCards)
		{
			this.createCards();
			StartCoroutine(myGameScript.instance.retrieveDecks());
			myGameView.myGameVM.isLoadedCards = false;
			myGameView.myGameVM.isCreatedCards = true;
		}
		if (myGameView.myGameVM.areDecksRetrieved && myGameView.myGameVM.isCreatedCards)
		{
			StartCoroutine(myGameScript.instance.retrieveCardsFromDeck(myGameView.myDecksVM.chosenIdDeck));
			myGameView.myGameVM.areDecksRetrieved = false;
		}
		if (myGameView.myGameVM.isLoadedDeck)
		{
			if (myGameView.myGameVM.isCreatedDeckCards)
			{
				displayDeckCards();
			} else
			{
				this.createDeckCards();
				myGameView.myGameVM.isCreatedDeckCards = true;
			}
			//applyFilters();
			this.displayPage();
			myGameView.myGameVM.displayDecks = true;
			myGameView.myGameVM.isLoadedDeck = false;
			myGameView.myGameVM.displayLoader = false;
			myGameView.filterVM.displayFilters = true;
		}		

		if (myGameView.sortVM.oldSortSelected != myGameView.sortVM.sortSelected)
		{
			if (myGameView.sortVM.oldSortSelected != 10)
			{
				myGameView.sortVM.sortButtonStyle [myGameView.sortVM.oldSortSelected] = myGameView.sortVM.sortDefaultButtonStyle;
			}
			myGameView.sortVM.sortButtonStyle [myGameView.sortVM.sortSelected] = myGameView.sortVM.sortActivatedButtonStyle;
			myGameView.sortVM.oldSortSelected = myGameView.sortVM.sortSelected;
		}
		if (!myGameView.myGameVM.isBeingDragged)
		{
			bool isMoved = false;
			myGameView.filterVM.maxLifeVal = Mathf.RoundToInt(myGameView.filterVM.maxLifeVal);
			myGameView.filterVM.minLifeVal = Mathf.RoundToInt(myGameView.filterVM.minLifeVal);
			myGameView.filterVM.maxAttackVal = Mathf.RoundToInt(myGameView.filterVM.maxAttackVal);
			myGameView.filterVM.minAttackVal = Mathf.RoundToInt(myGameView.filterVM.minAttackVal);
			myGameView.filterVM.maxMoveVal = Mathf.RoundToInt(myGameView.filterVM.maxMoveVal);
			myGameView.filterVM.minMoveVal = Mathf.RoundToInt(myGameView.filterVM.minMoveVal);
			myGameView.filterVM.maxQuicknessVal = Mathf.RoundToInt(myGameView.filterVM.maxQuicknessVal);
			myGameView.filterVM.minQuicknessVal = Mathf.RoundToInt(myGameView.filterVM.minQuicknessVal);
			
			if (myGameView.filterVM.oldMaxLifeVal != myGameView.filterVM.maxLifeVal)
			{
				myGameView.filterVM.oldMaxLifeVal = myGameView.filterVM.maxLifeVal;
				isMoved = true; 
			}
			if (myGameView.filterVM.oldMinLifeVal != myGameView.filterVM.minLifeVal)
			{
				myGameView.filterVM.oldMinLifeVal = myGameView.filterVM.minLifeVal;
				isMoved = true; 
			}
			if (myGameView.filterVM.oldMaxAttackVal != myGameView.filterVM.maxAttackVal)
			{
				myGameView.filterVM.oldMaxAttackVal = myGameView.filterVM.maxAttackVal;
				isMoved = true; 
			}
			if (myGameView.filterVM.oldMinAttackVal != myGameView.filterVM.minAttackVal)
			{
				myGameView.filterVM.oldMinAttackVal = myGameView.filterVM.minAttackVal;
				isMoved = true; 
			}
			if (myGameView.filterVM.oldMaxMoveVal != myGameView.filterVM.maxMoveVal)
			{
				myGameView.filterVM.oldMaxMoveVal = Mathf.RoundToInt(myGameView.filterVM.maxMoveVal);
				isMoved = true; 
			}
			if (myGameView.filterVM.oldMinMoveVal != myGameView.filterVM.minMoveVal)
			{
				myGameView.filterVM.oldMinMoveVal = Mathf.RoundToInt(myGameView.filterVM.minMoveVal);
				isMoved = true; 
			}
			if (myGameView.filterVM.oldMaxQuicknessVal != myGameView.filterVM.maxQuicknessVal)
			{
				myGameView.filterVM.oldMaxQuicknessVal = myGameView.filterVM.maxQuicknessVal;
				isMoved = true; 
			}
			if (myGameView.filterVM.oldMinQuicknessVal != myGameView.filterVM.minQuicknessVal)
			{
				myGameView.filterVM.oldMinQuicknessVal = myGameView.filterVM.minQuicknessVal;
				isMoved = true; 
			}
			if (isMoved)
			{
				myGameView.myGameVM.toReload = true;
			}
		}
	}

	public void initViewModels()
	{
		myGameView.filterVM = new FilterViewModel();
		myGameView.popupVM = new MyGamePopUpViewModel();
		myGameView.myDecksVM = new MyDecksViewModel();
		myGameView.sortVM = new SortViewModel();
		myGameView.focusVM = new FocusViewModel();
		myGameView.paginationVM = new PaginationViewModel();
		myGameView.myGameVM = new MyGameViewModel();
	}

	public void matchValuesInit()
	{
		myGameView.filterVM.matchValues = new List<string>();
	}

	public void toReload()
	{
		myGameView.myGameVM.toReload = true;
	}

	public void changeDeck(int i)
	{
		myGameView.myDecksVM.myDecksGuiStyle [myGameView.myDecksVM.chosenDeck] = myGameView.myDecksVM.deckStyle;
		myGameView.myDecksVM.myDecksButtonGuiStyle [myGameView.myDecksVM.chosenDeck] = myGameView.myDecksVM.deckButtonStyle;
		myGameView.myDecksVM.chosenDeck = i;
		myGameView.myDecksVM.myDecksGuiStyle [i] = myGameView.myDecksVM.deckChosenStyle;
		myGameView.myDecksVM.myDecksButtonGuiStyle [i] = myGameView.myDecksVM.deckButtonChosenStyle;
		myGameView.myDecksVM.chosenIdDeck = myGameView.myGameVM.myDecks [i].Id;
		StartCoroutine(myGameScript.instance.retrieveCardsFromDeck(myGameView.myDecksVM.chosenIdDeck));
	}

	public void changePage(int i)
	{
		myGameView.paginationVM.paginatorGuiStyle [myGameView.paginationVM.chosenPage] = myGameView.paginationVM.paginationStyle;
		myGameView.paginationVM.chosenPage = i;
		myGameView.paginationVM.paginatorGuiStyle [i] = myGameView.paginationVM.paginationActivatedStyle;
		displayPage();
	}

	public void changeSetPages(bool flag)
	{

		if (flag)
		{
			myGameView.paginationVM.pageDebut = myGameView.paginationVM.pageDebut - 15;
			myGameView.paginationVM.pageFin = myGameView.paginationVM.pageDebut + 15;
		} else
		{
			myGameView.paginationVM.pageDebut = myGameView.paginationVM.pageDebut + 15;
			myGameView.paginationVM.pageFin = Mathf.Min(myGameView.paginationVM.pageFin + 15, myGameView.paginationVM.nbPages);
		}
	}

	public void changeToggleStates(int i, bool toggle)
	{
		myGameView.myGameVM.togglesCurrentStates [i] = toggle;
	}
	
	#region style
	
	public void initStyles()
	{
		myGameView.filterVM.styles = new GUIStyle[FilterVMStyle.Length];
		for (int i = 0; i < FilterVMStyle.Length; i++)
		{
			myGameView.filterVM.styles [i] = FilterVMStyle [i];
		}
		myGameView.filterVM.initStyles();

		myGameView.popupVM.styles = new GUIStyle[PopUpVMStyle.Length];
		for (int i = 0; i < PopUpVMStyle.Length; i++)
		{
			myGameView.popupVM.styles [i] = PopUpVMStyle [i];
		}
		myGameView.popupVM.initStyles();

		myGameView.myDecksVM.styles = new GUIStyle[MydecksVMStyle.Length];
		for (int i = 0; i < MydecksVMStyle.Length; i++)
		{
			myGameView.myDecksVM.styles [i] = MydecksVMStyle [i];
		}
		myGameView.myDecksVM.initStyles();

		myGameView.sortVM.styles = new GUIStyle[SortVMStyle.Length];
		for (int i = 0; i < SortVMStyle.Length; i++)
		{
			myGameView.sortVM.styles [i] = SortVMStyle [i];
		}
		myGameView.sortVM.initStyles();

		myGameView.focusVM.styles = new GUIStyle[FocusVMStyle.Length];
		for (int i = 0; i < FocusVMStyle.Length; i++)
		{
			myGameView.focusVM.styles [i] = FocusVMStyle [i];
		}
		myGameView.focusVM.initStyles();

		myGameView.paginationVM.styles = new GUIStyle[PaginationVMStyle.Length];
		for (int i = 0; i < PaginationVMStyle.Length; i++)
		{
			myGameView.paginationVM.styles [i] = PaginationVMStyle [i];
		}
		myGameView.paginationVM.initStyles();

		myGameView.myDecksVM.textures = new Texture2D[MyDecksVMTexture.Length];
		for (int i = 0; i < MyDecksVMTexture.Length; i++)
		{
			myGameView.myDecksVM.textures [i] = MyDecksVMTexture [i];
		}
		myGameView.myDecksVM.initTextures();

		myGameView.myGameVM.textures = new Texture2D[MyGameVMTexture.Length];
		for (int i = 0; i < MyGameVMTexture.Length; i++)
		{
			myGameView.myGameVM.textures [i] = MyGameVMTexture [i];
		}
		myGameView.myGameVM.initTextures();

		myGameView.myGameVM.toReloadAll = true;
	}
	public void setStyles()
	{	
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		
		if (heightScreen < widthScreen)
		{
			myGameView.myDecksVM.decksTitleStyle.fontSize = heightScreen * 2 / 100;
			myGameView.myDecksVM.decksTitleStyle.fixedHeight = (int)heightScreen * 3 / 100;
			myGameView.myDecksVM.decksTitleStyle.fixedWidth = (int)widthScreen * 9 / 100;
			myGameView.myDecksVM.decksTitle = "Mes decks";
			
			myGameView.myDecksVM.myNewDeckButton.fontSize = heightScreen * 2 / 100;
			myGameView.myDecksVM.myNewDeckButton.fixedHeight = heightScreen * 3 / 100;
			myGameView.myDecksVM.myNewDeckButton.fixedWidth = widthScreen * 9 / 100;
			myGameView.myDecksVM.myNewDeckButton.normal.background = myGameView.myGameVM.backButton;
			myGameView.myDecksVM.myNewDeckButton.hover.background = myGameView.myGameVM.backActivatedButton;
			myGameView.myDecksVM.myNewDeckButtonTitle = "Nouveau";
			
			myGameView.popupVM.centralWindow = new Rect(widthScreen * 0.25f, 0.12f * heightScreen, widthScreen * 0.50f, 0.18f * heightScreen);
			
			myGameView.popupVM.centralWindowStyle.fixedWidth = widthScreen * 0.5f - 5;
			
			myGameView.popupVM.centralWindowTitleStyle.fontSize = heightScreen * 2 / 100;
			myGameView.popupVM.centralWindowTitleStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.popupVM.centralWindowTitleStyle.fixedWidth = widthScreen * 5 / 10;
			
			myGameView.popupVM.centralWindowTextFieldStyle.fontSize = heightScreen * 2 / 100;
			myGameView.popupVM.centralWindowTextFieldStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.popupVM.centralWindowTextFieldStyle.fixedWidth = widthScreen * 4 / 10;
			
			myGameView.popupVM.centralWindowButtonStyle.fontSize = heightScreen * 2 / 100;
			myGameView.popupVM.centralWindowButtonStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.popupVM.centralWindowButtonStyle.fixedWidth = widthScreen * 2 / 10;
			
			myGameView.popupVM.smallCentralWindowButtonStyle.fontSize = heightScreen * 15 / 1000;
			myGameView.popupVM.smallCentralWindowButtonStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.popupVM.smallCentralWindowButtonStyle.fixedWidth = widthScreen * 1 / 10;
			
			myGameView.myDecksVM.rectDeck = new Rect(widthScreen * 0.005f, 0.105f * heightScreen, widthScreen * 0.19f, 0.21f * heightScreen);
			myGameView.myDecksVM.rectInsideScrollDeck = new Rect(widthScreen * 0.005f, 0.12f * heightScreen, widthScreen * 0.18f, 0.18f * heightScreen);
			myGameView.myDecksVM.rectOutsideScrollDeck = new Rect(widthScreen * 0.005f, 0.12f * heightScreen, widthScreen * 0.19f, 0.18f * heightScreen);
			
			myGameView.myDecksVM.deckStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.myDecksVM.deckStyle.fixedWidth = widthScreen * 17 / 100;
			
			myGameView.myDecksVM.deckChosenStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.myDecksVM.deckChosenStyle.fixedWidth = widthScreen * 17 / 100;
			
			myGameView.myDecksVM.deckButtonStyle.fontSize = heightScreen * 2 / 100;
			myGameView.myDecksVM.deckButtonStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.myDecksVM.deckButtonStyle.fixedWidth = widthScreen * 12 / 100;
			
			myGameView.myDecksVM.deckButtonChosenStyle.fontSize = heightScreen * 2 / 100;
			myGameView.myDecksVM.deckButtonChosenStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.myDecksVM.deckButtonChosenStyle.fixedWidth = widthScreen * 12 / 100;
			
			myGameView.myDecksVM.myEditButtonStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.myDecksVM.myEditButtonStyle.fixedWidth = heightScreen * 3 / 100;
			
			myGameView.myDecksVM.mySuppressButtonStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.myDecksVM.mySuppressButtonStyle.fixedWidth = heightScreen * 3 / 100;
			
			myGameView.paginationVM.paginationStyle.fontSize = heightScreen * 2 / 100;
			myGameView.paginationVM.paginationStyle.fixedWidth = widthScreen * 3 / 100;
			myGameView.paginationVM.paginationStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.paginationVM.paginationActivatedStyle.fontSize = heightScreen * 2 / 100;
			myGameView.paginationVM.paginationActivatedStyle.fixedWidth = widthScreen * 3 / 100;
			myGameView.paginationVM.paginationActivatedStyle.fixedHeight = heightScreen * 3 / 100;
			
			myGameView.filterVM.filterTitleStyle.fixedWidth = widthScreen * 19 / 100;
			myGameView.filterVM.filterTitleStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.filterVM.filterTitleStyle.fontSize = heightScreen * 2 / 100;
			
			myGameView.filterVM.toggleStyle.fixedWidth = widthScreen * 19 / 100;
			myGameView.filterVM.toggleStyle.fixedHeight = heightScreen * 20 / 1000;
			myGameView.filterVM.toggleStyle.fontSize = heightScreen * 15 / 1000;
			
			myGameView.filterVM.filterTextFieldStyle.fontSize = heightScreen * 2 / 100;
			myGameView.filterVM.filterTextFieldStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.filterVM.filterTextFieldStyle.fixedWidth = widthScreen * 19 / 100;
			
			myGameView.filterVM.myStyle.fontSize = heightScreen * 15 / 1000;
			myGameView.filterVM.myStyle.fixedHeight = heightScreen * 20 / 1000;
			myGameView.filterVM.myStyle.fixedWidth = widthScreen * 19 / 100;
			
			myGameView.filterVM.smallPoliceStyle.fontSize = heightScreen * 15 / 1000;
			myGameView.filterVM.smallPoliceStyle.fixedHeight = heightScreen * 20 / 1000;
			
			myGameView.focusVM.focusButtonStyle.fontSize = heightScreen * 2 / 100;
			myGameView.focusVM.focusButtonStyle.fixedHeight = heightScreen * 6 / 100;
			myGameView.focusVM.focusButtonStyle.fixedWidth = widthScreen * 25 / 100;
			
			myGameView.focusVM.cantBuyStyle.fontSize = heightScreen * 2 / 100;
			myGameView.focusVM.cantBuyStyle.fixedHeight = heightScreen * 6 / 100;
			myGameView.focusVM.cantBuyStyle.fixedWidth = widthScreen * 25 / 100;
			
			// Style utilisé pour les bouttons de tri
			
			myGameView.sortVM.sortDefaultButtonStyle.fontSize = heightScreen * 2 / 100;
			myGameView.sortVM.sortDefaultButtonStyle.fixedHeight = (int)heightScreen * 3 / 100;
			myGameView.sortVM.sortDefaultButtonStyle.fixedWidth = (int)widthScreen * 12 / 1000;
			
			myGameView.sortVM.sortActivatedButtonStyle.fontSize = heightScreen * 2 / 100;
			myGameView.sortVM.sortActivatedButtonStyle.fixedHeight = (int)heightScreen * 3 / 100;
			myGameView.sortVM.sortActivatedButtonStyle.fixedWidth = (int)widthScreen * 12 / 1000;
			
			for (int i = 0; i < 10; i++)
			{
				if (myGameView.sortVM.sortSelected == 10)
				{
					myGameView.sortVM.sortButtonStyle [i] = myGameView.sortVM.sortDefaultButtonStyle;
				}
			}
		} else
		{
			myGameView.myDecksVM.decksTitleStyle.fontSize = heightScreen * 2 / 100;
			myGameView.myDecksVM.decksTitleStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.myDecksVM.decksTitleStyle.fixedWidth = widthScreen * 12 / 100;
			myGameView.myDecksVM.decksTitle = "Decks";
			
			myGameView.myDecksVM.myNewDeckButton.fontSize = heightScreen * 2 / 100;
			myGameView.myDecksVM.myNewDeckButton.fixedHeight = heightScreen * 3 / 100;
			myGameView.myDecksVM.myNewDeckButton.fixedWidth = heightScreen * 3 / 100;
			myGameView.myDecksVM.myNewDeckButton.normal.background = myGameView.myDecksVM.backNewDeckButton;
			myGameView.myDecksVM.myNewDeckButton.hover.background = myGameView.myDecksVM.backHoveredNewDeckButton;
			myGameView.myDecksVM.myNewDeckButtonTitle = "";
			
			myGameView.popupVM.centralWindow = new Rect(widthScreen * 0.10f, 0.10f * heightScreen, widthScreen * 0.80f, 0.80f * heightScreen);
			myGameView.popupVM.centralWindowTitleStyle.fontSize = heightScreen * 2 / 100;
			myGameView.popupVM.centralWindowTitleStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.popupVM.centralWindowTitleStyle.fixedWidth = widthScreen * 5 / 10;
			
			myGameView.popupVM.centralWindowTextFieldStyle.fontSize = heightScreen * 1 / 100;
			myGameView.popupVM.centralWindowTextFieldStyle.fixedHeight = heightScreen * 3 / 100;
			myGameView.popupVM.centralWindowTextFieldStyle.fixedWidth = widthScreen * 7 / 10;
		}
	}
	#endregion style

	#region click events

	public void clickedCard(GameObject go)
	{
		if (go.name.StartsWith("DeckCard"))
		{
			myGameScript.instance.onLeftClickOnDeckCard(go);
		} else if (go.name.StartsWith("Card"))
		{
			myGameScript.instance.onLeftClickOnCard(go);
		}
	}
	public void onLeftClickOnDeckCard(GameObject go)
	{
		myGameScript.instance.RemoveCardFromDeck(myGameView.myDecksVM.chosenIdDeck, 
		                                         myGameView.myGameVM.cards [myGameView.myGameVM.deckCardsIds [System.Convert.ToInt32(go.name.Substring(8))]].Id);
		int tempInt = System.Convert.ToInt32(go.name.Substring(8));
		myGameView.myGameVM.deckCardsIds.RemoveAt(tempInt);
		myGameView.myGameVM.myDecks [myGameView.myDecksVM.chosenDeck].NbCards--;
		displayDeckCards();
		//applyFilters();
		displayPage();
	}

	public void onLeftClickOnCard(GameObject go)
	{
		if (myGameView.myGameVM.deckCardsIds.Count != 5)
		{
			int tempInt = System.Convert.ToInt32(go.name.Substring(4));
			myGameView.myGameVM.deckCardsIds.Add(tempInt);
			myGameView.myGameVM.myDecks [myGameView.myDecksVM.chosenDeck].NbCards++;
			displayDeckCards();
			AddCardToDeck(myGameView.myDecksVM.chosenIdDeck, 
			              myGameView.myGameVM.cards [System.Convert.ToInt32(go.name.Substring(4))].Id);
			//applyFilters();
			displayPage();
		}
	}

	public void rightClickedCard(GameObject go)
	{
		if (go.name.Contains("DeckCard") || go.name.StartsWith("Card"))
		{
			myGameView.myGameVM.displayDecks = false;
			myGameView.filterVM.displayFilters = false;
			if (go.name.Contains("DeckCard"))
			{
				myGameView.focusVM.focusedCard = System.Convert.ToInt32(go.name.Substring(8));
			} else
			{
				myGameView.focusVM.focusedCard = System.Convert.ToInt32(go.name.Substring(4));
			}
			
			int finish = 3 * myGameView.myGameVM.nbCardsPerRow;
			for (int i = 0; i < finish; i++)
			{
				myGameView.myGameVM.displayedCards [i].SetActive(false);
			}
			for (int i = 0; i < myGameView.myGameVM.displayedDeckCards.Length; i++)
			{
				myGameView.myGameVM.displayedDeckCards [i].SetActive(false);
			}
			
			myGameView.myGameVM.cardFocused = Instantiate(myGameScript.instance.CardObject) as GameObject;

			float scale = myGameScript.instance.heightScreen / 120f;
			myGameView.myGameVM.cardFocused.transform.localScale = new Vector3(scale, scale, scale); 
			Vector3 vec = Camera.main.WorldToScreenPoint(myGameView.myGameVM.cardFocused.collider.bounds.size);
			myGameView.myGameVM.cardFocused.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0.50f * myGameScript.instance.widthScreen, 
			                                                                                          0.45f * myGameScript.instance.heightScreen - 1, 
			                                                                                          10)); 
			myGameView.myGameVM.cardFocused.gameObject.name = "FocusedCard";	
			
			if (go.name.Contains("DeckCard"))
			{
				myGameView.myGameVM.idFocused = myGameView.myGameVM.deckCardsIds [myGameView.focusVM.focusedCard];
				
			} else
			{
				myGameView.myGameVM.idFocused = myGameView.focusVM.focusedCard;
			}
			
			myGameView.myGameVM.cardId = myGameView.myGameVM.cards [myGameView.myGameVM.idFocused].Id;


			myGameView.myGameVM.cardFocused.AddComponent<CardMyGameController>();
			myGameView.myGameVM.cardFocused.GetComponent<CardController>().setCard(myGameView.myGameVM.cards [myGameView.myGameVM.idFocused]);
			myGameView.myGameVM.cardFocused.GetComponent<CardController>().setSkills();
			myGameView.myGameVM.cardFocused.GetComponent<CardController>().setExperience();
			myGameView.myGameVM.cardFocused.GetComponent<CardController>().show();
			myGameView.myGameVM.cardFocused.GetComponent<CardMyGameController>().setFocusMyGameFeatures();
			myGameView.myGameVM.cardFocused.GetComponent<CardController>().setCentralWindowRect(centralWindow);
			myGameView.focusVM.focusedCardPrice = myGameView.myGameVM.cards [myGameView.myGameVM.idFocused].getCost();
			
			myGameView.myDecksVM.rectFocus = new Rect(0.50f * myGameScript.instance.widthScreen + (vec.x - myGameScript.instance.widthScreen / 2f) / 2f, 0.15f * myGameScript.instance.heightScreen, 
			                               0.25f * myGameScript.instance.widthScreen, 0.8f * myGameScript.instance.heightScreen);
		}
	
	}

	public void onLeftClick()
	{
		myGameView.myGameVM.isBeingDragged = true;
	}

	public void onRightClick()
	{
		myGameView.myGameVM.isBeingDragged = false;
	}

	#endregion click events


	public void exitCard()
	{
		Destroy(myGameView.myGameVM.cardFocused);
		myGameView.myGameVM.displayDecks = true;
		myGameView.filterVM.displayFilters = true;
		displayPage();
		displayDeckCards();
	}


	#region skill
	public void displaySkills()
	{
		matchValuesInit();
		if (myGameView.valueSkill != "")
		{
			for (int i = 0; i < myGameView.myGameVM.skillsList.Length - 1; i++)
			{  
				if (myGameView.myGameVM.skillsList [i].ToLower().Contains(myGameView.valueSkill))
				{
					myGameView.filterVM.matchValues.Add(myGameView.myGameVM.skillsList [i]);
				}
			}
		}
	}
	#endregion skill
	public void clearCards()
	{
		for (int i = 0; i < 3 * myGameView.myGameVM.nbCardsPerRow; i++)
		{
			Destroy(myGameView.myGameVM.displayedCards [i]);
		}
	}
	
	public void clearDeckCards()
	{
		for (int i = 0; i < 5; i++)
		{
			Destroy(myGameView.myGameVM.displayedDeckCards [i]);
		}
	}

	void initCard(GameObject card, int i)
	{
		card.AddComponent<CardMyGameController>();
		card.GetComponent<CardController>().setCard(myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [i]]);
		card.GetComponent<CardController>().setSkills();
		card.GetComponent<CardController>().setExperience();
		card.GetComponent<CardController>().show();
		card.GetComponent<CardController>().setCentralWindowRect(centralWindow);
	}
	
	public void createCards()
	{	
		float tempF = 10f * widthScreen / heightScreen;
		float width = tempF * 0.78f;
		myGameView.myGameVM.nbCardsPerRow = Mathf.FloorToInt(width / 1.6f);
		float debutLargeur = -0.49f * tempF + 0.8f + (width - 1.6f * myGameView.myGameVM.nbCardsPerRow) / 2;
		myGameView.myGameVM.displayedCards = new GameObject[3 * myGameView.myGameVM.nbCardsPerRow];
		int nbCardsToDisplay = myGameView.myGameVM.cardsToBeDisplayed.Count;
		for (int i = 0; i < 3 * myGameView.myGameVM.nbCardsPerRow; i++)
		{
			GameObject card = myGameView.myGameVM.displayedCards [i] = Instantiate(myGameScript.instance.CardObject) as GameObject;


			myGameView.myGameVM.displayedCards [i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
			myGameView.myGameVM.displayedCards [i].transform.localPosition = new Vector3(debutLargeur + 1.6f * (i % myGameView.myGameVM.nbCardsPerRow), 
			                                                                            0.8f - (i - i % myGameView.myGameVM.nbCardsPerRow) / myGameView.myGameVM.nbCardsPerRow * 2.2f,
			                                                                 0); 
			myGameView.myGameVM.displayedCards [i].gameObject.name = "Card" + i + "";	
			if (i < nbCardsToDisplay)
			{
				initCard(card, i);
				//card.GetComponent<CardMyGameController>().setFocusMyGameFeatures();
			} else
			{
				card.AddComponent<CardMyGameController>();
				myGameView.myGameVM.displayedCards [i].SetActive(false);
			}
		}
		computePagination();
		resize();
		setFilters();
	}

	public void computePagination()
	{
		myGameView.paginationVM.nbPages = Mathf.CeilToInt(myGameView.myGameVM.cardsToBeDisplayed.Count / (3.0f * myGameView.myGameVM.nbCardsPerRow));
		myGameView.paginationVM.pageDebut = 0;
		if (myGameView.paginationVM.nbPages > 15)
		{
			myGameView.paginationVM.pageFin = 14;
		} else
		{
			myGameView.paginationVM.pageFin = myGameView.paginationVM.nbPages;
		}
		myGameView.paginationVM.chosenPage = 0;
		myGameView.paginationVM.paginatorGuiStyle = new GUIStyle[myGameView.paginationVM.nbPages];
		
		for (int i = 0; i < myGameView.paginationVM.nbPages; i++)
		{ 
			if (i == 0)
			{
				myGameView.paginationVM.paginatorGuiStyle [i] = myGameView.paginationVM.paginationActivatedStyle;
			} else
			{
				myGameView.paginationVM.paginatorGuiStyle [i] = myGameView.paginationVM.paginationStyle;
			}
		}
	}
	
	public void createDeckCards()
	{
		float tempF = 10f * widthScreen / heightScreen;
		float width = tempF * 0.6f;
		myGameView.myDecksVM.scaleDeck = Mathf.Min(1.6f, width / 6f);
		float pas = (width - 5f * myGameView.myDecksVM.scaleDeck) / 6f;
		float debutLargeur = -0.3f * tempF + pas + myGameView.myDecksVM.scaleDeck / 2;
		myGameView.myGameVM.displayedDeckCards = new GameObject[5];
		int nbDeckCardsToDisplay = myGameView.myGameVM.deckCardsIds.Count;
		
		for (int i = 0; i < 5; i++)
		{
			GameObject card = myGameView.myGameVM.displayedDeckCards [i] = Instantiate(myGameScript.instance.CardObject) as GameObject;

			card.transform.localScale = new Vector3(myGameView.myDecksVM.scaleDeck, myGameView.myDecksVM.scaleDeck, myGameView.myDecksVM.scaleDeck); 
			card.transform.localPosition = new Vector3(debutLargeur + (myGameView.myDecksVM.scaleDeck + pas) * i, 2.9f, 0); 
			card.gameObject.name = "DeckCard" + i + "";	
			if (i < nbDeckCardsToDisplay)
			{
				initCard(card, i);
				//card.GetComponent<CardMyGameController>().setFocusMyGameFeatures();
			} else
			{
				card.AddComponent<CardMyGameController>();
				myGameView.myGameVM.displayedDeckCards [i].SetActive(false);
			}
		}
		myGameView.myGameVM.areCreatedDeckCards = true; 
	}

	#region call models
	public IEnumerator addDeck(string decksName)
	{
		yield return StartCoroutine(Deck.create(decksName));
		StartCoroutine(retrieveDecks());
	}

	public IEnumerator deleteDeck(int deckId)
	{
		myGameView.myGameVM.areDecksRetrieved = false;
		Deck deck = new Deck(deckId);

		yield return StartCoroutine(deck.delete());
		StartCoroutine(retrieveDecks()); 
	}

	public IEnumerator editDeck(int deckToEdit, string decksName)
	{
		Deck deck = new Deck(deckToEdit);

		yield return StartCoroutine(deck.edit(decksName));
		StartCoroutine(retrieveDecks()); 
	}

	public IEnumerator retrieveDecks()
	{
		myGameView.myGameVM.myDecks = new List<Deck>();
		yield return StartCoroutine(userModel.getDecks(parseDecks));

		myGameView.myGameVM.areDecksRetrieved = true;
	}

	public void getCards()
	{
		myGameView.myGameVM.cardsToBeDisplayed = new List<int>();
		
		StartCoroutine(userModel.getCards(parseCards));
	}

	public IEnumerator retrieveCardsFromDeck(int idDeck)
	{
		Deck deck = new Deck(idDeck);

		yield return StartCoroutine(deck.retrieveCards(parseCardsFromDeck));

		myGameView.myGameVM.isLoadedDeck = true;
	}
	
	public void AddCardToDeck(int idDeck, int idCard)
	{		
		Deck deck = new Deck(idDeck);

		StartCoroutine(deck.addCard(idCard));
	}
	
	public void RemoveCardFromDeck(int idDeck, int idCard)
	{		
		Deck deck = new Deck(idDeck);
		StartCoroutine(deck.removeCard(idCard));
	}
	
	public IEnumerator sellCard(int idCard, int cost)
	{	
		ApplicationModel.credits += cost;
		Card card = new Card(idCard);
		//yield return StartCoroutine(card.sellCard(cost));
		yield return null;
		myGameView.myGameVM.soldCard = true;
	}
	
	public void putOnMarket(int idCard, int price)
	{
		myGameView.myGameVM.cards [myGameView.myGameVM.idFocused].onSale = 1;
		myGameView.myGameVM.cards [myGameView.myGameVM.idFocused].Price = price;
		Card card = new Card(idCard);
		
		StartCoroutine(card.toSell(price));
	}
	
	public void removeFromMarket(int idCard)
	{	
		myGameView.myGameVM.cards [myGameView.myGameVM.idFocused].onSale = 0;
		Card card = new Card(idCard);

		StartCoroutine(card.notToSell());
	}
	
	public void changeMarketPrice(int idCard, int price)
	{	
		myGameView.myGameVM.cards [myGameView.myGameVM.idFocused].Price = price;
		Card card = new Card(idCard);

		StartCoroutine(card.changePriceCard(price));
	}
	
	public void renameCard(int idCard, string newTitle, int renameCost)
	{

		myGameView.myDecksVM.newTitle = myGameView.myDecksVM.newTitle.Replace("\n", "");
		myGameView.myDecksVM.newTitle = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo
			.ToTitleCase(myGameView.myDecksVM.newTitle.ToLower());
		Card card = new Card(idCard);

		StartCoroutine(card.renameCard(newTitle, renameCost));
		
		ApplicationModel.credits -= renameCost;

		myGameView.myGameVM.cards [myGameView.myGameVM.idFocused].Title = myGameView.myDecksVM.newTitle;
		myGameView.GetComponent<GameCard>().Card.Title = myGameView.myDecksVM.newTitle;
		myGameView.myGameVM.cardFocused.GetComponent<GameCard>().ShowFace();
	}
	#endregion call models

	public void parseDecks(string text)
	{
		string[] decksInformation = text.Split('\n'); 
		string[] deckInformation;

		myGameView.myDecksVM.myDecksGuiStyle = new GUIStyle[decksInformation.Length - 1];
		myGameView.myDecksVM.myDecksButtonGuiStyle = new GUIStyle[decksInformation.Length - 1];
		
		for (int i = 0; i < decksInformation.Length - 1; i++) 		// On boucle sur les attributs d'un deck
		{
			if (i > 0)
			{
				myGameView.myDecksVM.myDecksGuiStyle [i] = myGameView.myDecksVM.deckStyle;
				myGameView.myDecksVM.myDecksButtonGuiStyle [i] = myGameView.myDecksVM.deckButtonStyle;
			} else
			{
				myGameView.myDecksVM.myDecksGuiStyle [i] = myGameView.myDecksVM.deckChosenStyle;
				myGameView.myDecksVM.myDecksButtonGuiStyle [i] = myGameView.myDecksVM.deckButtonChosenStyle;
			}
			deckInformation = decksInformation [i].Split('\\');
			myGameView.myGameVM.myDecks.Add(new Deck(System.Convert.ToInt32(deckInformation [0]), deckInformation [1], System.Convert.ToInt32(deckInformation [2])));
			
			if (i == 0)
			{
				myGameView.myDecksVM.chosenIdDeck = System.Convert.ToInt32(deckInformation [0]);
				myGameView.myDecksVM.chosenDeck = 0;
			}
		}
	}

	public void parseCards(string text)
	{
		string[] data = text.Split(new string[] { "END" }, System.StringSplitOptions.None);
		string[] cardsIDS = data [2].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
		string[] skillsIds = data [1].Split('\n');
		
		myGameView.myGameVM.cardTypeList = data [0].Split('\n');
		myGameView.myGameVM.togglesCurrentStates = new bool[myGameView.myGameVM.cardTypeList.Length];
		
		for (int i = 0; i < myGameView.myGameVM.cardTypeList.Length - 1; i++)
		{
			myGameView.myGameVM.togglesCurrentStates [i] = false;
		}
		myGameView.myGameVM.skillsList = new string[skillsIds.Length - 1];
		
		string[] tempString;
		for (int i = 0; i < skillsIds.Length - 1; i++)
		{
			tempString = skillsIds [i].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
			if (i > 0)
			{
				myGameView.myGameVM.skillsList [i - 1] = tempString [0];
			}
		}
		
		myGameView.myGameVM.cards = new List<Card>();
		myGameView.myGameVM.cardsIds = new List<int>();
		
		string[] cardInfo;
		string[] cardInfo2;
		for (int i = 0; i < cardsIDS.Length - 1; i++)
		{
			cardInfo = cardsIDS [i].Split('\n');
			for (int j = 1; j < cardInfo.Length - 1; j++)
			{
				cardInfo2 = cardInfo [j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
				if (j == 1)
				{
					myGameView.myGameVM.cards.Add(
						new Card(System.Convert.ToInt32(cardInfo2 [0]), // id
					         cardInfo2 [1], // title
					         System.Convert.ToInt32(cardInfo2 [2]), // life
					         System.Convert.ToInt32(cardInfo2 [3]), // attack
					         System.Convert.ToInt32(cardInfo2 [4]), // speed
					         System.Convert.ToInt32(cardInfo2 [5]), // move
					         System.Convert.ToInt32(cardInfo2 [6]), // artindex
					         System.Convert.ToInt32(cardInfo2 [7]), // idclass
					         myGameView.myGameVM.cardTypeList [System.Convert.ToInt32(cardInfo2 [7])], // titleclass
					         System.Convert.ToInt32(cardInfo2 [8]), // lifelevel
					         System.Convert.ToInt32(cardInfo2 [9]), // movelevel
					         System.Convert.ToInt32(cardInfo2 [10]),
					         System.Convert.ToInt32(cardInfo2 [11]),
					         System.Convert.ToInt32(cardInfo2 [12]),
					         System.Convert.ToInt32(cardInfo2 [13]),
					         System.Convert.ToInt32(cardInfo2 [14]),
					         System.Convert.ToInt32(cardInfo2 [15]),
					         System.Convert.ToInt32(cardInfo2 [16]))); 
					
					myGameView.myGameVM.cards [i].Skills = new List<Skill>();
					myGameView.myGameVM.cardsIds.Add(System.Convert.ToInt32(cardInfo2 [0]));
					myGameView.myGameVM.cardsToBeDisplayed.Add(i);
				} else
				{
					myGameView.myGameVM.cards [i].Skills.Add(
						new Skill(myGameView.myGameVM.skillsList [System.Convert.ToInt32(cardInfo2 [0])], 
					           System.Convert.ToInt32(cardInfo2 [0]),
					           System.Convert.ToInt32(cardInfo2 [1]),
					           System.Convert.ToInt32(cardInfo2 [2]),
					           System.Convert.ToInt32(cardInfo2 [3]),
					           System.Convert.ToInt32(cardInfo2 [4]),
					           cardInfo2 [5]));
				}
			}
		}
	
		myGameView.myGameVM.isLoadedCards = true;
		myGameView.myGameVM.isDisplayedCards = true;
	}

	public void parseCardsFromDeck(string text)
	{
		int tempInt;
		int tempInt2 = myGameView.myGameVM.cards.Count;
		bool tempBool;
		int j = 0;
		string[] cardDeckEntries = text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
		myGameView.myGameVM.deckCardsIds = new List<int>();
		for (int i = 0; i < cardDeckEntries.Length - 1; i++)
		{
			tempInt = System.Convert.ToInt32(cardDeckEntries [i]);
			tempBool = true; 
			j = 0;
			while (tempBool && j < tempInt2)
			{
				if (myGameView.myGameVM.cards [j].Id == tempInt)
				{
					tempBool = false;
				}
				j++;
			}
			j--;
			myGameView.myGameVM.deckCardsIds.Add(j);
		}
	}
	#region display
	public void displayPage()
	{	
		int start = 3 * myGameView.myGameVM.nbCardsPerRow * myGameView.paginationVM.chosenPage;
		int finish = start + 3 * myGameView.myGameVM.nbCardsPerRow;
		int nbCardsToDisplay = myGameView.myGameVM.cardsToBeDisplayed.Count;
		if (nbCardsToDisplay > 0)
		{
			for (int i = start; i < finish; i++)
			{
				//myGameVM.displayedCards[i].GetComponent<GameCard>().setTextResolution(1f);
				if (i < nbCardsToDisplay)
				{
					GameObject card = myGameView.myGameVM.displayedCards [i - start].gameObject;
					card.name = "Card" + myGameView.myGameVM.cardsToBeDisplayed [i] + "";
					card.SetActive(true);
				} else
				{
					myGameView.myGameVM.displayedCards [i - start].SetActive(false);
				}
			}
		}
	}
	public void displayDeckCards()
	{
		int nbDeckCardsToDisplay = myGameView.myGameVM.deckCardsIds.Count;
		
		for (int i = 0; i < 5; i++)
		{
			if (i < nbDeckCardsToDisplay)
			{
				GameObject card = myGameView.myGameVM.displayedDeckCards [i];
				card.SetActive(true);
				card.GetComponent<CardController>().setCard(myGameView.myGameVM.cards [myGameView.myGameVM.deckCardsIds [i]]);
				card.GetComponent<CardController>().setSkills();
				card.GetComponent<CardController>().setExperience();
				card.GetComponent<CardController>().show();
			} else
			{
				myGameView.myGameVM.displayedDeckCards [i].SetActive(false);
			}
		}
	}
	#endregion display

	#region filter
	public void setFilters()
	{
		myGameView.filterVM.minLifeLimit = 10000;
		myGameView.filterVM.maxLifeLimit = 0;
		myGameView.filterVM.minAttackLimit = 10000;
		myGameView.filterVM.maxAttackLimit = 0;
		myGameView.filterVM.minMoveLimit = 10000;
		myGameView.filterVM.maxMoveLimit = 0;
		myGameView.filterVM.minQuicknessLimit = 10000;
		myGameView.filterVM.maxQuicknessLimit = 0;
		
		int max = myGameView.myGameVM.cards.Count;
		for (int i = 0; i < max; i++)
		{
			if (myGameView.myGameVM.cards [i].Life < myGameView.filterVM.minLifeLimit)
			{
				myGameView.filterVM.minLifeLimit = myGameView.myGameVM.cards [i].Life;
			}
			if (myGameView.myGameVM.cards [i].Life > myGameView.filterVM.maxLifeLimit)
			{
				myGameView.filterVM.maxLifeLimit = myGameView.myGameVM.cards [i].Life;
			}
			if (myGameView.myGameVM.cards [i].Attack < myGameView.filterVM.minAttackLimit)
			{
				myGameView.filterVM.minAttackLimit = myGameView.myGameVM.cards [i].Attack;
			}
			if (myGameView.myGameVM.cards [i].Attack > myGameView.filterVM.maxAttackLimit)
			{
				myGameView.filterVM.maxAttackLimit = myGameView.myGameVM.cards [i].Attack;
			}
			if (myGameView.myGameVM.cards [i].Move < myGameView.filterVM.minMoveLimit)
			{
				myGameView.filterVM.minMoveLimit = myGameView.myGameVM.cards [i].Move;
			}
			if (myGameView.myGameVM.cards [i].Move > myGameView.filterVM.maxMoveLimit)
			{
				myGameView.filterVM.maxMoveLimit = myGameView.myGameVM.cards [i].Move;
			}
			if (myGameView.myGameVM.cards [i].Speed < myGameView.filterVM.minQuicknessLimit)
			{
				myGameView.filterVM.minQuicknessLimit = myGameView.myGameVM.cards [i].Speed;
			}
			if (myGameView.myGameVM.cards [i].Speed > myGameView.filterVM.maxQuicknessLimit)
			{
				myGameView.filterVM.maxQuicknessLimit = myGameView.myGameVM.cards [i].Speed;
			}
		}
		myGameView.filterVM.minLifeVal = myGameView.filterVM.minLifeLimit;
		myGameView.filterVM.maxLifeVal = myGameView.filterVM.maxLifeLimit;
		myGameView.filterVM.minAttackVal = myGameView.filterVM.minAttackLimit;
		myGameView.filterVM.maxAttackVal = myGameView.filterVM.maxAttackLimit;
		myGameView.filterVM.minMoveVal = myGameView.filterVM.minMoveLimit;
		myGameView.filterVM.maxMoveVal = myGameView.filterVM.maxMoveLimit;
		myGameView.filterVM.minQuicknessVal = myGameView.filterVM.minQuicknessLimit;
		myGameView.filterVM.maxQuicknessVal = myGameView.filterVM.maxQuicknessLimit;
	}

	public void applyFilters()
	{
		myGameView.myGameVM.cardsToBeDisplayed = new List<int>();
		IList<int> tempCardsToBeDisplayed = new List<int>();
		int nbFilters = myGameView.filterVM.filtersCardType.Count;
		bool testFilters = false;
		bool testDeck = false;
		bool test;		
		bool minLifeBool = (myGameView.filterVM.minLifeLimit == myGameView.filterVM.minLifeVal);
		bool maxLifeBool = (myGameView.filterVM.maxLifeLimit == myGameView.filterVM.maxLifeVal);
		bool minMoveBool = (myGameView.filterVM.minMoveLimit == myGameView.filterVM.minMoveVal);
		bool maxMoveBool = (myGameView.filterVM.maxMoveLimit == myGameView.filterVM.maxMoveVal);
		bool minQuicknessBool = (myGameView.filterVM.minQuicknessLimit == myGameView.filterVM.minQuicknessVal);
		bool maxQuicknessBool = (myGameView.filterVM.maxQuicknessLimit == myGameView.filterVM.maxQuicknessVal);
		bool minAttackBool = (myGameView.filterVM.minAttackLimit == myGameView.filterVM.minAttackVal);
		bool maxAttackBool = (myGameView.filterVM.maxAttackLimit == myGameView.filterVM.maxAttackVal);
		
		if (myGameView.isSkillChosen)
		{
			int max = myGameView.myGameVM.cards.Count;
			if (nbFilters == 0)
			{
				max = myGameView.myGameVM.cards.Count;
				if (myGameView.enVente)
				{
					for (int i = 0; i < max; i++)
					{
						if (myGameView.myGameVM.cards [i].hasSkill(myGameView.valueSkill) && myGameView.myGameVM.cards [i].onSale == 1)
						{
							testDeck = false;
							for (int j = 0; j < myGameView.myGameVM.deckCardsIds.Count; j++)
							{
								if (i == myGameView.myGameVM.deckCardsIds [j])
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
						if (myGameView.myGameVM.cards [i].hasSkill(myGameView.valueSkill))
						{
							testDeck = false;
							for (int j = 0; j < myGameView.myGameVM.deckCardsIds.Count; j++)
							{
								if (i == myGameView.myGameVM.deckCardsIds [j])
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
					if (myGameView.enVente)
					{
						while (!test && j != nbFilters)
						{
							if (myGameView.myGameVM.cards [i].IdClass == myGameView.filterVM.filtersCardType [j])
							{
								test = true;
								if (myGameView.myGameVM.cards [i].hasSkill(myGameView.valueSkill) && myGameView.myGameVM.cards [i].onSale == 1)
								{
									testDeck = false;
									for (int k = 0; k < myGameView.myGameVM.deckCardsIds.Count; k++)
									{
										if (i == myGameView.myGameVM.deckCardsIds [k])
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
							if (myGameView.myGameVM.cards [i].IdClass == myGameView.filterVM.filtersCardType [j])
							{
								test = true;
								if (myGameView.myGameVM.cards [i].hasSkill(myGameView.valueSkill))
								{
									testDeck = false;
									for (int k = 0; k < myGameView.myGameVM.deckCardsIds.Count; k++)
									{
										if (i == myGameView.myGameVM.deckCardsIds [k])
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
		} else
		{
			int max = myGameView.myGameVM.cards.Count;
			if (nbFilters == 0)
			{
				if (myGameView.enVente)
				{
					for (int i = 0; i < max; i++)
					{
						if (myGameView.myGameVM.cards [i].onSale == 1)
						{
							testDeck = false;
							for (int j = 0; j < myGameView.myGameVM.deckCardsIds.Count; j++)
							{
								if (i == myGameView.myGameVM.deckCardsIds [j])
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
						testDeck = false;
						for (int j = 0; j < myGameView.myGameVM.deckCardsIds.Count; j++)
						{
							if (i == myGameView.myGameVM.deckCardsIds [j])
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
				if (myGameView.enVente)
				{
					for (int i = 0; i < max; i++)
					{
						test = false;
						int j = 0;
						while (!test && j != nbFilters)
						{
							if (myGameView.myGameVM.cards [i].IdClass == myGameView.filterVM.filtersCardType [j])
							{
								if (myGameView.myGameVM.cards [i].onSale == 1)
								{
									test = true;
									testDeck = false;
									for (int k = 0; k < myGameView.myGameVM.deckCardsIds.Count; k++)
									{
										if (i == myGameView.myGameVM.deckCardsIds [k])
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
							if (myGameView.myGameVM.cards [i].IdClass == myGameView.filterVM.filtersCardType [j])
							{
								test = true;
								testDeck = false;
								for (int k = 0; k < myGameView.myGameVM.deckCardsIds.Count; k++)
								{
									if (i == myGameView.myGameVM.deckCardsIds [k])
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
		if (tempCardsToBeDisplayed.Count > 0)
		{
			myGameView.filterVM.minLifeLimit = 10000;
			myGameView.filterVM.maxLifeLimit = 0;
			myGameView.filterVM.minAttackLimit = 10000;
			myGameView.filterVM.maxAttackLimit = 0;
			myGameView.filterVM.minMoveLimit = 10000;
			myGameView.filterVM.maxMoveLimit = 0;
			myGameView.filterVM.minQuicknessLimit = 10000;
			myGameView.filterVM.maxQuicknessLimit = 0;
			
			for (int i = 0; i < tempCardsToBeDisplayed.Count; i++)
			{
				if (myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Life < myGameView.filterVM.minLifeLimit)
				{
					myGameView.filterVM.minLifeLimit = myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Life;
				}
				if (myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Life > myGameView.filterVM.maxLifeLimit)
				{
					myGameView.filterVM.maxLifeLimit = myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Life;
				}
				if (myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Attack < myGameView.filterVM.minAttackLimit)
				{
					myGameView.filterVM.minAttackLimit = myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Attack;
				}
				if (myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Attack > myGameView.filterVM.maxAttackLimit)
				{
					myGameView.filterVM.maxAttackLimit = myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Attack;
				}
				if (myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Move < myGameView.filterVM.minMoveLimit)
				{
					myGameView.filterVM.minMoveLimit = myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Move;
				}
				if (myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Move > myGameView.filterVM.maxMoveLimit)
				{
					myGameView.filterVM.maxMoveLimit = myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Move;
				}
				if (myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Speed < myGameView.filterVM.minQuicknessLimit)
				{
					myGameView.filterVM.minQuicknessLimit = myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Speed;
				}
				if (myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Speed > myGameView.filterVM.maxQuicknessLimit)
				{
					myGameView.filterVM.maxQuicknessLimit = myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].Speed;
				}
			}
			if (minLifeBool && myGameView.filterVM.maxLifeVal > myGameView.filterVM.minLifeLimit)
			{
				myGameView.filterVM.minLifeVal = myGameView.filterVM.minLifeLimit;
			} else
			{
				if (myGameView.filterVM.minLifeVal < myGameView.filterVM.minLifeLimit)
				{
					myGameView.filterVM.minLifeLimit = myGameView.filterVM.minLifeVal;
				}
			}
			if (maxLifeBool && myGameView.filterVM.minLifeVal < myGameView.filterVM.maxLifeLimit)
			{
				myGameView.filterVM.maxLifeVal = myGameView.filterVM.maxLifeLimit;
				print("Max " + myGameView.filterVM.maxLifeVal);
			} else
			{
				if (myGameView.filterVM.maxLifeVal > myGameView.filterVM.maxLifeLimit)
				{
					myGameView.filterVM.maxLifeLimit = myGameView.filterVM.maxLifeVal;
				}
				print("Max2 " + myGameView.filterVM.maxLifeVal);
			}
			if (minAttackBool && myGameView.filterVM.maxAttackVal > myGameView.filterVM.minAttackLimit)
			{
				myGameView.filterVM.minAttackVal = myGameView.filterVM.minAttackLimit;
			} else
			{
				if (myGameView.filterVM.minAttackVal < myGameView.filterVM.minAttackLimit)
				{
					myGameView.filterVM.minAttackLimit = myGameView.filterVM.minAttackVal;
				}
			}
			if (maxAttackBool && myGameView.filterVM.minAttackVal < myGameView.filterVM.maxAttackLimit)
			{
				myGameView.filterVM.maxAttackVal = myGameView.filterVM.maxAttackLimit;
			} else
			{
				if (myGameView.filterVM.maxAttackVal > myGameView.filterVM.maxAttackLimit)
				{
					myGameView.filterVM.maxAttackLimit = myGameView.filterVM.maxAttackVal;
				}
			}
			if (minMoveBool && myGameView.filterVM.maxMoveVal > myGameView.filterVM.minMoveLimit)
			{
				myGameView.filterVM.minMoveVal = myGameView.filterVM.minMoveLimit;
			} else
			{
				if (myGameView.filterVM.minMoveVal < myGameView.filterVM.minMoveLimit)
				{
					myGameView.filterVM.minMoveLimit = myGameView.filterVM.minMoveVal;
				}
			}
			if (maxMoveBool && myGameView.filterVM.minMoveVal < myGameView.filterVM.maxMoveLimit)
			{
				myGameView.filterVM.maxMoveVal = myGameView.filterVM.maxMoveLimit;
			} else
			{
				if (myGameView.filterVM.maxMoveVal > myGameView.filterVM.maxMoveLimit)
				{
					myGameView.filterVM.maxMoveLimit = myGameView.filterVM.maxMoveVal;
				}
			}
			if (minQuicknessBool && myGameView.filterVM.maxQuicknessVal > myGameView.filterVM.minQuicknessLimit)
			{
				myGameView.filterVM.minQuicknessVal = myGameView.filterVM.minQuicknessLimit;
			} else
			{
				if (myGameView.filterVM.minQuicknessVal < myGameView.filterVM.minQuicknessLimit)
				{
					myGameView.filterVM.minQuicknessLimit = myGameView.filterVM.minQuicknessVal;
				}
			}
			if (maxQuicknessBool && myGameView.filterVM.minQuicknessVal < myGameView.filterVM.maxQuicknessLimit)
			{
				myGameView.filterVM.maxQuicknessVal = myGameView.filterVM.maxQuicknessLimit;
			} else
			{
				if (myGameView.filterVM.maxQuicknessVal > myGameView.filterVM.maxQuicknessLimit)
				{
					myGameView.filterVM.maxQuicknessLimit = myGameView.filterVM.maxQuicknessVal;
				}
			}
			myGameView.filterVM.oldMinLifeVal = myGameView.filterVM.minLifeVal;
			myGameView.filterVM.oldMaxLifeVal = myGameView.filterVM.maxLifeVal;
			myGameView.filterVM.oldMinQuicknessVal = myGameView.filterVM.minQuicknessVal;
			myGameView.filterVM.oldMaxQuicknessVal = myGameView.filterVM.maxQuicknessVal;
			myGameView.filterVM.oldMinMoveVal = myGameView.filterVM.minMoveVal;
			myGameView.filterVM.oldMaxMoveVal = myGameView.filterVM.maxMoveVal;
			myGameView.filterVM.oldMinAttackVal = myGameView.filterVM.minAttackVal;
			myGameView.filterVM.oldMaxAttackVal = myGameView.filterVM.maxAttackVal;
		}
		
		if (myGameView.filterVM.minLifeVal != myGameView.filterVM.minLifeLimit)
		{
			testFilters = true;
		} else if (myGameView.filterVM.maxLifeVal != myGameView.filterVM.maxLifeLimit)
		{
			testFilters = true;
		} else if (myGameView.filterVM.minAttackVal != myGameView.filterVM.minAttackLimit)
		{
			testFilters = true;
		} else if (myGameView.filterVM.maxAttackVal != myGameView.filterVM.maxAttackLimit)
		{
			testFilters = true;
		} else if (myGameView.filterVM.minMoveVal != myGameView.filterVM.minMoveLimit)
		{
			testFilters = true;
		} else if (myGameView.filterVM.maxMoveVal != myGameView.filterVM.maxMoveLimit)
		{
			testFilters = true;
		} else if (myGameView.filterVM.minQuicknessVal != myGameView.filterVM.minQuicknessLimit)
		{
			testFilters = true;
		} else if (myGameView.filterVM.maxQuicknessVal != myGameView.filterVM.maxQuicknessLimit)
		{
			testFilters = true;
		}
		
		if (testFilters == true)
		{
			for (int i = 0; i < tempCardsToBeDisplayed.Count; i++)
			{
				if (myGameView.myGameVM.cards [tempCardsToBeDisplayed [i]].verifyC(myGameView.filterVM.minLifeVal, 
				                                                                 myGameView.filterVM.maxLifeVal, 
				                                                                 myGameView.filterVM.minAttackVal, 
				                                                                 myGameView.filterVM.maxAttackVal, 
				                                                                 myGameView.filterVM.minMoveVal, 
				                                                                 myGameView.filterVM.maxMoveVal, 
				                                                                 myGameView.filterVM.minQuicknessVal, 
				                                                                 myGameView.filterVM.maxQuicknessVal))
				{
					myGameView.myGameVM.cardsToBeDisplayed.Add(tempCardsToBeDisplayed [i]);
				}
			}
		} else
		{
			for (int i = 0; i < tempCardsToBeDisplayed.Count; i++)
			{
				myGameView.myGameVM.cardsToBeDisplayed.Add(tempCardsToBeDisplayed [i]);
			}
		}
		
		computePagination();
	}
	#endregion filter

	#region sort
	public void changeSort(int i)
	{
		myGameView.sortVM.sortSelected = i;
		myGameView.myGameVM.toReload = true;
	}

	public void sortCards()
	{	
		int tempA = new int();
		int tempB = new int();
		
		for (int i = 1; i < myGameView.myGameVM.cardsToBeDisplayed.Count; i++)
		{	
			for (int j = 0; j < i; j++)
			{				
				switch (myGameView.sortVM.sortSelected)
				{
					case 0:
						tempA = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [i]].Life;
						tempB = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [j]].Life;
						break;
					case 1:
						tempB = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [i]].Life;
						tempA = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [j]].Life;
						break;
					case 2:
						tempA = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [i]].Attack;
						tempB = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [j]].Attack;
						break;
					case 3:
						tempB = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [i]].Attack;
						tempA = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [j]].Attack;
						break;
					case 4:
						tempA = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [i]].Move;
						tempB = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [j]].Move;
						break;
					case 5:
						tempB = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [i]].Move;
						tempA = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [j]].Move;
						break;
					case 6:
						tempA = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [i]].Speed;
						tempB = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [j]].Speed;
						break;
					case 7:
						tempB = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [i]].Speed;
						tempA = myGameView.myGameVM.cards [myGameView.myGameVM.cardsToBeDisplayed [j]].Speed;
						break;
					default:
						break;
				}
				
				if (tempA < tempB)
				{
					myGameView.myGameVM.cardsToBeDisplayed.Insert(j, myGameView.myGameVM.cardsToBeDisplayed [i]);
					myGameView.myGameVM.cardsToBeDisplayed.RemoveAt(i + 1);
					break;
				}		
			}
		}	
	}
	#endregion sort
}

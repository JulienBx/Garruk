using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyGameView : MonoBehaviour 
{
	public FilterViewModel filterVM;
	public MyGamePopUpViewModel popupVM;
	public MyDecksViewModel myDecksVM;
	public SortViewModel sortVM;
	public FocusViewModel focusVM;
	public PaginationViewModel paginationVM;
	public MyGameViewModel myGameVM;


	int widthScreen = Screen.width; 
	int heightScreen = Screen.height;
	
	RaycastHit hit;
	Ray ray;

	void Start() 
	{	 
		setStyles();
		myGameScript.instance.MenuObject    = Instantiate(myGameScript.instance.MenuObject) as GameObject;
		filterVM.filtersCardType            = new List<int>();
		myGameVM.toReloadAll                = true;
	}
	
	void Update () 
	{
		if (Screen.width != widthScreen || Screen.height != heightScreen) 
		{
			this.setStyles();
			this.applyFilters();
			if (focusVM.focusedCard != -1)
			{
				Destroy(myGameVM.cardFocused);
				focusVM.focusedCard = -1;
			}
			this.clearCards();
			this.clearDeckCards();
			this.createCards();
			this.createDeckCards();
			filterVM.displayFilters = true;
			myGameVM.displayDecks = true;
			
		}
		if (myGameVM.toReload) 
		{
			this.applyFilters();
			if (sortVM.sortSelected != 10)
			{
				this.sortCards();
			}
			this.displayPage();
			myGameVM.toReload = false;
		}
		if (myGameVM.destroyAll)
		{
			this.clearCards();
			this.clearDeckCards();
			myGameVM.isCreatedDeckCards = false;
			myGameVM.toReloadAll = true;
			myGameVM.destroyAll = false;
		}
		if (myGameVM.toReloadAll) 
		{
			myGameVM.displayLoader = true;
			filterVM.displayFilters = false;
			
			myGameVM.areDecksRetrieved = false;
			myGameVM.areCreatedDeckCards = false;
			myGameVM.isLoadedCards = false;
			myGameVM.isLoadedDeck = false;
			
			myGameScript.instance.getCards();
			myGameVM.toReloadAll = false;
		}
		if (myGameVM.isLoadedCards)
		{
			this.createCards();
			StartCoroutine(myGameScript.instance.retrieveDecks());
			myGameVM.isLoadedCards = false;
			myGameVM.isCreatedCards = true;
		}
		if (myGameVM.areDecksRetrieved && myGameVM.isCreatedCards)
		{
			StartCoroutine(myGameScript.instance.retrieveCardsFromDeck(myDecksVM.chosenIdDeck));
			myGameVM.areDecksRetrieved = false;
		}
		if (myGameVM.isLoadedDeck)
		{
			if (myGameVM.isCreatedDeckCards)
			{
				this.displayDeckCards();
			}
			else
			{
				this.createDeckCards();
				myGameVM.isCreatedDeckCards = true;
			}
			this.applyFilters();
			this.displayPage();
			myGameVM.displayDecks = true;
			myGameVM.isLoadedDeck = false;
			myGameVM.displayLoader = false;
			filterVM.displayFilters = true;
		}
		
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray,out hit))
			{
				if (hit.collider.name.StartsWith("DeckCard"))
				{
					myGameScript.instance.RemoveCardFromDeck(myDecksVM.chosenIdDeck, 
					                                         myGameVM.cards[myGameVM.deckCardsIds[System.Convert.ToInt32(hit.collider.gameObject.name.Substring(8))]].Id);
					int tempInt = System.Convert.ToInt32(hit.collider.gameObject.name.Substring(8));
					myGameVM.deckCardsIds.RemoveAt(tempInt);
					myGameVM.myDecks[myDecksVM.chosenDeck].NbCards--;
					this.displayDeckCards();
					this.applyFilters();
					this.displayPage();
				}
				else if (hit.collider.name.StartsWith("Card"))
				{
					if (myGameVM.deckCardsIds.Count != 5)
					{
						int tempInt = System.Convert.ToInt32(hit.collider.gameObject.name.Substring(4));
						myGameVM.deckCardsIds.Add(tempInt);
						myGameVM.myDecks[myDecksVM.chosenDeck].NbCards++;
						this.displayDeckCards();
						myGameScript.instance.AddCardToDeck(myDecksVM.chosenIdDeck, 
						                                    myGameVM.cards[System.Convert.ToInt32(hit.collider.gameObject.name.Substring(4))].Id);
						this.applyFilters();
						this.displayPage();
					}
				}
			}
		}
		
		if (Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray,out hit))
			{
				if (hit.collider.name.Contains("DeckCard") || hit.collider.name.StartsWith("Card"))
				{
					myGameVM.displayDecks = false ;
					filterVM.displayFilters = false ;
					if (hit.collider.name.Contains("DeckCard"))
					{
						focusVM.focusedCard = System.Convert.ToInt32(hit.collider.gameObject.name.Substring(8));
					}
					else
					{
						focusVM.focusedCard = System.Convert.ToInt32(hit.collider.gameObject.name.Substring(4));
					}
					
					int finish = 3 * myGameVM.nbCardsPerRow;
					for(int i = 0 ; i < finish ; i++)
					{
						myGameVM.displayedCards[i].SetActive(false);
					}
					for(int i = 0 ; i < myGameVM.displayedDeckCards.Length ; i++)
					{
						myGameVM.displayedDeckCards[i].SetActive(false);
					}
					
					myGameVM.cardFocused = Instantiate(myGameScript.instance.CardObject) as GameObject;
					Destroy(myGameVM.cardFocused.GetComponent<GameNetworkCard>());
					Destroy(myGameVM.cardFocused.GetComponent<PhotonView>());
					float scale = heightScreen / 120f;
					myGameVM.cardFocused.transform.localScale = new Vector3(scale,scale,scale); 
					Vector3 vec = Camera.main.WorldToScreenPoint(myGameVM.cardFocused.collider.bounds.size);
					myGameVM.cardFocused.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0.50f * widthScreen, 
					                                                                                          0.45f * heightScreen - 1 , 
					                                                                                          10)); 
					myGameVM.cardFocused.gameObject.name = "FocusedCard";	
					
					if (hit.collider.name.Contains("DeckCard"))
					{
						myGameVM.idFocused = myGameVM.deckCardsIds[focusVM.focusedCard];
						
					}
					else{
						myGameVM.idFocused = focusVM.focusedCard;
					}
					
					myGameVM.cardId = myGameVM.cards[myGameVM.idFocused].Id;
					myGameVM.cardFocused.GetComponent<GameCard>().Card = myGameVM.cards[myGameVM.idFocused]; 
					focusVM.focusedCardPrice = myGameVM.cards[myGameVM.idFocused].getCost();
					if (myGameVM.cards[myGameVM.idFocused].onSale == 0)
					{
						myGameVM.textMarket = "Mettre la carte en vente sur le bazar";
						myGameVM.isMarketed = false;
					}
					else
					{
						myGameVM.textMarket = "La carte est mise en vente sur le bazar pour " + myGameVM.cards[myGameVM.idFocused].Price + " crédits. Modifier ?";
						myGameVM.isMarketed = true;
					}
					
					myGameVM.cardFocused.GetComponent<GameCard>().ShowFace();
					myGameVM.cardFocused.GetComponent<GameCard>().setTextResolution(2f);
					myGameVM.cardFocused.SetActive(true);
					myGameVM.cardFocused.transform.Find("texturedGameCard")
						.FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();

					myGameVM.cardFocused.transform.Find("texturedGameCard")
						.FindChild("ExperienceArea").GetComponent<GameCard_experience>().setTextResolution(2f);

					myDecksVM.rectFocus = new Rect(0.50f * widthScreen + (vec.x - widthScreen / 2f) / 2f, 0.15f * heightScreen, 
					                               0.25f * widthScreen, 0.8f * heightScreen);
				}
			}
		}
		
		if (myGameVM.destroySellingCardWindow)
		{
			myGameVM.isSellingCard              = false;
			myGameVM.destroySellingCardWindow   = false;
		}
		
		if (myGameVM.destroyUpgradingCardWindow)
		{
			myGameVM.isUpgradingCard            = false;
			myGameVM.destroyUpgradingCardWindow = false;
		}
		
		if (myGameVM.destroyRenamingCardWindow)
		{
			myGameVM.isRenamingCard             = false;
			myGameVM.destroyRenamingCardWindow  = false;
		}
		
		if (myGameVM.destroyFocus)
		{
			myGameVM.isSellingCard              = false;
			myGameVM.isMarketingCard            = false;
			myGameVM.isUpgradingCard            = false;
			myGameVM.isRenamingCard             = false;
			Destroy(myGameVM.cardFocused);
			focusVM.focusedCard                 = -1;
			filterVM.displayFilters             = true;
			myGameVM.displayDecks               = true;
			this.displayPage();
			this.displayDeckCards();
			myGameVM.destroyFocus               = false;
		}
		
		if (myGameVM.soldCard)
		{
			myGameVM.isSellingCard = false;
			Destroy(myGameVM.cardFocused);
			focusVM.focusedCard = -1;
			myGameVM.destroyAll = true ;
			myGameVM.soldCard = false;
		}
		
		if (myGameVM.isUpEscape)
		{
			myGameVM.isEscDown = false;
			myGameVM.isUpEscape = false;
		}
		
		if (myGameVM.isUpgradingCard)
		{
			if (Input.GetKeyDown(KeyCode.Return)) 
			{
				myGameVM.destroyUpgradingCardWindow = true;
				myGameVM.cardFocused.transform
					.Find("texturedGameCard")
						.FindChild("ExperienceArea")
						.GetComponent<GameCard_experience>()
						.addXp(myGameVM.cards[myGameVM.idFocused].getPriceForNextLevel(), myGameVM.cards[myGameVM.idFocused].getPriceForNextLevel());
				
			}
			else if (Input.GetKeyDown(KeyCode.Escape)) 
			{
				myGameVM.isUpgradingCard = false;
				myGameVM.isEscDown = true;
			}
		}
		else if (myGameVM.isRenamingCard)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				myGameVM.isRenamingCard = false;
				myGameVM.destroyRenamingCardWindow = true;
				myGameScript.instance.renameCard(myGameVM.cardFocused.GetComponent<GameCard>().Card.Id, myDecksVM.newTitle, popupVM.renameCost);
			}
			else if (Input.GetKeyDown(KeyCode.Escape))
			{
				myGameVM.isRenamingCard = false;
				myGameVM.isEscDown = true;
			}
			else if (myDecksVM.newTitle.Contains("\n"))
			{
				myGameVM.isRenamingCard = false;
				myGameVM.destroyRenamingCardWindow = true;
				myGameScript.instance.renameCard(myGameVM.cardFocused.GetComponent<GameCard>().Card.Id, myDecksVM.newTitle, popupVM.renameCost);
			}
		}
		else if (myGameVM.isSellingCard)
		{
			if (Input.GetKeyDown(KeyCode.Return)) 
			{
				myGameVM.isSellingCard = false;
				myGameVM.destroySellingCardWindow = true;
				StartCoroutine (myGameScript.instance.sellCard(myGameVM.cardId, focusVM.focusedCardPrice));
			}
			else if (Input.GetKeyDown(KeyCode.Escape))
			{
				myGameVM.isSellingCard = false;
				myGameVM.isEscDown = true;
			}
		}
		else if (myGameVM.isMarketingCard)
		{
			if (myGameVM.isChangingPrice)
			{
				if (Input.GetKeyDown(KeyCode.Return)) 
				{
					myGameVM.destroyFocus = true;
					myGameVM.isChangingPrice = false;
					myGameScript.instance.changeMarketPrice(myGameVM.cardId, focusVM.focusedCardPrice);
				}
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					myGameVM.isMarketingCard = false;
					myGameVM.isEscDown = true;
				}
			}
			
		}
		
		if (sortVM.oldSortSelected != sortVM.sortSelected)
		{
			if (sortVM.oldSortSelected != 10)
			{
				sortVM.sortButtonStyle[sortVM.oldSortSelected] = sortVM.sortDefaultButtonStyle;
			}
			sortVM.sortButtonStyle[sortVM.sortSelected] = sortVM.sortActivatedButtonStyle;
			sortVM.oldSortSelected = sortVM.sortSelected;
		}
		
	}
	
	void OnGUI()
	{
		if (focusVM.focusedCard != -1)
		{
			if (myGameVM.isSellingCard)
			{
				GUILayout.BeginArea(popupVM.centralWindow);
				{
					GUILayout.BeginVertical(popupVM.centralWindowStyle);
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label("Confirmer la désintégration de la carte (rapporte " + focusVM.focusedCardPrice + " crédits)", 
						                popupVM.centralWindowTitleStyle);

						GUILayout.Space(0.02f * heightScreen);
						GUILayout.BeginHorizontal();
						{
							GUILayout.Space(0.03f * widthScreen);
							if (GUILayout.Button("Désintégrer", popupVM.centralWindowButtonStyle))
							{
								myGameVM.destroySellingCardWindow = true;
								StartCoroutine(myGameScript.instance.sellCard(myGameVM.cardId, focusVM.focusedCardPrice));
							}
							GUILayout.Space(0.04f * widthScreen);
							if (GUILayout.Button("Annuler", popupVM.centralWindowButtonStyle))
							{
								myGameVM.destroySellingCardWindow = true;
							}
							GUILayout.Space(0.03f * widthScreen);
						}
						GUILayout.EndHorizontal();
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
			}
			else if (myGameVM.isUpgradingCard)
			{
				GUILayout.BeginArea(popupVM.centralWindow);
				{
					GUILayout.BeginVertical(popupVM.centralWindowStyle);
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label("Confirmer la montée de niveau de la carte (coûte " + 
						                myGameVM.cards[myGameVM.idFocused].getPriceForNextLevel() + " crédits)", 
						                popupVM.centralWindowTitleStyle);
						GUILayout.Space(0.02f * heightScreen);
						GUILayout.BeginHorizontal();
						{
							GUILayout.Space(0.03f * widthScreen);
							if (GUILayout.Button("Acheter", popupVM.centralWindowButtonStyle))
							{
								myGameVM.destroyUpgradingCardWindow = true ;
								myGameVM.cardFocused.transform
										.Find("texturedGameCard")
										.FindChild("ExperienceArea")
										.GetComponent<GameCard_experience>()
										.addXp(myGameVM.cards[myGameVM.idFocused].getPriceForNextLevel(), myGameVM.cards[myGameVM.idFocused].getPriceForNextLevel());
								
							}
							GUILayout.Space(0.04f * widthScreen);
							if (GUILayout.Button("Annuler", popupVM.centralWindowButtonStyle))
							{
								myGameVM.destroyUpgradingCardWindow = true;
							}
							GUILayout.Space(0.03f * widthScreen);
						}
						GUILayout.EndHorizontal();
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
			}
			else if (myGameVM.isRenamingCard)
			{
				GUILayout.BeginArea(popupVM.centralWindow);
				{
					GUILayout.BeginVertical(popupVM.centralWindowStyle);
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label("Renommer la carte pour " + popupVM.renameCost + " crédits", popupVM.centralWindowTitleStyle);
						GUILayout.FlexibleSpace();
						
						GUILayout.BeginHorizontal();
						{
							GUILayout.FlexibleSpace();
							myDecksVM.newTitle = GUILayout.TextField(myDecksVM.newTitle, 14, popupVM.centralWindowTextFieldStyle);
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndHorizontal();
						
						GUILayout.FlexibleSpace();
						GUILayout.BeginHorizontal();
						{
							GUILayout.FlexibleSpace();
							if (GUILayout.Button("Confirmer", popupVM.centralWindowButtonStyle))
							{
								myGameVM.destroyRenamingCardWindow = true ;
								myGameScript.instance.renameCard(myGameVM.cardFocused.GetComponent<GameCard>().Card.Id, myDecksVM.newTitle, 
								                                 popupVM.renameCost);
							}
							GUILayout.FlexibleSpace();
							if (GUILayout.Button("Annuler", popupVM.centralWindowButtonStyle))
							{
								myGameVM.destroyRenamingCardWindow = true;
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
			else if (myGameVM.isMarketingCard)
			{
				if (myGameVM.isChangingPrice)
				{	
					GUILayout.BeginArea(popupVM.centralWindow);
					{
						GUILayout.BeginVertical(popupVM.centralWindowStyle);
						{
							GUILayout.FlexibleSpace();
							GUILayout.Label("Changer le prix de vente de la carte sur le bazar", popupVM.centralWindowTitleStyle);
							GUILayout.FlexibleSpace();
							
							GUILayout.BeginHorizontal();
							{
								GUILayout.FlexibleSpace();
								myGameVM.tempPrice = GUILayout.TextField(myGameVM.tempPrice, popupVM.centralWindowTextFieldStyle);
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndHorizontal();
							
							GUILayout.FlexibleSpace();
							GUILayout.BeginHorizontal();
							{
								GUILayout.FlexibleSpace();
								if (GUILayout.Button("Confirmer", popupVM.centralWindowButtonStyle))
								{
									myGameVM.destroyFocus = true;
									myGameVM.isChangingPrice = false;
									myGameScript.instance.changeMarketPrice(myGameVM.cardId, System.Convert.ToInt32(myGameVM.tempPrice));
								}
								GUILayout.FlexibleSpace();
								if (GUILayout.Button("Annuler", popupVM.centralWindowButtonStyle))
								{
									myGameVM.isChangingPrice = false;
									myGameVM.isMarketingCard = false;
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
				else if (myGameVM.isMarketed)
				{
					if (Event.current.keyCode == KeyCode.Return)
					{
						myGameVM.destroyFocus = true;
						myGameScript.instance.removeFromMarket(myGameVM.cardId);
					}
					else if (Event.current.keyCode == KeyCode.Escape) 
					{
						myGameVM.isMarketingCard = false ;
						myGameVM.isEscDown = true ;
					}
					else{		
						GUILayout.BeginArea(popupVM.centralWindow);
						{
							GUILayout.BeginVertical(popupVM.centralWindowStyle);
							{
								GUILayout.FlexibleSpace();
								GUILayout.Label(myGameVM.textMarket, popupVM.centralWindowTitleStyle);
								GUILayout.Space(0.02f * heightScreen);
								GUILayout.BeginHorizontal();
								{
									GUILayout.FlexibleSpace();
									if (GUILayout.Button("Retirer du bazar", popupVM.smallCentralWindowButtonStyle))
									{
										myGameVM.destroyFocus = true ;
										myGameScript.instance.removeFromMarket(myGameVM.cardId);
										if (myGameVM.enVente)
										{
											myGameVM.toReload = true ;
										}
									}
									GUILayout.FlexibleSpace();
									if (GUILayout.Button("Modifier son prix", popupVM.smallCentralWindowButtonStyle))
									{
										myGameVM.isChangingPrice = true;
										myGameVM.tempPrice = "" + myGameVM.cards[myGameVM.idFocused].Price;
									}
									GUILayout.FlexibleSpace();
									if (GUILayout.Button("Annuler", popupVM.smallCentralWindowButtonStyle))
									{
										myGameVM.isMarketingCard = false;
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
				}
				else
				{
					if (Event.current.keyCode == KeyCode.Return)
					{
						myGameVM.destroyFocus = true;
						myGameScript.instance.putOnMarket(myGameVM.cardId, focusVM.focusedCardPrice);
					}
					else if (Event.current.keyCode == KeyCode.Escape) 
					{
						myGameVM.isMarketingCard = false;
						myGameVM.isEscDown = true;
					}
					else
					{		
						GUILayout.BeginArea(popupVM.centralWindow);
						{
							GUILayout.BeginVertical(popupVM.centralWindowStyle);
							{
								GUILayout.FlexibleSpace();
								GUILayout.Label("Choisir le prix en vente de la carte sur le bazar", popupVM.centralWindowTitleStyle);
								GUILayout.FlexibleSpace();
								GUILayout.BeginHorizontal();
								{
									GUILayout.FlexibleSpace();
									myGameVM.tempPrice = GUILayout.TextField(myGameVM.tempPrice, popupVM.centralWindowTextFieldStyle);
									GUILayout.FlexibleSpace();
								}
								GUILayout.EndHorizontal();
								GUILayout.FlexibleSpace();
								GUILayout.BeginHorizontal();
								{
									GUILayout.Space(0.03f * widthScreen);
									if (GUILayout.Button("Confirmer", popupVM.centralWindowButtonStyle))
									{
										myGameVM.destroyFocus = true;
										myGameScript.instance.putOnMarket(myGameVM.cardId, System.Convert.ToInt32(myGameVM.tempPrice));
									}
									GUILayout.Space(0.04f * widthScreen);
									if (GUILayout.Button("Annuler", popupVM.centralWindowButtonStyle))
									{
										myGameVM.isMarketingCard = false;
									}
									GUILayout.Space(0.03f * widthScreen);
								}
								GUILayout.EndHorizontal();
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
						}
						GUILayout.EndArea();
					}
				}
			}
			else
			{
				if (myGameVM.isEscDown) 
				{
					if (Input.GetKeyUp(KeyCode.Escape)) 
					{
						myGameVM.isUpEscape = true;
					}
				}
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					myGameVM.destroyFocus = true;
				}
				else
				{
					GUILayout.BeginArea(myDecksVM.rectFocus);
					{
						GUILayout.BeginVertical();
						{
							if (GUILayout.Button("Désintégrer (+" + focusVM.focusedCardPrice + " crédits)", focusVM.focusButtonStyle))
							{
								myGameVM.isSellingCard = true; 
							}
							if (GUILayout.Button(myGameVM.textMarket, focusVM.focusButtonStyle))
							{
								myGameVM.isMarketingCard = true ; 
								myGameVM.tempPrice = "" + myGameVM.cards[myGameVM.idFocused].getCost();
							}
							if (myGameVM.cards[myGameVM.idFocused].getPriceForNextLevel() != 0 
							    && myGameVM.cards[myGameVM.idFocused].getPriceForNextLevel() <= ApplicationModel.credits)
							{
								if (GUILayout.Button("Passer au niveau suivant (-" + myGameVM.cards[myGameVM.idFocused].getPriceForNextLevel() + " crédits)",
								                     focusVM.focusButtonStyle))
								{
									myGameVM.isUpgradingCard = true;
									myGameVM.isMarketingCard = true; 
									myGameVM.tempPrice = "" + myGameVM.cards[myGameVM.idFocused].getCost();
								}
							}
							if (myGameVM.cards[myGameVM.idFocused].getPriceForNextLevel() != 0 && 
							    myGameVM.cards[myGameVM.idFocused].getPriceForNextLevel() > ApplicationModel.credits)
							{
								GUILayout.Label("Passer au niveau suivant (-" + myGameVM.cards[myGameVM.idFocused].getPriceForNextLevel() + " crédits)", 
								                focusVM.cantBuyStyle);
							}
							if (popupVM.renameCost <= ApplicationModel.credits)
							{
								if (GUILayout.Button("Renommer la carte pour (-" + popupVM.renameCost + " crédits)", focusVM.focusButtonStyle))
								{
									myGameVM.isRenamingCard = true ;
									myDecksVM.newTitle = myGameVM.cardFocused.GetComponent<GameCard>().Card.Title;
								}
							}
							if (popupVM.renameCost > ApplicationModel.credits)
							{
								GUILayout.Label("Renommer la carte pour (-" + popupVM.renameCost + " crédits)", focusVM.cantBuyStyle);
							}
							string plurielWin = "";
							string plurielLoose = "";
							if (myGameVM.cards[myGameVM.idFocused].nbWin > 1)
							{
								plurielWin = "s";
							}
							if (myGameVM.cards[myGameVM.idFocused].nbLoose > 1)
							{
								plurielLoose = "s";
							}
							GUILayout.Label(myGameVM.cards[myGameVM.idFocused].nbWin + " victoire" + plurielWin + ", " +
							                myGameVM.cards[myGameVM.idFocused].nbLoose + " défaite" + plurielLoose, 
							                focusVM.cantBuyStyle);
							GUILayout.FlexibleSpace();
							if (GUILayout.Button("Revenir à mes cartes", focusVM.focusButtonStyle))
							{
								myGameVM.destroyFocus = true;
							}
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndArea();
				}
			}
		}
		
		if (myGameVM.displayDecks)
		{
			if (myDecksVM.IDDeckToEdit != -1)
			{
				if (Event.current.keyCode == KeyCode.Escape)
				{
					myDecksVM.IDDeckToEdit = -1;
					myDecksVM.tempText = "Nouveau deck";
				}
				else if (Event.current.keyCode == KeyCode.Return)
				{
					StartCoroutine(myGameScript.instance.deleteDeck(myDecksVM.IDDeckToEdit));
					myDecksVM.tempText = "Nouveau deck";
					myDecksVM.IDDeckToEdit = -1;
				}
				else
				{
					GUILayout.BeginArea(popupVM.centralWindow);
					{
						GUILayout.BeginVertical(popupVM.centralWindowStyle);
						{
							GUILayout.FlexibleSpace();
							GUILayout.Label("Voulez-vous supprimer le deck ?", popupVM.centralWindowTitleStyle);
							GUILayout.Space(0.02f * heightScreen);
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.03f * widthScreen);
								if (GUILayout.Button("Confirmer la suppression", popupVM.centralWindowButtonStyle))
								{
									StartCoroutine(myGameScript.instance.deleteDeck(myDecksVM.IDDeckToEdit));
									myDecksVM.tempText = "Nouveau deck";
									myDecksVM.IDDeckToEdit = -1;
								}
								GUILayout.Space(0.04f * widthScreen);
								if (GUILayout.Button("Annuler", popupVM.centralWindowButtonStyle))
								{
									myGameVM.displayCreationDeckWindow = false; 
									myDecksVM.IDDeckToEdit = -1;
								}
								GUILayout.Space(0.03f * widthScreen);
							}
							GUILayout.EndHorizontal();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndArea();
				}
			}
			if (myGameVM.displayCreationDeckWindow)
			{	
				if (Event.current.keyCode == KeyCode.Escape)
				{
					myGameVM.displayCreationDeckWindow = false;
					myDecksVM.tempText = "Nouveau deck";
				}
				else if (Event.current.keyCode == KeyCode.Return)
				{
					StartCoroutine(myGameScript.instance.addDeck(myDecksVM.tempText));
					myDecksVM.tempText = "Nouveau deck";
					myGameVM.displayCreationDeckWindow = false;
				}
				else
				{
					GUILayout.BeginArea(popupVM.centralWindow);
					{
						GUILayout.BeginVertical(popupVM.centralWindowStyle);
						{
							GUILayout.FlexibleSpace();
							GUILayout.Label("Choisissez le nom de votre nouveau deck", popupVM.centralWindowTitleStyle);
							GUILayout.Space(0.02f * heightScreen);
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.05f * widthScreen);
								myDecksVM.tempText = GUILayout.TextField(myDecksVM.tempText, popupVM.centralWindowTextFieldStyle);
							}
							GUILayout.EndHorizontal();
							GUILayout.Space(0.02f * heightScreen);
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.03f * widthScreen);
								if (GUILayout.Button("Créer le deck",popupVM.centralWindowButtonStyle))
								{
									StartCoroutine(myGameScript.instance.addDeck(myDecksVM.tempText));
									myDecksVM.tempText = "Nouveau deck";
									myGameVM.displayCreationDeckWindow = false ;
								}
								GUILayout.Space(0.04f * widthScreen);
								if (GUILayout.Button("Annuler", popupVM.centralWindowButtonStyle))
								{
									myGameVM.displayCreationDeckWindow = false ; 
									myDecksVM.tempText = "Nouveau deck";
								}
								GUILayout.Space(0.03f * widthScreen);
							}
							GUILayout.EndHorizontal();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndArea();
				}
			}
			if (myDecksVM.deckToEdit != -1)
			{	
				if (Event.current.keyCode == KeyCode.Escape)
				{
					myDecksVM.deckToEdit = -1;
					myDecksVM.tempText = "Nouveau deck";
				}
				else if (Event.current.keyCode == KeyCode.Return)
				{
					StartCoroutine(myGameScript.instance.editDeck(myDecksVM.deckToEdit, myDecksVM.tempText));
					myDecksVM.tempText = "Nouveau deck";
					myDecksVM.deckToEdit = -1;
				}
				else
				{
					GUILayout.BeginArea(popupVM.centralWindow);
					{
						GUILayout.BeginVertical(popupVM.centralWindowStyle);
						{
							GUILayout.FlexibleSpace();
							GUILayout.Label("Modifiez le nom de votre deck", popupVM.centralWindowTitleStyle);
							GUILayout.Space(0.02f * heightScreen);
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.05f * widthScreen);
								myDecksVM.tempText = GUILayout.TextField(myDecksVM.tempText, popupVM.centralWindowTextFieldStyle);
							}
							GUILayout.EndHorizontal();
							GUILayout.Space(0.02f * heightScreen);
							GUILayout.BeginHorizontal();
							{
								GUILayout.Space(0.03f * widthScreen);
								if (GUILayout.Button("Modifier", popupVM.centralWindowButtonStyle))
								{
									StartCoroutine(myGameScript.instance.editDeck(myDecksVM.deckToEdit, myDecksVM.tempText));
									myDecksVM.tempText = "Nouveau deck";
									myDecksVM.deckToEdit = -1;
								}
								GUILayout.Space(0.04f * widthScreen);
								if (GUILayout.Button("Annuler", popupVM.centralWindowButtonStyle))
								{
									myDecksVM.deckToEdit = -1; 
									myDecksVM.tempText = "Nouveau deck";
								}
								GUILayout.Space(0.03f * widthScreen);
							}
							GUILayout.EndHorizontal();
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndVertical();
					}
					GUILayout.EndArea();
				}
			}
			GUILayout.BeginArea(myDecksVM.rectDeck);
			{
				GUILayout.BeginVertical();
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label(myDecksVM.decksTitle, myDecksVM.decksTitleStyle);
						GUILayout.FlexibleSpace();
						if (GUILayout.Button(myDecksVM.myNewDeckButtonTitle, myDecksVM.myNewDeckButton))
						{
							myGameVM.displayCreationDeckWindow = true;
						}
					}
					GUILayout.EndHorizontal();
					
					GUILayout.Space(0.005f * heightScreen);
					
					myGameVM.scrollPosition = GUILayout.BeginScrollView(myGameVM.scrollPosition, GUILayout.Width(0.19f * widthScreen), 
					                                           GUILayout.Height(0.17f * heightScreen));
					
					for(int i = 0 ; i < myGameVM.myDecks.Count ; i++)
					{	
						GUILayout.BeginHorizontal(myDecksVM.myDecksGuiStyle[i]);
						{
							if (GUILayout.Button("(" + myGameVM.myDecks[i].NbCards + ") " + myGameVM.myDecks[i].Name, myDecksVM.myDecksButtonGuiStyle[i]))
							{
								if (myDecksVM.chosenDeck != i)
								{
									myDecksVM.myDecksGuiStyle[myDecksVM.chosenDeck] = myDecksVM.deckStyle;
									myDecksVM.myDecksButtonGuiStyle[myDecksVM.chosenDeck] = myDecksVM.deckButtonStyle;
									myDecksVM.chosenDeck = i;
									myDecksVM.myDecksGuiStyle[i] = myDecksVM.deckChosenStyle;
									myDecksVM.myDecksButtonGuiStyle[i] = myDecksVM.deckButtonChosenStyle;
									myDecksVM.chosenIdDeck = myGameVM.myDecks[i].Id;
									StartCoroutine(myGameScript.instance.retrieveCardsFromDeck(myDecksVM.chosenIdDeck));
								}
							}
							GUILayout.FlexibleSpace();
							if (GUILayout.Button("", myDecksVM.myEditButtonStyle))
							{
								myDecksVM.tempText = myGameVM.myDecks[i].Name;
								myDecksVM.deckToEdit = myGameVM.myDecks[i].Id;
							}
							
							if (GUILayout.Button("", myDecksVM.mySuppressButtonStyle))
							{
								myDecksVM.IDDeckToEdit = myGameVM.myDecks[i].Id;
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
		
		if (myGameVM.displayLoader)
		{
			GUILayout.BeginArea(new Rect(widthScreen * 0.22f, 0.26f * heightScreen,widthScreen * 0.78f, 0.64f * heightScreen));
			{
				GUILayout.BeginVertical(); 
				{
					GUILayout.Label("Cartes en cours de chargement...   " + myGameVM.cardsToBeDisplayed.Count + " carte(s) chargee(s)");
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
		if (filterVM.displayFilters)
		{
			GUILayout.BeginArea(new Rect(widthScreen * 0.01f, 0.965f * heightScreen, widthScreen * 0.78f, 0.03f * heightScreen));
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (paginationVM.pageDebut > 0)
					{
						if (GUILayout.Button("...", paginationVM.paginationStyle))
						{
							paginationVM.pageDebut = paginationVM.pageDebut - 15;
							paginationVM.pageFin = paginationVM.pageDebut + 15;
						}
					}
					GUILayout.Space(widthScreen * 0.01f);
					for (int i = paginationVM.pageDebut ; i < paginationVM.pageFin ; i++)
					{
						if (GUILayout.Button("" + (i + 1), paginationVM.paginatorGuiStyle[i]))
						{
							paginationVM.paginatorGuiStyle[paginationVM.chosenPage] = paginationVM.paginationStyle;
							paginationVM.chosenPage = i;
							paginationVM.paginatorGuiStyle[i] = paginationVM.paginationActivatedStyle;
							displayPage();
						}
						GUILayout.Space(widthScreen * 0.01f);
					}
					if (paginationVM.nbPages > paginationVM.pageFin)
					{
						if (GUILayout.Button("...", paginationVM.paginationStyle))
						{
							paginationVM.pageDebut = paginationVM.pageDebut + 15;
							paginationVM.pageFin = Mathf.Min(paginationVM.pageFin + 15, paginationVM.nbPages);
						}
					}
					GUILayout.FlexibleSpace();
				}
				
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0.80f * widthScreen,0.11f*heightScreen,widthScreen * 0.19f,0.85f * heightScreen));
			{
				bool toggle;
				string tempString;
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace(); 
					toggle = GUILayout.Toggle (myGameVM.enVente, "Cartes en vente",filterVM.toggleStyle);
					if (toggle != myGameVM.enVente) 
					{
						myGameVM.enVente = toggle;
						myGameVM.toReload = true;
					}
					
					GUILayout.FlexibleSpace(); 
					
					GUILayout.Label("Filtrer par classe", filterVM.filterTitleStyle);
					for (int i = 0 ; i < myGameVM.cardTypeList.Length - 1 ; i++) 
					{		
						toggle = GUILayout.Toggle (myGameVM.togglesCurrentStates [i], myGameVM.cardTypeList[i], filterVM.toggleStyle);
						if (toggle != myGameVM.togglesCurrentStates [i]) 
						{
							myGameVM.togglesCurrentStates[i] = toggle;
							if (toggle)
							{
								filterVM.filtersCardType.Add(i);
							}
							else
							{
								filterVM.filtersCardType.Remove(i);
							}
							myGameVM.toReload = true;
						}
					}
					
					GUILayout.FlexibleSpace();
					GUILayout.Label("Filtrer une capacité", filterVM.filterTitleStyle);
					tempString = GUILayout.TextField(myGameVM.valueSkill, filterVM.filterTextFieldStyle);
					if (tempString != myGameVM.valueSkill) 
					{
						if (tempString.Length > 0) 
						{
							myGameVM.isSkillToDisplay = true;
							myGameVM.valueSkill = tempString.ToLower();
							displaySkills();
						} 
						else 
						{
							myGameVM.isSkillToDisplay = false;
							myGameVM.valueSkill = "";
						}
						if (myGameVM.isSkillChosen)
						{
							myGameVM.isSkillChosen=false ;
							myGameVM.toReload = true ;
						}
					}
					if (myGameVM.isSkillToDisplay)
					{
						GUILayout.Space(-3);
						for (int j = 0 ; j < filterVM.matchValues.Count ; j++) 
						{
							if (GUILayout.Button(filterVM.matchValues[j], filterVM.myStyle)) 
							{
								myGameVM.valueSkill = filterVM.matchValues[j].ToLower();
								myGameVM.isSkillChosen = true;
								filterVM.matchValues = new List<string>();
								myGameVM.toReload = true;
							}
						}
					}
					
					GUILayout.FlexibleSpace();			
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Vie", filterVM.filterTitleStyle);
						GUILayout.FlexibleSpace();
						if (GUILayout.Button ("^", sortVM.sortButtonStyle[0])) 
						{
							sortVM.sortSelected = 0;
							myGameVM.toReload = true;
						}
						if (GUILayout.Button ("v", sortVM.sortButtonStyle[1])) 
						{
							sortVM.sortSelected = 1;
							myGameVM.toReload = true;
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label("Min:" + Mathf.Round(filterVM.minLifeVal), filterVM.smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label("Max:" + Mathf.Round(filterVM.maxLifeVal), filterVM.smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					MyGUI.MinMaxSlider(ref filterVM.minLifeVal, ref filterVM.maxLifeVal, filterVM.minLifeLimit, filterVM.maxLifeLimit);
					
					GUILayout.FlexibleSpace();
					
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Attaque", filterVM.filterTitleStyle);
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("^", sortVM.sortButtonStyle[2])) 
						{
							sortVM.sortSelected = 2;
							myGameVM.toReload = true;
						}
						if (GUILayout.Button("v", sortVM.sortButtonStyle[3])) 
						{
							sortVM.sortSelected = 3;
							myGameVM.toReload = true;
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:" + Mathf.Round(filterVM.minAttackVal), filterVM.smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:" + Mathf.Round(filterVM.maxAttackVal), filterVM.smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					MyGUI.MinMaxSlider(ref filterVM.minAttackVal, ref filterVM.maxAttackVal, filterVM.minAttackLimit, filterVM.maxAttackLimit);
					
					GUILayout.FlexibleSpace();
					
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label("Filtrer par Mouvement", filterVM.filterTitleStyle);
						GUILayout.FlexibleSpace();
						if (GUILayout.Button("^", sortVM.sortButtonStyle[4])) 
						{
							sortVM.sortSelected = 4;
							myGameVM.toReload = true;
						}
						if (GUILayout.Button("v", sortVM.sortButtonStyle[5])) 
						{
							sortVM.sortSelected = 5;
							myGameVM.toReload = true;
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label("Min:" + Mathf.Round(filterVM.minMoveVal), filterVM.smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label("Max:" + Mathf.Round(filterVM.maxMoveVal), filterVM.smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					MyGUI.MinMaxSlider(ref filterVM.minMoveVal, ref filterVM.maxMoveVal, filterVM.minMoveLimit, filterVM.maxMoveLimit);
					
					GUILayout.FlexibleSpace();
					
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Rapidité", filterVM.filterTitleStyle);
						GUILayout.FlexibleSpace();
						if (GUILayout.Button ("^", sortVM.sortButtonStyle[6])) 
						{
							sortVM.sortSelected = 6;
							myGameVM.toReload = true;
						}
						if (GUILayout.Button ("v", sortVM.sortButtonStyle[7])) 
						{
							sortVM.sortSelected = 7;
							myGameVM.toReload = true;
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:" + Mathf.Round(filterVM.minQuicknessVal), filterVM.smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:" + Mathf.Round(filterVM.maxQuicknessVal), filterVM.smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					MyGUI.MinMaxSlider(ref filterVM.minQuicknessVal, ref filterVM.maxQuicknessVal, filterVM.minQuicknessLimit, filterVM.maxQuicknessLimit);
					
					if (Input.GetMouseButtonDown(0))
					{
						myGameVM.isBeingDragged = true;
					}
					if (Input.GetMouseButtonUp(0))
					{
						myGameVM.isBeingDragged = false;
					}
					
					if (!myGameVM.isBeingDragged)
					{
						bool isMoved                = false;
						filterVM.maxLifeVal         = Mathf.RoundToInt(filterVM.maxLifeVal);
						filterVM.minLifeVal         = Mathf.RoundToInt(filterVM.minLifeVal);
						filterVM.maxAttackVal       = Mathf.RoundToInt(filterVM.maxAttackVal);
						filterVM.minAttackVal       = Mathf.RoundToInt(filterVM.minAttackVal);
						filterVM.maxMoveVal         = Mathf.RoundToInt(filterVM.maxMoveVal);
						filterVM.minMoveVal         = Mathf.RoundToInt(filterVM.minMoveVal);
						filterVM.maxQuicknessVal    = Mathf.RoundToInt(filterVM.maxQuicknessVal);
						filterVM.minQuicknessVal    = Mathf.RoundToInt(filterVM.minQuicknessVal);
						
						if (filterVM.oldMaxLifeVal != filterVM.maxLifeVal)
						{
							filterVM.oldMaxLifeVal = filterVM.maxLifeVal;
							isMoved = true; 
						}
						if (filterVM.oldMinLifeVal != filterVM.minLifeVal)
						{
							filterVM.oldMinLifeVal = filterVM.minLifeVal;
							isMoved = true; 
						}
						if (filterVM.oldMaxAttackVal != filterVM.maxAttackVal)
						{
							filterVM.oldMaxAttackVal = filterVM.maxAttackVal;
							isMoved = true; 
						}
						if (filterVM.oldMinAttackVal != filterVM.minAttackVal)
						{
							filterVM.oldMinAttackVal = filterVM.minAttackVal;
							isMoved = true; 
						}
						if (filterVM.oldMaxMoveVal != filterVM.maxMoveVal)
						{
							filterVM.oldMaxMoveVal = Mathf.RoundToInt(filterVM.maxMoveVal);
							isMoved = true; 
						}
						if (filterVM.oldMinMoveVal != filterVM.minMoveVal)
						{
							filterVM.oldMinMoveVal = Mathf.RoundToInt(filterVM.minMoveVal);
							isMoved = true; 
						}
						if (filterVM.oldMaxQuicknessVal != filterVM.maxQuicknessVal)
						{
							filterVM.oldMaxQuicknessVal = filterVM.maxQuicknessVal;
							isMoved = true; 
						}
						if (filterVM.oldMinQuicknessVal != filterVM.minQuicknessVal)
						{
							filterVM.oldMinQuicknessVal = filterVM.minQuicknessVal;
							isMoved = true; 
						}
						if (isMoved)
						{
							myGameVM.toReload = true;
						}
					}
				}
				GUILayout.EndVertical();	
			}
			GUILayout.EndArea();
		}
		
	}

	public void setStyles() 
	{	
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		
		if (heightScreen < widthScreen) 
		{
			myDecksVM.decksTitleStyle.fontSize                           = heightScreen * 2 / 100;
			myDecksVM.decksTitleStyle.fixedHeight                        = (int) heightScreen * 3 / 100;
			myDecksVM.decksTitleStyle.fixedWidth                         = (int) widthScreen * 9 / 100;
			myDecksVM.decksTitle                                         = "Mes decks";
			
			myDecksVM.myNewDeckButton.fontSize                           = heightScreen * 2 / 100;
			myDecksVM.myNewDeckButton.fixedHeight                        = heightScreen * 3 / 100;
			myDecksVM.myNewDeckButton.fixedWidth                         = widthScreen * 9 / 100;
			myDecksVM.myNewDeckButton.normal.background                  = myGameVM.backButton;
			myDecksVM.myNewDeckButton.hover.background                   = myGameVM.backActivatedButton;
			myDecksVM.myNewDeckButtonTitle                               = "Nouveau";
			
			popupVM.centralWindow = new Rect(widthScreen * 0.25f, 0.12f * heightScreen, widthScreen * 0.50f, 0.18f * heightScreen);
			
			popupVM.centralWindowStyle.fixedWidth                        = widthScreen * 0.5f-5;
			
			popupVM.centralWindowTitleStyle.fontSize                     = heightScreen * 2 / 100;
			popupVM.centralWindowTitleStyle.fixedHeight                  = heightScreen * 3 / 100;
			popupVM.centralWindowTitleStyle.fixedWidth                   = widthScreen * 5 / 10;
			
			popupVM.centralWindowTextFieldStyle.fontSize                 = heightScreen * 2 / 100;
			popupVM.centralWindowTextFieldStyle.fixedHeight              = heightScreen * 3 / 100;
			popupVM.centralWindowTextFieldStyle.fixedWidth               = widthScreen * 4 / 10;
			
			popupVM.centralWindowButtonStyle.fontSize                    = heightScreen * 2 / 100;
			popupVM.centralWindowButtonStyle.fixedHeight                 = heightScreen * 3 / 100;
			popupVM.centralWindowButtonStyle.fixedWidth                  = widthScreen * 2 / 10;
			
			popupVM.smallCentralWindowButtonStyle.fontSize               = heightScreen * 15 / 1000;
			popupVM.smallCentralWindowButtonStyle.fixedHeight            = heightScreen * 3 / 100;
			popupVM.smallCentralWindowButtonStyle.fixedWidth             = widthScreen * 1 / 10;
			
			myDecksVM.rectDeck              = new Rect(widthScreen * 0.005f, 0.105f * heightScreen, widthScreen * 0.19f, 0.21f * heightScreen);
			myDecksVM.rectInsideScrollDeck  = new Rect(widthScreen * 0.005f, 0.12f * heightScreen, widthScreen * 0.18f, 0.18f * heightScreen);
			myDecksVM.rectOutsideScrollDeck = new Rect(widthScreen * 0.005f, 0.12f * heightScreen, widthScreen * 0.19f, 0.18f * heightScreen);
			
			myDecksVM.deckStyle.fixedHeight                              = heightScreen * 3 / 100;
			myDecksVM.deckStyle.fixedWidth                               = widthScreen * 17 / 100;
			
			myDecksVM.deckChosenStyle.fixedHeight                        = heightScreen * 3 / 100;
			myDecksVM.deckChosenStyle.fixedWidth                         = widthScreen * 17 / 100;
			
			myDecksVM.deckButtonStyle.fontSize                           = heightScreen * 2 / 100;
			myDecksVM.deckButtonStyle.fixedHeight                        = heightScreen * 3 / 100;
			myDecksVM.deckButtonStyle.fixedWidth                         = widthScreen * 12 / 100;
			
			myDecksVM.deckButtonChosenStyle.fontSize                     = heightScreen * 2 / 100;
			myDecksVM.deckButtonChosenStyle.fixedHeight                  = heightScreen * 3 / 100;
			myDecksVM.deckButtonChosenStyle.fixedWidth                   = widthScreen * 12 / 100;
			
			myDecksVM.myEditButtonStyle.fixedHeight                      = heightScreen * 3 / 100;
			myDecksVM.myEditButtonStyle.fixedWidth                       = heightScreen * 3 / 100;
			
			myDecksVM.mySuppressButtonStyle.fixedHeight                  = heightScreen * 3 / 100;
			myDecksVM.mySuppressButtonStyle.fixedWidth                   = heightScreen * 3 / 100;
			
			paginationVM.paginationStyle.fontSize                        = heightScreen * 2 / 100;
			paginationVM.paginationStyle.fixedWidth                      = widthScreen * 3 / 100;
			paginationVM.paginationStyle.fixedHeight                     = heightScreen * 3 / 100;
			paginationVM.paginationActivatedStyle.fontSize               = heightScreen * 2 / 100;
			paginationVM.paginationActivatedStyle.fixedWidth             = widthScreen * 3 / 100;
			paginationVM.paginationActivatedStyle.fixedHeight            = heightScreen * 3 / 100;
			
			filterVM.filterTitleStyle.fixedWidth                         = widthScreen  *19 / 100;
			filterVM.filterTitleStyle.fixedHeight                        = heightScreen * 3 / 100;
			filterVM.filterTitleStyle.fontSize                           = heightScreen * 2 / 100;
			
			filterVM.toggleStyle.fixedWidth                              = widthScreen * 19 / 100;
			filterVM.toggleStyle.fixedHeight                             = heightScreen * 20 / 1000;
			filterVM.toggleStyle.fontSize                                = heightScreen * 15 / 1000;
			
			filterVM.filterTextFieldStyle.fontSize                       = heightScreen * 2 / 100;
			filterVM.filterTextFieldStyle.fixedHeight                    = heightScreen * 3 / 100;
			filterVM.filterTextFieldStyle.fixedWidth                     = widthScreen * 19 / 100;
			
			filterVM.myStyle.fontSize                                    = heightScreen * 15 / 1000;
			filterVM.myStyle.fixedHeight                                 = heightScreen * 20 / 1000;
			filterVM.myStyle.fixedWidth                                  = widthScreen * 19 / 100;
			
			filterVM.smallPoliceStyle.fontSize                           = heightScreen * 15 / 1000;
			filterVM.smallPoliceStyle.fixedHeight                        = heightScreen * 20 / 1000;
			
			focusVM.focusButtonStyle.fontSize                            = heightScreen * 2 / 100;
			focusVM.focusButtonStyle.fixedHeight                         = heightScreen * 6 / 100;
			focusVM.focusButtonStyle.fixedWidth                          = widthScreen * 25 / 100;
			
			focusVM.cantBuyStyle.fontSize                                = heightScreen * 2 / 100;
			focusVM.cantBuyStyle.fixedHeight                             = heightScreen * 6 / 100;
			focusVM.cantBuyStyle.fixedWidth                              = widthScreen * 25 / 100;
			
			// Style utilisé pour les bouttons de tri
			
			sortVM.sortDefaultButtonStyle.fontSize                       = heightScreen * 2 / 100;
			sortVM.sortDefaultButtonStyle.fixedHeight                    = (int) heightScreen * 3 / 100;
			sortVM.sortDefaultButtonStyle.fixedWidth                     = (int) widthScreen * 12 / 1000;
			
			sortVM.sortActivatedButtonStyle.fontSize                     = heightScreen * 2 / 100;
			sortVM.sortActivatedButtonStyle.fixedHeight                  = (int) heightScreen * 3 / 100;
			sortVM.sortActivatedButtonStyle.fixedWidth                   = (int) widthScreen * 12 / 1000;
			
			for (int i = 0 ; i < 10 ; i++)
			{
				if (sortVM.sortSelected == 10)
				{
					sortVM.sortButtonStyle[i] = sortVM.sortDefaultButtonStyle;
				}
			}
		}
		else
		{
			myDecksVM.decksTitleStyle.fontSize                           = heightScreen * 2 / 100;
			myDecksVM.decksTitleStyle.fixedHeight                        = heightScreen * 3 / 100;
			myDecksVM.decksTitleStyle.fixedWidth                         = widthScreen * 12 / 100;
			myDecksVM.decksTitle                                         = "Decks";
			
			myDecksVM.myNewDeckButton.fontSize                           = heightScreen * 2 / 100;
			myDecksVM.myNewDeckButton.fixedHeight                        = heightScreen * 3 / 100;
			myDecksVM.myNewDeckButton.fixedWidth                         = heightScreen * 3 / 100;
			myDecksVM.myNewDeckButton.normal.background                  = myDecksVM.backNewDeckButton ;
			myDecksVM.myNewDeckButton.hover.background                   = myDecksVM.backHoveredNewDeckButton ;
			myDecksVM.myNewDeckButtonTitle                               = "";
			
			popupVM.centralWindow = new Rect (widthScreen * 0.10f, 0.10f * heightScreen, widthScreen * 0.80f, 0.80f * heightScreen);
			popupVM.centralWindowTitleStyle.fontSize                     = heightScreen * 2 / 100;
			popupVM.centralWindowTitleStyle.fixedHeight                  = heightScreen * 3 / 100;
			popupVM.centralWindowTitleStyle.fixedWidth                   = widthScreen * 5 / 10;
			
			popupVM.centralWindowTextFieldStyle.fontSize                 = heightScreen * 1 / 100;
			popupVM.centralWindowTextFieldStyle.fixedHeight              = heightScreen * 3 / 100;
			popupVM.centralWindowTextFieldStyle.fixedWidth               = widthScreen * 7 / 10;
		}
	}
	
	private void displaySkills()
	{
		filterVM.matchValues = new List<string>();	
		if (myGameVM.valueSkill != "") 
		{
			filterVM.matchValues = new List<string> ();
			for (int i = 0 ; i < myGameVM.skillsList.Length - 1 ; i++) 
			{  
				if (myGameVM.skillsList[i].ToLower().Contains(myGameVM.valueSkill)) 
				{
					filterVM.matchValues.Add(myGameVM.skillsList[i]);
				}
			}
		}
	}
	
	private void applyFilters() 
	{
		myGameVM.cardsToBeDisplayed         = new List<int>();
		IList<int> tempCardsToBeDisplayed   = new List<int>();
		int nbFilters                       = filterVM.filtersCardType.Count;
		bool testFilters                    = false;
		bool testDeck                       = false;
		bool test;		
		bool minLifeBool                    = (filterVM.minLifeLimit == filterVM.minLifeVal);
		bool maxLifeBool                    = (filterVM.maxLifeLimit == filterVM.maxLifeVal);
		bool minMoveBool                    = (filterVM.minMoveLimit == filterVM.minMoveVal);
		bool maxMoveBool                    = (filterVM.maxMoveLimit == filterVM.maxMoveVal);
		bool minQuicknessBool               = (filterVM.minQuicknessLimit == filterVM.minQuicknessVal);
		bool maxQuicknessBool               = (filterVM.maxQuicknessLimit == filterVM.maxQuicknessVal);
		bool minAttackBool                  = (filterVM.minAttackLimit == filterVM.minAttackVal);
		bool maxAttackBool                  = (filterVM.maxAttackLimit == filterVM.maxAttackVal);

		if (myGameVM.isSkillChosen)
		{
			int max = myGameVM.cards.Count;
			if (nbFilters == 0)
			{
				max = myGameVM.cards.Count;
				if (myGameVM.enVente)
				{
					for (int i = 0 ; i < max ; i++) 
					{
						if (myGameVM.cards[i].hasSkill(myGameVM.valueSkill) && myGameVM.cards[i].onSale == 1)
						{
							testDeck = false;
							for (int j = 0 ; j < myGameVM.deckCardsIds.Count ; j++)
							{
								if (i == myGameVM.deckCardsIds[j])
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
					for (int i = 0 ; i < max ; i++)
					{
						if (myGameVM.cards[i].hasSkill(myGameVM.valueSkill))
						{
							testDeck = false;
							for (int j = 0 ; j < myGameVM.deckCardsIds.Count ; j++)
							{
								if (i == myGameVM.deckCardsIds[j])
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
			}
			else
			{
				for (int i = 0 ; i < max ; i++)
				{
					test = false;
					int j = 0;
					if (myGameVM.enVente)
					{
						while (!test && j != nbFilters)
						{
							if (myGameVM.cards[i].IdClass == filterVM.filtersCardType[j])
							{
								test = true;
								if (myGameVM.cards[i].hasSkill(myGameVM.valueSkill) && myGameVM.cards[i].onSale == 1)
								{
									testDeck = false;
									for (int k = 0 ; k < myGameVM.deckCardsIds.Count ; k++)
									{
										if (i == myGameVM.deckCardsIds[k])
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
					else
					{
						while (!test && j != nbFilters)
						{
							if (myGameVM.cards[i].IdClass == filterVM.filtersCardType[j])
							{
								test = true;
								if (myGameVM.cards[i].hasSkill(myGameVM.valueSkill))
								{
									testDeck = false;
									for (int k = 0 ; k < myGameVM.deckCardsIds.Count ; k++)
									{
										if (i == myGameVM.deckCardsIds[k])
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
			int max = myGameVM.cards.Count;
			if (nbFilters == 0)
			{
				if (myGameVM.enVente)
				{
					for (int i = 0 ; i < max ; i++)
					{
						if (myGameVM.cards[i].onSale == 1)
						{
							testDeck = false;
							for (int j = 0 ; j < myGameVM.deckCardsIds.Count ; j++)
							{
								if (i == myGameVM.deckCardsIds[j])
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
					for (int i = 0 ; i < max ; i++)
					{
						testDeck = false;
						for (int j = 0 ; j < myGameVM.deckCardsIds.Count ; j++)
						{
							if (i == myGameVM.deckCardsIds[j])
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
				if (myGameVM.enVente)
				{
					for (int i = 0 ; i < max ; i++)
					{
						test = false;
						int j = 0;
						while (!test && j != nbFilters)
						{
							if (myGameVM.cards[i].IdClass == filterVM.filtersCardType[j])
							{
								if (myGameVM.cards[i].onSale == 1)
								{
									test = true;
									testDeck = false;
									for (int k = 0 ; k < myGameVM.deckCardsIds.Count ; k++)
									{
										if (i == myGameVM.deckCardsIds[k])
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
				else
				{
					for (int i = 0 ; i < max ; i++) 
					{
						test = false;
						int j = 0;
						while (!test && j != nbFilters)
						{
							if (myGameVM.cards[i].IdClass == filterVM.filtersCardType[j])
							{
								test = true;
								testDeck = false;
								for (int k = 0 ; k < myGameVM.deckCardsIds.Count ; k++)
								{
									if (i == myGameVM.deckCardsIds[k])
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
			filterVM.minLifeLimit                = 10000;
			filterVM.maxLifeLimit                = 0;
			filterVM.minAttackLimit              = 10000;
			filterVM.maxAttackLimit              = 0;
			filterVM.minMoveLimit                = 10000;
			filterVM.maxMoveLimit                = 0;
			filterVM.minQuicknessLimit           = 10000;
			filterVM.maxQuicknessLimit           = 0;

			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++)
			{
				if (myGameVM.cards[tempCardsToBeDisplayed[i]].Life < filterVM.minLifeLimit)
				{
					filterVM.minLifeLimit = myGameVM.cards[tempCardsToBeDisplayed[i]].Life;
				}
				if (myGameVM.cards[tempCardsToBeDisplayed[i]].Life > filterVM.maxLifeLimit)
				{
					filterVM.maxLifeLimit = myGameVM.cards[tempCardsToBeDisplayed[i]].Life;
				}
				if (myGameVM.cards[tempCardsToBeDisplayed[i]].Attack < filterVM.minAttackLimit)
				{
					filterVM.minAttackLimit = myGameVM.cards[tempCardsToBeDisplayed[i]].Attack;
				}
				if (myGameVM.cards[tempCardsToBeDisplayed[i]].Attack > filterVM.maxAttackLimit)
				{
					filterVM.maxAttackLimit = myGameVM.cards[tempCardsToBeDisplayed[i]].Attack;
				}
				if (myGameVM.cards[tempCardsToBeDisplayed[i]].Move < filterVM.minMoveLimit)
				{
					filterVM.minMoveLimit = myGameVM.cards[tempCardsToBeDisplayed[i]].Move;
				}
				if (myGameVM.cards[tempCardsToBeDisplayed[i]].Move > filterVM.maxMoveLimit)
				{
					filterVM.maxMoveLimit = myGameVM.cards[tempCardsToBeDisplayed[i]].Move;
				}
				if (myGameVM.cards[tempCardsToBeDisplayed[i]].Speed < filterVM.minQuicknessLimit)
				{
					filterVM.minQuicknessLimit = myGameVM.cards[tempCardsToBeDisplayed[i]].Speed;
				}
				if (myGameVM.cards[tempCardsToBeDisplayed[i]].Speed > filterVM.maxQuicknessLimit)
				{
					filterVM.maxQuicknessLimit = myGameVM.cards[tempCardsToBeDisplayed[i]].Speed;
				}
			}
			if (minLifeBool && filterVM.maxLifeVal > filterVM.minLifeLimit)
			{
				filterVM.minLifeVal = filterVM.minLifeLimit;
			}
			else
			{
				if (filterVM.minLifeVal < filterVM.minLifeLimit)
				{
					filterVM.minLifeLimit = filterVM.minLifeVal;
				}
			}
			if (maxLifeBool && filterVM.minLifeVal < filterVM.maxLifeLimit)
			{
				filterVM.maxLifeVal = filterVM.maxLifeLimit;
				print("Max " + filterVM.maxLifeVal);
			}
			else
			{
				if (filterVM.maxLifeVal > filterVM.maxLifeLimit)
				{
					filterVM.maxLifeLimit = filterVM.maxLifeVal;
				}
				print("Max2 " + filterVM.maxLifeVal);
			}
			if (minAttackBool && filterVM.maxAttackVal > filterVM.minAttackLimit)
			{
				filterVM.minAttackVal = filterVM.minAttackLimit;
			}
			else
			{
				if (filterVM.minAttackVal < filterVM.minAttackLimit)
				{
					filterVM.minAttackLimit = filterVM.minAttackVal;
				}
			}
			if (maxAttackBool && filterVM.minAttackVal < filterVM.maxAttackLimit)
			{
				filterVM.maxAttackVal = filterVM.maxAttackLimit;
			}
			else
			{
				if (filterVM.maxAttackVal > filterVM.maxAttackLimit)
				{
					filterVM.maxAttackLimit = filterVM.maxAttackVal;
				}
			}
			if (minMoveBool && filterVM.maxMoveVal > filterVM.minMoveLimit)
			{
				filterVM.minMoveVal = filterVM.minMoveLimit;
			}
			else
			{
				if (filterVM.minMoveVal < filterVM.minMoveLimit)
				{
					filterVM.minMoveLimit = filterVM.minMoveVal;
				}
			}
			if (maxMoveBool && filterVM.minMoveVal < filterVM.maxMoveLimit)
			{
				filterVM.maxMoveVal = filterVM.maxMoveLimit;
			}
			else
			{
				if (filterVM.maxMoveVal > filterVM.maxMoveLimit)
				{
					filterVM.maxMoveLimit = filterVM.maxMoveVal;
				}
			}
			if (minQuicknessBool && filterVM.maxQuicknessVal > filterVM.minQuicknessLimit)
			{
				filterVM.minQuicknessVal = filterVM.minQuicknessLimit;
			}
			else
			{
				if (filterVM.minQuicknessVal < filterVM.minQuicknessLimit)
				{
					filterVM.minQuicknessLimit = filterVM.minQuicknessVal;
				}
			}
			if (maxQuicknessBool && filterVM.minQuicknessVal < filterVM.maxQuicknessLimit)
			{
				filterVM.maxQuicknessVal = filterVM.maxQuicknessLimit;
			}
			else
			{
				if (filterVM.maxQuicknessVal > filterVM.maxQuicknessLimit)
				{
					filterVM.maxQuicknessLimit = filterVM.maxQuicknessVal;
				}
			}
			filterVM.oldMinLifeVal               = filterVM.minLifeVal;
			filterVM.oldMaxLifeVal               = filterVM.maxLifeVal;
			filterVM.oldMinQuicknessVal          = filterVM.minQuicknessVal;
			filterVM.oldMaxQuicknessVal          = filterVM.maxQuicknessVal ;
			filterVM.oldMinMoveVal               = filterVM.minMoveVal;
			filterVM.oldMaxMoveVal               = filterVM.maxMoveVal;
			filterVM.oldMinAttackVal             = filterVM.minAttackVal;
			filterVM.oldMaxAttackVal             = filterVM.maxAttackVal;
		}
		
		if (filterVM.minLifeVal != filterVM.minLifeLimit)
		{
			testFilters = true;
		}
		else if (filterVM.maxLifeVal != filterVM.maxLifeLimit)
		{
			testFilters = true;
		}
		else if (filterVM.minAttackVal != filterVM.minAttackLimit)
		{
			testFilters = true;
		}
		else if (filterVM.maxAttackVal != filterVM.maxAttackLimit)
		{
			testFilters = true;
		}
		else if (filterVM.minMoveVal != filterVM.minMoveLimit)
		{
			testFilters = true;
		}
		else if (filterVM.maxMoveVal != filterVM.maxMoveLimit)
		{
			testFilters = true;
		}
		else if (filterVM.minQuicknessVal != filterVM.minQuicknessLimit)
		{
			testFilters = true;
		}
		else if (filterVM.maxQuicknessVal != filterVM.maxQuicknessLimit)
		{
			testFilters = true;
		}
		
		if (testFilters == true)
		{
			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++)
			{
				if (myGameVM.cards[tempCardsToBeDisplayed[i]].verifyC(filterVM.minLifeVal, 
							                                             filterVM.maxLifeVal, 
							                                             filterVM.minAttackVal, 
							                                             filterVM.maxAttackVal, 
							                                             filterVM.minMoveVal, 
							                                             filterVM.maxMoveVal, 
							                                             filterVM.minQuicknessVal, 
							                                             filterVM.maxQuicknessVal))
				{
					myGameVM.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
				}
			}
		}
		else
		{
			for (int i = 0 ; i < tempCardsToBeDisplayed.Count ; i++)
			{
				myGameVM.cardsToBeDisplayed.Add(tempCardsToBeDisplayed[i]);
			}
		}
		
		paginationVM.nbPages = Mathf.CeilToInt(myGameVM.cardsToBeDisplayed.Count / (3.0f * myGameVM.nbCardsPerRow));
		paginationVM.pageDebut = 0;
		if (paginationVM.nbPages > 15)
		{
			paginationVM.pageFin = 14;
		}
		else
		{
			paginationVM.pageFin = paginationVM.nbPages;
		}
		paginationVM.chosenPage = 0;
		paginationVM.paginatorGuiStyle = new GUIStyle[paginationVM.nbPages];
		for (int i = 0 ; i < paginationVM.nbPages ; i++) 
		{ 
			if (i == 0)
			{
				paginationVM.paginatorGuiStyle[i] = paginationVM.paginationActivatedStyle;
			}
			else
			{
				paginationVM.paginatorGuiStyle[i] = paginationVM.paginationStyle;
			}
		}
	}
	
	public void setFilters()
	{
		filterVM.minLifeLimit        = 10000;
		filterVM.maxLifeLimit        = 0;
		filterVM.minAttackLimit      = 10000;
		filterVM.maxAttackLimit      = 0;
		filterVM.minMoveLimit        = 10000;
		filterVM.maxMoveLimit        = 0;
		filterVM.minQuicknessLimit   = 10000;
		filterVM.maxQuicknessLimit   = 0;
		
		int max = myGameVM.cards.Count;
		for (int i = 0 ; i < max ; i++) 
		{
			if (myGameVM.cards[i].Life < filterVM.minLifeLimit)
			{
				filterVM.minLifeLimit = myGameVM.cards[i].Life;
			}
			if (myGameVM.cards[i].Life > filterVM.maxLifeLimit)
			{
				filterVM.maxLifeLimit = myGameVM.cards[i].Life;
			}
			if (myGameVM.cards[i].Attack < filterVM.minAttackLimit)
			{
				filterVM.minAttackLimit = myGameVM.cards[i].Attack;
			}
			if (myGameVM.cards[i].Attack > filterVM.maxAttackLimit)
			{
				filterVM.maxAttackLimit = myGameVM.cards[i].Attack;
			}
			if (myGameVM.cards[i].Move < filterVM.minMoveLimit)
			{
				filterVM.minMoveLimit = myGameVM.cards[i].Move;
			}
			if (myGameVM.cards[i].Move > filterVM.maxMoveLimit)
			{
				filterVM.maxMoveLimit = myGameVM.cards[i].Move;
			}
			if (myGameVM.cards[i].Speed < filterVM.minQuicknessLimit)
			{
				filterVM.minQuicknessLimit = myGameVM.cards[i].Speed;
			}
			if (myGameVM.cards[i].Speed > filterVM.maxQuicknessLimit)
			{
				filterVM.maxQuicknessLimit = myGameVM.cards[i].Speed;
			}
		}
		filterVM.minLifeVal         = filterVM.minLifeLimit;
		filterVM.maxLifeVal         = filterVM.maxLifeLimit;
		filterVM.minAttackVal       = filterVM.minAttackLimit;
		filterVM.maxAttackVal       = filterVM.maxAttackLimit;
		filterVM.minMoveVal         = filterVM.minMoveLimit;
		filterVM.maxMoveVal         = filterVM.maxMoveLimit;
		filterVM.minQuicknessVal    = filterVM.minQuicknessLimit;
		filterVM.maxQuicknessVal    = filterVM.maxQuicknessLimit;
	}

	    
	private void createCards()
	{	
		float tempF = 10f * widthScreen / heightScreen;
		float width = tempF * 0.78f;
		myGameVM.nbCardsPerRow = Mathf.FloorToInt(width / 1.6f);
		float debutLargeur = -0.49f * tempF + 0.8f + (width - 1.6f * myGameVM.nbCardsPerRow) / 2 ;
		myGameVM.displayedCards = new GameObject[3 * myGameVM.nbCardsPerRow];
		int nbCardsToDisplay = myGameVM.cardsToBeDisplayed.Count;
		for(int i = 0 ; i < 3 * myGameVM.nbCardsPerRow ; i++)
		{
			myGameVM.displayedCards[i] = Instantiate(myGameScript.instance.CardObject) as GameObject;
			Destroy(myGameVM.displayedCards[i].GetComponent<GameNetworkCard>());
			Destroy(myGameVM.displayedCards[i].GetComponent<PhotonView>());
			myGameVM.displayedCards[i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
			myGameVM.displayedCards[i].transform.localPosition = new Vector3(debutLargeur + 1.6f * (i % myGameVM.nbCardsPerRow), 
			                                                                    0.8f - (i - i % myGameVM.nbCardsPerRow) / myGameVM.nbCardsPerRow * 2.2f,
			                                                                    0); 
			myGameVM.displayedCards[i].gameObject.name = "Card" + i + "";	
			if (i < nbCardsToDisplay)
			{
				myGameVM.displayedCards[i].GetComponent<GameCard>().Card = myGameVM.cards[myGameVM.cardsToBeDisplayed[i]]; 
				myGameVM.displayedCards[i].GetComponent<GameCard>().ShowFace();
				myGameVM.displayedCards[i].transform.Find("texturedGameCard")
					.FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();
			}   
			else{
				myGameVM.displayedCards[i].SetActive(false);
			}
		}
		paginationVM.nbPages = Mathf.CeilToInt(myGameVM.cardsToBeDisplayed.Count / (3.0f * myGameVM.nbCardsPerRow));
		paginationVM.pageDebut = 0;
		if (paginationVM.nbPages > 15)
		{
			paginationVM.pageFin = 14 ;
		}
		else
		{
			paginationVM.pageFin = paginationVM.nbPages;
		}
		paginationVM.chosenPage = 0;
		paginationVM.paginatorGuiStyle = new GUIStyle[paginationVM.nbPages];

		for (int i = 0; i < paginationVM.nbPages; i++) 
		{ 
			if (i == 0)
			{
				paginationVM.paginatorGuiStyle[i] = paginationVM.paginationActivatedStyle;
			}
			else
			{
				paginationVM.paginatorGuiStyle[i] = paginationVM.paginationStyle;
			}
		}
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		this.setFilters();
	}
	
	private void displayPage()
	{	
		int start = 3 * myGameVM.nbCardsPerRow * paginationVM.chosenPage;
		int finish = start + 3 * myGameVM.nbCardsPerRow;
		for (int i = start ; i < finish ; i++)
		{
			//myGameVM.displayedCards[i].GetComponent<GameCard>().setTextResolution(1f);
			int nbCardsToDisplay = myGameVM.cardsToBeDisplayed.Count;
			if (i < nbCardsToDisplay)
			{
				myGameVM.displayedCards[i - start].gameObject.name = "Card" + myGameVM.cardsToBeDisplayed[i] + "";
				myGameVM.displayedCards[i - start].SetActive(true);
				myGameVM.displayedCards[i - start].GetComponent<GameCard>().Card = myGameVM.cards[myGameVM.cardsToBeDisplayed[i]]; 
				myGameVM.displayedCards[i - start].GetComponent<GameCard>().ShowFace();
				myGameVM.displayedCards[i - start].transform.Find("texturedGameCard")
					.FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();
			}
			else
			{
				myGameVM.displayedCards[i - start].SetActive(false);
			}
		}
	}
	
	private void clearCards()
	{
		for (int i = 0 ; i < 3 * myGameVM.nbCardsPerRow; i++) 
		{
			Destroy(myGameVM.displayedCards[i]);
		}
	}
	
	private void clearDeckCards()
	{
		for (int i = 0; i < 5; i++)
		{
			Destroy(myGameVM.displayedDeckCards[i]);
		}
	}
	
	private void createDeckCards()
	{
		float tempF                                                 = 10f * widthScreen / heightScreen;
		float width                                                 = tempF * 0.6f;
		myDecksVM.scaleDeck                                         = Mathf.Min(1.6f, width / 6f);
		float pas                                                   = (width - 5f * myDecksVM.scaleDeck) / 6f;
		float debutLargeur                                          = -0.3f * tempF + pas + myDecksVM.scaleDeck / 2;
		myGameVM.displayedDeckCards                                 = new GameObject[5];
		int nbDeckCardsToDisplay                                    = myGameVM.deckCardsIds.Count;

		for (int i = 0 ; i < 5 ; i++)
		{
			myGameVM.displayedDeckCards[i] = Instantiate(myGameScript.instance.CardObject) as GameObject;
			Destroy(myGameVM.displayedDeckCards[i].GetComponent<GameNetworkCard>());
			Destroy(myGameVM.displayedDeckCards[i].GetComponent<PhotonView>());
			myGameVM.displayedDeckCards[i].transform.localScale     = new Vector3(myDecksVM.scaleDeck, myDecksVM.scaleDeck, myDecksVM.scaleDeck); 
			myGameVM.displayedDeckCards[i].transform.localPosition  = new Vector3(debutLargeur + (myDecksVM.scaleDeck + pas) * i, 2.9f, 0); 
			myGameVM.displayedDeckCards[i].gameObject.name          = "DeckCard" + i + "";	
			if (i < nbDeckCardsToDisplay)
			{
				myGameVM.displayedDeckCards[i].GetComponent<GameCard>().Card = myGameVM.cards[myGameVM.deckCardsIds[i]]; 
				myGameVM.displayedDeckCards[i].GetComponent<GameCard>().ShowFace();
				myGameVM.displayedDeckCards[i].transform.Find("texturedGameCard")
					.FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();
			}   
			else
			{
				myGameVM.displayedDeckCards[i].SetActive(false);
			}
		}
		myGameVM.areCreatedDeckCards = true; 
	}
	
	private void displayDeckCards()
	{
		int nbDeckCardsToDisplay = myGameVM.deckCardsIds.Count;

		for (int i = 0 ; i < 5 ; i++)
		{
			if (i < nbDeckCardsToDisplay)
			{
				myGameVM.displayedDeckCards[i].SetActive (true);
				myGameVM.displayedDeckCards[i].GetComponent<GameCard>().Card = myGameVM.cards[myGameVM.deckCardsIds[i]]; 
				myGameVM.displayedDeckCards[i].GetComponent<GameCard>().ShowFace();
				myGameVM.displayedDeckCards[i].transform.Find("texturedGameCard")
					.FindChild("ExperienceArea").GetComponent<GameCard_experience>().setXpLevel();
			}   
			else
			{
				myGameVM.displayedDeckCards[i].SetActive(false);
			}
		}
	}

	public void sortCards()
	{	
		int tempA = new int();
		int tempB = new int();
		
		for (int i = 1 ; i < myGameVM.cardsToBeDisplayed.Count; i++) 
		{	
			for (int j = 0 ; j < i ; j++)
			{				
				switch (sortVM.sortSelected)
				{
					case 0:
						tempA = myGameVM.cards[myGameVM.cardsToBeDisplayed[i]].Life;
						tempB = myGameVM.cards[myGameVM.cardsToBeDisplayed[j]].Life;
						break;
					case 1:
						tempB = myGameVM.cards[myGameVM.cardsToBeDisplayed[i]].Life;
						tempA = myGameVM.cards[myGameVM.cardsToBeDisplayed[j]].Life;
						break;
					case 2:
						tempA = myGameVM.cards[myGameVM.cardsToBeDisplayed[i]].Attack;
						tempB = myGameVM.cards[myGameVM.cardsToBeDisplayed[j]].Attack;
						break;
					case 3:
						tempB = myGameVM.cards[myGameVM.cardsToBeDisplayed[i]].Attack;
						tempA = myGameVM.cards[myGameVM.cardsToBeDisplayed[j]].Attack;
						break;
					case 4:
						tempA = myGameVM.cards[myGameVM.cardsToBeDisplayed[i]].Move;
						tempB = myGameVM.cards[myGameVM.cardsToBeDisplayed[j]].Move;
						break;
					case 5:
						tempB = myGameVM.cards[myGameVM.cardsToBeDisplayed[i]].Move;
						tempA = myGameVM.cards[myGameVM.cardsToBeDisplayed[j]].Move;
						break;
					case 6:
						tempA = myGameVM.cards[myGameVM.cardsToBeDisplayed[i]].Speed;
						tempB = myGameVM.cards[myGameVM.cardsToBeDisplayed[j]].Speed;
						break;
					case 7:
						tempB = myGameVM.cards[myGameVM.cardsToBeDisplayed[i]].Speed;
						tempA = myGameVM.cards[myGameVM.cardsToBeDisplayed[j]].Speed;
						break;
					default:
						break;
				}

				if (tempA < tempB)
				{
					myGameVM.cardsToBeDisplayed.Insert(j, myGameVM.cardsToBeDisplayed[i]);
					myGameVM.cardsToBeDisplayed.RemoveAt(i + 1);
					break;
				}		
			}
		}	
	}
}

//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//public class MyGameView2 : MonoBehaviour
//{
//
//	public MyGameScreenViewModel myGameScreenVM;
//	public MyGameViewModel myGameVM;
//	public MyGameFiltersViewModel myGameFiltersVM;
//	public MyGameCardsViewModel myGameCardsVM;
//	public MyGameDecksViewModel myGameDecksVM;
//
//	public bool enVente = false;
//	public bool isSkillChosen = false;
//	public string valueSkill = "";
//
//	RaycastHit hit;
//	Ray ray;
//	Vector2 scrollPosition = new Vector2(0, 0);
//	int IDDeckToEdit = -1;
//	int deckToEdit = -1;
//	string tempText = "Nouveau deck";
//	bool displayCreationDeckWindow = false;
//	bool isSkillToDisplay = false;
//	bool isUpgradingCard = false;
//	bool isEscDown = false;
//	bool isRenamingCard = false;
//	bool isSellingCard = false;
//	bool isMarketingCard = false;
//	bool destroyRenamingCardWindow = false;
//	bool destroyFocus = false;
//	bool isChangingPrice;
//	bool destroyUpgradingCardWindow = false;
//
//
//	void Start()
//	{	 
//
//		this.myGameScreenVM= new MyGameScreenViewModel();
//		this.myGameVM=new MyGameViewModel ();
//		this.myGameFiltersVM = new MyGameFiltersViewModel();
//		this.myGameCardsVM =  MyGameCardsViewModel();
//		this.myGameDecksVM = MyGameDecksViewModel();
//	}
//
//	
//	void OnGUI()
//	{
//		if (myGameVM.displayDecks)
//		{
//			if (IDDeckToEdit != -1)
//			{
//				if (Event.current.keyCode == KeyCode.Escape)
//				{ 
//					IDDeckToEdit = -1;
//					tempText = "Nouveau deck";
//				} else if (Event.current.keyCode == KeyCode.Return)
//				{
//					StartCoroutine(myGameController.instance.deleteDeck(IDDeckToEdit));
//					tempText = "Nouveau deck";
//					IDDeckToEdit = -1;
//				} else
//				{
//					GUILayout.BeginArea(popupVM.centralWindow);
//					{
//						GUILayout.BeginVertical(popupVM.centralWindowStyle);
//						{
//							GUILayout.FlexibleSpace();
//							GUILayout.Label("Voulez-vous supprimer le deck ?", popupVM.centralWindowTitleStyle);
//							GUILayout.Space(0.02f * myGameController.instance.heightScreen);
//							GUILayout.BeginHorizontal();
//							{
//								GUILayout.Space(0.03f * myGameController.instance.widthScreen);
//								if (GUILayout.Button("Confirmer la suppression", popupVM.centralWindowButtonStyle))
//								{
//									StartCoroutine(myGameController.instance.deleteDeck(IDDeckToEdit));
//									tempText = "Nouveau deck";
//									IDDeckToEdit = -1;
//								}
//								GUILayout.Space(0.04f * myGameController.instance.widthScreen);
//								if (GUILayout.Button("Annuler", popupVM.centralWindowButtonStyle))
//								{
//									displayCreationDeckWindow = false; 
//									IDDeckToEdit = -1;
//								}
//								GUILayout.Space(0.03f * myGameController.instance.widthScreen);
//							}
//							GUILayout.EndHorizontal();
//							GUILayout.FlexibleSpace();
//						}
//						GUILayout.EndVertical();
//					}
//					GUILayout.EndArea();
//				}
//			}
//			if (displayCreationDeckWindow)
//			{	
//				if (Event.current.keyCode == KeyCode.Escape)
//				{
//					displayCreationDeckWindow = false;
//					tempText = "Nouveau deck";
//				} else if (Event.current.keyCode == KeyCode.Return)
//				{
//					StartCoroutine(myGameController.instance.addDeck(tempText));
//					tempText = "Nouveau deck";
//					displayCreationDeckWindow = false;
//				} else
//				{
//					GUILayout.BeginArea(popupVM.centralWindow);
//					{
//						GUILayout.BeginVertical(popupVM.centralWindowStyle);
//						{
//							GUILayout.FlexibleSpace();
//							GUILayout.Label("Choisissez le nom de votre nouveau deck", popupVM.centralWindowTitleStyle);
//							GUILayout.Space(0.02f * myGameController.instance.heightScreen);
//							GUILayout.BeginHorizontal();
//							{
//								GUILayout.Space(0.05f * myGameController.instance.widthScreen);
//								tempText = GUILayout.TextField(tempText, popupVM.centralWindowTextFieldStyle);
//							}
//							GUILayout.EndHorizontal();
//							GUILayout.Space(0.02f * myGameController.instance.heightScreen);
//							GUILayout.BeginHorizontal();
//							{
//								GUILayout.Space(0.03f * myGameController.instance.widthScreen);
//								if (GUILayout.Button("Créer le deck", popupVM.centralWindowButtonStyle))
//								{
//									StartCoroutine(myGameController.instance.addDeck(tempText));
//									tempText = "Nouveau deck";
//									displayCreationDeckWindow = false;
//								}
//								GUILayout.Space(0.04f * myGameController.instance.widthScreen);
//								if (GUILayout.Button("Annuler", popupVM.centralWindowButtonStyle))
//								{
//									displayCreationDeckWindow = false; 
//									tempText = "Nouveau deck";
//								}
//								GUILayout.Space(0.03f * myGameController.instance.widthScreen);
//							}
//							GUILayout.EndHorizontal();
//							GUILayout.FlexibleSpace();
//						}
//						GUILayout.EndVertical();
//					}
//					GUILayout.EndArea();
//				}
//			}
//			if (deckToEdit != -1)
//			{	
//				if (Event.current.keyCode == KeyCode.Escape)
//				{
//					deckToEdit = -1;
//					tempText = "Nouveau deck";
//				} else if (Event.current.keyCode == KeyCode.Return)
//				{
//					StartCoroutine(myGameController.instance.editDeck(deckToEdit, tempText));
//					tempText = "Nouveau deck";
//					deckToEdit = -1;
//				} else
//				{
//					GUILayout.BeginArea(popupVM.centralWindow);
//					{
//						GUILayout.BeginVertical(popupVM.centralWindowStyle);
//						{
//							GUILayout.FlexibleSpace();
//							GUILayout.Label("Modifiez le nom de votre deck", popupVM.centralWindowTitleStyle);
//							GUILayout.Space(0.02f * myGameController.instance.heightScreen);
//							GUILayout.BeginHorizontal();
//							{
//								GUILayout.Space(0.05f * myGameController.instance.widthScreen);
//								tempText = GUILayout.TextField(tempText, popupVM.centralWindowTextFieldStyle);
//							}
//							GUILayout.EndHorizontal();
//							GUILayout.Space(0.02f * myGameController.instance.heightScreen);
//							GUILayout.BeginHorizontal();
//							{
//								GUILayout.Space(0.03f * myGameController.instance.widthScreen);
//								if (GUILayout.Button("Modifier", popupVM.centralWindowButtonStyle))
//								{
//									StartCoroutine(myGameController.instance.editDeck(deckToEdit, tempText));
//									tempText = "Nouveau deck";
//									deckToEdit = -1;
//								}
//								GUILayout.Space(0.04f * myGameController.instance.widthScreen);
//								if (GUILayout.Button("Annuler", popupVM.centralWindowButtonStyle))
//								{
//									deckToEdit = -1; 
//									tempText = "Nouveau deck";
//								}
//								GUILayout.Space(0.03f * myGameController.instance.widthScreen);
//							}
//							GUILayout.EndHorizontal();
//							GUILayout.FlexibleSpace();
//						}
//						GUILayout.EndVertical();
//					}
//					GUILayout.EndArea();
//				}
//			}
//			GUILayout.BeginArea(myDecksVM.rectDeck);
//			{
//				GUILayout.BeginVertical();
//				{
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label(myDecksVM.decksTitle, myDecksVM.decksTitleStyle);
//						GUILayout.FlexibleSpace();
//						if (GUILayout.Button(myDecksVM.myNewDeckButtonTitle, myDecksVM.myNewDeckButton))
//						{
//							displayCreationDeckWindow = true;
//						}
//					}
//					GUILayout.EndHorizontal();
//					
//					GUILayout.Space(0.005f * myGameController.instance.heightScreen);
//					
//					scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(0.19f * myGameController.instance.widthScreen), 
//					                                                    GUILayout.Height(0.17f * myGameController.instance.heightScreen));
//					
//					for (int i = 0; i < myGameVM.myDecks.Count; i++)
//					{	
//						GUILayout.BeginHorizontal(myDecksVM.myDecksGuiStyle [i]);
//						{
//							if (GUILayout.Button("(" + myGameVM.myDecks [i].NbCards + ") " + myGameVM.myDecks [i].Name, myDecksVM.myDecksButtonGuiStyle [i]))
//							{
//								if (myDecksVM.chosenDeck != i)
//								{
//									myGameController.instance.changeDeck(i);
//								}
//							}
//							GUILayout.FlexibleSpace();
//							if (GUILayout.Button("", myDecksVM.myEditButtonStyle))
//							{
//								tempText = myGameVM.myDecks [i].Name;
//								deckToEdit = myGameVM.myDecks [i].Id;
//							}
//							
//							if (GUILayout.Button("", myDecksVM.mySuppressButtonStyle))
//							{
//								IDDeckToEdit = myGameVM.myDecks [i].Id;
//							}
//						}
//						GUILayout.EndHorizontal();
//					}
//					GUILayout.EndScrollView();
//				}
//				GUILayout.EndVertical();
//			}
//			GUILayout.EndArea();
//		}
//		if (myGameVM.displayLoader)
//		{
//			GUILayout.BeginArea(new Rect(myGameController.instance.widthScreen * 0.22f, 
//			                             0.26f * myGameController.instance.heightScreen, 
//			                             myGameController.instance.widthScreen * 0.78f, 
//			                             0.64f * myGameController.instance.heightScreen));
//			{
//				GUILayout.BeginVertical(); 
//				{
//					GUILayout.Label("Cartes en cours de chargement...   " + myGameVM.cardsToBeDisplayed.Count + " carte(s) chargee(s)");
//				}
//				GUILayout.EndVertical();
//			}
//			GUILayout.EndArea();
//		}
//		if (filterVM.displayFilters)
//		{
//			GUILayout.BeginArea(new Rect(myGameController.instance.widthScreen * 0.01f, 
//			                             0.965f * myGameController.instance.heightScreen, 
//			                             myGameController.instance.widthScreen * 0.78f, 
//			                             0.03f * myGameController.instance.heightScreen));
//			{
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.FlexibleSpace();
//					if (paginationVM.pageDebut > 0)
//					{
//						if (GUILayout.Button("...", paginationVM.paginationStyle))
//						{
//							myGameController.instance.changeSetPages(true);
//						}
//					}
//					GUILayout.Space(myGameController.instance.widthScreen * 0.01f);
//					for (int i = paginationVM.pageDebut; i < paginationVM.pageFin; i++)
//					{
//						if (GUILayout.Button("" + (i + 1), paginationVM.paginatorGuiStyle [i]))
//						{
//							myGameController.instance.changePage(i);
//						}
//						GUILayout.Space(myGameController.instance.widthScreen * 0.01f);
//					}
//					if (paginationVM.nbPages > paginationVM.pageFin)
//					{
//						if (GUILayout.Button("...", paginationVM.paginationStyle))
//						{
//							myGameController.instance.changeSetPages(false);
//						}
//					}
//					GUILayout.FlexibleSpace();
//				}
//				
//				GUILayout.EndHorizontal();
//			}
//			GUILayout.EndArea();
//			
//			GUILayout.BeginArea(new Rect(0.80f * myGameController.instance.widthScreen, 
//			                             0.11f * myGameController.instance.heightScreen, 
//			                             myGameController.instance.widthScreen * 0.19f, 
//			                             0.85f * myGameController.instance.heightScreen));
//			{
//				bool toggle;
//				string tempString;
//				GUILayout.BeginVertical();
//				{
//					GUILayout.FlexibleSpace(); 
//					toggle = GUILayout.Toggle(enVente, "Cartes en vente", filterVM.toggleStyle);
//					if (toggle != enVente)
//					{
//						enVente = toggle;
//						myGameController.instance.toReload();
//					}
//					
//					GUILayout.FlexibleSpace(); 
//					
//					GUILayout.Label("Filtrer par classe", filterVM.filterTitleStyle);
//					for (int i = 0; i < myGameVM.cardTypeList.Length - 1; i++)
//					{		
//						toggle = GUILayout.Toggle(myGameVM.togglesCurrentStates [i], myGameVM.cardTypeList [i], filterVM.toggleStyle);
//						if (toggle != myGameVM.togglesCurrentStates [i])
//						{
//							myGameController.instance.changeToggleStates(i, toggle);
//							if (toggle)
//							{
//								filterVM.filtersCardType.Add(i);
//							} else
//							{
//								filterVM.filtersCardType.Remove(i);
//							}
//							myGameController.instance.toReload();
//						}
//					}
//					
//					GUILayout.FlexibleSpace();
//					GUILayout.Label("Filtrer une capacité", filterVM.filterTitleStyle);
//					tempString = GUILayout.TextField(valueSkill, filterVM.filterTextFieldStyle);
//					if (tempString != valueSkill)
//					{
//						if (tempString.Length > 0)
//						{
//							isSkillToDisplay = true;
//							valueSkill = tempString.ToLower();
//							myGameController.instance.displaySkills();
//						} else
//						{
//							isSkillToDisplay = false;
//							valueSkill = "";
//						}
//						if (isSkillChosen)
//						{
//							isSkillChosen = false;
//							myGameController.instance.toReload();
//						}
//					}
//					if (isSkillToDisplay)
//					{
//						GUILayout.Space(-3);
//						for (int j = 0; j < filterVM.matchValues.Count; j++)
//						{
//							if (GUILayout.Button(filterVM.matchValues [j], filterVM.myStyle))
//							{
//								valueSkill = filterVM.matchValues [j].ToLower();
//								isSkillChosen = true;
//								myGameController.instance.matchValuesInit();
//								myGameController.instance.toReload();
//							}
//						}
//					}
//					
//					GUILayout.FlexibleSpace();			
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label("Filtrer par Vie", filterVM.filterTitleStyle);
//						GUILayout.FlexibleSpace();
//						if (GUILayout.Button("^", sortVM.sortButtonStyle [0]))
//						{
//							myGameController.instance.changeSort(0);
//						}
//						if (GUILayout.Button("v", sortVM.sortButtonStyle [1]))
//						{
//							myGameController.instance.changeSort(1);
//						}
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label("Min:" + Mathf.Round(filterVM.minLifeVal), filterVM.smallPoliceStyle);
//						GUILayout.FlexibleSpace();
//						GUILayout.Label("Max:" + Mathf.Round(filterVM.maxLifeVal), filterVM.smallPoliceStyle);
//					}
//					GUILayout.EndHorizontal();
//					MyGUI.MinMaxSlider(ref filterVM.minLifeVal, ref filterVM.maxLifeVal, filterVM.minLifeLimit, filterVM.maxLifeLimit);
//					
//					GUILayout.FlexibleSpace();
//					
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label("Filtrer par Attaque", filterVM.filterTitleStyle);
//						GUILayout.FlexibleSpace();
//						if (GUILayout.Button("^", sortVM.sortButtonStyle [2]))
//						{
//							myGameController.instance.changeSort(2);
//						}
//						if (GUILayout.Button("v", sortVM.sortButtonStyle [3]))
//						{
//							myGameController.instance.changeSort(3);
//						}
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label("Min:" + Mathf.Round(filterVM.minAttackVal), filterVM.smallPoliceStyle);
//						GUILayout.FlexibleSpace();
//						GUILayout.Label("Max:" + Mathf.Round(filterVM.maxAttackVal), filterVM.smallPoliceStyle);
//					}
//					GUILayout.EndHorizontal();
//					MyGUI.MinMaxSlider(ref filterVM.minAttackVal, ref filterVM.maxAttackVal, filterVM.minAttackLimit, filterVM.maxAttackLimit);
//					
//					GUILayout.FlexibleSpace();
//					
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label("Filtrer par Mouvement", filterVM.filterTitleStyle);
//						GUILayout.FlexibleSpace();
//						if (GUILayout.Button("^", sortVM.sortButtonStyle [4]))
//						{
//							myGameController.instance.changeSort(4);
//						}
//						if (GUILayout.Button("v", sortVM.sortButtonStyle [5]))
//						{
//							myGameController.instance.changeSort(5);
//						}
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label("Min:" + Mathf.Round(filterVM.minMoveVal), filterVM.smallPoliceStyle);
//						GUILayout.FlexibleSpace();
//						GUILayout.Label("Max:" + Mathf.Round(filterVM.maxMoveVal), filterVM.smallPoliceStyle);
//					}
//					GUILayout.EndHorizontal();
//					MyGUI.MinMaxSlider(ref filterVM.minMoveVal, ref filterVM.maxMoveVal, filterVM.minMoveLimit, filterVM.maxMoveLimit);
//					
//					GUILayout.FlexibleSpace();
//					
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label("Filtrer par Rapidité", filterVM.filterTitleStyle);
//						GUILayout.FlexibleSpace();
//						if (GUILayout.Button("^", sortVM.sortButtonStyle [6]))
//						{
//							myGameController.instance.changeSort(6);
//						}
//						if (GUILayout.Button("v", sortVM.sortButtonStyle [7]))
//						{
//							myGameController.instance.changeSort(7);
//						}
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label("Min:" + Mathf.Round(filterVM.minQuicknessVal), filterVM.smallPoliceStyle);
//						GUILayout.FlexibleSpace();
//						GUILayout.Label("Max:" + Mathf.Round(filterVM.maxQuicknessVal), filterVM.smallPoliceStyle);
//					}
//					GUILayout.EndHorizontal();
//					MyGUI.MinMaxSlider(ref filterVM.minQuicknessVal, ref filterVM.maxQuicknessVal, filterVM.minQuicknessLimit, filterVM.maxQuicknessLimit);
//				}
//				GUILayout.EndVertical();	
//			}
//			GUILayout.EndArea();
//		}	
//	}
//}

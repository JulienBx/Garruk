//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//
//public class MarketView : MonoBehaviour
//{
//	public MarketViewModel marketVM;
//	public MarketCardsViewModel marketCardsVM;
//	public MarketFiltersViewModel marketFiltersVM;
//	public MarketScreenViewModel marketScreenVM;
//	
//	void Start()
//	{
//		this.marketVM = new MarketViewModel ();
//		this.marketCardsVM = new MarketCardsViewModel ();
//		this.marketFiltersVM = new MarketFiltersViewModel ();
//		this.marketScreenVM = new MarketScreenViewModel ();
//
//	}
//	void Update()
//	{
//		if (Screen.width != marketScreenVM.widthScreen || Screen.height != marketScreenVM.heightScreen) {
//			MarketController.instance.loadData();
//		}
//		if(Input.GetKeyDown(KeyCode.Return)) 
//		{
//			MarketController.instance.returnPressed();
//		}
//		if(Input.GetKeyDown(KeyCode.Escape)) 
//		{
//			MarketController.instance.escapePressed();
//		}
//		if(Input.GetMouseButtonDown(0))
//		{
//			MarketController.instance.isBeingDragged ();
//		}
//		if(Input.GetMouseButtonUp(0))
//		{
//			MarketController.instance.isNotBeingDragged ();
//		}
//	}
//	void OnGUI()
//	{
//		GUI.enabled = marketVM.guiEnabled;
//		if(marketVM.displayView)
//		{
//			GUILayout.BeginArea(new Rect(marketScreenVM.blockLeft.min.x,
//			                             marketScreenVM.blockLeft.min.y,
//			                             marketScreenVM.blockLeftWidth,
//			                             marketScreenVM.blockLeftHeight));
//			{
//				GUILayout.BeginHorizontal();
//				{
//					if(marketCardsVM.newCardsToDisplay)
//					{	
//						if(GUILayout.Button(marketCardsVM.newCardsLabel,marketCardsVM.newCardsButtonStyle))
//						{
//							MarketController.instance.displayNewCards();
//						}
//					}
//				}
//				GUILayout.BeginHorizontal();
//			}
//			GUILayout.EndArea();
//			GUILayout.BeginArea(new Rect(marketScreenVM.blockLeft.min.x,
//			                              marketScreenVM.blockLeft.min.y,
//			                              marketScreenVM.blockLeftWidth,
//			                              marketScreenVM.blockLeftHeight*0.03f));
//			{
//				if(marketCardsVM.nbCards>marketCardsVM.cardsToBeDisplayed.Count)
//				{	
//					GUILayout.Label(marketCardsVM.cardsToBeDisplayed.Count + " résultat(s) affichée(s) / "+marketCardsVM.nbCards,marketCardsVM.nbCardsLabelStyle);
//				}
//			}
//			GUILayout.EndArea();
//			GUILayout.BeginArea(new Rect(marketScreenVM.blockLeft.min.x,
//			                             marketScreenVM.blockLeft.min.y+marketScreenVM.blockLeftHeight*0.965f,
//			                             marketScreenVM.blockLeftWidth,
//			                             marketScreenVM.blockLeftHeight*0.0325f));
//			{
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.FlexibleSpace();
//					if (marketCardsVM.pageDebut>0)
//					{
//						if (GUILayout.Button("...",marketVM.paginationStyle
//						                     ,GUILayout.Height(marketScreenVM.heightScreen*3/100)
//						                     ,GUILayout.Width(marketScreenVM.widthScreen*2/100)))
//						{
//							MarketController.instance.paginationBack();
//						}
//					}
//					GUILayout.Space(marketScreenVM.widthScreen*0.01f);
//					if(marketCardsVM.pageFin>1)
//					{
//						for (int i = marketCardsVM.pageDebut ; i < marketCardsVM.pageFin ; i++)
//						{
//							if (GUILayout.Button(""+(i+1),marketCardsVM.paginatorGuiStyle[i]
//							                     ,GUILayout.Height(marketScreenVM.heightScreen*3/100)
//							                     ,GUILayout.Width(marketScreenVM.widthScreen*2/100)))
//							{
//								MarketController.instance.paginationSelect(i);
//							}
//							GUILayout.Space(marketScreenVM.widthScreen*0.01f);
//						}
//					}
//					if (marketCardsVM.nbPages>marketCardsVM.pageFin)
//					{
//						if (GUILayout.Button("...",marketVM.paginationStyle
//						                     ,GUILayout.Height(marketScreenVM.heightScreen*3/100)
//						                     ,GUILayout.Width(marketScreenVM.widthScreen*2/100)))
//						{
//							MarketController.instance.paginationNext();
//						}
//					}
//					GUILayout.FlexibleSpace();
//				}
//				GUILayout.EndHorizontal();
//				GUILayout.FlexibleSpace();
//			}
//			GUILayout.EndArea();
//			GUILayout.BeginArea(marketScreenVM.blockRight);
//			{
//				bool toggle;
//				string tempString ;
//				string tempMinPrice ;
//				string tempMaxPrice ;
//				GUILayout.BeginVertical();
//				{
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label ("Filtrer par Prix",marketFiltersVM.filterTitleStyle);
//						GUILayout.FlexibleSpace();
//						if(GUILayout.Button ("^",marketFiltersVM.sortButtonStyle[0],GUILayout.Width(marketScreenVM.blockRightWidth*7/100))) 
//						{
//							MarketController.instance.sortCards(0);
//						}
//						GUILayout.Space (marketScreenVM.blockRightWidth*2/100);
//						if(GUILayout.Button ("v",marketFiltersVM.sortButtonStyle[1],GUILayout.Width(marketScreenVM.blockRightWidth*7/100))) 
//						{
//							MarketController.instance.sortCards(1);
//						}
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.Label ("Prix min :",marketFiltersVM.minmaxPriceStyle);
//					tempMinPrice = GUILayout.TextField(marketFiltersVM.minPrice, 9,marketFiltersVM.textFieldStyle);
//					if (tempMinPrice != marketFiltersVM.minPrice) 
//					{
//						MarketController.instance.editMinPrice(tempMinPrice);
//					}
//					GUILayout.Label ("Prix max :",marketFiltersVM.minmaxPriceStyle);
//					tempMaxPrice = GUILayout.TextField(marketFiltersVM.maxPrice, 9,marketFiltersVM.textFieldStyle);
//					if (tempMaxPrice != marketFiltersVM.maxPrice) 
//					{
//						MarketController.instance.editMaxPrice(tempMaxPrice);
//					}
//					GUILayout.FlexibleSpace();
//					GUILayout.Label ("Filtrer par classe",marketFiltersVM.filterTitleStyle);
//					for (int i=0; i<marketFiltersVM.cardTypeList.Length-1; i++) {		
//						toggle = GUILayout.Toggle (marketFiltersVM.togglesCurrentStates [i],marketFiltersVM.cardTypeList[i],marketFiltersVM.toggleStyle);
//						if (toggle != marketFiltersVM.togglesCurrentStates [i]) 
//						{
//							MarketController.instance.selectCardType(toggle,i);
//						}
//					}
//					GUILayout.FlexibleSpace();
//					GUILayout.Label ("Filtrer une capacité",marketFiltersVM.filterTitleStyle);
//					tempString = GUILayout.TextField (marketFiltersVM.valueSkill, marketFiltersVM.textFieldStyle);
//
//					if (tempString != marketFiltersVM.valueSkill) 
//					{
//						MarketController.instance.selectSkills(tempString);
//					}
//					if (marketFiltersVM.isSkillToDisplay)
//					{
//						GUILayout.Space(-3);
//						for (int j=0; j<marketFiltersVM.matchValues.Count; j++) 
//						{
//							if (GUILayout.Button (marketFiltersVM.matchValues [j], marketFiltersVM.skillListStyle)) 
//							{
//								MarketController.instance.filterASkill(marketFiltersVM.matchValues [j]);
//							}
//						}
//					}
//
//					GUILayout.FlexibleSpace();
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label ("Filtrer par Vie",marketFiltersVM.filterTitleStyle);
//						GUILayout.FlexibleSpace();
//						if(GUILayout.Button ("^",marketFiltersVM.sortButtonStyle[2],GUILayout.Width(marketScreenVM.blockRightWidth*7/100))) 
//						{
//							MarketController.instance.sortCards(2);
//						}
//						GUILayout.Space (marketScreenVM.blockRightWidth*2/100);
//						if(GUILayout.Button ("v",marketFiltersVM.sortButtonStyle[3],GUILayout.Width(marketScreenVM.blockRightWidth*7/100))) 
//						{
//							MarketController.instance.sortCards(3);
//						}
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.Space(-1);
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label ("Min:"+ Mathf.Round(marketFiltersVM.minLifeVal),marketFiltersVM.smallPoliceStyle);
//						GUILayout.FlexibleSpace();
//						GUILayout.Label ("Max:"+ Mathf.Round(marketFiltersVM.maxLifeVal),marketFiltersVM.smallPoliceStyle);
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.Space(-5);
//					MyGUI.MinMaxSlider (ref marketFiltersVM.minLifeVal, ref marketFiltersVM.maxLifeVal, marketFiltersVM.minLifeLimit, marketFiltersVM.maxLifeLimit);
//
//					GUILayout.FlexibleSpace();
//					
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label ("Filtrer par Attaque",marketFiltersVM.filterTitleStyle);
//						GUILayout.FlexibleSpace();
//						if(GUILayout.Button ("^",marketFiltersVM.sortButtonStyle[4],GUILayout.Width(marketScreenVM.blockRightWidth*7/100))) 
//						{
//							MarketController.instance.sortCards(4);
//						}
//						GUILayout.Space (marketScreenVM.blockRightWidth*2/100);
//						if(GUILayout.Button ("v",marketFiltersVM.sortButtonStyle[5],GUILayout.Width(marketScreenVM.blockRightWidth*7/100))) 
//						{
//							MarketController.instance.sortCards(5);
//						}
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.Space(-1);
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label ("Min:"+ Mathf.Round(marketFiltersVM.minAttackVal),marketFiltersVM.smallPoliceStyle);
//						GUILayout.FlexibleSpace();
//						GUILayout.Label ("Max:"+ Mathf.Round(marketFiltersVM.maxAttackVal),marketFiltersVM.smallPoliceStyle);
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.Space(-5);
//					MyGUI.MinMaxSlider (ref marketFiltersVM.minAttackVal, ref marketFiltersVM.maxAttackVal, marketFiltersVM.minAttackLimit, marketFiltersVM.maxAttackLimit);
//					
//					GUILayout.FlexibleSpace();
//					
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label ("Filtrer par Rapidité",marketFiltersVM.filterTitleStyle);
//						GUILayout.FlexibleSpace();
//						if(GUILayout.Button ("^",marketFiltersVM.sortButtonStyle[6],GUILayout.Width(marketScreenVM.blockRightWidth*7/100))) 
//						{
//							MarketController.instance.sortCards(6);
//						}
//						GUILayout.Space (marketScreenVM.blockRightWidth*2/100);
//						if(GUILayout.Button ("v",marketFiltersVM.sortButtonStyle[7],GUILayout.Width(marketScreenVM.blockRightWidth*7/100))) 
//						{
//							MarketController.instance.sortCards(7);
//						}
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.Space(-1);
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label ("Min:"+ Mathf.Round(marketFiltersVM.minQuicknessVal),marketFiltersVM.smallPoliceStyle);
//						GUILayout.FlexibleSpace();
//						GUILayout.Label ("Max:"+ Mathf.Round(marketFiltersVM.maxQuicknessVal),marketFiltersVM.smallPoliceStyle);
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.Space(-5);
//					MyGUI.MinMaxSlider (ref marketFiltersVM.minQuicknessVal, ref marketFiltersVM.maxQuicknessVal, marketFiltersVM.minQuicknessLimit, marketFiltersVM.maxQuicknessLimit);
//					
//					GUILayout.FlexibleSpace();
//				}
//				GUILayout.EndVertical();
//			}
//			GUILayout.EndArea();
//		}
//	}
//}
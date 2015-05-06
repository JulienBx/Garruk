using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MyGameView : MonoBehaviour
{
	public MyGameScreenViewModel myGameScreenVM;
	public MyGameViewModel myGameVM;
	public MyGameFiltersViewModel myGameFiltersVM;
	public MyGameCardsViewModel myGameCardsVM;
	public MyGameDecksViewModel myGameDecksVM;
	public MyGameDeckCardsViewModel myGameDeckCardsVM;
	
	void Start()
	{
		this.myGameScreenVM= new MyGameScreenViewModel();
		this.myGameVM=new MyGameViewModel ();
		this.myGameFiltersVM = new MyGameFiltersViewModel();
		this.myGameCardsVM =  new MyGameCardsViewModel();
		this.myGameDecksVM = new MyGameDecksViewModel();
		this.myGameDeckCardsVM = new MyGameDeckCardsViewModel ();
	}
	void Update()
	{
		if (Screen.width != myGameScreenVM.widthScreen || Screen.height != myGameScreenVM.heightScreen) {
			MyGameController.instance.loadAll();
		}
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			MyGameController.instance.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			MyGameController.instance.escapePressed();
		}
		if(Input.GetMouseButtonDown(0))
		{
			MyGameController.instance.isBeingDragged ();
		}
		if(Input.GetMouseButtonUp(0))
		{
			MyGameController.instance.isNotBeingDragged ();
		}
	}
	void OnGUI()
	{
		GUI.enabled = myGameVM.guiEnabled;
		if(myGameVM.displayView)
		{
			GUILayout.BeginArea(new Rect(myGameScreenVM.blockCards.min.x,
			                             myGameScreenVM.blockDeckCards.min.y+ (myGameScreenVM.blockCardsHeight+myGameScreenVM.blockDeckCardsHeight+myGameScreenVM.gapBetweenblocks)*0.97f,
			                             myGameScreenVM.blockCardsWidth,
			                             (myGameScreenVM.blockCardsHeight+myGameScreenVM.blockDeckCardsHeight+myGameScreenVM.gapBetweenblocks)*0.03f));
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (myGameCardsVM.pageDebut>0)
					{
						if (GUILayout.Button("...",myGameVM.paginationStyle
						                     ,GUILayout.Height(myGameScreenVM.heightScreen*3/100)
						                     ,GUILayout.Width(myGameScreenVM.widthScreen*2/100)))
						{
							MyGameController.instance.paginationBack();
						}
					}
					GUILayout.Space(myGameScreenVM.widthScreen*0.01f);
					for (int i = myGameCardsVM.pageDebut ; i < myGameCardsVM.pageFin ; i++)
					{
						if (GUILayout.Button(""+(i+1),myGameCardsVM.paginatorGuiStyle[i]
						                     ,GUILayout.Height(myGameScreenVM.heightScreen*3/100)
						                     ,GUILayout.Width(myGameScreenVM.widthScreen*2/100)))
						{
							MyGameController.instance.paginationSelect(i);
						}
						GUILayout.Space(myGameScreenVM.widthScreen*0.01f);
					}
					if (myGameCardsVM.nbPages>myGameCardsVM.pageFin)
					{
						if (GUILayout.Button("...",myGameVM.paginationStyle
						                     ,GUILayout.Height(myGameScreenVM.heightScreen*3/100)
						                     ,GUILayout.Width(myGameScreenVM.widthScreen*2/100)))
						{
							MyGameController.instance.paginationNext();
						}
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(myGameScreenVM.blockFilters);
			{
				bool toggle;
				string tempString ;
				string tempMinPrice ;
				string tempMaxPrice ;
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace(); 
					toggle = GUILayout.Toggle(myGameFiltersVM.onSale, "Cartes en vente", myGameFiltersVM.toggleStyle);
					if (toggle != myGameFiltersVM.onSale)
					{
						MyGameController.instance.selectOnSale(toggle);
					}
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Filtrer par classe",myGameFiltersVM.filterTitleStyle);
					for (int i=0; i<myGameFiltersVM.cardTypeList.Length-1; i++) 
					{		
						toggle = GUILayout.Toggle (myGameFiltersVM.togglesCurrentStates [i],myGameFiltersVM.cardTypeList[i],myGameFiltersVM.toggleStyle);
						if (toggle != myGameFiltersVM.togglesCurrentStates [i]) 
						{
							MyGameController.instance.selectCardType(toggle,i);
						}
					}
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Filtrer une capacité",myGameFiltersVM.filterTitleStyle);
					tempString = GUILayout.TextField (myGameFiltersVM.valueSkill, myGameFiltersVM.textFieldStyle);
					
					if (tempString != myGameFiltersVM.valueSkill) 
					{
						MyGameController.instance.selectSkills(tempString);
					}
					if (myGameFiltersVM.isSkillToDisplay)
					{
						GUILayout.Space(-3);
						for (int j=0; j<myGameFiltersVM.matchValues.Count; j++) 
						{
							if (GUILayout.Button (myGameFiltersVM.matchValues [j], myGameFiltersVM.skillListStyle)) 
							{
								MyGameController.instance.filterASkill(myGameFiltersVM.matchValues [j]);
							}
						}
					}
					
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Vie",myGameFiltersVM.filterTitleStyle);
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("^",myGameFiltersVM.sortButtonStyle[0],GUILayout.Width(myGameScreenVM.blockFiltersWidth*7/100))) 
						{
							MyGameController.instance.sortCards(0);
						}
						GUILayout.Space (myGameScreenVM.blockFiltersWidth*2/100);
						if(GUILayout.Button ("v",myGameFiltersVM.sortButtonStyle[1],GUILayout.Width(myGameScreenVM.blockFiltersWidth*7/100))) 
						{
							MyGameController.instance.sortCards(1);
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-1);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:"+ Mathf.Round(myGameFiltersVM.minLifeVal),myGameFiltersVM.smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:"+ Mathf.Round(myGameFiltersVM.maxLifeVal),myGameFiltersVM.smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-5);
					MyGUI.MinMaxSlider (ref myGameFiltersVM.minLifeVal, ref myGameFiltersVM.maxLifeVal, myGameFiltersVM.minLifeLimit, myGameFiltersVM.maxLifeLimit);
					
					GUILayout.FlexibleSpace();
					
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Attaque",myGameFiltersVM.filterTitleStyle);
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("^",myGameFiltersVM.sortButtonStyle[2],GUILayout.Width(myGameScreenVM.blockFiltersWidth*7/100))) 
						{
							MyGameController.instance.sortCards(2);
						}
						GUILayout.Space (myGameScreenVM.blockFiltersWidth*2/100);
						if(GUILayout.Button ("v",myGameFiltersVM.sortButtonStyle[3],GUILayout.Width(myGameScreenVM.blockFiltersWidth*7/100))) 
						{
							MyGameController.instance.sortCards(3);
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-1);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:"+ Mathf.Round(myGameFiltersVM.minAttackVal),myGameFiltersVM.smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:"+ Mathf.Round(myGameFiltersVM.maxAttackVal),myGameFiltersVM.smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-5);
					MyGUI.MinMaxSlider (ref myGameFiltersVM.minAttackVal, ref myGameFiltersVM.maxAttackVal, myGameFiltersVM.minAttackLimit, myGameFiltersVM.maxAttackLimit);
					
					GUILayout.FlexibleSpace();
					
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Mouvement",myGameFiltersVM.filterTitleStyle);
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("^",myGameFiltersVM.sortButtonStyle[4],GUILayout.Width(myGameScreenVM.blockFiltersWidth*7/100))) 
						{
							MyGameController.instance.sortCards(4);
						}
						GUILayout.Space (myGameScreenVM.blockFiltersWidth*2/100);
						if(GUILayout.Button ("v",myGameFiltersVM.sortButtonStyle[5],GUILayout.Width(myGameScreenVM.blockFiltersWidth*7/100))) 
						{
							MyGameController.instance.sortCards(5);
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-1);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:"+ Mathf.Round(myGameFiltersVM.minMoveVal),myGameFiltersVM.smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:"+ Mathf.Round(myGameFiltersVM.maxMoveVal),myGameFiltersVM.smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-5);
					MyGUI.MinMaxSlider (ref myGameFiltersVM.minMoveVal, ref myGameFiltersVM.maxMoveVal, myGameFiltersVM.minMoveLimit, myGameFiltersVM.maxMoveLimit);
					
					GUILayout.FlexibleSpace();
					
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Filtrer par Rapidité",myGameFiltersVM.filterTitleStyle);
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("^",myGameFiltersVM.sortButtonStyle[6],GUILayout.Width(myGameScreenVM.blockFiltersWidth*7/100))) 
						{
							MyGameController.instance.sortCards(6);
						}
						GUILayout.Space (myGameScreenVM.blockFiltersWidth*2/100);
						if(GUILayout.Button ("v",myGameFiltersVM.sortButtonStyle[7],GUILayout.Width(myGameScreenVM.blockFiltersWidth*7/100))) 
						{
							MyGameController.instance.sortCards(7);
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-1);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min:"+ Mathf.Round(myGameFiltersVM.minQuicknessVal),myGameFiltersVM.smallPoliceStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max:"+ Mathf.Round(myGameFiltersVM.maxQuicknessVal),myGameFiltersVM.smallPoliceStyle);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(-5);
					MyGUI.MinMaxSlider (ref myGameFiltersVM.minQuicknessVal, ref myGameFiltersVM.maxQuicknessVal, myGameFiltersVM.minQuicknessLimit, myGameFiltersVM.maxQuicknessLimit);
					
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(myGameScreenVM.blockDecks);
			{
				GUILayout.BeginVertical();
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label(myGameDecksVM.decksTitle, myGameDecksVM.decksTitleStyle,GUILayout.Height(0.17f * myGameScreenVM.blockDecksHeight));
						if (GUILayout.Button(myGameDecksVM.myNewDeckButtonTitle, myGameDecksVM.myNewDeckButtonStyle,GUILayout.Height(0.17f * myGameScreenVM.blockDecksHeight)))
						{

						}
					}
					GUILayout.EndHorizontal();

					GUILayout.Space(0.015f * myGameScreenVM.blockDecksHeight);
					
					myGameDecksVM.scrollPosition = GUILayout.BeginScrollView(myGameDecksVM.scrollPosition,GUILayout.Height(4*0.17f * myGameScreenVM.blockDecksHeight));
					
					for (int i = 0; i < myGameDecksVM.decksToBeDisplayed.Count; i++)
					{	
						GUILayout.BeginHorizontal(myGameDecksVM.myDecksGuiStyle [i]);
						{
							if (GUILayout.Button("(" + myGameDecksVM.decksNbCards [i] + ") " + myGameDecksVM.decksName [i], myGameDecksVM.myDecksButtonGuiStyle [i],GUILayout.Height(0.17f * myGameScreenVM.blockDecksHeight)))
							{
								if (myGameDecksVM.chosenDeck != i)
								{
									MyGameController.instance.displayDeck(i);
								}
							}
							GUILayout.Space(-myGameScreenVM.blockDecksHeight*2/6);
							if (GUILayout.Button("", myGameDecksVM.myEditButtonStyle,
							                     GUILayout.Width(0.17f * myGameScreenVM.blockDecksHeight),
							                     GUILayout.Height(0.17f * myGameScreenVM.blockDecksHeight)))
							{

							}
							if (GUILayout.Button("", myGameDecksVM.mySuppressButtonStyle,
							                     GUILayout.Width(0.17f * myGameScreenVM.blockDecksHeight),
							                     GUILayout.Height(0.17f * myGameScreenVM.blockDecksHeight)))
							{

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
	}
}
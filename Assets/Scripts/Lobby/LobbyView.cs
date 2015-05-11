using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public class LobbyView : MonoBehaviour
{

	public LobbyScreenViewModel screenVM;
	public LobbyDecksViewModel decksVM;
	public LobbyDeckCardsViewModel decksCardVM;
	public LobbyOpponentViewModel opponentVM;
	public LobbyResultsViewModel resultsVM;
	public LobbyViewModel lobbyVM;
	public LobbyDivisionGameViewModel divisionGameVM;
	public LobbyFriendlyGameViewModel friendlyGameVM;
	public LobbyCupGameViewModel cupGameVM;
	public LobbyPlayersViewModel playersVM;
	
	public LobbyView ()
	{
		this.screenVM = new LobbyScreenViewModel ();
		this.decksVM = new LobbyDecksViewModel ();
		this.decksCardVM = new LobbyDeckCardsViewModel ();
		this.opponentVM = new LobbyOpponentViewModel ();
		this.resultsVM = new LobbyResultsViewModel ();
		this.lobbyVM = new LobbyViewModel ();
		this.divisionGameVM = new LobbyDivisionGameViewModel ();
		this.friendlyGameVM = new LobbyFriendlyGameViewModel ();
		this.cupGameVM = new LobbyCupGameViewModel ();
		this.playersVM = new LobbyPlayersViewModel ();
	}
	void Update()
	{
		if (Screen.width != screenVM.widthScreen || Screen.height != screenVM.heightScreen) {
			LobbyController.instance.loadAll();
		}
		if(Input.GetKeyDown(KeyCode.Return)) 
		{
			LobbyController.instance.returnPressed();
		}
		if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			LobbyController.instance.escapePressed();
		}
	}
	void OnGUI()
	{
		if(lobbyVM.displayView)
		{
			GUI.depth=-5;
			GUI.enabled=lobbyVM.guiEnabled;
			GUILayout.BeginArea(screenVM.blockTopCenter);
			{
				if(decksCardVM.noDeckLabel!="")
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(decksCardVM.noDeckLabel,decksCardVM.noDeckLabelStyle);
					GUILayout.FlexibleSpace();
				}
				else
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.BeginVertical(GUILayout.Width(screenVM.blockTopCenterWidth*0.2f));
						{
							GUILayout.Label(decksVM.decksTitle, decksVM.decksTitleStyle,GUILayout.Height(0.17f * screenVM.blockTopCenterHeight));
							
							GUILayout.Space(0.015f * screenVM.blockTopCenterHeight);
							
							decksVM.scrollPosition = GUILayout.BeginScrollView(decksVM.scrollPosition,GUILayout.Height(4*0.17f * screenVM.blockTopCenterHeight));
							
							for (int i = 0; i < decksVM.decksToBeDisplayed.Count; i++)
							{	
								GUILayout.BeginHorizontal();
								{
									if (GUILayout.Button(decksVM.decksName [i], decksVM.myDecksButtonGuiStyle [i],GUILayout.Height(0.17f * screenVM.blockTopCenterHeight)))
									{
										if (decksVM.chosenDeck != i)
										{
											LobbyController.instance.displayDeck(i);
										}
									}
								}
								GUILayout.EndHorizontal();
							}
							GUILayout.EndScrollView();
						}
						GUILayout.EndVertical();
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
				}
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(screenVM.blockMiddleTop);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label(playersVM.label,playersVM.labelStyle);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			GUI.enabled=lobbyVM.gameButtonsEnabled;
			GUILayout.BeginArea(screenVM.blockMiddleLeft);
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button("",friendlyGameVM.buttonStyle,GUILayout.Width(9f/10f*screenVM.blockMiddleLeftHeight),GUILayout.Height(9f/10f*screenVM.blockMiddleLeftHeight)))
					{
						LobbyController.instance.joinFriendlyGame();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.Label("Match amical",friendlyGameVM.labelStyle,GUILayout.Height(1f/10f*screenVM.blockMiddleLeftHeight));
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(screenVM.blockMiddleCenter);
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button(divisionGameVM.divisionInformationsLabel,divisionGameVM.buttonStyle,GUILayout.Width(9f/10f*screenVM.blockMiddleCenterHeight),GUILayout.Height(9f/10f*screenVM.blockMiddleCenterHeight)))
					{
						LobbyController.instance.joinDivisionGame();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.Label("Match de division",divisionGameVM.labelStyle,GUILayout.Height(1f/10f*screenVM.blockMiddleCenterHeight));
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(screenVM.blockMiddleRight);
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button(cupGameVM.cupInformationsLabel,cupGameVM.buttonStyle,GUILayout.Width(9f/10f*screenVM.blockMiddleRightHeight),GUILayout.Height(9f/10f*screenVM.blockMiddleRightHeight)))
					{
						LobbyController.instance.joinCupGame();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.Label("Match de coupe",cupGameVM.labelStyle,GUILayout.Height(1f/10f*screenVM.blockMiddleRightHeight));
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			GUI.enabled=lobbyVM.guiEnabled;
			GUILayout.BeginArea(screenVM.blockBottom);
			{
				GUILayout.Label(resultsVM.resultsTitle, resultsVM.resultsTitleStyle,GUILayout.Height(0.2f * screenVM.blockBottomHeight));
				GUILayout.BeginHorizontal();
				{
					GUILayout.BeginVertical(GUILayout.Width(screenVM.blockBottomWidth*0.30f));
					{
						resultsVM.scrollPosition = GUILayout.BeginScrollView(resultsVM.scrollPosition,GUILayout.Height(4*0.2f * screenVM.blockBottomHeight));
						
						for (int i = 0; i < resultsVM.resultsLabel.Count; i++)
						{	
							GUILayout.BeginHorizontal();
							{
								if (GUILayout.Button("", resultsVM.resultsGameTypeStyles [i],GUILayout.Height(0.2f * screenVM.blockBottomHeight),GUILayout.Width(0.2f * screenVM.blockBottomHeight)))
								{
								}
								if (GUILayout.Button(resultsVM.resultsLabel[i], resultsVM.resultsStyles[i],GUILayout.Height(0.2f * screenVM.blockBottomHeight)))
								{
								}
								if (GUILayout.Button(">", resultsVM.focusButtonStyles[i],GUILayout.Height(0.2f * screenVM.blockBottomHeight),GUILayout.Width(0.2f * screenVM.blockBottomHeight)))
								{
									if (resultsVM.chosenResult != i)
									{
										LobbyController.instance.displayOpponent(i);
									}
								}
							}
							GUILayout.EndHorizontal();
						}
						GUILayout.EndScrollView();
					}
					GUILayout.EndVertical();
					GUILayout.BeginVertical();
					{
						GUILayout.BeginHorizontal(opponentVM.backgroundStyle);
						{
							GUILayout.Space(screenVM.blockBottomWidth*5/100);
							if(GUILayout.Button("",opponentVM.profilePictureButtonStyle,GUILayout.Height(4*0.2f * screenVM.blockBottomHeight),GUILayout.Width(4*0.2f * screenVM.blockBottomHeight)))
							{
								ApplicationModel.profileChosen=opponentVM.username;
								Application.LoadLevel("profile");
							}
							GUILayout.Space(screenVM.blockBottomWidth*5/100);
							GUILayout.BeginVertical();
							{
								GUILayout.FlexibleSpace();
								GUILayout.Label (opponentVM.username,opponentVM.usernameStyle);
								GUILayout.FlexibleSpace();
								GUILayout.FlexibleSpace();
								GUILayout.Label ("Victoires : "+opponentVM.totalNbWins,opponentVM.informationsStyle);
								GUILayout.FlexibleSpace();
								GUILayout.Label ("Défaites : "+opponentVM.totalNbLooses,opponentVM.informationsStyle);
								GUILayout.FlexibleSpace();
								GUILayout.Label ("Ranking : "+opponentVM.ranking,opponentVM.informationsStyle);
								GUILayout.FlexibleSpace();
								GUILayout.Label ("Ranking Points : "+opponentVM.rankingPoints,opponentVM.informationsStyle);
								GUILayout.FlexibleSpace();
								GUILayout.Label ("Division : "+opponentVM.currentDivision,opponentVM.informationsStyle);
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndVertical();
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}
	}
}


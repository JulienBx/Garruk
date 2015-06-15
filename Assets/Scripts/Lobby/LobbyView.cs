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
							
							GUI.enabled=lobbyVM.buttonsEnabled[0];
							for (int i = 0; i < decksVM.decksToBeDisplayed.Count; i++)
							{	
								GUILayout.BeginHorizontal();
								{
									if (GUILayout.Button(decksVM.decksName [i], decksVM.myDecksButtonGuiStyle [i],GUILayout.Height(0.17f * screenVM.blockTopCenterHeight)))
									{
										if (decksVM.chosenDeck != i)
										{
											LobbyController.instance.displayDeckHandler(i);
										}
									}
								}
								GUILayout.EndHorizontal();
							}
							GUILayout.EndScrollView();
							GUI.enabled=lobbyVM.guiEnabled;
						}
						GUILayout.EndVertical();
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
				}
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(screenVM.blockBottom);
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label(playersVM.label,playersVM.labelStyle);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(screenVM.blockMiddleLeft);
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUI.enabled=lobbyVM.buttonsEnabled[1];
					if(GUILayout.Button("",friendlyGameVM.buttonStyle,GUILayout.Width(5f/10f*screenVM.blockMiddleLeftHeight),GUILayout.Height(5f/10f*screenVM.blockMiddleLeftHeight)))
					{
						LobbyController.instance.joinFriendlyGame();
					}
					GUILayout.FlexibleSpace();
					GUI.enabled=lobbyVM.guiEnabled;
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
					GUI.enabled=lobbyVM.buttonsEnabled[2];
					if(GUILayout.Button(divisionGameVM.divisionInformationsLabel,divisionGameVM.buttonStyle,GUILayout.Width(5f/10f*screenVM.blockMiddleCenterHeight),GUILayout.Height(5f/10f*screenVM.blockMiddleCenterHeight)))
					{
						LobbyController.instance.joinDivisionLobby();
					}
					GUILayout.FlexibleSpace();
					GUI.enabled=lobbyVM.guiEnabled;
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
					GUI.enabled=lobbyVM.buttonsEnabled[3];
					if(GUILayout.Button(cupGameVM.cupInformationsLabel,cupGameVM.buttonStyle,GUILayout.Width(5f/10f*screenVM.blockMiddleRightHeight),GUILayout.Height(5f/10f*screenVM.blockMiddleRightHeight)))
					{
						LobbyController.instance.joinCupGame();
					}
					GUILayout.FlexibleSpace();
					GUI.enabled=lobbyVM.guiEnabled;
				}
				GUILayout.EndHorizontal();
				GUILayout.Label("Match de coupe",cupGameVM.labelStyle,GUILayout.Height(1f/10f*screenVM.blockMiddleRightHeight));
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndArea();
		}
	}
}


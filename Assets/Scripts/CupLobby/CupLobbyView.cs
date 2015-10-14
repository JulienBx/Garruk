//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Linq;
//
//public class CupLobbyView : MonoBehaviour
//{
//	
//	public CupLobbyBoardViewModel boardVM;
//	public CupLobbyViewModel cupLobbyVM;
//	public CupLobbyResultsViewModel resultsVM;
//	public CupLobbyOpponentViewModel opponentVM;
//	public CupLobbyScreenViewModel screenVM;
//	public CupLobbyCompetitionInfosViewModel competInfosVM;
//	
//	public CupLobbyView ()
//	{
//		this.boardVM = new CupLobbyBoardViewModel ();
//		this.cupLobbyVM = new CupLobbyViewModel ();
//		this.resultsVM = new CupLobbyResultsViewModel ();
//		this.opponentVM = new CupLobbyOpponentViewModel ();
//		this.screenVM = new CupLobbyScreenViewModel ();
//		this.competInfosVM = new CupLobbyCompetitionInfosViewModel ();
//	}
//	void Update()
//	{
//		if (Screen.width != screenVM.widthScreen || Screen.height != screenVM.heightScreen) 
//		{
//			CupLobbyController.instance.resize();
//		}
//	}
//	void OnGUI()
//	{
//		GUILayout.BeginArea (screenVM.blockTopLeft,screenVM.blockBackgroundStyle);
//		{
//			GUILayout.Label(boardVM.name,boardVM.cupLabelStyle);
//			GUILayout.FlexibleSpace();
//			GUILayout.BeginHorizontal();
//			{
//				GUILayout.Space (0.2f*screenVM.blockTopLeftWidth);
//				GUILayout.BeginVertical();
//				{
//					for (int i=0;i<boardVM.nbRounds;i++)
//					{
//						GUILayout.Label(boardVM.roundsName[i],boardVM.roundsStyle[i]);
//						GUILayout.Space ((screenVM.blockTopLeftHeight*0.5f*0.2f)/boardVM.nbRounds);
//					}
//				}
//				GUILayout.EndVertical();
//				GUILayout.Space (0.2f*screenVM.blockTopLeftWidth);
//			}
//			GUILayout.EndHorizontal();
//			GUILayout.FlexibleSpace();
//			GUILayout.FlexibleSpace();
//		}
//		GUILayout.EndArea();
//		
//		GUILayout.BeginArea(screenVM.blockTopRight,screenVM.blockBackgroundStyle);
//		{
//			GUI.enabled=cupLobbyVM.buttonsEnabled;
//			if(GUILayout.Button("Jouer",cupLobbyVM.buttonStyle))
//			{
//				CupLobbyController.instance.joinCupGame();
//			}
//			if(GUILayout.Button("Quitter",cupLobbyVM.buttonStyle))
//			{
//				CupLobbyController.instance.quitCupLobby();
//			}
//			GUI.enabled=true;
//		}
//		GUILayout.EndArea();
//		GUILayout.BeginArea (screenVM.blockMiddleRight, screenVM.blockBackgroundStyle);
//		{
//			GUILayout.Label (competInfosVM.title,competInfosVM.titleStyle);
//			GUILayout.FlexibleSpace();
//			GUILayout.BeginHorizontal();
//			{
//				GUILayout.FlexibleSpace();
//				if(GUILayout.Button("",competInfosVM.competitionPictureStyle,GUILayout.Width(screenVM.blockMiddleRightHeight*1f/2f),GUILayout.Height(screenVM.blockMiddleRightHeight*1f/2f)))
//				{
//				}
//				GUILayout.FlexibleSpace();
//			}
//			GUILayout.EndHorizontal();
//			GUILayout.FlexibleSpace();
//			GUILayout.Label ("Nombre de tours : "+competInfosVM.nbRounds,competInfosVM.informationsStyle);
//			GUILayout.Label ("Prime de victoire : "+competInfosVM.cupPrize+" crédits",competInfosVM.informationsStyle);
//			GUILayout.FlexibleSpace();
//		}
//		GUILayout.EndArea ();
//		if(resultsVM.resultsLabel.Count>0)
//		{
//			GUILayout.BeginArea(screenVM.blockBottom,screenVM.blockBackgroundStyle);
//			{
//				GUILayout.Label(resultsVM.resultsTitle, resultsVM.titleStyle,GUILayout.Height(0.15f * screenVM.blockBottomHeight));
//				GUILayout.BeginHorizontal();
//				{
//					GUILayout.BeginVertical(GUILayout.Width(screenVM.blockBottomWidth*0.30f));
//					{
//						resultsVM.scrollPosition = GUILayout.BeginScrollView(resultsVM.scrollPosition,GUILayout.Height(4*0.2f * screenVM.blockBottomHeight));
//						
//						for (int i = 0; i < resultsVM.resultsLabel.Count; i++)
//						{	
//							GUILayout.BeginHorizontal();
//							{
//								if (GUILayout.Button(resultsVM.resultsLabel[i], resultsVM.resultsStyles[i],GUILayout.Height(0.2f * screenVM.blockBottomHeight)))
//								{
//								}
//								if (GUILayout.Button(">", resultsVM.focusButtonStyles[i],GUILayout.Height(0.2f * screenVM.blockBottomHeight),GUILayout.Width(0.2f * screenVM.blockBottomHeight)))
//								{
//									if (resultsVM.chosenResult != i)
//									{
//										CupLobbyController.instance.displayOpponent(i);
//									}
//								}
//							}
//							GUILayout.EndHorizontal();
//						}
//						GUILayout.EndScrollView();
//					}
//					GUILayout.EndVertical();
//					GUILayout.BeginVertical();
//					{
//						GUILayout.BeginHorizontal(opponentVM.backgroundStyle,GUILayout.Height(4*0.2f * screenVM.blockBottomHeight));
//						{
//							GUILayout.Space(screenVM.blockBottomWidth*5/100);
//							if(GUILayout.Button("",opponentVM.profilePictureStyle,GUILayout.Height(4*0.2f * screenVM.blockBottomHeight),GUILayout.Width(4*0.2f * screenVM.blockBottomHeight)))
//							{
//								ApplicationModel.profileChosen=opponentVM.username;
//								Application.LoadLevel("profile");
//							}
//							GUILayout.Space(screenVM.blockBottomWidth*5/100);
//							GUILayout.BeginVertical();
//							{
//								GUILayout.FlexibleSpace();
//								GUILayout.Label (opponentVM.username,opponentVM.usernameStyle);
//								GUILayout.FlexibleSpace();
//								GUILayout.FlexibleSpace();
//								GUILayout.Label ("Victoires : "+opponentVM.totalNbWins,opponentVM.informationsStyle);
//								GUILayout.FlexibleSpace();
//								GUILayout.Label ("Défaites : "+opponentVM.totalNbLooses,opponentVM.informationsStyle);
//								GUILayout.FlexibleSpace();
//								GUILayout.Label ("Ranking : "+opponentVM.ranking,opponentVM.informationsStyle);
//								GUILayout.FlexibleSpace();
//								GUILayout.Label ("Ranking Points : "+opponentVM.rankingPoints,opponentVM.informationsStyle);
//								GUILayout.FlexibleSpace();
//								GUILayout.Label ("Division : "+opponentVM.division,opponentVM.informationsStyle);
//								GUILayout.FlexibleSpace();
//							}
//							GUILayout.EndVertical();
//						}
//						GUILayout.EndHorizontal();
//					}
//					GUILayout.EndHorizontal();
//				}
//				GUILayout.EndHorizontal();
//			}
//			GUILayout.EndArea();
//		}
//	}
//}
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class DivisionLobbyView : MonoBehaviour
{

	public DivisionLobbyBoardViewModel boardVM;
	public DivisionLobbyViewModel divisionLobbyVM;
	public DivisionLobbyResultsViewModel resultsVM;
	public DivisionLobbyOpponentViewModel opponentVM;
	public DivisionLobbyScreenViewModel screenVM;
	public DivisionLobbyCompetitionInfosViewModel competInfosVM;

	public DivisionLobbyView ()
	{
		this.boardVM = new DivisionLobbyBoardViewModel ();
		this.divisionLobbyVM = new DivisionLobbyViewModel ();
		this.resultsVM = new DivisionLobbyResultsViewModel ();
		this.opponentVM = new DivisionLobbyOpponentViewModel ();
		this.screenVM = new DivisionLobbyScreenViewModel ();
		this.competInfosVM = new DivisionLobbyCompetitionInfosViewModel ();
	}
	void Update()
	{
		if (Screen.width != screenVM.widthScreen || Screen.height != screenVM.heightScreen) 
		{
			DivisionLobbyController.instance.resize();
		}
	}
	void OnGUI()
	{
		GUILayout.BeginArea (screenVM.blockTopLeft,screenVM.blockBackgroundStyle);
		{
			GUILayout.Label(boardVM.divisionName,boardVM.divisionLabelStyle);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (screenVM.blockTopLeftWidth*5/100);
				GUILayout.Label("Série : "+boardVM.nbWinsDivision+" V, "+boardVM.nbLoosesDivision+" D",boardVM.divisionStrikeLabelStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label("Matchs restants : "+boardVM.remainingGames.ToString(),boardVM.remainingGamesStyle);
				GUILayout.Space (screenVM.blockTopLeftWidth*5/100);
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.BeginHorizontal(GUILayout.Height(5f / 100f * screenVM.blockTopLeftHeight));
					{
						GUILayout.Space (boardVM.startActiveGaugeBackgroundStyle.fixedWidth);
						GUILayout.Space (boardVM.activeGaugeBackgroundStyle.fixedWidth);
						GUILayout.Space (boardVM.gaugeSpace1Width);
						if(boardVM.relegationBarWidth!=0)
						{
							GUILayout.Space (boardVM.relegationBarStyle.fixedWidth/2f);
							GUILayout.Space (-boardVM.relegationLabelStyle.fixedWidth/2f);
							GUILayout.Label ("Relégation",boardVM.relegationLabelStyle);
							GUILayout.Space (-boardVM.relegationLabelStyle.fixedWidth/2f);
							GUILayout.Space (boardVM.relegationBarStyle.fixedWidth/2f);
						}
						GUILayout.Space (boardVM.gaugeSpace2Width);
						if(boardVM.promotionBarWidth!=0)
						{
							GUILayout.Space (boardVM.promotionBarStyle.fixedWidth/2f);
							GUILayout.Space (-boardVM.promotionLabelStyle.fixedWidth/2f);
							GUILayout.Label ("Promotion",boardVM.promotionLabelStyle);
							GUILayout.Space (-boardVM.promotionLabelStyle.fixedWidth/2f);
							GUILayout.Space (boardVM.promotionBarStyle.fixedWidth/2f);
						}
						GUILayout.Space (boardVM.gaugeSpace3Width);
						if(boardVM.titleBarWidth!=0)
						{
							GUILayout.Space (boardVM.titleBarStyle.fixedWidth/2f);
							GUILayout.Space (-boardVM.titleLabelStyle.fixedWidth/2f);
							GUILayout.Label ("Titre",boardVM.titleLabelStyle);
							GUILayout.Space (-boardVM.titleLabelStyle.fixedWidth/2f);
							GUILayout.Space (boardVM.titleBarStyle.fixedWidth/2f);
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(boardVM.gaugeBackgroundStyle);
					{
						GUILayout.Label (boardVM.nbWinsDivision+"V",boardVM.startActiveGaugeBackgroundStyle);
						GUILayout.Label ("",boardVM.activeGaugeBackgroundStyle);
						GUILayout.Space (boardVM.gaugeSpace1Width);
						GUILayout.Label ("",boardVM.relegationBarStyle);
						GUILayout.Space (boardVM.gaugeSpace2Width);
						GUILayout.Label ("",boardVM.promotionBarStyle);
						GUILayout.Space (boardVM.gaugeSpace3Width);
						GUILayout.Label ("",boardVM.titleBarStyle);
						GUILayout.Space (boardVM.gaugeSpace4Width);
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(GUILayout.Height(5f / 100f * screenVM.blockTopLeftHeight));
					{
						GUILayout.Space (boardVM.startActiveGaugeBackgroundStyle.fixedWidth);
						GUILayout.Space (boardVM.activeGaugeBackgroundStyle.fixedWidth);
						GUILayout.Space (boardVM.gaugeSpace1Width);
						if(boardVM.relegationBarWidth!=0)
						{
							GUILayout.Space (boardVM.relegationBarStyle.fixedWidth/2f);
							GUILayout.Space (-boardVM.relegationValueLabelStyle.fixedWidth/2f);
							GUILayout.Label (boardVM.nbWinsForRelegation.ToString()+" V",boardVM.relegationValueLabelStyle);
							GUILayout.Space (-boardVM.relegationValueLabelStyle.fixedWidth/2f);
							GUILayout.Space (boardVM.relegationBarStyle.fixedWidth/2f);
						}
						GUILayout.Space (boardVM.gaugeSpace2Width);
						if(boardVM.promotionBarWidth!=0)
						{
							GUILayout.Space (boardVM.promotionBarStyle.fixedWidth/2f);
							GUILayout.Space (-boardVM.promotionValueLabelStyle.fixedWidth/2f);
							GUILayout.Label (boardVM.nbWinsForPromotion.ToString()+" V",boardVM.promotionValueLabelStyle);
							GUILayout.Space (-boardVM.promotionValueLabelStyle.fixedWidth/2f);
							GUILayout.Space (boardVM.promotionBarStyle.fixedWidth/2f);
						}
						GUILayout.Space (boardVM.gaugeSpace3Width);
						if(boardVM.titleBarWidth!=0)
						{
							GUILayout.Space (boardVM.titleBarStyle.fixedWidth/2f);
							GUILayout.Space (-boardVM.titleValueLabelStyle.fixedWidth/2f);
							GUILayout.Label (boardVM.nbWinsForTitle.ToString()+" V",boardVM.titleValueLabelStyle);
							GUILayout.Space (-boardVM.titleValueLabelStyle.fixedWidth/2f);
							GUILayout.Space (boardVM.titleBarStyle.fixedWidth/2f);
						}
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();

		GUILayout.BeginArea(screenVM.blockTopRight,screenVM.blockBackgroundStyle);
		{
			if(GUILayout.Button("Jouer",divisionLobbyVM.buttonStyle))
			{
				DivisionLobbyController.instance.joinDivisionGame();
			}
			if(GUILayout.Button("Quitter",divisionLobbyVM.buttonStyle))
			{
				DivisionLobbyController.instance.quitDivisionLobby();
			}
		}
		GUILayout.EndArea();
		GUILayout.BeginArea (screenVM.blockMiddleRight, screenVM.blockBackgroundStyle);
		{
			GUILayout.Label (competInfosVM.title,competInfosVM.titleStyle);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("",competInfosVM.competitionPictureStyle,GUILayout.Width(screenVM.blockMiddleRightHeight*1f/2f),GUILayout.Height(screenVM.blockMiddleRightHeight*1f/2f)))
				{
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label ("Nombre de matchs : "+competInfosVM.nbGames,competInfosVM.informationsStyle);
			GUILayout.Label ("Prime de titre : "+competInfosVM.titlePrize+" crédits",competInfosVM.informationsStyle);
			if(competInfosVM.promotionPrize>0)
			{
				GUILayout.Label ("Prime de montée : "+competInfosVM.promotionPrize+" crédits",competInfosVM.informationsStyle);
			}
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea ();
		if(resultsVM.resultsLabel.Count>0)
		{
			GUILayout.BeginArea(screenVM.blockBottom,screenVM.blockBackgroundStyle);
			{
				GUILayout.Label(resultsVM.resultsTitle, resultsVM.titleStyle,GUILayout.Height(0.15f * screenVM.blockBottomHeight));
				GUILayout.BeginHorizontal();
				{
					GUILayout.BeginVertical(GUILayout.Width(screenVM.blockBottomWidth*0.30f));
					{
						resultsVM.scrollPosition = GUILayout.BeginScrollView(resultsVM.scrollPosition,GUILayout.Height(4*0.2f * screenVM.blockBottomHeight));
						
						for (int i = 0; i < resultsVM.resultsLabel.Count; i++)
						{	
							GUILayout.BeginHorizontal();
							{
								if (GUILayout.Button(resultsVM.resultsLabel[i], resultsVM.resultsStyles[i],GUILayout.Height(0.2f * screenVM.blockBottomHeight)))
								{
								}
								if (GUILayout.Button(">", resultsVM.focusButtonStyles[i],GUILayout.Height(0.2f * screenVM.blockBottomHeight),GUILayout.Width(0.2f * screenVM.blockBottomHeight)))
								{
									if (resultsVM.chosenResult != i)
									{
										DivisionLobbyController.instance.displayOpponent(i);
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
						GUILayout.BeginHorizontal(opponentVM.backgroundStyle,GUILayout.Height(4*0.2f * screenVM.blockBottomHeight));
						{
							GUILayout.Space(screenVM.blockBottomWidth*5/100);
							if(GUILayout.Button("",opponentVM.profilePictureStyle,GUILayout.Height(4*0.2f * screenVM.blockBottomHeight),GUILayout.Width(4*0.2f * screenVM.blockBottomHeight)))
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
								GUILayout.Label ("Division : "+opponentVM.division,opponentVM.informationsStyle);
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
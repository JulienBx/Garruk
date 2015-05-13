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
	public DivisionLobbyLastOpponentViewModel lastOpponentVM;
	public DivisionLobbyScreenViewModel screenVM;
	
	public DivisionLobbyView ()
	{
		this.boardVM = new DivisionLobbyBoardViewModel ();
		this.divisionLobbyVM = new DivisionLobbyViewModel ();
		this.resultsVM = new DivisionLobbyResultsViewModel ();
		this.lastOpponentVM = new DivisionLobbyLastOpponentViewModel ();
		this.screenVM = new DivisionLobbyScreenViewModel ();
	}
	void Update()
	{
		if (Screen.width != screenVM.widthScreen || Screen.height != screenVM.heightScreen) 
		{
			DivisionLobbyController.instance.loadData();
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
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space (screenVM.blockTopLeftWidth*5/100);
				GUILayout.Label("Prime de promotion : "+boardVM.promotionPrize.ToString()+" crédits",boardVM.promotionPrizeLabelStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label("Prime de titre : "+boardVM.titlePrize.ToString()+" crédits",boardVM.titlePrizeLabelStyle);
				GUILayout.Space (screenVM.blockTopLeftWidth*5/100);
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(screenVM.blockMiddleRight,screenVM.blockBackgroundStyle);
		{
			GUILayout.Label (resultsVM.resultsTitle,resultsVM.titleStyle,GUILayout.Height(screenVM.blockMiddleRightHeight*1/10));
			for (int i=0;i<resultsVM.username.Count;i++)
			{
				GUILayout.BeginHorizontal(resultsVM.backgroundStyles[i],GUILayout.Height(screenVM.blockMiddleRightHeight*1/7));
				{
					GUILayout.Space(screenVM.blockMiddleRightWidth*5/100);
					if(GUILayout.Button("",resultsVM.profilePictureButtonStyles[i],GUILayout.Width(screenVM.blockMiddleRightHeight*1/7)))
					{
						ApplicationModel.profileChosen=resultsVM.username[i];
						Application.LoadLevel("profile");
					}
					GUILayout.Space(screenVM.blockMiddleRightWidth*5/100);
					GUILayout.BeginVertical();
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label (resultsVM.label[i],resultsVM.labelStyles[i]);
						GUILayout.FlexibleSpace();
						GUILayout.Label (resultsVM.username[i],resultsVM.usernameLabelStyle);
						GUILayout.Label ("V : "+resultsVM.totalNbWins[i]+" D : " +resultsVM.totalNbLooses[i],resultsVM.informationsLabelStyle);
						GUILayout.Label ("Ranking : "+resultsVM.ranking[i],resultsVM.informationsLabelStyle);
						GUILayout.Label ("Division : "+resultsVM.division[i],resultsVM.informationsLabelStyle);
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(screenVM.blockMiddleRightHeight*15/1000);
			}
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea ();
		GUILayout.BeginArea(new Rect(screenVM.blockMiddleRight.xMin,
		                             screenVM.blockMiddleRight.yMax-1f/20f*screenVM.blockMiddleRightHeight,
		                             screenVM.blockMiddleRightWidth,
		                             screenVM.blockMiddleRightHeight*1f/20f));
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				if (resultsVM.pageDebut>0)
				{
					if (GUILayout.Button("...",divisionLobbyVM.paginationStyle,GUILayout.Width(screenVM.blockMiddleRightWidth*1/15)))
					{
						DivisionLobbyController.instance.paginationBack();
					}
				}
				GUILayout.Space(screenVM.blockMiddleRightWidth*0.01f);
				for (int i = resultsVM.pageDebut ; i < resultsVM.pageFin ; i++)
				{
					if (GUILayout.Button(""+(i+1),resultsVM.paginatorGuiStyle[i],GUILayout.Width(screenVM.blockMiddleRightWidth*1/15)))
					{
						DivisionLobbyController.instance.paginationSelect(i);
					}
					GUILayout.Space(screenVM.blockMiddleRightWidth*0.01f);
				}
				if (resultsVM.nbPages>resultsVM.pageFin)
				{
					if (GUILayout.Button("...",divisionLobbyVM.paginationStyle,GUILayout.Width(screenVM.blockMiddleRightWidth*1/15)))
					{
						DivisionLobbyController.instance.paginationNext();
					}
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(screenVM.blockMiddleRightHeight*10/1000);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(screenVM.blockBottomLeft,screenVM.blockBackgroundStyle);
		{
			GUILayout.Label (lastOpponentVM.title,lastOpponentVM.titleStyle,GUILayout.Height(screenVM.blockBottomLeftHeight*4/25));
			GUILayout.BeginHorizontal(lastOpponentVM.backgroundStyle,GUILayout.Height(screenVM.blockBottomLeftHeight*4/5));
			{
				if(lastOpponentVM.username!="")
				{
					GUILayout.Space(screenVM.blockBottomLeftWidth*5/100);
					if(GUILayout.Button("",lastOpponentVM.profilePictureStyle,GUILayout.Width(screenVM.blockBottomLeftHeight*4/5)))
					{
						ApplicationModel.profileChosen=lastOpponentVM.username;
						Application.LoadLevel("profile");
					}
					GUILayout.Space(screenVM.blockBottomLeftWidth*5/100);
					GUILayout.BeginVertical();
					{
						GUILayout.Label (lastOpponentVM.username,lastOpponentVM.usernameStyle);
						GUILayout.Label ("Victoires : "+lastOpponentVM.totalNbWins,lastOpponentVM.informationsStyle);
						GUILayout.Label ("Défaites : "+lastOpponentVM.totalNbLooses,lastOpponentVM.informationsStyle);
						GUILayout.Label ("Ranking : "+lastOpponentVM.ranking,lastOpponentVM.informationsStyle);
						GUILayout.Label ("Ranking Points : "+lastOpponentVM.rankingPoints,lastOpponentVM.informationsStyle);
						GUILayout.Label ("Division : "+lastOpponentVM.division,lastOpponentVM.informationsStyle);
					}
					GUILayout.EndVertical();
				}
				else
				{
					GUILayout.Label(lastOpponentVM.noOpponnentLabel,lastOpponentVM.noOpponentLabelStyle);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (screenVM.blockBottomLeftHeight*1/25);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(screenVM.blockTopRight,screenVM.blockBackgroundStyle);
		{
			if(GUILayout.Button("Jouer",divisionLobbyVM.buttonStyle))
			{
				DivisionLobbyController.instance.joinDivisionGame();
			}
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(screenVM.blockBottomRight,screenVM.blockBackgroundStyle);
		{
			if(GUILayout.Button("Quitter",divisionLobbyVM.buttonStyle))
			{
				DivisionLobbyController.instance.quitDivisionLobby();
			}
		}
		GUILayout.EndArea();
	}
}
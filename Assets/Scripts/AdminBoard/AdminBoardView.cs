using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Reflection;

public class AdminBoardView : MonoBehaviour
{
	public AdminBoardViewModel VM;
	
	public AdminBoardView()
	{
		this.VM= new AdminBoardViewModel();
	}
	void Update()
	{
		if (Screen.width !=VM.widthScreen || Screen.height != VM.heightScreen) 
		{
			AdminBoardController.instance.resize();
		}
	}
	void OnGUI()
	{
		GUILayout.BeginArea (new Rect (0, 0.1f * VM.heightScreen, VM.widthScreen, VM.heightScreen * 0.9f));
		{
			GUILayout.Space (VM.heightScreen*0.05f);
			GUILayout.Label ("ADMIN BOARD",VM.titleStyle);
			GUILayout.Space (VM.heightScreen*0.02f);
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				bool toggle;
				toggle = GUILayout.Toggle(VM.filteredStats, "Données filtrées", VM.toggleStyle);
				if (toggle != VM.filteredStats)
				{
					AdminBoardController.instance.filterData(toggle);
				}
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space (VM.heightScreen*0.02f);
			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginVertical(GUILayout.Width(VM.widthScreen*0.5f));
				{
					GUILayout.Label("Stats du jour",VM.titleStyle);
					GUILayout.Space (VM.heightScreen*0.05f);
					GUILayout.Label(VM.connectionsToday,VM.valueStyle);
					GUILayout.Label("Nb de connections",VM.textStyle);
					GUILayout.Label(VM.playersToday,VM.valueStyle);
					GUILayout.Label("Nb de joueurs",VM.textStyle);
					GUILayout.Label(VM.packBoughtToday,VM.valueStyle);
					GUILayout.Label("Nb de pack achetés",VM.textStyle);
					GUILayout.Label(VM.cardBoughtToday,VM.valueStyle);
					GUILayout.Label("Nb de cartes échangées",VM.textStyle);
					GUILayout.Label(VM.xpBoughtToday,VM.valueStyle);
					GUILayout.Label("Nb de cartes upgradées",VM.textStyle);
					GUILayout.Label(VM.cardsRenamedToday,VM.valueStyle);
					GUILayout.Label("Nb de cartes renommées",VM.textStyle);
					GUILayout.Label(VM.cardSoldToday,VM.valueStyle);
					GUILayout.Label("Nb de cartes détruites",VM.textStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.BeginVertical(GUILayout.Width(VM.widthScreen*0.5f));
				{
					GUILayout.Label("Stats sur période",VM.titleStyle);
					GUILayout.Space (VM.heightScreen*0.02f);
					GUILayout.BeginHorizontal();
					{	
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Début",VM.textStyle);
						GUILayout.Space (VM.widthScreen*0.01f);
						VM.startPeriod = GUILayout.TextField(VM.startPeriod, 10,VM.textfieldStyle);
						GUILayout.Space (VM.widthScreen*0.01f);
						GUILayout.Label ("Fin",VM.textStyle);
						GUILayout.Space (VM.widthScreen*0.01f);
						VM.endPeriod = GUILayout.TextField(VM.endPeriod, 10,VM.textfieldStyle);
						GUILayout.Space (VM.widthScreen*0.01f);
						if(GUILayout.Button("Afficher",VM.buttonStyle))
						{
							AdminBoardController.instance.refreshPeriodHandler();
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.Space (VM.heightScreen*0.02f);
					if(VM.error!="")
					{
						GUILayout.Label(VM.error,VM.errorStyle);
					}
					GUILayout.Label(VM.connectionsOnPeriod,VM.valueStyle);
					GUILayout.Label("Nb de connections",VM.textStyle);
					GUILayout.Label(VM.playersOnPeriod,VM.valueStyle);
					GUILayout.Label("Nb de joueurs",VM.textStyle);
					GUILayout.Label(VM.packBoughtOnPeriod,VM.valueStyle);
					GUILayout.Label("Nb de pack achetés",VM.textStyle);
					GUILayout.Label(VM.cardBoughtOnPeriod,VM.valueStyle);
					GUILayout.Label("Nb de cartes échangées",VM.textStyle);
					GUILayout.Label(VM.xpBoughtOnPeriod,VM.valueStyle);
					GUILayout.Label("Nb de cartes upgradées",VM.textStyle);
					GUILayout.Label(VM.cardsRenamedOnPeriod,VM.valueStyle);
					GUILayout.Label("Nb de cartes renommées",VM.textStyle);
					GUILayout.Label(VM.cardSoldOnPeriod,VM.valueStyle);
					GUILayout.Label("Nb de cartes détruites",VM.textStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea ();
	}
}
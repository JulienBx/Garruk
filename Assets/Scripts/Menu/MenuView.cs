using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MenuView : MonoBehaviour {
				
	public MenuViewModel menuVM;
	public NotificationsReminderViewModel notificationsReminderVM;
	public UserDataViewModel userDataVM;
	public MenuScreenViewModel menuScreenVM;

	public MenuView ()
	{
		menuVM = new MenuViewModel ();
		notificationsReminderVM = new NotificationsReminderViewModel ();
		userDataVM = new UserDataViewModel ();
		menuScreenVM = new MenuScreenViewModel ();
	}
	void Update()
	{
		if (Screen.width != menuScreenVM.widthScreen || Screen.height != menuScreenVM.heightScreen) {
			MenuController.instance.resize();
		}
	}
	void OnGUI(){
		GUILayout.BeginArea (new Rect(0,0,menuScreenVM.widthScreen,0.1f*menuScreenVM.heightScreen),menuVM.menuBackgroundStyle);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label ("Garruk, le jeu",menuVM.titleStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUI.enabled=menuVM.buttonsEnabled[0];
					if(GUILayout.Button ("Accueil",menuVM.buttonStyle)){
						MenuController.instance.homePageLink();
					}
					GUI.enabled=true;
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				if (notificationsReminderVM.nbNotificationsNonRead>0){
					GUILayout.BeginVertical();
					{
						GUILayout.FlexibleSpace();
						GUI.enabled=menuVM.buttonsEnabled[1];
						if(GUILayout.Button ("",notificationsReminderVM.nonReadNotificationsButtonStyle)){
							MenuController.instance.homePageLink();
						}
						GUI.enabled=true;
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndVertical();
					GUILayout.FlexibleSpace();
				}
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUI.enabled=menuVM.buttonsEnabled[2];
					if(GUILayout.Button ("Mes cartes",menuVM.buttonStyle)){
						MenuController.instance.myGameLink();
					}
					GUI.enabled=true;
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUI.enabled=menuVM.buttonsEnabled[3];
					if(GUILayout.Button ("La boutique",menuVM.buttonStyle)){
						MenuController.instance.shopLink();
					}
					GUI.enabled=true;
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUI.enabled=menuVM.buttonsEnabled[4];
					if(GUILayout.Button ("Le bazar",menuVM.buttonStyle)){
						MenuController.instance.marketLink();
					}
					GUI.enabled=true;
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUI.enabled=menuVM.buttonsEnabled[5];
					if(GUILayout.Button ("Jouer",menuVM.buttonStyle)){
						MenuController.instance.lobbyLink();
					}
					GUI.enabled=true;
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("Déconnexion",menuVM.logOutButtonStyle)){
							MenuController.instance.logOutLink();
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						GUI.enabled=menuVM.buttonsEnabled[6];
						if(GUILayout.Button (userDataVM.username,userDataVM.welcomeStyle)){
							MenuController.instance.profileLink();
						}
						GUI.enabled=true;
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label (userDataVM.credits+" crédits",userDataVM.welcomeStyle);
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
		if (notificationsReminderVM.nbNotificationsNonRead>0){
			GUILayout.BeginArea (new Rect(notificationsReminderVM.nonReadNotificationsCounter),notificationsReminderVM.nonReadNotificationsCounterStyle);
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(notificationsReminderVM.nbNotificationsNonRead.ToString(),notificationsReminderVM.nonReadNotificationsCounterPoliceStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();	
			}
			GUILayout.EndArea();
		}
	}
}


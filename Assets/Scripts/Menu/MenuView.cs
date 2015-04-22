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
					if(GUILayout.Button ("Accueil",menuVM.buttonStyle)){
						MenuController.instance.homePageLink();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				if (notificationsReminderVM.nbNotificationsNonRead>0){
					GUILayout.BeginVertical();
					{
						GUILayout.FlexibleSpace();
						if(GUILayout.Button ("",notificationsReminderVM.nonReadNotificationsButtonStyle)){
							MenuController.instance.homePageLink();
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndVertical();
					GUILayout.FlexibleSpace();
				}
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button ("Mes cartes",menuVM.buttonStyle)){
						MenuController.instance.myGameLink();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button ("La boutique",menuVM.buttonStyle)){
						MenuController.instance.shopLink();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button ("Le bazar",menuVM.buttonStyle)){
						MenuController.instance.marketLink();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					if(GUILayout.Button ("Jouer",menuVM.buttonStyle)){
						MenuController.instance.lobbyLink();
					}
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
						if(GUILayout.Button (userDataVM.username,userDataVM.welcomeStyle)){
							MenuController.instance.profileLink();
						}
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


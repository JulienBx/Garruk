using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class newMenuView : MonoBehaviour 
{
	
	public newMenuViewModel menuVM;
	public newNotificationsReminderViewModel notificationsReminderVM;
	public newUserDataViewModel userDataVM;
	public newMenuScreenViewModel menuScreenVM;
	
	public newMenuView ()
	{
		menuVM = new newMenuViewModel ();
		notificationsReminderVM = new newNotificationsReminderViewModel ();
		userDataVM = new newUserDataViewModel ();
		menuScreenVM = new newMenuScreenViewModel ();
	}
	void Update()
	{
		if (Screen.width != menuScreenVM.widthScreen || Screen.height != menuScreenVM.heightScreen) {
			newMenuController.instance.resize();
		}
	}
	void OnGUI()
	{
		GUILayout.BeginArea (menuScreenVM.mainBlock);
		{
			GUILayout.Space(menuScreenVM.mainBlock.height*(2f/100f));
			GUILayout.Label("",menuVM.logoStyle,GUILayout.Height(menuScreenVM.mainBlock.width/3.5f));
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndArea();

		GUILayout.BeginArea (userDataVM.profileRect);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.BeginHorizontal(GUILayout.Height(userDataVM.profilePictureHeight));
				{
					GUILayout.Label("",userDataVM.profilePictureStyle,GUILayout.Width(userDataVM.profilePictureHeight));
					GUILayout.Label(userDataVM.username,userDataVM.usernameStyle);
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();

		GUILayout.BeginArea (userDataVM.profilePictureBorderRect);
		{
			if(GUILayout.Button("",userDataVM.profilePictureBorderStyle))
			{
			}
		}
		GUILayout.EndArea ();

		GUILayout.BeginArea (userDataVM.creditsRect);
		{
			GUILayout.BeginHorizontal(GUILayout.Height(userDataVM.creditsPictureHeight));
			{
				GUILayout.Label("",userDataVM.creditPictureStyle,GUILayout.Width(userDataVM.creditsPictureHeight));
				GUILayout.Label(userDataVM.credits.ToString(),userDataVM.creditsStyle);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea ();

		if(notificationsReminderVM.nbNotificationsNonRead>0)
		{
			GUILayout.BeginArea(notificationsReminderVM.nonReadNotificationsLogo);
			{
				if(GUILayout.Button("",notificationsReminderVM.nonReadNotificationsButtonStyle))
				{
				}
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(notificationsReminderVM.nonReadNotificationsCounter);
			{
				GUILayout.Label(notificationsReminderVM.nbNotificationsNonRead.ToString(),notificationsReminderVM.nonReadNotificationsCounterPoliceStyle);
			}
			GUILayout.EndArea();
		}

		for(int i=0;i<menuScreenVM.buttonsArea.Length;i++)
		{
			GUILayout.BeginArea (menuScreenVM.buttonsArea [i]);
			{
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUI.enabled=menuVM.buttonsEnabled[i+1];
					if(GUILayout.Button (menuVM.buttonsLabels[i],menuVM.buttonStyle,GUILayout.Height(menuScreenVM.buttonHeight)))
					{

					}
					GUI.enabled=true;	
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
				if (Event.current.type == EventType.Repaint && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
				{
					newMenuController.instance.moveButton(i);
				}
			}
			GUILayout.EndArea();
		}
	}
}


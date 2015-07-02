using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class newNotificationsReminderViewModel {

	public Rect nonReadNotificationsLogo;
	public Rect nonReadNotificationsCounter;
	public GUIStyle nonReadNotificationsButtonStyle;
	public GUIStyle nonReadNotificationsCounterPoliceStyle;
	
	public int nbNotificationsNonRead;
	
	public newNotificationsReminderViewModel ()
	{
		this.nbNotificationsNonRead = ApplicationModel.nbNotificationsNonRead;
		this.nonReadNotificationsButtonStyle=new GUIStyle();
		this.nonReadNotificationsCounterPoliceStyle=new GUIStyle();
		this.nonReadNotificationsLogo = new Rect ();
		this.nonReadNotificationsCounter = new Rect ();
	}
	public void resize(int heightScreen)
	{	
		//this.nonReadNotificationsCounterPoliceStyle.fontSize = heightScreen*3/100;
	}
	
}


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class NotificationsReminderViewModel {

	public GUIStyle[] styles;
	public GUIStyle nonReadNotificationsButtonStyle;
	public GUIStyle nonReadNotificationsCounterPoliceStyle;
	public GUIStyle nonReadNotificationsCounterStyle;

	public float flexibleSpaceSize;
	public float distanceToNonReadNotificationsCounter;
	public Rect nonReadNotificationsCounter;

	public int nbNotificationsNonRead=ApplicationModel.nbNotificationsNonRead;

	public NotificationsReminderViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.nonReadNotificationsButtonStyle=new GUIStyle();
		this.nonReadNotificationsCounterPoliceStyle=new GUIStyle();
		this.flexibleSpaceSize = new float();
		this.distanceToNonReadNotificationsCounter = new float();
		this.nonReadNotificationsCounter = new Rect ();
	}
	public void initStyles()
	{	
		this.nonReadNotificationsButtonStyle = this.styles[0];
		this.nonReadNotificationsCounterPoliceStyle =this.styles[1];
		this.nonReadNotificationsCounterStyle=this.styles[2];
	}
	public void resize(int heightScreen)
	{	
		this.nonReadNotificationsCounterPoliceStyle.fontSize = heightScreen*2/100;

		this.nonReadNotificationsButtonStyle.fontSize = heightScreen*20/1000;
		this.nonReadNotificationsButtonStyle.fixedHeight = (int)heightScreen*6/100;
		this.nonReadNotificationsButtonStyle.fixedWidth = (int)heightScreen*6/100;
	}

}


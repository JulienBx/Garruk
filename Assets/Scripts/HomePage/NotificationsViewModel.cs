using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class NotificationsViewModel {

	public GUIStyle[] styles;
	public IList<GUIStyle> profilePicturesButtonStyle;
	public int nbPages;
	public int pageDebut;
	public int pageFin;
	public int chosenPage;
	public int start;
	public int finish;
	public int elementPerRow;
	public string labelNo;
	public string notificationsTitleLabel;

	public GUIStyle[] paginatorGuiStyle;
	public GUIStyle newStyle;
	public GUIStyle notificationContentStyle;
	public GUIStyle notificationDateStyle;

	public IList<DisplayedNotification> notifications;
	
	public IList<bool> nonReadNotifications;
	public int nbNonReadNotifications;
	public int displayedNonReadNotifications;

	public Rect[] blocks;
	public float blocksWidth;
	public float blocksHeight;

	public NotificationsViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.profilePicturesButtonStyle = new List<GUIStyle> ();
		this.paginatorGuiStyle=new GUIStyle[0];
		this.notificationContentStyle = new GUIStyle ();
		this.notificationDateStyle = new GUIStyle ();
		this.notifications = new List<DisplayedNotification> ();
		this.nonReadNotifications = new List<bool> ();
		this.blocks=new Rect[0];
	}
	public void initStyles()
	{	
		this.newStyle=this.styles[0];
		this.notificationContentStyle = this.styles [1];
		this.notificationDateStyle = this.styles [2];
	}
	public void resize(int heightScreen)
	{	
		this.notificationContentStyle.fontSize = heightScreen * 2 / 100;
		this.notificationDateStyle.fontSize = heightScreen * 2 / 100;
		this.newStyle.fontSize = heightScreen * 2 / 100;
	}
	public void displayPage(){
		
		this.start = this.chosenPage*(this.elementPerRow*10);
		if (this.notifications.Count < (10*this.elementPerRow*(this.chosenPage+1)))
		{
			this.finish = this.notifications.Count;
		}
		else{
			this.finish = (this.chosenPage+1)*(10 * this.elementPerRow);
		}
	}
}
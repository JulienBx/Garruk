using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DisplayedNotifications{

	public List<DisplayedNotification> notifications ;
	
	public DisplayedNotifications()
	{
		this.notifications = new List<DisplayedNotification>();
	}

	public DisplayedNotification getDisplayedNotification(int index)
	{
		return this.notifications [index];
	}

	public int getCount()
	{
		return this.notifications.Count;
	}

	public void parseDisplayedNotifications(string s)
	{

		data[2].Split(new string[] { "#N#" }, System.StringSplitOptions.None);

		for (int i=0;i<array.Length-1;i++)
		{
			string[] notificationData =array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			
			notifications.Add(new DisplayedNotification());

			notifications[i].Notification.Id=System.Convert.ToInt32(notificationData[0]);
			notifications[i].Notification.Date=DateTime.ParseExact(notificationData[1], "yyyy-MM-dd HH:mm:ss", null);
			notifications[i].Notification.IsRead=System.Convert.ToBoolean(System.Convert.ToInt32(notificationData[2]));
			notifications[i].Notification.IdNotificationType=System.Convert.ToInt32(notificationData[3]);
			notifications[i].Notification.HiddenParam= notificationData[4];
			notifications[i].SendingUser=this.users[returnUsersIndex(System.Convert.ToInt32(notificationData[5]))];

			if(ApplicationModel.player.Readnotificationsystem && notifications[i].Notification.IdNotificationType==1)
			{
				notifications[i].Notification.IsRead=true;
			}
			
			string tempContent=WordingNotifications.getContent(notifications[i].Notification.IdNotificationType-1);
			for(int j=6;j<notificationData.Length-1;j++)
			{
				string[] notificationObjectData = notificationData[j].Split (new char[] {':'},System.StringSplitOptions.None);
				switch (notificationObjectData[0])
				{
				case "user":
					notifications[i].Users.Add (new User());
					notifications[i].Users[notifications[i].Users.Count-1]=this.users[returnUsersIndex(System.Convert.ToInt32(notificationObjectData[1]))];
					tempContent=ReplaceFirst(tempContent,"#*user*#",notifications[i].Users[notifications[i].Users.Count-1].Username);
					break;
				case "card":
					string cardTitle ="";
					if(notificationObjectData[1]=="")
					{
						cardTitle=WordingCardTypes.getName(System.Convert.ToInt32(notificationObjectData[2]));
					}
					else
					{
						cardTitle=notificationObjectData[1];
					}
					notifications[i].Cards.Add (new Card (cardTitle));
					tempContent=ReplaceFirst(tempContent,"#*card*#",cardTitle);
					break;
				case "communication":
					notifications[i].Values.Add (notificationObjectData[1+ApplicationModel.player.IdLanguage]);
					tempContent=ReplaceFirst(tempContent,"#*communication*#",notificationObjectData[1+ApplicationModel.player.IdLanguage]);
					break;
				case "value":
					notifications[i].Values.Add (notificationObjectData[1]);
					tempContent=ReplaceFirst(tempContent,"#*value*#",notificationObjectData[1]);
					break;
				case "trophy":
					notifications[i].Values.Add (WordingGameModes.getName(System.Convert.ToInt32(notificationObjectData[2])));
					tempContent=ReplaceFirst(tempContent,"#*trophy*#",WordingGameModes.getName(System.Convert.ToInt32(notificationObjectData[2])));
					break;
				}
			}
			notifications[i].Content=tempContent;
		}
	}
}
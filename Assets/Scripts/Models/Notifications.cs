using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Notifications{

	public List<Notification> notifications ;
	public int notificationSystemIndex;

	private string URLUpdateReadNotifications = ApplicationModel.host+"update_read_notifications.php";
	
	public Notifications()
	{
		this.notifications = new List<Notification>();
		this.notificationSystemIndex = -1;
	}
	public Notification getNotification(int index)
	{
		return this.notifications [index];
	}
	public int getCount()
	{
		return this.notifications.Count;
	}
	public void add()
	{
		this.notifications.Add(new Notification());
	}
	public void remove(int index)
	{
		this.notifications.RemoveAt(index);
	}
	public void parseNotifications(string s, Player p)
	{
		string[] array = s.Split(new string[] { "#N#" }, System.StringSplitOptions.None);
		for (int i=0;i<array.Length-1;i++)
		{
			string[] notificationData =array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			
			notifications.Add(new Notification());
			notifications[i].Id=System.Convert.ToInt32(notificationData[0]);
			notifications[i].Date=DateTime.ParseExact(notificationData[1], "yyyy-MM-dd HH:mm:ss", null);
			notifications[i].IsRead=System.Convert.ToBoolean(System.Convert.ToInt32(notificationData[2]));
			notifications[i].IdNotificationType=System.Convert.ToInt32(notificationData[3]);
			notifications[i].HiddenParam= notificationData[4];
			notifications[i].SendingUser=p.Users.returnUsersIndex(System.Convert.ToInt32(notificationData[5]));

			if(ApplicationModel.player.Readnotificationsystem && notifications[i].IdNotificationType==1)
			{
				notifications[i].IsRead=true;
			}

			for(int j=6;j<notificationData.Length-1;j++)
			{
				string[] notificationObjectData = notificationData[j].Split (new char[] {':'},System.StringSplitOptions.None);
				switch (notificationObjectData[0])
				{
				case "user":
					notifications[i].Users.Add (p.Users.returnUsersIndex(System.Convert.ToInt32(notificationObjectData[1])));
					break;
				case "card":
					notifications[i].Cards.add ();
					notifications[i].Cards.getCard(notifications[i].Cards.getCount()-1).Title=notificationObjectData[1];
					notifications[i].Cards.getCard(notifications[i].Cards.getCount()-1).CardType.Id=System.Convert.ToInt32(notificationObjectData[2]);
					break;
				case "communication":
					notifications[i].Communications.Add (notificationObjectData[i]);
					notifications[i].Communications.Add (notificationObjectData[i+1]);
					break;
				case "value":
					notifications[i].Values.Add (notificationObjectData[1]);
					break;
				case "trophy":
					notifications[i].Trophies.add();
					notifications[i].Trophies.getTrophy(notifications[i].Trophies.getCount()-1).Id=System.Convert.ToInt32(notificationObjectData[2]);
					break;
				}
			}
		}
	}
	public void writeNotifications()
	{
		for (int i=0;i<notifications.Count;i++)
		{
			string tempContent=WordingNotifications.getContent(notifications[i].IdNotificationType-1);
			string newTempContent=tempContent;
			bool toReplace=true;
			int j=0;
			while(toReplace)
			{
				tempContent=newTempContent;
				if(notifications[i].Users.Count>j)
				{
					newTempContent=ApplicationModel.ReplaceFirst(newTempContent,"#*user*#",ApplicationModel.player.Users.getUser(notifications[i].Users[j]).Username);
				}
				if(notifications[i].Cards.getCount()>j)
				{
					if(notifications[i].Cards.getCard(j).Title=="")
					{
						newTempContent=ApplicationModel.ReplaceFirst(newTempContent,"#*card*#",WordingCardTypes.getName(System.Convert.ToInt32(notifications[i].Cards.getCard(j).CardType.Id)));
					}
					else
					{
						newTempContent=ApplicationModel.ReplaceFirst(newTempContent,"#*card*#",notifications[i].Cards.getCard(j).Title);
					}
				}
				if(notifications[i].Communications.Count>2*j)
				{
					newTempContent=ApplicationModel.ReplaceFirst(newTempContent,"#*communication*#",notifications[i].Communications[j+ApplicationModel.player.IdLanguage]);
				}
				if(notifications[i].Values.Count>j)
				{
					newTempContent=ApplicationModel.ReplaceFirst(newTempContent,"#*value*#",notifications[i].Values[j]);
				}
				if(notifications[i].Trophies.getCount()>j)
				{
					newTempContent=ApplicationModel.ReplaceFirst(newTempContent,"#*trophy*#",WordingGameModes.getName(notifications[i].Trophies.getTrophy(j).Id));	
				}
				if(newTempContent==tempContent)
				{
					toReplace=false;
				}
				j++;
			}
			notifications[i].Content=tempContent;
		}		
	}
	public void lookForNonReadNotification ()
	{
		int count=0;
		for(int i =0; i<this.notifications.Count;i++)
		{
			if(!ApplicationModel.player.Readnotificationsystem && this.notificationSystemIndex==-1 && notifications[i].IdNotificationType==1)
			{
				this.notificationSystemIndex=i;
				count++;
				break;
			}
		}
		for(int i =0; i<this.notifications.Count;i++)
		{
			if(i!=this.notificationSystemIndex && notifications[i].IdNotificationType!=1 && !notifications[i].IsRead)
			{
				count++;
			}
		}
		ApplicationModel.player.NbNotificationsNonRead=count;
	}
	public IEnumerator updateReadNotifications (IList<int> readNotifications,int totalNbResultLimit)
	{
		string query = "";
		bool isSystemNotification = false;
		for(int i=0;i<readNotifications.Count;i++)
		{
			this.notifications[readNotifications[i]].IsRead=true;
			if(this.notifications[readNotifications[i]].IdNotificationType==1)
			{
				isSystemNotification=true;
				ApplicationModel.player.Readnotificationsystem=true;
			}
			else
			{
				query=query+" id="+this.notifications[readNotifications[i]].Id+" or";
			}
		}
		if(query!="")
		{
			query=query.Substring(0,query.Length-3);
		}
		
		WWWForm form = new WWWForm(); 											
		form.AddField("myform_hash", ApplicationModel.hash); 					
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField ("myform_query", query);
		form.AddField ("myform_issystemnotification", isSystemNotification.ToString());
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		
		WWW w = new WWW(URLUpdateReadNotifications, form); 				
		yield return w;

		if (w.error != null) 
		{
			Debug.Log (w.error); 
		}

		ApplicationModel.player.NbNotificationsNonRead=System.Convert.ToInt32(w.text);
	}
}
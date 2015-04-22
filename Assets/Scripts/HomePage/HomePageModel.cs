using UnityEngine;
using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class HomePageModel 
{
	
	public IList<DisplayedNotification> notifications;
	public IList<DisplayedNews> news;
	public User player;

	private string URLInitialize = ApplicationModel.host+"get_homepage_data.php";
	private string URLUpdateReadNotifications = ApplicationModel.host+"update_read_notifications.php";
	
	public HomePageModel()
	{
	}
	public IEnumerator getData(int totalNbResultLimit){
		
		this.notifications = new List<DisplayedNotification>();
		this.news = new List<DisplayedNews>();
		this.player = new User();
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		
		WWW w = new WWW(URLInitialize, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			Debug.Log (w.error); 
		else 
		{
			bool firstSystemNotification=false;
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.player.readnotificationsystem=System.Convert.ToBoolean(System.Convert.ToInt32(data[0]));
			string[] notificationsData=data[1].Split(new string[] { "#N#" }, System.StringSplitOptions.None);
			string[] allNewsData=data[2].Split(new string[] { "#N#" }, System.StringSplitOptions.None);
			int nbNonRead =0;
			
			for (int i=0;i<notificationsData.Length-1;i++)
			{
				string[] notificationData =notificationsData[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
				
				this.notifications.Add(new DisplayedNotification(new Notification(System.Convert.ToInt32(notificationData[0]),
				                                                                  DateTime.ParseExact(notificationData[2], "yyyy-MM-dd HH:mm:ss", null),
				                                                                  System.Convert.ToBoolean(System.Convert.ToInt32(notificationData[3])),
				                                                                  System.Convert.ToInt32(notificationData[4])),
				                                                 new User(System.Convert.ToInt32(notificationData[5]),
				         notificationData[6],
				         notificationData[7],
				         System.Convert.ToInt32(notificationData[8]),
				         System.Convert.ToInt32(notificationData[9]),
				         System.Convert.ToInt32(notificationData[10]),
				         System.Convert.ToInt32(notificationData[11]),
				         System.Convert.ToInt32(notificationData[12]))));
				
				string tempContent=notificationData[1];
				for(int j=13;j<notificationData.Length-1;j++)
				{
					string[] notificationObjectData = notificationData[j].Split (new char[] {':'},System.StringSplitOptions.None);
					switch (notificationObjectData[0])
					{
					case "user":
						this.notifications[i].Users.Add (new User(System.Convert.ToInt32(notificationObjectData[1]),
						                                          notificationObjectData[2],
						                                          notificationObjectData[3],
						                                          System.Convert.ToInt32(notificationObjectData[4]),
						                                          System.Convert.ToInt32(notificationObjectData[5]),
						                                          System.Convert.ToInt32(notificationObjectData[6]),
						                                          System.Convert.ToInt32(notificationObjectData[7]),
						                                          System.Convert.ToInt32(notificationObjectData[8])));
						tempContent=ReplaceFirst(tempContent,"#*user*#",notificationObjectData[2]);
						break;
					case "card":
						this.notifications[i].Cards.Add (new Card (notificationObjectData[1]));
						tempContent=ReplaceFirst(tempContent,"#*card*#",notificationObjectData[1]);
						break;
					case "value":
						this.notifications[i].Values.Add (notificationObjectData[1]);
						tempContent=ReplaceFirst(tempContent,"#*value*#",notificationObjectData[1]);
						break;
					}
				}
				this.notifications[i].Content=tempContent;
			}
			for (int i=0;i<allNewsData.Length-1;i++)
			{
				string[] newsData =allNewsData[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
				
				this.news.Add(new DisplayedNews(new News(DateTime.ParseExact(newsData[1], "yyyy-MM-dd HH:mm:ss", null)),
				                                new User(System.Convert.ToInt32(newsData[2]),
				         newsData[3],
				         newsData[4],
				         System.Convert.ToInt32(newsData[5]),
				         System.Convert.ToInt32(newsData[6]),
				         System.Convert.ToInt32(newsData[7]),
				         System.Convert.ToInt32(newsData[8]),
				         System.Convert.ToInt32(newsData[9])))); 
				
				string tempContent=newsData[0];
				for(int j=10;j<newsData.Length-1;j++)
				{
					string[] newsObjectData = newsData[j].Split (new char[] {':'},System.StringSplitOptions.None);
					switch (newsObjectData[0])
					{
					case "user":
						this.news[i].Users.Add (new User(System.Convert.ToInt32(newsObjectData[1]),
						                                 newsObjectData[2],
						                                 newsObjectData[3],
						                                 System.Convert.ToInt32(newsObjectData[4]),
						                                 System.Convert.ToInt32(newsObjectData[5]),
						                                 System.Convert.ToInt32(newsObjectData[6]),
						                                 System.Convert.ToInt32(newsObjectData[7]),
						                                 System.Convert.ToInt32(newsObjectData[8])));
						tempContent=ReplaceFirst(tempContent,"#*user*#",newsObjectData[2]);
						break;
					case "card":
						this.news[i].Cards.Add (new Card (newsObjectData[1]));
						tempContent=ReplaceFirst(tempContent,"#*card*#",newsObjectData[1]);
						break;
					case "value":
						this.news[i].Values.Add (newsObjectData[1]);
						tempContent=ReplaceFirst(tempContent,"#*value*#",newsObjectData[1]);
						break;
					}
				}
				this.news[i].Content=tempContent;
			}
		}
	}
	public IEnumerator updateReadNotifications (IList<int> readNotifications)
	{
		string query = "";
		bool isSystemNotification = false;
		for(int i=0;i<readNotifications.Count;i++)
		{
			if(this.notifications[readNotifications[i]].Notification.IdNotificationType==1)
			{
				isSystemNotification=true;
				this.player.readnotificationsystem=true;
			}
			else
			{
				this.notifications[readNotifications[i]].Notification.IsRead=true;
				query=query+" id="+this.notifications[readNotifications[i]].Notification.Id+" or";
			}
		}
		if(query!="")
		{
			query=query.Substring(0,query.Length-3);
		}
		
		WWWForm form = new WWWForm(); 											
		form.AddField("myform_hash", ApplicationModel.hash); 					
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_query", query);
		form.AddField ("myform_issystemnotification", isSystemNotification.ToString());
		
		WWW w = new WWW(URLUpdateReadNotifications, form); 				
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 
		}
	}
	
	private string ReplaceFirst(string text, string search, string replace)
	{
		int pos = text.IndexOf(search);
		if (pos < 0)
		{
			return text;
		}
		return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
	}
}




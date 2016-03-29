using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NewHomePageModel 
{
	
	public IList<DisplayedNotification> notifications;
	public IList<DisplayedNews> news;
	public IList<User> users;
	public IList<int> friends;
	public string[] usernameList;
	public int notificationSystemIndex;
	public IList<Pack> packs;
	public IList<Competition> competitions;
	public IList<Deck> decks;
	//public string[] cardTypeList;
	//public string[] skillsList;
	
	private string URLInitialize = ApplicationModel.host+"get_homepage_data.php";
	private string URLUpdateReadNotifications = ApplicationModel.host+"update_read_notifications.php";
	
	public NewHomePageModel()
	{
	}
	public IEnumerator getData(int totalNbResultLimit){
		
		this.notifications = new List<DisplayedNotification>();
		this.news = new List<DisplayedNews>();
		this.competitions = new List<Competition> ();
		this.users = new List<User> ();
		this.friends = new List<int> ();
		this.notificationSystemIndex = -1;

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		form.AddField ("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck.ToString ());
		
		WWW w = new WWW(URLInitialize, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			Debug.Log (w.error); 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.parsePlayer(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.users = parseUsers(data[8].Split(new string[] { "#U#"  }, System.StringSplitOptions.None));
			this.users.Add(ApplicationModel.player);
			this.decks = this.parseDecks(data[1].Split(new string[] { "#DECK#" }, System.StringSplitOptions.None));
			this.notifications=parseNotifications(data[2].Split(new string[] { "#N#" }, System.StringSplitOptions.None));
			this.friends=parseFriends(data[3].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.news=this.filterNews(parseNews(data[4].Split(new string[] { "#N#" }, System.StringSplitOptions.None)),ApplicationModel.player.Id);
			ApplicationModel.player.CurrentDivision=parseDivision(data[5].Split(new string[] { "//" }, System.StringSplitOptions.None));
			ApplicationModel.player.CurrentCup=parseCup(data[6].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.packs=parsePacks(data[7].Split(new string[] { "#PACK#" }, System.StringSplitOptions.None));

			this.lookForNonReadNotification();
			this.competitions.Add (ApplicationModel.player.CurrentDivision);
			this.competitions.Add (ApplicationModel.player.CurrentCup);

			if(this.decks.Count>0)
			{
				ApplicationModel.player.HasDeck=true;
			}
			else
			{
				ApplicationModel.player.HasDeck=false;
			}

			usernameList=new string[this.users.Count];
			for(int i=0;i<this.users.Count;i++)
			{
				this.usernameList[i]=this.users[i].Username;
			}

		}
	}
	private IList<Pack> parsePacks (string[] array)
	{
		IList<Pack> packs = new List<Pack> ();
		for (int i=0; i<array.Length-1;i++)
		{
			string[] packData =array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			packs.Add (new Pack());
			packs[i].Id= System.Convert.ToInt32(packData[0]);
			packs[i].Name =WordingPacks.getName(System.Convert.ToInt32(packData[0])-1);
			packs[i].New= System.Convert.ToBoolean(System.Convert.ToInt32(packData[1]));
			packs[i].Picture=packData[2];
			packs[i].Price=System.Convert.ToInt32(packData[3]);
			packs[i].IdPicture=System.Convert.ToInt32(packData[4]);
		}
		return packs;
	}
	private Cup parseCup(string[] array)
	{
		Cup cup = new Cup ();
		cup.GamesPlayed= System.Convert.ToInt32(array[0]);
		cup.Id= System.Convert.ToInt32(array[1]);
		//cup.IdPicture= System.Convert.ToInt32(array[2]);
		cup.NbRounds = System.Convert.ToInt32(array [3]);
		cup.CupPrize = System.Convert.ToInt32(array [4]);
		return cup;
	}
	private Division parseDivision(string[] array)
	{
		Division division = new Division ();
		division.GamesPlayed = System.Convert.ToInt32 (array [0]);
		division.Id= System.Convert.ToInt32(array[1]);
		//division.IdPicture= System.Convert.ToInt32(array[2]);
		division.NbGames = System.Convert.ToInt32(array [3]);
		division.TitlePrize = System.Convert.ToInt32(array [4]);
		return division;
	}
	private void parsePlayer(string[] array)
	{
		ApplicationModel.player.Readnotificationsystem=System.Convert.ToBoolean(System.Convert.ToInt32(array[0]));
		ApplicationModel.player.Id= System.Convert.ToInt32(array[1]);
		ApplicationModel.player.Ranking = System.Convert.ToInt32 (array [2]);
		ApplicationModel.player.RankingPoints = System.Convert.ToInt32 (array [3]);
		ApplicationModel.player.TotalNbWins = System.Convert.ToInt32 (array [4]);
		ApplicationModel.player.TotalNbLooses = System.Convert.ToInt32 (array [5]);
		ApplicationModel.player.CollectionPoints = System.Convert.ToInt32 (array [6]);
		ApplicationModel.player.CollectionRanking = System.Convert.ToInt32 (array [7]);
		ApplicationModel.player.TutorialStep = System.Convert.ToInt32 (array [8]);
		ApplicationModel.player.NextLevelTutorial=System.Convert.ToBoolean(System.Convert.ToInt32(array[9]));
		ApplicationModel.player.SelectedDeckId = System.Convert.ToInt32 (array [10]);
		ApplicationModel.player.TrainingStatus = System.Convert.ToInt32 (array [11]);
		ApplicationModel.player.ConnectionBonus = System.Convert.ToInt32 (array [12]);
	}
	private IList<User> parseUsers(string[] array)
	{
		IList<User> users = new List<User>();

		for (int i=0;i<array.Length-1;i++)
		{
			string[] userData =array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);

			users.Add (new User());
			users[i].Id= System.Convert.ToInt32(userData[0]);
			users[i].Username= userData[1];
			users[i].IdProfilePicture= System.Convert.ToInt32(userData[2]);
			users[i].CollectionRanking = System.Convert.ToInt32 (userData [3]);
			users[i].RankingPoints = System.Convert.ToInt32 (userData [4]);
			users[i].Ranking = System.Convert.ToInt32 (userData [5]);
			users[i].TotalNbWins = System.Convert.ToInt32 (userData[6]);
			users[i].TotalNbLooses = System.Convert.ToInt32 (userData [7]);
			users[i].Division = System.Convert.ToInt32 (userData [8]);
			users[i].TrainingStatus = System.Convert.ToInt32 (userData [9]);
		}
		return users;
	}
	private void lookForNonReadNotification ()
	{
		int count=0;
		for(int i =0; i<this.notifications.Count;i++)
		{
			if(!ApplicationModel.player.Readnotificationsystem && this.notificationSystemIndex==-1 && notifications[i].Notification.IdNotificationType==1)
			{
				this.notificationSystemIndex=i;
				count++;
				break;
			}
		}
		for(int i =0; i<this.notifications.Count;i++)
		{
			if(i!=this.notificationSystemIndex && notifications[i].Notification.IdNotificationType!=1 && !notifications[i].Notification.IsRead)
			{
				count++;
			}
		}
		ApplicationModel.player.NbNotificationsNonRead=count;
	}
	private IList<DisplayedNews> filterNews(IList<DisplayedNews> newsToFilter, int playerId)
	{
		IList<DisplayedNews> filterednews = new List<DisplayedNews>();
		bool toAdd = true;
		for (int i=0;i<newsToFilter.Count;i++)
		{
			if(newsToFilter[i].News.IdNewsType==1)
			{
				if(newsToFilter[i].Users[0].Id!=playerId)
				{
					toAdd=true;
					for(int j=0;j<filterednews.Count;j++)
					{
						if (filterednews[j].News.IdNewsType==1 &&
						    filterednews[j].Users[0].Id==newsToFilter[i].User.Id &&
						    filterednews[j].User.Id==newsToFilter[i].Users[0].Id)
						{
							toAdd=false;
							break;
						}
					}
					if(toAdd)
					{
						filterednews.Add (newsToFilter[i]);
					}
				}
			}
			else if(newsToFilter[i].News.IdNewsType!=1)
			{
				filterednews.Add (newsToFilter[i]);
			}
		}
		return filterednews;
	}
	public IList<DisplayedNotification> parseNotifications(string[] array)
	{
		IList<DisplayedNotification> notifications = new List<DisplayedNotification> ();
		
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
					notifications[i].Values.Add (WordingGameModes.getName(System.Convert.ToInt32(notificationObjectData[1])-1,System.Convert.ToInt32(notificationObjectData[1])-1));
					tempContent=ReplaceFirst(tempContent,"#*trophy*#",WordingGameModes.getName(System.Convert.ToInt32(notificationObjectData[1])-1,System.Convert.ToInt32(notificationObjectData[1])-1));
					break;
				}
			}
			notifications[i].Content=tempContent;
		}
		
		return notifications;
	}
	public IList<DisplayedNews> parseNews(string[] array)
	{
		IList<DisplayedNews> news = new List<DisplayedNews> ();
		for (int i=0;i<array.Length-1;i++)
		{
			string[] newsData =array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);

			news.Add(new DisplayedNews());

			news[i].News.IdNewsType=System.Convert.ToInt32(newsData[0]);
			news[i].News.Date=DateTime.ParseExact(newsData[1], "yyyy-MM-dd HH:mm:ss", null);
			news[i].User=this.users[returnUsersIndex(System.Convert.ToInt32(newsData[2]))];
			
			string tempContent=WordingNews.getContent(news[i].News.IdNewsType-1);
			for(int j=3;j<newsData.Length-1;j++)
			{
				string[] newsObjectData = newsData[j].Split (new char[] {':'},System.StringSplitOptions.None);
				switch (newsObjectData[0])
				{
				case "user":
					news[i].Users.Add (new User());
					news[i].Users[news[i].Users.Count-1]=this.users[returnUsersIndex(System.Convert.ToInt32(newsObjectData[1]))];
					tempContent=ReplaceFirst(tempContent,"#*user*#",news[i].Users[news[i].Users.Count-1].Username);
					break;
				case "card":
					string cardTitle ="";
					if(newsObjectData[1]=="")
					{
						cardTitle=WordingCardTypes.getName(System.Convert.ToInt32(newsObjectData[2]));
					}
					else
					{
						cardTitle=newsObjectData[1];
					}
					news[i].Cards.Add (new Card (cardTitle));
					tempContent=ReplaceFirst(tempContent,"#*card*#",cardTitle);
					break;
				case "value":
					news[i].Values.Add (newsObjectData[1]);
					tempContent=ReplaceFirst(tempContent,"#*value*#",newsObjectData[1]);
					break;
				case "trophy":
					news[i].Values.Add (WordingGameModes.getName(System.Convert.ToInt32(newsObjectData[1]),System.Convert.ToInt32(newsObjectData[2])));
					tempContent=ReplaceFirst(tempContent,"#*trophy*#",WordingGameModes.getName(System.Convert.ToInt32(newsObjectData[1]),System.Convert.ToInt32(newsObjectData[2])));
					break;
				}
			}
			news[i].Content=tempContent;
		}
		return news;
	}
	public IEnumerator updateReadNotifications (IList<int> readNotifications,int totalNbResultLimit)
	{
		string query = "";
		bool isSystemNotification = false;
		for(int i=0;i<readNotifications.Count;i++)
		{
			this.notifications[readNotifications[i]].Notification.IsRead=true;
			if(this.notifications[readNotifications[i]].Notification.IdNotificationType==1)
			{
				isSystemNotification=true;
				ApplicationModel.player.Readnotificationsystem=true;
			}
			else
			{
				query=query+" id="+this.notifications[readNotifications[i]].Notification.Id+" or";
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
		else
		{
			ApplicationModel.player.NbNotificationsNonRead=System.Convert.ToInt32(w.text);
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
	private List<Deck> parseDecks(string[] decksData)
	{
		List<Deck> decks = new List<Deck> ();
		for(int i=0;i<decksData.Length-1;i++)
		{
			string[] deckInformation = decksData[i].Split(new string[] { "#DECKINFO#" }, System.StringSplitOptions.None);
			decks.Add (new Deck());
			decks[i].Id=System.Convert.ToInt32(deckInformation[0]);
			decks[i].Name=deckInformation[1];
			decks[i].NbCards=System.Convert.ToInt32(deckInformation[2]);
		}
		return decks;
	}
	private List<int> parseFriends(string[] friendsData)
	{
		List<int> friends = new List<int> ();
		for(int i=0;i<friendsData.Length-1;i++)
		{
			friends.Add (this.returnUsersIndex(System.Convert.ToInt32(friendsData[i])));
		}
		return friends;
	}
	private int returnUsersIndex(int id)
	{
		int index=new int();
		for(int j=0;j<this.users.Count;j++)
		{
			if(this.users[j].Id==id)
			{
				index=j;
				break;
			}
		}
		return index;
	}
	public void moveToFriend(int id)
	{
		this.friends.Add (this.returnUsersIndex(this.notifications[id].SendingUser.Id));
		this.notifications.RemoveAt(id);
	}
}




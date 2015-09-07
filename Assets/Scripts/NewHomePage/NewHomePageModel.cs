using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NewHomePageModel 
{
	
	public IList<DisplayedNotification> notifications;
	public IList<DisplayedNews> news;
	public User player;
	public int notificationSystemIndex;
	public Division currentDivision;
	public Cup currentCup;
	public FriendlyGame currentFriendlyGame;
	public IList<Pack> packs;
	public IList<Competition> competitions;
	public IList<Deck> decks;
	public string[] cardTypeList;
	public string[] skillsList;
	
	private string URLInitialize = ApplicationModel.host+"get_homepage_data.php";
	private string URLUpdateReadNotifications = ApplicationModel.host+"update_read_notifications.php";
	
	public NewHomePageModel()
	{
	}
	public IEnumerator getData(int totalNbResultLimit){
		
		this.notifications = new List<DisplayedNotification>();
		this.news = new List<DisplayedNews>();
		this.competitions = new List<Competition> ();
		this.player = new User();
		this.notificationSystemIndex = -1;
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		form.AddField ("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck.ToString ());
		
		WWW w = new WWW(URLInitialize, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			Debug.Log (w.error); 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.player = parseUser(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.decks = this.parseDecks(data[1].Split(new string[] { "#DECK#" }, System.StringSplitOptions.None));
			this.notifications=parseNotifications(data[2].Split(new string[] { "#N#" }, System.StringSplitOptions.None));
			this.news=this.filterNews(parseNews(data[3].Split(new string[] { "#N#" }, System.StringSplitOptions.None)),this.player.Id);
			this.currentDivision=parseDivision(data[4].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.currentCup=parseCup(data[5].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.packs=parsePacks(data[6].Split(new string[] { "#PACK#" }, System.StringSplitOptions.None));
			this.cardTypeList = data[7].Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.skillsList = data[8].Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.currentFriendlyGame = this.parseFriendlyGame(data[9].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.lookForNonReadSystemNotification();
			this.competitions.Add (this.currentDivision);
			this.competitions.Add (this.currentCup);
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
			packs[i].Name= packData[1];
			packs[i].New= System.Convert.ToBoolean(System.Convert.ToInt32(packData[2]));
			packs[i].Picture=packData[3];
			packs[i].Price=System.Convert.ToInt32(packData[4]);
		}
		return packs;
	}
	private Cup parseCup(string[] array)
	{
		Cup cup = new Cup ();
		cup.Name= array[0];
		cup.Picture= array[1];
		cup.NbRounds = System.Convert.ToInt32(array [2]);
		cup.CupPrize = System.Convert.ToInt32(array [3]);
		return cup;
	}
	private Division parseDivision(string[] array)
	{
		Division division = new Division ();
		division.Name= array[0];
		division.Picture= array[1];
		division.NbGames = System.Convert.ToInt32(array [2]);
		division.TitlePrize = System.Convert.ToInt32(array [3]);
		return division;
	}
	private User parseUser(string[] array)
	{
		User player = new User ();
		player.readnotificationsystem=System.Convert.ToBoolean(System.Convert.ToInt32(array[0]));
		player.Id= System.Convert.ToInt32(array[1]);
		player.Ranking = System.Convert.ToInt32 (array [2]);
		player.RankingPoints = System.Convert.ToInt32 (array [3]);
		player.TotalNbWins = System.Convert.ToInt32 (array [4]);
		player.TotalNbLooses = System.Convert.ToInt32 (array [5]);
		player.CollectionPoints = System.Convert.ToInt32 (array [6]);
		player.CollectionRanking = System.Convert.ToInt32 (array [7]);
		player.TutorialStep = System.Convert.ToInt32 (array [8]);
		player.SelectedDeckId = System.Convert.ToInt32 (array [9]);
		player.ConnectionBonus = System.Convert.ToInt32 (array [10]);
		player.Division = System.Convert.ToInt32 (array [11]);
		player.NbGamesDivision = System.Convert.ToInt32 (array [12]);
		player.Cup = System.Convert.ToInt32 (array [13]);
		player.NbGamesCup = System.Convert.ToInt32 (array [14]);
		return player;
	}
	private void lookForNonReadSystemNotification ()
	{
		for(int i =0; i<this.notifications.Count;i++)
		{
			if(!this.player.readnotificationsystem && this.notificationSystemIndex==-1 && notifications[i].Notification.IdNotificationType==1)
			{
				this.notificationSystemIndex=i;
				break;
			}
		}
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
			
			notifications.Add(new DisplayedNotification(new Notification(System.Convert.ToInt32(notificationData[0]),
			                                                             DateTime.ParseExact(notificationData[2], "yyyy-MM-dd HH:mm:ss", null),
			                                                             System.Convert.ToBoolean(System.Convert.ToInt32(notificationData[3])),
			                                                             System.Convert.ToInt32(notificationData[4]),
			                                                             notificationData[5]),
			                                            new User(System.Convert.ToInt32(notificationData[6]),
			         notificationData[7],
			         notificationData[8],
			         System.Convert.ToInt32(notificationData[9]),
			         System.Convert.ToInt32(notificationData[10]),
			         System.Convert.ToInt32(notificationData[11]),
			         System.Convert.ToInt32(notificationData[12]),
			         System.Convert.ToInt32(notificationData[13]))));
			
			string tempContent=notificationData[1];
			for(int j=14;j<notificationData.Length-1;j++)
			{
				string[] notificationObjectData = notificationData[j].Split (new char[] {':'},System.StringSplitOptions.None);
				switch (notificationObjectData[0])
				{
				case "user":
					notifications[i].Users.Add (new User(System.Convert.ToInt32(notificationObjectData[1]),
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
					notifications[i].Cards.Add (new Card (notificationObjectData[1]));
					tempContent=ReplaceFirst(tempContent,"#*card*#",notificationObjectData[1]);
					break;
				case "value":
					notifications[i].Values.Add (notificationObjectData[1]);
					tempContent=ReplaceFirst(tempContent,"#*value*#",notificationObjectData[1]);
					break;
				case "trophy":
					notifications[i].Values.Add (notificationObjectData[1]);
					tempContent=ReplaceFirst(tempContent,"#*trophy*#",notificationObjectData[1]);
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
			
			news.Add(new DisplayedNews(new News(System.Convert.ToInt32(newsData[1]),
			                                    DateTime.ParseExact(newsData[2], "yyyy-MM-dd HH:mm:ss", null),
			                                    newsData[3]),
			                           new User(System.Convert.ToInt32(newsData[4]),
			         newsData[5],
			         newsData[6],
			         System.Convert.ToInt32(newsData[7]),
			         System.Convert.ToInt32(newsData[8]),
			         System.Convert.ToInt32(newsData[9]),
			         System.Convert.ToInt32(newsData[10]),
			         System.Convert.ToInt32(newsData[11])))); 
			
			string tempContent=newsData[0];
			for(int j=12;j<newsData.Length-1;j++)
			{
				string[] newsObjectData = newsData[j].Split (new char[] {':'},System.StringSplitOptions.None);
				switch (newsObjectData[0])
				{
				case "user":
					news[i].Users.Add (new User(System.Convert.ToInt32(newsObjectData[1]),
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
					news[i].Cards.Add (new Card (newsObjectData[1]));
					tempContent=ReplaceFirst(tempContent,"#*card*#",newsObjectData[1]);
					break;
				case "value":
					news[i].Values.Add (newsObjectData[1]);
					tempContent=ReplaceFirst(tempContent,"#*value*#",newsObjectData[1]);
					break;
				case "trophy":
					news[i].Values.Add (newsObjectData[1]);
					tempContent=ReplaceFirst(tempContent,"#*trophy*#",newsObjectData[1]);
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
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		
		WWW w = new WWW(URLUpdateReadNotifications, form); 				
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 
		}
		else
		{
			this.player.nonReadNotifications=System.Convert.ToInt32(w.text);
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
	private FriendlyGame parseFriendlyGame(string[] friendlyGameData)
	{
		FriendlyGame friendlyGame =new FriendlyGame();
		friendlyGame.EarnXp_W = System.Convert.ToInt32 (friendlyGameData [0]);
		friendlyGame.EarnXp_L = System.Convert.ToInt32 (friendlyGameData [1]);
		friendlyGame.EarnCredits_W = System.Convert.ToInt32 (friendlyGameData [2]);
		friendlyGame.EarnCredits_L = System.Convert.ToInt32 (friendlyGameData [3]);
		return friendlyGame;
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
}



